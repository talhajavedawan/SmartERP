using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface ISaleOrderRepo
    {
        void Add(SaleOrder purchaseOrder);
        void Add(SaleOrder purchaseOrder, ProcurementProduct product);
        void addStatus(SaleOrderStatus status);
        void update(SaleOrder purchaseOrder);
        void update(SaleOrder purchaseOrder, Employee employee);
        void updateStatus(SaleOrder purchaseOrder, SaleOrderStatus status);
        void updateStatus(int purchaseOrderId, SaleOrderStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int purchaseOrderItemId, ItemType itemType);


        List<SaleOrder> getAll();
        List<SaleOrder> getAll(User user);
        List<SaleOrder> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        SaleOrder get(int purchaseOrderId);
    }
    public class SaleOrderRepo //: ISaleOrderRepo
    {

        DBContextERP context = new DBContextERP();





        public List<SplitPER> GetSplitPER(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.splitPERs

                .Where(x => x.saleOrder != null && (deptIds.Contains(x.saleOrder.dept_Id) && companyIds.Contains((int)x.saleOrder.company_Id) || (deptIds.Contains((int)x.saleOrder.InterDepartment_Id) && companyIds.Contains((int)x.saleOrder.InterCompany_Id) && x.saleOrder.isInterCompany == true)) && x.saleOrder.isVoid != true)

                .ToList();
        }

        /// <summary>
        /// Get All Reference Numbers
        /// </summary>
        /// <returns></returns>
        public List<VendorBillReference> GetAllBillReferenceNo()
        {
            return
            context.vendorBillReferences

            .ToList();
        }


        //static DBContextERP context;

        //public SaleOrderRepo()
        //{

        //    if (context == null)
        //    {
        //        context = new DBContextERP();
        //    }
        //    //        //return dbContext;

        //}

        /// <summary>
        /// Add SaleOrder in database
        /// </summary>
        /// <param name="purchaseOrder">SaleOrder Object</param>
        public void Add(SaleOrder purchaseOrder)
        {
            context.saleOrders.Add(purchaseOrder);
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
        /// <param name="purchaseOrder">SaleOrder Object (main object in which Item is to be added)</param>
        /// <param name="purchaseOrderItem">SaleOrderItem Object (item to be added)</param>
        public void Add(SaleOrder purchaseOrder, ProcurementProduct product)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
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
        /// Add SaleOrderStatus in database
        /// </summary>
        /// <param name="status">SaleOrderStatus Object</param>
        public void addStatus(SaleOrderStatus status)
        {
            context.saleOrderStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update SaleOrder
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void update(SaleOrder purchaseOrder)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate = purchaseOrder;
            context.SaveChanges();
        }

        /// <summary>
        /// Update SaleOrder
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void Approve(SaleOrder saleOrder)
        {
            SaleOrder saleOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == saleOrder.Id);
            saleOrdertoUpdate.isApproved = saleOrder.isApproved;
            saleOrdertoUpdate.stage = saleOrder.stage;
            context.SaveChanges();
        }

        /// <summary>
        /// Update SaleOrder and allocate it to new employee
        /// </summary>
        /// <param name="purchaseOrder">SaleOrder to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(SaleOrder purchaseOrder, Employee employee)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.employee = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all Active saleOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Get all Inactive saleOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllInActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(SaleOrderStatus status)
        {
            SaleOrderStatus poStatus = context.saleOrderStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="status"></param>
        public void updateStatus(SaleOrder purchaseOrder, SaleOrderStatus status)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.saleOrderStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status based on SaleOrder ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="status"></param>
        public void setSotoVoid(int SoId, bool isVoid)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == SoId);
            purchaseOrdertoUpdate.isVoid = isVoid;
            context.SaveChanges();
        }

        /// <summary>
        /// Change purchaseOrder status based on SaleOrder ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="status"></param>
        public void updateStatus(int purchaseOrderId, SaleOrderStatus status)
        {
            SaleOrder purchaseOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == purchaseOrderId);
            purchaseOrdertoUpdate.saleOrderStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status based on SaleOrder ID
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <param name="status"></param>
        public void updateStatusById(int saleOrderId, SaleOrderStatus status)
        {
            SaleOrder saleOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == saleOrderId);
            var SoStatus = context.saleOrderStatuses.FirstOrDefault(x => x.Id == status.Id);
            saleOrdertoUpdate.saleOrderStatus = SoStatus;
            context.SaveChanges();
        }
        public void updateStatusById(int saleOrderId, int statusId)
        {
            SaleOrder saleOrdertoUpdate = context.saleOrders.FirstOrDefault(x => x.Id == saleOrderId);
            var SoStatus = context.saleOrderStatuses.FirstOrDefault(x => x.Id == statusId);
            saleOrdertoUpdate.saleOrderStatus = SoStatus;
            context.SaveChanges();
        }

        ///// <summary>
        ///// Update Item type of specified Item
        ///// </summary>
        ///// <param name="purchaseOrderItem"></param>
        ///// <param name="itemType"></param>
        //public void upadteItemType(Item item, ItemType itemType)
        //{
        //    InqueryItem ItemtoUpdate = context.purchaseOrderItems.FirstOrDefault(x => x.Id == purchaseOrderItem.Id);
        //    ItemtoUpdate.itemType = itemType;
        //    context.SaveChanges();
        //}

        ///// <summary>
        ///// Update Item type of specified item ID
        ///// </summary>
        ///// <param name="purchaseOrderItemId"></param>
        ///// <param name="itemType"></param>
        //public void upadteItemType(int purchaseOrderItemId, ItemType itemType)
        //{
        //    InqueryItem ItemtoUpdate = context.purchaseOrderItems.FirstOrDefault(x => x.Id == purchaseOrderItemId);
        //    ItemtoUpdate.itemType = itemType;
        //    context.SaveChanges();
        //}

        /// <summary>
        /// Get all saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAll(int uid)
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

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if(user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.saleOrders
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            
        }

        /// <summary>
        /// Get all saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllForDepartments(int uid, List<int> deptIds)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.saleOrders


                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> getFirstAll(int uid)
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

            

            if(user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                .Where(x => (x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                
                ||

                ( ( (user.employee.SODataRetrievalDate != null &&  x.CreationDate >= user.employee.SODataRetrievalDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)
                || user.employee.SODataRetrievalDate != null && x.CreationDate >= user.employee.SODataRetrievalDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)))
                || (x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)
                || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) ) )
                && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                if (user.employee.SODataRetrievalDate != null && user.employee.SODataRetrievalDate > previousMonthDate)
                    previousMonthDate = user.employee.SODataRetrievalDate.Value;

                return context.saleOrders
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            
        }
        public List<SaleOrder> loadMoreActiveInActiveSo(DateTime date, int uid)
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
            if (date.Month != 1)
            {
                return context.saleOrders
           
                    .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                    /* .Skip(skipCount).Take(20)*/.ToList();
            }
            else
            {
                return context.saleOrders

                   .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                   /* .Skip(skipCount).Take(20)*/.ToList();
            }
        }

        public List<SaleOrder> ActiveInActiveSoByDateRange(DateTime from, DateTime to, int uid)
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

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders.Where(x =>x.isApproved == true && x.isVoid != true && 
                (
             
                (
                x.CreationDate != null &&
                RetrievalDate != null &&
                x.CreationDate >= RetrievalDate &&
                (
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)) || (x.isInterCompany == true && deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id))
                )
                )
             
                ||
             
                (
                x.CreationDate >= from && x.CreationDate <= to && (
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)) || (x.isInterCompany == true && deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id)) ) 
                && x.saleOrderStatus.isActive == true   
                )
                )
                ).ToList();

            }
            else
            {
                return context.saleOrders

                   .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                   (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                  .ToList();
            }
        }
        /// <summary>
        /// Get all Inactive saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
  
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> getAllFirstInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);

            if (user.employee.SODataRetrievalDate != null && user.employee.SODataRetrievalDate > previousMonthDate)
                previousMonthDate = user.employee.SODataRetrievalDate.Value;

            return context.saleOrders
             
      
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> loadMoreAllInActive(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (date.Month != 1)
            {

                return context.saleOrders
 
              .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
              .ToList();

            }
            else
            {
                return context.saleOrders
 
              .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
     
              .ToList();
            }
            return context.saleOrders

                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && (date.Month != 1 ? x.CreationDate.Value.Year == date.Year : x.CreationDate.Value.Year == date.Year - 1) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             
                .ToList();
        }


        public List<SaleOrder> AllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders

             .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
             (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             ||
             (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
             x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             )
             .ToList();
            }
            else
            {
                return context.saleOrders

            .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .ToList();
            }
        }
        /// <summary>
        /// Get all Active saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        public List<SaleOrder> getAllFirstActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);

            if (user.employee.SODataRetrievalDate != null && user.employee.SODataRetrievalDate > previousMonthDate)
                previousMonthDate = user.employee.SODataRetrievalDate.Value;

            return context.saleOrders
        
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> AllActivebyDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders.Where(x =>
        
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate && ( (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)) ||
                (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)))
        
                ||
        
                (x.CreationDate >= from && x.CreationDate <= to &&((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)) ||
                (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)))
        
                &&
                x.saleOrderStatus.isActive == true &&
                x.isApproved == true &&
                x.isVoid != true
    
                ).ToList();
            }
            else
            {

                return context.saleOrders
                    .Where(x => x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate && ( (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)) ||
                    (x.isInterCompany == true && deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id))) &&
                    x.saleOrderStatus.isActive == true &&
                    x.isApproved == true &&
                    x.isVoid != true
                    )
                    .ToList();
            }
        }

        /// <summary>
        /// Get all Inactive saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllInActiveandUnapproved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
           
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllActiveandUnapporved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
 
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all saleOrders for user.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SaleOrder> getSaleRegister(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {

                return new ObservableCollection<SaleOrder>(context.saleOrders

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isVoid != true)
                )
                .ToList());
            }
            else
            {
                return new ObservableCollection<SaleOrder>(context.saleOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList());
            }

        }

        /// <summary>
        /// Get all saleOrders for user.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SaleOrder> getSaleRegisterForDepartments(int uid, List<int> deptIds)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return new ObservableCollection<SaleOrder>(context.saleOrders

                  .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                  .ToList());

        }

        public List<SaleOrder> getFirstSaleRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> loadMoreSaleRegister(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (date.Month != 1)
            {
                return context.saleOrders
             .OrderByDescending(x => x.CreationDate)
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.saleOrders
            .OrderByDescending(x => x.CreationDate)
                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
            }
        }

        public List<SaleOrder> SaleRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                    .OrderByDescending(x => x.CreationDate)
                    .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                    ||
                    (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isVoid != true)
                    )
                    .ToList();
            }
            else
            {
                return context.saleOrders
                    .OrderByDescending(x => x.CreationDate)
                    .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                    .ToList();
            }

        }
        /// <summary>
        /// Get all saleOrders .
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getSaleRegisterAdministrator()
        {
            return context.saleOrders

    .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid != true)
    .ToList();
        }
        /// <summary>
        /// Get all saleOrders .
        /// </summary>
        /// <returns></returns>
        public int getSaleRegisterAdministratorCount()
        {
            return context.saleOrders
  
    .Where(x => x.isVoid != true)
    .Count();
        }
        /// <summary>
        /// Get all saleOrders for user count. 
        /// </summary>
        /// <returns></returns>
        public int getSaleRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all saleOrders own.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getSaleRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
         
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .ToList();
        }

        //#Void
        /// <summary>
        /// Get all Void saleOrders for user.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
       
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .ToList();
        }

        public List<SaleOrder> getVoidFirstRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
          
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get Total of Collections in all recipts against the non-void sale invoces for this specific Sale Order
        /// </summary>
        /// <param name="id">Sale order Id</param>
        /// <returns>Sum of Collections</returns>
        public double getCollectedAmountFromSO(int id)
        {
            var result = Convert.ToDouble(context.saleInvoices
        .Where(x => x.SaleOrderId == id && x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
            //var result = context.saleOrders.Where(x => x.isVoid != true && sa).Sum(a=> a.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
            return result;

        }
        /// <summary>
        /// Get Total of Credited Amount (Collection - Deductions) in all recipts against the non-void sale invoces for this specific Sale Order
        /// </summary>
        /// <param name="id">Sale order Id</param>
        /// <returns>Sum of Collections</returns>
        public double? getCreditedAmountFromSO(int id)
        {
            var collected = getCollectedAmountFromSO(id);
            var deducted = getDeductedAmountFromSO(id);
            return collected - deducted;

        }
        /// <summary>
        /// Get Total of deductions in all recipts against the non-void sale invoce for this specific Sale Order
        /// </summary>
        /// <param name="id">Sale order Id</param>
        /// <returns>Sum of Deductions</returns>
        public double getDeductedAmountFromSO(int id)
        {
            return Convert.ToDouble(context.saleInvoices
        .Where(x => x.SaleOrderId == id && x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount))));


        }
        /// <summary>
        /// Get all Void saleOrders .
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getVoidRegisterAdministrator()
        {
            return context.saleOrders

        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void saleOrders .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.saleOrders
  
    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void saleOrders for user count. 
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
            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void saleOrders own.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }

        /// <summary>
        /// Get all saleOrders  own count. 
        /// </summary>
        /// <returns></returns>
        public int getSaleRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending saleOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
    
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
             
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> loadMoreFirstPendingForReApproval(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            if (date.Month != 1)
            {
                return context.saleOrders
            
       
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.saleOrders
                            
                                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                                .ToList();
            }

        }

        public List<SaleOrder> PendingForReApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
            .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
           (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
            ||

            (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.saleOrderStatus.isActive == true && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
            )
            .ToList();
            }
            else
            {
                return context.saleOrders
            .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
           (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
            .ToList();
            }

        }
        /// <summary>
        /// Get all pending saleOrders by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
  
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        public List<SaleOrder> getAllFirstPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
  
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        public List<SaleOrder> loadMoreFirstPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
 
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> loadMorePendingForReApprovalDepartmental(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            if (date.Month != 1)
            {
                return context.saleOrders

                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.saleOrders
 
                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }

        }
        public List<SaleOrder> PendingForReApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                    .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                    ||
                    (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                    )
                    .ToList();
            }
            else
            {
                return context.saleOrders
                    .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                    .ToList();
            }

        }
        /// <summary>
        /// Get Count pending saleOrders by Departments
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
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }




        /// <summary>
        /// Get Own pending saleOrders by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
        
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
     
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 2 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<SaleOrder> loadMoreFirstPendingForReApprovalOwn(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            if (date.Month != 1)
            {
                return context.saleOrders
                      
                                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                                .ToList();
            }
            else
            {
                return context.saleOrders
             
                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
            }

        }

        public List<SaleOrder> PendingForReApprovalOwnByDateRamge(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
                            
                            .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                            .ToList();

        }
        /// <summary>
        /// Get Count pending saleOrders by user Id and <paramref name="UserID"/>.
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
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Get Own Count pending saleOrders user Id and <paramref name="UserID"/>.
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
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }




        /// <summary>
        /// Get all pending saleOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
          
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders
               
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)

                .ToList();
        }
        public List<SaleOrder> loadMoreAllPendingForApproval(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            if (date.Month != 1)
            {
                return context.saleOrders
                     
                                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                //.Skip(skipCount).Take(20)
                                .ToList();
            }
            else
            {
                return context.saleOrders
         
                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
            }

        }

        public List<SaleOrder> AllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                //(x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                return context.saleOrders

                            .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            ||
                            (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
                            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                            x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.saleOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true)
                            )
                            .ToList();
            }
            else
            {
                return context.saleOrders

                            .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            .ToList();
            }
        }


        /// <summary>
        /// Get all pending saleOrders by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> LoadMoreAllPendingForApprovalDepartmental(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            if (date.Month != 1)
            {
                return context.saleOrders

                    .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                    .ToList();
            }
            else
            {
                return context.saleOrders

                    .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)

                    .ToList();
            }

        }



        public List<SaleOrder> AllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                    .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                    ||
                    (x.CreationDate >= from & x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true)
                    )
                    .ToList();
            }
            else
            {
                return context.saleOrders
                    .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                    .ToList();
            }
        }
        /// <summary>
        /// Get Count pending saleOrders by Departments
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

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending saleOrders by Departmental
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> loadMorePendingForClosingDepartmental(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            if (date.Month != 1)
            {
                return context.saleOrders
                    .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
          
                    .ToList();
            }
            else
            {
                return context.saleOrders

                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
        }

        public List<SaleOrder> PendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.saleOrders
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true).ToList();
            }
        }


        /// <summary>
        /// Get all pending saleOrders by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own pending saleOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> loadMoreAllPendingForApprovalOwn(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            if (date.Year != 1)
            {
                return context.saleOrders

                    .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                    //.Skip(skipCount).Take(20)
                    .ToList();
            }
            else
            {
                return context.saleOrders
           
                    .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                    //.Skip(skipCount).Take(20)
                    .ToList();
            }
        }


        public List<SaleOrder> AllPendingForApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.saleOrders
                    .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                    (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                    .ToList();
            }

        }
        /// <summary>
        /// Get Count pending saleOrders by supervisor Id and <paramref name="StatusId"/>.
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

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending saleOrders
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

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending saleOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> getAllFirstPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> loadMoreAllPendingForClosing(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            if (date.Year != 1)
            {
                return context.saleOrders

                    .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                    .ToList();
            }
            else
            {
                return context.saleOrders
        
                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
            }
        }


        public List<SaleOrder> AllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {

                return context.saleOrders

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.saleOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get own pending saleOrders by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders
 
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders
          
    
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }

        public List<SaleOrder> loadMoreAllPendingForClosingOwn(DateTime date, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            if (date.Year != 1)
            {
                return context.saleOrders
     
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.saleOrders
                   
                                .Where(x => x.CreationDate.Value.Month > date.Month && x.CreationDate.Value.Month >= 12 && x.CreationDate.Value.Year == date.Year - 1 && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                                .ToList();
            }

        }


        public List<SaleOrder> AllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.saleOrders

            .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
            ||
            (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
            )
            .ToList();
            }
            else
            {
                return context.saleOrders

            .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
            }


        }

        /// <summary>
        /// Get Count pending for closing saleOrders by supervisor Id and <paramref name="StatusId"/>.
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

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count pending for closing saleOrders own <paramref name="StatusId"/>.
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

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all saleOrders count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getsaleOrdersCountByStatusId(int uid, int statusId)
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

                    return _DbContext.saleOrders

                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == statusId && x.isApproved == true && x.isVoid != true)
                        .Count();
                }
                else
                    return _DbContext.saleOrders.Where(x => x.saleOrderStatus.Id == statusId && x.isApproved == true && x.isVoid != true).Count();
            }
        }

        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getUserSOByYear(int userId, int year)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();


            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders
         
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getDepartmentalSOByYear(int departmentId, int companyId, int year)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.saleOrders
                
                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getDepartmentalSOForAllPrecedingMonths(int departmentId, int companyId, int year, int month)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.saleOrders
                
                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.CreationDate.Value.Month <= month && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getDepartmentalSOByMonth(int departmentId, int companyId, int year, int month)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.saleOrders
                
                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.CreationDate.Value.Month == month && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all saleOrders count
        /// </summary>
        /// <returns></returns>
        public int getsaleOrdersCount(int uid)
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

                return context.saleOrders

                    .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                    .Count();
            }
            else
                return context.saleOrders.Where(x => x.isApproved == true).Count();

        }
        /// <summary>
        /// Get all saleOrders by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.saleOrders
              
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<SaleOrder> getAllFirstPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders
              
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        public List<SaleOrder> loadMoreAllPobyStatusId(DateTime date, int uid, int StatusId)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            if (date.Month != 1)
            {
                return context.saleOrders
                             
                                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year
                                && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                                //.Skip(skipCount).Take(20)
                                .ToList();
            }
            else
            {
                return context.saleOrders
             
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && x.CreationDate.Value.Year == date.Year - 1
                && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
            }
        }

        public List<SaleOrder> AllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.SODataRetrievalDate != null)
                RetrievalDate = user.employee.SODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {

                return context.saleOrders

                            .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                            (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                            ||
                            (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            x.CreationDate >= from && x.CreationDate <= to &&
                            (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                            )
                            .ToList();
            }
            else
            {
                return context.saleOrders

                            .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                            (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                            (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.saleOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                            .ToList();
            }

        }

        /// <summary>
        /// Get all saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForAdministrator()
        {
            return context.saleOrders
              
                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
        }



        public List<SaleOrder> getAllFirstPendingForAdministrator()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders
              
            .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false && x.isVoid != true)
            .ToList();
        }


        public List<SaleOrder> loadMoreAllPendingForAdministrator(DateTime date)
        {
            return context.saleOrders
               
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && (date.Month != 1 ? x.CreationDate.Value.Year == date.Year : x.CreationDate.Value.Year == date.Year - 1) && x.isApproved == false && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
        }

        public List<SaleOrder> AllPendingForAdministrator(DateTime date)
        {
            return context.saleOrders
                
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && (date.Month != 1 ? x.CreationDate.Value.Year == date.Year : x.CreationDate.Value.Year == date.Year - 1) && x.isApproved == false && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
        }
        public List<SaleOrder> AllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.saleOrders
                
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        /// Get Count saleOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.saleOrders

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count saleOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.saleOrders

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllPendingForClosingAdministrator()
        {
            return context.saleOrders
                
                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<SaleOrder> getAllFirstPendingForClosingAdministrator()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.saleOrders
                
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }


        public List<SaleOrder> loadMoreAllPendingForClosingAdministrator(DateTime date)
        {
            return context.saleOrders
                
                .Where(x => x.CreationDate.Value.Month < date.Month && x.CreationDate.Value.Month >= date.Month - 1 && (date.Month != 1 ? x.CreationDate.Value.Year == date.Year : x.CreationDate.Value.Year == date.Year - 1) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                //.Skip(skipCount).Take(20)
                .ToList();
        }
        public List<SaleOrder> AllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.saleOrders
                
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .ToList();
        }

        /// <summary>
        /// Get all saleOrders.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAll()
        {
            return context.saleOrders
               
            .Where(x => x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all SaleOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrderStatus> getAllSaleOrderStatus()
        {
            //return context.saleOrderStatuses.Include("SaleOrders").ToList();
            return context.saleOrderStatuses.ToList();
        }
        /// <summary>
        /// Get all SaleOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrderStatus> getAllActiveSaleOrderStatus()
        {
            List<SaleOrderStatus> statuses = new List<SaleOrderStatus>();
            using (var _DbContext = new DBContextERP())
            {
                //return context.saleOrderStatuses.Include("SaleOrders").Where(x => x.isActive == true).ToList();
                statuses = _DbContext.saleOrderStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all SaleOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrderStatus> getAllInActiveSaleOrderStatus()
        {
            List<SaleOrderStatus> statuses = new List<SaleOrderStatus>();
            using (var _DbContext = new DBContextERP())
            {
                var st = _DbContext.saleOrderStatuses.Where(x => x.isActive == false);
                if (st != null)
                    statuses = st.ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all SaleOrders Status.
        /// </summary>
        ///  <param name="purchaseOrderstatusid"></param>
        /// <returns></returns>
        public SaleOrderStatus getstatus(int purchaseOrderstatusid)
        {
            return context.saleOrderStatuses.FirstOrDefault(x => x.Id == purchaseOrderstatusid);
        }


        /// <summary>
        /// Get all saleOrders of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<SaleOrder> getAll(Employee employee)
        {
            return context.saleOrders.Where(x => x.allocation_Id == employee.EmpId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get all saleOrders of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<SaleOrder> getAll(User user)
        {
            return context.saleOrders.Where(x => x.user_Id == user.id && x.isVoid != true).ToList();
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
        public SaleOrder get(string SoRef)
        {
            return context.saleOrders.FirstOrDefault(x => x.SalesReferenceNo == SoRef);
        }

        /// <summary>
        /// Get purchaseOrder based on SaleOrder ID.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public SaleOrder get(int purchaseOrderId)
        {
            return context.saleOrders
            //.Include("customerCompany")
            //.Include("customerCompany.company")
            //.Include("customerCompany.parentCompany.company")
            //.Include("customerCompany.contactPerson")
            //.Include("customerCompany.company.currency")
            //.Include("offer.customerCompany.company")
            //.Include("offer.customerCompany.company.currency")
            //.Include("vendors")
            //.Include("vendors.company")
            //.Include("principal.company")
            //.Include("company").Include("InterCompany").Include("InterDepartment")
            //.Include("employee")
            //.Include("CommissionSummarySheet.FieldValues")
            //.Include("user.employee.Supervisor")
            //.Include("user.employee.Supervisor.person")
            //.Include("user.employee.Supervisor.Supervisor")
            //.Include("user.employee.Supervisor.Supervisor.person")
            //.Include("user.employee.Supervisor.Supervisor.Supervisor")
            //.Include("user.employee.Supervisor.Supervisor.Supervisor.person")
            //.Include("department")
            //.Include("SaleOrderStatus")
            //.Include("products")
            //.Include("bid")
            //.Include("PurchaseOrders")
            //.Include("PurchaseOrders.PurchaseInvoices")
            //.Include("PurchaseOrders.PurchaseInvoices.Payments")
            //.Include("Bills.Payments")
            //.Include("SplitPERs")
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }
        /// <summary>
        /// Get all saleOrders Allocated to an Employee <para>EmployeeId</para>.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getAllAllcatedtoEmployee(int EmployeeId)
        {
            return context.saleOrders
                
                .Where(x => x.allocation_Id == EmployeeId && x.isApproved == true && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get list of all CostSheetFields in DB
        /// </summary>
        /// <returns>List of CostSheetFields Objects</returns>
        public List<CostSheetField> getallCostSheetField()
        {
            return context.costSheetFields
                .OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Get All Active CostSheetField
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == true).OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Get All inActive CostSheetFields
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getinActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == false).OrderBy(y => y.SortId).ToList();

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

        /// <summary>
        /// Get list of all SummarySheetFields in DB
        /// </summary>
        /// <returns>List of SummarySheetFields Objects</returns>
        public List<SummarySheetField> getallSummarySheetField()
        {
            return context.summarySheetFields
                .OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Get All Active SummarySheetField
        /// </summary>
        /// <returns></returns>
        public List<SummarySheetField> getActiveSummarySheetFields()
        {
            return context.summarySheetFields.Where(x => x.isActive == true).OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Get All inActive SummarySheetFields
        /// </summary>
        /// <returns></returns>
        public List<SummarySheetField> getinActiveSummarySheetFields()
        {
            return context.summarySheetFields.Where(x => x.isActive == false).OrderBy(y => y.SortId).ToList();

        }
        /// <summary>
        /// Add new SummarySheetField
        /// </summary>
        /// <param name="summarySheetField">SummarySheetField Object</param>
        public void AddSummarySheetField(SummarySheetField summarySheetField)
        {
            context.summarySheetFields.Add(summarySheetField);
            context.SaveChanges();
        }
        /// <summary>
        /// get SummarySheetField by ID
        /// </summary>
        /// <param name="summarySheetFieldid">SummarySheetField ID</param>
        /// <returns></returns>
        public SummarySheetField getSummarySheetField(int summarySheetFieldId)
        {
            return context.summarySheetFields
                .FirstOrDefault(x => x.Id == summarySheetFieldId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="SummarySheetField">SummarySheetField  Object</param>
        public void UpdateSummarySheetField(SummarySheetField summarySheetField)
        {
            SummarySheetField prod = context.summarySheetFields.FirstOrDefault(x => x.Id == summarySheetField.Id);
            prod = summarySheetField;
            context.SaveChanges();
        }
        public List<CostSheetBillField> GetCostSheetBillFields(int _billId, int _costSheetId)
        {
            return context.costSheetBillFields.Where(x => x.CostSheetId == _costSheetId && x.Bill_Id == _billId).ToList();
        }
        public void UpdateBillField(int _billId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedSystemValue)
        {
            var billField = context.costSheetBillFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Bill_Id == _billId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (billField != null)
            {
                //context.costSheetBillFields.Remove(billField);
                billField.Value = addedSystemValue;
                context.SaveChanges();
            }
        }
        public void UpdateSOField(int _soId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            var soFieldField = context.costSheetSOFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.SO_Id == _soId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (soFieldField != null)
            {
                soFieldField.Value = addedRSBCValue;
                //context.costSheetSOFields.Remove(soFieldField);
                context.SaveChanges();
            }
        }
        public CostSheetSOField GetSOField(int _soId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetSOField field = new CostSheetSOField();
            field = context.costSheetSOFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.SO_Id == _soId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetPOField GetPOField(int _poId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetPOField field = new CostSheetPOField();
            field = context.costSheetPOFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.PO_Id == _poId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetSIField GetSIField(int _siId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetSIField field = new CostSheetSIField();
            field = context.costSheetSIFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.SI_Id == _siId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetSaleReceiptField GetReceiptField(int _receiptId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetSaleReceiptField field = new CostSheetSaleReceiptField();
            field = context.costSheetSaleReceiptFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Receipt_Id == _receiptId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetPaymentField GetPaymentField(int _paymentId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetPaymentField field = new CostSheetPaymentField();
            field = context.costSheetPaymentFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Payment_Id == _paymentId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }

        public CostSheetBillField GetBillField(int _billId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedRSBCValue)
        {
            CostSheetBillField field = new CostSheetBillField();
            field = context.costSheetBillFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Bill_Id == _billId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetPOField GetPOField(int _poId, int _costSheetId, int fieldId, CostFieldType _fieldType)
        {
            CostSheetPOField field = new CostSheetPOField();
            field = context.costSheetPOFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.PO_Id == _poId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetSIField GetSIField(int _siId, int _costSheetId, int fieldId, CostFieldType _fieldType)
        {
            CostSheetSIField field = new CostSheetSIField();
            field = context.costSheetSIFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.SI_Id == _siId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetSaleReceiptField GetReceiptField(SalesReceipt _receipt, int _costSheetId, int fieldId, CostFieldType _fieldType)
        {
            CostSheetSaleReceiptField field = new CostSheetSaleReceiptField();
            field = context.costSheetSaleReceiptFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Receipt_Id == _receipt.Id && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetPaymentField GetPaymentField(Payment _payment, int _costSheetId, int fieldId, CostFieldType _fieldType)
        {
            CostSheetPaymentField field = new CostSheetPaymentField();
            field = context.costSheetPaymentFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Payment_Id == _payment.Id && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }
        public CostSheetBillField GetBillField(int _billId, int _costSheetId, int fieldId, CostFieldType _fieldType)
        {
            CostSheetBillField field = new CostSheetBillField();
            field = context.costSheetBillFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Bill_Id == _billId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (field != null)
            {
                return field;
            }
            else
                return null;
        }

        public decimal GetSystemCost(int _costSheetId, int fieldId)
        {
            var billFields = context.costSheetBillFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.Bill_Cost).ToList();
            var poFields = context.costSheetPOFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.PO_Cost).ToList();
            var receiptFields = context.costSheetSaleReceiptFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.Receipt_Cost).ToList();
            var paymentFields = context.costSheetPaymentFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.System_Payment).ToList();
            var siFields = context.costSheetSIFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.SI_Cost).ToList();

            var billFieldsSum = billFields.Select(x => x.Value).Sum();
            var poFieldsSum = poFields.Select(x => x.Value).Sum();
            var receriptFieldsSum = receiptFields.Select(x => x.Value).Sum();
            var paymentFieldsSum = paymentFields.Select(x => x.Value).Sum();
            var siFieldsSum = siFields.Select(x => x.Value).Sum();

            //var fieldValue=costSheet.FieldValues.FirstOrDefault(x => x.Field.Type == 35 && x.Id == fieldId);
            return billFieldsSum + poFieldsSum + receriptFieldsSum + paymentFieldsSum + siFieldsSum;
        }
        public decimal GetSBRC(int _costSheetId, int fieldId)
        {
            var soFields = context.costSheetSOFields.Where(x => x.CostSheetId == _costSheetId && x.FieldId == fieldId && x.FieldType == CostFieldType.soAmountSRBC).ToList();
            var soFieldsSum = soFields.Select(x => x.Value).Sum();
            return soFieldsSum;
        }
        public List<CostSheetPOField> GetCostSheetPOFields(int _purchaseOrderId, int _costSheetId)
        {
            return context.costSheetPOFields.Where(x => x.CostSheetId == _costSheetId && x.PO_Id == _purchaseOrderId).ToList();
        }
        public List<CostSheetSIField> GetCostSheetSIFields(int _saleInvoiceId, int _costSheetId)
        {
            return context.costSheetSIFields.Where(x => x.CostSheetId == _costSheetId && x.SI_Id == _saleInvoiceId).ToList();
        }
        public List<CostSheetSaleReceiptField> GetCostSheetReceiptFields(int _receiptId, int _costSheetId)
        {
            return context.costSheetSaleReceiptFields.Where(x => x.CostSheetId == _costSheetId && x.Receipt_Id == _receiptId).ToList();
        }
        public List<CostSheetPaymentField> GetCostSheetPaymentFields(int _paymentId, int _costSheetId)
        {
            return context.costSheetPaymentFields.Where(x => x.CostSheetId == _costSheetId && x.Payment_Id == _paymentId).ToList();
        }
        public List<CostSheetSOField> GetCostSheetSOFields(int _saleOrderId, int _costSheetId)
        {
            return context.costSheetSOFields.Where(x => x.CostSheetId == _costSheetId && x.SO_Id == _saleOrderId).ToList();
        }
        public void UpdatePOField(int _purchaseOrderId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedSystemValue)
        {
            var purchaseOrderField = context.costSheetPOFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.PO_Id == _purchaseOrderId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (purchaseOrderField != null)
            {
                // context.costSheetPOFields.Remove(purchaseOrderField);
                purchaseOrderField.Value = addedSystemValue;
                context.SaveChanges();
            }
        }
        public void UpdateSIField(int _saleInvoiceId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedSystemValue)
        {
            var saleInvoiceField = context.costSheetSIFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.SI_Id == _saleInvoiceId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (saleInvoiceField != null)
            {
                // context.costSheetPOFields.Remove(purchaseOrderField);
                saleInvoiceField.Value = addedSystemValue;
                context.SaveChanges();
            }
        }
        public void UpdateReceiptField(int _ReceiptId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedSystemValue)
        {
            var ReceiptField = context.costSheetSaleReceiptFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Receipt_Id == _ReceiptId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (ReceiptField != null)
            {
                // context.costSheetPOFields.Remove(purchaseOrderField);
                ReceiptField.Value = addedSystemValue;
                context.SaveChanges();
            }
        }

        public void UpdatePaymentField(int _PaymentId, int _costSheetId, int fieldId, CostFieldType _fieldType, decimal addedSystemValue)
        {
            var PaymentField = context.costSheetPaymentFields.FirstOrDefault(x => x.CostSheetId == _costSheetId && x.Payment_Id == _PaymentId && x.FieldId == fieldId && x.FieldType == _fieldType);
            if (PaymentField != null)
            {
                // context.costSheetPOFields.Remove(purchaseOrderField);
                PaymentField.Value = addedSystemValue;
                context.SaveChanges();
            }
        }

        public SaleOrder GetSaleOrderbyCostSheetId(int _costSheetId)
        {
            return context.saleOrders.FirstOrDefault(x => x.CostSheet_Id == _costSheetId);
        }
        public decimal getCostSheetPOAmount(int purchaseOrder_Id)
        {
            var values = context.costSheetPOFields.Where(x => x.PO_Id == purchaseOrder_Id).ToList();
            if (values.Count != 0)
            {
                var value = values.Sum(x => x.Value);
                return value;
            }
            else
                return 0;
        }
        public decimal getCostSheetBillAmount(int bill_Id)
        {
            var value = context.costSheetPOFields.Where(x => x.PO_Id == bill_Id).Sum(x => x.Value);
            return value;
        }
        public void AddPQ(PQDocument pqDocument)
        {
            var value = context.PQDocuments.Add(pqDocument);
            context.SaveChanges();
        }
        public void UpdatePQ(PQDocument pqDocument)
        {
            PQDocument _pqDocument = context.PQDocuments.FirstOrDefault(x => x.Id == pqDocument.Id);
            _pqDocument.Name = pqDocument.Name;
            context.SaveChanges();
        }
        public PQDocument GetPQ(int id)
        {
            return context.PQDocuments.FirstOrDefault(x => x.Id == id);
        }
        public List<PQDocument> GetPQDocuments()
        {
            return context.PQDocuments.ToList();
        }
        public void AddST(ShippingTerm term)
        {
            var value = context.ShippingTerms.Add(term);
            context.SaveChanges();
        }
        public void UpdateST(ShippingTerm term)
        {
            ShippingTerm _pqDocument = context.ShippingTerms.FirstOrDefault(x => x.Id == term.Id);
            _pqDocument.Name = term.Name;
            context.SaveChanges();
        }
        public ShippingTerm GetST(int id)
        {
            return context.ShippingTerms.FirstOrDefault(x => x.Id == id);
        }
        public List<ShippingTerm> GetShippingTerms()
        {
            return context.ShippingTerms.ToList();
        }
        public decimal GetSOSystemCost(int _costSheetId)
        {
            var billFields = context.costSheetBillFields.Where(x => x.CostSheetId == _costSheetId).ToList();
            var poFields = context.costSheetPOFields.Where(x => x.CostSheetId == _costSheetId).ToList();
            var receiptFields = context.costSheetSaleReceiptFields.Where(x => x.CostSheetId == _costSheetId).ToList();
            var paymentFields = context.costSheetPaymentFields.Where(x => x.CostSheetId == _costSheetId).ToList();
            var siFields = context.costSheetSIFields.Where(x => x.CostSheetId == _costSheetId).ToList();

            var billFieldsSum = billFields.Select(x => x.Value).Sum();
            var poFieldsSum = poFields.Select(x => x.Value).Sum();
            var receriptFieldsSum = receiptFields.Select(x => x.Value).Sum();
            var paymentFieldsSum = paymentFields.Select(x => x.Value).Sum();
            var siFieldsSum = siFields.Select(x => x.Value).Sum();

            return billFieldsSum + poFieldsSum + receriptFieldsSum + paymentFieldsSum+ siFieldsSum;
        }
        public ProcurementProduct GetProcurementProduct(int procProduct_Id)
        {
            return context.procurementProducts.FirstOrDefault(x => x.Id == procProduct_Id);
        }
        public InquiryProduct GetInquiryProduct(int inqProduct_Id)
        {
            return context.inquiryProducts.FirstOrDefault(x => x.Id == inqProduct_Id);
        }
        public List<SaleOrder> GellAllbyOfferId(int offerId)
        {
            return context.saleOrders
                .Where(x => x.offer_Id == offerId).ToList();
        }  
        public List<SaleOrder> GellAllbyModuleContractId(int ModuleContractId)
        {
            return context.saleOrders
                .Where(x => x.moduleContract_Id == ModuleContractId).ToList();
        }
        public decimal GetAdjustmentCost(int saleOrderId, int costSheetId)
        {
            if (costSheetId != 0)
            {
                var costSheet = context.costSheets.FirstOrDefault(x => x.Id == costSheetId);

                return Math.Round(Convert.ToDecimal(costSheet.FieldValues.Where(x => x.Type == 35).Sum(x => x.adjSCost)), 2);
            }
            else
                return 0;
        }


        public int getAllPendingForApprovalDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
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
            return context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
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
            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForReApprovalDepartmentalCountByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwnByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
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

            var count = context.saleOrders
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
            return context.saleOrders

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
            return context.saleOrders

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
            return context.saleOrders
                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
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

            return context.saleOrders
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.saleOrders
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.saleOrders

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
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
            return context.saleOrders
                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }

        public int getAllPendingForReApprovalDepartmentalCountByCompanyDept(List<Company> compList, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.saleOrders

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountByCompanyDept(List<Company> compList, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }

            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.saleOrders

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwnByCompanyDept(List<Company> compList, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.saleOrders

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
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
            return context.saleOrders

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
            return context.saleOrders

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
            return context.saleOrders

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }
        public bool GetParentOrders(int id)
        {
            var orders = context.saleOrders.Where(x => x.ParentSO_Id == id).ToList();
            if (orders.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        public decimal GetPaymentSystemCost(int _costSheetId, int paymentId)
        {
            var fields = context.costSheetPaymentFields
                .Where(x => x.FieldType == CostFieldType.System_Payment && x.CostSheetId == _costSheetId && x.Payment_Id == paymentId).ToList();
            var value = fields.Sum(x => x.Value);
            return Convert.ToDecimal(value);
        }
        public CustomerCompany getParent(int id)
        {
            return (CustomerCompany)context.customerCompanies.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public Department getParentDepart(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public List<SaleOrder> GetOpenSaleInvoicesByCustomer(int Id,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders.Where(x => (deptIds.Contains(x.dept_Id) &&
            companyIds.Contains((int)x.company_Id)
            &&
            x.customerCompany_Id == Id
            && x.saleOrderStatus.isActive == true
            && x.isApproved == true
            && x.isVoid != true
            || (deptIds.Contains((int)x.InterDepartment_Id)
            && companyIds.Contains((int)x.InterCompany_Id)
            && x.isInterCompany == true))
            && x.saleOrderStatus.isActive == true
            && x.isApproved == true && x.isVoid != true).ToList();


        }
        public List<SaleOrder> GetCloseSaleInvoicesByCustomer(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders.Where(x =>
            deptIds.Contains(x.dept_Id) &&
            companyIds.Contains((int)x.company_Id) && x.customerCompany_Id == Id &&
            x.isVoid != true && x.saleOrderStatus.isActive == false
            && x.isApproved == true ||




            (deptIds.Contains((int)x.InterDepartment_Id) &&
            companyIds.Contains((int)x.InterCompany_Id) &&
            x.customerCompany_Id == Id &&
            x.isInterCompany == true && x.isVoid != true &&
            x.saleOrderStatus.isActive == false
            && x.isApproved == true)).ToList();


        }
        public List<PurchaseOrder> GetOpenPurchaseInvoicesByVendor(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == Id);
            return vendor.PurchaseOrders.Where(x => deptIds.Contains(x.dept_Id)
            && companyIds.Contains((int)x.company_Id)
            && x.isVoid != true && x.PurchaseOrderStatus.isActive
            == true && x.isApproved == true
            ).ToList();
        }
        public List<PurchaseOrder> GetOpenPurchaseInvoicesByVendor(int vendorId)
        {
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == vendorId);
            return vendor.PurchaseOrders.Where(x => x.isVoid != true && x.PurchaseOrderStatus.isActive== true && x.isApproved == true).ToList();
        }
        public List<PurchaseOrder> GetClosePurchaseInvoicesByVendor(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == Id);
            return vendor.PurchaseOrders.Where(x =>
            deptIds.Contains(x.dept_Id) && 
            companyIds.Contains((int)x.company_Id) && 
            x.isVoid != true && x.PurchaseOrderStatus.isActive == false
            && x.isApproved == true )
                .ToList();
        }
        public List<PurchaseOrder> GetClosePurchaseInvoicesByVendor(int vendorId)
        {
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == vendorId);
            return vendor.PurchaseOrders.Where(x =>
            x.isVoid != true && x.PurchaseOrderStatus.isActive == false
            && x.isApproved == true)
                .ToList();
        }
        public List<PurchaseOrder> GetAllPurchaseInvoicesByVendor(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == Id);
            return vendor.PurchaseOrders.Where(x =>
            (deptIds.Contains(x.dept_Id) && x.isVoid != true
            && companyIds.Contains((int)x.company_Id) 
                )).ToList();
        }
        public List<PurchaseOrder> GetAllPurchaseInvoicesByVendor(int vendorId)
        {
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == vendorId);
            return vendor.PurchaseOrders.Where(x =>
             x.isVoid != true
                ).ToList();
        }
        public List<SaleOrder> GetAllSaleInvoicesByCustomer(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders.Where(x =>
            (deptIds.Contains(x.dept_Id) && x.customerCompany_Id==Id
            && companyIds.Contains((int)x.company_Id)

            && x.isVoid != true

            || (deptIds.Contains((int)x.InterDepartment_Id)
            && companyIds.Contains((int)x.InterCompany_Id)
            && x.isInterCompany == true))
            && x.isVoid != true && x.customerCompany_Id == Id)
                .ToList();
        }
        public List<PurchaseOrder> GetPendingForApprovalPurchaseInvoicesByVendor(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == Id);
            return vendor.PurchaseOrders.Where(x =>
            (deptIds.Contains(x.dept_Id) && x.isVoid != true
            && companyIds.Contains((int)x.company_Id) && x.isApproved == false
                )).ToList();
        }
        public List<PurchaseOrder> GetPendingForApprovalPurchaseInvoicesByVendor(int vendorId)
        {
 
            var vendor = context.Vendors.FirstOrDefault(x => x.Id == vendorId);
            return vendor.PurchaseOrders.Where(x =>x.isVoid != true && x.isApproved == false).ToList();
        }
        public List<SaleOrder> GetPendingForApprovalSaleInvoicesByCustomer(int Id, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.saleOrders.Where(x =>
            (deptIds.Contains(x.dept_Id)
            && x.customerCompany_Id == Id
            && companyIds.Contains((int)x.company_Id)
            && x.isVoid != true
            && x.isApproved==false 
            ||
            (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id)
            && x.isInterCompany == true)) && x.isApproved == false
            && x.isVoid != true && x.customerCompany_Id == Id)
                .ToList();
        }
        public List<SaleOrder> GetSOBYBudgetId(int budgetId)
        {
            return context.saleOrders.Where(x => x.Budget_Id == budgetId).ToList();
                 
        }
        public List<PurchaseOrder> GetPOBYBudgetId(int budgetId)
        {
            return context.purchaseOrders.Where(x => x.Budget_Id == budgetId).ToList();

        }
        public List<SaleOrdeRrefKey> GetAllSORef(List<int>compIds, List<int>deptIds)
        {
            var pendingKeys = context.saleOrdeRrefKeys.AsEnumerable().Where(key =>{
            int keyId;
           if (int.TryParse(key.key, out keyId))
           {
               // Check if the count of related saleOrders is less than the IntegerValue
               return context.saleOrders.Count(so => so.ParentSO_Id == keyId) < key.SaleOrderNumber;
           }
           return false; // Exclude keys that cannot be parsed as integers
       }).ToList();
            pendingKeys = pendingKeys.Where(x => compIds.Contains(x.comp_Id) && deptIds.Contains(x.dept_Id)).ToList();

            return pendingKeys;
        }
        public SaleOrdeRrefKey GetSORef(int key)
        {
            return context.saleOrdeRrefKeys.FirstOrDefault(x =>x.key==key.ToString());

        }
        public bool checkKey(int key)
        {
            var saleOrders= context.saleOrders.Where(x => x.SaleOrderKey_Id == key).ToList();
            if(Convert.ToInt32(saleOrders.Count())==2)
            {
                return false;
            }
            else
            if (Convert.ToInt32(saleOrders.Count()) == 1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public List<ERP_BL.Procurements.StatusClass.StatusClass> GetActiveStatusClasses()
        {
            return context.statusClasses.Where(x => x.isActive == true && x.transactionType == Enums.TransactionItemType.Sale_Order).ToList();
        }

        public ERP_BL.Procurements.StatusClass.StatusClass GetStatusClass(int _id)
        {
            return context.statusClasses.FirstOrDefault(x => x.Id == _id);
        }
        public List<SaleOrder> getAllByCustomer(int uid, int customerId)
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
            return context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.customerCompany_Id==customerId || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true && x.customerCompany_Id == customerId)
                .ToList();
        }

        public List<SaleOrder> GetAllAuditYearAdjustments(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            return context.saleOrders
                .Where(x =>deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.auditYearAdjustment_Id!=null).ToList();
        } 
        public SaleOrdeRrefKey GetSaleOrdeRefKey(int uid)
        {
            return context.saleOrdeRrefKeys.FirstOrDefault(x => x.key == uid.ToString());
        } 
        public List<SaleOrdeRrefKey> GetAllSORefKeys()
        {
            return context.saleOrdeRrefKeys.ToList();
        }
        public SaleOrdeRrefKey GetSaleOrderRefKey(int uid)
        {
            return context.saleOrdeRrefKeys.FirstOrDefault(x => x.Id == uid);
        } 
        public void UpdateKey(SaleOrdeRrefKey _key)
        {
            var key=context.saleOrdeRrefKeys.FirstOrDefault(x => x.Id == _key.Id);
            key = _key;
            context.SaveChanges();
        }
        public List<SaleOrder> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.saleOrders
                .Where(x => deptIds.Contains(x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.isVoid != true
                && x.saleOrderStatus.isActive == true
                && x.isApproved == false
                && x.InterCompany_Id == null
                && x.InterDepartment_Id == null
                ||
                x.isInterCompany == true
                && deptIds.Contains(x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.isVoid != true
                && x.saleOrderStatus.isActive == true
                && x.isApproved == false
                && x.InterCompany_Id == null
                && x.InterDepartment_Id == null)
                .ToList();
        }
    }
    
}
