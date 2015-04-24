using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using SQLSolutions.Models;

namespace SQLSolutions.ViewModel
{
    //ViewModel for the search page and display paged book list
    public class UserBookIndex
    {
        public IPagedList<Book> UserBooks { get; set; }
    }
}