using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.LoansAdvances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class SalesReceiptRepo
    {
        DBContextERP context = new DBContextERP();
        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = GetLastSaleReceiptId();
            if (lastPaymentId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastPaymentId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }


        public void addSalesReceipt(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            foreach (var salesReceipt in saleReceiptList)
            {
                salesReceipt.transactionGroupId = intGroupId;
                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");
                if (salesReceipt.CreationDate == null)
                    salesReceipt.CreationDate = DateTime.Now;
                if (salesReceipt.bank != null)
                    salesReceipt.bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.bank.Id);
                if (salesReceipt.account != null)
                    salesReceipt.account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.account.Id);
                if (salesReceipt.company != null)
                    salesReceipt.company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                if (salesReceipt.Currency != null)
                    salesReceipt.Currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                if (salesReceipt.Customer != null)
                    salesReceipt.Customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                if (salesReceipt.collectionMethod != null)
                    salesReceipt.collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                if (salesReceipt.saleInvoice != null)
                    salesReceipt.saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);
                if (salesReceipt.saleReceiptStatus != null)
                    salesReceipt.saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                if (salesReceipt.principal != null)
                    salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);
                if (salesReceipt.transactionHolderId != null)
                    salesReceipt.TransactionHolder = context.Employees.FirstOrDefault(x => x.EmpId == salesReceipt.transactionHolderId);


                

                if (salesReceipt.journalTransactions == null)
                {

                }
                else
                   if (salesReceipt.journalTransactions.Count != 0)
                {
                    foreach (var jt in salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }
                    }
                }
                context.salesReceipts.Add(salesReceipt);
            }
            context.SaveChanges();
        }


        public void AddLAReceipt(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            foreach (var salesReceipt in saleReceiptList)
            {
                salesReceipt.transactionGroupId = intGroupId;
                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");
                if (salesReceipt.CreationDate == null)
                    salesReceipt.CreationDate = DateTime.Now;
                if (salesReceipt.BankId != 0)
                    salesReceipt.bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.BankId);
                if (salesReceipt.AccountId != 0)
                    salesReceipt.account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.AccountId);
                if (salesReceipt.company != null)
                    salesReceipt.company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                if (salesReceipt.Currency != null)
                    salesReceipt.Currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                if (salesReceipt.Customer != null)
                    salesReceipt.Customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                if (salesReceipt.collectionMethod != null)
                    salesReceipt.collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                if (salesReceipt.saleInvoice != null)
                    salesReceipt.saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);
                if (salesReceipt.saleReceiptStatus != null)
                    salesReceipt.saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                if (salesReceipt.principal != null)
                    salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);
                if (salesReceipt.journalTransactions == null)
                {

                }
                else if (salesReceipt.journalTransactions.Count != 0)
                    foreach (var jt in salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }

                    }

                if (salesReceipt.pettyCashes != null && salesReceipt.pettyCashes.Count > 0)
                {
                    var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == salesReceipt.LoansAdvanceId).ToList();
                    var loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == salesReceipt.LoansAdvanceId);
                    context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.debit != 0).ToList());
                    loansAdvance.isDeposit = null;
                    loansAdvance.PettyCashRefId = null;
                }


                context.salesReceipts.Add(salesReceipt);
            }
            context.SaveChanges();
        }

        public void AddRentalReceipt(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            foreach (var salesReceipt in saleReceiptList)
            {
                salesReceipt.transactionGroupId = intGroupId;
                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");
                if (salesReceipt.CreationDate == null)
                    salesReceipt.CreationDate = DateTime.Now;
                if (salesReceipt.BankId != 0)
                    salesReceipt.bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.BankId);
                if (salesReceipt.AccountId != 0)
                    salesReceipt.account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.AccountId);
                if (salesReceipt.company != null)
                    salesReceipt.company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                if (salesReceipt.Currency != null)
                    salesReceipt.Currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                if (salesReceipt.Customer != null)
                    salesReceipt.Customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                if (salesReceipt.collectionMethod != null)
                    salesReceipt.collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                if (salesReceipt.saleInvoice != null)
                    salesReceipt.saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);
                if (salesReceipt.saleReceiptStatus != null)
                    salesReceipt.saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                if (salesReceipt.principal != null)
                    salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);
                if (salesReceipt.journalTransactions == null)
                {

                }
                else if (salesReceipt.journalTransactions.Count != 0)
                    foreach (var jt in salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }

                    }

                if (salesReceipt.pettyCashes != null && salesReceipt.pettyCashes.Count > 0)
                {
                    var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == salesReceipt.LoansAdvanceId).ToList();
                    var loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == salesReceipt.LoansAdvanceId);
                    context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.debit != 0).ToList());
                    loansAdvance.isDeposit = null;
                    loansAdvance.PettyCashRefId = null;
                }


                context.salesReceipts.Add(salesReceipt);
            }
            context.SaveChanges();
        }

        public void AddCustomerCreditReceipt(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            if (saleReceiptList != null && saleReceiptList.Count > 0)
            {

                foreach (var _receipt in saleReceiptList)
                {
                    _receipt.transactionGroupId = intGroupId;

                    if (_receipt.journalTransactions == null)
                    {

                    }
                    else if (_receipt.journalTransactions.Count != 0)
                    {
                        foreach (var jt in _receipt.journalTransactions)
                        {
                            var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                            if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                            {
                                jt.total = jt.total * -1;
                            }
                        }
                    }


                    var departments = _receipt.Departments;
                    if (departments != null)
                    {
                        _receipt.Departments = new List<Department>();
                        foreach (var _dept in departments)
                        {
                            var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                            _receipt.Departments.Add(department);
                        }
                    }


                    context.salesReceipts.Add(_receipt);
                }

            }
            context.SaveChanges();
        }

        public void AddDirectReceiptForPayment(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            foreach (var salesReceipt in saleReceiptList)
            {
                salesReceipt.transactionGroupId = intGroupId;
                
                if(salesReceipt.Departments != null && salesReceipt.Departments.Count > 0)
                {
                    var departments = salesReceipt.Departments;
                    if (departments != null)
                    {
                        salesReceipt.Departments = new List<Department>();
                        foreach (var _dept in departments)
                        {
                            var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                            salesReceipt.Departments.Add(department);
                        }
                    }
                }
                
                

                
                if (salesReceipt.journalTransactions == null)
                {

                }
                else
                   if (salesReceipt.journalTransactions.Count != 0)
                {
                    foreach (var jt in salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }

                    } 

                }
                context.salesReceipts.Add(salesReceipt);
            }
            context.SaveChanges();
        }


        public void AddDirectReceipt(List<SalesReceipt> saleReceiptList)
        {
            GroupIdCalculation();
            foreach (var salesReceipt in saleReceiptList)
            {
                salesReceipt.transactionGroupId = intGroupId;
                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                if (salesReceipt.department == null)
                    throw new NullReferenceException("Department object is null");
                if (salesReceipt.CreationDate == null)
                    salesReceipt.CreationDate = DateTime.Now;
                if (salesReceipt.BankId != 0)
                    salesReceipt.bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.BankId);
                if (salesReceipt.AccountId != 0)
                    salesReceipt.account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.AccountId);
                if (salesReceipt.company != null)
                    salesReceipt.company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                if (salesReceipt.Currency != null)
                    salesReceipt.Currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                if (salesReceipt.collectionMethod != null)
                    salesReceipt.collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                if (salesReceipt.saleReceiptStatus != null)
                    salesReceipt.saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                if (salesReceipt.journalTransactions == null)
                {

                }
                else
                   if (salesReceipt.journalTransactions.Count != 0)
                {
                    foreach (var jt in salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }

                    }

                }
                context.salesReceipts.Add(salesReceipt);
            }
            context.SaveChanges();
        }


        public List<ChartofAccount> GetChartofAccountsByCompanyDept(int uId, Company company, Department dept)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            //DepartmentRepo deptRepo = new DepartmentRepo();
            //var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();
            //if (deptMngt != null)
            //    foreach (var _dept in deptMngt)
            //    {
            //        if (deptIds.Contains(_dept.Id) == false)
            //            deptIds.Add(_dept.Id);
            //    }

            var dbAccounts = context.ChartofAccounts.Where(x => x.isApproved == true && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == true).ToList();


            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var _company in account.Companies)
                    {
                        if (companyIds.Contains(_company.Id) && _company.Id == company.Id)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
            }


            foreach (var account in chartofAccounts)
            {
                if (account.Departments.Count != 0)
                {
                    foreach (var _dept in account.Departments)
                    {
                        if (deptIds.Contains(_dept.Id) && _dept.Id == dept.Id)
                        {
                            accountsToReturn.Add(account);
                            break;
                        }
                    }
                }
                //else
                //{
                //    chartofAccounts.Add(account);
                //}
            }
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn;

        }



        public void updateSalesReceipt(List<SalesReceipt> salesReceipts)
        {
                if (salesReceipts != null && salesReceipts.Count > 0)
                {
                    foreach (SalesReceipt salesReceipt in salesReceipts)
                    {
                        if (salesReceipt.Id > 0)
                        {

                            var _salesReceipt = context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);

                            if (salesReceipt.CostSheet_Id != null)
                            {
                                _salesReceipt.CostSheet_Id = salesReceipt.CostSheet_Id;
                                //_salesReceipt.CostSheet = null;
                            }
                            if (salesReceipt.receiptDeductions.Count != 0)
                            {

                                _salesReceipt.receiptDeductions = salesReceipt.receiptDeductions;
                            }
                            if (salesReceipt.bankCharges.Count != 0)
                            {

                                _salesReceipt.bankCharges = salesReceipt.bankCharges;
                            }

                            var _salesInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);
                            _salesReceipt.saleInvoice = _salesInvoice;

                            _salesReceipt.transactionGroupId = salesReceipt.transactionGroupId;

                            var _company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);
                            _salesReceipt.company = _company;

                            //var _dept = context.Departments.FirstOrDefault(x => x.Id == salesReceipt.department.Id);
                            //_salesReceipt.department = _dept;

                            var departments = salesReceipt.Departments;
                            if (departments != null)
                            {
                                _salesReceipt.Departments = new List<Department>();
                                foreach (var _dept in departments)
                                {
                                    var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                                    _salesReceipt.Departments.Add(department);
                                }
                            }

                            //comment
                            if (salesReceipt.Customer != null)
                            {
                                var _customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                                _salesReceipt.Customer = _customer;
                            }

                            var _currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                            _salesReceipt.Currency = _currency;


                            var _collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                            _salesReceipt.collectionMethod = _collectionMethod;

                            _salesReceipt.AppliesToSales = false;

                            if (salesReceipt.bank != null)
                            {
                                var _bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.bank.Id);
                                _salesReceipt.bank = _bank;
                            }

                            if (salesReceipt.account != null)
                            {
                                var _account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.account.Id);
                                _salesReceipt.account = _account;
                            }



                            if (salesReceipt.saleReceiptStatus != null)
                            {
                                var _saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                                _salesReceipt.saleReceiptStatus = _saleReceiptStatus;
                            }

                            if (salesReceipt.principal != null)
                                _salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);

                            if (salesReceipt.transactionHolderId != null)
                                _salesReceipt.TransactionHolder = context.Employees.FirstOrDefault(x => x.EmpId == salesReceipt.transactionHolderId);
                            _salesReceipt.holderChangeDate = salesReceipt.holderChangeDate;
                            _salesReceipt.CollectionAmount = salesReceipt.CollectionAmount;
                            _salesReceipt.TotalCollectionAmount = salesReceipt.TotalCollectionAmount;
                            _salesReceipt.ReceiptRefNo = salesReceipt.ReceiptRefNo;
                            _salesReceipt.SystemRefNo = salesReceipt.SystemRefNo;
                            _salesReceipt.CreationDate = salesReceipt.CreationDate;
                            _salesReceipt.receiptType = salesReceipt.receiptType;
                            _salesReceipt.isBypassBank = salesReceipt.isBypassBank;
                            _salesReceipt.coaAccountId = salesReceipt.coaAccountId;

                            if (salesReceipt.PendingForClosing != null)
                            {
                                _salesReceipt.PendingForClosing = salesReceipt.PendingForClosing;
                            }
                            _salesReceipt.isApproved = salesReceipt.isApproved;

                        
                        //if (_salesReceipt.pettyCashes != null)
                        //{
                        //    var alltransactions = context.pettyCashes.Where(x => x.SaleReceiptId == _salesReceipt.Id).ToList();
                        //    if (alltransactions != null && alltransactions.Count() > 0)
                        //        context.pettyCashes.RemoveRange(alltransactions);
                        //}
                            _salesReceipt.pettyCashes = salesReceipt.pettyCashes;
                            _salesReceipt.PettyCashRefId = salesReceipt.PettyCashRefId;
                            _salesReceipt.VATBooks = salesReceipt.VATBooks;
                            _salesReceipt.VATBookRefId = salesReceipt.VATBookRefId;
                            if (_salesReceipt.journalTransactions == null)
                            {

                            }
                            else
                                   if (_salesReceipt.journalTransactions.Count != 0)
                            {
                                foreach (var jt in _salesReceipt.journalTransactions)
                                {
                                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                                    if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                                    {
                                        jt.total = jt.total * -1;
                                    }

                                }

                            }
                            
                        }
                    }
                context.SaveChanges();
            }
            
        }
        public void updateSalesRecpt(SalesReceipt salesReceipt)
        {
            var dbReceipt=context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);
            dbReceipt.transactionHolderId = salesReceipt.transactionHolderId;
            dbReceipt.holderChangeDate = salesReceipt.holderChangeDate;
            context.SaveChanges();

        }
            public void UpdateDirectReceipt(List<SalesReceipt> salesReceipts)
        {
            //var saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);

            //saleInvoice.RemainingBaseAmount = saleInvoice.totalBaseAmount - salesReceipt.CollectionAmount;
            
            foreach (var salesReceipt in salesReceipts)
            {


                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");

                var _salesReceipt = context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);

                //_salesReceipt = salesReceipt;

               // _salesReceipt.COAdebit_Id = salesReceipt.COAdebit_Id;
                _salesReceipt.CollectionAmount = salesReceipt.CollectionAmount;
                _salesReceipt.TotalCollectionAmount = salesReceipt.TotalCollectionAmount;
                //_salesReceipt.COAdebit_Id = salesReceipt.COAdebit.Id;
                _salesReceipt.COAcredit_Id = salesReceipt.COAcredit_Id;
                _salesReceipt.Description = salesReceipt.Description;

                if (salesReceipt.CostSheet_Id != null)
                {
                    _salesReceipt.CostSheet_Id = salesReceipt.CostSheet_Id;
                    _salesReceipt.CostSheet = null;
                }
                if (salesReceipt.receiptDeductions.Count != 0)
                {

                    _salesReceipt.receiptDeductions = salesReceipt.receiptDeductions;
                }

                _salesReceipt.transactionGroupId = salesReceipt.transactionGroupId;

                var _company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);
                _salesReceipt.company = _company;

                //var _dept = context.Departments.FirstOrDefault(x => x.Id == salesReceipt.department.Id);
                //_salesReceipt.department = _dept;

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                //comment
                if (salesReceipt.Customer != null)
                {
                    var _customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                    _salesReceipt.Customer = _customer;
                }
                if (salesReceipt.transactionHolderId != null)
                {
                    var _transactionHolder = context.Employees.FirstOrDefault(x => x.EmpId == salesReceipt.transactionHolderId);
                    _salesReceipt.TransactionHolder = _transactionHolder;
                }
                var _currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                _salesReceipt.Currency = _currency;


                var _collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                _salesReceipt.collectionMethod = _collectionMethod;

                _salesReceipt.AppliesToSales = false;

                if (salesReceipt.bank != null)
                {
                    var _bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.bank.Id);
                    _salesReceipt.bank = _bank;
                }

                if (salesReceipt.account != null)
                {
                    var _account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.account.Id);
                    _salesReceipt.account = _account;
                }



                if (salesReceipt.saleReceiptStatus != null)
                {
                    var _saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                    _salesReceipt.saleReceiptStatus = _saleReceiptStatus;
                }

                if (salesReceipt.principal != null)
                    _salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);


                _salesReceipt.CollectionAmount = salesReceipt.CollectionAmount;
                _salesReceipt.holderChangeDate = salesReceipt.holderChangeDate;
                _salesReceipt.ReceiptRefNo = salesReceipt.ReceiptRefNo;
                _salesReceipt.SystemRefNo = salesReceipt.SystemRefNo;
                _salesReceipt.CreationDate = salesReceipt.CreationDate;
                _salesReceipt.receiptType = salesReceipt.receiptType;
                _salesReceipt.isBypassBank = salesReceipt.isBypassBank;
                _salesReceipt.coaAccountId = salesReceipt.coaAccountId;
                _salesReceipt.LoansAdvanceId = salesReceipt.LoansAdvanceId;

                if (salesReceipt.PendingForClosing != null)
                {
                    _salesReceipt.PendingForClosing = salesReceipt.PendingForClosing;
                }
                _salesReceipt.isApproved = salesReceipt.isApproved;
                //context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.SaleReceiptId == salesReceipt.Id));
                //salesReceipt.CostSheet = null;
                //_salesReceipt = salesReceipt; 

                if (_salesReceipt.pettyCashes != null)
                {
                    //_salesReceipt.pettyCashes.Clear();
                    //_salesReceipt.pettyCashes = new List<CashBook.PettyCash>();
                    var alltransactions = context.pettyCashes.Where(x => x.SaleReceiptId == _salesReceipt.Id).ToList();
                    if (alltransactions != null && alltransactions.Count > 0)
                    {
                        alltransactions.ToList().ForEach(c => c.SaleReceiptId = null);
                        //context.pettyCashes.RemoveRange(alltransactions);
                    }
                }
                _salesReceipt.pettyCashes = salesReceipt.pettyCashes;
                _salesReceipt.PettyCashRefId = salesReceipt.PettyCashRefId;

                _salesReceipt.DeductionExchangeRate = salesReceipt.DeductionExchangeRate;
                _salesReceipt.DeductionSOC = salesReceipt.DeductionSOC;
                _salesReceipt.receiptDeductions = salesReceipt.receiptDeductions;
                _salesReceipt.ReceiptDeductionTaxes = salesReceipt.ReceiptDeductionTaxes;

                _salesReceipt.IsAdjustedDedVAT = salesReceipt.IsAdjustedDedVAT;
                _salesReceipt.bankCharges = salesReceipt.bankCharges;
                _salesReceipt.ReceiptBankTaxes = salesReceipt.ReceiptBankTaxes;
                _salesReceipt.IsAdjustedBankVAT = salesReceipt.IsAdjustedBankVAT;
                if (_salesReceipt.journalTransactions == null)
                {

                }
                else
                   if (_salesReceipt.journalTransactions.Count != 0)
                {
                    foreach (var jt in _salesReceipt.journalTransactions)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == jt.accountId);
                        if (coa.accountType == COA_AccountType.Loan || coa.accountType == COA_AccountType.Credit_Card || coa.accountType == COA_AccountType.Equity || coa.accountType == COA_AccountType.Accounts_Payable || coa.accountType == COA_AccountType.Longterm_Liability || coa.accountType == COA_AccountType.Other_Current_Liability || coa.accountType == COA_AccountType.Income || coa.accountType == COA_AccountType.Other_Income)
                        {
                            jt.total = jt.total * -1;
                        }

                    }

                }
            }
            context.SaveChanges();
        }



        public void UpdateLAReceipt(List<SalesReceipt> salesReceipts)
        {
            //var saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);

            //saleInvoice.RemainingBaseAmount = saleInvoice.totalBaseAmount - salesReceipt.CollectionAmount;

            foreach (var salesReceipt in salesReceipts)
            {


                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");

                var _salesReceipt = context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);



                if (salesReceipt.CostSheet_Id != null)
                {
                    _salesReceipt.CostSheet_Id = salesReceipt.CostSheet_Id;
                    _salesReceipt.CostSheet = null;
                }
                if (salesReceipt.receiptDeductions.Count != 0)
                {

                    _salesReceipt.receiptDeductions = salesReceipt.receiptDeductions;
                }

                _salesReceipt.transactionGroupId = salesReceipt.transactionGroupId;

                var _company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);
                _salesReceipt.company = _company;

                //var _dept = context.Departments.FirstOrDefault(x => x.Id == salesReceipt.department.Id);
                //_salesReceipt.department = _dept;

                var departments = salesReceipt.Departments;

                if (departments != null)
                {
                    _salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        _salesReceipt.Departments.Add(department);
                    }
                }

                //comment
                if (salesReceipt.Customer != null)
                {
                    var _customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                    _salesReceipt.Customer = _customer;
                }

                var _currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
                _salesReceipt.Currency = _currency;


                var _collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
                _salesReceipt.collectionMethod = _collectionMethod;

                _salesReceipt.AppliesToSales = false;

                if (salesReceipt.bank != null)
                {
                    var _bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.bank.Id);
                    _salesReceipt.bank = _bank;
                }

                if (salesReceipt.account != null)
                {
                    var _account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.account.Id);
                    _salesReceipt.account = _account;
                }



                if (salesReceipt.saleReceiptStatus != null)
                {
                    var _saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                    _salesReceipt.saleReceiptStatus = _saleReceiptStatus;
                }

                if (salesReceipt.principal != null)
                    _salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);

                if (salesReceipt.transactionHolderId != null)
                    _salesReceipt.TransactionHolder = context.Employees.FirstOrDefault(x => x.EmpId == salesReceipt.transactionHolderId);
                _salesReceipt.holderChangeDate = salesReceipt.holderChangeDate;
                _salesReceipt.CollectionAmount = salesReceipt.CollectionAmount;
                _salesReceipt.TotalCollectionAmount = salesReceipt.TotalCollectionAmount;
                _salesReceipt.ReceiptRefNo = salesReceipt.ReceiptRefNo;
                _salesReceipt.SystemRefNo = salesReceipt.SystemRefNo;
                _salesReceipt.CreationDate = salesReceipt.CreationDate;
                _salesReceipt.receiptType = salesReceipt.receiptType;
                _salesReceipt.isBypassBank = salesReceipt.isBypassBank;
                _salesReceipt.coaAccountId = salesReceipt.coaAccountId;
                _salesReceipt.LoansAdvanceId = salesReceipt.LoansAdvanceId;

                if (salesReceipt.PendingForClosing != null)
                {
                    _salesReceipt.PendingForClosing = salesReceipt.PendingForClosing;
                }
                _salesReceipt.isApproved = salesReceipt.isApproved;

                var transactions = context.journalTransactions.Where(x => x.SaleReceiptId == salesReceipt.Id).ToList();
                if(transactions != null && transactions.Count > 0)
                    context.journalTransactions.RemoveRange(transactions);
                //salesReceipt.CostSheet = null;
                //_salesReceipt = salesReceipt; 

                if (_salesReceipt.pettyCashes != null)
                {
                    var alltransactions1 = context.pettyCashes.Where(x => x.LoansAdvanceId == salesReceipt.LoansAdvanceId).ToList();

                    if(alltransactions1 != null && alltransactions1.Count > 0)
                    {
                        var loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == salesReceipt.LoansAdvanceId);
                        context.pettyCashes.RemoveRange(alltransactions1.Where(x => x.debit != 0).ToList());
                        loansAdvance.isDeposit = null;
                        loansAdvance.PettyCashRefId = null;
                    }


                    //var alltransactions = context.pettyCashes.Where(x => x.SaleReceiptId == _salesReceipt.Id).ToList();
                    //if (alltransactions != null && alltransactions.Count > 0)
                    //{
                    //    context.pettyCashes.RemoveRange(alltransactions);
                    //}

                    FixReceiptPettyCashTransactions(_salesReceipt.Id);
                }
                _salesReceipt.pettyCashes = salesReceipt.pettyCashes;
                _salesReceipt.PettyCashRefId = salesReceipt.PettyCashRefId;
                

            }
            context.SaveChanges();
        }

        public void UpdateCustomerCreditReceipt(List<SalesReceipt> salesReceipts)
        {
            //var saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);

            //saleInvoice.RemainingBaseAmount = saleInvoice.totalBaseAmount - salesReceipt.CollectionAmount;

            foreach (var salesReceipt in salesReceipts)
            {
                if (salesReceipt == null)
                    throw new NullReferenceException("Object can not be null");
                if (salesReceipt.company == null)
                    throw new NullReferenceException("Company Object is null");
                //if (salesReceipt.department == null)
                //    throw new NullReferenceException("Department object is null");

                var _salesReceipt = context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);

                if (_salesReceipt.pettyCashes != null)
                {
                    var alltransactions = context.pettyCashes.Where(x => x.SaleReceiptId == _salesReceipt.Id).ToList();
                    context.pettyCashes.RemoveRange(alltransactions);
                }
                _salesReceipt.pettyCashes = salesReceipt.pettyCashes;
                _salesReceipt.PettyCashRefId = salesReceipt.PettyCashRefId;

                var departments = salesReceipt.Departments;
                if (departments != null)
                {
                    salesReceipt.Departments = new List<Department>();
                    foreach (var _dept in departments)
                    {
                        var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        salesReceipt.Departments.Add(department);
                    }
                }

                if (salesReceipt.journalTransactions.Count != 0)
                {
                    try
                    {
                        var allTransactions = context.journalTransactions.Where(x => x.SaleReceiptId == salesReceipt.Id).ToList();
                        context.journalTransactions.RemoveRange(allTransactions);
                    }
                    catch (Exception ex)
                    {
                        var allTransactions = context.journalTransactions.Where(x => x.SaleReceiptId == salesReceipt.Id).ToList();
                        context.journalTransactions.RemoveRange(allTransactions);

                    }
                }
                _salesReceipt.journalTransactions = salesReceipt.journalTransactions;
            }
            context.SaveChanges();
        }

        public void FixReceiptPettyCashTransactions(int receiptId)
        {
            DBContextERP dBContext = new DBContextERP();
            var transactions = dBContext.pettyCashes.Where(x=>x.SaleReceiptId == receiptId);
            if (transactions != null && transactions.Count() > 0)
                dBContext.pettyCashes.RemoveRange(transactions);
            dBContext.SaveChanges();
        }


        public void updateSalesReceiptForDirectClose(SalesReceipt salesReceipt)
        {
            //var saleInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);

            //saleInvoice.RemainingBaseAmount = saleInvoice.totalBaseAmount - salesReceipt.CollectionAmount;
            if (salesReceipt == null)
                throw new NullReferenceException("Object can not be null");
            if (salesReceipt.company == null)
                throw new NullReferenceException("Company Object is null");
            //if (salesReceipt.department == null)
            //    throw new NullReferenceException("Department object is null");

            var _salesReceipt = context.salesReceipts.FirstOrDefault(x => x.Id == salesReceipt.Id);

            if (salesReceipt.CostSheet_Id != null)
            {
                _salesReceipt.CostSheet_Id = salesReceipt.CostSheet_Id;
                _salesReceipt.CostSheet = null;
            }
            if (salesReceipt.receiptDeductions != null)
                _salesReceipt.receiptDeductions = salesReceipt.receiptDeductions;

            if(salesReceipt.saleInvoice != null)
            {
                var _salesInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == salesReceipt.saleInvoice.Id);
                _salesReceipt.saleInvoice = _salesInvoice;
            }
            

            _salesReceipt.transactionGroupId = salesReceipt.transactionGroupId;

            var _company = context.Companies.FirstOrDefault(x => x.Id == salesReceipt.company.Id);
            _salesReceipt.company = _company;

            var departments = salesReceipt.Departments;

            if (departments != null)
            {
                _salesReceipt.Departments = new List<Department>();
                foreach (var _dept in departments)
                {
                    var department = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                    _salesReceipt.Departments.Add(department);
                }
            }

            if (salesReceipt.Customer != null)
            {
                var _customer = context.customerCompanies.FirstOrDefault(x => x.Id == salesReceipt.Customer.Id);
                _salesReceipt.Customer = _customer;
            }
            

            var _currency = context.currencies.FirstOrDefault(x => x.Id == salesReceipt.Currency.Id);
            _salesReceipt.Currency = _currency;


            var _collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == salesReceipt.collectionMethod.Id);
            _salesReceipt.collectionMethod = _collectionMethod;

            _salesReceipt.AppliesToSales = false;

            if (salesReceipt.bank != null)
            {
                var _bank = context.banks.FirstOrDefault(x => x.Id == salesReceipt.bank.Id);
                _salesReceipt.bank = _bank;
            }

            if (salesReceipt.account != null)
            {
                var _account = context.accounts.FirstOrDefault(x => x.Id == salesReceipt.account.Id);
                _salesReceipt.account = _account;
            }



            if (salesReceipt.saleReceiptStatus != null)
            {
                var _saleReceiptStatus = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == salesReceipt.saleReceiptStatus.Id);
                _salesReceipt.saleReceiptStatus = _saleReceiptStatus;
            }

            if (salesReceipt.principal != null)
                _salesReceipt.principal = context.Principals.FirstOrDefault(x => x.Id == salesReceipt.principal.Id);


            _salesReceipt.CollectionAmount = salesReceipt.CollectionAmount;
            _salesReceipt.ReceiptRefNo = salesReceipt.ReceiptRefNo;
            _salesReceipt.SystemRefNo = salesReceipt.SystemRefNo;
            _salesReceipt.CreationDate = salesReceipt.CreationDate;
            _salesReceipt.receiptType = salesReceipt.receiptType;

            if (salesReceipt.PendingForClosing != null)
            {
                _salesReceipt.PendingForClosing = salesReceipt.PendingForClosing;
            }
            _salesReceipt.isApproved = salesReceipt.isApproved;
            //context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.SaleReceiptId == salesReceipt.Id));
            

            context.SaveChanges();
        }

        public PurchaseOrder GetPurchaseOrder(int purchaseOrderId)
        {
            return context.purchaseOrders
                .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public SalesReceipt GetSaleReceipt(int groupId)
        {
            return context.salesReceipts
                .FirstOrDefault(x=>x.transactionGroupId == groupId);
        }
        public SalesReceipt GetSaleReceiptById(int receiptId)
        {
            return context.salesReceipts
                .FirstOrDefault(x => x.Id == receiptId);
        }

        public ERP_BL.Databases.Employee GetEmployeeForSaleReceipt(int empID)
        {
            return context.Employees

                .FirstOrDefault(x => x.EmpId == empID);
        }

        /// <summary>
        /// Approve Existing Bills
        /// </summary>
        /// <param name="bills"></param>
        /// <param name="groupId"></param>
        public void ApproveReceipts(List<SalesReceipt> receipts)
        {
            //List<AdminBill> _bills = new List<AdminBill>();
            if (receipts != null)
            {
                foreach (SalesReceipt receipt in receipts)
                {
                    var _receipt = context.salesReceipts.FirstOrDefault(x => x.Id == receipt.Id);

                    _receipt = receipt;

                }
            }

            context.SaveChanges();
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
        /// Get Rental Invoice.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public RentalInvoice GetRentalInvoice(int invoiceId)
        {
            return context.rentalInvoices

                .FirstOrDefault(x => x.Id == invoiceId);
        }

        public SalesReceipt GetSalesReceipt(int saleReceitpId)
        {
            if (saleReceitpId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.salesReceipts

                .FirstOrDefault(x => x.Id == saleReceitpId);
        }
        public SalesReceipt GetSalesReceiptForCostSheet(int saleReceitpId)
        {
            if (saleReceitpId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.salesReceipts

                
                .FirstOrDefault(x => x.Id == saleReceitpId);
        }





        public int GetAllSalesReceiptCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts

            .Where(x =>
                ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) 
               || (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id))) 
               )
               )
               )
            && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == true && x.PendingForClosing != true)
            .Count();
        }

        public int GetAllInActiveSalesReceiptCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts

            .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
            && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == false && x.PendingForClosing != true)
            .Count();
        }

        public SalesReceipt GetSalesReceiptforLedger(int saleReceitpId)
        {
            if (saleReceitpId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.salesReceipts
                .FirstOrDefault(x => x.Id == saleReceitpId);
        }

        public List<SalesReceipt> GetAllSalesReceipt(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
                && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == true && x.PendingForClosing != true)
                .ToList();
        }
        public List<SalesReceipt> GetAllSalesReceipFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == true && x.PendingForClosing != true)
                .ToList();
        }

        public List<SalesReceipt> GetAllSalesReceiptOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts
            .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true)
            .ToList();
        }
        public List<SalesReceipt> GetAllSalesReceiptOpenAndClosedFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
            .Where(x => x.CreationDate >= previousMonthDate 
            && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true)
            .ToList();
        }
        public List<SalesReceipt> GetAllSalesReceiptOpenAndClosed(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
            .Where(x => x.CreationDate >= from && x.CreationDate <= to 
            && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true)
            .ToList();
        }

        public List<SalesReceipt> GetAllSalesReceipts()
        {
            return context.salesReceipts
                .ToList();
        }



        /// <summary>
        /// Get all sale receipts by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllSaleReceiptsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }
        public List<SalesReceipt> getAllSaleReceiptsbyStatusIdFirst(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);

            return context.salesReceipts

                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }
        public List<SalesReceipt> getAllSaleReceiptsbyStatusId(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }

        /// <summary>
        /// Get Own pending sale receipts by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApprovalOwn(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all pending sale receipts by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId ||x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||  x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApproval(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        public List<SalesReceipt> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForApprovalDepartmental(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }



        public List<SalesReceipt> getAllPendingForClosing()
        {
            //var user = context.Users.Include("Employee").Include("employee.Supervisor").Include("employee.Supervisor.Supervisor").Include("employee.Supervisor.Supervisor.Supervisor").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate && x.PendingForClosing == true)
                .ToList();
        }

        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending sale receipts by supervisor Id and <paramref name="StatusId"/>.
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

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending bills
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

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending saleReceipts by Departments
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
            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending bills by user Id and <paramref name="UserID"/>.
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
            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                //.Where(x =>  x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending sale receipts user Id and <paramref name="UserID"/>.
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
            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                //.Where(x => x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Sale Receipt for user count. 
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
            return context.salesReceipts
                //.Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.department.Id)  && x.isVoid == true)
                .Where(x => x.isVoid == true)
                .Count();
        }

        /// <summary>
        /// Get all Void bills .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.salesReceipts
    .Where(x => x.isVoid == true)
    .Count();
        }


        /// Get Count bills.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.salesReceipts

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all bills for user count. 
        /// </summary>
        /// <returns></returns>
        public int getReceiptRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all sale receipts  own count. 
        /// </summary>
        /// <returns></returns>
        public int getReceiptRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all bills .
        /// </summary>
        /// <returns></returns>
        public int getReceiptRegisterAdministratorCount()
        {
            return context.salesReceipts
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending sale receipts by Departmental Count
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

            return context.salesReceipts

                //.Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending for closing bills by supervisor Id and <paramref name="StatusId"/>.
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

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) /*&& (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                //.Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending for closing sale receipt own <paramref name="StatusId"/>.
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

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                //.Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count sale receipts.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.salesReceipts

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all active sale receipts except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllActiveandUnapprovedReceiptsFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllActiveandUnapprovedReceipts(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Inactive sale receipts except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllInActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == false)
                .ToList();
        }
        public List<SalesReceipt> getAllInActiveandUnapprovedReceiptsFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x =>x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == false)
                .ToList();
        }
        public List<SalesReceipt> getAllInActiveandUnapprovedReceipts(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.saleReceiptStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == false)
                .ToList();
        }

        public List<SalesReceipt> getReceiptsByGroupId(int groupId)
        {
            return context.salesReceipts
                .Where(x => x.transactionGroupId == groupId)
                .ToList();
        }

        public List<SalesReceipt> getReceiptsByGroupIdForLA(int groupId)
        {
            return context.salesReceipts
                .Where(x => x.transactionGroupId == groupId)
                .ToList();
        }

        public List<SalesReceipt> getAllPendingForAdministrator()
        {
            return context.salesReceipts

                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
        }


        public List<SalesReceipt> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApprovalDepartmental(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        public List<SalesReceipt> getAllPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApproval(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get Own pending Sale Receipts by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForReApprovalOwn(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        /// <summary>
        /// Get all pending sale receipts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosingDepartmental(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all pending sale receipts by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosing(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get own pending sale receipts by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.department.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);

            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.department.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getAllPendingForClosingOwn(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains(x.department.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all sale receipts for closing.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getAllPendingForClosingAdministrator()
        {
            return context.salesReceipts
                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }



        /// <summary>
        /// Get all sale receipts for user.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getSaleReceiptRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
                && companyIds.Contains(x.company.Id) && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getSaleReceiptRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts
                .Where(x => x.CreationDate >= previousMonthDate 
                &&
                ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
                && companyIds.Contains(x.company.Id) && x.isVoid != true)
                .ToList();
        }
        public List<SalesReceipt> getSaleReceiptRegister(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts
                .Where(x => x.CreationDate >= from && x.CreationDate <= to
                &&
                ( (x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || ( x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )

                //&& ((x.Departments != null && deptIds.Intersect(x.Departments.Select(y => y.Id)).Count() > 0) || (x.department != null && deptIds.Contains(x.department.Id)))
                && companyIds.Contains(x.company.Id) && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all sale receipts .
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getSaleReceiptRegisterAdministrator()
        {
            return context.salesReceipts

    .Where(x => x.isVoid != true)
    .ToList();
        }
        public List<SalesReceipt> getSaleReceiptRegisterAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts

    .Where(x => x.CreationDate >= previousMonthDate && x.isVoid != true)
    .ToList();
        }
        public List<SalesReceipt> getSaleReceiptRegisterAdministrator(DateTime from, DateTime to)
        {
            return context.salesReceipts

    .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isVoid != true)
    .ToList();
        }



        public List<SalesReceipt> getAllPendingClosingForAdministrator()
        {
            return context.salesReceipts

                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }


        /// <summary>
        /// Get all Void Sale Receipts .
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getVoidRegisterAdministrator()
        {
            return context.salesReceipts

        .Where(x => x.isVoid == true)
    .ToList();
        }
        public List<SalesReceipt> getVoidRegisterAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts

        .Where(x => x.CreationDate >= previousMonthDate && x.isVoid == true)
    .ToList();
        }
        public List<SalesReceipt> getVoidRegisterAdministrator(DateTime from, DateTime to)
        {
            return context.salesReceipts

        .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isVoid == true)
    .ToList();
        }
        //#Void
        /// <summary>
        /// Get all Void Sale Receipts for user.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
                .ToList();
        }
        public List<SalesReceipt> getVoidRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts

                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
                .ToList();
        }
        public List<SalesReceipt> getVoidRegister(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.salesReceipts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
                .ToList();
        }



        /// <summary>
        /// Get all Void sale receipts own.
        /// </summary>
        /// <returns></returns>
        public List<SalesReceipt> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts

                .Where(x => ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }
        public List<SalesReceipt> getVoidRegisterOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.salesReceipts

                .Where(x => x.CreationDate >= previousMonthDate 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }
        public List<SalesReceipt> getVoidRegisterOwn(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.Include("employee").Include("employee.departments").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.salesReceipts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && ((x.loansAdvance != null && deptIds.Contains((int)x.loansAdvance.deptId))
               || (x.saleInvoice != null && deptIds.Contains((int)x.saleInvoice.dept_Id))
               || (x.rentalInvoice != null && deptIds.Contains((int)x.rentalInvoice.deptId))
               || (x.receiptType == ReceiptType.Direct_Receipt && (deptIds.Contains((int)x.deptId) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Admin_Bills && deptIds.Contains((int)x.payment.adminBill.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Loans_Advances && deptIds.Contains((int)x.payment.loansAdvance.deptId))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Purchase_Invoice && deptIds.Contains((int)x.payment.purchaseInvoice.dept_Id))) ||
               (x.payment != null && (x.payment.transactionType == PaymentTransactionType.Vendor_Bills && deptIds.Contains((int)x.payment.Bill.dept_Id)))
               )
               )
               )
               && companyIds.Contains((int)x.company.Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }

        public List<SaleInvoice> GetAllSaleInvoicesForCustomer(int Id)
        {
            return context.saleInvoices.Where(x => x.customerCompany_Id == Id)
                .ToList();
        }

        public void setSaleReceipttoVoid(int Id, bool isVoid)
        {
            SalesReceipt item = context.salesReceipts.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }
        public void UpdateSaleReceipt(SalesReceipt receipt)
        {
            SalesReceipt item = context.salesReceipts.FirstOrDefault(x => x.Id == receipt.Id);
            item = receipt;
            context.SaveChanges();
        }
        public void RemoveReceiptSystemCost(int Id, bool isVoid)
        {

            var alltransactions = context.costSheetSaleReceiptFields.Where(x => x.Receipt_Id == Id).ToList();
            context.costSheetSaleReceiptFields.RemoveRange(alltransactions);
            context.SaveChanges();
        }
        public void addCollectionMethod(CollectionMethod collectionMethod)
        {
            if (collectionMethod == null)
                throw new NullReferenceException("Object can not be null");

            context.collectionMethods.Add(collectionMethod);
            context.SaveChanges();
        }

        public void updateCollectionMethod(CollectionMethod collectionMethod)
        {
            if (collectionMethod == null)
                throw new NullReferenceException("Object can not be null");
            var _collectionMethod = context.collectionMethods.FirstOrDefault(x => x.Id == collectionMethod.Id);

            _collectionMethod.MethodName = collectionMethod.MethodName;
            _collectionMethod.isActive = collectionMethod.isActive;

            context.SaveChanges();
        }

        public CollectionMethod GetCollectionMethod(int CollectionMethodId)
        {
            if (CollectionMethodId <= 0)
                throw new NullReferenceException("Object can not be null");
            return context.collectionMethods.FirstOrDefault(x => x.Id == CollectionMethodId);
        }

        public List<CollectionMethod> GetAllCollectionMethods()
        {
            return context.collectionMethods.ToList();
        }


        public void addAccount(Account account)
        {
            Account _account = new Account();
            if (account == null)
                throw new NullReferenceException("Object can not be null");

            if (account.mainBankId != null)
                _account.mainBankId = account.mainBankId;
            if (account.bank != null)
                _account.bank = context.banks.FirstOrDefault(x => x.Id == account.bank.Id);
            if (account.company != null)
                _account.company = context.Companies.FirstOrDefault(x => x.Id == account.company.Id);
            if (account.currency != null)
                _account.currency = context.currencies.FirstOrDefault(x => x.Id == account.currency.Id);
            if (account.COA_accountId != 0)
                _account.COA_accountId = account.COA_accountId;
            if (account.departments != null)
            {
                foreach (var _dept in account.departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                    _account.departments.Add(dept);
                }
            }

            if (account.AccountNick != null)
                _account.AccountNick = account.AccountNick;

            if (account.AccountNo != null)
                _account.AccountNo = account.AccountNo;

            if (account.IBAN != null)
                _account.IBAN = account.IBAN;

            _account.accountType = account.accountType;

            if (account.COANo != null)
                _account.COANo = account.COANo;

            _account.COA_Type = account.COA_Type;
            _account.accountsCategory = account.accountsCategory;

            if (account.industryTypeId != null)
                _account.industryTypeId = account.industryTypeId; /*context.IndustryTypes.FirstOrDefault(x => x.Id == account.industryType.Id); ;*/
            if (account.vendor_Id != null)
                _account.vendor_Id = account.vendor_Id;

            _account.isActive = account.isActive;
            _account.isAdjustmentAccount = account.isAdjustmentAccount;

            //_account.isPersonal = account.isPersonal;
            context.accounts.Add(_account);
            context.SaveChanges();
        }

        public void updateAccount(Account account)
        {

            if (account == null)
                throw new NullReferenceException("Object can not be null");
            Account _account = context.accounts.FirstOrDefault(x => x.Id == account.Id);

            //_account = account;

            if (account.mainBankId != null)
                _account.mainBankId = account.mainBankId;

            if (account.bank != null)
                _account.bank = context.banks.FirstOrDefault(x => x.Id == account.bank.Id);

            if (account.company != null)
                _account.company = context.Companies.FirstOrDefault(x => x.Id == account.company.Id);

            _account.departments.Clear();
            if (account.departments != null)
            {
                //_account.departments = null;
                foreach (var _dept in account.departments)
                {
                    if (_account.departments.Contains(_dept) == false)
                    {
                        var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        _account.departments.Add(dept);
                    }
                }
            }

            if (account.currency != null)
                _account.currency = context.currencies.FirstOrDefault(x => x.Id == account.currency.Id);
            if (account.COA_accountId != 0)
                _account.COA_accountId = account.COA_accountId;

            if (account.AccountNick != null)
                _account.AccountNick = account.AccountNick;

            if (account.AccountNo != null)
                _account.AccountNo = account.AccountNo;

            if (account.IBAN != null)
                _account.IBAN = account.IBAN;

            _account.accountType = account.accountType;

            if (account.COANo != null)
                _account.COANo = account.COANo;


            _account.COA_Type = account.COA_Type;
            _account.accountsCategory = account.accountsCategory;

            if (account.industryTypeId != null)
                _account.industryTypeId = account.industryTypeId;
            else
                _account.industryTypeId = null;


            if (account.vendor_Id != null)
                _account.vendor_Id = account.vendor_Id;
            else
                _account.vendor = null;
            _account.isActive = account.isActive;
            _account.isAdjustmentAccount = account.isAdjustmentAccount;
            //_account.isPersonal = account.isPersonal;

            context.SaveChanges();
        }



        public Account GetAccount(int accountId)
        {
            if (accountId <= 0)
                throw new NullReferenceException("Object can not be null");
            return context.accounts
                .FirstOrDefault(x => x.Id == accountId);
        }

        public List<Account> GetAllAccounts()
        {
            return context.accounts
                .ToList();
        }

        public List<Account> GetAllAccountsByUserId(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<Account> accountsToReturn = new List<Account>();

            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var dbAccounts = context.accounts
                .Where(x => companyIds.Contains(x.company.Id))
                .ToList();

            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            foreach (var account in dbAccounts)
            {
                if (account.departments != null)
                {
                    foreach (var dept in account.departments)
                    {
                        if (deptIds.Contains(dept.Id))
                        {
                            accountsToReturn.Add(account);
                            break;
                        }
                    }
                }
                else
                {
                    accountsToReturn.Add(account);
                }

            }

            return accountsToReturn;
        }

        public List<Account> GetAllAccountsByBankId(int bank_id)
        {
            return context.accounts
                .Where(x => x.bank.Id == bank_id)
                .ToList();
        }

        public List<Account> GetAccountsForLaonsByBankId(int bank_id)
        {
            return context.accounts

                .Where(x => x.bank.Id == bank_id)
                .ToList();
        }


        public List<Account> GetAllAccountsForPayments(int bank_id)
        {
            return context.accounts

                .Where(x => x.bank.Id == bank_id)
                .ToList();
        }



        public void addBank(Bank bank)
        {
            if (bank == null)
                throw new NullReferenceException("Object can not be null");

            Bank _bank = new Bank();
            _bank.bankId = bank.bankId;
            _bank.BankName = bank.BankName;
            _bank.BranchCode = bank.BranchCode;
            _bank.SwiftCode = bank.SwiftCode;

            if (bank.address != null)
                _bank.address = bank.address;

            if (bank.contact != null)
                _bank.contact = bank.contact;

            if (bank.companies != null)
            {
                foreach (var _comp in bank.companies)
                {
                    var comp = context.Companies.FirstOrDefault(x => x.Id == _comp.Id);
                    _bank.companies.Add(comp);
                }
            }

            context.banks.Add(_bank);
            context.SaveChanges();
        }

        public void addBankContactPerson(Bank bank, ContactPerson contactPerson)
        {
            if (bank == null)
                throw new NullReferenceException("Object can not be null");


            if (contactPerson == null)
                throw new NullReferenceException("Object can not be null");

            var _bank = context.banks

                .FirstOrDefault(x => x.Id == bank.Id);

            _bank.contactPersons.Add(contactPerson);

            context.SaveChanges();
        }

        public void addBankContactPerson(int bankId, ContactPerson contactPerson)
        {
            if (contactPerson == null)
                throw new NullReferenceException("Object can not be null");

            //context.contactPersons.Add(contactPerson);

            var bank = context.banks
                .FirstOrDefault(x => x.Id == bankId);

            bank.contactPersons.Add(contactPerson);
            context.SaveChanges();
        }




        public void updateBank(Bank bank)
        {
            if (bank == null)
                throw new NullReferenceException("Object can not be null");
            var _bank = context.banks.FirstOrDefault(x => x.Id == bank.Id);

            if (bank.address != null)
            {
                _bank.address = bank.address;
            }

            _bank.mainBank = context.mainBanks.FirstOrDefault(x=>x.Id == bank.bankId);
            _bank.BankName = bank.BankName;
            _bank.BranchCode = bank.BranchCode;
            _bank.contact = bank.contact;
            _bank.Location = bank.Location;
            _bank.SwiftCode = bank.SwiftCode;


            _bank.companies.Clear();
            if (bank.companies != null)
            {
                //_account.departments = null;
                foreach (var _comp in bank.companies)
                {
                    if (_bank.companies.Contains(_comp) == false)
                    {
                        var comp = context.Companies.FirstOrDefault(x => x.Id == _comp.Id);
                        _bank.companies.Add(comp);
                    }
                }
            }

            context.SaveChanges();

        }

        public void updateContactPerson(ContactPerson contactPerson)
        {
            if (contactPerson == null)
                throw new NullReferenceException("Object can not be null");
            var _person = context.contactPersons.FirstOrDefault(x => x.Id == contactPerson.Id);
            var _bank = context.banks.FirstOrDefault(x => x.Id == contactPerson.bank.Id);

            if (contactPerson.bank != null)
                _person.bank = contactPerson.bank;

            if (contactPerson.designation != null)
                _person.designation = contactPerson.designation;

            if (contactPerson.contact != null)
                _person.contact = contactPerson.contact;

            if (contactPerson.person != null)
                _person.person = contactPerson.person;

            _person.isActive = contactPerson.isActive;

            context.SaveChanges();
        }

        public Bank GetBank(int bankId)
        {
            if (bankId <= 0)
                throw new NullReferenceException("Object can not be null");
            return context.banks

                .FirstOrDefault(x => x.Id == bankId);
        }

        public List<Bank> GetAllBranches()
        {
            return context.banks

                    .ToList();
        }

        public List<Bank> GetAllBanksByCompany(int companyId)
        {
            return context.banks

                    .ToList().Where(x=>x.companies.FirstOrDefault(y=>y.Id == companyId) != null).ToList();
        }

        public List<Bank> GetAllBankbyUserId(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<Company> companies = new List<Company>();
            foreach (var comp in user.employee.Companies)
                companies.Add(comp);

            List<Bank> banks = new List<Bank>();
            List<Bank> banksToReturn = new List<Bank>();

            banks = context.banks

                    .ToList();

            foreach (var _bank in banks)
            {
                if (_bank.companies.Count == 0)
                {
                    banksToReturn.Add(_bank);
                }
                else
                {
                    foreach (var _company in companies)
                    {
                        if (_bank.companies.Contains(_company))
                        {
                            banksToReturn.Add(_bank);
                            break;
                        }
                    }
                }
            }

            return banksToReturn;
        }

        public List<Bank> GetAllBranchesbyUserAndBank(int uid, int bankId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<Company> companies = new List<Company>();
            foreach (var comp in user.employee.Companies)
                companies.Add(comp);

            List<Bank> banks = new List<Bank>();
            List<Bank> banksToReturn = new List<Bank>();

            banks = context.banks

                    .Where(x => x.bankId == bankId)
                    .ToList();

            foreach (var _bank in banks)
            {
                if (_bank.companies.Count == 0)
                {
                    banksToReturn.Add(_bank);
                }
                else
                {
                    foreach (var _company in companies)
                    {
                        if (_bank.companies.Contains(_company))
                        {
                            banksToReturn.Add(_bank);
                            break;
                        }
                    }
                }
            }

            return banksToReturn;
        }


        public List<Bank> GetBanksbyCompany(Company company)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == m);

            //List<Company> companies = new List<Company>();
            //foreach (var comp in user.employee.Companies)
            //    companies.Add(comp);

            List<Bank> banks = new List<Bank>();
            List<Bank> banksToReturn = new List<Bank>();

            banks = context.banks

                    .ToList();

            foreach (var _bank in banks)
            {
                foreach (var _company in _bank.companies)
                {
                    if (_company.Id == company.Id)
                    {
                        banksToReturn.Add(_bank);
                        break;
                    }
                }
            }

            return banksToReturn;
        }


        public List<Bank> GetBanksbyCompanyForPayments(Company company)
        {
            List<Bank> banks = new List<Bank>();
            banks = context.banks

                    .Where(x => x.companies.FirstOrDefault(y => y.Id == company.Id) != null)
                    .ToList();

            return banks;
        }



        public List<Bank> GetBanksbyCompInterCompForPayments(Company company, Company InterComp)
        {


            List<Bank> banks = new List<Bank>();
            List<Bank> banksToReturn = new List<Bank>();

            banks = context.banks
     
                   
                    .Where(x => x.companies.FirstOrDefault(y => y.Id == company.Id) != null || x.companies.FirstOrDefault(y => y.Id == InterComp.Id) != null)
                    .ToList();


            return banks;
        }


        public List<ContactPerson> GetAllContactPerson(Bank bank)
        {
            return context.banks
          
                .FirstOrDefault(x => x.Id == bank.Id).contactPersons.ToList();
        }

        public List<ContactPerson> GetAllContactPerson(int bankId)
        {
            return context.banks
        
            .FirstOrDefault(x => x.Id == bankId).contactPersons.ToList();
        }

        public ContactPerson GetContactPerson(int contactPersonId)
        {
            if (contactPersonId <= 0)
                throw new NullReferenceException("Object can not be null");

            return context.contactPersons
     
            .FirstOrDefault(x => x.Id == contactPersonId);
        }

        /// <summary>
        /// Get last Sale Receipt
        /// </summary>
        /// <returns></returns>
        public int GetLastSaleReceiptId()
        {

            var receipt = context.salesReceipts.OrderByDescending(q => q.Id).FirstOrDefault();
            return receipt.transactionGroupId;
        }

        public List<Deduction> GetAllDeductions()
        {
            return context.deductions.ToList();
        }

        public Deduction GetDeductionById(int ded_id)
        {
            return context.deductions.FirstOrDefault(x=>x.Id == ded_id);
        }


        public void addDeduction(Deduction deduction)
        {
            if (deduction == null)
                throw new NullReferenceException("Object can not be null");

            context.deductions.Add(deduction);
            context.SaveChanges();
        }

        public void updateDeduction(Deduction deduction)
        {
            if (deduction == null)
                throw new NullReferenceException("Object can not be null");
            var _deduction = context.deductions.FirstOrDefault(x => x.Id == deduction.Id);

            _deduction.title = deduction.title;
            _deduction.isActive = deduction.isActive;
            if (deduction.chartofAccountId != 0)
                _deduction.chartofAccountId = deduction.chartofAccountId;

            context.SaveChanges();
        }


        //Functions for SaleRecepits Stausses

        public void AddSaleReceiptStatus(SalesReceiptStatus status)
        {
            context.salesReceiptStatuses.Add(status);
            context.SaveChanges();

        }

        public void UpdateSaleReceiptStatus(SalesReceiptStatus status)
        {
            if (status == null)
                throw new NullReferenceException("Object can not be null");
            var _status = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == status.Id);

            _status.Status = status.Status;
            _status.isActive = status.isActive;
            _status.backcolor = status.backcolor;

            context.SaveChanges();

        }

        public SalesReceiptStatus GetSaleReceiptStatus(int statusId)
        {
            var status = context.salesReceiptStatuses.FirstOrDefault(x => x.Id == statusId);
            return status;

        }

        public List<SalesReceiptStatus> GetAllSaleReceiptStatus()
        {
            var statusList = context.salesReceiptStatuses.ToList();
            return statusList;

        }

        public List<SalesReceiptStatus> GetAllOpenSaleReceiptStatus()
        {
            var statusList = context.salesReceiptStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        public List<SalesReceiptStatus> GetAllCloseSaleReceiptStatus()
        {
            var statusList = context.salesReceiptStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }
        public void updateForCostSheet(int costSheetId, int costSheetFieldId, Vendor vendor, Currency oc, string maker, string origin, DateTime? deliveryDate, PaymentTerm paymentTerm, Warranty warranty, Incoterm incoTerm)
        {
            SalesReceipt saleReceipttoUpdate = context.salesReceipts.FirstOrDefault(x => x.costSheetFieldId == costSheetFieldId && x.CostSheet_Id == costSheetId);
            if (saleReceipttoUpdate != null)
            {
                if (costSheetId != null)
                {
                    saleReceipttoUpdate.CostSheet_Id = costSheetId;

                }
                if (costSheetFieldId != null)
                {
                    saleReceipttoUpdate.costSheetFieldId = costSheetFieldId;
                }
                if (oc != null)
                {
                    saleReceipttoUpdate.Currency = oc;
                }
                context.SaveChanges();
            }
        }

        public void updateForCostSheet(int receiptId, int costSheetId)
        {
            SalesReceipt saleReceipttoUpdate = context.salesReceipts.FirstOrDefault(x => x.Id == receiptId);
            if (saleReceipttoUpdate != null)
            {
                if (costSheetId != 0)
                {
                    saleReceipttoUpdate.CostSheet_Id = costSheetId;

                }
                context.SaveChanges();
            }
        }
        public ReceiptDeduction GetReceiptDeduction(int deductionId)
        {
            return context.receiptDeductions.FirstOrDefault(x=>x.Id==deductionId);
        }


        public void AddMainBank(MainBank bank)
        {
            if (bank == null)
                throw new NullReferenceException("Object can not be null");

            
            context.mainBanks.Add(bank);
            context.SaveChanges();
        }

        public void UpdateMainBank(MainBank bank)
        {
            if (bank == null)
                throw new NullReferenceException("Object can not be null");

            MainBank _bank = context.mainBanks.FirstOrDefault(x=>x.Id == bank.Id);
            _bank = bank;
            
            context.SaveChanges();
        }

        public MainBank GetMainBank(int bankId)
        {
            return context.mainBanks.FirstOrDefault(x=>x.Id == bankId);
        }

        public List<MainBank> GetAllBanks()
        {
            return context.mainBanks.ToList();
        }
        public SalesReceipt GetForCostSheet(int saleReceitpId)
        {
            return context.salesReceipts
                .FirstOrDefault(x => x.Id == saleReceitpId);
        }
        public LoansAdvance getLAForJournalTransactions(int loansAdvanceId)
        {
            return context.loansAdvances
                .FirstOrDefault(x => x.Id == loansAdvanceId);
        }
        public RentalInvoice getRentalInvoiceForJournalTransactions(int rentalInvoiceId)
        {
            return context.rentalInvoices
                .FirstOrDefault(x => x.Id == rentalInvoiceId);
        }

        public SaleInvoice getSIForJournalTransactions(int saleInvoiceId)
        {
            return context.saleInvoices
                .FirstOrDefault(x => x.Id == saleInvoiceId);
        }
    }
}
