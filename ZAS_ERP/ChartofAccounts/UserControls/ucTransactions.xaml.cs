    using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Threading;
using ZAS_ERP.ChartofAccounts.ViewModels;
using ZAS_ERP.ChartofAccounts.Windows;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucTransactions.xaml
    /// </summary>
    public partial class ucTransactions : UserControl
    {
        public static int statusId;
        public static int AllActive;
        private CoaLogicClass ModuleLogic = new CoaLogicClass();
        JournalTransaction transaction = new JournalTransaction();
        JournalEntryRepo journalTransactionRepo = new JournalEntryRepo();
        public IList<JournalTransaction> JVList { get; set; }
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        public ucTransactions()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {


                var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

                deptIds = userDepartments.Select(x => x.Id).ToList();
                companyIds = userCompanies.Select(x => x.Id).ToList();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) JV List") != null)
                {
                    ApprovalCount = JVrepo.getAllPendingForApprovalCount(MainWindow.currentUserid, companyIds, deptIds);
                }
                else
                {
                    
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) JV List") != null)
                {
                    ReApprovalCount = JVrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid, companyIds, deptIds);

                }
                else
                {
                 
                }


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void JV") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = JVrepo.getVoidRegisterCount(MainWindow.currentUserid, companyIds, deptIds);
                    }
                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View JV Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    
                   
                        ApproveunapprovedCount = JVrepo.getJVRegisterCount(MainWindow.currentUserid, companyIds, deptIds);
                    
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) JV List") != null)
                {
                    ClosingCount = JVrepo.getAllPendingForClosingCount(MainWindow.currentUserid, companyIds, deptIds);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                    {
                        ClosingCount = JVrepo.getAllPendingForClosingCount(MainWindow.currentUserid, companyIds, deptIds);
                    }

                }
            }
        }
        JournalVoucherRepo JVrepo = new JournalVoucherRepo();
        JournalVoucher journalVoucher = new JournalVoucher();
        public IList<JournalVoucher> journalVouchers { get; set; }
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllAccountTransactions();
        }
        public void LoadAllAccountTransactions()
        {
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        journalVouchers = JVrepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive JV") != null)
                    {
                        journalVouchers = JVrepo.getAll(MainWindow.currentUserid,companyIds,deptIds);
                    }
                    else
                    {
                        journalVouchers = JVrepo.getAllActive(MainWindow.currentUserid, companyIds, deptIds);
                    }
                }
                else if (AllActive == 1)
                {
                    journalVouchers = JVrepo.getAllActive(MainWindow.currentUserid, companyIds, deptIds);

                }
                else if (AllActive == 2)
                {
                    journalVouchers = JVrepo.getAllInActive(MainWindow.currentUserid, companyIds, deptIds);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending for Approval (Journal Vouchers)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) JV List") != null)
                        {
                            journalVouchers = JVrepo.getAllPendingForApproval(MainWindow.currentUserid, companyIds, deptIds);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                            {
                                journalVouchers = JVrepo.getAllPendingForApproval(MainWindow.currentUserid, companyIds, deptIds);
                            }
                            else
                            {
                            }
                        }
                }
                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Journal Vouchers";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) JV List") != null)
                        {
                            journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                            {
                                journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);
                            }

                            else
                            {
                                journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);

                            }
                        }
                }

            }
            else
            {
                journalVouchers = JVrepo.getAllPobyStatusId(MainWindow.currentUserid, statusId, companyIds, deptIds);
            }

            grdListTransactions.ItemsSource = journalVouchers;
            {
                
                System.Threading.Thread th2 = new System.Threading.Thread(() =>
                {
                    if (grdListTransactions.Dispatcher.CheckAccess())
                    {
                        // The calling thread owns the dispatcher, and hence the UI element
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdListTransactions);
                    }
                    else
                    {
                        // Invokation required
                        grdListTransactions.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdListTransactions); }));
                    }


                });
                th2.Start();

                System.Threading.Thread th3 = new System.Threading.Thread(() =>
                {
                    recheckInnoke:

                    if (grdListTransactions.Dispatcher.CheckAccess())
                    {
                        // The calling thread owns the dispatcher, and hence the UI element
                        grdListTransactions.Columns.GetColumnByFieldName("Id").Visible = false;
                    }
                    else
                    {
                        // Invokation required
                        grdListTransactions.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {

                        }));
                    }
                });
                th3.Start();
            }
        }
        private void GrdListTransactions_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            double debit=0;
            double credit=0;
            double total = 0;
            if (e.IsGetData)
            {
                int voucherId = Convert.ToInt32( e.GetListSourceFieldValue("Id"));
                var voucherList = journalTransactionRepo.GetJournalTransactionsByVoucherId(voucherId);
                foreach(var vouchers in voucherList)
                {
                    
                    total =total+ vouchers.debit;
                }
                //double ded = Convert.ToDouble(e.GetListSourceFieldValue("debit"));
                e.Value = total;
            }
        }

        private void GrdListTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdListTransactions.GetFocusedRowCellValue(grdListTransactions.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.JV, (int)grdListTransactions.GetFocusedRowCellValue(grdListTransactions.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }

            //var journalWin = new ucJournalVoucher(grdListTransactions.SelectedItem as JournalVoucher);
            //    journalWin.Show();
            
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdListTransactions.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
           
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            UserControls.ucTransactions ucTransactionsGrid = new ucTransactions();
            this.Content = ucTransactionsGrid;
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null) ? true : false)
            {

                if (grdListTransactions.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    var row = grdListTransactions.GetFocusedRow() as JournalVoucher;
                    UserControls.ucJVStatusChange.inActiveStatuses = 1;

                    UserControls.ucJVStatusChange.journalVoucherid = (int)grdListTransactions.GetFocusedRowCellValue(grdListTransactions.Columns.GetColumnByFieldName("Id"));
                     frmJVStatusChange statusChange = new frmJVStatusChange(JVrepo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                   
                    if (UserControls.ucJVStatusChange.journalVoucherid != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null) ? true : false)
                        {
                            UserControls.ucJVStatusChange.journalVoucher.PendingForClosing = false;
                            UserControls.ucJVStatusChange.journalVoucher.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.JournalVoucherStatus.Status + ") to (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, UserControls.ucJVStatusChange.journalVoucherid, (int)TransactionItemType.JV, frmInputBox.comment);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                        {
                            UserControls.ucJVStatusChange.journalVoucher.stage = TransactionStage.AwaitingApproval.ToString();
                            if (UserControls.ucJVStatusChange.journalVoucher.PendingForClosing == null)
                            {
                                UserControls.ucJVStatusChange.journalVoucher.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.JournalVoucherStatus.Status + ") to (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, UserControls.ucJVStatusChange.journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null)
                        {
                            UserControls.ucJVStatusChange.journalVoucher.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (UserControls.ucJVStatusChange.journalVoucher.PendingForClosing != true)
                            {
                                UserControls.ucJVStatusChange.journalVoucher.PendingForClosing = true;

                            }

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.JournalVoucherStatus.Status + ") to (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, UserControls.ucJVStatusChange.journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                        }
                        else
                        {
                            UserControls.ucJVStatusChange.journalVoucher.stage = TransactionStage.AwaitingFirstReview.ToString();

                            UserControls.ucJVStatusChange.journalVoucher.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, UserControls.ucJVStatusChange.journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                        }
                    UserControls.ucJVStatusChange.journalVoucher.LastStatusChangeDate = System.DateTime.Now;
                    UserControls.ucJVStatusChange.journalVoucher.ClosingDate = System.DateTime.Now;
                    if (row.JournalVoucherStatus != UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, journalVoucher.Id, (int)TransactionItemType.JV, "While direct closing Status Changed from (" + row.JournalVoucherStatus.Status + ") to (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");
                    row = UserControls.ucJVStatusChange.journalVoucher;
                    grdListTransactions.RefreshData();
                    JVrepo.updateStatus(row.Id, row.JournalVoucherStatus);
                    MessageBox.Show("Journal Voucher status changed to InActive (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");

                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Journal Voucher Directly.");
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Journal Vouchers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) JV List") != null)
                {
                    journalVouchers = JVrepo.getAllPendingForApproval(MainWindow.currentUserid, companyIds, deptIds);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                    {
                        journalVouchers = JVrepo.getAllPendingForApproval(MainWindow.currentUserid, companyIds, deptIds);
                    }
                    else
                    {
                    }
                }
            grdListTransactions.ItemsSource = journalVouchers;
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View JV Register") != null )
                {
                    lblHeading.Text = "Journal Voucher Register";
                    journalVouchers = JVrepo.getJVRegister(MainWindow.currentUserid, companyIds, deptIds);
                }
                else
                {
                }
            grdListTransactions.ItemsSource = journalVouchers;
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending for ReApprovals Journal Vouchers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) JV List") != null)
                {
                    journalVouchers = JVrepo.getAllPendingForReApproval(MainWindow.currentUserid, companyIds,deptIds);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                    {
                        journalVouchers = JVrepo.getAllPendingForReApproval(MainWindow.currentUserid, companyIds, deptIds);
                    }
                }
            else
                journalVouchers = JVrepo.getAllPendingForReApproval(MainWindow.currentUserid, companyIds, deptIds);

            grdListTransactions.ItemsSource = journalVouchers;
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            lblHeading.Text = "(Pending for Closing) Journal Vouchers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) JV List") != null)
                {
                    journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                    {
                        journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);
                    }

                    else
                    {
                        journalVouchers = JVrepo.getAllPendingForClosing(MainWindow.currentUserid, companyIds, deptIds);

                    }
                }
            grdListTransactions.ItemsSource = journalVouchers;
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null) ? true : false)
            {
                if (grdListTransactions.GetFocusedRow() != null)
                {
                    JournalVoucher journalVoucher = new JournalVoucher();
                    //Inquiriess.ucStatuschange.purchaseOrderid = (int)grdpurchaseOrder.GetFocusedRowCellValue(grdpurchaseOrder.Columns.GetColumnByFieldName("Id"));
                    journalVoucher = grdListTransactions.SelectedItem as JournalVoucher;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    UsersRepo usersRepo = new UsersRepo();

                    if (journalVoucher.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null) ? true : false)
                        {
                            journalVoucher.isApproved = true;
                            journalVoucher.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                        {
                            journalVoucher.stage = TransactionStage.AwaitingApproval.ToString();
                            if (journalVoucher.isApproved == null)
                            {
                                journalVoucher.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null)
                        {
                            journalVoucher.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (journalVoucher.isApproved == null)
                            {
                                journalVoucher.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                        }
                        else
                        {
                            journalVoucher.isApproved = false;
                        }
                    else //reapprove when it is approved already
                    {
                        if (journalVoucher.isReApproved == false)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added JV") != null) ? true : false)
                            {
                                journalVoucher.isReApproved = true;
                                journalVoucher.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 JV") != null)
                            {
                                journalVoucher.stage = TransactionStage.AwaitingApproval.ToString();
                                if (journalVoucher.isReApproved == null)
                                {
                                    journalVoucher.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 JV") != null)
                            {
                                journalVoucher.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (journalVoucher.isReApproved == null)
                                {
                                    journalVoucher.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, journalVoucher.Id, (int)TransactionItemType.JV, frmInputBox.comment);
                            }
                            else
                            {
                                journalVoucher.isReApproved = false;
                            }
                    }
                    ModuleLogic.UpdateVoucher(journalVoucher);
                    MessageBox.Show("Journal Voucher is Approved (" + journalVoucher.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "Journal Voucher is Approved (" + journalVoucher.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve Journal Voucher Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Journal Voucher Directly user id=(" + MainWindow.currentUserid + ")");

            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Journal Vouchers";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Journal Vouchers") != null)
                {
                    journalVouchers = JVrepo.getVoidRegister(MainWindow.currentUserid,companyIds,deptIds);
                }

                else
                {
                    journalVouchers = JVrepo.getVoidRegister(MainWindow.currentUserid,companyIds,deptIds);

                }

            else
                journalVouchers = JVrepo.getVoidRegister(MainWindow.currentUserid, companyIds, deptIds);
            grdListTransactions.ItemsSource = journalVouchers;
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdListTransactions);
        }
    }
}
