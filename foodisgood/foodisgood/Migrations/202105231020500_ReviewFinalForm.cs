namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewFinalForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rewiews", "ReviewerFirstname", c => c.String());
            AddColumn("dbo.Rewiews", "ReviewerLastname", c => c.String());
            AlterColumn("dbo.Rewiews", "UserReviewer", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Rewiews", "UserReviewer", c => c.String(nullable: false));
            DropColumn("dbo.Rewiews", "ReviewerLastname");
            DropColumn("dbo.Rewiews", "ReviewerFirstname");
        }
    }
}
