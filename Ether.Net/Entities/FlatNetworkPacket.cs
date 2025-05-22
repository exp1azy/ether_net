using PacketDotNet;

namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents a flattened, easy-to-access wrapper around a network <see cref="Packet"/> instance.
    /// This class extracts and exposes all known protocol layers within the packet as nullable properties.
    /// 
    /// This allows consumers to quickly inspect any supported protocol layer without manually traversing the packet's protocol stack.
    /// </summary>
    public sealed class FlatNetworkPacket(Packet packet)
    {
        /// <summary>
        /// The original underlying <see cref="Packet"/> instance.
        /// </summary>
        public Packet Original => packet;

        /// <summary>Address Resolution Protocol (ARP) layer, if present.</summary>
        public ArpPacket? Arp => packet.Extract<ArpPacket>();

        /// <summary>Dynamic Host Configuration Protocol for IPv4 (DHCPv4) layer, if present.</summary>
        public DhcpV4Packet? DhcpV4 => packet.Extract<DhcpV4Packet>();

        /// <summary>Distributed Relational Database Architecture Data Description Module (DRDA DDM) layer, if present.</summary>
        public DrdaDdmPacket? DrdaDdm => packet.Extract<DrdaDdmPacket>();

        /// <summary>Distributed Relational Database Architecture (DRDA) layer, if present.</summary>
        public DrdaPacket? Drda => packet.Extract<DrdaPacket>();

        /// <summary>Ethernet layer, if present.</summary>
        public EthernetPacket? Ethernet => packet.Extract<EthernetPacket>();

        /// <summary>Generic Routing Encapsulation (GRE) layer, if present.</summary>
        public GrePacket? Gre => packet.Extract<GrePacket>();

        /// <summary>Internet Protocol version 4 (IPv4) layer, if present.</summary>
        public IPv4Packet? IpV4 => packet.Extract<IPv4Packet>();

        /// <summary>Internet Protocol version 6 (IPv6) layer, if present.</summary>
        public IPv6Packet? IpV6 => packet.Extract<IPv6Packet>();

        /// <summary>Internet Control Message Protocol version 4 (ICMPv4) layer, if present.</summary>
        public IcmpV4Packet? IcmpV4 => packet.Extract<IcmpV4Packet>();

        /// <summary>Internet Control Message Protocol version 6 (ICMPv6) layer, if present.</summary>
        public IcmpV6Packet? IcmpV6 => packet.Extract<IcmpV6Packet>();

        /// <summary>IEEE 802.1Q VLAN tagging layer, if present.</summary>
        public Ieee8021QPacket? Ieee8021Q => packet.Extract<Ieee8021QPacket>();

        /// <summary>Internet Group Management Protocol version 2 (IGMPv2) layer, if present.</summary>
        public IgmpV2Packet? IgmpV2 => packet.Extract<IgmpV2Packet>();

        /// <summary>IGMP version 3 Membership Query layer, if present.</summary>
        public IgmpV3MembershipQueryPacket? IgmpV3MembershipQuery => packet.Extract<IgmpV3MembershipQueryPacket>();

        /// <summary>IGMP version 3 Membership Report layer, if present.</summary>
        public IgmpV3MembershipReportPacket? IgmpV3MembershipReport => packet.Extract<IgmpV3MembershipReportPacket>();

        /// <summary>Layer 2 Tunneling Protocol (L2TP) layer, if present.</summary>
        public L2tpPacket? L2Tp => packet.Extract<L2tpPacket>();

        /// <summary>Linux "Cooked" Capture v2 (SLL2) pseudo-header layer, if present.</summary>
        public LinuxSll2Packet? LinuxSll2 => packet.Extract<LinuxSll2Packet>();

        /// <summary>Linux "Cooked" Capture (SLL) pseudo-header layer, if present.</summary>
        public LinuxSllPacket? LinuxSll => packet.Extract<LinuxSllPacket>();

        /// <summary>Link Layer Discovery Protocol (LLDP) layer, if present.</summary>
        public LldpPacket? Lldp => packet.Extract<LldpPacket>();

        /// <summary>Neighbor Discovery Protocol (NDP) IPv6 Neighbor Advertisement layer, if present.</summary>
        public NdpNeighborAdvertisementPacket? NdpNeighborAdvertisement => packet.Extract<NdpNeighborAdvertisementPacket>();

        /// <summary>Neighbor Discovery Protocol (NDP) IPv6 Neighbor Solicitation layer, if present.</summary>
        public NdpNeighborSolicitationPacket? NdpNeighborSolicitation => packet.Extract<NdpNeighborSolicitationPacket>();

        /// <summary>Neighbor Discovery Protocol (NDP) IPv6 Redirect Message layer, if present.</summary>
        public NdpRedirectMessagePacket? NdpRedirectMessage => packet.Extract<NdpRedirectMessagePacket>();

        /// <summary>Neighbor Discovery Protocol (NDP) IPv6 Router Advertisement layer, if present.</summary>
        public NdpRouterAdvertisementPacket? NdpRouterAdvertisement => packet.Extract<NdpRouterAdvertisementPacket>();

        /// <summary>Neighbor Discovery Protocol (NDP) IPv6 Router Solicitation layer, if present.</summary>
        public NdpRouterSolicitationPacket? NdpRouterSolicitation => packet.Extract<NdpRouterSolicitationPacket>();

        /// <summary>Null protocol layer, typically used for loopback interfaces, if present.</summary>
        public NullPacket? Null => packet.Extract<NullPacket>();

        /// <summary>Open Shortest Path First version 2 (OSPFv2) Database Descriptor packet, if present.</summary>
        public OspfV2DatabaseDescriptorPacket? OspfV2DatabaseDescriptor => packet.Extract<OspfV2DatabaseDescriptorPacket>();

        /// <summary>OSPFv2 Hello packet, if present.</summary>
        public OspfV2HelloPacket? OspfV2Hello => packet.Extract<OspfV2HelloPacket>();

        /// <summary>OSPFv2 Link State Acknowledgment packet, if present.</summary>
        public OspfV2LinkStateAcknowledgmentPacket? OspfV2LinkStateAcknowledgment => packet.Extract<OspfV2LinkStateAcknowledgmentPacket>();

        /// <summary>OSPFv2 Link State Request packet, if present.</summary>
        public OspfV2LinkStateRequestPacket? OspfV2LinkStateRequest => packet.Extract<OspfV2LinkStateRequestPacket>();

        /// <summary>OSPFv2 Link State Update packet, if present.</summary>
        public OspfV2LinkStateUpdatePacket? OspfV2LinkStateUpdate => packet.Extract<OspfV2LinkStateUpdatePacket>();

        /// <summary>Point-to-Point Protocol (PPP) layer, if present.</summary>
        public PppPacket? Ppp => packet.Extract<PppPacket>();

        /// <summary>PPP over Ethernet (PPPoE) layer, if present.</summary>
        public PppoePacket? Pppoe => packet.Extract<PppoePacket>();

        /// <summary>Raw IP packet layer, if present.</summary>
        public RawIPPacket? RawIp => packet.Extract<RawIPPacket>();

        /// <summary>Real-Time Control Protocol (RTCP) container packet, if present.</summary>
        public RtcpContainerPacket? RtcpContainer => packet.Extract<RtcpContainerPacket>();

        /// <summary>Real-Time Control Protocol (RTCP) packet, if present.</summary>
        public RtcpPacket? Rtcp => packet.Extract<RtcpPacket>();

        /// <summary>Real-time Transport Protocol (RTP) packet, if present.</summary>
        public RtpPacket? Rtp => packet.Extract<RtpPacket>();

        /// <summary>Transmission Control Protocol (TCP) packet, if present.</summary>
        public TcpPacket? Tcp => packet.Extract<TcpPacket>();

        /// <summary>User Datagram Protocol (UDP) packet, if present.</summary>
        public UdpPacket? Udp => packet.Extract<UdpPacket>();

        /// <summary>Virtual Extensible LAN (VXLAN) packet, if present.</summary>
        public VxlanPacket? Vxlan => packet.Extract<VxlanPacket>();

        /// <summary>Wake-on-LAN magic packet, if present.</summary>
        public WakeOnLanPacket? WakeOnLan => packet.Extract<WakeOnLanPacket>();
    }
}
