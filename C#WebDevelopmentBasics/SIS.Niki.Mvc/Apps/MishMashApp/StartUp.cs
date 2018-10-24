namespace MishMashWebApp
{
    using SIS.MvcFramework.Contracts;
    using SIS.MvcFramework.Services.Contracts;
    using SIS.MvcFramework.Loggers;
    using SIS.MvcFramework.Loggers.Contracts;

    public class Startup : IMvcApplication
    {
        public void Configure()
        {
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<ILogger, ConsoleLogger>();
        }
    }
}
