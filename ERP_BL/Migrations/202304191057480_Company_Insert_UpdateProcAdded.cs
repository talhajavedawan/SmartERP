namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Company_Insert_UpdateProcAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskGroupsCompanies", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroupsDepartments", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsDepartments", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.TaskGroupsCompanies", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroupsCompanies", new[] { "Company_Id" });
            DropIndex("dbo.TaskGroupsDepartments", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroupsDepartments", new[] { "Department_Id" });
            AddColumn("dbo.tabCompany", "TaskGroups_Id", c => c.Int());
            AddColumn("dbo.tabDepartment", "TaskGroups_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "payableAccount_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "cgsAccount_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "companyId", c => c.Int());
            AddColumn("dbo.TaskGroups", "dept_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "currency_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "Department_Id", c => c.Int());
            AddColumn("dbo.TaskGroups", "Company_Id", c => c.Int());
            AddColumn("dbo.JournalTransactions", "TargetRewardId", c => c.Int());
            AddColumn("dbo.TargetRewards", "financeRefNo", c => c.String());
            AddColumn("dbo.TargetRewards", "GlPostingDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "MER", c => c.Double(nullable: false));
            CreateIndex("dbo.tabCompany", "TaskGroups_Id");
            CreateIndex("dbo.tabDepartment", "TaskGroups_Id");
            CreateIndex("dbo.TaskGroups", "payableAccount_Id");
            CreateIndex("dbo.TaskGroups", "cgsAccount_Id");
            CreateIndex("dbo.TaskGroups", "companyId");
            CreateIndex("dbo.TaskGroups", "dept_Id");
            CreateIndex("dbo.TaskGroups", "currency_Id");
            CreateIndex("dbo.TaskGroups", "Department_Id");
            CreateIndex("dbo.TaskGroups", "Company_Id");
            CreateIndex("dbo.JournalTransactions", "TargetRewardId");
            AddForeignKey("dbo.TaskGroups", "cgsAccount_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.tabCompany", "TaskGroups_Id", "dbo.TaskGroups", "Id");
            AddForeignKey("dbo.TaskGroups", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.TaskGroups", "currency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.TaskGroups", "dept_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.tabDepartment", "TaskGroups_Id", "dbo.TaskGroups", "Id");
            AddForeignKey("dbo.TaskGroups", "payableAccount_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.JournalTransactions", "TargetRewardId", "dbo.TargetRewards", "Id");
            AddForeignKey("dbo.TaskGroups", "Department_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.TaskGroups", "Company_Id", "dbo.tabCompany", "Id");
            DropTable("dbo.TaskGroupsCompanies");
            DropTable("dbo.TaskGroupsDepartments");
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
                        TaskGroups_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [CustomerVAT], [SaleTaxRegistrationNumber], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive], [isLinkable], [TaskGroups_Id])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @CustomerVAT, @SaleTaxRegistrationNumber, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive, @isLinkable, @TaskGroups_Id)
                      
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
                        TaskGroups_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabCompany]
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [CustomerVAT] = @CustomerVAT, [SaleTaxRegistrationNumber] = @SaleTaxRegistrationNumber, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive, [isLinkable] = @isLinkable, [TaskGroups_Id] = @TaskGroups_Id
                      WHERE ([Id] = @Id)"
            );
            
            AlterStoredProcedure(
                "dbo.Company_Delete",
                p => new
                    {
                        Id = p.Int(),
                        TaskGroups_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabCompany]
                      WHERE (([Id] = @Id) AND (([TaskGroups_Id] = @TaskGroups_Id) OR ([TaskGroups_Id] IS NULL AND @TaskGroups_Id IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TaskGroupsDepartments",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Department_Id });
            
            CreateTable(
                "dbo.TaskGroupsCompanies",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Company_Id });
            
            DropForeignKey("dbo.TaskGroups", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroups", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.JournalTransactions", "TargetRewardId", "dbo.TargetRewards");
            DropForeignKey("dbo.TaskGroups", "payableAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabDepartment", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.TaskGroups", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.TaskGroups", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "cgsAccount_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.JournalTransactions", new[] { "TargetRewardId" });
            DropIndex("dbo.TaskGroups", new[] { "Company_Id" });
            DropIndex("dbo.TaskGroups", new[] { "Department_Id" });
            DropIndex("dbo.TaskGroups", new[] { "currency_Id" });
            DropIndex("dbo.TaskGroups", new[] { "dept_Id" });
            DropIndex("dbo.TaskGroups", new[] { "companyId" });
            DropIndex("dbo.TaskGroups", new[] { "cgsAccount_Id" });
            DropIndex("dbo.TaskGroups", new[] { "payableAccount_Id" });
            DropIndex("dbo.tabDepartment", new[] { "TaskGroups_Id" });
            DropIndex("dbo.tabCompany", new[] { "TaskGroups_Id" });
            DropColumn("dbo.TargetRewards", "MER");
            DropColumn("dbo.TargetRewards", "GlPostingDate");
            DropColumn("dbo.TargetRewards", "financeRefNo");
            DropColumn("dbo.JournalTransactions", "TargetRewardId");
            DropColumn("dbo.TaskGroups", "Company_Id");
            DropColumn("dbo.TaskGroups", "Department_Id");
            DropColumn("dbo.TaskGroups", "currency_Id");
            DropColumn("dbo.TaskGroups", "dept_Id");
            DropColumn("dbo.TaskGroups", "companyId");
            DropColumn("dbo.TaskGroups", "cgsAccount_Id");
            DropColumn("dbo.TaskGroups", "payableAccount_Id");
            DropColumn("dbo.tabDepartment", "TaskGroups_Id");
            DropColumn("dbo.tabCompany", "TaskGroups_Id");
            CreateIndex("dbo.TaskGroupsDepartments", "Department_Id");
            CreateIndex("dbo.TaskGroupsDepartments", "TaskGroups_Id");
            CreateIndex("dbo.TaskGroupsCompanies", "Company_Id");
            CreateIndex("dbo.TaskGroupsCompanies", "TaskGroups_Id");
            AddForeignKey("dbo.TaskGroupsDepartments", "Department_Id", "dbo.tabDepartment", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskGroupsDepartments", "TaskGroups_Id", "dbo.TaskGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskGroupsCompanies", "Company_Id", "dbo.tabCompany", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskGroupsCompanies", "TaskGroups_Id", "dbo.TaskGroups", "Id", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
