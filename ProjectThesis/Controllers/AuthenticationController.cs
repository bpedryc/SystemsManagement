using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ThesisDbContext _context;

        public AuthenticationController(ThesisDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            // Another possibility would be to query user with provided email,
            // then check hashed provided password against queried password
            if (user.Password == null || user.Email == null)
            {
                ViewData["Message"] = "Nale�y wpisa� swoje dane!";
                return View();
            }

            var hashedPassword = GetSha256FromString(user.Password);
            var matchedUser = _context.Users
                                .Where(u => (u.Email == user.Email && u.Password == hashedPassword))
                                .FirstOrDefault<User>();

            if (matchedUser != null)
            {
                HttpContext.Session.SetString("UserId", matchedUser.Id.ToString());
                return RedirectToAction("Index", "Home");
            }

            ViewData["Message"] = "Niepoprawny email lub has�o. Spr�buj ponownie.";
            return View();
        }

        public IActionResult Register()
        {
            var model = new RegisterStudentViewModel();
            model.Faculties = _context.Faculties.ToList();

            //TODO: Implement register logic here

            return View(model);
        }

        private static string GetSha256FromString(string strData)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(strData);
            var sha = new SHA256Managed();
            var hash = new StringBuilder();

            byte[] hashBytes = sha.ComputeHash(strBytes);
            foreach (byte hashByte in hashBytes)
            {
                hash.Append(String.Format("{0:x2}", hashByte));
            }
            return hash.ToString();
        }
    }
}