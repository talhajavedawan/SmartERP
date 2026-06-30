namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterBankTransfer_HrClasses_Added : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Designations", "ParentId");
            RenameColumn(table: "dbo.Designations", name: "parentDesignation_DesigId", newName: "ParentId");
            RenameIndex(table: "dbo.Designations", name: "IX_parentDesignation_DesigId", newName: "IX_ParentId");
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IsActive = c.Boolean(),
                        company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.company_Id);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DegreeType = c.Int(nullable: false),
                        DegreeTitle = c.String(),
                        Specialization = c.String(),
                        MarksObtained = c.Double(nullable: false),
                        MarksTotal = c.Double(nullable: false),
                        MarksPercentage = c.Double(nullable: false),
                        Division = c.String(),
                        StartYear = c.DateTime(),
                        PassingYear = c.DateTime(),
                        Institute = c.String(),
                        Location = c.String(),
                        IsLatest = c.Boolean(nullable: false),
                        IsDistinction = c.Boolean(nullable: false),
                        DistDetails = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        validTill = c.DateTime(),
                        Score = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        employeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.InterBankTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        TransactionDate = c.DateTime(),
                        InstrumentDate = c.DateTime(),
                        FinanceRefNo = c.String(),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        emp_Id = c.Int(),
                        AmountOC = c.Double(nullable: false),
                        currency_Id = c.Int(),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        bankFrom_Id = c.Int(),
                        accountFrom_Id = c.Int(),
                        bankTo_Id = c.Int(),
                        accountTo_Id = c.Int(),
                        transferType = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        InstrumentNo = c.String(),
                        transferMethod_Id = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        interBankTransStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountFrom_Id)
                .ForeignKey("dbo.Accounts", t => t.accountTo_Id)
                .ForeignKey("dbo.Banks", t => t.bankFrom_Id)
                .ForeignKey("dbo.Banks", t => t.bankTo_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.InterBankTransferStatus", t => t.interBankTransStatus_Id)
                .ForeignKey("dbo.TranferMethods", t => t.transferMethod_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.bankFrom_Id)
                .Index(t => t.accountFrom_Id)
                .Index(t => t.bankTo_Id)
                .Index(t => t.accountTo_Id)
                .Index(t => t.transferMethod_Id)
                .Index(t => t.user_Id)
                .Index(t => t.interBankTransStatus_Id);
            
            CreateTable(
                "dbo.InterBankTransferStatus",
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
                "dbo.TranferMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Assets", "stage", c => c.String());
            AddColumn("dbo.Assets", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.Assets", "isReApproved", c => c.Boolean());
            AddColumn("dbo.Assets", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.Assets", "user_Id", c => c.Int());
            AddColumn("dbo.Assets", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.Assets", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.Employees", "PassportNo", c => c.String());
            AddColumn("dbo.Employees", "BloodGroup", c => c.String());
            AddColumn("dbo.Employees", "address2_Id", c => c.Int());
            AddColumn("dbo.Employees", "empFunction_Id", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "InterBankTransfer_Id", c => c.Int());
            CreateIndex("dbo.Assets", "user_Id");
            CreateIndex("dbo.Employees", "address2_Id");
            CreateIndex("dbo.Employees", "empFunction_Id");
            CreateIndex("dbo.ProcurementProducts", "InterBankTransfer_Id");
            AddForeignKey("dbo.Employees", "address2_Id", "dbo.tabAddress", "Id");
            AddForeignKey("dbo.Employees", "empFunction_Id", "dbo.Functions", "Id");
            AddForeignKey("dbo.Assets", "user_Id", "dbo.Users", "id");
            AddForeignKey("dbo.ProcurementProducts", "InterBankTransfer_Id", "dbo.InterBankTransfers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransfers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.InterBankTransfers", "transferMethod_Id", "dbo.TranferMethods");
            DropForeignKey("dbo.ProcurementProducts", "InterBankTransfer_Id", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransfers", "interBankTransStatus_Id", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.InterBankTransfers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.InterBankTransfers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterBankTransfers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InterBankTransfers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterBankTransfers", "bankTo_Id", "dbo.Banks");
            DropForeignKey("dbo.InterBankTransfers", "bankFrom_Id", "dbo.Banks");
            DropForeignKey("dbo.InterBankTransfers", "accountTo_Id", "dbo.Accounts");
            DropForeignKey("dbo.InterBankTransfers", "accountFrom_Id", "dbo.Accounts");
            DropForeignKey("dbo.Assets", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Qualifications", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "empFunction_Id", "dbo.Functions");
            DropForeignKey("dbo.Functions", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Employees", "address2_Id", "dbo.tabAddress");
            DropIndex("dbo.InterBankTransfers", new[] { "interBankTransStatus_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "user_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "transferMethod_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "accountTo_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "bankTo_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "accountFrom_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "bankFrom_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "currency_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "emp_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "dept_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "company_Id" });
            DropIndex("dbo.Qualifications", new[] { "employeeId" });
            DropIndex("dbo.Functions", new[] { "company_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "InterBankTransfer_Id" });
            DropIndex("dbo.Employees", new[] { "empFunction_Id" });
            DropIndex("dbo.Employees", new[] { "address2_Id" });
            DropIndex("dbo.Assets", new[] { "user_Id" });
            DropColumn("dbo.ProcurementProducts", "InterBankTransfer_Id");
            DropColumn("dbo.Employees", "empFunction_Id");
            DropColumn("dbo.Employees", "address2_Id");
            DropColumn("dbo.Employees", "BloodGroup");
            DropColumn("dbo.Employees", "PassportNo");
            DropColumn("dbo.Assets", "LastStatusChangeDate");
            DropColumn("dbo.Assets", "ClosingDate");
            DropColumn("dbo.Assets", "user_Id");
            DropColumn("dbo.Assets", "ReApprovalDate");
            DropColumn("dbo.Assets", "isReApproved");
            DropColumn("dbo.Assets", "PendingForReApproval");
            DropColumn("dbo.Assets", "stage");
            DropTable("dbo.TranferMethods");
            DropTable("dbo.InterBankTransferStatus");
            DropTable("dbo.InterBankTransfers");
            DropTable("dbo.Qualifications");
            DropTable("dbo.Functions");
            RenameIndex(table: "dbo.Designations", name: "IX_ParentId", newName: "IX_parentDesignation_DesigId");
            RenameColumn(table: "dbo.Designations", name: "ParentId", newName: "parentDesignation_DesigId");
            AddColumn("dbo.Designations", "ParentId", c => c.Int());
        }
    }
}
