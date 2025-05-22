using Ether.Net.Entities;
using PacketDotNet;

namespace Ether.Net.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="FlatNetworkPacket"/> to enhance its functionality.
    /// </summary>
    public static class FlatNetworkPacketExtensions
    {
        /// <summary>
        /// Retrieves all protocol types present in the flattened network packet as an array of <see cref="PacketType"/>.
        /// 
        /// This method traverses the encapsulated packets in the original packet payload chain,
        /// mapping each packet's CLR type to a <see cref="PacketType"/> enum value, if available.
        /// </summary>
        /// <param name="np">The <see cref="FlatNetworkPacket"/> instance to analyze.</param>
        /// <returns>An array of <see cref="PacketType"/> representing all protocol layers found in the packet.</returns>
        public static PacketType[] GetActualPacketTypes(this FlatNetworkPacket np)
        {
            var result = new List<PacketType>();
            Packet? current = np.Original;

            while (current != null)
            {
                if (PacketTypeMap.TryGetValue(current.GetType(), out var type))               
                    result.Add(type);
                                               
                current = current.PayloadPacket;
            }

            return [.. result];
        }

        private static readonly Dictionary<Type, PacketType> PacketTypeMap = new()
        {
            [typeof(ArpPacket)] = PacketType.Arp,
            [typeof(DhcpV4Packet)] = PacketType.DhcpV4,
            [typeof(DrdaDdmPacket)] = PacketType.DrdaDdm,
            [typeof(DrdaPacket)] = PacketType.Drda,
            [typeof(EthernetPacket)] = PacketType.Ethernet,
            [typeof(GrePacket)] = PacketType.Gre,
            [typeof(IPv4Packet)] = PacketType.IpV4,
            [typeof(IPv6Packet)] = PacketType.IpV6,
            [typeof(IcmpV4Packet)] = PacketType.IcmpV4,
            [typeof(IcmpV6Packet)] = PacketType.IcmpV6,
            [typeof(Ieee8021QPacket)] = PacketType.Ieee8021Q,
            [typeof(IgmpV2Packet)] = PacketType.IgmpV2,
            [typeof(IgmpV3MembershipQueryPacket)] = PacketType.IgmpV3MembershipQuery,
            [typeof(IgmpV3MembershipReportPacket)] = PacketType.IgmpV3MembershipReport,
            [typeof(L2tpPacket)] = PacketType.L2Tp,
            [typeof(LinuxSll2Packet)] = PacketType.LinuxSll2,
            [typeof(LinuxSllPacket)] = PacketType.LinuxSll,
            [typeof(LldpPacket)] = PacketType.Lldp,
            [typeof(NdpNeighborAdvertisementPacket)] = PacketType.NdpNeighborAdvertisement,
            [typeof(NdpNeighborSolicitationPacket)] = PacketType.NdpNeighborSolicitation,
            [typeof(NdpRedirectMessagePacket)] = PacketType.NdpRedirectMessage,
            [typeof(NdpRouterAdvertisementPacket)] = PacketType.NdpRouterAdvertisement,
            [typeof(NdpRouterSolicitationPacket)] = PacketType.NdpRouterSolicitation,
            [typeof(NullPacket)] = PacketType.Null,
            [typeof(OspfV2DatabaseDescriptorPacket)] = PacketType.OspfV2DatabaseDescriptor,
            [typeof(OspfV2HelloPacket)] = PacketType.OspfV2Hello,
            [typeof(OspfV2LinkStateRequestPacket)] = PacketType.OspfV2LinkStateRequest,
            [typeof(OspfV2LinkStateUpdatePacket)] = PacketType.OspfV2LinkStateUpdate,
            [typeof(PppPacket)] = PacketType.Ppp,
            [typeof(PppoePacket)] = PacketType.Pppoe,
            [typeof(RawIPPacket)] = PacketType.RawIp,
            [typeof(RtcpContainerPacket)] = PacketType.RtcpContainer,
            [typeof(RtcpPacket)] = PacketType.Rtcp,
            [typeof(RtpPacket)] = PacketType.Rtp,
            [typeof(TcpPacket)] = PacketType.Tcp,
            [typeof(UdpPacket)] = PacketType.Udp,
            [typeof(VxlanPacket)] = PacketType.Vxlan,
            [typeof(WakeOnLanPacket)] = PacketType.WakeOnLan
        };
    }
}
