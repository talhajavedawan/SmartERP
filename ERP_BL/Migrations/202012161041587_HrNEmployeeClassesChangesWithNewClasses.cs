namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HrNEmployeeClassesChangesWithNewClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CardHolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        cardHolderType = c.Int(nullable: false),
                        BankID = c.Int(),
                        ParentID = c.Int(),
                        CardUserID = c.Int(),
                        CardNumber = c.String(),
                        SecondaryCardNumber = c.String(),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        CVV = c.Int(nullable: false),
                        cardHolder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankID)
                .ForeignKey("dbo.CardHolders", t => t.cardHolder_Id)
                .ForeignKey("dbo.CardHolders", t => t.CardUserID)
                .ForeignKey("dbo.CardHolders", t => t.ParentID)
                .Index(t => t.BankID)
                .Index(t => t.ParentID)
                .Index(t => t.CardUserID)
                .Index(t => t.cardHolder_Id);
            
            CreateTable(
                "dbo.Emergencyontacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Relation = c.String(),
                        contact = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeWorkExperiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company = c.String(),
                        JobDescription = c.String(),
                        JobTitle = c.String(),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        employerAddress = c.String(),
                        employerContact = c.String(),
                        employeeId = c.Int(),
                        isLatest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.ChartofAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        accountName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        accountType = c.Int(nullable: false),
                        parentId = c.Int(),
                        userId = c.Int(),
                        balanceTotal = c.Double(),
                        description = c.String(),
                        bankAccountNo = c.String(),
                        routingNo = c.String(),
                        creditCardNo = c.String(),
                        accountNo = c.String(),
                        openingBalanceTotalDate = c.DateTime(),
                        creationDate = c.DateTime(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        currency_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.parentId)
                .Index(t => t.userId)
                .Index(t => t.currency_Id);
            
            CreateTable(
                "dbo.Leaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveType = c.Int(nullable: false),
                        LeaveDes = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        ApplyDate = c.DateTime(),
                        currentStatus = c.Int(nullable: false),
                        stage = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        employee_EmpId = c.Int(),
                        leaveStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employee_EmpId)
                .ForeignKey("dbo.LeaveStatus", t => t.leaveStatus_Id)
                .Index(t => t.employee_EmpId)
                .Index(t => t.leaveStatus_Id);
            
            CreateTable(
                "dbo.LeaveStatus",
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
            
            AddColumn("dbo.tabPerson", "PassportNo", c => c.String());
            AddColumn("dbo.tabPerson", "BloodGroup", c => c.String());
            AddColumn("dbo.tabPerson", "CNICexpiryDate", c => c.DateTime());
            AddColumn("dbo.tabPerson", "passportExpiryDate", c => c.DateTime());
            AddColumn("dbo.tabPerson", "passportIssueDate", c => c.DateTime());
            AddColumn("dbo.Employees", "emergencyontact_Id", c => c.Int());
            AddColumn("dbo.Bills", "CardUserId", c => c.Int());
            AddColumn("dbo.Bills", "CreditCardNoId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "CreditedDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "DepositedDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "InstrumentNo", c => c.String());
            AddColumn("dbo.SalesReceipts", "InstrumentDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "principal_Id", c => c.Int());
            AlterColumn("dbo.Qualifications", "DegreeType", c => c.String());
            CreateIndex("dbo.Employees", "emergencyontact_Id");
            CreateIndex("dbo.Bills", "CardUserId");
            CreateIndex("dbo.Bills", "CreditCardNoId");
            CreateIndex("dbo.SalesReceipts", "principal_Id");
            AddForeignKey("dbo.Bills", "CardUserId", "dbo.CardHolders", "Id");
            AddForeignKey("dbo.SalesReceipts", "principal_Id", "dbo.Principals", "Id");
            AddForeignKey("dbo.Bills", "CreditCardNoId", "dbo.CreditCards", "Id");
            AddForeignKey("dbo.Employees", "emergencyontact_Id", "dbo.Emergencyontacts", "Id");
            DropColumn("dbo.Employees", "PassportNo");
            DropColumn("dbo.Employees", "BloodGroup");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "BloodGroup", c => c.String());
            AddColumn("dbo.Employees", "PassportNo", c => c.String());
            DropForeignKey("dbo.Leaves", "leaveStatus_Id", "dbo.LeaveStatus");
            DropForeignKey("dbo.Leaves", "employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.ChartofAccounts", "userId", "dbo.Users");
            DropForeignKey("dbo.ChartofAccounts", "parentId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ChartofAccounts", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.EmployeeWorkExperiences", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "emergencyontact_Id", "dbo.Emergencyontacts");
            DropForeignKey("dbo.Bills", "CreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "ParentID", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCards", "CardUserID", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCards", "cardHolder_Id", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCards", "BankID", "dbo.Banks");
            DropForeignKey("dbo.SalesReceipts", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.Bills", "CardUserId", "dbo.CardHolders");
            DropIndex("dbo.Leaves", new[] { "leaveStatus_Id" });
            DropIndex("dbo.Leaves", new[] { "employee_EmpId" });
            DropIndex("dbo.ChartofAccounts", new[] { "currency_Id" });
            DropIndex("dbo.ChartofAccounts", new[] { "userId" });
            DropIndex("dbo.ChartofAccounts", new[] { "parentId" });
            DropIndex("dbo.EmployeeWorkExperiences", new[] { "employeeId" });
            DropIndex("dbo.CreditCards", new[] { "cardHolder_Id" });
            DropIndex("dbo.CreditCards", new[] { "CardUserID" });
            DropIndex("dbo.CreditCards", new[] { "ParentID" });
            DropIndex("dbo.CreditCards", new[] { "BankID" });
            DropIndex("dbo.SalesReceipts", new[] { "principal_Id" });
            DropIndex("dbo.Bills", new[] { "CreditCardNoId" });
            DropIndex("dbo.Bills", new[] { "CardUserId" });
            DropIndex("dbo.Employees", new[] { "emergencyontact_Id" });
            AlterColumn("dbo.Qualifications", "DegreeType", c => c.Int(nullable: false));
            DropColumn("dbo.SalesReceipts", "principal_Id");
            DropColumn("dbo.SalesReceipts", "InstrumentDate");
            DropColumn("dbo.SalesReceipts", "InstrumentNo");
            DropColumn("dbo.SalesReceipts", "DepositedDate");
            DropColumn("dbo.SalesReceipts", "CreditedDate");
            DropColumn("dbo.Bills", "CreditCardNoId");
            DropColumn("dbo.Bills", "CardUserId");
            DropColumn("dbo.Employees", "emergencyontact_Id");
            DropColumn("dbo.tabPerson", "passportIssueDate");
            DropColumn("dbo.tabPerson", "passportExpiryDate");
            DropColumn("dbo.tabPerson", "CNICexpiryDate");
            DropColumn("dbo.tabPerson", "BloodGroup");
            DropColumn("dbo.tabPerson", "PassportNo");
            DropTable("dbo.LeaveStatus");
            DropTable("dbo.Leaves");
            DropTable("dbo.ChartofAccounts");
            DropTable("dbo.EmployeeWorkExperiences");
            DropTable("dbo.Emergencyontacts");
            DropTable("dbo.CreditCards");
            DropTable("dbo.CardHolders");
        }
    }
}
