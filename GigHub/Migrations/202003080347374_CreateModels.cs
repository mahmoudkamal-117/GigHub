namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GIgs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Venue = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Artist_Id = c.String(maxLength: 128),
                        Genre_Id = c.Byte(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Artist_Id)
                .ForeignKey("dbo.Genres", t => t.Genre_Id)
                .Index(t => t.Artist_Id)
                .Index(t => t.Genre_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GIgs", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.GIgs", "Artist_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GIgs", new[] { "Genre_Id" });
            DropIndex("dbo.GIgs", new[] { "Artist_Id" });
            DropTable("dbo.GIgs");
            DropTable("dbo.Genres");
        }
    }
}
