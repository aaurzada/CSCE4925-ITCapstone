using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace SQLSolutions.Models
{
    public class Book
    {
        //asset number which uniquely identifies books with same isbn num
        [Required]
        public virtual int assetNum { get; set; }
        //isbn number which uniquely defines a book 
        [Required]
        public virtual string isbn { get; set; }
        [Required]
        public virtual int courseSection { get; set; }
        [Required]
        public virtual int year { get; set; }
        [Required]
        public virtual int edition { get; set; }
        [Required]
        public virtual int isRequired { get; set; }
        //add author
        //add title
        //
    }
}