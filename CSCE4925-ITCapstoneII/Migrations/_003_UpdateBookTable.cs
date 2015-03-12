using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(3)]
    public class _003_UpdateBookTable : Migration
    {
        public override void Up()
        {
            Alter.Table("book")
                .AlterColumn("edition").AsInt16();
        }

        public override void Down()
        {
            Alter.Table("book")
                .AlterColumn("edition").AsString();
        }
    }
}