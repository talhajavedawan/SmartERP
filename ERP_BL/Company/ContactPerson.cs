using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;

namespace ERP_BL.Databases
{
    public class ContactPerson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Person person { get; set; }
        public virtual Contact contact { get; set; }
        public string designation { get; set; }
        public bool isActive { get; set; }
        public virtual Bank bank { get; set; }

        public int? customerCompanyId { get; set; }
        [ForeignKey ("customerCompanyId")]
        public virtual CustomerCompany customerCompany { get; set; }

         public int? ReligionId { get; set; }
        [ForeignKey ("ReligionId")]
        public virtual Religion religion { get; set; }  

    }
}
