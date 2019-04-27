namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_UserId_ForeignKey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogPostComments", "CommenterId", c => c.String(maxLength: 128));
            CreateIndex("dbo.BlogPostComments", "CommenterId");
            AddForeignKey("dbo.BlogPostComments", "CommenterId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPostComments", "CommenterId", "dbo.User");
            DropIndex("dbo.BlogPostComments", new[] { "CommenterId" });
            AlterColumn("dbo.BlogPostComments", "CommenterId", c => c.String());
        }
    }
}
