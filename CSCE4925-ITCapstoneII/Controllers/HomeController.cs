using SQLSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SQLSolutions.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.Login Login)
        {
            if (ModelState.IsValid) //check if input is valid
            {
                if (Login.IsValid(Login.euid, Login.password) == "admin") //given by view and sent to model IsValid function to check against user table and UNT auth
                {
                    Session["user"] = Login.euid; //session created 
                    Session["admin"] = true;
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "Home"); //admin page displayed
                }
                else if (Login.IsValid(Login.euid, Login.password) == "nonAdmin")
                {
                    Session["user"] = Login.euid; //session created 
                    Session["admin"] = false;
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "Home"); //non admin page displayed
                }
                else if (Login.IsValid(Login.euid, Login.password) == "notExists")
                {

                    ModelState.AddModelError("", "This account does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "The password entered is incorrect.");
                }
            }
            return View(Login);
        }
    }
}