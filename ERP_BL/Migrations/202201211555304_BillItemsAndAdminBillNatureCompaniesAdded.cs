namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillItemsAndAdminBillNatureCompaniesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CostSheetFields", "creditCoaId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CostSheetFields", "debitCoaId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.AdminBillNatures", "companyId", "dbo.tabCompany");
            DropIndex("dbo.CostSheetFields", new[] { "creditCoaId" });
            DropIndex("dbo.CostSheetFields", new[] { "debitCoaId" });
            DropIndex("dbo.AdminBillNatures", new[] { "companyId" });
            CreateTable(
                "dbo.BillItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountSOC = c.Double(nullable: false),
                        fieldId = c.Int(),
                        creditAccountId = c.Int(),
                        debitAccountId = c.Int(),
                        Bill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheetFields", t => t.fieldId)
                .ForeignKey("dbo.ChartofAccounts", t => t.creditAccountId)
                .ForeignKey("dbo.ChartofAccounts", t => t.debitAccountId)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .Index(t => t.fieldId)
                .Index(t => t.creditAccountId)
                .Index(t => t.debitAccountId)
                .Index(t => t.Bill_Id);
            
            CreateTable(
                "dbo.AdminBillNatureCompanies",
                c => new
                    {
                        AdminBillNature_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillNature_Id, t.Company_Id })
                .ForeignKey("dbo.AdminBillNatures", t => t.AdminBillNature_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.AdminBillNature_Id)
                .Index(t => t.Company_Id);
            
            AddColumn("dbo.ProcurementProducts", "creditAccountId", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "debitAccountId", c => c.Int());
            CreateIndex("dbo.ProcurementProducts", "creditAccountId");
            CreateIndex("dbo.ProcurementProducts", "debitAccountId");
            AddForeignKey("dbo.ProcurementProducts", "creditAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.ProcurementProducts", "debitAccountId", "dbo.ChartofAccounts", "Id");
            DropColumn("dbo.CostSheetFields", "creditCoaId");
            DropColumn("dbo.CostSheetFields", "debitCoaId");
            DropColumn("dbo.AdminBillNatures", "companyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdminBillNatures", "companyId", c => c.Int());
            AddColumn("dbo.CostSheetFields", "debitCoaId", c => c.Int());
            AddColumn("dbo.CostSheetFields", "creditCoaId", c => c.Int());
            DropForeignKey("dbo.BillItems", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillItems", "debitAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BillItems", "creditAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BillItems", "fieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.ProcurementProducts", "debitAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ProcurementProducts", "creditAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.AdminBillNatureCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.AdminBillNatureCompanies", "AdminBillNature_Id", "dbo.AdminBillNatures");
            DropIndex("dbo.AdminBillNatureCompanies", new[] { "Company_Id" });
            DropIndex("dbo.AdminBillNatureCompanies", new[] { "AdminBillNature_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "debitAccountId" });
            DropIndex("dbo.ProcurementProducts", new[] { "creditAccountId" });
            DropIndex("dbo.BillItems", new[] { "Bill_Id" });
            DropIndex("dbo.BillItems", new[] { "debitAccountId" });
            DropIndex("dbo.BillItems", new[] { "creditAccountId" });
            DropIndex("dbo.BillItems", new[] { "fieldId" });
            DropColumn("dbo.ProcurementProducts", "debitAccountId");
            DropColumn("dbo.ProcurementProducts", "creditAccountId");
            DropTable("dbo.AdminBillNatureCompanies");
            DropTable("dbo.BillItems");
            CreateIndex("dbo.AdminBillNatures", "companyId");
            CreateIndex("dbo.CostSheetFields", "debitCoaId");
            CreateIndex("dbo.CostSheetFields", "creditCoaId");
            AddForeignKey("dbo.AdminBillNatures", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.CostSheetFields", "debitCoaId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.CostSheetFields", "creditCoaId", "dbo.ChartofAccounts", "Id");
        }
    }
}
