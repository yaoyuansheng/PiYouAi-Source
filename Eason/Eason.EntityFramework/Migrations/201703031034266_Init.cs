namespace Eason.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        url = c.String(nullable: false, maxLength: 100),
                        creationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Roles", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(maxLength: 20),
                        creationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        creationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Roles", t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        password = c.String(nullable: false, maxLength: 128),
                        email = c.String(maxLength: 50),
                        telephone = c.String(maxLength: 15),
                        isDelete = c.Byte(nullable: false),
                        lastLoginTime = c.DateTime(nullable: false),
                        isActive = c.Byte(nullable: false),
                        creationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "id", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "id", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] { "id" });
            DropIndex("dbo.Permissions", new[] { "id" });
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
        }
    }
}
