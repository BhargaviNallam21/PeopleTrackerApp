using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
