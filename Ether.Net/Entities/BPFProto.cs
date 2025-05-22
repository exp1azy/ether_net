namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents protocol identifiers supported in BPF (Berkeley Packet Filter) expressions.
    /// </summary>
    public enum BPFProto
    {
        /// <summary>
        /// IPv4 traffic (Internet Protocol version 4).
        /// </summary>
        Ip,

        /// <summary>
        /// IPv6 traffic (Internet Protocol version 6).
        /// </summary>
        Ip6,

        /// <summary>
        /// Address Resolution Protocol traffic, used for mapping IP addresses to MAC addresses.
        /// </summary>
        Arp,

        /// <summary>
        /// Reverse ARP, used to discover an IP address from a known MAC address.
        /// </summary>
        Rarp,

        /// <summary>
        /// Transmission Control Protocol traffic (connection-oriented transport layer protocol).
        /// </summary>
        Tcp,

        /// <summary>
        /// User Datagram Protocol traffic (connectionless transport layer protocol).
        /// </summary>
        Udp,

        /// <summary>
        /// Internet Control Message Protocol (ICMP), typically used for diagnostics like ping.
        /// </summary>
        Icmp,

        /// <summary>
        /// Internet Control Message Protocol for IPv6.
        /// </summary>
        Icmp6,

        /// <summary>
        /// Internet Group Management Protocol, used for multicast group management.
        /// </summary>
        Igmp,

        /// <summary>
        /// Interior Gateway Routing Protocol (obsolete Cisco protocol).
        /// </summary>
        Igrp,

        /// <summary>
        /// Protocol Independent Multicast, used for multicast routing.
        /// </summary>
        Pim,

        /// <summary>
        /// Authentication Header protocol, used in IPsec for data integrity.
        /// </summary>
        Ah,

        /// <summary>
        /// Encapsulating Security Payload, another IPsec protocol for encryption and integrity.
        /// </summary>
        Esp,

        /// <summary>
        /// Virtual Router Redundancy Protocol, provides automatic default gateway failover.
        /// </summary>
        Vrrp,

        /// <summary>
        /// Ethernet-level packets.
        /// </summary>
        Ether,

        /// <summary>
        /// Fiber Distributed Data Interface protocol.
        /// </summary>
        Fddi,

        /// <summary>
        /// Token Ring protocol, a legacy LAN technology.
        /// </summary>
        Tr,

        /// <summary>
        /// Wireless LAN (IEEE 802.11) traffic.
        /// </summary>
        Wlan,

        /// <summary>
        /// DECnet protocol by Digital Equipment Corporation.
        /// </summary>
        Decnet,

        /// <summary>
        /// AppleTalk protocol, used in legacy Apple networks.
        /// </summary>
        Atalk,

        /// <summary>
        /// AppleTalk Address Resolution Protocol.
        /// </summary>
        Aarp,

        /// <summary>
        /// Systems Communication Architecture, a DEC networking protocol.
        /// </summary>
        Sca,

        /// <summary>
        /// Local Area Transport protocol used by DEC.
        /// </summary>
        Lat,

        /// <summary>
        /// Digital's Maintenance Operations Protocol (Data Link).
        /// </summary>
        Mopdl,

        /// <summary>
        /// Digital's Maintenance Operations Protocol (Remote Console).
        /// </summary>
        Moprc,

        /// <summary>
        /// ISO 8473-based protocols (CLNP).
        /// </summary>
        Iso,

        /// <summary>
        /// Spanning Tree Protocol (used to prevent loops in Ethernet networks).
        /// </summary>
        Stp,

        /// <summary>
        /// Internetwork Packet Exchange, used in legacy Novell networks.
        /// </summary>
        Ipx,

        /// <summary>
        /// NetBIOS Extended User Interface, used by Windows file and printer sharing.
        /// </summary>
        Netbeui
    }
}
