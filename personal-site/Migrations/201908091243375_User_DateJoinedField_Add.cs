namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_DateJoinedField_Add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "DateJoined", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "DateJoined");
        }
    }
}
