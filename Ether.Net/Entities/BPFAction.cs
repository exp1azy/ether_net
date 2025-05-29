namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents the action that a BPF (Berkeley Packet Filter) rule can take on a packet.
    /// </summary>
    public enum BPFAction
    {
        /// <summary>
        /// Allow the packet to pass.
        /// </summary>
        Pass,

        /// <summary>
        /// Block the packet from being processed.
        /// </summary>
        Block
    }
}