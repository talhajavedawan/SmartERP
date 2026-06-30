namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInInterBankTransfer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterBankTransfers", "currencyFromId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "AmountFrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "MERfrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "AmountMERfrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "currencyToId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "AmountTo", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "MERto", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "AmountMERto", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "AmountER", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "Description", c => c.String());
            CreateIndex("dbo.InterBankTransfers", "currencyFromId");
            CreateIndex("dbo.InterBankTransfers", "currencyToId");
            AddForeignKey("dbo.InterBankTransfers", "currencyFromId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.InterBankTransfers", "currencyToId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransfers", "currencyToId", "dbo.Currencies");
            DropForeignKey("dbo.InterBankTransfers", "currencyFromId", "dbo.Currencies");
            DropIndex("dbo.InterBankTransfers", new[] { "currencyToId" });
            DropIndex("dbo.InterBankTransfers", new[] { "currencyFromId" });
            DropColumn("dbo.InterBankTransfers", "Description");
            DropColumn("dbo.InterBankTransfers", "AmountER");
            DropColumn("dbo.InterBankTransfers", "AmountMERto");
            DropColumn("dbo.InterBankTransfers", "MERto");
            DropColumn("dbo.InterBankTransfers", "AmountTo");
            DropColumn("dbo.InterBankTransfers", "currencyToId");
            DropColumn("dbo.InterBankTransfers", "AmountMERfrom");
            DropColumn("dbo.InterBankTransfers", "MERfrom");
            DropColumn("dbo.InterBankTransfers", "AmountFrom");
            DropColumn("dbo.InterBankTransfers", "currencyFromId");
        }
    }
}
