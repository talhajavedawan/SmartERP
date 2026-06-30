using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{

    public class ChartofAccountsRepo
    {
        DBContextERP context = new DBContextERP();
        public void AddDirectApprovedAccounts(ChartofAccount Account)
        {
            if (Account != null)
            {
                context.ChartofAccounts.Add(Account);
            }
            context.SaveChanges();
        }
        public void AddAccount(ChartofAccount Account)
        {
            ChartofAccount chartofAccount = new ChartofAccount();
            if (Account != null)
            {
                chartofAccount.isActive = Account.isActive;
                chartofAccount.accountName = Account.accountName;
                chartofAccount.accountNo = Account.accountNo;
                chartofAccount.accountType = Account.accountType;
                chartofAccount.ApprovedDate = Account.ApprovedDate;
                chartofAccount.bankAccountNo = Account.bankAccountNo;
                chartofAccount.creationDate = Account.creationDate;
                chartofAccount.creditCardNo = Account.creditCardNo;
                chartofAccount.currencyId = Account.currencyId;
                chartofAccount.description = Account.description;
                chartofAccount.isApproved = Account.isApproved;
                chartofAccount.isReApproved = Account.isReApproved;
                chartofAccount.parentId = Account.parentId;
                chartofAccount.ReApprovalDate = Account.ReApprovalDate;
                chartofAccount.routingNo = Account.routingNo;
                chartofAccount.stage = Account.stage;
                chartofAccount.userId = Account.userId;
                if (Account.Companies != null)
                {
                    foreach (var company in Account.Companies)
                    {
                        var dbCompany = context.Companies.FirstOrDefault(x => x.Id == company.Id);
                        chartofAccount.Companies.Add(dbCompany);
                    }
                }
                if (Account.Departments != null)
                {
                    foreach (var department in Account.Departments)
                    {
                        var dbDepartment = context.Departments.FirstOrDefault(x => x.Id == department.Id);
                        chartofAccount.Departments.Add(dbDepartment);
                    }
                }
                if (Account.Employees != null)
                {
                    foreach (var employee in Account.Employees)
                    {
                        var dbEmployee = context.Employees.FirstOrDefault(x => x.EmpId == employee.EmpId);
                        chartofAccount.Employees.Add(dbEmployee);
                    }
                }
                context.ChartofAccounts.Add(chartofAccount);
            }
            context.SaveChanges();
        }
        public int getPendingForApprovalCount()
        {
            return context.ChartofAccounts
                .Where(x => x.isApproved == false)
                .Count();
        }

        public int getApprovedCount()
        {
            return context.ChartofAccounts
                .Where(x => x.isApproved == true)
                .Count();
        }
        public void UpdateAccount(ChartofAccount Account)
        {
            ChartofAccount dbAccount = new ChartofAccount();
            if (Account != null)
            {
                dbAccount = context.ChartofAccounts.FirstOrDefault(x => x.Id == Account.Id);
                dbAccount.accountName = Account.accountName;
                dbAccount.accountNo = Account.accountNo;
                dbAccount.accountType = Account.accountType;
                dbAccount.ApprovedDate = Account.ApprovedDate;
                dbAccount.bankAccountNo = Account.bankAccountNo;
                dbAccount.creationDate = Account.creationDate;
                dbAccount.creditCardNo = Account.creditCardNo;
                dbAccount.currencyId = Account.currencyId;
                dbAccount.description = Account.description;
                dbAccount.Id = Account.Id;
                dbAccount.isActive = Account.isActive;
                dbAccount.isApproved = Account.isApproved;
                dbAccount.isDebitIncrease = Account.isDebitIncrease;
                dbAccount.isOpeningBalance = Account.isOpeningBalance;
                dbAccount.isReApproved = Account.isReApproved;
                dbAccount.accociatedCompany = Account.accociatedCompany;
                dbAccount.manualBalanceOC = Account.manualBalanceOC;
                dbAccount.manualBalancePKR = Account.manualBalancePKR;
                if (Account.JournalTransactions.Count != 0)
                {
                    dbAccount.JournalTransactions.Clear();
                    var transactions = context.journalTransactions.Where(x => x.accountId == Account.Id).ToList();
                    dbAccount.JournalTransactions.AddRange(transactions);
                }
                if (Account.parentId != null || Account.parentId != 0)
                    dbAccount.parentId = Account.parentId;
                dbAccount.ReApprovalDate = Account.ReApprovalDate;
                dbAccount.routingNo = Account.routingNo;
                dbAccount.stage = Account.stage;

                dbAccount.Companies.Clear();
                if (Account.Companies != null)
                {
                    foreach (var accountCompany in Account.Companies)
                    {
                        var dbCompany = context.Companies.FirstOrDefault(x => x.Id == accountCompany.Id);
                        dbAccount.Companies.Add(dbCompany);
                    }
                }
                dbAccount.Departments.Clear();
                if (Account.Departments != null)
                {
                    foreach (var accountDepartment in Account.Departments)
                    {
                        var dbDepartment = context.Departments.FirstOrDefault(x => x.Id == accountDepartment.Id);
                        dbAccount.Departments.Add(dbDepartment);
                    }
                }
                dbAccount.Employees.Clear();
                if (Account.Employees != null)
                {
                    foreach (var accountEmployee in Account.Employees)
                    {
                        var dbEmployee = context.Employees.FirstOrDefault(x => x.EmpId == accountEmployee.EmpId);
                        dbAccount.Employees.Add(dbEmployee);
                    }
                }

                dbAccount.userId = Account.userId;

                //dbAccount.Accounts = Account.Accounts;
                context.SaveChanges();
            }

        }


        public void Update(List<ChartofAccount> chartofAccounts)
        {
            foreach (var account in chartofAccounts)
            {
                var companies = new List<Company>();
                var departments = new List<Department>();
                var employees = new List<ERP_BL.Databases.Employee>();
                var dbAccount = context.ChartofAccounts.FirstOrDefault(x => x.Id == account.Id);
                if (account.Companies != null)
                {
                    foreach (var accountCompany in account.Companies)
                    {
                        var dbCompany = context.Companies.FirstOrDefault(x => x.Id == accountCompany.Id);
                        companies.Add(dbCompany);
                    }
                }
                if (account.Departments != null)
                {
                    foreach (var accountDepartment in account.Departments)
                    {
                        var dbDepartment = context.Departments.FirstOrDefault(x => x.Id == accountDepartment.Id);
                        departments.Add(dbDepartment);
                    }
                }
                if (account.Employees != null)
                {
                    foreach (var accountEmployee in account.Employees)
                    {
                        var dbEmployee = context.Employees.FirstOrDefault(x => x.EmpId == accountEmployee.EmpId);
                        employees.Add(dbEmployee);
                    }
                }
                dbAccount.Companies = companies;
                dbAccount.Departments = departments;
                dbAccount.Employees = employees;
                //dbAccount = account;

            }
            context.SaveChanges();
        }
        public void UpdateWithGroup(ChartofAccountGroup group)
        {
            var dbGroup = context.chartofAccountGroups
                       .Include(g => g.ChartofAccounts) // Include related entities
                       .FirstOrDefault(x => x.Id == group.Id);

            if (dbGroup != null)
            {
                // Update properties as necessary
                dbGroup.Title = group.Title; // Update other properties as needed
                dbGroup.referenceNo = group.referenceNo; // Assuming this property exists

                // Clear the existing ChartofAccounts
                dbGroup.ChartofAccounts.Clear(); // Clear existing accounts

                foreach (var account in group.ChartofAccounts)
                {
                    // Attach existing accounts if they already exist in the database
                    var existingAccount = context.ChartofAccounts.Find(account.Id);
                    if (existingAccount != null)
                    {
                        // If the account exists, attach it to the current context
                        context.Entry(existingAccount).State = EntityState.Unchanged;
                        dbGroup.ChartofAccounts.Add(existingAccount); // Add existing account to the group
                    }
                    else
                    {
                        // If the account does not exist, add it as a new entity
                        dbGroup.ChartofAccounts.Add(account);
                    }
                }

                // Save changes to the context
                context.SaveChanges();
            }
        }

        public ChartofAccount GetAccountById(int accountId)
        {
            return context.ChartofAccounts.FirstOrDefault(x => x.Id == accountId);
        }


        public List<ChartofAccount> GetAllApprovedAcccounts(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts

            .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies != null)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && account.isApproved == true || account.Companies.Count == 0 && account.isApproved == true)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                {
                    chartofAccounts.Add(account);
                }

            }
            return chartofAccounts;
            //return context.ChartofAccounts.Include("company").Where(x=>x.isApproved==true).ToList() ;
        }
        public int getAllPendingForApprovalCompanyCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts.ToList();
            int count = 0;
            foreach (var account in dbAccounts)
            {
                foreach (var company in account.Companies)
                {
                    if (companyIds.Contains(company.Id) && account.isApproved == false && account.isVoid != true || account.Companies.Count == 0 && account.isApproved == false && account.isVoid != true)
                    {
                        count++;
                        break;
                    }
                }

            }
            return count;

        }
        public List<ChartofAccount> GetAllApprovedAcccountsbyType(COA_AccountType type, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> employeeIds = new List<int>();
            var dbAccounts = context.ChartofAccounts
                   .Where(x => x.isActive == true && x.isApproved == true && x.accountType == type).ToList();

            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            return chartofAccounts;
        }

        public void ApproveAccount(ChartofAccount Account)
        {


        }
        public double getSumByaccountId(int Id)
        {
            double value = 0;
            InterBankTransRepo interBankTransferRepo = new InterBankTransRepo();
            JournalVoucherRepo jvRepo = new JournalVoucherRepo();
            SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
            var list = context.journalTransactions.Where(x => x.accountId == Id).ToList();
            foreach (var item in list)
            {
                if (item.debit != 0)
                {
                    if (item.InterBankId != 0 && item.InterBankId != null)
                    {
                        var interBank = interBankTransferRepo.GetInterBankTransfer((int)item.InterBankId);

                        if (interBank.isVoid != true)
                            value = value + item.debit;
                    }
                    else
                        if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                    {
                        var jv = jvRepo.get((int)item.journalVoucher_id);

                        if (jv.isVoid != true)
                            value = value + item.debit;
                    }
                    else
                    if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                    {
                        var saleInvoice = saleInvoiceRepo.get((int)item.SaleInvoiceId);

                        if (saleInvoice.isVoid != true)
                            value = value + item.debit;
                    }
                }
                else
                    if (item.credit != 0)
                {
                    if (item.InterBankId != 0 && item.InterBankId != null)
                    {
                        var interBank = interBankTransferRepo.GetInterBankTransfer((int)item.InterBankId);

                        if (interBank.isVoid != true)
                            value = value - item.credit;
                    }
                    else
                        if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                    {
                        var jv = jvRepo.get((int)item.journalVoucher_id);

                        if (jv.isVoid != true)
                            value = value - item.credit;
                    }
                    else
                        if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                    {
                        var saleInvoice = saleInvoiceRepo.get((int)item.SaleInvoiceId);

                        if (saleInvoice.isVoid != true)
                            value = value - item.credit;
                    }
                }
            }
            return value;
        }
        public ChartofAccount get(int chartofAccountId)
        {
            SystemLog.LogInfo(this.GetType(), "retrived ChartofAccount with Id= " + chartofAccountId);

            return context.ChartofAccounts.FirstOrDefault(x => x.Id == chartofAccountId);

        }
        public List<ChartofAccount> GetAllApprovedAcccountforInterBank(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts.ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies != null)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Bank || account.Companies.Count == 0 && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Bank)
                        {
                            chartofAccounts.Add(account);
                            break;

                        }
                    }
                }
                else
                {
                    chartofAccounts.Add(account);
                }

            }
            return chartofAccounts;
        }
        public List<ChartofAccount> GetAllApprovedAcccountforDepartment(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts.ToList();


            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Account_Receivable || account.Companies.Count == 0 && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Account_Receivable
                            || companyIds.Contains(company.Id) && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Accounts_Payable || account.Companies.Count == 0 && account.isApproved == true && account.isVoid != true && account.accountType == COA_AccountType.Accounts_Payable)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                {
                    chartofAccounts.Add(account);
                }
            }
            return chartofAccounts;
        }
        public List<ChartofAccount> GetAllforBills(Company billCompany, Department billDept, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DepartmentRepo deptRepo = new DepartmentRepo();
            var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();
            if (deptMngt != null)
                foreach (var _dept in deptMngt)
                {
                    if (deptIds.Contains(_dept.Id) == false)
                        deptIds.Add(_dept.Id);
                }

            var dbAccounts = context.ChartofAccounts
              .Where(x => x.isApproved == true && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == true).ToList();


            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && company.Id == billCompany.Id)
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
                    foreach (var dept in account.Departments)
                    {
                        if (deptIds.Contains(dept.Id) && dept.Id == billDept.Id)
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

        public List<ChartofAccount> GetAllForInterCompany(Company billCompany, Department billDept, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            return context.ChartofAccounts
                .Where(x => x.isApproved == true && x.isVoid != true && (x.Companies.Count == 0 || x.Companies.FirstOrDefault(y => y.Id == billCompany.Id) != null) && x.Departments.FirstOrDefault(z => z.Id == billDept.Id) != null).ToList();

        }

        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var dbAccounts = context.ChartofAccounts.Where(x => x.isApproved == false && x.isVoid != true).ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn.Count;
        }
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            return accountsToReturn.Count;
        }
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var dbAccounts = context.ChartofAccounts

                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn.Count;
        }
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts

                .Where(x => (x.isReApproved == false && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn.Count;
        }
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var dbAccounts = context.ChartofAccounts

                .Where(x => (x.isReApproved == false && x.isApproved == true && x.isVoid != true))
                .ToList();



            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn.Count;
        }
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts

                .Where(x => (x.isReApproved == false && x.isVoid != true && x.isApproved == true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            accountsToReturn = chartofAccountsWithEmployees;
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn.Count;
        }
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => x.isVoid == true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            return chartofAccounts.Count;
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.ChartofAccounts

    .Where(x => x.isVoid == true)
    .Count();
        }
        public int getChartofAccountRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            return chartofAccounts.Count;
        }
        public int getChartofAccountRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x =>  /*(x.user.employee.EmpId == user.employee.EmpId) &&*/ x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts.Count;

        }
        public int getAllPendingForAdministratorCount()
        {
            return context.ChartofAccounts

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();

        }
        public int getChartofAccountRegisterAdministratorCount()
        {
            return context.ChartofAccounts
            .Where(x => x.isVoid != true)
            .Count();
        }


        public List<ChartofAccount> getAll()
        {
            return context.ChartofAccounts

            .Where(x => x.isVoid != true)
                .ToList();
        }

        public List<ChartofAccount> getAllforTax()
        {
            return context.ChartofAccounts

            .Where(x => x.isVoid != true /*&&  x.Companies.Count == 0*/)
                .ToList();
        }
        public List<ChartofAccount> getAll(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);



            var dbAccounts = context.ChartofAccounts



                .Where(x => (x.isApproved == true && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account.Employees.Count != 0)
                    {
                        List<int> employeeIds = new List<int>();

                        foreach (var employee in account.Employees)
                        {
                            employeeIds.Add(employee.EmpId);
                        }
                        foreach (var employee in account.Employees)
                        {

                            if (employeeIds.Contains(user.employeeId))
                            {
                                chartofAccountsWithEmployees.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithEmployees.Add(account);
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }


        public List<ChartofAccount> getAllActive(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithCompanies = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithDepartments = new List<ChartofAccount>();
            List<ChartofAccount> chartofAccountsWithEmployees = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();



            deptIds = user.employee.departments.Select(x => x.Id).ToList();
            companyIds = user.employee.Companies.Select(x => x.Id).ToList();



            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true) || x.Companies.Count == 0)
                .ToList();

            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id))
                        {
                            chartofAccountsWithCompanies.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccountsWithCompanies.Add(account);
            }
            if (chartofAccountsWithCompanies.Count != 0)
            {
                foreach (var account in chartofAccountsWithCompanies)
                {
                    if (account.Departments.Count != 0)
                    {
                        foreach (var department in account.Departments)
                        {
                            if (deptIds.Contains(department.Id))
                            {
                                chartofAccountsWithDepartments.Add(account);
                                break;
                            }
                        }
                    }
                    else
                        chartofAccountsWithDepartments.Add(account);
                }
            }
            if (chartofAccountsWithDepartments.Count != 0)
            {
                foreach (var account in chartofAccountsWithDepartments)
                {
                    if (account != null)
                    {
                        if (account.Employees.Count != 0)
                        {
                            List<int> employeeIds = new List<int>();

                            foreach (var employee in account.Employees)
                            {
                                employeeIds.Add(employee.EmpId);
                            }
                            foreach (var employee in account.Employees)
                            {

                                if (employeeIds.Contains(user.employeeId))
                                {
                                    chartofAccountsWithEmployees.Add(account);
                                    break;
                                }
                            }
                        }
                        else
                            chartofAccountsWithEmployees.Add(account);
                    }
                }
            }
            chartofAccounts = chartofAccountsWithEmployees;
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }


        public List<ChartofAccount> getAllPendingForApprovalDepartmental(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts

                .Where(x => (x.isApproved == false && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> getAllPendingForApproval(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x => ((x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);
            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> getAllPendingForApprovalOwn(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => ((x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }
        public List<ChartofAccount> getAllPendingForAdministrator()
        {
            return context.ChartofAccounts
                .Where(x => x.isApproved == false && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<ChartofAccount> getAllPendingForReApproval(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x => ((x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }
        public List<ChartofAccount> getAllPendingForReApprovalDepartmental(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x =>
                 x.isReApproved == false && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> getAllPendingForReApprovalOwn(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x => ((x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }
        public void setChartofAccounttoVoid(int chartofAccountId, bool isVoid)
        {
            ChartofAccount purchaseOrdertoUpdate = context.ChartofAccounts.FirstOrDefault(x => x.Id == chartofAccountId);
            purchaseOrdertoUpdate.isVoid = isVoid;
            context.SaveChanges();
        }
        public List<ChartofAccount> getVoidRegister(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isVoid == true))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }
        public List<ChartofAccount> getVoidRegisterOwn(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId)) || x.Companies.Count == 0 && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> getVoidRegisterAdministrator()
        {
            return context.ChartofAccounts
             .Where(x => x.isVoid == true || x.Companies.Count == 0 && x.isVoid == true).ToList();
        }
        public List<ChartofAccount> GetAllIncomeAccountsforItem(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true && x.accountType == COA_AccountType.Income || (x.isApproved == true && x.isVoid != true && x.accountType == COA_AccountType.Other_Income)) || x.Companies.Count == 0)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }
        public List<ChartofAccount> GetAllCGSAccountsforItem(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true && x.accountType == COA_AccountType.Cost_of_Goods_Sold) || x.Companies.Count == 0)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }
        public List<ChartofAccount> GetAllReceiveableAccountsforItem(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true && x.accountType == COA_AccountType.Account_Receivable) || x.Companies.Count == 0)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;


        }
        public List<ChartofAccount> getChartofAccountsByType(int uid, COA_AccountType type)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isVoid != true) && x.accountType == type || x.Companies.Count == 0 && x.accountType == type)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> getAccountsByType(int uid, COA_AccountType type)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isVoid != true) && x.accountType == type || x.Companies.Count == 0 && x.accountType == type)
                .ToList();
            //foreach (var account in dbAccounts)
            //{
            //    if (account.Companies.Count != 0)
            //    {
            //        foreach (var dbcompany in account.Companies)
            //        {
            //            if (companyIds.Contains(dbcompany.Id))
            //            {
            //                chartofAccounts.Add(account);
            //                break;
            //            }
            //        }
            //    }
            //    else
            //        chartofAccounts.Add(account);

            //}
            return dbAccounts;

        }

        public List<ChartofAccount> GetChartofAccountsForDeductions(int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isVoid != true) || x.Companies.Count == 0 && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(dbcompany.Id))
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }
        public List<ChartofAccount> GetChartofAccountsByCompany(int uid, Company company)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true) || x.Companies.Count == 0 && x.isApproved == true && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var dbcompany in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && dbcompany.Id == company.Id)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }



        public List<ChartofAccount> GetChartofAccountsByCompanyDept(int uId, Company company, Department dept)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var dbAccounts = context.ChartofAccounts
                .Where(x => (x.isApproved == true && x.isVoid != true) || x.Companies.Count == 0 && x.isApproved == true && x.isVoid != true)
                .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    //foreach (var dbcompany in account.Companies)
                    //{
                    if (companyIds.Contains(company.Id) && account.Companies.Find(x => x.Id == company.Id) != null && deptIds.Contains(dept.Id) && account.Departments.Find(x => x.Id == dept.Id) != null)
                    {
                        chartofAccounts.Add(account);
                        break;
                    }
                    //}
                }
                else
                    chartofAccounts.Add(account);

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;

        }


        public void Reconcile(List<JournalTransaction> journalTransactions, ChartofAccount chartofAccount)
        {
            var dbAccount = context.ChartofAccounts.FirstOrDefault(x => x.Id == chartofAccount.Id);
            foreach (var transaction in journalTransactions)
            {
                foreach (var dbTransaction in dbAccount.JournalTransactions)
                {
                    if (dbTransaction.Id == transaction.Id)
                    {
                        dbTransaction.isReconciled = true;
                        dbTransaction.reconcilationDate = transaction.reconcilationDate;
                        dbTransaction.reconcilationType = transaction.reconcilationType;
                    }
                }
            }
            dbAccount.Reconcilations.AddRange(chartofAccount.Reconcilations);
            context.SaveChanges();

        }
        public List<ChartofAccount> GetAll()
        {
            return context.ChartofAccounts/*.Where(x=>x.isActive==true)*/.ToList();
        }
        public List<ChartofAccount> GetAllforVendorBills(Company billCompany, Department billDept, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DepartmentRepo deptRepo = new DepartmentRepo();
            var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();
            if (deptMngt != null)
                foreach (var _dept in deptMngt)
                {
                    if (deptIds.Contains(_dept.Id) == false)
                        deptIds.Add(_dept.Id);
                }

            var dbAccounts = context.ChartofAccounts.Where(x => x.isApproved == true && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == true).ToList();


            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && company.Id == billCompany.Id)
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
                    foreach (var dept in account.Departments)
                    {
                        if (deptIds.Contains(dept.Id) && dept.Id == billDept.Id)
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
        public List<ChartofAccount> GetAllforPayment(Company billCompany, List<Department> Depts, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DepartmentRepo deptRepo = new DepartmentRepo();
            var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();
            if (deptMngt != null)
                foreach (var _dept in deptMngt)
                {
                    if (deptIds.Contains(_dept.Id) == false)
                        deptIds.Add(_dept.Id);
                }

            var dbAccounts = context.ChartofAccounts.Where(x => x.isApproved == true && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == true).ToList();


            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && company.Id == billCompany.Id)
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
                    foreach (var dept in account.Departments)
                    {
                        foreach (var department in Depts)
                        {
                            if (deptIds.Contains(department.Id) && dept.Id == department.Id)
                            {
                                accountsToReturn.Add(account);
                                break;
                            }
                        }

                    }
                }
            }
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn;
        }


        public void FixNullTransactions()
        {
            var journlTransactions = context.journalTransactions.Where(x =>
             x.journalVoucher_id == null &&
             x.Bill_Id == null &&
             x.SaleInvoiceId == null
             && x.SaleReceiptId == null &&
             x.PurchaseInvoiceId == null &&
             x.PaymentId == null &&
             x.InterBankId == null &&
             x.InterCompanyId == null &&
             x.AdminBillId == null
            ).ToList();
            context.journalTransactions.RemoveRange(journlTransactions);
            context.SaveChanges();

        }
        public List<ChartofAccount> getAllLedgerAccounts(int userId)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts.Where(x => x.JournalTransactions.Count > 0)

            .ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies != null)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && account.isApproved == true || account.Companies.Count == 0 && account.isApproved == true)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                {
                    chartofAccounts.Add(account);
                }

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }
        public List<ChartofAccount> getAllTrialBalanceAccounts(int userId)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var dbAccounts = context.ChartofAccounts.Where(x => x.JournalTransactions.Count > 0 && x.isActiveForTrialBalance==true).ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies != null)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && account.isApproved == true || account.Companies.Count == 0 && account.isApproved == true)
                        {
                            chartofAccounts.Add(account);
                            break;
                        }
                    }
                }
                else
                {
                    chartofAccounts.Add(account);
                }

            }
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }
        public List<ChartofAccount> GetAllInventoryAdjustments(Company billCompany, Department billDept, int uid)
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            List<ChartofAccount> accountsToReturn = new List<ChartofAccount>();
            var user = context.Users.FirstOrDefault(x => x.employeeId == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DepartmentRepo deptRepo = new DepartmentRepo();
            var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();
            if (deptMngt != null)
                foreach (var _dept in deptMngt)
                {
                    if (deptIds.Contains(_dept.Id) == false)
                        deptIds.Add(_dept.Id);
                }

            var dbAccounts = context.ChartofAccounts.Where(x => x.isApproved == true && x.isVoid != true || x.Companies.Count == 0 && x.isApproved == true).ToList();
            foreach (var account in dbAccounts)
            {
                if (account.Companies.Count != 0)
                {
                    foreach (var company in account.Companies)
                    {
                        if (companyIds.Contains(company.Id) && company.Id == billCompany.Id)
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
                    foreach (var dept in account.Departments)
                    {
                        if (deptIds.Contains(dept.Id) && dept.Id == billDept.Id)
                        {
                            accountsToReturn.Add(account);
                            break;
                        }
                    }
                }
            }
            accountsToReturn.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            accountsToReturn = accountsToReturn.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return accountsToReturn;
        }
        public List<Company> GetEmployeeCompanies(int empId)
        {          

            return context.Companies.Where(x => x.employees.FirstOrDefault(y=>y.EmpId == empId) != null).ToList(); 
        }
        public List<Department> GetEmployeeDepartments(int empId)
        {
            var employee = context.Employees.FirstOrDefault(x => x.EmpId == empId);
            return employee.departments;
        } 
        public void AddCoaGroup(ChartofAccountGroup _group)
        {
            //ChartofAccountGroup group = new ChartofAccountGroup();

            //group.Title = _group.Title;
            //group.referenceNo = _group.referenceNo;
            //if (_group.Companies != null)
            //{
            //    foreach (var company in _group.Companies)
            //    {
            //        var dbCompany = context.Companies.FirstOrDefault(x => x.Id == company.Id);
            //        group.Companies.Add(dbCompany);
            //    }
            //}
            //if (_group.Departments != null)
            //{
            //    foreach (var department in _group.Departments)
            //    {
            //        var dbDepartment = context.Departments.FirstOrDefault(x => x.Id == department.Id);
            //        group.Departments.Add(dbDepartment);
            //    }
            //}
            //if (_group.Employees != null)
            //{
            //    foreach (var employee in _group.Employees)
            //    {
            //        var dbEmployee = context.Employees.FirstOrDefault(x => x.EmpId == employee.EmpId);
            //        group.Employees.Add(dbEmployee);
            //    }
            //}
            context.chartofAccountGroups.Add(_group);
            context.SaveChanges();
        }
        public void UpdateCoaGroup(ChartofAccountGroup _group)
        {
            var dbGroup = context.chartofAccountGroups
                                 .Include(x => x.Companies)
                                 .Include(x => x.Departments)
                                 .Include(x => x.Employees)
                                 .FirstOrDefault(x => x.Id == _group.Id);

            if (dbGroup == null)
                throw new Exception("Group not found");

            dbGroup.referenceNo = _group.referenceNo;
            dbGroup.Title = _group.Title;

            // Update Companies
            dbGroup.Companies.Clear();
            foreach (var _id in _group.Companies.Select(x => x.Id))
            {
                var company = context.Companies.FirstOrDefault(x => x.Id == _id);
                if (company != null)
                    dbGroup.Companies.Add(company);
            }

            // Update Departments
            dbGroup.Departments.Clear();
            foreach (var _id in _group.Departments.Select(x => x.Id))
            {
                var department = context.Departments.FirstOrDefault(x => x.Id == _id);
                if (department != null)
                    dbGroup.Departments.Add(department);
            }

            // Update Employees
            dbGroup.Employees.Clear();
            foreach (var _id in _group.Employees.Select(x => x.EmpId))
            {
                var employee = context.Employees.FirstOrDefault(x => x.EmpId == _id);
                if (employee != null)
                    dbGroup.Employees.Add(employee);
            }

            // Save changes
            context.Entry(dbGroup).State = EntityState.Modified;
            context.SaveChanges();
        }
        public List<ChartofAccountGroup> GetAllChartofAccountGroups()
        {
              return context.chartofAccountGroups.ToList();
        }

        public List<ERP_BL.Databases.User> getAllusersByEmpId(int empId)
        {
            return context.Users

                .Where(x => x.employeeId== empId && x.isActive == true)
                .ToList();
        }
        public List<ChartofAccount> getAllActiveBankForCashFlow(int uid)
        {
            var user = context.Users
                     .Include(u => u.employee.departments)
                     .Include(u => u.employee.Companies)
                     .FirstOrDefault(x => x.id == uid);

            if (user == null || user.employee == null)
                return new List<ChartofAccount>();

            var deptIds = user.employee.departments.Select(x => x.Id).ToHashSet();
            var companyIds = user.employee.Companies.Select(x => x.Id).ToHashSet();

            var chartofAccounts = context.ChartofAccounts
                .Include(c => c.Companies)
                .Include(c => c.Departments)
                .Include(c => c.Employees)
                .Where(x => x.isApproved==true && x.isVoid!=true && x.JournalTransactions.Count > 0)
                .AsEnumerable()
                .Where(account => account.accountType==COA_AccountType.Bank &&
                    !account.Companies.Any() || account.accountType == COA_AccountType.Bank && account.Companies.Any(company => companyIds.Contains(company.Id)))
                .Where(account => account.accountType == COA_AccountType.Bank &&
                    !account.Departments.Any() || account.accountType == COA_AccountType.Bank && account.Departments.Any(department => deptIds.Contains(department.Id)))
                .Where(account => account.accountType == COA_AccountType.Bank &&
                    !account.Employees.Any() || account.accountType == COA_AccountType.Bank && account.Employees.Any(employee => employee.EmpId == user.employeeId))
                .ToList();

            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts.Where(account => account.accountType == COA_AccountType.Bank)).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;




        }
        public List<ChartofAccountGroup> GetAllChartofAccountGroups(int uid)
        {
            var user = context.Users
                       .Include(u => u.employee.departments)
                       .Include(u => u.employee.Companies)
                       .FirstOrDefault(x => x.id == uid);

            var deptIds = user.employee.departments.Select(x => x.Id).ToHashSet();
            var companyIds = user.employee.Companies.Select(x => x.Id).ToHashSet();

            var chartofAccounts = context.chartofAccountGroups
                .Include(c => c.Companies)
                .Include(c => c.Departments)
                .Include(c => c.Employees)
                .AsEnumerable()
                .Where(account => 
                    !account.Companies.Any() ||   account.Companies.Any(company => companyIds.Contains(company.Id)))
                .Where(account => 
                    !account.Departments.Any() ||   account.Departments.Any(department => deptIds.Contains(department.Id)))
                .Where(account =>
                    !account.Employees.Any() ||  account.Employees.Any(employee => employee.EmpId == user.employeeId))
                .ToList();

            return chartofAccounts;
        }
        public List<ChartofAccount> getAllActiveForCashFlow(int uid)
        {
            var user = context.Users
                  
                     .FirstOrDefault(x => x.id == uid);

            if (user == null || user.employee == null)
                return new List<ChartofAccount>();

            var deptIds = user.employee.departments.Select(x => x.Id).ToHashSet();
            var companyIds = user.employee.Companies.Select(x => x.Id).ToHashSet();

            var chartofAccounts = context.ChartofAccounts
                .Include(c => c.Companies)
                .Include(c => c.Departments)
                .Include(c => c.Employees)
                .Where(x => x.isApproved == true && x.isVoid != true && x.JournalTransactions.Count>0 && x.accountType==COA_AccountType.Bank)
                .AsEnumerable()
                .Where(account =>
                    !account.Companies.Any() || account.Companies.Any(company => companyIds.Contains(company.Id)))
                .Where(account =>
                    !account.Departments.Any() || account.Departments.Any(department => deptIds.Contains(department.Id)))
                .Where(account =>
                    !account.Employees.Any() || account.Employees.Any(employee => employee.EmpId == user.employeeId))
                .ToList();
            chartofAccounts.AddRange(user.employee.ChartofAccountGroups.SelectMany(x => x.ChartofAccounts).ToList());
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            return chartofAccounts;
        }
        public void UpdateAccountForCashFlow(ChartofAccount Account)
        {
            var dbAccount = context.ChartofAccounts.FirstOrDefault(x => x.Id == Account.Id);
            dbAccount.manualBalanceOC = Account.manualBalanceOC;
            dbAccount.manualBalancePKR = Account.manualBalancePKR;      
            context.SaveChanges();
        }
    }
}
