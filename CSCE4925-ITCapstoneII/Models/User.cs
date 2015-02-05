using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLSolutions.Models
{
    public class User
    {
        //user idNum {primary key}
        public virtual int idNum { get; set; }
        //user euid for log in purposes
        public virtual String euid { get; set; }
        //first name
        public virtual String firstName { get; set; }
        //last name
        public virtual String lastName { get; set; }
        //email
        public virtual String email { get; set; }
    }
}