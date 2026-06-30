namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillCategories : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Bills", "billCategoryId", c => c.Int());
            CreateIndex("dbo.Bills", "billCategoryId");
            AddForeignKey("dbo.Bills", "billCategoryId", "dbo.BillCategories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "billCategoryId", "dbo.BillCategories");
            DropIndex("dbo.Bills", new[] { "billCategoryId" });
            DropColumn("dbo.Bills", "billCategoryId");
            DropTable("dbo.BillCategories");
        }
    }
}
