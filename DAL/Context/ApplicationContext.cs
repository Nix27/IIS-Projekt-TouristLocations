using DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
