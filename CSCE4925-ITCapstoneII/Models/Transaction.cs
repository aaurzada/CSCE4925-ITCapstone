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
        public virtual int id { get; set; }
        //foreign key of user id 
        public virtual string  user_id { get; set; }
        //foreign key of book id
        public virtual int book_assetNum { get; set; }
        //date book was checked out
        public virtual DateTime checkoutDate { get; set; }
        //date book is due
        public virtual DateTime dueDate { get; set; }

    }

    public class TransactionMap : ClassMapping<Transaction>
    {
        public TransactionMap()
        {
            Id(x => x.id, x => x.Generator(Generators.Identity));

            Property(x => x.id, x => x.NotNullable(true));
            Property(x => x.user_id, x =>
            {
                x.Column("user_euid");
                x.NotNullable(true);
            });
            Property(x => x.book_assetNum, x =>
            {
                //override column name to book_assetnum
                x.Column("book_assetnum");
                x.NotNullable(true);
            });
            Property(x => x.checkoutDate, x => x.NotNullable(true));
            Property(x => x.dueDate, x => x.NotNullable(true));
        }
    }
}