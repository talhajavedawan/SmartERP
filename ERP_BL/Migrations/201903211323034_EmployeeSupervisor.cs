namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeSupervisor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "HireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "SupervisorId", c => c.Int());
            AddColumn("dbo.Employees", "DesignationTitle", c => c.String());
            AddColumn("dbo.Employees", "JobDescription", c => c.String());
            CreateIndex("dbo.Employees", "SupervisorId");
            AddForeignKey("dbo.Employees", "SupervisorId", "dbo.Employees", "EmpId");
            AlterStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        MaritalStatus = p.String(),
                        Disability = p.Boolean(),
                        DisDescription = p.String(),
                        isActive = p.Boolean(),
                        Status = p.Int(),
                        JoinDate = p.DateTime(),
                        HireDate = p.DateTime(),
                        BasicPay = p.Double(),
                        SupervisorId = p.Int(),
                        DesignationTitle = p.String(),
                        JobDescription = p.String(),
                        address_Id = p.Int(),
                        contact_Id = p.Int(),
                        Desig_DesigId = p.Int(),
                        person_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([MaritalStatus], [Disability], [DisDescription], [isActive], [Status], [JoinDate], [HireDate], [BasicPay], [SupervisorId], [DesignationTitle], [JobDescription], [address_Id], [contact_Id], [Desig_DesigId], [person_Id])
                      VALUES (@MaritalStatus, @Disability, @DisDescription, @isActive, @Status, @JoinDate, @HireDate, @BasicPay, @SupervisorId, @DesignationTitle, @JobDescription, @address_Id, @contact_Id, @Desig_DesigId, @person_Id)
                      
                      DECLARE @EmpId int
                      SELECT @EmpId = [EmpId]
                      FROM [dbo].[Employees]
                      WHERE @@ROWCOUNT > 0 AND [EmpId] = scope_identity()
                      
                      SELECT t0.[EmpId]
                      FROM [dbo].[Employees] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[EmpId] = @EmpId"
            );
            
            AlterStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        EmpId = p.Int(),
                        MaritalStatus = p.String(),
                        Disability = p.Boolean(),
                        DisDescription = p.String(),
                        isActive = p.Boolean(),
                        Status = p.Int(),
                        JoinDate = p.DateTime(),
                        HireDate = p.DateTime(),
                        BasicPay = p.Double(),
                        SupervisorId = p.Int(),
                        DesignationTitle = p.String(),
                        JobDescription = p.String(),
                        address_Id = p.Int(),
                        contact_Id = p.Int(),
                        Desig_DesigId = p.Int(),
                        person_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [MaritalStatus] = @MaritalStatus, [Disability] = @Disability, [DisDescription] = @DisDescription, [isActive] = @isActive, [Status] = @Status, [JoinDate] = @JoinDate, [HireDate] = @HireDate, [BasicPay] = @BasicPay, [SupervisorId] = @SupervisorId, [DesignationTitle] = @DesignationTitle, [JobDescription] = @JobDescription, [address_Id] = @address_Id, [contact_Id] = @contact_Id, [Desig_DesigId] = @Desig_DesigId, [person_Id] = @person_Id
                      WHERE ([EmpId] = @EmpId)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "SupervisorId", "dbo.Employees");
            DropIndex("dbo.Employees", new[] { "SupervisorId" });
            DropColumn("dbo.Employees", "JobDescription");
            DropColumn("dbo.Employees", "DesignationTitle");
            DropColumn("dbo.Employees", "SupervisorId");
            DropColumn("dbo.Employees", "HireDate");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
