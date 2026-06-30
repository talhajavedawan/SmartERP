namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class designation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Designations", "Title", c => c.String());
            AddColumn("dbo.Designations", "ParentId", c => c.Int());
            AddColumn("dbo.Designations", "companyId", c => c.Int());
            AddColumn("dbo.Designations", "departmentId", c => c.Int());
            AddColumn("dbo.Designations", "userId", c => c.Int());
            AddColumn("dbo.Designations", "parentDesignation_DesigId", c => c.Int());
            CreateIndex("dbo.Designations", "companyId");
            CreateIndex("dbo.Designations", "departmentId");
            CreateIndex("dbo.Designations", "userId");
            CreateIndex("dbo.Designations", "parentDesignation_DesigId");
            AddForeignKey("dbo.Designations", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.Designations", "departmentId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Designations", "parentDesignation_DesigId", "dbo.Designations", "DesigId");
            AddForeignKey("dbo.Designations", "userId", "dbo.Users", "id");
            DropColumn("dbo.Designations", "Desig");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Designations", "Desig", c => c.String());
            DropForeignKey("dbo.Designations", "userId", "dbo.Users");
            DropForeignKey("dbo.Designations", "parentDesignation_DesigId", "dbo.Designations");
            DropForeignKey("dbo.Designations", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.Designations", "companyId", "dbo.tabCompany");
            DropIndex("dbo.Designations", new[] { "parentDesignation_DesigId" });
            DropIndex("dbo.Designations", new[] { "userId" });
            DropIndex("dbo.Designations", new[] { "departmentId" });
            DropIndex("dbo.Designations", new[] { "companyId" });
            DropColumn("dbo.Designations", "parentDesignation_DesigId");
            DropColumn("dbo.Designations", "userId");
            DropColumn("dbo.Designations", "departmentId");
            DropColumn("dbo.Designations", "companyId");
            DropColumn("dbo.Designations", "ParentId");
            DropColumn("dbo.Designations", "Title");
        }
    }
}
