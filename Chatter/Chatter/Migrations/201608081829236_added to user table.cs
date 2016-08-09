namespace Chatter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedtousertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "JoinedDate", c => c.String());
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BirthDate");
            DropColumn("dbo.AspNetUsers", "JoinedDate");
        }
    }
}
