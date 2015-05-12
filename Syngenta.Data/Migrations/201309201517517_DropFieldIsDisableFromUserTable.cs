namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropFieldIsDisableFromUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "IsDisabled");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "IsDisabled", c => c.String());
        }
    }
}
