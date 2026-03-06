using ECommerceAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
    public class DbProvider
    {
        protected ApplicationDbContext _dbContext;

        public DbProvider(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
