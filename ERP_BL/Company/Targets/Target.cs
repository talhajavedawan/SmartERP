using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Target
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }

        public string Code { get; set; }
        public virtual List<TargetAward> TargetAwards { get; set; }
        public int Year { get; set; }
        public int typeId { get; set; }
        [ForeignKey("typeId")]
        public virtual TargetType Type { get; set; }
        public int currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency Currency { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? AchivedDate { get; set; }
        public DateTime? EditDate { get; set; }

        public bool isAchived { get; set; }

        public bool isActive { get; set; } = true;
        public int? companyId { get; set; }
        [ForeignKey("companyId ")]
        public virtual Company Company { get; set; }
        public int? departmentId { get; set; }
        [ForeignKey("departmentId ")]
        public virtual Department Department { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User User { get; set; }



    }
    public class TargetAward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }
        public double target { get; set; }
        public double IndviualAward { get; set; }
        public int NoOfEmployees { get; set; }
        public double TotalAward { get; set; }
        public int Month { get; set; }
        public int? targetId { get; set; }
        [ForeignKey("targetId ")]
        public virtual Target Target { get; set; }
    }
    public class TargetType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HierarchicalIndex { get; set; }
        public string Type { get; set; }
        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }

        public TargetFrequency Frequency { get; set; }
    }
    
}
