namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalContractStatusAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesReceipts", "customerCreditId", "dbo.CustomerCredits");
            DropIndex("dbo.SalesReceipts", new[] { "customerCreditId" });
            CreateTable(
                "dbo.RentalContractStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalContractStatusStatusClasses",
                c => new
                    {
                        RentalContractStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalContractStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalContractStatus", t => t.RentalContractStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalContractStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            AddColumn("dbo.CustomerCredits", "SerialNo", c => c.Int(nullable: false));
            AddColumn("dbo.SalesReceipts", "CustomerCreditSerialNo", c => c.Int(nullable: false));
            AddColumn("dbo.RentalContracts", "statusId", c => c.Int());
            AddColumn("dbo.RentalPeriodDetails", "isActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RentalContracts", "statusId");
            AddForeignKey("dbo.RentalContracts", "statusId", "dbo.RentalContractStatus", "Id");
            DropColumn("dbo.SalesReceipts", "customerCreditId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesReceipts", "customerCreditId", c => c.Int());
            DropForeignKey("dbo.RentalContractStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalContractStatusStatusClasses", "RentalContractStatus_Id", "dbo.RentalContractStatus");
            DropForeignKey("dbo.RentalContracts", "statusId", "dbo.RentalContractStatus");
            DropIndex("dbo.RentalContractStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalContractStatusStatusClasses", new[] { "RentalContractStatus_Id" });
            DropIndex("dbo.RentalContracts", new[] { "statusId" });
            DropColumn("dbo.RentalPeriodDetails", "isActive");
            DropColumn("dbo.RentalContracts", "statusId");
            DropColumn("dbo.SalesReceipts", "CustomerCreditSerialNo");
            DropColumn("dbo.CustomerCredits", "SerialNo");
            DropTable("dbo.RentalContractStatusStatusClasses");
            DropTable("dbo.RentalContractStatus");
            CreateIndex("dbo.SalesReceipts", "customerCreditId");
            AddForeignKey("dbo.SalesReceipts", "customerCreditId", "dbo.CustomerCredits", "Id");
        }
    }
}
