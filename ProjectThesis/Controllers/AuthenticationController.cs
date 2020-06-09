using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
            //return RedirectToAction("Index", "Home");
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
            var pomUser = _context.Set<User>();
         
            if (matchedUser != null)
            {
                Debug.WriteLine(matchedUser.Id);
                HttpContext.Session.SetString("UserId", matchedUser.Id.ToString());
                //HttpContext.Session.SetString("UserId", "29");
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
            model.Faculties = _context.Faculties.ToList();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    MailAddress m = new MailAddress(model.User.Email);
                }
                catch (FormatException)
                {
                    Debug.WriteLine("email");
                    return View(model);
                }

                var matchedUser = _context.Users
                                .Where(u => (u.Email == model.User.Email))
                                .FirstOrDefault<User>();
                if (matchedUser != null)
                {
                    Debug.WriteLine("email exists");
                    return View(model); //dodac blad
                }

                //TODO: check if StudetNO is only numeric signs
                //if(!Regex.IsMatch(model.User.FirstName, @"^[0-9]+$"))

                if (!Regex.IsMatch(model.User.FirstName, @"^[a-zA-Z]+$"))
                {
                    Debug.WriteLine("uncorrect name");
                    return View(model);
                }
                if (!Regex.IsMatch(model.User.LastName, @"^[a-zA-Z]+$"))
                {
                    Debug.WriteLine("uncorrect lastname");
                    return View(model);
                }

                var ps = GetSha256FromString(model.User.Password);
                model.User.Password = ps;
                _context.Users.Add(model.User);
                _context.SaveChanges();
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