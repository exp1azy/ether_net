namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents configuration options for starting a packet capture session.
    /// </summary>
    public class CaptureOptions
    {
        /// <summary>
        /// Gets or sets the BPF (Berkeley Packet Filter) expression used to filter captured packets. The default is an empty string.
        /// </summary>
        public string Filter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the capture should be done in promiscuous mode. The default is true.
        /// </summary>
        public bool Promiscuous { get; set; } = true;

        /// <summary>
        /// Gets or sets the read timeout for the capture device in milliseconds. The default is 1000.
        /// </summary>
        public int ReadTimeoutMs { get; set; } = 1000;
    }
}