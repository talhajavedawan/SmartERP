namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminBillNaturesAddedAndchangesInJornalTransactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminBillNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nature = c.String(),
                        companyId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            AddColumn("dbo.JournalTransactions", "MER", c => c.Double(nullable: false));
            AddColumn("dbo.JournalTransactions", "total", c => c.Double(nullable: false));
            AddColumn("dbo.tabAdminBill", "adminBillNature_Id", c => c.Int());
            CreateIndex("dbo.tabAdminBill", "adminBillNature_Id");
            AddForeignKey("dbo.tabAdminBill", "adminBillNature_Id", "dbo.AdminBillNatures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabAdminBill", "adminBillNature_Id", "dbo.AdminBillNatures");
            DropForeignKey("dbo.AdminBillNatures", "companyId", "dbo.tabCompany");
            DropIndex("dbo.AdminBillNatures", new[] { "companyId" });
            DropIndex("dbo.tabAdminBill", new[] { "adminBillNature_Id" });
            DropColumn("dbo.tabAdminBill", "adminBillNature_Id");
            DropColumn("dbo.JournalTransactions", "total");
            DropColumn("dbo.JournalTransactions", "MER");
            DropTable("dbo.AdminBillNatures");
        }
    }
}
