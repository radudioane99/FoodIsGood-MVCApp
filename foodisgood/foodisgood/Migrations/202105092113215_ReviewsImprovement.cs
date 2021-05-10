namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewsImprovement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rewiews", "date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Rewiews", "note", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rewiews", "note");
            DropColumn("dbo.Rewiews", "date");
        }
    }
}
