using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.Reports.UserDesigner.Native;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;

namespace ZAS_ERP.Reportss
{
    public class ReportStorage : IReportStorage
    {
        public ReportStorage()
        {

        }

        const string fileName = "ReportStorage.xml";
        ReportRepo ReportRepo = new ReportRepo();
        IList<Report> ReportStore = new List<Report>();
        ReportGroup group = new ReportGroup();
        int? groupId;
        public bool CanCreateNew()
        {
            if (MainWindow.currentUserid == 0)
                return true;
            else
                return false;
        }

        public bool CanOpen()
        {
            if (MainWindow.currentUserid == 0)
                return true;
            else
                return false;
        }

        public XtraReport CreateNew()
        {
            return new XtraReport();
        }

        public XtraReport CreateNewSubreport()
        {
            throw new NotImplementedException();
        }

        public string GetErrorMessage(Exception exception)
        {
            return ExceptionHelper.GetInnerErrorMessage(exception);
        }

        public XtraReport Load(string reportID, IReportSerializer designerReportSerializer)
        {
            Report report = ReportRepo.GetReportByName(reportID);
            //SystemLogic.SetReport(report);
            groupId = report.groupId;
            using (MemoryStream ms = SYSTEM_STATIC.ConvertToMemoryStream(report.ReportDesign))
            {
                return XtraReport.FromStream(ms, true);
            }
        }

        public string Open(IReportDesignerUI designer)
        {

            StorageEditorForm form = CreateForm();
            form.Owner = Window.GetWindow(designer as DependencyObject);
            form.textBox1.IsEnabled = false;
            bool? result = form.ShowDialog();
            if (result.HasValue && result.Value)
                return (string)form.textBox1.Tag;
            else return string.Empty;
        }

        public string Save(string reportID, IReportProvider reportProvider, bool saveAs, string reportTitle, IReportDesignerUI designer)
        {
            try
            {
                XtraReport report = reportProvider.GetReport();
                if (MainWindow.currentUserid != 0)
                {
                    SYSTEM_STATIC.SaveLayoutForCurrentReport(report);
                    return null;
                }

                //XtraReport report = reportProvider.GetReport();

                if (reportID == null)
                {
                    report.Name = reportTitle;
                    reportID = Guid.NewGuid().ToString();
                    saveAs = true;
                }
                if (!saveAs)
                {
                    SetData(reportID, reportTitle, report);
                    //SystemLogic.SaveLayoutForCurrentReport(report);
                }
                else
                {
                    if (ShowSaveAsDialog(ref reportTitle, designer))
                    {
                        SetData(reportID, reportTitle, report);
                    }
                    else return null;
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                
                //MessageBox.Show(ex.Message.ToString());

            }
            return reportID;
        }
        public bool ShowSaveAsDialog(ref string recordName, IReportDesignerUI designer)
        {
            StorageEditorForm form = CreateForm();
            form.Owner = Window.GetWindow(designer as DependencyObject);
            form.listBox1.IsEnabled = true;
            form.textBox1.IsEnabled = true;
            bool? result = form.ShowDialog();
            recordName = form.textBox1.Text;
            groupId = StorageEditorForm.GroupId;
            //group = form.lookupGroup.SelectedItem as ReportGroup;
            //if (group != null)
            //    groupId = group.Id;
            return result.Value;
        }
        public string ShowSaveDialog(string filePath, string reportTitle, IReportDesignerUI designer)
        {
            StorageEditorForm form = CreateForm();
            form.textBox1.Text = reportTitle;
            form.listBox1.IsEnabled = false;
            bool? result = form.ShowDialog();
            if (result.HasValue && result.Value)
            {
                string title = form.textBox1.Text;
                groupId= StorageEditorForm.GroupId;
                //if (group != null)
                //    groupId = group.Id;
                if (!string.IsNullOrEmpty(title))
                {
                    return title;
                }
                else
                {
                    MessageBox.Show("Incorrect report name", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
            return string.Empty;
        }
        StorageEditorForm CreateForm()
        {
            StorageEditorForm form = new StorageEditorForm();
            form.lookupGroup.ItemsSource = ReportRepo.GetALLReportGroups();
            form.listBox1.ItemsSource = ReportRepo.GetALLReports();
            return form;
        }
        public void SetData(string reportId, string title, XtraReport report)
        {
            //Report repor = new Report();
            //reportId = title;
            if (report != null)
                report.Name = title;
            SYSTEM_STATIC.SaveReport(StorageEditorForm.GroupId,title, report);
            //else
            //{
            //    row = ReportStorage.AddReportStorageRow(reportId, title, GetBuffer(report));
            //}
            //DataSet.WriteXml(StoragePath, XmlWriteMode.WriteSchema);
        }

    }
}
