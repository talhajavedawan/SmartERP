namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class multipleContactPersonsAddedInBank : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Banks", "contactPerson_Id", "dbo.ContactPersons");
            DropIndex("dbo.Banks", new[] { "contactPerson_Id" });
            AddColumn("dbo.ContactPersons", "Bank_Id", c => c.Int());
            CreateIndex("dbo.ContactPersons", "Bank_Id");
            AddForeignKey("dbo.ContactPersons", "Bank_Id", "dbo.Banks", "Id");
            DropColumn("dbo.Banks", "contactPerson_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Banks", "contactPerson_Id", c => c.Int());
            DropForeignKey("dbo.ContactPersons", "Bank_Id", "dbo.Banks");
            DropIndex("dbo.ContactPersons", new[] { "Bank_Id" });
            DropColumn("dbo.ContactPersons", "Bank_Id");
            CreateIndex("dbo.Banks", "contactPerson_Id");
            AddForeignKey("dbo.Banks", "contactPerson_Id", "dbo.ContactPersons", "Id");
        }
    }
}
