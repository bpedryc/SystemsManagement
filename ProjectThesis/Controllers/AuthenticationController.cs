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
        public enum UserRole
        {
            Student,
            Supervisor,
            Admin
        }

        public static Dictionary<UserRole, string> RoleToLayout = new Dictionary<UserRole, string> {
            {UserRole.Student, "_StudentHomeLayout"},
            {UserRole.Supervisor, "_SupervisorHomeLayout"},
            {UserRole.Admin, "_AdminHomeLayout"}
        };

        private static UserRole? GetUserRole(HttpContext http)
        {
            int? userRoleRaw = http.Session.GetInt32("UserRole");
            if (userRoleRaw == null || !Enum.IsDefined(typeof(UserRole), userRoleRaw))
            {
                return null;
            }
            return (UserRole)userRoleRaw;
        }
        public static bool IsUserAuthorized(HttpContext http, UserRole requiredRole)
        {
            UserRole? userRole = GetUserRole(http);
            if (userRole == null || userRole != requiredRole)
            {
                return false;
            }
            return true;
        }
        public static string GetUserLayout(HttpContext http)
        {
            var userRole = GetUserRole(http);
            if (userRole == null)
            {
                return null;
            }
            return RoleToLayout[(UserRole)userRole];
        }

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
            if (user.Password == null || user.Email == null)
            {
                ViewData["Message"] = "Nale¿y wpisaæ swoje dane!";
                return View();
            }

            var hashedPassword = GetSha256FromString(user.Password);
            var matchedUser = _context.Users
                .FirstOrDefault(u => (u.Email == user.Email && u.Password == hashedPassword));

            if (matchedUser != null)
            {
                var matchedStudent = _context.Students
                    .FirstOrDefault(s => s.UserId == matchedUser.Id);
                if (matchedStudent != null)
                {
                    HttpContext.Session.SetInt32("UserId", matchedUser.Id);
                    HttpContext.Session.SetInt32("UserRole", (int)UserRole.Student);
                    return RedirectToAction("Index", "StudentHome");
                }

                var matchedSupervisor = _context.Supervisors
                    .FirstOrDefault(s => s.UserId == matchedUser.Id);
                if (matchedSupervisor != null)
                {
                    HttpContext.Session.SetInt32("UserId", matchedUser.Id);
                    HttpContext.Session.SetInt32("UserRole", (int)UserRole.Supervisor);
                    return RedirectToAction("Index", "SupervisorHome");
                }

                var matchedAdmin = _context.Admins
                    .FirstOrDefault(a => a.UserId == matchedUser.Id);
                if (matchedAdmin != null)
                {
                    HttpContext.Session.SetInt32("UserId", matchedUser.Id);
                    HttpContext.Session.SetInt32("UserRole", (int)UserRole.Admin);
                    return RedirectToAction("Index", "AdminHome");
                }

                ViewData["Message"] = "Twoje konto nie zosta³o poprawnie aktywowane. Skontaktuj siê z administratorem";
                return View();
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
                var matchedUser = _context.Users
                    .FirstOrDefault(u => (u.Email == model.User.Email));
                if (matchedUser != null)
                {
                    ViewData["Message"] = "Taki u¿ytkownik istnieje ju¿ w systemie!";
                    return View(model);
                }

                model.User.Password = GetSha256FromString(model.User.Password);

                _context.Users.Add(model.User);
                _context.SaveChanges();

                model.Student.UserId = model.User.Id;
                _context.Students.Add(model.Student);
                _context.SaveChanges();

                transaction.Commit();
                HttpContext.Session.SetString("UserRole", "student");
            }
            TempData["Message"] = "Pomyœlnie zarejestrowano";

            return RedirectToAction("Login", "Authentication");   
        }

        public IActionResult ChangePassword(string newPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(NotAuthorized));
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user.Password = GetSha256FromString(newPassword);
            _context.SaveChanges();

            if (GetUserRole(HttpContext) == UserRole.Student)
            {
                return RedirectToAction("Index", "StudentHome");
            }
            if (GetUserRole(HttpContext) == UserRole.Supervisor)
            {
                return RedirectToAction("Index", "SupervisorHome");
            }
            return RedirectToAction(nameof(NotAuthorized));
        }

        public IActionResult ChangeEmail(string newEmail)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction(nameof(NotAuthorized));
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user.Email = newEmail;
            _context.SaveChanges();

            if (GetUserRole(HttpContext) == UserRole.Student)
            {
                return RedirectToAction("Index", "StudentHome");
            }
            if (GetUserRole(HttpContext) == UserRole.Supervisor)
            {
                return RedirectToAction("Index", "SupervisorHome");
            }
            return RedirectToAction(nameof(NotAuthorized));
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserRole");
            return RedirectToAction("Login", "Authentication");
        }

        public IActionResult NotAuthorized()
        {
            UserRole? role = GetUserRole(HttpContext);
            if (role == null)
            {
                return View();
            }
            ViewData["Layout"] = RoleToLayout[(UserRole)role];

            return View();
        }

        [HttpGet]
        public JsonResult GetSpecialties(int facultyId)
        {
            var specialtiesInFaculty = _context.Specialties
                                        .Where(s => s.FacId == facultyId)
                                        .Select(s => new { s.Id, s.Name });

            return Json(specialtiesInFaculty);
        }

        public static string GetSha256FromString(string strData)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(strData);
            var sha = new SHA256Managed();
            var hash = new StringBuilder();

            byte[] hashBytes = sha.ComputeHash(strBytes);
            foreach (byte hashByte in hashBytes)
            {
                hash.Append($"{hashByte:x2}");
            }
            return hash.ToString();
        }
    }
}