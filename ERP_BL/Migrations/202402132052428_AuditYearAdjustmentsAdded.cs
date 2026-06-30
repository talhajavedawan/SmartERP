namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditYearAdjustmentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditYearAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_Id = c.Int(),
                        auditYearAdjustmentType = c.Int(nullable: false),
                        company_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        allocation_Id = c.Int(),
                        principal_Id = c.Int(nullable: false),
                        auditYear = c.DateTime(),
                        SOAmount = c.Double(nullable: false),
                        auditCurrency_Id = c.Int(),
                        BudgetMargin = c.Double(nullable: false),
                        ActualMargin = c.Double(nullable: false),
                        SystemMargin = c.Double(nullable: false),
                        SOAmountAudit = c.Double(nullable: false),
                        BudgetMarginAudit = c.Double(nullable: false),
                        ActualMarginAudit = c.Double(nullable: false),
                        SystemMarginAudit = c.Double(nullable: false),
                        ExhangeRate = c.Double(nullable: false),
                        revenue = c.Double(nullable: false),
                        defferedIncome = c.Double(nullable: false),
                        cgs = c.Double(nullable: false),
                        accountReceivable = c.Double(nullable: false),
                        accountPayable = c.Double(nullable: false),
                        bank = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.auditCurrency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.auditCurrency_Id);
            
            AddColumn("dbo.PurchaseOrders", "expectedPaymentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "auditYearAdjustment_Id", c => c.Int());
            CreateIndex("dbo.SaleOrders", "auditYearAdjustment_Id");
            AddForeignKey("dbo.SaleOrders", "auditYearAdjustment_Id", "dbo.AuditYearAdjustments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "auditYearAdjustment_Id", "dbo.AuditYearAdjustments");
            DropForeignKey("dbo.AuditYearAdjustments", "user_Id", "dbo.Users");
            DropForeignKey("dbo.AuditYearAdjustments", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.AuditYearAdjustments", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.AuditYearAdjustments", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.AuditYearAdjustments", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.AuditYearAdjustments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.AuditYearAdjustments", "auditCurrency_Id", "dbo.Currencies");
            DropIndex("dbo.SaleOrders", new[] { "auditYearAdjustment_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "auditCurrency_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "principal_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "allocation_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "customerCompany_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "dept_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "company_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "user_Id" });
            DropColumn("dbo.SaleOrders", "auditYearAdjustment_Id");
            DropColumn("dbo.PurchaseOrders", "expectedPaymentAmount");
            DropTable("dbo.AuditYearAdjustments");
        }
    }
}
