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
        public virtual int idNum { get; set; }
        public virtual string isbn { get; set; }
        public virtual int copyNum { get; set; }
        public virtual int courseSection { get; set; }
        public virtual int year { get; set; }
        public virtual int edition { get; set; }
        public virtual int isRequired { get; set; }
        //add author
        //add title
        //
    }
}