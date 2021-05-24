namespace foodisgood.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTheReviewer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rewiews", "UserReviewer", c => c.String(nullable: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Rewiews", "UserReviewer");
        }
    }
}
