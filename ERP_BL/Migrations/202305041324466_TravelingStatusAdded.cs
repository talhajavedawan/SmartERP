namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelingStatusAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesReceipts", "COAdebit_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.SalesReceipts", new[] { "COAdebit_Id" });
            CreateTable(
                "dbo.TravelingStatus",
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
            
            AddColumn("dbo.tabDepartment", "IsTravelingRecordType", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleOrdeRrefKeys", "salesRefNo", c => c.String());
            AddColumn("dbo.SaleOrdeRrefKeys", "amountOC", c => c.Double(nullable: false));
            AddColumn("dbo.Travelers", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelingRecords", "companyId", c => c.Int());
            AddColumn("dbo.TravelingRecords", "deptId", c => c.Int());
            AddColumn("dbo.TravelingRecords", "statusId", c => c.Int());
            AddColumn("dbo.TravelingRecords", "stage", c => c.String());
            AddColumn("dbo.TravelingRecords", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelingRecords", "isReviewed", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "needReview", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "isApproved", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "isReApproved", c => c.Boolean());
            AddColumn("dbo.TravelingRecords", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "creatorId", c => c.Int());
            AddColumn("dbo.TravelingRecords", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.TravelingRecords", "companyId");
            CreateIndex("dbo.TravelingRecords", "deptId");
            CreateIndex("dbo.TravelingRecords", "statusId");
            CreateIndex("dbo.TravelingRecords", "creatorId");
            AddForeignKey("dbo.TravelingRecords", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.TravelingRecords", "creatorId", "dbo.Users", "id");
            AddForeignKey("dbo.TravelingRecords", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.TravelingRecords", "statusId", "dbo.TravelingStatus", "Id");
            DropColumn("dbo.SalesReceipts", "COAdebit_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesReceipts", "COAdebit_Id", c => c.Int());
            DropForeignKey("dbo.TravelingRecords", "statusId", "dbo.TravelingStatus");
            DropForeignKey("dbo.TravelingRecords", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TravelingRecords", "creatorId", "dbo.Users");
            DropForeignKey("dbo.TravelingRecords", "companyId", "dbo.tabCompany");
            DropIndex("dbo.TravelingRecords", new[] { "creatorId" });
            DropIndex("dbo.TravelingRecords", new[] { "statusId" });
            DropIndex("dbo.TravelingRecords", new[] { "deptId" });
            DropIndex("dbo.TravelingRecords", new[] { "companyId" });
            DropColumn("dbo.TravelingRecords", "LastStatusChangeDate");
            DropColumn("dbo.TravelingRecords", "ClosingDate");
            DropColumn("dbo.TravelingRecords", "creatorId");
            DropColumn("dbo.TravelingRecords", "ReApprovalDate");
            DropColumn("dbo.TravelingRecords", "isReApproved");
            DropColumn("dbo.TravelingRecords", "ApprovedDate");
            DropColumn("dbo.TravelingRecords", "isApproved");
            DropColumn("dbo.TravelingRecords", "PendingForReApproval");
            DropColumn("dbo.TravelingRecords", "PendingForClosing");
            DropColumn("dbo.TravelingRecords", "needReview");
            DropColumn("dbo.TravelingRecords", "isReviewed");
            DropColumn("dbo.TravelingRecords", "isVoid");
            DropColumn("dbo.TravelingRecords", "stage");
            DropColumn("dbo.TravelingRecords", "statusId");
            DropColumn("dbo.TravelingRecords", "deptId");
            DropColumn("dbo.TravelingRecords", "companyId");
            DropColumn("dbo.Travelers", "isActive");
            DropColumn("dbo.SaleOrdeRrefKeys", "amountOC");
            DropColumn("dbo.SaleOrdeRrefKeys", "salesRefNo");
            DropColumn("dbo.tabDepartment", "IsTravelingRecordType");
            DropTable("dbo.TravelingStatus");
            CreateIndex("dbo.SalesReceipts", "COAdebit_Id");
            AddForeignKey("dbo.SalesReceipts", "COAdebit_Id", "dbo.ChartofAccounts", "Id");
        }
    }
}
