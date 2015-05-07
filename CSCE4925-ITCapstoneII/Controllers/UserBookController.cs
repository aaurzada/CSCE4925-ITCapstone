using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using PagedList;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using SQLSolutions.ViewModel;

namespace SQLSolutions.Controllers
{
    public class UserBookController : Controller
    {
        //displays the list of books that were checked out by the logged in user
        [SelectedTab("Home")]
        public ActionResult BookDetails()
        {
            var user = Session["username"];
            var getuser = Database.Session.Query<User>().Where(u => u.Euid.Equals(user.ToString())).Select(u => u.Id);
            var userBook = (from book in Database.Session.Query<Book>()
                            join transaction in Database.Session.Query<Transaction>()
                            on book.AssetNum equals transaction.BookAssetNumber
                            where transaction.UserId.ToString() == getuser.ToString() &&
                            transaction.CheckInDate == null
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

        // GET: displays list of library books, provides search option
        [SelectedTab("Search")]
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
                UserBooks = Database.Session.Query<Book>().OrderBy(b => b.CourseSection).
                 ToPagedList(pageNumber, pageSize)
            };
            //searh book by title, ISBN, AssetNumber, course section, author
            if (!string.IsNullOrEmpty(searchBook))
            {
                bookList = new UserBookIndex
                {
                    UserBooks = Database.Session.Query<Book>().Where(b => b.Title.Contains(searchBook) 
                        || b.Isbn.Contains(searchBook) 
                        || b.Author.Contains(searchBook) 
                        || b.CourseSection.ToString().Contains(searchBook))
                        .ToPagedList(pageNumber, pageSize)
                };
            }

            return View(bookList);
        }

       
    }
}
