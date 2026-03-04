namespace maspire_angular.Features.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
    }
}
