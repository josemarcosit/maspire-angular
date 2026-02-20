using angular_vega.Core.Models;

namespace angular_vega.Core
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
    }
}
