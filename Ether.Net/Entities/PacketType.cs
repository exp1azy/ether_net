namespace Ether.Net.Entities
{
    /// <summary>
    /// Specifies known types of network packets that can be detected and classified.
    /// </summary>
    public enum PacketType
    {
        /// <summary>
        /// The type of the packet could not be determined.
        /// </summary>
        Unknown,

        /// <summary>
        /// Address Resolution Protocol packet used to resolve IP addresses to MAC addresses.
        /// </summary>
        Arp,

        /// <summary>
        /// DHCP for IPv4, used to assign dynamic IP addresses.
        /// </summary>
        DhcpV4,

        /// <summary>
        /// DRDA DDM protocol used in database access over the network.
        /// </summary>
        DrdaDdm,

        /// <summary>
        /// Distributed Relational Database Architecture protocol.
        /// </summary>
        Drda,

        /// <summary>
        /// Ethernet frame packet.
        /// </summary>
        Ethernet,

        /// <summary>
        /// Generic Routing Encapsulation packet used to tunnel protocols.
        /// </summary>
        Gre,

        /// <summary>
        /// Internet Protocol version 4 packet.
        /// </summary>
        IpV4,

        /// <summary>
        /// Internet Protocol version 6 packet.
        /// </summary>
        IpV6,

        /// <summary>
        /// Internet Control Message Protocol version 4, used for diagnostics (e.g., ping).
        /// </summary>
        IcmpV4,

        /// <summary>
        /// Internet Control Message Protocol version 6, similar to ICMPv4 but for IPv6.
        /// </summary>
        IcmpV6,

        /// <summary>
        /// IEEE 802.1Q VLAN tagging protocol.
        /// </summary>
        Ieee8021Q,

        /// <summary>
        /// Internet Group Management Protocol version 2.
        /// </summary>
        IgmpV2,

        /// <summary>
        /// IGMPv3 Membership Query, used for multicast group management.
        /// </summary>
        IgmpV3MembershipQuery,

        /// <summary>
        /// IGMPv3 Membership Report, used to report multicast group membership.
        /// </summary>
        IgmpV3MembershipReport,

        /// <summary>
        /// Layer 2 Tunneling Protocol, used to support VPNs.
        /// </summary>
        L2Tp,

        /// <summary>
        /// Linux "Cooked" Capture v2 (SLL2) pseudo-header packet.
        /// </summary>
        LinuxSll2,

        /// <summary>
        /// Linux "Cooked" Capture (SLL) pseudo-header packet.
        /// </summary>
        LinuxSll,

        /// <summary>
        /// Link Layer Discovery Protocol, used by network devices for topology discovery.
        /// </summary>
        Lldp,

        /// <summary>
        /// Neighbor Discovery Protocol (IPv6): Neighbor Advertisement message.
        /// </summary>
        NdpNeighborAdvertisement,

        /// <summary>
        /// Neighbor Discovery Protocol (IPv6): Neighbor Solicitation message.
        /// </summary>
        NdpNeighborSolicitation,

        /// <summary>
        /// Neighbor Discovery Protocol (IPv6): Redirect message.
        /// </summary>
        NdpRedirectMessage,

        /// <summary>
        /// Neighbor Discovery Protocol (IPv6): Router Advertisement message.
        /// </summary>
        NdpRouterAdvertisement,

        /// <summary>
        /// Neighbor Discovery Protocol (IPv6): Router Solicitation message.
        /// </summary>
        NdpRouterSolicitation,

        /// <summary>
        /// Null protocol, typically used in loopback interfaces.
        /// </summary>
        Null,

        /// <summary>
        /// OSPFv2: Database Description packet used in link-state routing.
        /// </summary>
        OspfV2DatabaseDescriptor,

        /// <summary>
        /// OSPFv2: Hello packet used to establish neighbor relationships.
        /// </summary>
        OspfV2Hello,

        /// <summary>
        /// OSPFv2: Link State Acknowledgment packet.
        /// </summary>
        OspfV2LinkStateAcknowledgment,

        /// <summary>
        /// OSPFv2: Link State Request packet.
        /// </summary>
        OspfV2LinkStateRequest,

        /// <summary>
        /// OSPFv2: Link State Update packet used to distribute routing information.
        /// </summary>
        OspfV2LinkStateUpdate,

        /// <summary>
        /// Point-to-Point Protocol, used in direct connections over serial links.
        /// </summary>
        Ppp,

        /// <summary>
        /// PPP over Ethernet, often used in DSL connections.
        /// </summary>
        Pppoe,

        /// <summary>
        /// Raw IP packets without link-layer headers.
        /// </summary>
        RawIp,

        /// <summary>
        /// RTCP packet container (Real-Time Control Protocol).
        /// </summary>
        RtcpContainer,

        /// <summary>
        /// Real-Time Control Protocol, used alongside RTP for media streaming feedback.
        /// </summary>
        Rtcp,

        /// <summary>
        /// Real-time Transport Protocol, commonly used for audio/video streaming.
        /// </summary>
        Rtp,

        /// <summary>
        /// Transmission Control Protocol, reliable transport-layer protocol.
        /// </summary>
        Tcp,

        /// <summary>
        /// User Datagram Protocol, lightweight transport-layer protocol.
        /// </summary>
        Udp,

        /// <summary>
        /// Virtual Extensible LAN, used in network virtualization (overlay networks).
        /// </summary>
        Vxlan,

        /// <summary>
        /// Wake-on-LAN magic packet used to remotely wake up machines.
        /// </summary>
        WakeOnLan
    }
}