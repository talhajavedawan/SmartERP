namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterComapnyTransAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.InterCompanyBankTransfers", name: "interBankTransStatus_Id", newName: "StatusId");
            RenameIndex(table: "dbo.InterCompanyBankTransfers", name: "IX_interBankTransStatus_Id", newName: "IX_StatusId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.InterCompanyBankTransfers", name: "IX_StatusId", newName: "IX_interBankTransStatus_Id");
            RenameColumn(table: "dbo.InterCompanyBankTransfers", name: "StatusId", newName: "interBankTransStatus_Id");
        }
    }
}
