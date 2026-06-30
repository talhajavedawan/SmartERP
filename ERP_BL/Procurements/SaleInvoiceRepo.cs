using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface ISaleInvoiceRepo
    {
        void Add(SaleInvoice purchaseOrder);
        void Add(SaleInvoice purchaseOrder, ProcurementProduct product);
        void addStatus(SaleInvoiceStatus status);
        void update(SaleInvoice purchaseOrder);
        void update(SaleInvoice purchaseOrder, Employee employee);
        void updateStatus(SaleInvoice purchaseOrder, SaleInvoiceStatus status);
        void updateStatus(int purchaseOrderId, SaleInvoiceStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int purchaseOrderItemId, ItemType itemType);


        List<SaleInvoice> getAll();
        List<SaleInvoice> getAll(User user);
        List<SaleInvoice> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        SaleInvoice get(int purchaseOrderId);
    }
    public class SaleInvoiceRepo : ISaleInvoiceRepo
    {


        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add SaleInvoice in database
        /// </summary>
        /// <param name="purchaseOrder">SaleInvoice Object</param>
        public void Add(SaleInvoice purchaseOrder)
        {
            context.saleInvoices.Add(purchaseOrder);
            context.SaveChanges();
        }
        ///// <summary>
        ///// Add ItemType in database
        ///// </summary>
        ///// <param name="itemType">ItemType Object</param>
        //public void additemtype(ItemType itemType)
        //{
        //    context.itemTypes.Add(itemType);
        //    context.SaveChanges();
        //} 
        /// <summary>
        /// Add item to specified purchaseOrder
        /// </summary>
        /// <param name="purchaseOrder">SaleInvoice Object (main object in which Item is to be added)</param>
        /// <param name="purchaseOrderItem">SaleInvoiceItem Object (item to be added)</param>
        public void Add(SaleInvoice purchaseOrder, ProcurementProduct product)
        {
            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.products.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// get vendorsCompany matching to ID
        /// </summary>
        /// <param name="vendorsCompID">Vendor Company ID</param>
        /// <returns></returns>
        public Vendor getVendor(int vendorsCompID)
        {
            return context.Vendors
              
                .FirstOrDefault(x => x.Id == vendorsCompID);
        }
        /// <summary>
        /// Add SaleInvoiceStatus in database
        /// </summary>
        /// <param name="status">SaleInvoiceStatus Object</param>
        public void addStatus(SaleInvoiceStatus status)
        {
            context.saleInvoiceStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update SaleInvoice
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void update(SaleInvoice purchaseOrder)
        {
            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrder.Id);
             context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.SaleInvoiceId == purchaseOrder.Id));
             context.inventories.RemoveRange(context.inventories.Where(x => x.SaleInviceId == purchaseOrder.Id));
             context.VATBooks.RemoveRange(context.VATBooks.Where(x => x.saleInvoiceId == purchaseOrder.Id));
            //foreach(var prod in purchaseOrdertoUpdate.products)
            //{
            //    if (prod.Id != 0)
            //    {
            //        context.inquiryProducts.Remove(context.inquiryProducts.FirstOrDefault(x => x.Id == prod.product_Id));
            //        context.procurementProducts.Remove(context.procurementProducts.FirstOrDefault(x => x.Id == prod.Id));
            //    }
            //}
            
            purchaseOrdertoUpdate = purchaseOrder;
            context.SaveChanges();
        }
        public void updateInvoice(SaleInvoice invoice)
        {
            SaleInvoice dbInvoice = context.saleInvoices.FirstOrDefault(x => x.Id == invoice.Id);
            dbInvoice.transactionHolderId = invoice.transactionHolderId;
            dbInvoice.holderChangeDate = invoice.holderChangeDate;
            context.SaveChanges();
        }
        public void updateForDirectClose(SaleInvoice purchaseOrder)
        {

            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            //context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.SaleInvoiceId == purchaseOrder.Id));
            purchaseOrdertoUpdate = purchaseOrder;
            context.SaveChanges();
        }

        /// <summary>
        /// Update SaleInvoice and allocate it to new employee
        /// </summary>
        /// <param name="purchaseOrder">SaleInvoice to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(SaleInvoice purchaseOrder, Employee employee)
        {
            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrder.Id);

            purchaseOrdertoUpdate.employee = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(SaleInvoiceStatus status)
        {
            SaleInvoiceStatus poStatus = context.saleInvoiceStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="status"></param>
        public void updateStatus(SaleInvoice purchaseOrder, SaleInvoiceStatus status)
        {
            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.saleInvoiceStatus = status;
            context.SaveChanges();
        }


        /// <summary>
        /// Change purchaseOrder status based on SaleInvoice ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="status"></param>
        public void updateStatus(int purchaseOrderId, SaleInvoiceStatus status)
        {
            SaleInvoice purchaseOrdertoUpdate = context.saleInvoices.FirstOrDefault(x => x.Id == purchaseOrderId);
            purchaseOrdertoUpdate.saleInvoiceStatus = status;
            context.SaveChanges();
        }



        /// <summary>
        /// Get all Active saleInvoices.
        /// </summary>
        /// <returns></returns>
        public int getAllActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices


            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Get all Inactive saleInvoices.
        /// </summary>
        /// <returns></returns>
        public int getAllInActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices


            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Change SaleInvoice to Void
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isVoid"></param>
        public void setSaleInvoicetoVoid(int Id, bool isVoid)
        {
            SaleInvoice item = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            if (item.saleInvoicetype == InquiryType.Inventory)
            {
                var inventories = context.inventories.Where(x => x.SaleInviceId == Id).ToList();
                context.inventories.RemoveRange(inventories);
            }
            var journalTransactions = context.journalTransactions.Where(x => x.SaleInvoiceId == Id).ToList();
            context.journalTransactions.RemoveRange(journalTransactions);
            context.SaveChanges();
        }

        /// <summary>
        /// Get all saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }

        public List<SaleInvoice> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
                .Where(x =>  x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }

        public List<SaleInvoice> getAllByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.isApproved == true))
                .ToList();
        }
        /// <summary>
        /// Get all Inactive saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllInActive(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
             
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }
        public List<SaleInvoice> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
           
                .Where(x => x.CreationDate >= previousMonthDate  && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }

        public List<SaleInvoice> getAllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
      
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true))
                .ToList();
        }

        /// <summary>
        /// Get all Active saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
           
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }
        public List<SaleInvoice> getAllActiveFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
          
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }
        public List<SaleInvoice> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
         
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true))
                .ToList();
        }

        /// <summary>
        /// Get all Inactive saleInvoices except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllInActiveandUnapproved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
              
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get all Active saleInvoices except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllActiveandUnapporved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
               
             
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.PendingForClosing != true)
                .ToList();
        }
        /// <summary>
        /// Get all saleInvoices except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllandUnapporved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
              
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }
        /// <summary>
        /// Get all saleInvoices except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllandUnapporvedAdministrator()
        {
            return context.saleInvoices
   
    .Where(x => x.PendingForClosing != true)
    .ToList();
        }
        /// <summary>
        /// Get all saleInvoices except pending for closing count. 
        /// </summary>
        /// <returns></returns>
        public int getAllandUnapporvedCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
               
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && x.PendingForClosing != true)
                .Count();
        }
        /// <summary>
        /// Get all saleInvoices except pending for closing own.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllandUnapporvedOwn(int uid)
        {
            
                
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
              
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing != true)
                .ToList();
        }

        /// <summary>
        /// Get all saleInvoices except pending for closing own count. 
        /// </summary>
        /// <returns></returns>
        public int getAllandUnapporvedCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing != true)
                .Count();
        }
        /// <summary>
        /// Get all pending saleInvoices by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
                
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
              
                .Where(x => x.CreationDate >= previousMonthDate &&  deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
              
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }

        /// <summary>
        /// Get Count pending saleInvoices by Departments
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
            return context.saleInvoices

                
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get all pending saleInvoices by Departmental
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
          
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
               
                .Where(x => x.CreationDate >= previousMonthDate  && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
                
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

        /// <summary>
        /// Get all pending saleInvoices by Departmental Count
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

            return context.saleInvoices

                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all pending saleInvoices by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
            

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
              
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
                

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }

        /// <summary>
        /// Get Own pending saleInvoices by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
               

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
     
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate &&(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
        
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }

        /// <summary>
        /// Get Count pending saleInvoices by supervisor Id and <paramref name="StatusId"/>.
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

            return context.saleInvoices

               
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending saleInvoices
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

            return context.saleInvoices

                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }

        //#Partially invoices
        /// <summary>
        /// Get all Partiall saleInvoices for user.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getFullyInvoiced(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
             
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount - x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount) == 0))
                .ToList();
        }
        public List<SaleInvoice> getFullyInvoicedFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
               
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount - x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount) == 0))
                .ToList();
        }
        public List<SaleInvoice> getFullyInvoicedByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
                
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount - x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount) == 0))
                .ToList();
        }

        /// <summary>
        /// Get all Partiall saleInvoices for user.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getPartiallyInvoiced(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
          
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount-x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount)!=0))
                .ToList();
        }
        public List<SaleInvoice> getPartiallyInvoicedFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
          
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount - x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount) != 0))
                .ToList();
        }
        public List<SaleInvoice> getPartiallyInvoicedByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
              
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.totalInvoiceAmount - x.salesReceipts.Where(y => y.isVoid != true).Sum(y => y.CollectionAmount) != 0))
                .ToList();
        }

        //#Void
        /// <summary>
        /// Get all Void saleInvoices for user.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleInvoices
            
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void saleInvoices .
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getVoidRegisterAdministrator()
        {
            return context.saleInvoices

        .Where(x => x.isVoid == true)
    .ToList();
        }
        public List<SaleInvoice> getVoidRegisterAdministratorFirst()
        {
            return context.saleInvoices
 
        .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void saleInvoices .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.saleInvoices
 
    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void saleInvoices for user count. 
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
            return context.saleInvoices
                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void saleInvoices own.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.saleInvoices
       
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all pending saleInvoices by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
         

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
           

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
         
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

        /// <summary>
        /// Get own pending saleInvoices by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
             

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
          
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }

        /// <summary>
        /// Get Count pending for closing saleInvoices by supervisor Id and <paramref name="StatusId"/>.
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

            return context.saleInvoices

               
                .Where(x =>  (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get Count pending for closing saleInvoices own <paramref name="StatusId"/>.
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

            return context.saleInvoices

                
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all saleInvoices count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getsaleInvoicesCountByStatusId(int uid, int statusId)
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

                    return _DbContext.saleInvoices
                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.saleInvoices.Where(x => x.saleInvoiceStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all saleInvoices count
        /// </summary>
        /// <returns></returns>
        public int getsaleInvoicesCount(int uid)
        {
            if (uid != 0)
            {
                var user = context.Users.FirstOrDefault(x => x.id == uid);
                List<int> deptIds = new List<int>();
                List<int> companyIds = new List<int>();
                foreach (var comp in user.employee.Companies)
                    companyIds.Add(comp.Id);
                foreach (var dpt in user.employee.departments)
                    deptIds.Add(dpt.Id);

                return context.saleInvoices

                    
                    .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true && x.isApproved == true)
                    .Count();
            }
            else
                return context.saleInvoices.Where(x => x.isApproved == true).Count();

        }
        /// <summary>
        /// Get all saleInvoices by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
           

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }
        public List<SaleInvoice> getAllPobyStatusIdFirst(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
           
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }
        public List<SaleInvoice> getAllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
                

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true && x.isVoid != true && x.saleInvoiceStatus.Id == StatusId && x.isApproved == true))
                .ToList();
        }

        /// <summary>
        /// Get all saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForAdministrator()
        {
            return context.saleInvoices

                .Where(x => x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
         
                .Where( x => x.CreationDate >= previousMonthDate && x.isApproved == false)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.saleInvoices

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false)
                .ToList();
        }

        /// Get Count saleInvoices.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.saleInvoices

                .Where(x => x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get Count saleInvoices.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.saleInvoices

                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllPendingForClosingAdministrator()
        {
            return context.saleInvoices
   
                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleInvoices
       
                .Where(x => x.CreationDate >= previousMonthDate  && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<SaleInvoice> getAllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.saleInvoices
              
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

        /// <summary>
        /// Get all saleInvoices.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAll()
        {
            try
            {
                return context.saleInvoices
           
                .ToList();
            }
            catch
            { return null; }

        }

        /// <summary>
        /// Get all saleInvoices by Department and Customer Id.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllInvoicesByCustomerId(int CompanyId, int currency_id, List<Department> departments, int customer_id)
        {
            try
            {
                List<int> dept_Ids = new List<int>();
                foreach (var _dept in departments)
                {
                    dept_Ids.Add(_dept.Id);
                }

                return context.saleInvoices
            
          
                .Where(x => x.company_Id ==CompanyId &&  x.currency_Id == currency_id && dept_Ids.Contains( x.dept_Id) && x.customerCompany.Id == customer_id && x.isVoid != true)
                .ToList();
            }
            catch
            { return null; }

        }


        /// <summary>
        /// Get all saleInvoices by Department Id.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllInvoicesByDepartmentId(int companyId, int currency_id, List<Department> departments, int principal_id)
        {
            try
            {
                List<int> dept_Ids = new List<int>();
                foreach (var _dept in departments)
                {
                    dept_Ids.Add(_dept.Id);
                }

                return context.saleInvoices
    
                .Where(x =>x.company_Id == companyId && x.currency_Id == currency_id && dept_Ids.Contains(x.dept_Id) && x.principal_Id == principal_id && x.isVoid != true)
                .ToList();
            }
            catch
            { return null; }

        }


        /// <summary>
        /// Get all saleInvoices by Department and Customer Id.
        /// </summary>
        /// <returns></returns>
        public List<CustomerCredit> getAllInvoicesForCustomerCredits(int CompanyId, int currency_id, List<Department> departments)
        {
            try
            {
                List<int> dept_Ids = new List<int>();
                foreach (var _dept in departments)
                {
                    dept_Ids.Add(_dept.Id);
                }

                return context.customerCredits
                .Where(x => x.SaleInvoice.company_Id == CompanyId && x.SaleInvoice.currency_Id == currency_id && dept_Ids.Contains( x.SaleInvoice.dept_Id) && x.SaleInvoice.saleInvoicetype == InquiryType.DistributionBiz_CustomerCredit && x.SaleInvoice.isVoid != true)
                .ToList();
            }
            catch
            { return null; }

        }

        /// <summary>
        /// Get all SaleInvoices Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoiceStatus> getAllSaleInvoiceStatus()
        {
            return context.saleInvoiceStatuses.ToList();
        }
        /// <summary>
        /// Get all SaleInvoices Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoiceStatus> getAllActiveSaleInvoiceStatus()
        {
            List<SaleInvoiceStatus> statuses = new List<SaleInvoiceStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses =_DbContext.saleInvoiceStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all SaleInvoices Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoiceStatus> getAllInActiveSaleInvoiceStatus()
        {
            List<SaleInvoiceStatus> statuses = new List<SaleInvoiceStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses =_DbContext.saleInvoiceStatuses/*.Include("SaleInvoices")*/.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all SaleInvoices Status.
        /// </summary>
        ///  <param name="purchaseOrderstatusid"></param>
        /// <returns></returns>
        public SaleInvoiceStatus getstatus(int purchaseOrderstatusid)
        {
            return context.saleInvoiceStatuses/*.Include("SaleInvoices")*/.FirstOrDefault(x => x.Id == purchaseOrderstatusid);
        }


        /// <summary>
        /// Get all saleInvoices of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<SaleInvoice> getAll(Employee employee)
        {
            return context.saleInvoices.Where(x => x.allocation_Id == employee.EmpId).ToList();
        }

        /// <summary>
        /// Get all saleInvoices of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<SaleInvoice> getAll(User user)
        {
            return context.saleInvoices.Where(x => x.user_Id == user.id).ToList();
        }



        ///// <summary>
        ///// Get all Item types
        ///// </summary>
        ///// <returns></returns>
        //public List<ItemType> getAllItemTypes()
        //{
        //    return  context.itemTypes.ToList();
        //}


        /// <summary>
        /// Get So based on Sales Refrence No.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public SaleInvoice get(string SoRef)
        {
            return context.saleInvoices.FirstOrDefault(x => x.SalesReferenceNo == SoRef);
        }

        /// <summary>
        /// Get So based on Sales Refrence No.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public CustomerCredit getCustomerCredit(int serialNo, int saleInvoiceId)
        {
            return context.customerCredits.FirstOrDefault(x => x.SerialNo == serialNo && x.SaleInvoiceId == saleInvoiceId);
        }

        /// <summary>
        /// Get all saleInvoices Allocated to an Employee <para>EmployeeId</para>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllBySOid(int saleOrderId)
        {
            return context.saleInvoices
                .Where(x => x.SaleOrderId == saleOrderId && x.isVoid != true).ToList();
        }
        /// <summary>
        /// Get saleInvoice based on SaleInvoice ID.
        /// </summary>
        /// <param name="saleInvoiceId"></param>
        /// <returns></returns>
        public SaleInvoice getForSIReports(int saleInvoiceId)
        {
            return context.saleInvoices
            .FirstOrDefault(x => x.Id == saleInvoiceId);
        }

        /// <summary>
        /// Get saleInvoice based on SaleInvoice ID.
        /// </summary>
        /// <param name="saleInvoiceId"></param>
        /// <returns></returns>
        public SaleInvoice get(int saleInvoiceId)
        {
            return context.saleInvoices
            .FirstOrDefault(x => x.Id == saleInvoiceId);
        }
        public SaleInvoice getForCommissionInvoice(int saleInvoiceId)
        {
            return context.saleInvoices

            .FirstOrDefault(x => x.Id == saleInvoiceId);
        }
        /// <summary>
        /// Update SaleOrder
        /// </summary>
        /// <param name="saleOrder"></param>
        public void update(SaleOrder saleOrder)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == saleOrder.Id);

            purchaseOrdertoUpdate = saleOrder;
            context.SaveChanges();
        }
        /// <summary>
        /// Get saleOrder based on SaleOrder ID.
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <returns></returns>
        public SaleOrder getsaleOrder(int saleOrderId)
        {
            return context.saleOrders
  
            .FirstOrDefault(x => x.Id == saleOrderId);
        }

        /// <summary>
        /// Get all saleInvoices Allocated to an Employee <para>EmployeeId</para>.
        /// </summary>
        /// <returns></returns>
        public List<SaleInvoice> getAllAllcatedtoEmployee(int EmployeeId)
        {
            return context.saleInvoices

                .Where(x => x.allocation_Id == EmployeeId && x.isApproved == true).ToList();
        }

        /// <summary>
        /// Get list of all CostSheetFields in DB
        /// </summary>
        /// <returns>List of CostSheetFields Objects</returns>
        public List<CostSheetField> getallCostSheetField()
        {
            return context.costSheetFields
                .ToList();

        }
        /// <summary>
        /// Get All Active CostSheetField
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive CostSheetFields
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getinActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new CostSheetField
        /// </summary>
        /// <param name="costSheetField">CostSheetField Object</param>
        public void AddCostSheetField(CostSheetField costSheetField)
        {
            context.costSheetFields.Add(costSheetField);
            context.SaveChanges();
        }
        /// <summary>
        /// get CostSheetField by ID
        /// </summary>
        /// <param name="costSheetFieldid">CostSheetField ID</param>
        /// <returns></returns>
        public CostSheetField getCostSheetField(int costSheetFieldId)
        {
            return context.costSheetFields
                .FirstOrDefault(x => x.Id == costSheetFieldId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="CostSheetField">CostSheetField  Object</param>
        public void UpdateCostSheetField(CostSheetField costSheetField)
        {
            CostSheetField prod = context.costSheetFields.FirstOrDefault(x => x.Id == costSheetField.Id);
            prod = costSheetField;
            context.SaveChanges();
        }

        //Get Sale Invoice by ID
        public SaleInvoice GetSaleInvoice(int id)
        {
            return context.saleInvoices
           
                .FirstOrDefault(x => x.Id == id);
        }
        public List<SaleInvoice> getSaleInvoiceRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices


        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }
        public List<SaleInvoice> getSaleInvoiceRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices


        .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }

        public List<SaleInvoice> getSaleInvoiceRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices


        .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).ToList();
        }
        public List<SaleInvoice> getInvoiceRegisterAdministrator()
        {
            return context.saleInvoices
 

        .Where(x => x.isVoid != true).ToList();
        }
        public List<SaleInvoice> getInvoiceRegisterAdministratorFirst()
        {
            return context.saleInvoices
    
        .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid != true).ToList();
        }
        public int getSaleInvoiceRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleInvoices
        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true).Count();
        }
        public int getInvoiceRegisterCountOWn()
        {
            return context.saleInvoices

        .Where(x => x.isVoid != true).Count();
        }
        ///// <summary>
        ///// Add new Comment
        ///// </summary>
        ///// <param name="viewInfo">viewInfo  Object</param>
        //public void Add(CommentLog viewInfo)
        //{
        //    context.CommentLogs.Add(viewInfo);
        //    context.SaveChanges();
        //    SystemLog.LogInfo(this.GetType(), "Added Comment for userName= " + viewInfo.User.userName + " Id= " + viewInfo.Id);

        //}
        ///// <summary>
        ///// Add new Comment for transaction
        ///// </summary>
        ///// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        //public void Add(int Transactionid, int Transactiontype, string comment, List<User> users)
        //{
        //    CommentLog viewInfo = new CommentLog();
        //    if (SystemLog.CurrentUserId != 0)
        //    {
        //        viewInfo.UserId = SystemLog.CurrentUserId;
        //        viewInfo.TransactionId = Transactionid;
        //        viewInfo.TransactionType = Transactiontype;
        //        viewInfo.Timestamp = System.DateTime.Now;
        //        viewInfo.ReadTimestamp = System.DateTime.Now;
        //        viewInfo.TaggedList = users;
        //        //viewInfo.Info = Info.ToString();
        //        viewInfo.Comment = comment;
        //        context.CommentLogs.Add(viewInfo);

        //        context.SaveChanges();
        //        SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
        //    }
        //}
        ///// <summary>
        ///// Add new Comment for transaction
        ///// </summary>
        ///// <param name="comment">comment  Object for Inquiry</param>
        //public void Add(int Transactionid, int Transactiontype, string comment)
        //{
        //    CommentLog viewInfo = new CommentLog();
        //    if (SystemLog.CurrentUserId != 0)
        //    {
        //        viewInfo.UserId = SystemLog.CurrentUserId;
        //        viewInfo.TransactionId = Transactionid;
        //        viewInfo.TransactionType = Transactiontype;
        //        viewInfo.Timestamp = System.DateTime.Now;
        //        viewInfo.ReadTimestamp = System.DateTime.Now;

        //        //viewInfo.Info = Info.ToString();
        //        viewInfo.Comment = comment;
        //        context.CommentLogs.Add(viewInfo);

        //        context.SaveChanges();
        //        SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
        //    }
        //}
        ///// <summary>
        ///// Add new Comment for transaction
        ///// </summary>
        ///// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        //public void AddComment( int Transactionid, int Transactiontype, string comment,List<User> TaggedUsers)
        //{
        //    CommentLog viewInfo = new CommentLog();
        //    if (SystemLog.CurrentUserId != 0)
        //    {
        //        viewInfo.UserId = SystemLog.CurrentUserId;
        //        viewInfo.TransactionId = Transactionid;
        //        viewInfo.TransactionType = Transactiontype;
        //        viewInfo.Timestamp = System.DateTime.Now;
        //        viewInfo.ReadTimestamp = System.DateTime.Now;
        //        //foreach(var user in TaggedUsers)
        //        {
        //            viewInfo.TaggedList = new List<User>();
        //            viewInfo.TaggedList = TaggedUsers;//.Add(new User() { id = user.id });

        //        }
        //        //viewInfo.Info = Info.ToString();
        //        viewInfo.Comment = comment;
        //        context.CommentLogs.Add(viewInfo);

        //        context.SaveChanges();
        //        SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
        //    }
        //}
        ///// <summary>
        ///// Get List of comments for currenttransaction
        ///// </summary>
        ///// 
        //public List<CommentLog> getcommentslog(int TransactionId, int transactiontype)
        //{
        //    return context.CommentLogs.Include("User").Include("User.employee.contact").Include("User.employee.person").Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        //}
        ///// <summary>
        ///// Get List of comments for currenttransaction
        ///// </summary>
        ///// 
        //public List<CommentLog> getcommentslogAsc(int TransactionId, int transactiontype)
        //{
        //    return context.CommentLogs.Include("User").Include("User.employee.contact").Include("User.employee.person").OrderBy(x=>x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        //}
        ///// <summary>
        ///// Get List of comments for currenttransaction
        ///// </summary>
        ///// 
        //public List<CommentLog> getcommentslogDsc(int TransactionId, int transactiontype)
        //{
        //    return context.CommentLogs.Include("User").Include("User.employee.contact").Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        //}
        public int getAllPendingForApprovalDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();

        }
        public int getAllActiveCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllInActiveCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllPendingforApprovalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingforApprovalCountOwnByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }

            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();

            var count = context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
            return count;
        }
        public int getAllPendingforClosingCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountownByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }


        public int getAllPendingForApprovalDepartmentalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllActiveCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            // var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }

            return context.saleInvoices
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllInActiveCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllPendingforApprovalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingforApprovalCountOwnByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices
                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountownByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.saleInvoices

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }
        public List<ERP_BL.Procurements.StatusClass.StatusClass> GetActiveStatusClasses()
        {
            return context.statusClasses.Where(x => x.isActive == true && x.transactionType== TransactionItemType.Sale_Invoice).ToList();
        } 
        public ERP_BL.Procurements.StatusClass.StatusClass GetStatusClass(int _id)
        {
            return context.statusClasses.FirstOrDefault(x => x.Id == _id);
        }
        public List<CustomerCredit> GetAllCustomerCredits()
        {
            return context.customerCredits.Where(x=>x.SaleInvoiceId!=null && x.SaleInvoice.isVoid!=true).ToList();
        }
        public List<CustomerCredit> GetAllSaleInvoiceCustomerCredits( int saleInvoiceId)
        {
            return context.customerCredits.Where(x=>x.SaleInvoiceId==saleInvoiceId  && x.SaleInvoice.isVoid != true).ToList();
        }
        public string getLastSystemReferenceNo()
        {

            var interBankTrans = context.interBankTransfers.ToList();
            if (interBankTrans == null || interBankTrans.Count == 0)
                return null;

            return interBankTrans.Last().SystemRefNo;
        }
        public List<SaleInvoice> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleInvoices
                .Where(x => deptIds.Contains(x.dept_Id) 
                && companyIds.Contains((int)x.company_Id) 
                && x.isVoid != true 
                && x.saleInvoiceStatus.isActive == true 
                && x.isApproved == true
                && x.InterCompany_Id == null
                && x.InterDepartment_Id == null
                ||
                //deptIds.Contains((int)x.InterDepartment_Id) && 
                //companyIds.Contains((int)x.InterCompany_Id)  && 
                x.isInterCompany == true
                && deptIds.Contains(x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.isVoid != true 
                && x.saleInvoiceStatus.isActive == true 
                && x.isApproved == true
                && x.InterCompany_Id==null
                && x.InterDepartment_Id==null)
                .ToList();
        }


    }
}
