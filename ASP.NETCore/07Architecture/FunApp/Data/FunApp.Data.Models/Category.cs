namespace FunApp.Data.Models
{
    using System.Collections.Generic;
    using FunApp.Data.Common;

    public class Category : BaseModel<int>
    {
        public Category()
        {
            this.Jokes = new HashSet<Joke>();
        }

        public string Name { get; set; }

        public virtual ICollection<Joke> Jokes { get; set; }
    }
}
