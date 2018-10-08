namespace SIS.WebServer
{
    using Routing;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class Server
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;
        private readonly TcpListener listener;
        private readonly ServerRoutingTable serverRoutingTable;

        private bool isRunning;

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

            //var task = Task.Run(this.ListenLoop);
            //task.Wait();
        }

        public async Task ListenLoop(Socket client)
        {
            var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
            await connectionHandler.ProcessRequestAsync();
        }

        //public async Task ListenLoop()
        //{
        //    while (this.isRunning)
        //    {
        //        var client = await this.listener.AcceptSocketAsync();
        //        var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
        //        var responseTask = connectionHandler.ProcessRequestAsync();
        //        responseTask.Wait();
        //    }
        //}
    }
}
