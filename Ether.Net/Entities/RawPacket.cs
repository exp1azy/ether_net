using PacketDotNet;

namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents a raw network packet captured from a network interface.
    /// </summary>
    /// <param name="Length">The total length of the packet in bytes.</param>
    /// <param name="Payload">The raw byte data of the packet.</param>
    /// <param name="Timestamp">The timestamp when the packet was captured.</param>
    /// <param name="LinkType">The link-layer protocol type (e.g., Ethernet, WiFi).</param>
    public readonly record struct RawPacket(int Length, ReadOnlyMemory<byte> Payload, DateTime Timestamp, LinkLayers LinkType);
}
