using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wedding_planner.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace wedding_planner.Controllers
{
    public class LogRegController : Controller
    {
        private MyContext _context { get; }

        public LogRegController (MyContext context)
        {
            _context = context;
        }

        //INDEX METHOD (REGISTRATION/LOGIN PAGE)
        [HttpGet("")]
        public ViewResult Index()
        {
            return View("Index");
        }


        //REGISTER USER METHOD 
        [HttpPost("register")]
        public IActionResult Register(User fromForm)
        {
            if(ModelState.IsValid)
            {
                //Check if email is already registerd in db
                if(_context.Users.Any(u => u.Email == fromForm.Email))
                {
                    //If Email is in db, byeeee
                    return Index();
                }

                //Otherwise, encrypt password and continue registration process
                PasswordHasher<User> hashbrown = new PasswordHasher<User>();

                fromForm.Password = hashbrown.HashPassword(fromForm, fromForm.Password);


                //Add User to Db, add user into session and redirect to user dashboard
                _context.Add(fromForm);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", fromForm.UserId);

                return RedirectToAction("Dashboard");
            }
            else
            {
                return Index();
            }
        }


        //LOGIN USER METHOD
        [HttpPost("login")]
        public IActionResult Login(LoginUser fromForm)
        {
            if(ModelState.IsValid)
            {
                //Grab user from Db using form data
                User inDb = _context.Users.FirstOrDefault(u => u.Email == fromForm.LogEmail);

                //Check if user exists in Db using their provided Email
                if(inDb == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    
                    return Index();
                }

                //If Yes then compare/verify hashed passwords
                PasswordHasher<LoginUser> hashbrown = new PasswordHasher<LoginUser>();

                var result = hashbrown.VerifyHashedPassword(fromForm, inDb.Password, fromForm.LogPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    
                    return Index();
                }

                //If hashed passwrods do match, create session and redirect user to their dashboard
                HttpContext.Session.SetInt32("UserId", inDb.UserId);
                
                return RedirectToAction("Dashboard", "Wedding");

            }
            else 
            {
                return Index();
            }
        }


        /*//USER DASHBOARD PAGE
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
                UserFirstName = _context.Users.FirstOrDefault(u => u.UserId == (int)UserId).FirstName
            };

            return View(ViewModel);
        }*/


        //LOGOUT USER METHOD
        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            
            return RedirectToAction("Index");
        }
    }
}