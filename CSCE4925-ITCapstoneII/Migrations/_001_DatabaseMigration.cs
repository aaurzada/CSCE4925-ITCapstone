﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(1)]
    public class _001_UserBookTransaction : Migration
    {
        //migrates data from code to database schema
        public override void Up()
        {
            //Defines user table
            Create.Table("user")
                //User's school ID number
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                //User's EUID
                .WithColumn("euid").AsString()
                //User's first name
                .WithColumn("firstName").AsString(128)
                //User's last name
                .WithColumn("lastName").AsString(128)
                //User's email
                .WithColumn("email").AsCustom("VARCHAR(256)");

            //Defines book table
            Create.Table("book")
                //internal book ID number
                .WithColumn("assetNum").AsInt16().Identity().PrimaryKey()
                //Book's ISBN number
                .WithColumn("isbn").AsCustom("INT(13)")
                //Book's title
                .WithColumn("title").AsString()
                //Book's author
                .WithColumn("author").AsString()
                //Course section book is used in
                .WithColumn("courseSection").AsString()
                //Year book was published
                .WithColumn("year").AsCustom("INT(4)")
                //Edition of book
                .WithColumn("edition").AsString()
                //Flag for if book is required for class
                .WithColumn("isRequired").AsBoolean();

            //Defines transaction table
            Create.Table("transaction")
                //transaction's internal ID number
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                //ID number of user who checked out book
                .WithColumn("user_id").AsInt32().ForeignKey("user", "id").OnDelete(Rule.Cascade)
                //Checked out book's internal ID number
                .WithColumn("book_assetNum").AsInt16().ForeignKey("book", "assetNum").OnDelete(Rule.Cascade)
                //Date book was checked out
                .WithColumn("checkoutDate").AsDate()
                //Date book is due
                .WithColumn("dueDate").AsDate();
        }

        //rolls back migration
        public override void Down()
        {
            //transaction table must always be deleted first due to cascade deletion rules of foreign key
            Delete.Table("transaction");
            Delete.Table("user");
            Delete.Table("book");
        }
    }
}