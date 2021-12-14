namespace Bis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "insuranceCategory", c => c.String());
            AddColumn("dbo.Company Category", "categoryDayCost", c => c.String());
            AddColumn("dbo.Company Category", "categoryOTCost", c => c.String());
            AlterColumn("dbo.Employee", "pf", c => c.String());
            AlterColumn("dbo.Employee", "esi", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee", "esi", c => c.Boolean());
            AlterColumn("dbo.Employee", "pf", c => c.Boolean());
            DropColumn("dbo.Company Category", "categoryOTCost");
            DropColumn("dbo.Company Category", "categoryDayCost");
            DropColumn("dbo.Employee", "insuranceCategory");
        }
    }
}
