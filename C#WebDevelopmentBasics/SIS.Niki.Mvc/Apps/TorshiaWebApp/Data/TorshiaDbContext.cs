namespace TorshiaWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class TorshiaDbContext : DbContext
    {
        public DbSet<Report> Reports { get; set; }

        public DbSet<Task> Tasks { get; set; }

        public DbSet<TaskSector> TasksSectors { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies();
        }
    }
}
