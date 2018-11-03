namespace FDMCWebApp.Models
{
    using Enums;

    public class Kitten
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Breed Breed { get; set; }
    }
}
