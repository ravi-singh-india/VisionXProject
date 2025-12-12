using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class LogsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
