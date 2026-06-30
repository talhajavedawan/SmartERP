using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ERP_BL.Procurements.InterBankTransfers
{
    public class InterCompanyBankTransferRepo
    {
        DBContextERP context = new DBContextERP();

        public void AddTransaction(InterCompanyBankTransfer transaction)
        {
            InterCompanyBankTransfer _transaction = new InterCompanyBankTransfer();
            if (transaction.journalTransactions == null)
            {
            }
            else
            if (transaction.journalTransactions.Count != 0)
            {
                foreach (var jt in transaction.journalTransactions)
                {
                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                    if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                    {
                        jt.total = jt.total * -1;
                    }

                }

            }
            _transaction = transaction;
            //if (transaction == null)
            //
            context.interCompanyBankTransfers.Add(_transaction);
            context.SaveChanges();
        }
        //update function start

        public void updateInterBankTransfer(InterCompanyBankTransfer interBankTransfer, List<ProcurementProduct> products) 
        {

            if (interBankTransfer == null)
                throw new NullReferenceException("Object can not be null");
           
            var _interBankTransfer = context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == interBankTransfer.Id);
            _interBankTransfer.transferType = interBankTransfer.transferType;
            //transection from section
            if (interBankTransfer.companyFrom_Id != null)
                _interBankTransfer.companyFrom_Id =  interBankTransfer.companyFrom_Id;
            if (interBankTransfer.deptFrom_Id != null)
                _interBankTransfer.deptFrom_Id = interBankTransfer.deptFrom_Id;
            if (interBankTransfer.bankFrom_Id != null)
                _interBankTransfer.bankFrom_Id = interBankTransfer.bankFrom_Id;
            if (interBankTransfer.accountFrom_Id != null)
                _interBankTransfer.accountFrom_Id = interBankTransfer.accountFrom_Id;
            _interBankTransfer.COAdebit_Id = interBankTransfer.COAdebit_Id;

            //transection to section
            if (interBankTransfer.companyTo_Id != null)
                _interBankTransfer.companyTo_Id = interBankTransfer.companyTo_Id;
            if (interBankTransfer.deptTo_Id != null)
                _interBankTransfer.deptTo_Id =  interBankTransfer.deptTo_Id;
            if (interBankTransfer.bankTo_Id != null)
                _interBankTransfer.bankTo_Id = interBankTransfer.bankTo_Id;
            if (interBankTransfer.accountTo_Id != null)
                _interBankTransfer.accountTo_Id = interBankTransfer.accountTo_Id;

            if (interBankTransfer.currency_Id != null)
                _interBankTransfer.currency_Id =  interBankTransfer.currency_Id;
            _interBankTransfer.COAcredit_Id = interBankTransfer.COAcredit_Id;


            _interBankTransfer.currencyFromId = interBankTransfer.currencyFromId;
            _interBankTransfer.AmountMERfrom = interBankTransfer.AmountMERfrom;
            _interBankTransfer.MERfrom = interBankTransfer.MERfrom;
            _interBankTransfer.AmountFrom = interBankTransfer.AmountFrom;
            _interBankTransfer.currencyToId = interBankTransfer.currencyToId;
            _interBankTransfer.AmountTo = interBankTransfer.AmountTo;
            _interBankTransfer.AmountMERto = interBankTransfer.AmountMERto;
            _interBankTransfer.MERto = interBankTransfer.MERto;
            _interBankTransfer.AmountER = interBankTransfer.AmountER;
            _interBankTransfer.Description = interBankTransfer.Description;

            //Second section
            _interBankTransfer.AmountOC = interBankTransfer.AmountOC;
            //_interBankTransfer.currency = interBankTransfer.currency;
            _interBankTransfer.MER = interBankTransfer.MER;
            _interBankTransfer.AmountMER = interBankTransfer.AmountMER;

            _interBankTransfer.CreationDate = interBankTransfer.CreationDate;
            _interBankTransfer.SystemRefNo = interBankTransfer.SystemRefNo;
            _interBankTransfer.FinanceRefNo = interBankTransfer.FinanceRefNo;
            _interBankTransfer.TransactionDate = interBankTransfer.TransactionDate;
            _interBankTransfer.InstrumentNo = interBankTransfer.InstrumentNo;
            _interBankTransfer.InstrumentDate = interBankTransfer.InstrumentDate;

            _interBankTransfer.PettyCashRefFromId = interBankTransfer.PettyCashRefFromId;
            _interBankTransfer.PettyCashRefToId = interBankTransfer.PettyCashRefToId;
            if (interBankTransfer.journalTransactions == null)
            {
            }
            else
           if (interBankTransfer.journalTransactions.Count != 0)
            {
                foreach (var jt in interBankTransfer.journalTransactions)
                {
                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                    if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                    {
                        jt.total = jt.total * -1;
                    }

                }

            }


            if (interBankTransfer.tranferMethod != null)
                _interBankTransfer.tranferMethod = context.tranferMethods.FirstOrDefault(x => x.Id == interBankTransfer.transferMethod_Id);
            if (interBankTransfer.StatusId != null)
                _interBankTransfer.interBankTransStatus = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == interBankTransfer.StatusId);

            _interBankTransfer.products = interBankTransfer.products; //GetInterCompanyBankTransfer(interBankTransfer.Id).products;
            //_interBankTransfer.products.Clear();
            //_interBankTransfer.products = products;

            _interBankTransfer.stage = interBankTransfer.stage;
            _interBankTransfer.isReviewed = interBankTransfer.isReviewed;
            _interBankTransfer.needReview = interBankTransfer.needReview;
            _interBankTransfer.PendingForClosing = interBankTransfer.PendingForClosing;
            _interBankTransfer.PendingForReApproval = interBankTransfer.PendingForReApproval;
            _interBankTransfer.isApproved = interBankTransfer.isApproved;
            _interBankTransfer.ApprovedDate = interBankTransfer.ApprovedDate;
            _interBankTransfer.isReApproved = interBankTransfer.isReApproved;
            _interBankTransfer.ReApprovalDate = interBankTransfer.ReApprovalDate;

            if (interBankTransfer.user_Id != null)
                _interBankTransfer.user_Id = interBankTransfer.user_Id;


            if (interBankTransfer.ClosingDate != null)
                _interBankTransfer.ClosingDate = interBankTransfer.ClosingDate;

            if (interBankTransfer.LastStatusChangeDate != null)
                _interBankTransfer.LastStatusChangeDate = interBankTransfer.LastStatusChangeDate;
            if (_interBankTransfer.journalTransactions.Count != 0)
            {
                context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.InterCompanyId == interBankTransfer.Id));
            }
            else
            {
                _interBankTransfer.journalTransactions = null;
            }
            if (_interBankTransfer.VATBooks.Count != 0)
            {
                var vatBooksRemove = context.VATBooks.Where(x => x.interCompanyId == interBankTransfer.Id).ToList();

                if (vatBooksRemove != null && vatBooksRemove.Count > 0)
                    context.VATBooks.RemoveRange(vatBooksRemove);
            }
            else
            {
                _interBankTransfer.VATBooks = null;
            }
            foreach (var prod in _interBankTransfer.products)
            {
                if (prod.Id != 0)
                {
                    context.inquiryProducts.Remove(context.inquiryProducts.FirstOrDefault(x => x.Id == prod.product_Id));
                    context.procurementProducts.Remove(context.procurementProducts.FirstOrDefault(x => x.Id == prod.Id));
                }
            }

            if (_interBankTransfer.pettyCashesFrom != null)
            {
                var alltransactions = context.pettyCashes.Where(x => x.InterCompanyId == _interBankTransfer.Id).ToList();
                if(alltransactions != null && alltransactions.Count > 0)
                    context.pettyCashes.RemoveRange(alltransactions);
                _interBankTransfer.pettyCashesFrom = interBankTransfer.pettyCashesFrom;
            }
            if (_interBankTransfer.pettyCashesTo != null)
            {
                var alltransactions = context.pettyCashes.Where(x => x.InterCompanyId == _interBankTransfer.Id).ToList();
                if (alltransactions != null && alltransactions.Count > 0)
                    context.pettyCashes.RemoveRange(alltransactions);
                _interBankTransfer.pettyCashesTo = interBankTransfer.pettyCashesTo;
            }


            context.SaveChanges();
        }

        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public int getAllActiveandTransactionsCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interCompanyBankTransfers

            //.Where(x => x.interBankTransStatus.isActive == true)
            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Get all Inactive Inter-Bank Transfers except pending for closing.
        /// </summary>
        /// <returns></returns>
        public int getAllInActiveTransactionsCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interCompanyBankTransfers

            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
            //.Where(x => x.interBankTransStatus.isActive == false)
            .Count();
        }


        public void approveInterBankTransfer(InterCompanyBankTransfer interBankTransfer, List<ProcurementProduct> products)
        {
            if (interBankTransfer == null)
                throw new NullReferenceException("Object can not be null");
            if (interBankTransfer.companyFrom == null)
                throw new NullReferenceException("Company Object is null");
            if (interBankTransfer.departmentFrom == null)
                throw new NullReferenceException("Department object is null");


            var _interBankTransfer = context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == interBankTransfer.Id);
            //_interBankTransfer = interBankTransfer;
            //_interBankTransfer = interBankTransfer;

            _interBankTransfer.transferType = interBankTransfer.transferType;

            //transection from section
            if (interBankTransfer.companyFrom != null)
                _interBankTransfer.companyFrom = context.Companies.FirstOrDefault(x => x.Id == interBankTransfer.companyFrom.Id);
            if (interBankTransfer.departmentFrom != null)
                _interBankTransfer.departmentFrom = context.Departments.FirstOrDefault(x => x.Id == interBankTransfer.departmentFrom.Id);
            if (interBankTransfer.BankFrom != null)
                _interBankTransfer.BankFrom = context.banks.FirstOrDefault(x => x.Id == interBankTransfer.BankFrom.Id);
            if (interBankTransfer.AccountFrom != null)
                _interBankTransfer.AccountFrom = context.accounts.FirstOrDefault(x => x.Id == interBankTransfer.AccountFrom.Id);

            //transection to section
            if (interBankTransfer.companyTo != null)
                _interBankTransfer.companyTo = context.Companies.FirstOrDefault(x => x.Id == interBankTransfer.companyFrom.Id);
            if (interBankTransfer.departmentTo != null)
                _interBankTransfer.departmentTo = context.Departments.FirstOrDefault(x => x.Id == interBankTransfer.departmentFrom.Id);
            if (interBankTransfer.BankTo != null)
                _interBankTransfer.BankTo = context.banks.FirstOrDefault(x => x.Id == interBankTransfer.BankFrom.Id);
            if (interBankTransfer.AccountTo != null)
                _interBankTransfer.AccountTo = context.accounts.FirstOrDefault(x => x.Id == interBankTransfer.AccountFrom.Id);

            if (interBankTransfer.currency != null)
                _interBankTransfer.currency = context.currencies.FirstOrDefault(x => x.Id == interBankTransfer.currency.Id);

            _interBankTransfer.currencyFromId = interBankTransfer.currencyFromId;
            _interBankTransfer.AmountMERfrom = interBankTransfer.AmountMERfrom;
            _interBankTransfer.MERfrom = interBankTransfer.MERfrom;
            _interBankTransfer.AmountFrom = interBankTransfer.AmountFrom;
            _interBankTransfer.currencyToId = interBankTransfer.currencyToId;
            _interBankTransfer.AmountTo = interBankTransfer.AmountTo;
            _interBankTransfer.AmountMERto = interBankTransfer.AmountMERto;
            _interBankTransfer.MERto = interBankTransfer.MERto;
            _interBankTransfer.AmountER = interBankTransfer.AmountER;
            _interBankTransfer.Description = interBankTransfer.Description;

            //Second section
            _interBankTransfer.AmountOC = interBankTransfer.AmountOC;
            //_interBankTransfer.currency = interBankTransfer.currency;
            _interBankTransfer.MER = interBankTransfer.MER;
            _interBankTransfer.AmountMER = interBankTransfer.AmountMER;

            _interBankTransfer.CreationDate = interBankTransfer.CreationDate;
            _interBankTransfer.SystemRefNo = interBankTransfer.SystemRefNo;
            _interBankTransfer.FinanceRefNo = interBankTransfer.FinanceRefNo;
            _interBankTransfer.TransactionDate = interBankTransfer.TransactionDate;
            _interBankTransfer.InstrumentNo = interBankTransfer.InstrumentNo;
            _interBankTransfer.InstrumentDate = interBankTransfer.InstrumentDate;

            _interBankTransfer.interBankTransStatus = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == interBankTransfer.interBankTransStatus.Id);
            _interBankTransfer.tranferMethod = context.tranferMethods.FirstOrDefault(x => x.Id == interBankTransfer.tranferMethod.Id);




            if (interBankTransfer.tranferMethod != null)
                _interBankTransfer.tranferMethod = context.tranferMethods.FirstOrDefault(x => x.Id == interBankTransfer.tranferMethod.Id);
            if (interBankTransfer.interBankTransStatus != null)
                _interBankTransfer.interBankTransStatus = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == interBankTransfer.interBankTransStatus.Id);

            _interBankTransfer.products = GetInterCompanyBankTransfer(interBankTransfer.Id).products;
            _interBankTransfer.products.Clear();
            _interBankTransfer.products = products;

            _interBankTransfer.stage = interBankTransfer.stage;
            _interBankTransfer.isReviewed = interBankTransfer.isReviewed;
            _interBankTransfer.needReview = interBankTransfer.needReview;
            _interBankTransfer.PendingForClosing = interBankTransfer.PendingForClosing;
            _interBankTransfer.PendingForReApproval = interBankTransfer.PendingForReApproval;
            _interBankTransfer.isApproved = interBankTransfer.isApproved;
            _interBankTransfer.ApprovedDate = interBankTransfer.ApprovedDate;
            _interBankTransfer.isReApproved = interBankTransfer.isReApproved;
            _interBankTransfer.ReApprovalDate = interBankTransfer.ReApprovalDate;

            if (interBankTransfer.user != null)
                _interBankTransfer.user = context.Users.FirstOrDefault(x => x.id == interBankTransfer.user.id);

            if (interBankTransfer.ClosingDate != null)
                _interBankTransfer.ClosingDate = interBankTransfer.ClosingDate;

            if (interBankTransfer.LastStatusChangeDate != null)
                _interBankTransfer.LastStatusChangeDate = interBankTransfer.LastStatusChangeDate;
            //  context.interCompanyBankTransfers.Add(_interBankTransfer);
            context.SaveChanges();
        }


        public string getLastSystemReferenceNo()
        {

            var interBankTrans = context.interCompanyBankTransfers.ToList();
            if (interBankTrans == null || interBankTrans.Count == 0)
                return null;

            return interBankTrans.Last().SystemRefNo;
        }

        //update function end

        public InterCompanyBankTransfer GetInterCompanyBankTransfer(int interBankTransId) 
        {
            if (interBankTransId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interCompanyBankTransfers
                .FirstOrDefault(x => x.Id == interBankTransId);
        }




        public List<InterCompanyBankTransfer> getAllInterCompanyBankTransfer(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interCompanyBankTransfers
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getAllActiveandTransactions(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interCompanyBankTransfers
                //.Where(x => x.interBankTransStatus.isActive == true)
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        //start

        /// <summary>
        /// Inter-Bank Transfers Pending for approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }


        //end
        //start

        /// <summary>
        /// Get all Inter-Bank Transfers
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getInterBankTransferRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers


                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isVoid != true)
                .ToList();
        }

        //end

        //start

        /// <summary>
        /// Get all Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getInterBankTransferRegisterAdministrator()
        {
            return context.interCompanyBankTransfers
             .Where(x => x.isVoid != true)
             .ToList();
        }

        //end

        //start

        /// <summary>
        /// Get all pending for closing Inter-Bank Transfer by Departmental
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.PendingForClosing == true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == true)
                .ToList();
        }

        //end

        /// <summary>
        /// Get all Inactive Inter-Bank Transfers except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getAllInActiveTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interCompanyBankTransfers
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.interBankTransStatus.isActive == false)
                .ToList();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getAllTransactionsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers
        

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.interBankTransStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                //.Where(x=>x.interBankTransStatus.Id == StatusId)
                .ToList();
        }

       

        //start

        //#Void
        /// <summary>
        /// Get all Void Inter-Bank Tranfers for user.
        /// </summary>
        /// <returns></returns>
        public List<InterCompanyBankTransfer> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers
              
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isVoid == true)
                .ToList();
        }


        //end
        public void setInterCompanyBankTransfertoVoid(int Id, bool isVoid)
        {
            InterCompanyBankTransfer item = context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }





        public InterBankTransferStatus GetInterBankTransStatus(int statusId)
        {
            var status = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == statusId);
            return status;

        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid) 
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending Inter-Bank Transfers by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending Inter-Bank Transfers by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfer for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interCompanyBankTransfers
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.interCompanyBankTransfers
    .Where(x => x.isVoid == true)
    .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers
                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isVoid != true)
                .Count();
        }


        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.interCompanyBankTransfers

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.interCompanyBankTransfers
                .Where(x => x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all pending Inter-Bank Transfer by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfer by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfers own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interCompanyBankTransfers

                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyIds.Contains(x.companyFrom.Id) || companyIds.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.interCompanyBankTransfers

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public InterBankTransfer GetInterBankTransfer(int interBankTransId)
        {
            if (interBankTransId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers
                .FirstOrDefault(x => x.Id == interBankTransId);
        }
        public List<InterBankTransfer> GetReceiptInterBankTransfers(int groupId, int uid, int company_Id)
        {
            var user = context.Users.FirstOrDefault(x => x.employeeId == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers
                .Where(x=> deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.receiptGroupId==groupId && x.company_Id==company_Id).ToList();
        }
        public List<InterBankTransfer> GetPaymentInterBankTransfers(int groupId, int uid , int company_Id )
        {
            var user = context.Users.FirstOrDefault(x => x.employeeId == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.paymentGroupId == groupId && x.company_Id== company_Id).ToList();
        }
        public List<InterBankTransfer> GetVendorBillInterBankTransfers(int groupId, int uid, int company_Id)
        {
            var user = context.Users.FirstOrDefault(x => x.employeeId == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.vendorBillId == groupId && x.company_Id == company_Id).ToList();
        }
        public List<InterBankTransfer> GetSTLInterBankTransfers(int groupId, int uid , int company_Id)
        {
            var user = context.Users.FirstOrDefault(x => x.employeeId == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.stlId == groupId && x.company_Id == company_Id).ToList();
        }
    }
}
