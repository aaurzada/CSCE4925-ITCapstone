using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLSolutions.Models
{
    public class Transaction
    {
        //id of the transaction {primary key}
        public virtual int idNum { get; set; }
        //foreign key of user id 
        public virtual int user_idNum { get; set; }
        //foreign key of book id
        public virtual int book_idNum { get; set; }
        //date book was checked out
        public virtual DateTime checkoutDate { get; set; }
        //date book is due
        public virtual DateTime dueDate { get; set; }

    }
}