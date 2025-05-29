using Ether.Net.Entities;
using Ether.Net.Interfaces;
using SharpPcap;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace Ether.Net
{
    /// <summary>
    /// Class that provides functionality to capture network packets from a given network device using SharpPcap.
    /// </summary>
    public class PacketCaptureProvider : IPacketCaptureProvider
    {
        private readonly CaptureOptions _options;
        private Channel<RawPacket> _channel;
        private ICaptureDevice _device;
        private SessionCaptureMetrics _metrics;
        private bool _started;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCaptureProvider"/> class.
        /// </summary>
        /// <param name="options">Optional capture options. If <c>null</c>, default options will be used.</param>
        public PacketCaptureProvider(CaptureOptions? options = null)
        {
            _options = options ?? new CaptureOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCaptureProvider"/> class.
        /// </summary>
        /// <remarks>
        /// You don't need to call <see cref="SetDevice(ICaptureDevice)"/> explicitly.
        /// </remarks>
        /// <param name="deviceToIntercept">The network device to capture packets from.</param>
        /// <param name="options">Optional capture options. If <c>null</c>, default options will be used.</param>
        public PacketCaptureProvider(ICaptureDevice deviceToIntercept, CaptureOptions? options = null) : this(options)
        {
            SetDevice(deviceToIntercept);
        }

        /// <summary><inheritdoc/></summary>
        public bool Started => _started;

        /// <summary><inheritdoc/></summary>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public async IAsyncEnumerable<RawPacket> CaptureAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await _channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            {
                while (_channel.Reader.TryRead(out var packet))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    yield return packet;
                }
            }
        }

        /// <summary><inheritdoc/></summary>
        /// <returns><inheritdoc/></returns>
        public SessionCaptureMetrics GetMetrics()
        {
            return _metrics;
        }

        /// <summary><inheritdoc/></summary>
        /// <remarks><inheritdoc/></remarks>
        /// <param name="device"><inheritdoc/></param>
        public void SetDevice(ICaptureDevice device)
        {
            ArgumentNullException.ThrowIfNull(device);

            if (_device != null)
                throw new InvalidOperationException($"Device is already set. The device is: {device.Description}");

            _device = device;
            _device.OnPacketArrival += OnPacketArrival;
        }

        /// <summary><inheritdoc/></summary>
        /// <remarks><inheritdoc/></remarks>
        public void Start()
        {
            if (_started)
                return;

            _device.Open(_options.Promiscuous ? DeviceModes.Promiscuous : DeviceModes.None, _options.ReadTimeoutMs);
            _device.Filter = _options.Filter;

            _metrics = new SessionCaptureMetrics(_device);
            _channel = Channel.CreateUnbounded<RawPacket>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });

            _ = Task.Run(() =>
            {
                try
                {
                    _device.Capture();
                }
                catch (Exception ex)
                {
                    _metrics.OnError(ex);
                    _channel.Writer.TryComplete(ex);
                }
            });

            _started = true;
            _metrics.OnCaptureStarted();
        }

        /// <summary><inheritdoc/></summary>
        /// <remarks><inheritdoc/></remarks>
        public void Stop()
        {
            if (!_started)
                return;

            _device.StopCapture();
            _device.Close();
            _device.OnPacketArrival -= OnPacketArrival;
            _channel.Writer.TryComplete();
            _channel = null!;
            _started = false;
            _metrics.OnCaptureComplete();
        }

        /// <summary><inheritdoc/></summary>
        /// <returns><inheritdoc/></returns>
        public bool TryRemoveDevice()
        {
            if (_device == null)
                return true;

            if (_started)
                return false;

            _device.OnPacketArrival -= OnPacketArrival;
            _device = null!;

            return true;
        }

        private void OnPacketArrival(object sender, PacketCapture e)
        {
            try
            {
                var p = e.GetPacket();

                var raw = new RawPacket(
                    Length: p.PacketLength,
                    Payload: p.Data,
                    Timestamp: p.Timeval.Date,
                    LinkType: p.LinkLayerType
                );

                _channel.Writer.TryWrite(raw);
                _metrics.OnPacketCaptured(p.PacketLength);
            }
            catch (Exception ex)
            {
                _metrics.OnError(ex);
            }
        }
    }
}