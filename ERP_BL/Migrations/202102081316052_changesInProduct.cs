namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "parentId", c => c.Int());
            CreateIndex("dbo.Products", "parentId");
            AddForeignKey("dbo.Products", "parentId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "parentId", "dbo.Products");
            DropIndex("dbo.Products", new[] { "parentId" });
            DropColumn("dbo.Products", "parentId");
        }
    }
}
