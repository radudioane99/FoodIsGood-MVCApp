namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OffersRequiredAdded1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "BuyerUserID" });
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "BuyerUserID");
            AddForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "BuyerUserID" });
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "BuyerUserID");
            AddForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
