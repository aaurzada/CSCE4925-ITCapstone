using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NHibernate.Bytecode.CodeDom;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

//mapping between C# Transaction class and MySQL "transaction" table
namespace SQLSolutions.Models
{
    public class Transaction
    {
        //id of the transaction {primary key}
        public virtual int Id { get; set; }
        //foreign key of user id 
        //public virtual string  UserEuid { get; set; }
        public virtual int UserId { get; set; }
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
            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Id, x => x.NotNullable(true));
            //Property(x => x.UserEuid, x =>
            Property(x => x.UserId, x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });
            Property(x => x.BookAssetNumber, x =>
            {
                //override column name to book_assetnum
                x.Column("book_assetnum");
                x.NotNullable(true);
            });
            Property(x => x.CheckoutDate, x => x.NotNullable(true));
            Property(x => x.DueDate, x => x.NotNullable(true));
        }
    }
}