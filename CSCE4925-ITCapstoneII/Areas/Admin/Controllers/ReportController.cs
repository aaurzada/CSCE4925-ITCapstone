using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Mapping;
using SQLSolutions.Areas.Admin.ViewModels;
using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using PagedList;



namespace SQLSolutions.Areas.Admin.Controllers
{

    public class ReportController : Controller
    {
        // GET: Admin/Report
        [SelectedTab("Reports")]
        public ActionResult IndexBookReport(string currentSelect, int? page, string selected = null)
        {
            //store selected value in the dropdown menu to return correct page
            ViewBag.currentSelect = selected;
            //specify how many entries to display on the page
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var bookList = (from books in Database.Session.Query<Book>()
                            select new ReportBook
                            {
                                AssetNum = books.AssetNum,
                                Isbn = books.Isbn,
                                Title = books.Title,
                                Author = books.Author,
                                CourseSection = books.CourseSection,
                                Year = books.Year,
                                Edition = books.Edition,
                                IsRequired = (books.IsRequired ? "Yes" : "No"),
                                InStock = (books.InStock ? "Yes" : "No")
                            });

            //var bookList = new ReportBookList() { Books = Database.Session.Query<Book>().ToList().ToPagedList(pageNumber, pageSize) };

            //populate dropdownlist
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.AddRange(new[]
            {
                new SelectListItem() {Text = "All Books", Value = "0", Selected = true},
                new SelectListItem() {Text = "Available Books", Value = "1", Selected = false},
                new SelectListItem() {Text = "Unavailable Books", Value = "2", Selected = false}
            });
            //assign dropdown to the ViewBag
            ViewBag.Selected = ListItems;

            //check what user has selected, if option All Books with value = "0" then display list of all books
            if (selected == "0")
            {
                bookList = (from books in Database.Session.Query<Book>()
                            select new ReportBook
                            {
                                AssetNum = books.AssetNum,
                                Isbn = books.Isbn,
                                Title = books.Title,
                                Author = books.Author,
                                CourseSection = books.CourseSection,
                                Year = books.Year,
                                Edition = books.Edition,
                                IsRequired = (books.IsRequired ? "Yes" : "No"),
                                InStock = (books.InStock ? "Yes" : "No")
                            });

            }

            //check what user has selected, if option Available with value = "1" then display list of all available books
            if (selected == "1")
            {
                bookList = (from books in Database.Session.Query<Book>()
                            where books.InStock.Equals(true)
                            select new ReportBook
                            {
                                AssetNum = books.AssetNum,
                                Isbn = books.Isbn,
                                Title = books.Title,
                                Author = books.Author,
                                CourseSection = books.CourseSection,
                                Year = books.Year,
                                Edition = books.Edition,
                                IsRequired = (books.IsRequired ? "Yes" : "No"),
                                InStock = (books.InStock ? "Yes" : "No")
                            });
            }
            //check what user has selected, if option Available with value = "2" then display list of all unavailable books
            if (selected == "2")
            {
                bookList = (from books in Database.Session.Query<Book>()
                            where books.InStock.Equals(false)
                            select new ReportBook
                            {
                                AssetNum = books.AssetNum,
                                Isbn = books.Isbn,
                                Title = books.Title,
                                Author = books.Author,
                                CourseSection = books.CourseSection,
                                Year = books.Year,
                                Edition = books.Edition,
                                IsRequired = (books.IsRequired ? "Yes" : "No"),
                                InStock = (books.InStock ? "Yes" : "No")
                            });
            }
            //store queries in the TempData to pass it to the Export method 
            TempData["list"] = bookList.ToList();

            //pass bookList object to the IPagedList ReportBookList 
            var bookLists = new ReportBookList()
              {
                  Books = bookList.ToPagedList(pageNumber, pageSize)
              };

            return View(bookLists);
        }




