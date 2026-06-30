using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Printing;
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
using ZAS_ERP.Bankings.InterBankTransfer.Windows;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using DevExpress.CodeParser;
using ERP_BL.ToDoTasks.Taskss;

namespace ZAS_ERP.Bankings
{
    /// <summary>
    /// Interaction logic for ucFrmBankTransfers.xaml
    /// </summary>
    public partial class ucFrmBankTransfers : UserControl
    {
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        ERP_BL.Procurements.InterBankTransfers.InterBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();
        public int bankTransferId = 0;
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        public UcListWindow frmBankTranfer = new UcListWindow();
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<Product> products { get; set; }
        List<ViewInfo> views = new List<ViewInfo>();
        UsersRepo UsersRepo = new UsersRepo();
        Department department = new Department();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        InterBankTransferStatus oldStatus = new InterBankTransferStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        User loginUser = new User();
        List<Bank> allowedBanks = new List<Bank>();
        double AmountMerCheck = 0;
        ERP_BL.Procurements.InterBankTransfers.InterBankTransfer trackingOrder = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();

        
        static InterBankTransferStatus statusChanged = new InterBankTransferStatus();


        static string systemRefIntitials = "IBT-";


        public bool editFlag = false;

        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        public int paymentGroupId = 0;
        public int receiptGroupId = 0;
        public  int vendorBillId= 0;
        public  int adminBillId= 0;
        public  int stlId= 0;
        PaymentRepo paymentRepo = new PaymentRepo();
        TaxRepo taxRepo = new TaxRepo();
        List<InterBankTransferStatus> allInterBankTransStatus = new List<InterBankTransferStatus>();
        List<cmbitem> interBankTransStatusLst = new List<cmbitem>();

