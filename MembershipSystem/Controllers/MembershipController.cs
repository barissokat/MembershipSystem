using MembershipSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipSystem.Controllers
{
    public class MembershipController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Membership
        public ActionResult Index()
        {
            var users = _db.Users.Select(u => new User
            {
                Id = u.Id,
                Name = u.UserName
            }).ToList();
            return View(users);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = newUser.Name
                };
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            return View();
        }
    }
}