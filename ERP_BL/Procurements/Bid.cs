using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Databases
{
   public class Bid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string value { get; set; } // value
        public string refNo { get; set; } // refrence number
        public string bankName{ get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expireDate { get; set; }

        public DateTime submitDate { get; set; }

        public bool isactive { get; set; }
    }
}
