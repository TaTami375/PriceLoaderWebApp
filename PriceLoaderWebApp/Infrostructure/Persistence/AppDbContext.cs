using Microsoft.EntityFrameworkCore;
using PriceLoaderWebApp.Domain.Entities;

namespace PriceLoaderWebApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<PriceItem> PriceItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceItem>().ToTable("PriceItems");
            base.OnModelCreating(modelBuilder);
        }
    }
}