        public ucFrmBankTransfers()
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
        }
        public ucFrmBankTransfers(TransactionItemType type, int _billId)
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            if (type == TransactionItemType.Bill)
            {
                vendorBillId = _billId;
            }
            else
            if (type == TransactionItemType.Admin_Bill)
            {
                adminBillId = _billId;
            }
            else
            if (type == TransactionItemType.STL)
            {
                stlId = _billId;
            }
        }
        public ucFrmBankTransfers(int _paymentGroupId)
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            paymentGroupId = _paymentGroupId;

        }
        public ucFrmBankTransfers(int _receiptGroupId, bool _isReceipt)
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();

            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            receiptGroupId = _receiptGroupId;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBankTransferData();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Inter-Bank Transfer") != null)
            {
                datCreationDate.IsEnabled = true;
            }
        }


        private void loadBillReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();
            references = BillsRepo.GetAllActiveBillReferenceNo((cmbxCompany.SelectedItem as Company).Id);

            if (editFlag == true && bankTransfer != null)
            {
                if (bankTransfer.PettyCashRef != null && references.FirstOrDefault(x => x.Id == bankTransfer.PettyCashRefId) == null)
                    references.Add(bankTransfer.PettyCashRef);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }
            cmbxPettyCashRef.ItemsSource = cmbitems;
        }

        /// <summary>
        /// Company Selected index Changed Event
        /// </summary>
        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxCompany.SelectedItem != null)
            {
                var company = cmbxCompany.SelectedItem as Company;

                if (company != null)
                    if (company.departments != null)
                    {
                        List<Department> departments = new List<Department>();
                        foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsInterBankTransferType == true))
                        {
                            if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                                departments.Add(_dept);
                        }
                        if (bankTransfer != null && bankTransfer.Id > 0 && editFlag == true)
                            if (bankTransfer.department != null && departments.FirstOrDefault(x => x.Id == bankTransfer.dept_Id) == null)
                                departments.Add(bankTransfer.department);

                        cmbxDepartment.ItemsSource = departments;

                        if (departments.Count == 0)
                        {
                            MessageBox.Show("This company dosen't contain any department mapped with the current User");
                        }
                    }
                loadBillReferenceNo();
                loadVATBookReferenceNo();

                var bankList = receiptRepo.GetAllBanksByCompany(company.Id);

                //Only Allowed departments to Employee will show in Dropdown
                cmbxBankFrom.ItemsSource = bankList;
                cmbxBankTo.ItemsSource = bankList;
            }
            
        }

        /// <summary>
        /// Department Selected index Changed Event
        /// </summary>
        private void CmbxDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            if (cmbxDepartment.SelectedItem != null)
            {
                //Saving the Selected Company
                department = new Department();
                department = cmbxDepartment.SelectedItem as ERP_BL.Databases.Department;

                var accounts = department.accounts;
                var employees = department.employees;

                cmbxEmpId.ItemsSource = employees;

                //List<Bank> bankList = new List<Bank>();
                //foreach (var _account in accounts)
                //{
                //    //allAccounts.Add(_account);
                //    if (!bankList.Contains(_account.bank))
                //        bankList.Add(_account.bank);
                //}
                //cmbxBankFrom.ItemsSource = bankList;
                //cmbxBankTo.ItemsSource = bankList;
            }

        }

        /// <summary>
        /// Bank Selected index Changed Event
        /// </summary>
        private void CmbxBankFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxBankFrom.SelectedItem != null)
            {
                var bank = cmbxBankFrom.SelectedItem as Bank;
                var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x=>x.isActive == true).ToList();/*.Where(x => x.departments.Contains(department) && x.company.Id == (cmbxCompany.SelectedItem as Company).Id && x.accountsCategory == AccountsCategory.Company).ToList()*/
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(department.Id) && _account.bank.Id == bank.Id && _account.company.Id == (cmbxCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                cmbxAccountFrom.ItemsSource = allowedAccounts;
            }
        }

        /// <summary>
        /// Bank Selected index Changed Event
        /// </summary>
        private void CmbxBankTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.IBT_Single_Currency || cmbxTransferType.SelectedIndex == 2)
            {
                if (cmbxBankTo.SelectedItem != null)
                {
                    var bank = cmbxBankTo.SelectedItem as Bank;
                    var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();

                    List<Account> allowedAccounts = new List<Account>();


                    foreach (var _account in accntList)
                    {
                        List<int> dept_ids = new List<int>();

                        foreach (var _dept in _account.departments)
                        {
                            dept_ids.Add(_dept.Id);
                        }
                        if (dept_ids.Contains(department.Id) && _account.bank.Id == bank.Id && _account.company.Id == (cmbxCompany.SelectedItem as Company).Id)
                            allowedAccounts.Add(_account);
                    }
                    cmbxAccountTo.ItemsSource = allowedAccounts;
                }
            }
            
        }


        private string calculateSystemRefNo()
        {
            string sysRefNo;
            string lastSystemRefNo;

            lastSystemRefNo = bankTransRepo.getLastSystemReferenceNo();

            if (lastSystemRefNo == null)
            {
                sysRefNo = systemRefIntitials + "1";
            }
            else
            {
                int refNo = Convert.ToInt32(lastSystemRefNo.Remove(0, 4));
                refNo = refNo + 1;
                sysRefNo = systemRefIntitials + refNo.ToString();
            }

            return sysRefNo;
        }
        public void LoadStatuses()
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") != null)
                allInterBankTransStatus = bankTransRepo.GetAllInterBankTransStatus();
            else
                allInterBankTransStatus = bankTransRepo.GetAllOpenBankTransferStatus();
            allInterBankTransStatus = allInterBankTransStatus.Where(x => x.isDisable != true).ToList();

            if (allInterBankTransStatus != null)
            {
                Parallel.ForEach(allInterBankTransStatus, delegate (InterBankTransferStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {
                    interBankTransStatusLst.Add
                    (new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                });
                cmbxInterBankTransStatus.ItemsSource = interBankTransStatusLst;
            }

        }
        public void LoadStatuses(InterBankTransferStatus _status)
        {
            allInterBankTransStatus.Add(_status);
            if (allInterBankTransStatus != null)
            {
                Parallel.ForEach(allInterBankTransStatus, delegate (InterBankTransferStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {
                    interBankTransStatusLst.Add
                    (new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                });
                cmbxInterBankTransStatus.ItemsSource = interBankTransStatusLst;
            }
        }
        private void LoadBankTransferData()
        {
            loadIndustryTypes();
            loadTaxFlag();
            loadTaxTypes();

            loginUser = UsersRepo.getuserForTenant(MainWindow.currentUserid);

            loadCompanies();
            loadCurrencies();
            loadProcurementProducts();
            loadTransferTypes();
            loadTransferMethods();

            LoadStatuses();


           
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of Inter-Bank Transfer") != null)
            {
                datglPostingdate.IsEnabled = true;
            }
            else
            {
                datglPostingdate.IsEnabled = false;

            }

            if (editFlag == true && bankTransferId != 0)
            {
                bankTransfer = bankTransRepo.GetInterBankTransfer(bankTransferId);
                if(bankTransfer.GLPostingDate!=null)
                {
                    datglPostingdate.EditValue = bankTransfer.GLPostingDate;
                }
                else
                {
                    datglPostingdate.EditValue = bankTransfer.CreationDate;
                        
                }

                if (bankTransfer.isDeposit != null)
                    if (bankTransfer.isDeposit == true)
                        btnDeposit.IsChecked = true;
                    else if (bankTransfer.isDeposit == false)
                        btnPayment.IsChecked = true; 

                if(bankTransfer.isApproved == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Accounting Fields After Approval") == null)
                    {
                        layoutGrpCurrency.IsEnabled = false;
                        layoutGrpTransferFrom.IsEnabled = false;
                        layoutGrpTransferTo.IsEnabled = false;
                    }
                }



                if (bankTransfer.interBankTransStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Inter-Bank Transfer") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") == null)
                    {
                        cmbxInterBankTransStatus.IsEnabled = false;
                    }
                }
                else if (bankTransfer.interBankTransStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inter-Bank Transfer") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                }
                var creditJournalTransactions = bankTransfer.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = bankTransfer.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }
                //if (bankTransfer.journalTransactions != null && bankTransfer.journalTransactions.Count > 0)
                //{
                //    btnPushtoGL.IsChecked = true;
                //}


                if (bankTransfer.FinanceRefNo != null)
                {
                    lblTransferRefNo.Text = " (" + bankTransfer.FinanceRefNo + ")";
                }

                if (bankTransfer.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (bankTransfer.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";
                }
                else if (bankTransfer.isApproved == true && bankTransfer.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bankTransfer.isApproved == true && bankTransfer.interBankTransStatus.isActive == false && bankTransfer.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bankTransfer.isApproved == true && bankTransfer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bankTransfer.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Amount in Inter-Bank Transfer After Approval") != null)
                        txtAmountOC.IsReadOnly = false;
                    else
                        txtAmountOC.IsReadOnly = true;

                }
                else if (bankTransfer.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Amount in Inter-Bank Transfer Under Approval") != null)
                        txtAmountOC.IsReadOnly = false;
                    else
                        txtAmountOC.IsReadOnly = true;

                }
                else if (bankTransfer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }


                

                if (bankTransfer.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                }


                views = UsersRepo.getViwerInfo(bankTransfer.Id, 13);
                grdUsers.ItemsSource = views;
                loadcomments();
                loadonInterBankTransdata();

                datCreationDate.DateTime = (DateTime)bankTransfer.CreationDate;
                datTransactionDate.EditValue = bankTransfer.TransactionDate;
                datInstrumentDate.EditValue = bankTransfer.InstrumentDate;
                txtFinanceRefNo.Text = bankTransfer.FinanceRefNo;
                txtSystemRefNo.Text = bankTransfer.SystemRefNo;
                txtInstrument.Text = bankTransfer.InstrumentNo;
                AmountMerCheck = bankTransfer.AmountMER;
                int index = 0;
                if (bankTransfer.interBankTransStatus != null)
                {
                    var disAbleStatus = allInterBankTransStatus.FirstOrDefault(x => x.Id == bankTransfer.interBankTransStatus.Id);
                    if (disAbleStatus == null)
                    {
                        LoadStatuses(bankTransfer.interBankTransStatus);
                    }
                }
                //Select Status
                if (bankTransfer.interBankTransStatus != null)
                {
                    oldStatus = bankTransfer.interBankTransStatus;
                    foreach (var _status in interBankTransStatusLst)
                    {
                        if (_status.id == bankTransfer.interBankTransStatus.Id)
                        {
                            cmbxInterBankTransStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtDescription.Text = bankTransfer.Description;
                //Select Transfer Type
                if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                {
                    cmbxTransferType.SelectedIndex = 2;
                    txtMerTo.Text = bankTransfer.MERto.ToString();
                    txtAmountMERto.Text = bankTransfer.AmountMERto.ToString();
                    txtAmountOCto.Text = bankTransfer.AmountTo.ToString();

                    txtMerFrom.Text = bankTransfer.MERfrom.ToString();
                    txtAmountMERfrom.Text = bankTransfer.AmountMERfrom.ToString();
                    txtAmountOCfrom.Text = bankTransfer.AmountFrom.ToString();
                    txtER.Text = bankTransfer.AmountER.ToString();
                }
                else
                    for (int i = 0; i <= (int)ERP_BL.Enums.TransferType.Advances; i++)
                    {
                        if (((ERP_BL.Enums.TransferType)i).ToString() == bankTransfer.transferType.ToString())
                        {
                            cmbxTransferType.SelectedIndex = i;
                            break;
                        }
                    }


                //Select Tax Flag
                for (int i = 0; i <= (int)ERP_BL.Enums.TaxFlag.No_Tax; i++)
                {
                    if (((ERP_BL.Enums.TaxFlag)i).ToString() == bankTransfer.taxFlag.ToString())
                    {
                        cmbxTaxFlag.SelectedIndex = i;
                        break;
                    }
                }

                //Select Company
                var companyList = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                if (bankTransfer.company != null)
                {
                    index = 0;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == bankTransfer.company.Id)
                        {
                            cmbxCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Company
                var IBTrefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
                if (bankTransfer.PettyCashRef != null)
                {
                    index = 0;
                    foreach (var _ref in IBTrefList)
                    {
                        if (_ref.id == bankTransfer.PettyCashRef.Id)
                        {
                            cmbxPettyCashRef.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Department
                var deptList = (cmbxDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : cmbxDepartment.ItemsSource as List<Department>;
                if (bankTransfer.department != null)
                {
                    foreach (var _dept in deptList)
                    {
                        if (_dept.Id == bankTransfer.department.Id)
                        {
                            cmbxDepartment.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Employee
                if (bankTransfer.employee != null && cmbxEmpId.ItemsSource != null)
                    cmbxEmpId.Text = bankTransfer.employee.person.FName;

                var item = cmbxEmpId.SelectedItem;


                txtAmountOC.Text = bankTransfer.AmountOC.ToString();

                //Select TransferMethod
                var transferMethodList = (cmbxTransferMethod.ItemsSource as List<TranferMethod>) == null ? new List<TranferMethod>() : cmbxTransferMethod.ItemsSource as List<TranferMethod>;
                if (bankTransfer.tranferMethod != null)
                {
                    foreach (var _method in transferMethodList)
                    {
                        if (_method.Id == bankTransfer.tranferMethod.Id)
                        {
                            cmbxTransferMethod.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Currency
                var currencyList = (cmbxCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : cmbxCurrency.ItemsSource as List<Currency>;
                if (bankTransfer.currency != null)
                {
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.Id == bankTransfer.currency.Id)
                        {
                            cmbxCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtMER.Text = bankTransfer.MER.ToString();
                txtAmountMer.Text = bankTransfer.AmountMER.ToString();

                //Select Bank From
                var bankList = (cmbxBankFrom.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBankFrom.ItemsSource as List<Bank>;
                if (bankTransfer.BankFrom != null)
                {
                    foreach (var _bank in bankList)
                    {
                        if (_bank.Id == bankTransfer.BankFrom.Id)
                        {
                            cmbxBankFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (bankTransfer.transferType == TransferType.Advances)
                {
                    //Select Account To
                    var industryList = (cmbIndustry.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbIndustry.ItemsSource as List<cmbitem>;
                    if (bankTransfer.industryType != null)
                    {
                        index = 0;
                        foreach (var _industry in industryList)
                        {
                            if (_industry.id == bankTransfer.industryType.Id)
                            {
                                cmbIndustry.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    //Select Bank From
                    var vendorList = (cmbxVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : cmbxVendor.ItemsSource as List<Vendor>;
                    if (bankTransfer.vendor != null)
                    {
                        if(vendorList.FirstOrDefault(x=>x.Id == bankTransfer.vendor_Id) == null)
                        {
                            vendorList.Add(bankTransfer.vendor);
                            cmbxVendor.ItemsSource = null;
                            cmbxVendor.ItemsSource = vendorList;
                        }
                        cmbxVendor.Text = bankTransfer.vendor.company.CompanyName;
                    }

                    //Select Account To
                    var accntList = (cmbxAccountTo.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccountTo.ItemsSource as List<Account>;
                    if (bankTransfer.AccountTo != null)
                    {
                        foreach (var _account in accntList)
                        {
                            if (_account.Id == bankTransfer.AccountTo.Id)
                            {
                                cmbxAccountTo.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                }
                else
                {
                    //Select Bank To
                    if (bankTransfer.BankTo != null)
                    {
                        foreach (var _bank in bankList)
                        {
                            if (_bank.Id == bankTransfer.BankTo.Id)
                            {
                                cmbxBankTo.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    //Select Account To
                    var accntList = (cmbxAccountTo.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccountTo.ItemsSource as List<Account>;
                    if (bankTransfer.AccountTo != null)
                    {
                        foreach (var _account in accntList)
                        {
                            if (_account.Id == bankTransfer.AccountTo.Id)
                            {
                                cmbxAccountTo.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                }

                    

                //Select Account From
                var accountList = (cmbxAccountFrom.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccountFrom.ItemsSource as List<Account>;
                if (bankTransfer.AccountFrom != null)
                {
                    index = 0;
                    foreach (var _account in accountList)
                    {
                        if (_account.Id == bankTransfer.AccountFrom.Id)
                        {
                            cmbxAccountFrom.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if(bankTransfer.bankChargesFrom != null)
                    txtBankChargesFrom.Text = bankTransfer.bankChargesFrom.Sum(x=>x.Amount).ToString();
                if (bankTransfer.bankChargesTo != null)
                    txtBankChargesTo.Text = bankTransfer.bankChargesTo.Sum(x => x.Amount).ToString();
                if (bankTransfer.InterBankTransferVATfrom != null)
                    txtVATFrom.Text = bankTransfer.InterBankTransferVATfrom.Sum(x => x.Amount).ToString();
                if (bankTransfer.InterBankTransferVATto != null)
                    txtVATTo.Text = bankTransfer.InterBankTransferVATto.Sum(x => x.Amount).ToString();


                if (bankTransfer.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdPOItems.ItemsSource = offer.products;
                    foreach (var procurementProduct in bankTransfer.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive

                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            //caption1 = cmbcaption1.Text.Trim(),
                            //caption2 = cmbcaption2.Text.Trim(),
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount
                            //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,


                        });


                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdCntrlItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdCntrlItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdCntrlItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdCntrlItems.ItemsSource = procurementProducts;
                }

                if(bankTransfer.transferType == TransferType.Advances)
                {
                    txtAmountPaidOC.Text = bankTransfer.AmountOCPaid.ToString();
                    txtAmountPaidMER.Text = bankTransfer.AmountMERPaid.ToString();

                    if(bankTransfer.taxFlag == TaxFlag.Tax)
                    {
                        var taxTypeList = (cmbxTaxType.ItemsSource as List<TaxType>) == null ? new List<TaxType>() : cmbxTaxType.ItemsSource as List<TaxType>;
                        if (bankTransfer.taxType != null)
                        {
                            index = 0;
                            foreach (var _taxType in taxTypeList)
                            {
                                if (_taxType.Id == bankTransfer.taxType.Id)
                                {
                                    cmbxTaxType.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }
                        }

                        var taxNameList = (cmbxTaxName.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : cmbxTaxName.ItemsSource as List<TaxName>;
                        if (bankTransfer.taxName != null)
                        {
                            index = 0;
                            foreach (var _taxName in taxNameList)
                            {
                                if (_taxName.Id == bankTransfer.taxName.Id)
                                {
                                    cmbxTaxName.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }
                        }

                        txtTaxAmountOC.Text = bankTransfer.TaxAmountOC.ToString();
                        txtTaxAmountMER.Text = bankTransfer.TaxAmountMER.ToString();
                    }
                    

                }
                if (bankTransfer.VATBookRefId != 0 && bankTransfer.VATBookRefNumber != null)
                {
                    btnVATBookPost.IsChecked = true;
                    var vatBookSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                    var term = vatBookSource.Find(x => x.id == bankTransfer.VATBookRefId);

                    if (term == null)
                    {
                        vatBookSource.Add(new cmbitem() { name = bankTransfer.VATBookRefNumber.VATBookReferenceNo, id = bankTransfer.VATBookRefNumber.Id });
                        cmbxVATBookRef.ItemsSource = null;
                        cmbxVATBookRef.ItemsSource = vatBookSource;
                    }
                    cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == bankTransfer.VATBookRefId))];
                }
            }
            else
            {
                datCreationDate.DateTime = DateTime.Now;
                datglPostingdate.DateTime = DateTime.Now;
                btnPushDebits.IsChecked = true;
                btnPushCredits.IsChecked = true;
            }

        }

        private void loadProcurementProducts()
        {
            grdCntrlItems.ItemsSource = procurementProducts;
            //products = SYSTEM_STATIC.GetItemsForCurrentUser();
            products = bankTransRepo.getAllUserProducts(MainWindow.currentUserid);
            lookupProductsinGrid.ItemsSource = products;
        }

        private void loadTransferTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TransferType.IBT_Multiple_Currency; i++)
            {
                if (((ERP_BL.Enums.TransferType)i) != ERP_BL.Enums.TransferType.Inter_Company)
                    cmbxTransferType.Items.Add(((ERP_BL.Enums.TransferType)i).ToString());
            }
        }
        private void loadTransferMethods()
        {
            cmbxTransferMethod.ItemsSource = bankTransRepo.GetAllTransferMethods();
        }

        private void loadCompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                this.cmbxCompany.ItemsSource = cont.GetCompanies();
                return;
            }

            cmbxCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
        }

        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();
            cmbxCurrency.ItemsSource = currencies;
            cmbxCurrencyFrom.ItemsSource = currencies;
            cmbxCurrencyTo.ItemsSource = currencies;
        }
        public void loadTaxTypes()
        {
            TaxRepo taxRepo = new TaxRepo();
            cmbxTaxType.ItemsSource = taxRepo.getAllTaxType();
        }

        public void loadTaxFlag()
        {
            //Populating Combobox Account Type
            for (int i = 0; i <= (int)ERP_BL.Enums.TaxFlag.No_Tax; i++)
            {
                cmbxTaxFlag.Items.Add(((ERP_BL.Enums.TaxFlag)i).ToString());
            }
        }

        public void loadIndustryTypes()
        {


            List<IndustryType> industryTypes = new List<IndustryType>();
            CompanyRepo repo = new CompanyRepo();
            industryTypes = repo.GetVendorIndustryTypes();
            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }

        private void TxtAmountOC_KeyUp(object sender, KeyEventArgs e)
        {
            //var amountOC = Convert.ToDouble(txtAmountOC.Text);
            //var MER = Convert.ToDouble(txtMER.Text);

            //if (MER == 0)
            //{
            //    txtAmountMer.Text = amountOC.ToString();
            //}
            //else
            //{
            //    txtAmountMer.Text = (amountOC * MER).ToString();
            //}
        }

        private void TxtAmountOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amountOC = Convert.ToDouble(txtAmountOC.Text);
            var MER = Convert.ToDouble(txtMER.Text);

            if (MER == 0)
            {
                txtAmountMer.Text = amountOC.ToString();
            }
            else
            {
                txtAmountMer.Text = (amountOC * MER).ToString();
            }



            // Setting "Tranfer From" and "Transfer To" Layout group fields values
            txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
            txtAmountReceivedOC.Text = txtAmountOC.Text;

            double marginExchangeRate = 1;
            if (txtAmountOC.Text != "")
            {
                double totalcfr = Convert.ToDouble(txtAmountOC.Text);
                if (txtMER.Text != "")
                {
                    marginExchangeRate = Convert.ToDouble(txtMER.Text);
                    txtAmountMer.Text = (marginExchangeRate * totalcfr).ToString();

                    // Setting "Tranfer From" and "Transfer To" Layout group fields values
                    txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
                    txtAmountReceivedMER.Text = txtAmountMer.Text;
                }
            }

            taxCalculations();
        }


        private void TxtMER_KeyUp(object sender, KeyEventArgs e)
        {

            if (cmbxTransferType.SelectedIndex == 2)
            {
                var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                var amountMER = Convert.ToDouble(txtMER.Text);
                txtAmountMer.Text = (amountFrom * amountMER).ToString();
                return;
            }

            var amountOC = Convert.ToDouble(txtAmountOC.Text);
            var MER = Convert.ToDouble(txtMER.Text);

            txtAmountMer.Text = (amountOC * MER).ToString();

            if ( ((ERP_BL.Enums.TaxFlag)cmbxTaxFlag.SelectedIndex) == ERP_BL.Enums.TaxFlag.Tax)
            {
                // Setting "Tranfer From" and "Transfer To" Layout group fields values
                txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
                txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
                txtAmountReceivedMER.Text = txtAmountMer.Text;
                txtAmountReceivedOC.Text = txtAmountOC.Text;

                taxCalculations();
            }
            else
            {
                txtAmountPaidMER.Text = txtAmountMer.Text;
                txtAmountPaidOC.Text = txtAmountOC.Text;
            }

        }

        private void CmbxDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void CmbxBankFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void CmbxBankTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void CmbxAccountFrom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxBankFrom.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank first!");
                return;
            }
        }

        private void CmbxAccountTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.IBT_Single_Currency)
            {
                if (cmbxBankTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank first!");
                    return;
                }
            }
            else if ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.Advances)
            {
                if (cmbxVendor.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Vendor first!");
                    return;
                }
            }
        }
        private void CmbxEmpId_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void calculatetotal()
        {
            //double sumfob = 0;
            double sumAmount = 0;
            //decimal? weight = 0;
            //double Quantity = 0;

            if (grdCntrlItems.ItemsSource != null)
                foreach (var item in grdCntrlItems.ItemsSource as List<ProcurementProduct>)
                {

                    {
                        //if (item.inquiryProduct.quantity != 0)
                        //{
                        //    weight += (item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0) * Convert.ToDecimal(item.inquiryProduct.quantity);
                        //}
                        //else
                        //{
                        //    weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;
                        //}

                        //Quantity += item.inquiryProduct.quantity;
                        //sumfob += item.value1;
                        sumAmount += item.value2;
                    }
                }
            //txtAmountOC.Text = sumAmount.ToString();
            //decimal amountOC;
            ////fob = Convert.ToDecimal(txtfob.Text);
            //amountOC = Convert.ToDecimal(txtAmountOC.Text);

            //// Setting "Tranfer From" and "Transfer To" Layout group fields values
            //txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
            //txtAmountReceivedOC.Text = txtAmountOC.Text;

            //decimal marginExchangeRate = 1;
            //if (txtAmountOC.Text != "")
            //{
            //    decimal totalcfr = Convert.ToDecimal(txtAmountOC.Text);
            //    if (txtMER.Text != "")
            //    {
            //        marginExchangeRate = Convert.ToDecimal(txtMER.Text);
            //        txtAmountMer.Text = (marginExchangeRate * totalcfr).ToString();

            //        // Setting "Tranfer From" and "Transfer To" Layout group fields values
            //        txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
            //        txtAmountReceivedMER.Text = txtAmountMer.Text;
            //    }
            //}

            //taxCalculations();
        }


        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> bankTransItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdCntrlItems.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            //product = inquiryProduct;
                            //product.product_Id = inquiryProduct.product.Id;
                            //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            //offerItems.Add(procurementProduct);
                            bankTransItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    //,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,

                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                            bankTransItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    // ,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,

                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                    }
                    //product = procurementProduct;
                    //product.product_Id = procurementProduct.inquiryProduct.product.Id;
                    //product.inquiryProduct.UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                    //offerItems.Add(product);
                }
                else
                {
                    // if user is adding completely new prodcut first time.
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            //product = inquiryProduct;
                            //product.product_Id = inquiryProduct.product.Id;
                            //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            bankTransItems.Add(new ProcurementProduct()
                            {
                                Id = procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    //Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    product = new Product()
                                    {
                                        Id = procurementProduct.inquiryProduct.product.Id,
                                        categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                        item = procurementProduct.inquiryProduct.product.item,
                                        code = procurementProduct.inquiryProduct.product.code,
                                        itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                        ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                        nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                        unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                        //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                        //nature = procurementProduct.inquiryProduct.product.nature,
                                        //category = procurementProduct.inquiryProduct.product.category,
                                        isActive = procurementProduct.inquiryProduct.product.isActive
                                    }
                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        // if user reloaded he offer and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {

                            bankTransItems.Add(new ProcurementProduct()
                            {
                                Id= procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    //product = new Product()
                                    //{
                                    //    Id = procurementProduct.inquiryProduct.product.Id,
                                    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    //    item = procurementProduct.inquiryProduct.product.item,
                                    //    code = procurementProduct.inquiryProduct.product.code,
                                    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    //    //nature = procurementProduct.inquiryProduct.product.nature,
                                    //    //category = procurementProduct.inquiryProduct.product.category,
                                    //    isActive = procurementProduct.inquiryProduct.product.isActive
                                    //}
                                    // ,
                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                //caption1 = cmbcaption1.Text.Trim(),
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                            //offerItems.Add(new ProcurementProduct()
                            //{
                            //    Id = procurementProduct.Id,

                            //    //inquiryProduct = new InquiryProduct()
                            //    //{
                            //    //    Id = procurementProduct.inquiryProduct.Id,
                            //    //    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            //    //    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            //    //    quantity = procurementProduct.inquiryProduct.quantity,

                            //    //    product= procurementProduct.inquiryProduct.product,
                            //    //    //product = new Product()
                            //    //    //{
                            //    //    //    Id = procurementProduct.inquiryProduct.product.Id,
                            //    //    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                            //    //    //    item = procurementProduct.inquiryProduct.product.item,
                            //    //    //    code = procurementProduct.inquiryProduct.product.code,
                            //    //    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                            //    //    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                            //    //    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                            //    //    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                            //    //    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                            //    //    //    //nature = procurementProduct.inquiryProduct.product.nature,
                            //    //    //    //category = procurementProduct.inquiryProduct.product.category,
                            //    //    //    isActive = procurementProduct.inquiryProduct.product.isActive
                            //    //    //},
                            //    //    product_Id = procurementProduct.inquiryProduct.product.Id


                            //    //},
                            //    product_Id = procurementProduct.inquiryProduct.Id,
                            //    value1 = procurementProduct.value1,
                            //    value2 = procurementProduct.value2,
                            //    caption1 = cmbcaption1.Text.Trim(),
                            //    caption2 = cmbcaption2.Text.Trim()


                            //});
                        }
                    }
                }
            }


            return bankTransItems;
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == false)
                    txtSystemRefNo.Text = calculateSystemRefNo();

                //bankTransRepo = new InterBankTransRepo();
                if (cmbxTransferType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Transfer type!");
                    return;
                }
                if (String.IsNullOrEmpty(txtFinanceRefNo.Text))
                {
                    DXMessageBox.Show("Please enter Finance Reference number!");
                    return;
                }

                if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company first!");
                    return;
                }
                if (cmbxDepartment.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Department first!");
                    return;
                }
                if (txtSystemRefNo.Text == null)
                {
                    DXMessageBox.Show("Please enter System Ref # first!");
                    return;
                }
                if (cmbxEmpId.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Employee first!");
                    return;
                }
                if (String.IsNullOrEmpty(txtFinanceRefNo.Text))
                {
                    DXMessageBox.Show("Please enter Finance Ref # first!");
                    return;
                }
                if (cmbxTransferMethod.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Transfer Method first!");
                    return;
                }

                if (editFlag == false && bankTransfer.Id == 0)
                {
                    if (cmbxInterBankTransStatus.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Status first!");
                        return;
                    }
                }

                if ((txtInstrument.Text == null))
                {
                    DXMessageBox.Show("Please enter Instrument # first!");
                    return;
                }



                if (String.IsNullOrEmpty(txtMER.Text) || (Convert.ToDouble(txtMER.Text) == 0))
                {
                    DXMessageBox.Show("Please enter MER first!");
                    return;
                }
                if (cmbxBankFrom.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank first!");
                    return;
                }

                if (cmbxAccountFrom.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account first!");
                    return;
                }

                if ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.Advances)
                {

                    if ((txtAmountOC.Text == null) || (Convert.ToDouble(txtAmountOC.Text) == 0))
                    {
                        DXMessageBox.Show("Please enter Amount(OC) first!");
                        return;
                    }
                    if (cmbxCurrency.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Currency first!");
                        return;
                    }
                    if (String.IsNullOrEmpty(txtAmountPaidOC.Text))
                    {
                        DXMessageBox.Show("Please enter Amount(OC) to Pay!");
                        txtAmountPaidOC.Focus();
                        return;
                    }
                    if (String.IsNullOrEmpty(txtAmountPaidMER.Text))
                    {
                        DXMessageBox.Show("Please enter Amount(MER) to Pay!");
                        txtAmountPaidMER.Focus();
                        return;
                    }
                    if ((ERP_BL.Enums.TaxFlag)cmbxTaxFlag.SelectedIndex == TaxFlag.Tax)
                    {
                        if (cmbxTaxType.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please select Tax Type!");
                            cmbxTaxType.Focus();
                            return;
                        }
                        if (cmbxTaxName.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please select Tax Name!");
                            cmbxTaxName.Focus();
                            return;
                        }
                    }

                    if (cmbIndustry.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Industry!");
                        cmbIndustry.Focus();
                        return;
                    }
                    if (cmbxVendor.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Vendor!");
                        cmbxVendor.Focus();
                        return;
                    }
                }
                else if ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.IBT_Single_Currency)
                {
                    if ((txtAmountOC.Text == null) || (Convert.ToDouble(txtAmountOC.Text) == 0))
                    {
                        DXMessageBox.Show("Please enter Amount(OC) first!");
                        return;
                    }
                    if (cmbxCurrency.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Currency first!");
                        return;
                    }
                    if (cmbxBankTo.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Bank first!");
                        return;
                    }
                }
                else if (cmbxTransferType.SelectedIndex == 2)
                {
                    if (cmbxBankTo.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Bank first!");
                        return;
                    }
                    if (cmbxCurrencyFrom.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Account having Currency Linked!");
                        cmbxAccountFrom.Focus();
                        return;
                    }
                    if (cmbxCurrencyTo.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Account having Currency Linked!");
                        cmbxAccountTo.Focus();
                        return;
                    }
                    if (String.IsNullOrEmpty(txtAmountOCfrom.Text) || Convert.ToDouble(txtAmountOCfrom.Text) == 0)
                    {
                        DXMessageBox.Show("Please Enter Amount from the Bank you want to Transfer Amount!");
                        txtAmountOCfrom.Focus();
                        return;
                    }
                    if (String.IsNullOrEmpty(txtAmountOCto.Text) || Convert.ToDouble(txtAmountOCto.Text) == 0)
                    {
                        DXMessageBox.Show("Please Enter Amount to the Bank you want to Transfer Amount!");
                        txtAmountOCto.Focus();
                        return;
                    }
                }

                if (cmbxAccountTo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account first!");
                    return;
                }

                if(btnDeposit.IsChecked == true || btnPayment.IsChecked == true)
                {
                    if(cmbxPettyCashRef.SelectedIndex <= 0)
                    {
                        DXMessageBox.Show("Please select Petty Cash Reference #!");
                        cmbxPettyCashRef.Focus();
                        return;
                    }
                }
                //else-
                //{
                if (cmbxTransferType.SelectedIndex == 2)
                    bankTransfer.transferType = TransferType.IBT_Multiple_Currency;
                else
                    bankTransfer.transferType = ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex);

                if (editFlag == false)
                {
                    if (paymentGroupId != 0)
                    {
                        bankTransfer.paymentGroupId = paymentGroupId;
                    }
                    if (receiptGroupId != 0)
                    {
                        bankTransfer.receiptGroupId = receiptGroupId;
                    }
                    if (vendorBillId != 0)
                    {
                        bankTransfer.vendorBillId = vendorBillId;
                    }
                    if (adminBillId != 0)
                    {
                        bankTransfer.adminBillId = adminBillId;
                    }
                    if (stlId != 0)
                    {
                        bankTransfer.stlId = stlId;
                    }
                }
                bankTransfer.CreationDate = datCreationDate.DateTime;
                bankTransfer.GLPostingDate = datglPostingdate.DateTime;
                bankTransfer.TransactionDate = datTransactionDate.DateTime;
                bankTransfer.FinanceRefNo = txtFinanceRefNo.Text;
                bankTransfer.SystemRefNo = txtSystemRefNo.Text;
                bankTransfer.transferMethod_Id = (cmbxTransferMethod.SelectedItem as TranferMethod).Id;
                bankTransfer.company_Id = (cmbxCompany.SelectedItem as Company).Id;
                bankTransfer.dept_Id = (cmbxDepartment.SelectedItem as Department).Id;
                bankTransfer.emp_Id = (cmbxEmpId.SelectedItem as ERP_BL.Databases.Employee).EmpId;

                if (((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex) == TransferType.IBT_Single_Currency || ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex) == TransferType.Advances)
                {
                    bankTransfer.AmountOC = Convert.ToDouble(txtAmountOC.Text);
                    bankTransfer.currency_Id = (cmbxCurrency.SelectedItem as Currency).Id;
                }
                else if (cmbxTransferType.SelectedIndex == 2)
                {
                    bankTransfer.currencyFromId = (cmbxCurrencyFrom.SelectedItem as Currency).Id;

                    bankTransfer.AmountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                    bankTransfer.MERfrom = Convert.ToDouble(txtMerFrom.Text);
                    bankTransfer.AmountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);

                    bankTransfer.AmountER = Convert.ToDouble(txtER.Text);

                    bankTransfer.currencyToId = (cmbxCurrencyTo.SelectedItem as Currency).Id;
                    bankTransfer.AmountTo = Convert.ToDouble(txtAmountOCto.Text);
                    bankTransfer.MERto = Convert.ToDouble(txtMerTo.Text);
                    bankTransfer.AmountMERto = Convert.ToDouble(txtAmountMERto.Text);

                }

                bankTransfer.Description = txtDescription.Text;

                bankTransfer.MER = Convert.ToDouble(txtMER.Text);
                bankTransfer.AmountMER = Convert.ToDouble(txtAmountMer.Text);
                bankTransfer.bankFrom_Id = (cmbxBankFrom.SelectedItem as Bank).Id;
                bankTransfer.accountFrom_Id = (cmbxAccountFrom.SelectedItem as Account).Id;

                if (cmbxBankTo.SelectedIndex > -1)
                    bankTransfer.bankTo_Id = (cmbxBankTo.SelectedItem as Bank).Id;

                bankTransfer.accountTo_Id = (cmbxAccountTo.SelectedItem as Account).Id;
                bankTransfer.InstrumentNo = txtInstrument.Text;
                bankTransfer.InstrumentDate = datInstrumentDate.DateTime;
                if (bankTransfer.user_Id == null)
                    bankTransfer.user_Id = MainWindow.currentUserid;

                if (cmbxPettyCashRef.SelectedIndex > -1)
                    bankTransfer.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;

                if (((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex) == TransferType.IBT_Single_Currency || ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex) == TransferType.Advances)
                {
                    bankTransfer.products = getProductsdata();
                    //if (bankTransfer.products.Count == 0)
                    //{
                    //    DXMessageBox.Show("Please Select items against which you want to create a Bank Transfer", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    //lookupDepartment.Focus();
                    //    return;
                    //}
                }


                //Receipt Asset Status 
                if ((cmbxInterBankTransStatus.SelectedItem as cmbitem) != null)
                {
                    var status = bankTransRepo.GetInterBankTransStatus((cmbxInterBankTransStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        bankTransfer.statusId = status.Id;
                    }
                }


                //When transfer type Selected as Advances
                if (bankTransfer.transferType == ERP_BL.Enums.TransferType.Advances)
                {
                    bankTransfer.AmountOCPaid = Convert.ToDouble(txtAmountPaidOC.Text);
                    bankTransfer.AmountMERPaid = Convert.ToDouble(txtAmountPaidMER.Text);

                    bankTransfer.taxFlag = (ERP_BL.Enums.TaxFlag)cmbxTaxFlag.SelectedIndex;
                    if (bankTransfer.taxFlag == ERP_BL.Enums.TaxFlag.Tax)
                    {
                        bankTransfer.taxTypeId = (cmbxTaxType.SelectedItem as TaxType).Id;
                        bankTransfer.taxNameId = (cmbxTaxName.SelectedItem as TaxName).Id;
                        bankTransfer.TaxAmountOC = Convert.ToDouble(txtTaxAmountOC.Text);
                        bankTransfer.TaxAmountMER = Convert.ToDouble(txtTaxAmountMER.Text);
                    }

                    bankTransfer.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                    bankTransfer.vendor_Id = (cmbxVendor.SelectedItem as Vendor).Id;
                }

                if (editFlag == true && bankTransfer.Id != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null && bankTransfer.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            bankTransfer.stage = TransactionStage.Approved.ToString();
                            bankTransfer.isApproved = true;
                            bankTransfer.ApprovedDate = System.DateTime.Now;
                        }
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inter-Bank Transfer") != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null && bankTransfer.isReApproved == false)
                        {
                            if (AmountMerCheck != double.Parse(txtAmountMer.Text))
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Re-Approval State! Do you want to Re-Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    bankTransfer.stage = TransactionStage.Approved.ToString();
                                    bankTransfer.isReApproved = true;
                                    bankTransfer.ReApprovalDate = System.DateTime.Now;
                                }
                            }
                            else
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Re-Approval State! Do you want to Re-Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    bankTransfer.stage = TransactionStage.Approved.ToString();
                                    bankTransfer.isReApproved = true;
                                    bankTransfer.ReApprovalDate = System.DateTime.Now;
                                }
                            }

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null && bankTransfer.isReApproved == null && AmountMerCheck != double.Parse(txtAmountMer.Text))
                        { }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null && bankTransfer.isReApproved == true && AmountMerCheck != double.Parse(txtAmountMer.Text))
                        { }

                        //else if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null && AmountMerCheck != double.Parse(txtAmountMer.Text))
                        //{
                        //    if(bankTransfer.isReApproved != true)
                        //    {
                        //        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Transaction is in Re-Approval State! Do you want to Re-Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        //        {
                        //            bankTransfer.stage = TransactionStage.Approved.ToString();
                        //            bankTransfer.isReApproved = true;
                        //            bankTransfer.ReApprovalDate = System.DateTime.Now;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        bankTransfer.stage = TransactionStage.Approved.ToString();
                        //        bankTransfer.isReApproved = true;
                        //        bankTransfer.ReApprovalDate = System.DateTime.Now;
                        //    }

                        //}

                        else if (AmountMerCheck != double.Parse(txtAmountMer.Text) && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Amount in Inter-Bank Transfer") != null)
                        {
                            bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer.isReApproved = false;
                        }
                        if (bankTransfer.journalTransactions.Count != 0)
                        {

                            JournalEntryRepo repo = new JournalEntryRepo();
                            List<JournalTransaction> transactionList = new List<JournalTransaction>();
                            if (bankTransfer.transferType == TransferType.Advances)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                       bankTransfer.AccountFrom.COA_accountId &&
                                       x.credit == Convert.ToDouble(txtAmountPaidOC.Text) &&
                                       x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                       x.deptId == department.Id
                                        );
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountFrom.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountPaidOC.Text),
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(txtAmountPaidOC.Text),
                                            deptId = department.Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationDate = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null
                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountFrom.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountPaidOC.Text),
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(txtAmountPaidOC.Text),
                                            deptId = department.Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                      bankTransfer.AccountTo.COA_accountId &&
                                      x.debit == Convert.ToDouble(txtAmountReceivedOC.Text) &&
                                      x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                      x.deptId == department.Id
                                       );
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountTo.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            debit = Convert.ToDouble(txtAmountReceivedOC.Text),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = Convert.ToDouble(txtAmountReceivedOC.Text) - 0,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationDate = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountTo.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            debit = Convert.ToDouble(txtAmountReceivedOC.Text),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = Convert.ToDouble(txtAmountReceivedOC.Text) - 0,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                                if (cmbxTaxName.SelectedIndex != -1)
                                {
                                    TaxRepo taxRepo = new TaxRepo();
                                    var tax = taxRepo.getTaxtById((cmbxTaxName.SelectedItem as TaxName).Id);
                                    if (tax.chartofAccount != null && !string.IsNullOrEmpty(txtFinanceRefNo.Text))
                                    {
                                        if (btnPushCredits.IsChecked == true)
                                        {
                                            var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 tax.chartofAccount.Id &&
                                                 x.credit == Convert.ToDouble(txtTaxAmountOC.Text) &&
                                                 x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                 x.deptId == department.Id
                                                  );
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction transactionTax = new JournalTransaction()
                                                {
                                                    accountId = tax.chartofAccount.Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = Convert.ToDouble(txtTaxAmountOC.Text),
                                                    debit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    deptId = department.Id,
                                                    taxFlag = true,
                                                    total = 0 - Convert.ToDouble(txtTaxAmountOC.Text),
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                                    reconcilationDate = null,
                                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                                    isReconciled = false,
                                                    ReconcilationId = null
                                                };
                                                transactionList.Add(transactionTax);
                                            }
                                            else
                                            {
                                                JournalTransaction transactionTax = new JournalTransaction()
                                                {
                                                    accountId = tax.chartofAccount.Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = Convert.ToDouble(txtTaxAmountOC.Text),
                                                    debit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    deptId = department.Id,
                                                    taxFlag = true,
                                                    total = 0 - Convert.ToDouble(txtTaxAmountOC.Text),
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                                };
                                                transactionList.Add(transactionTax);
                                            }
                                        }
                                    }

                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = charges.deduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }


                                bankTransfer.journalTransactions = transactionList;
                            }
                            else
                            if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                               bankTransfer.AccountFrom.COA_accountId &&
                                               x.credit == Convert.ToDouble(txtAmountOC.Text) &&
                                               x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                               x.deptId == department.Id
                                                );
                                    if (dbTransaction != null && dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountFrom.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountOC.Text),
                                            debit = 0,
                                            memo = "Inter Bank transafer",
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationDate = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null

                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountFrom.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountOC.Text),
                                            debit = 0,
                                            memo = "Inter Bank transafer",
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                               bankTransfer.AccountTo.COA_accountId &&
                                               x.debit == Convert.ToDouble(txtAmountOC.Text) &&
                                               x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                               x.deptId == department.Id
                                                );
                                    if (dbTransaction != null && dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountTo.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            debit = Convert.ToDouble(txtAmountOC.Text),
                                            memo = "Inter Bank transafer",
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null,
                                            reconcilationDate = null

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = bankTransfer.AccountTo.COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            debit = Convert.ToDouble(txtAmountOC.Text),
                                            memo = "Inter Bank transafer",
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            deptId = department.Id,
                                            total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);
                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }

                                bankTransfer.journalTransactions = transactionList;
                            }
                            else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                               (cmbxAccountFrom.SelectedItem as Account).COA_accountId &&
                                               x.credit == Convert.ToDouble(txtAmountOCfrom.Text) &&
                                               x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                               x.deptId == department.Id
                                                );
                                    if (dbTransaction != null && dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null,
                                            reconcilationDate = null


                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionCredit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionCredit);
                                    }
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    var dbTransaction = bankTransfer.journalTransactions.FirstOrDefault(x => x.accountId ==
                                               (cmbxAccountTo.SelectedItem as Account).COA_accountId &&
                                               x.debit == Convert.ToDouble(txtAmountOCto.Text) &&
                                               x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                               x.deptId == department.Id
                                                );
                                    if (dbTransaction != null && dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = Convert.ToDouble(txtAmountOCto.Text),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions,
                                            isReconciled = false,
                                            ReconcilationId = null,
                                            reconcilationDate = null


                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                    else
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = Convert.ToDouble(txtAmountOCto.Text),
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }

                                bankTransfer.journalTransactions = transactionList;
                            }
                           
                        }
                        else
                        {
                            List<JournalTransaction> transactionList = new List<JournalTransaction>();
                            if (bankTransfer.transferType == TransferType.Advances)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction transactionCredit = new JournalTransaction()
                                    {
                                        accountId = bankTransfer.AccountFrom.COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = Convert.ToDouble(txtAmountPaidOC.Text),
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        memo = txtFinanceRefNo.Text,
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        total = 0 - Convert.ToDouble(txtAmountPaidOC.Text),
                                        deptId = department.Id,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                    };
                                    transactionList.Add(transactionCredit);
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    JournalTransaction transactionDebit = new JournalTransaction()
                                    {
                                        accountId = bankTransfer.AccountTo.COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = 0,
                                        debit = Convert.ToDouble(txtAmountReceivedOC.Text),
                                        memo = txtFinanceRefNo.Text,
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        deptId = department.Id,
                                        total = Convert.ToDouble(txtAmountReceivedOC.Text) - 0,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                    };
                                    transactionList.Add(transactionDebit);
                                }
                                if (cmbxTaxName.SelectedIndex != -1)
                                {
                                    TaxRepo taxRepo = new TaxRepo();
                                    var tax = taxRepo.getTaxtById((cmbxTaxName.SelectedItem as TaxName).Id);
                                    if (tax.chartofAccount != null && !string.IsNullOrEmpty(txtFinanceRefNo.Text))
                                    {
                                        if (btnPushCredits.IsChecked == true)
                                        {
                                            JournalTransaction transactionTax = new JournalTransaction()
                                            {
                                                accountId = tax.chartofAccount.Id,
                                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                creationDate = bankTransfer.GLPostingDate,
                                                credit = Convert.ToDouble(txtTaxAmountOC.Text),
                                                debit = 0,
                                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                memo = txtFinanceRefNo.Text,
                                                transactionRefno = txtSystemRefNo.Text,
                                                userId = SYSTEM_STATIC.currentUser.id,
                                                deptId = department.Id,
                                                taxFlag = true,
                                                total = 0 - Convert.ToDouble(txtTaxAmountOC.Text),
                                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                            };
                                            transactionList.Add(transactionTax);
                                        }
                                    }

                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }

                                bankTransfer.journalTransactions = transactionList;
                            }
                            else
                            if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction transactionCredit = new JournalTransaction()
                                    {
                                        accountId = bankTransfer.AccountFrom.COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = Convert.ToDouble(txtAmountOC.Text),
                                        debit = 0,
                                        memo = "Inter Bank transafer",
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        deptId = department.Id,
                                        total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                    };
                                    transactionList.Add(transactionCredit);
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    JournalTransaction transactionDebit = new JournalTransaction()
                                    {
                                        accountId = bankTransfer.AccountTo.COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = 0,
                                        debit = Convert.ToDouble(txtAmountOC.Text),
                                        memo = "Inter Bank transafer",
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        deptId = department.Id,
                                        total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                    };
                                    transactionList.Add(transactionDebit);
                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }

                                bankTransfer.journalTransactions = transactionList;
                            }
                            else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction transactionCredit = new JournalTransaction()
                                    {
                                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                                        memo = txtFinanceRefNo.Text,
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id
                                    };
                                    transactionList.Add(transactionCredit);
                                }
                                if (btnPushDebits.IsChecked == true)
                                {
                                    JournalTransaction transactionDebit = new JournalTransaction()
                                    {
                                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                        debit = Convert.ToDouble(txtAmountOCto.Text),
                                        memo = txtFinanceRefNo.Text,
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                    };
                                    transactionList.Add(transactionDebit);
                                }
                                if (bankTransfer.bankChargesFrom != null)
                                {
                                    if (bankTransfer.bankChargesFrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesFrom)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.bankChargesTo != null)
                                {
                                    if (bankTransfer.bankChargesTo.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.bankChargesTo)
                                        {
                                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.chartofAccountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATfrom != null)
                                {
                                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }
                                if (bankTransfer.InterBankTransferVATto != null)
                                {
                                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                                    {
                                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                                        {
                                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                            if (btnPushDebits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebitVat = new JournalTransaction()
                                                {
                                                    accountId = dbDuduction.COA_Id,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = charges.Amount,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = Convert.ToDouble(charges.Amount) - 0,
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebitVat);

                                            }
                                            if (btnPushCredits.IsChecked == true)
                                            {
                                                JournalTransaction transactionDebit = new JournalTransaction()
                                                {
                                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                                    creationDate = bankTransfer.GLPostingDate,
                                                    credit = charges.Amount,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    debit = 0,
                                                    memo = txtFinanceRefNo.Text,
                                                    transactionRefno = txtSystemRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    total = 0 - Convert.ToDouble(charges.Amount),
                                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                };
                                                transactionList.Add(transactionDebit);
                                            }
                                        }
                                    }
                                }

                                bankTransfer.journalTransactions = transactionList;
                            }
                        }
                        List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
                        if (editFlag == true )
                        {
                            if (cmbxVATBookRef.SelectedIndex > -1)
                            {
                                bankTransfer.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                                if (btnVATBookPost.IsChecked == true)
                                {
                                    int currencyFrom = 0;
                                    int currencyTo = 0;
                                    if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                                    {
                                        currencyFrom = (cmbxCurrency.SelectedItem as Currency).Id;
                                        currencyTo = (cmbxCurrency.SelectedItem as Currency).Id;

                                    }
                                    else
                                    if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                                    {
                                        currencyFrom = (cmbxCurrencyFrom.SelectedItem as cmbitem).id;
                                        currencyTo = (cmbxCurrencyFrom.SelectedItem as cmbitem).id;
                                    }

                                    foreach (var tax in bankTransfer.InterBankTransferVATfrom)
                                    {
                                        vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                        {
                                            CreationDate = datCreationDate.DateTime,
                                            GLPostingDate = datCreationDate.DateTime,
                                            interBankTransferId = bankTransfer.Id,
                                            TransactionType = TransactionItemType.InterBank_Transfer,
                                            debit = tax.Amount,
                                            credit = 0,
                                            total = tax.Amount - 0,
                                            FinanceRefNo = txtFinanceRefNo.Text,
                                            SystemRefNo = txtSystemRefNo.Text,
                                            MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = currencyFrom,
                                            VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                        });
                                    }
                                    foreach (var tax in bankTransfer.InterBankTransferVATto)
                                    {
                                        vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                        {
                                            CreationDate = datCreationDate.DateTime,
                                            GLPostingDate = datCreationDate.DateTime,
                                            paymentId = bankTransfer.Id,
                                            TransactionType = TransactionItemType.InterBank_Transfer,
                                            debit = tax.Amount,
                                            credit = 0,
                                            total = tax.Amount - 0,
                                            FinanceRefNo = txtFinanceRefNo.Text,
                                            SystemRefNo = txtSystemRefNo.Text,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = currencyTo,
                                            VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                        });
                                    }
                                    bankTransfer.VATBooks = vatBooks;
                                }
                            }
                        }
                        if ( bankTransfer.transferType == TransferType.IBT_Single_Currency)
                            bankTransfer.pettyCashes = getPettyCash( SYSTEM_STATIC.currentUser.id, Convert.ToDouble(txtAmountOC.Text));
                        else if(bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                                bankTransfer.pettyCashes = getPettyCash(SYSTEM_STATIC.currentUser.id, Convert.ToDouble(txtAmountOCto.Text));

                        //}
                        //else
                        //{
                        //    bankTransfer.journalTransactions.Clear();

                        //}

                        //Comment to show in commit
                        bankTransRepo.updateInterBankTransfer(bankTransfer);

                        if (oldStatus != null)
                        {
                            if (oldStatus.Id != bankTransfer.interBankTransStatus.Id)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status of Transaction has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (bankTransfer.department != null && bankTransfer.department.Id != 0 && bankTransfer.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.department.Id, bankTransfer.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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
                                string oldStat = oldStatus.Status;
                                string newStat = bankTransfer.interBankTransStatus.Status;
                                string symbolCurr = "";

                                if (bankTransfer.currency != null)
                                {
                                    symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of InterBank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ",null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0, user.id, "New Comment ",null);
                                    }
                                }
                            }
                        }


                        DXMessageBox.Show("Updated Successfully!");
                        frmBankTranfer.Close();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Edit Inter-Bank Transfer!");
                        return;
                    }
                }
                else if (editFlag == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null)
                    {
                        bankTransfer.stage = TransactionStage.Approved.ToString();
                        bankTransfer.isApproved = true;
                        bankTransfer.ApprovedDate = System.DateTime.Now;
                    }
                    else
                    {
                        bankTransfer.stage = TransactionStage.AwaitingFirstReview.ToString();
                        bankTransfer.isApproved = false;
                    }
                        List<JournalTransaction> transactionList = new List<JournalTransaction>();
                    if (bankTransfer.transferType == TransferType.Advances)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            JournalTransaction transactionCredit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = Convert.ToDouble(txtAmountPaidOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                deptId = department.Id,
                                total = 0 - Convert.ToDouble(txtAmountPaidOC.Text),
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                            };
                            transactionList.Add(transactionCredit);
                        }
                        if (bankTransfer.taxNameId != null)
                        {
                            TaxRepo taxRepo = new TaxRepo();
                            var tax = taxRepo.getTaxtById((int)bankTransfer.taxNameId);
                            if (tax.chartofAccount != null && !string.IsNullOrEmpty(txtFinanceRefNo.Text))
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction transactionTax = new JournalTransaction()
                                    {
                                        accountId = tax.chartofAccount.Id,
                                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                        creationDate = bankTransfer.GLPostingDate,
                                        credit = Convert.ToDouble(txtTaxAmountOC.Text),
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        memo = txtFinanceRefNo.Text,
                                        transactionRefno = txtSystemRefNo.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        deptId = department.Id,
                                        taxFlag = true,
                                        total = 0 - Convert.ToDouble(txtTaxAmountOC.Text),
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                    };
                                    transactionList.Add(transactionTax);
                                }

                            }
                        }
                        if (btnPushDebits.IsChecked == true)
                        {
                            JournalTransaction transactionDebit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                debit = Convert.ToDouble(txtAmountReceivedOC.Text),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                total = Convert.ToDouble(txtAmountReceivedOC.Text) - 0,
                                deptId = department.Id,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionDebit);
                        }
                        if (bankTransfer.bankChargesFrom != null)
                        {
                            if (bankTransfer.bankChargesFrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesFrom)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.bankChargesTo != null)
                        {
                            if (bankTransfer.bankChargesTo.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesTo)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATfrom != null)
                        {
                            if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATto != null)
                        {
                            if (bankTransfer.InterBankTransferVATto.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATto)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }

                        bankTransfer.journalTransactions = transactionList;
                    }
                    else if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            JournalTransaction transactionCredit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = Convert.ToDouble(txtAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                total = 0 - Convert.ToDouble(txtAmountOC.Text),
                                deptId = department.Id,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionCredit);
                        }
                        if (btnPushDebits.IsChecked == true)
                        {
                            JournalTransaction transactionDebit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                debit = Convert.ToDouble(txtAmountOC.Text),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                total = Convert.ToDouble(txtAmountOC.Text) - 0,
                                deptId = department.Id,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionDebit);
                        }
                        if (bankTransfer.bankChargesFrom!=null)
                        {
                            if (bankTransfer.bankChargesFrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesFrom)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.bankChargesTo != null)
                        {
                            if (bankTransfer.bankChargesTo.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesTo)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATfrom != null)
                        {
                            if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATto != null)
                        {
                            if (bankTransfer.InterBankTransferVATto.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATto)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }

                        bankTransfer.journalTransactions = transactionList;
                    }
                    else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            JournalTransaction transactionCredit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = Convert.ToDouble(txtAmountOCfrom.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                                deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionCredit);
                        }
                        if (btnPushDebits.IsChecked == true)
                        {
                            JournalTransaction transactionDebit = new JournalTransaction()
                            {
                                accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                debit = Convert.ToDouble(txtAmountOCto.Text),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                                deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionDebit);

                        }

                        if (bankTransfer.bankChargesFrom != null)
                        {
                            if (bankTransfer.bankChargesFrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesFrom)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.bankChargesTo != null)
                        {
                            if (bankTransfer.bankChargesTo.Count > 0)
                            {
                                foreach (var charges in bankTransfer.bankChargesTo)
                                {
                                    var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATfrom != null)
                        {
                            if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }
                        if (bankTransfer.InterBankTransferVATto != null)
                        {
                            if (bankTransfer.InterBankTransferVATto.Count > 0)
                            {
                                foreach (var charges in bankTransfer.InterBankTransferVATto)
                                {
                                    var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebitVat = new JournalTransaction()
                                        {
                                            accountId = dbDuduction.COA_Id,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = charges.Amount,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = Convert.ToDouble(charges.Amount) - 0,
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebitVat);

                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction transactionDebit = new JournalTransaction()
                                        {
                                            accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                            coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                            creationDate = bankTransfer.GLPostingDate,
                                            credit = charges.Amount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            debit = 0,
                                            memo = txtFinanceRefNo.Text,
                                            transactionRefno = txtSystemRefNo.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            total = 0 - Convert.ToDouble(charges.Amount),
                                            deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                        };
                                        transactionList.Add(transactionDebit);
                                    }
                                }
                            }
                        }

                        bankTransfer.journalTransactions = transactionList;
                    }
                    if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                        bankTransfer.pettyCashes = getPettyCash(SYSTEM_STATIC.currentUser.id, Convert.ToDouble(txtAmountOC.Text));
                    if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                        bankTransfer.pettyCashes = getPettyCash(SYSTEM_STATIC.currentUser.id, Convert.ToDouble(txtAmountOCto.Text));


                    bankTransRepo.addInterBankTransfer(bankTransfer);
                    DXMessageBox.Show("Added Successfully!");
                    frmBankTranfer.Close();
                }


                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        public List<PettyCash> getPettyCash(int userId, double AmountOC)
        {
            List<PettyCash> pettyCashes = new List<PettyCash>();
            //ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

            //var debitChartofAccount = coaRepo.get(chartofAccountDebitId);
            //var creditChartofAccount = coaRepo.get(chartofAccountCreditId);
            double Amount = 0;
            if (AmountOC < 0)
            {
                Amount = AmountOC * (-1);
            }
            else
                Amount = AmountOC;
            if (bankTransferId != 0)
            {
                int currencyId = 0;
                double MER = 1;
                if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                {
                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                }
                else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                {
                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id;
                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                }

                if (btnDeposit.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        interBankTransferId = bankTransferId,
                        TransactionType = TransactionItemType.InterBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = MER,
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = currencyId,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDeposit = true;
                }
                else if(btnPayment.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        interBankTransferId = bankTransferId,
                        TransactionType = TransactionItemType.InterBank_Transfer,
                        debit = 0,
                        credit = Amount,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = MER,
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = currencyId,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDeposit = false;
                }
                else
                {
                    bankTransfer.isDeposit = null;
                    bankTransfer.isPettyCashAmountOC = null;
                }
            }
            else
            {
                int currencyId = 0;
                double MER = 1;
                if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
                {
                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                }
                else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
                {
                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id;
                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                }

                if (btnDeposit.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        interBankTransferId = bankTransferId,
                        TransactionType = TransactionItemType.InterBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = Amount - 0,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = MER,
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = currencyId,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDeposit = true;
                }
                else if (btnPayment.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = bankTransfer.CreationDate.Value,
                        interBankTransferId = bankTransferId,
                        TransactionType = TransactionItemType.InterBank_Transfer,
                        debit = Amount,
                        credit = 0,
                        total = 0 - Amount,
                        FinanceRefNo = txtFinanceRefNo.Text,
                        SystemRefNo = txtSystemRefNo.Text,
                        MER = MER,
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = currencyId,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    bankTransfer.isDeposit = false;
                }
                else
                {
                    bankTransfer.isDeposit = null;
                    bankTransfer.isPettyCashAmountOC = null;
                }
            }
            return pettyCashes;
        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            calculatetotal();
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
            frmItemadd.ShowDialog();
            //products = productrepo.getAll();
            //products = SYSTEM_STATIC.GetItemsForCurrentUser();
            products = bankTransRepo.getAllUserProducts(MainWindow.currentUserid);
            //lookupProductinGrid.ItemsSource = products;
            lookupProductsinGrid.ItemsSource = products;
        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //Product pro = (Product)lookupProductsinGrid.GetItemFromValue(grdPOItems.GetCellValue(e.RowHandle, "inquiryProduct.product"));
            (grdCntrlItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            //(grdPOItems.CurrentItem as ProcurementProduct).inquiryProduct.product =  as Product;
            //grdPOItems.SetCellValue(e.RowHandle, "inquiryProduct.product.itemDescription", pro.itemDescription);

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (bankTransfer.Id > 0)
            {
                if (bankTransfer.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inter-Bank Transfer") != null))
                {
                    if (DXMessageBox.Show("This Transaction is currently in the list of Void Inter-Bank Transfers! Do you want to remove it from Void?", "Remove Void Inter-Bank Transfers", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        bankTransfer.isVoid = false;
                        bankTransRepo.setInterBankTransfertoVoid(bankTransfer.Id, false);
                        grdVoid.Visibility = Visibility.Collapsed;

                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("Inter-Bank Transfer has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (bankTransfer.department != null && bankTransfer.department.Id != 0 && bankTransfer.company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.department.Id, bankTransfer.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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
                        if (bankTransfer.currency != null)
                        {
                            symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Inter-Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "IBT UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inter-Bank Transfer") != null)
                {
                    if (DXMessageBox.Show("This Transaction is not currently in the list of Void Inter-Bank Transfers! Do you want to move it to Void Inter-Bank Transfers?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        bankTransfer.isVoid = true;
                        bankTransRepo.setInterBankTransfertoVoid(bankTransfer.Id, true);
                        grdVoid.Visibility = Visibility.Visible;
                        txtVoid.RenderTransform = new RotateTransform(-45);

                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("Inter-Bank Transfer has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (bankTransfer.department != null && bankTransfer.department.Id != 0 && bankTransfer.company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.department.Id, bankTransfer.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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
                        if (bankTransfer.currency != null)
                        {
                            symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Inter-Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                            Timestamp = DateTime.Now,
                            Subject = "IBT Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Permission Required to Mark or UnMark a Transaction to Void!");
                }
            }
            loadcomments();
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (bankTransfer.isApproved == false)
            {
                
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null)
                {
                    //bankTransRepo = new InterBankTransRepo();
                    bankTransfer = bankTransRepo.GetInterBankTransfer(bankTransfer.Id);

                    bankTransfer.isReviewed = true;
                    bankTransfer.isApproved = true;
                    bankTransfer.stage = TransactionStage.Approved.ToString();
                    bankTransRepo.approveInterBankTransfer(bankTransfer);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inter-Bank Transfer has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bankTransfer.department != null && bankTransfer.department.Id != 0 && bankTransfer.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.department.Id, bankTransfer.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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


                        //if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        //{
                        //    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(purchaseOrder.department.Id, purchaseOrder.company.Id));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //}
                        //else
                        //{
                        //    winTagUsers win = new winTagUsers();
                        //    win.ShowDialog();
                        //}

                    }

                    string symbolCurr = "";
                    if (bankTransfer.currency != null)
                    {
                        symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Inter-Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "IBT Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.FinanceRefNo, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                else
                {
                    DXMessageBox.Show("You need permission to Approve IBT!");
                }
            }
            else if (bankTransfer.isApproved == true)
            {
                //bankTransRepo = new InterBankTransRepo();
                bankTransfer = bankTransRepo.GetInterBankTransfer(bankTransfer.Id);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null)
                {
                    bankTransfer.isReviewed = true;
                    bankTransfer.isApproved = false;
                    bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                    bankTransRepo.approveInterBankTransfer(bankTransfer);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inter-Bank Transfer has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bankTransfer.department != null && bankTransfer.department.Id != 0 && bankTransfer.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(bankTransfer.department.Id, bankTransfer.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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


                        //if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        //{
                        //    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(purchaseOrder.department.Id, purchaseOrder.company.Id));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //}
                        //else
                        //{
                        //    winTagUsers win = new winTagUsers();
                        //    win.ShowDialog();
                        //}

                    }

                    string symbolCurr = "";
                    if (bankTransfer.currency != null)
                    {
                        symbolCurr = bankTransfer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Inter-Bank Transfer (Amount OC) having value: " + bankTransfer.AmountOC.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "IBT UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.AmountOC, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in IBT #" + bankTransfer.AmountOC, bankTransfer.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
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


        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)
            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {
                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    e.Value = fname + " " + lname;
                }
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
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



        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (bankTransfer.Id != 0)
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
                        destination += "Attachments\\InterBank_Transfer\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += bankTransfer.Id + "_" + TransactionItemType.InterBank_Transfer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);
                           
                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                            {
                                ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                var result = attachment.startUploading(TransactionItemType.InterBank_Transfer);
                                if (result.Item1)
                                {
                                    AttachmentsRepo repo = new AttachmentsRepo();
                                    //Attachment attachmen= new Attachment();
                                    repo.Add(System.IO.Path.GetFileName(result.Item2), bankTransfer.Id, TransactionItemType.InterBank_Transfer, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                    UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bankTransfer.Id, 13, "Added a New attachment");

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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
                            //MessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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


        //private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        //{
        //    string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
        //    string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
        //    if (cmbCategory.SelectedItem != null)
        //    {
        //        if (bankTransfer.Id != 0)
        //        {
        //            try
        //            {

        //                int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
        //                OpenFileDialog fileDialog = new OpenFileDialog();
        //                fileDialog.Multiselect = false;
        //                string sourceFile = @"";
        //                string exePath = System.Environment.GetCommandLineArgs()[0];
        //                string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
        //                destination += "Attachments\\InterBank_Transfer\\ToUpload\\";
        //                //string destination = @"D:\MovedFiles\new\";
        //                System.IO.Directory.CreateDirectory(destination);
        //                if (fileDialog.ShowDialog() == true) // Test result.
        //                {
        //                    imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
        //                    btnAttachNew.ToolTip = "Uploading";
        //                    btnAttachNew.IsEnabled = true;

        //                    btnAttachment.Content = "Uploading File . . .";
        //                    sourceFile = fileDialog.FileName;
        //                    destination += bankTransfer.Id + "_" + TransactionItemType.InterBankTransfer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
        //                    System.IO.File.Move(sourceFile, destination);

        //                    System.Threading.Thread thread = new System.Threading.Thread(() =>
        //                    {
        //                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
        //                        var result = attachment.startUploading(TransactionItemType.InterBankTransfer);
        //                        if (result.Item1)
        //                        {
        //                            AttachmentsRepo repo = new AttachmentsRepo();
        //                            //Attachment attachmen= new Attachment();
        //                            repo.Add(System.IO.Path.GetFileName(result.Item2), bankTransfer.Id, TransactionItemType.InterBankTransfer, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
        //                            _usersRepo.Add(TransactionInfo.Attachment_Uploaded, bankTransfer.Id, 13, "Added a New attachment");

        //                            this.Dispatcher.Invoke(() =>
        //                            {
        //                                treeViewAttachments.ItemsSource = SystemLogic.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterBankTransfer);
        //                                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

        //                                btnAttachNew.ToolTip = "Attach";
        //                                btnAttachNew.IsEnabled = true;
        //                                btnAttachment.Content = "Select";
        //                            });



        //                        }
        //                    });
        //                    thread.Start();


        //                    //MessageBox.Show("Attachment Uploaded");


        //                }


        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.ToString());
        //                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

        //                btnAttachNew.ToolTip = "Attach";
        //                btnAttachNew.IsEnabled = true;
        //            }
        //            finally
        //            {



        //            }
        //        }
        //        else
        //            return;
        //    }
        //    else
        //    {
        //        DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
        //    }

        //}


        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sSelectedPath = "";

                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    // OpenFileDialog openFileDialog = new OpenFileDialog();
                    //var a = openFileDialog.ShowDialog();
                    //var len = str.Length;
                    //len = len - 26;
                    //var str1 = str.Substring(26,len);

                    //System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    //fbd.Description = "Custom Description";

                    //if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //{
                    //     sSelectedPath = fbd.SelectedPath;
                    //}
                    //sSelectedPath = sSelectedPath;


                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.InterBank_Transfer);


                        // var res =  Tuple.Create( true, sSelectedPath);

                        //result = sSelectedPath;
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
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        public void loadonInterBankTransdata()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterBank_Transfer);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
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
                if (bankTransfer.Id > 0)
                {
                    if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartment(department.Id), comment, TransactionItemType.InterBank_Transfer);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (bankTransfer.Id > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.InterBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                       
                        else if (bankTransfer.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Inter-Bank Transfer first to add a comment!");
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
                if (department != null && department.Id != 0 && cmbxCompany.SelectedIndex > -1)
                {
                    var company = cmbxCompany.SelectedItem as Company;
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.InterBank_Transfer);
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

                if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.InterBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bankTransfer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

            if (department != null && department.Id != 0 && cmbxCompany.SelectedIndex > -1)
            {
                var company = cmbxCompany.SelectedItem as Company;
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.InterBank_Transfer);
                inputBox.ShowDialog();

            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (bankTransfer.Id != 0)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && bankTransfer.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inter-Bank Transfer #" + txtSystemRefNo.Text, bankTransfer.Id, TransactionItemType.InterBank_Transfer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }
                    }

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bankTransfer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                }

            }
        }

        public void loadcomments()
        {
            try
            {
                if (bankTransfer.Id != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(bankTransfer.Id, TransactionItemType.InterBank_Transfer);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (bankTransfer.Id != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, bankTransfer.Id, 13, "Viewed details of Inter-Bank Transfer");
            }
            //SystemLogic.SaveUserSettingForCurrentWindow(grdCntrlSalesReceipt);
        }

        private void CmbxAccountFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxAccountFrom.SelectedItem != null)
            {
                var bankAccount = cmbxAccountFrom.SelectedItem as Account;
                var GlDate = datglPostingdate.DateTime;

                layoutGrpTransferFrom.Header = "Transfer From" + " [" +bankAccount.AccountNick + "]";

                if (cmbxTransferType.SelectedIndex == 2)
                {
                    cmbxCurrencyFrom.Text = bankAccount.currency.CurrencyName;

                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        &&
                        x.creationDate <= GlDate.Date
                        ).Sum(x => x.total);
                        
                        var amountOC = Convert.ToDouble(txtAmountOCfrom.Text);
                        if (editFlag == true)
                        {
                            //if(bankTransfer.isApproved == false)
                            //{
                            accntBalance = accntBalance + bankTransfer.AmountOC;
                            txtExistingBalanceFrom.Text = accntBalance.ToString();
                            txtAfterTransferFrom.Text = (accntBalance - bankTransfer.AmountOC).ToString();
                            //}
                            //else
                            //{
                            //    txtExistingBalanceFrom.Text = accntBalance.ToString();
                            //    txtAfterTransferFrom.Text = accntBalance.ToString();
                            //}
                        }
                        else
                        {
                            txtExistingBalanceFrom.Text = accntBalance.ToString();
                            txtAfterTransferFrom.Text = (accntBalance - amountOC).ToString();
                        }
                    }
                }
                else
                {
                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        &&
                        x.creationDate <= GlDate
                        ).Sum(x => x.total);

                        var amountOC = Convert.ToDouble(txtAmountOC.Text);
                        if (editFlag == true)
                        {
                            //if (bankTransfer.isApproved == false)
                            //{
                            accntBalance = accntBalance + bankTransfer.AmountOC;
                            txtExistingBalanceFrom.Text = accntBalance.ToString();
                            txtAfterTransferFrom.Text = (accntBalance - bankTransfer.AmountOC).ToString();
                            //}
                            //else
                            //{
                            //    txtExistingBalanceFrom.Text = accntBalance.ToString();
                            //    txtAfterTransferFrom.Text = accntBalance.ToString();
                            //}
                        }
                        else
                        {
                            txtExistingBalanceFrom.Text = accntBalance.ToString();
                            txtAfterTransferFrom.Text = (accntBalance - amountOC).ToString();
                        }
                        
                    }
                }
            }
        }

        private void CmbxAccountTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxAccountTo.SelectedItem != null)
            {
                var bankAccount = cmbxAccountTo.SelectedItem as Account;
                var GlDate = datglPostingdate.DateTime;

                layoutGrpTransferTo.Header = "Transfer To" + " [" + bankAccount.AccountNick + "]";
                if (cmbxTransferType.SelectedIndex == 2)
                {
                    cmbxCurrencyTo.Text = bankAccount.currency.CurrencyName;

                    if(bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        &&
                        x.creationDate <= GlDate
                        ).Sum(x => x.total);
                        var amountOC = Convert.ToDouble(txtAmountOCto.Text);
                        if (editFlag == true)
                        {
                            //if (bankTransfer.isApproved == false)
                            //{
                            accntBalance = accntBalance - bankTransfer.AmountOC;
                            txtExistingBalanceTo.Text = accntBalance.ToString();
                            txtAfterTransferTo.Text = (accntBalance + bankTransfer.AmountOC).ToString();
                            //}
                            //else
                            //{
                            //    txtExistingBalanceTo.Text = accntBalance.ToString();
                            //    txtAfterTransferTo.Text = accntBalance.ToString();
                            //}

                        }
                        else
                        {
                            txtExistingBalanceTo.Text = accntBalance.ToString();
                            txtAfterTransferTo.Text = (accntBalance - amountOC).ToString();
                        }
                    }
                    return;
                }
                else
                {
                    if (bankAccount.COAaccount != null && bankAccount.COAaccount.JournalTransactions != null)
                    {
                        var chartofAccount = bankAccount.COAaccount;
                        var accntBalance = chartofAccount.JournalTransactions.Where(
                        x => x.AdminBill?.isVoid != true
                        && x.Bill?.isVoid != true
                        && x.PurchaseInvoice?.isVoid != true
                        && x.SaleInvoice?.isVoid != true
                        && x.SalesReceipt?.isVoid != true
                        && x.journalVoucher?.isVoid != true
                        && x.Payment?.isVoid != true
                        && x.InterBank?.isVoid != true
                        && x.InterCompanyTransfer?.isVoid != true
                        &&
                        x.deptId != null && deptIds.Contains((int)x.deptId)
                        &&
                        x.companyId != null && companyIds.Contains((int)x.companyId)
                        &&
                        x.creationDate <= GlDate
                        ).Sum(x => x.total);
                        var amountOC = Convert.ToDouble(txtAmountOC.Text);
                        if (editFlag == true)
                        {
                            //if(bankTransfer.isApproved == false)
                            //{
                            accntBalance = accntBalance - bankTransfer.AmountOC;
                            txtExistingBalanceTo.Text = accntBalance.ToString();
                            txtAfterTransferTo.Text = (accntBalance + bankTransfer.AmountOC).ToString();
                            //}
                            //else
                            //{
                            //    txtExistingBalanceTo.Text = accntBalance.ToString();
                            //    txtAfterTransferTo.Text = accntBalance.ToString();
                            //}

                        }
                        else
                        {
                            txtExistingBalanceTo.Text = accntBalance.ToString();
                            txtAfterTransferTo.Text = (accntBalance + amountOC).ToString();
                        }
                    }
                    
                }

                if ((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == TransferType.Advances)
                {
                    //var accnt = cmbxAccountTo.SelectedItem as Account;
                    var bank = bankAccount.bank;
                    cmbxBankTo.NullText = bank.BankName;
                    cmbxBankTo.NullTextForeground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        private void CmbxTransferType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            
            if((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == ERP_BL.Enums.TransferType.Advances)
            {
                lblTransactionType.Text = "Advances-";
                grdTaxNAmountPaid.Visibility = Visibility.Visible;

                grdVendorType.Visibility = Visibility.Visible;
                grdVendor.Visibility = Visibility.Visible;
                grdAmountReceivedOC.Visibility = Visibility.Visible;
                grdAmountReceivedMER.Visibility = Visibility.Visible;

                grdBankTo.SetValue(Grid.ColumnProperty, 2);
                cmbxBankTo.IsReadOnly = true;
                cmbxBankTo.IsEnabled = false;


                grdAccountTo.SetValue(Grid.RowProperty, 2);

                grdItems.Visibility = Visibility.Visible;
                grdCurrency.Visibility = Visibility.Visible;

                grdCurrencyFrom.Visibility = Visibility.Collapsed;
                grdAmountFrom.Visibility = Visibility.Collapsed;
                grdCurrencyTo.Visibility = Visibility.Collapsed;
                grdAmountTo.Visibility = Visibility.Collapsed;

                imgArrow.Visibility = Visibility.Visible;
                layoutGrpER.Visibility = Visibility.Collapsed;


                layoutGrpCurrency.Visibility = Visibility.Visible;
                grdMerFrom.Visibility = Visibility.Collapsed;
                grdAmountMerFrom.Visibility = Visibility.Collapsed;
                grdAmountMerTo.Visibility = Visibility.Collapsed;
                grdMerTo.Visibility = Visibility.Collapsed;
                //grdAccountTo.SetValue(Grid.ColumnProperty, 0);
            }
            else if((ERP_BL.Enums.TransferType)cmbxTransferType.SelectedIndex == ERP_BL.Enums.TransferType.IBT_Single_Currency)
            {
                lblTransactionType.Text = "IBT-";
                grdTaxNAmountPaid.Visibility = Visibility.Collapsed;

                grdVendorType.Visibility = Visibility.Collapsed;
                grdVendor.Visibility = Visibility.Collapsed;
                grdAmountReceivedOC.Visibility = Visibility.Collapsed;
                grdAmountReceivedMER.Visibility = Visibility.Collapsed;

                grdBankTo.SetValue(Grid.ColumnProperty, 0);
                cmbxBankTo.IsReadOnly = false;
                cmbxBankTo.IsEnabled = true;

                grdAccountTo.SetValue(Grid.RowProperty, 0);
                grdItems.Visibility = Visibility.Visible;
                grdCurrency.Visibility = Visibility.Visible;


                grdCurrencyFrom.Visibility = Visibility.Collapsed;
                grdAmountFrom.Visibility = Visibility.Collapsed;
                grdCurrencyTo.Visibility = Visibility.Collapsed;
                grdAmountTo.Visibility = Visibility.Collapsed;

                imgArrow.Visibility = Visibility.Visible;
                layoutGrpER.Visibility = Visibility.Collapsed;
                //grdAccountTo.SetValue(Grid.ColumnProperty, 2);

                layoutGrpCurrency.Visibility = Visibility.Visible;
                grdMerFrom.Visibility = Visibility.Collapsed;
                grdAmountMerFrom.Visibility = Visibility.Collapsed;
                grdAmountMerTo.Visibility = Visibility.Collapsed;
                grdMerTo.Visibility = Visibility.Collapsed;
            }
            else if (cmbxTransferType.SelectedIndex == 2)
            {
                lblTransactionType.Text = "IBT-";
                grdTaxNAmountPaid.Visibility = Visibility.Collapsed;

                grdVendorType.Visibility = Visibility.Collapsed;
                grdVendor.Visibility = Visibility.Collapsed;
                grdAmountReceivedOC.Visibility = Visibility.Collapsed;
                grdAmountReceivedMER.Visibility = Visibility.Collapsed;

                grdBankTo.SetValue(Grid.ColumnProperty, 0);
                cmbxBankTo.IsReadOnly = false;
                cmbxBankTo.IsEnabled = true;

                grdAccountTo.SetValue(Grid.RowProperty, 0);

                grdItems.Visibility = Visibility.Collapsed;
                //grdCurrency.Visibility = Visibility.Collapsed;


                grdCurrencyFrom.Visibility = Visibility.Visible;
                grdAmountFrom.Visibility = Visibility.Visible;
                grdCurrencyTo.Visibility = Visibility.Visible;
                grdAmountTo.Visibility = Visibility.Visible;

                imgArrow.Visibility = Visibility.Collapsed;
                layoutGrpER.Visibility = Visibility.Visible;

                layoutGrpCurrency.Visibility = Visibility.Collapsed;
                grdMerFrom.Visibility = Visibility.Visible;
                grdAmountMerFrom.Visibility = Visibility.Visible;
                grdAmountMerTo.Visibility = Visibility.Visible;
                grdMerTo.Visibility = Visibility.Visible;
            }
        }

        public ucFrmBankTransfers(InterBankTransferStatus status)
        {
            statusChanged = status;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer") != null)
            {
                var selectedBankTransfer = bankTransfer;//grdInterBankTransfer.SelectedItem as InterBankTransfer;
                UsersRepo usersRepo = new UsersRepo();

                if (selectedBankTransfer != null && selectedBankTransfer.Id != 0)
                {
                    //bankTransRepo = new InterBankTransRepo();

                    var selectedRow = bankTransfer;//grdInterBankTransfer.SelectedItem as InterBankTransfer;
                    ERP_BL.Procurements.InterBankTransfers.InterBankTransfer bankTransfer1 = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();
                    bankTransfer1 = bankTransRepo.GetInterBankTransfer(bankTransfer.Id);

                    List<ProcurementProduct> products = new List<ProcurementProduct>();
                    foreach (var _prod in bankTransfer1.products)
                    {
                        products.Add(_prod);
                    }

                    if (bankTransfer1.isApproved == false)
                    {
                        MessageBox.Show("Transaction should be approved before closing!");
                        return;
                    }
                    var previous_status = bankTransfer1.interBankTransStatus.Status;


                    statusChanged = null;
                    ZAS_ERP.Bankings.UserControls.ucFrmDirectClose ucFrmDirectClose = new ZAS_ERP.Bankings.UserControls.ucFrmDirectClose();

                    if (bankTransfer1.interBankTransStatus != null)
                    {
                        ucFrmDirectClose.statusName.Text = bankTransfer1.interBankTransStatus.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(bankTransfer1.interBankTransStatus.backcolor);
                    }

                    ucFrmDirectClose.IBTflag = true;
                   ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null)
                        {
                            bankTransfer1.PendingForClosing = false;
                            bankTransfer1.stage = TransactionStage.Approved.ToString();
                            bankTransfer1.statusId = statusChanged.Id;
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;

                            bankTransRepo.approveInterBankTransfer(bankTransfer1);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                        {
                            bankTransfer1.statusId = statusChanged.Id;
                            bankTransfer1.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            if (bankTransfer1.PendingForClosing == null)
                            {
                                bankTransfer1.PendingForClosing = true;
                            }
                            bankTransRepo.approveInterBankTransfer(bankTransfer1);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {
                            bankTransfer1.statusId = statusChanged.Id;
                            bankTransfer1.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            if (bankTransfer1.PendingForClosing != true)
                            {
                                bankTransfer1.PendingForClosing = true;

                            }
                            bankTransRepo.approveInterBankTransfer(bankTransfer1);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else
                        {
                            bankTransfer1.statusId = statusChanged.Id;
                            bankTransfer1.stage = TransactionStage.AwaitingFirstReview.ToString();
                            bankTransfer1.PendingForClosing = true;
                            bankTransfer1.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer1.ClosingDate = System.DateTime.Now;
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                            bankTransRepo.approveInterBankTransfer(bankTransfer1);
                        }
                        //Adding signature (comment)

                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Bank Tranfer has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (selectedRow.department != null && selectedRow.department.Id != 0 && selectedRow.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(selectedRow.department.Id, selectedRow.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
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
                            string oldStat = "";
                            if (oldStatus != null)
                            {
                                oldStat = oldStatus.Status;
                            }
                            string newStat = statusChanged.Status;
                            string symbolCurr = "";
                            if (selectedRow.currency != null)
                            {
                                symbolCurr = selectedRow.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Internal BankTransfer (Amount OC) having value: " + selectedRow.AmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(selectedRow.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ",null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterBank_Transfer, comment.Comment, 0,user.id, "New Comment ",null);
                                }
                            }

                        }
                        MessageBox.Show("Inter-Bank Transfer status changed to InActive (" + statusChanged.Status + ")");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Close Inter-Bank Transfer!");
                return;
            }
        }

        private void cmbIndustry_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select department first!");
                cmbxDepartment.Focus();
                return;
            }
        }

        private void cmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int deptId = (cmbxDepartment.SelectedItem as Department).Id;
            int typeId = (cmbIndustry.SelectedItem as cmbitem).id;
            if (typeId == 0)
            {
                frmIndustryTypeAdd industryadd = new frmIndustryTypeAdd();
                industryadd.ShowDialog();
                loadIndustryTypes();
            }
            

            VendorRepo vendorRepo = new VendorRepo();
            var vendors = vendorRepo.getAllByIndustryTypeDept(typeId, deptId);
            cmbxVendor.ItemsSource = vendors;
            cmbxAccountTo.SelectedIndex = -1;
            cmbxVendor.SelectedIndex = -1;
        }

        private void CmbxTaxType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxTaxType.SelectedItem != null)
            {
                int idd = (cmbxTaxType.SelectedItem as TaxType).Id;
                if(idd != 0)
                {
                    List<TaxName> taxes = new List<TaxName>();
                    TaxRepo taxRepo = new TaxRepo();

                    taxes = taxRepo.getAllTaxes().Where(x=>x.taxType.Id == idd).ToList();
                    cmbxTaxName.ItemsSource = taxes;
                }
            }
        }

        private void CmbxTaxFlag_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var selectedCategory = ((ERP_BL.Enums.TaxFlag)cmbxTaxFlag.SelectedIndex);
            if (selectedCategory == ERP_BL.Enums.TaxFlag.Tax)
            {
                grpTaxDetails.Visibility = Visibility.Visible;
                

                txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
                txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();

                taxCalculations();
            }
            else
            {
                grpTaxDetails.Visibility = Visibility.Collapsed;
                txtAmountPaidMER.Text = txtAmountMer.Text;
                txtAmountPaidOC.Text = txtAmountOC.Text;
            }
        }

        private void CmbxTaxName_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            taxCalculations();
        }

        private void taxCalculations()
        {
            if (cmbxTaxName.SelectedItem != null)
            {
                var tax = cmbxTaxName.SelectedItem as TaxName;
                var taxPercent = tax.percentage;
                var amountOC = Convert.ToDouble(txtAmountOC.Text);
                var amountMER = Convert.ToDouble(txtAmountMer.Text);

                txtTaxAmountOC.Text = ((taxPercent * amountOC) / 100).ToString();
                txtTaxAmountMER.Text = ((taxPercent * amountMER) / 100).ToString();

                txtAmountPaidMER.Text = (Convert.ToDouble(txtAmountMer.Text) - Convert.ToDouble(txtTaxAmountMER.Text)).ToString();
                txtAmountPaidOC.Text = (Convert.ToDouble(txtAmountOC.Text) - Convert.ToDouble(txtTaxAmountOC.Text)).ToString();
            }
        }

        private void CmbxVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            receiptRepo = new SalesReceiptRepo();

            if(cmbxVendor.SelectedIndex >= 0)
            {
                var accnts = receiptRepo.GetAllAccounts().Where(x => x.vendor_Id == (cmbxVendor.SelectedItem as Vendor).Id).ToList();
                cmbxAccountTo.ItemsSource = accnts;
                cmbxAccountTo.SelectedIndex = -1;
                cmbxBankTo.NullText = null;
            }

        }

        private void TxtAmountOCfrom_KeyUp(object sender, KeyEventArgs e)
        {
            if(cmbxTransferType.SelectedIndex == 2)
            {
                var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
                var amountMER = Convert.ToDouble(txtMER.Text);
                var amountER = Convert.ToDouble(txtER.Text);
                txtAmountMer.Text = (amountFrom * amountMER).ToString();

                txtAmountOCto.Text = (amountFrom * amountER).ToString();
            }
            
        }

        private void TxtER_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if(amountFrom > 0)
            {
                var amountER = Convert.ToDouble(txtER.Text);
                txtAmountOCto.Text = (amountFrom * amountER).ToString();
            }
        }

        private void TxtAmountOCto_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);
                txtER.Text = (amountTo/ amountFrom).ToString();
            }
        }

        private void TxtMerFrom_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var MERfrom = Convert.ToDouble(txtMerFrom.Text);
                var total = MERfrom * amountFrom;
                txtAmountMERfrom.Text = total.ToString();
                txtAmountMERto.Text = total.ToString();

                var amountMERto = Convert.ToDouble(txtAmountMERto.Text);
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);

                txtMerTo.Text = Math.Round((amountMERto / amountTo)).ToString();
            }
        }

        private void TxtAmountMERfrom_KeyUp(object sender, KeyEventArgs e)
        {
            var amountFrom = Convert.ToDouble(txtAmountOCfrom.Text);
            if (amountFrom > 0)
            {
                var amountMERfrom = Convert.ToDouble(txtAmountMERfrom.Text);
                txtMerFrom.Text = (amountMERfrom / amountFrom).ToString();
                txtAmountMERto.Text = txtAmountMERfrom.Text;

                var amountMERto = Convert.ToDouble( txtAmountMERto.Text);
                var amountTo = Convert.ToDouble(txtAmountOCto.Text);

                txtMerTo.Text = (amountMERto / amountTo).ToString();
            }
        }

        private void TxtMerTo_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void TxtAmountMERto_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if (bankTransfer.Id != 0)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveIBTAttachmentCategories();
                }
                grdAttach1.Visibility = Visibility.Visible;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (bankTransfer.Id != 0)
                {
                    List<TreeItem> atachments = SYSTEM_STATIC.GetIBTAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterBank_Transfer);
                    treeViewAttachments1.ItemsSource = atachments;
                }
                grdAttachments1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (bankTransfer.Id != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\InterBank_Transfer\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";

                        if (sourceFile.Length < 74)
                        {
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += bankTransfer.Id + "_" + TransactionItemType.InterBank_Transfer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.InterBank_Transfer);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), bankTransfer.Id, TransactionItemType.InterBank_Transfer, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bankTransfer.Id, 13, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bankTransfer.Id, TransactionItemType.InterBank_Transfer);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                });
                                thread.Start();


                                //MessageBox.Show("Attachment Uploaded");


                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid File name size");
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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

        private void BtnPayment_Checked(object sender, RoutedEventArgs e)
        {
            btnDeposit.IsChecked = false;
        }

        private void BtnDeposit_Checked(object sender, RoutedEventArgs e)
        {
            btnPayment.IsChecked = false;
        }

        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getJournalTransactions());
                generalJournal.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        }
        public List<JournalTransaction> getJournalTransactions()
        {
            List<JournalTransaction> transactionList = new List<JournalTransaction>();
            if (bankTransfer.transferType == TransferType.Advances)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionCredit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = Convert.ToDouble(txtAmountPaidOC.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        deptId = department.Id,
                        total = 0 - Convert.ToDouble(txtAmountPaidOC.Text),
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                    };
                    transactionList.Add(transactionCredit);
                }
                if (bankTransfer.taxNameId != null)
                {
                    TaxRepo taxRepo = new TaxRepo();
                    var tax = taxRepo.getTaxtById((int)bankTransfer.taxNameId);
                    if (tax.chartofAccount != null && !string.IsNullOrEmpty(txtFinanceRefNo.Text))
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            JournalTransaction transactionTax = new JournalTransaction()
                            {
                                accountId = tax.chartofAccount.Id,
                                coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                creationDate = bankTransfer.GLPostingDate,
                                credit = Convert.ToDouble(txtTaxAmountOC.Text),
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                memo = txtFinanceRefNo.Text,
                                transactionRefno = txtSystemRefNo.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                deptId = department.Id,
                                taxFlag = true,
                                total = 0 - Convert.ToDouble(txtTaxAmountOC.Text),
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                            };
                            transactionList.Add(transactionTax);
                        }

                    }
                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionDebit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        debit = Convert.ToDouble(txtAmountReceivedOC.Text),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        total = Convert.ToDouble(txtAmountReceivedOC.Text) - 0,
                        deptId = department.Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                    };
                    transactionList.Add(transactionDebit);
                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATfrom != null)
                {
                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATto != null)
                {
                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
            }
            else if (bankTransfer.transferType == TransferType.IBT_Single_Currency)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionCredit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = Convert.ToDouble(txtAmountOC.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        total = 0 - Convert.ToDouble(txtAmountOC.Text),
                        deptId = department.Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                    };
                    transactionList.Add(transactionCredit);
                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionDebit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                        debit = Convert.ToDouble(txtAmountOC.Text),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        total = Convert.ToDouble(txtAmountOC.Text) - 0,
                        deptId = department.Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                    };
                    transactionList.Add(transactionDebit);
                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATfrom != null)
                {
                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATto != null)
                {
                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }

            }
            else if (bankTransfer.transferType == TransferType.IBT_Multiple_Currency)
            {
                if (btnPushCredits.IsChecked == true)
                {
                    JournalTransaction transactionCredit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = Convert.ToDouble(txtAmountOCfrom.Text),
                        debit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMerFrom.Text), 2),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        total = 0 - Convert.ToDouble(txtAmountOCfrom.Text),
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                    };
                    transactionList.Add(transactionCredit);
                }
                if (btnPushDebits.IsChecked == true)
                {
                    JournalTransaction transactionDebit = new JournalTransaction()
                    {
                        accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                        coaTransactionsType = coaTransactionsType.InterBankTransfer,
                        creationDate = bankTransfer.GLPostingDate,
                        credit = 0,
                        MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                        debit = Convert.ToDouble(txtAmountOCto.Text),
                        memo = txtFinanceRefNo.Text,
                        transactionRefno = txtSystemRefNo.Text,
                        userId = SYSTEM_STATIC.currentUser.id,
                        total = Convert.ToDouble(txtAmountOCto.Text) - 0,
                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                        currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                    };
                    transactionList.Add(transactionDebit);

                }
                if (bankTransfer.bankChargesFrom != null)
                {
                    if (bankTransfer.bankChargesFrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesFrom)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.bankChargesTo != null)
                {
                    if (bankTransfer.bankChargesTo.Count > 0)
                    {
                        foreach (var charges in bankTransfer.bankChargesTo)
                        {
                            var dbDuduction = paymentRepo.GetDeduction((int)charges.deduction_Id);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATfrom != null)
                {
                    if (bankTransfer.InterBankTransferVATfrom.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATfrom)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountFrom.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyFrom.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
                if (bankTransfer.InterBankTransferVATto != null)
                {
                    if (bankTransfer.InterBankTransferVATto.Count > 0)
                    {
                        foreach (var charges in bankTransfer.InterBankTransferVATto)
                        {
                            var dbDuduction = taxRepo.getTaxName((int)charges.taxNameId);

                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = dbDuduction.COA_Id,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = charges.Amount,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = Convert.ToDouble(charges.Amount) - 0,
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);

                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction transactionDebit = new JournalTransaction()
                                {
                                    accountId = (cmbxAccountTo.SelectedItem as Account).COA_accountId,
                                    coaTransactionsType = coaTransactionsType.InterBankTransfer,
                                    creationDate = bankTransfer.GLPostingDate,
                                    credit = charges.Amount,
                                    MER = Math.Round(Convert.ToDouble(txtMerTo.Text), 2),
                                    debit = 0,
                                    memo = txtFinanceRefNo.Text,
                                    transactionRefno = txtSystemRefNo.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    total = 0 - Convert.ToDouble(charges.Amount),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrencyTo.SelectedItem as Currency).Id

                                };
                                transactionList.Add(transactionDebit);
                            }
                        }
                    }
                }
            }
            return transactionList;

        }

        private void BtnCreateSO_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                frmProcurmentPanel frmProcurmentPanel = new frmProcurmentPanel();

                //bankTransfer.ucStatuschange.offerid = transactionId;
                //Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                //statusChange.Owner = this;
                //statusChange.ShowDialog();
                //usercongrid.Children.Clear();
                ZAS_ERP.Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new ZAS_ERP.Procurementss.SaleOrderss.ucSaleOrderAdd(bankTransferId);
                frmProcurmentPanel.usercongrid.Children.Add(ucSaleOrderAdd);
                    //SaleOrderss.ucSaleOrderAdd.offerid = transactionId;
                    Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                    Procurementss.Offerss.ucOfferAdd.offerid = 0;
                frmProcurmentPanel.Show();



            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (bankTransferId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(bankTransferId, TransactionItemType.InterBank_Transfer);
                trackingWindow.ShowDialog();
            }
        }

        //private void MbtnBankChargesFrom_Click(object sender, RoutedEventArgs e)
        //{
           
        //}

        //private void MbtnBankChargesTo_Click(object sender, RoutedEventArgs e)
        //{
           
        //}

        private void BtnAddBankChargesFrom_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (bankTransfer != null)
                    {
                        //var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
                        winIBTbankCharges winIBTbankCharges = new winIBTbankCharges(bankTransferId);
                        winIBTbankCharges.BankChargesFrom = true;
                        winIBTbankCharges.ShowDialog();
                        bankTransfer.bankChargesFrom = winIBTbankCharges.finalBankCharges;
                        txtBankChargesFrom.Text = winIBTbankCharges.totalBankCharges.ToString();

                        bankTransfer.InterBankTransferVATfrom = winIBTbankCharges.finalVATs;
                        txtVATFrom.Text = winIBTbankCharges.totalVAT.ToString();
                        //receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        //receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        bankTransfer.IsAdjustedVATfrom = winIBTbankCharges.IsAdjustedFrom;
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("dedVAT", ucSalesReceiptDeduction.totalVAT);

                    }
                    //else
                    //    DXMessageBox.Show("Please Select Receipt to Add Deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Inter-Bank Transfer first!");
                return;
            }
        }

        private void BtnAddBankChargesTo_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (bankTransfer != null)
                    {
                        //var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
                        winIBTbankCharges winIBTbankCharges = new winIBTbankCharges(bankTransferId);
                        winIBTbankCharges.BankChargesFrom = false;
                        winIBTbankCharges.ShowDialog();
                        bankTransfer.bankChargesTo = winIBTbankCharges.finalBankCharges;
                        txtBankChargesTo.Text = winIBTbankCharges.totalBankCharges.ToString();

                        bankTransfer.InterBankTransferVATto = winIBTbankCharges.finalVATs;
                        txtVATTo.Text = winIBTbankCharges.totalVAT.ToString();
                        //receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        //receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        bankTransfer.IsAdjustedVATto = winIBTbankCharges.IsAdjustedTo;
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("dedVAT", ucSalesReceiptDeduction.totalVAT);

                    }
                    //else
                    //    DXMessageBox.Show("Please Select Receipt to Add Deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Inter-Bank Transfer first!");
                return;
            }
        }

        private void cmbxCurrency_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            DateTime creationDate = (DateTime)datCreationDate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (cmbxCurrency.SelectedIndex != -1 && cmbxCompany.SelectedIndex != -1)
                {
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbxCurrency.SelectedItem as Currency).Id, (cmbxCompany.SelectedItem as Company).currency.Id, creationDate.Year);
                    if ((cmbxCompany.SelectedItem as Company).currency.Id == (cmbxCurrency.SelectedItem as Currency).Id)
                    {
                        txtMER.Text = 1.ToString();
                    }
                    else
                    {

                        if (exchangeRateGroupMER != null)
                        {
                            exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (cmbxCompany.SelectedItem as Company).Id);
                            switch (creationDate.Month)
                            {
                                case 1:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateJan.ToString();
                                    break;
                                case 2:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateFeb.ToString();
                                    break;
                                case 3:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateMar.ToString();
                                    break;
                                case 4:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateApr.ToString();
                                    break;
                                case 5:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateMay.ToString();
                                    break;
                                case 6:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateJun.ToString();
                                    break;
                                case 7:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateJul.ToString();
                                    break;
                                case 8:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateAug.ToString();
                                    break;
                                case 9:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateSep.ToString();
                                    break;
                                case 10:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateOct.ToString();
                                    break;
                                case 11:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateNov.ToString();
                                    break;
                                case 12:
                                    if (exchangeRate != null)
                                        txtMER.Text = exchangeRate.rateDec.ToString();
                                    break;
                                default:
                                    if (exchangeRate != null)
                                        txtMER.Text = 0.ToString();
                                    break;
                            }
                        }
                    }
                }

            }
        }

        private void txtMER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amountOC = Convert.ToDouble(txtAmountOC.Text);
            var MER = Convert.ToDouble(txtMER.Text);

            if (MER == 0)
            {
                txtAmountMer.Text = amountOC.ToString();
            }
            else
            {
                txtAmountMer.Text = (amountOC * MER).ToString();
            }
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                gridOrderStageTrack.Visibility = Visibility.Visible;
                GellAllOrdersTracking();
            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();

        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;

            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
            if (grid.SelectedItem != null)
            {

                var item = grid.SelectedItem as AllOrdersView;

                if (item.transactionType == TransactionItemType.STL)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                    {
                        STLRepo sTLRepo = new STLRepo();

                        var selectedStl = sTLRepo.Get(Convert.ToInt32(item.Id));
                        winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                        stl.stl = selectedStl;
                        stl.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else

                if (item.transactionType == TransactionItemType.InterBank_Transfer)
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(item.Id);

                            if (selectedBankTransfer != null)
                            {
                                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                CompanyRepo compRepo = new CompanyRepo();
                                bankTransRepo = new InterBankTransRepo();

                                //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBankTransfer.editFlag = true;

                                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                    ucFrmBankTransfer.frmBankTranfer.Show();
                                }


                            }
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    if (item.transactionType == TransactionItemType.Sale_Receipt)
                    {
                        GrdSaleReceiptListLoad(item.Id);
                        return;
                    }

                    if (item.transactionType == TransactionItemType.Tasks)
                    {
                        ucTaskAdd taskAdd = new ucTaskAdd();
                        taskAdd.taskId = Convert.ToInt32(item.Id);
                        taskAdd.editFlag = true;
                        Window win = new Window();
                        win.Content = taskAdd;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.LoansAdvances)
                    {
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id);
                        frmLoansAdvances.editFlag = true;
                        Window win = new Window();
                        win.Content = frmLoansAdvances;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.TargetReward)
                    {
                        ucFrmBasicTargetRewards frmTargetRewards = new ucFrmBasicTargetRewards();
                        frmTargetRewards.rewardId = Convert.ToInt32(item.Id);
                        //frmTargetRewards.editFlag = true;
                        Window win = new Window();
                        win.Content = frmTargetRewards;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.transactionType == TransactionItemType.Admin_Bill)
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(item.Id);

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmBill.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId;
                                frmBill.Content = frmBillAdd;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                frmBill.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.transactionType == TransactionItemType.Payments)
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(item.Id);
                        //var payments = paymentRepo.GetPaymentsByGroupId(payment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = payment.transactionGroupId;
                                        frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                        frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                        frmLApayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLApayment.editFlag = true;
                                    frmLApayment.groupId = payment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Target_Reward:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRpayment.editFlag = true;
                                        frmTRpayment.groupId = payment.transactionGroupId;
                                        frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                        frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRpayment.editFlag = true;
                                    frmTRpayment.groupId = payment.transactionGroupId;
                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRpayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
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
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                         
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;

                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                       
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                   
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                            
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                  
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                               
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }
                        return;
                    }
                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.transactionType.ToString()), item.Id);
                    procurmentPanele.Show();
                }
            }
        }

        public void GellAllOrdersTracking()
        {
            trackingOrder = bankTransRepo.GetInterBankTransfer(bankTransferId);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.InterBank_Transfer);
            }
        }
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                    {
                        ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                        //paymentRepo = new PaymentRepo();
                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                        Window frmPiPaymentWindow = new Window();
                        if (saleReceipt.saleReceiptStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = saleReceipt.transactionGroupId;
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
                            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
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
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;


                        if (saleReceipt == null)
                        {
                            return;
                        }

                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;
                        }

                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;
                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
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

                        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }



            }
            catch
            {

            }
        }
        private void loadVATBookReferenceNo()
        {
            if (cmbxCompany.SelectedItem != null)
            {
                ERP_BL.VATBook.VATBookRepo vatBookRepo = new ERP_BL.VATBook.VATBookRepo();
                var references = vatBookRepo.GetAllActiveVATBookReferenceNo((cmbxCompany.SelectedItem as Company).Id);
                List<cmbitem> cmbitems = new List<cmbitem>();
                cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
                foreach (ERP_BL.VATBook.VATBookRefNumber _ref in references)
                {
                    cmbitems.Add(new cmbitem() { name = _ref.VATBookReferenceNo, id = _ref.Id });
                }
                cmbxVATBookRef.ItemsSource = cmbitems;
            }
        }


    }
}
