namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInRentalContractsAndAssetRentals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany");
            DropIndex("dbo.AssetRentals", new[] { "companyId" });
            DropIndex("dbo.RentalContracts", new[] { "companyId" });
            DropIndex("dbo.RentalContracts", new[] { "TenantRentalId" });
            RenameColumn(table: "dbo.AssetRentals", name: "address_Id", newName: "addressId");
            RenameColumn(table: "dbo.TenantRentals", name: "address_Id", newName: "addressId");
            RenameColumn(table: "dbo.TenantRentals", name: "contact_Id", newName: "contactId");
            RenameColumn(table: "dbo.TenantRentals", name: "person_Id", newName: "personId");
            RenameIndex(table: "dbo.AssetRentals", name: "IX_address_Id", newName: "IX_addressId");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_contact_Id", newName: "IX_contactId");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_person_Id", newName: "IX_personId");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_address_Id", newName: "IX_addressId");
            AddColumn("dbo.RentalInvoices", "InvoiceAmount", c => c.Double(nullable: false));
            AddColumn("dbo.RentalInvoices", "TotalInvoiceAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.AssetRentals", "companyId", c => c.Int());
            AlterColumn("dbo.RentalContracts", "companyId", c => c.Int());
            CreateIndex("dbo.AssetRentals", "companyId");
            CreateIndex("dbo.RentalContracts", "companyId");
            CreateIndex("dbo.RentalContracts", "tenantRentalId");
            AddForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany", "Id");
            DropColumn("dbo.RentalInvoices", "RentAmount");
            DropColumn("dbo.RentalInvoices", "TotalRentAmount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalInvoices", "TotalRentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.RentalInvoices", "RentAmount", c => c.Double(nullable: false));
            DropForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany");
            DropIndex("dbo.RentalContracts", new[] { "tenantRentalId" });
            DropIndex("dbo.RentalContracts", new[] { "companyId" });
            DropIndex("dbo.AssetRentals", new[] { "companyId" });
            AlterColumn("dbo.RentalContracts", "companyId", c => c.Int(nullable: false));
            AlterColumn("dbo.AssetRentals", "companyId", c => c.Int(nullable: false));
            DropColumn("dbo.RentalInvoices", "TotalInvoiceAmount");
            DropColumn("dbo.RentalInvoices", "InvoiceAmount");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_addressId", newName: "IX_address_Id");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_personId", newName: "IX_person_Id");
            RenameIndex(table: "dbo.TenantRentals", name: "IX_contactId", newName: "IX_contact_Id");
            RenameIndex(table: "dbo.AssetRentals", name: "IX_addressId", newName: "IX_address_Id");
            RenameColumn(table: "dbo.TenantRentals", name: "personId", newName: "person_Id");
            RenameColumn(table: "dbo.TenantRentals", name: "contactId", newName: "contact_Id");
            RenameColumn(table: "dbo.TenantRentals", name: "addressId", newName: "address_Id");
            RenameColumn(table: "dbo.AssetRentals", name: "addressId", newName: "address_Id");
            CreateIndex("dbo.RentalContracts", "TenantRentalId");
            CreateIndex("dbo.RentalContracts", "companyId");
            CreateIndex("dbo.AssetRentals", "companyId");
            AddForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany", "Id", cascadeDelete: true);
        }
    }
}
