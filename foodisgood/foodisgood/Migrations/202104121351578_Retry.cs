namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Retry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PriceUnit = c.Single(nullable: false),
                        Quantity = c.Single(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Expired = c.Boolean(nullable: false),
                        Description = c.String(),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Quality = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BuyerUserID = c.Int(nullable: false),
                        OfferID = c.Int(nullable: false),
                        DesiredQuantity = c.Single(nullable: false),
                        BuyerUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.BuyerUser_Id)
                .ForeignKey("dbo.Offers", t => t.OfferID, cascadeDelete: true)
                .Index(t => t.OfferID)
                .Index(t => t.BuyerUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "OfferID", "dbo.Offers");
            DropForeignKey("dbo.Orders", "BuyerUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offers", "ProductID", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "BuyerUser_Id" });
            DropIndex("dbo.Orders", new[] { "OfferID" });
            DropIndex("dbo.Offers", new[] { "ProductID" });
            DropTable("dbo.Orders");
            DropTable("dbo.Products");
            DropTable("dbo.Offers");
        }
    }
}
