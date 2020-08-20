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
    public class WallController : Controller
    {
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private MyContext db;
        public WallController(MyContext context)
        {
            db = context;
        }

        [HttpGet("/board")]
        public IActionResult Board()
        {
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.AllMsg = db.Messages
            .Include(msg => msg.MessageComments)
            .Include(msg => msg.Poster)
            .OrderByDescending(msg => msg.CreatedAt)
            .ToList();

            ViewBag.AllComm = db.Comments
            .Include(comm => comm.User)
            .ToList();
            return View("Board");
        }

        [HttpPost("/CreateMsg")]
        public IActionResult CreateMsg(Message newMsg)
        {
            newMsg.UserId = (int)uid;
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Add(newMsg);
                db.SaveChanges();
            }
            return RedirectToAction("Board", "Wall");
        }

        [HttpPost("/CreateComment")]
        public IActionResult CreateComment(Comment newComment, int messageId)
        {
            newComment.UserId = (int)uid;
            newComment.MessageId = messageId;

            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Add(newComment);
                db.SaveChanges();
            }
            ViewBag.AllMsg = db.Messages
            .Include(msg => msg.Poster)
            .Include(msg => msg.MessageComments)
            .OrderByDescending(msg => msg.CreatedAt)
            .ToList();

            ViewBag.AllComm = db.Comments
            .Include(comm => comm.User)
            .ToList();
            
            return RedirectToAction("Board", "Wall");
        }

        [HttpPost("/Delete")]
        public IActionResult DeleteMsg(int messageId)
        {
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Message thisMsg = db.Messages
            .Include(msg => msg.Poster)
            .FirstOrDefault(msg => msg.MessageId == messageId);
            
            db.Messages.Remove(thisMsg);
            db.SaveChanges();

            return RedirectToAction("Board", "Wall");
        }

        [HttpPost("/DeleteComment")]
        public IActionResult DeleteComment(int commentId)
        {
            if (uid == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Comment thisComm = db.Comments
            .Include(msg => msg.User)
            .FirstOrDefault(msg => msg.CommentId == commentId);
            
            db.Comments.Remove(thisComm);
            db.SaveChanges();

            return RedirectToAction("Board", "Wall");
        }
    }
}