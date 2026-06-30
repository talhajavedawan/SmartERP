namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferandSOtoMemosale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MemorandumSales", "Offer_Id", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemorandumSales", "Offer_Id", "dbo.Offers");
        }
    }
}
