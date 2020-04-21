namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OverrideConventions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GIgs", "Artist_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.GIgs", "Genre_Id", "dbo.Genres");
            DropIndex("dbo.GIgs", new[] { "Artist_Id" });
            DropIndex("dbo.GIgs", new[] { "Genre_Id" });
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.GIgs", "Venue", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.GIgs", "Artist_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.GIgs", "Genre_Id", c => c.Byte(nullable: false));
            CreateIndex("dbo.GIgs", "Artist_Id");
            CreateIndex("dbo.GIgs", "Genre_Id");
            AddForeignKey("dbo.GIgs", "Artist_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GIgs", "Genre_Id", "dbo.Genres", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GIgs", "Genre_Id", "dbo.Genres");
            DropForeignKey("dbo.GIgs", "Artist_Id", "dbo.AspNetUsers");
            DropIndex("dbo.GIgs", new[] { "Genre_Id" });
            DropIndex("dbo.GIgs", new[] { "Artist_Id" });
            AlterColumn("dbo.GIgs", "Genre_Id", c => c.Byte());
            AlterColumn("dbo.GIgs", "Artist_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.GIgs", "Venue", c => c.String());
            AlterColumn("dbo.Genres", "Name", c => c.String());
            CreateIndex("dbo.GIgs", "Genre_Id");
            CreateIndex("dbo.GIgs", "Artist_Id");
            AddForeignKey("dbo.GIgs", "Genre_Id", "dbo.Genres", "Id");
            AddForeignKey("dbo.GIgs", "Artist_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
