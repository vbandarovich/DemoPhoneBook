using Microsoft.EntityFrameworkCore;
using PhoneBookApp.Data.Entities;

namespace PhoneBookApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PhoneBookEntity> PhoneBook { get; set; }

        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection"));
        }
    }
}
