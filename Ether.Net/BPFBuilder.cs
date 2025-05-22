using Ether.Net.Entities;

namespace Ether.Net
{
    /// <summary>
    /// A fluent builder for constructing Berkeley Packet Filter (BPF) expressions in a more readable and type-safe manner.
    /// </summary>
    public class BPFBuilder
    {
        private readonly List<string> _parts = [];

        /// <summary>
        /// Creates a new instance of <see cref="BPFBuilder"/>.
        /// </summary>
        /// <returns>A new <see cref="BPFBuilder"/> instance.</returns>
        public static BPFBuilder Create() => new();

        /// <summary>
        /// Finalizes and returns the composed BPF filter string.
        /// </summary>
        /// <returns>A valid BPF filter string.</returns>
        public string Build() => string.Join(" ", _parts);

        /// <summary>
        /// True if the packet has a protocol of <paramref name="protocol"/>.
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Protocol(BPFProto protocol) => Append(protocol.ToString().ToLowerInvariant());

        /// <summary>
        /// True if the packet matches the custom filter <paramref name="custom"/>.
        /// </summary>
        /// <param name="custom"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Custom(string custom) => Append(custom);

        /// <summary>
        /// True if either the source or destination port of the packet is <paramref name="port"/>.
        /// </summary>
        /// <param name="port"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Port(int port) => Append($"port {port}");

        /// <summary>
        /// True if the packet has a source port value of <paramref name="port"/>.
        /// </summary>
        /// <param name="port"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder SrcPort(int port) => Append($"src port {port}");

        /// <summary>
        /// True if the packet is ip/tcp, ip/udp, ip6/tcp or ip6/udp and has a destination port value of <paramref name="port"/>. The port can be a number or a name.
        /// </summary>
        /// <remarks>
        /// If a name is used, both the port number and protocol are checked.
        /// If a number or ambiguous name is used, only the port number is checked 
        /// (e.g., dst port 513 will print both tcp/login traffic and udp/who traffic, and port domain will print both tcp/domain and udp/domain traffic).
        /// </remarks>
        /// <param name="port"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DstPort(int port) => Append($"dst port {port}");

        /// <summary>
        /// True if either the source or destination port of the packet is between <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <remarks>
        /// Any of the port or port range expressions can be prepended with the keywords, tcp or udp.
        /// </remarks>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder PortRange(int start, int end) => Append($"portrange {start}-{end}");

        /// <summary>
        /// True if the packet has a source port value between <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder SrcPortRange(int start, int end) => Append($"src portrange {start}-{end}");

        /// <summary>
        /// True if the packet is ip/tcp, ip/udp, ip6/tcp or ip6/udp and has a destination port value between <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="start"/> and <paramref name="end"/> are interpreted in the same fashion as the port parameter for port.
        /// </remarks>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DstPortRange(int start, int end) => Append($"dst portrange {start}-{end}");

        /// <summary>
        /// True if either the IPv4/v6 source or destination of the packet is <paramref name="host"/>, which may be either an address or a name.
        /// </summary>
        /// <remarks>
        /// Any of the host expressions can be prepended with the keywords, ip, arp, rarp, or ip6.
        /// </remarks>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Host(string host) => Append($"host {host}");

        /// <summary>
        /// True if the IPv4/v6 source field of the packet is <paramref name="host"/>, which may be either an address or a name.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder SrcHost(string host) => Append($"src host {host}");

        /// <summary>
        /// True if the IPv4/v6 destination field of the packet is <paramref name="host"/>, which may be either an address or a name.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DstHost(string host) => Append($"dst host {host}");

        /// <summary>
        /// True if either the IPv4/v6 source or destination address of the packet has a network number of <paramref name="net"/>.
        /// </summary>
        /// <param name="net"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Net(string net) => Append($"net {net}");

        /// <summary>
        /// True if the IPv4/v6 source address of the packet has a network number of <paramref name="net"/>.
        /// </summary>
        /// <param name="net"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder SrcNet(string net) => Append($"src net {net}");

        /// <summary>
        /// True if the IPv4/v6 destination address of the packet has a network number of <paramref name="net"/>.
        /// </summary>
        /// <remarks>
        /// The <paramref name="net"/> may be either a name from the networks database or a network number.
        /// An IPv4 network number can be written as a dotted quad (e.g., 192.168.1.0), dotted triple (e.g., 192.168.1), dotted pair (e.g, 172.16), or single number (e.g., 10);
        /// the netmask is 255.255.255.255 for a dotted quad (which means that it's really a host match), 255.255.255.0 for a dotted triple, 255.255.0.0 for a dotted pair, or 255.0.0.0 for a single number.
        /// An IPv6 network number must be written out fully; 
        /// the netmask is ff:ff:ff:ff:ff:ff:ff:ff, so IPv6 "network" matches are really always host matches, and a network match requires a netmask length.
        /// </remarks>
        /// <param name="net"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DstNet(string net) => Append($"dst net {net}");

