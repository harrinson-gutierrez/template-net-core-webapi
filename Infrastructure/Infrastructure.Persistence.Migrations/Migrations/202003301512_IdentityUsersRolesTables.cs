using FluentMigrator;

namespace Infrastructure.Persistence.Migrations
{
    [Migration(202003301512, "Add User and roles")]
    public class IdentityUsersRolesTables : Migration
    {
        public override void Down()
        {
            Delete.Table("users_roles");
            Delete.Table("roles");
            Delete.Table("users");
        }

        public override void Up()
        {
            Create.Table("users")
            .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("username").AsString().NotNullable()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("email_confirmed").AsBoolean()
            .WithColumn("password_hash").AsString().NotNullable()
            .WithColumn("security_stamp").AsString();

            Create.Table("roles")
            .WithColumn("role_id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("role_name").AsString().NotNullable();

            Create.Table("users_roles")
            .WithColumn("user_id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("role_id").AsInt32().NotNullable().PrimaryKey();

            Create.ForeignKey("FK_users_roles_role_id_role_id")
            .FromTable("users_roles").ForeignColumn("role_id")
            .ToTable("roles").PrimaryColumn("role_id");

            Create.ForeignKey("FK_users_roles_user_id_user_id")
           .FromTable("users_roles").ForeignColumn("user_id")
           .ToTable("users").PrimaryColumn("user_id");
        }
    }
}
