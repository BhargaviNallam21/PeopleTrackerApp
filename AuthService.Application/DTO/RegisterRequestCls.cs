﻿namespace AuthService.Application.DTO
{
    public class RegisterRequestCls
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; }
    }
}
