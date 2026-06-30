
namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabDeptChangesNull : DbMigration
    {
        // dontot un comment migraiton lines. As its only here to match the Db image. 
        public override void Up()
        {
            //AddColumn("dbo.tabDepartment", "applyMERasSER", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.tabDepartment", "applyMERasSER");
        }
    }
}
