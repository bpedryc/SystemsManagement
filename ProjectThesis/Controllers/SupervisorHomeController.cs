﻿using System;
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
using System.Text;
using System.Security.Cryptography;

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
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Supervisor))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            var userId = HttpContext.Session.GetInt32("UserId");

            var user = _context.Users
                .FirstOrDefault(u => (u.Id == userId));

            var supervisor = _context.Supervisors
                .FirstOrDefault(s => (s.UserId == userId));

            var faculty = _context.Faculties
                .FirstOrDefault(f => (f.Id == supervisor.FacultyId));

            return View(new SupervisorViewModel { User = user, Faculty = faculty});
        }

        public IActionResult Theses()
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Supervisor))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            var userId = HttpContext.Session.GetInt32("UserId");

            var super = _context.Supervisors
                .FirstOrDefault(s => s.UserId == userId);

            var students = _context.Students
                .Include(s => s.ChosenThesis)
                .Where(s => s.ChosenThesis.SuperId == super.Id)
                .Include(s => s.User);


            var thesesNotChosen = _context.Theses
                .Where(t => t.SuperId == super.Id && t.StudentId == null)
                .ToList();

            var specialtiesForSupervisor = from s in _context.Specialties
                                            join f in _context.Faculties on s.FacId equals f.Id
                                            where f.Id == super.FacultyId
                                            select new Specialty
                                            {
                                                Id = s.Id,
                                                Name = s.Name
                                            };

            return View(new SupervisorPanelViewModel { 
                Students = students, 
                ThesesNotChosen = thesesNotChosen, 
                SpecialitiesForSupervisor = specialtiesForSupervisor
            });
        }

        public IActionResult removeThesis(int thesisId)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Supervisor))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            var thes = _context.Theses.FirstOrDefault(t => t.Id == thesisId);
            _context.Theses.Remove(thes);
            _context.SaveChanges();
            TempData["Success"] = "Temat został pomyślnie usunięty";
            return RedirectToAction("Theses", "SupervisorHome");
        }

        public IActionResult changeThesis(string thesisSubject)
        {
            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Supervisor))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }

            if (thesisSubject.Substring(thesisSubject.IndexOf(" ") + 1) == "")
            {
                TempData["Error"] = "Temat nie może być pusty";
                return RedirectToAction("Theses", "SupervisorHome");
            }

            var thesisId = int.Parse(thesisSubject.Substring(0, thesisSubject.IndexOf(" ")));
            thesisSubject = thesisSubject.Substring(thesisSubject.IndexOf(" ") + 1);

            var thes = _context.Theses.FirstOrDefault(t => t.Id == thesisId);
            thes.Subject = thesisSubject;
            _context.SaveChanges();
            TempData["Success"] = "Temat został pomyślnie zmieniony";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult createThesis(string thesisSubjectCreate, int specialityType, int degreeCycle)
        {
            if (string.IsNullOrEmpty(thesisSubjectCreate))
            {
                TempData["Error"] = "Temat nie może być pusty";
                return RedirectToAction("Theses", "SupervisorHome");
            }

            if (!AuthenticationController.IsUserAuthorized(HttpContext, AuthenticationController.UserRole.Supervisor))
            {
                return RedirectToAction("NotAuthorized", "Authentication");
            }
            var userId = HttpContext.Session.GetInt32("UserId");

            var sup = _context.Supervisors
                .FirstOrDefault(s => s.UserId == userId);

            var thes = new Thesis { Subject = thesisSubjectCreate, DegreeCycle = degreeCycle, 
                SpecId = specialityType, SuperId = sup.Id, StudentId = null};
            _context.Add(thes);
            _context.SaveChanges();

            TempData["Success"] = "Nowy temat został dodany";
            return RedirectToAction("Theses", "SupervisorHome");
        }

        public IActionResult changePassword(string newPassword)
        {

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user.Password = GetSha256FromString(newPassword);
            _context.SaveChanges();
            return RedirectToAction("Index", "SupervisorHome");
        }

        public IActionResult changeEmail(string newEmail)
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            var us = _context.Users.First(u => u.Id == userId);
            us.Email = newEmail;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
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