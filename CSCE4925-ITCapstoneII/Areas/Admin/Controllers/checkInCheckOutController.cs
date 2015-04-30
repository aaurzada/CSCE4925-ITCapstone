using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLSolutions.Models;
using NHibernate.Linq;
using SQLSolutions.Areas.Admin.ViewModels;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Web.WebPages;
using Mono.Security.X509;
using NHibernate.Mapping.ByCode.Impl;
using PagedList;
using SQLSolutions.Infrastructure;

namespace SQLSolutions.Areas.Admin.Controllers
{
    public class checkInCheckOutController : Controller
    {
        // GET: Admin/checkInCheckOut
        [SelectedTab("Checkout/Check In")]
        public ActionResult Index()
        {
            return View(new Transaction());
        }


        [HttpPost]
        [SelectedTab("Checkout/Check In")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Transaction user)
        {
            if (Database.Session.Query<Book>().Any(b => b.AssetNum == user.BookAssetNumber))
            {
                //book exists in book table
                //query transaction table
                Session["bookAssetNum"] = user.BookAssetNumber;


                if (Database.Session.Query<Book>().Any(b => b.AssetNum == user.BookAssetNumber && b.InStock == false))//if book exists and is not in stock
                {
                    var bookCheckIn = Database.Session.Get<Book>(user.BookAssetNumber);
                    if (bookCheckIn == null)
                    {
                        return HttpNotFound();
                    }
                    Session["book_state"] = "checkIn";
                    return View("checkInBook", new Book
                    {
                        AssetNum = bookCheckIn.AssetNum,
                        Isbn = bookCheckIn.Isbn,
                        Title = bookCheckIn.Title,
                        Author = bookCheckIn.Author,
                        CourseSection = bookCheckIn.CourseSection,
                        Year = bookCheckIn.Year,
                        Edition = bookCheckIn.Edition,
                        IsRequired = bookCheckIn.IsRequired,
                        InStock = bookCheckIn.InStock
                    });


                    //return View("checkInBook"); //if book exists and has not yet been checked in then load check in form
                }
                else
                {
                    var bookCheckOut = Database.Session.Get<Book>(user.BookAssetNumber);
                    if (bookCheckOut == null)
                    {
                        return HttpNotFound();
                    }
                    Session["book_state"] = "checkOut";
                    return View("checkOut", new Book
                    {
                        AssetNum = bookCheckOut.AssetNum,
                        Isbn = bookCheckOut.Isbn,
                        Title = bookCheckOut.Title,
                        Author = bookCheckOut.Author,
                        CourseSection = bookCheckOut.CourseSection,
                        Year = bookCheckOut.Year,
                        Edition = bookCheckOut.Edition,
                        IsRequired = bookCheckOut.IsRequired,
                        InStock = bookCheckOut.InStock
                    });

                }


            }

            string date = "01/01/0001"; //sometimes database is defaulting null value to this
            DateTime defaultDate = Convert.ToDateTime(date);

            //check asset number typed in
            //if asset number is in transaction table and book is currently checked out display check in form
            //if asset number is in transaction table and book is not currently checked out display check out form
            //else if asset number exists in book table diesplay check out form
            //display book does not exist
            if (Session["book_state"] == "checkIn")
            {
                DateTime today = new DateTime();
                today = DateTime.Today;
                MySqlConnection Connection = new MySqlConnection("Server=localhost;Database=sqlsolutions;User Id=sqlsolutions;Password=Password01!");

                using (MySqlCommand command = new MySqlCommand("UPDATE transaction SET checkInDate = @today WHERE book_assetNum = @assetNum AND checkInDate = @checkIn OR checkInDate IS NULL", Connection))
                {
                    command.Parameters.AddWithValue("@today", today.Date); //todays date
                    command.Parameters.AddWithValue("@assetNum", Session["bookAssetNum"]); //asset Number sent by form
                    command.Parameters.AddWithValue("@checkIn", defaultDate);
                    Connection.Open();
                    command.ExecuteNonQuery();
                    Connection.Close();
                }
                using (MySqlCommand command2 = new MySqlCommand("UPDATE book SET inStock = true WHERE assetNum = @asset_num", Connection))
                {
                    command2.Parameters.AddWithValue("@asset_num", Session["bookAssetNum"]);
                    Connection.Open();
                    command2.ExecuteNonQuery();
                    Connection.Close();
                }
                Session["book_state"] = "";
                Response.Write("<script>alert('Book has successfully been checked in.');</script>");
            }
            else if (Session["book_state"] == "checkOut")
            {
                //get information back from form
                string euid = Request["euid"].ToString();
                if (euid == "")
                {
                    var bookCheckOut = Database.Session.Get<Book>((int)Session["bookAssetNum"]);
                    ModelState.AddModelError("", "Error: Book was not checked out. You did not enter a euid.");
                    return View("checkOut", new Book
                    {
                        AssetNum = bookCheckOut.AssetNum,
                        Isbn = bookCheckOut.Isbn,
                        Title = bookCheckOut.Title,
                        Author = bookCheckOut.Author,
                        CourseSection = bookCheckOut.CourseSection,
                        Year = bookCheckOut.Year,
                        Edition = bookCheckOut.Edition,
                        IsRequired = bookCheckOut.IsRequired,
                        InStock = bookCheckOut.InStock
                    });
                }


                var checkOutDate = Request["checkOut"];
                DateTime checkOut = Convert.ToDateTime(checkOutDate);
                var dueDate = Request["dueDate"];
                {
                    if (dueDate == "") //check form to assure due date field is filled out by user
                    {
                        var bookCheckOut = Database.Session.Get<Book>((int)Session["bookAssetNum"]);
                        ModelState.AddModelError("", "Error: Book was not checked out. Due date was not entered. ");
                        return View("checkOut", new Book
                        {
                            AssetNum = bookCheckOut.AssetNum,
                            Isbn = bookCheckOut.Isbn,
                            Title = bookCheckOut.Title,
                            Author = bookCheckOut.Author,
                            CourseSection = bookCheckOut.CourseSection,
                            Year = bookCheckOut.Year,
                            Edition = bookCheckOut.Edition,
                            IsRequired = bookCheckOut.IsRequired,
                            InStock = bookCheckOut.InStock
                        });

                    }
                }
                DateTime dueBack = Convert.ToDateTime(dueDate);
                if (dueBack < DateTime.Now.Date) //if user has selected a due date that has already passed reload form and throw error
                {
                    var bookCheckOut = Database.Session.Get<Book>((int)Session["bookAssetNum"]);
                    ModelState.AddModelError("", "Error: Book was not checked out. Due date can not have already passed. ");
                    return View("checkOut", new Book
                    {
                        AssetNum = bookCheckOut.AssetNum,
                        Isbn = bookCheckOut.Isbn,
                        Title = bookCheckOut.Title,
                        Author = bookCheckOut.Author,
                        CourseSection = bookCheckOut.CourseSection,
                        Year = bookCheckOut.Year,
                        Edition = bookCheckOut.Edition,
                        IsRequired = bookCheckOut.IsRequired,
                        InStock = bookCheckOut.InStock
                    });
                }

                MySqlConnection Connection = new MySqlConnection("Server=localhost;Database=sqlsolutions;User Id=sqlsolutions;Password=Password01!");

                //with connection verify euid
                int userId = 0;
                if (euid == "")
                {
                    var bookCheckOut = Database.Session.Get<Book>((int)Session["bookAssetNum"]);
                    ModelState.AddModelError("", "Error: Book was not checked out. The euid entered was not valid.");
                    return View("checkOut", new Book
                    {
                        AssetNum = bookCheckOut.AssetNum,
                        Isbn = bookCheckOut.Isbn,
                        Title = bookCheckOut.Title,
                        Author = bookCheckOut.Author,
                        CourseSection = bookCheckOut.CourseSection,
                        Year = bookCheckOut.Year,
                        Edition = bookCheckOut.Edition,
                        IsRequired = bookCheckOut.IsRequired,
                        InStock = bookCheckOut.InStock
                    });
                }
                else
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT id FROM user WHERE euid = @userId", Connection))
                    {


                        command.Parameters.AddWithValue("@userId", euid);
                        Connection.Open();
                        MySqlDataReader myReader = command.ExecuteReader();
                        while (myReader.Read())
                        {
                            var id = myReader["id"];
                            userId = (int)id;
                        }
                        Connection.Close();
                        if (userId == 0)
                        {
                            var bookCheckOut = Database.Session.Get<Book>((int)Session["bookAssetNum"]);
                            ModelState.AddModelError("", "Error: Book was not checked out. The euid entered was not valid.");
                            return View("checkOut", new Book
                            {
                                AssetNum = bookCheckOut.AssetNum,
                                Isbn = bookCheckOut.Isbn,
                                Title = bookCheckOut.Title,
                                Author = bookCheckOut.Author,
                                CourseSection = bookCheckOut.CourseSection,
                                Year = bookCheckOut.Year,
                                Edition = bookCheckOut.Edition,
                                IsRequired = bookCheckOut.IsRequired,
                                InStock = bookCheckOut.InStock
                            });
                        }
                    }
                    MySqlConnection Connection2 = new MySqlConnection("Server=localhost;Database=sqlsolutions;User Id=sqlsolutions;Password=Password01!");
                    if (userId != 0)
                    {
                        Connection2.Open();
                        ////get all info for book with asset number
                        //string isbnNum = null;
                        int bookAssetNum = (int)Session["bookAssetNum"];

                        //user exists. Run check out queries, create new transaction, set book inStock to false
                        using (MySqlCommand command3 = new MySqlCommand("INSERT INTO transaction (user_id, book_assetNum, checkoutDate, dueDate)VALUES(@user_id, @book_assetNum, @checkoutDate, @dueDate)", Connection2))
                        {

                            command3.Parameters.AddWithValue("@user_id", userId); //users id
                            command3.Parameters.AddWithValue("@book_assetNum", bookAssetNum);
                            command3.Parameters.AddWithValue("@checkoutDate", checkOut);
                            command3.Parameters.AddWithValue("@dueDate", dueBack);
                            command3.ExecuteNonQuery();
                            Connection2.Close();
                        }
                        using (MySqlCommand command4 = new MySqlCommand("UPDATE book SET inStock = false WHERE assetNum = @asset_num", Connection2))
                        {
                            command4.Parameters.AddWithValue("@asset_num", Session["bookAssetNum"]);
                            Connection2.Open();
                            command4.ExecuteNonQuery();
                            Connection2.Close();
                        }

                    }
                    //else
                    //{
                    //    //user does not exist..return form
                    //}
                }
                Session["book_state"] = "";

                Response.Write("<script>alert('Book has successfully been checked out.');</script>");
            }
            //if euid exists then get id
            //make new transaction with euid, assetNum, checkOutDate, and dueDate
            //mark book as isAvailable = false;

