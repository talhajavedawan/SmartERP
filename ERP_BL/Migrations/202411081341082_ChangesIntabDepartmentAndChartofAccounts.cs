namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIntabDepartmentAndChartofAccounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsDocumentType", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChartofAccounts", "accociatedCompany", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChartofAccounts", "accociatedCompany");
            DropColumn("dbo.tabDepartment", "IsDocumentType");
        }
    }
}
