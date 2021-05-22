namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BuyerUserChanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "BuyerUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "BuyerUser_Id" });
            RenameColumn(table: "dbo.Orders", name: "BuyerUser_Id", newName: "BuyerUserID");
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "BuyerUserID");
            AddForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "BuyerUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "BuyerUserID", c => c.String(nullable: false));
            DropForeignKey("dbo.Orders", "BuyerUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "BuyerUserID" });
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.String(maxLength: 128));
            RenameColumn(table: "dbo.Orders", name: "BuyerUserID", newName: "BuyerUser_Id");
            CreateIndex("dbo.Orders", "BuyerUser_Id");
            AddForeignKey("dbo.Orders", "BuyerUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
