using Microsoft.EntityFrameworkCore;
using RestaurantOpeningApi.Models;

namespace RestaurantOpeningApi.DataContext
{
    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantTime> RestaurantTimes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           
        }
    }
}
