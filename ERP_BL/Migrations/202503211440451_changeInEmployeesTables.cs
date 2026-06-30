namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInEmployeesTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "RetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "InquiryDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "OfferDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "SODataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "MemorandumSaleDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "SIDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "PODataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "PIDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "VendorBillDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "SRDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "FixedAssetsDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "IBTDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "AdminBillDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "PaymentDataRetrievalDate", c => c.DateTime());
            AddColumn("dbo.Employees", "LoansAdvancesDataRetrievalDate", c => c.DateTime());
            DropColumn("dbo.Employees", "DataRetrievalDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "DataRetrievalDate", c => c.DateTime());
            DropColumn("dbo.Employees", "LoansAdvancesDataRetrievalDate");
            DropColumn("dbo.Employees", "PaymentDataRetrievalDate");
            DropColumn("dbo.Employees", "AdminBillDataRetrievalDate");
            DropColumn("dbo.Employees", "IBTDataRetrievalDate");
            DropColumn("dbo.Employees", "FixedAssetsDataRetrievalDate");
            DropColumn("dbo.Employees", "SRDataRetrievalDate");
            DropColumn("dbo.Employees", "VendorBillDataRetrievalDate");
            DropColumn("dbo.Employees", "PIDataRetrievalDate");
            DropColumn("dbo.Employees", "PODataRetrievalDate");
            DropColumn("dbo.Employees", "SIDataRetrievalDate");
            DropColumn("dbo.Employees", "MemorandumSaleDataRetrievalDate");
            DropColumn("dbo.Employees", "SODataRetrievalDate");
            DropColumn("dbo.Employees", "OfferDataRetrievalDate");
            DropColumn("dbo.Employees", "InquiryDataRetrievalDate");
            DropColumn("dbo.Employees", "RetrievalDate");
        }
    }
}
