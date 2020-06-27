using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;
using Remotion.Linq.Clauses;

namespace ProjectThesis.Controllers
{
    public class StudentsController : Controller
    { 
        private readonly ThesisDbContext _context;

        public StudentsController(ThesisDbContext context)
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

            var students = _context.Students
                .Include(s => s.User)
                .ToList();
            return View(students);
        }

        public ActionResult Create()
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel model)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var enteredStudent = model.Student;
            var enteredUser = model.Student.User;

            using (var transaction = _context.Database.BeginTransaction())
            {
                var matchedUser = _context.Users
                    .FirstOrDefault(u => (u.Email == model.Student.User.Email));
                if (matchedUser != null)
                {
                    ViewData["Message"] = "Taki użytkownik istnieje już w systemie!";
                    return View(model);
                }

                enteredUser.Password = AuthenticationController.GetSha256FromString(enteredUser.Password);

                _context.Users.Add(enteredUser);
                _context.SaveChanges();

                model.Student.UserId = enteredUser.Id;
                _context.Students.Add(enteredStudent);
                _context.SaveChanges();

                transaction.Commit();
            }
            return RedirectToAction("Index", "Students");
        }

        public ActionResult Edit(int id)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var student = _context.Students
                .FirstOrDefault(s => s.Id == id);
            student.User = _context.Users
                .FirstOrDefault(u => u.Id == student.UserId);
            student.Specialty = _context.Specialties
                .FirstOrDefault(s => s.Id == student.SpecialtyId);

            return View(new StudentViewModel{ Student = student });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentViewModel viewModel)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var enteredStudent = viewModel.Student;
            var enteredUser = viewModel.Student.User;

            var student = _context.Students
                .FirstOrDefault(s => s.Id == viewModel.Student.Id);
            var user = _context.Users
                .FirstOrDefault(u => u.Id == student.UserId);

            if (!string.IsNullOrWhiteSpace(enteredUser.Email))
            {
                user.Email = enteredUser.Email;
            }
            if (!string.IsNullOrWhiteSpace(enteredUser.FirstName))
            {
                user.FirstName = enteredUser.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(enteredUser.LastName))
            {
                user.LastName = enteredUser.LastName;
            }
            if (!string.IsNullOrWhiteSpace(enteredUser.Password))
            {
                if (enteredUser.Password != viewModel.ConfirmPassword)
                {
                    ViewData["Message"] = "Wpisane hasła nie są takie same";
                    return View();
                }
                user.Password = AuthenticationController.GetSha256FromString(enteredUser.Password);
            }

            if (enteredStudent.SpecialtyId != student.SpecialtyId)
            {
                student.SpecialtyId = enteredStudent.SpecialtyId;
            }
            if (enteredStudent.DegreeCycle != student.DegreeCycle)
            {
                student.DegreeCycle = enteredStudent.DegreeCycle;
            }
            if (enteredStudent.StudentNo != student.StudentNo)
            {
                student.StudentNo = enteredStudent.StudentNo;
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Students");
        }

        public ActionResult Delete(int id)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

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
