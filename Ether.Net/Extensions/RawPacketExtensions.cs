using Ether.Net.Entities;
using PacketDotNet;
using System.Diagnostics.CodeAnalysis;

namespace Ether.Net.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="RawPacket"/> to enable parsing and extraction of packet layers.
    /// </summary>
    public static class RawPacketExtensions
    {
        /// <summary>
        /// Parses the <see cref="RawPacket"/> data into a <see cref="FlatNetworkPacket"/>, which exposes all recognized protocol layers.
        /// </summary>
        /// <param name="rawPacket">The raw packet to parse.</param>
        /// <returns>A <see cref="FlatNetworkPacket"/> wrapping the parsed packet.</returns>
        public static FlatNetworkPacket ParseToFlat(this RawPacket rawPacket)
        {
            var p = Packet.ParsePacket(rawPacket.LinkType, rawPacket.Payload.Span.ToArray());
            return new FlatNetworkPacket(p);
        }

        /// <summary>
        /// Attempts to parse the raw packet and extract a specific packet type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The packet type to extract, which must derive from <see cref="Packet"/>.</typeparam>
        /// <param name="rawPacket">The raw packet to parse.</param>
        /// <param name="packet">
        /// When this method returns, contains the extracted packet of type <typeparamref name="T"/>,
        /// or <c>null</c> if extraction failed.
        /// </param>
        /// <returns><c>true</c> if extraction was successful; otherwise, <c>false</c>.</returns>
        public static bool TryParseTo<T>(this RawPacket rawPacket, [NotNullWhen(true)] out T? packet) where T : Packet
        {
            var p = Packet.ParsePacket(rawPacket.LinkType, rawPacket.Payload.Span.ToArray());
            packet = p.Extract<T>();
            return packet != null;
        }

        /// <summary>
        /// Checks whether the raw packet can be parsed into a packet of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The packet type, which must derive from <see cref="Packet"/></typeparam>
        /// <param name="rawPacket">The raw packet.</param>
        /// <returns><c>true</c> if the raw packet can be parsed into a packet of type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
        public static bool Is<T>(this RawPacket rawPacket) where T : Packet
        {
            return rawPacket.TryParseTo<T>(out _);
        }
    }
}