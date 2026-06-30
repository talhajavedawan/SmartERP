namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleExpensesUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VehicleExpenses", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.VehicleExpenses", "SystemRefNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VehicleExpenses", "SystemRefNo");
            DropColumn("dbo.VehicleExpenses", "transactionGroupId");
        }
    }
}
