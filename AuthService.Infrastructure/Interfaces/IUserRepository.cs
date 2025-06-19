using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);

    }
}
