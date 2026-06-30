namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInInterCompanyBankTransfers10122021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterCompanyBankTransfers", "currencyFromId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "AmountFrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "MERfrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "AmountMERfrom", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "currencyToId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "AmountTo", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "MERto", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "AmountMERto", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "AmountER", c => c.Double(nullable: false));
            AddColumn("dbo.InterCompanyBankTransfers", "Description", c => c.String());
            CreateIndex("dbo.InterCompanyBankTransfers", "currencyFromId");
            CreateIndex("dbo.InterCompanyBankTransfers", "currencyToId");
            AddForeignKey("dbo.InterCompanyBankTransfers", "currencyFromId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "currencyToId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterCompanyBankTransfers", "currencyToId", "dbo.Currencies");
            DropForeignKey("dbo.InterCompanyBankTransfers", "currencyFromId", "dbo.Currencies");
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currencyToId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currencyFromId" });
            DropColumn("dbo.InterCompanyBankTransfers", "Description");
            DropColumn("dbo.InterCompanyBankTransfers", "AmountER");
            DropColumn("dbo.InterCompanyBankTransfers", "AmountMERto");
            DropColumn("dbo.InterCompanyBankTransfers", "MERto");
            DropColumn("dbo.InterCompanyBankTransfers", "AmountTo");
            DropColumn("dbo.InterCompanyBankTransfers", "currencyToId");
            DropColumn("dbo.InterCompanyBankTransfers", "AmountMERfrom");
            DropColumn("dbo.InterCompanyBankTransfers", "MERfrom");
            DropColumn("dbo.InterCompanyBankTransfers", "AmountFrom");
            DropColumn("dbo.InterCompanyBankTransfers", "currencyFromId");
        }
    }
}
