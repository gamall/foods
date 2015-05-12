namespace Syngenta.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserBookNewColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserBooks", "SelectedPagesIntro", c => c.String());
            AddColumn("dbo.UserBooks", "SelectedPagesAdvanced", c => c.String());
            DropColumn("dbo.UserBooks", "SelectedPages");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserBooks", "SelectedPages", c => c.String());
            DropColumn("dbo.UserBooks", "SelectedPagesAdvanced");
            DropColumn("dbo.UserBooks", "SelectedPagesIntro");
        }
    }
}
