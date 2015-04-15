using SQLSolutions.Infrastructure;
using SQLSolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLSolutions.Infrastructure;
using System;
using System.Collections.Generic;

using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using MySql.Data.MySqlClient;
using ConnectLDAP;
namespace SQLSolutions.Controllers
{
    public class HomeController:Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            Session.Abandon(); //destroy all session variables
            return View();
        }
     
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(Login user)
        { 
            if (ModelState.IsValid)
            {
                if (IsValid(user.euid, user.password) == "admin") //given by view and sent to model IsValid function to check against user table and UNT auth
                {
                    Session["username"] = user.euid; //store euid as session variable
                    Session["isAdmin"] = "true"; //store is admin as session id. Check at each page 
                    //check if admin and display pages accordingly
                    return RedirectToAction("Index", "User", new { area = "admin" });//admin page displayed
                }
                else if (IsValid(user.euid, user.password) == "nonAdmin")
                {
                    Session["username"] = user.euid; //store euid as session variable
                    Session["isAdmin"] = "false"; //store is admin as session id. Check at each page
                    return RedirectToAction("Index", "nonAdmin");
                }
                else if (IsValid(user.euid, user.password) == "notExists") //if username does not exist in user table then display does not exist
                {
                    ModelState.AddModelError("", "This account does not exist.");
                }
                else
                {
                    ModelState.AddModelError("", "The password entered is incorrect."); //if it exists then password is incorrect
                }
            }
            return View(user);
        }
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
                //}        
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
    
