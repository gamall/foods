namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSelectedPageFromUserBookTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserBooks", "SelectedPagesIntro");
            DropColumn("dbo.UserBooks", "SelectedPagesAdvanced");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserBooks", "SelectedPagesAdvanced", c => c.String());
            AddColumn("dbo.UserBooks", "SelectedPagesIntro", c => c.String());
        }
    }
}
