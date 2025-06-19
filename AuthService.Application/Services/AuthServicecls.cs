using AuthService.Application.DTO;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Application.Services
{
    public class AuthServicecls : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public AuthServicecls(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        public async Task<bool> RegisterUserAsync(RegisterRequestCls request)
        {

            var existingUser = await _repo.GetUserByUsernameAsync(request.Username);
            if (existingUser != null) return false;

            var passwordHash = ComputeHash(request.PasswordHash);
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            await _repo.AddUserAsync(newUser);
            return true;
        }

        private string ComputeHash(string input) =>
            Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(input)));


        //public async Task<string?> AuthenticateAsync(string username, string password)
        //{

        //    Console.WriteLine($"Attempting login for {username}");

        //    var user = await _repo.GetUserAsync(username, password);

        //    if (user == null)
        //    {
        //        Console.WriteLine("Invalid username or password");
        //        return null;
        //    }

        //    Console.WriteLine("Login success. Generating token...");

        //    // JWT generation code...



        //    // var user = await _repo.GetUserAsync(username, password);
        //    if (user == null) return null;

        //    var key = _config["JwtSettings:Key"];
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[] {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Name, user.Username)
        //    }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        //            SecurityAlgorithms.HmacSha256Signature
        //        )
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);


        //    return tokenHandler.WriteToken(token);
        //}
        public async Task<LoginResponse?> AuthenticateAsync(string username, string password)
        {
            Console.WriteLine($"Attempting login for {username}");

            var user = await _repo.GetUserAsync(username, password);
            if (user == null)
            {
                Console.WriteLine("Invalid username or password");
                return null;
            }

            Console.WriteLine("Login success. Generating token...");

            var key = _config["JwtSettings:Key"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponse
            {
                Username = user.Username,
                Token = tokenString
            };
        }
    }
}
