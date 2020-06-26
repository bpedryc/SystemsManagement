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
            var supervisors = _context.Supervisors
                .Include(s => s.User)
                .ToList();
            return View(supervisors);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supervisor supervisor)
        {
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
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
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
            if (!String.IsNullOrWhiteSpace(model.User.Password))
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

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var supervisor =_context.Supervisors
                .FirstOrDefault(s => s.Id == id);
            var user = _context.Users
                .FirstOrDefault(u => u.Id == supervisor.UserId);

            _context.Entry(supervisor).State = EntityState.Deleted;
            _context.Entry(user).State = EntityState.Deleted;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
