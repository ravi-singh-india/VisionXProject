using Microsoft.AspNetCore.Mvc;

namespace VisionX.ATMS.Web.Controllers
{
    public class NMSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
