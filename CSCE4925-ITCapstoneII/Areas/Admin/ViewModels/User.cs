using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FluentMigrator.Infrastructure;
using SQLSolutions.Models;
using System.ComponentModel;


namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class UserIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UserNew
    {
        
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [DisplayName("Euid:")]
        [Required]
        [RegularExpression(@"^[a-z0-9''\s]{8}$", ErrorMessage = "EUID should be no longer than 6 characters (letters and numbers only)")]
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
        [DisplayName("Email:")]
        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Role:")]
        [Required]
        public bool isAdmin { get; set; }
    }

    public class UserEdit
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [DisplayName("Euid:")]
        [Required]
        [RegularExpression(@"^[a-z0-9''\s]{8}$", ErrorMessage = "EUID should be no longer than 6 characters (letters and numbers only)")]
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
        [DisplayName("Email:")]
        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Role:")]
        [Required]
        public bool isAdmin { get; set; }
    }

    //display user's first and last name and list of books borrowed by the user
    public class UserDetailsList
    {
        public IEnumerable<UserDetails> UserBooks { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    //fields for the book details and due dates
    public class UserDetails
    {
       
        public string Title { get; set; }
        public string Author { get; set; }
        public string CourseSection { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat (DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime CheckoutDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime DueDate { get; set; }
    }
}