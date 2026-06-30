using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;

namespace ERP_BL.Databases
{
    public class Land
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public int assetId { get; set; }
        //[ForeignKey("assetId")]
        public virtual Asset asset { get; set; }

        //public int? unitId { get; set; }
        //[ForeignKey("unitId")]
        public virtual Area measureUnit { get; set; }

        public bool isMortgaged { get; set; }
        //public int? mortgeeId { get; set; }
        //[ForeignKey("mortgeeId")]
        public virtual Mortgagee mortgagee { get; set; }
        public double mortgagedValue { get; set; }
        public virtual OfficialAuth OfficialAuths { get; set; }
    }

    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public int assetId { get; set; }
        //[ForeignKey("assetId")]
        public virtual Asset asset { get; set; }

        //public int? unitId { get; set; }
        //[ForeignKey("unitId")]
        public virtual Area measureUnit { get; set; }
        public int Floors { get; set; }

        public bool isMortgaged { get; set; }
        //public int? mortgeeId { get; set; }
        //[ForeignKey("mortgeeId")]
        public virtual Mortgagee mortgagee { get; set; }
        public double mortgagedValue { get; set; }
        public virtual OfficialAuth OfficialAuths { get; set; }
    }

    public class Mortgagee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string  Name { get; set; }
        public bool isActive { get; set; }

    }

    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Length { get; set; }
        public double width { get; set; }
        public MeasureUnitType measureUnitType { get; set; }
    }
}
