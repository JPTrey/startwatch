namespace StartWatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoLatest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "LatestPing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "LatestPing", c => c.Int(nullable: false));
        }
    }
}
