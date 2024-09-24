using Microsoft.EntityFrameworkCore;
using crudApp.Persistence.Models;

namespace crudApp.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        // migration command:
        // add-migration name -o Persistence/Migrations

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

    }
}
