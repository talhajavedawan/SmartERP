using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Budget
{
    public class BudgetCostCenterRepo
    {

        DBContextERP context = new DBContextERP();

        public void Add(BudgetCostSheet budgetCostSheet)
        {
            context.budgetCostSheets.Add(budgetCostSheet);
            context.SaveChanges();
        }
        public void update(BudgetCostSheet budget)
        {
            BudgetCostSheet budgettoUpdate = context.budgetCostSheets.FirstOrDefault(x => x.Id == budget.Id);
            budgettoUpdate = budget;
            context.SaveChanges();
        }
        public void AddBudgetCostHead(BudgetSheetHead head)
        {
            context.budgetCostHeads.Add(head);
            context.SaveChanges();
        }
        public List<BudgetCostSheetStatus> getAllActiveBudgetCostStatus()
        {
            List<BudgetCostSheetStatus> statuses = new List<BudgetCostSheetStatus>();
            using (var _DbContext = new DBContextERP())
            {
                //return context.saleOrderStatuses.Include("SaleOrders").Where(x => x.isActive == true).ToList();
                statuses = _DbContext.budgetCostSheetStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        public List<BudgetCostSheetStatus> getAllBudgetCostStatus()
        {
            return context.budgetCostSheetStatuses.ToList();
        }
        public BudgetCostSheetStatus getstatus(int statusid)
        {
            return context.budgetCostSheetStatuses.FirstOrDefault(x => x.Id == statusid);
        }
        public void addStatus(BudgetCostSheetStatus status)
        {
            context.budgetCostSheetStatuses.Add(status);
            context.SaveChanges();
        }
        public void updateStatus(BudgetCostSheetStatus status)
        {
            BudgetCostSheetStatus Status = context.budgetCostSheetStatuses.FirstOrDefault(x => x.Id == status.Id);
            Status = status;
            context.SaveChanges();
        }
        public void updateStatusById(int budgetId, int statusid)
        {
            BudgetCostSheet budgettoUpdate = context.budgetCostSheets.FirstOrDefault(x => x.Id == budgetId);
            var SoStatus = context.budgetCostSheetStatuses.FirstOrDefault(x => x.Id == statusid);
            budgettoUpdate.budgetCostSheetStatus = SoStatus;
            context.SaveChanges();
        }
        public BudgetCostSheet get(int budgetId)
        {
            return context.budgetCostSheets

            .FirstOrDefault(x => x.Id == budgetId);
        }
        public List<BudgetSheetHead> getallCostSheetField()
        {
            return context.budgetCostHeads
                .OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Get All Active CostSheetField
        /// </summary>
        /// <returns></returns>
        public List<BudgetSheetHead> getActiveCostSheetFields()
        {
            return context.budgetCostHeads.Where(x => x.isActive == true).OrderBy(y => y.SortId).ToList();


        }
        public void AddCostSheetField(BudgetSheetHead costSheetField)
        {
            context.budgetCostHeads.Add(costSheetField);
            context.SaveChanges();
        }
        public BudgetSheetHead getCostSheetField(int costSheetFieldId)
        {
            return context.budgetCostHeads
                .FirstOrDefault(x => x.Id == costSheetFieldId);
        }
        public void UpdateCostSheetField(BudgetSheetHead costSheetField)
        {
            BudgetSheetHead prod = context.budgetCostHeads.FirstOrDefault(x => x.Id == costSheetField.Id);
            prod = costSheetField;
            context.SaveChanges();
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

            return context.budgetCostSheets

                .Where(x => deptIds.Contains((int)x.deptId)
                && companyIds.Contains((int)x.companyId) 
                && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets

                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                 && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets

                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)  
                && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) && 
                

                 x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                && 
                 x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                && x.isVoid == true)
                .Count();
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.budgetCostSheets
            .Where(x => x.isVoid == true)
            .Count();
        }
        public int getBudgetRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)
                &&  x.isVoid != true)
                .Count();
        }
        public int getBudgetRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                 && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForAdministratorCount()
        {
            return context.budgetCostSheets

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getBudgetRegisterAdministratorCount()
        {
            return context.budgetCostSheets
                .Where(x => x.isVoid != true)
                .Count();
        }
        public List<BudgetCostSheet> getAll()
        {
            return context.budgetCostSheets
            .Where(x => x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getFirstAll(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate 
                && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate &&
                deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)
                && x.CreationDate >= previousMonthDate
                && x.budgetCostSheetStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets
                .Where(x => x.CreationDate >= previousMonthDate &&
                deptIds.Contains((int)x.deptId) &&
                companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate &&
                x.budgetCostSheetStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate &&
                deptIds.Contains((int)x.deptId) &&
                companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate && 
                 x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains((int)x.deptId)
                && companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate
                && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate && 
                deptIds.Contains((int)x.deptId) &&
                companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate 
                && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForAdministrator()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

            .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false && x.isVoid != true)
            .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate 
                && deptIds.Contains((int)x.deptId)
                && companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate 
            
                 && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.CreationDate >= previousMonthDate 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets


                .Where(x => x.CreationDate >= previousMonthDate 
                && deptIds.Contains((int)x.deptId)
                && companyIds.Contains((int)x.companyId)
                && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForClosingAdministrator()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= previousMonthDate
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId)
                && x.budgetCostSheetStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) &&
                x.isApproved == true && 
                x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) &&
                x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                && x.isApproved == true 
                && x.PendingForClosing == true
                && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.budgetCostSheets

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public List<BudgetCostSheet> getAllInActiveandUnapproved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)
                && x.budgetCostSheetStatus.isActive == false 
                && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> GetAllOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

            return context.budgetCostSheets

          .Where(x => deptIds.Contains((int)x.deptId) 
          && companyIds.Contains((int)x.companyId) 
          && x.isApproved == true && x.isReApproved != false 
          && x.PendingForClosing != true && x.isVoid != true)
          .ToList();
        }
        public List<BudgetCostSheet> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
             .Where(x => deptIds.Contains((int)x.deptId) 
              && companyIds.Contains((int)x.companyId) &&
              x.budgetCostSheetStatus.isActive == true && 
              x.isApproved == true 
              && x.isReApproved != false 
              && x.PendingForClosing != true 
              && x.isVoid != true)
             .ToList();
        }
        public List<BudgetCostSheet> ActiveInActiveSoByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets

                   .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains((int)x.deptId) 
                   && companyIds.Contains((int)x.companyId) 
                   && x.isApproved == true && x.isVoid != true)
                  .ToList();
        }
        public List<BudgetCostSheet> AllActivebyDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                                .Where(x => x.CreationDate >= from && x.CreationDate <= to && 
                                deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
                                &&
                                x.CreationDate >= from && x.CreationDate <= to
                                && x.budgetCostSheetStatus.isActive == true 
                                && x.isApproved == true && x.isVoid != true)

                                .ToList();
        }
        public List<BudgetCostSheet> AllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);


            return context.budgetCostSheets

             .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains((int)x.deptId)
             && companyIds.Contains((int)x.companyId) 
             && x.budgetCostSheetStatus.isActive == false 
             && x.isApproved == true && x.isVoid != true)
             .ToList();
        }
        public List<BudgetCostSheet> AllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= from & x.CreationDate <= to
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> AllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                            .Where(x => x.CreationDate >= from && x.CreationDate <= to
                            && deptIds.Contains((int)x.deptId)
                            && companyIds.Contains((int)x.companyId) &&
                            x.CreationDate >= from && x.CreationDate <= to
                            && x.isApproved == false && x.isVoid != true)
                            .ToList();

        }
        public List<BudgetCostSheet> AllPendingForApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets
                .Where(x => x.CreationDate >= from && x.CreationDate <= to
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> AllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= from && x.CreationDate <= to
                && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> PendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets
                .Where(x => x.CreationDate >= from && x.CreationDate <= to 
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isApproved == true 
                && x.PendingForClosing == true 
                && x.isVoid != true)
                .ToList();

        }
        public List<BudgetCostSheet> AllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.budgetCostSheets
                .Where(x => x.CreationDate >= from 
                && x.CreationDate <= to 
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> AllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets

            .Where(x => x.CreationDate >= from 
            && x.CreationDate <= to 
            && deptIds.Contains((int)x.deptId) 
            && companyIds.Contains((int)x.companyId) 
            && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }
        public List<BudgetCostSheet> AllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.budgetCostSheets

                .Where(x => x.CreationDate >= from && x.CreationDate <= to
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }
        public List<BudgetCostSheet> PendingForReApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
            .Where(x => x.CreationDate >= from && x.CreationDate <= to 
            && deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) 
            && x.isReApproved == false 
            && x.isApproved == true 
            && x.isVoid != true)
            .ToList();

        }
        public List<BudgetCostSheet> PendingForReApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
            .Where(x => x.CreationDate >= from && x.CreationDate <= to 
            && deptIds.Contains((int)x.deptId) 
            && companyIds.Contains((int)x.companyId)        
            && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
            .ToList();

        }
        public List<BudgetCostSheet> PendingForReApprovalOwnByDateRamge(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                            .Where(x => x.CreationDate >= from && x.CreationDate <= to
                            && deptIds.Contains((int)x.deptId) 
                            && companyIds.Contains((int)x.companyId) 
                            && x.isReApproved == false
                            && x.isVoid != true 
                            && x.isApproved == true)
                            .ToList();

        }
        public List<BudgetCostSheet> BudgetRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
        .OrderByDescending(x => x.CreationDate)
            .Where(x => x.CreationDate >= from && x.CreationDate <= to
            && deptIds.Contains((int)x.deptId) 
            && companyIds.Contains((int)x.companyId)
            && x.isVoid != true)
            .ToList();


        }
        public List<BudgetCostSheet> AllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.budgetCostSheets

                            .Where(x => x.CreationDate >= from && x.CreationDate <= to
                            && deptIds.Contains((int)x.deptId)
                            && companyIds.Contains((int)x.companyId) 
                            && x.budgetCostSheetStatus.Id == StatusId
                            && x.isApproved == true && x.isVoid != true)
                            .ToList();

        }
        public List<BudgetCostSheet> getFirstSaleRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month 
                && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 
                && x.CreationDate.Value.Year == DateTime.Today.Year 
                && x.CreationDate.Value.Month <= DateTime.Today.Month 
                && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 
                && x.CreationDate.Value.Year == DateTime.Today.Year 
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                 && x.isVoid != true)

                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month 
                && x.CreationDate.Value.Month >= DateTime.Today.Month - 2
                && x.CreationDate.Value.Year == DateTime.Today.Year
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isReApproved == false 
                && x.isApproved == true
                && x.isVoid != true)
                .ToList();
        }
        public List<BudgetCostSheet> getAllFirstPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month 
                && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 
                && x.CreationDate.Value.Year == DateTime.Today.Year 
                && deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)                
                && x.isReApproved == false
                && x.isVoid != true 
                && x.isApproved == true)
                .ToList();

        }
        public List<BudgetCostSheet> getAllFirstPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month 
                && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 
                && x.CreationDate.Value.Year == DateTime.Today.Year 
                && deptIds.Contains((int)x.deptId) 
                && companyIds.Contains((int)x.companyId) 
                && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public void Approve(BudgetCostSheet budget)
        {
            BudgetCostSheet budgettoUpdate = context.budgetCostSheets.FirstOrDefault(x => x.Id == budget.Id);
            budgettoUpdate.isApproved = budget.isApproved;
            budgettoUpdate.stage = budget.stage;
            context.SaveChanges();
        }
        public List<BudgetCostSheet> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.budgetCostSheets
                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId)  && x.isVoid == true).ToList();
        }
        public List<BudgetCostSheet> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.budgetCostSheets

                .Where(x => deptIds.Contains((int)x.deptId) && companyIds.Contains((int)x.companyId) && x.isVoid == true )
                .ToList();
        }
        public List<BudgetCostSheet> getVoidRegisterAdministrator()
        {
            return context.budgetCostSheets.Where(x => x.isVoid == true)
                .ToList();
        }
        public List<BudgetCostSheet> getByCompanyDepartment(int compId, int deptId)
        {
            return context.budgetCostSheets.Where(x => x.deptId == deptId && x.companyId == compId).ToList();
        }
        public BudgetSystemCostField GetSystemBudget(int _id)
        {
            return context.budgetSystemCostFields.FirstOrDefault(x => x.Id == _id);
        }
     
        public List<SaleOrder> getBudgetSaleOrders(int budgetId)
        {
            return context.saleOrders.Where(x => x.Budget_Id == budgetId && x.isVoid != true).ToList();
        }
        public List<PurchaseOrder> getBudgetPurchaseOrders(int budgetId)
        {
            return context.purchaseOrders.Where(x => x.Budget_Id == budgetId && x.isVoid != true).ToList();
        }


        public PerformanceSheetHead getPerformanceSheetHead(int costSheetFieldId)
        {
            return context.performanceSheetHeads
                .FirstOrDefault(x => x.Id == costSheetFieldId);
        }
        public void AddPerformanceSheetField(PerformanceSheetHead costSheetField)
        {
            context.performanceSheetHeads.Add(costSheetField);
            context.SaveChanges();
        }
        public void UpdatePerformanceSheetField(PerformanceSheetHead costSheetField)
        {
            PerformanceSheetHead prod = context.performanceSheetHeads.FirstOrDefault(x => x.Id == costSheetField.Id);
            prod = costSheetField;
            context.SaveChanges();
        }
        public List<PerformanceSheetHead> getallPerformanceSheetFields()
        {
            return context.performanceSheetHeads
                .OrderBy(y => y.SortId).ToList();

        }
        public List<PerformanceSheetHead> getActivePerformanceSheetHeads()
        {
            return context.performanceSheetHeads.Where(x => x.isActive == true).OrderBy(y => y.SortId).ToList();


        }
    
    }

}
