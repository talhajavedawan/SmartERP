using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Bankings;
using ERP_BL.Databases;
using ERP_BL.Documents;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks;
using ERP_BL.ToDoTasks.Taskss;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.StatusClass
{
    public class StatusClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClassName { get; set; }
        public TransactionItemType transactionType { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isDisable { get; set; }


        [InverseProperty("inquiryStatusSubClasses")]
        public virtual List<InquiryStatus> inquiryStatuses { get; set; }
        [InverseProperty("offerStatusSubClasses")]
        public virtual List<OfferStatus> offerStatuses { get; set; }
        [InverseProperty("soStatusSubClasses")]
        public virtual List<SaleOrderStatus> soStatuses { get; set; }
        [InverseProperty("siStatusSubClasses")]
        public virtual List<SaleInvoiceStatus> siStatuses { get; set; }
        [InverseProperty("poStatusSubClasses")]
        public virtual List<PurchaseOrderStatus> poStatuses { get; set; }
        [InverseProperty("piStatusSubClasses")]
        public virtual List<PurchaseInvoiceStatus> piStatuses { get; set; }
        [InverseProperty("srStatusSubClasses")]
        public virtual List<SalesReceiptStatus> srStatuses { get; set; }
        [InverseProperty("billStatusSubClasses")]
        public virtual List<BillStatus> billStatuses { get; set; }
        [InverseProperty("laStatusSubClasses")]
        public virtual List<LoansAdvanceStatus> laStatuses { get; set; }

        [InverseProperty("ibtStatusSubClasses")]
        public virtual List<InterBankTransferStatus> ibtStatuses { get; set; }

        [InverseProperty("adminBillStatusSubClasses")]
        public virtual List<AdminBillStatus> adminBillStatuses { get; set; }
        [InverseProperty("paymentStatusSubClasses")]
        public virtual List<ERP_BL.Payments.PaymentStatus> paymentStatuses { get; set; }
        [InverseProperty("taskStatusSubClasses")]
        public virtual List<TasksStatus> taskStatuses { get; set; }
        [InverseProperty("todoTaskStatusSubClasses")]
        public virtual List<ToDoTaskStatus> todoTaskStatuses { get; set; }
        [InverseProperty("targetRewardStatusSubClasses")]
        public virtual List<TargetRewardStatus> targetRewardStatuses { get; set; }
        [InverseProperty("loanStatusSubClasses")]
        public virtual List<LoansStatus> loanStatuses { get; set; }

        [InverseProperty("rentalContractStatusSubClasses")]
        public virtual List<RentalContractStatus> rentalContractStatuses { get; set; }

        [InverseProperty("assetRentalStatusSubClasses")]
        public virtual List<AssetRentalStatus> assetRentalStatuses { get; set; }

        [InverseProperty("rentalInvoiceStatusSubClasses")]
        public virtual List<RentalInvoiceStatus> rentalInvoiceStatuses { get; set; }

        [InverseProperty("rentalOrderStatusSubClasses")]
        public virtual List<RentalOrderStatus> rentalOrderStatuses { get; set; }

        [InverseProperty("tenantRentalStatusSubClasses")]
        public virtual List<TenantRentalStatus> tenantRentalStatuses { get; set; }
        [InverseProperty("moduleContractStatusSubClasses")]
        public virtual List<ModuleContractStatus> moduleContractStatuses { get; set; } 
        [InverseProperty("documentStatusSubClasses")]
        public virtual List<DocumentStatus> documentStatuses { get; set; }

    }

}
