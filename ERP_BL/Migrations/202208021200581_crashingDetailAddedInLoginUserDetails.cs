namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crashingDetailAddedInLoginUserDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoginUserDetails", "crashingDetail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoginUserDetails", "crashingDetail");
        }
    }
}
