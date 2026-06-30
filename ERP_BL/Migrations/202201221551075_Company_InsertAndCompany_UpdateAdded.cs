namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_InsertAndCompany_UpdateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabCompany", "SaleTaxRegistrationNumber", c => c.String());
            AddColumn("dbo.tabContact", "ContactNo1", c => c.String());
            AddColumn("dbo.tabContact", "ContactNo2", c => c.String());
            AddColumn("dbo.tabContact", "ContactNo3", c => c.String());
            AddColumn("dbo.tabContact", "Email1", c => c.String());
            AddColumn("dbo.tabContact", "Email2", c => c.String());
            AddColumn("dbo.tabContact", "Email3", c => c.String());
            AddColumn("dbo.BillItems", "BillAmount", c => c.Double(nullable: false));
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        CustomerVAT = p.String(),
                        SaleTaxRegistrationNumber = p.String(),
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
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [CustomerVAT], [SaleTaxRegistrationNumber], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @CustomerVAT, @SaleTaxRegistrationNumber, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive)
                      
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
                        CustomerVAT = p.String(),
                        SaleTaxRegistrationNumber = p.String(),
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
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [CustomerVAT] = @CustomerVAT, [SaleTaxRegistrationNumber] = @SaleTaxRegistrationNumber, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillItems", "BillAmount");
            DropColumn("dbo.tabContact", "Email3");
            DropColumn("dbo.tabContact", "Email2");
            DropColumn("dbo.tabContact", "Email1");
            DropColumn("dbo.tabContact", "ContactNo3");
            DropColumn("dbo.tabContact", "ContactNo2");
            DropColumn("dbo.tabContact", "ContactNo1");
            DropColumn("dbo.tabCompany", "SaleTaxRegistrationNumber");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
