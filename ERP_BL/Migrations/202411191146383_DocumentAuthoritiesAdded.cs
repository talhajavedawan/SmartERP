namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentAuthoritiesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentAuthorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorityName = c.String(),
                        documentTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTemplates", t => t.documentTemplate_Id)
                .Index(t => t.documentTemplate_Id);
            
            AddColumn("dbo.Documents", "documentAuthority_Id", c => c.Int());
            CreateIndex("dbo.Documents", "documentAuthority_Id");
            AddForeignKey("dbo.Documents", "documentAuthority_Id", "dbo.DocumentAuthorities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "documentAuthority_Id", "dbo.DocumentAuthorities");
            DropForeignKey("dbo.DocumentAuthorities", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropIndex("dbo.DocumentAuthorities", new[] { "documentTemplate_Id" });
            DropIndex("dbo.Documents", new[] { "documentAuthority_Id" });
            DropColumn("dbo.Documents", "documentAuthority_Id");
            DropTable("dbo.DocumentAuthorities");
        }
    }
}
