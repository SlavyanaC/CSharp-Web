namespace SIS.Demo
{
    using HTTP.Enums;
    using WebServer;
    using WebServer.Routing;

    public class Launcher
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index();

            Server server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}
