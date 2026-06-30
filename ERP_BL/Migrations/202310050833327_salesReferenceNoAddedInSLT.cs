namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salesReferenceNoAddedInSLT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.STLs", "salesReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.STLs", "salesReferenceNo");
        }
    }
}
