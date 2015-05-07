using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Mapping;
using PagedList;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Infrastructure;
using SQLSolutions.Migrations;
using SQLSolutions.Models;


namespace SQLSolutions.Areas.Admin.Controllers
{
    [SelectedTab("User Records")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(string searchUser,string currentFilter, int ? page)
        {
            // currentFilter provides the view with the current filter string. currentFilter will maintain
            // the filter settings during paging and it must be restored to the text box when the page is redisplayed. 
            // If the search string is changed during paging, the page has to be reset to 1, because the new filter 
            // can result in different data to display. The search string is changed when a value is entered in the 
            // text box and the submit button is pressed. In that case, the searchString parameter is not null.
            if (searchUser != null)
            {
                page = 1;
            }
            else
            {
                searchUser = currentFilter;
            }

            ViewBag.currentFilter = searchUser;
            //converts the book query to a single page of books in a collection type that supports paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var userList = new UserIndex { Users = Database.Session.Query<User>().OrderBy(u => u.LastName).ToPagedList(pageNumber, pageSize) };
            //search user by last name and first name and euid
            //check if user typed something in the search box
            if (!string.IsNullOrEmpty(searchUser))
            {
                userList = new UserIndex { Users = Database.Session.Query<User>().Where(u => u.LastName.Contains(searchUser) 
                    || u.FirstName.Contains(searchUser) || u.Euid.Contains(searchUser)).ToPagedList(pageNumber, pageSize)
                };
            }

            return View(userList);
        }
       

        // GET: User/Details/5
        [SelectedTab("User Records")]
        public ActionResult Details(int id)
        {
            //get user id
            var user = Database.Session.Get<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //query Book, Transaction tables to retrieve data about books borrowed by the user
            //and get DueDates and CheckoutDates and return UserDetails entity defined in ViewModels
            var userBook = (from book in Database.Session.Query<Book>()
                            join transaction in Database.Session.Query<Transaction>() 
                            on book.AssetNum equals transaction.BookAssetNumber
                            where transaction.UserId == id && book.InStock.Equals(false) && transaction.CheckInDate == null
                            select new UserDetails
                            {

                                Title = book.Title,
                                Author = book.Author,
                                CourseSection = book.CourseSection,
                                DueDate = transaction.DueDate,
                                CheckoutDate = transaction.CheckoutDate,
                                CheckInDate = transaction.CheckInDate,
                                Isbn = book.Isbn,
                                AssetNum = book.AssetNum

                            });
            //convert userDetails entity to IEnumerable and pass it to the view
            var result = new UserDetailsList
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserBooks = userBook
            };

            return View(result);

        }

        // GET: User/Create
        [SelectedTab("User Records")]
        public ActionResult Create()
        {

            return View(new UserNew { });
        }

        // POST: User/Create
        [HttpPost]
        [SelectedTab("User Records")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserNew form)
        {
            //check if user ID and Euid already exists in the database
            if (Database.Session.Query<User>().Any(u => u.Id == form.Id))
                ModelState.AddModelError("Id", "ID must be unique");
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
                Id = form.Id,
                Euid = form.Euid,
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                isAdmin = form.isAdmin
            };
            //save user to the database
            Database.Session.Save(user);
            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        //get the is of the user and display user information for edit
        [SelectedTab("User Records")]
        public ActionResult Edit(int id)
        {
            var editUser = Database.Session.Load<User>(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            return View(new UserEdit
            {
                Id = editUser.Id,
                Euid = editUser.Euid,
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                Email = editUser.Email,
                isAdmin = editUser.isAdmin
            });
        }

        // POST: User/Edit/5
        [HttpPost]
        [SelectedTab("User Records")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserEdit form)
        {
            var editUser = Database.Session.Get<User>(id);
            if (editUser == null)
            {
                return HttpNotFound();
            }
            //check if user ID and Euid already exists in the database and if this user is a
            //different that is trying to update the filed
            if (Database.Session.Query<User>().Any(u => u.Id == form.Id && u.Id != id))
                ModelState.AddModelError("Id", "ID must be unique");
            if (Database.Session.Query<User>().Any(e => e.Euid == form.Euid && e.Id != id))
                ModelState.AddModelError("Euid", "Euid must be unique");
            //check if Model complient with requirements
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //update fields
            editUser.Id = form.Id;
            editUser.Euid = form.Euid;
            editUser.FirstName = form.FirstName;
            editUser.LastName = form.LastName;
            editUser.Email = form.Email;
            editUser.isAdmin = form.isAdmin;
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

        //public void checkSessionVars()//checks if session variables exist and user is admin
        //{
        //    if (Session["username"] == null || Session["isAdmin"] == null)
        //    {
        //        RedirectToAction("Index", "Home");
        //    }
        //}
    }
}
