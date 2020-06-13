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
    public class HomeController : Controller
    {
        private readonly ThesisDbContext _context;

        public HomeController(ThesisDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = _context.Users
                                .Where(u => (u.Id == userId))
                                .FirstOrDefault();
            var student = _context.Students
                                .Where(s => (s.UserId == userId))
                                .FirstOrDefault();

            var thesis = _context.Theses
                                .Where(t => (t.StudentId == student.Id))
                                .FirstOrDefault();
            if (thesis == null){
                thesis = new Thesis { Subject = "Brak Wybranej Pracy" };
            }

            /*var supervisor = _context.Supervisors
                                .Where(s => (s.Id == student.SuperId))
                                .FirstOrDefault();*/
            Supervisor supervisor = null;
            var supervisorUser = new User { FirstName = "Brak", LastName = "Promotora" };
            if (supervisor != null){
                supervisorUser = _context.Users
                                        .Where(u => (u.Id == supervisor.UserId))
                                        .FirstOrDefault();
            }
            
            return View(new StudentPanelViewModel { User = user, Student = student, Thesis = thesis, Supervisor = supervisorUser });
        }

        [HttpGet]
        public IActionResult Thesis()
        {
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var specialtyId = _context.Students
                                .Where(s => s.UserId == userId)
                                .Select(s => s.SpecialtyId)
                                .FirstOrDefault();
            var facultyId = _context.Specialties
                                .Where(s => s.Id == specialtyId)
                                .Select(s => s.FacId)
                                .FirstOrDefault();
            var supervisorToNumberOfStudents = (
                        from s in _context.Supervisors
                        join t in _context.Theses on s.Id equals t.SuperId
                        where s.FacultyId == facultyId && t.StudentId != null
                        select new { superId = s.Id, thesisId = t.Id } into x
                        group x by x.superId into g
                        select new
                        {
                            SupervisorId = g.Key,
                            ThesisCount = g.Count()
                        }).ToList();



            /*var supervisors = _context.Supervisors
                            .Where(s => (s.FacultyId == facultyId && s.StudentLimit));*/

            /*foreach (var supervisor in supervisors)
            {
                var supervisors = _context.Theses
                    .Where(t => (
                        t.SuperId == supervisor.Id && 
                        t.StudentId == null &&
                        ))
                    .ToList<Thesis>();
            }*/

            /*List<string> superData = new List<string>();
            foreach(var super in supers)
            {
                var user = _context.Users
                            .Where(u => (u.Id == super.UserId))
                            .FirstOrDefault<User>();
                superData.Add(super.Id + " " + user.FirstName + " " + user.LastName);
            }*/


            var supers = _context.Supervisors
                .Include(s => s.User)
                .ToList();
            return View(supers);
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public IActionResult Thesis(Thesis model)
        //{
        //    /*_context.Users.Add(new User { Username = model.Username, Password = model.Password});
        //    _context.SaveChanges();*/
        //    return View();
        //}

        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("UserId", "");
            return RedirectToAction("Login", "Authentication");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
