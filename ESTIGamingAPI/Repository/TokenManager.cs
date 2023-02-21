using ESTIGamingAPI.Interfaces;
using ESTIGamingAPI.Models;

namespace ESTIGamingAPI.Repository
{
    public class TokenManager : ITokenManager
    {
        private List<Token> tokensList;
        public TokenManager()
        {
            tokensList = new List<Token>();
        }

        public Token GenerateToken()
        {
            var token = new Token();
            token.Value = Guid.NewGuid().ToString();
            token.ExpirationDate = DateTime.Now.AddDays(1);
            this.tokensList.Add(token);
            return token;
        }

        public bool VerifyToken(string token)
        {
            return this.tokensList.Any(t => t.Value == token
                && t.ExpirationDate > DateTime.Now);
        }
    }
}
