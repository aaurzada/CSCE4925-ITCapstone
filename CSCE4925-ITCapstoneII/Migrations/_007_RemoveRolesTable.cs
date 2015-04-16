using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    [Migration(7)]
    public class _007_RemoveRolesTable : Migration
    {
        public override void Up()
        {
            //Delete roles table
            Delete.Table("roles_user");
            Delete.Table("roles");

            //add isAdmin column to user table
            Alter.Table("user").AddColumn("isAdmin").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}