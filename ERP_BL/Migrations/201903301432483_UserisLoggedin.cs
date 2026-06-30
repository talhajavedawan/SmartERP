namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserisLoggedin : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "userName" });
            AddColumn("dbo.Users", "isLoggedIn", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "userName", c => c.String(maxLength: 60, unicode: false));
            CreateIndex("dbo.Users", "userName", unique: true);
            AlterStoredProcedure(
                "dbo.User_Insert",
                p => new
                    {
                        employeeId = p.Int(),
                        userName = p.String(maxLength: 60, unicode: false),
                        password = p.String(),
                        isActive = p.Boolean(),
                        isLoggedIn = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Users]([employeeId], [userName], [password], [isActive], [isLoggedIn])
                      VALUES (@employeeId, @userName, @password, @isActive, @isLoggedIn)
                      
                      DECLARE @id int
                      SELECT @id = [id]
                      FROM [dbo].[Users]
                      WHERE @@ROWCOUNT > 0 AND [id] = scope_identity()
                      
                      SELECT t0.[id]
                      FROM [dbo].[Users] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[id] = @id"
            );
            
            AlterStoredProcedure(
                "dbo.User_Update",
                p => new
                    {
                        id = p.Int(),
                        employeeId = p.Int(),
                        userName = p.String(maxLength: 60, unicode: false),
                        password = p.String(),
                        isActive = p.Boolean(),
                        isLoggedIn = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Users]
                      SET [employeeId] = @employeeId, [userName] = @userName, [password] = @password, [isActive] = @isActive, [isLoggedIn] = @isLoggedIn
                      WHERE ([id] = @id)"
            );
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "userName" });
            AlterColumn("dbo.Users", "userName", c => c.String(maxLength: 25, unicode: false));
            DropColumn("dbo.Users", "isLoggedIn");
            CreateIndex("dbo.Users", "userName", unique: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
