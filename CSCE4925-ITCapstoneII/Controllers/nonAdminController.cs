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
            
            
           // var bookList = new TransIndex { Transactions = Database.Session.Query<Transaction>().ToList() };
            try
            {
                //searh book by title, ISBN, AssetNumber, course section, author
                var bookList = Database.Session.Query<Transaction>().Where(b => (b.UserEuid == ((string)Session["user"]))&&( b.DueDate != null)).ToList();// && b.DueDate != null);
                return View(bookList);
            }
            catch
            {
                return View();
            }

            
        }
    }
}