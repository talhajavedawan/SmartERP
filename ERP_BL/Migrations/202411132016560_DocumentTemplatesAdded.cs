namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentTemplatesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Documents", "documentTemplate_Id", c => c.Int());
            AddColumn("dbo.Documents", "DocumentNo", c => c.String());
            AddColumn("dbo.DocumentTypes", "documentTemplate_Id", c => c.Int());
            CreateIndex("dbo.Documents", "documentTemplate_Id");
            CreateIndex("dbo.DocumentTypes", "documentTemplate_Id");
            AddForeignKey("dbo.Documents", "documentTemplate_Id", "dbo.DocumentTemplates", "Id");
            AddForeignKey("dbo.DocumentTypes", "documentTemplate_Id", "dbo.DocumentTemplates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentTypes", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.Documents", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropIndex("dbo.DocumentTypes", new[] { "documentTemplate_Id" });
            DropIndex("dbo.Documents", new[] { "documentTemplate_Id" });
            DropColumn("dbo.DocumentTypes", "documentTemplate_Id");
            DropColumn("dbo.Documents", "DocumentNo");
            DropColumn("dbo.Documents", "documentTemplate_Id");
            DropTable("dbo.DocumentTemplates");
        }
    }
}
