namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isSelectForComparativeStatements : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComparativeStatements", "isSelect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComparativeStatements", "isSelect");
        }
    }
}
