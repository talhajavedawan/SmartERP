using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP_BL.Databases
{
    public class ViewInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User{ get; set; }
        
        public DateTime Timestamp{ get; set; }
        public string Info { get; set; }
        public string Comment { get; set; }


        public int TransactionType { get; set; }
        public int TransactionId{ get; set; }

    }
}
