using Microsoft.AspNetCore.Mvc;

namespace VODLibrary.Controllers
{
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
