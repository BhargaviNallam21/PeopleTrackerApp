using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthService.Infrastructure.Data
{
    public class AuthDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();

            // Use your local SQL Server connection string here
            optionsBuilder.UseSqlServer("Server=VEDASRICN\\MSSQLSERVER01;Database=PeopleTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new AuthDbContext(optionsBuilder.Options);
        }
    }
}
