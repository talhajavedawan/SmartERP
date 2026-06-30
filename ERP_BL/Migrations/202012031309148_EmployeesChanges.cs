namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeesChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeWorkingStatus",
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
            
            AddColumn("dbo.tabContact", "OfficialSkype", c => c.String());
            AddColumn("dbo.tabContact", "OffSkypePassword", c => c.String());
            AddColumn("dbo.tabContact", "OfficialTeams", c => c.String());
            AddColumn("dbo.tabContact", "OffTeamsPassword", c => c.String());
            AddColumn("dbo.tabContact", "PersonalSkype", c => c.String());
            AddColumn("dbo.tabContact", "PersonalTeams", c => c.String());
            AddColumn("dbo.Employees", "EmployeeId", c => c.String());
            AddColumn("dbo.Employees", "employeeApproval_Id", c => c.Int());
            AddColumn("dbo.Employees", "employeeStatus_Id", c => c.Int());
            AddColumn("dbo.Designations", "isActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Employees", "employeeApproval_Id");
            CreateIndex("dbo.Employees", "employeeStatus_Id");
            AddForeignKey("dbo.Employees", "employeeApproval_Id", "dbo.EmployeeApprovals", "Id");
            AddForeignKey("dbo.Employees", "employeeStatus_Id", "dbo.EmployeeWorkingStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "employeeStatus_Id", "dbo.EmployeeWorkingStatus");
            DropForeignKey("dbo.Employees", "employeeApproval_Id", "dbo.EmployeeApprovals");
            DropIndex("dbo.Employees", new[] { "employeeStatus_Id" });
            DropIndex("dbo.Employees", new[] { "employeeApproval_Id" });
            DropColumn("dbo.Designations", "isActive");
            DropColumn("dbo.Employees", "employeeStatus_Id");
            DropColumn("dbo.Employees", "employeeApproval_Id");
            DropColumn("dbo.Employees", "EmployeeId");
            DropColumn("dbo.tabContact", "PersonalTeams");
            DropColumn("dbo.tabContact", "PersonalSkype");
            DropColumn("dbo.tabContact", "OffTeamsPassword");
            DropColumn("dbo.tabContact", "OfficialTeams");
            DropColumn("dbo.tabContact", "OffSkypePassword");
            DropColumn("dbo.tabContact", "OfficialSkype");
            DropTable("dbo.EmployeeWorkingStatus");
            DropTable("dbo.EmployeeApprovals");
        }
    }
}
