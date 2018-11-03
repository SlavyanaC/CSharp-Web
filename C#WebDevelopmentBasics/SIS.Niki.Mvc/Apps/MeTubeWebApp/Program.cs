using SIS.MvcFramework;

namespace MeTubeWebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
