using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectThesis.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            return View();
        }
    }
}
