using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NHibernate.Linq;
using ConnectLDAP;
using System.Security;

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
        [Display(Name = "Password:")]
        public virtual string password { get; set; } //may delete...dont actually need to store

        public String IsValid(string _username, string _password)
        {
            bool accountExists = false; //bool if account exists in user table initialized to false
            bool isAdmin = true; //bool if account is admin in user table initialized to false
            //check if euid is valid in user table
       
            var queryEuid = Database.Session.Query<User>().Where(u => u.Euid.Equals(_username)).Select(u => u.Euid).SingleOrDefault<String>();
            //--- ACCOUNT EXISTS IN OUR User TABLE---
            if (queryEuid != null) //if user exists in user table then check euid and password with ldap
            { 
                var connect = new LDAPConnect();

                if (connect.useLDAP("uid=" + _username + ",ou=people,o=unt", _password))
                {
                accountExists = true; //account exists in User table
                }
                else
                    accountExists = false;

               
                isAdmin = false; //will only be set to true if isAdmin = 1 in user table              
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