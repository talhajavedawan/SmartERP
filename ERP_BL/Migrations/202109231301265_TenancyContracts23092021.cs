namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TenancyContracts23092021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TenancyContracts", "CreationDate", c => c.DateTime());
            AddColumn("dbo.TenancyContracts", "rentalBasis", c => c.Int(nullable: false));
            AddColumn("dbo.TenancyContracts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.TenancyContracts", "companyId", c => c.Int());
            AddColumn("dbo.TenancyContracts", "deptId", c => c.Int());
            CreateIndex("dbo.TenancyContracts", "companyId");
            CreateIndex("dbo.TenancyContracts", "deptId");
            AddForeignKey("dbo.TenancyContracts", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.TenancyContracts", "deptId", "dbo.tabDepartment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TenancyContracts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TenancyContracts", "companyId", "dbo.tabCompany");
            DropIndex("dbo.TenancyContracts", new[] { "deptId" });
            DropIndex("dbo.TenancyContracts", new[] { "companyId" });
            DropColumn("dbo.TenancyContracts", "deptId");
            DropColumn("dbo.TenancyContracts", "companyId");
            DropColumn("dbo.TenancyContracts", "isActive");
            DropColumn("dbo.TenancyContracts", "rentalBasis");
            DropColumn("dbo.TenancyContracts", "CreationDate");
        }
    }
}
