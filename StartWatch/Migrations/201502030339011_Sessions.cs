namespace StartWatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sessions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "SessionCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "SessionCount");
        }
    }
}
