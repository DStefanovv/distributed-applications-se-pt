using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Models;

namespace RestaurantReservation.Data
{
    public class RestaurantReservationContext : DbContext
    {
        public RestaurantReservationContext(DbContextOptions<RestaurantReservationContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
