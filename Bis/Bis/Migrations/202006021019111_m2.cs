namespace Bis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employee", "createUser", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employee", "createUser", c => c.String(maxLength: 10));
        }
    }
}
