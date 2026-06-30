namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInInquiry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcurementProducts", "AmountSOC", c => c.Double(nullable: false));
            AddColumn("dbo.InquiryStatus", "isVoid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InquiryStatus", "isVoid");
            DropColumn("dbo.ProcurementProducts", "AmountSOC");
        }
    }
}
