namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationFlagsAndTenancyContractsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalAssets", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.RentalAssets", new[] { "person_Id" });
            CreateTable(
                "dbo.NotificationFlags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Flag = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenancyContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContractDateFrom = c.DateTime(),
                        ContractDateTo = c.DateTime(),
                        TenancyContractDate = c.DateTime(),
                        ContractReferenceNo = c.String(),
                        RentalAmount = c.Double(nullable: false),
                        AssetId = c.Int(),
                        UnitId = c.Int(),
                        TenantId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssets", t => t.AssetId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .ForeignKey("dbo.RentalAssets", t => t.UnitId)
                .Index(t => t.AssetId)
                .Index(t => t.UnitId)
                .Index(t => t.TenantId);
            
            AddColumn("dbo.FieldValues", "isExportLicense", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "CcUserId", c => c.Int());
            AddColumn("dbo.Notifications", "FlagId", c => c.Int());
            AddColumn("dbo.RentalAssets", "companyId", c => c.Int());
            AddColumn("dbo.RentalAssets", "deptId", c => c.Int());
            AddColumn("dbo.Tenants", "companyId", c => c.Int());
            AddColumn("dbo.Tenants", "deptId", c => c.Int());
            AlterColumn("dbo.Notifications", "UserId", c => c.Int());
            AlterColumn("dbo.Tenants", "CreationDate", c => c.DateTime());
            CreateIndex("dbo.Notifications", "UserId");
            CreateIndex("dbo.Notifications", "CcUserId");
            CreateIndex("dbo.Notifications", "FlagId");
            CreateIndex("dbo.RentalAssets", "companyId");
            CreateIndex("dbo.RentalAssets", "deptId");
            CreateIndex("dbo.Tenants", "companyId");
            CreateIndex("dbo.Tenants", "deptId");
            AddForeignKey("dbo.Notifications", "CcUserId", "dbo.Users", "id");
            AddForeignKey("dbo.Notifications", "FlagId", "dbo.NotificationFlags", "Id");
            AddForeignKey("dbo.RentalAssets", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.RentalAssets", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Tenants", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.Tenants", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Notifications", "UserId", "dbo.Users", "id");
            DropColumn("dbo.RentalAssets", "person_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalAssets", "person_Id", c => c.Int());
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.TenancyContracts", "UnitId", "dbo.RentalAssets");
            DropForeignKey("dbo.TenancyContracts", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Tenants", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Tenants", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.TenancyContracts", "AssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalAssets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalAssets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Notifications", "FlagId", "dbo.NotificationFlags");
            DropForeignKey("dbo.Notifications", "CcUserId", "dbo.Users");
            DropIndex("dbo.Tenants", new[] { "deptId" });
            DropIndex("dbo.Tenants", new[] { "companyId" });
            DropIndex("dbo.TenancyContracts", new[] { "TenantId" });
            DropIndex("dbo.TenancyContracts", new[] { "UnitId" });
            DropIndex("dbo.TenancyContracts", new[] { "AssetId" });
            DropIndex("dbo.RentalAssets", new[] { "deptId" });
            DropIndex("dbo.RentalAssets", new[] { "companyId" });
            DropIndex("dbo.Notifications", new[] { "FlagId" });
            DropIndex("dbo.Notifications", new[] { "CcUserId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            AlterColumn("dbo.Tenants", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Notifications", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Tenants", "deptId");
            DropColumn("dbo.Tenants", "companyId");
            DropColumn("dbo.RentalAssets", "deptId");
            DropColumn("dbo.RentalAssets", "companyId");
            DropColumn("dbo.Notifications", "FlagId");
            DropColumn("dbo.Notifications", "CcUserId");
            DropColumn("dbo.FieldValues", "isExportLicense");
            DropTable("dbo.TenancyContracts");
            DropTable("dbo.NotificationFlags");
            CreateIndex("dbo.RentalAssets", "person_Id");
            CreateIndex("dbo.Notifications", "UserId");
            AddForeignKey("dbo.Notifications", "UserId", "dbo.Users", "id", cascadeDelete: true);
            AddForeignKey("dbo.RentalAssets", "person_Id", "dbo.tabPerson", "Id");
        }
    }
}
