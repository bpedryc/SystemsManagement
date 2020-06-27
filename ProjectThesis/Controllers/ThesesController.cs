using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class ThesesController : Controller
    {
        private readonly ThesisDbContext _context;

        public ThesesController(ThesisDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var theses = _context.Theses
                .Include(t => t.Super)
                .Include(t => t.Super.User)
                .Include(t => t.Student)
                .Include(t => t.Student.User)
                .Include(t => t.Spec)
                .Include(t => t.Spec.Fac)
                .OrderBy(t => (t.Student == null))
                .ThenBy(t => t.Spec.Fac.Name)
                .ThenBy(t => t.Spec.Name)
                .ToList();
            return View(theses);
        }

        public IActionResult RemoveStudent(int thesisId)
        {
            var thesis = _context.Theses
                .FirstOrDefault(t => t.Id == thesisId);

            thesis.StudentId = null;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
