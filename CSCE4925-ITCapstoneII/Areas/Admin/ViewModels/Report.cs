using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class ReportBook
    {
        public  int AssetNum { get; set; }
        public  string Isbn { get; set; }
        public  string Title { get; set; }
        public  string Author { get; set; }
        public  string CourseSection { get; set; }
        public  int? Year { get; set; }
        public  int? Edition { get; set; }
        public  string IsRequired { get; set; }
        public  string  InStock { get; set; }
    }

    public class ReportBookList
    {
        public IPagedList <ReportBook> Books { get; set; }
    }

   
    public class TransactionReport
    {
        public string Euid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Isbn { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CourseSection { get; set; }
        public int? Year { get; set; }
        public int? Edition { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string CheckoutDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string DueDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime ? CheckInDate { get; set; }
        public int AssetNum { get; set; }
        public string IsRequired { get; set; }

    }

    public class TransactionReportList
    {
        public IPagedList <TransactionReport> TransactionReports { get; set; }
    }
    public class TransactionReportList2 
    {
        public IEnumerable<TransactionReport> TransactionReports2 { get; set; }
    }
}