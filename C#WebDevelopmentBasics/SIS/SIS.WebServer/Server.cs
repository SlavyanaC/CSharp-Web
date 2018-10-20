namespace SIS.WebServer
{
    using System;
    using System.Threading.Tasks;
    using System.Net;
    using System.Net.Sockets;
    using Routing;
    using Api.Contracts;

    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;
        private readonly TcpListener listener;
        private readonly ServerRoutingTable serverRoutingTable;
        private readonly IHttpHandler requestHandler;
        private readonly IHttpHandler fileHandler;

        private bool isRunning;

        public Server(int port, IHttpHandler requestHandler, IHttpHandler fileHandler)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.requestHandler = requestHandler;
            this.fileHandler = fileHandler;
        }

        public Server(int port, ServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
            this.serverRoutingTable = serverRoutingTable;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");
            while (isRunning)
            {
                var client = listener.AcceptSocketAsync().GetAwaiter().GetResult();
                Task.Run(() => this.ListenLoop(client));
            }
        }

        public async Task ListenLoop(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.requestHandler, this.fileHandler);
            await connectionHandler.ProcessRequestAsync();
        }
    }
}
