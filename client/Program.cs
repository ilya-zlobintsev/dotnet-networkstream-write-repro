// This is an extremely crude test that is not accurate of real world server peformance, but it is enough to show the performance differences between write calls on the server.
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

IPEndPoint ipEndPoint = new(IPAddress.Any, 9000);

using TcpClient client = new();
await client.ConnectAsync(ipEndPoint);
await using var stream = client.GetStream();
StreamReader reader = new(stream);

const int count = 100;

var watch = Stopwatch.StartNew();

var request = Encoding.UTF8.GetBytes("\n");

for (int i = 0; i < count; i++)
{
    await stream.WriteAsync(request);
    await reader.ReadLineAsync();
}

watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;

Console.WriteLine($"Processing {count} messages took {elapsedMs}ms ({(float)elapsedMs / count}ms per item)");
