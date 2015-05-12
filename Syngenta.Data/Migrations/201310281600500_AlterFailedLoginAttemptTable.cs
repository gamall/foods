namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFailedLoginAttemptTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FailedLoginAttempts", "IsAdmin", c => c.Boolean(false, false, "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FailedLoginAttempts", "IsAdmin");
        }
    }
}
