using SharpPcap;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Ether.Net
{
    /// <summary>
    /// Represents a metrics collector for packet capturing operations using <see cref="ICaptureDevice"/>.
    /// </summary>
    /// <param name="device">The capture device associated with the metrics.</param>
    public class SessionCaptureMetrics(ICaptureDevice device)
    {
        private readonly ICaptureDevice _device = device;
        private readonly ConcurrentQueue<Exception> _errors = new();
        private readonly Stopwatch _stopwatch = new();
        private long _lastPacketTick;
        private long _totalInterPacketTicks;
        private long _currentSecondPackets;
        private long _currentSecondBytes;
        private long _lastSecondStartTick;
        private long _totalPackets;
        private long _totalBytes;
        private long _maxPacketsPerSecond;
        private long _maxBytesPerSecond;
        private DateTime? _endTime;
        private DateTime? _startTime;

        /// <summary>
        /// Gets the total number of packets successfully captured.
        /// </summary>
        public long ReceivedPackets => _totalPackets;

        /// <summary>
        /// Gets the total number of bytes received across all captured packets.
        /// </summary>
        public long ReceivedBytes => _totalBytes;

        /// <summary>
        /// Gets the number of packets dropped by the capture mechanism (according to SharpPcap).
        /// </summary>
        public long DroppedPackets => GetSharpPcapStats()?.DroppedPackets ?? 0;

        /// <summary>
        /// Gets the number of packets dropped by the interface itself (according to SharpPcap).
        /// </summary>
        public long DroppedPacketsOnInterface => GetSharpPcapStats()?.InterfaceDroppedPackets ?? 0;

        /// <summary>
        /// Gets the list of exceptions that occurred during the capture session.
        /// </summary>
        public List<Exception> Errors => [.. _errors];

        /// <summary>
        /// Gets the timestamp when capture started.
        /// </summary>
        public DateTime? StartTime => _startTime;

        /// <summary>
        /// Gets the timestamp when capture completed.
        /// </summary>
        public DateTime? EndTime => _endTime;

        /// <summary>
        /// Gets the total duration of the capture session.
        /// If the session is still running, it returns the time elapsed so far.
        /// </summary>
        public TimeSpan? Duration => (_startTime.HasValue && _endTime.HasValue)
            ? _endTime - _startTime
            : (_startTime.HasValue ? DateTime.UtcNow - _startTime : null);

        /// <summary>
        /// Gets the average number of packets received per second since the start of capture.
        /// </summary>
        public double AveragePacketsPerSecond => Duration?.TotalSeconds > 0
            ? ReceivedPackets / Duration.Value.TotalSeconds
            : 0;

        /// <summary>
        /// Gets the average number of bytes received per second since the start of capture.
        /// </summary>
        public double AverageBytesPerSecond => Duration?.TotalSeconds > 0
            ? ReceivedBytes / Duration.Value.TotalSeconds
            : 0;

        /// <summary>
        /// Gets the average size of a single packet in bytes.
        /// </summary>
        public double AveragePacketSize => ReceivedPackets > 0
            ? (double)ReceivedBytes / ReceivedPackets
            : 0;

        /// <summary>
        /// Gets the maximum observed packet throughput per second.
        /// </summary>
        public double MaxPacketsPerSecond => _maxPacketsPerSecond;

        /// <summary>
        /// Gets the maximum observed byte throughput per second.
        /// </summary>
        public double MaxBytesPerSecond => _maxBytesPerSecond;

        /// <summary>
        /// Gets the average time between packets in milliseconds.
        /// </summary>
        public double AverageTimeBetweenPacketsMs => ReceivedPackets > 1
            ? TimeSpan.FromTicks(_totalInterPacketTicks / (ReceivedPackets - 1)).TotalMilliseconds
            : 0;

        /// <summary>
        /// Should be called for every captured packet to update packet and byte counters.
        /// </summary>
        /// <param name="packetLength">The length of the captured packet in bytes.</param>
        public void OnPacketCaptured(int packetLength)
        {
            var nowTick = _stopwatch.ElapsedTicks;

            Interlocked.Increment(ref _totalPackets);
            Interlocked.Add(ref _totalBytes, packetLength);

            var lastTick = Interlocked.Exchange(ref _lastPacketTick, nowTick);
            if (lastTick > 0)
                Interlocked.Add(ref _totalInterPacketTicks, nowTick - lastTick);

            Interlocked.Increment(ref _currentSecondPackets);
            Interlocked.Add(ref _currentSecondBytes, packetLength);

            var lastSecondStart = Interlocked.Read(ref _lastSecondStartTick);
            if (nowTick - lastSecondStart >= Stopwatch.Frequency)
            {
                var pps = Interlocked.Exchange(ref _currentSecondPackets, 0);
                var bps = Interlocked.Exchange(ref _currentSecondBytes, 0);

                Interlocked.Exchange(ref _lastSecondStartTick, nowTick);
                Max(ref _maxPacketsPerSecond, pps);
                Max(ref _maxBytesPerSecond, bps);
            }
        }

        /// <summary>
        /// Initializes the metric collection and resets all counters.
        /// Should be called when capture starts.
        /// </summary>
        public void OnCaptureStarted()
        {
            _totalPackets = 0;
            _totalBytes = 0;
            _totalInterPacketTicks = 0;
            _lastPacketTick = 0;
            _maxPacketsPerSecond = 0;
            _maxBytesPerSecond = 0;
            _lastSecondStartTick = 0;
            _currentSecondPackets = 0;
            _currentSecondBytes = 0;
            _errors.Clear();

            _startTime = DateTime.UtcNow;
            _stopwatch.Restart();
        }

        /// <summary>
        /// Marks the capture session as complete and records the end time.
        /// </summary>
        public void OnCaptureComplete()
        {
            _stopwatch.Stop();
            _endTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds an error to the internal queue of exceptions encountered during capture.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        public void OnError(Exception ex)
        {
            _errors.Enqueue(ex);
        }

        /// <summary>
        /// Returns a textual summary of the current metrics, including all counters and statistics.
        /// </summary>
        /// <returns>A formatted string containing summary statistics.</returns>
        public override string ToString()
        {
            return $"Received packets: {ReceivedPackets};\n" +
                $"Received bytes: {ReceivedBytes};\n" +
                $"Dropped packets: {DroppedPackets};\n" +
                $"Dropped packets on interface: {DroppedPacketsOnInterface};\n" +
                $"Errors: {Errors.Count}\n" +
                $"Capture start time: {StartTime}\n" +
                $"Capture end time: {EndTime}\n" +
                $"Total duration: {Duration}\n" +
                $"Average packets/sec: {AveragePacketsPerSecond}\n" +
                $"Average bytes/sec: {AverageBytesPerSecond}\n" +
                $"Average packet size: {AveragePacketSize}\n" +
                $"Max packets/sec: {MaxPacketsPerSecond}\n" +
                $"Max bytes/sec: {MaxBytesPerSecond}\n" +
                $"Average inter-packet time (ms): {AverageTimeBetweenPacketsMs}\n";
        }

        private ICaptureStatistics? GetSharpPcapStats()
        {
            try
            {
                return _device.Statistics;
            }
            catch
            {
                return null;
            }
        }

        private static void Max(ref long target, long value)
        {
            long initial, computed;

            do
            {
                initial = Interlocked.Read(ref target);
                if (value <= initial) return;
                computed = value;
            } 
            while (Interlocked.CompareExchange(ref target, computed, initial) != initial);
        }
    }
}
