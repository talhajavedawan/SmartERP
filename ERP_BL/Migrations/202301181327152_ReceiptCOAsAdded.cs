namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReceiptCOAsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReceiptCOAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        COAcredit_Id = c.Int(),
                        COAdebit_Id = c.Int(),
                        SalesReceipt_Id = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COAcredit_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COAdebit_Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceipt_Id)
                .Index(t => t.COAcredit_Id)
                .Index(t => t.COAdebit_Id)
                .Index(t => t.SalesReceipt_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiptCOAs", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptCOAs", "COAdebit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ReceiptCOAs", "COAcredit_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.ReceiptCOAs", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.ReceiptCOAs", new[] { "COAdebit_Id" });
            DropIndex("dbo.ReceiptCOAs", new[] { "COAcredit_Id" });
            DropTable("dbo.ReceiptCOAs");
        }
    }
}
