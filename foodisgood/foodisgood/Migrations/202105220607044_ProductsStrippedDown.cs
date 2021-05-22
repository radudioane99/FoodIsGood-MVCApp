namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductsStrippedDown : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Type");
            DropColumn("dbo.Products", "Quality");
            DropColumn("dbo.Products", "ExpirationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ExpirationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Products", "Quality", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "Type", c => c.String(nullable: false));
        }
    }
}
