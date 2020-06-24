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
    public class StudentsController : Controller
    { 
        private readonly ThesisDbContext _context;

        public StudentsController(ThesisDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var students = _context.Students
                .Include(s => s.User)
                .ToList();
            return View(students);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            throw new NotImplementedException();
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var student = _context.Students
                .FirstOrDefault(s => s.Id == id);
            var user = new User {Id = student.UserId};

            using (var transaction = _context.Database.BeginTransaction())
            {
                var studentThesis = _context.Theses
                    .FirstOrDefault(t => t.StudentId == student.Id);
                if (studentThesis != null)
                {
                    studentThesis.StudentId = null;
                    _context.SaveChanges();
                }

                _context.Remove(student);
                _context.SaveChanges();

                _context.Entry(user).State = EntityState.Deleted;
                _context.SaveChanges();

                transaction.Commit();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
