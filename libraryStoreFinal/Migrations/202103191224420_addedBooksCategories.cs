namespace libraryStoreFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedBooksCategories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryBooks", "Category_ID", "dbo.Categories");
            DropForeignKey("dbo.CategoryBooks", "Book_BookID", "dbo.Books");
            DropIndex("dbo.CategoryBooks", new[] { "Category_ID" });
            DropIndex("dbo.CategoryBooks", new[] { "Book_BookID" });
            CreateTable(
                "dbo.BooksCategories",
                c => new
                    {
                        BookId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookId, t.CategoryId })
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.CategoryId);
            
            DropTable("dbo.CategoryBooks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CategoryBooks",
                c => new
                    {
                        Category_ID = c.Int(nullable: false),
                        Book_BookID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_ID, t.Book_BookID });
            
            DropForeignKey("dbo.BooksCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.BooksCategories", "BookId", "dbo.Books");
            DropIndex("dbo.BooksCategories", new[] { "CategoryId" });
            DropIndex("dbo.BooksCategories", new[] { "BookId" });
            DropTable("dbo.BooksCategories");
            CreateIndex("dbo.CategoryBooks", "Book_BookID");
            CreateIndex("dbo.CategoryBooks", "Category_ID");
            AddForeignKey("dbo.CategoryBooks", "Book_BookID", "dbo.Books", "BookID", cascadeDelete: true);
            AddForeignKey("dbo.CategoryBooks", "Category_ID", "dbo.Categories", "ID", cascadeDelete: true);
        }
    }
}
