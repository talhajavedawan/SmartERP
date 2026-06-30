namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorBillReferencesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorBillReferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        companyId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            AddColumn("dbo.Bills", "BillRefNoId", c => c.Int());
            CreateIndex("dbo.Bills", "BillRefNoId");
            AddForeignKey("dbo.Bills", "BillRefNoId", "dbo.VendorBillReferences", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "BillRefNoId", "dbo.VendorBillReferences");
            DropForeignKey("dbo.VendorBillReferences", "companyId", "dbo.tabCompany");
            DropIndex("dbo.VendorBillReferences", new[] { "companyId" });
            DropIndex("dbo.Bills", new[] { "BillRefNoId" });
            DropColumn("dbo.Bills", "BillRefNoId");
            DropTable("dbo.VendorBillReferences");
        }
    }
}
