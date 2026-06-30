using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ERP_BL.Databases
{

    public class InquiryProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UOM { get; set; }
        public string ownDiscription { get; set; }
        public double quantity { get; set; }
        public decimal? Weight { get; set; }


        public int product_Id { get; set; }
        [ForeignKey("product_Id ")]

        public virtual Product product { get; set; }

    }

}
