using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FluentMigrator.Infrastructure;
using SQLSolutions.Models;


namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class UserIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UserNew
    {
        
        [HiddenInput(DisplayValue = false)]
        //only 8 numbers are allowed, no less, no more
        //[RegularExpression(@"^[0-9]{8}$", ErrorMessage = "ID should contain 8 numbers")]
        public int Id { get; set; }

        [Required]
        [StringLength(6)]
        [RegularExpression(@"^[a-z0-9'''-'\s]{6}$", ErrorMessage = "EUID should be no longer than 6 characters (letters and numbers only)")]
        public string Euid { get; set; }

        [Required]
        //make sure that only upper and lower characters are allowed
        [RegularExpression(@"^[a-zA-Z'''-'\s]{1,40}", ErrorMessage = "Please use letters only")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z'''-'\s]{1,40}", ErrorMessage = "Please use letters only")]
        public string LastName { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }

    public class UserEdit
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Euid { get; set; }

        [Required, MaxLength(128)]
        public string FirstName { get; set; }

        [Required, MaxLength(128)]
        public string LastName { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }


    public class UserDetailsList
    {
        public IEnumerable<UserDetails> UserBooks { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public class UserDetails
    {
       
        public string Title { get; set; }
        public string Author { get; set; }
        public int CourseSection { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat (DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime CheckoutDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MMMM.yyyy}")]
        public DateTime DueDate { get; set; }
    }
}