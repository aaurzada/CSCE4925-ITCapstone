using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

//mapping between C# Transaction class and MySQL "transaction" table
namespace SQLSolutions.Models
{
    public class Transaction
    {
        //id of the transaction {primary key}
        public virtual int IdNum { get; set; }
        //foreign key of user id 
        public virtual int UserIdNum { get; set; }
        //foreign key of book id
        public virtual int BookAssetNumber { get; set; }
        //date book was checked out
        public virtual DateTime CheckoutDate { get; set; }
        //date book is due
        public virtual DateTime DueDate { get; set; }

    }

    public class TransactionMap : ClassMapping<Transaction>
    {
        public TransactionMap() 
        {
            Id(x => x.IdNum, x => x.Generator(Generators.Identity));

            Property(x => x.IdNum, x => x.NotNullable(true));
            Property(x => x.UserIdNum, x => x.NotNullable(true));
            Property(x => x.BookAssetNumber, x => x.NotNullable(true));
            Property(x => x.CheckoutDate, x => x.NotNullable(true));
            Property(x => x.DueDate, x => x.NotNullable(true));
        }
    }
}