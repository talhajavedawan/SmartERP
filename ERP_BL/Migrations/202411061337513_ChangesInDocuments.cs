namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInDocuments : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "allocation_Id", newName: "employee_Id");
            RenameIndex(table: "dbo.Documents", name: "IX_allocation_Id", newName: "IX_employee_Id");
            AddColumn("dbo.Documents", "CreationDate", c => c.DateTime());
            AddColumn("dbo.Documents", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "SystemRefNo", c => c.String());
            AddColumn("dbo.Documents", "Employee", c => c.String());
            AddColumn("dbo.Documents", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.Documents", "LastStatusChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "LastStatusChangeDate");
            DropColumn("dbo.Documents", "ClosingDate");
            DropColumn("dbo.Documents", "Employee");
            DropColumn("dbo.Documents", "SystemRefNo");
            DropColumn("dbo.Documents", "transactionGroupId");
            DropColumn("dbo.Documents", "CreationDate");
            RenameIndex(table: "dbo.Documents", name: "IX_employee_Id", newName: "IX_allocation_Id");
            RenameColumn(table: "dbo.Documents", name: "employee_Id", newName: "allocation_Id");
        }
    }
}
