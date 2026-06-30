namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class receiptDeductionStructureChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        isActive = c.Boolean(nullable: false),
                        receiptType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ReceiptDeductions", "deduction_Id", c => c.Int());
            CreateIndex("dbo.ReceiptDeductions", "deduction_Id");
            AddForeignKey("dbo.ReceiptDeductions", "deduction_Id", "dbo.Deductions", "Id");
            DropColumn("dbo.ReceiptDeductions", "title");
            DropColumn("dbo.ReceiptDeductions", "receiptType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiptDeductions", "receiptType", c => c.Int(nullable: false));
            AddColumn("dbo.ReceiptDeductions", "title", c => c.String());
            DropForeignKey("dbo.ReceiptDeductions", "deduction_Id", "dbo.Deductions");
            DropIndex("dbo.ReceiptDeductions", new[] { "deduction_Id" });
            DropColumn("dbo.ReceiptDeductions", "deduction_Id");
            DropTable("dbo.Deductions");
        }
    }
}
