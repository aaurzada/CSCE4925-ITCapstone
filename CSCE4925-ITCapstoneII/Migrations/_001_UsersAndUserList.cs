using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(1)]
    public class _001_UsersAndUserList : Migration
    {
        //migrates data from code to database schema
        public override void Up()
        {
            //Creates user table with
            //int User ID Number
            //string User First Name
            //string User Last Name
            //string User Email
            Create.Table("user")
                .WithColumn("UserIdNum").AsInt32().Identity().PrimaryKey()
                .WithColumn("UserFirstName").AsString(128)
                .WithColumn("UserLastName").AsString(128)
                .WithColumn("UserEmail").AsCustom("VARCHAR(256)");
        }

        //rolls back migration
        public override void Down()
        {
            Delete.Table("user");
        }
    }
}