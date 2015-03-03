using System.Collections.Generic;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class TransactionIndex
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}