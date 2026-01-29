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
    }

    public async void Listen()
    {
        while (true)
            ClientStream(await _listener.AcceptSocketAsync(cts.Token));
    }

    public async void ClientStream(Socket client)
    {
        
    }
}