namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewFieldToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsDisabled", c => c.Boolean(false, false, "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsDisabled");
        }
    }
}
