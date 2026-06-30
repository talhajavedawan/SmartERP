namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinAdminBillAndLoans : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.tabLoan", name: "facilityNatureId", newName: "MainLimitNatureId");
            RenameIndex(table: "dbo.tabLoan", name: "IX_facilityNatureId", newName: "IX_MainLimitNatureId");
            AddColumn("dbo.tabCompany", "CustomerVAT", c => c.String());
            AddColumn("dbo.tabPerson", "Signature", c => c.Binary());
            AddColumn("dbo.tabAdminBill", "hasWAT", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "tax_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "TaxAmount", c => c.Double(nullable: false));
            AddColumn("dbo.tabAdminBill", "AmountWithTax", c => c.Double(nullable: false));
            AddColumn("dbo.tabLoan", "LimitDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "ExpiryDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "ExtensionDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "SubLimitNatureId", c => c.Int());
            AddColumn("dbo.tabLoan", "MainLimitAmount", c => c.Double(nullable: false));
            AddColumn("dbo.tabLoan", "SubLimitAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.tabAdminBill", "tax_Id");
            CreateIndex("dbo.tabLoan", "SubLimitNatureId");
            AddForeignKey("dbo.tabAdminBill", "tax_Id", "dbo.TaxNames", "Id");
            AddForeignKey("dbo.tabLoan", "SubLimitNatureId", "dbo.FacilityNatures", "Id");
            DropColumn("dbo.tabLoan", "Amount");
            AlterStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        CustomerVAT = p.String(),
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
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [CustomerVAT], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @CustomerVAT, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive)
                      
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
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [CustomerVAT] = @CustomerVAT, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabLoan", "Amount", c => c.Double(nullable: false));
            DropForeignKey("dbo.tabLoan", "SubLimitNatureId", "dbo.FacilityNatures");
            DropForeignKey("dbo.tabAdminBill", "tax_Id", "dbo.TaxNames");
            DropIndex("dbo.tabLoan", new[] { "SubLimitNatureId" });
            DropIndex("dbo.tabAdminBill", new[] { "tax_Id" });
            DropColumn("dbo.tabLoan", "SubLimitAmount");
            DropColumn("dbo.tabLoan", "MainLimitAmount");
            DropColumn("dbo.tabLoan", "SubLimitNatureId");
            DropColumn("dbo.tabLoan", "ExtensionDate");
            DropColumn("dbo.tabLoan", "ExpiryDate");
            DropColumn("dbo.tabLoan", "LimitDate");
            DropColumn("dbo.tabAdminBill", "AmountWithTax");
            DropColumn("dbo.tabAdminBill", "TaxAmount");
            DropColumn("dbo.tabAdminBill", "tax_Id");
            DropColumn("dbo.tabAdminBill", "hasWAT");
            DropColumn("dbo.tabPerson", "Signature");
            DropColumn("dbo.tabCompany", "CustomerVAT");
            RenameIndex(table: "dbo.tabLoan", name: "IX_MainLimitNatureId", newName: "IX_facilityNatureId");
            RenameColumn(table: "dbo.tabLoan", name: "MainLimitNatureId", newName: "facilityNatureId");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
