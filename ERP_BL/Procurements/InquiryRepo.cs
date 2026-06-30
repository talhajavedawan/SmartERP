using DevExpress.Data.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IInqueryRepo
    {
        void Add(Inquiry inquiry);
        void Add(Inquiry inquiry, InquiryProduct product);
        void addStatus(InquiryStatus status);
        void update(Inquiry inquiry);
        void update(Inquiry inquiry, Employee employee);
        void updateStatus(Inquiry inquiry, InquiryStatus status);
        void updateStatus(int inquiryId, InquiryStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int inqueryItemId, ItemType itemType);


        List<Inquiry> getAll();
        List<Inquiry> getAll(User user);
        List<Inquiry> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        Inquiry get(int inquiryId);


    }
    public class InquiryRepo : IInqueryRepo
    {

        DBContextERP context= new DBContextERP();


        /// <summary>
        /// Add Inquiry in database
        /// </summary>
        /// <param name="inquiry">Inquiery Object</param>
        public void Add(Inquiry inquiry)
        {
            context.inquiries.Add(inquiry);
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
        /// Add item to specified inquiry
        /// </summary>
        /// <param name="inquiry">Inquiery Object (main object in which Item is to be added)</param>
        /// <param name="inqueryItem">InquieryItem Object (item to be added)</param>
        public void Add(Inquiry inquiry, InquiryProduct product)
        {
            Inquiry inquirytoUpdate = context.inquiries.FirstOrDefault(x => x.Id == inquiry.Id);
            inquirytoUpdate.products.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// Add InquiryStatus in database
        /// </summary>
        /// <param name="status">InquieryStatus Object</param>
        public void addStatus(InquiryStatus status)
        {
            context.inquiryStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Inquiry
        /// </summary>
        /// <param name="inquiry"></param>
        public void update(Inquiry inquiry)
        {
            Inquiry inquirytoUpdate = context.inquiries.FirstOrDefault(x => x.Id == inquiry.Id);
            inquirytoUpdate = inquiry;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Inquiry and allocate it to new employee
        /// </summary>
        /// <param name="inquiry">Inquiery to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(Inquiry inquiry, Employee employee)
        {
            Inquiry inquirytoUpdate = context.inquiries.FirstOrDefault(x => x.Id == inquiry.Id);
            inquirytoUpdate.employee = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="inquiry"></param>
        /// <param name="status"></param>
        public void updateStatus(Inquiry inquiry, InquiryStatus status)
        {
            Inquiry inquirytoUpdate = context.inquiries.FirstOrDefault(x => x.Id == inquiry.Id);
            inquirytoUpdate.inquiryStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(InquiryStatus status)
        {
            InquiryStatus inquiryStatus = context.inquiryStatuses.FirstOrDefault(x => x.Id == status.Id);
            inquiryStatus = status;
            context.SaveChanges();
        }

        /// <summary>
        /// Change inquiry status based on Inquiry ID
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <param name="status"></param>
        public void updateStatus(int inquiryId, InquiryStatus status)
        {
            Inquiry inquirytoUpdate = context.inquiries.FirstOrDefault(x => x.Id == inquiryId);
            inquirytoUpdate.inquiryStatus = status;
            context.SaveChanges();
        }


        ///// <summary>
        ///// Update Item type of specified Item
        ///// </summary>
        ///// <param name="inqueryItem"></param>
        ///// <param name="itemType"></param>
        //public void upadteItemType(Item item, ItemType itemType)
        //{
        //    InqueryItem ItemtoUpdate = context.inqueryItems.FirstOrDefault(x => x.Id == inqueryItem.Id);
        //    ItemtoUpdate.itemType = itemType;
        //    context.SaveChanges();
        //}

        ///// <summary>
        ///// Update Item type of specified item ID
        ///// </summary>
        ///// <param name="inqueryItemId"></param>
        ///// <param name="itemType"></param>
        //public void upadteItemType(int inqueryItemId, ItemType itemType)
        //{
        //    InqueryItem ItemtoUpdate = context.inqueryItems.FirstOrDefault(x => x.Id == inqueryItemId);
        //    ItemtoUpdate.itemType = itemType;
        //    context.SaveChanges();
        //}

        /// <summary>
        /// Change Inquiry to Void
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isVoid"></param>
        public void setInquirytoVoid(int Id, bool isVoid)
        {
            Inquiry item= context.inquiries.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAll(int uid)
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
                return context.inquiries
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                ||
                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.inquiryStatus.isActive == true && x.isVoid != true && x.isApproved == true)
                )
               .ToList();
            }
            else
            {
                return context.inquiries
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
               .ToList();
            }
        }

        public List<Inquiry> getAllFirst(int uid)
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
                return context.inquiries
                    .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                    ||
                    (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.inquiryStatus.isActive == true && x.isVoid != true && x.isApproved == true)
                    )
                    .ToList();
            }
            else
            {
                return context.inquiries
                    .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                    .ToList();
            }
        }

        public List<Inquiry> getAllByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.inquiryStatus.isActive == true && x.isVoid != true && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries
               .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
               .ToList();
            }

        }
        //#Void
        /// <summary>
        /// Get all Void inquiries for user.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.inquiries
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void inquiries .
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getVoidRegisterAdministrator()
        {
            return context.inquiries
        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void inquiries .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.inquiries
    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void inquiries for user count. 
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
            return context.inquiries
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void inquiries own.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }

        /// <summary>
        /// Get all Inquiriess count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getinquiriessCountByStatusId(int uid, int statusId)
        {
            using (var _DbConetxt = new DBContextERP())
            {

                if (uid != 0)
                {
                    //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);

                    var user = _DbConetxt.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    foreach (var comp in user.employee.Companies)
                        companyIds.Add(comp.Id);
                    foreach (var dpt in user.employee.departments)
                        deptIds.Add(dpt.Id);
                    return _DbConetxt.inquiries

                        .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbConetxt.inquiries.Where(x => x.inquiryStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all Inquiriess count 
        /// </summary>
        /// <returns></returns>
        public int getinquiriessCount(int uid)
        {
            using (var _DbConetxt = new DBContextERP())
            {

                if (uid != 0)
                {
                    var user = _DbConetxt.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    foreach (var comp in user.employee.Companies)
                        companyIds.Add(comp.Id);
                    foreach (var dpt in user.employee.departments)
                        deptIds.Add(dpt.Id);
                    return context.inquiries

                        .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbConetxt.inquiries.Where(x => x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all inquiries by Status Id
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getInquiriesByStatusId(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getInquiriesByStatusIdFirst(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getInquiriesByStatusIdByDateRange(DateTime from, DateTime to, int uid, int statusId)
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
                return context.inquiries
                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries
                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.inquiryStatus.Id == statusId && x.isApproved == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all Closed inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id); return context.inquiries
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getAllInActiveByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing!=true && x.inquiryStatus.isActive == true && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getAllActiveFirst(int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && 
                deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                ||

                (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) &&
                deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                .ToList();
            }
        }
        public List<Inquiry> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

               .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
               .ToList();
            }

        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId|| x.user.employee.EmpId == user.employee.EmpId|| x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
            }
        }

        /// <summary>
        /// Get all inquiries.
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
            return context.inquiries


            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Get all Closed inquiries.
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
                deptIds.Add(dpt.Id); return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == false && x.isApproved == true)
                .Count();
        }


        /// <summary>
        /// Get Own inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && ( x.user.employee.EmpId == user.employee.EmpId ) && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalOwnByDateRange(DateTime from, DateTime to,int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
            }

        }



        /// <summary>
        /// Get all pending inquiries by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<Inquiry> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
            }
        }
        /// <summary>
        /// Get Count pending inquiries by Departments
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

            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get all pending inquiries by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries
   
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
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

                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                //||
                //(x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all pending inquiries by Departmental Count
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

            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all inquiries Pending For Approval Count.
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
            return context.inquiries
                
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get Own inquiries Pending For Approval Count.
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
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && ( x.user.employee.EmpId == user.employee.EmpId ) && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForAdministrator()
        {
            return context.inquiries

                .Where(x =>  x.isApproved == false).ToList();
        }
        public List<Inquiry> getAllPendingForAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false).ToList();
        }
        public List<Inquiry> getAllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.inquiries

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false).ToList();
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingInquiriesForApprovalAdministratorCount()
        {
            return context.inquiries
                
                .Where(x => x.isApproved == false).Count();
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries
 
                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.EmpId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get own pending inquiries by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inquiries
  
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        public List<Inquiry> getAllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
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
                return context.inquiries

                .Where(x => ((x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                ||
                (x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)                
                )
                .ToList();
            }
            else
            {
                return context.inquiries

                .Where(x => (x.CreationDate != null && RetrievalDate != null && x.CreationDate >= RetrievalDate) && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
            }
        }
        /// <summary>
        /// Get all Pending inquiries For Closing Count .
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
            return context.inquiries
                
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }
        /// <summary>
        /// Get all Pending inquiries For Closing Count .
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
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId ) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }
        /// <summary>
        /// Get all Pending for closing inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAllPendingForClosingAdministrator()
        {
            return context.inquiries

                .Where(x => x.PendingForClosing == true && x.isApproved == true).ToList();
        }
        public List<Inquiry> getAllPendingForClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.inquiries

                .Where(x => x.CreationDate >= previousMonthDate && x.PendingForClosing == true && x.isApproved == true).ToList();
        }
        public List<Inquiry> getAllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.inquiries

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.PendingForClosing == true && x.isApproved == true).ToList();
        }
        /// <summary>
        /// Get all inquiries Pending for Closing.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingInquiriesForClosingAdministratorCount()
        {
            return context.inquiries

                .Where(x => x.PendingForClosing == true && x.isApproved == true).Count();
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public List<Inquiry> getAll()
        {
            return context.inquiries

                .ToList();
        }


        public List<Inquiry> getAllTemp()
        {
            return context.inquiries

                .ToList();
        }
        /// <summary>
        /// Get all inquiries.
        /// </summary>
        /// <returns></returns>
        public EntityServerModeSource getAllQuick()
        {
            EntityServerModeSource ee = new EntityServerModeSource();
            ee.QueryableSource = context.inquiries
                 .Include("customerCompany.Company")
                 .Include("customerCompany.parentCompany.Company")
                 .Include("customerCompany.contactPerson")
                 .Include("products")
                 .Include("products.product")
                 .Include("InquiryStatus")
                 .Include("company")
                 .Include("company.parentCompany")
                 .Include("company.currency")
                 .Include("department")
                 .Include("department.customers")
                 .Include("department.parentDepartment")
                 .Include("company.Contact")
                 .Include("company.departments")
                 .Include("company.departments.parentDepartment")
                 .Include("customerCompany.company.currency")
                 .AsQueryable();

            return ee;
        }


        /// <summary>
        /// Get all Inquiries Status.
        /// </summary>
        /// <returns></returns>
        public List<InquiryStatus> getAllInquiryStatus()
        {
            return context.inquiryStatuses.ToList();
        }

        /// <summary>
        /// Get all Inquiries Status.
        /// </summary>
        /// <returns></returns>
        public List<InquiryStatus> getAllInactiveStatus()
        {
            List<InquiryStatus> statuses = new List<InquiryStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.inquiryStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all Inquiries Status.
        /// </summary>
        /// <returns></returns>
        public List<InquiryStatus> getAllActiveStatus()
        {
            List<InquiryStatus> statuses = new List<InquiryStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.inquiryStatuses.Where(x => x.isActive == true && x.isVoid != true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all Inquiries Status.
        /// </summary>
        ///  <param name="inquirystatusid"></param>
        /// <returns></returns>
        public InquiryStatus getstatus(int inquirystatusid)
        {
            return context.inquiryStatuses.FirstOrDefault(x => x.Id == inquirystatusid /*&& x.isVoid!=true*/);
        }


        /// <summary>
        /// Get all inquiries of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<Inquiry> getAll(Employee employee)
        {
            return context.inquiries.Where(x => x.allocation_Id == employee.EmpId).ToList();
        }

        /// <summary>
        /// Get all inquiries of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Inquiry> getAll(User user)
        {
            return context.inquiries.Where(x => x.user_Id == user.id).ToList();
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
        /// Get inquiry based on Inquiry ID.
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        public Inquiry get(int inquiryId)
        {
            return context.inquiries

                .FirstOrDefault(x => x.Id == inquiryId);
        }


        public int getAllPendingForApprovalDepartmentalCountByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllActiveCountByCompanies(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
          
            return context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
            .Count();

        }
        public int getAllInActiveCountByCompanies(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
         
            return context.inquiries.Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true).Count();
        }
        public int getAllPendingForApprovalCountByCompany(List<Company> compList, int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForApprovalCountOwnbyCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingCountByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }
        public int getAllPendingForClosingCountWithoutApproveByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
        }
        public int getAllPendingForClosingCountOwnbyCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }



        public int getAllPendingForApprovalDepartmentalCountByCompanyDept(List<Company> compList, List<Department> deptList)
        {
           // var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            return context.inquiries

                .Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllActiveCountByCompaniesDept(List<Company> compList, List<Department> deptList)
        {
           // var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.inquiries.Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
            .Count();

        }
        public int getAllInActiveCountByCompaniesDept(List<Company> compList, List<Department> deptList)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.inquiries.Where(x => x.inquiryStatus.isActive == false && departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true).Count();
        }
        public int getAllPendingForApprovalCountByCompanyDept(List<Company> compList, List<Department> deptList, int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForApprovalCountOwnbyCompanyDept(List<Company> compList, List<Department> deptList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries.Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingCountByCompanyDept(List<Company> compList, List<Department> deptList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }
        public int getAllPendingForClosingCountWithoutApproveByCompanyDept(List<Company> compList, List<Department> deptList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
        }
        public int getAllPendingForClosingCountOwnbyCompanyDept(List<Company> compList, List<Department> deptList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptList)
            {
                departmentId.Add(_dept.Id);
            }
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }
            return context.inquiries

                .Where(x => departmentId.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
        }
    }
}
