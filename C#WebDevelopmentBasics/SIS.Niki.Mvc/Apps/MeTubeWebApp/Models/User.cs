namespace MeTubeWebApp.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Tubes = new HashSet<Tube>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Tube> Tubes { get; set; }
    }
}
