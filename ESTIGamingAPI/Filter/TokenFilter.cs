using ESTIGamingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ESTIGamingAPI.Filter
{
    public class TokenFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var tokenManager = (ITokenManager)context.HttpContext.RequestServices
                .GetService(typeof(ITokenManager));

            var result = false;
            if (context.HttpContext.Request.Headers.ContainsKey("Token"))
            {
                result = true;
            }

            string token = "";
            if (result)
            {
                token = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Token").Value;
                if (!tokenManager.VerifyToken(token))
                {
                    result = false;
                }
            }

            if (!result)
            {
                context.ModelState.AddModelError("Autorização", "Não possuí autorização para realizar esta ação!");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
