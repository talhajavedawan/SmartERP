namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reversalOfCompanyTaskGroupRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabCompany", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroups", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.TaskGroups", "Company_Id", "dbo.tabCompany");
            DropIndex("dbo.tabCompany", new[] { "TaskGroups_Id" });
            DropIndex("dbo.tabDepartment", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroups", new[] { "companyId" });
            DropIndex("dbo.TaskGroups", new[] { "dept_Id" });
            DropIndex("dbo.TaskGroups", new[] { "Department_Id" });
            DropIndex("dbo.TaskGroups", new[] { "Company_Id" });
            CreateTable(
                "dbo.TaskGroupsCompanies",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Company_Id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.TaskGroupsDepartments",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Department_Id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.Department_Id);
            
            DropColumn("dbo.tabCompany", "TaskGroups_Id");
            DropColumn("dbo.tabDepartment", "TaskGroups_Id");
            DropColumn("dbo.TaskGroups", "companyId");
            DropColumn("dbo.TaskGroups", "dept_Id");
            DropColumn("dbo.TaskGroups", "Department_Id");
            DropColumn("dbo.TaskGroups", "Company_Id");
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
            
            AlterStoredProcedure(
                "dbo.Company_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabCompany]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskGroups", "Company_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "Department_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "dept_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "companyId", c => c.Int());
            AddColumn("dbo.tabDepartment", "TaskGroups_Id", c => c.Int());
            AddColumn("dbo.tabCompany", "TaskGroups_Id", c => c.Int());
            DropForeignKey("dbo.TaskGroupsDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.TaskGroupsDepartments", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroupsCompanies", "TaskGroups_Id", "dbo.TaskGroups");
            DropIndex("dbo.TaskGroupsDepartments", new[] { "Department_Id" });
            DropIndex("dbo.TaskGroupsDepartments", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroupsCompanies", new[] { "Company_Id" });
            DropIndex("dbo.TaskGroupsCompanies", new[] { "TaskGroups_Id" });
            DropTable("dbo.TaskGroupsDepartments");
            DropTable("dbo.TaskGroupsCompanies");
            CreateIndex("dbo.TaskGroups", "Company_Id");
            CreateIndex("dbo.TaskGroups", "Department_Id");
            CreateIndex("dbo.TaskGroups", "dept_Id");
            CreateIndex("dbo.TaskGroups", "companyId");
            CreateIndex("dbo.tabDepartment", "TaskGroups_Id");
            CreateIndex("dbo.tabCompany", "TaskGroups_Id");
            AddForeignKey("dbo.TaskGroups", "Company_Id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.TaskGroups", "Department_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.tabDepartment", "TaskGroups_Id", "dbo.TaskGroups", "Id");
            AddForeignKey("dbo.TaskGroups", "dept_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.TaskGroups", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.tabCompany", "TaskGroups_Id", "dbo.TaskGroups", "Id");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
