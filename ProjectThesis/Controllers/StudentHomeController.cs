using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class StudentHomeController : Controller
    {
        private readonly ThesisDbContext _context;

        public StudentHomeController(ThesisDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Student))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            var user = _context.Users
                .FirstOrDefault(u => (u.Id == userId));
            var student = _context.Students
                .FirstOrDefault(s => (s.UserId == userId));
            student.Specialty = _context.Specialties
                .FirstOrDefault(s => s.Id == student.SpecialtyId);
            student.Specialty.Fac = _context.Faculties
                .FirstOrDefault(f => f.Id == student.Specialty.FacId);

            var thesis = _context.Theses
                .FirstOrDefault(t => (t.StudentId == student.Id)) ?? new Thesis { Id = 0, Subject = "Brak Wybranej Pracy" };
            var supervisorUser = new User {FirstName = "Brak", LastName = "Promotora"};
            if (thesis.Id != 0)
            {
                supervisorUser = _context.Supervisors
                    .Where(s => s.Id == thesis.SuperId)
                    .Include(s => s.User)
                    .Select(s => s.User)
                    .FirstOrDefault();
            }

            return View(new StudentPanelViewModel { User = user, Student = student, Thesis = thesis, Supervisor = supervisorUser });
        }

        [HttpGet]
        public IActionResult Theses()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Student))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            int? userId = HttpContext.Session.GetInt32("UserId");

            var loggedStudent = _context.Students
                .FirstOrDefault(s => s.UserId == userId);
            loggedStudent.Specialty = _context.Specialties
                .FirstOrDefault(s => s.Id == loggedStudent.SpecialtyId);

            var chosenThesis = _context.Theses
                .FirstOrDefault(t => t.StudentId == loggedStudent.Id);

            if (chosenThesis != null)
            {
                TempData["Error"] = "Wybrałeś już temat pracy! W razie problemów skontaktuj się ze swoim promotorem.";
                return RedirectToAction("Index", "StudentHome");
            }

            int specialtyId = loggedStudent.SpecialtyId;
            int degreeCycle = loggedStudent.DegreeCycle;
            int facultyId = loggedStudent.Specialty.FacId;

            var supervisorsByStudentCounts = _context.Supervisors
                .Where(s => s.FacultyId == facultyId)
                .ToDictionary(s => s.Id, s => 0);

            var studentCounts = (
                from s in _context.Supervisors
                join t in _context.Theses on s.Id equals t.SuperId
                where s.FacultyId == facultyId && t.StudentId != null
                select new {superId = s.Id, thesisId = t.Id}
                into x
                group x by x.superId
                into g
                select new
                {
                    SupervisorId = g.Key,
                    ThesisCount = g.Count()
                }).ToList();

            foreach (var entry in studentCounts)
            {
                supervisorsByStudentCounts[entry.SupervisorId] = entry.ThesisCount;
            }

            var supers = _context.Supervisors
                .Where(s => s.FacultyId == facultyId && supervisorsByStudentCounts[s.Id] < s.StudentLimit)
                .Include(s => s.User)
                .ToList();

            return View(new ThesesListViewModel
            {
                Supervisors = supers,
                SupervisorsByStudentCounts = supervisorsByStudentCounts,
                FacultyId = facultyId,
                SpecialtyId = specialtyId,
                DegreeCycle = degreeCycle
            });
        }

        public IActionResult ReserveThesis(int thesisId)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Student))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            var chosenThesis = _context.Theses
                .FirstOrDefault(t => t.Id == thesisId && t.StudentId == null);
            if (chosenThesis == null)
            {
                TempData["Error"] = "Ten temat został właśnie zajęty";
                return RedirectToAction("Index");
            }

            var supervisor = _context.Supervisors
                .FirstOrDefault(s => s.Id == chosenThesis.SuperId);
            var supervisorThesesCount = _context.Theses
                .Count(t => t.SuperId == chosenThesis.SuperId);
            if (supervisorThesesCount >= supervisor.StudentLimit)
            {
                TempData["Error"] = "Ten promotor ma już maksymalną ilość studentów";
                return RedirectToAction("Index");
            }

            var loggedStudent = _context.Students
                .FirstOrDefault(s => s.UserId == userId);
            chosenThesis.StudentId = loggedStudent.Id;
            _context.SaveChanges();

            TempData["Success"] = "Temat został pomyślnie przydzielony";
            return RedirectToAction("Index", "StudentHome");
        }

        public JsonResult GetSupervisorTheses(int supervisorId, int specialtyId, int degreeCycle)
        {
            var theses = _context.Theses
                .Where(t => (t.SuperId == supervisorId &&
                             t.SpecId == specialtyId &&
                             t.DegreeCycle == degreeCycle &&
                             t.StudentId == null));
            return Json(theses);
        }

        public IActionResult newThesis(int supersId, string thesisSubject)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Student))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            var stud = _context.Students
                .FirstOrDefault(s => s.UserId == userId);

            var thesis = new Thesis{Subject = thesisSubject, DegreeCycle = stud.DegreeCycle, SpecId = stud.SpecialtyId, SuperId = supersId, StudentId = stud.Id};
            _context.Add(thesis);
            _context.SaveChanges();
            return RedirectToAction("Index", "StudentHome");
        }

        private static string GetSha256FromString(string strData)
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
