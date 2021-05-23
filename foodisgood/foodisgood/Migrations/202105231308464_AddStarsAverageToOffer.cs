namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStarsAverageToOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "StarsAverage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "StarsAverage");
        }
    }
}
