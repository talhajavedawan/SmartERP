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
   public class LoginUserDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }

        public DateTime? LoginTime { get; set; } 
        public DateTime? LogoutTime { get; set; }

        public string crashingDetail { get; set; } 
    }
}
