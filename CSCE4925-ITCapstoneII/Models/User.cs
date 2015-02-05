using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQLSolutions.Models
{//test
    public class User
    {
        //user idNum {primary key}
        [Required]
        public virtual int idNum { get; set; }
        //user euid for log in purposes
        [Required]
        public virtual String euid { get; set; }
        //first name
        [Required]
        public virtual String firstName { get; set; }
        //last name
        [Required]
        public virtual String lastName { get; set; }
        //email
        [Required]
        public virtual String email { get; set; }
    }
}