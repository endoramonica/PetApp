using Microsoft.AspNetCore.Mvc;

namespace Pet.AdminWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
