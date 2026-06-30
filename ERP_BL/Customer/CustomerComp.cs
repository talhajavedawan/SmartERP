using ERP_BL.Procurements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class CustomerCompany
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Person contactPerson { get; set; }
        public virtual Address shippingAddress { get; set; }
        public virtual Address billingAddres { get; set; }
        public virtual Contact contact { get; set; }
        public virtual Company company { get; set; }
        public bool IsSubsidary { get; set; }
        [InverseProperty("customers")]
        public virtual List<Department> departments { get; set; }

        [InverseProperty("disableCustomers")]
        public virtual List<Department> disableDepartments { get; set; }
        public int? ParentID { get; set; }
        public bool isActive { get; set; } = true;
        [ForeignKey("ParentID")]
        public virtual CustomerCompany parentCompany { get; set; }
        [InverseProperty("Customers")]
        public virtual List<Company> Companies { get; set; }
        public virtual List<ContactPerson> ContactPersons { get; set; }
        [InverseProperty("CustomerCompanies")]
        public virtual List<IndustryType> IndustryTypes { get; set; }
        [InverseProperty("customerCompany")]
        public virtual List<AuditYearAdjustment> CustomerAduitAdjustmnets { get; set; }
        public CustomerCompany()
        {
            departments = new List<Department>();
        }
    }
}
