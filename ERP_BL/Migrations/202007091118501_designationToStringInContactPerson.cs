namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class designationToStringInContactPerson : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactPersons", "designation_DesigId", "dbo.Designations");
            DropIndex("dbo.ContactPersons", new[] { "designation_DesigId" });
            AddColumn("dbo.ContactPersons", "designation", c => c.String());
            DropColumn("dbo.ContactPersons", "designation_DesigId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactPersons", "designation_DesigId", c => c.Int());
            DropColumn("dbo.ContactPersons", "designation");
            CreateIndex("dbo.ContactPersons", "designation_DesigId");
            AddForeignKey("dbo.ContactPersons", "designation_DesigId", "dbo.Designations", "DesigId");
        }
    }
}
