using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Principal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Person contactPerson { get; set; }
        public virtual Address shippingAddress { get; set; }
        public virtual Address billingAddres { get; set; }
        public virtual Contact contact { get; set; }
        public virtual Company company { get; set; }
        public double targetAmount { get; set; }
        public double marginTargetAmount { get; set; }
        
        public virtual List<Department> departments { get; set; }
        public bool isActive { get; set; } = true;
        public bool IsSubsidary { get; set; }
        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual Principal parentDepartment { get; set; }


    }
}
