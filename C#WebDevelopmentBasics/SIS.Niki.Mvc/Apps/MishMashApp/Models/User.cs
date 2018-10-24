namespace MishMashWebApp.Models
{
    using System.Collections.Generic;
    using Enums;

    public class User
    {
        public User()
        {
            this.Channels = new HashSet<UserChannel>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }

        public virtual ICollection<UserChannel> Channels { get; set; }
    }
}
