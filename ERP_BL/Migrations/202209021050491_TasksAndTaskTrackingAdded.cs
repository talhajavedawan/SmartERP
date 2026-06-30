namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksAndTaskTrackingAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(),
                        transactionId = c.Int(nullable: false),
                        transactionType = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        statusId = c.Int(),
                        supervisedById = c.Int(),
                        assignedToId = c.Int(),
                        assignedById = c.Int(),
                        creatorId = c.Int(),
                        currencyId = c.Int(),
                        saleOrderId = c.Int(),
                        purchaseOrderId = c.Int(),
                        saleInvoiceId = c.Int(),
                        SystemRef = c.String(),
                        ManualAmount = c.Double(nullable: false),
                        StartDate = c.DateTime(),
                        TentativeCompletionDate = c.DateTime(),
                        CompletionDate = c.DateTime(),
                        EfficiencyPoints = c.Double(nullable: false),
                        Description = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        PendingForClosing = c.Boolean(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        stage = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.assignedById)
                .ForeignKey("dbo.Users", t => t.assignedToId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrderId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrderId)
                .ForeignKey("dbo.TasksStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.supervisedById)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.statusId)
                .Index(t => t.supervisedById)
                .Index(t => t.assignedToId)
                .Index(t => t.assignedById)
                .Index(t => t.creatorId)
                .Index(t => t.currencyId)
                .Index(t => t.saleOrderId)
                .Index(t => t.purchaseOrderId)
                .Index(t => t.saleInvoiceId);
            
            CreateTable(
                "dbo.TasksStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTrackings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tasksId = c.Int(),
                        UpdateDateTime = c.DateTime(nullable: false),
                        UpdatedById = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.tasksId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.tasksId)
                .Index(t => t.UpdatedById);
            
            AddColumn("dbo.InterCompanyBankTransfers", "paymentGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "receiptGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "vendorBillId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "adminBillGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "adminBillId", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "Tasks", c => c.Int());
            AddColumn("dbo.tabBackground", "taskGroupId", c => c.Int());
            AddColumn("dbo.tabBackground", "isGroup", c => c.Boolean(nullable: false));
            CreateIndex("dbo.InterCompanyBankTransfers", "vendorBillId");
            CreateIndex("dbo.InterCompanyBankTransfers", "adminBillId");
            CreateIndex("dbo.tabBackground", "taskGroupId");
            AddForeignKey("dbo.InterCompanyBankTransfers", "adminBillId", "dbo.tabAdminBill", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "vendorBillId", "dbo.Bills", "Id");
            AddForeignKey("dbo.tabBackground", "taskGroupId", "dbo.TaskGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskTrackings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TaskTrackings", "tasksId", "dbo.Tasks");
            DropForeignKey("dbo.Tasks", "supervisedById", "dbo.Users");
            DropForeignKey("dbo.Tasks", "statusId", "dbo.TasksStatus");
            DropForeignKey("dbo.Tasks", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.Tasks", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.Tasks", "purchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.Tasks", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Tasks", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tasks", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Tasks", "assignedToId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "assignedById", "dbo.Users");
            DropForeignKey("dbo.tabBackground", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.InterCompanyBankTransfers", "vendorBillId", "dbo.Bills");
            DropForeignKey("dbo.InterCompanyBankTransfers", "adminBillId", "dbo.tabAdminBill");
            DropIndex("dbo.TaskTrackings", new[] { "UpdatedById" });
            DropIndex("dbo.TaskTrackings", new[] { "tasksId" });
            DropIndex("dbo.Tasks", new[] { "saleInvoiceId" });
            DropIndex("dbo.Tasks", new[] { "purchaseOrderId" });
            DropIndex("dbo.Tasks", new[] { "saleOrderId" });
            DropIndex("dbo.Tasks", new[] { "currencyId" });
            DropIndex("dbo.Tasks", new[] { "creatorId" });
            DropIndex("dbo.Tasks", new[] { "assignedById" });
            DropIndex("dbo.Tasks", new[] { "assignedToId" });
            DropIndex("dbo.Tasks", new[] { "supervisedById" });
            DropIndex("dbo.Tasks", new[] { "statusId" });
            DropIndex("dbo.Tasks", new[] { "deptId" });
            DropIndex("dbo.Tasks", new[] { "companyId" });
            DropIndex("dbo.tabBackground", new[] { "taskGroupId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "adminBillId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "vendorBillId" });
            DropColumn("dbo.tabBackground", "isGroup");
            DropColumn("dbo.tabBackground", "taskGroupId");
            DropColumn("dbo.AttachmentCategories", "Tasks");
            DropColumn("dbo.InterCompanyBankTransfers", "adminBillId");
            DropColumn("dbo.InterCompanyBankTransfers", "adminBillGroupId");
            DropColumn("dbo.InterCompanyBankTransfers", "vendorBillId");
            DropColumn("dbo.InterCompanyBankTransfers", "receiptGroupId");
            DropColumn("dbo.InterCompanyBankTransfers", "paymentGroupId");
            DropTable("dbo.TaskTrackings");
            DropTable("dbo.TasksStatus");
            DropTable("dbo.Tasks");
        }
    }
}
