﻿namespace SIS.Framework
{
    using System;
    using System.Reflection;
    using WebServer;

    public static class MvcEngine
    {
        public static void Run(Server server)
        {
            RegisterAssemblyName();
            RegisterControllerDate();
            RegisterViewsData();
            RegisterModelsData();

            try
            {
                server.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }

        private static void RegisterControllerDate()
        {
            MvcContext.Get.ControllersFolder = "Controllers";
            MvcContext.Get.ControllerSuffix = "Controller";
        }

        private static void RegisterViewsData()
        {
            MvcContext.Get.ViewsFolder = "Views";
        }

        private static void RegisterModelsData()
        {
            MvcContext.Get.ModelsFolder = "Models";
        }
    }
}
