using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQLSolutions.Controllers
{
    public class HomeController:Controller
    {
        // GET: Home

   

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(Models.Login user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.euid, user.password) == "admin") //given by view and sent to model IsValid function to check against user table and UNT auth
                {
                    Session["user"] = user.euid; //session created 
                  
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "User", new { area = "Admin" });//admin page displayed
                }
                else if (user.IsValid(user.euid, user.password) == "nonAdmin")
                {
                    Session["user"] = user.euid; //session created 
                    Session["admin"] = false;
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "nonAdmin"); //non admin page displayed
                }
                else if (user.IsValid(user.euid, user.password) == "notExists")
                {

                    ModelState.AddModelError("", "This account does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "The password entered is incorrect.");
                }
            }
            return View(user);
        }
    }
}