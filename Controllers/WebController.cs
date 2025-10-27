using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult guest()
        {
            return Redirect("/");
        }
    }
}
