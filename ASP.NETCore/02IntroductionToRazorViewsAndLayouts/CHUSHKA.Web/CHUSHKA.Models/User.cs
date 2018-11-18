namespace CHUSHKA.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser<string>
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
        }

        public string FullName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}