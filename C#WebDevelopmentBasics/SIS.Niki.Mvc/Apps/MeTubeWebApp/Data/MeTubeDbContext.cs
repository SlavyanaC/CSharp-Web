namespace MeTubeWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class MeTubeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Tube> Tubes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies();
        }
    }
}
