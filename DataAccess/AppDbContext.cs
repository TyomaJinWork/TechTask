using DataAccess.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SingleProduct> SingleProduct { get; set; }
        public DbSet<ComboProduct> ComboProduct { get; set; }
        public DbSet<TimeInterval> TimeInterval { get; set; }
        public DbSet<Window> Window { get; set; }
        public DbSet<Price> Price { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ComboProductConfiguration());
            modelBuilder.ApplyConfiguration(new PriceConfiguration());
            modelBuilder.ApplyConfiguration(new SingleProductConfiguration());
            modelBuilder.ApplyConfiguration(new TimeIntervalConfiguration());
            modelBuilder.ApplyConfiguration(new WindowConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
