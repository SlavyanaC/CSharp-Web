﻿namespace CakesWebApp
{
    using SIS.MvcFramework.Loggers;
    using SIS.MvcFramework.Loggers.Contracts;
    using SIS.MvcFramework.Services;
    using SIS.MvcFramework.Services.Contracts;
    using SIS.MvcFramework.ViewEngine;
    using SIS.MvcFramework.ViewEngine.Contracts;
    using SIS.MvcFramework.Contracts;
    using SIS.MvcFramework;

    public class Startup : IMvcApplication
    {
        public MvcFrameworkSettings Configure()
        {
            return new MvcFrameworkSettings();
        }

        public void ConfigureServices(IServiceCollection collection)
        {
            collection.AddService<IHashService, HashService>();
            collection.AddService<IUserCookieService, UserCookieService>();
            collection.AddService<IViewEngine, ViewEngine>();
            collection.AddService<ILogger>(() => new FileLogger("log.txt"));
        }
    }
}
