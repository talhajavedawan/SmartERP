namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInDocumentsAndTargetModules : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocumentTypes", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropIndex("dbo.DocumentTypes", new[] { "documentTemplate_Id" });
            CreateTable(
                "dbo.DocumentTemplateDocumentTypes",
                c => new
                    {
                        DocumentTemplate_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentTemplate_Id, t.DocumentType_Id })
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.DocumentTemplate_Id)
                .Index(t => t.DocumentType_Id);
            
            CreateTable(
                "dbo.DepartmentDocumentTemplates",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        DocumentTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.DocumentTemplate_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.DocumentTemplate_Id);
            
            CreateTable(
                "dbo.CompanyDocumentTemplates",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        DocumentTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.DocumentTemplate_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.DocumentTemplate_Id);
            
            AddColumn("dbo.ToDoTasks", "EstimatedGrossProfitSE", c => c.Double(nullable: false));
            DropColumn("dbo.DocumentTypes", "documentTemplate_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentTypes", "documentTemplate_Id", c => c.Int());
            DropForeignKey("dbo.CompanyDocumentTemplates", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.CompanyDocumentTemplates", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentDocumentTemplates", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.DepartmentDocumentTemplates", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DocumentTemplateDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTemplateDocumentTypes", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropIndex("dbo.CompanyDocumentTemplates", new[] { "DocumentTemplate_Id" });
            DropIndex("dbo.CompanyDocumentTemplates", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentDocumentTemplates", new[] { "DocumentTemplate_Id" });
            DropIndex("dbo.DepartmentDocumentTemplates", new[] { "Department_Id" });
            DropIndex("dbo.DocumentTemplateDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.DocumentTemplateDocumentTypes", new[] { "DocumentTemplate_Id" });
            DropColumn("dbo.ToDoTasks", "EstimatedGrossProfitSE");
            DropTable("dbo.CompanyDocumentTemplates");
            DropTable("dbo.DepartmentDocumentTemplates");
            DropTable("dbo.DocumentTemplateDocumentTypes");
            CreateIndex("dbo.DocumentTypes", "documentTemplate_Id");
            AddForeignKey("dbo.DocumentTypes", "documentTemplate_Id", "dbo.DocumentTemplates", "Id");
        }
    }
}
