using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class ThesesController : Controller
    {
        private readonly ThesisDbContext _context;

        public ThesesController(ThesisDbContext context)
        {
            _context = context;
        }
        private ActionResult checkRole()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role.Equals("student"))
                return RedirectToAction("Index", "StudentHome");
            else if (role.Equals("supervisor"))
                return RedirectToAction("Index", "SupervisorHome");
            return null;
        }

        public ActionResult Index()
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var theses = _context.Theses
                .Include(t => t.Super)
                .Include(t => t.Super.User)
                .Include(t => t.Student)
                .Include(t => t.Student.User)
                .Include(t => t.Spec)
                .Include(t => t.Spec.Fac)
                .ToList();
            return View(theses);
        }

        public IActionResult RemoveStudent(int thesisId)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var thesis = _context.Theses
                .FirstOrDefault(t => t.Id == thesisId);

            thesis.StudentId = null;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: ThesesController/Details/5
        public ActionResult Details(int id)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            return View();
        }

        // GET: ThesesController/Create
        public ActionResult Create()
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            return View();
        }

        // POST: ThesesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThesesController/Edit/5
        public ActionResult Edit(int id)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            return View();
        }

        // POST: ThesesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ThesesController/Delete/5
        public ActionResult Delete(int id)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            return View();
        }

        // POST: ThesesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
