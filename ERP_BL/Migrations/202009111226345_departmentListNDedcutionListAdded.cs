namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departmentListNDedcutionListAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accounts", "department_Id", "dbo.tabDepartment");
            DropIndex("dbo.Accounts", new[] { "department_Id" });
            CreateTable(
                "dbo.ReceiptDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        Amount = c.Double(nullable: false),
                        receiptType = c.Int(nullable: false),
                        SalesReceipt_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceipt_Id)
                .Index(t => t.SalesReceipt_Id);
            
            AddColumn("dbo.ContactPersons", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "Account_Id", c => c.Int());
            CreateIndex("dbo.tabDepartment", "Account_Id");
            AddForeignKey("dbo.tabDepartment", "Account_Id", "dbo.Accounts", "Id");
            DropColumn("dbo.Accounts", "department_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "department_Id", c => c.Int());
            DropForeignKey("dbo.tabDepartment", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.ReceiptDeductions", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropIndex("dbo.ReceiptDeductions", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.tabDepartment", new[] { "Account_Id" });
            DropColumn("dbo.tabDepartment", "Account_Id");
            DropColumn("dbo.ContactPersons", "isActive");
            DropTable("dbo.ReceiptDeductions");
            CreateIndex("dbo.Accounts", "department_Id");
            AddForeignKey("dbo.Accounts", "department_Id", "dbo.tabDepartment", "Id");
        }
    }
}
