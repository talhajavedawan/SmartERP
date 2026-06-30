namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTargets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Targets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Code = c.String(),
                        Year = c.Int(nullable: false),
                        January = c.Double(nullable: false),
                        Feburary = c.Double(nullable: false),
                        March = c.Double(nullable: false),
                        April = c.Double(nullable: false),
                        May = c.Double(nullable: false),
                        June = c.Double(nullable: false),
                        July = c.Double(nullable: false),
                        August = c.Double(nullable: false),
                        September = c.Double(nullable: false),
                        October = c.Double(nullable: false),
                        November = c.Double(nullable: false),
                        December = c.Double(nullable: false),
                        Extra = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        typeId = c.Int(nullable: false),
                        currencyId = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        AchivedDate = c.DateTime(),
                        EditDate = c.DateTime(),
                        isAchived = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        departmentId = c.Int(),
                        user_Id = c.Int(),
                        AchivedJanuary = c.Double(),
                        AchivedFeburary = c.Double(),
                        AchivedMarch = c.Double(),
                        AchivedApril = c.Double(),
                        AchivedMay = c.Double(),
                        AchivedJune = c.Double(),
                        AchivedJuly = c.Double(),
                        AchivedAugust = c.Double(),
                        AchivedSeptember = c.Double(),
                        AchivedOctober = c.Double(),
                        AchivedNovember = c.Double(),
                        AchivedDecember = c.Double(),
                        AchivedExtra = c.Double(),
                        AchivedTotal = c.Double(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.currencyId, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.TargetTypes", t => t.typeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.typeId)
                .Index(t => t.currencyId)
                .Index(t => t.departmentId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.TargetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Targets", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Targets", "typeId", "dbo.TargetTypes");
            DropForeignKey("dbo.TargetTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Targets", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.Targets", "currencyId", "dbo.Currencies");
            DropIndex("dbo.TargetTypes", new[] { "user_Id" });
            DropIndex("dbo.Targets", new[] { "user_Id" });
            DropIndex("dbo.Targets", new[] { "departmentId" });
            DropIndex("dbo.Targets", new[] { "currencyId" });
            DropIndex("dbo.Targets", new[] { "typeId" });
            DropTable("dbo.TargetTypes");
            DropTable("dbo.Targets");
        }
    }
}
