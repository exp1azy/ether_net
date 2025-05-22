# Ether.Net
**Ether.Net** is a modern, lightweight and asynchronous wrapper around the popular [SharpPcap](https://github.com/dotpcap/sharppcap) library, designed to simplify the capture and analysis of packets and metrics in .NET applications, using modern technologies such as `IAsyncEnumerable` for reading packets, `Channel` for storing them, etc. It provides an API for high-performance and non-blocking network packet capture.

### NOTICE!
*Before using the library, you need to install the [Npcap](https://npcap.com/) utility. This allows Windows software to capture raw network traffic.*

## Usage
### Preparation
Before you start capturing network traffic, you need to select the device on which you want to capture packets. To do this, use the `DeviceObserver` class and the `GetAvailableDevices()` static method:
```csharp
var devices = DeviceObserver.GetAvailableDevices();
```
The method will return `IList<ICaptureDevice>`, from there you will get the network device you need.

Now you can create an instance of the `PacketCaptureProvider` class by passing the selected device as the constructor arguments:
```csharp
var pcapProvider = new PacketCaptureProvider(devices.First());
```

There are cases when you need to initialize the `PacketCaptureProvider` without knowing in advance what device to pass to the constructor. To do this, you can use the `SetDeviceToCaptureFrom()` method to pass the device when it is available, leaving the constructor empty during initialization:
```csharp
var pcapProvider = new PacketCaptureProvider();
var devices = DeviceObserver.GetAvailableDevices();
pcapProvider.SetDeviceToCaptureFrom(devices.First());
```

You can also specify capture options by passing an instance of the `CaptureOptions` class to the constructor:
```csharp
var pcapProvider = new PacketCaptureProvider(devices.First(), new CaptureOptions { Filter = string.Empty, Promiscuous = true, ReadTimeoutMs = 1000 });
```

For more convenient filter construction or if you don't know the BPF (Berkeley Packet Filter) syntax for filtering, you can use `BPFBuilder` to build the BPF expression:
```csharp
string filter = BPFBuilder.Create()
    .Protocol(BPFProto.Ip)
    .Build();

var pcapProvider = new PacketCaptureProvider(devices.First(), new CaptureOptions { Filter = filter });
```

### Capturing And Extracting
Now you can start capturing network packets using the `Start()` method:
```csharp
pcapProvider.Start();
```

Using `await foreach` and `CaptureAsync()` method, you can asynchronously get the next intercepted packet in its raw form:
```csharp
await foreach (var rawPacket in pcapProvider.CaptureAsync())
{
    Console.WriteLine(rawPacket.Length);
}
```

To retrieve the packet of the required protocol, use the `TryParseTo<T>()` extension method:
```csharp
rawPacket.TryParseTo<IPv4Packet>(out var ipv4)
```

You can see all supported protocols in the [PacketDotNet](https://github.com/dotpcap/packetnet) library.

If you don't know in advance what protocol the received packet is, you can parse the raw packet in `FlatNetworkPacket` using `ParseToFlat()` method.
This class extracts and exposes all known protocol levels in the packet as nullable properties. By examining its properties, you can easily determine all the extracted protocols:
```csharp
var flat = rawPacket.ParseToFlat();
```
Also with `FlatNetworkPacket` you can extract all protocol types present in a compressed network packet as an array:
```csharp
var actualTypes = flat.GetActualPacketTypes();
```

To stop capturing packets on a device, use the `Stop()` method:
```csharp
pcapProvider.Stop();
```

You can also retrieve all metrics using `GetMetrics()` calculated within the current packet capture session, and the overridden `ToString()` method will allow you to get human-readable metrics as a string:
```csharp
var metrics = pcapProvider.GetMetrics();
Console.WriteLine(metrics.ToString());
```
Below are all the metrics you can get:
1. Total number of packets successfully captured
2. Total number of bytes received across all captured packets
3. Number of packets dropped by the capture mechanism (according to [SharpPcap](https://github.com/dotpcap/sharppcap))
4. Number of packets dropped by the interface itself (according to [SharpPcap](https://github.com/dotpcap/sharppcap))
5. List of exceptions that occurred during the capture session
6. Timestamp when capture started
7. Timestamp when capture completed
8. Total duration of the capture session
9. Average number of packets received per second since the start of capture
10. Average number of bytes received per second since the start of capture
11. Average size of a single packet in bytes
12. Maximum observed packet throughput per second
13. Maximum observed byte throughput per second
14. Average time between packets in milliseconds

## Utilities
You can integrate `PacketCaptureProvider` into your **ASP.NET** application by registering it in the dependency container. After that, you can get an instance of `IPacketCaptureProvider` from the dependency container inside your services and controllers:
```csharp
builder.Services.AddPacketCaptureProvider(df =>
{
    var devices = DeviceObserver.GetAvailableDevices();
    return devices.First();
});
```

You also have the option to try to resolve an IP address to a domain name using the `TryResolveHostname()` method.
In addition, there is a `GetIpGeolocationAsync()` method that allows you to asynchronously get the geolocation of an IP address using the public API from [ip-api.com](https://ip-api.com/). *Be mindful of request limits for free usage!* These methods are provided by the `EtherNetUtils` class. Below is an example of usage:
```csharp
var utils = new EtherNetUtils();
var task = Task.Run(async () => await Task.Delay(60_000));

await foreach (var rawPacket in pcapProvider.CaptureAsync())
{
    if (task.IsCompleted) 
        break;

    if (rawPacket.TryParseTo<IPv4Packet>(out var ipv4) == false)
        continue;

    var geo = await utils.GetIpGeolocationAsync(ipv4.SourceAddress);
    utils.TryResolveHostname(ipv4.SourceAddress, out var hostname);

    Console.WriteLine($"Hostname: {hostname ?? "unknown"}\nGeo: {geo}\n");
}
```
