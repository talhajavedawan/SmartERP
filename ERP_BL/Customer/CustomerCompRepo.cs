using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    interface ICustomerCompRepo
    {
        void Add(CustomerCompany customerCompany);
        void Add(CustomerCompany parentComp, CustomerCompany subsidaryComp);
        void Add(int parentCompId, CustomerCompany subsidaryComp);

        List<CustomerCompany> getAll();
        CustomerCompany get(int customerCompID);

        List<CustomerCompany> getSubsidaries(int customerCompID);
        List<CustomerCompany> getSubsidaries(CustomerCompany parentCustomer);


        void Update(CustomerCompany customerCompany);
       
        void Update(CustomerCompany customerCompany, Contact contact);
        void Update(int customerCompID, Contact contact);
        void Update(CustomerCompany customerCompany, Person contactPerson);
        void Update(int customerCompID, Person person);

        void UpdateBillingAddress(CustomerCompany customerCompany, Address billingAddress);
        void UpdateShippingAddress(CustomerCompany customerCompany, Address shippingAddress);
        void UpdateBillingAddress(int customerCompID, Address billingAddress);
        void UpdateShippingAddress(int customerCompID, Address shippingAddress);

    }
    public class CustomerCompRepo : ICustomerCompRepo
    {
        DBContextERP context = new DBContextERP();

        public CustomerCompRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created");

        }
        /// <summary>
        /// Add new Customer Company or Subsidary company
        /// </summary>
        /// <param name="customerCompany">Customer Company Object</param>
        public void Add(CustomerCompany customerCompany)
        {
            context.customerCompanies.Add(customerCompany);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Customer with Name= " + customerCompany.company.CompanyName + " Id= " + customerCompany.Id);

        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            SystemLog.LogInfo(this.GetType(), "Get List of All departments ");

            return context.Departments.ToList();
        }

        /// <summary>
        /// Add Subsidary Company based to parent company object
        /// </summary>
        /// <param name="parentComp">CustomerCompany Object as Parent Customer Company</param>
        /// <param name="subsidaryComp">CustomerCompany Object as Subsidary Customer Company</param>
        public void Add(CustomerCompany parentComp, CustomerCompany subsidaryComp)
        {
            SystemLog.LogInfo(this.GetType(), "Added Sub Customer with Name= " + subsidaryComp.company.CompanyName + " Id= " + subsidaryComp.Id +" to Parent Company=" +parentComp.company.CompanyName);
            subsidaryComp.parentCompany = parentComp;
            context.customerCompanies.Add(subsidaryComp);
            context.SaveChanges();
        }

        /// <summary>
        /// Add subsidary company based on parent company ID
        /// </summary>
        /// <param name="parentCompId">parent Company ID</param>
        /// <param name="subsidaryComp">CustomerCompany Object as Subsidary company </param>
        public void Add(int parentCompId, CustomerCompany subsidaryComp)
        {
            CustomerCompany parentComp = context.customerCompanies.FirstOrDefault(x => x.Id == parentCompId);
            subsidaryComp.parentCompany = parentComp;
            context.customerCompanies.Add(subsidaryComp);
            context.SaveChanges();
        }

        /// <summary>
        /// return all Customer Company list.
        /// </summary>
        /// <returns></returns>
        public List<CustomerCompany> getAll()
        {
            return context.customerCompanies

                .ToList();
        }
        /// <summary>
        /// get all companies
        /// </summary>
        /// <returns></returns>
        public List<ERP_BL.Databases.Company> GetCompanies()
        {
            return context.Companies
.Where(s => s.compnayType == Enums.CompnayTypes.Company)
                .ToList();
        }
        /// <summary>
        /// get CustomerCompany matching to ID
        /// </summary>
        /// <param name="customerCompID">Customer Company ID</param>
        /// <returns></returns>
        public CustomerCompany get(int customerCompID)
        {
            return context.customerCompanies

                .FirstOrDefault(x => x.Id == customerCompID);
        }

        /// <summary>
        /// Get all subsidaries related to customer Company
        /// </summary>
        /// <param name="customerCompID">Customer Company ID </param>
        /// <returns></returns>
        public List<CustomerCompany> getSubsidaries(int customerCompID)
        {
            return context.customerCompanies

                .Where(x => x.ParentID == customerCompID).ToList();
        }
        /// <summary>
        /// Get all Customers related to Company and A department
        /// </summary>
        /// <param name="CompanyId">Company ID</param>
        /// <param name="deptId">Department Id </param>
        /// <returns></returns>
        public List<CustomerCompany> getCustomersForCompanyAndDepartment(int CompanyId, int deptId)
        {
            return context.customerCompanies

                .Where(x => x.Companies.Any(y=>y.Id == CompanyId)&& x.departments.Any(Z => Z.Id == deptId)).ToList();
        }
        public List<CustomerCompany> getSubsidaries(CustomerCompany parentCustomer)
        {
            return context.customerCompanies

                .Where(x => x.ParentID == parentCustomer.Id).ToList();
        }

        /// <summary>
        /// update Customer Company object details
        /// </summary>
        /// <param name="customerCompany">CustomerCompany Object</param>
        public void Update(CustomerCompany customerCompany)
        {
            CustomerCompany compToUpdate = context.customerCompanies.FirstOrDefault(x => x.Id == customerCompany.Id);
            compToUpdate = customerCompany;
            context.SaveChanges();
        }

        /// <summary>
        /// Update shipping address of customer company
        /// </summary>
        /// <param name="customerCompany">Object of CustomerCompany Object to be updates</param>
        /// <param name="shippingAddress">Address object with changes</param>
        public void UpdateShippingAddress(CustomerCompany customerCompany, Address shippingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == customerCompany.shippingAddress.Id);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Billing address of customer company
        /// </summary>
        /// <param name="customerCompany">Object of CustomerCompany Object to be updates</param>
        /// <param name="billingAddress">Address object with changes</param>
        public void UpdateBillingAddress(CustomerCompany customerCompany, Address billingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == customerCompany.billingAddres.Id);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update shipping address of customer company
        /// </summary>
        /// <param name="customerCompID">CustomerCompnay ID</param>
        /// <param name="shippingAddress">Address Object with changes</param>
        public void UpdateShippingAddress(int customerCompID, Address shippingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == customerCompID);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Billing address of customer company
        /// </summary>
        /// <param name="customerCompID">CustomerCompnay ID</param>
        /// <param name="billingAddress">Address Object with changes</param>
        public void UpdateBillingAddress(int customerCompID, Address billingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == customerCompID);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of customer company based on CustomerCompany Object
        /// </summary>
        /// <param name="customerCompany">CustomerCompnay Object</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(CustomerCompany customerCompany, Contact contact)
        {

            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == customerCompany.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of customer company based on customerCompany ID
        /// </summary>
        /// <param name="customerCompID">CustomerCompnay ID</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(int customerCompID, Contact contact)
        {
            CustomerCompany customerCompany = context.customerCompanies.FirstOrDefault(x => x.Id == customerCompID);
            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == customerCompany.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Contact Person details of customer company based on customerCompany Object
        /// </summary>
        /// <param name="customerCompany">CustomerCompnay Object</param>
        /// <param name="contactPerson">Person Object</param>
        public void Update(CustomerCompany customerCompany, Person contactPerson)
        {
            Person ContToUpdate = context.Persons.FirstOrDefault(x => x.Id == contactPerson.Id);
            ContToUpdate = contactPerson;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact Person details of customer company based on customerCompany ID
        /// </summary>
        /// <param name="customerCompID">CustomerCompnay ID</param>
        /// <param name="person">Person Object</param>
        public void Update(int customerCompID, Person person)
        {
            CustomerCompany customerCompany = context.customerCompanies.FirstOrDefault(x => x.Id == customerCompID);
            Person personToUpdate = context.Persons.FirstOrDefault(x => x.Id == customerCompany.contactPerson.Id);
            personToUpdate = person;
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
        /// Get Customer Departments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ERP_BL.Databases.Department> GetCustomerDepartments(int id)
        {
            return  context.customerCompanies
                                
                .FirstOrDefault(x => x.Id == id).departments.ToList();
             
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
        public List<Company> GetEmployeeCompanies(int empId)
        {

            return context.Companies.Where(x => x.employees.FirstOrDefault(y => y.EmpId == empId) != null).ToList();
        }
        public object getUserCustomers(int currentUserid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == currentUserid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.customerCompanies
            
                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id))&& x.Companies.Any(y => companyIds.Contains(y.Id)) && x.isActive == true)
                .ToList();
        }
        /// <summary>
        /// return all Vendor Companies list for a specific User.
        /// </summary>
        /// <returns></returns>
        public List<Vendor> getCurrentUserVendors(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.Vendors
             
                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.isActive == true)
                .ToList();
        }

        public List<CustomerCompany> GetAllCustomerCompanies()
        {
            return context.customerCompanies
                  
                    .ToList();
        }

        public List<ContactPerson> GetAllContactPersonsByCustomer(int custId)
        {
            return context.contactPersons 
             .Where(x=>x.customerCompanyId == custId)
                    .ToList();
        }

        public List<ContactPerson> GetAllInActiveContactPersonsByCustomer(int custId)
        {
            return context.contactPersons.Where(x => x.customerCompanyId == custId && x.isActive == false)
                    .ToList();
        }

        public List<ContactPerson> GetAllActiveContactPersonsByCustomer(int custId)
        {
            return context.contactPersons
                  .Where(x => x.customerCompanyId == custId && x.isActive == true)
                    .ToList();
        }
        public List<CustomerCompany> GetAllContactPersons(int custId)
        { 
            return context.customerCompanies
                   .Where(x => x.ContactPersons.Any(y=>y.customerCompanyId == custId))
                    .ToList();
        }
        public CustomerCompany GetCustomerCompany(int comPersonId)
        {
            return context.customerCompanies
      
                    .FirstOrDefault(x => x.Id == comPersonId);
        }
    
        public void AddReligionType(Religion religion)
        {
            context.religions.Add(religion);
            context.SaveChanges();
        }
        public void UpdateReligionType(Religion method)
        {
            Religion _method = context.religions.FirstOrDefault(x => x.Id == method.Id);
            _method = method;
            context.SaveChanges();
        }
        public List<Religion> GetAllReligion() 
        {
            return context.religions
                .ToList();
        }
        public Religion GetReligion(int methodId) 
        {
            return
                context.religions.FirstOrDefault(x => x.Id == methodId);
        }
        public void AddContactPerson(ContactPerson contactPerson)
        {
            context.contactPersons.Add(contactPerson);
            context.SaveChanges();
        }
        public void UpdateContactPerson(ContactPerson person) 
        {
            ContactPerson _method = context.contactPersons.FirstOrDefault(x => x.Id == person.Id);
            _method = person;
            context.SaveChanges();
        }
        public List< ContactPerson> GetAllContactPerson()
        {
          return  context.contactPersons

                    .ToList();
        }
        public ContactPerson GetContactPerson(int comPersonId)
        {
            return context.contactPersons

                    .FirstOrDefault(x => x.Id == comPersonId);
        }

        /// <summary>
        /// Get all IndustryTypes
        /// </summary>
        /// <returns>List of IndustryTypes Objects</returns>
        public List<ERP_BL.Databases.IndustryType> GetIndustryTypes()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.IndustryTypes.ToList();

        }
        public List<string> GetRegionList()
        {
            return context.Address.Select(x => x.region).Distinct().ToList();

        }
        public List<string> GetCountryList() 
        {
            return context.Address.Select(x=>x.Country).Distinct().ToList();
              
        }
        public List<string> GetCountryListByRegion(string region)
        {
            return context.Address.Where(y=>y.region == region)
                .Select(x => x.Country)
                .Distinct().ToList();
        }
        public List<string> GetCityListByCountry(string country)
        {
            return context.Address.Where(x => x.Country == country).Select(y => y.City)
                    .Distinct()
                    .ToList();
        }

        public List<string> GetCityListByCountryRegionState(string country, string region, string state)
        {
            return context.Address.Where(x => x.Country == country && x.region == region && x.State == state).Select(y => y.City)
                    .Distinct()
                    .ToList();
        }

        public List<string> GetStateListByCountryRegion(string country, string region)
        {
            return context.Address.Where(x => x.Country == country && x.region == region).Select(y=>y.State)
                    .Distinct()
                    .ToList();
        }
        public List<string> GetStateListByCountry(string country)
        {
            return context.Address.Where(x => x.Country == country).Select(y => y.State)
                    .Distinct()
                    .ToList();
        }
        public bool CheckCompanyNameExixtence(string companyName, int Id)
        {
            if (context.customerCompanies.FirstOrDefault(x => x.company.CompanyName == companyName && x.Id != Id) != null)
                return true;
            else
                return false;
        }
        public bool CheckOrders(int deptId, int customerId)
        {
            List<Inquiry> inquiries = context.inquiries.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            List<SaleOrder> saleOrders = context.saleOrders.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            List<Bill> vendorBills = context.bills.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            List<SaleInvoice> saleInvoice = context.saleInvoices.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            List<SalesReceipt> salesReceipts = context.salesReceipts.Where(x => x.CustomerId == customerId && x.deptId == deptId).ToList();
            List<PurchaseOrder> purchaseOrders = context.purchaseOrders.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            List<PurchaseInvoice> purchaseInvoices = context.purchaseInvoices.Where(x => x.customerCompany_Id == customerId && x.dept_Id == deptId).ToList();
            if (inquiries.Count > 0 || saleOrders.Count > 0 || vendorBills.Count > 0 || saleInvoice.Count > 0 || salesReceipts.Count > 0 || salesReceipts.Count > 0 || purchaseOrders.Count > 0 || purchaseInvoices.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
