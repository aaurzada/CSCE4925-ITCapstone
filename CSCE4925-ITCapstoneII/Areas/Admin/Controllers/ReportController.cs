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
    public class ReportController : Controller
    {
        // GET: Admin/Report
        [SelectedTab("Reports")]
        public ActionResult IndexBookReport(bool avail)
        {
            var bookList = new ReportBookList() { Books = Database.Session.Query<Book>().ToList() };
            //ViewBag.isAvail = Database.Session.Query<Book>().Where(b => )
            //{
            //    bookList = new ReportBookList()
            //    {
            //        Books = Database.Session.Query<Book>().ToList()
                        
            //    };
            //}
            
            return View(bookList);
        }

        // GET: Admin/Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
