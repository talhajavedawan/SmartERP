namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LotNumbersTabesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LotNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        LotNo = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.companyId)
                .Index(t => t.deptId);
            
            AddColumn("dbo.Tasks", "lotNumberId", c => c.Int());
            AddColumn("dbo.SaleInvoices", "lotNumberId", c => c.Int());
            CreateIndex("dbo.Tasks", "lotNumberId");
            CreateIndex("dbo.SaleInvoices", "lotNumberId");
            AddForeignKey("dbo.SaleInvoices", "lotNumberId", "dbo.LotNumbers", "Id");
            AddForeignKey("dbo.Tasks", "lotNumberId", "dbo.LotNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "lotNumberId", "dbo.LotNumbers");
            DropForeignKey("dbo.SaleInvoices", "lotNumberId", "dbo.LotNumbers");
            DropForeignKey("dbo.LotNumbers", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.LotNumbers", "companyId", "dbo.tabCompany");
            DropIndex("dbo.LotNumbers", new[] { "deptId" });
            DropIndex("dbo.LotNumbers", new[] { "companyId" });
            DropIndex("dbo.SaleInvoices", new[] { "lotNumberId" });
            DropIndex("dbo.Tasks", new[] { "lotNumberId" });
            DropColumn("dbo.SaleInvoices", "lotNumberId");
            DropColumn("dbo.Tasks", "lotNumberId");
            DropTable("dbo.LotNumbers");
        }
    }
}
