namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInTemplatesModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        elementType = c.String(),
                        transactionType = c.Int(nullable: false),
                        moduleField_Id = c.Int(),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModuleFields", t => t.moduleField_Id)
                .Index(t => t.moduleField_Id);
            
            CreateTable(
                "dbo.ModuleFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        transactionType = c.Int(nullable: false),
                        templateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.templateId)
                .Index(t => t.templateId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        transactionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabNewBills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billTemplate = c.Int(nullable: false),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        emp_Id = c.Int(),
                        SystemRefNo = c.String(),
                        FinanceRefNo = c.String(),
                        TransactionDate = c.DateTime(),
                        currency_Id = c.Int(),
                        COA = c.String(),
                        vendor_Id = c.Int(),
                        payee_Id = c.Int(),
                        BillingMonthFrom = c.DateTime(),
                        BillingMonthTo = c.DateTime(),
                        DueDate = c.DateTime(),
                        AmountOC = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        CardUserId = c.Int(),
                        CreditCardNoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardHolders", t => t.CardUserId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardNoId)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.Payees", t => t.payee_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.payee_Id)
                .Index(t => t.CardUserId)
                .Index(t => t.CreditCardNoId);
            
            CreateTable(
                "dbo.Payees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayeeName = c.String(),
                        ParentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payees", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.PayeeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(),
                        Payee_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payees", t => t.Payee_Id)
                .Index(t => t.Payee_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabNewBills", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.tabNewBills", "payee_Id", "dbo.Payees");
            DropForeignKey("dbo.PayeeCategories", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.Payees", "ParentId", "dbo.Payees");
            DropForeignKey("dbo.tabNewBills", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.tabNewBills", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabNewBills", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.tabNewBills", "CreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.tabNewBills", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabNewBills", "CardUserId", "dbo.CardHolders");
            DropForeignKey("dbo.ModuleFields", "templateId", "dbo.Templates");
            DropForeignKey("dbo.Fields", "moduleField_Id", "dbo.ModuleFields");
            DropIndex("dbo.PayeeCategories", new[] { "Payee_Id" });
            DropIndex("dbo.Payees", new[] { "ParentId" });
            DropIndex("dbo.tabNewBills", new[] { "CreditCardNoId" });
            DropIndex("dbo.tabNewBills", new[] { "CardUserId" });
            DropIndex("dbo.tabNewBills", new[] { "payee_Id" });
            DropIndex("dbo.tabNewBills", new[] { "vendor_Id" });
            DropIndex("dbo.tabNewBills", new[] { "currency_Id" });
            DropIndex("dbo.tabNewBills", new[] { "emp_Id" });
            DropIndex("dbo.tabNewBills", new[] { "dept_Id" });
            DropIndex("dbo.tabNewBills", new[] { "company_Id" });
            DropIndex("dbo.ModuleFields", new[] { "templateId" });
            DropIndex("dbo.Fields", new[] { "moduleField_Id" });
            DropTable("dbo.PayeeCategories");
            DropTable("dbo.Payees");
            DropTable("dbo.tabNewBills");
            DropTable("dbo.Templates");
            DropTable("dbo.ModuleFields");
            DropTable("dbo.Fields");
        }
    }
}
