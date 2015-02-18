using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NHibernate.Linq;

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
            bool accountExists = true; //bool if account exists in user table initialized to false
            bool isAdmin = true; //bool if account is admin in user table initialized to false
            //check if euid is valid in user table
       
            var queryEuid = Database.Session.Query<User>().Where(u => u.Euid.Equals(_username)).Select(u => u.Euid).SingleOrDefault<String>();
            //--- ACCOUNT EXISTS IN OUR User TABLE---
            if (queryEuid != null)
            { 
                //check if queryEuid is admin in User table 
                isAdmin = true;

                accountExists = true; //account exists in User table
               
            }
            //---CHECK EUID AND PASSWORD VALID IN UNT AUTH---
            if (accountExists == true && isAdmin == true) //if account exists then check password validity through UNT auth
            {
                //send _username and _password to UNT auth
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
                return "notExists"; 
        }
    }
}