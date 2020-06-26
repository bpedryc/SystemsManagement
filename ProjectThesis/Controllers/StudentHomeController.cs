using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            //TODO: What if there is no session variable?? Also we need to check on every action that requires authentication
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            
            var user = _context.Users
                .FirstOrDefault(u => (u.Id == userId));
            var student = _context.Students
                .FirstOrDefault(s => (s.UserId == userId));

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
            int userId = int.Parse(HttpContext.Session.GetString("UserId")); //TODO: what if he's not logged in?

            var loggedStudent = _context.Students
                .FirstOrDefault(s => s.UserId == userId);

            var chosenThesis = _context.Theses
                .FirstOrDefault(t => t.StudentId == loggedStudent.Id);
            if (chosenThesis != null)
            {
                TempData["Error"] = "Wybrałeś już temat pracy! W razie problemów skontaktuj się ze swoim promotorem.";
                return RedirectToAction("Index", "StudentHome");
            }


            int specialtyId = loggedStudent.SpecialtyId;
            int degreeCycle = loggedStudent.DegreeCycle;

            var facultyId = _context.Specialties
                                .Where(s => s.Id == specialtyId)
                                .Select(s => s.FacId)
                                .FirstOrDefault();


            //<This is ugly>
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
            //</This is ugly> - but works

            var supers = _context.Supervisors
                .Include(s => s.User)
                .Where(s => s.FacultyId == facultyId)
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
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

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
                .Where(t => t.SuperId == chosenThesis.SuperId)
                .Count();
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
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var stud = _context.Students
                .Where(s => s.UserId == userId)
                .FirstOrDefault();

            var thes = new Thesis{Subject = thesisSubject, DegreeCycle = 0, SpecId = stud.SpecialtyId, SuperId = supersId, StudentId = stud.Id};
            _context.Add<Thesis>(thes);
            _context.SaveChanges();
            Debug.WriteLine(supersId + " " + thesisSubject);
            return RedirectToAction("Index", "StudentHome");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
