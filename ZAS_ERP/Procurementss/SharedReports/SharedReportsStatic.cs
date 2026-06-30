using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Procurementss.SharedReports
{
    public static class SharedReportsStatic
    {
       public static List<Department> departments;
       public static List<Company> companies;
       public static List<ERP_BL.Databases.Employee> employees;
       public static List<Department> selectedDepartments;
       public static List<Company> selectedCompanies;
       public static List<ERP_BL.Databases.Employee> selectedEmployees;

    }
}
