using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.AdminBillss.Windows
{
    /// <summary>
    /// Interaction logic for ucAdminBIllSharedReport.xaml
    /// </summary>
    public partial class ucAdminBIllSharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        public MainWindow myParent = null;
        string reportTitle;
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
        public ucAdminBIllSharedReport()
        {
            InitializeComponent();
        }
        public ucAdminBIllSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdBillsList.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnUpdateReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    var repo = new GridReportRepo();
                    //var report = repo.GetReportByName(this.Title);
                    var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
                    string str = report.reportName;
                    SharedGridGroup groupDetails = group;
                    if (str != "")
                    {
                        ReportLogic.UpdateGridReport(grdBillsList, str, groupDetails, report.Id, report.settingkey);

                        DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void MbtnRenameReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to rename " + this.Title + " report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    SelectSharedGroup selectSharedGroup = new SelectSharedGroup(report);
                    selectSharedGroup.ShowDialog();
                }
                else
                {
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void MbtnDeleteReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {
                GridReportRepo repo = new GridReportRepo();
                repo.DeleteSharedReport(report.Id);
                DXMessageBox.Show("Report has been deleted successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdBillsList.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
                grdBillsList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                txtUserName.Text = report.Creater.userName;
                txtDesignation.Text = report.Creater.employee.DesignationTitle;
                List<int> empIds = new List<int>();
                List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                List<Department> userdepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                foreach (var userDept in userdepartments)
                {
                    empIds.AddRange(userDept.employees.Select(x => x.EmpId).Distinct().ToList());
                }
                foreach (var emp in report.SharedGridGroup.Employees)
                {
                    if (empIds.Contains(emp.EmpId))
                    {
                        employees.Add(emp);
                    }
                    employees.Distinct();

                }
                grdEmployee.ItemsSource = employees;
                var image = GetBitmapImageFromByteArray(report.Creater.employee.person.Photo);

                UserImage.Source = image;
                if (report.settingkey == "Admin Bill Register")
                {
                    grdBillsList.ItemsSource = adminBillsRepo.GetAllBills(SYSTEM_STATIC.currentUser.id);
                }
                else
                {
                    grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                }
            }
            grdBillsList.ShowLoadingPanel = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

            SetColumnsVisibility();
        }

        private void SetColumnsVisibility()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Admin Bills") == null)
            {
                grdBillsList.Columns["hasSummary"].Visible = false;
                grdBillsList.Columns["hasSummary"].ShowInColumnChooser = false;
                grdBillsList.Columns["managementSummary.SummaryName"].Visible = false;
                grdBillsList.Columns["managementSummary.SummaryName"].ShowInColumnChooser = false;
                grdBillsList.Columns["SummaryMemo"].Visible = false;
                grdBillsList.Columns["SummaryMemo"].ShowInColumnChooser = false;
            }
        }
        private void GrdBillsList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var bill = grdBillsList.GetRowByListIndex(e.ListSourceRowIndex) as AdminBill;

            if (e.IsGetData)
            {
                switch (e.Column.FieldName)
                {
                    case "Closed":
                        if (bill.Payments.Count > 0 && bill.Payments.Where(x => x.Status != null && x.Status.isActive == true).Count() == 0)
                        {
                            e.Value = "Closed";
                        }
                        else
                        {
                            e.Value = "Open";
                        }
                        break;
                    case "PayeeName":
                        if (e.GetListSourceFieldValue("payee_Id") != null)
                        {
                            var payee_id = Convert.ToInt32(e.GetListSourceFieldValue("payee_Id"));
                            if (payee_id > 0)
                            {
                                var payee = billsRepo.GetPayeeForBillRegister(payee_id);
                                string payeeName;
                                payeeName = payee.PayeeName;
                                while (payee.ParentId != null)
                                {
                                    payee = billsRepo.GetPayeeForBillRegister((int)payee.ParentId);
                                    payeeName = payee.PayeeName + " > " + payeeName;
                                }
                                e.Value = payeeName;
                            }
                        }
                        break;

                    case "paidAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymnts = bill.Payments.Where(x => x.isVoid != true).ToList();
                            e.Value = pymnts.Sum(x => x.DebitedAmount);
                        }
                        break;

                    case "balanceAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymntss = bill.Payments.Where(x => x.isVoid != true).ToList();
                            var amountPaid = pymntss.Sum(x => x.DebitedAmount);
                            e.Value = bill.AmountOC - amountPaid;
                        }
                        break;

                    case "percBalanceAmount":
                        var finalPayments = bill.Payments.Where(x => x.isVoid != true).ToList();
                        var paidAmount = finalPayments.Sum(x => x.DebitedAmount);
                        var percent = (((paidAmount) / bill.AmountOC) * 100);
                        e.Value = Math.Round(percent, 2);
                        break;

                    case "BillCreator":
                        if (bill.Creator != null && bill.Creator.person != null)
                        {
                            var userName = bill.Creator.person.FName + " " + bill.Creator.person.LName;
                            e.Value = userName;
                        }
                        break;
                }
            }
        }

        private void GrdBillsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateBillWindow();
        }
        private void UpdateBillWindow()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                DXWindow frmBill = new DXWindow();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                var selectedRow = grdBillsList.SelectedItem as AdminBill;

                if (selectedRow != null)
                {
                    billsRepo = new AdminBillsRepo();
                    frmBillAdd.bills = new List<AdminBill>();
                    frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBillAdd.isProgressiveCost = selectedRow.isProgressiveCost;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }

        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);
            SharedReport report = repo.GetSharedReportByName(reportTitlePath);
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            if (report.settingkey == "Admin Bill Register")
            { grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenAndClosed(SYSTEM_STATIC.currentUser.id); }
            else
            { grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id); }
            grdBillsList.ShowLoadingPanel = false;
        }
        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
