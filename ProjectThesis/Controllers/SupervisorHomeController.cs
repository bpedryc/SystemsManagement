using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;
using System.Runtime.InteropServices.ComTypes;

namespace ProjectThesis.Controllers
{
    public class SupervisorHomeController : Controller
    {
        private readonly ThesisDbContext _context;

        public SupervisorHomeController(ThesisDbContext context)
        {
            _context = context;
        }
        private IActionResult checkRole()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role.Equals("admin"))
                return RedirectToAction("Index", "AdminHome");
            else if (role.Equals("student"))
                return RedirectToAction("Index", "StudentsHome");
            return null;
        }

        public IActionResult Index()
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var role = HttpContext.Session.GetString("UserRole");
            if (role.Equals("admin"))
                return RedirectToAction("Index", "AdminHome");
            else if (role.Equals("student"))
                return RedirectToAction("Index", "StudentHome");

            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var super = _context.Supervisors
                .FirstOrDefault(s => s.UserId == userId);

            var students = _context.Students
                .Include(s => s.ChosenThesis)
                .Where(s => s.ChosenThesis.SuperId == super.Id)
                .Include(s => s.User);


            var thesesNotChosen = _context.Theses
                .Where(t => t.SuperId == super.Id && t.StudentId == null)
                .ToList();

            var specialitiesForSupervisor = from s in _context.Specialties
                                            join f in _context.Faculties on s.FacId equals f.Id
                                            where f.Id == super.FacultyId
                                            select new Specialty
                                            {
                                                Id = s.Id,
                                                Name = s.Name
                                            };

            return View(new SupervisorPanelViewModel { Students = students, 
                ThesesNotChosen = thesesNotChosen, SpecialitiesForSupervisor = specialitiesForSupervisor});
        }

        public IActionResult removeThesis(int thesisId)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var thes = _context.Theses.Where(t => t.Id == thesisId).First();
            _context.Theses.Remove(thes);
            _context.SaveChanges();
            TempData["Success"] = "Temat został pomyślnie usunięty";
            return RedirectToAction("Index", "SupervisorHome");
        }

        public IActionResult changeThesis(string thesisSubject)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var thesisId = int.Parse(thesisSubject.Substring(0, thesisSubject.IndexOf(" ")));
            thesisSubject = thesisSubject.Substring(thesisSubject.IndexOf(" ") + 1);

            var thes = _context.Theses.Where(t => t.Id == thesisId).First();
            thes.Subject = thesisSubject;
            _context.SaveChanges();
            TempData["Success"] = "Temat został pomyślnie zmieniony";
            return RedirectToAction("Index", "SupervisorHome");
        }

        public IActionResult createThesis(string thesisSubjectCreate, int specialityType)
        {
            var roleAction = checkRole();
            if (roleAction != null)
                return roleAction;

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var sup = _context.Supervisors
               .Where(s => s.UserId == userId)
               .FirstOrDefault();

            var thes = new Thesis { Subject = thesisSubjectCreate, DegreeCycle = 0, 
                SpecId = specialityType, SuperId = sup.Id, StudentId = null};
            _context.Add<Thesis>(thes);
            _context.SaveChanges();

            TempData["Success"] = "Nowy temat został dodany";
            return RedirectToAction("Index", "SupervisorHome");
        }
    }
}