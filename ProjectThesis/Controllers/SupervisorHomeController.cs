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
        public IActionResult Index()
        {
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

            return View(new SupervisorPanelViewModel{Students = students, ThesesNotChosen = thesesNotChosen});
        }

        //public IActionResult Post()
        //{
            //When supervisor limit is end we need to reject others thesis subjects
            //Re: we won't let students apply to such supervisors
            // THIS LOGIC SHOULD BE IMPLEMENTED IN StudentHomeController>ReserveThesis
            
            /*int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var thesesSubjects = (    from th in _context.Theses
                                       from sp in _context.Supervisors
                                       from us in _context.Users
                                       where sp.UserId == us.Id && th.SuperId == sp.Id && us.Id == userId && th.StudentId != null
                                       select new
                                       {
                                          thes = th.Subject
                                       });
            var countOfStudents = thesesSubjects.Count();
            var superv = _context.Supervisors
                .FirstOrDefault(s => s.UserId == userId);
            Debug.WriteLine(countOfStudents + " / " + superv.StudentLimit);
            return View();
        }*/

        public IActionResult RemoveStudent(int thesisId)
        {
            var thesis = _context.Theses
                .FirstOrDefault(t => t.Id == thesisId);

            thesis.StudentId = null;
            _context.SaveChanges();

            TempData["Success"] = "Pomyślnie odsunięto studenta od tematu";
            return RedirectToAction("Index", "SupervisorHome");
        }

        public IActionResult removeThesis(int thesisId)
        {
            var thes = _context.Theses.Where(t => t.Id == thesisId).First();
            _context.Theses.Remove(thes);
            _context.SaveChanges();
            return RedirectToAction("Index", "SupervisorHome");
        }

        public IActionResult changeThesis(string thesisSubject)
        {
            var thesisId = int.Parse(thesisSubject.Substring(0, thesisSubject.IndexOf(" ")));
            thesisSubject = thesisSubject.Substring(thesisSubject.IndexOf(" ") + 1);

            var thes = _context.Theses.Where(t => t.Id == thesisId).First();
            thes.Subject = thesisSubject;
            _context.SaveChanges();
            return RedirectToAction("Index", "SupervisorHome");
        }


        /*[HttpPost]
        public IActionResult Post(string decisionButton)
        {
            Debug.WriteLine(decisionButton);
            if (decisionButton.Contains("acc"))
            {
                Debug.WriteLine("accept");
                //Action for accept thesis subject
            }
            else if (decisionButton.Contains("rej"))
            {
                Debug.WriteLine("reject");
                //Action for reject thesis subject
            }

            return this.Post();
        }*/
    }
}