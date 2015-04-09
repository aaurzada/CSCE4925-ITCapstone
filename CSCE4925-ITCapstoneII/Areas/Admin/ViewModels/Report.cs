﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class ReportBookList
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<string> Title { get; set; }
    }



   
}