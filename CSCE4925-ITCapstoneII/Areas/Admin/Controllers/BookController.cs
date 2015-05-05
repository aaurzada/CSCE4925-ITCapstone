using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using PagedList;

namespace SQLSolutions.Areas.Admin.Controllers
{
    [SelectedTab("Catalog Management")]
    public class BookController : Controller
    {
        // GET: Admin/Book
        public ActionResult Index(string searchBook, string currentFilter, int ? page)
        {
            // currentFilter provides the view with the current filter string. currentFilter will maintain
            // the filter settings during paging and it must be restored to the text box when the page is redisplayed. 
            // If the search string is changed during paging, the page has to be reset to 1, because the new filter 
            // can result in different data to display. The search string is changed when a value is entered in the 
            // text box and the submit button is pressed. In that case, the searchString parameter is not null.
            if (searchBook != null)
            {
                page = 1;
            }
            else
            {
                searchBook = currentFilter;
            }

            ViewBag.currentFilter = searchBook;
            //converts the book query to a single page of books in a collection type that supports paging
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var bookList = new BookIndex { Books = Database.Session.Query<Book>().OrderBy(b => b.AssetNum).
                                            ToPagedList(pageNumber, pageSize) };
           //searh book by title, ISBN, AssetNumber, course section, author
            if (!string.IsNullOrEmpty(searchBook))
            {
                bookList = new BookIndex
                {
                    Books = Database.Session.Query<Book>().Where(b => b.Title.Contains(searchBook)
                        || b.Isbn.Contains(searchBook) || b.Author.Contains(searchBook)
                        || b.AssetNum.ToString().Contains(searchBook)
                        || b.CourseSection.ToString().Contains(searchBook)).ToPagedList(pageNumber, pageSize)
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
        [SelectedTab("Catalog Management")]
        public ActionResult Create()
        {
            return View( new BookNew {});
        }

        // POST: Admin/Book/Create
        [HttpPost]
        [SelectedTab("Catalog Management")]
        [ValidateAntiForgeryToken]
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
                IsRequired = form.IsRequired,
                InStock = true
            };
            //save user to the database
            Database.Session.Save(book);
            return RedirectToAction("Index");
        }

        // GET: Admin/Book/Edit/5
        [SelectedTab("Catalog Management")]
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
                IsRequired = bookEdit.IsRequired,
            });
        }

        // POST: Admin/Book/Edit/5
        [HttpPost]
        [SelectedTab("Catalog Management")]
        [ValidateAntiForgeryToken]
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
