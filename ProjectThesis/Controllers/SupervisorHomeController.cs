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


            //It's not the best option, but it works

            var thesesWithStudents = (from st in _context.Students
                              from th in _context.Theses
                              from sp in _context.Supervisors
                              from us in _context.Users
                              where sp.Id == super.Id && us.Id == st.UserId && th.StudentId == st.Id && sp.Id == th.SuperId
                              select new
                              {
                                  sub = th.Subject,
                                  name = us.FirstName,
                                  lname = us.LastName,
                                  email = us.Email,
                                  number = st.StudentNo
                              });

            var restOfTheses = (from th in _context.Theses
                         where th.SuperId == super.Id && th.StudentId == null
                         select new
                         {
                             sub = th.Subject,
                         });

            List<string> allTheses = new List<string>();

            foreach (var thes in thesesWithStudents)
            {
                allTheses.Add(thes.sub + "," + thes.name + " " + thes.lname + " " + thes.number + " " + thes.email);
            }

            foreach (var thes in restOfTheses)
            {
                allTheses.Add(thes.sub + "," + "Nie wybrano");
            }

            return View(allTheses);
        }

        public IActionResult Post()
        {
            //Get students new thesis subjects and return them to proper supervisor

            //When supervisor limit is end we need to reject others thesis subjects 
            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
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
                                .Where(s => s.UserId == userId)
                                .FirstOrDefault<Supervisor>();
            Debug.WriteLine(countOfStudents + " / " + superv.StudentLimit);
            return View();
        }

        [HttpPost]
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
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.SetString("UserId", "");
            return RedirectToAction("Login", "Authentication");
        }
    }
}