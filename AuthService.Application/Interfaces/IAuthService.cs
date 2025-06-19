using AuthService.Application.DTO;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(string username, string password);
        Task<bool> RegisterUserAsync(RegisterRequestCls request);
    }
}
