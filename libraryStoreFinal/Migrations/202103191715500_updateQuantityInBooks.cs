namespace libraryStoreFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateQuantityInBooks : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Quantity", c => c.Int());
        }
    }
}
