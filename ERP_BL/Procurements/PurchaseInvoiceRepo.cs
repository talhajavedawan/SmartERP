using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IPurchaseInvoiceRepo
    {
        void Add(PurchaseInvoice purchaseInvoice);

        void Add(PurchaseInvoice purchaseInvoice, ProcurementProduct product);
        void addStatus(PurchaseInvoiceStatus status);
        void update(PurchaseInvoice purchaseInvoice);
        void update(PurchaseInvoice purchaseInvoice, ERP_BL.Databases.Employee employee);
        void updateStatus(PurchaseInvoice purchaseInvoice, PurchaseInvoiceStatus status);
        void updateStatus(int purchaseInvoiceId, PurchaseInvoiceStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int purchaseOrderItemId, ItemType itemType);


        List<PurchaseInvoice> getAll();
        List<PurchaseInvoice> getAll(User user);
        List<PurchaseInvoice> getAll(ERP_BL.Databases.Employee employee);

        //List<ItemType> getAllItemTypes();
        PurchaseInvoice get(int purchaseOrderId);
        List<PurchaseInvoiceStatus> getAllPurchaseInvoiceStatus();
        List<PurchaseInvoiceStatus> getAllActivePurchaseInvoiceStatus();
        PurchaseInvoiceStatus getstatus(int statusId);
        int getAllPendingForApprovalDepartmentalCount(int uid);
        int getAllPendingForApprovalCount(int uid);
        int getAllPendingForApprovalCountOwn(int uid);
        int getAllPendingForClosingDepartmentalCount(int uid);
        int getAllPendingForClosingCountOwn(int uid);
        int getAllPendingForClosingCount(int uid);
        int getPurchaseInvoiceRegisterCount(int uid);
        int getInvoiceRegisterCountOWn();
        int getAllPendingForClosingAdministratorCount();
        int getVoidRegisterCount(int uid);
        int getVoidRegisterAdministratorCount();
    }
    public class PurchaseInvoiceRepo : IPurchaseInvoiceRepo
    {
        DBContextERP context = new DBContextERP();

        public void Add(PurchaseInvoice purchaseInvoice)
        {
            context.purchaseInvoices.Add(purchaseInvoice);
            context.SaveChanges();
            //context.SaveChanges();
        }

        public void Add(PurchaseInvoice purchaseInvoice, ProcurementProduct product)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);
            purchaseInvoicetoUpdate.products.Add(product);
            context.SaveChanges();
        }

        public void addStatus(PurchaseInvoiceStatus status)
        {
            context.purchaseInvoiceStatuses.Add(status);
            context.SaveChanges();
        }
        public void update(PurchaseInvoice purchaseInvoice)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);
            context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.PurchaseInvoiceId == purchaseInvoice.Id));
            context.inventories.RemoveRange(context.inventories.Where(x => x.PurchaseInvoiceId == purchaseInvoice.Id));
            context.VATBooks.RemoveRange(context.VATBooks.Where(x => x.purchaseInvoiceId == purchaseInvoice.Id));

            purchaseInvoicetoUpdate = purchaseInvoice;
            context.SaveChanges();
        }
        public void updatePI(PurchaseInvoice purchaseInvoice)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);
            purchaseInvoicetoUpdate.transactionHolderId = purchaseInvoice.transactionHolderId;
            purchaseInvoicetoUpdate.holderChangeDate = purchaseInvoice.holderChangeDate;
            context.SaveChanges();
        }
        public void updateForDirectClose(PurchaseInvoice purchaseInvoice)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);
            //context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.PurchaseInvoiceId == purchaseInvoice.Id));
            purchaseInvoicetoUpdate = purchaseInvoice;
            context.SaveChanges();
        }
        public void update(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);

            purchaseOrdertoUpdate = purchaseOrder;
            context.SaveChanges();
        }

        public void update(PurchaseInvoice purchaseInvoice, Employee employee)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);

            purchaseInvoicetoUpdate.employee = employee;
            context.SaveChanges();
        }

        public void updateStatus(PurchaseInvoiceStatus status)
        {
            PurchaseInvoiceStatus piStatus = context.purchaseInvoiceStatuses.FirstOrDefault(x => x.Id == status.Id);
            piStatus = status;
            context.SaveChanges();
        }

        public void updateStatus(PurchaseInvoice purchaseInvoice, PurchaseInvoiceStatus status)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoice.Id);
            purchaseInvoicetoUpdate.PurchaseInvoiceStatus = status;
            context.SaveChanges();
        }

        
        public List<PurchaseInvoice> GetPurchaseInvoicesByPoId(int PId)
        {
            return context.purchaseInvoices
                .Where(x=>x.purchaseOrder_Id == PId && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get All Bills of Company & Inter-company, Departments & Inter-Department and Vendor
        /// </summary>
        /// <returns></returns>
        public List<PurchaseInvoice> GetAllBillsByCompDeptVendorInterComp(int compId, List<Department> departments, int interCompId, int interDeptId, int vendorId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var purchaseInvoices = context.purchaseInvoices

           .Where(x => ((x.company_Id == compId && dept_Ids.Contains(x.dept_Id)) || (x.InterCompany_Id == interCompId && x.InterDepartment_Id == interDeptId && x.isInterCompany == true)) && x.currency_Id == currId && x.vendor_Id == vendorId && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();


            //List<PurchaseInvoice> billsToReturn = new List<PurchaseInvoice>();
            //billsToReturn = bills;
            //foreach (var _bill in bills)
            //{
            //    if (dept_Ids.Contains((int)_bill.dept_Id))
            //    {
            //        billsToReturn.Add(_bill);
            //    }
            //}

            return purchaseInvoices;
        }

        /// <summary>
        /// Get All Bills of Company and Departments 
        /// </summary>
        /// <returns></returns>
        public List<PurchaseInvoice> getBillsByCompanyDeptInterComp(int compId, List<Department> departments, int interCompId, int interDeptId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var purchaseInvoices = context.purchaseInvoices
               
           .Where(x => ((x.company_Id == compId && dept_Ids.Contains(x.dept_Id)) || (x.InterCompany_Id == interCompId && x.InterDepartment_Id == interDeptId && x.isInterCompany == true)) && x.currency_Id == currId && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            //List<PurchaseInvoice> billsToReturn = new List<PurchaseInvoice>();
            //billsToReturn = bills;
            //foreach (var _bill in bills)
            //{
            //    if (dept_Ids.Contains((int)_bill.dept_Id))
            //    {
            //        billsToReturn.Add(_bill);
            //    }
            //}

            return purchaseInvoices;
        }


        /// <summary>
        /// Get Purchase Invoices by Company, Dept, Vendor and Currency
        /// </summary>
        /// <returns></returns>
        public List<PurchaseInvoice> GetAllPIbyCompDeptVendor(int compId, List<Department> departments, int vendorId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var purchaseInvoices = context.purchaseInvoices
      
           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<PurchaseInvoice> PinvoicesToReturn = new List<PurchaseInvoice>();
            foreach (var _PI in purchaseInvoices)
            {
                if (dept_Ids.Contains((int)_PI.dept_Id) && _PI.vendors.FirstOrDefault(x=>x.Id == vendorId) != null)
                {
                    PinvoicesToReturn.Add(_PI);
                }
            }

            return PinvoicesToReturn;
        }

        /// <summary>
        /// Get Purchase Invoices by Company, Dept and Currency
        /// </summary>
        /// <returns></returns>
        public List<PurchaseInvoice> getPIbyCompanyDept(int compId, List<Department> departments, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var purchaseInvoices = context.purchaseInvoices
    
           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<PurchaseInvoice> PinvoicesToReturn = new List<PurchaseInvoice>();
            foreach (var _PI in purchaseInvoices)
            {
                if (dept_Ids.Contains((int)_PI.dept_Id))
                {
                    PinvoicesToReturn.Add(_PI);
                }
            }

            return PinvoicesToReturn;
        }




        public PurchaseInvoice get(int purchaseOrderId)
        {
            return context.purchaseInvoices


            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public PurchaseInvoice getForPayments(int purchaseOrderId)
        {
            return context.purchaseInvoices
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public PurchaseInvoice getForGrid(int purchaseOrderId)
        {
            return context.purchaseInvoices
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public List<PurchaseInvoice> getAll()
        {
            try
            {
                return context.purchaseInvoices
                .ToList();
            }
            catch
            { return null; }
        }

        public List<PurchaseInvoice> getAll(User user)
        {
            return context.purchaseInvoices.Where(x => x.user_Id == user.id).ToList();
        }

        public List<PurchaseInvoice> getAll(Employee employee)
        {
            return context.purchaseInvoices.Where(x => x.allocation_Id == employee.EmpId).ToList();
        }
        public void updateStatus(int purchaseInvoiceId, PurchaseInvoiceStatus status)
        {
            PurchaseInvoice purchaseInvoicetoUpdate = context.purchaseInvoices.FirstOrDefault(x => x.Id == purchaseInvoiceId);
            purchaseInvoicetoUpdate.PurchaseInvoiceStatus = status;
            context.SaveChanges();
        }
        public List<PurchaseInvoiceStatus> getAllPurchaseInvoiceStatus()
        {
            return context.purchaseInvoiceStatuses.ToList();
        }
        public List<PurchaseInvoiceStatus> getAllActivePurchaseInvoiceStatus()
        {
            List<PurchaseInvoiceStatus> statuses = new List<PurchaseInvoiceStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses =_DbContext.purchaseInvoiceStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        public PurchaseInvoiceStatus getstatus(int purchaseOrderstatusid)
        {
            return context.purchaseInvoiceStatuses.FirstOrDefault(x => x.Id == purchaseOrderstatusid);
        }
        public Vendor getVendor(int vendorsCompID)
        {
            return context.Vendors
                .FirstOrDefault(x => x.Id == vendorsCompID);
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
            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
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

            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
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

            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForAdministratorCount()
        {
            return context.purchaseInvoices

                .Where(x => x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
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

            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
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

            return context.purchaseInvoices


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getPurchaseInvoiceRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).Count();
        }
        public int getInvoiceRegisterCountOWn()
        {
            return context.purchaseInvoices

        .Where(x => x.isVoid != true).Count();
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.purchaseInvoices

                .Where(x => x.isApproved == true && x.PendingForClosing == true)
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
            return context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .Count();
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.purchaseInvoices
    .Where(x => x.isVoid == true)
    .Count();
        }
        public List<PurchaseInvoice> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices    
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
            

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
        
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
     
        public List<PurchaseInvoice> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForAdministrator(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
         
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
      
        public List<PurchaseInvoice> getAllPendingForAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
      
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingAdministrator()
        {
            return context.purchaseInvoices
                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<PurchaseInvoice> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllPobyStatusIdFirst(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoice> getAllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.PurchaseInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }
        public List<PurchaseInvoiceStatus> getAllInActivePurchaseInvoiceStatus()
        {
            List<PurchaseInvoiceStatus> statuses = new List<PurchaseInvoiceStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses =_DbContext.purchaseInvoiceStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }
        public List<PurchaseInvoice> getSaleInvoiceRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseInvoices
        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getSaleInvoiceRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
        .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getSaleInvoiceRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.purchaseInvoices
        .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getInvoiceRegisterAdministrator()
        {
            return context.purchaseInvoices
        .Where(x => x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getInvoiceRegisterAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseInvoices
        .Where(x => x.CreationDate >= previousMonthDate && x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getInvoiceRegisterAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseInvoices
        .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isVoid != true).ToList();
        }
        public List<PurchaseInvoice> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseInvoices
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .ToList();
        }
        public List<PurchaseInvoice> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.purchaseInvoices
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }
        public List<PurchaseInvoice> getVoidRegisterAdministrator()
        {
            return context.purchaseInvoices
        .Where(x => x.isVoid == true)
    .ToList();
        }
        public int getpurchaseInvoicesCountByStatusId(int uid, int statusId)
        {
            using (var _DbContext = new DBContextERP())
            {
                if (uid != 0)
                {
                    var user = _DbContext.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    foreach (var comp in user.employee.Companies)
                        companyIds.Add(comp.Id);
                    foreach (var dpt in user.employee.departments)
                        deptIds.Add(dpt.Id);

                    return _DbContext.purchaseInvoices


                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.PurchaseInvoiceStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.purchaseInvoices.Where(x => x.PurchaseInvoiceStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        public void setPurchaseInvoicetoVoid(int Id, bool isVoid)
        {
            PurchaseInvoice item = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            if (item.PurchaseInvoicetype == InquiryType.Inventory)
            {
                var inventories = context.inventories.Where(x => x.PurchaseInvoiceId == Id).ToList();
                context.inventories.RemoveRange(inventories);
            }
            var journalTransactions = context.journalTransactions.Where(x => x.PurchaseInvoiceId == Id).ToList();
            context.journalTransactions.RemoveRange(journalTransactions);
           
            context.SaveChanges();
        }
        public List <PurchaseInvoice> getbyPOID(int purchaseOrderId)
        {
            return context.purchaseInvoices
            .Where(x => x.purchaseOrder_Id == purchaseOrderId && x.isVoid!=true).ToList();
        }



        public int getAllActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseInvoices

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
        }

    }


}
