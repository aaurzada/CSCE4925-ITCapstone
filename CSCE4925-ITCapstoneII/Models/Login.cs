using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SQLSolutions.Models
{
    public class Login
    {
        //user euid for log in purposes...must be a required field
        [Required(ErrorMessage = "Please enter your username (euid)", AllowEmptyStrings = false)]
        [Display(Name = "Username:")]
        public virtual String euid { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password", AllowEmptyStrings = false)]
        [Display(Name = "Password:")]
        public virtual String password { get; set; } //may delete...dont actually need to store

        public String IsValid(string _username, string _password)
        {
            bool accountExists = false; //account exists in user table
            bool isAdmin = false; //account is admin in user table
            //check if euid is valid in user table
            if (Database.Session.Query<User>().Any(u => u.euid == _username))
            { 
                //check if user is admin in user table
                isAdmin = true;
                accountExists = true;
               
            }
            //---check if euid and password valid in UNT auth---
            if (accountExists == true && isAdmin == true) //if account exists then check password validity through UNT auth
            {
                //send _username and _password 
                //return bool depending if username and password match
                //if return true then return "admin"
                return "admin";
            }
            else if (accountExists == true && isAdmin == false)
            { 
                //send _username and _password to UNT auth
                //return bool depending if username and password match
                //if return true then return "nonAdmin"
                return "nonAdmin";
            }
            else
                return "notExists"; //just for now
        }
    }
}