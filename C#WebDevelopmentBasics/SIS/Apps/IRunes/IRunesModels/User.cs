namespace IRunesModels
{
    using System.Collections.Generic;

    public class User : BaseModel<string>
    {
        public User()
        {
            this.UserAlbums = new HashSet<UserAlbum>();
        }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public string Email { get; set; }

        public virtual ICollection<UserAlbum> UserAlbums { get; set; }
    }
}