            return RedirectToAction("Index");

        }
        //string sql = "UPDATE transaction SET checkInDate = @today WHERE book_assetNum = @assetNum AND checkInDate = @checkIn";
        //MySqlCommand command = new MySqlCommand(sql, Connection); //run sql query to update transaction table where return date for asset number is null

        //Connection.Close();
        //find book assetNum ..if inStock == 0 set to 1
        //locate assetNum in transaction table where checkInDate is null or = 0001-01-01 and set checkInDate to today


        //list current transactions where checkindate is null
        [SelectedTab("Transactions")]
        public ActionResult List(string currentSelect, int? page,string currentFilter, string searchT = null)
        {
            
            if (searchT != null)
            {
                page = 1;
            }
            else
            {
                searchT = currentFilter;
            }
            //save values in the ViewBag to return the correct page during the search
            //save searchValue
            ViewBag.currentFilter = searchT;

            var transaction = (from book in Database.Session.Query<Book>()
                           join transact in Database.Session.Query<Transaction>()
                               on book.AssetNum equals transact.BookAssetNumber
                           join borrower in Database.Session.Query<User>()
                               on transact.UserId equals borrower.Id
                           where transact.CheckInDate == null 
                          
                           orderby transact.CheckoutDate
                               descending
                           select new Transactions
                           {
                               Id = transact.Id,
                               AssetNum = book.AssetNum,
                               Isbn = book.Isbn,
                               Title = book.Title,
                               Author = book.Author,
                               CourseSection = book.CourseSection,
                               Euid = borrower.Euid,
                               FirstName = borrower.FirstName,
                               LastName = borrower.LastName,
                               CheckoutDate = transact.CheckoutDate,
                               DueDate = transact.DueDate,

                           }).ToList();

            if (!string.IsNullOrEmpty(searchT))
            {
                transaction = transaction.AsQueryable().Where(u => u.Euid.Contains(searchT)
                                                                   || u.FirstName.Contains(searchT) ||
                                                                   u.LastName.Contains(searchT)
                                                                   || u.Isbn.Contains(searchT)).ToList();
               
            }
           
            const int pageSize = 5;
            int pageNumber = (page ?? 1);
            var list = new TransactioList
            {
                TrasactList = transaction.OrderBy(n => n.CheckoutDate).ToPagedList(pageNumber, pageSize)

            };
            
            return View(list);
        }

