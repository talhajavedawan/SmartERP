using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.CashBook
{
    public class PettyCashRepo
    {
        DBContextERP context = new DBContextERP();


        public void FixNullTransactions()
        {
            var NullTransactions = context.pettyCashes.Where(x=>x.interBankTransferId == null && x.InterCompanyId == null && x.AdminBillId == null && x.PaymentId == null && x.SaleReceiptId == null && x.LoansAdvanceId == null && x.billId == null).ToList();
            context.pettyCashes.RemoveRange(NullTransactions);
            context.SaveChanges();
        }


        public List<PettyCash> GetAllPettyCash(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.PettyCashCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.pettyCashes

                .Where(x=> deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();
        }

        public List<PettyCash> GetAllActivePettyCash(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.PettyCashCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var pettyCashes = context.pettyCashes
                .Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.PettyCashRef.isActive == true && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x=> x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }

        public List<PettyCash> GetAllActiveWithoutDepartmentalCheck(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.PettyCashCompanies)
                companyIds.Add(comp.Id);
           
            var pettyCashes = context.pettyCashes
                .Where(x => companyIds.Contains((int)x.companyId) && x.PettyCashRef.isActive == true && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x => x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }

        public List<PettyCash> GetAllActiveWithoutCompDepartmentalCheck()
        {
            var pettyCashes = context.pettyCashes
                .Where(x => x.PettyCashRef.isActive == true && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x => x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }

        public List<PettyCash> GetAllInActivePettyCash(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.PettyCashCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var pettyCashes = context.pettyCashes

                .Where(x => deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId) && x.PettyCashRef.isActive == false && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x => x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }


        public List<PettyCash> GetAllInActiveWithoutDepartmentalCheck(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.PettyCashCompanies)
                companyIds.Add(comp.Id);

            var pettyCashes = context.pettyCashes
                .Where(x => companyIds.Contains((int)x.companyId) && x.PettyCashRef.isActive == false && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x => x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }


        public List<PettyCash> GetAllInActiveWithoutCompDepartmentalCheck()
        {
            var pettyCashes = context.pettyCashes
                .Where(x => x.PettyCashRef.isActive == false && (x.interBankTransferId != null || x.InterCompanyId != null || x.AdminBillId != null || x.PaymentId != null || x.SaleReceiptId != null || x.LoansAdvanceId != null || x.billId != null))
                .ToList();

            return pettyCashes.Where(x => x.adminBill?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.interBankTransfer?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        && x.bill?.isVoid != true).ToList();
        }
    }
}
