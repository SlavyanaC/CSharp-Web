namespace MeTubeWebApp.Models
{
    public class Tube
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public string YouTubeId { get; set; }

        public int Views { get; set; }

        public int UploaderId { get; set; }

        public virtual User Uploader { get; set; }
    }
}
