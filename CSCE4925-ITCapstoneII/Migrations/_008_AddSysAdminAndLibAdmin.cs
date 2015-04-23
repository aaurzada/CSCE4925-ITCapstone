using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;
using FluentMigrator.Expressions;

//TODO: add migration to add all of development team as sysadmins (role 1) to the system;
//TODO: create migratin to add Daisy as a library admin (role 2) 
//TODO: create migration to add role 3 - user to the roles table

namespace SQLSolutions.Migrations
{
    [Migration(8)]
    public class _008_AddSysAdminAndLibAdmin : Migration
    {
        public override void Up()
        {
            //Add administrator users into user table
            Insert.IntoTable("user")
                .Row(new
                    {
                        euid = "aa0361",
                        firstName = "Andrew",
                        lastName = "Aurzada",
                        email = "andrewaurzada@my.unt.edu",
                        isAdmin = "1"
                    });
            Insert.IntoTable("user")
                .Row(new
                {
                    euid = "cdh0231",
                    firstName = "Cjivona",
                    lastName = "Hicks",
                    email = "cjivonahicks@my.unt.edu",
                    isAdmin = "1"
                });
            Insert.IntoTable("user")
                .Row(new
                {
                    euid = "sen0043",
                    firstName = "Spencer",
                    lastName = "Newell",
                    email = "spencernewell@my.unt.edu",
                    isAdmin = "1"
                });
            Insert.IntoTable("user")
                .Row(new
                {
                    euid = "es0300",
                    firstName = "Katerina",
                    lastName = "Tiagunov",
                    email = "katerinatiagunov@my.unt.edu",
                    isAdmin = "1"
                });
        }

        public override void Down()
        {
            Delete.FromTable("user").Row(new {firstName = "Andrew"});
            Delete.FromTable("user").Row(new {firstName = "Cjivona"});
            Delete.FromTable("user").Row(new {firstName = "Katerina"});
            Delete.FromTable("user").Row(new {firstName = "Spencer"});
        }
    }
}