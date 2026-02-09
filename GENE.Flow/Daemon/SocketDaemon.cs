using System.Net;
using System.Net.Sockets;

namespace GENE.Flow.Daemon;

public class SocketDaemon(int port=10070)
{
    public readonly int Port = port;
    
    public readonly CancellationTokenSource CancellationToken = new();
    private readonly TcpListener _listener = new(IPAddress.Loopback,  port);
    public void Start()
    {
        _listener.Start();
        _ = Task.Run(Listen); // explicit background execution
    }

    private async Task Listen()
    {
        try
        {
            while (!CancellationToken.IsCancellationRequested)
            {
                var socket = await _listener.AcceptSocketAsync(CancellationToken.Token)
                    .ConfigureAwait(false);
                
                _ = HandleClientAsync(socket);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception e)
        {
            Logger.Error("Error in daemon listen loop:", e);
        }
    }

    private async Task HandleClientAsync(Socket client)
    {
        try
        {
            Logger.Info("Client connected:", client.RemoteEndPoint);
            await Task.Yield(); // placeholder for real async work
        }
        catch (Exception e)
        {
            Logger.Error("Client handler failed:", e);
        }
        finally
        {
            client.Dispose();
        }
    }

}