        //return view for editing the transaction
        [SelectedTab("Transactions")]
        public ActionResult Edit(int id)
        {
            //run a query to find transaction that needs to be edited based on the transaction Id
            var getTransaction = (from book in Database.Session.Query<Book>()
                                  join transact in Database.Session.Query<Transaction>()
                                      on book.AssetNum equals transact.BookAssetNumber
                                  join borrower in Database.Session.Query<User>()
                                      on transact.UserId equals borrower.Id
                                  where transact.Id.Equals(id) && transact.CheckInDate == null
                                  select new Transactions
                                  {
                                      Id = transact.Id,
                                      AssetNum = book.AssetNum,
                                      Isbn = book.Isbn,
                                      Title = book.Title,
                                      Author = book.Author,
                                      CourseSection = book.CourseSection,
                                      Euid = borrower.Euid,
                                      FirstName = borrower.FirstName,
                                      LastName = borrower.LastName,
                                      CheckoutDate = transact.CheckoutDate,
                                      DueDate = transact.DueDate,
                                      CheckInDate = transact.CheckInDate
                                  }).SingleOrDefault();
               
            if (getTransaction == null)
            {
                return HttpNotFound();
            }
            //if transaction exists, return the view
            return View(new TransactionEdit
            {

                AssetNum = getTransaction.AssetNum,
                Isbn = getTransaction.Isbn,
                Title = getTransaction.Title,
                Author = getTransaction.Author,
                CourseSection = getTransaction.CourseSection,
                Euid = getTransaction.Euid,
                FirstName = getTransaction.FirstName,
                LastName = getTransaction.LastName,
                CheckoutDate = getTransaction.CheckoutDate.ToShortDateString(),
                DueDate = getTransaction.DueDate.ToShortDateString(),

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SelectedTab("Transactions")]
        public ActionResult Edit(int id, TransactionEdit form)
        {
            //runa query to get the transaction that needs to be updated
            var getTransaction = (from book in Database.Session.Query<Book>()
                                  join transact in Database.Session.Query<Transaction>()
                                      on book.AssetNum equals transact.BookAssetNumber
                                  join borrower in Database.Session.Query<User>()
                                      on transact.UserId equals borrower.Id
                                  where transact.Id.Equals(id) && transact.CheckInDate == null
                                  select new TransactionPost
                                  {
                                      CheckoutDate = transact.CheckoutDate,
                                      DueDate = transact.DueDate,
                                      Id = transact.Id,
                                      UserId = transact.UserId,
                                      BookAssetNumber = transact.BookAssetNumber

                                  }).SingleOrDefault();
            if (getTransaction == null)
            {
                return HttpNotFound();
            }
            //check if Model complient with requirements
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            
           
            var t = new Transaction();
            //call class to map between viewmodel and transaction model 
            MapTransaction(t, form, getTransaction);
            //save update to the database
            Database.Session.Update(t);

            return RedirectToAction("List");
        }

        //map ViewModel TransactionPost to existing Transaction model in order to save updates
        private void MapTransaction( Transaction transaction, TransactionEdit edit, TransactionPost ts)
        {

            transaction.CheckoutDate = edit.CheckoutDate.AsDateTime();
            transaction.DueDate = edit.DueDate.AsDateTime();
            transaction.Id = ts.Id;
            transaction.UserId = ts.UserId;
            transaction.BookAssetNumber = ts.BookAssetNumber;
            transaction.CheckInDate = null;
        }

    }


}
