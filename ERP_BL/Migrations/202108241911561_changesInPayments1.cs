namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPayments1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldValues", "isdgGood", c => c.Boolean(nullable: false));
            AddColumn("dbo.FieldValues", "isPacking", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "InterCompany_Id", c => c.Int());
            AddColumn("dbo.Payments", "isInterCompany", c => c.Boolean());
            AddColumn("dbo.Payments", "InterDepartment_Id", c => c.Int());
            CreateIndex("dbo.Payments", "InterCompany_Id");
            CreateIndex("dbo.Payments", "InterDepartment_Id");
            AddForeignKey("dbo.Payments", "InterDepartment_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.Payments", "InterCompany_Id", "dbo.tabCompany", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "InterDepartment_Id", "dbo.tabDepartment");
            DropIndex("dbo.Payments", new[] { "InterDepartment_Id" });
            DropIndex("dbo.Payments", new[] { "InterCompany_Id" });
            DropColumn("dbo.Payments", "InterDepartment_Id");
            DropColumn("dbo.Payments", "isInterCompany");
            DropColumn("dbo.Payments", "InterCompany_Id");
            DropColumn("dbo.FieldValues", "isPacking");
            DropColumn("dbo.FieldValues", "isdgGood");
        }
    }
}
