using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements
{
    public class InterBankTransRepo
    {
        DBContextERP context = new DBContextERP();

        public void addInterBankTransfer(InterBankTransfer transaction)
        {
            InterBankTransfer _transaction = new InterBankTransfer();

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
            
            if(transaction.transferType == Enums.TransferType.Advances || transaction.transferType == Enums.TransferType.IBT_Single_Currency)
            {
                _transaction.products = transaction.products;

                _transaction.journalTransactions.AddRange(transaction.journalTransactions);
            }
            
            context.interBankTransfers.Add(_transaction);
            context.SaveChanges();
        }

        /// <summary>
        /// Approve and Direct Close method
        /// </summary>
        /// <param name="interBankTransfer"></param>
        /// <param name="products"></param>

        public void approveInterBankTransfer(InterBankTransfer interBankTransfer)
        {
            if (interBankTransfer == null)
                throw new NullReferenceException("Object can not be null");
            if (interBankTransfer.company == null)
                throw new NullReferenceException("Company Object is null");
            if (interBankTransfer.department == null)
                throw new NullReferenceException("Department object is null");
            if (interBankTransfer.employee == null)
                throw new NullReferenceException("Department object is null");

            var _interBankTransfer = context.interBankTransfers.FirstOrDefault(x => x.Id == interBankTransfer.Id);
            _interBankTransfer = interBankTransfer;

            context.SaveChanges();
        }


        /// <summary>
        /// Get list of all Products in DB for a current user
        /// </summary>
        /// <returns>List of Products Objects in db for current user</returns>
        public List<Product> getAllUserProducts(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            //List<int> deptIds = new List<int>();
            List<Product> products = new List<Product>();
            foreach (var dpt in user.employee.departments)
                foreach (var pro in dpt.Products)
                {
                    if (pro.isActive == true)
                        products.Add(pro);
                }
            return products.Distinct().ToList();//context.Products.Where(x => x.departments.Where(y=> deptIds.Contains(y.Id))!=null).ToList();

        }


        public void updateInterBankTransfer(InterBankTransfer interBankTransfer)
        {
            if (interBankTransfer == null)
                throw new NullReferenceException("Object can not be null");
            if (interBankTransfer.company == null)
                throw new NullReferenceException("Company Object is null");
            if (interBankTransfer.department == null)
                throw new NullReferenceException("Department object is null");
            if (interBankTransfer.employee == null)
                throw new NullReferenceException("Department object is null");

            var _interBankTransfer = context.interBankTransfers.FirstOrDefault(x => x.Id == interBankTransfer.Id);

           

            if (_interBankTransfer.pettyCashes != null)
            {
                var alltransactions = context.pettyCashes.Where(x => x.interBankTransferId == _interBankTransfer.Id).ToList();
                if (alltransactions.Count > 0)
                    context.pettyCashes.RemoveRange(alltransactions);
            }

            if(interBankTransfer.currency_Id != null && interBankTransfer.currency_Id > 0)
                _interBankTransfer.currency = context.currencies.FirstOrDefault(x=>x.Id == interBankTransfer.currency_Id);
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

                var allDbtransactions = context.journalTransactions.Where(x => x.InterBankId == _interBankTransfer.Id).ToList();
                if (allDbtransactions.Count > 0)
                    context.journalTransactions.RemoveRange(allDbtransactions);
            }
            if (_interBankTransfer.VATBooks.Count != 0)
            {
                context.VATBooks.RemoveRange(context.VATBooks.Where(x => x.interCompanyId == interBankTransfer.Id));
            }
            else
            {
                _interBankTransfer.VATBooks = null;
            }
            _interBankTransfer = interBankTransfer;

            if (interBankTransfer.currencyFromId != null && interBankTransfer.currencyFromId > 0)
                _interBankTransfer.currencyFrom = context.currencies.FirstOrDefault(x => x.Id == interBankTransfer.currencyFromId);

            if (interBankTransfer.currencyToId != null && interBankTransfer.currencyToId > 0)
                _interBankTransfer.currencyTo = context.currencies.FirstOrDefault(x => x.Id == interBankTransfer.currencyToId);

            //_interBankTransfer.bankFrom_Id = null;
            //_interBankTransfer.bankTo_Id = null;
            //_interBankTransfer.accountFrom_Id = null;
            //_interBankTransfer.accountTo_Id = null;
            //_interBankTransfer.currencyFromId = null;
            //_interBankTransfer.currencyToId = null;

            context.SaveChanges();
        }

        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public int getAllActiveandUnapprovedTransactionsCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers

            .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get all Inactive Inter-Bank Transfers except pending for closing.
        /// </summary>
        /// <returns></returns>
        public int getAllInActiveandUnapprovedReceiptsCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers

            .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
            .Count();
        }


        public List<InterBankTransfer> GetAllInterBankTransfers(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers

            .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
            
                .ToList();
        }
        public List<InterBankTransfer> GetAllInterBankTransfersByUserID(int id)
        {
            return context.interBankTransfers

            .Where(x => x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.interBankTransStatus.isActive == true && x.PendingForClosing != true && x.user_Id==id)
                .ToList();
        }


        public InterBankTransfer GetInterBankTransfer(int interBankTransId)
        {
            if (interBankTransId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.interBankTransfers

                .FirstOrDefault(x => x.Id == interBankTransId);
        }

        public void addTransMethod(TranferMethod tranferMethod)
        {
            if (tranferMethod == null)
                throw new NullReferenceException("Object can not be null");

            context.tranferMethods.Add(tranferMethod);
            context.SaveChanges();
        }

        public void updatedTransferMethod(TranferMethod tranferMethod)
        {
            if (tranferMethod == null)
                throw new NullReferenceException("Object can not be null");
            var _tranferMethod = context.tranferMethods.FirstOrDefault(x => x.Id == tranferMethod.Id);

            _tranferMethod.MethodName = tranferMethod.MethodName;
            _tranferMethod.isActive = tranferMethod.isActive;

            context.SaveChanges();
        }

        public List<TranferMethod> GetAllTransferMethods()
        {
            return context.tranferMethods
                .ToList();
        }

        public TranferMethod GetTransferMethod(int transferMethodId)
        {
            if (transferMethodId == 0)
                throw new NullReferenceException("Object can not be null");

            return context.tranferMethods
                .FirstOrDefault(x => x.Id == transferMethodId);
        }


        public void AddInterBankTransferStatus(InterBankTransferStatus status)
        {
            context.interBankTransferStatuses.Add(status);
            context.SaveChanges();

        }

        public void UpdateInterBankTransferStatus(InterBankTransferStatus status)
        {
            if (status == null)
                throw new NullReferenceException("Object can not be null");
            var _status = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == status.Id);

            _status.Status = status.Status;
            _status.isActive = status.isActive;
            _status.backcolor = status.backcolor;

            context.SaveChanges();

        }

        public InterBankTransferStatus GetInterBankTransStatus(int statusId)
        {
            var status = context.interBankTransferStatuses.FirstOrDefault(x => x.Id == statusId);
            return status;

        }

        public List<InterBankTransferStatus> GetAllInterBankTransStatus()
        {
            var statusList = context.interBankTransferStatuses.ToList();
            return statusList;

        }

        public List<InterBankTransferStatus> GetAllOpenInterBankTransferStatus()
        {
            var statusList = context.interBankTransferStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        public List<InterBankTransferStatus> GetAllCloseInterBankTransferStatus()
        {
            var statusList = context.interBankTransferStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        /// <summary>
        /// Inter-Bank Transfers Pending for approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<InterBankTransfer> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Inter-Bank Transfers Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<InterBankTransfer> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all pending for closing Inter-Bank Transfer by Departmental
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getInterBankTransferRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interBankTransfers
          
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getInterBankTransferRegisterAdministrator()
        {
            return context.interBankTransfers

    .Where(x => x.isVoid != true)
    .ToList();
        }


        //#Void
        /// <summary>
        /// Get all Void Inter-Bank Tranfers for user.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
                .ToList();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfer own.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company.Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getVoidRegisterAdministrator()
        {
            return context.interBankTransfers

        .Where(x => x.isVoid == true)
    .ToList();
        }


        public List<InterBankTransfer> GetAllTransactionsOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers
   
                //.Where(x => x.PendingForClosing == true || x.PendingForClosing == null)
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Inactive Inter-Bank Transfers except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getAllInActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.interBankTransfers
              
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == false)
                .ToList();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<InterBankTransfer> getAllSaleReceiptsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.interBankTransfers
          
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.interBankTransStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }


        public void setInterBankTransfertoVoid(int Id, bool isVoid)
        {
            InterBankTransfer item = context.interBankTransfers.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }


        public string getLastSystemReferenceNo()
        {

            var interBankTrans = context.interBankTransfers.ToList();
            if (interBankTrans == null || interBankTrans.Count == 0)
                return null;

            return interBankTrans.Last().SystemRefNo;
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
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
            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
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
            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
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
            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
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
            return context.interBankTransfers
                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.interBankTransfers
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

            return context.interBankTransfers
                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all Inter-Bank Transfer  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.interBankTransfers
                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.interBankTransfers

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.interBankTransfers
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
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

            return context.interBankTransfers

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.interBankTransfers

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        public List<InterBankTransferStatus> GetAllOpenBankTransferStatus()
        {
            var statusList = context.interBankTransferStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        public List<InterBankTransferStatus> GetAllCloseBankTransferStatus()
        {
            var statusList = context.interBankTransferStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }
        public List<InterBankTransfer> GetInterBankTransfersByPaymentGroupId(int groupId)
        {
           return context.interBankTransfers.Where(x => x.paymentGroupId == groupId)
                .ToList();
        }
        public List<InterBankTransfer> GetInterBankTransfersByReceiptGroupId(int groupId)
        {
           return context.interBankTransfers.Where(x => x.receiptGroupId == groupId)
                .ToList();
        }

        public void FixDuplicateTransactions()
        {
            //var distinctItems = context.procurementProducts.Where(a => a.interBankTransferId != null).ToList();
            //var items = (distinctItems.GroupBy(x => new { x.interBankTransferId, x.inquiryProduct.product_Id }).Select(y => y.First())).ToList();
            //items = distinctItems.Except(items).ToList();
            //if (items.Count() > 0)
            //{
            //    context.procurementProducts.RemoveRange(items);
            //    context.SaveChanges();
            //}
        }

    }
}
