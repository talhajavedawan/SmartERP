using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZAS_ERP.ChartofAccounts.ViewModels;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucChartofAccountList.xaml
    /// </summary>
    /// 
    public partial class ucChartofAccountList : UserControl
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        public int pendingforApprovedCounte = 0;
        public int ApprovedCounter = 0;
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        Company company = new Company();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        bool isFirstLoad = true;
        EmployeeRepo employeeRepo = new EmployeeRepo();

        public ucChartofAccountList()
        {

            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            //var companies = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
             companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x=>x.Id).ToList();
            //companyIds.AddRange(companies.Select(x => x.Id).ToList());
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();
            //deptIds = userDepartments.Select(x => x.Id).ToList();
           
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (this.isFirstLoad == true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LoadCounts();

                    });
                    //mbtnAllCompanies.ItemsSource = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId); 
                    mbtnAllCompanies.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;

                    //System.Threading.Thread th = new System.Threading.Thread(() =>
                    //{
                    LoadAccounts();
                    //});
                    //th.Start();
                    isFirstLoad = false;

                    Mouse.OverrideCursor = Cursors.Arrow;

                }
            });
        }
        public void LoadAccounts()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to COA Admin Panel") != null)
            {
                btnAdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdminPanel.Visibility = Visibility.Collapsed;
            }


            repo = new ChartofAccountsRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass COA Permissions") != null)
            {
                chartofAccounts = repo.GetAll();
            }
            else
            {
                if (MainWindow.currentUserid == 0)
                {
                    chartofAccounts = repo.getAll();
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Chart of Accounts") != null)
                {
                    chartofAccounts = repo.getAll(MainWindow.currentUserid);
                }
                else
                {
                    chartofAccounts = repo.getAllActive(MainWindow.currentUserid);
                }
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdAccounts);
            grdAccounts.ItemsSource = chartofAccounts;
            lblHeading.Text = "Chart of Accounts Register";
        }
        private void BtnNewAccount_Click(object sender, RoutedEventArgs e)
        {
            formSelectAccountType addAccountType = new formSelectAccountType();
            addAccountType.myParent = this;
            addAccountType.Show();
        }
        private void BtnEditAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                repo = new ChartofAccountsRepo();
                var account = repo.GetAccountById((grdAccounts.SelectedItem as ChartofAccount).Id);
                formAccountName addAccountForm = new formAccountName(account);
                addAccountForm.myParent = this;
                addAccountForm.Show();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdAccounts.ShowLoadingPanel = true;
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
             repo = new ChartofAccountsRepo();
            if (company.Id != 0)
            {
                chartofAccounts = repo.GetChartofAccountsByCompany(SYSTEM_STATIC.currentUser.id, company);
                grdAccounts.ItemsSource = chartofAccounts;
            }
            else
            {
                ucChartofAccountList ucChartofAccounts = new ucChartofAccountList();
                this.Content = ucChartofAccounts;
                grdAccounts.ShowLoadingPanel = false;
            }
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null) ? true : false)
            {
                if (grdAccounts.GetFocusedRow() != null)
                {
                    ChartofAccount chartofAccount = new ChartofAccount();
                    chartofAccount = repo.GetAccountById((grdAccounts.SelectedItem as ChartofAccount).Id);
                    UsersRepo usersRepo = new UsersRepo();
                    if (chartofAccount.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null) ? true : false)
                        {
                            chartofAccount.isApproved = true;
                            chartofAccount.stage = TransactionStage.Approved.ToString();
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                        {
                            chartofAccount.stage = TransactionStage.AwaitingApproval.ToString();
                            if (chartofAccount.isApproved == null)
                            {
                                chartofAccount.isApproved = false;
                            }
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null)
                        {
                            chartofAccount.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (chartofAccount.isApproved == null)
                            {
                                chartofAccount.isApproved = false;

                            }
                        }
                        else
                        {
                            chartofAccount.isApproved = false;
                        }
                    else //reapprove when it is approved already
                    {
                        if (chartofAccount.isReApproved == false)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add ChartofAccount without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added ChartofAccount") != null) ? true : false)
                            {
                                chartofAccount.isReApproved = true;
                                chartofAccount.stage = TransactionStage.Approved.ToString();
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                            {
                                chartofAccount.stage = TransactionStage.AwaitingApproval.ToString();
                                if (chartofAccount.isReApproved == null)
                                {
                                    chartofAccount.isReApproved = false;

                                }
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null)
                            {
                                chartofAccount.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (chartofAccount.isReApproved == null)
                                {
                                    chartofAccount.isReApproved = false;

                                }
                            }
                            else
                            {
                                chartofAccount.isReApproved = false;
                            }

                    }

                    ModuleLogic.UpdateAccount(chartofAccount);

                    DXMessageBox.Show("Account is Approved (" + chartofAccount.Id + ")", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCounts();
                }
            }
            else
            {
                DXMessageBox.Show("You are not Allowed to Approve Account Directly", "Permission Required!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null)
                {
                    var pendingApprovedAccount = grdAccounts.SelectedItem as ChartofAccount;
                    if (pendingApprovedAccount != null)
                    {
                        repo.ApproveAccount(pendingApprovedAccount);
                        DXMessageBox.Show("Account has been Approved!", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadAccounts();
                        LoadCounts();
                    }
                }
                else
                    DXMessageBox.Show("Permission required to approve Account", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            LoadPendingForApprovalsList();
        }
        public void LoadPendingForApprovalsList()
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Chart of Accounts";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Chart of Account List") != null)
                {
                    chartofAccounts = repo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                    {
                        chartofAccounts = repo.getAllPendingForApproval(MainWindow.currentUserid);
                    }
                    else
                    {
                        chartofAccounts = repo.getAllPendingForApprovalOwn(MainWindow.currentUserid);
                    }
                }
            else
                chartofAccounts = repo.getAllPendingForAdministrator();
            grdAccounts.ItemsSource = chartofAccounts;
        }
        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
        }
        private void GrdAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdAccounts.SelectedItem as ChartofAccount;
           var company= mbtnAllCompanies.SelectedItem as Company;
            
            if (selectedAccount != null)
            {
                var chartofAccount=repo.get(selectedAccount.Id);
                if (company != null)
                {
                    winLedger ledger = new winLedger(chartofAccount, company);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
                else
                {
                    winLedger ledger = new winLedger(chartofAccount);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
               
                //ledger.deptIds = deptIds;
                //ledger.companyIds = companyIds;
                
            }
        }
        public void LoadCounts()
        {
            repo = new ChartofAccountsRepo();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Chart of Account List") != null)
                {
                    CountersModel._ApprovalCount = repo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Account") != null)
                    {
                        CountersModel._ApprovalCount = repo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        CountersModel._ApprovalCount = repo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Chart of Accounts List") != null)
                {
                    CountersModel._ReApprovalCount = repo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                    {
                        CountersModel._ReApprovalCount = repo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        CountersModel._ReApprovalCount = repo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Chart of Accounts") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        CountersModel._VoidCount = repo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        CountersModel._VoidCount = repo.getVoidRegisterAdministratorCount();

                    }
                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Chart of Account Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Chart of Account") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Chart of Account without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                    {
                        CountersModel._ApproveunapprovedCount = repo.getChartofAccountRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        CountersModel._ApproveunapprovedCount = repo.getChartofAccountRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                CountersModel._ApprovalCount = repo.getAllPendingForAdministratorCount();
                CountersModel._ApproveunapprovedCount = repo.getChartofAccountRegisterAdministratorCount();
            }
            DataContext = CountersModel.GetCountersDetails();
           
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();

            lblHeading.Text = "Pending for ReApprovals Chart of Accounts";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Chart of Accounts List") != null)
                {
                    chartofAccounts = repo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added ChartofAccount") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Chart of Accounts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Chart of Accounts") != null)
                    {
                        chartofAccounts = repo.getAllPendingForReApproval(MainWindow.currentUserid);
                    }

                    else
                    {
                        chartofAccounts = repo.getAllPendingForReApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else
                chartofAccounts = repo.getAllPendingForAdministrator();
            grdAccounts.ItemsSource = chartofAccounts;
           
        }
        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            lblHeading.Text = "Void Chart of Accounts";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Chart of Accounts") != null)
                {
                    chartofAccounts = repo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    chartofAccounts = repo.getVoidRegisterOwn(MainWindow.currentUserid);

                }
            else
                chartofAccounts = repo.getVoidRegisterAdministrator();
            grdAccounts.ItemsSource = chartofAccounts;
        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdAccounts);
        }

        private void MbtnApprovedtransactions_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MbtnAlltransactions_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Clear COA Transactions") != null)
            {
                var account = grdAccounts.SelectedItem as ChartofAccount;
                JournalEntryRepo repo = new JournalEntryRepo();
                var transactions = repo.GetAllJournalTransactionByaccountId(account.Id, deptIds);
               
                repo.RemoveTransactions(transactions);
                DXMessageBox.Show("Transactions has been removed successfully!", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                DXMessageBox.Show("You do not have permission Clear COA Transactions!", "Permission Denied",MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MbtnAllCompanies_Click(object sender, RoutedEventArgs e)
        {
           
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompletedAllCompanies;
            worker.RunWorkerAsync();
        }
        private void OnRunWorkerCompletedAllCompanies(object o, RunWorkerCompletedEventArgs args)
        {



            ucChartofAccountList chartofAccounts = new ucChartofAccountList();


            this.Content = chartofAccounts;

          
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());

            PrintableControlLink link = new PrintableControlLink((TreeListView)grdAccounts.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }
        private void GrdAccountsTree_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {

                    var chartofAccount = e.Node.Content as ChartofAccount;
                    //var transactions = chartofAccount.JournalTransactions.Where(x =>
                    //  x.AdminBill?.isVoid != true &&
                    //  x.Bill?.isVoid != true &&
                    //  x.journalVoucher?.isVoid != true &&
                    //  x.InterBank?.isVoid != true &&
                    //  x.Payment?.isVoid != true &&
                    //  x.SaleInvoice?.isVoid != true &&
                    //  x.SalesReceipt?.isVoid != true &&
                    //  x.InterCompanyTransfer?.isVoid != true &&
                    //  x.deptId != null &&
                    //  deptIds.Contains((int)x.deptId) &&
                    //  x.companyId != null &&
                    //  companyIds.Contains((int)x.companyId)).ToList();
                    //var totalDebits = transactions.Sum(x => x.debit);
                    //var totalDebitsPKR = transactions.Sum(x => x.debit * x.MER);

                    //var totalCredits = transactions.Sum(x => x.credit);
                    //var totalCreditsPKR = transactions.Sum(x => x.credit * x.MER);



                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = e.Node.Content as ChartofAccount;
                        if (chartofAccount.Companies != null && chartofAccount.Companies.Count != 0)
                        {
                            var res = String.Join(", ", chartofAccount.Companies.Select(x => x.CompanyName));
                            e.Value = res;
                        }
                    }
                    else
                   if (e.Column.FieldName == "Department")
                    {
                        var chartofAccount2 = e.Node.Content as ChartofAccount;
                        var res = String.Join(", ", chartofAccount.Departments.Select(x => x.DeptName));
                        e.Value = res;
                    }
                    //else
                   //if (e.Column.FieldName == "balanceOC")
                   // {
                   //     if (mbtnAllCompanies.SelectedIndex > -1)
                   //     {
                   //         var company = mbtnAllCompanies.SelectedItem as Company;
                   //         var value = chartofAccount.JournalTransactions.Where(
                   //                                     x => x.AdminBill?.isVoid != true
                   //                                     && x.Bill?.isVoid != true
                   //                                     && x.PurchaseInvoice?.isVoid != true
                   //                                     && x.SaleInvoice?.isVoid != true
                   //                                     && x.SalesReceipt?.isVoid != true
                   //                                     && x.journalVoucher?.isVoid != true
                   //                                     && x.Payment?.isVoid != true
                   //                                     && x.InterBank?.isVoid != true
                   //                                     && x.InterCompanyTransfer?.isVoid != true &&
                   //                                     x.creationDate != null &&
                   //                                     x.deptId != null &&
                   //                                     deptIds.Contains((int)x.deptId) &&
                   //                                     x.companyId != null &&
                   //                                     x.companyId==company.Id &&
                   //                                     companyIds.Contains((int)x.companyId)
                   //                             ).Sum(x => x.total);
                   //                     e.Value = e.Value = Math.Round(value, 2);
                   //     }
                   //     else
                   //     {
                   //         var value = chartofAccount.JournalTransactions.Where(
                   //           x => x.AdminBill?.isVoid != true
                   //           && x.Bill?.isVoid != true
                   //           && x.PurchaseInvoice?.isVoid != true
                   //           && x.SaleInvoice?.isVoid != true
                   //           && x.SalesReceipt?.isVoid != true
                   //           && x.journalVoucher?.isVoid != true
                   //           && x.Payment?.isVoid != true
                   //           && x.InterBank?.isVoid != true
                   //           && x.InterCompanyTransfer?.isVoid != true &&
                   //           x.creationDate != null &&
                   //           x.deptId != null &&
                   //           deptIds.Contains((int)x.deptId) &&
                   //           x.companyId != null &&
                   //           companyIds.Contains((int)x.companyId)
                   //         ).Sum(x => x.total);
                   //         e.Value = e.Value = Math.Round(value, 2);
                   //     }
                        

                   // }
                   // if (e.Column.FieldName == "balancePKR")
                   // {
                   //     if (mbtnAllCompanies.SelectedIndex > -1)
                   //     {
                   //         var company = mbtnAllCompanies.SelectedItem as Company;
                   //         var value = chartofAccount.JournalTransactions.Where(
                   //        x => x.AdminBill?.isVoid != true
                   //         && x.Bill?.isVoid != true
                   //         && x.PurchaseInvoice?.isVoid != true
                   //         && x.SaleInvoice?.isVoid != true
                   //         && x.SalesReceipt?.isVoid != true
                   //         && x.journalVoucher?.isVoid != true
                   //         && x.Payment?.isVoid != true
                   //         && x.InterBank?.isVoid != true
                   //         &&
                   //       x.creationDate != null

                   //         && x.InterCompanyTransfer?.isVoid != true
                   //         &&
                   //        x.deptId != null && deptIds.Contains((int)x.deptId)
                   //        &&
                   //        x.companyId != null && companyIds.Contains((int)x.companyId) &&
                   //         x.companyId == company.Id 
                   //        ).Sum(x => x.total * x.MER);

                   //         e.Value = value;
                   //     }
                   //     else
                   //     {
                   //         var value = chartofAccount.JournalTransactions.Where(
                   //         x => x.AdminBill?.isVoid != true
                   //          && x.Bill?.isVoid != true
                   //          && x.PurchaseInvoice?.isVoid != true
                   //          && x.SaleInvoice?.isVoid != true
                   //          && x.SalesReceipt?.isVoid != true
                   //          && x.journalVoucher?.isVoid != true
                   //          && x.Payment?.isVoid != true
                   //          && x.InterBank?.isVoid != true
                   //          &&
                   //        x.creationDate != null

                   //          && x.InterCompanyTransfer?.isVoid != true
                   //          &&
                   //         x.deptId != null && deptIds.Contains((int)x.deptId)
                   //         &&
                   //         x.companyId != null && companyIds.Contains((int)x.companyId)
                   //         ).Sum(x => x.total * x.MER);

                   //         e.Value = value;
                   //     }
                   //     //e.Value = Math.Round(totalDebitsPKR, 2);
                   // }
                
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MbtnExportReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Chart of Accounts Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
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
                        ReportLogic.SaveGridReport(grdAccounts, reportName, reportType, reportGroup, lblHeading.Text,report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Chart of Account Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void mbtnAlltrans_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Clear COA Transactions") != null)
            {
                ucClearTransaction ucClearTransaction = new ucClearTransaction();
                ucClearTransaction.ShowDialog();
                if (ucClearTransaction.company.Id != 0 && ucClearTransaction.department.Id != 0)
                {
                    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to clear " + ucClearTransaction.company.CompanyName + " and " + ucClearTransaction.department.DeptName + " Transactions?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {
                        var account = grdAccounts.SelectedItem as ChartofAccount;
                        JournalEntryRepo repo = new JournalEntryRepo();
                        int compid = ucClearTransaction.company.Id;
                        int deptId = ucClearTransaction.department.Id;
                        repo.RemoveByCompsnyDep(compid, deptId);
                        DXMessageBox.Show("Transactions has been removed successfully!", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            else
            {
                DXMessageBox.Show("You do not have permission Clear COA Transactions!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void MbtnBypass_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass COA Permissions") != null)
            {
                grdAccounts.ShowLoadingPanel = true;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += OnDoWork1;
                worker.RunWorkerCompleted += OnRunWorkerCompleted1;
                worker.RunWorkerAsync();
            }
            else
            {
                DXMessageBox.Show("You do not have permission Bypass COA Permissions!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void OnDoWork1(object sender, DoWorkEventArgs e)
        {
            Task.Delay(4000).Wait();
        }

        private void OnRunWorkerCompleted1(object sender, RunWorkerCompletedEventArgs e)
        {
            repo = new ChartofAccountsRepo();
                chartofAccounts = repo.GetAll();
                grdAccounts.ItemsSource = chartofAccounts;
            grdAccounts.ShowLoadingPanel = false;
        }

        private void BtnCollaps_Click(object sender, RoutedEventArgs e)
        {
            grdAccounts.ShowLoadingPanel = true;
            grdAccountsTree.CollapseAllNodes();
            grdAccounts.ShowLoadingPanel = false;
        }

        private void BtnExpand_Click(object sender, RoutedEventArgs e)
        {
            grdAccounts.ShowLoadingPanel = true;
            grdAccountsTree.ExpandAllNodes();
            grdAccounts.ShowLoadingPanel = false;
        }

        private void mbtnAllCompanies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            grdAccounts.RefreshData();
            //if((mbtnAllCompanies.SelectedItem as Company)!=null)
            //{
            //    //var company = mbtnAllCompanies.SelectedItem as Company;
            //    //ChartofAccountsRepo accountsRepo = new ChartofAccountsRepo();
            //    //grdAccounts.ItemsSource=accountsRepo.GetChartofAccountsByCompany(SYSTEM_STATIC.currentUser.id,company);
            //}
        }

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            winCOAAdminPanel adminPanel = new winCOAAdminPanel();
            adminPanel.Show();
        }
        private void btnGroupAdminPanel_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnLoanBalances_Click(object sender, RoutedEventArgs e)
        {
            if (grdAccounts.SelectedItem != null)
            {
                var chartofAccount = grdAccounts.SelectedItem as ChartofAccount;
                LoadBalanceOC(chartofAccount);
                LoadBalanceCMER(chartofAccount);
                grdAccounts.RefreshData();
            }
        }
        public void LoadBalanceOC(ChartofAccount chartofAccount)
        {
            if (mbtnAllCompanies.SelectedIndex > -1)
            {
                var company = mbtnAllCompanies.SelectedItem as Company;
                var value = chartofAccount.JournalTransactions.Where(
                                            x => x.AdminBill?.isVoid != true
                                            && x.Bill?.isVoid != true
                                            && x.PurchaseInvoice?.isVoid != true
                                            && x.SaleInvoice?.isVoid != true
                                            && x.SalesReceipt?.isVoid != true
                                            && x.journalVoucher?.isVoid != true
                                            && x.Payment?.isVoid != true
                                            && x.InterBank?.isVoid != true
                                            && x.InterCompanyTransfer?.isVoid != true &&
                                            x.creationDate != null &&
                                            x.deptId != null &&
                                            deptIds.Contains((int)x.deptId) &&
                                            x.companyId != null &&
                                            x.companyId == company.Id &&
                                            companyIds.Contains((int)x.companyId)
                                    ).Sum(x => x.total);

                chartofAccount.BalanceOC = Math.Round(value, 2);
            }
            else
            {
                var value = chartofAccount.JournalTransactions.Where(
                  x => x.AdminBill?.isVoid != true
                  && x.Bill?.isVoid != true
                  && x.PurchaseInvoice?.isVoid != true
                  && x.SaleInvoice?.isVoid != true
                  && x.SalesReceipt?.isVoid != true
                  && x.journalVoucher?.isVoid != true
                  && x.Payment?.isVoid != true
                  && x.InterBank?.isVoid != true
                  && x.InterCompanyTransfer?.isVoid != true &&
                  x.creationDate != null &&
                  x.deptId != null &&
                  deptIds.Contains((int)x.deptId) &&
                  x.companyId != null &&
                  companyIds.Contains((int)x.companyId)
                ).Sum(x => x.total);
                chartofAccount.BalanceOC = Math.Round(value, 2);
            }
        }
        public void LoadBalanceCMER(ChartofAccount chartofAccount)
        {
            if (mbtnAllCompanies.SelectedIndex > -1)
            {
                var company = mbtnAllCompanies.SelectedItem as Company;
                var value = chartofAccount.JournalTransactions.Where(
               x => x.AdminBill?.isVoid != true
                && x.Bill?.isVoid != true
                && x.PurchaseInvoice?.isVoid != true
                && x.SaleInvoice?.isVoid != true
                && x.SalesReceipt?.isVoid != true
                && x.journalVoucher?.isVoid != true
                && x.Payment?.isVoid != true
                && x.InterBank?.isVoid != true
                &&
              x.creationDate != null

                && x.InterCompanyTransfer?.isVoid != true
                &&
               x.deptId != null && deptIds.Contains((int)x.deptId)
               &&
               x.companyId != null && companyIds.Contains((int)x.companyId) &&
                x.companyId == company.Id
               ).Sum(x => x.total * x.MER);

                chartofAccount.BalancePKR = Math.Round(value, 2);
            }
            else
            {
                var value = chartofAccount.JournalTransactions.Where(
                x => x.AdminBill?.isVoid != true
                 && x.Bill?.isVoid != true
                 && x.PurchaseInvoice?.isVoid != true
                 && x.SaleInvoice?.isVoid != true
                 && x.SalesReceipt?.isVoid != true
                 && x.journalVoucher?.isVoid != true
                 && x.Payment?.isVoid != true
                 && x.InterBank?.isVoid != true
                 &&
               x.creationDate != null

                 && x.InterCompanyTransfer?.isVoid != true
                 &&
                x.deptId != null && deptIds.Contains((int)x.deptId)
                &&
                x.companyId != null && companyIds.Contains((int)x.companyId)
                ).Sum(x => x.total * x.MER);

                chartofAccount.BalancePKR = Math.Round(value, 2);
            }
        }
    }
}
    //=================================================================================================================================================

