namespace StartWatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PingLong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ping", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ping");
        }
    }
}
