using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
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
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucPaymentSharedReport.xaml
    /// </summary>
    public partial class ucPaymentSharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        public MainWindow myParent = null;
        string reportTitle;
        PaymentRepo paymentRepo = new PaymentRepo();
        public ucPaymentSharedReport()
        {
            InitializeComponent();
        }
        public ucPaymentSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdPaymentRegister.ShowLoadingPanel = true;
            SaleOrderRepo soRepo = new SaleOrderRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
                //Permissions Should be here       
                grdPaymentRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "Payments" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Payments" + "/" + group.groupName + "/" + reportTitle;
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

                if (report.settingkey == "Payment Register")
                { grdPaymentRegister.ItemsSource = paymentRepo.getPaymentRegister(SYSTEM_STATIC.currentUser.id); }
                else
                    grdPaymentRegister.ItemsSource = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
            }
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdPaymentRegister.View);
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
                        ReportLogic.UpdateGridReport(grdPaymentRegister, str, groupDetails, report.Id, report.settingkey);

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
            grdPaymentRegister.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        private void GrdPaymentList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Depts" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string deptNames = "";

                if (pymnt.departments != null && pymnt.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", pymnt.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }
        }
        private void GrdPaymentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedPayment = grdPaymentRegister.SelectedItem as Payment;

                if (selectedPayment != null)
                {
                    CompanyRepo compRepo = new CompanyRepo();
                    switch (selectedPayment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                            {
                                ucFrmPayments frmPayments = new ucFrmPayments();

                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                               
                                    if (pymnt.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmPayments.editFlag = true;
                                            frmPayments.groupId = selectedPayment.transactionGroupId;
                                            frmPayments.frmPaymentWindow.Content = frmPayments;
                                            //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                            //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                            frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                            //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                            //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                            frmPayments.frmPaymentWindow.Title = "Payments";
                                            frmPayments.frmPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = selectedPayment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                            }

                            break;

                        case PaymentTransactionType.Vendor_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                            {
                                ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId);  //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (pymnt != null)
                                {
                                    if (pymnt.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmBillPayments.editFlag = true;
                                            frmBillPayments.groupId = pymnt.transactionGroupId;
                                            frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                            frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                            frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                            frmBillPayments.frmBillPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                }
                            }


                            break;
                        case PaymentTransactionType.Purchase_Invoice:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                            {
                                ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();

                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId);  //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (pymnt != null)
                                {
                                    if (pymnt.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmPIpayment.editFlag = true;
                                            frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                            frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;

                                            frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                            frmPIpayment.frmPiPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            grdPaymentRegister.ShowLoadingPanel = true;
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);
            SharedReport report = repo.GetSharedReportByName(reportTitlePath);
            if (report.settingkey == "Payment Register")
            { grdPaymentRegister.ItemsSource = paymentRepo.getPaymentRegister(SYSTEM_STATIC.currentUser.id); }
            else
                grdPaymentRegister.ItemsSource = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
            grdPaymentRegister.ShowLoadingPanel = false;

        }
    }
}
