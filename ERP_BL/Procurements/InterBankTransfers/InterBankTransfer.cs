using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.InterBankTransfers
{
    public class InterBankTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? InstrumentDate { get; set; }

        public string FinanceRefNo { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }

        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        public virtual Department department { get; set; }

        public int? emp_Id { get; set; }
        [ForeignKey("emp_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        public double AmountOC { get; set; }

        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }
        public double MER { get; set; }
        public double AmountMER { get; set; }

        public int? bankFrom_Id { get; set; }
        [ForeignKey("bankFrom_Id")]
        public virtual Bank BankFrom { get; set; }

        public int? accountFrom_Id { get; set; }
        [ForeignKey("accountFrom_Id")]
        public virtual Account AccountFrom { get; set; }

        public int? bankTo_Id { get; set; }
        [ForeignKey("bankTo_Id")]
        public virtual Bank BankTo { get; set; }

        public int? accountTo_Id { get; set; }
        [ForeignKey("accountTo_Id")]
        public virtual Account AccountTo { get; set; }

        public TransferType transferType { get; set; }
        public string SystemRefNo { get; set; }
        public string InstrumentNo { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual InterBankTransferStatus interBankTransStatus { get; set; }


        public int? transferMethod_Id { get; set; }
        [ForeignKey("transferMethod_Id")]
        public virtual TranferMethod tranferMethod { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }

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

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        public virtual List<JournalTransaction> journalTransactions { get; set; }

        public double AmountOCPaid { get; set; }
        public double AmountMERPaid { get; set; }

        public TaxFlag taxFlag { get; set; }

        public int? taxTypeId { get; set; }
        [ForeignKey("taxTypeId")]
        public virtual TaxType taxType { get; set; }

        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }

        public double TaxAmountOC { get; set; }
        public double TaxAmountMER { get; set; }

        public int? industryTypeId { get; set; }
        [ForeignKey("industryTypeId")]
        public virtual IndustryType industryType { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        public virtual Vendor vendor { get; set; }

        public int? currencyFromId { get; set; }
        [ForeignKey("currencyFromId")]
        public virtual Currency currencyFrom { get; set; }

        public double AmountFrom { get; set; }
        public double MERfrom { get; set; }
        public double AmountMERfrom { get; set; }

        public int? currencyToId { get; set; }
        [ForeignKey("currencyToId")]
        public virtual Currency currencyTo { get; set; }

        public double AmountTo { get; set; }
        public double MERto { get; set; }
        public double AmountMERto { get; set; }

        public double AmountER { get; set; }

        public string Description { get; set; }
        public DateTime? GLPostingDate { get; set; }

        public virtual List<PettyCash> pettyCashes { get; set; }
        public bool? isDeposit { get; set; }
        public bool? isPettyCashAmountOC { get; set; }

        public int? PettyCashRefId { get; set; }
        [ForeignKey("PettyCashRefId")]
        public virtual BillRefNumber PettyCashRef { get; set; }
        public virtual List<SaleOrder> SaleOrders { get; set; }
        public int paymentGroupId { get; set; }
        public int receiptGroupId { get; set; }
        public int? vendorBillId { get; set; }
        [ForeignKey("vendorBillId")]
        public virtual ERP_BL.Databases.Bill Bill { get; set; }
        public int adminBillGroupId { get; set; }

        public int? adminBillId { get; set; }
        [ForeignKey("adminBillId")]
        public virtual AdminBill AdminBill { get; set; }

        public bool? IsAdjustedVATfrom { get; set; }
        public virtual List<InterBankTransferVAT> InterBankTransferVATfrom { get; set; }
        public virtual List<IBTbankCharges> bankChargesFrom { get; set; }

        public bool? IsAdjustedVATto { get; set; }
        public virtual List<InterBankTransferVAT> InterBankTransferVATto { get; set; }
        public virtual List<IBTbankCharges> bankChargesTo { get; set; }
        public int? stlId { get; set; }
        [ForeignKey("stlId")]
        public virtual STL STL { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
    }

    public class InterBankTransferVAT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }

        public int? InterBankTransferFromId { get; set; }
        [ForeignKey("InterBankTransferFromId")]
        [InverseProperty("InterBankTransferVATfrom")]
        public virtual InterBankTransfer interBankTransferFrom { get; set; }

        public int? InterBankTransferToId { get; set; }
        [ForeignKey("InterBankTransferToId")]
        [InverseProperty("InterBankTransferVATto")]
        public virtual InterBankTransfer interBankTransferTo { get; set; }

        public double Amount { get; set; }

    }

    public class IBTbankCharges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? deduction_Id { get; set; }
        [ForeignKey("deduction_Id")]
        public virtual Deduction deduction { get; set; }

        public int? InterBankTransferFromId { get; set; }
        [ForeignKey("InterBankTransferFromId")]
        [InverseProperty("bankChargesFrom")]
        public virtual InterBankTransfer interBankTransferFrom { get; set; }

        public int? InterBankTransferToId { get; set; }
        [ForeignKey("InterBankTransferToId")]
        [InverseProperty("bankChargesTo")]
        public virtual InterBankTransfer interBankTransferTo { get; set; }

        public double Amount { get; set; }

    }


    public class InterCompanyBankTransfer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }

        public string SystemRefNo { get; set; }
        public string FinanceRefNo { get; set; }

        public DateTime? TransactionDate { get; set; }

        public string InstrumentNo { get; set; }
        public DateTime? InstrumentDate { get; set; }

        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual InterBankTransferStatus interBankTransStatus { get; set; }

        public int? transferMethod_Id { get; set; }
        [ForeignKey("transferMethod_Id")]
        public virtual TranferMethod tranferMethod { get; set; }

        public virtual ICollection<ProcurementProduct> products { get; set; }

        public double AmountOC { get; set; }

        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public double MER { get; set; }
        public double AmountMER { get; set; }

        public int? companyFrom_Id { get; set; }
        [ForeignKey("companyFrom_Id ")]
        public virtual Company companyFrom { get; set; }

        public int? deptFrom_Id { get; set; }
        [ForeignKey("deptFrom_Id")]
        public virtual Department departmentFrom { get; set; }

        public int? bankFrom_Id { get; set; }
        [ForeignKey("bankFrom_Id")]
        public virtual Bank BankFrom { get; set; }

        public int? accountFrom_Id { get; set; }
        [ForeignKey("accountFrom_Id")]
        public virtual Account AccountFrom { get; set; }

        public int? companyTo_Id { get; set; }
        [ForeignKey("companyTo_Id ")]
        public virtual Company companyTo { get; set; }

        public int? deptTo_Id { get; set; }
        [ForeignKey("deptTo_Id")]
        public virtual Department departmentTo { get; set; }

        public int? bankTo_Id { get; set; }
        [ForeignKey("bankTo_Id")]
        public virtual Bank BankTo { get; set; }

        public int? accountTo_Id { get; set; }
        [ForeignKey("accountTo_Id")]
        public virtual Account AccountTo { get; set; }


        public int? emp_Id { get; set; }
        [ForeignKey("emp_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }

        public TransferType transferType { get; set; }

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

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public  virtual ERP_BL.Databases.User user { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        public virtual List<JournalTransaction> journalTransactions { get; set; }

        public int? COAdebit_Id { get; set; }
        [ForeignKey("COAdebit_Id")]
        public virtual ChartofAccount COAdebit { get; set; }

        public int? COAcredit_Id { get; set; }
        [ForeignKey("COAcredit_Id")]
        public virtual ChartofAccount COAcredit { get; set; }

        public int? currencyFromId { get; set; }
        [ForeignKey("currencyFromId")]
        public virtual Currency currencyFrom { get; set; }

        public double AmountFrom { get; set; }
        public double MERfrom { get; set; }
        public double AmountMERfrom { get; set; }

        public int? currencyToId { get; set; }
        [ForeignKey("currencyToId")]
        public virtual Currency currencyTo { get; set; }

        public double AmountTo { get; set; }
        public double MERto { get; set; }
        public double AmountMERto { get; set; }

        public double AmountER { get; set; }

        public string Description { get; set; }
        public DateTime? GLPostingDate { get; set; }

        public virtual List<PettyCash> pettyCashesFrom { get; set; }
        public virtual List<PettyCash> pettyCashesTo { get; set; }

        public bool? isDepositFrom { get; set; }
        public bool? isDepositTo { get; set; }
        public bool? isAmountOCfrom { get; set; }
        public bool? isAmountOCto { get; set; }

        public int? PettyCashRefFromId { get; set; }
        [ForeignKey("PettyCashRefFromId")]
        public virtual BillRefNumber PettyCashRefFrom { get; set; }

        public int? PettyCashRefToId { get; set; }
        [ForeignKey("PettyCashRefToId")]
        public virtual BillRefNumber PettyCashRefTo { get; set; }
        public int paymentGroupId { get; set; }
        public int receiptGroupId { get; set; }
        public int? vendorBillId { get; set; }
        [ForeignKey("vendorBillId")]
        public virtual ERP_BL.Databases.Bill Bill { get; set; }
        public int adminBillGroupId { get; set; }

        public int? adminBillId { get; set; }
        [ForeignKey("adminBillId")]
        public virtual AdminBill AdminBill { get; set; }

        public bool? IsAdjustedVATfrom { get; set; }
        public virtual List<InterCompBankTransferVAT> InterCompBankTransferVATFrom { get; set; }
        public virtual List<InterCompBankTransferCharges> bankChargesFrom { get; set; }

        public bool? IsAdjustedVATto { get; set; }
        public virtual List<InterCompBankTransferVAT> InterCompBankTransferVATto { get; set; }
        public virtual List<InterCompBankTransferCharges> bankChargesTo { get; set; }
        public int? stlId { get; set; }
        [ForeignKey("stlId")]
        public virtual STL STL { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
    }

    public class InterCompBankTransferVAT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }

        public int? InterCompanyBankTransferFromId { get; set; }
        [ForeignKey("InterCompanyBankTransferFromId")]
        [InverseProperty("InterCompBankTransferVATFrom")]
        public virtual InterCompanyBankTransfer InterCompanyBankTransferFrom { get; set; }

        public int? InterCompanyBankTransferToId { get; set; }
        [ForeignKey("InterCompanyBankTransferToId")]
        [InverseProperty("InterCompBankTransferVATto")]
        public virtual InterCompanyBankTransfer InterCompanyBankTransferTo { get; set; }

        public double Amount { get; set; }

    }

    public class InterCompBankTransferCharges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? deduction_Id { get; set; }
        [ForeignKey("deduction_Id")]
        public virtual Deduction deduction { get; set; }

        public int? InterCompanyBankTransferFromId { get; set; }
        [ForeignKey("InterCompanyBankTransferFromId")]
        [InverseProperty("bankChargesFrom")]
        public virtual InterCompanyBankTransfer InterCompanyBankTransferFrom { get; set; }

        public int? InterCompanyBankTransferToId { get; set; }
        [ForeignKey("InterCompanyBankTransferToId")]
        [InverseProperty("bankChargesTo")]
        public virtual InterCompanyBankTransfer InterCompanyBankTransferTo { get; set; }

        public double Amount { get; set; }

    }

    public class InterBankTransferStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<InterBankTransfer> InterBankTransfers { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }

        [InverseProperty("ibtStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> ibtStatusSubClasses { get; set; }

    }

    public class TranferMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String MethodName { get; set; }
        public bool isActive { get; set; }
    }
}
