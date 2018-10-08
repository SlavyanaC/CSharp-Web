namespace IRunesModels
{
    public class UserAlbum
    {
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