        /// <summary>
        /// True if the IPv4 address matches <paramref name="net"/> with the specific <paramref name="mask"/>. May be qualified with src or dst.
        /// </summary>
        /// <remarks>
        /// Note that this syntax is not valid for IPv6 <paramref name="net"/>.
        /// </remarks>
        /// <param name="net"></param>
        /// <param name="mask"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder NetMask(string net, string mask) => Append($"net {net} mask {mask}");

        /// <summary>
        /// True if the DECNET source address is <paramref name="host"/>, which may be an address of the form ``10.123'', or a DECNET host name.
        /// </summary>
        /// <remarks>
        /// DECNET host name support is only available on ULTRIX systems that are configured to run DECNET.
        /// </remarks>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DecnetSrc(string host) => Append($"decnet src {host}");

        /// <summary>
        /// True if the DECNET destination address is <paramref name="host"/>.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DecnetDst(string host) => Append($"decnet dst {host}");

        /// <summary>
        /// True if either the DECNET source or destination address is <paramref name="host"/>.
        /// </summary>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder DecnetHost(string host) => Append($"decnet host {host}");

        /// <summary>
        /// True if either the Ethernet source or destination address is <paramref name="ehost"/>.
        /// </summary>
        /// <param name="ehost"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder EtherHost(string ehost) => Append($"ether host {ehost}");

        /// <summary>
        /// True if the Ethernet source address is <paramref name="ehost"/>.
        /// </summary>
        /// <param name="ehost"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder EtherSrc(string ehost) => Append($"ether src {ehost}");

        /// <summary>
        /// True if the Ethernet destination address is <paramref name="ehost"/>.
        /// </summary>
        /// <param name="ehost"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder EtherDst(string ehost) => Append($"ether dst {ehost}");

        /// <summary>
        /// True if the packet used <paramref name="proto"/> as a protocol.
        /// </summary>
        /// <remarks>
        /// Protocol can be a number or one of the names icmp, icmp6, igmp, igrp, pim, ah, esp, vrrp, udp, or tcp. 
        /// Note that the identifiers tcp, udp, and icmp are also keywords and must be escaped via backslash (\), which is \\ in the C-shell. 
        /// Note that this primitive does not chase the protocol header chain.
        /// </remarks>
        /// <param name="proto"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Proto(BPFProto proto) => Append($"proto {proto.ToString().ToLowerInvariant()}");

        /// <summary>
        /// True if the packet is a broadcast packet.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Broadcast() => Append("broadcast");

        /// <summary>
        /// True if the packet is a multicast packet.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Multicast() => Append("multicast");

        /// <summary>
        /// True if the packet used <paramref name="host"/> as a gateway.
        /// </summary>
        /// <remarks>
        /// I.e., the Ethernet source or destination address was <paramref name="host"/>, but neither the IP source nor the IP destination was <paramref name="host"/>.
        /// The <paramref name="host"/> must be a name and must be found both by the machine's host-name-to-IP-address resolution mechanisms (host name file, DNS, NIS, etc.) 
        /// and by the machine's host-name-to-Ethernet-address resolution mechanism.
        /// </remarks>
        /// <param name="host"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Gateway(string host) => Append($"gateway {host}");

        /// <summary>
        /// True if the packet was logged with the specified PF reason code.
        /// </summary>
        /// <remarks>
        /// The known codes are: match, bad-offset, fragment, short, normalize, and memory. Use <see cref="BPFReasonCode"/> to obtain the list of known codes.
        /// </remarks>
        /// <param name="code"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Reason(string code) => Append($"reason {code}");

        /// <summary>
        /// True if the packet was logged as coming from the specified interface.
        /// </summary>
        /// <param name="iface"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Ifname(string iface) => Append($"ifname {iface}");

        /// <summary>
        /// Synonymous with the ifname modifier.
        /// </summary>
        /// <param name="iface"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder On(string iface) => Append($"on {iface}");

        /// <summary>
        /// True if the packet was logged as matching the specified PF rule number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Rnr(int num) => Append($"rnr {num}");

        /// <summary>
        /// Synonymous with the rnr modifier.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Rulenum(int num) => Append($"rulenum {num}");

        /// <summary>
        /// True if the packet was logged as matching the specified PF ruleset name of an anchored ruleset.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Rset(string name) => Append($"rset {name}");

        /// <summary>
        /// Synonymous with the rset modifier.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Ruleset(string name) => Append($"ruleset {name}");

        /// <summary>
        /// True if the packet was logged as matching the specified PF rule number of an anchored ruleset.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Srnr(int num) => Append($"srnr {num}");

        /// <summary>
        /// Synonymous with the srnr modifier.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Subrulenum(int num) => Append($"subrulenum {num}");

