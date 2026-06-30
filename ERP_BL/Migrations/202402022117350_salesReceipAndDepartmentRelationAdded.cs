namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salesReceipAndDepartmentRelationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "salesReceiptId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "TotalCollectionAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.tabDepartment", "salesReceiptId");
            AddForeignKey("dbo.tabDepartment", "salesReceiptId", "dbo.SalesReceipts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabDepartment", "salesReceiptId", "dbo.SalesReceipts");
            DropIndex("dbo.tabDepartment", new[] { "salesReceiptId" });
            DropColumn("dbo.SalesReceipts", "TotalCollectionAmount");
            DropColumn("dbo.tabDepartment", "salesReceiptId");
        }
    }
}
