namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PersonPhotoesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeePhoto = c.Binary(),
                        PhotoName = c.String(),
                        Person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabPerson", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonPhotoes", "Person_Id", "dbo.tabPerson");
            DropIndex("dbo.PersonPhotoes", new[] { "Person_Id" });
            DropTable("dbo.PersonPhotoes");
        }
    }
}
