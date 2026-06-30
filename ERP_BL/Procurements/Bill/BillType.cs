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
    public class BillType
    {
        
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string billType { get; set; }
            public bool isActive { get; set; } = true;
            public int? user_Id { get; set; }
            [ForeignKey("user_Id ")]
            public virtual User user { get; set; }

        
    }
}
