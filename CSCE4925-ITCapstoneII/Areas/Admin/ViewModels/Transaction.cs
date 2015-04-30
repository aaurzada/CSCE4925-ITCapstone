using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PagedList;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class TransactionIndex
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }

    //defone  transaction property 
    public class Transactions
    {

        public int Id { get; set; }

        public int AssetNum { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string CourseSection { get; set; }

        public string Euid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        
        public int UserId { get; set; }

        public int BookAssetNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CheckoutDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DueDate { get; set; }

        public DateTime? CheckInDate { get; set; }
    }

    //list all transactions
    public class TransactioList
    {
        public IPagedList<Transactions> TrasactList { get; set; }
    }


    public class TransactionEdit
    {
        public int Id { get; set; }

        [Required]
        // [RegularExpression(@"^(?!0{4})[0-9\s]{4}$", ErrorMessage = "Please enter valid Asset Number")]
        [DisplayName("Asset Number")]
        public int AssetNum { get; set; }

        [Required]
        [RegularExpression(@"^[0-9\s]{13}$", ErrorMessage = "Please enter valid ISBN13 Number")]
        public string Isbn { get; set; }

        [Required, MaxLength(128)]
        public string Title { get; set; }

        [Required]
        //restrict to only upper and lower characters, 80 chars allows
        [RegularExpression(@"^[a-zA-Z''\s_.-]{1,100}", ErrorMessage = "Please use letters only")]
        public string Author { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,15}", ErrorMessage = "Please enter valid Course Number")]
        public string CourseSection { get; set; }

        [DisplayName("Euid:")]
        [Required]
        [RegularExpression(@"^[a-z0-9''\s]{6,9}$", ErrorMessage = "EUID should be no longer than 9 characters (letters and numbers only)")]
        public string Euid { get; set; }
        [DisplayName("First Name:")]
        [Required]
        //restrict to only upper and lower characters
        [RegularExpression(@"^[a-zA-Z''\s]{1,40}", ErrorMessage = "Please use letters only")]
        public string FirstName { get; set; }
        [DisplayName("Last Name:")]
        [Required]
        //restrict to only upper and lower characters
        [RegularExpression(@"^[a-zA-Z''\s]{1,40}", ErrorMessage = "Please use letters only")]
        public string LastName { get; set; }

        public int UserId { get; set; }

        public int BookAssetNumber { get; set; }

        
        public string CheckoutDate { get; set; }
        //date book is due
        
        public string DueDate { get; set; }

        public DateTime? CheckInDate { get; set; }
    }

    //create an object to POST
    public class TransactionPost
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookAssetNumber { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CheckoutDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DueDate { get; set; }

        public DateTime? CheckInDate { get; set; }
    }
}