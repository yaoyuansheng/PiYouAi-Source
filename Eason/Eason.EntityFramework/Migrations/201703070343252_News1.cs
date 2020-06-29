namespace Eason.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class News1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleComments",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        parentId = c.Long(nullable: false),
                        contents = c.String(),
                        userId = c.Long(nullable: false),
                        userName = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Articles", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        title = c.String(maxLength: 100),
                        author = c.String(maxLength: 20),
                        creationTime = c.DateTime(nullable: false),
                        contents = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Categories", t => t.id)
                .ForeignKey("dbo.Channels", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        title = c.String(maxLength: 20),
                        sort = c.Int(nullable: false),
                        parentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Channels",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        title = c.String(maxLength: 20),
                        name = c.String(maxLength: 20),
                        sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleComments", "id", "dbo.Articles");
            DropForeignKey("dbo.Articles", "id", "dbo.Channels");
            DropForeignKey("dbo.Articles", "id", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "id" });
            DropIndex("dbo.ArticleComments", new[] { "id" });
            DropTable("dbo.Channels");
            DropTable("dbo.Categories");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleComments");
        }
    }
}
