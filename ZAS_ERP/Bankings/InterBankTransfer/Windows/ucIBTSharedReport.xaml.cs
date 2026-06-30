using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Procurements;
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
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Bankings.InterBankTransfer.Windows
{
    /// <summary>
    /// Interaction logic for ucIBTSharedReport.xaml
    /// </summary>
    public partial class ucIBTSharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        public MainWindow myParent = null;
        string reportTitle;
        public ucIBTSharedReport()
        {
            InitializeComponent();
        }
        public ucIBTSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InterBankTransRepo interBankTransferRepo = new InterBankTransRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
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
                //Permissions Should be here       
                grdCntrlInterBankTransfer.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "Inter-Bank Transfers" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Inter-Bank Transfers" + "/" + group.groupName + "/" + reportTitle;
                if (report.settingkey == "Inter-Bank Transfer Register")
                {
                    grdCntrlInterBankTransfer.ItemsSource = interBankTransferRepo.getInterBankTransferRegister(SYSTEM_STATIC.currentUser.id);
                }
                else
                {
                    grdCntrlInterBankTransfer.ItemsSource = interBankTransferRepo.GetAllTransactionsOpenAndClosed(SYSTEM_STATIC.currentUser.id);
                }
                grdCntrlInterBankTransfer.ShowLoadingPanel = false;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdCntrlInterBankTransfer.View);
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
                        ReportLogic.UpdateGridReport(grdCntrlInterBankTransfer, str, groupDetails, report.Id, report.settingkey);

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
            grdCntrlInterBankTransfer.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }
    

        private void GrdInterBankTransfer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                {

                    var selectedBankTransfer = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;

                    if (selectedBankTransfer != null)
                    {
                        ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                        CompanyRepo compRepo = new CompanyRepo();
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();

                        //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                        ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (selectedBankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                            {
                                ucFrmBankTransfer.editFlag = true;

                                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                ucFrmBankTransfer.frmBankTranfer.ShowDialog();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View InActive Inter-Bank Transfer!");
                                return;
                            }
                        }
                        else
                        {
                            ucFrmBankTransfer.editFlag = true;

                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                            //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                            //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                            ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                            ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                            ucFrmBankTransfer.frmBankTranfer.ShowDialog();
                        }


                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GrdCntrlInterBankTransfer_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            grdCntrlInterBankTransfer.ShowLoadingPanel = true;
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);
            SharedReport report = repo.GetSharedReportByName(reportTitlePath);
            InterBankTransRepo interBankTransRepo = new InterBankTransRepo();
            if (report.settingkey == "Inter-Bank Transfer Register")
            {
                grdCntrlInterBankTransfer.ItemsSource = interBankTransRepo.getInterBankTransferRegister(SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                grdCntrlInterBankTransfer.ItemsSource = interBankTransRepo.GetAllTransactionsOpenAndClosed(SYSTEM_STATIC.currentUser.id);
            }
            grdCntrlInterBankTransfer.ShowLoadingPanel = false;
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
