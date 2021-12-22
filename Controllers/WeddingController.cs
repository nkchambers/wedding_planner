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
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            DashboardView ViewModel = new DashboardView
            {
                LoggedUserId = (int)UserId,
                
                UserFirstName = _context.Users.FirstOrDefault(u => u.UserId == (int)UserId).FirstName,
                
                AllWeddings = _context.Wedding
                .Include(w => w.WeddingAttendees)
                .ToList()
            };

            return View(ViewModel);
        }


        //GET NEW WEDDING FORM
        [HttpGet("wedding/new")]
        public IActionResult NewWedding()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            return View("NewWedding");
        }


        //CREATE WEDDING - POST 
        [HttpPost("wedding/create")]
        public IActionResult CreateWedding(Wedding fromForm)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            if (ModelState.IsValid)
            {

                fromForm.UserId = (int)UserId;

                _context.Add(fromForm);
                _context.SaveChanges();


                return RedirectToAction("dashboard");
            }
            else
            {
                return View("NewWedding");
            }
        }


        //GET ONE WEDDING - READ INFO & INCLUDE GUEST LIST 
        [HttpGet("wedding/{weddingId}")]
        
        public IActionResult WeddingInfo(int weddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            WeddingInfoView ViewModel = new WeddingInfoView()
            {

                Wedding = _context.Wedding
                .Include(w => w.WeddingAttendees)
                    .ThenInclude(wa => wa.AttendedBy)
                .FirstOrDefault(w => w.WeddingId == weddingId)
            };

            if(ViewModel == null)
            {
                return RedirectToAction("Dashboard");
            }

            return View("WeddingInfo", ViewModel);
        }


        [HttpGet("wedding/{weddingId}/Un-RSVP")]
        public IActionResult UnRSVP(int weddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            WeddingAttendees GuestToRemove = _context.WeddingAttendees
            .Where(wa => wa.WeddingId == weddingId && wa.UserId == UserId)
            .FirstOrDefault();


            _context.Remove(GuestToRemove);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");

        }


        [HttpGet("wedding/{weddingId}/RSVP")]
        public IActionResult RSVP(int weddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            WeddingAttendees GuestToAdd = new WeddingAttendees()
            {
                WeddingId = weddingId,
                UserId = Convert.ToInt32(UserId)
            };

            _context.Add(GuestToAdd);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");

        }




        //DELETE WEDDING METHOD
        [HttpGet("wedding/{weddingId}/delete")]
        public RedirectToActionResult DeleteWedding(int weddingId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return RedirectToAction("Index", "LogReg");
            }

            Wedding toDelete = _context.Wedding.FirstOrDefault(w => w.WeddingId == weddingId);

            if((int)UserId != toDelete.UserId)
            {
                return RedirectToAction("Dashboard");
            }


            _context.Remove(toDelete);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

    } 
}
