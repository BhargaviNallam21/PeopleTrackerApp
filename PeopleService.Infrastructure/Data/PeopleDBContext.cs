using Microsoft.EntityFrameworkCore;
using PeopleService.Domain.Entities;

namespace PeopleService.Infrastructure.Data
{
    public class PeopleDbContext : DbContext
    {
        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(p => p.Email).IsUnique();
                entity.Property(p => p.City).IsRequired().HasMaxLength(50);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
