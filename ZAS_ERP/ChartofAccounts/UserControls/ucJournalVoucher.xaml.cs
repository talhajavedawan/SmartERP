using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZAS_ERP.ChartofAccounts.ViewModels;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucJournalVoucher.xaml
    /// </summary>
    public partial class ucJournalVoucher : UserControl
    {
        JournalEntryRepo transactionRepo = new JournalEntryRepo();
        List<JournalTransaction> transactions = new List<JournalTransaction>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
        JournalTransaction transaction = new JournalTransaction();
        List<TransactionsViewModel> entries = new List<TransactionsViewModel>();
        JournalVoucherRepo jvRepo = new JournalVoucherRepo();
        JournalVoucher voucher = new JournalVoucher();
        public JournalVoucherStatus checkStatus = new JournalVoucherStatus();
        Currency currency = new Currency();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        Department department = new Department();
        Company company = new Company();
        //ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        int employeeId = 0;
        EmployeeRepo employeeRepo = new EmployeeRepo();
        DepartmentRepo departmentRepo = new DepartmentRepo();
        UsersRepo usersRepo = new UsersRepo();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        JournalVoucherStatus oldStatus = new JournalVoucherStatus();
        private int billId;
        private int purchaseOrderId;
        private int paymentGroupId;
        private int receiptGroupId;
        private int _EditableVoucherId;
        private bool isReceipt = false;

        public Bill bill= new Bill();
        public ucJournalVoucher()
        {
            InitializeComponent();
        }
        public ucJournalVoucher(int _paymentGroupId)
        {
            InitializeComponent();
            paymentGroupId = _paymentGroupId;
        }
        public ucJournalVoucher(int _receiptGroupId,bool _isReceipt)
        {
            InitializeComponent();
            receiptGroupId = _receiptGroupId;
            isReceipt = _isReceipt;
        }
        public ucJournalVoucher(string EditableVoucherId)
        {
            InitializeComponent();
            _EditableVoucherId =Convert.ToInt32( EditableVoucherId);
            this.voucher = jvRepo.GetJournalVoucherById(Convert.ToInt32(EditableVoucherId));
            if (voucher.Bill != null)
                billId = voucher.Bill.Id;
            if (voucher.PurchaseOrder != null)
                purchaseOrderId = voucher.PurchaseOrder.Id;
        }
        public ucJournalVoucher(int _transactionsId, TransactionItemType transactionType)
        {
            InitializeComponent();
            if (transactionType==TransactionItemType.Bill)
            billId = _transactionsId;
            if (transactionType == TransactionItemType.Purchase_Order)
                purchaseOrderId = _transactionsId;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);
            var userCompanies = companyRepo.GetUserCompanies(user.id);
            foreach (var company in userCompanies)
                companyIds.Add(company.Id);
            loadVoucherStatus();
            LoadCurrencies();
            LoadUserCompanies();
            LoadVoucherData();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with JV") != null)
            {
                btnAttachNew.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachNew.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with JV") != null)
            {
                btnAttachmentList.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachmentList.Visibility = Visibility.Collapsed;
            }
            if (voucher != null)
            {
                if (voucher.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (voucher.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (voucher.isApproved == true && voucher.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (voucher.isApproved == true && voucher.JournalVoucherStatus.isActive == false && voucher.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (voucher.isApproved == true && voucher.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (voucher.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (voucher.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (voucher.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View JV") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit JV") == null)
                {
                   
                    voucher = jvRepo.get(voucher.Id);

                    views = usersRepo.getViwerInfo(voucher.Id, (int)TransactionItemType.JV);
                    grdUsers.ItemsSource = views;
                    loadcomments();
                    btnSave.IsEnabled = false;
                 
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View JV") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit JV") != null)
                {
                    voucher = jvRepo.get(voucher.Id);
                    views = usersRepo.getViwerInfo(voucher.Id, (int)TransactionItemType.JV);
                    grdUsers.ItemsSource = views;
                    loadcomments();
                    btnSave.IsEnabled = true;

                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View JV") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit JV") != null)
                {
                    //isloading = true;
                    voucher = jvRepo.get(voucher.Id);

                    views = usersRepo.getViwerInfo(voucher.Id, (int)TransactionItemType.JV);
                    grdUsers.ItemsSource = views;
                    loadcomments();

                    btnSave.IsEnabled = true;
                }

                //Setting void stamp
                if (voucher != null)
                {
                    if (voucher.isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Edit JV!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void JV") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }

                if(!String.IsNullOrEmpty( voucher.voucherRefno))
                    lblJVRefNo.Text = voucher.voucherRefno;
            }
        }
        public void LoadUserCompanies()
        {
            cmbxCompany.ItemsSource = companyRepo.GetUserCompanies(MainWindow.currentUserid);
        }
        public void LoadCurrencies()
        {
            var currencyRepo = new CurrencyRepo();
            cmbxCurrency.ItemsSource = currencyRepo.getAll();
        }
        private void LoadVoucherData()
        {
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(voucher.Id, TransactionItemType.JV);

            if (voucher.Id != 0)
            {
                txtEntryNo.Text = voucher.entryNo;
                datPosting.EditValue = voucher.postingDate;
                txtVoucherRefNo.Text = voucher.voucherRefno;
                if (voucher.currencyId != null || voucher.Currency != null)
                {
                    currency = voucher.Currency;
                    cmbxCurrency.Text = currency.CurrencyName;
                }
                else
                {
                    cmbxCurrency.Text = "Select Currency";
                }

                if (voucher.company_Id != null || voucher.company != null)
                {
                    company = voucher.company;
                    cmbxCompany.Text = company.CompanyName;
                }
                else
                {
                    cmbxCompany.Text = "Select Company";
                }
                if (voucher.dept_Id != null || voucher.department != null)
                {
                    department = voucher.department;
                    lookupDepartment.Text = department.DeptName;
                }
                else
                {
                    lookupDepartment.Text = "Select Department";
                }
                if (voucher.emp_Id != null || voucher.employee != null)
                {
                    employeeId = voucher.employee.EmpId;
                    cmbxEmpId.Text = voucher.employee.person.FName;
                }
                else
                {
                    cmbxEmpId.Text = "Select Employee";
                }
                if (voucher.MER != 0)
                {
                    txtMER.Text = voucher.MER.ToString();
                }
                else
                {
                    txtMER.Text = 0.ToString();
                }
                grdJournalEntry.ItemsSource = jvRepo.getAllJournalTransactionsbyVoucherId(voucher.Id);
                lookupAccountsinGrid.ItemsSource = coaRepo.getAll(SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                voucher = null;
                grdJournalEntry.ItemsSource = transactions;
                lookupAccountsinGrid.ItemsSource = coaRepo.getAll(SYSTEM_STATIC.currentUser.id);

            }
        }

        public void loadVoucherStatus()
        {

            List<JournalVoucherStatus> VoucherStatuses = new List<JournalVoucherStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed JV") != null)
            {
                VoucherStatuses = jvRepo.getAllVoucherStatus();
            }
            else

                VoucherStatuses = jvRepo.getAllActiveVoucherStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();
            List<cmbitem> cmbitemsChange = new List<cmbitem>();

            Parallel.ForEach(VoucherStatuses, delegate (JournalVoucherStatus status)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            });
            cmbJVStatus.ItemsSource = cmbitems;
            if (voucher.Id != 0 && voucher.JournalVoucherStatus != null)
            {
                var JVSource = (List<cmbitem>)cmbJVStatus.Items.SourceCollection;

                if (voucher.JournalVoucherStatus.isActive == false)
                {
                    try
                    {
                        cmbJVStatus.SelectedItem = cmbJVStatus.Items[cmbJVStatus.Items.IndexOf(JVSource.Find(x => x.name == voucher.JournalVoucherStatus.Status))];
                    }

                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed JV status! " + ex.ToString());
                    }
                }
                else
                {
                    cmbJVStatus.SelectedItem = cmbJVStatus.Items[cmbJVStatus.Items.IndexOf(JVSource.Find(x => x.name == voucher.JournalVoucherStatus.Status))];
                }
                checkStatus = voucher.JournalVoucherStatus;
            }
        }

        public void LoadEntriesList()
        {
            var JVList = transactionRepo.GetAllTransactions();
            ChartofAccountsRepo accountRepo = new ChartofAccountsRepo();
            foreach (var transaction in JVList)
            {
                var trans = new TransactionsViewModel();
                trans.Id = transaction.Id;
                trans.Debit = transaction.debit;
                trans.Credit = transaction.credit;
                trans.coaTransactionsType = transaction.coaTransactionsType;
                trans.creationDate = transaction.creationDate;
                trans.isAdjustment = transaction.isAdjustment;
                trans.Memo = transaction.memo;
                trans.transactionRef = transaction.transactionRefno;
                trans.Account = accountRepo.GetAccountById(Convert.ToInt32(transaction.accountId));
                trans.journalVoucher = transaction.journalVoucher;
                entries.Add(trans);
            }
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            grdJournalEntry.Columns["debit"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;


            grdJournalEntry.Columns["credit"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            formSelectAccountType addAccountType = new formSelectAccountType();
            addAccountType.Show();
        }
        UsersRepo UsersRepo = new UsersRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (voucher == null)
            {
                voucher = new JournalVoucher();
            }
            CoaLogicClass ModuleLogic = new CoaLogicClass();
            try
            {
                if (Convert.ToDouble(grdJournalEntry.Columns["debit"].TotalSummaryText) == Convert.ToDouble(grdJournalEntry.Columns["credit"].TotalSummaryText))
                {
                    if (string.IsNullOrEmpty(txtEntryNo.Text))
                    {
                        DXMessageBox.Show("Please add Entry number to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtEntryNo.Focus();
                        return;
                    }
                    else
                    if (string.IsNullOrEmpty(txtVoucherRefNo.Text))
                    {
                        DXMessageBox.Show("Please add transaction reference number to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtVoucherRefNo.Focus();
                        return;
                    }
                    else
                    if (string.IsNullOrEmpty(datPosting.Text))
                    {
                        DXMessageBox.Show("Please add posting date to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        datPosting.Focus();
                        return;
                    }
                    else if (cmbJVStatus.SelectedIndex == -1 && voucher.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Current Status of Journal Voucher to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbJVStatus.Focus();
                        return;
                    }
                    else if (cmbxCompany.SelectedIndex == -1 && voucher.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Company for Journal Voucher to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbxCompany.Focus();
                        return;
                    }
                    else if (lookupDepartment.SelectedIndex == -1 && voucher.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Department for Journal Voucher to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupDepartment.Focus();
                        return;
                    }
                    else if (cmbxEmpId.SelectedIndex == -1 && voucher.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Employee for Journal Voucher to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbxEmpId.Focus();
                        return;
                    }
                    else
                    {
                        if(billId != 0)
                        {
                            voucher.bill_Id = billId;
                        }
                        if (purchaseOrderId != 0)
                        {
                            voucher.purchaseOrder_Id = purchaseOrderId;
                        }
                        if (currency != null)
                        {
                            voucher.currencyId = currency.Id;
                            voucher.Currency = currency;
                        }
                        if (employeeId > 0)
                        {
                            voucher.emp_Id = employeeId;

                        }
                        if (company != null)
                        {
                            voucher.company_Id = company.Id;

                        }
                        if (department != null)
                        {
                            voucher.dept_Id = department.Id;

                        }
                       
                        

                        voucher.postingDate = (DateTime)datPosting.EditValue;
                        voucher.voucherRefno = txtVoucherRefNo.Text.Trim();
                        voucher.entryNo = txtEntryNo.Text.Trim();
                        voucher.userId = SYSTEM_STATIC.currentUser.id;
                        if (!string.IsNullOrEmpty(txtMER.Text))
                            voucher.MER = Convert.ToDouble(txtMER.Text);
                        JournalVoucherStatus status = new JournalVoucherStatus();
                        if ((cmbJVStatus.SelectedItem as cmbitem) != null)
                        {
                             status = jvRepo.getstatus((cmbJVStatus.SelectedItem as cmbitem).id);
                            voucher.JournalVoucherStatus = status;
                        }
                        if (voucher.Id != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved JV") != null))
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null && voucher.isApproved != true)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This JV is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    voucher.stage = TransactionStage.Approved.ToString();
                                    voucher.isApproved = true;
                                    voucher.ApprovedDate = System.DateTime.Now;
                                }
                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without ReApproval") != null && voucher.isReApproved != true)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This JV is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    voucher.stage = TransactionStage.Approved.ToString();
                                    voucher.isReApproved = true;
                                    voucher.ReApprovalDate = System.DateTime.Now;
                                }
                            }
                            if (checkStatus != null && checkStatus.Id != 0)
                            {
                                if (checkStatus.Id != voucher.JournalVoucherStatus.Id)
                                {
                                    voucher.LastStatusChangeDate = System.DateTime.Now;
                                    if (voucher.JournalVoucherStatus.isActive != true)
                                    {
                                        voucher.ClosingDate = System.DateTime.Now;
                                    }
                                }
                            }
                            SystemLog.LogInfo(this.GetType(), "Journal Voucher Updated Succesfully refrence No= " + voucher.voucherRefno + " Id=" + voucher.Id);
                            ModuleLogic.UpdateVoucher(grdJournalEntry.VisibleItems, voucher);

                            if (checkStatus.Id != status.Id)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status of Journal Voucher has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (cmbxCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment((lookupDepartment.SelectedItem as Department).Id, (cmbxCompany.SelectedItem as Company).Id), voucher.Id, TransactionItemType.JV)
                                        {

                                        };
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

                                string oldStat = checkStatus.Status;
                                string newStat = status.Status;
                                string symbolCurr = "";
                                if (voucher.Currency != null)
                                {
                                    symbolCurr = voucher.Currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog();

                                comment.Comment = "Status of Journal Voucher having Voucher Ref #: " + voucher.voucherRefno.ToString() + " (" + symbolCurr + ")\n "
                           + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                comment.Timestamp = DateTime.Now;
                                comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                                procurementRepo.Add(voucher.Id, TransactionItemType.JV, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {

                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Journal Voucher with Voucher Ref #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {


                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Journal Voucher with Voucher Ref #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }



                            }

                            var myWindow = Window.GetWindow(this);
                            myWindow.Close();
                        }
                        else if (voucher.Id == 0)
                        {
                            if (MainWindow.currentUserid == 0)
                            {
                                DXMessageBox.Show("Please Create another Account to Create JV, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                                return;
                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV") != null)
                                {
                                    if (MainWindow.currentUserid == 0)
                                    {
                                        DXMessageBox.Show("Please Create another Account to Create journal Voucher, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                                        return;
                                    }
                                    else
                                        voucher.userId = MainWindow.currentUserid;
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null)
                                    {
                                        {
                                            voucher.stage = TransactionStage.Approved.ToString();
                                            voucher.isApproved = true;
                                            voucher.ApprovedDate = System.DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        voucher.stage = TransactionStage.AwaitingFirstReview.ToString();

                                        voucher.isApproved = false;
                                    }
                                    SystemLog.LogInfo(this.GetType(), "JournalVoucher Added Succesfully refrence No= " + voucher.voucherRefno + " Id=" + voucher.Id);
                                    if (paymentGroupId != 0)
                                    {
                                        voucher.paymentGroupId = paymentGroupId;
                                    }
                                    if (receiptGroupId != 0)
                                    {
                                        voucher.receiptGroupId = receiptGroupId;
                                    }
                                    ModuleLogic.AddVoucher(grdJournalEntry.VisibleItems, voucher);
                                    var myWindow = Window.GetWindow(this);
                                    myWindow.Close();
                                }
                            }
                        }
                    }
                }
                else
                    DXMessageBox.Show("Debit / credit are not equal", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void GrdListEntries_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                double cred_amount = Convert.ToDouble(e.GetListSourceFieldValue("debit"));
                double deb = Convert.ToDouble(e.GetListSourceFieldValue("credit"));
                if (cred_amount != 0)
                {
                    e.Value = cred_amount;
                }
                if (deb != 0)
                {
                    e.Value = deb;
                }
            }
        }

        private void CmbJVStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbJVStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbJVStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    formJournalVoucherStatusAdd statusAdd = new formJournalVoucherStatusAdd();
                    statusAdd.ShowDialog();
                    loadVoucherStatus();
                }
            }
        }

        private void CmbxCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxCurrency.SelectedItem as Currency != null)
            {
                int idd = (cmbxCurrency.SelectedItem as Currency).Id;
                currency = currencyRepo.get(idd);
            }
        }

        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedItem as Company != null)
            {
                int idd = (cmbxCompany.SelectedItem as Company).Id;
                company = companyRepo.GetCompany(idd);
                var departments = company.departments;
                lookupDepartment.ItemsSource = departments;
            }
        }
        private void CmbxEmpId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }
        private void CmbxEmpId_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxEmpId.SelectedItem as ERP_BL.Databases.Employee != null)
            {
                int idd = (cmbxEmpId.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                employeeId = idd;
            }
        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedItem as Department != null)
            {
                int idd = (lookupDepartment.SelectedItem as Department).Id;
                department = departmentRepo.GetDepartment(idd);
                var employees = department.employees;
                cmbxEmpId.ItemsSource = employees;
            }
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            Companiess.frmcompanyCenter.Editit = 1;
            Companiess.frmcompanyCenter.companyId = company.Id;
            frmcompanyadd.ShowDialog();
        }

        private void GrdJournalEntry_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "amountMER" && e.IsGetData)
            {
                var debit = Convert.ToDouble(e.GetListSourceFieldValue("debit"));
                var credit = Convert.ToDouble(e.GetListSourceFieldValue("credit"));
                if (debit != 0 && !string.IsNullOrEmpty(txtMER.Text) || credit != 0 && !string.IsNullOrEmpty(txtMER.Text))
                    e.Value = (debit - credit) * Convert.ToDouble(txtMER.Text);

            }
        }
        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            var transactionRef = (e.Value);
            if (transactionRef == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (transactionRef == null)
                {
                    e.ErrorContent = string.Format("The Transaction reference number can't be empty!",
                                                (e.CellValue));
                    return;
                }
            }
        }
        private void AccountName_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            var accountName = (e.Value);
            if (accountName == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (accountName == null)
                {
                    e.ErrorContent = string.Format("Please select account, Account can't be null!",
                                                (e.CellValue));
                    return;
                }
            }
        }
        private void DebitAmount_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            var creditAmount = Convert.ToDouble(((JournalTransaction)e.Row).credit);
            var debitAmount = (e.Value);

            if (creditAmount == 0 && debitAmount == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (accountName == null)
                {
                    e.ErrorContent = string.Format("Please add debit or credit amount, Account can't be null!",
                                                (e.CellValue));
                    return;
                }
            }

        }

        private void CreditAmount_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            var debitAmount = Convert.ToDouble(((JournalTransaction)e.Row).debit);
            var creditAmount = (e.Value);
            if (creditAmount == null && debitAmount == 0)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                if (accountName == null)
                {
                    e.ErrorContent = string.Format("Please add debit or credit amount, Account can't be null!",
                                                (e.CellValue));
                    return;
                }
            }
        }
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                voucher = jvRepo.GetJournalVoucherById(Convert.ToInt32(_EditableVoucherId));
                if (voucher != null )
                {

                    jvRepo = new JournalVoucherRepo();
                   
                    UsersRepo usersRepo = new UsersRepo();

                    if (voucher != null)
                    {
                        if (voucher.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("JV is Approved, Do you want to UnApprove this JV?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    voucher.isApproved = false;
                                    voucher.stage = TransactionStage.AwaitingApproval.ToString();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, voucher.Id, 6, "JV has been UnApproved");
                                    jvRepo.Approve(voucher);
                                    var res1 = MessageBox.Show("JV has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), voucher.Id, TransactionItemType.JV);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), voucher.Id, TransactionItemType.JV);
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
                                    string symbolCurr = "";
                                    if (voucher.Currency != null)
                                    {
                                        symbolCurr = voucher.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "JV (Amount OC) having value: " + voucher.journalTransactions.Sum(x=>x.debit).ToString() + "(" + symbolCurr + ") " + " has been UnApproved \nFrom: ",
                                        Timestamp = DateTime.Now,
                                        Subject = "JV UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(voucher.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.Purchase_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("JV is UnApproved (" + voucher.voucherRefno + ")");
                                    SystemLog.LogInfo(this.GetType(), "JV is UnApproved (" + voucher.Id + ")");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve JV Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve JV Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }
                        }
                        else if (voucher.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added JV") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();
                                var res = MessageBox.Show("JV are Pending for Approval, Do you want to Approve this JV?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    voucher.isApproved = true;
                                    voucher.stage = TransactionStage.Approved.ToString();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, voucher.Id, 6, "JV has been Approved");
                                    jvRepo.Approve(voucher);
                                    var res1 = MessageBox.Show("JV has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), voucher.Id, TransactionItemType.JV);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), voucher.Id, TransactionItemType.JV);
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

                                    string symbolCurr = "";
                                    if (voucher.Currency != null)
                                    {
                                        symbolCurr = voucher.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "JV (Amount OC) having value: " + voucher.journalTransactions.Sum(x=>x.debit).ToString() + "(" + symbolCurr + ") " + " has been Approved \nFrom: ",
                                        Timestamp = DateTime.Now,
                                        Subject = "JV Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(voucher.Id, TransactionItemType.JV, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("JV is Approved (" + voucher.voucherRefno + ")");
                                    SystemLog.LogInfo(this.GetType(), "JV is Approved (" + voucher.Id + ")");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve JV Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve JV Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }
                        }
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close JV without Approval") != null) ? true : false)
            {
                UsersRepo usersRepo = new UsersRepo();
                if (voucher.Id == 0)
                {
                    return;
                }
                voucher = jvRepo.get(voucher.Id);

                var row = voucher;
                if (row.JournalVoucherStatus != null)
                {
                    oldStatus = row.JournalVoucherStatus;
                }
                UserControls.ucJVStatusChange.inActiveStatuses = 1;
                UserControls.ucJVStatusChange.journalVoucherid = voucher.Id;
                frmJVStatusChange statusChange = new frmJVStatusChange(jvRepo);
                var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();

                //    return;
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
                    usersRepo.Add(TransactionInfo.Status_Changed, voucher.Id, (int)TransactionItemType.JV, "While direct closing Status Changed from (" + row.JournalVoucherStatus.Status + ") to (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");
                row = UserControls.ucJVStatusChange.journalVoucher;

                jvRepo.updateStatus(row.Id, row.JournalVoucherStatus);
                MessageBox.Show("Journal Voucher status changed to InActive (" + UserControls.ucJVStatusChange.journalVoucher.JournalVoucherStatus.Status + ")");



            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Journal Voucher Directly.");
            }

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (department != null && department.Id != 0 && company?.Id != 0 /*&& InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0*/)
            {


                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(deptIds, companyIds), TransactionItemType.JV);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.JV);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (voucher != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && voucher.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(voucher.Id, TransactionItemType.JV, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }
                    }

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (voucher.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Journal Voucher first to add a comment!");
                }

            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                loadcomments();
                grdComments.Visibility = Visibility.Visible;

            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (voucher.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void JV") != null))
            {
                if (DXMessageBox.Show("This Journal Voucher is currently in the list of Void Journal Vouchers! Do you want to remove it from Void?", "Remove Void Journal Voucher", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    voucher.isVoid = false;
                    jvRepo.setSotoVoid(voucher.Id, false);
                }
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                UsersRepo userRepo = new UsersRepo();
                //Asking for Tag
                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                var res1 = MessageBox.Show("JV has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res1 == MessageBoxResult.Yes)
                {
                    if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                    {
                        winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { voucher.department.Id }, new List<int> { voucher.company.Id }), voucher.Id, TransactionItemType.JV);
                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                      
                    }
                    else if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0)
                    {
                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(voucher.department.Id, voucher.company.Id), voucher.Id, TransactionItemType.JV);
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

                string symbolCurr = "";
                if (voucher.Currency != null)
                {
                    symbolCurr = voucher.Currency.Abbrivation.ToString();
                }
                CommentLog comment = new CommentLog()
                {
                    Comment = "JV (Amount OC) having value: " + voucher.journalTransactions.Sum(x=>x.debit).ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                    Timestamp = DateTime.Now,
                    Subject = "JV UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                };
                procurementRepo.Add(voucher.Id, TransactionItemType.JV, comment, SYSTEM_STATIC.currentUser.employeeId);
                //Creating Comments
                if (tagUsers.Count != 0)
                {
                    foreach (var user in tagUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, user.id, "New Comment ", null);

                    }
                }

                if (ccUsers.Count != 0)
                {
                    foreach (var user in ccUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, 0, user.id, "New Comment ", null);
                    }
                }

            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void JV") != null)
            {
                if (DXMessageBox.Show("This JV is not currently in the list of Void Journal Vouchers! Do you want to move it to Journal Vouchers?", "Add to Void Journal Vouchers", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    voucher.isVoid = true;
                    jvRepo.setSotoVoid(voucher.Id, true);
                }
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                UsersRepo userRepo = new UsersRepo();
                //Asking for Tag
                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                var res1 = MessageBox.Show("JV has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res1 == MessageBoxResult.Yes)
                {
                    if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0)
                    {
                        winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { voucher.department.Id }, new List<int> { voucher.company.Id}), voucher.Id, TransactionItemType.JV);
                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        
                    }
                    else if (voucher.department != null && voucher.department.Id != 0 && voucher.company?.Id != 0 )
                    {
                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(voucher.department.Id, voucher.company.Id), voucher.Id, TransactionItemType.JV);
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

                string symbolCurr = "";
                if (voucher.Currency != null)
                {
                    symbolCurr = voucher.Currency.Abbrivation.ToString();
                }
                CommentLog comment = new CommentLog()
                {
                    Comment = "JV (Amount OC) having value: " + voucher.journalTransactions.Sum(x=>x.debit).ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                    Timestamp = DateTime.Now,
                    Subject = "JV Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                };
                procurementRepo.Add(voucher.Id, TransactionItemType.JV, comment, SYSTEM_STATIC.currentUser.employeeId);
                //Creating Comments
                if (tagUsers.Count != 0)
                {
                    foreach (var user in tagUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, user.id, "New Comment ", null);

                    }
                }

                if (ccUsers.Count != 0)
                {
                    foreach (var user in ccUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in JV #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, comment.Comment, 0, user.id, "New Comment ", null);
                    }
                }
            }
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (voucher.Id != 0)
                {


                    if (department != null && department.Id != 0 && company?.Id != 0 /*&& InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0*/)
                    {


                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(deptIds, companyIds), comment, TransactionItemType.JV);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.JV);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (voucher != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && voucher.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(voucher.Id, TransactionItemType.JV, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in JournalVoucher #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in JournalVoucher #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in JournalVoucher #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in JournalVoucher #" + voucher.voucherRefno, voucher.Id, TransactionItemType.JV, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.JV, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (voucher.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Journal Voucher first to add a comment!");
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (department != null && department.Id != 0 && company?.Id != 0 /*&& InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0*/)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(deptIds, companyIds), comment, TransactionItemType.JV);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.JV);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && voucher.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.JV, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (voucher.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }
            }
            loadcomments();
        }


        public void loadcomments()
        {
            try
            {
                if (voucher != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(voucher.Id, TransactionItemType.JV);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {


                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    //DateTime date;

                    e.Value = fname + " " + lname;
                }
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (voucher.Id != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\JV\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);

                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += voucher.Id + "_" + TransactionItemType.JV.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.JV);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), voucher.Id, TransactionItemType.JV, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        usersRepo.Add(TransactionInfo.Attachment_Uploaded, voucher.Id, (int)TransactionItemType.JV, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(voucher.Id, TransactionItemType.JV);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.JV);
                        if (!string.IsNullOrEmpty(result.Item2))
                        {

                            Process.Start(result.Item2);

                        }
                        else

                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
                        }
                    });
                    thread.Start();
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void DXWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if (voucher != null && voucher.Id != 0)
                usersRepo.Add(TransactionInfo.viewed, voucher.Id, (int)TransactionItemType.JV, "Viewed details of Journal Voucher");
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (voucher.Id != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(voucher.Id, TransactionItemType.JV);
                trackingWindow.ShowDialog();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
