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
   public class Attachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string fileServerAdress{ get; set; }
        public string fileLocalAdress { get; set; }

        public string fileName { get; set; } // refrence name
        public fileType fileType { get; set; }
        public DateTime lastOpendate { get; set; }
        public DateTime additionDate { get; set; }
        public string comment { get; set; } // Comment
        public UploadFlag currentStatus { get; set; }

        public Byte[] thumbnail{ get; set; }

        public bool isactive { get; set; }
        public TransactionItemType transactionType { get; set; }
        public int transactionId { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        public int? categoryId { get; set; }

        [ForeignKey("categoryId")]
        public virtual AttachmentCategory category { get; set; }

    }
    public class AttachmentCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } // refrence name
        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual AttachmentCategory parentCategory{ get; set; }
        public virtual List<Attachment> attachments{ get; set; }
        public DateTime lastChangedDate { get; set; }
        public DateTime additionDate { get; set; }
        public string description { get; set; } //Description
        public Byte[] thumbnail { get; set; }
        public virtual List<TransactionItem> TransactionTypes { get; set; }
        public bool isActive { get; set; }
        //public TransactionItemType TransactionType { get; set; }
        //public int TransactionId { get; set; }
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }
        public  TransactionItemType? Inquiry { get; set; }
        public  TransactionItemType? Offer { get; set; }
        public  TransactionItemType? ModuleContract { get; set; }
        public  TransactionItemType? PO { get; set; }
        public  TransactionItemType? SO { get; set; }
        public  TransactionItemType? SI { get; set; }
        public  TransactionItemType? SR { get; set; }
        public  TransactionItemType? PI { get; set; }
        public  TransactionItemType? Payment { get; set; }
        public  TransactionItemType? VBill { get; set; }
        public  TransactionItemType? ABill { get; set; }
        public  TransactionItemType? IBT { get; set; }
        public  TransactionItemType? ICBT { get; set; }
        public  TransactionItemType? MS { get; set; }
        public TransactionItemType? LoansAdvances { get; set; }
        public TransactionItemType? Tasks { get; set; }
        public TransactionItemType? TargetReward { get; set; }
        public TransactionItemType? TravelingRecord { get; set; }
        public TransactionItemType? STL { get; set; }
        public TransactionItemType? ProcurementProducts { get; set; }
        public TransactionItemType? VehicleExpenses { get; set; }
        public TransactionItemType? RentalContract { get; set; }
        public TransactionItemType? RentalOrder { get; set; }
        public TransactionItemType? RentalInvoice { get; set; }
        public TransactionItemType? Memo { get; set; }
        public TransactionItemType? Document { get; set; }
        public TransactionItemType? AssetRental { get; set; }
        public TransactionItemType? TenantRental { get; set; }
    }
}
