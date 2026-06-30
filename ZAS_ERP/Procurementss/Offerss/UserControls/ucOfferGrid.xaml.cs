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

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for ucOfferGrid.xaml
    /// </summary>
    public partial class ucOfferGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        OfferStatus oldStatus = new OfferStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        OfferRepo offerRepo = new OfferRepo();
        public ucOfferGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                {

                    ApprovalCount = offerrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                    {
                        ApprovalCount = offerrepo.getAllPendingforApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = offerrepo.getAllPendingforApprovalCountOwn(MainWindow.currentUserid);

                    }

                }

            }

            else
            {
                ApprovalCount = offerrepo.getAllPendingsforApprovalAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                {

                    ClosingCount = offerrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                    {
                        ClosingCount = offerrepo.getAllPendingforClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = offerrepo.getAllPendingforClosingCountown(MainWindow.currentUserid);

                    }

                }

            }

            else
            {
                ClosingCount = offerrepo.getAllPendingforClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Offers") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = offerrepo.getVoidRegisterCount(MainWindow.currentUserid);
                }
                else
                {
                    VoidCount = offerrepo.getVoidRegisterAdministratorCount();
                }
            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Offer Register") != null)
            {
                mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null ||
                    SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                {
                    ApproveunapprovedCount = offerRepo.getOfferRegisterCount(MainWindow.currentUserid);
                }
                else
                {
                    ApproveunapprovedCount = offerRepo.getOfferRegisterCountOWn(MainWindow.currentUserid);
                }
            }
            else
            {
                mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
            }
        }

        OfferRepo offerrepo = new OfferRepo();
        Offer offer = new Offer();
        List<ERP_BL.Databases.Offer> offers = new List<ERP_BL.Databases.Offer>();
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }

        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucofferGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null)
            //{
            //    mbtnPending.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    mbtnPending.Visibility = Visibility.Collapsed;

            //}
            loadOffergrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });



        }
        /// <summary>
        /// Loads data into offer Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadOffergrid()

        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Offers(Open)" || lblHeading.Text == "Offers(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        offers = offerrepo.getAll();


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Offers") != null)
                    {
                        offers = offerrepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        offers = offerrepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    offers = offerrepo.getAllActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    offers = offerrepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (Offers)";
                    if (MainWindow.currentUserid != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                        {

                            offers = offerrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)

                                offers = offerrepo.getAllPendingforApprovalFirst(MainWindow.currentUserid);
                            else
                            {
                                offers = offerrepo.getAllPendingforApprovalOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    }

                    else
                        offers = offerrepo.getAllPendingsforAdministratorFirst();
                    //grdoffer.Columns["CreationDate"].VisibleIndex = 0;

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Offers";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                        {

                            offers = offerrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                            {
                                offers = offerrepo.getAllPendingforClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                offers = offerrepo.getAllPendingforClosingOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    else
                        offers = offerrepo.getAllPendingforClosingAdministratorFirst();

                    //grdoffer.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                offers = offerrepo.getAllOffersByStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdoffer);
            this.grdoffer.ItemsSource = offers;

        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null) ? true : false)
            {
                if (grdoffer.SelectedItem != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    Offer offer = new Offer();
                    //Inquiriess.ucStatuschange.offerid = (int)grdoffer.GetFocusedRowCellValue(grdoffer.Columns.GetColumnByFieldName("Id"));
                    offer = grdoffer.SelectedItem as Offer;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    if (offer.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null) ? true : false)
                        {
                            offer.isApproved = true;
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, offer.Id, 2, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                        {
                            offer.stage = TransactionStage.AwaitingApproval.ToString();
                            if (offer.isApproved == null)
                            {
                                offer.isApproved = false;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null)
                        {
                            offer.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (offer.isApproved == null)
                            {
                                offer.isApproved = false;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        }
                        else
                        {
                            offer.isApproved = false;
                        }




                    offerrepo.updateFromGrid(offer);
                    MessageBox.Show("Inquiry is Approved (" + offer.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "Inquiry is Approved (" + offer.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve Offer Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve offer Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }
        private void grdoffer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditOffer();
        }

        private void EditOffer()
        {
            if (grdoffer.GetFocusedRowCellValue(grdoffer.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Offer, (int)grdoffer.GetFocusedRowCellValue(grdoffer.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();
            }
        }

        private void btnNewOffer_Click(object sender, RoutedEventArgs e)
        {


            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Offer, 0);
            procurmentPanel.Show();
        }

        private void btnEditOffer_Click(object sender, RoutedEventArgs e)
        {
            EditOffer();
        }

        private void Ucoffergrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(grdoffer);
            //statusId = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdoffer.View.ShowPrintPreview(this);
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

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Offers Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
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
                        ReportLogic.SaveGridReport(grdoffer, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Offers Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucOfferGrid ucOfferGrid = new ucOfferGrid();
            this.Content = ucOfferGrid;
            //loadOffergrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            PrintableControlLink link = new PrintableControlLink((TableView)grdoffer.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            //link.ReportHeaderTemplate= text as DataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdoffer.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null) ? true : false)
            {
                if (grdoffer.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    Offerss.ucStatuschange.offerid = (int)grdoffer.GetFocusedRowCellValue(grdoffer.Columns.GetColumnByFieldName("Id"));

                    var row = grdoffer.GetFocusedRow() as Offer;

                    if (row.offerStatus != null)
                    {
                        oldStatus = row.offerStatus;
                    }


                    Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (Offerss.ucStatuschange.offer.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null) ? true : false)
                        {
                            Offerss.ucStatuschange.offer.PendingForClosing = false;
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingApproval.ToString();
                            if (Offerss.ucStatuschange.offer.PendingForClosing != true)
                            {
                                Offerss.ucStatuschange.offer.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null)
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (Offerss.ucStatuschange.offer.PendingForClosing != true)
                            {
                                Offerss.ucStatuschange.offer.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                        }
                        else
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingFirstReview.ToString();

                            Offerss.ucStatuschange.offer.PendingForClosing = true;
                        }
                    //Adding Signature
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Offer has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Offer);
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
                    string newStat = Offerss.ucStatuschange.offer.offerStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Offer(CFR) having value: " + row.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + row.SalesReferenceNo, row.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + row.SalesReferenceNo, row.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);
                        }
                    }
                    //}
                    Offerss.ucStatuschange.offer.user_Id = MainWindow.currentUserid;
                    Offerss.ucStatuschange.offer.LastStatusChangeDate = System.DateTime.Now;
                    Offerss.ucStatuschange.offer.closingDate = System.DateTime.Now;
                    Offerss.ucStatuschange.offerRepo.updateFromGrid(Offerss.ucStatuschange.offer);
                    MessageBox.Show("Offer status changed to InActive (" + Offerss.ucStatuschange.offer.offerStatus.Status + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Offer Directly.");
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "Pending Offers";
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                {

                    offers = offerrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)

                        offers = offerrepo.getAllPendingforApprovalFirst(MainWindow.currentUserid);
                    else
                    {
                        offers = offerrepo.getAllPendingforApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            }

            else
                offers = offerrepo.getAllPendingsforAdministratorFirst();
            grdoffer.ItemsSource = offers;
            grdoffer.Columns["CreationDate"].VisibleIndex = 0;
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
            lblHeading.Text = "(Pending for Closing) Offers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                {

                    offers = offerrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                    {
                        offers = offerrepo.getAllPendingforClosingFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        offers = offerrepo.getAllPendingforClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                offers = offerrepo.getAllPendingforClosingAdministratorFirst();

            grdoffer.ItemsSource = offers;
            grdoffer.Columns["CreationDate"].VisibleIndex = 0;
            AllActive = 4;
            statusId = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdoffer);
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {


            lblHeading.Text = "Void Offers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Offers") != null)
                {
                    offers = offerrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    offers = offerrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                offers = offerrepo.getVoidRegisterAdministrator();
            grdoffer.ItemsSource = offers;
            grdoffer.Columns["CreationDate"].VisibleIndex = 0;
        }
        private void loadOffergridByDateRange()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Offers(Open)" || lblHeading.Text == "Offers(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {

                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        offers = offerrepo.getAll();


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Offers") != null)
                    {
                        offers = offerrepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                    else
                    {
                        offers = offerrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                if (AllActive == 1)
                {
                    offers = offerrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 2)
                {
                    offers = offerrepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (Offers)";
                    if (MainWindow.currentUserid != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Offer List") != null)
                        {

                            offers = offerrepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)

                                offers = offerrepo.getAllPendingforApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            else
                            {
                                offers = offerrepo.getAllPendingforApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    }

                    else
                        offers = offerrepo.getAllPendingsforAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    grdoffer.ItemsSource = offers;
                    grdoffer.Columns["CreationDate"].VisibleIndex = 0;

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Offers";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Offer List") != null)
                        {

                            offers = offerrepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                            {
                                offers = offerrepo.getAllPendingforClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                offers = offerrepo.getAllPendingforClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                        offers = offerrepo.getAllPendingforClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    grdoffer.Columns["CreationDate"].VisibleIndex = 0;
                }
                else
                    if (AllActive == 6)
                {

                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Offer Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                        {
                            offers = offerRepo.OfferRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            //All bills instead of own bills
                            //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                            offers = offerrepo.OfferRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    else
                        offers = offerrepo.getOfferRegisterAdministrator();
                    grdoffer.ItemsSource = offers;
                    statusId = 0;
                    AllActive = 6;
                }
            }
            else
            {
                offers = offerrepo.getAllOffersByStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdoffer);
            RemoveSourceObjects();
            this.grdoffer.ItemsSource = offers;

        }
        private void RemoveSourceObjects()
        {
            grdoffer.Columns.GetColumnByFieldName("Id").Visible = false;
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("company"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("incoterm"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("offerStatus"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("paymentTerm"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("TitleValue1"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("TitleValue2"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("currency"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("bid"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("offer"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("vendor"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("employee"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("department"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("customerCompany"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("principal"));

            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("company_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("incoterm_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("offerStatus"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("paymentterm_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("TitleValue1Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("TitleValue2Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("currency_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("bid_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("offer_Id"));
            //grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("offer"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("allocation_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("dept_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("principal_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("user_Id"));
            grdoffer.Columns.Remove(grdoffer.Columns.GetColumnByFieldName("user"));


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
                loadOffergridByDateRange();
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
            lblHeading.Text = "Offer Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                {
                    offers = offerRepo.getFirstOfferRegister(MainWindow.currentUserid);
                }
                else
                {
                    offers = offerRepo.getFirstOfferRegister(MainWindow.currentUserid);
                }
            else
                offers = offerRepo.getOfferRegisterAdministrator();
            grdoffer.ItemsSource = offers;
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
