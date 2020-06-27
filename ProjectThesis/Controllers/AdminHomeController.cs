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
            var role = HttpContext.Session.GetString("UserRole");
            if(role.Equals("student"))
                return RedirectToAction("Index", "StudentHome");
            else if(role.Equals("supervisor"))
                return RedirectToAction("Index", "SupervisorHome");

            return View();
        }
    }
}
