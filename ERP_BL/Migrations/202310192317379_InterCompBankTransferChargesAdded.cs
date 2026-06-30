namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterCompBankTransferChargesAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.STLs", name: "cashMarginAccount_Id", newName: "cashMarginDrAccount_Id");
            RenameIndex(table: "dbo.STLs", name: "IX_cashMarginAccount_Id", newName: "IX_cashMarginDrAccount_Id");
            CreateTable(
                "dbo.InterCompBankTransferCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_Id = c.Int(),
                        InterCompanyBankTransferFromId = c.Int(),
                        InterCompanyBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferFromId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferToId)
                .Index(t => t.deduction_Id)
                .Index(t => t.InterCompanyBankTransferFromId)
                .Index(t => t.InterCompanyBankTransferToId);
            
            CreateTable(
                "dbo.InterCompBankTransferVATs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        InterCompanyBankTransferFromId = c.Int(),
                        InterCompanyBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferFromId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferToId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.InterCompanyBankTransferFromId)
                .Index(t => t.InterCompanyBankTransferToId);
            
            AddColumn("dbo.InterCompanyBankTransfers", "IsAdjustedVATfrom", c => c.Boolean());
            AddColumn("dbo.InterCompanyBankTransfers", "IsAdjustedVATto", c => c.Boolean());
            AddColumn("dbo.STLs", "cashMarginCrAccount_Id", c => c.Int());
            AddForeignKey("dbo.STLs", "cashMarginDrAccount_Id", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.STLs", "cashMarginDrAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.InterCompBankTransferVATs", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.InterCompBankTransferVATs", "InterCompanyBankTransferToId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferVATs", "InterCompanyBankTransferFromId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferCharges", "InterCompanyBankTransferToId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferCharges", "InterCompanyBankTransferFromId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferCharges", "deduction_Id", "dbo.Deductions");
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "InterCompanyBankTransferToId" });
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "InterCompanyBankTransferFromId" });
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "taxNameId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "InterCompanyBankTransferToId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "InterCompanyBankTransferFromId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "deduction_Id" });
            DropColumn("dbo.STLs", "cashMarginCrAccount_Id");
            DropColumn("dbo.InterCompanyBankTransfers", "IsAdjustedVATto");
            DropColumn("dbo.InterCompanyBankTransfers", "IsAdjustedVATfrom");
            DropTable("dbo.InterCompBankTransferVATs");
            DropTable("dbo.InterCompBankTransferCharges");
            RenameIndex(table: "dbo.STLs", name: "IX_cashMarginDrAccount_Id", newName: "IX_cashMarginAccount_Id");
            RenameColumn(table: "dbo.STLs", name: "cashMarginDrAccount_Id", newName: "cashMarginAccount_Id");
        }
    }
}
