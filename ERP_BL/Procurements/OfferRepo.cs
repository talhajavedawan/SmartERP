using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IOfferRepo
    {
        void Add(Offer offer);
        void Add(Offer offer, ProcurementProduct product);
        void addStatus(OfferStatus status);
        void update(Offer offer);
        void update(Offer offer, Employee employee);
        void updateStatus(Offer offer, OfferStatus status);
        void updateStatus(int offerId, OfferStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int offerItemId, ItemType itemType);


        List<Offer> getAll();
        List<Offer> getAll(User user);
        List<Offer> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        Offer get(int offerId);
    }
    public class OfferRepo : IOfferRepo
    {


        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add Offer in database
        /// </summary>
        /// <param name="offer">Offer Object</param>
        public void Add(Offer offer)
        {

            context.offers.Add(offer);
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
        /// Add item to specified offer
        /// </summary>
        /// <param name="offer">Offer Object (main object in which Item is to be added)</param>
        /// <param name="offerItem">OfferItem Object (item to be added)</param>
        public void Add(Offer offer, ProcurementProduct product)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offer.Id);
            offertoUpdate.products.Add(product);
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
        /// Add OfferStatus in database
        /// </summary>
        /// <param name="status">OfferStatus Object</param>
        public void addStatus(OfferStatus status)
        {
            context.offerStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Offer
        /// </summary>
        /// <param name="offer"></param>
        public void update(Offer offer)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offer.Id);
            //foreach (var prod in offertoUpdate.products)
            //{
            //    if (prod.Id != 0)
            //    {
            //        context.inquiryProducts.Remove(context.inquiryProducts.FirstOrDefault(x => x.Id == prod.product_Id));
            //        context.procurementProducts.Remove(context.procurementProducts.FirstOrDefault(x => x.Id == prod.Id));
            //    }
            //}
            offertoUpdate = offer;
            context.SaveChanges();
        }
        public void updateFromGrid(Offer offer)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offer.Id);
            offertoUpdate = offer;
            offertoUpdate.offerStatus = context.offerStatuses.FirstOrDefault(x => x.Id == offer.offerStatus.Id);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Offer and allocate it to new employee
        /// </summary>
        /// <param name="offer">Offer to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(Offer offer, Employee employee)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offer.Id);
            offertoUpdate.employee = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change offer status
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="status"></param>
        public void updateStatus(Offer offer, OfferStatus status)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offer.Id);
            offertoUpdate.offerStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Get list of all Products in DB
        /// </summary>
        /// <returns>List of Products Objects</returns>
        public List<Product> getAllProducts()
        {
            return context.Products.ToList();

        }


        /// <summary>
        /// Change offer status based on Offer ID
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="status"></param>
        public void updateStatus(int offerId, OfferStatus status)
        {
            Offer offertoUpdate = context.offers.FirstOrDefault(x => x.Id == offerId);
            offertoUpdate.offerStatus = status;
            context.SaveChanges();
        }


        /// <summary>
        /// Change offer to Void
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isVoid"></param>
        public void setOffertoVoid(int Id, bool isVoid)
        {
            Offer item = context.offers.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.offerStatus.isActive == true && x.isVoid != true)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
            }

        }
        public List<Offer> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers

                .Where(x => (x.CreationDate >= RetrievalDate & (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate & (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                ||
                ((deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.offerStatus.isActive ==true)
                )
                .ToList();
            }
            else
            {
                return context.offers

                .Where(x => x.CreationDate >= RetrievalDate & (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate & (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }

        }
        public List<Offer> getAllByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {

                return context.offers

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.offerStatus.isActive == true && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                )
                .ToList();
            }
            else
            {
                return context.offers

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }
        }

        /// <summary>
        /// Get all offers.
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

            return context.offers

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }



        /// <summary>
        /// Get all Closed offers.
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

            return context.offers

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }


        //#Void
        /// <summary>
        /// Get all Void offers for user.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void offers .
        /// </summary>
        /// <returns></returns>
        public List<Offer> getVoidRegisterAdministrator()
        {
            return context.offers
            
        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void offers .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.offers
    
    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void offers for user count. 
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
            return context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void offers own.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.offers
       
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all pending offers by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
             
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<Offer> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
        
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<Offer> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.offerStatus.isActive == true && x.isVoid != true && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count pending offers by Departments
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

            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
        }


        /// <summary>
        /// Get all pending offers by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
      
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<Offer> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<Offer> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.offerStatus.isActive == true && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all pending offers by Departmental Count
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

            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }


        /// <summary>
        /// Get all offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingforApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
    
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<Offer> getAllPendingforApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
              
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<Offer> getAllPendingforApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.offerStatus.isActive == true && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
            }
        }
        /// <summary>
        /// Get all offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingforApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
            
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<Offer> getAllPendingforApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
       
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<Offer> getAllPendingforApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.offerStatus.isActive == true && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
            }

        }
        /// <summary>
        /// Get Count offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();

        }




        /// <summary>
        /// Get Count offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();

        }



        /// <summary>
        /// Get all offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingsforAdministrator()
        {
            return context.offers
          
                .Where(x => x.isApproved == false).ToList();
        }
        public List<Offer> getAllPendingsforAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
          
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false).ToList();
        }
        public List<Offer> getAllPendingsforAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.offers
      
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false).ToList();
        }
        /// <summary>
        /// Get Count of offers pending for approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingsforApprovalAdministratorCount()
        {
            return context.offers

                .Where(x => x.isApproved == false).Count();
        }
        /// <summary>
        /// Get all offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingforClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.offers
            
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<Offer> getAllPendingforClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
              
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<Offer> getAllPendingforClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x =>((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing == true)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all offers Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingforClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
               
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<Offer> getAllPendingforClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<Offer> getAllPendingforClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;

            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing == true)
                )
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count offers Pending for Approval .
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }

        /// <summary>
        /// Get Count offers Pending for Approval .
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingCountown(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }



        /// <summary>
        /// Get all pending for Closing offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllPendingforClosingAdministrator()
        {
            return context.offers

                .Where(x => x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        public List<Offer> getAllPendingforClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
         
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        public List<Offer> getAllPendingforClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.offers
              
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        /// <summary>
        /// Get Count pending for Closing offers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingAdministratorCount()
        {
            return context.offers

                .Where(x => x.isApproved == true && x.PendingForClosing == true).Count();
        }
        /// <summary>
        /// Get all Closed offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
    
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<Offer> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => x.CreationDate >= RetrievalDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => x.CreationDate >= RetrievalDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }

        }
        public List<Offer> getAllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                )
                .ToList();
            }
            else
            {
                return context.offers

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
  
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<Offer> getAllActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {

                return context.offers

                .Where(x => x.CreationDate >= RetrievalDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }
            else
            {
                return context.offers

                .Where(x => x.CreationDate >= RetrievalDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= RetrievalDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }

        }
        public List<Offer> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {

                return context.offers

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }
            else
            {
                return context.offers

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();
            }

        }
        /// <summary>
        /// Get all offers count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getOffersCountByStatusId(int uid, int statusId)
        {
            using (var _DbContext = new DBContextERP())
            {
                if (uid != 0)
                {
                    var user = _DbContext.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    //foreach (var comp in user.employee.Companies)
                    for (int i = 0; i < user.employee.Companies.Count(); i++)
                        companyIds.Add(user.employee.Companies[i].Id);

                    //foreach (var dpt in user.employee.departments)
                    for (int j = 0; j < user.employee.departments.Count; j++)
                        deptIds.Add(user.employee.departments[j].Id);

                    return _DbContext.offers
                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.offers.Where(x => x.offerStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all offers count 
        /// </summary>
        /// <returns></returns>
        public int getOffersCount(int uid)
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
                    return context.offers

                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.offers.Where(x => x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all offers by Status Id.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAllOffersByStatusId(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.offers
               
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.Id == statusId && x.isApproved == true)
                .ToList();

        }
        public List<Offer> getAllOffersByStatusIdFirst(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.offers
             
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.Id == statusId && x.isApproved == true)
                .ToList();

        }
        public List<Offer> getAllOffersByStatusIdByDateRange(DateTime from, DateTime to, int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.Id == statusId && x.isApproved == true))
                .ToList();
            }
            else
            {
                return context.offers
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.Id == statusId && x.isApproved == true))
                .ToList();
            }
        }
        /// <summary>
        /// 
        /// Get all offers.
        /// </summary>
        /// <returns></returns>
        public List<Offer> getAll()
        {
            return context.offers
 
                .ToList();
        }
        /// <summary>
        /// Get all Offers Status.
        /// </summary>
        /// <returns></returns>
        public List<OfferStatus> getAllOfferStatus()
        {
            return context.offerStatuses.ToList();
        }
        /// <summary>
        /// Get all Active Offers Status.
        /// </summary>
        /// <returns></returns>
        public List<OfferStatus> getAllActiveStatus()
        {
            List<OfferStatus> statuses = new List<OfferStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.offerStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;

        }/// <summary>
         /// Get all Inactive Offers Status.
         /// </summary>
         /// <returns></returns>
        public List<OfferStatus> getAllInactiveStatus()
        {
            List<OfferStatus> statuses = new List<OfferStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.offerStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all Offers Status.
        /// </summary>
        ///  <param name="offerstatusid"></param>
        /// <returns></returns>
        public OfferStatus getstatus(int offerstatusid)
        {
            return context.offerStatuses.FirstOrDefault(x => x.Id == offerstatusid);
        }


        /// <summary>
        /// Get all offers of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<Offer> getAll(Employee employee)
        {
            return context.offers.Where(x => x.allocation_Id == employee.EmpId).ToList();
        }

        /// <summary>
        /// Get all offers of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Offer> getAll(User user)
        {
            return context.offers.Where(x => x.user_Id == user.id).ToList();
        }
        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(OfferStatus status)
        {
            OfferStatus offerStatus = context.offerStatuses.FirstOrDefault(x => x.Id == status.Id);
            offerStatus = status;
            context.SaveChanges();
        }

        //}

        /// <summary>
        /// Get offer based on Offer ID.
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public Offer get(int offerId)
        {
            return context.offers




            .FirstOrDefault(x => x.Id == offerId);
        }
        public List<Offer> getAllByInquiryId(int inquiryId)
        {
            return context.offers
                .Where(x => x.inquiry_Id == inquiryId)
                .ToList();
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
            return context.offers
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
            return context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.offers

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
            return context.offers
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

            var count = context.offers
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
            return context.offers

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
            return context.offers

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
            return context.offers
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

            return context.offers
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.offers
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.offers

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
            return context.offers
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
            return context.offers

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
            return context.offers

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
            return context.offers

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }
        public void AddCompatativeStatement(ComparativeStatement statement)
        {
            context.comparativeStatements.Add(statement);
            context.SaveChanges();
        }
        public void UpdateCompatativeStatement(ComparativeStatement statement)
        {
            var dbStatement = context.comparativeStatements.FirstOrDefault(x => x.Id == statement.Id);
            dbStatement = statement;
            context.SaveChanges();
        }
        public ComparativeStatement GetCompatativeStatementById(int id)
        {
            return context.comparativeStatements
               .FirstOrDefault(x => x.Id == id);

        }
        public ComparativeStatementItem GetCompatativeStatementItemById(int id)
        {
            return context.comparativeStatementItems.FirstOrDefault(x => x.Id == id);

        }
        public Offer GetForComparativeStatement(int offerId)
        {
            return context.offers
           
            .FirstOrDefault(x => x.Id == offerId);
        }
        public FOCSampling GetFOCSampling(int _Id)
        {
            return context.fOCSamplings

            .FirstOrDefault(x => x.Id == _Id);
        }
        public PassOn GetPassOn(int _Id)
        {
            return context.passOns

            .FirstOrDefault(x => x.Id == _Id);
        }
        public ClaimDiscount GetClaimDiscount(int _Id)
        {
            return context.claimDiscounts

            .FirstOrDefault(x => x.Id == _Id);
        }
        public ClaimDiscount GetClaimDiscountByName(string _name)
        {
            return context.claimDiscounts

            .FirstOrDefault(x => x.discountName == _name);
        }
        public void AddFOCSampling(FOCSampling sampl)
        {
             context.fOCSamplings

            .Add(sampl);
            context.SaveChanges();
        }
        public void AddPassOn(PassOn passOn)
        {
            context.passOns

           .Add(passOn);
            context.SaveChanges();
        }
        public void  AddClaimDiscount(ClaimDiscount claim)
        {
             context.claimDiscounts
            .Add(claim);
            context.SaveChanges();
        }
        public void UpdateFOCSampling(FOCSampling sampl)
        {

            var dbSamp = context.fOCSamplings.FirstOrDefault(x => x.Id == sampl.Id);
            dbSamp = sampl;
            context.SaveChanges();
        }
        public void UpdatePassOn(PassOn passOn)
        {

            var dbSamp = context.passOns.FirstOrDefault(x => x.Id == passOn.Id);
            dbSamp = passOn;
            context.SaveChanges();
        }
        public void UpdateClaimDiscount(ClaimDiscount claim)
        {
            var dbClaim=context.claimDiscounts.FirstOrDefault(x=>x.Id==claim.Id);
            dbClaim = claim;
            context.SaveChanges();
        }
        public List<FOCSampling> GetAllActiveFOCSamplings()
        {
            return context.fOCSamplings.Where(x => x.isActive == true).ToList();
        }
        public List<ClaimDiscount> GetAllActiveClaimDiscounts()
        {
            return context.claimDiscounts.Where(x => x.isActive == true).ToList();
          
        }
        public List<FOCSampling> GetAllFOCSamplings()
        {
            return context.fOCSamplings/*.Where(x => x.isActive == true)*/.ToList();
        }
        public List<ClaimDiscount> GetAllClaimDiscounts()
        {
            return context.claimDiscounts/*.Where(x => x.isActive == true)*/.ToList();

        }
        public List<PassOn> GetAllPassOns()
        {
            return context.passOns/*.Where(x => x.isActive == true)*/.ToList();

        }
     
        public BookerStatementItem GetBookerStatement(int bookerId)
        {
            return context.bookerStatementItems.FirstOrDefault(x => x.Id == bookerId);

        }
        public FOCSampling GetFOCSamplingByName(string _name)
        {
            return context.fOCSamplings

            .FirstOrDefault(x => x.samplingtName == _name);
        }
        public PassOn GetPassOnByName(string _name)
        {
            return context.passOns

            .FirstOrDefault(x => x.passOnName == _name);
        }
        public void AddUniqueNUmber(UniqueNumber uniqueNumber)
        {
            context.uniqueNumbers

           .Add(uniqueNumber);
            context.SaveChanges();
        }
        public void UpdateUniqueNumber(UniqueNumber uniqueNumber)
        {

            var dbSamp = context.uniqueNumbers.FirstOrDefault(x => x.Id == uniqueNumber.Id);
            dbSamp = uniqueNumber;
            context.SaveChanges();
        }
        public UniqueNumber GetUniqueNumber(int _Id)
        {
            return context.uniqueNumbers

            .FirstOrDefault(x => x.Id == _Id);
        }
        public List<UniqueNumber> GetAllUniqueNumbers()
        {
            return context.uniqueNumbers/*.Where(x => x.isActive == true)*/.ToList();
        }
        public List<UniqueNumber> GetAllActiveUniqueNumbers()
        {
            return context.uniqueNumbers.Where(x => x.isActive == true).ToList();
        }
        public List<Offer> getFirstOfferRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.offers

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)

                .ToList();
        }
        public List<Offer> getOfferRegisterAdministrator()
        {
            return context.offers

                    .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid != true)
                    .ToList();
        }
        public int getOfferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .Count();
        }
        public int getOfferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }
        public List<Offer> OfferRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);


            DateTime? RetrievalDate = null;

            if (user.employee.InquiryDataRetrievalDate != null)
                RetrievalDate = user.employee.InquiryDataRetrievalDate;


            if (user.employee.AllowOpenTransactions == true)
            {
                return context.offers
        .OrderByDescending(x => x.CreationDate)
            .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
            ||
            (x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.offerStatus.isActive == true && x.isVoid != true)
            )
            .ToList();
            }
            else
            {
                return context.offers
        .OrderByDescending(x => x.CreationDate)
            .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
            .ToList();
            }

        }
    }
}
