namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInReceipts : DbMigration
    {

        //*******************************************************//
        // keep this migration commented, as its only required  //
        // blank to match migration history in demo db.         //
        //******************************************************//

        public override void Up()
        {
            //AddColumn("dbo.PurchaseOrders", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.PurchaseOrders", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.PurchaseOrders", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.Bills", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.Bills", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.Bills", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.SaleOrders", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.SaleOrders", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.SaleOrders", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.Offers", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.Offers", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.Offers", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.Inquiries", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.Inquiries", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.Inquiries", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.SaleInvoices", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.SaleInvoices", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.SaleInvoices", "isInterCompany", c => c.Boolean());
            //AddColumn("dbo.MemorandumSales", "InterDepartment_Id", c => c.Int());
            //AddColumn("dbo.MemorandumSales", "InterCompany_Id", c => c.Int());
            //AddColumn("dbo.MemorandumSales", "isInterCompany", c => c.Boolean());
            //CreateIndex("dbo.Bills", "InterDepartment_Id");
            //CreateIndex("dbo.Bills", "InterCompany_Id");
            //CreateIndex("dbo.PurchaseOrders", "InterDepartment_Id");
            //CreateIndex("dbo.PurchaseOrders", "InterCompany_Id");
            //CreateIndex("dbo.SaleOrders", "InterDepartment_Id");
            //CreateIndex("dbo.SaleOrders", "InterCompany_Id");
            //CreateIndex("dbo.Offers", "InterDepartment_Id");
            //CreateIndex("dbo.Offers", "InterCompany_Id");
            //CreateIndex("dbo.Inquiries", "InterDepartment_Id");
            //CreateIndex("dbo.Inquiries", "InterCompany_Id");
            //CreateIndex("dbo.SaleInvoices", "InterDepartment_Id");
            //CreateIndex("dbo.SaleInvoices", "InterCompany_Id");
            //CreateIndex("dbo.MemorandumSales", "InterDepartment_Id");
            //CreateIndex("dbo.MemorandumSales", "InterCompany_Id");
            //AddForeignKey("dbo.Bills", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.Bills", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.Inquiries", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.MemorandumSales", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.Offers", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.PurchaseOrders", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.SaleInvoices", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.SaleOrders", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            //AddForeignKey("dbo.Inquiries", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.MemorandumSales", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.Offers", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.PurchaseOrders", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.SaleInvoices", "InterCompany_Id", "dbo.tabCompany", "Id");
            //AddForeignKey("dbo.SaleOrders", "InterCompany_Id", "dbo.tabCompany", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.SaleOrders", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.SaleInvoices", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.PurchaseOrders", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.Offers", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.MemorandumSales", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.Inquiries", "InterCompany_Id", "dbo.tabCompany");
            //DropForeignKey("dbo.SaleOrders", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.SaleInvoices", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.PurchaseOrders", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.Offers", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.MemorandumSales", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.Inquiries", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.Bills", "InterDepartment_Id", "dbo.tabDepartment");
            //DropForeignKey("dbo.Bills", "InterCompany_Id", "dbo.tabCompany");
            //DropIndex("dbo.MemorandumSales", new[] { "InterCompany_Id" });
            //DropIndex("dbo.MemorandumSales", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.SaleInvoices", new[] { "InterCompany_Id" });
            //DropIndex("dbo.SaleInvoices", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.Inquiries", new[] { "InterCompany_Id" });
            //DropIndex("dbo.Inquiries", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.Offers", new[] { "InterCompany_Id" });
            //DropIndex("dbo.Offers", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.SaleOrders", new[] { "InterCompany_Id" });
            //DropIndex("dbo.SaleOrders", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.PurchaseOrders", new[] { "InterCompany_Id" });
            //DropIndex("dbo.PurchaseOrders", new[] { "InterDepartment_Id" });
            //DropIndex("dbo.Bills", new[] { "InterCompany_Id" });
            //DropIndex("dbo.Bills", new[] { "InterDepartment_Id" });
            //DropColumn("dbo.MemorandumSales", "isInterCompany");
            //DropColumn("dbo.MemorandumSales", "InterCompany_Id");
            //DropColumn("dbo.MemorandumSales", "InterDepartment_Id");
            //DropColumn("dbo.SaleInvoices", "isInterCompany");
            //DropColumn("dbo.SaleInvoices", "InterCompany_Id");
            //DropColumn("dbo.SaleInvoices", "InterDepartment_Id");
            //DropColumn("dbo.Inquiries", "isInterCompany");
            //DropColumn("dbo.Inquiries", "InterCompany_Id");
            //DropColumn("dbo.Inquiries", "InterDepartment_Id");
            //DropColumn("dbo.Offers", "isInterCompany");
            //DropColumn("dbo.Offers", "InterCompany_Id");
            //DropColumn("dbo.Offers", "InterDepartment_Id");
            //DropColumn("dbo.SaleOrders", "isInterCompany");
            //DropColumn("dbo.SaleOrders", "InterCompany_Id");
            //DropColumn("dbo.SaleOrders", "InterDepartment_Id");
            //DropColumn("dbo.Bills", "isInterCompany");
            //DropColumn("dbo.Bills", "InterCompany_Id");
            //DropColumn("dbo.Bills", "InterDepartment_Id");
            //DropColumn("dbo.PurchaseOrders", "isInterCompany");
            //DropColumn("dbo.PurchaseOrders", "InterCompany_Id");
            //DropColumn("dbo.PurchaseOrders", "InterDepartment_Id");
        }
    }
}
