using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string email, string fullName, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
