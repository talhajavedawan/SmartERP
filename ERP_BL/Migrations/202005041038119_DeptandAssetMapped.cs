namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeptandAssetMapped : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "managingDept_Id", c => c.Int());
            CreateIndex("dbo.Assets", "managingDept_Id");
            AddForeignKey("dbo.Assets", "managingDept_Id", "dbo.tabDepartment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "managingDept_Id", "dbo.tabDepartment");
            DropIndex("dbo.Assets", new[] { "managingDept_Id" });
            DropColumn("dbo.Assets", "managingDept_Id");
        }
    }
}
