namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInLoansAdvances1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoansAdvances", "advanceTemplate", c => c.Int(nullable: false));
            AddColumn("dbo.LoanApplicants", "LenderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoanApplicantTypes", "LenderType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoanApplicantTypes", "LenderType");
            DropColumn("dbo.LoanApplicants", "LenderType");
            DropColumn("dbo.LoansAdvances", "advanceTemplate");
        }
    }
}
