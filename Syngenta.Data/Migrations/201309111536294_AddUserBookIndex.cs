namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBookIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.UserBooks", "UserId");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserBooks", new[] { "UserId" });
        }
    }
}
