namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemorandumSales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemorandumSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        CustomerReferenceNo = c.String(),
                        PrincipleReferenceNo = c.String(),
                        CustomerReferenceDate = c.DateTime(),
                        PrincipleReferenceDate = c.DateTime(),
                        ExpectedClosingDate = c.DateTime(),
                        memorandumSalesDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        InvoiceDate = c.DateTime(),
                        ApplySaleRegister = c.Boolean(nullable: false),
                        exchangeRate = c.Single(nullable: false),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        SaleOrder_Id = c.Int(),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        memorandumSaleStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.MemorandumSaleStatus", t => t.memorandumSaleStatus_Id)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.SaleOrder_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.memorandumSaleStatus_Id);
            
            CreateTable(
                "dbo.MemorandumSaleStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Offers", "TotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Offers", "TotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Inquiries", "TotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Inquiries", "TotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ProcurementProducts", "MemorandumSale_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "RemainingBaseFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "RemainingBaseCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "ReceivedAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "TotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "TotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.ProcurementProducts", "MemorandumSale_Id");
            AddForeignKey("dbo.ProcurementProducts", "MemorandumSale_Id", "dbo.MemorandumSales", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemorandumSales", "user_Id", "dbo.Users");
            DropForeignKey("dbo.MemorandumSales", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.MemorandumSales", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.MemorandumSales", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.ProcurementProducts", "MemorandumSale_Id", "dbo.MemorandumSales");
            DropForeignKey("dbo.MemorandumSales", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.MemorandumSales", "memorandumSaleStatus_Id", "dbo.MemorandumSaleStatus");
            DropForeignKey("dbo.MemorandumSales", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.MemorandumSales", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.MemorandumSales", "company_Id", "dbo.tabCompany");
            DropIndex("dbo.MemorandumSales", new[] { "memorandumSaleStatus_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "TitleValue2Id" });
            DropIndex("dbo.MemorandumSales", new[] { "TitleValue1Id" });
            DropIndex("dbo.MemorandumSales", new[] { "SaleOrder_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "company_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "principal_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "user_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "dept_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "customerCompany_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "MemorandumSale_Id" });
            DropColumn("dbo.SaleOrders", "TotalQuantity");
            DropColumn("dbo.SaleOrders", "TotalWeight");
            DropColumn("dbo.SaleOrders", "ReceivedAmount");
            DropColumn("dbo.SaleOrders", "RemainingBaseCFRValue");
            DropColumn("dbo.SaleOrders", "RemainingBaseFOBValue");
            DropColumn("dbo.ProcurementProducts", "MemorandumSale_Id");
            DropColumn("dbo.Inquiries", "TotalQuantity");
            DropColumn("dbo.Inquiries", "TotalWeight");
            DropColumn("dbo.Offers", "TotalQuantity");
            DropColumn("dbo.Offers", "TotalWeight");
            DropTable("dbo.MemorandumSaleStatus");
            DropTable("dbo.MemorandumSales");
        }
    }
}
