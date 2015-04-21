using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using PagedList;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Models;
using SQLSolutions.ViewModel;

namespace SQLSolutions.Controllers
{
    public class UserBookController : Controller
    {
       
        // GET: Details/Details/5
        public ActionResult Details()
        {
            var user = TempData["userId"].ToString();
            var userBook = (from book in Database.Session.Query<Book>()
                            join transaction in Database.Session.Query<Transaction>()
                            on book.AssetNum equals transaction.BookAssetNumber
                            where transaction.UserId.ToString() == user
                            select new TransactionDetails
                            {

                                Title = book.Title,
                                Author = book.Author,
                                CourseSection = book.CourseSection,
                                DueDate = transaction.DueDate,
                                CheckoutDate = transaction.CheckoutDate,
  

                            }).ToList();
            var details = new TransactionDetailsList
            {
                UserBooks = userBook.ToList()
            };

            return View(details);
        }

        // GET: User/Index
        public ActionResult BookIndex(string searchBook, string currentFilter, int? page)
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

            var bookList = new UserBookIndex
            {
                UserBooks = Database.Session.Query<Book>().Where(b => b.InStock.Equals(true)).OrderBy(b => b.CourseSection).
                 ToPagedList(pageNumber, pageSize)
            };
            //searh book by title, ISBN, AssetNumber, course section, author
            if (!string.IsNullOrEmpty(searchBook))
            {
                bookList = new UserBookIndex
                {
                    UserBooks = Database.Session.Query<Book>().Where(b => b.Title.Contains(searchBook) && b.InStock.Equals(true)
                        || b.Isbn.Contains(searchBook) && b.InStock.Equals(true) 
                        || b.Author.Contains(searchBook) && b.InStock.Equals(true)
                        || b.Isbn.ToString().Contains(searchBook) && b.InStock.Equals(true)
                        || b.CourseSection.ToString().Contains(searchBook) && b.InStock.Equals(true))
                        .ToPagedList(pageNumber, pageSize)
                };
            }

            return View(bookList);
        }

       
    }
}
