using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZAS_ERP.Procurementss;
using ZAS_ERP.Reportss;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;

namespace ZAS_ERP.SaleOrderFolder.Windows
{
    /// <summary>
    /// Interaction logic for ucSaleReceiptView.xaml
    /// </summary>
    public partial class ucSaleReceiptView : DXWindow
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();
        CompanyRepo compRepo = new CompanyRepo();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        CustomerCompRepo custRepo = new CustomerCompRepo();
        GridReport report = new GridReport();
        public List<SalesReceipt> receiptList = new List<SalesReceipt>();
        //public List<AllSaleReceipts> SaleReceiptList = new List<AllSaleReceipts>();
        //public List<AllSaleReceipts> saleReceipts = new List<AllSaleReceipts>();


        string reportTitle;
        public MainWindow myParent = null;
        public ucSaleReceiptView()
        {
            InitializeComponent();
        }
        public ucSaleReceiptView(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void GrdSaleReceiptList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt;


            if (selectedRow.receiptType == ReceiptType.Direct_Receipt)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                {
                    ucFrmDirectSaleReceipt frmLAreceipt = new ucFrmDirectSaleReceipt();
                    //paymentRepo = new PaymentRepo();
                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                    Window frmPiPaymentWindow = new Window();
                    if (selectedRow.saleReceiptStatus.isActive == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                        {
                            frmLAreceipt.editFlag = true;
                            frmLAreceipt.groupId = selectedRow.transactionGroupId;
                            frmPiPaymentWindow.Content = frmLAreceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Direct Receipts";
                            frmPiPaymentWindow.Show();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                            return;
                        }
                    }
                    else
                    {
                        frmLAreceipt.editFlag = true;
                        frmLAreceipt.groupId = selectedRow.transactionGroupId;
                        frmPiPaymentWindow.Content = frmLAreceipt;
                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmPiPaymentWindow.Title = "Direct Receipts";
                        frmPiPaymentWindow.Show();
                    }

                }
                else
                {
                    DXMessageBox.Show("Permission required to View Direct Receipts!");
                }
                return;
            }

