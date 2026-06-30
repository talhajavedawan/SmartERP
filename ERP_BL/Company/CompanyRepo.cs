using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Config;
using ERP_BL.Databases;
namespace ERP_BL.Databases
{
   public class CompanyRepo 
    {
        
         DBContextERP context = new DBContextERP();
        //private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public CompanyRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created! ");
            NLog.Logger logge = NLog.LogManager.GetCurrentClassLogger();
            logge.Info("Hello {0}", "Earth");
        }
        /// <summary>
        /// Get list of all companies in DB
        /// </summary>
        /// <returns>List of Company Objects</returns>
        /// 
        public List<ERP_BL.Databases.Company> GetCompanieswithGroupCompany()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive List of All Companies with Group Company ");
            return context.Companies
                .Where(s => s.compnayType ==Enums.CompnayTypes.Company || s.compnayType== Enums.CompnayTypes.Group)
                .ToList();
        }
        /// <summary>
        /// Get list of all companies in DB
        /// </summary>
        /// <returns>List of Company Objects</returns>
        public List<ERP_BL.Databases.Company> GetCompanies()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Companies other than Group Companies ");
            return context.Companies
                .Where(s => s.compnayType == Enums.CompnayTypes.Company)
                .ToList();
        }
        /// <summary>
        /// Get list of all companies in DB
        /// </summary>
        /// <returns>List of Company Objects</returns>
        public List<ERP_BL.Databases.Company> GetActiveCompanies()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Active Companies other than Group Companies ");
            return context.Companies
                .Where(s => s.compnayType == Enums.CompnayTypes.Company && s.isActive==true)
                .ToList();
        }
        public List<ERP_BL.Databases.Company> GetActiveCompaniesByEmpl(int EmpId)
        {
            var employee = context.Employees.FirstOrDefault(x => x.EmpId == EmpId);
            return employee.Companies
                .Where(s => s.compnayType == Enums.CompnayTypes.Company && s.isActive == true)
                .ToList();
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
        /// Get Group company 
        /// </summary>
        /// <returns>Single Company Object</returns>
        public ERP_BL.Databases.Company GetGroupCompany()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Group Companies ");

            return context.Companies.FirstOrDefault(x => x.compnayType == Enums.CompnayTypes.Group);
        }

        /// <summary>
        /// Get single company against specified company ID
        /// </summary>
        /// <param name="CompID">Company ID</param>
        /// <returns>Single Company Object</returns>
        public ERP_BL.Databases.Company GetCompany(int CompID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Company having Id=("+CompID+")" );

            return context.Companies

                .FirstOrDefault(x => x.Id == CompID);
        }

        /// <summary>
        /// Get single company against specified company ID
        /// </summary>
        /// <param name="CompID">Company ID</param>
        /// <returns>Single Company Object</returns>
        public ERP_BL.Databases.Company GetCompanyForAdminBills(int CompID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Company having Id=(" + CompID + ")");

            return context.Companies
               
                .FirstOrDefault(x => x.Id == CompID);
        }

        /// <summary>
        /// get list of all departments mapped to the specific company
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public List<Department> GetDepartments(int CompID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Department in a Company having Company Id=(" + CompID + ")");

            Company compForDept = context.Companies.FirstOrDefault(x => x.Id == CompID);
            return compForDept.departments.ToList(); 
        }

        /// <summary>
        /// Get list of Companies Order by Company Name
        /// </summary>
        /// <returns>List of Company Objects</returns>
        public List<ERP_BL.Databases.Company> GetCompaniesOrderByName()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived companies Ordered By Name");
            return context.Companies.OrderBy(x=>x.CompanyName).ToList();
        }


        /// <summary>
        /// return all employees of a company
        /// </summary>
        /// <param name="company">Company whose employees required</param>
        /// <returns>List of Employee Object</returns>
        public List<ERP_BL.Databases.Employee> GetCompanyEmployees(Company company)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Employees in a Company having Company Id=(" + company.Id + ") Name =" +company.CompanyName);
            Company compForEmployees = context.Companies.FirstOrDefault(x => x.Id == company.Id);
            return compForEmployees.employees.ToList();
            
        }

        /// <summary>
        /// Get/Find specific Employee for/inside specific compnay
        /// </summary>
        /// <param name="company">Company object from which you are looking for employee profile</param>
        /// <param name="EmpID">Employee ID whose's profile is required</param>
        /// <returns>Employee Object</returns>
        public ERP_BL.Databases.Employee GetCompanyEmployee(Company company, int EmpID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Employee Having Id=("+EmpID +")  in a Company having Company Id=(" + company.Id + ") Name =" + company.CompanyName);

            Company compForEmployees = context.Companies.FirstOrDefault(x => x.Id == company.Id);
            return compForEmployees.employees.FirstOrDefault(x=>x.EmpId==EmpID);

        }
        /// <summary>
        /// Add Subsidary Company based to parent company object
        /// </summary>
        /// <param name="groupComp">Company Object as Group or parent Company</param>
        /// <param name="subsidaryComp">Company Object as Subsidary Company</param>
        public void Add(Company groupComp, Company subsidaryComp)
        {
            subsidaryComp.parentCompany = groupComp;
            context.Companies.Add(subsidaryComp);
            context.SaveChanges();
        }

        /// <summary>
        /// Add subsidary company based on parent company ID
        /// </summary>
        /// <param name="parentCompId">parent Company ID</param>
        /// <param name="subsidaryComp">Company Object as Subsidary company </param>
        public void Add(int parentCompId, Company subsidaryComp)
        {
            Company parentComp = context.Companies.FirstOrDefault(x => x.Id == parentCompId);
            subsidaryComp.parentCompany = parentComp;
            context.Companies.Add(subsidaryComp);
            context.SaveChanges();
        }

        /// <summary>
        /// return all Company list.
        /// </summary>
        /// <returns></returns>
        public List<Company> getAll()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Companies other than Group Companies ");

            return context.Companies
               .Where(s => s.compnayType == Enums.CompnayTypes.Company || s.compnayType == Enums.CompnayTypes.Group)
                .ToList();
        }

        /// <summary>
        /// get Company matching to ID
        /// </summary>
        /// <param name="CompID">Company ID</param>
        /// <returns></returns>
        public CustomerCompany get(int customerCompID)
        {
            return context.customerCompanies


                .FirstOrDefault(x => x.Id == customerCompID);
        }
      

        /// <summary>
        /// Get all subsidaries related to Group Company
        /// </summary>
        /// <param name="CompID"> Company ID </param>
        /// <returns></returns>
        public List<Company> getSubsidaries(int CompID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Subsidary Companies id ="+CompID);

            return context.Companies

                .Where(x => x.ParentID == CompID).ToList();
        }
        /// <summary>
        /// Get all subsidaries related to Group Company
        /// </summary>
        /// <param name="parentcompany"> Company object </param>
        /// <returns></returns>
        public List<Company> getSubsidaries(Company parentcompany)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Subsidary Companies id =" + parentcompany.CompanyName+ parentcompany.Id);
            return context.Companies
 
                .Where(x => x.ParentID == parentcompany.Id).ToList();
        }


        /// <summary>
        /// Get list of addresses for any company
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <returns>List of Adress Objects</returns>
        public List<ERP_BL.Databases.Address> GetAddresses(Company company)
        {
            return context.Address.Where(s => s.Id == company.addressId).ToList();
                //context.Companies.OrderBy(x => x.CompanyName).ToList();
        }


        /// <summary>
        /// Get list single address agaisnt address ID
        /// </summary>
        /// <param name="addressID">Address ID</param>
        /// <returns>Single Address Object</returns>
        public ERP_BL.Databases.Address GetAddresses(int addressID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Adress by id =" + addressID);

            var addresses = context.Address.ToList();
            return addresses.Single(x => x.Id == addressID);
        }

        /// <summary>
        /// Get list of contact of any company
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <returns>List of Contact Onjects</returns>
        public List<ERP_BL.Databases.Contact> GetContacts(Company company)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Contacts of Company Name=" + company.CompanyName + company.Id);
            return context.Contacts.Where(s => s.Id == company.contactId).ToList();
            //context.Companies.OrderBy(x => x.CompanyName).ToList();
        }

        /// <summary>
        /// Get single Contact againt contact ID
        /// </summary>
        /// <param name="contactID">Contact ID </param>
        /// <returns>Single Contact Object</returns>
        public ERP_BL.Databases.Contact GetContacts(int contactID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Contact by Id=" + contactID);
            var contacts = context.Contacts.ToList();
            return contacts.Single(x => x.Id == contactID);
        }

        /// <summary>
        /// Add new company in DB
        /// </summary>
        /// <param name="company">Company Object</param>
        public void addCompany(Company company)
        {
            
            context.Companies.Add(company);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "New Company Added with Name=" + company.CompanyName +"Id="+ company.Id);
        }

        /// <summary>
        /// update company information
        /// </summary>
        /// <param name="company">Company Object</param>
        public void updateCompany(Company company)
        {
            //context = new DBContextERP();
            Company compToUpdate = context.Companies.FirstOrDefault(x => x.Id == company.Id);

            compToUpdate = company;
 
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Company Name=" + company.CompanyName + company.Id);
        }

        /// <summary>
        /// Update company address details
        /// </summary>
        /// <param name="address">address object</param>
        public void updateCompany(Address address)
        {

            Address addToUpdate = context.Address.FirstOrDefault(x => x.Id == address.Id);
            addToUpdate = address;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Adress Id=" + address.Id);
        }

        /// <summary>
        /// Update company contact details
        /// </summary>
        /// <param name="contact">Contact Object</param>
        public void updateCompany(Contact contact)
        {
            Contact contToUpdate = context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            contToUpdate = contact;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Contact Id=" +contact.Id);
        }

        /// <summary>
        /// Get all IndustryTypes
        /// </summary>
        /// <returns>List of IndustryTypes Objects</returns>
        public List<ERP_BL.Databases.IndustryType> GetIndustryTypes()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=" );
            return context.IndustryTypes.Where(x=>x.isVoid!=true).ToList();

        }
        public List<ERP_BL.Databases.VendorNature> GetIndustryTypesManual()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=" );
            return context.vendorNatures.Where(x=>x.isVoid!=true).ToList();

        }

        
        public List<VendorNatureManual> GetIndustryTypesManuals()
        {
            return context.vendorNaturesManual.Where(x => x.isVoid != true).ToList();

        }
        public List<ERP_BL.Databases.IndustryType> GetVoidIndustryTypes()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.IndustryTypes.Where(x=>x.isVoid==true).ToList();

        } 
        public List<ERP_BL.Databases.VendorNature> GetVoidVendorNature()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.vendorNatures.Where(x=>x.isVoid==true).ToList();

        }
        public List<ERP_BL.Databases.VendorNatureManual> GetVoidVendorNatureManual()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.vendorNaturesManual.Where(x => x.isVoid == true).ToList();

        }

        /// <summary>
        /// Get all IndustryTypes of Type Vendor
        /// </summary>
        /// <returns>List of IndustryTypes Objects</returns>
        public List<ERP_BL.Databases.IndustryType> GetVendorIndustryTypes()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.IndustryTypes.Where(x=>x.isVendorType == true && x.isVoid!=true)
                .ToList();

        } 
        public List<ERP_BL.Databases.VendorNature> GetVendorNatures()
        {
            return context.vendorNatures.Where(x=> x.isVoid!=true)
                .ToList();

        }

        /// <summary>
        /// Get  IndustryType  by Id
        /// </summary>
        /// <param name="industryTypeid"></param>
        /// <returns>List of IndustryTypes Objects</returns>
        public ERP_BL.Databases.IndustryType GetIndustryType(int industryTypeid)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Industry Type by Id=" + industryTypeid);
            return context.IndustryTypes.FirstOrDefault(x => x.Id == industryTypeid); ;
        } 
        public ERP_BL.Databases.VendorNature GetVendorNatures(int industryTypeid)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Industry Type by Id=" + industryTypeid);
            return context.vendorNatures.FirstOrDefault(x => x.Id == industryTypeid); 
        }
        public ERP_BL.Databases.VendorNature GetVendorNature(int industryTypeid)
        {
            return context.vendorNatures.FirstOrDefault(x => x.Id == industryTypeid);
        }
        public List< ERP_BL.Databases.VendorNatureManual> GetVendorNaturesManuals()
        {
            return context.vendorNaturesManual.Where(x => x.isActive == true).ToList();
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
        public List<ERP_BL.Databases.Company> GetUserCompaniesForChartofAccount(int id)
        {
            var user = context.Users               
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }

        /// <summary>
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompaniesForBillNature(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }


        /// <summary>
        /// Get all User Admin Bill Companies
        /// </summary>
        /// <returns>List of Admin Bill Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserAdminBillCompanies(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.AdminBillCompanies;
        }

        /// <summary>
        /// Get  IndustryType by Id
        /// </summary>
        /// <returns>IndustryType Object</returns>
        public void updateIndustryType(IndustryType industryType)
        {
            IndustryType industry = new IndustryType();
            industry = industryType;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Industry Type  Id=" + industryType.Id);

        }   
        public void updateVendorNatures(VendorNature industryType)
        {
            VendorNature industry = new VendorNature();
            industry = industryType;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Industry Type  Id=" + industryType.Id);

        } 
        public void updateVendorNaturesManual(VendorNatureManual industryType)
        {
            VendorNatureManual industry = new VendorNatureManual();
            industry = industryType;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Industry Type  Id=" + industryType.Id);

        }
        /// <summary>
        /// Add new company in DB
        /// </summary>
        /// <param name="company">Company Object</param>
        public void addIndustryType(IndustryType industryType)
        {
            context.IndustryTypes.Add(industryType);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added New Industry Type  Id=" + industryType.Id);

        }
        public void addVendorNatures(VendorNature industryTypeManual)
        {
            context.vendorNatures.Add(industryTypeManual);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added New Industry Type  Id=" + industryTypeManual.Id);

        }
        public void addVendorNaturesManual(VendorNatureManual industryTypeManual)
        {
            context.vendorNaturesManual.Add(industryTypeManual);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added New Industry Type  Id=" + industryTypeManual.Id);

        }
        public object GetAllCustomers()
        {
            return context.customerCompanies

                .Where(x=>x.company.compnayType == Enums.CompnayTypes.CustomerCompany)
                .ToList();
        }

        public object GetAllEmployees()
        {
            return context.Employees.ToList();
        }

     public List<Employee> getAdminBillEmployee()
        {
            return context.Employees.ToList();
        }
        public Company GetCompanyByName(string companyName)
        {
            return context.Companies.FirstOrDefault(x=>x.CompanyName==companyName&& x.isActive==true);
        }
        public List<Principal> getPrinciplesByDepartment(int deptId)
        {
           var dbDepartment = context.Departments.FirstOrDefault(x => x.Id == deptId);
            return dbDepartment.Principals;

        }
        public Department getParent(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public List<ERP_BL.Databases.Company> GetAllActiveLinkAble()
        {
            return context.Companies.Where(x => x.isActive == true && x.isLinkable == true).ToList();
        }
    }
}
