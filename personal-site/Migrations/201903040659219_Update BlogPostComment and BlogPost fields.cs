namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBlogPostCommentandBlogPostfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogPostComments", "CommentContent", c => c.String());
            AddColumn("dbo.BlogPostComments", "CommenterId", c => c.String());
            AddColumn("dbo.BlogPostComments", "PostedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.BlogPosts", "PostContent", c => c.String());
            DropColumn("dbo.BlogPostComments", "Content");
            DropColumn("dbo.BlogPosts", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogPosts", "Content", c => c.String());
            AddColumn("dbo.BlogPostComments", "Content", c => c.String());
            DropColumn("dbo.BlogPosts", "PostContent");
            DropColumn("dbo.BlogPostComments", "PostedDate");
            DropColumn("dbo.BlogPostComments", "CommenterId");
            DropColumn("dbo.BlogPostComments", "CommentContent");
        }
    }
}
