using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IPurchaseOrderRepo
    {
        void Add(PurchaseOrder purchaseOrder);
        void Add(PurchaseOrder purchaseOrder, ProcurementProduct product);
        void addStatus(PurchaseOrderStatus status);
        void update(PurchaseOrder purchaseOrder);
        void update(PurchaseOrder purchaseOrder, Employee employee);
        void updateStatus(PurchaseOrder purchaseOrder, PurchaseOrderStatus status);
        void updateStatus(int purchaseOrderId, PurchaseOrderStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int purchaseOrderItemId, ItemType itemType);


        List<PurchaseOrder> getAll();
        List<PurchaseOrder> getAll(User user);
        List<PurchaseOrder> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        PurchaseOrder get(int purchaseOrderId);
    }
    public class PurchaseOrderRepo //: IPurchaseOrderRepo
    {

        DBContextERP context = new DBContextERP();
        //static DBContextERP context;

        //public PurchaseOrderRepo()
        //{

        //    if (context == null)
        //    {
        //        context = new DBContextERP();
        //    }
        //    //        //return dbContext;

        //}

        /// <summary>
        /// Add PurchaseOrder in database
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrder Object</param>
        public void Add(PurchaseOrder purchaseOrder)
        {

            context.purchaseOrders.Add(purchaseOrder);
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
        /// <param name="purchaseOrder">PurchaseOrder Object (main object in which Item is to be added)</param>
        /// <param name="purchaseOrderItem">PurchaseOrderItem Object (item to be added)</param>
        public void Add(PurchaseOrder purchaseOrder, ProcurementProduct product)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.products.Add(product);
            context.SaveChanges();
        }


        /// <summary>
        /// Update SaleOrder
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void Approve(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.isApproved = purchaseOrder.isApproved;
            purchaseOrdertoUpdate.stage = purchaseOrder.stage;
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
        /// Add PurchaseOrderStatus in database
        /// </summary>
        /// <param name="status">PurchaseOrderStatus Object</param>
        public void addStatus(PurchaseOrderStatus status)
        {
            context.purchaseOrderStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update PurchaseOrder
        /// </summary>
        /// <param name="purchaseOrder"></param>
        public void update(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            //foreach (var prod in purchaseOrdertoUpdate.products)
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
        public void updatePO(PurchaseOrder purchaseOrder)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.transactionHolderId = purchaseOrder.transactionHolderId;
            purchaseOrdertoUpdate.holderChangeDate = purchaseOrder.holderChangeDate;
            context.SaveChanges();
        }
        public void updateForCostSheet(int costSheetId, int costSheetFieldId, Vendor vendor, Currency oc, string maker, string origin, DateTime? deliveryDate, PaymentTerm paymentTerm, Warranty warranty, Incoterm incoTerm)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.CostSheetFieldId == costSheetFieldId && x.CostSheet_Id == costSheetId);
            if (purchaseOrdertoUpdate != null)
            {
                if (costSheetId != null)
                {
                    purchaseOrdertoUpdate.CostSheet_Id = costSheetId;

                }
                if (costSheetFieldId != null)
                {
                    purchaseOrdertoUpdate.CostSheetFieldId = costSheetFieldId;

                }
                if (vendor != null)
                {
                    var dbVendor = context.Vendors.FirstOrDefault(x => x.Id == vendor.Id);

                    purchaseOrdertoUpdate.vendors.Clear();
                    purchaseOrdertoUpdate.vendors.Add(dbVendor);
                }
                if (oc != null)
                {
                    purchaseOrdertoUpdate.currency_Id = oc.Id;
                }
                if (maker != null)
                {
                    purchaseOrdertoUpdate.maker = maker;

                }
                if (origin != null)
                {
                    purchaseOrdertoUpdate.origin = origin;

                }
                if (deliveryDate != null)
                {
                    purchaseOrdertoUpdate.DeliveryDate = deliveryDate;

                }
                if (paymentTerm != null)
                {
                    purchaseOrdertoUpdate.POPaymentterm_Id = paymentTerm.Id;

                }
                if (warranty != null)
                {
                    purchaseOrdertoUpdate.POWarrantyId = warranty.Id;

                }
                if (incoTerm.Id != 0)
                {
                    
                    purchaseOrdertoUpdate.incoterm_Id = incoTerm.Id;

                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all Active purchaseOrders.
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
            return context.purchaseOrders

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get all Inactive purchaseOrders.
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
            return context.purchaseOrders

            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Update PurchaseOrder and allocate it to new employee
        /// </summary>
        /// <param name="purchaseOrder">PurchaseOrder to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(PurchaseOrder purchaseOrder, Employee employee)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.AllocateTo = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(PurchaseOrderStatus status)
        {
            PurchaseOrderStatus poStatus = context.purchaseOrderStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="status"></param>
        public void updateStatus(PurchaseOrder purchaseOrder, PurchaseOrderStatus status)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrder.Id);
            purchaseOrdertoUpdate.PurchaseOrderStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status based on PurchaseOrder ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="status"></param>
        public void setSotoVoid(int SoId, bool isVoid)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == SoId);
            purchaseOrdertoUpdate.isVoid = isVoid;
            var poFields = context.costSheetPOFields.Where(x => x.PO_Id == SoId).ToList();
            if (poFields != null)
                context.costSheetPOFields.RemoveRange(poFields);
            context.SaveChanges();
        }

        /// <summary>
        /// Change purchaseOrder status based on PurchaseOrder ID
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="status"></param>
        public void updateStatus(int purchaseOrderId, PurchaseOrderStatus status)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == purchaseOrderId);
            purchaseOrdertoUpdate.PurchaseOrderStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change purchaseOrder status based on PurchaseOrder ID
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <param name="status"></param>
        public void updateStatusById(int saleOrderId, int statusId)
        {
            PurchaseOrder purchaseOrdertoUpdate = context.purchaseOrders.FirstOrDefault(x => x.Id == saleOrderId);
            var POStatus = context.purchaseOrderStatuses.FirstOrDefault(x => x.Id == statusId);
            purchaseOrdertoUpdate.PurchaseOrderStatus = POStatus;
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
        /// Get all purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders
                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
        }
        public List<PurchaseOrder> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders
                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) 
                && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate)
                && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true))
                .ToList();
            }
        }


        public List<PurchaseOrder> getAllByDateRange(DateTime from,DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x =>  ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
               .ToList();
            }
        }
        /// <summary>
        /// Get all Inactive purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                
         
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders
                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) 
                && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true) )
                .ToList();
            }
            else
            {
                return context.purchaseOrders
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
        }

        public List<PurchaseOrder> getAllInActiveByDateRange(DateTime from, DateTime to ,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) 
                && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                //||
                //(x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) 
                && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all Active purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
        }
        public List<PurchaseOrder> getAllActiveByDateRange(DateTime from,DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
               .ToList();
            }
        }
        /// <summary>
        /// Get all Inactive purchaseOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllInActiveandUnapproved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Active purchaseOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllActiveandUnapporved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all purchaseOrders for user.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getPurchaseRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x =>  (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getPurchaseRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && 
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true
                ||
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
               .ToList();
            }
        }

        public List<PurchaseOrder> getPurchaseRegisterByDateRange(DateTime from, DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                ||
                ((x.CreationDate != null && x.CreationDate >= from && x.CreationDate <= to) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                ((x.CreationDate != null && x.CreationDate >= from && x.CreationDate <= to) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isVoid != true)
                )
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all purchaseOrders .
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getPurchaseRegisterAdministrator()
        {
            return context.purchaseOrders

                .Where(x => x.isVoid != true)
   
                .ToList();
        }
        public List<PurchaseOrder> getPurchaseRegisterAdministratorFirst()
        {
            return context.purchaseOrders

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid != true)

                .ToList();
        }


        public List<PurchaseOrder> getPurchaseRegisterAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseOrders

    .Where(x => x.CreationDate>=from && x.CreationDate<=to && x.isVoid != true)
    .ToList();
        }
        /// <summary>
        /// Get all purchaseOrders .
        /// </summary>
        /// <returns></returns>
        public int getPurchaseRegisterAdministratorCount()
        {
            return context.purchaseOrders

    .Where(x => x.isVoid != true)
    .Count();
        }
        /// <summary>
        /// Get all purchaseOrders for user count. 
        /// </summary>
        /// <returns></returns>
        public int getPurchaseRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all purchaseOrders own.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getPurchaseRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .ToList();
        }

        //#Void
        /// <summary>
        /// Get all Void purchaseOrders for user.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void purchaseOrders .
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getVoidRegisterAdministrator()
        {
            return context.purchaseOrders

        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void purchaseOrders .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.purchaseOrders

    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void purchaseOrders for user count. 
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
            return context.purchaseOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void purchaseOrders own.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }

        /// <summary>
        /// Get all purchaseOrders  own count. 
        /// </summary>
        /// <returns></returns>
        public int getPurchaseRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPendingForReApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true) 
                ||
                (
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
               x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
               x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
               x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
               .ToList();
            }

        }
        /// <summary>
        /// Get all pending purchaseOrders by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
          
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForReApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count pending purchaseOrders by Departments
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
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own pending purchaseOrders by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
    
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }


        public List<PurchaseOrder> getAllPendingForReApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x =>( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                ||
                 x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isReApproved == false && x.isVoid != true && x.isApproved == true
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count pending purchaseOrders by user Id and <paramref name="UserID"/>.
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
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending purchaseOrders user Id and <paramref name="UserID"/>.
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
            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }




        /// <summary>
        /// Get all pending purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
 
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
              (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
              (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
              (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
              (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
               .ToList();
            }
        }


        public List<PurchaseOrder> getAllPendingForApprovalByDateRange(DateTime from,DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true)

                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
            }
        }



        /// <summary>
        /// Get all pending purchaseOrders by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForApprovalDepartmental( int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
               ||
               ( (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
              (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true)
               )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
            }
        }



        public List<PurchaseOrder> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count pending purchaseOrders by Departments
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending purchaseOrders by Departmental
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true
               ||
               (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true
               )
                .ToList();
            }
            else
            {
                return context.purchaseOrders
                   .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                   (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                   .ToList();

            }
        }



        public List<PurchaseOrder> getAllPendingForClosingDepartmentalByDateRange( DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true
                ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
               .ToList();
            }
        }
        /// <summary>
        /// Get all pending purchaseOrders by Departmental Count
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own pending purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true
                ||
                 (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
            }
        }



        public List<PurchaseOrder> getAllPendingForApprovalOwnByDateRange(DateTime from, DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true
                ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isApproved == false && x.isVoid != true
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
               (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
               .ToList();
            }
        }
        /// <summary>
        /// Get Count pending purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending purchaseOrders
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
        
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive ==true && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ||
                ( (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                x.user.employee.EmpId == user.employee.EmpId ||
                x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
        }


        public List<PurchaseOrder> getAllPendingForClosingByDateRange(DateTime from, DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => ( (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get own pending purchaseOrders by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
 
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.PODataRetrievalDate != null)
                RetrievalDate = user.employee.PODataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true
                ||
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PurchaseOrderStatus.isActive == true && x.PendingForClosing == true && x.isVoid != true
                )
                .ToList();
            }
            else
            {
                return context.purchaseOrders

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
            }
        }

        public List<PurchaseOrder> getAllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
                .Where(x => x.CreationDate>=from && x.CreationDate<=to &&(deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get Count pending for closing purchaseOrders by supervisor Id and <paramref name="StatusId"/>.
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count pending for closing purchaseOrders own <paramref name="StatusId"/>.
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

            return context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all purchaseOrders count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getpurchaseOrdersCountByStatusId(int uid, int statusId)
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

                    return _DbContext.purchaseOrders

                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.Id == statusId && x.isApproved == true && x.isVoid != true)
                        .Count();
                }
                else
                    return _DbContext.purchaseOrders.Where(x => x.PurchaseOrderStatus.Id == statusId && x.isApproved == true && x.isVoid != true).Count();
            }
        }

        /// <summary>
        /// Get all Active purchaseOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getUserSOByYear(int userId, int year)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();


            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Active purchaseOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getDepartmentalSOByYear(int departmentId, int companyId, int year)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.purchaseOrders
     
                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Active purchaseOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getDepartmentalSOByMonth(int departmentId, int companyId, int year, int month)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.purchaseOrders
   
                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.CreationDate.Value.Month == month && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all purchaseOrders count
        /// </summary>
        /// <returns></returns>
        public int getpurchaseOrdersCount(int uid)
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

                return context.purchaseOrders

                    .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                    .Count();
            }
            else
                return context.purchaseOrders.Where(x => x.isApproved == true).Count();

        }
        /// <summary>
        /// Get all purchaseOrders by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
     
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPobyStatusIdFirst(int uid, int StatusId)
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
       
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.purchaseOrders
      
                .Where(x => x.CreationDate>=from && x.CreationDate<=to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
                x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForAdministrator()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseOrders

                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<PurchaseOrder> getAllPendingForAdministratorFirst()
        {
            return context.purchaseOrders

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isApproved == false && x.isVoid != true)
                .ToList();
        }


        public List<PurchaseOrder> getAllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseOrders

                .Where(x => x.CreationDate>=from && x.CreationDate<=to && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        /// Get Count purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.purchaseOrders

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.purchaseOrders

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAllPendingForClosingAdministrator()
        {
            return context.purchaseOrders

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPendingForClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.purchaseOrders
    
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        public List<PurchaseOrder> getAllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.purchaseOrders
       
                .Where (x =>x.CreationDate>=from&& x.CreationDate<=to && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all purchaseOrders.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrder> getAll()
        {
            return context.purchaseOrders
       
            .Where(x => x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrderStatus> getAllPurchaseOrderStatus()
        {
            return context.purchaseOrderStatuses.ToList();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrderStatus> getAllActivePurchaseOrderStatus()
        {
            List<PurchaseOrderStatus> statuses = new List<PurchaseOrderStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.purchaseOrderStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<PurchaseOrderStatus> getAllInActivePurchaseOrderStatus()
        {
            List<PurchaseOrderStatus> statuses = new List<PurchaseOrderStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.purchaseOrderStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        ///  <param name="purchaseOrderstatusid"></param>
        /// <returns></returns>
        public PurchaseOrderStatus getstatus(int purchaseOrderstatusid)
        {
            return context.purchaseOrderStatuses.FirstOrDefault(x => x.Id == purchaseOrderstatusid);
        }
        /// <summary>
        /// Get all purchaseOrders of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<PurchaseOrder> getAll(Employee employee)
        {
            return context.purchaseOrders.Where(x => x.allocation_Id == employee.EmpId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get all purchaseOrders of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<PurchaseOrder> getAll(User user)
        {
            return context.purchaseOrders.Where(x => x.user_Id == user.id && x.isVoid != true).ToList();
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
        public PurchaseOrder get(string SoRef)
        {
            return context.purchaseOrders.FirstOrDefault(x => x.SalesReferenceNo == SoRef);
        }

        /// <summary>
        /// Get purchaseOrder based on PurchaseOrder ID.
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        /// 

        public PurchaseOrder get(int costSheetId, int costSheetField)
        {
            return context.purchaseOrders

            .FirstOrDefault(x => x.CostSheet_Id == costSheetId && x.CostSheetFieldId == costSheetField && x.isVoid != true);
        }


        public PurchaseOrder getForGrid(int purchaseOrderId)
        {
            return context.purchaseOrders                                            
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public PurchaseOrder getForPI(int purchaseOrderId)
        {
            return context.purchaseOrders
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }

        public PurchaseOrder get(int purchaseOrderId)
        {
            return context.purchaseOrders

            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }
       
        /// <summary>
        /// Get all purchaseOrders Allocated to an Employee <para>EmployeeId</para>.
        /// </summary>
        /// <returns></returns>
        /// 
        public List<PurchaseOrder> getAllAllcatedtoEmployee(int EmployeeId)
        {
            return context.purchaseOrders
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
        public int getTodayPOCount()
        {
            return context.purchaseOrders.Where(x => x.isVoid != true && EntityFunctions.TruncateTime(x.CreationDate) == DateTime.Today)
                .Count();
        }
        public PurchaseOrder GetForCostsheet( int purchaseOrderId)
        {
            return context.purchaseOrders
            .FirstOrDefault(x => x.Id == purchaseOrderId);
        }
          public CustomerCompany getParent(int id)
        {
            return (CustomerCompany)context.customerCompanies.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public Department getParentDepart(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public List<ERP_BL.Procurements.StatusClass.StatusClass> GetActiveStatusClasses()
        {
            return context.statusClasses.Where(x => x.isActive == true && x.transactionType == Enums.TransactionItemType.Purchase_Order).ToList();
        }
        public ERP_BL.Procurements.StatusClass.StatusClass GetStatusClass(int _id)
        {
            return context.statusClasses.FirstOrDefault(x => x.Id == _id);
        }
        public List<PurchaseOrder> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.purchaseOrders
                .Where(x => deptIds.Contains(x.dept_Id) 
                && companyIds.Contains((int)x.company_Id)
                && x.InterCompany_Id == null
                && x.InterDepartment_Id == null
               
                && x.isVoid != true
                && x.PurchaseOrderStatus.isActive == true
                || x.PurchaseOrderStatus.isActive == true 
                && 
                 deptIds.Contains(x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.InterCompany_Id==null
                && x.InterDepartment_Id==null
                && x.isVoid != true).ToList();
        }

    }
}
