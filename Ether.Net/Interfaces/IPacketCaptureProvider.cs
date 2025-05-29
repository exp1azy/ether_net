using Ether.Net.Entities;
using SharpPcap;

namespace Ether.Net.Interfaces
{
    /// <summary>
    /// Interface that provides functionality to capture network packets from a given network device using SharpPcap.
    /// </summary>
    public interface IPacketCaptureProvider
    {
        /// <summary>
        /// Gets a value indicating whether the capture has started.
        /// </summary>
        public bool Started { get; }

        /// <summary>
        /// Sets the capture device to use for packet capture.
        /// </summary>
        /// /// <remarks>
        /// If you create a new instance of <see cref="PacketCaptureProvider"/> passing a capture device as parameter, you don't need to call this method.
        /// </remarks>
        /// <param name="device">The capture device to use.</param>
        public void SetDevice(ICaptureDevice device);

        /// <summary>
        /// Tries to remove the capture device.
        /// <para>
        /// If packet capture is running, the method returns <c>false</c>.
        /// </para>
        /// </summary>
        /// <returns><c>true</c>, if the capture device was successfully removed; otherwise, <c>false</c>.</returns>
        public bool TryRemoveDevice();

        /// <summary>
        /// Starts the packet capture using the configured device and options.
        /// </summary>
        /// <remarks>
        /// Opens the capture device and begins capturing packets asynchronously.
        /// If the capture is already started, calling this method has no effect.
        /// </remarks>
        public void Start();

        /// <summary>
        /// Asynchronously yields captured packets as an <see cref="IAsyncEnumerable{RawPacket}"/>.
        /// </summary>
        /// <param name="cancellationToken">Optional token to cancel the packet stream.</param>
        /// <returns>An asynchronous sequence of <see cref="RawPacket"/> objects.</returns>
        public IAsyncEnumerable<RawPacket> CaptureAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the packet capture and closes the capture device.
        /// </summary>
        /// <remarks>
        /// Marks the channel as complete and triggers final metric calculations.
        /// </remarks>
        public void Stop();

        /// <summary>
        /// Gets the current capture metrics collected during the session.
        /// </summary>
        /// <returns>The <see cref="SessionCaptureMetrics"/> object containing statistics and errors.</returns>
        public SessionCaptureMetrics GetMetrics();
    }
}