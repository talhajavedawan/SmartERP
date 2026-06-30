using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{

    public class JournalEntryRepo
    {
        DBContextERP context = new DBContextERP();
        public void AddJournalTransaction(List<JournalTransaction> transactions)
        {
            if (transactions != null)
            {
               context.journalTransactions.AddRange(transactions);    
               context.SaveChanges();
            }
        }
        public List<JournalTransaction> GetAllJournalTransactionByaccountId(int id,List<int> departmentIds)
        {
            return context.journalTransactions

                .Where(x => x.accountId == id 
                && x.journalVoucher.isVoid != true 
                && x.InterBank.isVoid != true 
                && x.SaleInvoice.isVoid != true 
                && x.PurchaseInvoice.isVoid != true 
                && x.AdminBill.isVoid != true 
                && x.Bill.isVoid != true 
                && x.SalesReceipt.isVoid != true
                && x.TargetReward.isVoid != true
                && x.Payment.isVoid != true /*&& departmentIds.Contains((int)x.deptId)*/).ToList();
        }
      
        public List<JournalTransaction> GetAllJournalTransactionByaccountId(int id, List<int> departmentIds, DateTime datePeriod)
        {
            return context.journalTransactions
                .Where(x => x.accountId == id 
                && x.journalVoucher.isVoid != true 
                && x.InterBank.isVoid != true 
                && x.SaleInvoice.isVoid != true 
                && x.AdminBill.isVoid != true 
                && x.SalesReceipt.isVoid != true
                &&
                       x.creationDate != null
                     
                && x.Payment.isVoid!=true 
                &&x.PurchaseInvoice.isVoid!=true 
                &&x.InterCompanyTransfer.isVoid!=true &&
                departmentIds.Contains((int)x.deptId)).ToList();
        }
        public List<JournalTransaction> GetAllJournalTransactionByDepartmentId(int departmentid,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.journalTransactions.Where(x => x.deptId == departmentid && x.journalVoucher.isVoid != true && x.InterBank.isVoid != true && x.SaleInvoice.isVoid != true && deptIds.Contains(departmentid)).ToList();
        }

        public void AddTransaction(JournalTransaction Transaction)
        {
            if(Transaction!=null)
            {
                context.journalTransactions.Add(Transaction);
                context.SaveChanges();
            }
            
        }


        public void AddJV(List<JournalTransaction> transactions)
        {
            context.journalTransactions.AddRange(transactions);
        }
        public List<JournalTransaction> GetAllTransactions()
        {
            return context.journalTransactions.ToList();
        }
        public List<JournalTransaction> GetAllTransactionsforTrialBalance(List<int> departmentIds)
        {
            return context.journalTransactions

                .Where(x => x.account != null && x.SaleInvoice.isVoid != true && x.journalVoucher.isVoid != true && x.InterBank.isVoid != true && x.AdminBill.isVoid != true && x.SalesReceipt.isVoid != true && x.SaleInvoice.isVoid != true && departmentIds.Contains((int)x.deptId) 
                                ).ToList();
        }
       
        public List<JournalTransaction> GetJournalTransactionsByVoucherId(int id)
        {
            return context.journalTransactions.Where(x => x.journalVoucher.Id == id).ToList();
        }
        public JournalTransaction GetAllJournalTransactionById(int id )
        {
            return context.journalTransactions.FirstOrDefault(x => x.Id == id);
        }
        public void  RemoveTransactions(List <JournalTransaction> transactions)
        {
             context.journalTransactions.RemoveRange(transactions);
            context.SaveChanges();
        }
        public void RemoveByCompsnyDep(int compId, int deptId)
        {
            var transactions = context.journalTransactions.Where(x => x.companyId == compId && x.deptId == deptId);
            context.journalTransactions.RemoveRange(transactions);
            context.SaveChanges();
        }
        public double getAllDebits(int id)
        {
            double value = 0;
            var transactions = context.journalTransactions.Where(x=>x.accountId == id).ToList();

            
            return value;



            //var result =  transactions.Sum(x => Convert.ToDouble(x.debit)) - transactions.Sum(x => Convert.ToDouble(x.credit));
            //return result;
        }
        public List<JournalTransaction> getAllReconciledTransactions(int id,List<int> departmentIds)
        {
            return context.journalTransactions.Where(x => x.accountId == id
          && x.journalVoucher.isVoid != true
                && x.InterBank.isVoid != true
                && x.SaleInvoice.isVoid != true
                && x.AdminBill.isVoid != true
                && x.SalesReceipt.isVoid != true
                &&
                       x.creationDate != null

                && x.Payment.isVoid != true
                && x.PurchaseInvoice.isVoid != true
                && x.InterCompanyTransfer.isVoid != true
            && departmentIds.Contains((int)x.deptId) && x.isReconciled == true).ToList();
        }
        public List<JournalTransaction> getAllReconciledTransactions(int id, List<int> departmentIds, DateTime datePeriod)
        {
            return context.journalTransactions.Where(x => x.accountId == id && x.journalVoucher.isVoid != true && x.InterBank.isVoid != true && x.SaleInvoice.isVoid != true && x.AdminBill.isVoid != true && x.SalesReceipt.isVoid != true && departmentIds.Contains((int)x.deptId) && x.creationDate <= datePeriod && x.isReconciled == false).ToList();
        }
        public void UpdateReconciledTransactions(List<JournalTransaction> journalTransactions,DateTime datePeriod)
        {
            foreach(var transaction in journalTransactions)
            {
                var dbTransaction = context.journalTransactions.FirstOrDefault(x => x.Id == transaction.Id);
                dbTransaction.isReconciled = true;
                dbTransaction.reconcilationDate = DateTime.Now;
                var dbCoa = context.ChartofAccounts.FirstOrDefault(x => x.Id == dbTransaction.account.Id);
                dbCoa.reconcilationDate = datePeriod;
            }
            context.SaveChanges();
        }
        public JournalTransaction getTransBYIBCT(int _id)
        {
            return context.journalTransactions.FirstOrDefault(x => x.InterCompanyId == _id);
                 
        }

    }
}
