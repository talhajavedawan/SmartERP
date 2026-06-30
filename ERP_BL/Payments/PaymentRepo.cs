using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Payments
{
    interface IPaymentRepoAdd
    {
        void AddPayment(List<Payment> payments);
        void AddPaymentMethod(PaymentMethod method);
        void AddPaymentStatus(PaymentStatus paymentStatus);
    }

    interface IPaymentRepoUpdate
    {
        void UpdatePayments(List<Payment> payments);
        void UpdatePaymentStatus(PaymentStatus paymentStatus);
        void UpdatePaymentMethod(PaymentMethod method);
    }

    interface IPaymentRepoGet
    {
        List<Payment> GetPaymentsByGroupId(int transactionGroupId);
    }
    

    public class PaymentRepo : IPaymentRepoAdd, IPaymentRepoUpdate, IPaymentRepoGet
    {
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Get All groups by Creator Id
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllTaskGroupsByUser(int userId)
        {
            var groups = context.taskGroups

                .Where(x => x.users.FirstOrDefault(y => y.id == userId) != null && x.isBackground == false)
                .ToList();
            if (groups.Count > 0)
            {
                return groups;
            }
            else
            {
                return null;
            }

        }

        public void AddPayment(List<Payment> payments)
        {
            if (payments != null && payments.Count > 0)
            {
               
                foreach (var _payment in payments)
                {
                    var departments = _payment.departments;

                    if (departments != null)
                    {
                        _payment.departments = new List<Department>();
                        foreach (var _dept in departments)
                        {
                            var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                            _payment.departments.Add(department);
                        }
                    }

                    if (_payment.pettyCashes != null && _payment.pettyCashes.Count > 0)
                    {
                        if (_payment.transactionType == Enums.PaymentTransactionType.Admin_Bills)
                        {
                            var alltransactions1 = context.pettyCashes.Where(x => x.AdminBillId == _payment.AdminBill_Id).ToList();
                            var adminBill = context.adminBills.FirstOrDefault(x => x.Id == _payment.AdminBill_Id);
                            context.pettyCashes.RemoveRange(alltransactions1);
                            adminBill.isDeposit = null;
                            adminBill.BillRefNoId = null;
                            adminBill.isAmountOC = null;
                        }
                        if (_payment.transactionType == Enums.PaymentTransactionType.Loans_Advances)
                        {
                            var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == _payment.LoansAdvanceId).ToList();
                            var loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == _payment.LoansAdvanceId);
                            context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                            loansAdvance.isDeposit = null;
                            loansAdvance.PettyCashRefId = null;
                            //loansAdvance. = null;
                        }
                        if (_payment.transactionType == Enums.PaymentTransactionType.Vendor_Bills)
                        {
                            var alltransactions1 = context.pettyCashes.Where(x => x.billId == _payment.Bill_Id).ToList();
                            var bill = context.bills.FirstOrDefault(x => x.Id == _payment.Bill_Id);
                            context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                            bill.isDeposit = null;
                            bill.PettyCashRefId = null;
                            //loansAdvance. = null;
                        }
                    }
                    
                    if (_payment.journalTransactions == null)
                    {

                    }
                    else
                    if (_payment.journalTransactions.Count != 0)
                    {
                        foreach (var jt in _payment.journalTransactions)
                        {
                            var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                            if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                            {
                                jt.total = jt.total * -1;
                            }

                        }

                    }

                    context.payments.Add(_payment);
                }
               
                context.SaveChanges();
            }
        }

        public ERP_BL.Databases.Employee GetEmployeeForPayments(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        public PurchaseInvoice getPIForJournalTransactions(int purchaseOrderId)
        {
            return context.purchaseInvoices
                .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public LoansAdvance getLAForJournalTransactions(int loansAdvanceId)
        {
            return context.loansAdvances
                .FirstOrDefault(x => x.Id == loansAdvanceId);
        }

        public TargetRewards getTRForJournalTransactions(int targetRewardId)
        {
            return context.targetRewards
                .FirstOrDefault(x => x.Id == targetRewardId);
        }

        /// <summary>
        /// Get bill based on Bill ID.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public Bill getBillForJournalTransactions(int billId)
        {
            return context.bills
            .FirstOrDefault(x => x.Id == billId);
        }

        public void UpdatePayments(List<Payment> payments)
        {
            try
            {
                if (payments != null && payments.Count > 0)
                {
                    foreach (Payment _payment in payments)
                    {
                        if (_payment.Id > 0)
                        {


                            var payment = context.payments.FirstOrDefault(x => x.Id == _payment.Id);

                            payment = _payment;
                            if (payment.pettyCashes != null && payment.pettyCashes.Count != 0)
                            {
                                var alltransactions = context.pettyCashes.Where(x => x.PaymentId == payment.Id).ToList();
                                if (alltransactions.Count > 0)
                                    context.pettyCashes.RemoveRange(alltransactions);
                                if (payment.transactionType == Enums.PaymentTransactionType.Admin_Bills)
                                {
                                    var alltransactions1 = context.pettyCashes.Where(x => x.AdminBillId == payment.AdminBill_Id).ToList();
                                    context.pettyCashes.RemoveRange(alltransactions1);
                                    payment.adminBill.isDeposit = null;
                                    payment.adminBill.BillRefNoId = null;
                                    payment.adminBill.isAmountOC = null;
                                }
                                if (payment.transactionType == Enums.PaymentTransactionType.Loans_Advances)
                                {
                                    var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == payment.LoansAdvanceId).ToList();
                                    context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                                    payment.loansAdvance.isDeposit = null;
                                    payment.loansAdvance.PettyCashRefId = null;
                                }
                                if (payment.transactionType == Enums.PaymentTransactionType.Vendor_Bills)
                                {
                                    var alltransactions1 = context.pettyCashes.Where(x => x.billId == payment.Bill_Id).ToList();
                                    context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                                    payment.Bill.isDeposit = null;
                                    payment.Bill.PettyCashRefId = null;
                                }
                            }
                            payment.pettyCashes = _payment.pettyCashes;
                            if (payment.VATBooks != null && payment.VATBooks.Count != 0)
                            {
                                var alltransactions = context.VATBooks.Where(x => x.paymentId == payment.Id).ToList();
                                if (alltransactions.Count > 0)
                                    context.VATBooks.RemoveRange(alltransactions);
                                if (payment.transactionType == Enums.PaymentTransactionType.Admin_Bills)
                                {
                                    var alltransactions1 = context.VATBooks.Where(x => x.adminBillId == payment.AdminBill_Id).ToList();
                                    context.VATBooks.RemoveRange(alltransactions1);
                                    payment.isVATBookPosted = true;
                                }
                                if (payment.transactionType == Enums.PaymentTransactionType.Loans_Advances)
                                {
                                    var alltransactions1 = context.VATBooks.Where(x => x.loansAdvanceId == payment.LoansAdvanceId).ToList();
                                    context.VATBooks.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                                    payment.isVATBookPosted = true;
                                }
                            }
                            payment.VATBooks = _payment.VATBooks;
                            if (payment.journalTransactions.Count != 0)
                            {
                                try
                                {
                                    var allTransactions = context.journalTransactions.Where(x => x.PaymentId == payment.Id).ToList();
                                    context.journalTransactions.RemoveRange(allTransactions);
                                }
                                catch (Exception ex)
                                {
                                    var allTransactions = context.journalTransactions.Where(x => x.PaymentId == payment.Id).ToList();
                                    context.journalTransactions.RemoveRange(allTransactions);

                                }
                            }
                            payment.journalTransactions = _payment.journalTransactions;

                            if (_payment.journalTransactions == null)
                            {

                            }
                            else
                            if (_payment.journalTransactions.Count != 0)
                            {
                                foreach (var jt in _payment.journalTransactions)
                                {
                                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                                    if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                                    {
                                        jt.total = jt.total * -1;
                                    }

                                }

                            }
                        }
                        else
                        {
                            var departments = _payment.departments;

                            if (departments != null)
                            {
                                _payment.departments = new List<Department>();
                                foreach (var _dept in departments)
                                {
                                    var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                                    _payment.departments.Add(department);
                                }
                            }

                            if (_payment.pettyCashes != null && _payment.pettyCashes.Count > 0)
                            {
                                if (_payment.transactionType == Enums.PaymentTransactionType.Admin_Bills)
                                {
                                    var alltransactions1 = context.pettyCashes.Where(x => x.AdminBillId == _payment.AdminBill_Id).ToList();
                                    var adminBill = context.adminBills.FirstOrDefault(x => x.Id == _payment.AdminBill_Id);
                                    context.pettyCashes.RemoveRange(alltransactions1);
                                    adminBill.isDeposit = null;
                                    adminBill.BillRefNoId = null;
                                    adminBill.isAmountOC = null;
                                }
                                if (_payment.transactionType == Enums.PaymentTransactionType.Loans_Advances)
                                {
                                    var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == _payment.LoansAdvanceId).ToList();
                                    var loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == _payment.LoansAdvanceId);
                                    context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.credit != 0).ToList());
                                    loansAdvance.isDeposit = null;
                                    loansAdvance.PettyCashRefId = null;
                                    //loansAdvance. = null;
                                }
                            }

                            if (_payment.journalTransactions == null)
                            {

                            }
                            else
                            if (_payment.journalTransactions.Count != 0)
                            {
                                foreach (var jt in _payment.journalTransactions)
                                {
                                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                                    if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                                    {
                                        jt.total = jt.total * -1;
                                    }

                                }

                            }

                            context.payments.Add(_payment);
                        }
                    }
                    context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        public Payment GetPayment(int paymentId)
        {
            if (paymentId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.payments
                .FirstOrDefault(x => x.Id == paymentId);
        }

        public Payment GetPaymentByGroupId(int groupId)
        {
            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.payments

                .FirstOrDefault(x => x.transactionGroupId == groupId);
        }
        public Payment GetPaymentByGroupId(int groupId, double amount)
        {
            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.payments
                .FirstOrDefault(x => x.transactionGroupId == groupId && x.DebitedAmount==amount);
        }

        public List<Payment> getPaymentsByLAid(int LAid , int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            if (LAid == 0)
                throw new NullReferenceException("Object can not be null");

            return context.payments
                .Where(x => x.LoansAdvanceId == LAid && ((deptIds.Intersect(x.departments.Select(y => y.Id)).Count() > 0 && companyIds.Contains((int)x.company_Id)) || (deptIds.Contains(x.InterDepartment_Id.Value) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)))
                .ToList();
        }

        /// <summary>
        /// Approve Existing Bills
        /// </summary>
        /// <param name="bills"></param>
        /// <param name="groupId"></param>
        public void ApprovePayment(List<Payment> payments, List<Department> depts)
        {
            if (payments != null)
            {
                var deptIds = depts.Select(x=>x.Id).ToList();
                foreach (Payment payment in payments)
                {
                    var _payment = context.payments.FirstOrDefault(x => x.Id == payment.Id);
                    _payment = payment;

                    _payment.departments.Clear();
                    if(deptIds != null && deptIds.Count > 0)
                    {
                        _payment.departments = new List<Department>();
                        foreach (var _id in deptIds)
                        {
                            if (_payment.departments.FirstOrDefault(x=>x.Id == _id) == null)
                            {
                                var dept = context.Departments.FirstOrDefault(x => x.Id == _id);
                                _payment.departments.Add(dept);
                            }
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        public void setPaymentsToVoid(List<Payment> _payments,int groupId, bool isVoid)
        {
            List<Payment> payments = GetPaymentsByGroupId(groupId);

            foreach (Payment _payment in payments)
            {
                var payment = context.payments.FirstOrDefault(x => x.Id == _payment.Id);
                if(_payments.FirstOrDefault(x=>x.Id==_payment.Id).transactionHolderId!=_payment.transactionHolderId)
                {
                    payment.holderChangeDate = DateTime.Now;
                }
                context.costSheetPaymentFields.RemoveRange(context.costSheetPaymentFields.Where(x => x.Payment_Id == payment.Id).ToList());
                payment.transactionHolderId= _payments.FirstOrDefault(x => x.Id == _payment.Id).transactionHolderId;
                payment.isVoid = isVoid;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Get All Admin Bill Payments by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetPaymentsByGroupId(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");
            var payments = context.payments

                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();
            return payments;
        }

        /// <summary>
        /// Get All Payments for Approval
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetPaymentForApproval(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");
            var payments = context.payments
                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();
            return payments;
        }

        /// <summary>
        /// Get All Admin Bill Payments by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetAdminBillPaymentsByGroupId(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");
            var payments = context.payments
          
                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();
            return payments;
        }

        /// <summary>
        /// Get All Vendor Bill Payments by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetVendorBillPaymentsByGroupId(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");

            var payments = context.payments
                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();

            return payments;
        }


        /// <summary>
        /// Get All Admin Bill Payments by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetPIpaymentsByGroupId(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");

            var payments = context.payments
                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();

            return payments;
        }


        /// <summary>
        /// Get All Admin Bill Payments by Group Id
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetLApaymentsByGroupId(int transactionGroupId)
        {
            if (transactionGroupId == 0)
                throw new NullReferenceException("Object can not be null");
            var payments = context.payments

                .Where(x => x.transactionGroupId == transactionGroupId)
                .ToList();
            return payments;
        }


        /// <summary>
        /// Get All Open Payments
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetAllOpenPayments(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x=>x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get All Pending For Approval Payments
        /// </summary>
        /// <returns></returns>
        public List<Payment> GetAllPendingForApprovalPayments(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            //return context.payments
            //    .Where(x => ((deptIds.Intersect(x.departments.Select(y => y.Id)).Count() > 0 && companyIds.Contains((int)x.company_Id)) || (deptIds.Contains(x.InterDepartment_Id.Value) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
            //    .ToList();

            return context.payments
               .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
               && x.isApproved == false && x.isVoid != true)
               .ToList();
        }


        /// <summary>
        /// Get all pending for closing Payments by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Payment> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )

                && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Admin Payments Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Payment> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Void Payment own.
        /// </summary>
        /// <returns></returns>
        public List<Payment> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isVoid == true)
                .ToList();
        }


        public List<Payment> GetAllTransactionsOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }



        /// <summary>
        /// Get all active Payments except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Payment> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Inactive Payments except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Payment> getAllInActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.Status.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Payment> getAllSaleReceiptsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.Status.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get all Payment
        /// </summary>
        /// <returns></returns>
        public List<Payment> getPaymentRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Payments.
        /// </summary>
        /// <returns></returns>
        public List<Payment> getPaymentAdministrator()
        {
            return context.payments
                .Where(x => x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get Count pending Payments by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            //return context.payments
            //    .Where(x => ((deptIds.Intersect(x.departments.Select(y => y.Id)).Count() > 0 && companyIds.Contains((int)x.company_Id)) || (deptIds.Contains(x.InterDepartment_Id.Value) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true).Count();

            return context.payments
               .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
               && x.isApproved == false && x.isVoid != true)
               .Count();
        }



        /// <summary>
        /// Get Count pending Payments by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
              .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
              && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) 
              && x.isApproved == false && x.isVoid != true).Count();
        }

        /// <summary>
        /// Get Own Count pending payments
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
              .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
              && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true).Count();
            
        }



        /// <summary>
        /// Get Count pending payments by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
              .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
              && x.isApproved == true && x.isReApproved == false && x.isVoid != true).Count();
        }


        /// <summary>
        /// Get Count pending payments by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
               .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
               && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) 
               && x.isReApproved == false && x.isApproved == true && x.isVoid != true).Count();
        }


        /// <summary>
        /// Get Own Count pending payments user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
               .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
               && (x.user.employee.EmpId == user.employee.EmpId) 
               && x.isReApproved == false && x.isVoid != true && x.isApproved == true).Count();
        }

        /// <summary>
        /// Get all Void payments for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
               .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
               && x.isVoid == true).Count();
        }


        /// <summary>
        /// Get all Void payments
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.payments
                .Where(x => x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all payments for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isVoid != true).Count();
        }

        /// <summary>
        /// Get all payments  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true).Count();
        }

        /// Get Count payments.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.payments

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all payments.
        /// </summary>
        /// <returns></returns>
        public int getPaymentAdministratorCount()
        {
            return context.payments
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending payments by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true).Count();
        }


        /// <summary>
        /// Get Count pending for closing payments by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true).Count();
        }


        /// <summary>
        /// Get Count pending for closing payments own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();
            
            return context.payments
                .Where(x => (
               (x.loansAdvance != null && ((deptIds.Contains((int)x.loansAdvance.deptId) && companyIds.Contains((int)x.loansAdvance.companyId))))
               || (x.adminBill != null && ((deptIds.Contains((int)x.adminBill.dept_Id) && companyIds.Contains((int)x.adminBill.company_Id))))
               || (x.targetRewards != null && ((deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Count() > 0 && companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(y => y.Id)).Count() > 0)))
               || (x.Bill != null && ((deptIds.Contains(x.Bill.dept_Id) && companyIds.Contains((int)x.Bill.company_Id)) || (deptIds.Contains(x.Bill.InterDepartment_Id.Value) && companyIds.Contains((int)x.Bill.InterCompany_Id) && x.Bill.isInterCompany == true)))
               || (x.purchaseInvoice != null && ((deptIds.Contains(x.purchaseInvoice.dept_Id) && companyIds.Contains((int)x.purchaseInvoice.company_Id)) || (deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) && companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id) && x.purchaseInvoice.isInterCompany == true)))
               )
                && (x.user.employee.EmpId == user.employee.EmpId) 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true).Count();
        }

        /// <summary>
        /// Get Count payments.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.payments

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        public List<PaymentStatus> GetAllOpenPaymentStatus()
        {
            var statusList = context.paymentStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        public List<PaymentStatus> GetAllClosePaymentStatus()
        {
            var statusList = context.paymentStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLastPaymentId()
        {

             var payment = context.payments.OrderByDescending(q => q.Id).FirstOrDefault();
            return payment.transactionGroupId;
        }

        /// <summary>
        /// Add New Payment Status
        /// </summary>
        /// <param name="paymentStatus"></param>
        public void AddPaymentStatus(PaymentStatus paymentStatus)
        {
            context.paymentStatuses.Add(paymentStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Payment Status
        /// </summary>
        /// <param name="paymentStatus"></param>
        public void UpdatePaymentStatus(PaymentStatus paymentStatus)
        {
            PaymentStatus _paymentStatus = context.paymentStatuses.FirstOrDefault(x => x.Id == paymentStatus.Id);
            _paymentStatus.Status = paymentStatus.Status;
            _paymentStatus.isActive = paymentStatus.isActive;
            _paymentStatus.forecolor = paymentStatus.forecolor;
            _paymentStatus.backcolor = paymentStatus.backcolor;
            _paymentStatus.isPaid = paymentStatus.isPaid;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Payment Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public PaymentStatus GetPaymentStatus(int statusId)
        {
            return context.paymentStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Payment Statuses
        /// </summary>
        /// <returns></returns>
        public List<PaymentStatus> GetAllPaymentStatuses()
        {
            return context.paymentStatuses
                //.Include("payments")
                .ToList();
        }


        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="method"></param>
        public void AddPaymentMethod(PaymentMethod method)
        {
            context.paymentMethods.Add(method);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Payment Method
        /// </summary>
        /// <param name="method"></param>
        public void UpdatePaymentMethod(PaymentMethod method)
        {
            PaymentMethod _method= context.paymentMethods.FirstOrDefault(x => x.Id == method.Id);

            _method.MethodName = method.MethodName;
            _method.isActive = method.isActive;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public PaymentMethod GetPaymentMethod(int methodId)
        {
            return
                context.paymentMethods.FirstOrDefault(x => x.Id == methodId);
        }


        /// <summary>
        /// Get All Payment Methods
        /// </summary>
        /// <returns></returns>
        public List<PaymentMethod> GetAllPaymentMethods()
        {
            return
            context.paymentMethods
            .ToList();
        }
        public List <Payment> GetAllPaymentsByPIId(int purchaseInvoiceId)
        {

            return context.payments
                .Where(x => x.PInvoice_Id == purchaseInvoiceId).ToList(); ;
        }

        public Payment GetPaymentForCostSheet(int paymentId)
        {
            if (paymentId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.payments
                .FirstOrDefault(x => x.Id == paymentId);
        }
        public void updatePaymentForCostSheet(int paymentId, int costSheetId)
        {
            Payment salePaymenttoUpdate = context.payments.FirstOrDefault(x => x.Id == paymentId);
            if (salePaymenttoUpdate != null)
            {
                if (costSheetId != 0)
                {
                    salePaymenttoUpdate.CostSheetId = costSheetId;

                }
                context.SaveChanges();
            }
        }
        public Deduction GetDeduction(int deductionId)
        {
            return context.deductions.FirstOrDefault(x => x.Id == deductionId);
        }
        public List< PaymentDeduction> GetDeductions(int deductionId)
        {
            return context.paymentDeductions.Where(x => x.Id == deductionId).ToList();
        }


        // <summary>
        /// Get Vendor Bill based on Bill Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public Bill GetVendorBill(int billId)
        {
            return context.bills
        
                .FirstOrDefault(x => x.Id == billId);
        }

        // <summary>
        /// Get Purchase Invoice based on PI Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public PurchaseInvoice GetPurchaseInvoice(int piId)
        {
            return context.purchaseInvoices
            
                .FirstOrDefault(x => x.Id == piId);
        }

        // <summary>
        /// Get Purchase Invoice based on PI Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public LoansAdvance GetLoansAdvance(int piId)
        {
            return context.loansAdvances
             
                .FirstOrDefault(x => x.Id == piId);
        }

        // <summary>
        /// Get Admin Bill based on Bill Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public AdminBill GetAdminBill(int billId)
        {
            return context.adminBills
             
                .FirstOrDefault(x => x.Id == billId);
        }

        // <summary>
        /// Get Admin Bill based on Bill Id.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public PurchaseOrder GetPurchaseOrder(int poId)
        {
            return context.purchaseOrders
              
                .FirstOrDefault(x => x.Id == poId);
        }
        public Payment GetForCostSheet(int paymentId)
        {
            return context.payments
                .FirstOrDefault(x => x.Id == paymentId);
        }
        public TargetRewards getTRewardForJournalTransactions(int rewardId)
        {
            return context.targetRewards
                .FirstOrDefault(x => x.Id == rewardId);
        }

        public List<Payment> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            deptIds = user.employee.departments.Select(x => x.Id).ToList();

            List<int> companyIds = new List<int>();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();

            var activeUnpaidPayments = context.payments
    .Where(x => x.Status.isActive==true && !x.Status.isPaid && x.statusId != null && x.isVoid != true);

            var filteredPayments = activeUnpaidPayments.Where(x =>
                (x.loansAdvance != null &&
                 deptIds.Contains((int)x.loansAdvance.deptId) &&
                 companyIds.Contains((int)x.loansAdvance.companyId)) ||

                (x.adminBill != null &&
                 deptIds.Contains((int)x.adminBill.dept_Id) &&
                 companyIds.Contains((int)x.adminBill.company_Id)) ||

                (x.targetRewards != null &&
                 deptIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(d => d.Id)).Any() &&
                 companyIds.Intersect(x.targetRewards.toDoTask.taskGroup.Companies.Select(c => c.Id)).Any()) ||

                (x.Bill != null && (
                     (deptIds.Contains(x.Bill.dept_Id) &&
                      companyIds.Contains((int)x.Bill.company_Id)) ||
                     (x.Bill.isInterCompany == true &&
                      deptIds.Contains(x.Bill.InterDepartment_Id.Value) &&
                      companyIds.Contains((int)x.Bill.InterCompany_Id))
                 )) ||

                (x.purchaseInvoice != null && (
                     (deptIds.Contains(x.purchaseInvoice.dept_Id) &&
                      companyIds.Contains((int)x.purchaseInvoice.company_Id)) ||
                     (x.purchaseInvoice.isInterCompany == true &&
                      deptIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value) &&
                      companyIds.Contains((int)x.purchaseInvoice.InterCompany_Id))
                 ))
            ).ToList();

            return filteredPayments;

        }




    }
}