        /// <summary>
        /// True if PF took the specified action when the packet was logged.
        /// </summary>
        /// <remarks>
        /// Known actions are: pass and block.
        /// </remarks>
        /// <param name="act"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Action(BPFAction act) => Append($"action {act.ToString().ToLowerInvariant()}");

        /// <summary>
        /// True if the packet is an IEEE 802.1Q VLAN packet. If <paramref name="vlanId"/> is specified, only true if the packet has the specified <paramref name="vlanId"/>.
        /// </summary>
        /// <remarks>
        /// Note that the first vlan keyword encountered in expression changes the decoding offsets for the remainder of expression on the assumption that the packet is a VLAN packet.
        /// The vlan <paramref name="vlanId"/> expression may be used more than once, to filter on VLAN hierarchies.
        /// Each use of that expression increments the filter offsets by 4.
        /// </remarks>
        /// <param name="vlanId"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Vlan(int vlanId) => Append($"vlan {vlanId}");

        /// <summary>
        /// True if the packet is an MPLS packet. If <paramref name="label"/> is specified, only true if the packet has the specified <paramref name="label"/>.
        /// </summary>
        /// <remarks>
        /// Note that the first mpls keyword encountered in expression changes the decoding offsets for the remainder of expression on the assumption that the packet is a MPLS-encapsulated IP packet.
        /// The mpls <paramref name="label"/> expression may be used more than once, to filter on MPLS hierarchies.
        /// Each use of that expression increments the filter offsets by 4.
        /// </remarks>
        /// <param name="label"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Mpls(int label) => Append($"mpls {label}");

        /// <summary>
        /// True if the packet is a PPP-over-Ethernet Discovery packet (Ethernet type 0x8863).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Pppoed() => Append("pppoed");

        /// <summary>
        /// True if the packet is a PPP-over-Ethernet Session packet (Ethernet type 0x8864).
        /// </summary>
        /// <remarks>
        /// Note that the first pppoes keyword encountered in expression changes the decoding offsets for the remainder of expression on the assumption that the packet is a PPPoE session packet.
        /// </remarks>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Pppoes() => Append("pppoes");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, with a virtual path identifier of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Vpi(string n) => Append($"vpi {n}");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, with a virtual channel identifier of <paramref name="n"/>.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Vci(string n) => Append($"vci {n}");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is an ATM LANE packet.
        /// </summary>
        /// <remarks>
        /// Note that the first lane keyword encountered in expression changes the tests done in the remainder of expression on the assumption that the packet is either a LANE emulated Ethernet packet or a LANE LE Control packet.
        /// If lane isn't specified, the tests are done under the assumption that the packet is an LLC-encapsulated packet.
        /// </remarks>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Lane() => Append("lane");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is an LLC-encapsulated packet.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Llc() => Append("llc");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is a segment OAM F4 flow cell (VPI=0 & VCI=3).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Oamf4s() => Append("oamf4s");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is an end-to-end OAM F4 flow cell (VPI=0 & VCI=4).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Oamf4e() => Append("oamf4e");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is a segment or end-to-end OAM F4 flow cell (VPI=0 & (VCI=3 | VCI=4)).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Oamf4() => Append("oamf4");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is a segment or end-to-end OAM F4 flow cell (VPI=0 & (VCI=3 | VCI=4)).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Oam() => Append("oam");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is on a meta signaling circuit (VPI=0 & VCI=1).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Metac() => Append("metac");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is on a broadcast signaling circuit (VPI=0 & VCI=2).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Bcc() => Append("bcc");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is on a signaling circuit (VPI=0 & VCI=5).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Sc() => Append("sc");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, and is on an ILMI circuit (VPI=0 & VCI=16).
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Ilmic() => Append("ilmic");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, 
        /// and is on a signaling circuit and is a Q.2931 Setup, Call Proceeding, Connect, Connect Ack, Release, or Release Done message.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Connectmsg() => Append("connectmsg");

        /// <summary>
        /// True if the packet is an ATM packet, for SunATM on Solaris, 
        /// and is on a meta signaling circuit and is a Q.2931 Setup, Call Proceeding, Connect, Release, or Release Done message.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Metaconnect() => Append("metaconnect");

        /// <summary>
        /// True if the packet has a length less than or equal to <paramref name="length"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder LengthLessOrEqual(int length) => Append($"len <= {length}");

        /// <summary>
        /// True if the packet has a length greater than or equal to <paramref name="length"/>.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder LengthGreaterOrEqual(int length) => Append($"len >= {length}");

        /// <summary>
        /// Combines the previous and next expressions into one logical AND.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder And() => Append("and");

        /// <summary>
        /// Combines the previous and next expressions into one logical OR.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Or() => Append("or");

        /// <summary>
        /// Excludes the next expression.
        /// </summary>
        /// <returns>The current <see cref="BPFBuilder"/> instance.</returns>
        public BPFBuilder Not() => Append("not");

        private BPFBuilder Append(string part)
        {
            _parts.Add(part);
            return this;
        }
    }
}
