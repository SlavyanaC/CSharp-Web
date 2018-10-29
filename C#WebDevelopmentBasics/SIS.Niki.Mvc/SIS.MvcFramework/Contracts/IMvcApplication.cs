namespace SIS.MvcFramework.Contracts
{
    using Services.Contracts;

    public interface IMvcApplication
    {
        MvcFrameworkSettings Configure();

        void ConfigureServices(IServiceCollection collection);
    }
}
