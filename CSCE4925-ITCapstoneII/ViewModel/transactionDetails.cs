using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQLSolutions.ViewModel
{
    public class TransactionDetails
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

        public DateTime ? CheckInDate { get; set; }

    }

    public class TransactionDetailsList
    {
        //make IEnumerable to display list of info
        public IEnumerable<TransactionDetails> UserBooks { get; set; }
     
    }
}