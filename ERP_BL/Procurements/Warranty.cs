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
    public class Warranty
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
        [InverseProperty("SOWarranty")]

        public virtual List<PurchaseOrder> POsAgainstSOWarranty { get; set; }
        [InverseProperty("POWarranty")]

        public virtual List<PurchaseOrder> POsAgainstPOWarranty{ get; set; }


    }
}
