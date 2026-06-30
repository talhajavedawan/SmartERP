namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrincipalExchangeRate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "PER", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SoAmountPER", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "SoAmountCfrPER", c => c.Double(nullable: false));
            DropColumn("dbo.Targets", "AchivedJanuary");
            DropColumn("dbo.Targets", "AchivedFeburary");
            DropColumn("dbo.Targets", "AchivedMarch");
            DropColumn("dbo.Targets", "AchivedApril");
            DropColumn("dbo.Targets", "AchivedMay");
            DropColumn("dbo.Targets", "AchivedJune");
            DropColumn("dbo.Targets", "AchivedJuly");
            DropColumn("dbo.Targets", "AchivedAugust");
            DropColumn("dbo.Targets", "AchivedSeptember");
            DropColumn("dbo.Targets", "AchivedOctober");
            DropColumn("dbo.Targets", "AchivedNovember");
            DropColumn("dbo.Targets", "AchivedDecember");
            DropColumn("dbo.Targets", "AchivedExtra");
            DropColumn("dbo.Targets", "AchivedTotal");
            DropColumn("dbo.Targets", "Discriminator");
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.SaleOrders", "SoAmountCfrPER");
            DropColumn("dbo.SaleOrders", "SoAmountPER");
            DropColumn("dbo.SaleOrders", "PER");
        }
    }
}
