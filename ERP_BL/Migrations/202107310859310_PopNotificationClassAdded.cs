namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopNotificationClassAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tabPopupNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        popupText = c.String(),
                        EmployeeIds = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tabPopupNotifications");
        }
    }
}
