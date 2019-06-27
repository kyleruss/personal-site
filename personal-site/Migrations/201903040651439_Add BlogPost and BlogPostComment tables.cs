namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBlogPostandBlogPostCommenttables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogPostComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        PostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.BlogPosts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        TimePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogPostComments", "PostId", "dbo.BlogPosts");
            DropIndex("dbo.BlogPostComments", new[] { "PostId" });
            DropTable("dbo.BlogPosts");
            DropTable("dbo.BlogPostComments");
        }
    }
}
