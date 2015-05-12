namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterUserTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "LastLoginDate");
            DropColumn("dbo.Users", "PreviousLoginDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PreviousLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "LastLoginDate", c => c.DateTime(nullable: false));
        }
    }
}
