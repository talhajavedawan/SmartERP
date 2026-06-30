using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class ReportRepo
    {
        DBContextERP context = new DBContextERP();
        /// <summary>
        /// add Report to db
        /// </summary>
        /// <param name="Report"></param>
        public void AddReport(Report Report)
        {
            context.Reports.Add(Report);
            context.SaveChanges();
        }
        /// <summary>
        /// add Report to db
        /// </summary>
        /// <param name="Report"></param>
        public void updateReport(Report Report)
        {
            Report useer = new Report();
            useer = Report;
            context.SaveChanges();
        }
        ///<summary>
        ///Get All Reports 
        /// </summary>


        public List<Report> GetALLReports()
        {
            return context.Reports.ToList();

        }
        ///<summary>
        ///Get current Report Settings
        /// </summary>
        /// <param name="ReportId"> Report Id</param>

        public List<Report> GetReportsByReportId(int ReportId)
        {
            return context.Reports.Where(x => x.Id == ReportId).ToList();

        }
        ///<summary>
        ///Get current Report Settings
        /// </summary>
        /// <param name="ReportId"  > Report Id</param>

        public Report GetReportByName(string ReportName)
        {
            return context.Reports.FirstOrDefault(x => x.ReportName == ReportName);
        }

        ///<summary>
        ///Get current Report Settings
        /// </summary>
        /// <param name="ReportId"  > Report Id</param>

        public Report GetReportById(int ReportId)
        {
            return context.Reports.FirstOrDefault(x => x.Id == ReportId);
        }
        ///<summary>
        ///Get current Report Settings
        /// </summary>
        /// <param name="ReportId"  > Report Id</param>
        /// <param name="reportName">Setting Key</param>
        public Report GetReportsByReport(int ReportId, string reportName)
        {
            return context.Reports.FirstOrDefault(x => x.Id == ReportId && x.ReportName == reportName);
        }
        ///<summary>
        ///Save Report Settings Against Report
        /// </summary>
        /// <param name="Report"> Report Object</param>
        public void SaveReport(Report Report)
        {
            Report ReportSet = new Report();
            ReportSet = context.Reports.FirstOrDefault(x => x.ReportName == Report.ReportName && x.groupId == Report.groupId);
            // Check if the Settings Already Exist in database for current element and Report
            if (ReportSet != null)
            { //if true Update the value 
                ReportSet.ReportDesign = Report.ReportDesign;
                ReportSet.lastModified = System.DateTime.Now;
                context.SaveChanges();

            }
            else
            {
                //Create a new Report Setting

                context.Reports.Add(Report);
                context.SaveChanges();
            }

        }
        ///<summary>
        ///Update Report Settings Against Report
        /// </summary>
        /// <param name="Report"> Report Object</param>
        public void UpdateReport(Report Report)
        {

            {

                Report ReportSet = new Report();
                ReportSet = Report;
                context.SaveChanges();
            }

        }

        // <summary>
        /// add ReportGroup to db
        /// </summary>
        /// <param name="ReportGroup"></param>
        public void AddReportGroup(ReportGroup Report)
        {
            context.ReportGroups.Add(Report);
            context.SaveChanges();
        }
        /// <summary>
        /// add Report to db
        /// </summary>
        /// <param name="Report"></param>
        public void UpdateReportGroup(ReportGroup Reportgroup)
        {
            ReportGroup useer = new ReportGroup();
            useer = Reportgroup;
            context.SaveChanges();
        }
        ///<summary>
        ///Get All ReportGroups 
        /// </summary>


        public List<ReportGroup> GetALLReportGroups()
        {
            return context.ReportGroups.ToList();

        }
        ///<summary>
        ///Get current Report Group
        /// </summary>
        /// <param name="ReportGroupId"> Report Group Id</param>

        public ReportGroup GetReportGroup(int ReportGroupId)
        {
            return context.ReportGroups.FirstOrDefault(x => x.Id == ReportGroupId);

        }

        // /// /// unbound Report 
        ///<summary>
        ///Get current UnBoundReports
        /// </summary>
        /// <param name="UserId"> User Id</param>

        public List<UnBoundReport> GetUnBoundReportByUserId(int UserId)
        {
            return context.UnBoundReport.Where(x => x.userId == UserId).ToList();

        }
        ///<summary>
        ///Get current UnBoundReports
        /// </summary>
        /// <param name="UserId"  > User Id</param>
        /// <param name="settingKey">Setting Key</param>
        public UnBoundReport GetUnBoundReportByUser(int UserId, string settingKey)
        {
            return context.UnBoundReport.FirstOrDefault(x => x.userId == UserId && x.reportName == settingKey);
        }
        ///<summary>
        ///Save UnBoundReports Against User
        /// </summary>
        /// <param name="unBoundReport"> UnBoundReport Object</param>
        public void SaveUnBoundReport(UnBoundReport unBoundReport)
        {
            UnBoundReport userSet = new UnBoundReport();
            userSet = context.UnBoundReport.FirstOrDefault(x => x.reportName == unBoundReport.reportName && x.userId == unBoundReport.userId);
            // Check if the Settings Already Exist in database for current element and user
            if (userSet != null)
            { //if true Update the value 
                userSet.template = unBoundReport.template;
                userSet.lastModified = System.DateTime.Now;
                context.SaveChanges();

            }
            else
            {
                //Create a new user Setting

                context.UnBoundReport.Add(unBoundReport);
                context.SaveChanges();
            }

        }
        ///<summary>
        ///Add UnBoundReports Against User
        /// </summary>
        /// <param name="unBoundReport"> UnBoundReport Object</param>
        public void AddUnBoundReport(UnBoundReport unBoundReport)
        {

            context.UnBoundReport.Add(unBoundReport);
            context.SaveChanges();


        }
        ///<summary>
        ///Update UnBoundReports Against User
        /// </summary>
        /// <param name="unBoundReport"> UnBoundReport Object</param>
        public void UpdateUnBoundReport(UnBoundReport unBoundReport)
        {

            {

                UnBoundReport userSet = new UnBoundReport();
                userSet = unBoundReport;
                context.SaveChanges();
            }

        }
    }
}