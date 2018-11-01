namespace SIS.MvcFramework
{
    public class MvcFrameworkSettings
    {
        public string WwwrootPath { get; set; } = "wwwroot";

        public string LoginPageUrl { get; set; } = "/users/login";

        public int Port { get; set; } = 80;
    }
}
