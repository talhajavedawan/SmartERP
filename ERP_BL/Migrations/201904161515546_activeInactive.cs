namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeInactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabCompany", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.CustomerCompanies", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Principals", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabVendor", "isActive", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabCompany]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabCompany] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Update",
                p => new
                    {
                        Id = p.Int(),
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[tabCompany]
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabVendor", "isActive");
            DropColumn("dbo.Principals", "isActive");
            DropColumn("dbo.CustomerCompanies", "isActive");
            DropColumn("dbo.tabDepartment", "isActive");
            DropColumn("dbo.tabCompany", "isActive");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
