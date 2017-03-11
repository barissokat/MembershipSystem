﻿using MembershipSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult EditUser(string id)
        {
            var users = (from u in _db.Users
                         where u.Id == id
                         select new User
                         {
                             Id = u.Id,
                             Name = u.UserName
                         });
            if (users.Count() == 0)
                return RedirectToAction("Index");

            var user = users.FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = _db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                    oldUser.UserName = user.Name;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return this.EditUser(user.Id);
                }
            }
            return View();
        }

        UserManager<ApplicationUser> UserManager { get; set; }

        public MembershipController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        public ActionResult FindUserByName(string id)
        {
            var userInfo = UserManager.FindByName(id);
            var user = new User
            {
                Id = userInfo.Id,
                Name = userInfo.UserName
            };
            return View("DetailsUser", user);
        }

        public ActionResult FindUserById(string id)
        {
            var userInfo = UserManager.FindById(id);
            var user = new User
            {
                Id = userInfo.Id,
                Name = userInfo.UserName
            };
            return View("DetailsUser", user);
        }

        public ActionResult PasswordForUser(string id)
        {
            bool passwordControl = UserManager.HasPassword(id);
            if (passwordControl == false)
            {
                ApplicationUser userInfo = UserManager.FindById(id);
                var user = new UserPassword
                {
                    Id = userInfo.Id,
                    Name = userInfo.UserName
                };
                return View(user);
            }
            else
                return this.FindUserById(id);
        }

        [HttpPost]
        public ActionResult PasswordForUser(string id, FormCollection password)
        {
            if (ModelState.IsValid)
            {
                bool passwordControl = UserManager.HasPassword(id);
                if (passwordControl == false && password["Password"] == password["Password2"])
                {
                    var result = UserManager.AddPassword(id, password["Password"]);
                    if (result.Succeeded)
                        return this.FindUserById(id);
                    else
                        return this.PasswordForUser(id);
                }
                else
                    return this.PasswordForUser(id);
            }
            else
                return this.PasswordForUser(id);
        }
        
        public ActionResult ChangePassword(string id, FormCollection password)
        {
            if (ModelState.IsValid)
            {
                bool passwordControl = UserManager.HasPassword(id);
                if (passwordControl == true)
                {
                    if (password["NewPassword"] == password["NewPassword2"])
                    {
                        var result = UserManager.ChangePassword(id, password["OldPassword"], password["NewPassword"]);
                        if (result.Succeeded)
                            return this.FindUserById(id);
                        else
                            return this.ChangePassword(id, password);
                    }
                    else
                        return this.ChangePassword(id, password);
                }
                else
                    return this.FindUserById(id);
            }
            else
                return this.ChangePassword(id, password);
        }

        /*id ler sorunlu*/
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword password)
        {
            if (ModelState.IsValid)
            {
                bool passwordControl = UserManager.HasPassword(password.Id);
                if (passwordControl == true)
                {
                    if (password.NewPassword == password.NewPassword2)
                    {
                        var result = UserManager.ChangePassword(password.Id, password.OldPassword, password.NewPassword);
                        if (result.Succeeded)
                            return this.FindUserById(password.Id);
                        else
                            return ChangePassword(id);
                    }
                    else
                        return ChangePassword(id);
                }
                else
                    return this.FindUserById(password.Id);
            }
            else
                return ChangePassword(id);
        }
    }
}