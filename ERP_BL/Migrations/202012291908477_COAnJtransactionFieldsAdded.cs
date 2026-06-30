namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    public partial class COAnJtransactionFieldsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChartofAccounts", "currency_Id", "dbo.Currencies");
            DropIndex("dbo.ChartofAccounts", new[] { "currency_Id" });
            RenameColumn(table: "dbo.LeaveApplications", name: "employee_EmpId", newName: "employeeId");
            RenameColumn(table: "dbo.LeaveApplications", name: "leave_Id", newName: "leaveId");
            RenameIndex(table: "dbo.LeaveApplications", name: "IX_leave_Id", newName: "IX_leaveId");
            RenameIndex(table: "dbo.LeaveApplications", name: "IX_employee_EmpId", newName: "IX_employeeId");
            CreateTable(
                "dbo.JournalTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        entryNumber = c.String(),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        memo = c.String(),
                        creationDate = c.DateTime(),
                        accountId = c.Int(),
                        userId = c.Int(),
                        isAdjustment = c.Boolean(nullable: false),
                        postingDate = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                        transactionRefno = c.String(),
                        coaTransactionsType = c.Int(nullable: false),
                    })
                    
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.accountId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.accountId)
                .Index(t => t.userId);
            
            AddColumn("dbo.ChartofAccounts", "isDebitIncrease", c => c.Boolean(nullable: false));
            DropColumn("dbo.ChartofAccounts", "balanceTotal");
            DropColumn("dbo.ChartofAccounts", "currency_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChartofAccounts", "currency_Id", c => c.Int());
            AddColumn("dbo.ChartofAccounts", "balanceTotal", c => c.Double());
            DropForeignKey("dbo.JournalTransactions", "userId", "dbo.Users");
            DropForeignKey("dbo.JournalTransactions", "accountId", "dbo.ChartofAccounts");
            DropIndex("dbo.JournalTransactions", new[] { "userId" });
            DropIndex("dbo.JournalTransactions", new[] { "accountId" });
            DropColumn("dbo.ChartofAccounts", "isDebitIncrease");
            DropTable("dbo.JournalTransactions");
            RenameIndex(table: "dbo.LeaveApplications", name: "IX_employeeId", newName: "IX_employee_EmpId");
            RenameIndex(table: "dbo.LeaveApplications", name: "IX_leaveId", newName: "IX_leave_Id");
            RenameColumn(table: "dbo.LeaveApplications", name: "leaveId", newName: "leave_Id");
            RenameColumn(table: "dbo.LeaveApplications", name: "employeeId", newName: "employee_EmpId");
            CreateIndex("dbo.ChartofAccounts", "currency_Id");
            AddForeignKey("dbo.ChartofAccounts", "currency_Id", "dbo.Currencies", "Id");
        }
    }
}
