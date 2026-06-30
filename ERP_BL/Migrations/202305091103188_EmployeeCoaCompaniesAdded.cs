namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeCoaCompaniesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeCoaCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        compId = c.Int(),
                        EmpId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.compId)
                .ForeignKey("dbo.Employees", t => t.EmpId)
                .Index(t => t.compId)
                .Index(t => t.EmpId);
            
            AddColumn("dbo.TaskGroups", "CalculationType_Id", c => c.Int());
            CreateIndex("dbo.TaskGroups", "CalculationType_Id");
            AddForeignKey("dbo.TaskGroups", "CalculationType_Id", "dbo.StatusCalculationTypes", "Id");
            DropColumn("dbo.TargetGroups", "forStep");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TargetGroups", "forStep", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.EmployeeCoaCompanies", "EmpId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeCoaCompanies", "compId", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroups", "CalculationType_Id", "dbo.StatusCalculationTypes");
            DropIndex("dbo.EmployeeCoaCompanies", new[] { "EmpId" });
            DropIndex("dbo.EmployeeCoaCompanies", new[] { "compId" });
            DropIndex("dbo.TaskGroups", new[] { "CalculationType_Id" });
            DropColumn("dbo.TaskGroups", "CalculationType_Id");
            DropTable("dbo.EmployeeCoaCompanies");
        }
    }
}
