namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanChanges18122021 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.JournalTransactions", name: "InterCompanyBankTransfer_Id", newName: "InterCompanyId");
            RenameIndex(table: "dbo.JournalTransactions", name: "IX_InterCompanyBankTransfer_Id", newName: "IX_InterCompanyId");
            CreateTable(
                "dbo.LoansStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabLoan", "statusId", c => c.Int());
            AddColumn("dbo.tabLoan", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.tabLoan", "SystemRefNo", c => c.String());
            AddColumn("dbo.tabLoan", "FinanceRefNo", c => c.String());
            AddColumn("dbo.tabLoan", "stage", c => c.String());
            AddColumn("dbo.tabLoan", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabLoan", "isReviewed", c => c.Boolean());
            AddColumn("dbo.tabLoan", "needReview", c => c.Boolean());
            AddColumn("dbo.tabLoan", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.tabLoan", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.tabLoan", "isApproved", c => c.Boolean());
            AddColumn("dbo.tabLoan", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "isReApproved", c => c.Boolean());
            AddColumn("dbo.tabLoan", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "user_Id", c => c.Int());
            AddColumn("dbo.tabLoan", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.tabLoan", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.tabLoan", "statusId");
            CreateIndex("dbo.tabLoan", "user_Id");
            AddForeignKey("dbo.tabLoan", "statusId", "dbo.LoansStatus", "Id");
            AddForeignKey("dbo.tabLoan", "user_Id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabLoan", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabLoan", "statusId", "dbo.LoansStatus");
            DropIndex("dbo.tabLoan", new[] { "user_Id" });
            DropIndex("dbo.tabLoan", new[] { "statusId" });
            DropColumn("dbo.tabLoan", "LastStatusChangeDate");
            DropColumn("dbo.tabLoan", "ClosingDate");
            DropColumn("dbo.tabLoan", "user_Id");
            DropColumn("dbo.tabLoan", "ReApprovalDate");
            DropColumn("dbo.tabLoan", "isReApproved");
            DropColumn("dbo.tabLoan", "ApprovedDate");
            DropColumn("dbo.tabLoan", "isApproved");
            DropColumn("dbo.tabLoan", "PendingForReApproval");
            DropColumn("dbo.tabLoan", "PendingForClosing");
            DropColumn("dbo.tabLoan", "needReview");
            DropColumn("dbo.tabLoan", "isReviewed");
            DropColumn("dbo.tabLoan", "isVoid");
            DropColumn("dbo.tabLoan", "stage");
            DropColumn("dbo.tabLoan", "FinanceRefNo");
            DropColumn("dbo.tabLoan", "SystemRefNo");
            DropColumn("dbo.tabLoan", "transactionGroupId");
            DropColumn("dbo.tabLoan", "statusId");
            DropTable("dbo.LoansStatus");
            RenameIndex(table: "dbo.JournalTransactions", name: "IX_InterCompanyId", newName: "IX_InterCompanyBankTransfer_Id");
            RenameColumn(table: "dbo.JournalTransactions", name: "InterCompanyId", newName: "InterCompanyBankTransfer_Id");
        }
    }
}
