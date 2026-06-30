namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PQDocumentsTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PQDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PQDocuments");
        }
    }
}
