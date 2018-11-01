namespace ChushkaWebApp
{
    using SIS.MvcFramework;

    public class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
