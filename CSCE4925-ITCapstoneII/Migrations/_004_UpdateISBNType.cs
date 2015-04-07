using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(4)]
    public class _004_UpdateISBNType : Migration
    {
        public override void Up()
        {
            Alter.Table("book")
                .AlterColumn("isbn").AsString();
        }

        public override void Down()
        {
            Alter.Table("book")
                .AlterColumn("isbn").AsCustom("INT(13)");
        }
    }
}