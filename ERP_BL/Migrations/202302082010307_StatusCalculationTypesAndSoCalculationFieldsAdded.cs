namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusCalculationTypesAndSoCalculationFieldsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusCalculationTypes",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TypeName = c.String(),
                    AchievedField_Id = c.Int(),
                    TotalField_Id = c.Int(),
                    stepType = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SoCalculationFields", t => t.AchievedField_Id)
                .ForeignKey("dbo.SoCalculationFields", t => t.TotalField_Id)
                .Index(t => t.AchievedField_Id)
                .Index(t => t.TotalField_Id);

            CreateTable(
                "dbo.SoCalculationFields",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DisplayName = c.String(),
                    SOFieldName = c.String(),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.ToDoTasks", "calculationType_Id", c => c.Int());
            AddColumn("dbo.ToDoTaskStatus", "Percentage", c => c.Double());
            CreateIndex("dbo.ToDoTasks", "calculationType_Id");
            AddForeignKey("dbo.ToDoTasks", "calculationType_Id", "dbo.StatusCalculationTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoTasks", "calculationType_Id", "dbo.StatusCalculationTypes");
            DropForeignKey("dbo.StatusCalculationTypes", "TotalField_Id", "dbo.SoCalculationFields");
            DropForeignKey("dbo.StatusCalculationTypes", "AchievedField_Id", "dbo.SoCalculationFields");
            DropIndex("dbo.StatusCalculationTypes", new[] { "TotalField_Id" });
            DropIndex("dbo.StatusCalculationTypes", new[] { "AchievedField_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "calculationType_Id" });
            DropColumn("dbo.ToDoTaskStatus", "Percentage");
            DropColumn("dbo.ToDoTasks", "calculationType_Id");
            DropTable("dbo.SoCalculationFields");
            DropTable("dbo.StatusCalculationTypes");
        }
    }
}
