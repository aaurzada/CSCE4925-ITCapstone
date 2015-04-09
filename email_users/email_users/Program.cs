using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            //connection string that connects to database  
            string connectionString = "Server=localhost;Database=sqlsolutions;UID=sqlsolutions;Password=Password01!";

            try
            {
                MySqlConnection connection = new MySqlConnection(connectionString); //create connection with connectionString
                connection.Open(); //open sql connection
                string query = "SELECT * FROM transaction JOIN user ON (transaction.user_id = user.id)  JOIN book ON (transaction.book_assetNum = book.assetNum) WHERE (transaction.dueDate < CURDATE() AND checkInDate IS NULL)";
                using (MySqlCommand command = new MySqlCommand(query, connection)) //run query command in mySQL
                {
                    MySqlDataReader reader = command.ExecuteReader(); //reader reads data from sql query
                    while (reader.Read()) //loops through all records query 
                    {
                        string userId = reader.GetString("user_id");  // get userId and store as string
                        string firstName = reader.GetString("firstName"); //get user's first name and store as string
                        string lastName = reader.GetString("lastName"); // get user's last name 
                        string bookName = reader.GetString("title"); // get title of book that is past due
                        string email = reader.GetString("email"); // get email of user
                        string checkedOut = reader.GetString("checkoutDate"); //get check out transaction date of book
                        string dueDate = reader.GetString("dueDate"); //get due date of book from transaction table
                    
                        //Console.WriteLine("id = " + userId + "euid = " + firstName+ "email = " + email);
                        send_email(email, firstName, lastName, bookName, checkedOut, dueDate);
                    }
                }

                connection.Close(); //close database sql connection
            }
            catch //if connection to database failed
            {
                Console.WriteLine("FAILED"); //display failed
            }
          
        }

        public static void send_email(string user_email, string users_name, string users_lastName, string book_name, string checked_out, string due_date)
        {
            var fromAddress = new MailAddress("untcscelib@gmail.com", "UNT CSCE Library"); //name of email sending emails to users 
            var toAddress = new MailAddress(user_email, users_name); //send to queried user email and name
            const string fromPassword = "curlyplanet723"; //password of untcselib@gmail.com email
            string subject = book_name + " is past due"; //Title of email
            //Message body of email sent to users
            string body = "Dear, " + users_name + " " + users_lastName + Environment.NewLine + Environment.NewLine + "NOTICE: You checked out the book '" + book_name + "' from the CSCE Library on " + checked_out + 
                ". It was due back by " + due_date + ". Please return '" + book_name + "' to F220 at your earliest convenience.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", //connecting to gmail smtp server
                Port = 587, //using port 587
                EnableSsl = true, //security requirement to connect to gmail
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false, //require email credentials
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword) //pass email and password 
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            smtp.Send(message); //send message to user        
        }
       
    }

}
