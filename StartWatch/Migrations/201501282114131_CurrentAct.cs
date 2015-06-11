namespace StartWatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrentAct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CurrentActivity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CurrentActivity");
        }
    }
}
