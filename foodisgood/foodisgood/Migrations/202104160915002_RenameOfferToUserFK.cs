namespace foodisgood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameOfferToUserFK : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Offers", name: "ApplicationUser_Id", newName: "UserID");
            RenameIndex(table: "dbo.Offers", name: "IX_ApplicationUser_Id", newName: "IX_UserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Offers", name: "IX_UserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Offers", name: "UserID", newName: "ApplicationUser_Id");
        }
    }
}
