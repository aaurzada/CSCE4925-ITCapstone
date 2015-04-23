using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using ConnectLDAP;
using System.Security;
using System.IO;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace SQLSolutions.Models
{

    public class Login 
    {

        //user euid for log in purposes...must be a required field
        [Required(ErrorMessage = "Please enter your username (euid)", AllowEmptyStrings = false)]
        [Display(Name = "Username")]
        public virtual String euid { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        public virtual string password { get; set; } //may delete...dont actually need to store

    }
}