using Microsoft.AspNetCore.Mvc;

namespace Pet.AdminWeb.Controllers
{
    public class UploadPhotoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
