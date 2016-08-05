namespace Chatter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Chits", name: "ApplicationUserId", newName: "User_Id");
            RenameIndex(table: "dbo.Chits", name: "IX_ApplicationUserId", newName: "IX_User_Id");
            AddColumn("dbo.AspNetUsers", "DisplayTitle", c => c.String());
            DropColumn("dbo.AspNetUsers", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "DisplayTitle");
            RenameIndex(table: "dbo.Chits", name: "IX_User_Id", newName: "IX_ApplicationUserId");
            RenameColumn(table: "dbo.Chits", name: "User_Id", newName: "ApplicationUserId");
        }
    }
}
