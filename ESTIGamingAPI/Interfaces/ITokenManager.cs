using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Interfaces
{
    public interface ITokenManager
    {
        Token GenerateToken();
        bool VerifyToken(string token);
    }
}
