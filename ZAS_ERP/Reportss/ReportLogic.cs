using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Reportss
{
   public class ReportLogic
    {
        
        public static void SaveGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, String labelText, int? titleId)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            //Checking It is "register" or not
            if (labelText == "Loans" || labelText == "Advances" || labelText == "Sale Register" || labelText == "Bill Register" || labelText == "Purchase Register"|| labelText == "Admin Bill Register" || labelText== "Purchase Invoice Register"|| labelText== "Payment Register" || labelText == "Step Register")
            { gridReport.settingkey = labelText; }
            else
            { gridReport.settingkey =labelText; }
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;                
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.titleId = titleId;

            //gridReport.departments = department;
            reportRepo.SaveReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }
        public static void SaveGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, String labelText,Company _company, DateTime from, DateTime to)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = labelText; 
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.company_Id = _company.Id;
            gridReport.from = from;
            gridReport.to = to;
            reportRepo.SaveReport(gridReport);
            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }


        public static void ExportToStandard(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, String labelText)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            //Checking It is "register" or not
            if (labelText == "Sale Register" || labelText == "Bill Register" || labelText == "Purchase Register")
            { gridReport.settingkey = labelText; }
            else
            { gridReport.settingkey = labelText; }
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;

            //gridReport.departments = department;
            reportRepo.SaveReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }

        public static void RenameGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId,string settingKey, int? reporttTitleId)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = Convert.ToInt32(reportId);
            gridReport.titleId = Convert.ToInt32(reporttTitleId);
            //gridReport.departments = department;
            reportRepo.SaveReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }
        public static void RenameGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId, string settingKey, Company _company, DateTime from, DateTime to)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = Convert.ToInt32(reportId);
            gridReport.company_Id = _company.Id;
            gridReport.from = from;
            gridReport.to = to;
            //gridReport.departments = department;
            reportRepo.SaveReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }


        public Stream GetReportbyName(String ReportName)
        {
            Stream obj = null;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            gridReport = reportRepo.GetReportByName(ReportName);
            if (gridReport != null)
            {
                var gridCon = new GridControl();
                obj = ConvertToMemoryStream(gridReport.settingValue);

                //SystemLog.LogInfo(gridCon.Parent.GetType(), "Formate restored for Unbound Report Name = " + gridCon.Name);
            }
            return obj;


        }

        public static string ConvertToString(MemoryStream memoryStream)
        {
            //StreamWriter writer = new StreamWriter(memoryStream);
            //writer.Write(Layoutstream);
            //writer.Flush();
            string Layoutstream = "";
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(memoryStream);
            return Layoutstream = reader.ReadToEnd();


        }

        public static MemoryStream ConvertToMemoryStream(string memoryStream)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(memoryStream);
            MemoryStream stream = new MemoryStream(byteArray);
            //MemoryStream Stream = new MemoryStream();
            //Stream.Position = 0;
            //StreamReader reader = new StreamReader(memoryStream);
            //memoryStream = reader.ReadToEnd();
            return stream;
        }

        /// <summary>
        /// Convert Memory Stream to String 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        /// 

        public static void SaveUserSettingForCurrentWindow(GridControl gridControl, int userId)
        { 
                // do something with tb here
                if (MainWindow.currentUserid == 0)
                    return;
                EmployeeRepo employeeRepo = new EmployeeRepo();
                UserSettings userSetting = new UserSettings();
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                gridControl.SaveLayoutToStream(memoryStream);
                // Need to Add these lines of Code after Setting userId nullable 
                    
                     //if (userId != 0)
                     //   {
                     //       userSetting.userId = MainWindow.currentUserid;
                     //   }
                userSetting.userId = MainWindow.currentUserid;
                userSetting.settingkey = gridControl.Name;
                userSetting.settingValue = ConvertToString(memoryStream).Trim();
                userSetting.lastModified = DateTime.Now;
                employeeRepo.SaveUserSetting(userSetting);
                SystemLog.LogInfo(gridControl.Parent.GetType(), "Settings Saved for current window and Grid Name= " + gridControl.Name);
            
        }

        public static void UpdateGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId,string settingKey)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = reportId;

            //gridReport.departments = department;
            reportRepo.updateReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }
        public static void UpdateGridReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId, string settingKey, Company _company, DateTime from, DateTime to)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = reportId;
            gridReport.company_Id = _company.Id;
            gridReport.from = from;
            gridReport.to = to;

            //gridReport.departments = department;
            reportRepo.updateReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }

        public static void DeleteReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId,string settingKey)
        {
             if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = reportId;

            reportRepo.DeleteReport(gridReport);
        }
        public static void DeleteReport(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId, string settingKey, Company _company, DateTime from, DateTime to)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            GridReport gridReport = new GridReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.gridReportType = type;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.gridReportGroup = group;
            gridReport.group_Id = group.Id;
            gridReport.Id = reportId;

            reportRepo.DeleteReport(gridReport);
        }

        public static void ExportToMemorized(GridControl gridControl, String ReportName, GridReportType type, GridReportGroup group, int reportId,string settingKey)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;  
                GridReportRepo reportRepo = new GridReportRepo();
                GridReport gridReport = new GridReport();
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                gridControl.SaveLayoutToStream(memoryStream);
                gridReport.settingkey = settingKey;
                gridReport.reportName = ReportName.Trim();
                gridReport.settingValue = ConvertToString(memoryStream).Trim();
                gridReport.lastModified = DateTime.Now;
                gridReport.gridReportType = type;
                gridReport.userId = SYSTEM_STATIC.currentUser.id;
                gridReport.gridReportGroup = group;
                gridReport.group_Id = group.Id;
                gridReport.Id = reportId;

                //gridReport.departments = department;
                reportRepo.ExportToMemorized(gridReport);

                SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);

            
            
        }
        public static void UpdateGridReport(GridControl gridControl, String ReportName, SharedGridGroup group, int reportId, string settingKey)
        {
            if (SYSTEM_STATIC.currentUser.id == 0)
                return;
            GridReportRepo reportRepo = new GridReportRepo();
            SharedReport gridReport = new SharedReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            gridReport.settingkey = settingKey;
            gridReport.reportName = ReportName.Trim();
            gridReport.settingValue = ConvertToString(memoryStream).Trim();
            gridReport.lastModified = DateTime.Now;
            gridReport.userId = SYSTEM_STATIC.currentUser.id;
            gridReport.SharedGridGroup = group;
            gridReport.sharedGroupId = group.Id;
            gridReport.Id = reportId;
            //gridReport.departments = department;
            reportRepo.updateReport(gridReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for GridReport Report Name = " + gridControl.Name);
        }


    }
}
