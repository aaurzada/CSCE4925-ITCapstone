using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SQLSolutions.Models;

namespace SQLSolutions.Areas.Admin.ViewModels
{
    public class BookIndex
    {
        public IEnumerable<Book> Books { get; set; }
    }

    // ? - Make integers nullable, to remove 0 from diplaying in the testbox
    public class BookNew
    {
        

        [Required]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Please enter valid Asset Number")]
        public int AssetNum { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "Please enter valid ISBN13 Number")]
        public string Isbn { get; set; }
        [Required, MaxLength(128)]
        public string Title { get; set; }
        [Required]
        //restrict to only upper and lower characters, only 80 chars allowed
        [RegularExpression(@"^[a-zA-Z''\s]{1,80}", ErrorMessage = "Please enter valid First and Last name")]
        public string Author { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9'''-'\s]{1,80}", ErrorMessage = "Please enter valid Course Number")]
        public  string CourseSection { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Please enter valid year")]
        public int ? Year { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{1,4}$", ErrorMessage = "Please enter valid edition")]
        public int ? Edition { get; set; }
        [Required]
        public bool IsRequired { get; set; }
    }

    public class BookEdit
    {
        [Required]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Please enter valid Asset Number")]
        public int AssetNum { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{13}$", ErrorMessage = "Please enter valid ISBN13 Number")]
        public string Isbn { get; set; }

        [Required, MaxLength(128)]
        public string Title { get; set; }

        [Required]
        //restrict to only upper and lower characters, 80 chars allows
        [RegularExpression(@"^[a-zA-Z''\s]{1,80}", ErrorMessage = "Please use letters only")]
        public string Author { get; set; }

        [Required]
        public string CourseSection { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Please enter valid year. Example: 2015")]
        public int ? Year { get; set; }

        [Required]
        //should input minimum 1 number, max 4 numbers
        [RegularExpression(@"^[0-9]{1,4}$", ErrorMessage = "Please enter valid edition")]
        public int ? Edition { get; set; }

        [Required]
        public bool IsRequired { get; set; }
    }
}