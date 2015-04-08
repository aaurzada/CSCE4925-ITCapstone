using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Admin/Report
        [SelectedTab("Reports")]
        public ActionResult IndexBookReport(string selected = null)
        {

            var bookList = new ReportBookList() { Books = Database.Session.Query<Book>().ToList() };

            //populate dropdownlist
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.AddRange(new[]
            {
                new SelectListItem() {Text = "All Books", Value = "0", Selected = true},
                new SelectListItem() {Text = "Available Books", Value = "1", Selected = false},
                new SelectListItem() {Text = "Not Available Books", Value = "2", Selected = false}
            });
            //assign dropdown to the ViewBag
            ViewBag.Selected = ListItems;

            //check what user has selected, if option All Books with value = "0" then display list of all books
            if (selected == "0")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().ToList()
                };
            }
            //check what user has selected, if option Available with value = "1" then display list of all available books
            if (selected == "1")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().Where(b => b.InStock == true).ToList()
                };
            }
            //check what user has selected, if option Available with value = "2" then display list of all unavailable books
            if (selected == "2")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().Where(b => b.InStock == false).ToList()
                };
            }

            return View(bookList);
        }



        // GET: Admin/Report/Details/5
        public ActionResult TransactionReports(string searchValue = null)
        {
            var transaction = (from book in Database.Session.Query<Book>()
                               join transact in Database.Session.Query<Transaction>()
                               on book.AssetNum equals transact.BookAssetNumber
                               join borrower in Database.Session.Query<User>()
                               on transact.UserId equals borrower.Id
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
                                   DueDate = transact.DueDate,
                                   CheckoutDate = transact.CheckoutDate,
                                   CheckInDate = transact.CheckInDate,
                                   Isbn = book.Isbn,
                                   AssetNum = book.AssetNum,
                                   Edition = book.Edition,
                                   IsRequired = book.IsRequired.ToString().Replace("1", "Yes")
                               }).ToList();
            //List<SelectListItem> ListItems = new List<SelectListItem>();
            //ListItems.AddRange(new[]
            //{
            //    new SelectListItem() {Text = "All", Value = "0", Selected = true},
            //    new SelectListItem() {Text = "EUID", Value = "1", Selected = false},
            //    new SelectListItem() {Text = "Name", Value = "2", Selected = false},
            //    new SelectListItem() {Text = "Email", Value = "3", Selected = false},
            //    new SelectListItem() {Text = "ISBN", Value = "4", Selected = false},
            //    new SelectListItem() {Text = "Title", Value = "5", Selected = false},
            //    new SelectListItem() {Text = "Author", Value = "6", Selected = false},
            //    new SelectListItem() {Text = "Course", Value = "7", Selected = false},
            //    new SelectListItem() {Text = "Checkout Date", Value = "8", Selected = false},
            //    new SelectListItem() {Text = "Due Date", Value = "9", Selected = false},
            //    new SelectListItem() {Text = "Check-in Date", Value = "10", Selected = false},
            //    new SelectListItem() {Text = "Asset Number", Value = "11", Selected = false},
            //});

            //if (selected == "0")
            //{
            //    transaction = transaction;
            //}
            //if (selected == "1" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where
            //                   borrower.Euid.Contains(searchValue)
            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "2" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where borrower.FirstName.Contains(searchValue)
            //                   || borrower.LastName.Contains(searchValue)
                              
            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "3" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where borrower.Email.Contains(searchValue)

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "4" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where book.Isbn.Contains(searchValue)

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "5" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where book.Title.Contains(searchValue)

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "6" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where book.Author.Contains(searchValue)

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "7" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where book.CourseSection.Contains(searchValue)

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "8" && !string.IsNullOrEmpty(searchValue))
            //{
            //    DateTime temp = DateTime.Parse(searchValue);
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where transact.CheckoutDate == temp

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "9" && !string.IsNullOrEmpty(searchValue))
            //{
            //    DateTime temp = DateTime.Parse(searchValue);
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where transact.DueDate == temp

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "10" && !string.IsNullOrEmpty(searchValue))
            //{
            //    DateTime temp = DateTime.Parse(searchValue);
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where transact.CheckInDate == temp

            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
            //if (selected == "11" && !string.IsNullOrEmpty(searchValue))
            //{
            //    transaction = (from book in Database.Session.Query<Book>()
            //                   join transact in Database.Session.Query<Transaction>()
            //                   on book.AssetNum equals transact.BookAssetNumber
            //                   join borrower in Database.Session.Query<User>()
            //                   on transact.UserId equals borrower.Id
            //                   where book.AssetNum.ToString().Contains(searchValue)
            //                   select new TransactionReport
            //                   {
            //                       Euid = borrower.Euid,
            //                       FirstName = borrower.FirstName,
            //                       LastName = borrower.LastName,
            //                       Email = borrower.Email,
            //                       Title = book.Title,
            //                       Author = book.Author,
            //                       CourseSection = book.CourseSection,
            //                       Year = book.Year,
            //                       DueDate = transact.DueDate,
            //                       CheckoutDate = transact.CheckoutDate,
            //                       CheckInDate = transact.CheckInDate,
            //                       Isbn = book.Isbn,
            //                       AssetNum = book.AssetNum,
            //                       IsRequired = book.IsRequired.ToString()

            //                   }).ToList();
            //}
           

            if (!string.IsNullOrEmpty(searchValue))
            {
                DateTime? date;
                if (searchValue.IsDateTime())
                {
                    date = DateTime.Parse(searchValue);
                }
                else
                {
                    date = null;
                }
                transaction = (from book in Database.Session.Query<Book>()
                               join transact in Database.Session.Query<Transaction>()
                               on book.AssetNum equals transact.BookAssetNumber
                               join borrower in Database.Session.Query<User>()
                               on transact.UserId equals borrower.Id
                               where borrower.FirstName.Contains(searchValue)
                               || borrower.LastName.Contains(searchValue)
                               || borrower.Euid.Contains(searchValue)
                               || borrower.Email.Contains(searchValue)
                               || book.Title.Contains(searchValue)
                               || book.Author.Contains(searchValue)
                               || book.Isbn.Contains(searchValue)
                               || transact.CheckoutDate == date
                               || transact.CheckInDate == date
                               || transact.DueDate == date
                               orderby searchValue
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
                                   DueDate = transact.DueDate,
                                   CheckoutDate = transact.CheckoutDate,
                                   CheckInDate = transact.CheckInDate,
                                   Isbn = book.Isbn,
                                   AssetNum = book.AssetNum,
                                   IsRequired = book.IsRequired.ToString()

                               }).ToList();
            }
            //create temdata to pass to the Export method

            TempData["list"] = transaction;
            var transactList = new TransactionReportList()
            {
                TransactionReports = transaction
            };

            return View(transactList);
        }
        //create method to export report to excel file
        public void Export()
        {

            var grid = new GridView();
            grid.DataSource = TempData["list"];
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename = ExportedTransactioReport.xlsx");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);
            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.End();
        }




        // GET: Admin/Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Report/Create
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

        // GET: Admin/Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Report/Delete/5
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
