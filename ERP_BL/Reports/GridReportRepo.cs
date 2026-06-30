//using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;

namespace ERP_BL.Reports
{
    public class GridReportRepo
    {
        DBContextERP context = new DBContextERP();

        //------------------------Grid Reports Work---------------------------Grid Reports Work------------------------------Grid Reports Work-------------------
        public void AddReport(GridReport Report)
        {
            context.GridReports.Add(Report);
            context.SaveChanges();
        }

        public void updateReport(GridReport Report)
        {
            GridReport dbReport = new GridReport();
            if (Report != null)
            {
                dbReport = context.GridReports.FirstOrDefault(x => x.Id == Report.Id);
                //dbReport.settingkey = Report.settingkey;
                dbReport.settingValue = Report.settingValue;
                dbReport.lastModified = DateTime.Now;
            }
            context.SaveChanges();
        }
        public void MarkFavouriteReport(GridReport Report)
        {
            GridReport dbReport = new GridReport();
            dbReport = context.GridReports.FirstOrDefault(x => x.Id == Report.Id);
            dbReport.isFavourite = Report.isFavourite;
            context.SaveChanges();
        }
        public void updateReport(SharedReport Report)
        {
            SharedReport dbReport = new SharedReport();
            if (Report != null)
            {
                dbReport = context.sharedReports.FirstOrDefault(x => x.Id == Report.Id);
                //dbReport.settingkey = Report.settingkey;
                dbReport.settingValue = Report.settingValue;
                dbReport.lastModified = DateTime.Now;
            }
            context.SaveChanges();
        }

        public List<GridReport> GetALLReports()
        {
            var reports = context.GridReports;
            if (reports == null || reports.Count() == 0)
            {
                return null;
            }
            return reports.ToList();
        }

        public List<GridReport> GetALLReportsbyGroupId(int groupId)
        {
            var gridReports = context.GridReports.Where(x => x.group_Id == groupId).Distinct().ToList();
            return gridReports;
        }

        public GridReport GetReportsByReportId(int ReportId)
        {
            return context.GridReports.FirstOrDefault(x => x.Id == ReportId);
        }

        public GridReport GetGridReportByGroupId(int groupId)
        {
            return context.GridReports.FirstOrDefault(x => x.group_Id == groupId);
        }

        public GridReport GetReportByName(string ReportName)
        {
            return context.GridReports.FirstOrDefault(x => x.reportName == ReportName);
        }
        public GridReport GetReportByNameAndUserId(string ReportName, int userId)
        {

            return context.GridReports.FirstOrDefault(x => x.reportName == ReportName && x.userId == userId && x.gridReportGroup.userId == userId && x.gridReportGroup.gridReportType == GridReportType.MemorizedReport);
        }
        public List<GridReport> GetAllFavouriteReports(int userId)
        {

            return context.GridReports.Where(x => x.userId == userId && x.gridReportGroup.userId == userId && x.gridReportGroup.gridReportType == GridReportType.MemorizedReport && x.isFavourite==true).ToList();
        }

        public GridReport GetReportById(int ReportId)
        {
            return context.GridReports.FirstOrDefault(x => x.Id == ReportId);
        }

