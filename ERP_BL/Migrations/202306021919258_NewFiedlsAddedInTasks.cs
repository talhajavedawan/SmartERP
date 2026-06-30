namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewFiedlsAddedInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "OfficeSupportRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "LositicSupportRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "LogisticAreaFrom", c => c.String());
            AddColumn("dbo.Tasks", "LogisticAreaTo", c => c.String());
            AddColumn("dbo.Tasks", "inHouseLogistics", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "outsourceLogistics", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "PlannedExecutionDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "FinalExecutionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "FinalExecutionDate");
            DropColumn("dbo.Tasks", "PlannedExecutionDate");
            DropColumn("dbo.Tasks", "outsourceLogistics");
            DropColumn("dbo.Tasks", "inHouseLogistics");
            DropColumn("dbo.Tasks", "LogisticAreaTo");
            DropColumn("dbo.Tasks", "LogisticAreaFrom");
            DropColumn("dbo.Tasks", "LositicSupportRequired");
            DropColumn("dbo.Tasks", "OfficeSupportRequired");
        }
    }
}
