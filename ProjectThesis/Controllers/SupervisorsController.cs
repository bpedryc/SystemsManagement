using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ThesisDbContext _context;

        public SupervisorsController(ThesisDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            ViewData["Layout"] = AuthenticationController.GetUserLayout(HttpContext);

            var supervisors = _context.Supervisors
                .Include(s => s.User)
                .Include(s => s.Faculty)
                .OrderBy(s => s.Faculty.Name)
                .ThenByDescending(s => s.StudentLimit)
                .ToList();
            return View(supervisors);
        }

        public ActionResult Create()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            ViewData["Layout"] = AuthenticationController.GetUserLayout(HttpContext);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supervisor supervisor)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            ViewData["Layout"] = AuthenticationController.GetUserLayout(HttpContext);

            if (!ModelState.IsValid)
            {
                return View();
            }

            supervisor.User.Password = AuthenticationController.GetSha256FromString(supervisor.User.Password);

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Users.Add(supervisor.User);
                _context.SaveChanges();

                _context.Supervisors.Add(supervisor);
                _context.SaveChanges();

                transaction.Commit();
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            ViewData["Layout"] = AuthenticationController.GetUserLayout(HttpContext);

            var supervisor = _context.Supervisors
                .FirstOrDefault(s => s.Id == id);
            var user = _context.Users
                .FirstOrDefault(u => u.Id == supervisor.UserId);
            supervisor.User = user;

            return View(supervisor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supervisor model)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            var supervisor = _context.Supervisors
                .FirstOrDefault(s => s.Id == model.Id);
            var user = _context.Users
                .FirstOrDefault(u => u.Id == model.UserId);

            if (model.User.FirstName != user.FirstName)
            {
                user.FirstName = model.User.FirstName;
            }
            if (model.User.LastName != user.LastName)
            {
                user.LastName = model.User.LastName;
            }
            if (model.User.Email != user.Email)
            {
                user.Email = model.User.Email;
            }
            if (!string.IsNullOrWhiteSpace(model.User.Password))
            {
                user.Password = AuthenticationController.GetSha256FromString(model.User.Password);
            }

            if (model.FacultyId != supervisor.FacultyId)
            {
                supervisor.FacultyId = model.FacultyId;
            }
            if (model.StudentLimit != supervisor.StudentLimit)
            {
                supervisor.StudentLimit = model.StudentLimit;
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Admin))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            var theses = _context.Theses
                .Where(t => t.SuperId == id)
                .ToList();
            foreach (var thesis in theses)
            {
                _context.Entry(thesis).State = EntityState.Deleted;
            }

            var supervisor =_context.Supervisors
                .FirstOrDefault(s => s.Id == id);
            var user = _context.Users
                .FirstOrDefault(u => u.Id == supervisor.UserId);

            _context.Entry(supervisor).State = EntityState.Deleted;
            _context.Entry(user).State = EntityState.Deleted;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
