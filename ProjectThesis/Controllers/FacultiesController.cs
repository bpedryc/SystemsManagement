using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly ThesisDbContext _context;

        public FacultiesController(ThesisDbContext context)
        {
            _context = context;
        }
        public ActionResult GetAllFaculties()
        {
            var faculties = _context.Faculties
                .Select(f => new {f.Id, f.Name})
                .ToList();
            return Json(faculties);
        }

        
    }
}
