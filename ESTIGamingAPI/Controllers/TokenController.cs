using ESTIGamingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ESTIGamingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private ITokenManager _tokenManager;

        public TokenController(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public IActionResult GetToken()
        {
            return Ok(_tokenManager.GenerateToken());
        }
    }
}
