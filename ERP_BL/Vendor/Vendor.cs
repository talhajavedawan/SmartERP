using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements;

namespace ERP_BL.Databases
{
    [Table("tabVendor")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Person contactPerson { get; set; }
        public virtual Address shippingAddress { get; set; }
        public virtual Address billingAddres { get; set; }
        public virtual Contact contact { get; set; }
        public virtual Company company { get; set; }

        [InverseProperty("Vendors")]
        public virtual List<Company> Companies { get; set; }

        [InverseProperty("Vendors")]
        public virtual List<Department> departments { get; set; }
        [InverseProperty("vendors")]
        public virtual List<PurchaseOrder> PurchaseOrders { get; set; }
        [InverseProperty("vendor")]
        public virtual List<Bill> Bills { get; set; }

        [InverseProperty("billVendor")]
        public virtual List<Bill> billVendorBills { get; set; }

        [InverseProperty("POVendor")]
        public virtual List<Bill> POVendorBills { get; set; }
        [InverseProperty("vendors")]
        public virtual List<SaleOrder> SaleOrders { get; set; }
        [InverseProperty("vendors")]
        public virtual List<SaleInvoice> SaleInvoices { get; set; }
        [InverseProperty("vendors")]
        public virtual List<Offer> Offers { get; set; }

        [InverseProperty("vendors")]
        public virtual List<ModuleContract> ModuleContracts { get; set; }
        public bool isActive { get; set; } = true;
        public bool isBlackList { get; set; } = true;
        public int Rating { get; set; }

        public bool IsSubsidary { get; set; }
        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual Vendor ParentVendor { get; set; }

        public virtual ICollection<Payee> payees { get; set; }
        public virtual ICollection<AdminBillType> AdminBillTypes { get; set; }
        [InverseProperty("vendors")]
        public virtual List<PurchaseInvoice> PurchaseInvoices { get; set; }
        public int? vendorNatureId { get; set; }
        [ForeignKey("vendorNatureId")]
        public virtual VendorNature vendorNature { get; set; }
        public int? vendorNatureManualId { get; set; }
        [ForeignKey("vendorNatureManualId")]
        public virtual VendorNatureManual vendorNatureManual { get; set; }
        public Vendor()
        { }
    }
}

