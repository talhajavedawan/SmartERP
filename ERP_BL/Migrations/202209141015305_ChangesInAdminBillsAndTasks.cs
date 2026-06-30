namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAdminBillsAndTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "isAdjustedTax", c => c.Boolean());
            AddColumn("dbo.Tasks", "inquiryId", c => c.Int());
            AddColumn("dbo.Tasks", "offerId", c => c.Int());
            CreateIndex("dbo.Tasks", "inquiryId");
            CreateIndex("dbo.Tasks", "offerId");
            AddForeignKey("dbo.Tasks", "inquiryId", "dbo.Inquiries", "Id");
            AddForeignKey("dbo.Tasks", "offerId", "dbo.Offers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "offerId", "dbo.Offers");
            DropForeignKey("dbo.Tasks", "inquiryId", "dbo.Inquiries");
            DropIndex("dbo.Tasks", new[] { "offerId" });
            DropIndex("dbo.Tasks", new[] { "inquiryId" });
            DropColumn("dbo.Tasks", "offerId");
            DropColumn("dbo.Tasks", "inquiryId");
            DropColumn("dbo.tabAdminBill", "isAdjustedTax");
        }
    }
}
