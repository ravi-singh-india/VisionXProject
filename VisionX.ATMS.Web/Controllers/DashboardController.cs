using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
