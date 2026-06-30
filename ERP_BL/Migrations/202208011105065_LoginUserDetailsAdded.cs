namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginUserDetailsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoginUserDetails",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(),
                        LoginTime = c.DateTime(),
                        LogoutTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId);
            
            AddColumn("dbo.SaleOrders", "ParentSO_Id", c => c.Int());
            AddColumn("dbo.GridReports", "isFavourite", c => c.Boolean(nullable: false));
            CreateIndex("dbo.SaleOrders", "ParentSO_Id");
            AddForeignKey("dbo.SaleOrders", "ParentSO_Id", "dbo.SaleOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "ParentSO_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.LoginUserDetails", "userId", "dbo.Users");
            DropIndex("dbo.SaleOrders", new[] { "ParentSO_Id" });
            DropIndex("dbo.LoginUserDetails", new[] { "userId" });
            DropColumn("dbo.GridReports", "isFavourite");
            DropColumn("dbo.SaleOrders", "ParentSO_Id");
            DropTable("dbo.LoginUserDetails");
        }
    }
}
