namespace personal_site.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_UserAccessToken_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAccessTokens",
                c => new
                    {
                        AccessId = c.Int(nullable: false, identity: true),
                        AccessToken = c.String(),
                        AccessTokenSecret = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AccessId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccessTokens", "UserId", "dbo.User");
            DropIndex("dbo.UserAccessTokens", new[] { "UserId" });
            DropTable("dbo.UserAccessTokens");
        }
    }
}
