using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using The_Wall.Models;

namespace The_Wall.Controllers
{
    public class HomeController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private MyContext db;
        public HomeController(MyContext context)
        {
            db = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }
        
        [HttpGet("LogIndex")]
        public IActionResult LogIndex()
        {
            return View("LogIndex");
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
            }
            if (ModelState.IsValid == false)
            {
                return View("Index");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);
            db.Users.Add(user);
            db.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.FirstName);
            return RedirectToAction("Board", "Wall");
        }

        [HttpPost("Login")]
        public IActionResult Login(LogUser logUser)
        {
            User checkExists = db.Users.FirstOrDefault(u => u.Email == logUser.LogEmail);
            if (ModelState.IsValid)
            {
                if (checkExists == null)
                {
                    ModelState.AddModelError("LogEmail", "Email or password is invalid");
                    return View("LogIndex");
                }

            User dbUser = db.Users.FirstOrDefault(user => user.Email == logUser.LogEmail);
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            PasswordVerificationResult comparePassword = hasher
            .VerifyHashedPassword(logUser, dbUser.Password, logUser.LogPassword);

                if (comparePassword == 0)
                {
                    ModelState.AddModelError("LogEmail", "Email or password is invalid");
                    return View("LogIndex");
                }

                HttpContext.Session.SetInt32("UserId", dbUser.UserId);
                HttpContext.Session.SetString("UserName", dbUser.FirstName);
                return RedirectToAction("Board", "Wall");
            }
            return View("LogIndex");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
