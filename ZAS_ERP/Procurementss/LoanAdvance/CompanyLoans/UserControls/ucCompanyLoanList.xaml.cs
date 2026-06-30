using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.LoansAdvances;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.Windows;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.LoanAdvance.CompanyLoans.UserControls
{
    /// <summary>
    /// Interaction logic for ucLoanList.xaml
    /// </summary>
    public partial class ucCompanyLoanList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        CompanyLoansRepo loansAdvanceRepo = new CompanyLoansRepo();
        public ucCompanyLoanList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Company Loans List") != null)
                {
                    ApprovalCount = loansAdvanceRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Company Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Company Loans") != null)
                    {
                        ApprovalCount = loansAdvanceRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = loansAdvanceRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Company Loans List") != null)
                {
                    ReApprovalCount = loansAdvanceRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Company Loans without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Company Loans") != null)
                    {
                        ReApprovalCount = loansAdvanceRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = loansAdvanceRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Company Loans") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = loansAdvanceRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = loansAdvanceRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Company Loans") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = loansAdvanceRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
                    //}
                    //else
                    //{
                    //    ApproveunapprovedCount = bankTransRepo.getBankTransferRegisterCountOWn(MainWindow.currentUserid);
                    //}
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = loansAdvanceRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = loansAdvanceRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Company Loans List") != null)
                {
                    ClosingCount = loansAdvanceRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Company Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Company Loans") != null)
                    {
                        ClosingCount = loansAdvanceRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = loansAdvanceRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = loansAdvanceRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdLoansAdvancesRegister.ItemsSource = loansAdvanceRepo.GetAllActiveLoansAdvances(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLoansAdvancesRegister);
        }

        public void GridControlSetUserSettings()
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLoansAdvancesRegister);
        }

        private void grdLoansAdvancesRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdLoansAdvancesRegister.SelectedItem as LoansAdvance;
            if (selectedItem != null)
            {
                switch (selectedItem.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Advance:
                        switch (selectedItem.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmAdminBillLoan frmLoansAdvances = new ucFrmAdminBillLoan();
                                    frmLoansAdvances.loansAdvanceId = selectedItem.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmVendorBillLoan frmBillLoansAdvance = new ucFrmVendorBillLoan();
                                    frmBillLoansAdvance.loansAdvanceId = selectedItem.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;

                    case LoansAdvanceTemplate.Loan:
                        switch (selectedItem.loansAdvanceType)
                        {
                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Company Loans") != null)
                                {
                                    ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void grdLoansAdvancesRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "Employee":
                    var item = grdLoansAdvancesRegister.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;
                    if (item.ApplicantEmployee != null)
                        e.Value = item.ApplicantEmployee.person.FName + " " + item.ApplicantEmployee.person.LName;
                    break;
                case "BalanaceAmountOC":
                    var loansAdvance = grdLoansAdvancesRegister.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;

                    if (loansAdvance.loansAdvanceType == LoansAdvanceType.Vendor_Bill)
                    {

                        double paidAmount = 0, receivedAmount = 0, adjustedAmount = 0;
                        if (loansAdvance.Payments != null && loansAdvance.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            paidAmount = loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                        }

                        if (loansAdvance.SalesReceipts != null && loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            receivedAmount = loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        }

                        if (loansAdvance.bills != null && loansAdvance.bills.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            foreach (var bill in loansAdvance.bills.Where(x => x.isVoid != true))
                            {
                                adjustedAmount = adjustedAmount + bill.Adjustmentss.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                            }
                        }
                        e.Value = Math.Round(receivedAmount - paidAmount - adjustedAmount, 2);
                    }
                    else if (loansAdvance.loansAdvanceType == LoansAdvanceType.Admin_Bill)
                    {
                        double paidAmount = 0, receivedAmount = 0, adjustedAmount = 0;
                        if (loansAdvance.Payments != null && loansAdvance.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            paidAmount = loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                        }

                        if (loansAdvance.SalesReceipts != null && loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            receivedAmount = loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        }

                        if (loansAdvance.AdminBills != null && loansAdvance.AdminBills.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            foreach (var adminBill in loansAdvance.AdminBills.Where(x => x.isVoid != true))
                            {
                                adjustedAmount = adjustedAmount + adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                            }
                        }
                        e.Value = Math.Round(receivedAmount - paidAmount - adjustedAmount, 2);

                    }
                    break;
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Company Loans") != null)
            {
                ucSelectLoansAdvanceType frmLoansAdvances = new ucSelectLoansAdvanceType();
                frmLoansAdvances.advanceTemplate = LoansAdvanceTemplate.Loan;
                Window win = new Window();
                win.Height = 250;
                win.Width = 400;
                win.Content = frmLoansAdvances;
                //win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdLoansAdvancesRegister.SelectedItem as LoansAdvance;
            if (selectedItem != null)
            {
                switch (selectedItem.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Advance:
                        switch (selectedItem.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmAdminBillLoan frmLoansAdvances = new ucFrmAdminBillLoan();
                                    frmLoansAdvances.loansAdvanceId = selectedItem.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmVendorBillLoan frmBillLoansAdvance = new ucFrmVendorBillLoan();
                                    frmBillLoansAdvance.loansAdvanceId = selectedItem.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;

                    case LoansAdvanceTemplate.Loan:
                        switch (selectedItem.loansAdvanceType)
                        {
                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Company Loans") != null)
                                {
                                    ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void mbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Company Loans Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("Loans");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdLoansAdvancesRegister, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Company Loans Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucCompanyLoanList loansAdvancesList = new ucCompanyLoanList();
            this.DataContext = loansAdvancesList;
            loansAdvanceRepo = new CompanyLoansRepo();
            grdLoansAdvancesRegister.ItemsSource = loansAdvanceRepo.GetAllActiveLoansAdvances(SYSTEM_STATIC.currentUser.id);
            lblHeading.Text = "Company Loans";
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdLoansAdvancesRegister);
        }


        static LoansAdvanceStatus statusChanged = new LoansAdvanceStatus();
        LoansAdvanceStatus oldStatus = new LoansAdvanceStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucCompanyLoanList(LoansAdvanceStatus loansAdvanceStatus)
        {
            statusChanged = loansAdvanceStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            loansAdvanceRepo = new CompanyLoansRepo();
            var selectedRow = (LoansAdvance)grdLoansAdvancesRegister.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var loansAdvance = loansAdvanceRepo.GetLoansAdvance(selectedRow.Id);
                if (loansAdvance != null)
                {
                    if (loansAdvance.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmLoansAdvanceStatusChange ucFrmDirectClose = new frmLoansAdvanceStatusChange();
                    if (selectedRow.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = selectedRow.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Company Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Company Loans without Approval") != null)
                        {

                            loansAdvance.PendingForClosing = false;
                            loansAdvance.stage = TransactionStage.Closed.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Company Loan having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Company Loans #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Company Loans #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            loansAdvance.PendingForClosing = true;
                            loansAdvance.stage = TransactionStage.AwaitingApproval.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Company Loan having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Company Loan #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void mbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdLoansAdvancesRegister.GetFocusedRow() != null)
                {
                    loansAdvanceRepo = new CompanyLoansRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (LoansAdvance)grdLoansAdvancesRegister.GetFocusedRow();
                    var loansAdvance = loansAdvanceRepo.GetLoansAdvance(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (loansAdvance != null)
                    {
                        if (loansAdvance.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Company Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Company Loans") != null) ? true : false)
                            {

                                loansAdvance.isApproved = true;
                                loansAdvance.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Company Loan is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Company Loan is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Admin Bill Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loansAdvance.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Company Loans without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Company Loans") != null) ? true : false)
                            {
                                loansAdvance.isReApproved = true;
                                loansAdvance.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Company Loan is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Company Loan is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Company Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Company Loans Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                    }
                    //Load_Receipts();
                }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void mbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Company Loans List") != null)
            {
                loansAdvanceRepo = new CompanyLoansRepo();
                lblHeading.Text = "Pending for Approval Company Loans";
                var loansAdvanceList = loansAdvanceRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdLoansAdvancesRegister.ItemsSource = loansAdvanceList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Admin Bills!!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Company Loans List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Company Loans";
                var loansAdvances = loansAdvanceRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdLoansAdvancesRegister.ItemsSource = loansAdvances;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Company Loans!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Company Loans List") != null)
            {

                var loansAdvanves = loansAdvanceRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Company Loans";
                grdLoansAdvancesRegister.ItemsSource = loansAdvanves;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Company Loans!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Company Loans") != null)
            {
                loansAdvanceRepo = new CompanyLoansRepo();
                var loansAdvanceList = loansAdvanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id);
                grdLoansAdvancesRegister.ItemsSource = loansAdvanceList;
                lblHeading.Text = "Company Loans Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Company Loans!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Company Loans") != null)
            {
                lblHeading.Text = "Void Company Loans";

                var loansAdvanceList = loansAdvanceRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdLoansAdvancesRegister.ItemsSource = loansAdvanceList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Admin Bills!!");
            }
        }
    }
}