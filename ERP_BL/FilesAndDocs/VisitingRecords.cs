using ERP_BL.Countryy;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.FilesAndDocs
{
    public class TravelingRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? travelerNameId { get; set; }
        [ForeignKey("travelerNameId")]
        public virtual Traveler travelerName { get; set; }

        public int transactionGroupId { get; set; }

        public string SystemRefNo { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual TravelingStatus Status { get; set; }

        public virtual List<VisitingCountry> visitingCountries { get; set; }
        public DateTime? residentYear { get; set; }
        public DateTime? residentFromDate { get; set; }
        public DateTime? residentToDate { get; set; }

        public int? residentCountryId { get; set; }
        [ForeignKey("residentCountryId")]
        public virtual ResidentCountry residentCountry { get; set; }

        public double DaysInResidentCountry { get; set; }
        public double DaysInOtherCountries { get; set; }
        public double TotalDays { get; set; }
        public double RequiredResidentDays { get; set; }

        public string stage { get; set; }

        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }

        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }

        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }


        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
    }

    public class TravelingStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<TravelingRecords> travelingRecords { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
    }

    public class Traveler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<ResidentCountry> ResidentCountries { get; set; }

        public bool isActive { get; set; }
    }

    public class ResidentCountry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public int? travelerId { get; set; }
        [ForeignKey("travelerId")]
        [InverseProperty("ResidentCountries")]
        public virtual Traveler traveler { get; set; }

        public int? countryId { get; set; }
        [ForeignKey("countryId")]
        public virtual Country country { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }

    public class VisitingCountry
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int sequence { get; set; }

        public int transactionGroupId { get; set; }

        public string SystemRefNo { get; set; }

        public string TicketNo { get; set; }

        public int? TravelingRecordsId { get; set; }
        [ForeignKey("TravelingRecordsId")]
        [InverseProperty("visitingCountries")]
        public virtual TravelingRecords travelingRecord { get; set; }

        public int? departingCountryId { get; set; }
        [ForeignKey("departingCountryId")]
        public virtual Country departingCountry { get; set; }

        public int? visitingCountryId { get; set; }
        [ForeignKey("visitingCountryId")]
        public virtual Country visitingCountry { get; set; }

        public int? airlineId { get; set; }
        [ForeignKey("airlineId")]
        public virtual Airline airline { get; set; }
        public DateTime? residentYear { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? LeavingDate { get; set; }

        public bool End { get; set; }

        public ItineraryStatus? itineraryStatus { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public double TicketCost { get; set; }
        public double MER { get; set; }
        public double TicketCostMER { get; set; }
    }

    public class Airline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
