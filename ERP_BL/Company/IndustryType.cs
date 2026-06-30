using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;

namespace ERP_BL.Databases
{
    public class IndustryType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }

        public bool isApproved { get; set; }

        public bool isActive { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public DateTime addedDate { get; set; }

        public bool isVendorType { get; set; }
        public bool isVoid { get; set; }

        [InverseProperty("IndustryTypes")]
        public virtual ICollection<CustomerCompany> CustomerCompanies { get; set; } 

        public IndustryType()
        { }
    }
    public class VendorNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }

        public bool isApproved { get; set; }
        public bool isActive { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public DateTime addedDate { get; set; }
        public bool isVoid { get; set; }

        public VendorNature()
        { }
    }
    public class VendorNatureManual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }

        public bool isApproved { get; set; }
        public bool isActive { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public DateTime addedDate { get; set; }
        public bool isVoid { get; set; }

        public VendorNatureManual()
        { }
    }
}
