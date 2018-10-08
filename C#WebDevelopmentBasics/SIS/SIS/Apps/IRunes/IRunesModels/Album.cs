namespace IRunesModels
{
    using System.Linq;
    using System.Collections.Generic;

    public class Album : BaseModel<string>
    {
        public Album()
        {
            this.UserAlbums = new HashSet<UserAlbum>();
            this.TrackAlbums = new HashSet<TrackAlbum>();
        }

        public string Name { get; set; }

        public string Cover { get; set; }

        public decimal? Price => this.TrackAlbums.Sum(t => t.Track.Price) * 0.87m;

        public virtual ICollection<UserAlbum> UserAlbums { get; set; }

        public virtual ICollection<TrackAlbum> TrackAlbums { get; set; }
    }
}
