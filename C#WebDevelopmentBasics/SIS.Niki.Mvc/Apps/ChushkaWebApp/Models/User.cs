namespace ChushkaWebApp.Models
{
    using System.Collections.Generic;
    using Enums;

    public class User
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
