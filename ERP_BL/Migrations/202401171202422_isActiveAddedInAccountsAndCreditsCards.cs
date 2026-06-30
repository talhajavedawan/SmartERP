namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isActiveAddedInAccountsAndCreditsCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CreditCards", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditCards", "isActive");
            DropColumn("dbo.Accounts", "isActive");
        }
    }
}
