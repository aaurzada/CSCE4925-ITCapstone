using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQLSolutions.ViewModel
{
    public class transactionDetails
    {
        //info to be used by controller and passed to view
        //title of book
        public string Title { get; set; }
        //book author
        public string Author { get; set; }
        //book course section 
        public string CourseSection { get; set; }
        //the date the book was checked out
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:MMMM dd, yyyy}")]
        public  DateTime CheckoutDate { get; set; }
        //date book is due
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime DueDate { get; set; }
    }

    public class transactionDetailsList
    {
        //make IEnumerable to display list of info
        public IEnumerable<transactionDetails> UserBooks { get; set; }
     
    }
}