using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using SQLSolutions.ViewModel;

namespace SQLSolutions.Controllers
{
   
    [AllowAnonymous]
    public class nonAdminController : Controller
    {
        // GET: Admin/Book
        public ActionResult Index()
        {
            int id = 0;

            if (TempData["userId"] != null)
            {
                id = (int)TempData["userId"];
            }
            var userBook = (from book in Database.Session.Query<Book>()
                           join transaction in Database.Session.Query<Transaction>() on book.AssetNum equals transaction.BookAssetNumber
                           where transaction.UserId == id select new transactionDetails
                           {
                                Title = book.Title,
                                Author = book.Author,
                                CourseSection = book.CourseSection,
                                DueDate = transaction.DueDate,
                                CheckoutDate = transaction.CheckoutDate
                           }).ToList();
            var transResult = new transactionDetailsList
            {
               UserBooks = userBook
            };
            return View(transResult);    
        }
    }
}