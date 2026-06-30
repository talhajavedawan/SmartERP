namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagnesInReconClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reconcilations", "chartofAccountId", c => c.Int());
            AddColumn("dbo.Reconcilations", "reconcilationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reconcilations", "reconcilationAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.Reconcilations", "chartofAccountId");
            AddForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            DropColumn("dbo.Reconcilations", "reconcilationfromDate");
            DropColumn("dbo.Reconcilations", "reconcilationToDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reconcilations", "reconcilationToDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reconcilations", "reconcilationfromDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            DropColumn("dbo.Reconcilations", "reconcilationAmount");
            DropColumn("dbo.Reconcilations", "reconcilationDate");
            DropColumn("dbo.Reconcilations", "chartofAccountId");
        }
    }
}
