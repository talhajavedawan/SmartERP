using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.VATBook
{

    public class VATBookRepo
    {
        DBContextERP context = new DBContextERP();
        public void FixNullTransactions()
        {
            var NullTransactions = context.VATBooks.Where(x => x.purchaseInvoiceId == null && x.vendorBillId == null  && x.saleInvoiceId == null /*&& x.saleOrderId == null*/ && x.interBankTransferId == null && x.interCompanyId == null && x.adminBillId == null && x.paymentId == null && x.saleReceiptId == null && x.loansAdvanceId == null).ToList();
            context.VATBooks.RemoveRange(NullTransactions);
            context.SaveChanges();
        }
        public List<VATBook> GetAllPettyCash(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.VATBooks.Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null /*|| x.saleOrderId != null*/ || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
        }
        public List<VATBook> GetAllActiveVATBook(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var VATBooks = context.VATBooks.Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null || x.saleInvoiceId != null || x.paymentId != null/*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        } 
        public List<VATBook> GetAllActiveVATBookByType(int uid, TransactionItemType type)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var VATBooks = context.VATBooks.Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null || x.saleInvoiceId != null || x.paymentId != null/*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public List<VATBook> GetAllActiveWithoutDepartmentalCheck(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var VATBooks = context.VATBooks.Where(x => companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/|| x.paymentId != null || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        }
        public List<VATBook> GetAllActiveWithoutDepartmentalCheckByType(int uid, TransactionItemType type)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var VATBooks = context.VATBooks.Where(x => companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/|| x.paymentId != null || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public List<VATBook> GetAllActiveWithoutCompDepartmentalCheck()
        {
            var VATBooks = context.VATBooks.Where(x => x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/ || x.saleInvoiceId != null || x.paymentId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        }
        public List<VATBook> GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType type)
        {
            var VATBooks = context.VATBooks.Where(x => x.vatBookRefNumber.isActive == true && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/ || x.saleInvoiceId != null || x.paymentId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public List<VATBook> GetAllInActiveVATBook(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var VATBooks = context.VATBooks.Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null /*|| x.saleOrderId != null*/ || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        } 
        public List<VATBook> GetAllInActiveVATBookByType(int uid, TransactionItemType type)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var VATBooks = context.VATBooks.Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null /*|| x.saleOrderId != null*/ || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public List<VATBook> GetAllInActiveWithoutDepartmentalCheck(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var VATBooks = context.VATBooks.Where(x => companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null/*|| x.saleOrderId != null*/ || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        }   
        public List<VATBook> GetAllInActiveWithoutDepartmentalCheckByType(int uid, TransactionItemType type)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var VATBooks = context.VATBooks.Where(x => companyIds.Contains((int)x.companyId) && x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null || x.paymentId != null/*|| x.saleOrderId != null*/ || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public List<VATBook> GetAllInActiveWithoutCompDepartmentalCheck()
        {
            var VATBooks = context.VATBooks.Where(x => x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/ || x.paymentId != null || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true).ToList();
        }
        public List<VATBook> GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType type)
        {
            var VATBooks = context.VATBooks.Where(x => x.vatBookRefNumber.isActive == false && (x.interBankTransferId != null || x.interCompanyId != null || x.adminBillId != null || x.paymentId != null || x.saleReceiptId != null || x.loansAdvanceId != null /*|| x.saleOrderId != null*/ || x.paymentId != null || x.saleInvoiceId != null /*|| x.purchaseOrderId != null*/ || x.purchaseInvoiceId != null || x.vendorBillId != null)).ToList();
            return VATBooks.Where(x => x.adminBill?.isVoid != true
                        && x.salesReceipt?.isVoid != true
                        && x.payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.interCompanyTransfer?.isVoid != true
                        //&& x.saleOrder?.isVoid != true
                        && x.saleInvoice?.isVoid != true
                        //&& x.purchaseOrder?.isVoid != true
                        && x.purchaseInvoice?.isVoid != true
                        && x.vendorBill?.isVoid != true
                        && x.TransactionType==type
                        ).ToList();
        }
        public void AddVATBookReferenceNo(VATBookRefNumber refNumber)
        {
            context.VATBookRefNumbers.Add(refNumber);
            context.SaveChanges();
        }


        /// <summary>
        /// Update VAT Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        public void UpdateVATBookReferenceNo(VATBookRefNumber refNumber)
        {
            VATBookRefNumber _billRefNumber = context.VATBookRefNumbers.FirstOrDefault(x => x.Id == refNumber.Id);

            _billRefNumber.VATBookReferenceNo = refNumber.VATBookReferenceNo;
            _billRefNumber.companyId = refNumber.companyId;
            _billRefNumber.isActive = refNumber.isActive;
            context.SaveChanges();
        }



        /// <summary>
        /// Get VAT Reference Number
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        public VATBookRefNumber GetVATBookReferenceNo(int refId)
        {
            return
                context.VATBookRefNumbers

                .FirstOrDefault(x => x.Id == refId);
        }


        /// <summary>
        /// Get All Active Reference Numbers
        /// </summary>
        /// <returns></returns>
        public List<VATBookRefNumber> GetAllActiveVATBookReferenceNo(int compId)
        {
            return
            context.VATBookRefNumbers

            .Where(x => x.companyId == compId && x.isActive == true)
            .ToList();
        }
        public List<VATBookRefNumber> GetAllActiveVATBookReferenceNo()
        {
            return
            context.VATBookRefNumbers

            .Where(x => x.isActive == true)
            .ToList();
        } 
        public List<VATBookRefNumber> GetAllInActiveVATBookReferenceNo()
        {
            return
            context.VATBookRefNumbers

            .Where(x => x.isActive == true)
            .ToList();
        }

        /// <summary>
        /// Get All Reference Numbers
        /// </summary>
        /// <returns></returns>
        public List<VATBookRefNumber> GetAllVATBookRefNo()
        {
            return
            context.VATBookRefNumbers

            .ToList();
        }
        public List<VATBook> GetAllVATBookByType(TransactionItemType type)
        {
            return
            context.VATBooks
            .Where(x => x.TransactionType == type)
            .ToList();
        }
    }
}
