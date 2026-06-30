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
    public class UnBoundReport
    
    {
        //Composite key For defining user settings
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key, Column(Order = 2)]

        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        [Key, Column(Order = 3, TypeName = "VARCHAR")]
        public string reportName { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string template { get; set; }
        public int reportType { get; set; }
        public DateTime lastModified { get; set; }

    }
}
