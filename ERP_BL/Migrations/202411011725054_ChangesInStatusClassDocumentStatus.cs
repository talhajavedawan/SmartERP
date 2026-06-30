namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInStatusClassDocumentStatus : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StatusClassDocumentStatus", newName: "DocumentStatusStatusClasses");
            DropPrimaryKey("dbo.DocumentStatusStatusClasses");
            AddPrimaryKey("dbo.DocumentStatusStatusClasses", new[] { "DocumentStatus_Id", "StatusClass_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DocumentStatusStatusClasses");
            AddPrimaryKey("dbo.DocumentStatusStatusClasses", new[] { "StatusClass_Id", "DocumentStatus_Id" });
            RenameTable(name: "dbo.DocumentStatusStatusClasses", newName: "StatusClassDocumentStatus");
        }
    }
}
