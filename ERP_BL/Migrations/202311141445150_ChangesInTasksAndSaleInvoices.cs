namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTasksAndSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "TrackingNoIn", c => c.String());
            AddColumn("dbo.Tasks", "ETDin", c => c.DateTime());
            AddColumn("dbo.Tasks", "ETAin", c => c.DateTime());
            AddColumn("dbo.Tasks", "ADDin", c => c.DateTime());
            AddColumn("dbo.Tasks", "TrackingNoOut", c => c.String());
            AddColumn("dbo.Tasks", "ETDout", c => c.DateTime());
            AddColumn("dbo.Tasks", "ETAout", c => c.DateTime());
            AddColumn("dbo.Tasks", "ADDout", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "stlSTLCurrency_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "stlAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.SaleInvoices", "stlSTLCurrency_Id");
            AddForeignKey("dbo.SaleInvoices", "stlSTLCurrency_Id", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleInvoices", "stlSTLCurrency_Id", "dbo.Currencies");
            DropIndex("dbo.SaleInvoices", new[] { "stlSTLCurrency_Id" });
            DropColumn("dbo.SaleInvoices", "stlAmount");
            DropColumn("dbo.SaleInvoices", "stlSTLCurrency_Id");
            DropColumn("dbo.Tasks", "ADDout");
            DropColumn("dbo.Tasks", "ETAout");
            DropColumn("dbo.Tasks", "ETDout");
            DropColumn("dbo.Tasks", "TrackingNoOut");
            DropColumn("dbo.Tasks", "ADDin");
            DropColumn("dbo.Tasks", "ETAin");
            DropColumn("dbo.Tasks", "ETDin");
            DropColumn("dbo.Tasks", "TrackingNoIn");
        }
    }
}
