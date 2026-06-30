namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payeeCatChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PayeeCategories", "Payee_Id", "dbo.Payees");
            DropIndex("dbo.PayeeCategories", new[] { "Payee_Id" });
            CreateTable(
                "dbo.PayeeCategoryPayees",
                c => new
                    {
                        PayeeCategory_Id = c.Int(nullable: false),
                        Payee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PayeeCategory_Id, t.Payee_Id })
                .ForeignKey("dbo.PayeeCategories", t => t.PayeeCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .Index(t => t.PayeeCategory_Id)
                .Index(t => t.Payee_Id);
            
            AddColumn("dbo.Fields", "Tag", c => c.String());
            AddColumn("dbo.Fields", "ElementName", c => c.String());
            AlterColumn("dbo.Fields", "LastModified", c => c.DateTime());
            DropColumn("dbo.Fields", "Name");
            DropColumn("dbo.Fields", "Description");
            DropColumn("dbo.Fields", "elementType");
            DropColumn("dbo.Fields", "transactionType");
            DropColumn("dbo.PayeeCategories", "Payee_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PayeeCategories", "Payee_Id", c => c.Int());
            AddColumn("dbo.Fields", "transactionType", c => c.Int(nullable: false));
            AddColumn("dbo.Fields", "elementType", c => c.String());
            AddColumn("dbo.Fields", "Description", c => c.String());
            AddColumn("dbo.Fields", "Name", c => c.String());
            DropForeignKey("dbo.PayeeCategoryPayees", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.PayeeCategoryPayees", "PayeeCategory_Id", "dbo.PayeeCategories");
            DropIndex("dbo.PayeeCategoryPayees", new[] { "Payee_Id" });
            DropIndex("dbo.PayeeCategoryPayees", new[] { "PayeeCategory_Id" });
            AlterColumn("dbo.Fields", "LastModified", c => c.DateTime(nullable: false));
            DropColumn("dbo.Fields", "ElementName");
            DropColumn("dbo.Fields", "Tag");
            DropTable("dbo.PayeeCategoryPayees");
            CreateIndex("dbo.PayeeCategories", "Payee_Id");
            AddForeignKey("dbo.PayeeCategories", "Payee_Id", "dbo.Payees", "Id");
        }
    }
}
