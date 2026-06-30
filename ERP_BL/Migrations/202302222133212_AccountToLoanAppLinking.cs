namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountToLoanAppLinking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoanApplicantTypes", "accountId", c => c.Int());
            CreateIndex("dbo.LoanApplicantTypes", "accountId");
            AddForeignKey("dbo.LoanApplicantTypes", "accountId", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoanApplicantTypes", "accountId", "dbo.ChartofAccounts");
            DropIndex("dbo.LoanApplicantTypes", new[] { "accountId" });
            DropColumn("dbo.LoanApplicantTypes", "accountId");
        }
    }
}
