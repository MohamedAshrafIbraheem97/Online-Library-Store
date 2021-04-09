namespace libraryStoreFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CategoryBooks",
                c => new
                    {
                        Category_ID = c.Int(nullable: false),
                        Book_BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_ID, t.Book_BookID })
                .ForeignKey("dbo.Categories", t => t.Category_ID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_BookID, cascadeDelete: true)
                .Index(t => t.Category_ID)
                .Index(t => t.Book_BookID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryBooks", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.CategoryBooks", "Category_ID", "dbo.Categories");
            DropIndex("dbo.CategoryBooks", new[] { "Book_BookID" });
            DropIndex("dbo.CategoryBooks", new[] { "Category_ID" });
            DropTable("dbo.CategoryBooks");
            DropTable("dbo.Categories");
        }
    }
}
