namespace FunApp.Web.Models.Jokes
{
    using System.ComponentModel.DataAnnotations;

    public class CreateJokeInputModel
    {
        [Required]
        [MinLength(20)]
        public string Content { get; set; }

        [ValidCategoryId]
        public int CategoryId { get; set; }
    }
}
