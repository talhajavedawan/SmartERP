namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInComparativeStatements20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComparativeStatements", "vendorName", c => c.String());
            AddColumn("dbo.ComparativeStatements", "productId", c => c.Int());
            AddColumn("dbo.ComparativeStatementItems", "ownDescription", c => c.String());
            CreateIndex("dbo.ComparativeStatements", "productId");
            AddForeignKey("dbo.ComparativeStatements", "productId", "dbo.ProcurementProducts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComparativeStatements", "productId", "dbo.ProcurementProducts");
            DropIndex("dbo.ComparativeStatements", new[] { "productId" });
            DropColumn("dbo.ComparativeStatementItems", "ownDescription");
            DropColumn("dbo.ComparativeStatements", "productId");
            DropColumn("dbo.ComparativeStatements", "vendorName");
        }
    }
}
