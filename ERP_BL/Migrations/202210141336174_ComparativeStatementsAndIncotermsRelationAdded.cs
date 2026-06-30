namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComparativeStatementsAndIncotermsRelationAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComparativeStatementItems", "itemIncoTermId", "dbo.Incoterms");
            DropIndex("dbo.ComparativeStatementItems", new[] { "itemIncoTermId" });
            AddColumn("dbo.ComparativeStatements", "vendorIncoTermId", c => c.Int());
            CreateIndex("dbo.ComparativeStatements", "vendorIncoTermId");
            AddForeignKey("dbo.ComparativeStatements", "vendorIncoTermId", "dbo.Incoterms", "Id");
            DropColumn("dbo.ComparativeStatementItems", "itemIncoTermId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComparativeStatementItems", "itemIncoTermId", c => c.Int());
            DropForeignKey("dbo.ComparativeStatements", "vendorIncoTermId", "dbo.Incoterms");
            DropIndex("dbo.ComparativeStatements", new[] { "vendorIncoTermId" });
            DropColumn("dbo.ComparativeStatements", "vendorIncoTermId");
            CreateIndex("dbo.ComparativeStatementItems", "itemIncoTermId");
            AddForeignKey("dbo.ComparativeStatementItems", "itemIncoTermId", "dbo.Incoterms", "Id");
        }
    }
}
