namespace SIS.Demo
{
    using Framework;
    using Framework.Routers;
    using WebServer;
    using Framework.Services;
    using Services;
    using Services.Contracts;

    public class Launcher
    {
        public static void Main(string[] args)
        {
            var dependencyContainer = new DependencyContainer();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();
            dependencyContainer.RegisterDependency<IHashService, HashService>();

            var server = new Server(80, new ControllerRouter(dependencyContainer), new ResourceRouter());
            MvcEngine.Run(server);
        }
    }
}
