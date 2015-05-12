namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBookCompletedColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBooks", "IsCompleted", c => c.Boolean(false, false, "0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBooks", "IsCompleted");
        }
    }
}
