namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClassesForTaskAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProcurementProducts", name: "InterBankTransfer_Id", newName: "interBankTransferId");
            RenameIndex(table: "dbo.ProcurementProducts", name: "IX_InterBankTransfer_Id", newName: "IX_interBankTransferId");
            CreateTable(
                "dbo.TaskTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isProcurementType = c.Boolean(nullable: false),
                        isSaleOrder = c.Boolean(nullable: false),
                        isPurchaseOrder = c.Boolean(nullable: false),
                        isSaleInvoice = c.Boolean(nullable: false),
                        isOffer = c.Boolean(nullable: false),
                        isInquiry = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentTaskTypes",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.TaskType_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.TaskType_Id);
            
            CreateTable(
                "dbo.CompanyTaskTypes",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.TaskType_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.TaskType_Id);
            
            AddColumn("dbo.Tasks", "taskTypeId", c => c.Int());
            CreateIndex("dbo.Tasks", "taskTypeId");
            AddForeignKey("dbo.Tasks", "taskTypeId", "dbo.TaskTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.CompanyTaskTypes", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.DepartmentTaskTypes", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Tasks", "taskTypeId", "dbo.TaskTypes");
            DropIndex("dbo.CompanyTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.CompanyTaskTypes", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.DepartmentTaskTypes", new[] { "Department_Id" });
            DropIndex("dbo.Tasks", new[] { "taskTypeId" });
            DropColumn("dbo.Tasks", "taskTypeId");
            DropTable("dbo.CompanyTaskTypes");
            DropTable("dbo.DepartmentTaskTypes");
            DropTable("dbo.TaskTypes");
            RenameIndex(table: "dbo.ProcurementProducts", name: "IX_interBankTransferId", newName: "IX_InterBankTransfer_Id");
            RenameColumn(table: "dbo.ProcurementProducts", name: "interBankTransferId", newName: "InterBankTransfer_Id");
        }
    }
}
