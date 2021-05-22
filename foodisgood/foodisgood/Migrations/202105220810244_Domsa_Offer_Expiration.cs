namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Domsa_Offer_Expiration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "Expired", c => c.Boolean(nullable: false));
            DropColumn("dbo.Offers", "OfferType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "OfferType", c => c.Boolean(nullable: false));
            DropColumn("dbo.Offers", "Expired");
        }
    }
}
