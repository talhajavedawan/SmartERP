namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataRetrievalDateAddedInEmployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "DataRetrievalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "DataRetrievalDate");
        }
    }
}
