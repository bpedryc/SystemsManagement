using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ThesisDbContext _context;

        public HomeController(ThesisDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var uId = HttpContext.Session.GetString("UserId");
            var matchedUser = _context.Users
                                .Where(u => (u.Id == Int64.Parse(uId)))
                                .FirstOrDefault<User>();
            //var model = new RegisterStudentViewModel().User;
            //model = matchedUser;
            Debug.WriteLine(matchedUser.FirstName + " " + matchedUser.Id);
            return View(matchedUser);
            //return View(await _context.Students.ToListAsync());
        }

        [HttpGet]
        public IActionResult Thesis()
        {
            var supers = _context.Supervisors
                            .Where(s => (s.FacultyId == 1));

            List<string> superData = new List<string>();
            foreach(var super in supers)
            {
                var user = _context.Users
                            .Where(u => (u.Id == super.UserId))
                            .FirstOrDefault<User>();
                Debug.WriteLine(super.Id);
                superData.Add(super.Id + " " + user.FirstName + " " + user.LastName);
            }
            return View(superData);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Thesis(Thesis model)
        {
            /*_context.Users.Add(new User { Username = model.Username, Password = model.Password});
            _context.SaveChanges();*/
            return View();
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("UserId", "");
            return RedirectToAction("Login", "Authentication");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
