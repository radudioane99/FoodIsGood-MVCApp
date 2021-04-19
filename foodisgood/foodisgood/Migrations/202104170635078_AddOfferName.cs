namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfferName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "Name");
        }
    }
}
