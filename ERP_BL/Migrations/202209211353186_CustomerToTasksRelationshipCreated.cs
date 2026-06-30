namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerToTasksRelationshipCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "customerId", c => c.Int());
            CreateIndex("dbo.Tasks", "customerId");
            AddForeignKey("dbo.Tasks", "customerId", "dbo.CustomerCompanies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "customerId", "dbo.CustomerCompanies");
            DropIndex("dbo.Tasks", new[] { "customerId" });
            DropColumn("dbo.Tasks", "customerId");
        }
    }
}
