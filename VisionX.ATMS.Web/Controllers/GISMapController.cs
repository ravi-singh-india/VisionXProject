using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class GISMapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
