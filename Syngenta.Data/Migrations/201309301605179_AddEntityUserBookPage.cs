namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntityUserBookPage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserBookPages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserBookId = c.Int(nullable: false),
                        PageNumber = c.Int(nullable: false),
                        IsIntroductory = c.Boolean(nullable: false),
                        Datestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserBooks", t => t.UserBookId, cascadeDelete: true)
                .Index(t => t.UserBookId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserBookPages", new[] { "UserBookId" });
            DropForeignKey("dbo.UserBookPages", "UserBookId", "dbo.UserBooks");
            DropTable("dbo.UserBookPages");
        }
    }
}
