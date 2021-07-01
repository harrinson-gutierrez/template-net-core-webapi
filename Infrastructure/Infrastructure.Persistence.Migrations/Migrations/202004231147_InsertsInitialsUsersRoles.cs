using FluentMigrator;

namespace Infrastructure.Persistence.Migrations
{
    [Migration(202004231147, "Inserts of users and roles")]
    public class InsertsInitialsUsersRoles : ForwardOnlyMigration
    {

        public override void Up()
        {
            Execute.EmbeddedScript("202004231147_InsertsInitialsUsersRoles.sql");
        }
    }
}
