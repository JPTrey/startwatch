namespace StartWatch.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoneTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ProgressTotal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "ProgressTotal");
        }
    }
}