        //transaction method
        [SelectedTab("Reports")]
        public ActionResult TransactionReports(string currentFilter, string currentSelect, string currentBegin, string
            currentEnd, int? page, DateTime? begin,
            DateTime? end, string ex, string selected = null, string searchValue = null)
        {
            // currentFilter provides the view with the current filter string. currentFilter will maintain
            // the filter settings during paging and it must be restored to the text box when the page is redisplayed. 
            // If the search string is changed during paging, the page has to be reset to 1, because the new filter 
            // can result in different data to display. The search string is changed when a value is entered in the 
            // text box and the submit button is pressed. In that case, the searchString parameter is not null.
            if (searchValue != null)
            {
                page = 1;
            }
            else
            {
                searchValue = currentFilter;
            }
            //save values in the ViewBag to return the correct page during the search
            //save searchValue
            ViewBag.currentFilter = searchValue;
            //save selected option in the dropdown list
            ViewBag.currentSelect = selected;
            //check if begin and end dates are entered, then convert them to short date fomrat
            if (begin != null || end != null)
            {
                //save begin date and convert begin to short date to display just date 
                ViewBag.currentBegin = begin.ToString().AsDateTime().ToShortDateString();
                //save the end date
                ViewBag.currentEnd = end.ToString().AsDateTime().ToShortDateString();
            }
            //otherwise make them null
            else
            {
                //save begin date and convert begin to short date to display just date 
                ViewBag.currentBegin = begin; //.ToString().AsDateTime().ToShortDateString();
                //save the end date
                ViewBag.currentEnd = end; //.ToString().AsDateTime().ToShortDateString();
            }

            //converts the book query to a single page of books in a collection type that supports paging
            //pageSize specifies number of entries that will be displayed on the page
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var transaction = (from book in Database.Session.Query<Book>()
                               join transact in Database.Session.Query<Transaction>()
                                   on book.AssetNum equals transact.BookAssetNumber
                               join borrower in Database.Session.Query<User>()
                                   on transact.UserId equals borrower.Id
                               orderby transact.CheckoutDate
                               descending 
                               select new TransactionReport
                               {
                                   Euid = borrower.Euid,
                                   FirstName = borrower.FirstName,
                                   LastName = borrower.LastName,
                                   Email = borrower.Email,
                                   Title = book.Title,
                                   Author = book.Author,
                                   CourseSection = book.CourseSection,
                                   Year = book.Year,
                                   DueDate = transact.DueDate.ToShortDateString(),
                                   CheckoutDate = transact.CheckoutDate.ToShortDateString(),
                                   CheckInDate = transact.CheckInDate,
                                   Isbn = book.Isbn,
                                   AssetNum = book.AssetNum,
                                   Edition = book.Edition,
                                   IsRequired = (book.IsRequired ? "Yes" : "No")
                               }).ToList();

            //create drop down menu and assign values to the items
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.AddRange(new[]
            {
                //selecte item "All" as default
                new SelectListItem() {Text = "All", Value = "0", Selected = true},
                new SelectListItem() {Text = "EUID", Value = "1", Selected = false},
                new SelectListItem() {Text = "Name", Value = "2", Selected = false},
                new SelectListItem() {Text = "Email", Value = "3", Selected = false},
                new SelectListItem() {Text = "ISBN", Value = "4", Selected = false},
                new SelectListItem() {Text = "Title", Value = "5", Selected = false},
                new SelectListItem() {Text = "Author", Value = "6", Selected = false},
                new SelectListItem() {Text = "Course", Value = "7", Selected = false},
                new SelectListItem() {Text = "Checkout Date", Value = "8", Selected = false},
                new SelectListItem() {Text = "Due Date", Value = "9", Selected = false},
                new SelectListItem() {Text = "Check-in Date", Value = "10", Selected = false},
                new SelectListItem() {Text = "Asset Number", Value = "11", Selected = false},
                new SelectListItem() {Text = "--------------------"},
                new SelectListItem() {Text = "Unavailable Books", Value = "12", Selected = false}
            });
            //assign dropdown to the viewbag to display it
            ViewBag.Selected = ListItems;


            if (!string.IsNullOrEmpty(searchValue))
            {
                DateTime? date;
                switch (selected)
                {
                    case "0":

                       
                        //check if searchValue is in DateTime format
                        if (searchValue.IsDateTime())
                        {
                            //if so, store it in the date variable
                            date = DateTime.Parse(searchValue);
                        }
                        else
                        {
                            date = null;
                        }
                        
                        transaction = transaction.AsQueryable().
                            Where(u => u.Euid.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.FirstName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.LastName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.Title.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.Isbn.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.Author.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                       || u.CheckoutDate.AsDateTime() == date
                                       || u.CheckInDate == date
                                       || u.DueDate.AsDateTime() == date
                                       || u.AssetNum.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.CheckoutDate)
                            .ToList();

                        break;

                    case "1":

                        transaction = transaction.AsQueryable()
                            .Where(u => u.Euid.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.LastName)
                            .ToList();

                        break;

                    case "2":
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            transaction = transaction.AsQueryable().
                                Where(u => u.FirstName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                           || u.LastName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                                .OrderByDescending(u => u.LastName)
                                .ToList();

                        }
                        break;
                    case "3":
                        transaction = transaction.AsQueryable()
                            .Where(u => u.Email.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.LastName)
                            .ToList();
                        break;
                    case "4":

                        transaction = transaction.AsQueryable()
                            .Where(u => u.Isbn.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.Isbn)
                            .ToList();

                        break;
                    case "5":

                        transaction = transaction.AsQueryable()
                            .Where(u => u.Title.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.Title)
                            .ToList();
                        break;
                    case "6":

                        transaction = transaction.AsQueryable()
                            .Where(u => u.Author.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.Author)
                            .ToList();

                        break;
                    case "7":
                        transaction = transaction.AsQueryable()
                            .Where(u => u.CourseSection.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.CourseSection)
                            .ToList();
                        break;
                    case "8":

                        if (searchValue.IsDateTime())
                        {
                            date = DateTime.Parse(searchValue);
                        }
                        else
                        {
                            date = null;
                        }
                        transaction = transaction.AsQueryable()
                            .Where(
                                    u =>u.CheckoutDate.AsDateTime().Year.ToString()
                                    .IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                    || u.CheckoutDate.AsDateTime() == date)
                            .OrderByDescending(u => u.CheckoutDate)
                            .ToList();
                        break;
                    case "9":
                        if (searchValue.IsDateTime())
                        {
                            date = DateTime.Parse(searchValue);
                        }
                        else
                        {
                            date = null;
                        }
                        transaction = transaction.AsQueryable()
                            .Where(
                                u =>u.DueDate.AsDateTime().Year.ToString()
                                    .IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                    || u.DueDate.AsDateTime() == date)
                            .OrderByDescending(u => u.DueDate)
                            .ToList();
                        break;
                    case "10":
                        if (searchValue.IsDateTime())
                        {
                            date = DateTime.Parse(searchValue);
                        }
                        else
                        {
                            date = null;
                        }
                        transaction = transaction.AsQueryable()
                            .Where(
                                    u =>u.CheckInDate.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0
                                        && u.CheckInDate !=null
                                    || u.CheckInDate == date && date != null)
                            .OrderByDescending(u => u.CheckInDate)
                            .ToList();
                        break;
                    case "11":
                        transaction = transaction.AsQueryable()
                            .Where(
                                u => u.AssetNum.ToString().IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >= 0)
                            .OrderByDescending(u => u.AssetNum)
                            .ToList();
                        break;
                    

                    default:
                        transaction = (from tr in transaction.AsQueryable()
                                       select new TransactionReport
                                       {
                                           Euid = tr.Euid,
                                           FirstName = tr.FirstName,
                                           LastName = tr.LastName,
                                           Email = tr.Email,
                                           Title = tr.Title,
                                           Author = tr.Author,
                                           CourseSection = tr.CourseSection,
                                           Year = tr.Year,
                                           DueDate = tr.DueDate,
                                           CheckoutDate = tr.CheckoutDate,
                                           CheckInDate = tr.CheckInDate,
                                           Isbn = tr.Isbn,
                                           AssetNum = tr.AssetNum,
                                           Edition = tr.Edition,
                                           IsRequired = tr.IsRequired
                                       }).ToList();
                        break;

                }
            }
            if (selected == "12")
            {
                transaction = transaction.AsQueryable()
                           .Where(u => u.CheckInDate == null)
                           .OrderByDescending(u => u.CheckInDate)
                           .ToList();
            }
            //filter by the begin date and end date
            if (begin != null && end != null)
            {
                transaction = transaction.AsQueryable()
                            .Where(u => u.CheckoutDate.AsDateTime() >= begin && u.CheckoutDate.AsDateTime() <= end
                                     || u.CheckInDate >= begin && u.CheckInDate <= end
                                     || u.DueDate.AsDateTime() >= begin && u.DueDate.AsDateTime() <= end
                                     || u.CheckoutDate.AsDateTime() >= begin && u.CheckoutDate.AsDateTime() <= end && u.CheckInDate == null)
                            .OrderByDescending(u => u.CheckoutDate)
                            .ToList();
            }

         

            //create temdata to pass to the Export method
            TempData["list"] = transaction;


            //add transaction object to list of ViewModels.TransactionReports 
            var transactList = new TransactionReportList()
            {

                TransactionReports = transaction.ToPagedList(pageNumber, pageSize)
            };
            
            return View(transactList);

        }



        //create method to export report to excel file
        public void Export ()
        {

            

            var grid = new GridView();
            grid.DataSource = TempData["list"];
            grid.DataBind();
            Response.ClearContent();
            Response.ContentEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachement; filename = Report.xls");
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.End();
        }


    }

}
