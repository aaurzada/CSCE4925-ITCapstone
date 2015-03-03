using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SQLSolutions.Models;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

//mapping between C# Transaction class and MySQL "transaction" table
namespace SQLSolutions.Models
{
    public class TransIndex
    {
        public IEnumerable<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        //id of the transaction {primary key}
        public virtual int IdNum { get; set; }
        //foreign key of user id 
        public virtual String UserEuid { get; set; }
        //foreign key of book id
        public virtual int BookAssetNumber { get; set; }
        //date book was checked out
        public virtual DateTime CheckoutDate { get; set; } //can be null
        //date book is due
        public virtual DateTime DueDate { get; set; } //can be null

    }

    public class TransactionMap : ClassMapping<Transaction>
    {
        public TransactionMap() 
        {
            Id(x => x.IdNum, x => x.Generator(Generators.Identity));

            Property(x => x.IdNum, x => x.NotNullable(true));
            Property(x => x.UserEuid, x => x.NotNullable(true));
            Property(x => x.BookAssetNumber, x => x.NotNullable(true));
            Property(x => x.CheckoutDate, x => x.NotNullable(true));
            Property(x => x.DueDate, x => x.NotNullable(true));
        }
    }
}