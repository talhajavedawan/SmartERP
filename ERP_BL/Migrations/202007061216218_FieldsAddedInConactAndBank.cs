namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldsAddedInConactAndBank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "bank_Id", c => c.Int());
            AddColumn("dbo.tabContact", "SecondaryContact", c => c.String());
            CreateIndex("dbo.Accounts", "bank_Id");
            AddForeignKey("dbo.Accounts", "bank_Id", "dbo.Banks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "bank_Id", "dbo.Banks");
            DropIndex("dbo.Accounts", new[] { "bank_Id" });
            DropColumn("dbo.tabContact", "SecondaryContact");
            DropColumn("dbo.Accounts", "bank_Id");
        }
    }
}
