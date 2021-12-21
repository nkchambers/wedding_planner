using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wedding_planner.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace wedding_planner.Controllers
{
    public class WeddingController : Controller
    {
        private MyContext _context { get; }

        public WeddingController(MyContext context)
        {
            _context = context;
        }


        //USER DASHBOARD PAGE
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            DashboardView ViewModel = new DashboardView
            {
                UserFirstName = _context.Users.FirstOrDefault(u => u.UserId == (int)UserId).FirstName,
                AllWeddings = _context.Wedding
                .Include(w => w.WeddingAttendees)
                .ToList()
            };

            return View(ViewModel);
        }
    } 
}
