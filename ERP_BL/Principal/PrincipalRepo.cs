using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
   public class PrincipalRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add new Principal Company 
        /// </summary>
        /// <param name="principal">Principal Company Object</param>
        public void Add(Principal principal)
        {
            var deptIds = principal.departments.Select(x=>x.Id);
            principal.departments = new List<Department>();
            foreach(var deptId in deptIds)
            {
                principal.departments.Add(context.Departments.FirstOrDefault(x => x.Id == deptId));
            }
            context.Principals.Add(principal);
            context.SaveChanges();
        }





        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Departments ");

            return context.Departments.ToList();
        }
        /// <summary>
        /// return all Principal Companies list.
        /// </summary>
        /// <returns></returns>
        public List<Principal> getAll()
        {
            return context.Principals

                .ToList();
        }

        /// <summary>
        /// return all Principal Companies list.
        /// </summary>
        /// <returns></returns>
        public List<Principal> getAllByDept(int deptId)
        {
            return context.Principals

                .Where(x=>x.departments.FirstOrDefault(y=>y.Id == deptId) != null)
                .ToList();
        }

        /// <summary>
        /// return all Principal by Departments list.
        /// </summary>
        /// <returns></returns>
        public List<Principal> getAllByMultiDept(List<Department> departments)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            return context.Principals
                .Where(x => x.departments.FirstOrDefault(y => dept_Ids.Contains( y.Id)) != null)
                .ToList();
        }

        /// <summary>
        /// get principalCompany matching to ID
        /// </summary>
        /// <param name="principalCompID">Principal Company ID</param>
        /// <returns></returns>
        public Principal get(int principalCompID)
        {
            return context.Principals

                .FirstOrDefault(x => x.Id == principalCompID);
        }



        /// <summary>
        /// update Principal  object details
        /// </summary>
        /// <param name="principal">Principal  Object</param>
        public void Update(Principal principal)
        {
            var deptIds = principal.departments.Select(x => x.Id);
            principal.departments = new List<Department>();
            foreach (var deptId in deptIds)
            {
                principal.departments.Add(context.Departments.FirstOrDefault(x => x.Id == deptId));
            }
            Principal compToUpdate = context.Principals.FirstOrDefault(x => x.Id == principal.Id);
            compToUpdate = principal;
            context.SaveChanges();
        }

        /// <summary>
        /// Update shipping address of principal 
        /// </summary>
        /// <param name="principal">Object of principal Object to be updates</param>
        /// <param name="shippingAddress">Address object with changes</param>
        public void UpdateShippingAddress(Principal principal, Address shippingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == principal.shippingAddress.Id);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Billing address of principal company
        /// </summary>
        /// <param name="principal">Object of principal Object to be updates</param>
        /// <param name="billingAddress">Address object with changes</param>
        public void UpdateBillingAddress(Principal principal, Address billingAddress)
        {
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == principal.billingAddres.Id);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update shipping address of principal company
        /// </summary>
        /// <param name="principal">principal ID</param>
        /// <param name="shippingAddress">Address Object with changes</param>
        public void UpdateShippingAddress(int principal, Address shippingAddress)
        {
            Principal vendr = context.Principals.FirstOrDefault(x => x.Id == principal);
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendr.shippingAddress.Id);
            addToUpdate = shippingAddress;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Billing address of principal company
        /// </summary>
        /// <param name="principal">principal ID</param>
        /// <param name="billingAddress">Address Object with changes</param>
        public void UpdateBillingAddress(int principal, Address billingAddress)
        {
            Principal vendr = context.Principals.FirstOrDefault(x => x.Id == principal);
            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == vendr.billingAddres.Id);
            addToUpdate = billingAddress;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of principal company based on principal Object
        /// </summary>
        /// <param name="principal">principal Object</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(Principal principal, Contact contact)
        {

            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == principal.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact details of principal company based on principal ID
        /// </summary>
        /// <param name="principal">principal ID</param>
        /// <param name="contact">Contact Object with changes</param>
        public void Update(int principal, Contact contact)
        {
            Principal vendr = context.Principals.FirstOrDefault(x => x.Id == principal);
            Contact ContToUpdate = context.Contacts.FirstOrDefault(x => x.Id == vendr.contact.Id);
            ContToUpdate = contact;
            context.SaveChanges();
        }
        /// <summary>
        /// Update Contact Person details of principal company based on principal Object
        /// </summary>
        /// <param name="principal">principal Object</param>
        /// <param name="contactPerson">Person Object</param>
        public void Update(Principal principal, Person contactPerson)
        {
            Person ContToUpdate = context.Persons.FirstOrDefault(x => x.Id == contactPerson.Id);
            ContToUpdate = contactPerson;
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contact Person details of principal company based on principal ID
        /// </summary>
        /// <param name="principalCompID">principal ID</param>
        /// <param name="person">Person Object</param>
        public void Update(int principalCompID, Person person)
        {
            Principal principal = context.Principals.FirstOrDefault(x => x.Id == principalCompID);
            Person personToUpdate = context.Persons.FirstOrDefault(x => x.Id == principal.contactPerson.Id);
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
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompanies(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }
        public object getUserPrincipal(int currentUserid) 
        {
            var user = context.Users.FirstOrDefault(x => x.id == currentUserid);
            List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.Principals

                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.isActive == true)
                .ToList();

        }
        public Department getParent(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }
    }
}
