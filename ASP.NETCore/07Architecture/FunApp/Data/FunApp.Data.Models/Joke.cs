namespace FunApp.Data.Models
{
    using FunApp.Data.Common;

    public class Joke : BaseModel<int>
    {
        public string Content { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
