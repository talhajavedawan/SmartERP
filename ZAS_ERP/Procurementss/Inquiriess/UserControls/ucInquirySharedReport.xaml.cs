using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss.Inquiriess.UserControls
{
    /// <summary>
    /// Interaction logic for ucInquirySharedReport.xaml
    /// </summary>
    public partial class ucInquirySharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        Inquiry inquiry = new Inquiry();
        public MainWindow myParent = null;
        string reportTitle;
        public ucInquirySharedReport()
        {
            InitializeComponent();
        }
        public ucInquirySharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }



        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdinquiry.View);
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

        private void MbtnSaveAsNew1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

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
                        ReportLogic.UpdateGridReport(grdinquiry, str, groupDetails, report.Id, report.settingkey);

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
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

       

     

        private void Grdinquiry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditInquiry();
            inquiry = ((Inquiry)grdinquiry.GetFocusedRow());
        }
        private void EditInquiry()
        {
            if (grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")) != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, (int)grdinquiry.GetFocusedRowCellValue(grdinquiry.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.inq.Add(inquiry);
                procurmentPanel.Show();
                inquiry = null;
            }
        }
        private void grdinquiry_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {

            Inquiry inq = grdinquiry.CurrentItem as Inquiry;
            if (e.Column.FieldName == "inquiryStatus.Status" && inq != null)
            {
                string mycolor = inq.inquiryStatus.backcolor;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(mycolor.ToString());
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                Style st = new Style(typeof(LightweightCellEditor));
                object key = new DevExpress.Xpf.Grid.Themes.GridRowThemeKeyExtension() { ResourceKey = DevExpress.Xpf.Grid.Themes.GridRowThemeKeys.CellStyle };
                SolidColorBrush s = new SolidColorBrush(newColor);
                Binding myBinding = new Binding();
                myBinding.ElementName = "grdinquiry";
                myBinding.Path = new PropertyPath("CurrentItem.inquiryStatus.backcolor");
                myBinding.Mode = BindingMode.TwoWay;
                Style myStyle = new Style(typeof(DataGridCell));
                myStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new Binding("SelectedColour[0]")));
                st.Setters.Add(new Setter(LightweightCellEditor.BackgroundProperty, newColor));
            }
        }

        private void grdinquiry_AutoGeneratingColumn(object sender, DevExpress.Xpf.Grid.AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.FieldName == "inquiryStatus.Status")
            {
                Inquiry inq = grdinquiry.CurrentItem as Inquiry;
                var cb = new GridColumn();
                cb.Header = "Status";
            }
        }

     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InquiryRepo inquiryRepo = new InquiryRepo();
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

                grdinquiry.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "Inquiries" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Inquiries" + "/" + group.groupName + "/" + reportTitle;
                grdinquiry.ItemsSource = inquiryRepo.getAll(SYSTEM_STATIC.currentUser.id);

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            InquiryRepo repo = new InquiryRepo();
            grdinquiry.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);

            loadingGif.Visibility = Visibility.Hidden;
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
