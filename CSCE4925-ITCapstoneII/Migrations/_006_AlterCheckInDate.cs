using System.Data;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(6)]
    public class _006_AlterCheckInDate : Migration
    {
        public override void Up()
        {
            Delete.Column("checkInDate").FromTable("transaction");

            Alter.Table("transaction")
                .AddColumn("checkInDate").AsDate().Nullable();
        }

        public override void Down()
        {
            Delete.Column("checkInDate").FromTable("transaction");

            Alter.Table("transaction")
                .AddColumn("checkInDate").AsDate();
        }
    }
}