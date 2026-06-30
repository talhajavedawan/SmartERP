namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stIdChangesInInterBankTransfers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterBankTransfers", "stlId", c => c.Int());
            CreateIndex("dbo.InterBankTransfers", "stlId");
            AddForeignKey("dbo.InterBankTransfers", "stlId", "dbo.STLs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransfers", "stlId", "dbo.STLs");
            DropIndex("dbo.InterBankTransfers", new[] { "stlId" });
            DropColumn("dbo.InterBankTransfers", "stlId");
        }
    }
}
