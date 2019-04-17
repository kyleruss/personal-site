namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_User_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "DisplayName", c => c.String());
            AddColumn("dbo.User", "ProfilePicture", c => c.String());
            AddColumn("dbo.User", "Provider", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Provider");
            DropColumn("dbo.User", "ProfilePicture");
            DropColumn("dbo.User", "DisplayName");
        }
    }
}
