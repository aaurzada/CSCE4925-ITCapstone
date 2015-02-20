using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index (string searchUser)
        {
            //var userList = Database.Session.Query<User>().ToList();
             var userList = new UserIndex {Users = Database.Session.Query<User>().ToList()};
            //search user by last name and first name
            //check if user typed something in the search box
             if (!string.IsNullOrEmpty(searchUser))
             {
                 userList = new UserIndex {Users = Database.Session.Query<User>().Where(u => u.LastName.Contains(searchUser) || u.FirstName.Contains(searchUser)).ToList()};
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

            return View( new UserNew {});
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserNew form)
        {
           //check if user ID and Euid already exists in the database
            if(Database.Session.Query<User>().Any(u => u.IdNum == form.IdNum))
                ModelState.AddModelError("IdNum", "ID must be unique");
            if (Database.Session.Query<User>().Any(e => e.Euid == form.Euid))
                ModelState.AddModelError("Euid", "Euid must be unique");
            //check if Model complient with requirements
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //create new user entity
            var user = new User
            {
                IdNum = form.IdNum,
                Euid = form.Euid,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email
            };
            //save user to the database
            Database.Session.Save(user);
            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        //get the is of the user and display user information for edit
        public ActionResult Edit(int id)
        {
            var editUser = Database.Session.Load<User>(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            return View(new UserEdit
            {
                IdNum = editUser.IdNum,
                Euid = editUser.Euid,
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                Email = editUser.Email
            });
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, UserEdit form)
        {
            var editUser = Database.Session.Get<User>(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            //check if user ID and Euid already exists in the database and if this user is a
            //different that is trying to update the filed
            if (Database.Session.Query<User>().Any(u => u.IdNum == form.IdNum && u.IdNum != id))
                ModelState.AddModelError("IdNum", "ID must be unique");
            if (Database.Session.Query<User>().Any(e => e.Euid == form.Euid && e.IdNum != id))
                ModelState.AddModelError("Euid", "Euid must be unique");
            //check if Model complient with requirements
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //update fields
            editUser.IdNum = form.IdNum;
            editUser.Euid = form.Euid;
            editUser.FirstName = form.FirstName;
            editUser.LastName = form.LastName;
            editUser.Email = form.Email;
            //save update to the database
            Database.Session.Update(editUser);

            return RedirectToAction("Index");
        }

        // GET: User/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var deleteUser = Database.Session.Get<User>(id);
            if (deleteUser == null)
            {
                return HttpNotFound();
            }
            //delete user from the database
            Database.Session.Delete(deleteUser);
            return RedirectToAction("Index");
        }
    }
}
