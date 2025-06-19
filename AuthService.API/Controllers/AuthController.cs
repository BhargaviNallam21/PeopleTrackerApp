using AuthService.Application.DTO;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestCls request)
        {
            var result = await _authService.RegisterUserAsync(request);
            return result ? Ok("User registered successfully.") : BadRequest("Username already exists.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestCls request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.PasswordHash);
            return token == null ? Unauthorized() : Ok(new { Token = token });
        }
    }
}
