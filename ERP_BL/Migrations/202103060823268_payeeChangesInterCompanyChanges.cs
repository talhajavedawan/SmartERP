namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payeeChangesInterCompanyChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payees", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payees", "dept_Id", "dbo.tabDepartment");
            DropIndex("dbo.Payees", new[] { "company_Id" });
            DropIndex("dbo.Payees", new[] { "dept_Id" });
            RenameColumn(table: "dbo.tabAdminBill", name: "status_Id", newName: "statusId");
            RenameIndex(table: "dbo.tabAdminBill", name: "IX_status_Id", newName: "IX_statusId");
            CreateTable(
                "dbo.InterCompanyBankTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        SystemRefNo = c.String(),
                        FinanceRefNo = c.String(),
                        TransactionDate = c.DateTime(),
                        InstrumentNo = c.String(),
                        InstrumentDate = c.DateTime(),
                        transferMethod_Id = c.Int(),
                        AmountOC = c.Double(nullable: false),
                        currency_Id = c.Int(),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        companyFrom_Id = c.Int(),
                        deptFrom_Id = c.Int(),
                        bankFrom_Id = c.Int(),
                        accountFrom_Id = c.Int(),
                        companyTo_Id = c.Int(),
                        deptTo_Id = c.Int(),
                        bankTo_Id = c.Int(),
                        accountTo_Id = c.Int(),
                        emp_Id = c.Int(),
                        transferType = c.Int(nullable: false),
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
                .ForeignKey("dbo.tabCompany", t => t.companyFrom_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyTo_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.deptFrom_Id)
                .ForeignKey("dbo.tabDepartment", t => t.deptTo_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.InterBankTransferStatus", t => t.interBankTransStatus_Id)
                .ForeignKey("dbo.TranferMethods", t => t.transferMethod_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.transferMethod_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.companyFrom_Id)
                .Index(t => t.deptFrom_Id)
                .Index(t => t.bankFrom_Id)
                .Index(t => t.accountFrom_Id)
                .Index(t => t.companyTo_Id)
                .Index(t => t.deptTo_Id)
                .Index(t => t.bankTo_Id)
                .Index(t => t.accountTo_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.user_Id)
                .Index(t => t.interBankTransStatus_Id);
            
            CreateTable(
                "dbo.PayeeCompanies",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.Company_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.PayeeDepartments",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.Department_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.Department_Id);
            
            AddColumn("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id", c => c.Int());
            AddColumn("dbo.JournalTransactions", "InterCompanyBankTransfer_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "Bill_Number", c => c.String());
            AddColumn("dbo.Payees", "isSubsidiary", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id");
            CreateIndex("dbo.JournalTransactions", "InterCompanyBankTransfer_Id");
            AddForeignKey("dbo.JournalTransactions", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers", "Id");
            AddForeignKey("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers", "Id");
            DropColumn("dbo.tabAdminBill", "billNumber");
            DropColumn("dbo.Payees", "company_Id");
            DropColumn("dbo.Payees", "dept_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payees", "dept_Id", c => c.Int());
            AddColumn("dbo.Payees", "company_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "billNumber", c => c.String());
            DropForeignKey("dbo.InterCompanyBankTransfers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.InterCompanyBankTransfers", "transferMethod_Id", "dbo.TranferMethods");
            DropForeignKey("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.JournalTransactions", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompanyBankTransfers", "interBankTransStatus_Id", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.InterCompanyBankTransfers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.InterCompanyBankTransfers", "deptTo_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterCompanyBankTransfers", "deptFrom_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterCompanyBankTransfers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InterCompanyBankTransfers", "companyTo_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterCompanyBankTransfers", "companyFrom_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterCompanyBankTransfers", "bankTo_Id", "dbo.Banks");
            DropForeignKey("dbo.InterCompanyBankTransfers", "bankFrom_Id", "dbo.Banks");
            DropForeignKey("dbo.InterCompanyBankTransfers", "accountTo_Id", "dbo.Accounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "accountFrom_Id", "dbo.Accounts");
            DropForeignKey("dbo.PayeeDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PayeeDepartments", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.PayeeCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PayeeCompanies", "Payee_Id", "dbo.Payees");
            DropIndex("dbo.PayeeDepartments", new[] { "Department_Id" });
            DropIndex("dbo.PayeeDepartments", new[] { "Payee_Id" });
            DropIndex("dbo.PayeeCompanies", new[] { "Company_Id" });
            DropIndex("dbo.PayeeCompanies", new[] { "Payee_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "interBankTransStatus_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "user_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "emp_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "accountTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "bankTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "deptTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "companyTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "accountFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "bankFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "deptFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "companyFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currency_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "transferMethod_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "InterCompanyBankTransfer_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "InterCompanyBankTransfer_Id" });
            DropColumn("dbo.Payees", "isSubsidiary");
            DropColumn("dbo.tabAdminBill", "Bill_Number");
            DropColumn("dbo.JournalTransactions", "InterCompanyBankTransfer_Id");
            DropColumn("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id");
            DropTable("dbo.PayeeDepartments");
            DropTable("dbo.PayeeCompanies");
            DropTable("dbo.InterCompanyBankTransfers");
            RenameIndex(table: "dbo.tabAdminBill", name: "IX_statusId", newName: "IX_status_Id");
            RenameColumn(table: "dbo.tabAdminBill", name: "statusId", newName: "status_Id");
            CreateIndex("dbo.Payees", "dept_Id");
            CreateIndex("dbo.Payees", "company_Id");
            AddForeignKey("dbo.Payees", "dept_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Payees", "company_Id", "dbo.tabCompany", "Id");
        }
    }
}
