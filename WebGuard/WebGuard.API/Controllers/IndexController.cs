using Microsoft.AspNetCore.Mvc;

namespace WebGuard.API.Controllers
{
    [Route("/")]
    public class IndexController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}