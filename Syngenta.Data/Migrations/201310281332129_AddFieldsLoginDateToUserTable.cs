namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsLoginDateToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LastLoginDate", c => c.DateTime(nullable: true));
            AddColumn("dbo.Users", "PreviousLoginDate", c => c.DateTime(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PreviousLoginDate");
            DropColumn("dbo.Users", "LastLoginDate");
        }
    }
}
