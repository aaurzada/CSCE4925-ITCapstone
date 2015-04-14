using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLSolutions.Infrastructure;
using System;
using System.Collections.Generic;

using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
namespace SQLSolutions.Controllers
{
    public class HomeController:Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(Login user)
        { 
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.euid, user.password) == "admin") //given by view and sent to model IsValid function to check against user table and UNT auth
                {
                    Session["username"] = user.euid; //store euid as session variable
                    Session["isAdmin"] = true; //store is admin as session id. Check at each page 
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "User", new { area = "admin" });//admin page displayed
                }
                else if (user.IsValid(user.euid, user.password) == "nonAdmin")
                {
                    Session["username"] = user.euid; //store euid as session variable
                    Session["isAdmin"] = false; //store is admin as session id. Check at each page
                    return RedirectToAction("Index", "nonAdmin");
                }
                else if (user.IsValid(user.euid, user.password) == "notExists") //if username does not exist in user table then display does not exist
                {
                    ModelState.AddModelError("", "This account does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "The password entered is incorrect."); //if it exists then password is incorrect
                }
            }
            return View(user);
        }
    }
}