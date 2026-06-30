using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int assetId { get; set; }
        [ForeignKey("assetId")]
        public virtual Asset asset { get; set; }
        public virtual Manufacturer manufacturer { get; set; }
        public int Model { get; set; }
        public string Chesis { get; set; }
        public string EngineNo { get; set; }
        public string RegNo { get; set; }
        public bool isNew { get; set; }
        public double EngineReading { get; set; }
        public virtual OfficialAuth OfficialAuths { get; set; }
        public virtual VehicleType vehicleType { get; set; }
        public bool LifeTimeToken { get; set; }
        public DateTime tokenValidTill { get; set; }
        public virtual VehicleBuyingStatus BuyingStatus { get; set; }

        public virtual VehicleCurrentStatus currentStatus { get; set; }
        public bool isLeased { get; set; }
        public int? lesseId { get; set; }
        [ForeignKey("lesseId")]
        public Lesee lesee { get;set;}

        public double leasingValue { get; set; }
        public virtual List<ConditionImage> ConditionImages { get; set; }
    }

    public class VehicleCurrentStatus   
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string currentStatusName { get; set; }
        public bool isActive { get; set; }
    }

    public class VehicleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool isActive { get; set; }
    }

    public class ConditionImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int vehicleId { get; set; }
        [ForeignKey("vehicleId")]
        public virtual Vehicle Vehicle { get; set; }
        public string ImageName { get; set; }
        public string location { get; set; }
        public DateTime uploadDate { get; set; }
    }
    public class Lesee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LesseName { get; set; }
        public bool isActive { get; set; }
    }

    public class Manufacturer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ManufacturerName { get; set; }
        public bool isActive { get; set; }
    }
    public class VehicleBuyingStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string buyingStatus { get; set; }
        public bool isNew { get; set; }
    }
}
