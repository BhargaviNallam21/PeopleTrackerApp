namespace AuthService.Domain.Entities
{
    public class LoginRequestCls
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
