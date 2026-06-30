namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowOpenTransactionsAddedInEmployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "AllowOpenTransactions", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "AllowOpenTransactions");
        }
    }
}
