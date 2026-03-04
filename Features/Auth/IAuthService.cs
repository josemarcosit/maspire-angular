namespace maspire_angular.Features.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(string email, string fullName, string password);
        Task<AuthResult> LoginAsync(string email, string password);
        Task<User> GetUserByEmailAsync(string email);
    }

}
