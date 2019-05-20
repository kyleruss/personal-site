namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Blog_Description_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "Description");
        }
    }
}
