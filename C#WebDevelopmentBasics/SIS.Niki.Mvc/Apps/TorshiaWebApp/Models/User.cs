namespace TorshiaWebApp.Models
{
    using Enums;

    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public virtual Role Role { get; set; }
    }
}
