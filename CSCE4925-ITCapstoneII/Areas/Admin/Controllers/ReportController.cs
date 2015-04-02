using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Admin/Report
        [SelectedTab("Reports")]
        public ActionResult IndexBookReport(string Selected)
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
            if (Selected == "0")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().ToList()
                };
            }
            //check what user has selected, if option Available with value = "1" then display list of all available books
            if (Selected == "1")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().Where(b => b.InStock == true).ToList()
                };
            }
            //check what user has selected, if option Available with value = "2" then display list of all unavailable books
            if (Selected == "2")
            {
                bookList = new ReportBookList()
                {
                    Books = Database.Session.Query<Book>().Where(b => b.InStock == false).ToList()
                };
            }
            
            return View(bookList);
        }

     

        // GET: Admin/Report/Details/5
        public ActionResult Details()
        {
            var transaction = (from book in Database.Session.Query<Book>()
                            join transact in Database.Session.Query<Transaction>()
                            on book.AssetNum equals transact.BookAssetNumber
                            join borrower in Database.Session.Query<User>() 
                            on transact.UserId equals borrower.Id
                            select new TransactionReport
                            {
                                FirstName = borrower.FirstName,
                                LastName = borrower.LastName,
                                Euid = borrower.Euid,
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
                                IsRequired = book.IsRequired

                            }).ToList();
            var transactList = new TransactionReportList()
            {
                TransactionReports = transaction.ToList()
            };

            var grid = new GridView();
            grid.DataSource = transactList;
            
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename = ExportedTransactioReport.xlsx");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(sw);

            grid.RenderControl(htmlTextWriter);
            Response.Write(sw.ToString());
            Response.End();
            return View(transactList);
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
