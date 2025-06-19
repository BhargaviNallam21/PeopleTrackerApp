using AuthService.Domain.Entities;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context) => _context = context;

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var hashed = ComputeHash(password);
            return await _context.Users.FirstOrDefaultAsync(
                u => u.Username == username && u.PasswordHash == hashed);
        }



        private string ComputeHash(string input)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
