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
var pcapProvider = new PacketCaptureProvider(devices.First(), new CaptureOptions { Filter = string.Empty });
```

If you don't know the BPF (Berkeley Packet Filter) syntax for filtering, you can use `BPFBuilder` to build the BPF expression:
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

To stop capturing packets on a device, use the `Stop()` method:
```csharp
pcapProvider.Stop();
```

You can also retrieve all metrics using `GetMetrics()` calculated within the current packet capture session, and the overridden `ToString()` method will allow you to get human-readable metrics as a string:
```csharp
var metrics = pcapProvider.GetMetrics();
Console.WriteLine(metrics.ToString());
```
