namespace libraryStoreFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BooksCategories", "CategoryId", "dbo.Categories");
            DropPrimaryKey("dbo.Categories");
            DropColumn("dbo.Categories", "ID");

            AddColumn("dbo.Categories", "CategoryID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Categories", "CategoryID");
            AddForeignKey("dbo.BooksCategories", "CategoryId", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.BooksCategories", "CategoryId", "dbo.Categories");
            DropPrimaryKey("dbo.Categories");
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
            DropColumn("dbo.Categories", "CategoryID");
            AddPrimaryKey("dbo.Categories", "ID");
            AddForeignKey("dbo.BooksCategories", "CategoryId", "dbo.Categories", "ID", cascadeDelete: true);
        }
    }
}
