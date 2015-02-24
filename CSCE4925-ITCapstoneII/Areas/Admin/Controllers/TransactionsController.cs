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
    [SelectedTab("transactions")]
    public class TransactionsController : Controller
    {
        public ActionResult Index()
        {
            //list all transactions in database
            return View(new TransactionsIndex
            {
                Transactions = Database.Session.Query<Transaction>().ToList()
            });
        }
    }
}