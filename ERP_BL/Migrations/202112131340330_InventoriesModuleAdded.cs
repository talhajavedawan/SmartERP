namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoriesModuleAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(),
                        prodId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        Weight = c.Double(nullable: false),
                        Debit = c.Double(nullable: false),
                        Credit = c.Double(nullable: false),
                        UnitRate = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        transactionRefno = c.String(),
                        TransactionsType = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        currencyId = c.Int(),
                        userId = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        PurchaseInvoiceId = c.Int(),
                        SaleInviceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Products", t => t.prodId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoiceId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInviceId)
                .Index(t => t.prodId)
                .Index(t => t.currencyId)
                .Index(t => t.userId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.PurchaseInvoiceId)
                .Index(t => t.SaleInviceId);
            
            AddColumn("dbo.Products", "cgsInvenAccount_id", c => c.Int());
            AddColumn("dbo.Products", "cAssetAccount_id", c => c.Int());
            CreateIndex("dbo.Products", "cgsInvenAccount_id");
            CreateIndex("dbo.Products", "cAssetAccount_id");
            AddForeignKey("dbo.Products", "cAssetAccount_id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.Products", "cgsInvenAccount_id", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventories", "SaleInviceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.Inventories", "PurchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.Inventories", "prodId", "dbo.Products");
            DropForeignKey("dbo.Products", "cgsInvenAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Products", "cAssetAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Inventories", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Inventories", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Inventories", "userId", "dbo.Users");
            DropForeignKey("dbo.Inventories", "companyId", "dbo.tabCompany");
            DropIndex("dbo.Products", new[] { "cAssetAccount_id" });
            DropIndex("dbo.Products", new[] { "cgsInvenAccount_id" });
            DropIndex("dbo.Inventories", new[] { "SaleInviceId" });
            DropIndex("dbo.Inventories", new[] { "PurchaseInvoiceId" });
            DropIndex("dbo.Inventories", new[] { "deptId" });
            DropIndex("dbo.Inventories", new[] { "companyId" });
            DropIndex("dbo.Inventories", new[] { "userId" });
            DropIndex("dbo.Inventories", new[] { "currencyId" });
            DropIndex("dbo.Inventories", new[] { "prodId" });
            DropColumn("dbo.Products", "cAssetAccount_id");
            DropColumn("dbo.Products", "cgsInvenAccount_id");
            DropTable("dbo.Inventories");
        }
    }
}
