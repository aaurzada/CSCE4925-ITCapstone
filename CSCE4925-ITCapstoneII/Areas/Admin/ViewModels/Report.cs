using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class ReportBookList
    {
        public IEnumerable<Book> Books { get; set; }
    }

    public class TransactionReport
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Euid { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CourseSection { get; set; }
        public int? Year { get; set; }
        public int? Edition { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime CheckoutDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime DueDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime CheckInDate { get; set; }
        public string Isbn { get; set; }
        public int AssetNum { get; set; }
        public bool IsRequired { get; set; }

    }

    public class TransactionReportList
    {
        public IEnumerable<TransactionReport> TransactionReports { get; set; }
    }

}