namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentSalesReceiptsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabDepartment", "salesReceiptId", "dbo.SalesReceipts");
            DropIndex("dbo.tabDepartment", new[] { "salesReceiptId" });
            CreateTable(
                "dbo.DepartmentSalesReceipts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        SalesReceipt_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.SalesReceipt_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceipt_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.SalesReceipt_Id);
            
            DropColumn("dbo.tabDepartment", "salesReceiptId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabDepartment", "salesReceiptId", c => c.Int());
            DropForeignKey("dbo.DepartmentSalesReceipts", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropForeignKey("dbo.DepartmentSalesReceipts", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.DepartmentSalesReceipts", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.DepartmentSalesReceipts", new[] { "Department_Id" });
            DropTable("dbo.DepartmentSalesReceipts");
            CreateIndex("dbo.tabDepartment", "salesReceiptId");
            AddForeignKey("dbo.tabDepartment", "salesReceiptId", "dbo.SalesReceipts", "Id");
        }
    }
}
