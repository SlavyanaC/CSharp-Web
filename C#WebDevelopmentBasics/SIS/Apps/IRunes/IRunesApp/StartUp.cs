namespace IRunesApp
{
    using Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    using SIS.WebServer.Results;

    public class StartUp
    {
        static void Main(string[] args)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            //Home
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Home/Index"] = request => new RedirectResult("/");

            //User
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Login"] = request => new UserController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Register"] = request => new UserController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Users/Login"] = request => new UserController().PostLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Users/Logout"] = request => new UserController().Logout(request);

            //Album
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/All"] = request => new AlbumController().All(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/Create"] = request => new AlbumController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Albums/Details"] = request => new AlbumController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Users/Register"] = request => new UserController().PostRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Albums/Create"] = request => new AlbumController().CreatePost(request);

            //Track
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Tracks/Create"] = request => new TrackController().Create(request);
            serverRoutingTable.Routes[HttpRequestMethod.GET]["/Tracks/Details"] = request => new TrackController().Details(request);
            serverRoutingTable.Routes[HttpRequestMethod.POST]["/Tracks/Create"] = request => new TrackController().CreatePost(request);

            Server server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}
