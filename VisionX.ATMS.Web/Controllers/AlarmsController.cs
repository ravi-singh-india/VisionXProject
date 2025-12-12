using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class AlarmsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
