using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class VendorRepo
    {
        DBContextERP context = new DBContextERP();

        public Department getParent(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }


        /// <summary>
        /// Add new Vendor Company 
        /// </summary>
        /// <param name="vendor">Vendor Company Object</param>
        public void Add(Vendor vendor)
        {
            var deptIds = vendor.departments.Select(x => x.Id);
            vendor.departments = new List<Department>();
            foreach (var _id in deptIds)
            {
                vendor.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
            }
            context.Vendors.Add(vendor);
            context.SaveChanges();
        }
        public void Update(Vendor vendor)
        {
            Vendor compToUpdate = context.Vendors.FirstOrDefault(x => x.Id == vendor.Id);
            compToUpdate = vendor;

            var compIds = compToUpdate.Companies.Select(x => x.Id);
            var deptIds = compToUpdate.departments.Select(x => x.Id);
            compToUpdate.Companies = new List<Company>();
            foreach (var _id in compIds)
            {
                compToUpdate.Companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
            } 
            compToUpdate.departments = new List<Department>();
            foreach (var _id in deptIds)
            {
                compToUpdate.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            return context.Departments.ToList();
        }



        /// <summary>
        /// return all Vendor Companies list.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getAll()
        {
            return context.Vendors
                
                .ToList();
        }

        /// <summary>
        /// Check Vendor in the Database.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> CheckVendorName(string vendorName)
        {
            return context.Vendors.Where(x=>x.company.CompanyName == vendorName && x.company.compnayType == Enums.CompnayTypes.VendorCompany)
                .ToList();
        }

        public List<Vendor> getAllbyDepartment(Employee employee)
        {
            List<int> deptIds = new List<int>();
            //foreach (var dpt in employee.departments)
            //    deptIds.Add(dpt.Id);
            deptIds = employee.departments.Select(x=>x.Id).ToList();
            
            var vendors = context.Vendors
                .Where(x=>x.departments.Any(y=>deptIds.Contains(y.Id)))
                .ToList();
            return vendors;
        }
        public List<Vendor> getAllActivebyDepartment(Employee employee)
        {
            List<int> deptIds = new List<int>();
            //foreach (var dpt in employee.departments)
            //    deptIds.Add(dpt.Id);

            employee.departments.Add(context.Departments.FirstOrDefault(x => x.DeptName == "VDummy"));

            deptIds = employee.departments.Select(x => x.Id).ToList();

            var vendors = context.Vendors
              .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.isActive == true)
                .ToList();
            return vendors;
        }

        /// <summary>
        /// return all Vendor on the Basis of IndustryType.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getAllByIndustryType(int industryTypeId)
        {
            return context.Vendors
         
                .Where(x=>x.company.industryTypeId == industryTypeId)
                .ToList();
        }

        /// <summary>
        /// return all Vendor on the Basis of IndustryType.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getAllByIndustryTypeDept(int industryTypeId, int deptid)
        {
            return context.Vendors

                .Where(x => x.company.industryTypeId == industryTypeId && x.departments.Any(y => y.Id == deptid))
                .ToList();
        }

        /// <summary>
        /// return all Vendor on the Basis of IndustryType.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getVendorsByMultipleIndustryTypes(List<IndustryType> industryTypes)
        {
            List<Vendor> resultVendors = new List<Vendor>();

            foreach(var _industry in industryTypes)
            {
                var vendorList = getAllByIndustryType(_industry.Id);
                if(vendorList != null)
                    vendorList.ForEach(item => resultVendors.Add(item));
            }

            return resultVendors;
        }

        /// <summary>
        /// return all Vendor Companies list for a specific User.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getCurrentUserVendors(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.Vendors
                .Include("Company")
                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.isActive == true)
                .ToList();
        }
        /// <summary>
        /// get vendorCompany matching to ID
        /// </summary>
        /// <param name="vendorCompID">Vendor Company ID</param>
        /// <returns></returns>
        public Vendor get(int vendorCompID)
        {
            return context.Vendors
      
                .FirstOrDefault(x => x.Id == vendorCompID);
        }
        public Department getDepartment(int departmentID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Department Having Id= " + departmentID);

            return context.Departments.FirstOrDefault(x => x.Id == departmentID);
        }



        /// <summary>
        /// update Vendor Company object details
        /// </summary>
        /// <param name="vendor">Vendor Company Object</param>

        public List<Company> GetEmployeeCompanies(int empId)
        {

            return context.Companies.Where(x => x.employees.FirstOrDefault(y => y.EmpId == empId) != null).ToList();
        }
        /// <summary>
        /// Update shipping address of vendor company
        /// </summary>
        /// <param name="vendor">Object of vendor Object to be updates</param>
        /// <param name="shippingAddress">Address object with changes</param>
        public void UpdateShippingAddress(Vendor vendor, Address shippingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendor.shippingAddress.Id);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Billing address of vendor company
        /// </summary>
        /// <param name="vendor">Object of vendor Object to be updates</param>
        /// <param name="billingAddress">Address object with changes</param>
        public void UpdateBillingAddress(Vendor vendor, Address billingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendor.billingAddres.Id);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update shipping address of vendor company
        /// </summary>
        /// <param name="vendor">vendor ID</param>
        /// <param name="shippingAddress">Address Object with changes</param>
        public void UpdateShippingAddress(int vendor, Address shippingAddress)
        {
            Vendor vendr = context.Vendors.FirstOrDefault(x => x.Id == vendor);
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendr.shippingAddress.Id);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetUserDepartments(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.departments;
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
        /// Update Billing address of vendor company
        /// </summary>
        /// <param name="vendor">vendor ID</param>
        /// <param name="billingAddress">Address Object with changes</param>
        public void UpdateBillingAddress(int vendor, Address billingAddress)
        {
            Vendor vendr = context.Vendors.FirstOrDefault(x => x.Id == vendor);
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendr.billingAddres.Id);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of vendor company based on vendor Object
        /// </summary>
        /// <param name="vendor">vendor Object</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(Vendor vendor, Contact contact)
        {

            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == vendor.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of vendor company based on vendor ID
        /// </summary>
        /// <param name="vendor">vendor ID</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(int vendor, Contact contact)
        {
            Vendor vendr = context.Vendors.FirstOrDefault(x => x.Id == vendor);
            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == vendr.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Contact Person details of vendor company based on vendor Object
        /// </summary>
        /// <param name="vendor">vendor Object</param>
        /// <param name="contactPerson">Person Object</param>
        public void Update(Vendor vendor, Person contactPerson)
        {
            Person ContToUpdate = context.Persons.FirstOrDefault(x => x.Id == contactPerson.Id);
            ContToUpdate = contactPerson;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact Person details of vendor company based on vendor ID
        /// </summary>
        /// <param name="vendorCompID">vendor ID</param>
        /// <param name="person">Person Object</param>
        public void Update(int vendorCompID, Person person)
        {
           Vendor vendor = context.Vendors.FirstOrDefault(x => x.Id == vendorCompID);
            Person personToUpdate = context.Persons.FirstOrDefault(x => x.Id == vendor.contactPerson.Id);
            personToUpdate = person;
            context.SaveChanges();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<VendorPaymentStatus> getAllVendorPaymentStatus()
        {
            return context.vendorPaymentStatuses.ToList();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<VendorPaymentStatus> getAllActiveVendorPaymentStatus()
        {
            return context.vendorPaymentStatuses.Where(x => x.isActive == true).ToList();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        /// <returns></returns>
        public List<VendorPaymentStatus> getAllInActiveVendorPaymentStatus()
        {
            return context.vendorPaymentStatuses.Where(x => x.isActive == false).ToList();
        }
        /// <summary>
        /// Add VendorPaymentStatus in database
        /// </summary>
        /// <param name="status">VendorPaymentStatus Object</param>
        public void addStatus(VendorPaymentStatus status)
        {
            context.vendorPaymentStatuses.Add(status);
            context.SaveChanges();
        }
        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(VendorPaymentStatus status)
        {
            VendorPaymentStatus poStatus = context.vendorPaymentStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Get all PurchaseOrders Status.
        /// </summary>
        ///  <param name="VendorPaymentStatusid"></param>
        /// <returns></returns>
        public VendorPaymentStatus getstatus(int VendorPaymentStatusid)
        {
            return context.vendorPaymentStatuses.FirstOrDefault(x => x.Id == VendorPaymentStatusid);
        }
        public Vendor Get(int vendorId)
        {
           return context.Vendors.FirstOrDefault(x=>x.Id==vendorId);
            
        }
    }
}
