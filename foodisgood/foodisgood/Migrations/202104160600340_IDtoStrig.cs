namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IDtoStrig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "BuyerUserID", c => c.Int(nullable: false));
        }
    }
}
