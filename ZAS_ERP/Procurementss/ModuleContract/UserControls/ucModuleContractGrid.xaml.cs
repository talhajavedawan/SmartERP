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
using ERP_BL.Databases;
using ERP_BL;
using DevExpress.Xpf.Grid;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using ERP_BL.Enums;
using ZAS_ERP.Reportss;
using ERP_BL.Reports;
using ERP_BL.Procurements;

namespace ZAS_ERP.Procurementss.ModuleContract.UserControls
{
    /// <summary>
    /// Interaction logic for ucModuleContractGrid.xaml
    /// </summary>
    public partial class ucModuleContractGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        ModuleContractStatus oldStatus = new ModuleContractStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        ModuleContractRepo ModuleContractRepo = new ModuleContractRepo();
        public ucModuleContractGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) ModuleContract List") != null)
                {

                    ApprovalCount = ModuleContractrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                    {
                        ApprovalCount = ModuleContractrepo.getAllPendingforApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = ModuleContractrepo.getAllPendingforApprovalCountOwn(MainWindow.currentUserid);

                    }

                }

            }

            else
            {
                ApprovalCount = ModuleContractrepo.getAllPendingsforApprovalAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) ModuleContract List") != null)
                {

                    ClosingCount = ModuleContractrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                    {
                        ClosingCount = ModuleContractrepo.getAllPendingforClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = ModuleContractrepo.getAllPendingforClosingCountown(MainWindow.currentUserid);

                    }

                }

            }

            else
            {
                ClosingCount = ModuleContractrepo.getAllPendingforClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void ModuleContracts") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = ModuleContractrepo.getVoidRegisterCount(MainWindow.currentUserid);
                }
                else
                {
                    VoidCount = ModuleContractrepo.getVoidRegisterAdministratorCount();
                }
            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View ModuleContract Register") != null)
            {
                mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null ||
                    SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                {
                    ApproveunapprovedCount = ModuleContractRepo.getModuleContractRegisterCount(MainWindow.currentUserid);
                }
                else
                {
                    ApproveunapprovedCount = ModuleContractRepo.getModuleContractRegisterCountOWn(MainWindow.currentUserid);
                }
            }
            else
            {
                mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
            }
        }

        ModuleContractRepo ModuleContractrepo = new ModuleContractRepo();
        ERP_BL.Procurements.ModuleContract ModuleContract = new ERP_BL.Procurements.ModuleContract();
        List<ERP_BL.Procurements.ModuleContract> ModuleContracts = new List<ERP_BL.Procurements.ModuleContract>();
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }

        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucModuleContractGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null)
            //{
            //    mbtnPending.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    mbtnPending.Visibility = Visibility.Collapsed;

            //}
            loadModuleContractgrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });



        }
        /// <summary>
        /// Loads data into ModuleContract Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadModuleContractgrid()

        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "ModuleContracts(Open)" || lblHeading.Text == "ModuleContracts(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        ModuleContracts = ModuleContractrepo.getAll();


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive ModuleContracts") != null)
                    {
                        ModuleContracts = ModuleContractrepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        ModuleContracts = ModuleContractrepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    ModuleContracts = ModuleContractrepo.getAllActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    ModuleContracts = ModuleContractrepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (ModuleContracts)";
                    if (MainWindow.currentUserid != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) ModuleContract List") != null)
                        {

                            ModuleContracts = ModuleContractrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)

                                ModuleContracts = ModuleContractrepo.getAllPendingforApprovalFirst(MainWindow.currentUserid);
                            else
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforApprovalOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    }

                    else
                        ModuleContracts = ModuleContractrepo.getAllPendingsforAdministratorFirst();
                    //grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) ModuleContracts";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) ModuleContract List") != null)
                        {

                            ModuleContracts = ModuleContractrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforClosingOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    else
                        ModuleContracts = ModuleContractrepo.getAllPendingforClosingAdministratorFirst();

                    //grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                ModuleContracts = ModuleContractrepo.getAllModuleContractsByStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdModuleContract);
            this.grdModuleContract.ItemsSource = ModuleContracts;

        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null) ? true : false)
            {
                if (grdModuleContract.SelectedItem != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    ERP_BL.Procurements.ModuleContract ModuleContract = new ERP_BL.Procurements.ModuleContract();
                    ModuleContract = grdModuleContract.SelectedItem as ERP_BL.Procurements.ModuleContract;
                 
                    if (ModuleContract.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null) ? true : false)
                        {
                            ModuleContract.isApproved = true;
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, ModuleContract.Id, 2, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.ModuleContractRepo.update(Inquiriess.ucStatuschange.ModuleContract);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                        {
                            ModuleContract.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ModuleContract.isApproved == null)
                            {
                                ModuleContract.isApproved = false;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ModuleContract.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null)
                        {
                            ModuleContract.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ModuleContract.isApproved == null)
                            {
                                ModuleContract.isApproved = false;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ModuleContract.Id, 2, frmInputBox.comment);
                        }
                        else
                        {
                            ModuleContract.isApproved = false;
                        }




                    ModuleContractrepo.updateFromGrid(ModuleContract);
                    MessageBox.Show("Inquiry is Approved (" + ModuleContract.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "Inquiry is Approved (" + ModuleContract.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve ModuleContract Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve ModuleContract Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }
        private void grdModuleContract_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditModuleContract();
        }

        private void EditModuleContract()
        {
            if (grdModuleContract.GetFocusedRowCellValue(grdModuleContract.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.ModuleContract, (int)grdModuleContract.GetFocusedRowCellValue(grdModuleContract.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();
            }
        }

        private void btnNewModuleContract_Click(object sender, RoutedEventArgs e)
        {


            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.ModuleContract, 0);
            procurmentPanel.Show();
        }

        private void btnEditModuleContract_Click(object sender, RoutedEventArgs e)
        {
            EditModuleContract();
        }

        private void UcModuleContractgrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(grdModuleContract);
            //statusId = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdModuleContract.View.ShowPrintPreview(this);
            //ShowDesigner(tableView);
        }
        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ShowDesigner(tableView);
        }
        // Initializes and runs a Report Designer. 
        public static void ShowDesigner(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        {
            var report = new XtraReport();
            ReportGenerationExtensions<ColumnWrapper, RowBaseWrapper>.Generate(report, factory);
            Reportss.frmReportPanel frmReport = new Reportss.frmReportPanel(report);
            frmReport.Show();
            //var reportDesigner = new ReportDesigner();
            //reportDesigner.Loaded += (s, e) => {
            //    reportDesigner.OpenDocument(report);
            //};
            //reportDesigner.ShowWindow(factory as FrameworkElement);
        }

        private void MbtnReportCreate_Click(object sender, RoutedEventArgs e)
        {
            ShowDesigner(tableView);
        }
        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export ModuleContracts Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContracts") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdModuleContract, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export ModuleContracts Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucModuleContractGrid ucModuleContractGrid = new ucModuleContractGrid();
            this.Content = ucModuleContractGrid;
            //loadModuleContractgrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            PrintableControlLink link = new PrintableControlLink((TableView)grdModuleContract.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            //link.ReportHeaderTemplate= text as DataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdModuleContract.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null) ? true : false)
            {
                if (grdModuleContract.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContractid = (int)grdModuleContract.GetFocusedRowCellValue(grdModuleContract.Columns.GetColumnByFieldName("Id"));

                    var row = grdModuleContract.GetFocusedRow() as ERP_BL.Procurements.ModuleContract;

                    if (row.ModuleContractStatus != null)
                    {
                        oldStatus = row.ModuleContractStatus;
                    }


                    ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusChange statusChange = new ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null) ? true : false)
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing = false;
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.Id, 2, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.ModuleContractRepo.update(Inquiriess.ucStatuschange.ModuleContract);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing != true)
                            {
                                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null)
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing != true)
                            {
                                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.Id, 2, frmInputBox.comment);
                        }
                        else
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.stage = TransactionStage.AwaitingFirstReview.ToString();

                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.PendingForClosing = true;
                        }
                    //Adding Signature
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();
                    List<User> tagUsersRecommendation = new List<User>();
                    List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("ModuleContract has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.ModuleContract);
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
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.ModuleContractStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of ModuleContract(CFR) having value: " + row.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",
                        TaggedList = tagUsers,
                        CCUsersList = ccUsers,
                        TaggedRecomenndedList = tagUsersRecommendation,
                        CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.ModuleContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in ModuleContract #" + row.SalesReferenceNo, row.Id, TransactionItemType.ModuleContract, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in ModuleContract #" + row.SalesReferenceNo, row.Id, TransactionItemType.ModuleContract, comment.Comment, user.id, "New Comment ", null);
                        }
                    }
                    //}
                    ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.user_Id = MainWindow.currentUserid;
                    ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.LastStatusChangeDate = System.DateTime.Now;
                    ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.closingDate = System.DateTime.Now;
                    ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContractRepo.updateFromGrid(ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract);
                    MessageBox.Show("ModuleContract status changed to InActive (" + ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContract.ModuleContractStatus.Status + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close ModuleContract Directly.");
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "Pending ModuleContracts";
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) ModuleContract List") != null)
                {

                    ModuleContracts = ModuleContractrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)

                        ModuleContracts = ModuleContractrepo.getAllPendingforApprovalFirst(MainWindow.currentUserid);
                    else
                    {
                        ModuleContracts = ModuleContractrepo.getAllPendingforApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            }

            else
                ModuleContracts = ModuleContractrepo.getAllPendingsforAdministratorFirst();
            grdModuleContract.ItemsSource = ModuleContracts;
            grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;
            AllActive = 3;
            statusId = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "(Pending for Closing) ModuleContracts";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) ModuleContract List") != null)
                {

                    ModuleContracts = ModuleContractrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                    {
                        ModuleContracts = ModuleContractrepo.getAllPendingforClosingFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        ModuleContracts = ModuleContractrepo.getAllPendingforClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                ModuleContracts = ModuleContractrepo.getAllPendingforClosingAdministratorFirst();

            grdModuleContract.ItemsSource = ModuleContracts;
            grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;
            AllActive = 4;
            statusId = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdModuleContract);
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {


            lblHeading.Text = "Void ModuleContracts";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void ModuleContracts") != null)
                {
                    ModuleContracts = ModuleContractrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    ModuleContracts = ModuleContractrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                ModuleContracts = ModuleContractrepo.getVoidRegisterAdministrator();
            grdModuleContract.ItemsSource = ModuleContracts;
            grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;
        }
        private void loadModuleContractgridByDateRange()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "ModuleContracts(Open)" || lblHeading.Text == "ModuleContracts(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {

                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        ModuleContracts = ModuleContractrepo.getAll();


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive ModuleContracts") != null)
                    {
                        ModuleContracts = ModuleContractrepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                    else
                    {
                        ModuleContracts = ModuleContractrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    ModuleContracts = ModuleContractrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    ModuleContracts = ModuleContractrepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (ModuleContracts)";
                    if (MainWindow.currentUserid != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) ModuleContract List") != null)
                        {

                            ModuleContracts = ModuleContractrepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)

                                ModuleContracts = ModuleContractrepo.getAllPendingforApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            else
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    }

                    else
                        ModuleContracts = ModuleContractrepo.getAllPendingsforAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    grdModuleContract.ItemsSource = ModuleContracts;
                    grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) ModuleContracts";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) ModuleContract List") != null)
                        {

                            ModuleContracts = ModuleContractrepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                ModuleContracts = ModuleContractrepo.getAllPendingforClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                        ModuleContracts = ModuleContractrepo.getAllPendingforClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    grdModuleContract.Columns["CreationDate"].VisibleIndex = 0;
                }
                else
                    if (AllActive == 6)
                {

                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "ModuleContract Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                        {
                            ModuleContracts = ModuleContractRepo.ModuleContractRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            //All bills instead of own bills
                            //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                            ModuleContracts = ModuleContractrepo.ModuleContractRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    else
                        ModuleContracts = ModuleContractrepo.getModuleContractRegisterAdministrator();
                    grdModuleContract.ItemsSource = ModuleContracts;
                    statusId = 0;
                    AllActive = 6;
                }
            }
            else
            {
                ModuleContracts = ModuleContractrepo.getAllModuleContractsByStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdModuleContract);
            RemoveSourceObjects();
            this.grdModuleContract.ItemsSource = ModuleContracts;

        }
        private void RemoveSourceObjects()
        {
            grdModuleContract.Columns.GetColumnByFieldName("Id").Visible = false;
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("company"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("incoterm"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("ModuleContractStatus"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("paymentTerm"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("TitleValue1"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("TitleValue2"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("currency"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("bid"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("ModuleContract"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("vendor"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("employee"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("department"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("customerCompany"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("principal"));

            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("company_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("incoterm_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("ModuleContractStatus"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("paymentterm_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("TitleValue1Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("TitleValue2Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("currency_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("bid_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("ModuleContract_Id"));
            //grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("ModuleContract"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("allocation_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("dept_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("principal_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("user_Id"));
            grdModuleContract.Columns.Remove(grdModuleContract.Columns.GetColumnByFieldName("user"));


        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

                return;

            }
            else
            {
                loadModuleContractgridByDateRange();
                RemoveSourceObjects();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });

            }
        }

        private void mbtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
            lblHeading.Text = "ModuleContract Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ModuleContract without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 ModuleContract") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 ModuleContract") != null)
                {
                    ModuleContracts = ModuleContractRepo.getFirstModuleContractRegister(MainWindow.currentUserid);
                }
                else
                {
                    ModuleContracts = ModuleContractRepo.getFirstModuleContractRegister(MainWindow.currentUserid);
                }
            else
                ModuleContracts = ModuleContractRepo.getModuleContractRegisterAdministrator();
            grdModuleContract.ItemsSource = ModuleContracts;
            statusId = 0;
            AllActive = 6;
            mbtnExportToReport.IsEnabled = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
    }
}
