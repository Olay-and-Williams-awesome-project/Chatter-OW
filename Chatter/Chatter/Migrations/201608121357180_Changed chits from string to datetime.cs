namespace Chatter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changedchitsfromstringtodatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Chits", "ChitCreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Chits", "ChitCreatedAt", c => c.String());
        }
    }
}
