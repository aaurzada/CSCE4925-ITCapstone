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
                
      
           // var bookList = new TransIndex { Transactions = Database.Session.Query<Transaction>().ToList() };
            try
            {
               
               // IQueryOver<Transaction, Book> TransQuery = Database.Session.QueryOver<Transaction>().JoinQueryOver(b => b.BookAssetNumber);
                //searh book by title, ISBN, AssetNumber, course section, author
                var bookList = new TransIndex {Transactions = Database.Session.Query<Transaction>().Where(b => (b.UserId.Equals(id))).ToList()};// && b.DueDate != null);
             
                return View(bookList);
            }
            catch
            {
                return View();
            }

            
        }
    }
}