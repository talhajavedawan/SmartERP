namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "taskTemplate", c => c.Int());
            AddColumn("dbo.Tasks", "vendorId", c => c.Int());
            AddColumn("dbo.Tasks", "employeeId", c => c.Int());
            AddColumn("dbo.Tasks", "taxableIncome", c => c.Double());
            AddColumn("dbo.Tasks", "chargeableTax", c => c.Double());
            AddColumn("dbo.Tasks", "NTNno", c => c.String());
            AddColumn("dbo.Tasks", "taxYear", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tasks", "FilerName", c => c.String());
            AddColumn("dbo.Tasks", "depositedTax", c => c.Double());
            AddColumn("dbo.Tasks", "NoticeRefNo", c => c.String());
            CreateIndex("dbo.Tasks", "vendorId");
            CreateIndex("dbo.Tasks", "employeeId");
            AddForeignKey("dbo.Tasks", "employeeId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Tasks", "vendorId", "dbo.tabVendor", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.Tasks", "employeeId", "dbo.Employees");
            DropIndex("dbo.Tasks", new[] { "employeeId" });
            DropIndex("dbo.Tasks", new[] { "vendorId" });
            DropColumn("dbo.Tasks", "NoticeRefNo");
            DropColumn("dbo.Tasks", "depositedTax");
            DropColumn("dbo.Tasks", "FilerName");
            DropColumn("dbo.Tasks", "taxYear");
            DropColumn("dbo.Tasks", "NTNno");
            DropColumn("dbo.Tasks", "chargeableTax");
            DropColumn("dbo.Tasks", "taxableIncome");
            DropColumn("dbo.Tasks", "employeeId");
            DropColumn("dbo.Tasks", "vendorId");
            DropColumn("dbo.Tasks", "taskTemplate");
        }
    }
}
