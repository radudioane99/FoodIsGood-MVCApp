namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Location", c => c.String());
            CreateIndex("dbo.Offers", "ApplicationUser_Id");
            AddForeignKey("dbo.Offers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Offers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Location");
            DropColumn("dbo.Offers", "ApplicationUser_Id");
        }
    }
}
