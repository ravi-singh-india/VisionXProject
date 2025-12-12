using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class DMSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
