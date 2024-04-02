using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TouristLocation> TouristLocations { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Continent> Continents { get; set; } = null!;
    }
}
