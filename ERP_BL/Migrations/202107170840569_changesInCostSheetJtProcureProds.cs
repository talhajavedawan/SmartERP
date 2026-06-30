namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCostSheetJtProcureProds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheetFields", "creditCoaId", c => c.Int());
            AddColumn("dbo.CostSheetFields", "debitCoaId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "Bill_Id", c => c.Int());
            AddColumn("dbo.JournalTransactions", "costSheetFieldId", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "fieldId", c => c.Int());
            AddColumn("dbo.tabBackground", "EmployeeIds", c => c.String());
            CreateIndex("dbo.CostSheetFields", "creditCoaId");
            CreateIndex("dbo.CostSheetFields", "debitCoaId");
            CreateIndex("dbo.JournalTransactions", "Bill_Id");
            CreateIndex("dbo.JournalTransactions", "costSheetFieldId");
            CreateIndex("dbo.ProcurementProducts", "fieldId");
            AddForeignKey("dbo.CostSheetFields", "creditCoaId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.CostSheetFields", "debitCoaId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.JournalTransactions", "Bill_Id", "dbo.Bills", "Id");
            AddForeignKey("dbo.JournalTransactions", "costSheetFieldId", "dbo.CostSheetFields", "Id");
            AddForeignKey("dbo.ProcurementProducts", "fieldId", "dbo.CostSheetFields", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcurementProducts", "fieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.JournalTransactions", "costSheetFieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.JournalTransactions", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.CostSheetFields", "debitCoaId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CostSheetFields", "creditCoaId", "dbo.ChartofAccounts");
            DropIndex("dbo.ProcurementProducts", new[] { "fieldId" });
            DropIndex("dbo.JournalTransactions", new[] { "costSheetFieldId" });
            DropIndex("dbo.JournalTransactions", new[] { "Bill_Id" });
            DropIndex("dbo.CostSheetFields", new[] { "debitCoaId" });
            DropIndex("dbo.CostSheetFields", new[] { "creditCoaId" });
            DropColumn("dbo.tabBackground", "EmployeeIds");
            DropColumn("dbo.ProcurementProducts", "fieldId");
            DropColumn("dbo.JournalTransactions", "costSheetFieldId");
            DropColumn("dbo.JournalTransactions", "Bill_Id");
            DropColumn("dbo.CostSheetFields", "debitCoaId");
            DropColumn("dbo.CostSheetFields", "creditCoaId");
        }
    }
}
