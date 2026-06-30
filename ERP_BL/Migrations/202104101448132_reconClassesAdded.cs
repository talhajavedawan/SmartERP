namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reconClassesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reconcilations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        reconcilationfromDate = c.DateTime(nullable: false),
                        reconcilationToDate = c.DateTime(nullable: false),
                        chartofAccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
            AddColumn("dbo.JournalTransactions", "Reconcilation_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "FinanceRefNo2", c => c.String());
            CreateIndex("dbo.JournalTransactions", "Reconcilation_Id");
            AddForeignKey("dbo.JournalTransactions", "Reconcilation_Id", "dbo.Reconcilations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalTransactions", "Reconcilation_Id", "dbo.Reconcilations");
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            DropIndex("dbo.JournalTransactions", new[] { "Reconcilation_Id" });
            DropColumn("dbo.tabAdminBill", "FinanceRefNo2");
            DropColumn("dbo.JournalTransactions", "Reconcilation_Id");
            DropTable("dbo.Reconcilations");
        }
    }
}
