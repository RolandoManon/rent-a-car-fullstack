using Microsoft.EntityFrameworkCore;
using RentACarAPI.Models;

namespace RentACarAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.PrecioPorDia)
                .HasPrecision(18, 2);
        }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Renta> Rentas { get; set; }
    }

}
