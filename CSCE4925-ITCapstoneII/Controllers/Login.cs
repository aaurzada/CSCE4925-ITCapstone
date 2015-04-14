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
        [Display(Name = "Password:")]
        public virtual string password { get; set; } //may delete...dont actually need to store

        public String IsValid(string _username, string _password)
        {

            bool accountExists = false; //bool if account exists in user table initialized to false
            bool isAdmin = false; //bool if account is admin in user table initialized to false
            //check if euid is valid in user table
       
            var queryEuid = Database.Session.Query<User>().Where(u => u.Euid.Equals(_username)).Select(u => u.Euid).SingleOrDefault<String>();
            //--- ACCOUNT EXISTS IN OUR User TABLE---
            if (queryEuid != null) //if user exists in user table then check euid and password with ldap
            { 
                var connect = new LDAPConnect();
                //CONNECT TO LDAP 
        //        if (connect.useLDAP("uid=" + _username + ",ou=people,o=unt", _password))
          //      {
                        int id = 0;
                        string strIsAdmin = "0";
                
                        var queryId = Database.Session.Query<User>().Where(u => u.Euid.Equals(_username)).Select(u => u.Id); //get users id number
                        id = queryId.SingleOrDefault();
                               
                        string connectionString = "Server=localhost;Database=sqlsolutions;UID=sqlsolutions;Password=Password01!";

                        MySqlConnection connection = new MySqlConnection(connectionString); //create connection with connectionString
                        connection.Open(); //open sql connection
                        string query = "SELECT role_id FROM roles_user WHERE user_id = " + id;
                        using (MySqlCommand command = new MySqlCommand(query, connection)) //run query command in mySQL
                        {
                            MySqlDataReader reader = command.ExecuteReader(); //reader reads data from sql query
                            while (reader.Read()) //loops through all records query 
                            {
                                strIsAdmin = reader.GetString("role_id");  // get userId and store as string
                            }
                        }
                        if (strIsAdmin == "1")
                            isAdmin = true;
                        else
                            isAdmin = false;
                        accountExists = true; //account exists in User table                         
            }
            if (accountExists == true && isAdmin == true) //if account exists then return admin, nonadmin, or nonexists to homeController
            {
                //if return true then return "admin"
                return "admin";
            }
            else if (accountExists == true && isAdmin == false)
            { 
                //if return true then return "nonAdmin"
                return "nonAdmin";
            }
            else
                return "notExists"; 
        }
    }
}