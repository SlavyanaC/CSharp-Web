namespace SIS.MvcFramework
{
    using Services.Contracts;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IServiceCollection collection);
    }
}
