namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAccountsAndSalesOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "mainBankId", c => c.Int());
            AddColumn("dbo.SaleOrders", "interBankTransfer_Id", c => c.Int());
            CreateIndex("dbo.Accounts", "mainBankId");
            CreateIndex("dbo.SaleOrders", "interBankTransfer_Id");
            AddForeignKey("dbo.SaleOrders", "interBankTransfer_Id", "dbo.InterBankTransfers", "Id");
            AddForeignKey("dbo.Accounts", "mainBankId", "dbo.MainBanks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "mainBankId", "dbo.MainBanks");
            DropForeignKey("dbo.SaleOrders", "interBankTransfer_Id", "dbo.InterBankTransfers");
            DropIndex("dbo.SaleOrders", new[] { "interBankTransfer_Id" });
            DropIndex("dbo.Accounts", new[] { "mainBankId" });
            DropColumn("dbo.SaleOrders", "interBankTransfer_Id");
            DropColumn("dbo.Accounts", "mainBankId");
        }
    }
}
