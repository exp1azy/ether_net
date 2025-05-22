namespace Ether.Net.Entities
{
    /// <summary>
    /// Provides a set of standardized string constants that describe reasons for packet actions taken by a BPF rule.
    /// </summary>
    public static class BPFReasonCode
    {
        /// <summary>
        /// Indicates that the packet matched the filter rule.
        /// </summary>
        public const string Match = "match";

        /// <summary>
        /// Indicates a packet was dropped due to an invalid or unreachable offset in the packet.
        /// </summary>
        public const string BadOffset = "bad-offset";

        /// <summary>
        /// Indicates that the packet was a fragment and was discarded or ignored.
        /// </summary>
        public const string Fragment = "fragment";

        /// <summary>
        /// Indicates the packet was too short to evaluate properly.
        /// </summary>
        public const string Short = "short";

        /// <summary>
        /// Indicates the packet was modified by normalization rules and dropped.
        /// </summary>
        public const string Normalize = "normalize";

        /// <summary>
        /// Indicates a memory failure occurred during packet processing.
        /// </summary>
        public const string Memory = "memory";
    }
}
