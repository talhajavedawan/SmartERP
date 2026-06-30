namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentTypesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Documents", "allocation_Id", "dbo.Employees");
            DropIndex("dbo.Documents", new[] { "dept_Id" });
            DropIndex("dbo.Documents", new[] { "allocation_Id" });
            RenameColumn(table: "dbo.Documents", name: "DocumentStatus_Id", newName: "status_Id");
            RenameColumn(table: "dbo.Documents", name: "user_Id", newName: "creator_Id");
            RenameIndex(table: "dbo.Documents", name: "IX_user_Id", newName: "IX_creator_Id");
            RenameIndex(table: "dbo.Documents", name: "IX_DocumentStatus_Id", newName: "IX_status_Id");
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "documentType_Id", c => c.Int());
            AddColumn("dbo.Documents", "country_Id", c => c.Int());
            AddColumn("dbo.Documents", "IssueDate", c => c.DateTime());
            AddColumn("dbo.Documents", "ExpiryDate", c => c.DateTime());
            AddColumn("dbo.Documents", "isOpen", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "stage", c => c.String());
            AddColumn("dbo.SaleOrders", "BillRefNoId", c => c.Int());
            AlterColumn("dbo.Documents", "dept_Id", c => c.Int());
            AlterColumn("dbo.Documents", "allocation_Id", c => c.Int());
            CreateIndex("dbo.Documents", "documentType_Id");
            CreateIndex("dbo.Documents", "dept_Id");
            CreateIndex("dbo.Documents", "country_Id");
            CreateIndex("dbo.Documents", "allocation_Id");
            CreateIndex("dbo.SaleOrders", "BillRefNoId");
            AddForeignKey("dbo.Documents", "country_Id", "dbo.Countries", "Id");
            AddForeignKey("dbo.Documents", "documentType_Id", "dbo.DocumentTypes", "Id");
            AddForeignKey("dbo.SaleOrders", "BillRefNoId", "dbo.VendorBillReferences", "Id");
            AddForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Documents", "allocation_Id", "dbo.Employees", "EmpId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrders", "BillRefNoId", "dbo.VendorBillReferences");
            DropForeignKey("dbo.Documents", "documentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "country_Id", "dbo.Countries");
            DropIndex("dbo.SaleOrders", new[] { "BillRefNoId" });
            DropIndex("dbo.Documents", new[] { "allocation_Id" });
            DropIndex("dbo.Documents", new[] { "country_Id" });
            DropIndex("dbo.Documents", new[] { "dept_Id" });
            DropIndex("dbo.Documents", new[] { "documentType_Id" });
            AlterColumn("dbo.Documents", "allocation_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Documents", "dept_Id", c => c.Int(nullable: false));
            DropColumn("dbo.SaleOrders", "BillRefNoId");
            DropColumn("dbo.Documents", "stage");
            DropColumn("dbo.Documents", "isOpen");
            DropColumn("dbo.Documents", "ExpiryDate");
            DropColumn("dbo.Documents", "IssueDate");
            DropColumn("dbo.Documents", "country_Id");
            DropColumn("dbo.Documents", "documentType_Id");
            DropTable("dbo.DocumentTypes");
            RenameIndex(table: "dbo.Documents", name: "IX_status_Id", newName: "IX_DocumentStatus_Id");
            RenameIndex(table: "dbo.Documents", name: "IX_creator_Id", newName: "IX_user_Id");
            RenameColumn(table: "dbo.Documents", name: "creator_Id", newName: "user_Id");
            RenameColumn(table: "dbo.Documents", name: "status_Id", newName: "DocumentStatus_Id");
            CreateIndex("dbo.Documents", "allocation_Id");
            CreateIndex("dbo.Documents", "dept_Id");
            AddForeignKey("dbo.Documents", "allocation_Id", "dbo.Employees", "EmpId", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment", "Id", cascadeDelete: true);
        }
    }
}
