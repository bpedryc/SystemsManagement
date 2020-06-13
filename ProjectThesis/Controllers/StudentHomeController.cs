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
                .ToList();
            return View(new ThesesListViewModel
            {
                Supervisors = supers,
                SupervisorsByStudentCounts = supervisorsByStudentCounts,
                FacultyId = facultyId,
                DegreeCycle = degreeCycle
            });
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public IActionResult Thesis(Thesis model)
        //{
        //    /*_context.Users.Add(new User { Username = model.Username, Password = model.Password});
        //    _context.SaveChanges();*/
        //    return View();
        //}

        public JsonResult GetSupervisorTheses(int supervisorId, int specialtyId, int degreeCycle)
        {
            var theses = _context.Theses
                .Where(t => (t.SuperId == supervisorId &&
                             t.SpecId == specialtyId &&
                             t.DegreeCycle == degreeCycle &&
                             t.StudentId == null));
            return Json(theses);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
