namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetAwards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetAwards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        target = c.Double(nullable: false),
                        IndviualAward = c.Double(nullable: false),
                        NoOfEmployees = c.Int(nullable: false),
                        TotalAward = c.Double(nullable: false),
                        Month = c.Int(nullable: false),
                        targetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Targets", t => t.targetId)
                .Index(t => t.targetId);
            
            AddColumn("dbo.Targets", "companyId", c => c.Int());
            AddColumn("dbo.TargetTypes", "Frequency", c => c.Int(nullable: false));
            CreateIndex("dbo.Targets", "companyId");
            AddForeignKey("dbo.Targets", "companyId", "dbo.tabCompany", "Id");
            DropColumn("dbo.Targets", "January");
            DropColumn("dbo.Targets", "Feburary");
            DropColumn("dbo.Targets", "March");
            DropColumn("dbo.Targets", "April");
            DropColumn("dbo.Targets", "May");
            DropColumn("dbo.Targets", "June");
            DropColumn("dbo.Targets", "July");
            DropColumn("dbo.Targets", "August");
            DropColumn("dbo.Targets", "September");
            DropColumn("dbo.Targets", "October");
            DropColumn("dbo.Targets", "November");
            DropColumn("dbo.Targets", "December");
            DropColumn("dbo.Targets", "Extra");
            DropColumn("dbo.Targets", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Targets", "Total", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "Extra", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "December", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "November", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "October", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "September", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "August", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "July", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "June", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "May", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "April", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "March", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "Feburary", c => c.Double(nullable: false));
            AddColumn("dbo.Targets", "January", c => c.Double(nullable: false));
            DropForeignKey("dbo.TargetAwards", "targetId", "dbo.Targets");
            DropForeignKey("dbo.Targets", "companyId", "dbo.tabCompany");
            DropIndex("dbo.TargetAwards", new[] { "targetId" });
            DropIndex("dbo.Targets", new[] { "companyId" });
            DropColumn("dbo.TargetTypes", "Frequency");
            DropColumn("dbo.Targets", "companyId");
            DropTable("dbo.TargetAwards");
        }
    }
}
