namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIn_Bills_PettyCash_VATBooks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "PettyCashRefId", c => c.Int());
            AddColumn("dbo.Bills", "isDeposit", c => c.Boolean());
            AddColumn("dbo.PettyCashes", "billId", c => c.Int());
            AlterColumn("dbo.PettyCashes", "CreationDate", c => c.DateTime());
            AlterColumn("dbo.VATBooks", "CreationDate", c => c.DateTime());
            AlterColumn("dbo.VATBooks", "GLPostingDate", c => c.DateTime());
            CreateIndex("dbo.Bills", "PettyCashRefId");
            CreateIndex("dbo.PettyCashes", "billId");
            AddForeignKey("dbo.PettyCashes", "billId", "dbo.Bills", "Id");
            AddForeignKey("dbo.Bills", "PettyCashRefId", "dbo.BillRefNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "billId", "dbo.Bills");
            DropIndex("dbo.PettyCashes", new[] { "billId" });
            DropIndex("dbo.Bills", new[] { "PettyCashRefId" });
            AlterColumn("dbo.VATBooks", "GLPostingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.VATBooks", "CreationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PettyCashes", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.PettyCashes", "billId");
            DropColumn("dbo.Bills", "isDeposit");
            DropColumn("dbo.Bills", "PettyCashRefId");
        }
    }
}
