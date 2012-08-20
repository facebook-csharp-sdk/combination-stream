# Combination Stream

Allows multiple streams to be represented as a single stream. Only reading stream (both synchronous and asynchronous)
is supported.

## NuGet

```
Install-Package CombinationStream
```

*CombinationStream is only distrubuted via nuget as a single `CombinationStream.cs` file. 
For those using older versions of Visual Studio that does not support NuGet Package Manager, 
please download the [command line version of NuGet.exe](http://nuget.codeplex.com/releases/view/58939) and 
run the following command.*

    nuget install CombinationStream
    
## Supported Platforms
* .NET 2.0+ (client profiles also supported)
* Windows 8 Metro Style Applications - WinRT (Windows 8)
* Silverlight 4+
* Windows Phone 7.0+
* Portable Class Library

*`NETFX_CORE` must be defined for Windows 8 Metro Style Applications*

## Usage

```csharp
Stream stream0 = .....;
Stream stream1 = .....;

var streams = new List<Stream>();
streams.Add(stream0);
streams.Add(stream1);

Stream combinationStream = new CombinationStream(streams);
```

You can get the internal list of streams using `InteralStreams` property.

```csharp
IList<Stream> streams = combinationStream.InternalStreams;
```

When calling `combinationStream.Dispose()` all streams will be disposed. If you want to have explict control over 
disposing certain streams, you can make use of the overload contructor and pass the list of stream index that you 
would like to auto dispose when calling `Dispose()` on the CombinationStream.


```csharp
Stream stream0 = .....;
Stream stream1 = .....;
Stream stream2 = .....;

var streams = new List<Stream>();
streams.Add(stream0);
streams.Add(stream1);
streams.Add(stream2);

Stream combinationStream = new CombinationStream(streams, new [] { 0, 2 });
combinationStream.Dispose(); // disposes only stream at index 0 and 2.
stream1.Dispose(); // need to explicitly dispose stream at index 1
```
