namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentDocumentTypesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentDocumentTypes",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.DocumentType_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.DocumentType_Id);
            
            CreateTable(
                "dbo.CompanyDocumentTypes",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.DocumentType_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.DocumentType_Id);
            
            AddColumn("dbo.PaymentStatus", "isPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleOrdeRrefKeys", "SaleOrderNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.CompanyDocumentTypes", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.DepartmentDocumentTypes", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.CompanyDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.CompanyDocumentTypes", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.DepartmentDocumentTypes", new[] { "Department_Id" });
            DropColumn("dbo.SaleOrdeRrefKeys", "SaleOrderNumber");
            DropColumn("dbo.PaymentStatus", "isPaid");
            DropTable("dbo.CompanyDocumentTypes");
            DropTable("dbo.DepartmentDocumentTypes");
        }
    }
}
