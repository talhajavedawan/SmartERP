namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInEmployeesAndAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "coreCompanyId", c => c.Int());
            AddColumn("dbo.Employees", "coreDeptId", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "LoansAdvances", c => c.Int());
            CreateIndex("dbo.Employees", "coreCompanyId");
            CreateIndex("dbo.Employees", "coreDeptId");
            AddForeignKey("dbo.Employees", "coreDeptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Employees", "coreCompanyId", "dbo.tabCompany", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "coreCompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.Employees", "coreDeptId", "dbo.tabDepartment");
            DropIndex("dbo.Employees", new[] { "coreDeptId" });
            DropIndex("dbo.Employees", new[] { "coreCompanyId" });
            DropColumn("dbo.AttachmentCategories", "LoansAdvances");
            DropColumn("dbo.Employees", "coreDeptId");
            DropColumn("dbo.Employees", "coreCompanyId");
        }
    }
}
