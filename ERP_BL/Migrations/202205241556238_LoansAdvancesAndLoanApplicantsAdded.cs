namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoansAdvancesAndLoanApplicantsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoansAdvances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        creatorId = c.Int(),
                        isEmployee = c.Boolean(nullable: false),
                        applicantTypeId = c.Int(),
                        applicantId = c.Int(),
                        employeeId = c.Int(),
                        LoanAmountOC = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        LoanAmountMER = c.Double(nullable: false),
                        Purpose = c.String(),
                        LoanTenureDays = c.Int(nullable: false),
                        LoanReturnDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanApplicants", t => t.applicantId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.LoanApplicantTypes", t => t.applicantTypeId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.creatorId)
                .Index(t => t.applicantTypeId)
                .Index(t => t.applicantId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.LoanApplicants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        applicantTypeId = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanApplicantTypes", t => t.applicantTypeId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.applicantTypeId)
                .Index(t => t.companyId)
                .Index(t => t.deptId);
            
            CreateTable(
                "dbo.LoanApplicantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MainBanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomReportGeoups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        LastUpdatedDate = c.DateTime(),
                        Title = c.String(),
                        creatorId = c.Int(),
                        updatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Users", t => t.updatorId)
                .Index(t => t.creatorId)
                .Index(t => t.updatorId);
            
            CreateTable(
                "dbo.CustomReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        transactionItemType = c.Int(nullable: false),
                        customReportField = c.Int(nullable: false),
                        accountsType = c.Int(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        chartOfAccountId = c.Int(),
                        CustomReportGeoup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartOfAccountId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.CustomReportGeoups", t => t.CustomReportGeoup_Id)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.chartOfAccountId)
                .Index(t => t.CustomReportGeoup_Id);
            
            AddColumn("dbo.Banks", "bankId", c => c.Int());
            AddColumn("dbo.Payments", "LoansAdvanceId", c => c.Int());
            AddColumn("dbo.Inquiries", "emailBody", c => c.String());
            AddColumn("dbo.Inquiries", "emailSubject", c => c.String());
            AddColumn("dbo.SalesReceipts", "LoansAdvanceId", c => c.Int());
            CreateIndex("dbo.Banks", "bankId");
            CreateIndex("dbo.Payments", "LoansAdvanceId");
            CreateIndex("dbo.SalesReceipts", "LoansAdvanceId");
            AddForeignKey("dbo.Payments", "LoansAdvanceId", "dbo.LoansAdvances", "Id");
            AddForeignKey("dbo.SalesReceipts", "LoansAdvanceId", "dbo.LoansAdvances", "Id");
            AddForeignKey("dbo.Banks", "bankId", "dbo.MainBanks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomReportGeoups", "updatorId", "dbo.Users");
            DropForeignKey("dbo.CustomReports", "CustomReportGeoup_Id", "dbo.CustomReportGeoups");
            DropForeignKey("dbo.CustomReports", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomReports", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.CustomReports", "chartOfAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CustomReportGeoups", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Banks", "bankId", "dbo.MainBanks");
            DropForeignKey("dbo.SalesReceipts", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Payments", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.LoansAdvances", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.LoansAdvances", "creatorId", "dbo.Users");
            DropForeignKey("dbo.LoansAdvances", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.LoansAdvances", "applicantTypeId", "dbo.LoanApplicantTypes");
            DropForeignKey("dbo.LoansAdvances", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.LoansAdvances", "applicantId", "dbo.LoanApplicants");
            DropForeignKey("dbo.LoanApplicants", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.LoanApplicants", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.LoanApplicants", "applicantTypeId", "dbo.LoanApplicantTypes");
            DropIndex("dbo.CustomReports", new[] { "CustomReportGeoup_Id" });
            DropIndex("dbo.CustomReports", new[] { "chartOfAccountId" });
            DropIndex("dbo.CustomReports", new[] { "deptId" });
            DropIndex("dbo.CustomReports", new[] { "companyId" });
            DropIndex("dbo.CustomReportGeoups", new[] { "updatorId" });
            DropIndex("dbo.CustomReportGeoups", new[] { "creatorId" });
            DropIndex("dbo.LoanApplicants", new[] { "deptId" });
            DropIndex("dbo.LoanApplicants", new[] { "companyId" });
            DropIndex("dbo.LoanApplicants", new[] { "applicantTypeId" });
            DropIndex("dbo.LoansAdvances", new[] { "employeeId" });
            DropIndex("dbo.LoansAdvances", new[] { "applicantId" });
            DropIndex("dbo.LoansAdvances", new[] { "applicantTypeId" });
            DropIndex("dbo.LoansAdvances", new[] { "creatorId" });
            DropIndex("dbo.LoansAdvances", new[] { "deptId" });
            DropIndex("dbo.LoansAdvances", new[] { "companyId" });
            DropIndex("dbo.SalesReceipts", new[] { "LoansAdvanceId" });
            DropIndex("dbo.Payments", new[] { "LoansAdvanceId" });
            DropIndex("dbo.Banks", new[] { "bankId" });
            DropColumn("dbo.SalesReceipts", "LoansAdvanceId");
            DropColumn("dbo.Inquiries", "emailSubject");
            DropColumn("dbo.Inquiries", "emailBody");
            DropColumn("dbo.Payments", "LoansAdvanceId");
            DropColumn("dbo.Banks", "bankId");
            DropTable("dbo.CustomReports");
            DropTable("dbo.CustomReportGeoups");
            DropTable("dbo.MainBanks");
            DropTable("dbo.LoanApplicantTypes");
            DropTable("dbo.LoanApplicants");
            DropTable("dbo.LoansAdvances");
        }
    }
}