            if (selectedRow.receiptType == ReceiptType.Loans_Advances)
            {
                switch (selectedRow.loansAdvance.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Loan:
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                        {
                            ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (selectedRow.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Sale Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                        }
                        break;
                    default:
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                        {
                            ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (selectedRow.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Sale Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                        }
                        break;
                }

                return;
            }
            repo = new SalesReceiptRepo();
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                {
                    ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                    repo = new SalesReceiptRepo();
                    updateSaleReceiptObj.saveEditFlag = 1;

                    if (selectedRow == null)
                    {
                        return;
                    }
                    var saleReceipt = repo.GetSalesReceipt(selectedRow.Id);
                    if (saleReceipt == null)
                    {
                        return;
                    }

                    if (saleReceipt.saleReceiptStatus != null)
                    {
                        var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                        updateSaleReceiptObj.selectedStatus = status;
                        if (status.isActive == false && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null))
                        {
                            DXMessageBox.Show("Permission required to View Closed Receipts!");
                            return;
                        }
                    }

                    for (int i = 0; i <= (int)ERP_BL.Enums.ReceiptType.Sales_Department; i++)
                    {
                        //if (((ERP_BL.Enums.ReceiptType)i).ToString() == selectedRow.receiptType)
                        //{
                        //    index = i;
                        //    break;
                        //    //break;
                        //}
                    }
                    updateSaleReceiptObj.dateEditcreationDate.EditValue = saleReceipt.CreationDate;
                    updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                    updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                    updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                    updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                    updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                    updateSaleReceiptObj.receiptId = saleReceipt.Id;
                    updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;


                    //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                    ////ucFrmAddAccount obj = new ucFrmAddAccount();
                    //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                    //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                    //updateSaleReceiptObj.enter_receipt_win.Show();

                    updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                    updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                    updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                    updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                    updateSaleReceiptObj.enterReceiptWindowFlag = true;


                    if (saleReceipt.CreditedDate != null)
                        updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                    if (saleReceipt.DepositedDate != null)
                        updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                    if (saleReceipt.InstrumentDate != null)
                        updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                    if (saleReceipt.InstrumentNo != null)
                        updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                    if (saleReceipt.principal != null)
                    {
                        PrincipalRepo prinRepo = new PrincipalRepo();
                        var principal = prinRepo.get(saleReceipt.principal.Id);
                    }

                    updateSaleReceiptObj.enter_receipt_win.Show();
                    //Load_Receipts();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                    return;
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdSaleReceiptList.View);
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
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdSaleReceiptList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Standard Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdSaleReceiptList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Memorized Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void MbtnRenameReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdSaleReceiptList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdSaleReceiptList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnUpdateReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {
                                    ReportLogic.UpdateGridReport(grdSaleReceiptList, str, reportType, groupDetails, report.Id, report.settingkey);

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {
                                    ReportLogic.UpdateGridReport(grdSaleReceiptList, str, reportType, groupDetails, report.Id, report.settingkey);

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void MbtnDeleteReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                /*GridReport report = repo.GetReportByName(this.Title)*/
                                ;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdSaleReceiptList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

                case GridReportType.MemorizedReport:
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                /*GridReport report = repo.GetReportByName(this.Title)*/
                                ;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdSaleReceiptList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

            }
        }

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {

                grdSaleReceiptList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            }
            else
            {
            }
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void MbtnExportToMemorizedReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToMemorizedReport = setReportName.report;
                                if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.gridReportType != null && exportToMemorizedReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToMemorizedReport.gridReportGroup;
                                    var reportType = exportToMemorizedReport.gridReportType;
                                    var reportName = exportToMemorizedReport.reportName;
                                    ReportLogic.SaveGridReport(grdSaleReceiptList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Memorized reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnExportToStandardReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


            switch (report.gridReportType)
            {
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToStandardReport = setReportName.report;
                                if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.gridReportType != null && exportToStandardReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToStandardReport.gridReportGroup;
                                    var reportType = exportToStandardReport.gridReportType;
                                    var reportName = exportToStandardReport.reportName;
                                    ReportLogic.ExportToStandard(grdSaleReceiptList, reportName, reportType, reportGroup, report.settingkey);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

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
            GridReport report = repo.GetReportByName(reportTitlePath);
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            if (report.settingkey == "Sale Receipt Register")
            {
                receiptList = receiptRepo.getSaleReceiptRegister(SYSTEM_STATIC.currentUser.id);
                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                grdSaleReceiptList.ItemsSource = receiptList;
            }
            else
            {
                receiptList = receiptRepo.GetAllSalesReceipt(SYSTEM_STATIC.currentUser.id);
                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                grdSaleReceiptList.ItemsSource = receiptList;
            }
            loadingGif.Visibility = Visibility.Hidden;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            //grdSaleReceiptList.Columns["SerialNo"].Visible = false;
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here       
                           
                            if (report.settingkey == "Sale Receipt Register")
                            {
                                receiptList = receiptRepo.getSaleReceiptRegister(SYSTEM_STATIC.currentUser.id);
                                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                                grdSaleReceiptList.ItemsSource = receiptList;
                            }
                            else
                            {
                                receiptList = receiptRepo.GetAllSalesReceipt(SYSTEM_STATIC.currentUser.id);
                                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                                grdSaleReceiptList.ItemsSource = receiptList;
                            }
                            mbtnExportToStandardReport1.IsVisible = false;
                            grdSaleReceiptList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Sale Receipts" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Sale Receipts" + "/" + group.groupName + "/" + reportTitle;
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here     
                           
                            if (report.settingkey == "Sale Receipt Register")
                            {
                                receiptList = receiptRepo.getSaleReceiptRegister(SYSTEM_STATIC.currentUser.id);
                                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                                grdSaleReceiptList.ItemsSource = receiptList;
                            }
                            else
                            {
                                receiptList = receiptRepo.getSaleReceiptRegister(SYSTEM_STATIC.currentUser.id);
                                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(receiptList);
                                grdSaleReceiptList.ItemsSource = receiptList;
                            }
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdSaleReceiptList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Sale Receipts" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Sale Receipts" + "/" + group.groupName + "/" + reportTitle;
                        }
                    }
                    break;
            }
            Loader.DeferedVisibility = false;
        }

        private void GrdSaleReceiptList_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tblViewLandTypeLst.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tblViewLandTypeLst.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tblViewLandTypeLst.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }
        private void GrdSaleReceiptList_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var receipt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                switch (e.Column.FieldName)
                {

                    case "departmentLevel1":

                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            var node = receipt.department;

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
                        break;
                    case "departmentLevel2":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            var node = receipt.department;

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
                        break;
                    case "departmentLevel3":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            var node = receipt.department;

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
                        break;
                    case "departmentLevel4":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            var node = receipt.department;

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
                        break;
                    case "departmentLevel5":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            var node = receipt.department;

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
                        break;

                    case "Customerr":
                        if (receipt != null)
                        {
                            if (receipt.Customer != null && receipt.Customer.company != null)
                            {
                                e.Value = receipt.Customer.company.CompanyName;
                            }
                            else if (receipt.saleInvoice != null && receipt.saleInvoice.customerCompany != null && receipt.saleInvoice.customerCompany.company != null)
                            {
                                e.Value = receipt.saleInvoice.customerCompany.company.CompanyName;
                            }
                        }
                        break;

                    case "Deductionss":
                        if (receipt != null)
                        {
                            var receiptDeductions = receipt.receiptDeductions;
                            if (receiptDeductions != null && receiptDeductions.Count != 0)
                            {
                                e.Value = Convert.ToDecimal(receiptDeductions.Sum(x => x.Amount));
                            }
                            else
                                e.Value = 0;
                        }
                        break;
                    case "VAT":

                        if (receipt != null)
                        {
                            Decimal taxAmount = 0;

                            if (receipt.ReceiptDeductionTaxes != null && receipt.ReceiptDeductionTaxes.Count != 0)
                            {

                                taxAmount = taxAmount + Convert.ToDecimal(receipt.ReceiptDeductionTaxes.Sum(x => x.Amount));
                            }
                            if (receipt.ReceiptBankTaxes != null && receipt.ReceiptBankTaxes.Count != 0)
                            {

                                taxAmount = taxAmount + Convert.ToDecimal(receipt.ReceiptBankTaxes.Sum(x => x.Amount));
                            }

                            e.Value = taxAmount;
                        }


                        break;
                    case "bankchargess":

                        if (receipt != null)
                        {


                            if (receipt.bankCharges != null && receipt.bankCharges.Count != 0)
                            {

                                e.Value = Convert.ToDecimal(receipt.bankCharges.Sum(x => x.Amount));
                            }
                            else
                                e.Value = 0;
                        }


                        break;
                    case "CreditedAmount":
                        if (e.GetListSourceFieldValue("Id") != null)
                        {
                            var recpt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                            if (recpt != null && recpt.receiptType != ReceiptType.Direct_Receipt)
                            {


                                var collectionAmount = e.GetListSourceFieldValue("CollectionAmount");
                                var _receipt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                                List<ReceiptDeduction> receiptDeductions = _receipt.receiptDeductions;
                                if (receiptDeductions != null && receiptDeductions.Count != 0)
                                {

                                    var totalDeductions = receiptDeductions.Sum(x => x.Amount);
                                    var VAT = _receipt.ReceiptDeductionTaxes.Sum(x => x.Amount);
                                    e.Value = Convert.ToDecimal(collectionAmount) - Convert.ToDecimal(totalDeductions) - Convert.ToDecimal(VAT);
                                }
                                else
                                {
                                    e.Value = Convert.ToDecimal(collectionAmount);
                                }
                            }
                            else
                            {
                                e.Value = recpt.CollectionAmount;
                            }
                        }
                        break;
                    case "RemainingAmount":
                        if (e.GetListSourceFieldValue("Id") != null)
                        {
                            var saleInvoice = e.GetListSourceFieldValue("saleInvoice") as SaleInvoice;

                            //List<ReceiptDeduction> receiptDeductions = (e.GetListSourceFieldValue("receiptDeductions")) as List<ReceiptDeduction>;
                            if (saleInvoice != null)
                            {
                                if (saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count != 0)
                                {
                                    var reciepts = saleInvoice.salesReceipts;
                                    var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                                    e.Value = saleInvoice.totalInvoiceAmount - result;

                                }
                                else
                                {
                                    e.Value = Convert.ToDecimal(saleInvoice.totalInvoiceAmount);
                                }
                            }

                        }
                        break;
                }
            }
        }

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();
            SalesReceiptStatus statusChanged = new SalesReceiptStatus();

            try
            {

                SalesReceipt SR = new SalesReceipt();
                UsersRepo usersRepo = new UsersRepo();
                var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt;
                var Id = selectedRow.Id;
                if (Id != 0)
                {
                    SR = repo.GetSalesReceipt(Id);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null) ? true : false)
                {
                    if (selectedRow != null)
                    {
                        var receipt = repo.GetSalesReceipt(Id);
                        var previous_status = receipt.saleReceiptStatus.Status;
                        var receiptList = repo.getReceiptsByGroupId(receipt.transactionGroupId);
                        var totalSum = receiptList.Sum(x => x.CollectionAmount);

                        if (receiptList.Count > 0 && receiptList[0].isApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null))
                                {
                                    receiptList.ForEach(z => z.isApproved = true);
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to Approve the Sale Receipt!");
                                    return;
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Sale Receipt cannot be Closed without Approval!", "Alert", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                        }
                        else if (receiptList.Count > 0 && receiptList[0].isReApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under ReApproval, Do you want to ReApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null))
                                {
                                    receiptList.ForEach(z => z.isReApproved = true);
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to ReApprove the Sale Receipt!");
                                    return;
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Sale Receipt cannot be Closed without Approval!", "Alert", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                        }


                        statusChanged = null;
                        ucFrmDirectClose ucFrmDirectClose = new ucFrmDirectClose();
                        if (receipt.saleReceiptStatus != null)
                        {
                            ucFrmDirectClose.statusName.Text = receipt.saleReceiptStatus.Status;

                            var brush = new BrushConverter();
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(receipt.saleReceiptStatus.backcolor);
                        }

                        ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                        ucFrmDirectClose.directCloseWin.Width = 450;
                        ucFrmDirectClose.directCloseWin.Height = 650;
                        ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                        ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        ucFrmDirectClose.directCloseWin.ShowDialog();
                        double totalValue = 0;
                        if (statusChanged.Id != 0)
                        {
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sale Receipt has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (SR.department != null && SR.department.Id != 0 && SR.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(SR.department.Id, SR.company.Id), SR.transactionGroupId, TransactionItemType.Sale_Receipt);
                                    //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();

                                }
                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null)
                            {

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.PendingForClosing = false;
                                    _receipt.stage = TransactionStage.Closed.ToString();
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    //Adding signature (comment)



                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having Collection Ammount: " + totalValue.ToString() + " (" + receipt.Currency.Abbrivation.ToString() + ")" + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                                //Load_Receipts();
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                totalValue = 0;
                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingApproval.ToString();
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    if (_receipt.PendingForClosing == null)
                                    {
                                        _receipt.PendingForClosing = true;
                                    }
                                    totalValue = totalValue + _receipt.CollectionAmount;


                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                totalValue = 0;

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingApproval.ToString();
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    if (_receipt.PendingForClosing != true)
                                    {
                                        _receipt.PendingForClosing = true;
                                    }
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                totalValue = 0;

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingFirstReview.ToString();
                                    _receipt.PendingForClosing = true;
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                            }
                            DXMessageBox.Show("Sale Receipt status changed to InActive (" + statusChanged.Status + ")");
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("You are not Allowed to Close Sale Receipt Directly.");
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }

        }

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if (grdSaleReceiptList.GetFocusedRow() != null)
                {
                    var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt; ;
                    var recept = repo.GetSalesReceipt(selectedRow.Id);
                    var receipts = repo.getReceiptsByGroupId(recept.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (recept.isApproved != true)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isApproved = true;
                                    receipts[i].stage = TransactionStage.Approved.ToString();
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, recept.transactionGroupId, 11, frmInputBox.comment);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                if (recept.isApproved == null)
                                {
                                    for (int i = 0; i < receipts.Count; i++)
                                    {
                                        receipts[i].isApproved = false;
                                        receipts[i].stage = TransactionStage.Approved.ToString();
                                    }

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, 11, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                if (recept.isApproved == null)
                                {
                                    for (int i = 0; i < receipts.Count; i++)
                                    {
                                        receipts[i].isApproved = false;
                                    }

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, 11, frmInputBox.comment);
                            }
                            else
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isApproved = false;
                                }
                            }

                            repo.updateSalesReceipt(receipts);


                            MessageBox.Show("SaleReceipts are Approved (" + recept.transactionGroupId + ")");
                            SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + recept.transactionGroupId + ")");
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Approve SaleReceipt Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleReceipt Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }

                    }
                    else if (recept.isReApproved == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null) ? true : false)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isReApproved = true;
                                    receipts[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].stage = TransactionStage.Approved.ToString();
                                    if (receipts[i].isReApproved == null)
                                    {
                                        receipts[i].isReApproved = false;
                                    }
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (receipts[i].isReApproved == null)
                                    {
                                        receipts[i].isReApproved = false;
                                    }
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isReApproved = false;
                                }
                            }

                            repo.updateSalesReceipt(receipts);


                            MessageBox.Show("SaleReceipts are Approved (" + recept.transactionGroupId + ")");
                            SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + recept.transactionGroupId + ")");
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Re-Approve SaleReceipt Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleReceipt Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
