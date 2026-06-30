namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenantRentalStatusAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TenantRentalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusClassTenantRentalStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TenantRentalStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TenantRentalStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TenantRentalStatus", t => t.TenantRentalStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TenantRentalStatus_Id);
            
            CreateTable(
                "dbo.CompanyVendors",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Vendor_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Vendor_Id);
            
            AddColumn("dbo.TenantRentals", "statusId", c => c.Int());
            AddColumn("dbo.TenantRentals", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.TenantRentals", "SystemRef", c => c.String());
            AddColumn("dbo.TenantRentals", "creatorId", c => c.Int());
            AddColumn("dbo.TenantRentals", "stage", c => c.String());
            AddColumn("dbo.TenantRentals", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.TenantRentals", "isReviewed", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "needReview", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "isApproved", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.TenantRentals", "isReApproved", c => c.Boolean());
            AddColumn("dbo.TenantRentals", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.TenantRentals", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.TenantRentals", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.TenantRentals", "statusId");
            CreateIndex("dbo.TenantRentals", "creatorId");
            AddForeignKey("dbo.TenantRentals", "creatorId", "dbo.Users", "id");
            AddForeignKey("dbo.TenantRentals", "statusId", "dbo.TenantRentalStatus", "Id");
            DropColumn("dbo.TenantRentals", "isActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TenantRentals", "isActive", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.CompanyVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.CompanyVendors", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.StatusClassTenantRentalStatus", "TenantRentalStatus_Id", "dbo.TenantRentalStatus");
            DropForeignKey("dbo.StatusClassTenantRentalStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.TenantRentals", "statusId", "dbo.TenantRentalStatus");
            DropForeignKey("dbo.TenantRentals", "creatorId", "dbo.Users");
            DropIndex("dbo.CompanyVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.CompanyVendors", new[] { "Company_Id" });
            DropIndex("dbo.StatusClassTenantRentalStatus", new[] { "TenantRentalStatus_Id" });
            DropIndex("dbo.StatusClassTenantRentalStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.TenantRentals", new[] { "creatorId" });
            DropIndex("dbo.TenantRentals", new[] { "statusId" });
            DropColumn("dbo.TenantRentals", "LastStatusChangeDate");
            DropColumn("dbo.TenantRentals", "ClosingDate");
            DropColumn("dbo.TenantRentals", "ReApprovalDate");
            DropColumn("dbo.TenantRentals", "isReApproved");
            DropColumn("dbo.TenantRentals", "ApprovedDate");
            DropColumn("dbo.TenantRentals", "isApproved");
            DropColumn("dbo.TenantRentals", "PendingForReApproval");
            DropColumn("dbo.TenantRentals", "PendingForClosing");
            DropColumn("dbo.TenantRentals", "needReview");
            DropColumn("dbo.TenantRentals", "isReviewed");
            DropColumn("dbo.TenantRentals", "isVoid");
            DropColumn("dbo.TenantRentals", "stage");
            DropColumn("dbo.TenantRentals", "creatorId");
            DropColumn("dbo.TenantRentals", "SystemRef");
            DropColumn("dbo.TenantRentals", "transactionGroupId");
            DropColumn("dbo.TenantRentals", "statusId");
            DropTable("dbo.CompanyVendors");
            DropTable("dbo.StatusClassTenantRentalStatus");
            DropTable("dbo.TenantRentalStatus");
        }
    }
}
