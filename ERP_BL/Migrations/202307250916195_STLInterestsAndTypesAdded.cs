namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class STLInterestsAndTypesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.STLInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        interestTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        isManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.STLInterestTypes", t => t.interestTypeId)
                .Index(t => t.interestTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.STLInterestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.STLs", "interest_Id", c => c.Int());
            AddColumn("dbo.STLs", "SER", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "MER", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "STLAmountSER", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "STLAmountMER", c => c.Double(nullable: false));
            CreateIndex("dbo.STLs", "interest_Id");
            AddForeignKey("dbo.STLs", "interest_Id", "dbo.STLInterests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.STLs", "interest_Id", "dbo.STLInterests");
            DropForeignKey("dbo.STLInterests", "interestTypeId", "dbo.STLInterestTypes");
            DropForeignKey("dbo.STLInterests", "COA_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.STLs", new[] { "interest_Id" });
            DropIndex("dbo.STLInterests", new[] { "COA_Id" });
            DropIndex("dbo.STLInterests", new[] { "interestTypeId" });
            DropColumn("dbo.STLs", "STLAmountMER");
            DropColumn("dbo.STLs", "STLAmountSER");
            DropColumn("dbo.STLs", "MER");
            DropColumn("dbo.STLs", "SER");
            DropColumn("dbo.STLs", "interest_Id");
            DropTable("dbo.STLInterestTypes");
            DropTable("dbo.STLInterests");
        }
    }
}
