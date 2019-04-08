namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PostImage_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPosts", "PostImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogPosts", "PostImage");
        }
    }
}
