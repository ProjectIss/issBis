namespace Bis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "createUser", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "createUser");
        }
    }
}
