using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class TransactionsIndex
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}