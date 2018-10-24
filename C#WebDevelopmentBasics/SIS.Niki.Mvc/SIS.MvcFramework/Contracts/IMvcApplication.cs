namespace SIS.MvcFramework.Contracts
{
    using SIS.MvcFramework.Services.Contracts;

    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IServiceCollection collection);
    }
}
