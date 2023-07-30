using System.Net;
using System.Net.Sockets;
using System.Text;

IPEndPoint ipEndPoint = new(IPAddress.Any, 9000);
TcpListener listener = new(ipEndPoint);

var fast = false;

try
{
    listener.Start();

    Console.WriteLine("Waiting for connection");
    using TcpClient handler = await listener.AcceptTcpClientAsync();
    using NetworkStream stream = handler.GetStream();

    Console.WriteLine("Handling requests");

    // While it is expected that writing the entire response in one call is faster, the difference is about 1000x, which seems like too much
    if (fast)
    {
        var fullResponse = Encoding.UTF8.GetBytes("OK\n");

        while (true)
        {
            stream.ReadByte();
            stream.Write(fullResponse);
        }
    }
    else
    {
        // The second buffer size being 1 byte does not make a difference - it is still slow even if both writes have more data
        var response = Encoding.UTF8.GetBytes("OK");
        var newline = Encoding.UTF8.GetBytes("\n");

        while (true)
        {
            stream.ReadByte();
            // WriteByte and WriteAsync are slow too
            stream.Write(response);
            stream.Write(newline);
        }
    }
}
catch (IOException ex)
{
    Console.WriteLine($"Closing stream: {ex.Message}");
}
finally
{
    listener.Stop();
}
