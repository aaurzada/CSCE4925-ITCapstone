using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using SQLSolutions.Models;

namespace SQLSolutions.ViewModel
{
    public class UserBookIndex
    {
        public IPagedList<Book> UserBooks { get; set; }
    }
}