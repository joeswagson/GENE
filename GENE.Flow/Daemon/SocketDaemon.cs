using System.Net;
using System.Net.Sockets;

namespace GENE.Flow.Daemon;

public class SocketDaemon(int port=10070)
{
    public readonly int Port = port;
    
    public readonly CancellationTokenSource cts = new CancellationTokenSource();
    private TcpListener _listener = new(IPAddress.Loopback,  port);
    public void Start()
    {
        _listener.Start();
        Listen().ConfigureAwait(false);
    }

    public async Task Listen()
    {
        try
        {
            while (true)
                ClientStream(await _listener.AcceptSocketAsync(cts.Token));
        }
        catch (Exception e)
        {
            Logger.Error("Error in daemon listen loop:", e);
            Console.WriteLine(e);
        }
    }

    public async void ClientStream(Socket client)
    {
        Logger.Info("Client connected to server:", client.RemoteEndPoint);
    }
}