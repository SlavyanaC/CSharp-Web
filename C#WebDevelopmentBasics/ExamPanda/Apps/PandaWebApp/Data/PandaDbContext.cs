namespace PandaWebApp.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class PandaDbContext : DbContext
    {
        public DbSet<Package> Packages { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString).UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>()
                .HasOne(r => r.Package)
                .WithOne(p => p.Receipt)
                .HasForeignKey<Receipt>(r => r.PackageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
