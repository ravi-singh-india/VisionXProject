using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using VisionX.ATMS.Web.Models;
using VisionX.Interface.Repositories;

namespace VisionX.ATMS.Web.Controllers
{
    public class AccountController : Controller
    {
        IAccount _IAccount;

        public AccountController(IAccount iAccount)
        {
            _IAccount = iAccount;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Username == "Admin" && model.Password == "123")
                {
                    // Temporary check, later connect to DB
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                   //DataSet _ds = _IAccount.LoginAuthentication(model.Username,model.Password);
                }
                ViewBag.Error = "Invalid credentials";
            }
            return View(model);
        }
    }
}
