namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInBiilsToConnectLoans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "loanAdvanceCompany_Id", c => c.Int());
            AddColumn("dbo.Bills", "loanAdvanceDept_Id", c => c.Int());
            AddColumn("dbo.LoansAdvances", "isLinkable", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Bills", "loanAdvanceCompany_Id");
            CreateIndex("dbo.Bills", "loanAdvanceDept_Id");
            AddForeignKey("dbo.Bills", "loanAdvanceCompany_Id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.Bills", "loanAdvanceDept_Id", "dbo.tabDepartment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "loanAdvanceDept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Bills", "loanAdvanceCompany_Id", "dbo.tabCompany");
            DropIndex("dbo.Bills", new[] { "loanAdvanceDept_Id" });
            DropIndex("dbo.Bills", new[] { "loanAdvanceCompany_Id" });
            DropColumn("dbo.LoansAdvances", "isLinkable");
            DropColumn("dbo.Bills", "loanAdvanceDept_Id");
            DropColumn("dbo.Bills", "loanAdvanceCompany_Id");
        }
    }
}
