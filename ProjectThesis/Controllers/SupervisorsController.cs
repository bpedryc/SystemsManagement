﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectThesis.Models;
using ProjectThesis.ViewModels;

namespace ProjectThesis.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ThesisDbContext _context;

        public SupervisorsController(ThesisDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var supervisors = _context.Supervisors
                .Include(s => s.User)
                .ToList();
            return View(supervisors);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supervisor supervisor)
        {
            

            return RedirectToAction("Index", "Supervisors");
        }

        // GET: SupervisorsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SupervisorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SupervisorsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SupervisorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
