namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBookEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.UserBooks",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   UserId = c.Int(nullable: false),
                   SelectedPages = c.String(false,1024),
                   LowQuality = c.Boolean(false, false, "0"),
                   HighQuality = c.Boolean(false, false, "0")
               })
               .PrimaryKey(t => t.Id)
               .ForeignKey("Users", o => o.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserBooks");
        }
    }
}
