namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorBillNaturesAndExchangeRateGroupsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorBillNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nature = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExchangeRateGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExchangeType = c.Int(nullable: false),
                        TargetYear = c.Int(nullable: false),
                        AddedOn = c.DateTime(nullable: false),
                        transaction_currency_Id = c.Int(),
                        base_currency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.base_currency_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.transaction_currency_Id)
                .Index(t => t.transaction_currency_Id)
                .Index(t => t.base_currency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id);
            
            CreateTable(
                "dbo.ExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        rateJan = c.Double(nullable: false),
                        rateFeb = c.Double(nullable: false),
                        rateMar = c.Double(nullable: false),
                        rateApr = c.Double(nullable: false),
                        rateMay = c.Double(nullable: false),
                        rateJun = c.Double(nullable: false),
                        rateJul = c.Double(nullable: false),
                        rateAug = c.Double(nullable: false),
                        rateSep = c.Double(nullable: false),
                        rateOct = c.Double(nullable: false),
                        rateNov = c.Double(nullable: false),
                        rateDec = c.Double(nullable: false),
                        company_Id = c.Int(),
                        ExchangeRateGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.ExchangeRateGroups", t => t.ExchangeRateGroup_Id)
                .Index(t => t.company_Id)
                .Index(t => t.ExchangeRateGroup_Id);
            
            CreateTable(
                "dbo.ToDoTaskStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorBillNatureCompanies",
                c => new
                    {
                        VendorBillNature_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VendorBillNature_Id, t.Company_Id })
                .ForeignKey("dbo.VendorBillNatures", t => t.VendorBillNature_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.VendorBillNature_Id)
                .Index(t => t.Company_Id);
            
            AddColumn("dbo.Bills", "vendorBillNature_Id", c => c.Int());
            AddColumn("dbo.ToDoTasks", "statusId", c => c.Int());
            AddColumn("dbo.ToDoTasks", "AchievedPoints", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "PercentageAchieved", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "PointsUpdatedOn", c => c.DateTime());
            AlterColumn("dbo.ToDoTasks", "TaskPoints", c => c.Double(nullable: false));
            CreateIndex("dbo.Bills", "vendorBillNature_Id");
            CreateIndex("dbo.ToDoTasks", "statusId");
            AddForeignKey("dbo.Bills", "vendorBillNature_Id", "dbo.VendorBillNatures", "Id");
            AddForeignKey("dbo.ToDoTasks", "statusId", "dbo.ToDoTaskStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoTasks", "statusId", "dbo.ToDoTaskStatus");
            DropForeignKey("dbo.ExchangeRateGroups", "transaction_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "ExchangeRateGroup_Id", "dbo.ExchangeRateGroups");
            DropForeignKey("dbo.ExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ExchangeRateGroups", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.ExchangeRateGroups", "base_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRateGroups", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.Bills", "vendorBillNature_Id", "dbo.VendorBillNatures");
            DropForeignKey("dbo.VendorBillNatureCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.VendorBillNatureCompanies", "VendorBillNature_Id", "dbo.VendorBillNatures");
            DropIndex("dbo.VendorBillNatureCompanies", new[] { "Company_Id" });
            DropIndex("dbo.VendorBillNatureCompanies", new[] { "VendorBillNature_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "statusId" });
            DropIndex("dbo.ExchangeRates", new[] { "ExchangeRateGroup_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "base_currency_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "transaction_currency_Id" });
            DropIndex("dbo.Bills", new[] { "vendorBillNature_Id" });
            AlterColumn("dbo.ToDoTasks", "TaskPoints", c => c.Int(nullable: false));
            DropColumn("dbo.ToDoTasks", "PointsUpdatedOn");
            DropColumn("dbo.ToDoTasks", "PercentageAchieved");
            DropColumn("dbo.ToDoTasks", "AchievedPoints");
            DropColumn("dbo.ToDoTasks", "statusId");
            DropColumn("dbo.Bills", "vendorBillNature_Id");
            DropTable("dbo.VendorBillNatureCompanies");
            DropTable("dbo.ToDoTaskStatus");
            DropTable("dbo.ExchangeRates");
            DropTable("dbo.ExchangeRateGroups");
            DropTable("dbo.VendorBillNatures");
        }
    }
}
