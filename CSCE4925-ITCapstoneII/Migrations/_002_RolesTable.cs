using System.Data;
using FluentMigrator;

namespace SQLSolutions.Migrations
{
    
    [Migration(2)]
    public class _002_RolesTable : Migration
    {
        public override void Up()
        {
            Create.Table("roles")
                .WithColumn("id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("name").AsString(128).NotNullable();


            Create.Table("roles_user")
                .WithColumn("user_id").AsInt32().ForeignKey("user", "id").OnDelete(Rule.Cascade)
                .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);
        }

        public override void Down()
        {
            Delete.Table("roles_user");
            Delete.Table("roles");
        }
    }
}