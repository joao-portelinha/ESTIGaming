using Microsoft.AspNetCore.Mvc;

namespace SmartCampingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.UserType = HttpContext.Session.GetString("userType");
            return View();
        } 
    }
}