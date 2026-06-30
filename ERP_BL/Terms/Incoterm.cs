using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Incoterm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string term { get; set; }
        public string discription { get; set; }
        public bool isActive { get; set; } = true;

        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }
        [InverseProperty("incoterm")]
        public virtual List<PurchaseOrder> POsAgainstIncoterm{ get; set; }
        [InverseProperty("TitleValue1")]
        public virtual List<PurchaseOrder> POsAgainstTitle1{ get; set; }
        [InverseProperty("TitleValue2")]
        public virtual List<PurchaseOrder> POsAgainstTitle2 { get; set; }

    }
}
