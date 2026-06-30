namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPaymentsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "insuranceRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "insuranceApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "insuranceNotApplicable", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "insuranceAppliedBy_Id", c => c.Int());
            CreateIndex("dbo.Payments", "insuranceAppliedBy_Id");
            AddForeignKey("dbo.Payments", "insuranceAppliedBy_Id", "dbo.Employees", "EmpId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "insuranceAppliedBy_Id", "dbo.Employees");
            DropIndex("dbo.Payments", new[] { "insuranceAppliedBy_Id" });
            DropColumn("dbo.Payments", "insuranceAppliedBy_Id");
            DropColumn("dbo.Payments", "insuranceNotApplicable");
            DropColumn("dbo.Payments", "insuranceApplied");
            DropColumn("dbo.Payments", "insuranceRequired");
        }
    }
}
