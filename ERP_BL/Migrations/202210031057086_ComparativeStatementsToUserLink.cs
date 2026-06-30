namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComparativeStatementsToUserLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComparativeStatements", "creatorId", c => c.Int());
            CreateIndex("dbo.ComparativeStatements", "creatorId");
            AddForeignKey("dbo.ComparativeStatements", "creatorId", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComparativeStatements", "creatorId", "dbo.Users");
            DropIndex("dbo.ComparativeStatements", new[] { "creatorId" });
            DropColumn("dbo.ComparativeStatements", "creatorId");
        }
    }
}
