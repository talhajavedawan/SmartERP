namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisabilityFlagsAddedInDfferentTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminBillStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProcurementProducts", "ReceivedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "ReceivedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransferStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.STLStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.InquiryStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseInvoiceStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoiceStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleOrderStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesReceiptStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.OfferStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrderStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.TargetRewardStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.ToDoTaskStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoansAdvanceStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.BillStatus", "isDisable", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoansStatus", "isDisable", c => c.Boolean(nullable: false));
            DropColumn("dbo.ProcurementProducts", "DispatchedQuantity");
            DropColumn("dbo.ProcurementProducts", "DispatchedWeight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProcurementProducts", "DispatchedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "DispatchedQuantity", c => c.Double(nullable: false));
            DropColumn("dbo.LoansStatus", "isDisable");
            DropColumn("dbo.BillStatus", "isDisable");
            DropColumn("dbo.LoansAdvanceStatus", "isDisable");
            DropColumn("dbo.ToDoTaskStatus", "isDisable");
            DropColumn("dbo.TargetRewardStatus", "isDisable");
            DropColumn("dbo.PaymentStatus", "isDisable");
            DropColumn("dbo.PurchaseOrderStatus", "isDisable");
            DropColumn("dbo.OfferStatus", "isDisable");
            DropColumn("dbo.SalesReceiptStatus", "isDisable");
            DropColumn("dbo.SaleOrderStatus", "isDisable");
            DropColumn("dbo.SaleInvoiceStatus", "isDisable");
            DropColumn("dbo.PurchaseInvoiceStatus", "isDisable");
            DropColumn("dbo.InquiryStatus", "isDisable");
            DropColumn("dbo.STLStatus", "isDisable");
            DropColumn("dbo.InterBankTransferStatus", "isDisable");
            DropColumn("dbo.ProcurementProducts", "ReceivedWeight");
            DropColumn("dbo.ProcurementProducts", "ReceivedQuantity");
            DropColumn("dbo.AdminBillStatus", "isDisable");
        }
    }
}
