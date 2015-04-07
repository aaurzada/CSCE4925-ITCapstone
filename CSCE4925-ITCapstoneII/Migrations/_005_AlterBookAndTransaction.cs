using System.Data;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(5)]
    public class _005_AlterBookAndTransaction : Migration
    {
        public override void Up()
        {
            Alter.Table("book")
                .AddColumn("inStock").AsBoolean();

            Alter.Table("transaction")
                .AddColumn("checkInDate").AsDate();

        }

        public override void Down()
        {
            Delete.Column("book_inStock")
                .FromTable("transaction");

            Delete.Column("inStock")
                .FromTable("book");
        }
    }
}