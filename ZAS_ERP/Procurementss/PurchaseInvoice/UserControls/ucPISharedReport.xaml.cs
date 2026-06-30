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

namespace ZAS_ERP.Procurementss.PurchaseInvoice.UserControls
{
    /// <summary>
    /// Interaction logic for ucPISharedReport.xaml
    /// </summary>
    public partial class ucPISharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        string reportTitle;
        public MainWindow myParent = null;
        public ucPISharedReport()
        {
            InitializeComponent();
        }
        public ucPISharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            PurchaseInvoiceRepo poRepo = new PurchaseInvoiceRepo();
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
                grdpurchaseInvoice.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                if (report.settingkey == "Purchase Invoice Register")
                { grdpurchaseInvoice.ItemsSource = poRepo.getSaleInvoiceRegister(SYSTEM_STATIC.currentUser.id); }
                else
                    grdpurchaseInvoice.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void GrdpurchaseInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")) != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();
            }
        }
        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

            if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    e.Value = deptList[0].DeptName;
                }
            }
            if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[1].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[2].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[3].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;  
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[4].DeptName;
                            break;
                    }
                }
            }




            if (e.Column.FieldName == "PaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = invoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PO.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        if (PO.tax != null)
                        {
                            amountWithTax = paidAmount + (paidAmount * Convert.ToDecimal(PO.tax.percentage) / 100);
                        }
                        else
                        {
                            amountWithTax = paidAmount;
                        }
                    e.Value = amountWithTax;
                }
            }

            if (e.Column.FieldName == "UnpaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PI = invoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PI.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            //paidAmount = paidAmount + ((paidAmount * Convert.ToDecimal(PI.PurchaseOrder.tax.percentage)) / 100);
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount * PI.PurchaseOrder.tax.percentage) / 100) - paidAmount, 2);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PI.totalInvoiceAmount) - paidAmount;
                        }
                    }
                    else
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount * PI.PurchaseOrder.tax.percentage) / 100), 2);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PI.totalInvoiceAmount);
                        }
                    }
                    e.Value = amountWithTax;
                }
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
                        ReportLogic.UpdateGridReport(grdpurchaseInvoice, str, groupDetails, report.Id, report.settingkey);

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
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdpurchaseInvoice.View);
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

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdpurchaseInvoice.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
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
            PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
            if (report.settingkey == "Purchase Invoice Register")
            { grdpurchaseInvoice.ItemsSource = purchaseInvoiceRepo.getSaleInvoiceRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdpurchaseInvoice.ItemsSource = purchaseInvoiceRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            grdpurchaseInvoice.ShowLoadingPanel = false;        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
                {
                    switch (cmbxDepartmentFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 0:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 1:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {

                                case 1:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Department Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
                {
                    cmbxDepartmentTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater department!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxDepartmentTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
