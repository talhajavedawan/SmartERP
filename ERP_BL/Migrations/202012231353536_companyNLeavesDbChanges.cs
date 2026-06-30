namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class companyNLeavesDbChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Leaves", new[] { "leaveStatus_Id" });
            RenameColumn(table: "dbo.Leaves", name: "employee_EmpId", newName: "employeeId");
            RenameIndex(table: "dbo.Leaves", name: "IX_employee_EmpId", newName: "IX_employeeId");
            CreateTable(
                "dbo.EmployeeHRInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplyDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        currentStatus = c.Int(nullable: false),
                        isHalf = c.Boolean(nullable: false),
                        LeaveDes = c.String(),
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
                        approver_EmpId = c.Int(),
                        employee_EmpId = c.Int(),
                        HRinfo_Id = c.Int(),
                        initiator_EmpId = c.Int(),
                        leave_Id = c.Int(),
                        leaveStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.approver_EmpId)
                .ForeignKey("dbo.Employees", t => t.employee_EmpId)
                .ForeignKey("dbo.EmployeeHRInfoes", t => t.HRinfo_Id)
                .ForeignKey("dbo.Employees", t => t.initiator_EmpId)
                .ForeignKey("dbo.Leaves", t => t.leave_Id)
                .Index(t => t.approver_EmpId)
                .Index(t => t.employee_EmpId)
                .Index(t => t.HRinfo_Id)
                .Index(t => t.initiator_EmpId)
                .Index(t => t.leave_Id)
                .Index(t => t.leaveStatus_Id);
            
            CreateTable(
                "dbo.CompanyBanks",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Bank_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Bank_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Banks", t => t.Bank_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Bank_Id);
            
            AddColumn("dbo.Employees", "HrInfoId", c => c.Int());
            AddColumn("dbo.Employees", "empDescription", c => c.String());
            AddColumn("dbo.Designations", "AnnualLeaveDays", c => c.Double(nullable: false));
            AddColumn("dbo.Designations", "AnnualLeaveHours", c => c.Double(nullable: false));
            AddColumn("dbo.Designations", "CasualLeaveDays", c => c.Double(nullable: false));
            AddColumn("dbo.Designations", "CasualLeaveHours", c => c.Double(nullable: false));
            AddColumn("dbo.Leaves", "LeaveDays", c => c.Double(nullable: false));
            AddColumn("dbo.Leaves", "LeaveHours", c => c.Double(nullable: false));
            AddColumn("dbo.Leaves", "daysCarried", c => c.Double(nullable: false));
            AddColumn("dbo.Leaves", "DateFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.Leaves", "DateTo", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Employees", "HrInfoId");
            AddForeignKey("dbo.Employees", "HrInfoId", "dbo.EmployeeHRInfoes", "Id");
            DropColumn("dbo.Leaves", "StartDate");
            DropColumn("dbo.Leaves", "EndDate");
            DropColumn("dbo.Leaves", "currentStatus");
            DropColumn("dbo.Leaves", "stage");
            DropColumn("dbo.Leaves", "isVoid");
            DropColumn("dbo.Leaves", "isReviewed");
            DropColumn("dbo.Leaves", "needReview");
            DropColumn("dbo.Leaves", "PendingForClosing");
            DropColumn("dbo.Leaves", "PendingForReApproval");
            DropColumn("dbo.Leaves", "isApproved");
            DropColumn("dbo.Leaves", "ApprovedDate");
            DropColumn("dbo.Leaves", "isReApproved");
            DropColumn("dbo.Leaves", "ReApprovalDate");
            DropColumn("dbo.Leaves", "ClosingDate");
            DropColumn("dbo.Leaves", "LastStatusChangeDate");
            //DropColumn("dbo.Leaves", "leaveStatus_Id");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Leaves", "leaveStatus_Id", c => c.Int());
            AddColumn("dbo.Leaves", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.Leaves", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.Leaves", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.Leaves", "isReApproved", c => c.Boolean());
            AddColumn("dbo.Leaves", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.Leaves", "isApproved", c => c.Boolean());
            AddColumn("dbo.Leaves", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.Leaves", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.Leaves", "needReview", c => c.Boolean());
            AddColumn("dbo.Leaves", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Leaves", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Leaves", "stage", c => c.Int(nullable: false));
            AddColumn("dbo.Leaves", "currentStatus", c => c.Int(nullable: false));
            AddColumn("dbo.Leaves", "EndDate", c => c.DateTime());
            AddColumn("dbo.Leaves", "StartDate", c => c.DateTime());
            DropForeignKey("dbo.CompanyBanks", "Bank_Id", "dbo.Banks");
            DropForeignKey("dbo.CompanyBanks", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Employees", "HrInfoId", "dbo.EmployeeHRInfoes");
            DropForeignKey("dbo.LeaveApplications", "leave_Id", "dbo.Leaves");
            DropForeignKey("dbo.LeaveApplications", "initiator_EmpId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "HRinfo_Id", "dbo.EmployeeHRInfoes");
            DropForeignKey("dbo.LeaveApplications", "employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "approver_EmpId", "dbo.Employees");
            DropIndex("dbo.CompanyBanks", new[] { "Bank_Id" });
            DropIndex("dbo.CompanyBanks", new[] { "Company_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "leaveStatus_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "leave_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "initiator_EmpId" });
            DropIndex("dbo.LeaveApplications", new[] { "HRinfo_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "employee_EmpId" });
            DropIndex("dbo.LeaveApplications", new[] { "approver_EmpId" });
            DropIndex("dbo.Employees", new[] { "HrInfoId" });
            DropColumn("dbo.Leaves", "DateTo");
            DropColumn("dbo.Leaves", "DateFrom");
            DropColumn("dbo.Leaves", "daysCarried");
            DropColumn("dbo.Leaves", "LeaveHours");
            DropColumn("dbo.Leaves", "LeaveDays");
            DropColumn("dbo.Designations", "CasualLeaveHours");
            DropColumn("dbo.Designations", "CasualLeaveDays");
            DropColumn("dbo.Designations", "AnnualLeaveHours");
            DropColumn("dbo.Designations", "AnnualLeaveDays");
            DropColumn("dbo.Employees", "empDescription");
            DropColumn("dbo.Employees", "HrInfoId");
            DropTable("dbo.CompanyBanks");
            DropTable("dbo.LeaveApplications");
            DropTable("dbo.EmployeeHRInfoes");
            RenameIndex(table: "dbo.Leaves", name: "IX_employeeId", newName: "IX_employee_EmpId");
            RenameColumn(table: "dbo.Leaves", name: "employeeId", newName: "employee_EmpId");
            CreateIndex("dbo.Leaves", "leaveStatus_Id");
        }
    }
}
