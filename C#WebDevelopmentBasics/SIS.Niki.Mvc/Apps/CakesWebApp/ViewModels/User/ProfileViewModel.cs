namespace CakesWebApp.ViewModels.User
{
    using System;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public int OrdersCount { get; set; }
    }
}
