namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IBTbankChargesAndInterBankTransferVATsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IBTbankCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_Id = c.Int(),
                        InterBankTransferFromId = c.Int(),
                        InterBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_Id)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferFromId)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferToId)
                .Index(t => t.deduction_Id)
                .Index(t => t.InterBankTransferFromId)
                .Index(t => t.InterBankTransferToId);
            
            CreateTable(
                "dbo.InterBankTransferVATs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        InterBankTransferFromId = c.Int(),
                        InterBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferFromId)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferToId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.InterBankTransferFromId)
                .Index(t => t.InterBankTransferToId);
            
            AddColumn("dbo.InterBankTransfers", "IsAdjustedVATfrom", c => c.Boolean());
            AddColumn("dbo.InterBankTransfers", "IsAdjustedVATto", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransferVATs", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.InterBankTransferVATs", "InterBankTransferToId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransferVATs", "InterBankTransferFromId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.IBTbankCharges", "InterBankTransferToId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.IBTbankCharges", "InterBankTransferFromId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.IBTbankCharges", "deduction_Id", "dbo.Deductions");
            DropIndex("dbo.InterBankTransferVATs", new[] { "InterBankTransferToId" });
            DropIndex("dbo.InterBankTransferVATs", new[] { "InterBankTransferFromId" });
            DropIndex("dbo.InterBankTransferVATs", new[] { "taxNameId" });
            DropIndex("dbo.IBTbankCharges", new[] { "InterBankTransferToId" });
            DropIndex("dbo.IBTbankCharges", new[] { "InterBankTransferFromId" });
            DropIndex("dbo.IBTbankCharges", new[] { "deduction_Id" });
            DropColumn("dbo.InterBankTransfers", "IsAdjustedVATto");
            DropColumn("dbo.InterBankTransfers", "IsAdjustedVATfrom");
            DropTable("dbo.InterBankTransferVATs");
            DropTable("dbo.IBTbankCharges");
        }
    }
}
