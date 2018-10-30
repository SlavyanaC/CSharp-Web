namespace TorshiaWebApp
{
    using SIS.MvcFramework;
    using SIS.MvcFramework.Contracts;
    using SIS.MvcFramework.Loggers;
    using SIS.MvcFramework.Loggers.Contracts;
    using SIS.MvcFramework.Services.Contracts;

    public class Startup : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings();
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<ILogger, ConsoleLogger>();
        }
    }
}
