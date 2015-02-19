using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string searchUser)
        {
            var userList = Database.Session.Query<User>().ToList();

            //search user by last name and first name
            if (!string.IsNullOrEmpty(searchUser))
            {
                userList = Database.Session.Query<User>().Where(u => u.LastName.Contains(searchUser) || u.FirstName.Contains(searchUser)).ToList();
            }
            return View(userList);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            
            
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        //get the is of the user and display user information for edit
        public ActionResult Edit(int id)
        {
            var editUser = Database.Session.Get<User>(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            return View(editUser);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, User user)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
