using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
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
                ViewData["Message"] = "Nale¿y wpisaæ swoje dane!";
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

            ViewData["Message"] = "Niepoprawny email lub has³o. Spróbuj ponownie.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterStudentViewModel();
            model.Faculties = _context.Faculties.ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Users.Add(model.User);
                _context.SaveChanges();
                Debug.WriteLine(model.Student.SpecialtyId);
                model.Student.UserId = model.User.Id;
                _context.Students.Add(model.Student);
                
                _context.SaveChanges();

                transaction.Commit();
            }
            
            return RedirectToAction("Login", "Authentication"); //TODO: redirect to view telling you that registration was successful    
        }
        
        [HttpGet]
        public JsonResult GetSpecialties(int facultyId)
        {
            var specialtiesInFaculty = _context.Specialties
                                        .Where(s => s.FacId == facultyId)
                                        .Select(s => new { s.Id, s.Name });

            return Json(specialtiesInFaculty);
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