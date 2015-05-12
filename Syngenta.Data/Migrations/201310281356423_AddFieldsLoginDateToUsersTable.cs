namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsLoginDateToUsersTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "LastLoginDate", c => c.DateTime());
            AlterColumn("dbo.Users", "PreviousLoginDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PreviousLoginDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "LastLoginDate", c => c.DateTime(nullable: false));
        }
    }
}
