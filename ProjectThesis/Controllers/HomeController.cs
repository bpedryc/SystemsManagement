using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SignIn(User model)
        {
            /*_context.Users.Add(new User { Username = model.Username, Password = model.Password});
            _context.SaveChanges();*/
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
