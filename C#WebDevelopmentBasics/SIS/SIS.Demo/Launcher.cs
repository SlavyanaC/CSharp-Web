namespace SIS.Demo
{
    using Framework;
    using Framework.Routers;
    using WebServer;

    public class Launcher
    {
        static void Main(string[] args)
        {
            var server = new Server(80, new ControllerRouter(), new ResourceRouter());
            MvcEngine.Run(server);
        }
    }
}
