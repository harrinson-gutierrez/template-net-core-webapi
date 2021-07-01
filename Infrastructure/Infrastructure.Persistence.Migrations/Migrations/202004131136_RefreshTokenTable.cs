using FluentMigrator;

namespace Infrastructure.Persistence.Migrations
{
    [Migration(202004131136, "Add Refresh Tokens")]
    public class RefreshTokenTable : Migration
    {
        public override void Down()
        {
            Delete.Table("refresh_tokens");
        }
        public override void Up()
        {
            Create.Table("refresh_tokens")
                .WithColumn("token").AsGuid().PrimaryKey()
                .WithColumn("jwt_id").AsString().NotNullable()
                .WithColumn("creation_date").AsDateTime().NotNullable()
                .WithColumn("expired_date").AsDateTime().NotNullable()
                .WithColumn("used").AsBoolean()
                .WithColumn("invalidated").AsBoolean()
                .WithColumn("user_id").AsInt32();

           Create.ForeignKey("FK_refresh_tokens_user_id_user_id")
              .FromTable("refresh_tokens").ForeignColumn("user_id")
              .ToTable("users").PrimaryColumn("user_id");
        }
    }
}
