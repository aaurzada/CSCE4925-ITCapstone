using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Infrastructure;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [SelectedTab("catalog")]
    public class BookController : Controller
    {
        // GET: Admin/Book
        public ActionResult Index(string searchBook)
        {
            var bookList = new BookIndex { Books = Database.Session.Query<Book>().ToList() };
           //searh book by title, ISBN, AssetNumber, course section, author
            if (!string.IsNullOrEmpty(searchBook))
            {
                bookList = new BookIndex
                {
                    Books = Database.Session.Query<Book>().Where(b => b.Title.Contains(searchBook)
                        || b.Isbn.Contains(searchBook) || b.Author.Contains(searchBook)
                        || b.AssetNum.ToString().Contains(searchBook) || b.CourseSection.ToString().Contains(searchBook)).ToList()
                };
            }
            
            return View(bookList);
        }

        // GET: Admin/Book/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Book/Create
        public ActionResult Create()
        {
            return View( new BookNew {});
        }

        // POST: Admin/Book/Create
        [HttpPost]
        public ActionResult Create(BookNew form)
        {
            if (Database.Session.Query<Book>().Any(b => b.AssetNum == form.AssetNum))
                ModelState.AddModelError("AssetNum", "Asset Number must be unique");
            
            //check if Model complient with requirements
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            //create new user entity
            var book = new Book
            {
                AssetNum = form.AssetNum,
                Isbn = form.Isbn,
                Title = form.Title,
                Author = form.Author,
                CourseSection = form.CourseSection,
                Year = form.Year,
                Edition = form.Edition,
                IsRequired = form.IsRequired
            };
            //save user to the database
            Database.Session.Save(book);
            return RedirectToAction("Index");
        }

        // GET: Admin/Book/Edit/5
        public ActionResult Edit(int id)
        {
            var bookEdit = Database.Session.Get<Book>(id);
            if (bookEdit == null)
            {
                return HttpNotFound();
            }

            return View( new BookEdit
            {
                AssetNum = bookEdit.AssetNum,
                Isbn = bookEdit.Isbn,
                Title = bookEdit.Title,
                Author = bookEdit.Author,
                CourseSection = bookEdit.CourseSection,
                Year = bookEdit.Year,
                Edition = bookEdit.Edition,
                IsRequired = bookEdit.IsRequired
            });
        }

        // POST: Admin/Book/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, BookEdit form)
        {
            var bookEdit = Database.Session.Get<Book>(id);
            if (bookEdit == null)
            {
                return HttpNotFound();
            }
            if (Database.Session.Query<Book>().Any(b => b.AssetNum == form.AssetNum && b.AssetNum != id))
                ModelState.AddModelError("AssetNum", "Asset Number must be unique");
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            bookEdit.AssetNum = form.AssetNum;
            bookEdit.Isbn = form.Isbn;
            bookEdit.Title = form.Title;
            bookEdit.Author = form.Author;
            bookEdit.CourseSection = form.CourseSection;
            bookEdit.Year = form.Year;
            bookEdit.Edition = form.Edition;
            bookEdit.IsRequired = form.IsRequired;

            Database.Session.Update(bookEdit);

            return RedirectToAction("Index");
        }

        // GET: Admin/Book/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Book/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var bookDelete = Database.Session.Get<Book>(id);
            if (bookDelete == null)
            {
                return HttpNotFound();
            }
            //delete book from the database
            Database.Session.Delete(bookDelete);
            return RedirectToAction("Index");
        }
    }
}
