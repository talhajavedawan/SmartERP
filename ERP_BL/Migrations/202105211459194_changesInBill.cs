namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "hasTax", c => c.Boolean());
            AddColumn("dbo.Bills", "tax_Id", c => c.Int());
            AddColumn("dbo.Bills", "billWithTax", c => c.Double());
            AddColumn("dbo.Bills", "hasWHT", c => c.Boolean());
            AddColumn("dbo.Bills", "WHT_Id", c => c.Int());
            AddColumn("dbo.Bills", "billAfterTax", c => c.Double());
            CreateIndex("dbo.Bills", "tax_Id");
            CreateIndex("dbo.Bills", "WHT_Id");
            AddForeignKey("dbo.Bills", "tax_Id", "dbo.TaxNames", "Id");
            AddForeignKey("dbo.Bills", "WHT_Id", "dbo.TaxNames", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "WHT_Id", "dbo.TaxNames");
            DropForeignKey("dbo.Bills", "tax_Id", "dbo.TaxNames");
            DropIndex("dbo.Bills", new[] { "WHT_Id" });
            DropIndex("dbo.Bills", new[] { "tax_Id" });
            DropColumn("dbo.Bills", "billAfterTax");
            DropColumn("dbo.Bills", "WHT_Id");
            DropColumn("dbo.Bills", "hasWHT");
            DropColumn("dbo.Bills", "billWithTax");
            DropColumn("dbo.Bills", "tax_Id");
            DropColumn("dbo.Bills", "hasTax");
        }
    }
}
