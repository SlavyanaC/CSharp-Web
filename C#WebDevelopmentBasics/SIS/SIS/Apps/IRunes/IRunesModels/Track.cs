namespace IRunesModels
{
    using System.Collections.Generic;

    public class Track : BaseModel<string>
    {
        public Track()
        {
            this.TrackAlbums = new HashSet<TrackAlbum>();
        }

        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<TrackAlbum> TrackAlbums { get; set; }
    }
}
