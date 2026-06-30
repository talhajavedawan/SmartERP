using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Bill;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IBillRepo
    {
        void Add(Bill bill);
        void Add(Bill bill, ProcurementProduct product);
        void addStatus(BillStatus status);
        void update(Bill bill);
        void update(Bill bill, Employee employee);
        void updateStatus(Bill bill, BillStatus status);
        void updateStatus(int billId, BillStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int billItemId, ItemType itemType);


        List<Bill> getAll();
        List<Bill> getAll(User user);
        List<Bill> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        Bill get(int billId);
    }
    public class BillRepo //: IBillRepo
    {

        DBContextERP context = new DBContextERP();



        public void AddBillCategory(BillCategory billCategory)
        {
            context.billCategories.Add(billCategory);
            context.SaveChanges();
        }

        public void UpdateBillCategory(BillCategory billCategory)
        {
            var _billCategory = context.billCategories.FirstOrDefault(x => x.Id == billCategory.Id);
            _billCategory.Category = billCategory.Category;
            _billCategory.isActive = billCategory.isActive;
            //context.facilityNatures.Add(_facilityNature);
            context.SaveChanges();
        }

        public void AddAdjustments(List<VendorBillAdjustment> adjustments)
        {
            foreach (var _adj in adjustments)
            {
                if (_adj.Id == 0)
                    context.vendorBillAdjustments.Add(_adj);
            }

            foreach (var entry in context.ChangeTracker.Entries())
            {
                Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
            }

            context.SaveChanges();
        }

        public List<VendorBillAdjustment> GetAdjustmentsByBillId(int billId)
        {
            return context.vendorBillAdjustments.Where(x => x.bill_Id == billId).ToList();
        }

        public BillCategory GetBillCategory(int categoryId)
        {
            return context.billCategories
                .FirstOrDefault(x => x.Id == categoryId);
        }

        public List<BillCategory> GetAllBillCategories()
        {
            return context.billCategories.ToList();
        }



        /// <summary>
        /// Add Bill in database
        /// </summary>
        /// <param name="bill">Bill Object</param>
        public void Add(Bill bill)
        {


            context.bills.Add(bill);
            context.SaveChanges();
        }

        /// <summary>
        /// Add item to specified bill
        /// </summary>
        /// <param name="bill">Bill Object (main object in which Item is to be added)</param>
        /// <param name="billItem">BillItem Object (item to be added)</param>
        public void Add(Bill bill, ProcurementProduct product)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == bill.Id);
            billtoUpdate.products.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// get vendorCompany matching to ID
        /// </summary>
        /// <param name="vendorCompID">Vendor Company ID</param>
        /// <returns></returns>
        public Vendor getVendor(int vendorCompID)
        {
            return context.Vendors
     
                .FirstOrDefault(x => x.Id == vendorCompID);
        }
        /// <summary>
        /// Add BillStatus in database
        /// </summary>
        /// <param name="status">BillStatus Object</param>
        public void addStatus(BillStatus status)
        {
            context.billStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Get all Active bills.
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
            return context.bills

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Get all Inactive bills.
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
            return context.bills

            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Update Bill
        /// </summary>
        /// <param name="bill"></param>
        public void update(Bill bill)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == bill.Id);
            var JournalTransactions = context.journalTransactions.Where(x => x.Bill_Id == billtoUpdate.Id).ToList();
            if (JournalTransactions.Count > 0)
                context.journalTransactions.RemoveRange(JournalTransactions);
            context.VATBooks.RemoveRange(context.VATBooks.Where(x => x.vendorBillId == bill.Id));

            billtoUpdate = bill;

            if (bill.pettyCashes != null)
            {
                var alltransactions = context.pettyCashes.Where(x => x.billId == bill.Id).ToList();
                context.pettyCashes.RemoveRange(alltransactions);
            }
            billtoUpdate.pettyCashes = bill.pettyCashes;

            //if (bill.Adjustments != null)
            //{
            //    billtoUpdate.Adjustments = bill.Adjustments;
            //}
            //string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ef-log.txt");

            //if (File.Exists(logFilePath))
            //    File.Delete(logFilePath); // Optional: clear previous logs

            //context.Database.Log = log => File.AppendAllText(logFilePath, log);

            context.SaveChanges();

            

        }

        /// <summary>
        /// Update Bill and allocate it to new employee
        /// </summary>
        /// <param name="bill">Bill to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(Bill bill, Employee employee)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == bill.Id);
            billtoUpdate.AllocateTo = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(BillStatus status)
        {
            BillStatus poStatus = context.billStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change bill status
        /// </summary>
        /// <param name="bill"></param>
        /// <param name="status"></param>
        public void updateStatus(Bill bill, BillStatus status)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == bill.Id);
            billtoUpdate.BillStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change bill status based on Bill ID
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="status"></param>
        public void setSotoVoid(int SoId, bool isVoid)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == SoId);
            billtoUpdate.isVoid = isVoid;
            var billFields = context.costSheetBillFields.Where(x => x.Bill_Id == SoId).ToList();
            if(billFields!=null)
            context.costSheetBillFields.RemoveRange(billFields);
            context.SaveChanges();
        }

        /// <summary>
        /// Change bill status based on Bill ID
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="status"></param>
        public void updateStatus(int billId, BillStatus status)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == billId);
            billtoUpdate.BillStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change bill status based on Bill ID
        /// </summary>
        /// <param name="saleOrderId"></param>
        /// <param name="status"></param>
        public void updateStatusById(int saleOrderId,int statusId)
        {
            Bill billtoUpdate = context.bills.FirstOrDefault(x => x.Id == saleOrderId);
            billtoUpdate.BillStatus.Id = statusId;
            context.SaveChanges();
        }



        /// Get all bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills


                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllByDateRange(DateTime from, DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Inactive bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => deptIds.Contains(x.dept_Id) &&  companyIds.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllInActiveFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllInActiveByDateRange(DateTime from, DateTime to,int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Active bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id); 

            for (int i = 0; i< user.employee.departments.Count; i++)
                deptIds.Add(user.employee.departments[i].Id);

            List<int> companyIds = new List<int>();
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);

            for (int i = 0; i < user.employee.Companies.Count; i++)
                companyIds.Add(user.employee.Companies[i].Id);


            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
       
        public List<Bill> getAllActiveFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id); 

            for (int i = 0; i < user.employee.departments.Count; i++)
                deptIds.Add(user.employee.departments[i].Id);

            List<int> companyIds = new List<int>();
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);

            for (int i = 0; i < user.employee.Companies.Count; i++)
                companyIds.Add(user.employee.Companies[i].Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id); 

            for (int i = 0; i < user.employee.departments.Count; i++)
                deptIds.Add(user.employee.departments[i].Id);

            List<int> companyIds = new List<int>();
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);

            for (int i = 0; i < user.employee.Companies.Count; i++)
                companyIds.Add(user.employee.Companies[i].Id);


            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Inactive bills except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllInActiveandUnapproved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
     
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.BillStatus.isActive == false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Active bills except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllActiveandUnapporved(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
       
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.BillStatus.isActive == true && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all bills for user.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getPurchaseRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
 
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getPurchaseRegisterFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getPurchaseRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
         
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all bills .
        /// </summary>
        /// <returns></returns>
        public List<Bill> getPurchaseRegisterAdministrator()
        {
            return context.bills

    .Where(x =>  x.isVoid != true)
    .ToList();
        }
        public List<Bill> getPurchaseRegisterAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
   
            .Where(x => x.CreationDate >= previousMonthDate && x.isVoid != true).ToList();
        }
        public List<Bill> getPurchaseRegisterAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.bills

            .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isVoid != true).ToList();
        }
        /// <summary>
        /// Get all bills .
        /// </summary>
        /// <returns></returns>
        public int getPurchaseRegisterAdministratorCount()
        {
            return context.bills

    .Where(x =>  x.isVoid != true)
    .Count();
        }
        /// <summary>
        /// Get all bills for user count. 
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

            return context.bills
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all bills own.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getPurchaseRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
     
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .ToList();
        }

        //#Void
        /// <summary>
        /// Get all Void bills for user.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void bills .
        /// </summary>
        /// <returns></returns>
        public List<Bill> getVoidRegisterAdministrator()
        {
            return context.bills

        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void bills .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.bills

    .Where(x =>  x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void bills for user count. 
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
            return context.bills
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void bills own.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }

        /// <summary>
        /// Get all bills  own count. 
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
            return context.bills
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
    
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x =>x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId ||x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId ||x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId ||x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all pending bills by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get Count pending bills by Departments
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
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own pending bills by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
        

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalOwnFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<Bill> getAllPendingForReApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
    
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
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
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending bills user Id and <paramref name="UserID"/>.
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
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }




        /// <summary>
        /// Get all pending bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all pending bills by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
  
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
 
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get Count pending bills by Departments
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending bills by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all pending bills by Departmental Count
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Own pending bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
            
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get Count pending bills by supervisor Id and <paramref name="StatusId"/>.
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all pending bills by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
            
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
      
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
        
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get own pending bills by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
            
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
      
                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
              
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count pending for closing bills own <paramref name="StatusId"/>.
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

            return context.bills

                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all bills count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getbillsCountByStatusId(int uid, int statusId)
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

                    return _DbContext.bills

                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.Id == statusId && x.isApproved == true && x.isVoid != true)
                        .Count();
                }
                else
                    return _DbContext.bills.Where(x => x.BillStatus.Id == statusId && x.isApproved == true && x.isVoid != true).Count();
            }
        }

        /// <summary>
        /// Get all Active bills except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getUserSOByYear(int userId, int year)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();


            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
 
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Active bills except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getDepartmentalSOByYear(int departmentId, int companyId, int year)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Active bills except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getDepartmentalSOByMonth(int departmentId, int companyId, int year, int month)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();

            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.bills

                .Where(x => x.dept_Id == departmentId && x.company_Id == companyId && x.CreationDate.Value.Year == year && x.CreationDate.Value.Month == month && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all bills count
        /// </summary>
        /// <returns></returns>
        public int getbillsCount(int uid)
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

                return context.bills

                    .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.isApproved == true && x.isVoid != true)
                    .Count();
            }
            else
                return context.bills.Where(x => x.isApproved == true).Count();

        }
        /// <summary>
        /// Get all bills by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
                .Where(x => (deptIds.Contains(x.dept_Id)&& companyIds.Contains((int)x.company_Id) ||(deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany==true)) && x.BillStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPobyStatusIdFirst(int uid, int StatusId)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPobyStatusIdByDateRange(DateTime from, DateTime to, int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.bills
               
      
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.Id == StatusId && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForAdministrator()
        {
            return context.bills
 
                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.bills

                .Where(x =>x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        /// Get Count bills.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.bills

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get Count bills.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.bills

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        /// <summary>
        /// Get all bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllPendingForClosingAdministrator()
        {
            return context.bills

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.bills
           
                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<Bill> getAllPendingForClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.bills
   
                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all bills.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAll()
        {
            return context.bills

            .Where(x=>  x.isVoid != true)
                .ToList();
        }
        /// <summary>
        /// Get all Bills Status.
        /// </summary>
        /// <returns></returns>
        public List<BillStatus> getAllBillStatus()
        {
            return context.billStatuses.ToList();
        }
        /// <summary>
        /// Get all Bills Status.
        /// </summary>
        /// <returns></returns>
        public List<BillStatus> getAllActiveBillStatus()
        {
            List<BillStatus> statuses = new List<BillStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses= _DbContext.billStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all Bills Status.
        /// </summary>
        /// <returns></returns>
        public List<BillStatus> getAllInActiveBillStatus()
        {
              List<BillStatus> statuses = new List<BillStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.billStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all Bills Status.
        /// </summary>
        ///  <param name="billstatusid"></param>
        /// <returns></returns>
        public BillStatus getstatus(int billstatusid)
        {
            return context.billStatuses.FirstOrDefault(x => x.Id == billstatusid);
        }


        /// <summary>
        /// Get all bills of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<Bill> getAll(Employee employee)
        {
            return context.bills.Where(x => x.allocation_Id == employee.EmpId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get all bills of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Bill> getAll(User user)
        {
            return context.bills.Where(x => x.user_Id == user.id && x.isVoid != true).ToList();
        }


        /// <summary>
        /// Get So based on Sales Refrence No.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public Bill get(string SoRef)
        {
            return context.bills.FirstOrDefault(x => x.SalesReferenceNo == SoRef);
        }


        /// <summary>
        /// Get bill based on Bill ID.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public Bill getForGrid(int billId)
        {
            return context.bills

            .FirstOrDefault(x => x.Id == billId);
        }


        /// <summary>
        /// Get bill based on Bill ID.
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        public Bill get(int billId)
        {
            return context.bills


            .FirstOrDefault(x => x.Id == billId);
        }
        /// <summary>
        /// Get all bills Allocated to an Employee <para>EmployeeId</para>.
        /// </summary>
        /// <returns></returns>
        public List<Bill> getAllAllcatedtoEmployee(int EmployeeId)
        {
            return context.bills

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
        /// <summarBillType
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
        /// <summarBillType
        /// </summary>
        /// <param name="SummarySheetField">SummarySheetField  Object</param>
        public void UpdateSummarySheetField(SummarySheetField summarySheetField)
        {
            SummarySheetField prod = context.summarySheetFields.FirstOrDefault(x => x.Id == summarySheetField.Id);
            prod = summarySheetField;
            context.SaveChanges();
        }
        /// <summary>
        /// Get list of all BillTypes in DB
        /// </summary>
        /// <returns>List of BillTypes Objects</returns>
        public List<BillType> getallbillType()
        {
            return context.billTypes.ToList();

        }
        /// <summary>
        /// Get All Active BillTypes
        /// </summary>
        /// <returns></returns>
        public List<BillType> getActiveBillTypes()
        {
            return context.billTypes.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive BillTypes
        /// </summary>
        /// <returns></returns>
        public List<BillType> getinActiveBillTypes()
        {
            return context.billTypes.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new BillType
        /// </summary>
        /// <param name="productbillType">BillType Object</param>
        public void AddbillType(BillType productbillType)
        {
            context.billTypes.Add(productbillType);
            context.SaveChanges();
        }
        /// <summary>
        /// get BillType by ID
        /// </summary>
        /// <param name="Natureid">BillType ID</param>
        /// <returns></returns>
        public BillType getbillType(int billTypeId)
        {
            return context.billTypes
                .FirstOrDefault(x => x.Id == billTypeId);
        }
        /// <summarBillType
        /// </summary>
        /// <param name="product">BillType  Object</param>
        public void UpdatebillType(BillType productbillType)
        {
            BillType prod = context.billTypes.FirstOrDefault(x => x.Id == productbillType.Id);
            prod = productbillType;
            context.SaveChanges();
        }
        public int getTodayBillCount()
        {
            return context.bills.Where(x => x.isVoid != true && EntityFunctions.TruncateTime(x.CreationDate) == DateTime.Today)
                .Count();
        }


        /// <summary>
        /// Get All Bills of Company, Departments and Vendor
        /// </summary>
        /// <returns></returns>
        public List<Bill> GetAllBillsByCompDeptVendor(int compId, List<Department> departments, int vendorId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }
            var bills = context.bills
           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.vendor_Id == vendorId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<Bill> billsToReturn = new List<Bill>();
            foreach (var _bill in bills)
            {
                if (dept_Ids.Contains((int)_bill.dept_Id))
                {
                    billsToReturn.Add(_bill);
                }
            }

            return billsToReturn;
        }

        /// <summary>
        /// Get All Bills of Company & Inter-company, Departments & Inter-Department and Vendor
        /// </summary>
        /// <returns></returns>
        public List<Bill> GetAllBillsByCompDeptVendorInterComp(int compId, List<Department> departments, int interCompId, int interDeptId, int vendorId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var bills = context.bills

           .Where(x => ((x.company_Id == compId && dept_Ids.Contains(x.dept_Id)) || (x.InterCompany_Id == interCompId && x.InterDepartment_Id == interDeptId && x.isInterCompany == true)) && x.currency_Id == currId && x.vendor_Id == vendorId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();


            List<Bill> billsToReturn = new List<Bill>();
            billsToReturn = bills;


            return billsToReturn;
        }

        /// <summary>
        /// Get All Bills of Company and Departments 
        /// </summary>
        /// <returns></returns>
        public List<Bill> getBillsByCompanyDeptInterComp(int compId, List<Department> departments, int interCompId, int interDeptId, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var bills = context.bills

           .Where(x => ((x.company_Id == compId && dept_Ids.Contains(x.dept_Id)) || (x.InterCompany_Id == interCompId && x.InterDepartment_Id == interDeptId && x.isInterCompany == true)) && x.currency_Id == currId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<Bill> billsToReturn = new List<Bill>();
            billsToReturn = bills;
            //foreach (var _bill in bills)
            //{
            //    if (dept_Ids.Contains((int)_bill.dept_Id))
            //    {
            //        billsToReturn.Add(_bill);
            //    }
            //}

            return billsToReturn;
        }


        /// <summary>
        /// Get All Bills of Company and Departments 
        /// </summary>
        /// <returns></returns>
        public List<Bill> getBillsByCompanyDept(int compId, List<Department> departments, int currId )
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var bills = context.bills

           .Where(x => x.company_Id == compId && x.currency_Id == currId && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
           .ToList();

            List<Bill> billsToReturn = new List<Bill>();
            foreach (var _bill in bills)
            {
                if (dept_Ids.Contains((int)_bill.dept_Id))
                {
                    billsToReturn.Add(_bill);
                }
            }

            return billsToReturn;
        }
        public void RemoveBillCostSheetFields(int bill_Id)
        {
           

        }
        public BillItem GetBillItem(int item_Id)
        {
            return context.billItems.FirstOrDefault(x => x.Id == item_Id);

        }


        /// <summary>
        /// Add New Bill Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        public void AddBillReferenceNo(VendorBillReference refNumber)
        {
            context.vendorBillReferences.Add(refNumber);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Bill Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        public void UpdateBillReferenceNo(VendorBillReference refNumber)
        {
            VendorBillReference _billRefNumber = context.vendorBillReferences.FirstOrDefault(x => x.Id == refNumber.Id);

            _billRefNumber.Reference = refNumber.Reference;
            _billRefNumber.companyId = refNumber.companyId;
            _billRefNumber.isActive = refNumber.isActive;
            context.SaveChanges();
        }



        /// <summary>
        /// Get Bill Reference Number
        /// </summary>
        /// <param name="refId"></param>
        /// <returns></returns>
        public VendorBillReference GetBillReferenceNo(int refId)
        {
            return
                context.vendorBillReferences
    
                .FirstOrDefault(x => x.Id == refId);
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



         public void AddVendorBillNature(VendorBillNature billNature)
        {
            context.vendorBillNatures.Add(billNature);
            context.SaveChanges();
        }

        public void UpdateVendorBillNature(VendorBillNature billNature)
        {
            VendorBillNature _billNature = context.vendorBillNatures.FirstOrDefault(x => x.Id == billNature.Id);
            _billNature = billNature;
            //_billNature.companyId = billNature.companyId;
            //_billNature.Nature = billNature.Nature;
            //_billNature.isActive = billNature.isActive;
            context.SaveChanges();
        }

        public VendorBillNature GetVendorBillNature(int natureId)
        {
            return context.vendorBillNatures
              
                .FirstOrDefault(x => x.Id == natureId);
        }

        public List<VendorBillNature> GetAllVendorBillNature()
        {
            return context.vendorBillNatures
               
                .ToList();
        }

        public List<VendorBillNature> GetAllVendorBillNatureByComp(int compId)
        {
            return context.vendorBillNatures
                
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
        public Bill GetForCostsheet(int billId)
        {
            return context.bills
            .FirstOrDefault(x => x.Id == billId);
        }
        public List<Bill> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.bills
                .Where(x => deptIds.Contains(x.dept_Id)
                && companyIds.Contains((int)x.company_Id)
                && x.InterCompany_Id == null
                && x.InterDepartment_Id == null
                && x.isApproved == true
                && x.isVoid != true
                && x.BillStatus.isActive == true
               ).ToList();
        }
    }
}
