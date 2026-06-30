namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleOrdeRrefKeysAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleOrdeRrefKeys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        key = c.String(nullable: false),
                        keyDate = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        comp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.comp_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .Index(t => t.dept_Id)
                .Index(t => t.comp_Id);
            
            AddColumn("dbo.tabCompany", "isLinkable", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "isLinkable", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleOrders", "SaleOrderKey_Id", c => c.Int());
            CreateIndex("dbo.SaleOrders", "SaleOrderKey_Id");
            AddForeignKey("dbo.SaleOrders", "SaleOrderKey_Id", "dbo.SaleOrdeRrefKeys", "Id");
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
                        isLinkable = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [CustomerVAT], [SaleTaxRegistrationNumber], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive], [isLinkable])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @CustomerVAT, @SaleTaxRegistrationNumber, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive, @isLinkable)
                      
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
                        isLinkable = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[tabCompany]
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [CustomerVAT] = @CustomerVAT, [SaleTaxRegistrationNumber] = @SaleTaxRegistrationNumber, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive, [isLinkable] = @isLinkable
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "SaleOrderKey_Id", "dbo.SaleOrdeRrefKeys");
            DropForeignKey("dbo.SaleOrdeRrefKeys", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrdeRrefKeys", "comp_Id", "dbo.tabCompany");
            DropIndex("dbo.SaleOrdeRrefKeys", new[] { "comp_Id" });
            DropIndex("dbo.SaleOrdeRrefKeys", new[] { "dept_Id" });
            DropIndex("dbo.SaleOrders", new[] { "SaleOrderKey_Id" });
            DropColumn("dbo.SaleOrders", "SaleOrderKey_Id");
            DropColumn("dbo.tabDepartment", "isLinkable");
            DropColumn("dbo.tabCompany", "isLinkable");
            DropTable("dbo.SaleOrdeRrefKeys");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
