namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentGroupIdAddedInInterBankTransfers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterBankTransfers", "paymentGroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterBankTransfers", "paymentGroupId");
        }
    }
}
