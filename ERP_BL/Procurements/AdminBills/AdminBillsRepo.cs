using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace ERP_BL.Procurements.AdminBills
{
    public class AdminBillsRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Adding new Multiple bills
        /// </summary>
        /// <param name="bill"></param>
        public void AddBills(List<AdminBill> bills)
        {
            if(bills != null)
            {
                foreach(var _bill in bills)
                {
                    context.adminBills.Add(_bill);
                }
                context.SaveChanges();
            }
        }

        public void FixNullTransactions()
        {
            var NullTransactions = context.adjustments.Where(x => x.adminBillId == null).ToList();
            context.adjustments.RemoveRange(NullTransactions);
            context.SaveChanges();
        }

        public void UpdateBillsNew(List<AdminBill> bills, List<AdminBill> removedBills, List<AdminBill> addedBills)
        {
            //using (context)
            //{
            List<int> jtIds = new List<int>();
            List<int> pettyCashIds = new List<int>();
            List<int> adjustmentIds = new List<int>();
            List<int> vatBooksIds = new List<int>();
            foreach (AdminBill bill in bills)
                {
                    var _bill = context.adminBills.FirstOrDefault(x => x.Id == bill.Id);

                    jtIds.AddRange(context.journalTransactions.Where(c => c.AdminBillId == _bill.Id).Select(x=>x.Id).ToList());
                    pettyCashIds.AddRange(context.pettyCashes.Where(c => c.AdminBillId == _bill.Id).Select(x => x.Id).ToList());
                    adjustmentIds.AddRange(context.adjustments.Where(p => p.adminBillId == _bill.Id).Select(x => x.Id).ToList());
                    vatBooksIds.AddRange(context.VATBooks.Where(p => p.adminBillId == _bill.Id).Select(x => x.Id).ToList());
                    _bill = bill;
                }

                if (removedBills != null)
                {
                    foreach (AdminBill _bill in removedBills)
                    {
                        var bill = context.adminBills.FirstOrDefault(x => x.Id == _bill.Id);
                        context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.AdminBillId == _bill.Id));
                        context.adminBills.Remove(bill);
                    }
                }

                if (addedBills.Count > 0)
                {
                    foreach (var _bill in addedBills)
                    {
                        context.adminBills.Add(_bill);
                    }
                }
            context.SaveChanges();

            if (jtIds.Count > 0)
            {
                jtIds.Distinct();
                context.journalTransactions.Where(x => jtIds.Contains(x.Id)).Delete();
            }
            if (pettyCashIds.Count > 0)
            {
                pettyCashIds.Distinct();
                context.pettyCashes.Where(x => pettyCashIds.Contains(x.Id)).Delete();
            }    
            if (vatBooksIds.Count > 0)
            {
                vatBooksIds.Distinct();
                context.VATBooks.Where(x => vatBooksIds.Contains(x.Id)).Delete();
            }
        }
        /// <summary>
        /// Update Existing Bills
        /// </summary>
        /// <param name="bills"></param>
        /// <param name="groupId"></param>
        public void UpdateBill(List<AdminBill> bills, List<AdminBill> removedBills, List<AdminBill> addedBills)
        {
            //List<AdminBill> _bills = new List<AdminBill>();
            if (bills != null)
            {
                foreach(AdminBill bill in bills)
                {
                    var _bill = context.adminBills.FirstOrDefault(x => x.Id == bill.Id);

                    

                    if (_bill.journalTransactions != null)
                    {
                        var alltransactions = context.journalTransactions.Where(x => x.AdminBillId == _bill.Id).ToList();
                        //context.journalTransactions.RemoveRange(alltransactions);
                        foreach(var _trans in alltransactions)
                            context.journalTransactions.Remove(_trans);

                        
                    }
                    if (_bill.pettyCashes != null)
                    {
                        var alltransactions = context.pettyCashes.Where(x => x.AdminBillId == _bill.Id).ToList();
                        //context.pettyCashes.RemoveRange(alltransactions);
                        foreach (var _trans in alltransactions)
                            context.pettyCashes.Remove(_trans);
                    }
                    if (_bill.Adjustments != null)
                    {
                        var alltransactions = context.adjustments.Where(x => x.adminBillId == null).ToList();
                        //context.adjustments.RemoveRange(alltransactions);
                        foreach (var _trans in alltransactions)
                            context.adjustments.Remove(_trans);
                    }
                    _bill = bill;
                }
            }

            if (removedBills != null)
            {
                foreach (AdminBill _bill in removedBills)
                {
                    var bill = context.adminBills.FirstOrDefault(x => x.Id == _bill.Id);
                    context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.AdminBillId == _bill.Id));
                    context.adminBills.Remove(bill);
                }
            }

            if (addedBills.Count > 0)
            {
                foreach (var _bill in addedBills)
                {
                    context.adminBills.Add(_bill);
                }
            }

            context.SaveChanges();

        }

        public List<Adjustment> GetAdjustmentsByBillId(int billId)
        {
            return context.adjustments.Where(x=>x.adminBillId == billId).ToList();
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

            return context.adminBills

            //.Where(x => x.PendingForClosing == true || x.PendingForClosing == null)
            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getAllAssetTypeBills(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.adminBills

            //.Where(x => x.PendingForClosing == true || x.PendingForClosing == null)
            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid)  && x.isVoid != true && x.billTypes == Enums.AdminBillTypes.Asset).ToList();
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

            return context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
            //.Where(x => x.PendingForClosing == false)
            .Count();
        }


        /// <summary>
        /// Approve Existing Bills
        /// </summary>
        /// <param name="bills"></param>
        /// <param name="groupId"></param>
        public void ApproveBill(List<AdminBill> bills)
        {
            //List<AdminBill> _bills = new List<AdminBill>();
            if (bills != null)
            {
                foreach (AdminBill bill in bills)
                {
                    var _bill = context.adminBills.FirstOrDefault(x => x.Id == bill.Id);

                    _bill = bill;

                }
            }

            context.SaveChanges();
        }


        /// <summary>
        /// Approve Existing Bills
        /// </summary>
        /// <param name="bills"></param>
        /// <param name="groupId"></param>
        public void DirectCloseBill(List<AdminBill> bills)
        {
            if (bills != null)
            {
                foreach (AdminBill bill in bills)
                {
                    var _bill = context.adminBills.FirstOrDefault(x => x.Id == bill.Id);

                    _bill = bill;
                  
                    
                }
            }

            context.SaveChanges();
        }


        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public int GetLastBill()
        {
            //var bills = context.adminBills.ToList();
            //if (bills == null || bills.Count == 0)
            //    return null;

            //return bills.Last();
            var month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var currentMonthYear = DateTime.Now.Year.ToString() + month.ToString();

            
            //var lastId = context.adminBills.Max(x => x.transactionGroupId);
            var listOfBills = context.adminBills.Where(x => x.SystemRefNo.Contains(currentMonthYear)).ToList();
            if (listOfBills.Count > 0)
            {
                var lastId = listOfBills.Max(x => x.transactionGroupId);
                return lastId;
            }
            else
                return 0;
            //return 0;
            
        }


        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public int GetLastVehicleExpense()
        {
            //var bills = context.adminBills.ToList();
            //if (bills == null || bills.Count == 0)
            //    return null;

            //return bills.Last();
            var month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var currentMonthYear = DateTime.Now.Year.ToString() + month.ToString();


            //var lastId = context.adminBills.Max(x => x.transactionGroupId);
            var listOfBills = context.vehicleExpenses.Where(x => x.SystemRefNo.Contains(currentMonthYear)).ToList();
            if (listOfBills.Count > 0)
            {
                var lastId = listOfBills.Max(x => x.transactionGroupId);
                return lastId;
            }
            else
                return 0;
            //return 0;

        }


        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public VehicleExpenses GetLastVehicleExpenseByPayee(int payeeId)
        {

            //var lastId = context.adminBills.Max(x => x.transactionGroupId);
            var listOfVehicleExpenses = context.vehicleExpenses.Where(x => x.payeeId == payeeId && x.expenseType== Enums.VehicleExpenseType.Maintenance).ToList();
            if (listOfVehicleExpenses != null && listOfVehicleExpenses.Count > 0)
            {
                var lastEntry = listOfVehicleExpenses.Last();
                return lastEntry;
            }
            else
                return null;
            //return 0;

        }

        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public List< VehicleExpenses> GetAllVehicleExpensesByPayee(int payeeId)
        {
            return context.vehicleExpenses.Where(x=>x.payeeId==payeeId).ToList();
        }


        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public VehicleExpenses GetLastFuelExpensesByPayee(int payeeId)
        {

            //var lastId = context.adminBills.Max(x => x.transactionGroupId);
            var listOfVehicleExpenses = context.vehicleExpenses.Where(x => x.payeeId == payeeId && x.expenseType == Enums.VehicleExpenseType.Fuel).ToList();
            if (listOfVehicleExpenses != null && listOfVehicleExpenses.Count > 0)
            {
                var lastEntry = listOfVehicleExpenses.Last();
                return lastEntry;
            }
            else
                return null;
            //return 0;

        }

        /// <summary>
        /// Get All Bills
        /// </summary>
        /// <returns></returns>
        public AdminBill GetBill(int billId)
        {
            return
            context.adminBills


            .FirstOrDefault(x=>x.Id == billId);
        }

        /// <summary>
        /// Get All Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllBillsByCompDept(int compId, List<Department> departments, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach(var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var adminBills = context.adminBills

           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<AdminBill> billsToReturn = new List<AdminBill>();
            foreach(var _bill in adminBills)
            {
                if (dept_Ids.Contains((int)_bill.dept_Id))
                {
                    billsToReturn.Add(_bill);
                }
            }

            return billsToReturn;
        }


        /// <summary>
        /// Get All Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllBillsByCompDeptVendor(int compId, List<Department> departments, int vendorId, int currId)
        {

            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var adminBills = context.adminBills

           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.vendor_Id == vendorId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();


            List<AdminBill> billsToReturn = new List<AdminBill>();
            foreach (var _bill in adminBills)
            {
                if (dept_Ids.Contains((int)_bill.dept_Id))
                {
                    billsToReturn.Add(_bill);
                }
            }

            return billsToReturn;
            //if (adminFlag == true)
            //    return adminBills.Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id)).ToList();
            //else
            //    return adminBills.Where(x => x.creatorId == user.employeeId).ToList();
        }



        /// <summary>
        /// Get All Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllBillsByCompDeptPrimaryCard(int compId, List<Department> departments, int cardId, int currId)
        {

            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var adminBills = context.adminBills
          

           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.PrimaryCreditCardNoId == cardId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();


            List<AdminBill> billsToReturn = new List<AdminBill>();
            foreach (var _bill in adminBills)
            {
                if (dept_Ids.Contains((int)_bill.dept_Id))
                {
                    billsToReturn.Add(_bill);
                }
            }

            return billsToReturn;

        }


        /// <summary>
        /// Get All Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllBills(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);


            //bool adminFlag = false;
            //DepartmentRepo deptRepo = new DepartmentRepo();
            //var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();

            //if (deptMngt != null)
            //    foreach (var _dept in deptMngt)
            //    {
            //        if (deptIds.Contains(_dept.Id))
            //        {
            //            adminFlag = true;
            //            break;
            //        }
            //    }


            //List<AdminBill> adminBills = new List<AdminBill>();
            //adminBills = 
             return   context.adminBills
            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isVoid != true)
            .ToList();

            //if (adminFlag == true)
            //    return adminBills.Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id)).ToList();
            //else
            //    return adminBills.Where(x => x.creatorId == user.employeeId).ToList();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfer own.
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

               return context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isVoid == true)
                .ToList();


        }


        /// <summary>
        /// Get All Open Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllOpenBills(int uid)
        {
           

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

               var Bills = context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
            .ToList();

            return Bills;
            //if (adminFlag == true)
            //    return adminBills.Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id)).ToList(); 
            //else
            //    return adminBills.Where(x=>x.creatorId == user.employeeId).ToList();
        }


        /// <summary>
        /// Get All Pending For Approval Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllPendingForApprovalBills(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

               return context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isApproved == false && x.isVoid != true)
            .ToList();


        }


        /// <summary>
        /// Admin Bills Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<AdminBill> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

               return context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();

        }


        /// <summary>
        /// Get All Pending For Closing Bills
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetAllPendingForClosingBills()
        {
            return
            context.adminBills

            .Where(x => x.isApproved == false && x.isVoid != true)
            .ToList();
        }



        public List<AdminBill> GetAllOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

               return context.adminBills

             .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
             .ToList();
        }



        /// <summary>
        /// Get all active Inter-Bank Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

               return context.adminBills


                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Inactive Inter-Bank Transfers except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getAllInActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

              return  context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getAllSaleReceiptsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

              return  context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.BillStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();

        }


        /// <summary>
        /// Get all pending for closing Bills by Departmental
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.adminBills

            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.PendingForClosing == true && x.isVoid != true)    
            .ToList();
        }

        /// <summary>
        /// Get All Bills by Group Id
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetBillsByGroupId(int transactionGroupId)
        {
            return
            context.adminBills
            .Where(x=>x.transactionGroupId == transactionGroupId)
            .ToList();
        }


        /// <summary>
        /// Get All Bills by Group Id
        /// </summary>
        /// <returns></returns>
        public List<AdminBill> GetBillsByGroupIdForForm(int transactionGroupId)
        {
            return
            context.adminBills

            .Where(x => x.transactionGroupId == transactionGroupId)
            .ToList();
        }


        public List<AdminBillStatus> GetAllCloseBillStatus()
        {
            var statusList = context.adminBillStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }



        /// <summary>
        /// Add New Bill Status
        /// </summary>
        /// <param name="billStatus"></param>
        public void AddBillStatus(AdminBillStatus billStatus)
        {
            context.adminBillStatuses.Add(billStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing bill Status
        /// </summary>
        /// <param name="billStatus"></param>
        public void UpdateBillStatus(AdminBillStatus billStatus)
        {
            AdminBillStatus _billStatus = context.adminBillStatuses.FirstOrDefault(x=>x.Id == billStatus.Id);
            _billStatus.Status = billStatus.Status;
            _billStatus.isActive = billStatus.isActive;
            _billStatus.forecolor = billStatus.forecolor;
            _billStatus.backcolor = billStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A bill Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public AdminBillStatus GetBillStatus(int statusId)
        {
            return context.adminBillStatuses
                //.Include("Bills")
                .FirstOrDefault(x=>x.Id == statusId);
        }


        /// <summary>
        /// Get All Bill Statuses
        /// </summary>
        /// <returns></returns>
        public List<AdminBillStatus> GetAllBillStatuses()
        {
            return context.adminBillStatuses
                //.Include("Bills")
                .ToList();
        }

        /// <summary>
        /// Get All Bill Statuses
        /// </summary>
        /// <returns></returns>
        public List<AdminBillStatus> GetAllOpenBillStatuses()
        {
            return context.adminBillStatuses
                //.Include("Bills")
                .Where(x=>x.isActive == true)
                .ToList();
        }


        /// <summary>
        /// Add New Category
        /// </summary>
        /// <param name="category"></param>
        public void AddPayeeCategpry(PayeeCategory category)
        {
            context.payeeCategories.Add(category);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Payee Category
        /// </summary>
        /// <param name="category"></param>
        public void UpdatePayeeCategory(PayeeCategory category)
        {
            PayeeCategory _category = context.payeeCategories.FirstOrDefault(x => x.Id == category.Id);

            _category.Name = category.Name;
            _category.isActive = category.isActive;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Payee Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public PayeeCategory GetPayeeCategory(int categoryId)
        {
            return
                context.payeeCategories.FirstOrDefault(x=>x.Id == categoryId);
        }


        /// <summary>
        /// Get All Payee Categories
        /// </summary>
        /// <returns></returns>
        public List<PayeeCategory> GetAllPayeeCategories()
        {
            return
            context.payeeCategories
            .ToList();
        }


        /// <summary>
        /// Add New Bill Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        public void AddBillReferenceNo(BillRefNumber refNumber)
        {
            context.billRefNumbers.Add(refNumber);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Bill Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        public void UpdateBillReferenceNo(BillRefNumber refNumber)
        {
            BillRefNumber _billRefNumber = context.billRefNumbers.FirstOrDefault(x => x.Id == refNumber.Id);

            _billRefNumber.BillReferenceNo = refNumber.BillReferenceNo;
            _billRefNumber.companyId = refNumber.companyId;
            _billRefNumber.isActive = refNumber.isActive;
            context.SaveChanges();
        }



        /// <summary>
        /// Get Bill Reference Number
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        public BillRefNumber GetBillReferenceNo(int refId)
        {
            return
                context.billRefNumbers
      
                .FirstOrDefault(x => x.Id == refId);
        }


        /// <summary>
        /// Get All Active Reference Numbers
        /// </summary>
        /// <returns></returns>
        public List<BillRefNumber> GetAllActiveBillReferenceNo(int compId)
        {
            return
            context.billRefNumbers

            .Where(x=>x.companyId == compId && x.isActive == true)
            .ToList();
        }


        /// <summary>
        /// Get All Reference Numbers
        /// </summary>
        /// <returns></returns>
        public List<BillRefNumber> GetAllBillRefNo()
        {
            return
            context.billRefNumbers
    
            .ToList();
        }


        /// <summary>
        /// Add New Payee
        /// </summary>
        /// <param name="payee"></param>
        public void AddPayee(Payee payee)
        {
            Payee _payee = new Payee();
            if (payee == null)
                throw new NullReferenceException("Object can not be null");

            if (payee.ParentId != null)
                _payee.parentPayee = context.payees.FirstOrDefault(x => x.Id == payee.ParentId);

            //if (payee.IndustryTypeId != null)
            //    _payee.industryType = context.IndustryTypes.FirstOrDefault(x => x.Id == payee.IndustryTypeId);

            _payee.isActive = payee.isActive;
            _payee.isSubsidiary = payee.isSubsidiary;
            _payee.PayeeName = payee.PayeeName;

            if (payee.isVehicleType == true)
            {
                _payee.isVehicleType = true;
                if (payee.isOwned == true)
                {
                    _payee.isOwned = true;

                    if (payee.companyId != null)
                        _payee.companyId = payee.companyId;
                }
                else
                {
                    payee.isOwned = false;

                    if (payee.vehicleOwnerId != null)
                        _payee.VehicleOwner = context.rentedVehicleOwners.FirstOrDefault(x => x.Id == payee.vehicleOwnerId);
                }
            }
            else
            {
                _payee.isVehicleType = false;
                _payee.isOwned = false;
                _payee.companyId = null;
                _payee.vehicleOwnerId = null;
            }

            if (payee.Companies != null)
            {
                _payee.Companies = new List<Company>();
                foreach (var _company in payee.Companies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == _company.Id);
                    _payee.Companies.Add(company);
                }
            }

            if (payee.departments != null)
            {
                _payee.departments = new List<Department>();
                foreach (var _dept in payee.departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                    _payee.departments.Add(dept);
                }
            }

            if (payee.AdminBillTypes != null)
            {
                _payee.AdminBillTypes = new List<AdminBillType>();
                foreach (var _type in payee.AdminBillTypes)
                {
                    var type = context.adminBillTypes.FirstOrDefault(x => x.Id == _type.Id);
                    _payee.AdminBillTypes.Add(type);
                }
            }

            if (payee.vendors != null)
            {
                _payee.vendors = new List<Vendor>();
                foreach (var _vendor in payee.vendors)
                {
                    var vendor = context.Vendors.FirstOrDefault(x => x.Id == _vendor.Id);
                    _payee.vendors.Add(vendor);
                }
            }

            context.payees.Add(_payee);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Payee
        /// </summary>
        /// <param name="category"></param>
        public void UpdatePayee(Payee payee)
        {
            Payee _payee = context.payees.FirstOrDefault(x => x.Id == payee.Id);

            if (payee == null)
                throw new NullReferenceException("Object can not be null");

            

            _payee.isActive = payee.isActive;

            if(payee.isSubsidiary == true)
            {
                _payee.isSubsidiary = true;
                if (payee.ParentId != null)
                    _payee.parentPayee = context.payees.FirstOrDefault(x => x.Id == payee.ParentId);
            }
            else
            {
                _payee.isSubsidiary = false;
                _payee.parentPayee = null;
                _payee.ParentId = null;
            }

            if (payee.isVehicleType == true)
            {
                _payee.isVehicleType = true;
                if (payee.isOwned == true)
                {
                    _payee.isOwned = true;
                        
                    if (payee.companyId != null)
                    {
                        _payee.companyId = payee.companyId;
                        _payee.vehicleOwnerId = null;
                    }
                        
                }
                else
                {
                    _payee.isOwned = false;

                    if (payee.vehicleOwnerId != null)
                    {
                        _payee.vehicleOwnerId = payee.vehicleOwnerId;
                        _payee.companyId = null;
                    }

                }
            }
            else
            {
                _payee.isVehicleType = false;
                _payee.isOwned = false;
                _payee.companyId = null;
                _payee.vehicleOwnerId = null;
            }

            //if (payee.IndustryTypeId != null)
            //    _payee.industryType = context.IndustryTypes.FirstOrDefault(x => x.Id == payee.IndustryTypeId);

            _payee.PayeeName = payee.PayeeName;

            _payee.Companies.Clear();
            if (payee.Companies != null)
            {
                foreach (var _company in payee.Companies)
                {
                    if (_payee.Companies.Contains(_company) == false)
                    {
                        var company = context.Companies.FirstOrDefault(x => x.Id == _company.Id);
                        _payee.Companies.Add(company);
                    }
                }
            }

            _payee.departments.Clear();
            if (payee.departments != null)
            {
                foreach (var _dept in payee.departments)
                {
                    if (_payee.departments.Contains(_dept) == false)
                    {
                        var dept = context.Departments.FirstOrDefault(x => x.Id == _dept.Id);
                        _payee.departments.Add(dept);
                    }
                }
            }


            _payee.AdminBillTypes.Clear();
            if (payee.AdminBillTypes != null)
            {
                foreach (var _type in payee.AdminBillTypes)
                {
                    if (_payee.AdminBillTypes.Contains(_type) == false)
                    {
                        var type = context.adminBillTypes.FirstOrDefault(x => x.Id == _type.Id);
                        _payee.AdminBillTypes.Add(type);
                    }
                }
            }


            _payee.vendors.Clear();
            if (payee.vendors != null)
            {
                foreach (var _vendor in payee.vendors)
                {
                    if (_payee.vendors.Contains(_vendor) == false)
                    {
                        var vendor = context.Vendors.FirstOrDefault(x => x.Id == _vendor.Id);
                        _payee.vendors.Add(vendor);
                    }
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Get Payee
        /// </summary>
        /// <param name="payeeId"></param>
        /// <returns></returns>
        public Payee GetPayeeForBillRegister(int payeeId)
        {
            return

                context.payees
                .FirstOrDefault(x => x.Id == payeeId);
        }

        /// <summary>
        /// Get Payee
        /// </summary>
        /// <param name="payeeId"></param>
        /// <returns></returns>
        public Payee GetPayee(int payeeId)
        {
            return
                
                context.payees

                .FirstOrDefault(x => x.Id == payeeId);
        }


        /// <summary>
        /// Get All Payees
        /// </summary>
        /// <returns></returns>
        public List<Payee> GetAllPayees()
        {
            return
            context.payees

            .ToList();
        }


        /// <summary>
        /// Get Payees of a Company and Department
        /// </summary>
        /// <returns></returns>
        public List<Payee> GetAllPayeesOfCompanyDept( Company company, Department department,AdminBillType adminBillType, Vendor vendor)
        {
            List<Payee> payees = new List<Payee>();
            //List<Payee> payeesToReturn = new List<Payee>();


            payees = context.payees

            .Where(x=>x.Companies.FirstOrDefault(y=>y.Id == company.Id) != null && x.departments.FirstOrDefault(y=>y.Id == department.Id)!=null && x.AdminBillTypes.FirstOrDefault(y=>y.Id == adminBillType.Id)!=null && x.vendors.FirstOrDefault(y=>y.Id== vendor.Id) !=null && x.isActive == true)
            .ToList();
            return payees;
        }



        /// <summary>
        /// Add New Admin Bill Type
        /// </summary>
        /// <param name="payee"></param>
        public void AddAdminBillType(AdminBillType adminBillType)
        {
            AdminBillType _adminBillType = new AdminBillType();
            if (adminBillType == null)
                throw new NullReferenceException("Object can not be null");

            _adminBillType.name = adminBillType.name;
            _adminBillType.isActive = adminBillType.isActive;
            _adminBillType.isApproved = adminBillType.isApproved;
            _adminBillType.user_Id = adminBillType.user_Id;
            _adminBillType.addedDate = DateTime.Now;
            
            if (adminBillType.vendors != null)
            {
                _adminBillType.vendors = new List<Vendor>();
                foreach (var _vendor in adminBillType.vendors)
                {
                    var vendor = context.Vendors.FirstOrDefault(x => x.Id == _vendor.Id);
                    _adminBillType.vendors.Add(vendor);
                }
            }

            if (adminBillType.ChartofAccounts != null)
            {
                _adminBillType.ChartofAccounts = new List<ChartofAccount>();
                foreach (var _coa in adminBillType.ChartofAccounts)
                {
                    var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == _coa.Id);
                    _adminBillType.ChartofAccounts.Add(coa);
                }
            }

            context.adminBillTypes.Add(_adminBillType);
            context.SaveChanges();
        }


        /// <summary>
        /// update Admin Bill Type
        /// </summary>
        /// <param name="category"></param>
        public void UpdateAdminBillType(AdminBillType adminBillType)
        {
            AdminBillType _adminBillType = context.adminBillTypes.FirstOrDefault(x => x.Id == adminBillType.Id);

            if (adminBillType == null)
                throw new NullReferenceException("Object can not be null");

            _adminBillType.name = adminBillType.name;
            _adminBillType.isActive = adminBillType.isActive;
            _adminBillType.isApproved = adminBillType.isApproved;
            _adminBillType.user_Id = adminBillType.user_Id;
            _adminBillType.addedDate = DateTime.Now;
          
            _adminBillType.vendors.Clear();
            if (adminBillType.vendors != null)
            {
                foreach (var _vendor in adminBillType.vendors)
                {
                    if (_adminBillType.vendors.Contains(_vendor) == false)
                    {
                        var vendor = context.Vendors.FirstOrDefault(x => x.Id == _vendor.Id);
                        _adminBillType.vendors.Add(vendor);
                    }
                }
            }

            _adminBillType.ChartofAccounts.Clear();
            if (adminBillType.ChartofAccounts != null)
            {
                foreach (var _coa in adminBillType.ChartofAccounts)
                {
                    if (_adminBillType.ChartofAccounts.Contains(_coa) == false)
                    {
                        var coa = context.ChartofAccounts.FirstOrDefault(x => x.Id == _coa.Id);
                        _adminBillType.ChartofAccounts.Add(coa);
                    }
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Get Payee
        /// </summary>
        /// <param name="payeeId"></param>
        /// <returns></returns>
        public AdminBillType GetAdminBillType(int Id)
        {
            return

                context.adminBillTypes

                .FirstOrDefault(x => x.Id == Id);
        }


        /// <summary>
        /// Get All Payees
        /// </summary>
        /// <returns></returns>
        public List<AdminBillType> GetAllAdminBillTypes()
        {
            
            var billTypes = context.adminBillTypes

            .ToList();
            return billTypes;
        }

        /// <summary>
        /// Get All Payees
        /// </summary>
        /// <returns></returns>
        public List<AdminBillType> GetAllAdminBillTypesForAdminBill()
        {

            var billTypes = context.adminBillTypes

            .ToList();
            return billTypes;
        }

        /// <summary>
        /// Get All Payees
        /// </summary>
        /// <returns></returns>
        public List<AdminBillType> GetAllAdminBillTypesByCOA(ChartofAccount coa)
        {
            List<AdminBillType> typesToReturn = new List<AdminBillType>();
            var billTypes = context.adminBillTypes

                .Where(x=>x.ChartofAccounts.FirstOrDefault(y=>y.Id == coa.Id) != null)
            .ToList();

            return billTypes;
        }








        public void setAdminBillsToVoid(int groupId, bool isVoid)
        {
            List<AdminBill> adminBills = GetBillsByGroupId(groupId);
            
            foreach(AdminBill _bill in adminBills)
            {
                var bill = context.adminBills.FirstOrDefault(x=>x.Id ==_bill.Id);
                bill.isVoid = isVoid;
            }
            context.SaveChanges();
        }


        /// <summary>
        /// Get Count pending Admin Bills by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id)  && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Admin Bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

                return context.adminBills
                  // //.Where(x => x.isApproved == false && x.isVoid != true)
                .Where(x => ((deptIds.Contains(x.department.Id)  && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Get Own Count pending Admin Bills
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            
                return context.adminBills

               .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
               .Count();
               
        }



        /// <summary>
        /// Get Count pending Admin Bills by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

                return context.adminBills

                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
           
        }


        /// <summary>
        /// Get Count pending Admin Bills by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

          
                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
            
        }


        /// <summary>
        /// Get Own Count pending Admin Bills user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            
                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
            
        }

        /// <summary>
        /// Get all Void Admin Bills for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

            
                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isVoid == true)
                .Count();
           
        }


        /// <summary>
        /// Get all Void Admin Bills.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.adminBills
    .Where(x => x.isVoid == true)
    .Count();
        }


        /// <summary>
        /// Get all Admin Bills for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            return context.adminBills
            .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isVoid != true)
            .Count();
           
        }

        /// <summary>
        /// Get all Admin Bills  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Admin Bills.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.adminBills

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Admin Bills
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.adminBills
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending Admin Bills by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

                return context.adminBills
               .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && x.isApproved != false && x.PendingForClosing == true && x.isVoid != true)
               .Count();
          
        }


        /// <summary>
        /// Get Count pending for closing Admin Bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Admin Bills own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

           
                return context.adminBills
                .Where(x => ((deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company_Id)) || x.user_Id == uid) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Admin Bills
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.adminBills

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public AdminBill get(int _billId)
        {
            return context.adminBills.FirstOrDefault(x => x.Id ==_billId);
        }

        public void AddManagementSummary(ManagementSummary managementSummary)
        {
            context.managementSummaries.Add(managementSummary);
            context.SaveChanges();
        }

        public void UpdateManagementSummary(ManagementSummary managementSummary)
        {
            var _managementSummary = context.managementSummaries.FirstOrDefault(x => x.Id == managementSummary.Id);
            _managementSummary.ParentId = managementSummary.ParentId;
            _managementSummary.SummaryName = managementSummary.SummaryName;
            _managementSummary.isActive = managementSummary.isActive;
            context.SaveChanges();
        }

        public ManagementSummary GetManagementSummary(int natureId)
        {
            return context.managementSummaries
               
                .FirstOrDefault(x => x.Id == natureId);
        }

        public List<ManagementSummary> GetAllManagementSummary()
        {
            return context.managementSummaries
                
                .ToList();
        }



        public void AddAdminBillNature(AdminBillNature billNature)
        {
            context.adminBillNatures.Add(billNature);
            context.SaveChanges();
        }

        public void UpdateAdminBillNature(AdminBillNature billNature)
        {
            AdminBillNature _billNature = context.adminBillNatures.FirstOrDefault(x => x.Id == billNature.Id);
            _billNature = billNature;
            context.SaveChanges();
        }

        public AdminBillNature GetAdminBillNature(int natureId)
        {
            return context.adminBillNatures
           
                .FirstOrDefault(x => x.Id == natureId);
        }

        public List<AdminBillNature> GetAllAdminBillNature()
        {
            return context.adminBillNatures
          
                .ToList();
        }

        public List<AdminBillNature> GetAllAdminBillNatureByComp(int compId)
        {
            return context.adminBillNatures
            
                .Where(x=>x.Companies.FirstOrDefault(y=>y.Id == compId) != null)
                .ToList();
        }

        /// <summary>
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompanies(int id)
        {
            var user = context.Users
               
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }

        /// <summary>
        /// Add Rented Vehicle Owner
        /// </summary>
        /// <param name="vehicleOwner"></param>
        public void AddRentedVehicleOwner(RentedVehicleOwner vehicleOwner)
        {
            context.rentedVehicleOwners.Add(vehicleOwner);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Rented Vehicle Owner
        /// </summary>
        /// <param name="vehicleOwner"></param>
        public void UpdateRentedVehicleOwner(RentedVehicleOwner vehicleOwner)
        {
            var _vehicleOwner = context.rentedVehicleOwners.FirstOrDefault(x => x.Id == vehicleOwner.Id);
            _vehicleOwner = vehicleOwner;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Rented Vehicle Owner
        /// </summary>
        /// <param name="natureId"></param>
        /// <returns></returns>
        public RentedVehicleOwner GetRentedVehicleOwner(int natureId)
        {
            return context.rentedVehicleOwners

                .FirstOrDefault(x => x.Id == natureId);
        }

        /// <summary>
        /// Get All Rented Vehicle Owner
        /// </summary>
        /// <returns></returns>
        public List<RentedVehicleOwner> GetAllRentedVehicleOwner()
        {
            return context.rentedVehicleOwners

                .ToList();
        }

        /// <summary>
        /// Add Maintenance Head
        /// </summary>
        /// <param name="maintenanceHead"></param>
        public void AddMaintenanceHead(MaintenanceHead maintenanceHead)
        {
            context.maintenanceHeads.Add(maintenanceHead);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Maintenance Head
        /// </summary>
        /// <param name="maintenanceHead"></param>
        public void UpdateMaintenanceHead(MaintenanceHead maintenanceHead)
        {
            var _maintenanceHead = context.maintenanceHeads.FirstOrDefault(x => x.Id == maintenanceHead.Id);
            _maintenanceHead = maintenanceHead;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Maintenance Head
        /// </summary>
        /// <param name="maintenanceHeadId"></param>
        /// <returns></returns>
        public MaintenanceHead GetMaintenanceHead(int maintenanceHeadId)
        {
            return context.maintenanceHeads

                .FirstOrDefault(x => x.Id == maintenanceHeadId);
        }

        /// <summary>
        /// Get All Maintenance Head
        /// </summary>
        /// <returns></returns>
        public List<MaintenanceHead> GetAllMaintenanceHead()
        {
            return context.maintenanceHeads
                .ToList();
        }
        public List<AdminBill> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.adminBills
                .Where(x => deptIds.Contains((int)x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.isApproved == true
                && x.isVoid != true
                && x.BillStatus.isActive == true
                ).ToList();
        }
    }
}