        public void SaveReport(GridReport Report)
        {
            var ReportSet = context.GridReports.FirstOrDefault(x => x.Id == Report.Id);

            if (ReportSet == null)
                ReportSet = new GridReport();

            // Check if the Settings Already Exist in database for current element and Report
            if (Report.titleId != 0)
                ReportSet.titleId = Report.titleId;
            if (Report.Creater != null)
                Report.Creater = context.Users.FirstOrDefault(x => x.id == Report.Creater.id);
            if (Report.gridReportGroup != null)
                Report.group_Id = context.GridReportGroups.FirstOrDefault(x => x.Id == Report.gridReportGroup.Id) == null ? 0 : context.GridReportGroups.FirstOrDefault(x => x.Id == Report.gridReportGroup.Id).Id;
            if (Report.Id != null && Report.Id != 0)
            {
                //var rprt = context.GridReports.FirstOrDefault(x => x.Id == Report.Id); 
                ReportSet.reportName = Report.reportName;
                ReportSet.lastModified = System.DateTime.Now;
            
                context.SaveChanges();
            }
            else if (ReportSet != null && ReportSet.Id != 0)
            {
                //if true Update the value  
                ReportSet.gridReportGroup = context.GridReportGroups.FirstOrDefault(x => x.Id == Report.gridReportGroup.Id);
                ReportSet.lastModified = System.DateTime.Now;
                context.SaveChanges();
            }
            else
            {
                //Create a new Report Setting                             
                try
                {
                    //ReportSet.gridReportGroup = context.GridReportGroups.FirstOrDefault(x => x.Id == Report.gridReportGroup.Id);
                    Report.gridReportGroup = null;
                    //Report.groupId = null
                    Report.Id = 1;
                    if (Report.titleId != 0)
                        ReportSet.titleId = Report.titleId;
                    context.GridReports.Add(Report);
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }

        public GridReport GetGridReports(string settingKey)
        {
            return context.GridReports.FirstOrDefault(x => x.reportName == settingKey);
        }

        public void DeleteReport(GridReport report)
        {
            var reporToDelete = new GridReport();
            reporToDelete = context.GridReports.FirstOrDefault(x => x.Id == report.Id);
            context.GridReports.Remove(reporToDelete);
            context.SaveChanges();
        }

        public void ExportToMemorized(GridReport Report)
        {
            GridReport dbReport = new GridReport();
            if (Report != null)
            {
                dbReport = context.GridReports.FirstOrDefault(x => x.Id == Report.Id);
                dbReport.gridReportType = Report.gridReportType;
                dbReport.group_Id = Report.gridReportGroup.Id;
                dbReport.userId = SystemLog.CurrentUserId;
                dbReport.lastModified = DateTime.Now;
            }
            context.SaveChanges();
        }

        public bool GetReportByNameAndUserId(GridReport Report)
        {
            var report = context.GridReports.FirstOrDefault(x => x.Id == Report.Id && x.userId == SystemLog.CurrentUserId && x.gridReportType == GridReportType.MemorizedReport && x.reportName == Report.reportName && x.group_Id == Report.group_Id);
            if (report != null)
                return true;
            else
                return false;

        }
        //------------------------Grid Report Groups Work---------------------------Grid Report Groups Work------------------------------Grid Report Groups Work-------------------

        public List<GridReportGroup> GetAllStandardGroups()
        {
            var standardReportGroups = context.GridReportGroups.Where(x => x.gridReportType == GridReportType.StandardReport).ToList();
            return standardReportGroups;
        }

        public List<GridReportGroup> GetAllMemorizedGroups(int userId)
        {
            var memorizedReportGroups = context.GridReportGroups.Where(x => x.gridReportType == GridReportType.MemorizedReport && x.userId==userId).ToList();
            return memorizedReportGroups;
        }

        public void AddGridReportGroup(GridReportGroup group)
        {
            group.parent = null;
            group.userId = SystemLog.CurrentUserId;
            context.GridReportGroups.Add(group);
            context.SaveChanges();
        }

        public void UpdateReportGroup(GridReportGroup reportgroup)
        {
            if (reportgroup.Id != null || reportgroup.Id != 0)
            {
                var gridReports = context.GridReportGroups.FirstOrDefault(x => x.Id == reportgroup.Id);
                gridReports.groupName = reportgroup.groupName;
                context.SaveChanges();
            }
        }

        public List<GridReportGroup> GetALLReportGroups()
        {
            return context.GridReportGroups.ToList();
        }

        public GridReportGroup GetReportGroupByGroupId(int groupId)
        {
            return context.GridReportGroups.FirstOrDefault(x => x.Id == groupId);
        }

        public GridReportGroup GetReportGroup(int ReportGroupId)
        {
            return context.GridReportGroups.FirstOrDefault(x => x.Id == ReportGroupId);
        }

        public void DeleteGroup(GridReportGroup group)
        {
            var groupToDelete = new GridReportGroup();
            groupToDelete = context.GridReportGroups.FirstOrDefault(x => x.Id == group.Id);
            context.GridReportGroups.Remove(groupToDelete);
            context.SaveChanges();
        }
        public void AddSharedGroup(SharedGridGroup group)
        {
            List<Company> companies = new List<Company>();
            List<Department> departments = new List<Department>();




            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();

            if (group.Companies != null)
                foreach (Company company in group.Companies)
                {
                    var comp = context.Companies.FirstOrDefault(x => x.Id == company.Id);
                    companies.Add(comp);
                }
            if (group.Departments != null)
                foreach (Department department in group.Departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == department.Id);
                    departments.Add(dept);

                }
            if (group.Employees != null)
                foreach (ERP_BL.Databases.Employee employee in group.Employees)
                {
                    var emp = context.Employees.FirstOrDefault(x => x.EmpId == employee.EmpId);
                    employees.Add(emp);
                }
            group.Companies = companies;
            group.Departments = departments;
            group.Employees = employees;
            context.sharedGridGroups.Add(group);
            context.SaveChanges();
        }

        public void UpdateSharedGroup(SharedGridGroup group)
        {
            var dbGroup = context.sharedGridGroups.FirstOrDefault(x => x.Id == group.Id);
            List<Company> companies = new List<Company>();
            List<Department> departments = new List<Department>();
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            if (group.Companies != null)
                foreach (Company company in group.Companies)
                {
                    var comp = context.Companies.FirstOrDefault(x => x.Id == company.Id);
                    companies.Add(comp);
                }
            if (group.Departments != null)
                foreach (Department department in group.Departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == department.Id);
                    departments.Add(dept);
                }
            if (group.Employees != null)
                foreach (ERP_BL.Databases.Employee employee in group.Employees)
                {
                    var emp = context.Employees.FirstOrDefault(x => x.EmpId == employee.EmpId);
                    employees.Add(emp);
                }
            group.Companies = companies;
            group.Departments = departments;
            group.Employees = employees;
            if (companies.Count == 0)
                group.Companies = null;
            if (departments.Count == 0)
                group.Departments = null;
            if (employees.Count == 0)
                group.Employees = null;

            dbGroup.parentId = group.parentId;
            dbGroup.groupName = group.groupName;
            context.SaveChanges();
        }
        public List<SharedGridGroup> GetAllSharedGroups()
        {
            return context.sharedGridGroups.Where(x => x.isVoid != true).ToList();
        }
        public SharedGridGroup GetSharedGroup(int groupId)
        {
            return context.sharedGridGroups.FirstOrDefault(x => x.Id == groupId);

        }
        public void AddSharedReport(SharedReport report)
        {
            context.sharedReports.Add(report);
            context.SaveChanges();
        }
        public SharedReport GetSharedReportByName(string ReportName)
        {
            return context.sharedReports.FirstOrDefault(x => x.reportName == ReportName);
        }
        public void DeleteSharedReport(int reportId)
        {
            var dbReport = context.sharedReports.FirstOrDefault(x => x.Id == reportId);
            context.sharedReports.Remove(dbReport);
            context.SaveChanges();
        }
        public SharedReport GetSharedReport(int id)
        {
            return context.sharedReports.FirstOrDefault(x => x.Id == id);
        }
        public void RenameReport(SharedReport Report)
        {
            SharedReport dbReport = new SharedReport();
            if (Report != null)
            {
                dbReport = context.sharedReports.FirstOrDefault(x => x.Id == Report.Id);
                dbReport = Report;
            }
            context.SaveChanges();
        }
        public List<GridReportGroup> GetGroupsByTransactionType(ReportTransactionType type, GridReportType groupType)
        {
            return context.GridReportGroups.Where(x => x.transactionType == type && x.gridReportType==groupType).ToList();
        }
        public List<GridReportGroup> GetGroupsByTransactionType(ReportTransactionType type, GridReportType groupType, int userId)
        {
            return context.GridReportGroups.Where(x => (x.transactionType == type && x.gridReportType == groupType && x.userId== userId) || (x.transactionType == type && x.gridReportType == groupType && x.parentId==null)).ToList();
        }
        public void SaveGroup(GridReportGroup group)
        {
            context.GridReportGroups.Add(group);
            context.SaveChanges();
        }
        public void UpdateGroup(GridReportGroup group)
        {
            var dbGroup=context.GridReportGroups.FirstOrDefault(x=>x.Id== group.Id);

            dbGroup.parentId = group.parentId;
            dbGroup.userId = group.userId;
            dbGroup.groupName = group.groupName;
            dbGroup.isActive = group.isActive;
            dbGroup.gridReportType = group.gridReportType;
            dbGroup.transactionType = group.transactionType;
            //dbGroup = group;
            context.SaveChanges();
        } 
        public List<ReportTitle> GetReportStandardTitles()
        {
            return context.reportTitles.Where(x=>x.gridReportType==GridReportType.StandardReport).ToList();

        }
        public List<ReportTitle> GetReportMemorizedTitles(int userId)
        {
            return context.reportTitles.Where(x => x.gridReportType == GridReportType.MemorizedReport && x.userId== userId).ToList();
        }
        public ReportTitle GetReportTitle(int _titleId)
        {
            return context.reportTitles.FirstOrDefault(x => x.Id == _titleId);
            
        }
        public void UpdateReportTitle(ReportTitle title)
        {
            var dbTitle= context.reportTitles.FirstOrDefault(x => x.Id == title.Id);
            dbTitle = title;
            context.SaveChanges();
        }
        public void SaveReportTitle(ReportTitle title)
        {
            context.reportTitles.Add(title);
            context.SaveChanges();
        } 
       
    }
}
