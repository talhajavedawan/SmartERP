using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.Budget;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits
{
    /// <summary>
    /// Interaction logic for ucFrmCustomerCreditReceipt.xaml
    /// </summary>
    public partial class ucFrmCustomerCreditReceipt : UserControl
    {
        Company company = new Company();
        Department department = new Department();
        List<CustomerCredit> customerCredits = new List<CustomerCredit>();
        List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
        UsersRepo UsersRepo = new UsersRepo();
        public int groupId = 0;
        public bool editFlag = false;
        public int receiptId;
        public int SerialNo = 0;
        public int saleInvoiceId = 0;

        static SalesReceiptStatus statusChanged = new SalesReceiptStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();

        SalesReceiptRepo salesReceiptRepo = new SalesReceiptRepo();
        string stage;
        bool? isApproved;
        //bool? isReApproved;
        DateTime? approvalDate;
        int banktransactionFlag = 0;
        SalesReceiptStatus checkStatus = new SalesReceiptStatus();
        List<SalesReceiptStatus> ReceiptStatuses = new List<SalesReceiptStatus>();
        TaxRepo taxRepo = new TaxRepo();

        List<Account> accntList = new List<Account>();
        List<Bank> bankList = new List<Bank>();
        List<Bank> allBanks = new List<Bank>();
        List<Account> allAccounts = new List<Account>();
        Bank bank = new Bank();
        SalesReceiptRepo repo = new SalesReceiptRepo();

        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<Department> deptList = new List<Department>();
        bool? companyChnaged = null;

        List<CustomerCreditsSaleReceiptModelView> _visibleItems = new List<CustomerCreditsSaleReceiptModelView>();
        public ucFrmCustomerCreditReceipt()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                loadReceiptTypes();
                loadCompanies();
                loadCurrencies();
                loadCollectionMethods();
                loadReceiptStatus();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of SaleReceipt") != null)
                {
                    datglPostingdate.IsEnabled = true;
                }
                else
                {
                    datglPostingdate.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass SaleReceipt Bank Account") != null)
                {
                    grdBypass.IsEnabled = true;
                }
                else
                {
                    grdBypass.IsEnabled = false;
                }
                if (editFlag == false)
                {

                    datCreationDate.DateTime = DateTime.Now;
                    datglPostingdate.DateTime = DateTime.Now;
                    btnRefresh.IsEnabled = false;
                    datglPostingdate.EditValue = System.DateTime.Now;

                    if (SerialNo != 0 && saleInvoiceId != 0)
                    {
                        btnLoad.IsEnabled = false;
                        SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
                        CustomerCredit _cusCredit = new CustomerCredit();
                        List<CustomerCreditsSaleReceiptModelView> modelViewList = new List<CustomerCreditsSaleReceiptModelView>();


                        _cusCredit = saleInvoiceRepo.getCustomerCredit(SerialNo, saleInvoiceId);
                        customerCredits.Add(_cusCredit);

                        if (_cusCredit != null)
                        {

                            int index = 0;
                            //Select Company
                            var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                            if (_cusCredit.SaleInvoice.company != null)
                            {
                                if (_cusCredit.SaleInvoice.company != null && companyList.Find(x => x.Id == _cusCredit.SaleInvoice.company_Id) == null)
                                {
                                    companyList.Add(_cusCredit.SaleInvoice.company);
                                    lookupCompany.ItemsSource = null;
                                    lookupCompany.ItemsSource = companyList;
                                }
                                //lookupCompany.Text = _bill.company.CompanyName;
                                index = 0;
                                foreach (var _company in companyList)
                                {
                                    if (_company.Id == _cusCredit.SaleInvoice.company_Id)
                                    {
                                        lookupCompany.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }
                            }


                            //var departmentList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            //if (_cusCredit.SaleInvoice.department != null)
                            //{

                            //    if (_cusCredit.SaleInvoice.department != null && departmentList.Find(x => x.Id == _cusCredit.SaleInvoice.dept_Id) == null)
                            //    {

                            //        departmentList.Add(_cusCredit.SaleInvoice.department);
                            //        lookupDepartment.ItemsSource = null;
                            //        lookupDepartment.ItemsSource = departmentList;
                            //    }

                            //    index = 0;
                            //    foreach (var _dept in departmentList)
                            //    {
                            //        if (_dept.Id == _cusCredit.SaleInvoice.dept_Id)
                            //        {
                            //            lookupDepartment.SelectedIndex = index;
                            //            index = 0;
                            //            break;
                            //        }
                            //        index++;
                            //    }

                            //}


                            string deptNames = "";
                            if (_cusCredit.SaleInvoice.department != null)
                            {
                                deptList.Add(_cusCredit.SaleInvoice.department);
                            }


                            if (deptList.Count > 0)
                            {
                                foreach (var _dept in deptList)
                                {
                                    deptNames = deptNames + " | " + _dept.DeptName;

                                    allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                                }
                            }
                            loadEmployees();

                            lookupDepartment1.EditValue = deptNames;


                            //Select Currency
                            var currencyList = (lookupCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookupCurrency.ItemsSource as List<Currency>;
                            if (_cusCredit.SaleInvoice.currency != null)
                            {

                                index = 0;
                                foreach (var _currency in currencyList)
                                {

                                    if (_currency.Id == _cusCredit.SaleInvoice.currency_Id)
                                    {
                                        lookupCurrency.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }

                            }

                            lookupCompany.IsEnabled = false;
                            lookupDepartment1.IsEnabled = false;
                            lookupCurrency.IsEnabled = false;

                            CustomerCreditsSaleReceiptModelView SI = new CustomerCreditsSaleReceiptModelView();
                            //if ((_invoice.customerCompany.company.Id == custCompany.Id) && (_invoice.currency.Id == selectedCurrency.Id) && (_invoice.department.Id == department.Id))
                            //{
                            SI.InvoiceId = _cusCredit.SaleInvoice.Id;
                            SI.serialNo = _cusCredit.SerialNo;
                            SI.Date = _cusCredit.SaleInvoice.ApprovedDate;
                            SI.Customer = _cusCredit.CustomerCompany.company.CompanyName;
                            SI.SalesReferenceNo = _cusCredit.SaleInvoice.SalesReferenceNo;
                            SI.FinanceRefNo = _cusCredit.SaleInvoice.FinanceRefrenceNo;
                            SI.Currency = _cusCredit.SaleInvoice.currency.CurrencyName;

                            double invoiceAmount = 0;
                            invoiceAmount = _cusCredit.creditAmount;



                            SI.OriginalAmount = invoiceAmount;

                            SI.InvoiceStage = GetInvoiceStatus(_cusCredit.SaleInvoice);

                            var reciepts = _cusCredit.SaleInvoice.salesReceipts;
                            if (reciepts != null)
                            {
                                var result = Math.Round(reciepts.Where(x => x.isVoid != true && x.CustomerCreditSerialNo == SerialNo).Sum(x => x.CollectionAmount), 2);

                                SI.AmountDue = invoiceAmount - result;
                            }

                            customerCredits.Add(_cusCredit);

                            SI.InvoiceStage = GetInvoiceStatus(_cusCredit.SaleInvoice);

                            modelViewList.Add(SI);
                            grdCntrlSalesReceipt.ItemsSource = modelViewList;
                        }
                    }
                    if (editFlag == false)
                    {
                        btnPushDebits.IsChecked = true;
                        btnPushCredits.IsChecked = true;
                    }
                    else
                    {
                        if (saleReceipts[0].isBypassBank == true && saleReceipts[0].coaAccountId != null)
                        {
                            isBypassCOA.IsChecked = true;
                            lookupCOA.Text = saleReceipts[0].ChartofAccount.accountName;
                        }
                    }
                }
               

                if (editFlag == true && groupId > 0)
                {
                    saleReceipts = salesReceiptRepo.getReceiptsByGroupIdForLA(groupId);

                    if (saleReceipts[0].isDeposit == true)
                        btnDeposit.IsChecked = true;
                    else if (saleReceipts[0].isDeposit == false)
                        btnPayment.IsChecked = true;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Customer Credit Receipts") == null)
                    {
                        btnSave.IsEnabled = false;

                    }

                    if (saleReceipts[0].saleReceiptStatus.isActive == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleReceipt") != null)
                            btnSave.IsEnabled = true;
                        else
                            btnSave.IsEnabled = false;
                    }


                    if (saleReceipts[0].isApproved == false)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Receipt") == null)
                            btnSave.IsEnabled = false;
                        else
                            btnSave.IsEnabled = true;
                    }


                    btnLoad.IsEnabled = false;


                    views = UsersRepo.getViwerInfo(groupId, 11);
                    grdUsers.ItemsSource = views;
                    loadcomments();


                    txtSystemRef.Text = saleReceipts[0].SystemRefNo;
                    lblReceiptRefNo.Text = " (" + saleReceipts[0].SystemRefNo + ")";


                    if (saleReceipts[0].isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                        txtVoid.RenderTransform = new RotateTransform(-45);
                        //lblStage.Text = "Void";
                    }
                    else if (saleReceipts[0].isReApproved == false)
                    {
                        //lblStage.Text = "Under Re-Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (saleReceipts[0].isApproved == true && saleReceipts[0].stage == "Closed")
                    {
                        //lblStage.Text = "Approved and Closed";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (saleReceipts[0].isApproved == true && saleReceipts[0].saleReceiptStatus.isActive == false && saleReceipts[0].PendingForClosing != true)
                    {
                        //lblStage.Text = "Approved and Closed";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (saleReceipts[0].isApproved == true && saleReceipts[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (saleReceipts[0].isApproved == true)
                    {
                        //lblStage.Text = "Approved";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (saleReceipts[0].isApproved == false)
                    {
                        //lblStage.Text = "Under Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (saleReceipts[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }

                    if (saleReceipts[0].GLPostingDate != null)
                    {
                        datglPostingdate.EditValue = saleReceipts[0].GLPostingDate;
                    }
                    else
                    {
                        datglPostingdate.EditValue = saleReceipts[0].CreationDate;
                    }


                    loadcomments();
                    loadAttachments();


                    //if (saleReceipts[0].createdFromBill == true)
                    //{
                    lookupCompany.IsEnabled = false;
                    lookupDepartment1.IsEnabled = false;
                    lookupCurrency.IsEnabled = false;

                    //}


                    int index = 0;
                    cmbxReceiptType.SelectedIndex = 0;

                    if (saleReceipts[0].CreationDate != null)
                        datCreationDate.EditValue = (DateTime)saleReceipts[0].CreationDate;


                    if (saleReceipts[0].CreditedDate != null)
                        dateCreditedDate.EditValue = (DateTime)saleReceipts[0].CreditedDate;


                    if (saleReceipts[0].InstrumentDate != null)
                        datInstrumentDate.EditValue = (DateTime)saleReceipts[0].InstrumentDate;


                    if (saleReceipts[0].DepositedDate != null)
                        datDepositedDate.EditValue = (DateTime)saleReceipts[0].DepositedDate;


                    // Select Company
                    if (saleReceipts[0].company != null)
                    {

                        var companylist = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                        if (saleReceipts[0].company != null && companylist.Find(x => x.Id == saleReceipts[0].company.Id) == null)
                        {
                            companylist.Add(saleReceipts[0].company);
                            lookupCompany.ItemsSource = null;
                            lookupCompany.ItemsSource = companylist;
                        }

                        index = 0;
                        foreach (var _company in companylist)
                        {
                            if (_company.Id == saleReceipts[0].company.Id)
                            {
                                lookupCompany.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    else
                    {
                        lookupCompany.Text = "Select Company";
                    }


                    //if (saleReceipts[0].department != null)
                    //{
                    //    var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                    //    if (saleReceipts[0].department != null && deptList.Find(x => x.Id == saleReceipts[0].department.Id) == null)
                    //    {

                    //        deptList.Add(saleReceipts[0].department);
                    //        lookupDepartment.ItemsSource = null;
                    //        lookupDepartment.ItemsSource = deptList;
                    //        //lookupCompany.IsEnabled = false;
                    //    }

                    //    index = 0;
                    //    foreach (var _dept in deptList)
                    //    {
                    //        if (_dept.Id == saleReceipts[0].department.Id)
                    //        {
                    //            lookupDepartment.SelectedIndex = index;
                    //            index = 0;
                    //            break;
                    //        }
                    //        index++;
                    //    }
                    //}
                    //else
                    //{
                    //    lookupCompany.Text = "Select Department";
                    //}


                    string deptNames = "";

                    if (saleReceipts[0].department != null)
                    {
                        var departmentlist = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;

                        if (saleReceipts[0].department != null && departmentlist.Find(x => x.Id == saleReceipts[0].deptId) == null)
                        {
                            departmentlist.Add(saleReceipts[0].department);
                            lookupDepartment1.ItemsSource = null;
                            lookupDepartment1.ItemsSource = departmentlist;
                        }
                        deptList.Add(saleReceipts[0].department);
                    }
                    else if (saleReceipts[0].Departments != null && saleReceipts[0].Departments.Count > 0)
                    {
                        deptList = saleReceipts[0].Departments;
                    }

                    List<CustomerCompany> customers = new List<CustomerCompany>();
                    if (deptList.Count > 0)
                    {
                        foreach (var _dept in deptList)
                        {
                            deptNames = deptNames + " | " + _dept.DeptName; if (lookupCustomers.SelectedIndex > -1)
                                foreach (var _customer in _dept.customers)
                                {
                                    if (!customers.Contains(_customer))
                                        customers.Add(_customer);
                                }
                            allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                        }
                    }
                    loademployees();

                    lookupDepartment1.EditValue = deptNames;

                    lookupCustomers.ItemsSource = customers;


                    //Select Payment Reference No
                    var pettyCashRefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
                    if (saleReceipts[0].PettyCashRef != null)
                    {

                        index = 0;
                        foreach (var _ref in pettyCashRefList)
                        {

                            if (_ref.id == saleReceipts[0].PettyCashRefId)
                            {
                                cmbxPettyCashRef.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }




                    if (saleReceipts[0].CreditedDate != null)
                        dateCreditedDate.DateTime = (DateTime)saleReceipts[0].CreditedDate;


                    if (saleReceipts[0].DepositedDate != null)
                        datDepositedDate.DateTime = (DateTime)saleReceipts[0].DepositedDate;


                    if (saleReceipts[0].SystemRefNo != null)
                        txtSystemRef.Text = saleReceipts[0].SystemRefNo;


                    if (saleReceipts[0].ReceiptRefNo != null)
                        txtReceiptRef.Text = saleReceipts[0].ReceiptRefNo;


                    //Select Currency
                    var currencyList = (lookupCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookupCurrency.ItemsSource as List<Currency>;
                    if (saleReceipts[0].Currency != null)
                    {

                        index = 0;
                        foreach (var _currency in currencyList)
                        {

                            if (_currency.Id == saleReceipts[0].Currency.Id)
                            {
                                lookupCurrency.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    txtCollectionAmnt.Text = Convert.ToDouble(saleReceipts.Sum(x => x.CollectionAmount)).ToString();
                    if (saleReceipts[0].saleReceiptStatus != null)
                    {
                        var disAbleStatus = ReceiptStatuses.FirstOrDefault(x => x.Id == saleReceipts[0].saleReceiptStatus.Id);
                        if (disAbleStatus == null)
                        {
                            loadReceiptStatus(saleReceipts[0].saleReceiptStatus);
                        }
                    }
                    //Select Status
                    var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                    if (saleReceipts[0].saleReceiptStatus != null)
                    {

                        checkStatus = saleReceipts[0].saleReceiptStatus;
                        index = 0;
                        foreach (var _status in statusList)
                        {

                            if (_status.id == saleReceipts[0].saleReceiptStatus.Id)
                            {
                                cmbStatus.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Payment Method
                    var collectionMethodList = (lookupCollectionMethod.ItemsSource as List<CollectionMethod>) == null ? new List<CollectionMethod>() : lookupCollectionMethod.ItemsSource as List<CollectionMethod>;
                    if (saleReceipts[0].collectionMethod != null)
                    {

                        index = 0;
                        foreach (var _method in collectionMethodList)
                        {

                            if (_method.Id == saleReceipts[0].collectionMethod.Id)
                            {
                                lookupCollectionMethod.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Bank
                    var bankList = (lookupBanks.ItemsSource as List<Bank>) == null ? new List<Bank>() : lookupBanks.ItemsSource as List<Bank>;
                    if (saleReceipts[0].bank != null)
                    {

                        index = 0;
                        foreach (var _bank in bankList)
                        {

                            if (_bank.Id == saleReceipts[0].BankId)
                            {
                                lookupBanks.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Bank
                    var accountList = (lookupAccounts.ItemsSource as List<Account>) == null ? new List<Account>() : lookupAccounts.ItemsSource as List<Account>;
                    if (saleReceipts[0].AccountId != null)
                    {

                        //index = 0;
                        //foreach (var _account in accountList)
                        //{

                        //    if (_account.Id == saleReceipts[0].AccountId)
                        //    {
                        //        lookupAccounts.SelectedIndex = index;
                        //        index = 0;
                        //        break;
                        //    }
                        //    index++;
                        //}

                        var accntSource = (lookupAccounts.ItemsSource as List<Account>) == null ? new List<Account>() : lookupAccounts.ItemsSource as List<Account>;

                        if (saleReceipts[0].account != null && accntSource.Find(x => x.Id == saleReceipts[0].account.Id) == null)
                        {
                            accntSource.Add(saleReceipts[0].account);
                            lookupAccounts.ItemsSource = null;
                            lookupAccounts.ItemsSource = accntSource;
                        }
                        lookupAccounts.Text = saleReceipts[0].account.AccountNo;
                    }

                    if (saleReceipts[0].InstrumentNo != null)
                        txtInstrumentNo.Text = saleReceipts[0].InstrumentNo;


                    if (saleReceipts[0].InstrumentDate != null)
                        datInstrumentDate.DateTime = (DateTime)saleReceipts[0].InstrumentDate;
                    if (saleReceipts[0].VATBookRefId != 0 && saleReceipts[0].VATBookRefNumber != null)
                    {
                        var vatBookSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                        var term = vatBookSource.Find(x => x.id == saleReceipts[0].VATBookRefId);

                        if (term == null)
                        {
                            vatBookSource.Add(new cmbitem() { name = saleReceipts[0].VATBookRefNumber.VATBookReferenceNo, id = saleReceipts[0].VATBookRefNumber.Id });
                            cmbxVATBookRef.ItemsSource = null;
                            cmbxVATBookRef.ItemsSource = vatBookSource;
                        }
                        cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == saleReceipts[0].VATBookRefId))];
                    }
                    else
                        btnVATBookPost.IsChecked = false;
                    LoadSaleInvoiceData();
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushCredits.IsEnabled = true;
                    btnPushDebits.IsEnabled = true;
                }
                else
                {
                    btnPushCredits.IsEnabled = false;
                    btnPushDebits.IsEnabled = false;
                }
                if (editFlag == false)
                {
                    btnPushDebits.IsChecked = true;
                    btnPushCredits.IsChecked = true;
                }
                else
                {
                    if (saleReceipts[0].isBypassBank == true && saleReceipts[0].coaAccountId != null)
                    {
                        isBypassCOA.IsChecked = true;
                        lookupCOA.Text = saleReceipts[0].ChartofAccount.accountName;
                    }
                    if (saleReceipts[0].transactionHolderId != null)
                    {
                        try
                        {
                            var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                            cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == saleReceipts[0].transactionHolderId))];
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }


        public void loadEmployees()
        {
            cmbTransactionHolder.ItemsSource = null;
            allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(y => y.FirstOrDefault()).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (ERP_BL.Databases.Employee employee in allEmployees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbTransactionHolder.ItemsSource = cmbitems;
            //cmbAppliedBy.ItemsSource = cmbitems;

        }

        private void LoadSaleInvoiceData()
        {
            SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();
            double collectedAmount = 0;
            List<CustomerCreditsSaleReceiptModelView> invoices = new List<CustomerCreditsSaleReceiptModelView>();
            foreach (var _receipt in saleReceipts)
            {
                CustomerCreditsSaleReceiptModelView recpt = new CustomerCreditsSaleReceiptModelView();
                var creditJournalTransactions = _receipt.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = _receipt.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }

                if (_receipt.GLPostingDate != null)
                {
                    datglPostingdate.EditValue = _receipt.GLPostingDate;
                }
                else
                {
                    datglPostingdate.EditValue = _receipt.CreationDate;
                }

                var _invoice = _receipt.saleInvoice;
                collectedAmount = collectedAmount + _receipt.CollectionAmount;

                recpt.Id = _receipt.Id;
                recpt.Date = _invoice.CreationDate;
                if(_invoice.CustomerCredits.Count>0)
                recpt.Customer = _invoice.CustomerCredits.FirstOrDefault(x=>x.SerialNo == _receipt.CustomerCreditSerialNo).CustomerCompany.company.CompanyName;
                recpt.SalesReferenceNo = _invoice.SalesReferenceNo;
                recpt.FinanceRefNo = _invoice.FinanceRefrenceNo;
                recpt.Currency = _invoice.currency.CurrencyName;

                double invoiceAmount = 0;
                if (_invoice.CustomerCredits.Count > 0)
                    invoiceAmount = _invoice.CustomerCredits.FirstOrDefault(x => x.SerialNo == _receipt.CustomerCreditSerialNo).creditAmount;

                recpt.OriginalAmount = invoiceAmount;

                recpt.InvoiceStage = GetInvoiceStatus(_invoice);
                var invoice1 = invoiceRepo.GetSaleInvoice(_invoice.Id);
                var reciepts = invoice1.salesReceipts;
                var result = Math.Round(reciepts.Where(x => x.isVoid != true && x.CustomerCreditSerialNo == _receipt.CustomerCreditSerialNo).Sum(x => x.CollectionAmount), 2);

                recpt.AmountDue = invoiceAmount - result;
                recpt.InvoiceId = _invoice.Id;
                recpt.serialNo = _receipt.CustomerCreditSerialNo;

                recpt.receiptDeductions = _receipt.receiptDeductions;
                if (_receipt.receiptDeductions.Count != 0)
                {
                    recpt.Deductions = _receipt.receiptDeductions.Sum(x => x.Amount);
                }
                recpt.bankChargess = _receipt.bankCharges;
                if (_receipt.bankCharges.Count != 0)
                {
                    recpt.BankCharges = _receipt.bankCharges.Sum(x => x.Amount);
                }

                double VAT = 0;
                recpt.IsDedAdjusted = _receipt.IsAdjustedDedVAT;
                if (_receipt.ReceiptDeductionTaxes.Count != 0)
                {
                    recpt.receiptDedTaxes = _receipt.ReceiptDeductionTaxes;
                    var sum = _receipt.ReceiptDeductionTaxes.Sum(x => x.Amount);
                    VAT = sum;
                    recpt.dedVAT = sum;
                }

                recpt.IsBankAdjusted = _receipt.IsAdjustedBankVAT;
                if (_receipt.ReceiptBankTaxes.Count != 0)
                {
                    recpt.receiptBankTaxes = _receipt.ReceiptBankTaxes;
                    var sum = _receipt.ReceiptBankTaxes.Sum(x => x.Amount);
                    VAT = VAT + sum;
                    recpt.bankVAT = sum;
                }
                recpt.VAT = VAT;

                recpt.CreditedAmount = _receipt.CollectionAmount - recpt.Deductions - recpt.dedVAT;

                recpt.BudgetSystemCostFields = _receipt.BudgetSystemCostFields;
                if (_receipt.transactionHolderId != null)
                {
                    recpt.transactionHolderId = _receipt.transactionHolderId;
                    recpt.holderChangeDate = _receipt.holderChangeDate;
                }
                else
                {
                    recpt.holderChangeDate = DateTime.Now;
                }
                if (_receipt.holderChangeDate != null)
                {
                    var time = DateTime.Now - _receipt.holderChangeDate;
                    txtHolderDays.Text = time.Days.ToString();
                }
                invoices.Add(recpt);
            }

            grdCntrlSalesReceipt.ItemsSource = invoices;
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }
        public void loadcomments()
        {
            try
            {
                if (groupId != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(groupId, TransactionItemType.Sale_Receipt);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private string GetInvoiceStatus(SaleInvoice invoicee)
        {
            if (invoicee.isVoid == true)
            {
                return "Void";
            }
            //else if (invoicee.isReApproved == false)
            //{
            //    return "Under Re-Approval";
            //}
            else if (invoicee.isApproved == true && invoicee.stage == "Closed")
            {
                return "Closed";
            }
            else if (invoicee.isApproved == true && invoicee.saleInvoiceStatus.isActive == false && invoicee.PendingForClosing != true)
            {
                return "Closed";
            }
            else if (invoicee.isApproved == true && invoicee.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else if (invoicee.isApproved == true)
            {
                return "Approved";
            }
            else if (invoicee.isApproved == false)
            {
                return "Under Approval";
            }
            else if (invoicee.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else
            {
                return "No Status";
            }
        }

        private void loadReceiptTypes()
        {
            for (int i = 4; i <= (int)ERP_BL.Enums.ReceiptType.Customer_Credits; i++)
            {
                cmbxReceiptType.Items.Add(((ERP_BL.Enums.ReceiptType)i).ToString());
            }
        }

        private void loadCompanies()
        {
            //EmployeeRepo empRepo = new EmployeeRepo();
            empUser = salesReceiptRepo.GetEmployeeForSaleReceipt(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
            //lookupInterCompany.ItemsSource = empUser.Companies;
        }

        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll().Where(x => x.isVoid != true).ToList();
            lookupCurrency.ItemsSource = currencies;
        }

        private void loadCollectionMethods()
        {
            lookupCollectionMethod.ItemsSource = salesReceiptRepo.GetAllCollectionMethods();
        }

        private void loadReceiptStatus()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipt Statuses") != null)
                ReceiptStatuses = salesReceiptRepo.GetAllSaleReceiptStatus();
            else
                ReceiptStatuses = salesReceiptRepo.GetAllSaleReceiptStatus().Where(x => x.isActive == true).ToList();
            ReceiptStatuses = ReceiptStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(ReceiptStatuses, delegate (SalesReceiptStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbStatus.ItemsSource = cmbitems;
        }

        private void loadReceiptStatus(SalesReceiptStatus _status)
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            ReceiptStatuses.Add(_status);
            Parallel.ForEach(ReceiptStatuses, delegate (SalesReceiptStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbStatus.ItemsSource = cmbitems;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxReceiptType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Receipt Type!");
                    cmbxReceiptType.Focus();
                    return;
                }
                if (lookupCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company!");
                    lookupCompany.Focus();
                    return;
                }
                if (deptList == null || deptList.Count == 0)
                {
                    DXMessageBox.Show("Please select Department!");
                    lookupDepartment1.Focus();
                    return;
                }
                //if (lookUpVendor.SelectedIndex < 0)
                //{
                //    DXMessageBox.Show("Please select Vendor!");
                //    lookUpVendor.Focus();
                //    return;
                //}
                if (datCreationDate.EditValue == null)
                {
                    DXMessageBox.Show("Please select Credited Date!");
                    datCreationDate.Focus();
                    return;
                }
                if (datDepositedDate.EditValue == null)
                {
                    DXMessageBox.Show("Please select Deposited date!");
                    datDepositedDate.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtReceiptRef.Text))
                {
                    DXMessageBox.Show("Please select Receipt Ref #!");
                    txtReceiptRef.Focus();
                    return;
                }
                if (lookupCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency!");
                    lookupCurrency.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtCollectionAmnt.Text) || Convert.ToDouble(txtCollectionAmnt.Text) == 0)
                {
                    DXMessageBox.Show("Please enter Collection amount!");
                    txtCollectionAmnt.Focus();
                    return;
                }
                if (cmbStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Status!");
                    cmbStatus.Focus();
                    return;
                }
                if (lookupCollectionMethod.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Collection method!");
                    lookupCollectionMethod.Focus();
                    return;
                }
                if (lookupBanks.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank!");
                    lookupBanks.Focus();
                    return;
                }
                if (lookupAccounts.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account No.!");
                    lookupAccounts.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtInstrumentNo.Text))
                {
                    DXMessageBox.Show("Please enter Instrument #!");
                    txtInstrumentNo.Focus();
                    return;
                }
                if (datInstrumentDate.EditValue == null)
                {
                    DXMessageBox.Show("Please select Instrument date!");
                    datInstrumentDate.Focus();
                    return;
                }
                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }

                var tempAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                var totalCollection = Math.Round(double.Parse(tempAmount.ToString()), 2);
                if (totalCollection != Convert.ToDouble(txtCollectionAmnt.Text))
                {
                    DXMessageBox.Show("Collection amount is not matching with Total amount!");
                    return;
                }

                List<CustomerCreditsSaleReceiptModelView> selectedItems = new List<CustomerCreditsSaleReceiptModelView>();
                foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
                {
                    if ((_item as CustomerCreditsSaleReceiptModelView).TotalAmount != 0)
                        selectedItems.Add((CustomerCreditsSaleReceiptModelView)_item);
                }

                List<SalesReceipt> receiptList = new List<SalesReceipt>();
                int index = 0;

                if (editFlag == true && saleReceipts.Count > 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null && saleReceipts[0].isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Rceipt is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            stage = TransactionStage.Approved.ToString();
                            isApproved = true;
                            approvalDate = System.DateTime.Now;
                        }
                    }
                }

                foreach (var _item in selectedItems)
                {
                    //comment
                    SalesReceipt receipt = new SalesReceipt();

                    if (editFlag == true)
                    {
                        receipt = saleReceipts[index];
                        index++;

                        if (isApproved != null)
                            receipt.isApproved = isApproved;
                        if (stage != null)
                            receipt.stage = stage;
                        if (approvalDate != null)
                            receipt.ApprovedDate = approvalDate;
                    }
                    else
                    {
                        receipt.isApproved = false;
                        receipt.stage = ERP_BL.Enums.TransactionStage.AwaitingApproval.ToString();
                    }

                    receipt.receiptType = ReceiptType.Customer_Credits;
                    receipt.CreationDate = datCreationDate.DateTime;
                    receipt.GLPostingDate = datglPostingdate.DateTime;
                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        receipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        receipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }

                    //if (editFlag == false)
                    //    receipt.transactionGroupId = intGroupId;

                    receipt.companyId = (lookupCompany.SelectedItem as Company).Id;
                    //payment.dept_Id = (lookupDepartment.SelectedItem as Department).Id;

                    //receipt.deptId = (lookupDepartment.SelectedItem as Department).Id;

                    if (deptList.Count != 0)
                    {
                        receipt.Departments = new List<Department>();
                        foreach (Department _dept in deptList)
                        {
                            if (!receipt.Departments.Contains(_dept))
                            {
                                receipt.Departments.Add(_dept);
                            }
                        }
                        receipt.department = null;
                    }


                    if (cmbxPettyCashRef.SelectedIndex > 0)
                        receipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                    else
                        receipt.PettyCashRefId = null;

                    receipt.CreditedDate = dateCreditedDate.DateTime;
                    receipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                    receipt.DepositedDate = datDepositedDate.DateTime;
                    receipt.SystemRefNo = txtSystemRef.Text;
                    receipt.ReceiptRefNo = txtReceiptRef.Text;
                    receipt.CurrencyId = (lookupCurrency.SelectedItem as Currency).Id;
                    //receipt.CollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                    receipt.StatusId = (cmbStatus.SelectedItem as cmbitem).id;
                    receipt.collectionMethodId = (lookupCollectionMethod.SelectedItem as CollectionMethod).Id;
                    receipt.BankId = (lookupBanks.SelectedItem as Bank).Id;
                    receipt.AccountId = (lookupAccounts.SelectedItem as Account).Id;
                    receipt.InstrumentNo = txtInstrumentNo.Text;
                    receipt.InstrumentDate = datInstrumentDate.DateTime;

                    receipt.saleInvoiceId = _item.InvoiceId;
                    
                    receipt.CollectionAmount = _item.CreditedAmount;
                    receipt.CustomerCreditSerialNo = _item.serialNo;

                    if (editFlag == false)
                    {
                        receipt.user_Id = SYSTEM_STATIC.currentUser.id;
                    }

                    List<PettyCash> pettyCashes = new List<PettyCash>();
                    if (editFlag == true && groupId != 0)
                    {
                        if (btnDeposit.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                SaleReceiptId = receipt.Id,
                                TransactionType = TransactionItemType.Sale_Receipt,
                                debit = _item.TotalAmount,
                                credit = 0,
                                total = _item.TotalAmount - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.saleInvoice.dept_Id,
                                companyId = receipt.saleInvoice.company_Id,
                                currencyId = (lookupCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            receipt.isDeposit = true;
                        }
                        else if (btnPayment.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                SaleReceiptId = receipt.Id,
                                TransactionType = TransactionItemType.Sale_Receipt,
                                debit = 0,
                                credit = _item.TotalAmount,
                                total = 0 - _item.TotalAmount,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.saleInvoice.dept_Id,
                                companyId = receipt.saleInvoice.company_Id,
                                currencyId = (lookupCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            receipt.isDeposit = false;
                        }
                        else
                        {
                            receipt.isDeposit = null;
                        }
                    }
                    else
                    {
                        if (btnDeposit.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                SaleReceiptId = 0,
                                TransactionType = TransactionItemType.Sale_Receipt,
                                debit = _item.TotalAmount,
                                credit = 0,
                                total = _item.TotalAmount - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.saleInvoice?.dept_Id,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            receipt.isDeposit = true;
                        }
                        else if (btnPayment.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                SaleReceiptId = 0,
                                TransactionType = TransactionItemType.Sale_Receipt,
                                debit = 0,
                                credit = _item.TotalAmount,
                                total = 0 - _item.TotalAmount,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.saleInvoice?.dept_Id,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            receipt.isDeposit = false;
                        }
                        else
                        {
                            receipt.isDeposit = null;
                        }

                    }
                    receipt.pettyCashes = new List<PettyCash>();
                    receipt.pettyCashes = pettyCashes;

                  
                    var account = lookupAccounts.SelectedItem as Account;
                    var invoice = salesReceiptRepo.getSIForJournalTransactions((int)receipt.saleInvoiceId);
                    List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                    var selBank = lookupBanks.SelectedItem as Bank;
                    var selAccnt = lookupAccounts.SelectedItem as Account;
                    if (isBypassCOA.IsChecked == true)
                    {
                        receipt.isBypassBank = true;
                        if (lookupCOA.SelectedIndex != -1)
                        {
                            receipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                            if (banktransactionFlag == 0)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    var dbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (lookupCOA.SelectedItem as ChartofAccount).Id);
                                    JournalTransaction bankTransaction = new JournalTransaction();
                                    if (dbtrans == null)
                                    {
                                        bankTransaction.accountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                        bankTransaction.credit = 0;
                                        bankTransaction.userId = receipt.user_Id;
                                        bankTransaction.SaleReceiptId = receipt.Id;
                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                        bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                        bankTransaction.deptId = receipt.department.Id;
                                        bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                    }
                                    else
                                    {
                                        var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                       dbtrans.accountId &&
                                       x.debit == dbtrans.debit &&
                                       x.companyId == dbtrans.companyId &&
                                       x.deptId == dbtrans.deptId
                                       );
                                        if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                        {
                                            bankTransaction.accountId = dbtrans.accountId;
                                            bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            bankTransaction.debit = dbtrans.debit;
                                            bankTransaction.MER = dbtrans.MER;
                                            bankTransaction.credit = dbtrans.credit;
                                            bankTransaction.userId = dbtrans.userId;
                                            bankTransaction.SaleReceiptId = dbtrans.SaleReceiptId;
                                            bankTransaction.transactionRefno = dbtrans.transactionRefno;
                                            bankTransaction.total = dbtrans.total;
                                            bankTransaction.deptId = dbtrans.deptId;
                                            bankTransaction.companyId = dbtrans.companyId;
                                            bankTransaction.currencyId = dbtrans.currencyId;
                                            bankTransaction.isReconciled = dbtrans.isReconciled;
                                            bankTransaction.reconcilationDate = dbtrans.reconcilationDate;
                                            bankTransaction.isReconciled = false;
                                            bankTransaction.reconcilationDate = null;
                                            bankTransaction.ReconcilationId = null;
                                            bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                        }
                                        else
                                        {
                                            bankTransaction.accountId = dbtrans.accountId;
                                            bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            bankTransaction.debit = dbtrans.debit;
                                            bankTransaction.MER = dbtrans.MER;
                                            bankTransaction.credit = dbtrans.credit;
                                            bankTransaction.userId = dbtrans.userId;
                                            bankTransaction.SaleReceiptId = dbtrans.SaleReceiptId;
                                            bankTransaction.transactionRefno = dbtrans.transactionRefno;
                                            bankTransaction.total = dbtrans.total;
                                            bankTransaction.deptId = dbtrans.deptId;
                                            bankTransaction.companyId = dbtrans.companyId;
                                            bankTransaction.currencyId = dbtrans.currencyId;
                                            bankTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                            bankTransaction.reconcilationType = dbTransaction.reconcilationType;
                                            bankTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                            bankTransaction.isReconciled = dbTransaction.isReconciled;
                                        }

                                    }
                                    journalTransactions.Add(bankTransaction);

                                }
                                banktransactionFlag = 1;

                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select Chart of Account");
                            return;
                        }
                    }
                    else
                    {
                        receipt.isBypassBank = false;
                        receipt.coaAccountId = null;
                     
                        if (banktransactionFlag == 0)
                        {
                            if (lookupAccounts.SelectedIndex > -1)
                            {
                                var bankAccount = lookupAccounts.SelectedItem as Account;

                                if(bankAccount!=null)
                                {
                                    if(bankAccount.COA_accountId!=null)
                                    {
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            JournalTransaction bankTransaction = new JournalTransaction();
                                            if (receipt.journalTransactions == null)
                                            {
                                               
                                                bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                                bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(invoice.exchangeRate), 2);
                                                bankTransaction.credit = 0;
                                                bankTransaction.userId = receipt.user_Id;
                                                bankTransaction.SaleReceiptId = receipt.Id;
                                                bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                bankTransaction.deptId = invoice.department.Id;
                                                bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                                bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                            }
                                            else
                                            {



                                                var dbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (lookupAccounts.SelectedItem as Account).COA_accountId);
                                                
                                                var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 (lookupAccounts.SelectedItem as Account).COA_accountId &&
                                                 x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) &&
                                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                                 x.deptId == invoice.department?.Id
                                                 );
                                                if (dbtrans != null && dbtrans.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value))
                                                {
                                                    bankTransaction.accountId = dbtrans.accountId;
                                                    bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                                    bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                    bankTransaction.debit = dbtrans.debit;
                                                    bankTransaction.MER = dbtrans.MER;
                                                    bankTransaction.credit = dbtrans.credit;
                                                    bankTransaction.userId = dbtrans.userId;
                                                    bankTransaction.SaleReceiptId = dbtrans.SaleReceiptId;
                                                    bankTransaction.transactionRefno = dbtrans.transactionRefno;
                                                    bankTransaction.total = dbtrans.total;
                                                    bankTransaction.deptId = dbtrans.deptId;
                                                    bankTransaction.companyId = dbtrans.companyId;
                                                    bankTransaction.currencyId = dbtrans.currencyId;
                                                    bankTransaction.currencyId = dbtrans.currencyId;
                                                    bankTransaction.isReconciled = dbtrans.isReconciled;
                                                    bankTransaction.reconcilationDate = dbtrans.reconcilationDate;

                                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                                    {
                                                        bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                        bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                        bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                        bankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                                        bankTransaction.credit = 0;
                                                        bankTransaction.userId = receipt.user_Id;
                                                        bankTransaction.SaleReceiptId = receipt.Id;
                                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                        bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                        bankTransaction.deptId = invoice.department?.Id;
                                                        bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                                        bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                                        bankTransaction.isReconciled = false;
                                                        bankTransaction.reconcilationDate = null;
                                                        bankTransaction.ReconcilationId = null;
                                                        bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                    }

                                                }
                                                else
                                                {

                                                    bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                    bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                    bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                    bankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                                    bankTransaction.credit = 0;
                                                    bankTransaction.userId = receipt.user_Id;
                                                    bankTransaction.SaleReceiptId = receipt.Id;
                                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                    bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                    bankTransaction.deptId = invoice.department?.Id;
                                                    bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                                    bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;


                                                }
                                            }
                                            journalTransactions.Add(bankTransaction);
                                        }
                                        banktransactionFlag = 1;
                                    }

                                }
                            }
                        }
                    }
                    if (receipt.receiptDeductions.Count != 0)
                    {
                        foreach (var receiptDeduction in receipt.receiptDeductions)
                        {
                            var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                            var debitDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);
                            //var creditDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.credit == receiptDeduction.Amount && x.accountId== deduction.chartofAccountId);

                            if (receiptDeduction.Amount != 0)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    if (deduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deductionTransaction = new JournalTransaction();

                                        if (debitDbtrans == null)
                                        {
                                            deductionTransaction.accountId = deduction.chartofAccountId;
                                            deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                            deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            deductionTransaction.debit = receiptDeduction.Amount;
                                            deductionTransaction.credit = 0;
                                            deductionTransaction.userId = receipt.user_Id;
                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                            deductionTransaction.SaleReceiptId = receipt.Id;
                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                            deductionTransaction.deptId = invoice.department?.Id;
                                            deductionTransaction.total = receiptDeduction.Amount - 0;
                                            deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                        }
                                        else
                                        {
                                            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 debitDbtrans.accountId &&
                                                 x.debit == debitDbtrans.debit &&
                                                 x.companyId == debitDbtrans.companyId &&
                                                 x.deptId == debitDbtrans.deptId
                                                 );
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.isReconciled = false;
                                                deductionTransaction.reconcilationDate = null;
                                                deductionTransaction.ReconcilationId = null;
                                                deductionTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            }
                                            else
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                deductionTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                deductionTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                deductionTransaction.isReconciled = dbTransaction.isReconciled;
                                            }

                                        }
                                        journalTransactions.Add(deductionTransaction);
                                    }
                                }
                                //if (btnPushCredits.IsChecked == true)
                                //{
                                //    JournalTransaction deducBankTransaction = new JournalTransaction();
                                //    if (creditDbtrans == null)
                                //    {
                                //        deducBankTransaction.accountId = selAccnt.COA_accountId;
                                //        deducBankTransaction.coaTransactionsType = coaTransactionsType.receipt;
                                //        deducBankTransaction.creationDate = receipt.GLPostingDate;
                                //        deducBankTransaction.debit = 0;
                                //        deducBankTransaction.credit = receiptDeduction.Amount;
                                //        //MER = 1,
                                //        deducBankTransaction.userId = receipt.user_Id;
                                //        deducBankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                //        deducBankTransaction.receiptId = receipt.Id;
                                //        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                //        deducBankTransaction.deptId = receipt.department.Id;
                                //        deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                //        deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                //        deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                //    }
                                //    else
                                //    {
                                //        deducBankTransaction = creditDbtrans;
                                //    }

                                //    journalTransactions.Add(deducBankTransaction);
                                //}

                            }
                        }

                    }
                    if (receipt.bankCharges != null && receipt.bankCharges.Count != 0)
                    {
                        foreach (var receiptDeduction in receipt.bankCharges)
                        {
                            var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                            var debitDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);

                            var creditDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.credit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);

                            if (receiptDeduction.Amount != 0)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    if (deduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deductionTransaction = new JournalTransaction();

                                        if (debitDbtrans == null)
                                        {
                                            deductionTransaction.accountId = deduction.chartofAccountId;
                                            deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                            deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            deductionTransaction.debit = receiptDeduction.Amount;
                                            deductionTransaction.credit = 0;
                                            deductionTransaction.userId = receipt.user_Id;
                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                            deductionTransaction.SaleReceiptId = receipt.Id;
                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                            deductionTransaction.deptId = invoice.department?.Id;
                                            deductionTransaction.total = receiptDeduction.Amount - 0;
                                            deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                        }
                                        else
                                        {
                                            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 debitDbtrans.accountId &&
                                                 x.debit == debitDbtrans.debit &&
                                                 x.companyId == debitDbtrans.companyId &&
                                                 x.deptId == debitDbtrans.deptId
                                                 );
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.isReconciled = false;
                                                deductionTransaction.reconcilationDate = null;
                                                deductionTransaction.ReconcilationId = null;
                                                deductionTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            }
                                            else
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                deductionTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                deductionTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                deductionTransaction.isReconciled = dbTransaction.isReconciled;
                                            }

                                        }
                                        journalTransactions.Add(deductionTransaction);
                                    }
                                }
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction deducBankTransaction = new JournalTransaction();
                                    if (creditDbtrans == null)
                                    {
                                        deducBankTransaction.accountId = selAccnt.COA_accountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        deducBankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = receiptDeduction.Amount;
                                        //MER = 1,
                                        deducBankTransaction.userId = receipt.user_Id;
                                        deducBankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                        deducBankTransaction.SaleReceiptId = receipt.Id;
                                        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                        deducBankTransaction.deptId = invoice.department?.Id;
                                        deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                        deducBankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        deducBankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                    }
                                    else
                                    {
                                        creditDbtrans.creationDate = (DateTime)datglPostingdate.EditValue;

                                        if (creditDbtrans.creationDate != (DateTime)datDepositedDate.EditValue)
                                        {
                                            creditDbtrans.isReconciled = false;
                                            creditDbtrans.reconcilationDate = null;
                                            creditDbtrans.ReconcilationId = null;
                                            creditDbtrans.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            deducBankTransaction = creditDbtrans;
                                        }
                                        else
                                        {
                                            deducBankTransaction = creditDbtrans;

                                        }
                                    }

                                    journalTransactions.Add(deducBankTransaction);
                                }

                            }
                        }

                    }
                    if (receipt.ReceiptBankTaxes != null && receipt.ReceiptBankTaxes.Count > 0)
                    {
                        foreach (var receiptBankTax in receipt.ReceiptBankTaxes)
                        {
                            var vatTax = taxRepo.getTaxtById((int)receiptBankTax.taxNameId);
                            var debitDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == receiptBankTax.Amount && x.accountId == vatTax.COA_Id);

                            var creditDbtrans = receipt.journalTransactions.FirstOrDefault(x => x.credit == receiptBankTax.Amount && x.accountId == vatTax.COA_Id);

                            if (receiptBankTax.Amount != 0)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    if (vatTax.COA_Id != null)
                                    {
                                        JournalTransaction deductionTransaction = new JournalTransaction();

                                        if (debitDbtrans == null)
                                        {
                                            deductionTransaction.accountId = vatTax.COA_Id;
                                            deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                            deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            deductionTransaction.debit = receiptBankTax.Amount;
                                            deductionTransaction.credit = 0;
                                            deductionTransaction.userId = receipt.user_Id;
                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                            deductionTransaction.SaleReceiptId = receipt.Id;
                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                            deductionTransaction.deptId = invoice.department?.Id;
                                            deductionTransaction.total = receiptBankTax.Amount - 0;
                                            deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                        }
                                        else
                                        {
                                            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                debitDbtrans.accountId &&
                                                x.debit == debitDbtrans.debit &&
                                                x.companyId == debitDbtrans.companyId &&
                                                x.deptId == debitDbtrans.deptId
                                                );
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.isReconciled = false;
                                                deductionTransaction.reconcilationDate = null;
                                                deductionTransaction.ReconcilationId = null;
                                                deductionTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            }
                                            else
                                            {
                                                deductionTransaction.accountId = debitDbtrans.accountId;
                                                deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                                deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                deductionTransaction.debit = debitDbtrans.debit;
                                                deductionTransaction.credit = debitDbtrans.credit;
                                                deductionTransaction.userId = debitDbtrans.userId;
                                                deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                                deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                                deductionTransaction.MER = debitDbtrans.MER;
                                                deductionTransaction.deptId = debitDbtrans.deptId;
                                                deductionTransaction.total = debitDbtrans.total;
                                                deductionTransaction.companyId = debitDbtrans.companyId;
                                                deductionTransaction.currencyId = debitDbtrans.currencyId;
                                                deductionTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                deductionTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                deductionTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                deductionTransaction.isReconciled = dbTransaction.isReconciled;
                                            }

                                        }
                                        journalTransactions.Add(deductionTransaction);
                                    }
                                }
                                if (btnPushCredits.IsChecked == true)
                                {
                                    JournalTransaction deducBankTransaction = new JournalTransaction();
                                    if (creditDbtrans == null)
                                    {
                                        deducBankTransaction.accountId = selAccnt.COA_accountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        deducBankTransaction.creationDate = receipt.GLPostingDate;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = receiptBankTax.Amount;
                                        //MER = 1,
                                        deducBankTransaction.userId = receipt.user_Id;
                                        deducBankTransaction.MER = Math.Round(Convert.ToDouble(receipt.saleInvoice.exchangeRate), 2);
                                        deducBankTransaction.SaleReceiptId = receipt.Id;
                                        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                        deducBankTransaction.deptId = invoice.department?.Id;
                                        deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                        deducBankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        deducBankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                    }
                                    else
                                    {
                                        creditDbtrans.creationDate = (DateTime)datglPostingdate.EditValue;

                                        if (creditDbtrans.creationDate != (DateTime)datDepositedDate.EditValue)
                                        {
                                            creditDbtrans.isReconciled = false;
                                            creditDbtrans.reconcilationDate = null;
                                            creditDbtrans.ReconcilationId = null;
                                            creditDbtrans.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            deducBankTransaction = creditDbtrans;
                                        }
                                        else
                                        {
                                            deducBankTransaction = creditDbtrans;
                                        }
                                    }
                                    journalTransactions.Add(deducBankTransaction);
                                }

                            }
                        }

                    }

                    if (invoice.department != null && invoice.department.chartofAccountId != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if(receipt.journalTransactions==null)
                            {
                                JournalTransaction receivableTransaction = new JournalTransaction();
                                receivableTransaction.accountId = invoice.department.chartofAccountId;
                                receivableTransaction.deptId = invoice.department.Id;

                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                receivableTransaction.debit = 0;
                                receivableTransaction.MER = Math.Round(Convert.ToDouble(invoice.exchangeRate), 2);

                                receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                //receivableTransaction.MER = 1;
                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                receivableTransaction.userId = receipt.user_Id;
                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                receivableTransaction.SaleReceiptId = receipt.Id;
                                receivableTransaction.deptId = invoice.department.Id;
                                receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                receivableTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                journalTransactions.Add(receivableTransaction);
                            }
                            else
                            {
                                var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                              invoice.department.chartofAccountId &&
                                              x.credit == Math.Round(_item.TotalAmount, 2) &&
                                              x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                              x.deptId == invoice.department.Id
                                              );

                                if (dbTransaction == null)
                                {
                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                    receivableTransaction.accountId = invoice.department.chartofAccountId;
                                    receivableTransaction.deptId = invoice.department.Id;

                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    receivableTransaction.debit = 0;
                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(invoice.exchangeRate), 2);

                                    receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                    //receivableTransaction.MER = 1;
                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    receivableTransaction.userId = receipt.user_Id;
                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                    receivableTransaction.SaleReceiptId = receipt.Id;
                                    receivableTransaction.deptId = invoice.department.Id;
                                    receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                    receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    receivableTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                    journalTransactions.Add(receivableTransaction);
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction receivableTransaction = new JournalTransaction();
                                        receivableTransaction.accountId = invoice.department.chartofAccountId;
                                        receivableTransaction.deptId = invoice.department.Id;

                                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        receivableTransaction.debit = 0;
                                        receivableTransaction.MER = Math.Round(Convert.ToDouble(invoice.exchangeRate), 2);

                                        receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                        //receivableTransaction.MER = 1;
                                        //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                        receivableTransaction.userId = receipt.user_Id;
                                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                        receivableTransaction.SaleReceiptId = receipt.Id;
                                        receivableTransaction.deptId = invoice.department.Id;
                                        receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                        receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        receivableTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                        receivableTransaction.isReconciled = false;
                                        receivableTransaction.reconcilationDate = null;
                                        receivableTransaction.ReconcilationId = null;
                                        receivableTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                        journalTransactions.Add(receivableTransaction);
                                    }
                                    else
                                    {
                                        JournalTransaction receivableTransaction = new JournalTransaction();
                                        receivableTransaction.accountId = invoice.department.chartofAccountId;
                                        receivableTransaction.deptId = invoice.department.Id;

                                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        receivableTransaction.debit = 0;
                                        receivableTransaction.MER = Math.Round(Convert.ToDouble(invoice.exchangeRate), 2);

                                        receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                        //receivableTransaction.MER = 1;
                                        //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                        receivableTransaction.userId = receipt.user_Id;
                                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                        receivableTransaction.SaleReceiptId = receipt.Id;
                                        receivableTransaction.deptId = invoice.department.Id;
                                        receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                        receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        receivableTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                        receivableTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                        receivableTransaction.reconcilationType = dbTransaction.reconcilationType;
                                        receivableTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                        receivableTransaction.isReconciled = dbTransaction.isReconciled;
                                        journalTransactions.Add(receivableTransaction);

                                    }
                                }
                            }
                           

                        }
                    }
                    receipt.journalTransactions = journalTransactions;
                    List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
                    if (editFlag == true && groupId != 0)
                    {
                        if (cmbxVATBookRef.SelectedIndex > -1)
                        {
                            receipt.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                        }
                        if (btnVATBookPost.IsChecked == true)
                        {
                            foreach (var tax in receipt.bankCharges)
                            {
                                Company company = null;
                                Department department = null;
                                double exchangeRate = 0;

                                if (receipt.payment.transactionType == PaymentTransactionType.Admin_Bills)
                                {
                                    company = receipt.payment.adminBill.company;
                                    department = receipt.payment.adminBill.department;
                                    exchangeRate = receipt.payment.adminBill.MER;

                                }
                                else
                                if (receipt.payment.transactionType == PaymentTransactionType.Loans_Advances)
                                {
                                    company = receipt.payment.loansAdvance.company;
                                    department = receipt.payment.loansAdvance.department;
                                    exchangeRate = receipt.payment.loansAdvance.MER;

                                }
                                else
                                if (receipt.payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                                {
                                    company = receipt.payment.purchaseInvoice.company;
                                    department = receipt.payment.purchaseInvoice.department;
                                    exchangeRate = receipt.payment.purchaseInvoice.exchangeRate;

                                }
                                //else
                                //if (receipt.payment.transactionType == PaymentTransactionType.Target_Reward)
                                //{
                                //    company = receipt.payment.tar.company;
                                //    department = receipt.payment.adminBill.department;

                                //}
                                else
                                if (receipt.payment.transactionType == PaymentTransactionType.Vendor_Bills)
                                {
                                    company = receipt.payment.Bill.company;
                                    department = receipt.payment.Bill.department;
                                    exchangeRate = Convert.ToDouble(receipt.payment.Bill.ExchangeRate);

                                }
                                vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                {
                                    CreationDate = datCreationDate.DateTime,
                                    saleReceiptId = receipt.Id,
                                    TransactionType = TransactionItemType.Sale_Receipt,
                                    debit = tax.Amount,
                                    credit = 0,
                                    total = tax.Amount - 0,
                                    FinanceRefNo = txtReceiptRef.Text,
                                    SystemRefNo = txtSystemRef.Text,
                                    MER = Math.Round(exchangeRate, 2),
                                    deptId = department.Id,
                                    companyId = company.Id,
                                    currencyId = (lookupCurrency.SelectedItem as Currency).Id,
                                    VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                });
                            }
                            receipt.VATBooks = vatBooks;
                        }
                    }

                    if (editFlag == true)
                            saleReceipts[saleReceipts.FindIndex(x => x.Id == receipt.Id)] = receipt;
                        else
                            receiptList.Add(receipt);
                    
                }
                if (editFlag == true)
                {
                    salesReceiptRepo.UpdateCustomerCreditReceipt(saleReceipts);

                    var selectedStatus = cmbStatus.SelectedItem as cmbitem;
                    if (checkStatus.Id != selectedStatus.id)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Sale Receipt has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            var companyy = lookupCompany.SelectedItem as Company;
                            //var dept = lookupDepartment.SelectedItem as Department;
                            if (deptList != null && companyy?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                List<User> usersList = new List<User>();
                                foreach (var _dept in deptList)
                                {
                                    usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, companyy.Id));

                                }
                                var userss = usersList.Distinct().ToList();

                                winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Sale_Receipt);
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
                        string newStat = selectedStatus.name;
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Receipt having System Reference: " + txtSystemRef.Text + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(groupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, groupId, (int)TransactionItemType.Sale_Receipt, "Status Changed from (" + checkStatus.Status + ") to (" + selectedStatus.name + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, groupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);



                    DXMessageBox.Show("Successfully Updated!");

                }
                else if (editFlag == false)
                {
                    salesReceiptRepo.AddCustomerCreditReceipt(receiptList);
                    DXMessageBox.Show("Successfully Added!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 11, "Viewed details of Sale Receipt");
            }
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
                if (saleReceipts[0].transactionGroupId > 0)
                {
                    if (deptList != null && deptList.Count > 0)
                    {
                        List<User> usersList = new List<User>();
                        foreach (var _dept in deptList)
                        {
                            usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                        }
                        var userss = usersList.Distinct().ToList();

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, comment, TransactionItemType.Sale_Receipt);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (saleReceipts[0].transactionGroupId > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && groupId != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Sale_Receipt, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (saleReceipts[0].transactionGroupId == 0)
                        {
                            DXMessageBox.Show("Kindly save Sale Receipt first to add a comment!");
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
                if (deptList != null && deptList.Count > 0)
                {
                    List<User> usersList = new List<User>();
                    foreach (var _dept in deptList)
                    {
                        usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                    }
                    var userss = usersList.Distinct().ToList();

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.Sale_Receipt);
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

                if (frmInputBox.commentAdded == true && groupId != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (groupId == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Receipt first to add a comment!");
                }
            }
            loadcomments();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
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

        private void btnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (editFlag == true && groupId > 0)
                {

                    SalesReceiptRepo repo = new SalesReceiptRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    saleReceipts = repo.getReceiptsByGroupId(groupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (saleReceipts != null && saleReceipts.Count > 0)
                    {
                        if (saleReceipts[0].isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Sale Receipts are Approved, Do you want to UnApprove these Receipts?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < saleReceipts.Count; i++)
                                    {
                                        saleReceipts[i].isApproved = false;
                                        saleReceipts[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, saleReceipts[0].transactionGroupId, 11, frmInputBox.comment);

                                    repo.ApproveReceipts(saleReceipts);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Sale Receipt has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (deptList != null && deptList.Count > 0)
                                        {
                                            List<User> usersList = new List<User>();
                                            foreach (var _dept in deptList)
                                            {
                                                usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                                            }
                                            var userss = usersList.Distinct().ToList();

                                            winTagUsers win = new winTagUsers(usersList, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            for (int i = 0; i < saleReceipts.Count; i++)
                                            {
                                                if (tagUsers.Count > 0)
                                                {
                                                    if (saleReceipts[i].transactionHolderId != tagUsers[0].employeeId)
                                                    {
                                                        saleReceipts[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    saleReceipts[i].transactionHolderId = tagUsers[0].employeeId;
                                                    repo.updateSalesRecpt(saleReceipts[i]);

                                                }
                                            }

                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (saleReceipts[0].Currency != null)
                                    {
                                        symbolCurr = saleReceipts[0].Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Sale Receipt (Amount OC) having value: " + saleReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Sale Receipt UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + saleReceipts[0].ReceiptRefNo, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + saleReceipts[0].ReceiptRefNo, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }


                                    MessageBox.Show("Sale Receipts are UnApproved (" + saleReceipts[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Receipts is UnApproved (" + saleReceipts[0].transactionGroupId + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Receipts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Receipts Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (saleReceipts[0].isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Sale Receipts are Pending for Approval, Do you want to Approve these Receipts?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < saleReceipts.Count; i++)
                                    {
                                        saleReceipts[i].isApproved = true;
                                        saleReceipts[i].stage = TransactionStage.Approved.ToString();
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, saleReceipts[0].transactionGroupId, 17, frmInputBox.comment);

                                    repo.ApproveReceipts(saleReceipts);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Sale Receipt has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (deptList != null && deptList.Count > 0)
                                        {
                                            List<User> usersList = new List<User>();
                                            foreach (var _dept in deptList)
                                            {
                                                usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                                            }
                                            var userss = usersList.Distinct().ToList();

                                            winTagUsers win = new winTagUsers(usersList, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            for (int i = 0; i < saleReceipts.Count; i++)
                                            {
                                                if (tagUsers.Count > 0)
                                                {
                                                    if (saleReceipts[i].transactionHolderId != tagUsers[0].employeeId)
                                                    {
                                                        saleReceipts[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    saleReceipts[i].transactionHolderId = tagUsers[0].employeeId;
                                                    repo.updateSalesRecpt(saleReceipts[i]);

                                                }
                                            }

                                        }

                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (saleReceipts[0].Currency != null)
                                    {
                                        symbolCurr = saleReceipts[0].Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Sale Receipt (Amount OC) having value: " + saleReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Sale Receipt Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + saleReceipts[0].ReceiptRefNo, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + saleReceipts[0].ReceiptRefNo, saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Sale Receipts are Approved (" + saleReceipts[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Receipts is Approved (" + saleReceipts[0].transactionGroupId + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Receipts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Receipts Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (saleReceipts[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null) ? true : false)
                            {
                                for (int i = 0; i < saleReceipts.Count; i++)
                                {
                                    saleReceipts[i].isReApproved = true;
                                    saleReceipts[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, saleReceipts[0].transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                repo.ApproveReceipts(saleReceipts);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Sale Receipts are Approved (" + saleReceipts[0].transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + saleReceipts[0].transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Sale Receipts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Receipt Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
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

        public ucFrmCustomerCreditReceipt(SalesReceiptStatus status)
        {
            statusChanged = status;
            //InitializeComponent();
            //receipt_register_win.Closing += ReceiptRegister_Window_Closing;
            //grdSaleReceiptList.Columns["SerialNo"].Visible = false;
        }

        private void btnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            try
            {

                if (editFlag == true && groupId > 0)
                {
                    SalesReceiptRepo repo = new SalesReceiptRepo();
                    var SR = repo.GetSaleReceipt(groupId);

                    if (SR != null)
                    {
                        var previous_status = SR.saleReceiptStatus.Status;
                        saleReceipts = repo.getReceiptsByGroupId(SR.transactionGroupId);

                        if (saleReceipts.Count > 0 && saleReceipts[0].isApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null))
                                {
                                    saleReceipts.ForEach(z => z.isApproved = true);
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
                        else if (saleReceipts.Count > 0 && saleReceipts[0].isReApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under ReApproval, Do you want to ReApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null))
                                {
                                    saleReceipts.ForEach(z => z.isReApproved = true);
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

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null) ? true : false)
                        {
                            statusChanged = null;
                            ucFrmDirectClose ucFrmDirectClose = new ucFrmDirectClose();
                            if (SR.saleReceiptStatus != null)
                            {
                                ucFrmDirectClose.statusName.Text = SR.saleReceiptStatus.Status;

                                var brush = new BrushConverter();
                                ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(SR.saleReceiptStatus.backcolor);
                            }

                            ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                            ucFrmDirectClose.frmFlag = true;
                            ucFrmDirectClose.type = "CustomerCredits";

                            ucFrmDirectClose.directCloseWin.Width = 450;
                            ucFrmDirectClose.directCloseWin.Height = 650;
                            ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                            ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            ucFrmDirectClose.directCloseWin.ShowDialog();
                            double totalValue = 0;
                            if (statusChanged != null)
                            {
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
                                List<User> ccUsers = new List<User>();
                                List<User> tagUsersRecommendation = new List<User>();
                                List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Sale Receipt has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                List<User> usrs = new List<User>();

                                if (res == MessageBoxResult.Yes)
                                {
                                    if (deptList != null && deptList.Count > 0)
                                    {
                                        List<User> usersList = new List<User>();
                                        foreach (var _dept in deptList)
                                        {
                                            usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                                        }
                                        var userss = usersList.Distinct().ToList();

                                        winTagUsers win = new winTagUsers(userss, SR.transactionGroupId, TransactionItemType.Sale_Receipt);
                                        //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                        win.ShowDialog();
                                        tagUsers = win.tagUsers;
                                        ccUsers = win.ccUsers;
                                        tagUsersRecommendation = win.tagRecommendationUsers;
                                        ccUsersRecommendation = win.ccRecommendationUsers;
                                        usrs = win.tagUsers;

                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();

                                    }
                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null)
                                {
                                    foreach (var _receipt in saleReceipts)
                                    {
                                        _receipt.PendingForClosing = false;
                                        _receipt.stage = TransactionStage.Closed.ToString();
                                        _receipt.saleReceiptStatus = statusChanged;
                                        _receipt.LastStatusChangeDate = System.DateTime.Now;
                                        _receipt.ClosingDate = System.DateTime.Now;
                                        totalValue = totalValue + _receipt.CollectionAmount;
                                        if (usrs.Count > 0)
                                        {
                                            if (_receipt.transactionHolderId != usrs[0].employeeId)
                                            {
                                                _receipt.holderChangeDate = DateTime.Now;
                                            }
                                            _receipt.transactionHolderId = usrs[0].employeeId;
                                        }
                                        repo.updateSalesReceiptForDirectClose(_receipt);
                                    }

                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                    if (statusChanged != null)
                                    {
                                        //Adding signature (comment)



                                        string oldStat = previous_status;
                                        string newStat = statusChanged.Status;
                                        CommentLog comment = new CommentLog()
                                        {
                                            Comment = "Status of Receipt having Collection Ammount: " + totalValue.ToString() + " (" + SR.Currency.Abbrivation.ToString() + ")" + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                            Timestamp = DateTime.Now,
                                            Subject = "Status Changed using Direct Close",
                                            TaggedList = tagUsers,
                                            CCUsersList = ccUsers,
                                            TaggedRecomenndedList = tagUsersRecommendation,
                                            CCRecomenndedList = ccUsersRecommendation
                                        };
                                        if (saleReceipts.Count != 0)
                                        {
                                            procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                                }
                                            }
                                        }
                                    }
                                    //Load_Receipts();
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                {
                                    totalValue = 0;
                                    foreach (var _receipt in saleReceipts)
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
                                        if (usrs.Count > 0)
                                        {
                                            if (_receipt.transactionHolderId != usrs[0].employeeId)
                                            {
                                                _receipt.holderChangeDate = DateTime.Now;
                                            }
                                            _receipt.transactionHolderId = usrs[0].employeeId;
                                        }
                                        repo.updateSalesReceiptForDirectClose(_receipt);
                                    }
                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                    if (statusChanged != null)
                                    {
                                        string oldStat = previous_status;
                                        string newStat = statusChanged.Status;
                                        CommentLog comment = new CommentLog()
                                        {
                                            Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                            Timestamp = DateTime.Now,
                                            Subject = "Status Changed",
                                            TaggedList = tagUsers,
                                            CCUsersList = ccUsers,
                                            TaggedRecomenndedList = tagUsersRecommendation,
                                            CCRecomenndedList = ccUsersRecommendation
                                        };
                                        if (saleReceipts.Count != 0)
                                        {
                                            procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                                }
                                            }
                                        }
                                    }

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                                {
                                    totalValue = 0;

                                    foreach (var _receipt in saleReceipts)
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
                                        if (usrs.Count > 0)
                                        {
                                            if (_receipt.transactionHolderId != usrs[0].employeeId)
                                            {
                                                _receipt.holderChangeDate = DateTime.Now;
                                            }
                                            _receipt.transactionHolderId = usrs[0].employeeId;
                                        }
                                        repo.updateSalesReceiptForDirectClose(_receipt);
                                    }
                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                    if (statusChanged != null)
                                    {
                                        string oldStat = previous_status;
                                        string newStat = statusChanged.Status;
                                        CommentLog comment = new CommentLog()
                                        {
                                            Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                            Timestamp = DateTime.Now,
                                            Subject = "Status Changed",
                                            TaggedList = tagUsers,
                                            CCUsersList = ccUsers,
                                            TaggedRecomenndedList = tagUsersRecommendation,
                                            CCRecomenndedList = ccUsersRecommendation
                                        };
                                        if (saleReceipts.Count != 0)
                                        {
                                            procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    totalValue = 0;

                                    foreach (var _receipt in saleReceipts)
                                    {
                                        _receipt.saleReceiptStatus = statusChanged;
                                        _receipt.stage = TransactionStage.AwaitingFirstReview.ToString();
                                        _receipt.PendingForClosing = true;
                                        _receipt.LastStatusChangeDate = System.DateTime.Now;
                                        _receipt.ClosingDate = System.DateTime.Now;
                                        usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                        totalValue = totalValue + _receipt.CollectionAmount;
                                        if (usrs.Count > 0)
                                        {
                                            if (_receipt.transactionHolderId != usrs[0].employeeId)
                                            {
                                                _receipt.holderChangeDate = DateTime.Now;
                                            }
                                            _receipt.transactionHolderId = usrs[0].employeeId;
                                        }
                                        repo.updateSalesReceiptForDirectClose(_receipt);

                                    }

                                    if (statusChanged != null)
                                    {
                                        string oldStat = previous_status;
                                        string newStat = statusChanged.Status;
                                        CommentLog comment = new CommentLog()
                                        {
                                            Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                            Timestamp = DateTime.Now,
                                            Subject = "Status Changed",
                                            TaggedList = tagUsers,
                                            CCUsersList = ccUsers,
                                            TaggedRecomenndedList = tagUsersRecommendation,
                                            CCRecomenndedList = ccUsersRecommendation
                                        };
                                        if (saleReceipts.Count != 0)
                                        {
                                            procurementRepo.Add(saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                                }
                                            }
                                        }
                                    }
                                }
                                DXMessageBox.Show("Sale Receipt status changed to InActive (" + statusChanged.Status + ")");

                                var myWindow = Window.GetWindow(this);
                                myWindow.Close();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("You are not Allowed to Close Sale Receipt Directly.");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                if (deptList != null && deptList.Count > 0)
                {
                    List<User> usersList = new List<User>();
                    foreach (var _dept in deptList)
                    {
                        usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                    }
                    var userss = usersList.Distinct().ToList();

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.Sale_Receipt);
                    inputBox.ShowDialog();

                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (groupId != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && groupId != 0)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Sale_Receipt, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }
                        if (frmInputBox.Comment.TaggedList.Count > 0)
                        {
                            var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                            var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                            cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                        }

                        MessageBox.Show("Comment Added!");
                        loadcomments();
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (groupId == 0)
                    {
                        DXMessageBox.Show("Kindly save Payment first to add a comment!");
                    }

                }
            }
            else
            {
                DXMessageBox.Show("You have to save the Sale Receipt first!");
            }
        }

        private void btnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void btnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlSalesReceipt.SelectedItem != null)
            {
                var idd = (grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView).Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.Sale_Receipt);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Select any Sale Receipt first!");
            }
        }

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                List<SalesReceipt> salesReceiptss = new List<SalesReceipt>();
                salesReceiptss = salesReceiptRepo.getReceiptsByGroupId(groupId);

                if (salesReceiptss[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleReceipt") != null))
                {
                    if (DXMessageBox.Show("This Receipt is currently in the list of Void Sale Receipts! Do you want to remove it from Void?", "Remove Void Sale Receipt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (var _receipt in salesReceiptss)
                        {
                            _receipt.isVoid = false;
                            salesReceiptRepo.setSaleReceipttoVoid(_receipt.Id, false);
                        }
                        grdVoid.Visibility = Visibility.Collapsed;

                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("Sale Receipt has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (salesReceiptss[0].department != null && salesReceiptss[0].department.Id != 0 && salesReceiptss[0].company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                for (int i = 0; i < salesReceiptss.Count; i++)
                                {
                                    if (tagUsers.Count > 0)
                                    {
                                        if (salesReceiptss[i].transactionHolderId != tagUsers[0].employeeId)
                                        {
                                            salesReceiptss[i].holderChangeDate = DateTime.Now;
                                        }
                                        salesReceiptss[i].transactionHolderId = tagUsers[0].employeeId;
                                        salesReceiptRepo.UpdateSaleReceipt(salesReceiptss[i]);
                                    }
                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                        }
                        string symbolCurr = "";
                        if (salesReceiptss[0].Currency != null)
                        {
                            symbolCurr = salesReceiptss[0].Currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Sale Receipt (Amount OC) having value: " + salesReceiptss[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "Sale Receipt UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceiptss[0].ReceiptRefNo, salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceiptss[0].ReceiptRefNo, salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleReceipt") != null)
                {
                    if (DXMessageBox.Show("This Receipt is not currently in the list of Void Sale Receipts! Do you want to move it to Void SaleReceipts?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var _receipt in salesReceiptss)
                        {
                            _receipt.isVoid = true;
                            salesReceiptRepo.setSaleReceipttoVoid(_receipt.Id, true);
                            salesReceiptRepo.RemoveReceiptSystemCost(_receipt.Id, true);
                        }
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

                        var res1 = MessageBox.Show("Sale Receipt has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (salesReceiptss[0].department != null && salesReceiptss[0].department.Id != 0 && salesReceiptss[0].company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                for (int i = 0; i < salesReceiptss.Count; i++)
                                {
                                    if (tagUsers.Count > 0)
                                    {
                                        if (salesReceiptss[i].transactionHolderId != tagUsers[0].employeeId)
                                        {
                                            salesReceiptss[i].holderChangeDate = DateTime.Now;
                                        }
                                        salesReceiptss[i].transactionHolderId = tagUsers[0].employeeId;
                                        salesReceiptRepo.UpdateSaleReceipt(salesReceiptss[i]);
                                    }
                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }

                        }

                        string symbolCurr = "";
                        if (salesReceiptss[0].Currency != null)
                        {
                            symbolCurr = salesReceiptss[0].Currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Sale Receipt (Amount OC) having value: " + salesReceiptss[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                            Timestamp = DateTime.Now,
                            Subject = "Sale Receipt Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceiptss[0].ReceiptRefNo, salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceiptss[0].ReceiptRefNo, salesReceiptss[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
            }
        }

        private void btnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if (editFlag == true)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSRAttachmentCategories();
                }
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void btnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                if (grdCntrlSalesReceipt.SelectedItem != null)
                {
                    var idd = (grdCntrlSalesReceipt.SelectedItem as SaleReceipts).InvoiceNo;
                    if (idd != 0)
                    {
                        SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
                        var saleInvoice = saleInvoiceRepo.get(idd);
                        List<TreeItem> otherAttachments = new List<TreeItem>();
                        var saleOrder = saleInvoice.SaleOrder;
                        List<TreeItem> atachments = SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Sale_Order);
                        if (saleOrder.offer != null)
                        {
                            if (saleOrder.offer_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                            if (saleOrder.offer.inquiry_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                        }

                        if (saleOrder.SaleInvoices.Count != 0)
                        {
                            foreach (var invoice in saleOrder.SaleInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                                if (invoice.salesReceipts.Count != 0)
                                    foreach (var receipt in invoice.salesReceipts)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                    }
                            }
                        }
                        if (saleOrder.PurchaseOrders.Count != 0)
                        {
                            foreach (var pO in saleOrder.PurchaseOrders)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                                if (pO.PurchaseInvoices.Count != 0)
                                    foreach (var pI in pO.PurchaseInvoices)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                        if (pI.Payments.Count != 0)
                                            foreach (var payment in pI.Payments)
                                            {
                                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                            }
                                    }
                            }
                        }
                        if (saleOrder.Bills.Count != 0)
                        {
                            foreach (var bill in saleOrder.Bills)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                if (bill.Payments.Count != 0)
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetSRAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                    }
                            }
                        }

                        foreach (var cat in atachments)
                        {
                            foreach (var otherCat in otherAttachments)
                            {
                                if (otherCat.name == cat.name)
                                {
                                    foreach (var file in otherCat.Items)
                                    {
                                        cat.Items.Add(file);
                                    }
                                }
                            }
                        }



                        treeViewAttachments1.ItemsSource = atachments;
                        grdAttachments1.Visibility = Visibility.Visible;
                    }
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                    });
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                    });
                    DXMessageBox.Show("Select any Sale Receipt first!");
                }

            }
        }

        public void GellAllOrdersTracking()
        {
            if (grdCntrlSalesReceipt.SelectedItem != null)
            {
                var idd = (grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView).Id;

                if (idd != 0)
                {
                    OrderTracking tracking = new OrderTracking();
                    grdOrdersTracking.ItemsSource = tracking.getTransactions(idd, TransactionItemType.Sale_Receipt);
                    grdTrackingTree.ExpandAllNodes();

                }
            }
            else
            {
                DXMessageBox.Show("Select any Sale Receipt first!");
            }

        }

        

        private void btnGJournal_Click(object sender, RoutedEventArgs e)
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

        private void btnCreateIBT_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers(groupId, true);
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                ucFrmBankTransfer.frmBankTranfer.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;

            }
        }

        private void btnCreateVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.JV, 0, groupId, true);
                procurmentPanel.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;

            }
        }

        private void btnCreateIBCT_Click(object sender, RoutedEventArgs e)
        {
            ucBankTransferInterCompany uc = new ucBankTransferInterCompany(groupId, true);
            Window win = new Window();
            win.Content = uc;

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        
       



        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void grdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)
            {
                {
                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    e.Value = fname + " " + lname;
                }
            }
        }

        private void grdCommentss_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sSelectedPath = "";

                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Inquiry"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Inquiry);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }


                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
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

        private void btnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (groupId != 0)
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
                        destination += "Attachments\\Sale_Receipt\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Sale_Receipt.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Receipt);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Sale_Receipt, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 11, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);
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

        private void btnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (groupId != 0)
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
                        destination += "Attachments\\Sale_Receipt\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Sale_Receipt.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Receipt);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Sale_Receipt, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 11, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);
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

        private void ComboBoxEdit_ReceiptType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //grdCntrlSalesReceipt.Columns["TotalAmount"].Visible = false;
            //column.Visible = false;

            if (cmbxReceiptType.SelectedIndex == 0)
            {
                grdPrincipal.Visibility = Visibility.Collapsed;

                grdCustomer.Visibility = Visibility.Visible;
                lookupCustomers.Visibility = Visibility.Visible;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = false;
            }
            else
            {
                grdPrincipal.Visibility = Visibility.Visible;

                grdCustomer.Visibility = Visibility.Collapsed;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = true;
                lookupCustomers.Visibility = Visibility.Collapsed;
            }
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            loadDepartments();
            loadBillReferenceNo();
            loadVATBookReferenceNo();
        }
        private void loadVATBookReferenceNo()
        {
            if (lookupCompany.SelectedItem != null)
            {
                ERP_BL.VATBook.VATBookRepo vatBookRepo = new ERP_BL.VATBook.VATBookRepo();
                var references = vatBookRepo.GetAllActiveVATBookReferenceNo((lookupCompany.SelectedItem as Company).Id);
                List<cmbitem> cmbitems = new List<cmbitem>();
                cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
                foreach (ERP_BL.VATBook.VATBookRefNumber _ref in references)
                {
                    cmbitems.Add(new cmbitem() { name = _ref.VATBookReferenceNo, id = _ref.Id });
                }
                cmbxVATBookRef.ItemsSource = cmbitems;
            }
        }
        private void loadBillReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();


            references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);


            if (editFlag == true && saleReceipts.Count > 0)
            {
                if (saleReceipts[0].PettyCashRef != null && references.FirstOrDefault(x => x.Id == saleReceipts[0].PettyCashRefId) == null)
                    references.Add(saleReceipts[0].PettyCashRef);
            }


            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }

            cmbxPettyCashRef.ItemsSource = cmbitems;
        }
        private void loadDepartments()
        {
            List<Department> departments = new List<Department>();
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            //if (MainWindow.currentUserid == 0)
            //{
            //    DepartmentRepo departmentRepo = new DepartmentRepo();
            //    this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
            //    return;
            //}
            //if (company != null)
            //    if (company.departments != null)
            //    {
            //        List<Department> departments = new List<Department>();
            //        foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x=>x.IsSaleReceiptType == true))
            //        {
            //            if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
            //                departments.Add(_dept);
            //        }
            //        if (receiptsList != null && receiptsList.Count > 0 && receiptsList[0].Id > 0 && saveEditFlag == 1)
            //            if (receiptsList[0].department != null && departments.FirstOrDefault(x => x.Id == receiptsList[0].department.Id) == null)
            //                departments.Add(receiptsList[0].department);

            //        lookupDepartment.ItemsSource = departments;
            //        if (departments.Count == 0)
            //        {
            //            MessageBox.Show("This company dosen't contain any department mapped with the current User");
            //        }
            //    }


            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.IsSaleReceiptType == true && x.isActive == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (saleReceipts != null && saleReceipts.Count > 0 && editFlag == true)
                        if (saleReceipts[0].Departments != null)
                            foreach (var _dept in saleReceipts[0].Departments)
                            {
                                if (departments.FirstOrDefault(x => x.Id == _dept.Id) == null)
                                    departments.Add(_dept);
                            }
                }

                deptList.Clear();
                lookupDepartment1.Text = "";

                if (editFlag == true && companyChnaged == null)
                    companyChnaged = false;
                else if (editFlag == true && companyChnaged == false)
                    companyChnaged = true;

                lookupDepartment1.ItemsSource = departments;

                loadBillReferenceNo();

                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompany(company);
                lookupBanks.ItemsSource = banks;
            }
        }

        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Select Company first");
            }
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            ////Saving the Selected Company
            //department = new Department();
            //department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

            ////Clearing Bank and Account list on Selected Department changed
            //bank = new Bank();
            //bankList.Clear();
            //accntList.Clear();
            //lookupAccounts.ItemsSource = null;
            //lookupBanks.ItemsSource = null;

            ////Getting all customers linked with selected department
            //lookupCustomers.ItemsSource = department.customers.Where(x => x.isActive == true).ToList();

            //PrincipalRepo principalRepo = new PrincipalRepo();
            //var allPrincipals = principalRepo.getAll();

            //List<Principal> principalList = new List<Principal>();
            //foreach (var _principal in allPrincipals)
            //{
            //    foreach (var _dept in _principal.departments)
            //    {
            //        if (_dept.Id == department.Id)
            //        {
            //            principalList.Add(_principal);
            //            break;
            //        }
            //    }
            //}
            //lookupPrincipal.ItemsSource = principalList;




            //var accounts = department.accounts;
            //allAccounts.Clear();

            //foreach (var _account in accounts)
            //{
            //    allAccounts.Add(_account);

            //    if (!bankList.Contains(_account.bank))
            //        bankList.Add(_account.bank);
            //}

            //lookupBanks.ItemsSource = bankList;
            //loademployees();
        }


        GridControl gridControl = new GridControl();
        private void LookupDepartment1_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            List<CustomerCompany> customers = new List<CustomerCompany>();
            string deptNames = "";

            gridControl = lookupDepartment1.GetGridControl();
            var treeView = gridControl.View as TreeListView;

            deptList.Clear();



            CallRecursive(treeView);
            //foreach(var _node in treeView.Nodes)
            //{
            //    var nodes = _node.Nodes;
            //    while(nodes != null)
            //    {
            //        foreach (var _nodee in nodes)
            //        {

            //        }
            //    }
            //    var row = gridControl.GetRow(_node.RowHandle) as Department;
            //    deptList.Add(row);
            //}
            allEmployees.Clear();
            if (deptList.Count > 0)
            {

                foreach (var _dept in deptList)
                {
                    deptNames = deptNames + " | " + _dept.DeptName;
                    if (lookupCompany.SelectedIndex > -1)
                        foreach (var _customer in _dept.customers)
                        {
                            if (!customers.Contains(_customer))
                                customers.Add(_customer);
                        }
                    allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                }
            }
            loademployees();

            lookupDepartment1.EditValue = deptNames;
            lookupCustomers.ItemsSource = customers;


            //    //Getting all customers linked with selected department
            //    cmbxCustomers.ItemsSource = department.customers.Where(x=>x.isActive == true).ToList();

            PrincipalRepo principalRepo = new PrincipalRepo();
            var principalList = principalRepo.getAllByMultiDept(deptList);

            //    List<Principal> principalList = new List<Principal>();
            //    foreach (var _principal in allPrincipals)
            //    {
            //        foreach (var _dept in _principal.departments)
            //        {
            //            if (_dept.Id == department.Id)
            //            {
            //                principalList.Add(_principal);
            //                break;
            //            }
            //        }
            //    }
            lookupPrincipal.ItemsSource = principalList;

        }

        private void PrintRecursive(TreeListNode treeNode)
        {
            // Print the node.  
            if (treeNode.IsChecked == true)
            {
                var row = gridControl.GetRow(treeNode.RowHandle) as Department;
                deptList.Add(row);
            }


            // Visit each node recursively.  
            foreach (TreeListNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
            }
        }

        // Call the procedure using the TreeView.  
        private void CallRecursive(TreeListView treeView)
        {
            // Print each node recursively.  
            foreach (TreeListNode n in treeView.Nodes)
            {
                //recursiveTotalNodes++;
                PrintRecursive(n);
            }
        }


        private void TreeListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && companyChnaged == false)
            {
                var lookupDepts = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;
                int index = 0;
                if (saleReceipts[0].department != null && deptList != null && deptList.Count > 0)
                {
                    var grid = lookupDepartment1.GetGridControl();
                    gridControl = lookupDepartment1.GetGridControl();
                    var treeView = gridControl.View as TreeListView;

                    List<int> deptIds = new List<int>();
                    foreach (var _dept in deptList)
                        deptIds.Add(_dept.Id);

                    foreach (Department _deptt in lookupDepts)
                    {
                        if (deptIds.Contains(_deptt.Id))
                        {
                            //var node = treeView.Nodes[index];
                            //node.IsChecked = true;
                            CallRecursion(treeView, _deptt.Id, true);
                        }
                        else
                        {
                            CallRecursion(treeView, _deptt.Id, false);
                            //var node = treeView.Nodes[index];
                            //node.IsChecked = false;
                        }
                        index++;
                    }
                }
                else if (saleReceipts[0].Departments != null && saleReceipts[0].Departments.Count > 0)
                {
                    var grid = lookupDepartment1.GetGridControl();
                    gridControl = lookupDepartment1.GetGridControl();
                    var treeView = gridControl.View as TreeListView;

                    var column = grid.Columns[0];
                    index = 0;
                    deptList = saleReceipts[0].Departments;

                    List<int> deptIds = new List<int>();
                    foreach (var _dept in deptList)
                        deptIds.Add(_dept.Id);

                    foreach (Department _deptt in lookupDepts)
                    {
                        if (deptIds.Contains(_deptt.Id))
                        {
                            //var node = treeView.Nodes[index];
                            //node.IsChecked = true;
                            CallRecursion(treeView, _deptt.Id, true);
                        }
                        else
                        {
                            CallRecursion(treeView, _deptt.Id, false);
                            //var node = treeView.Nodes[index];
                            //node.IsChecked = false;
                        }
                        index++;
                    }
                }
            }

        }

        // Call the procedure using the TreeView.  
        private void CallRecursion(TreeListView treeView, int id, bool check)
        {
            // Print each node recursively.  
            foreach (TreeListNode n in treeView.Nodes)
            {
                //recursiveTotalNodes++;
                CheckNodes(n, id, check);
            }
        }

        private void CheckNodes(TreeListNode treeNode, int id, bool check)
        {
            // Print the node.  
            //if (treeNode.IsChecked == true)
            //{
            var row = gridControl.GetRow(treeNode.RowHandle) as Department;
            //deptList.Add(row);
            if (row.Id == id)
            {
                treeNode.IsChecked = check;
                return;
            }
            // }


            // Visit each node recursively.  
            foreach (TreeListNode tn in treeNode.Nodes)
            {
                CheckNodes(tn, id, check);
            }
        }

        private void TreeListView_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            var grid = lookupDepartment1.GetGridControl();
            var row = grid.GetRow(e.Node.RowHandle) as Department;

            if (e.Node.IsChecked == true)
                deptList.Add(row);
            else
                deptList.Remove(row);

        }

        public void loademployees()
        {
            cmbTransactionHolder.ItemsSource = null;
            allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(y => y.FirstOrDefault()).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (ERP_BL.Databases.Employee employee in allEmployees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbTransactionHolder.ItemsSource = cmbitems;
            //cmbAppliedBy.ItemsSource = cmbitems;
        }

        private void lookupCustomers_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Select Department First");
                lookupDepartment1.Focus();
                return;
            }
        }

      

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            //var userChartofAccounts = chartofAccountsRepo.GetAllforVendorBills(cmbxCompany.SelectedItem as Company, lookupDepartment.SelectedItem as Department, SYSTEM_STATIC.currentUser.id);
            //lookupCOA.ItemsSource = userChartofAccounts;

            List<ChartofAccount> userChartofAccounts = new List<ChartofAccount>();

            foreach (var _dept in deptList)
            {
                userChartofAccounts.AddRange(chartofAccountsRepo.GetAllforVendorBills(lookupCompany.SelectedItem as Company, _dept, SYSTEM_STATIC.currentUser.id));
            }

            userChartofAccounts = userChartofAccounts.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            lookupCOA.ItemsSource = userChartofAccounts;
        }

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void lookupCOA_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void lookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddPrincipal_Click(object sender, RoutedEventArgs e)
        {
            Principalss.frmPrincipaladd Principaladd = new Principalss.frmPrincipaladd();
            Principaladd.ShowDialog();
            loadPrincipals();
        }
        public void loadPrincipals()
        {
            lookupPrincipal.ItemsSource = department.Principals;
        }

        private void LookupCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void txtDeductionAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();
        }

        private void TxtVAT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
        }

        private void TxtTotalAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();
        }

        private void TxtExchangeRate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtBankChargesER.Text = txtDedER.Text;
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();

            int count = grdCntrlSalesReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdCntrlSalesReceipt.SetCellValue(i, grdCntrlSalesReceipt.Columns["ExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }

        private void txtBankCharges_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();
        }

        private void txtBankChargesVAT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
        }

        private void txtTotalBankCharges_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();
        }

        private void txtBankChargesER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtDedER.Text = txtBankChargesER.Text;
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();

            int count = grdCntrlSalesReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdCntrlSalesReceipt.SetCellValue(i, grdCntrlSalesReceipt.Columns["ExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }

        private void lookupBanks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Select Department First");
                lookupDepartment1.Focus();
                return;
            }
            if (lookupCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please currency first!");
            }
        }

        private void lookupBanks_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var curr = lookupCurrency.SelectedItem as Currency;
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();

            List<int> dept_Ids = new List<int>();
            foreach (var _dept in deptList)
            {
                dept_Ids.Add(_dept.Id);
            }

            if (lookupBanks.SelectedItem != null)
            {
                var bank = lookupBanks.SelectedItem as Bank;
                var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();/*.Where(x => x.departments.Contains(department) && x.company.Id == (cmbxCompany.SelectedItem as Company).Id && x.accountsCategory == AccountsCategory.Company).ToList()*/
                List<Account> allowedAccounts = new List<Account>();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Vendor Bank Accounts in Payments") == null)
                {
                    foreach (var _account in accntList)
                    {
                        if (_account.currency.Id == curr.Id && _account.departments.FirstOrDefault(x => dept_Ids.Contains(x.Id)) != null && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        {
                            allowedAccounts.Add(_account);
                        }
                    }
                }
                else
                {
                    foreach (var _account in accntList)
                    {
                        if ((_account.currency.Id == curr.Id || _account.isAdjustmentAccount == true) && _account.departments.FirstOrDefault(x => dept_Ids.Contains(x.Id)) != null && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal || _account.accountsCategory == AccountsCategory.Vendor))
                        {
                            allowedAccounts.Add(_account);
                        }
                    }
                }


                lookupAccounts.ItemsSource = allowedAccounts;
            }
        }

        private void lookupAccounts_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupBanks.SelectedIndex < 0)
            {
                DXMessageBox.Show("Select Bank First");
            }
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editFlag == true)
            {
                var source = grdCntrlSalesReceipt.ItemsSource as List<CustomerCreditsSaleReceiptModelView>;
                var receipt = source.First();
                if (cmbTransactionHolder.SelectedItem != null)
                {
                    if (receipt.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = receipt.holderChangeDate;
                    }
                }
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            List<CustomerCredit> customerCredits = new List<CustomerCredit>();
            GridColumn column = new GridColumn();
            SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();

            //Condition to check the Receipt Type
            if (cmbxReceiptType.SelectedIndex == 0)
            {
                if (lookupCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Company");
                }
                else if (deptList == null || deptList.Count == 0)
                {
                    DXMessageBox.Show("Select Department First");
                    lookupDepartment1.Focus();
                    return;
                }
                else if (lookupCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Currency");
                }
                else if (txtCollectionAmnt.Text.ToString() == "" || Convert.ToDouble(txtCollectionAmnt.Text.ToString()) == 0)
                {
                    DXMessageBox.Show("Enter Collection Amount");
                }
                else
                {
                    //Selected Customer from Combobox
                    List<CustomerCreditsSaleReceiptModelView> invoices = new List<CustomerCreditsSaleReceiptModelView>();

                    var selectedCurrency = lookupCurrency.SelectedItem as Currency;

                    customerCredits = saleInvoiceRepo.getAllInvoicesForCustomerCredits(company.Id, selectedCurrency.Id, deptList);

                    customerCredits = customerCredits.Where(x => x.SaleInvoice.saleInvoiceStatus.isActive == true).ToList();

                    //customerCredits = customerCredits.RemoveRange(closedInvoices);

                    if (customerCredits.Count > 0)
                    {
                        // Matching all invoices one by one
                        foreach (var _customerCredit in customerCredits)
                        {
                            CustomerCreditsSaleReceiptModelView receipt = new CustomerCreditsSaleReceiptModelView();
                                //if ((_invoice.customerCompany.company.Id == custCompany.Id) && (_invoice.currency.Id == selectedCurrency.Id) && (_invoice.department.Id == department.Id))
                                //{
                                receipt.InvoiceId = _customerCredit.SaleInvoice.Id;
                                receipt.Date = _customerCredit.SaleInvoice.ApprovedDate;
                                receipt.Customer = _customerCredit.CustomerCompany.company.CompanyName;
                                receipt.SalesReferenceNo = _customerCredit.SaleInvoice.SalesReferenceNo;
                                receipt.FinanceRefNo = _customerCredit.SaleInvoice.FinanceRefrenceNo;
                                receipt.Currency = _customerCredit.SaleInvoice.currency.CurrencyName;

                                double invoiceAmount = 0;    
                                invoiceAmount = _customerCredit.creditAmount;



                                receipt.OriginalAmount = invoiceAmount;

                                receipt.InvoiceStage = GetInvoiceStatus(_customerCredit.SaleInvoice);

                                var reciepts = _customerCredit.SaleInvoice.salesReceipts;
                                if (reciepts != null)
                                {
                                    var result = Math.Round(reciepts.Where(x => x.isVoid != true && x.CustomerCreditSerialNo == _customerCredit.SerialNo).Sum(x => x.CollectionAmount), 2);
                                    receipt.AmountDue = invoiceAmount - result;
                                }


                                receipt.serialNo = _customerCredit.SerialNo;

                                invoices.Add(receipt);
                                //}
                        }
                        grdCntrlSalesReceipt.ItemsSource = invoices;
                    }
                    else
                    {
                        DXMessageBox.Show("No invoice found!");
                    }
                }
            }

            grdCntrlSalesReceipt.Columns["CreditedAmount"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            //grdCntrlSalesReceipt.Columns["Id"].Visible = false;
            grdCntrlSalesReceipt.Columns.Remove(column);

            _visibleItems = new List<CustomerCreditsSaleReceiptModelView>();
            foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
            {
                var item = (CustomerCreditsSaleReceiptModelView)_item;
                _visibleItems.Add(item);
            }

            foreach (var item in tblViewLandTypeLst.VisibleColumns)
            {
                tblViewLandTypeLst.BestFitColumn(item);
            }
        }

        private void grdCntrlSalesReceipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlSalesReceipt.GetRowByListIndex(e.ListSourceRowIndex) as CustomerCreditsSaleReceiptModelView;
            if (e.IsGetData && e.Column.FieldName == "Total")
            {

                if (row.receiptDedTaxes != null)
                {
                    row.TotalAmount = row.CreditedAmount + row.Deductions + row.receiptDedTaxes.Sum(x => x.Amount);
                }
                else
                {
                    row.TotalAmount = row.CreditedAmount + row.Deductions;

                }
                e.Value = row.TotalAmount;
            }

            if (e.IsGetData && e.Column.FieldName == "VAT")
            {
                var rcpt = grdCntrlSalesReceipt.GetRowByListIndex(e.ListSourceRowIndex) as CustomerCreditsSaleReceiptModelView;

                //Adding the Values of four columns to show in the Total column
                e.Value = rcpt.dedVAT + rcpt.bankVAT;
            }
            //column.Visible = false;
        }

        private void grdCntrlSalesReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            var tableView = sender as TableView;
            if (tableView != null)
            {
                foreach (var item in tableView.VisibleColumns)
                {
                    tableView.BestFitColumn(item);
                }
            }
        }

        private void tblViewLandTypeLst_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tblViewLandTypeLst_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as CustomerCreditsSaleReceiptModelView;
            var ded = row.Deductions + row.VAT + row.BankCharges;
            row.ExchangeRate = Convert.ToDouble(txtDedER.Text);

            var rcpt = saleReceipts.FirstOrDefault(x => x.Id == row.Id);

            if (editFlag == true && rcpt != null)
            {
                var credt = row.CreditedAmount;
                //var adjustmentAmount = pymnt.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                var rcptss = rcpt.saleInvoice.salesReceipts.Where(x => x.isVoid != true && x.Id != rcpt.Id && x.CustomerCreditSerialNo == rcpt.CustomerCreditSerialNo);
                var amountReceived = rcptss.Sum(y => y.CollectionAmount) /*+ adjustmentAmount*/;
                amountReceived = Math.Round(amountReceived + credt, 2);

                double invoiceAmount = 0;
                invoiceAmount = row.OriginalAmount;

                if (Math.Abs(amountReceived) > Math.Abs(invoiceAmount))
                {
                    DXMessageBox.Show("Credited Amount cannot exceed Bill Amount!");
                    row.CreditedAmount = rcpt.CollectionAmount;

                    if (row.receiptDedTaxes != null)
                    {
                        row.DeductionSOC = (row.Deductions + row.receiptDedTaxes.Sum(x => x.Amount)) * row.ExchangeRate;
                        row.TotalAmount = row.CreditedAmount + row.Deductions + row.receiptDedTaxes.Sum(x => x.Amount);
                    }
                    else
                    {
                        row.DeductionSOC = row.Deductions * row.ExchangeRate;
                        row.TotalAmount = row.CreditedAmount + row.Deductions;

                    }

                    //row.Total = rcpt.CollectionAmount /*+ ded*/;
                    return;
                }
            }

            if (Math.Round(row.CreditedAmount, 2) > Math.Round(row.AmountDue, 2) && editFlag == false)
            {
                DXMessageBox.Show("Credited amount cannot exceed Amount Due!");
                ((DataViewBase)sender).Background = Brushes.LightBlue;
                row.CreditedAmount = 0;

                //row.Deductions = 0;
                return;
            }


            if (row.receiptBankTaxes != null)
                row.BankChargesSOC = (row.BankCharges + row.receiptBankTaxes.Sum(x => x.Amount)) * row.ExchangeRate;
            else
                row.BankChargesSOC = row.BankCharges * row.ExchangeRate;

            if (row.receiptDedTaxes != null)
            {
                row.DeductionSOC = (row.Deductions + row.receiptDedTaxes.Sum(x => x.Amount)) * row.ExchangeRate;
                row.TotalAmount = row.CreditedAmount + row.Deductions + row.receiptDedTaxes.Sum(x => x.Amount);
            }
            else
            {
                row.DeductionSOC = row.Deductions * row.ExchangeRate;
                row.TotalAmount = row.CreditedAmount + row.Deductions;

            }
            row.TotalDeductionSOC = Math.Round(row.ExchangeRate * ded, 2);
        }

        private void tblViewLandTypeLst_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {
            switch (e.MenuInfo.Column.FieldName)
            {
                case "Deductions":
                    mbtnAddDeductions.IsVisible = true;
                    mbtnBankCharges.IsVisible = false;
                    break;
                case "BankCharges":
                    mbtnAddDeductions.IsVisible = false;
                    mbtnBankCharges.IsVisible = true;
                    break;
                default:
                    mbtnAddDeductions.IsVisible = false;
                    mbtnBankCharges.IsVisible = false;
                    break;
            }
        }

        private void mbtnAddDeductions_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (grdCntrlSalesReceipt.SelectedItem != null)
                    {
                        var receipt = grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView;
                        ucSalesReceiptDeduction ucSalesReceiptDeduction = new ucSalesReceiptDeduction(receipt.Id);
                        ucSalesReceiptDeduction.BankCharges = false;
                        ucSalesReceiptDeduction.ShowDialog();
                        receipt.receiptDeductions = ucSalesReceiptDeduction.finalDeductions;
                        receipt.Deductions = ucSalesReceiptDeduction.totalDeduction;
                        grdCntrlSalesReceipt.SetFocusedRowCellValue("Deductions", ucSalesReceiptDeduction.totalDeduction);

                        receipt.receiptDedTaxes = ucSalesReceiptDeduction.finalTaxes;
                        receipt.dedVAT = ucSalesReceiptDeduction.totalVAT;
                        receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        receipt.IsDedAdjusted = ucSalesReceiptDeduction.IsDedAdjusted;
                    }
                    else
                        DXMessageBox.Show("Please Select Receipt to Add Deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Sale Receipt first!");
                return;
            }
        }

        private void MbtnAddBankCharges_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (grdCntrlSalesReceipt.SelectedItem != null)
                    {
                        var receipt = grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView;
                        ucSalesReceiptDeduction ucSalesReceiptDeduction = new ucSalesReceiptDeduction(receipt.Id);
                        ucSalesReceiptDeduction.BankCharges = true;
                        ucSalesReceiptDeduction.ShowDialog();
                        receipt.bankChargess = ucSalesReceiptDeduction.finalDeductions;
                        receipt.BankCharges = ucSalesReceiptDeduction.totalDeduction;
                        grdCntrlSalesReceipt.SetFocusedRowCellValue("BankCharges", ucSalesReceiptDeduction.totalDeduction);

                        receipt.receiptBankTaxes = ucSalesReceiptDeduction.finalTaxes;
                        receipt.bankVAT = ucSalesReceiptDeduction.totalVAT;
                        receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        receipt.IsBankAdjusted = ucSalesReceiptDeduction.IsBankAdjusted;
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("bankVAT", ucSalesReceiptDeduction.totalVAT);

                    }
                    else
                        DXMessageBox.Show("Please Select Receipt to Add Bank Charges.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Sale Receipt first!");
                return;
            }
        }

        private void btnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView;
            if (selectedReceipt != null)
            {
                SalesReceipt dbreceipt = salesReceiptRepo.GetSalesReceiptForCostSheet(selectedReceipt.Id);
                var deductionAmont = dbreceipt.receiptDeductions.Sum(x => x.Amount);
                var amountSOC = selectedReceipt.TotalDeductionSOC;


                //if (amountSOC != 0)
                //{

                if (dbreceipt.saleInvoice.saleInvoicetype == InquiryType.DistributionBiz)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                    {
                        SaleOrder saleOrder = new SaleOrder();
                        saleOrder = dbreceipt.saleInvoice.SaleOrder;
                        if (dbreceipt.BudgetSystemCostFields.Count != 0)
                        {
                            winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, true, Convert.ToDouble(amountSOC), dbreceipt, saleOrder);
                            winAddBudgetSystemCost.costSheetFields = dbreceipt.BudgetSystemCostFields;
                            systemCost.ShowDialog();
                            dbreceipt.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;
                        }
                        else
                        {
                            if (saleOrder.Budget_Id != null)
                            {
                                winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, false, Convert.ToDouble(amountSOC), dbreceipt, saleOrder);
                                systemCost.ShowDialog();
                                dbreceipt.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;

                            }
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Punch Budget System Cost", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if ((grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView).CostSheetId == null)
                    {
                        salesReceiptRepo.updateForCostSheet(dbreceipt.Id, (int)dbreceipt.saleInvoice.SaleOrder.CostSheet_Id);
                        try
                        {
                            var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Receipt?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update System Cost from Sales Receipt") != null)

                                {
                                    if (dbreceipt.saleInvoice.SaleOrderId != 0 && dbreceipt.Id != 0)
                                    {
                                        winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(dbreceipt.saleInvoice.SaleOrder, Convert.ToDecimal(amountSOC), dbreceipt);
                                        winSelectCostSheetFields.ShowDialog();
                                    }
                                }
                                else
                                    DXMessageBox.Show("You do not have permisssion Update System Cost from Sales Receipt", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Receipt?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update System Cost from Sales Receipt") != null)
                                {
                                    if (dbreceipt.saleInvoice.SaleOrderId != 0 && dbreceipt.Id != 0)
                                    {
                                        winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(dbreceipt.saleInvoice.SaleOrder, Convert.ToDecimal(amountSOC), dbreceipt);
                                        winSelectCostSheetFields.ShowDialog();
                                    }
                                }
                                else
                                    DXMessageBox.Show("You do not have permisssion Update System Cost from Sales Receipt", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void btnCostSheetPunching_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView;
                if (selectedReceipt != null)
                {
                    ProcurementRepo repo = new ProcurementRepo();
                    string systemCost = "";
                    List<CostSheetSaleReceiptField> costSheetReceiptValues = repo.GetSystemReceiptCosts(selectedReceipt.Id);
                    foreach (CostSheetSaleReceiptField field in costSheetReceiptValues)
                    {
                        if (field.Value != 0)
                        {
                            systemCost += field.Field.Title + " " + " " + field.Value + "\n";
                        }

                    }
                    var btn = e.Source as SimpleButton;
                    btn.ToolTip = systemCost;
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            string currency = null;
            string incoTerm = null;
            string creationDate = null;
            string paymentTerm = null;
            string maker = null;
            string origin = null;
            string packing = null;
            string Warranty = null;
            var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView;
            if (selectedReceipt.Id != 0)
            {
                SalesReceipt dbreceipt = salesReceiptRepo.GetSalesReceiptForCostSheet(selectedReceipt.Id);


                if (dbreceipt.saleInvoice.saleInvoicetype == InquiryType.DistributionBiz)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
                    {

                        SaleOrder saleOrder = new SaleOrder();
                        saleOrder = dbreceipt.saleInvoice.SaleOrder;

                        if (saleOrder.Budget_Id != null)
                        {
                            frmBudgetAdd budget = new frmBudgetAdd((int)saleOrder.Budget_Id);
                            budget.ShowDialog();
                        }
                        else
                        {
                            DXMessageBox.Show("Please attach budget with sale order first", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("View Budget", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    if ((grdCntrlSalesReceipt.SelectedItem as CustomerCreditsSaleReceiptModelView).CostSheetId == 0)
                    {
                        salesReceiptRepo.updateForCostSheet(dbreceipt.Id, (int)dbreceipt.saleInvoice.SaleOrder.CostSheet_Id);
                        try
                        {
                            var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Receipt?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Cost Center from Sales Receipt") != null)
                                {
                                    if (dbreceipt.saleInvoice.SaleOrderId != 0)
                                    {
                                        if (dbreceipt.saleInvoice.SaleOrder.currency != null)
                                        {
                                            currency = dbreceipt.saleInvoice.SaleOrder.currency.Symbol;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain currency please set currency at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.incoterm != null)
                                        {
                                            incoTerm = dbreceipt.saleInvoice.SaleOrder.incoterm.term;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain incoterm please set incoterm at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.CreationDate != null)
                                        {
                                            creationDate = dbreceipt.saleInvoice.SaleOrder.CreationDate.ToString();
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain CreationDate please set CreationDate at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.paymentTerm != null)
                                        {
                                            paymentTerm = dbreceipt.saleInvoice.SaleOrder.paymentTerm.term;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain paymentTerm please set paymentTerm at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.maker != null)
                                        {
                                            maker = dbreceipt.saleInvoice.SaleOrder.maker;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain maker please set maker at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.origin != null)
                                        {
                                            origin = dbreceipt.saleInvoice.SaleOrder.origin;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain origin please set origin at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (views.Count != 0)
                                        {
                                            var AllViews = views;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.packing != null)
                                        {
                                            packing = dbreceipt.saleInvoice.SaleOrder.packing;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain packing please set packing at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.Warranty != null)
                                        {
                                            Warranty = dbreceipt.saleInvoice.SaleOrder.Warranty.name;

                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain Warranty please set Warranty at So First", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                            return;
                                        }
                                        frmCostSheet frmCostSheet = new frmCostSheet(dbreceipt.saleInvoice.SaleOrder,
                                            currency,
                                            incoTerm,
                                            creationDate,
                                            paymentTerm,
                                            maker,
                                            origin,
                                            views,
                                            dbreceipt.saleInvoice.SaleOrder.saleOrdertype,
                                            packing,
                                            Warranty, dbreceipt.saleInvoice.SaleOrder.isApproved, dbreceipt.saleInvoice.SaleOrder.isReApproved);
                                        frmCostSheet.ShowDialog();
                                    }
                                }
                                else
                                    DXMessageBox.Show("You do not have permisssion View Cost Center from Sales Receipt", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                    else
                    {
                        try
                        {

                            var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Receipt?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Cost Center from Sales Receipt") != null)
                                {
                                    if (dbreceipt.saleInvoice.SaleOrderId != 0)
                                    {
                                        if (dbreceipt.saleInvoice.SaleOrder.currency != null)
                                        {
                                            currency = dbreceipt.saleInvoice.SaleOrder.currency.Symbol;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain currency please set currency at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.incoterm != null)
                                        {
                                            incoTerm = dbreceipt.saleInvoice.SaleOrder.incoterm.term;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain incoterm please set incoterm at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.CreationDate != null)
                                        {
                                            creationDate = dbreceipt.saleInvoice.SaleOrder.CreationDate.ToString();
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain CreationDate please set CreationDate at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.paymentTerm != null)
                                        {
                                            paymentTerm = dbreceipt.saleInvoice.SaleOrder.paymentTerm.term;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain paymentTerm please set paymentTerm at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.maker != null)
                                        {
                                            maker = dbreceipt.saleInvoice.SaleOrder.maker;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain maker please set maker at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.origin != null)
                                        {
                                            origin = dbreceipt.saleInvoice.SaleOrder.origin;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain origin please set origin at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (views.Count != 0)
                                        {
                                            var AllViews = views;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.packing != null)
                                        {
                                            packing = dbreceipt.saleInvoice.SaleOrder.packing;
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain packing please set packing at So First", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                            return;
                                        }
                                        if (dbreceipt.saleInvoice.SaleOrder.Warranty != null)
                                        {
                                            Warranty = dbreceipt.saleInvoice.SaleOrder.Warranty.name;

                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Related Sale Order does not contain Warranty please set Warranty at So First", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                            return;
                                        }
                                        frmCostSheet frmCostSheet = new frmCostSheet(dbreceipt.saleInvoice.SaleOrder,
                                            currency,
                                            incoTerm,
                                            creationDate,
                                            paymentTerm,
                                            maker,
                                            origin,
                                            views,
                                            dbreceipt.saleInvoice.SaleOrder.saleOrdertype,
                                            packing,
                                            Warranty, dbreceipt.saleInvoice.SaleOrder.isApproved, dbreceipt.saleInvoice.SaleOrder.isReApproved);
                                        frmCostSheet.ShowDialog();
                                    }
                                }
                                else
                                    DXMessageBox.Show("You do not have permisssion View Cost Center from Sales Receipt", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        public List<JournalTransaction> getJournalTransactions()
        {
            SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            List<CustomerCreditsSaleReceiptModelView> _visibleItems = new List<CustomerCreditsSaleReceiptModelView>();
            _visibleItems = new List<CustomerCreditsSaleReceiptModelView>();
            foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
            {
                _visibleItems.Add((CustomerCreditsSaleReceiptModelView)_item);

            }
            var _vitem = _visibleItems[0];
            SalesReceipt saleReceipt = new SalesReceipt();
            saleReceipt = salesReceiptRepo.GetSalesReceipt(_vitem.Id);
            var selBank = lookupBanks.SelectedItem as Bank;
            var selAccnt = lookupAccounts.SelectedItem as Account;
            for (int i = 0; i < _visibleItems.Count; i++)
            {
                var _item = _visibleItems[i];
                var _SI = invoiceRepo.GetSaleInvoice(_item.InvoiceId);
                saleReceipt = new SalesReceipt();
                saleReceipt = salesReceiptRepo.GetSalesReceipt(_item.Id);
                saleReceipt.Id = _item.Id;
                saleReceipt.receiptType = ReceiptType.Customer_Credits;
                saleReceipt.CreationDate = datCreationDate.DateTime;
                saleReceipt.company = lookupCompany.SelectedItem as Company;
                saleReceipt.deptId = _SI.dept_Id;
                saleReceipt.Customer = lookupCustomers.SelectedItem as CustomerCompany;
                saleReceipt.SystemRefNo = txtSystemRef.Text;
                saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                saleReceipt.Currency = lookupCurrency.SelectedItem as ERP_BL.Databases.Currency;
                saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                saleReceipt.bank = selBank;
                saleReceipt.BankId = selBank.Id;
                saleReceipt.account = selAccnt;
                saleReceipt.saleInvoice = _SI;
                saleReceipt.AccountId = selAccnt.Id;

                saleReceipt.receiptDeductions = _item.receiptDeductions;



                if (isBypassCOA.IsChecked == true)
                {
                    saleReceipt.isBypassBank = true;
                    if (lookupCOA.SelectedIndex != -1)
                    {
                        saleReceipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                        if (banktransactionFlag == 0)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (lookupCOA.SelectedItem as ChartofAccount).Id);
                                JournalTransaction bankTransaction = new JournalTransaction();
                                if (dbtrans == null)
                                {
                                    bankTransaction.accountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    bankTransaction.creationDate = saleReceipt.GLPostingDate;
                                    bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                    bankTransaction.credit = 0;
                                    bankTransaction.userId = saleReceipt.user_Id;
                                    bankTransaction.SaleReceiptId = saleReceipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                    bankTransaction.deptId = saleReceipt.department.Id;
                                    bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                }
                                else
                                {
                                    bankTransaction.accountId = dbtrans.accountId;
                                    bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                    bankTransaction.creationDate = dbtrans.creationDate;
                                    bankTransaction.debit = dbtrans.debit;
                                    bankTransaction.MER = dbtrans.MER;
                                    bankTransaction.credit = dbtrans.credit;
                                    bankTransaction.userId = dbtrans.userId;
                                    bankTransaction.SaleReceiptId = dbtrans.SaleReceiptId;
                                    bankTransaction.transactionRefno = dbtrans.transactionRefno;
                                    bankTransaction.total = dbtrans.total;
                                    bankTransaction.deptId = dbtrans.deptId;
                                    bankTransaction.companyId = dbtrans.companyId;
                                    bankTransaction.currencyId = dbtrans.currencyId;

                                }
                                journalTransactions.Add(bankTransaction);

                            }
                            banktransactionFlag = 1;

                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    saleReceipt.isBypassBank = false;
                    saleReceipt.coaAccountId = null;
                    if (banktransactionFlag == 0)
                    {
                        if (saleReceipt.account.COA_accountId != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (lookupAccounts.SelectedItem as Account).COA_accountId);
                                JournalTransaction bankTransaction = new JournalTransaction();
                                if (dbtrans != null && dbtrans.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value))
                                {

                                    bankTransaction.accountId = dbtrans.accountId;
                                    bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                    bankTransaction.creationDate = dbtrans.creationDate;
                                    bankTransaction.debit = dbtrans.debit;
                                    bankTransaction.MER = dbtrans.MER;
                                    bankTransaction.credit = dbtrans.credit;
                                    bankTransaction.userId = dbtrans.userId;
                                    bankTransaction.SaleReceiptId = dbtrans.SaleReceiptId;
                                    bankTransaction.transactionRefno = dbtrans.transactionRefno;
                                    bankTransaction.total = dbtrans.total;
                                    bankTransaction.deptId = dbtrans.deptId;
                                    bankTransaction.companyId = dbtrans.companyId;
                                    bankTransaction.currencyId = dbtrans.currencyId;
                                    bankTransaction.currencyId = dbtrans.currencyId;
                                    bankTransaction.isReconciled = dbtrans.isReconciled;
                                    bankTransaction.reconcilationDate = dbtrans.reconcilationDate;
                                }
                                else
                                {
                                    bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    bankTransaction.creationDate = saleReceipt.GLPostingDate;
                                    bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                    bankTransaction.credit = 0;
                                    bankTransaction.userId = saleReceipt.user_Id;
                                    bankTransaction.SaleReceiptId = saleReceipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                    bankTransaction.deptId = saleReceipt.department.Id;
                                    bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                }
                                journalTransactions.Add(bankTransaction);
                            }
                            banktransactionFlag = 1;
                        }
                    }
                }
                if (saleReceipt.receiptDeductions.Count != 0)
                {
                    foreach (var receiptDeduction in saleReceipt.receiptDeductions)
                    {
                        var deduction = salesReceiptRepo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                        var debitDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);
                        //var creditDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.credit == receiptDeduction.Amount && x.accountId== deduction.chartofAccountId);

                        if (receiptDeduction.Amount != 0)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                if (deduction.chartofAccountId != null)
                                {
                                    JournalTransaction deductionTransaction = new JournalTransaction();

                                    if (debitDbtrans == null)
                                    {
                                        deductionTransaction.accountId = deduction.chartofAccountId;
                                        deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        deductionTransaction.creationDate = saleReceipt.GLPostingDate;
                                        deductionTransaction.debit = receiptDeduction.Amount;
                                        deductionTransaction.credit = 0;
                                        deductionTransaction.userId = saleReceipt.user_Id;
                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                        deductionTransaction.deptId = saleReceipt.department.Id;
                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                        deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                    }
                                    else
                                    {
                                        deductionTransaction.accountId = debitDbtrans.accountId;
                                        deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                        deductionTransaction.creationDate = debitDbtrans.creationDate;
                                        deductionTransaction.debit = debitDbtrans.debit;
                                        deductionTransaction.credit = debitDbtrans.credit;
                                        deductionTransaction.userId = debitDbtrans.userId;
                                        deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                        deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                        deductionTransaction.MER = debitDbtrans.MER;
                                        deductionTransaction.deptId = debitDbtrans.deptId;
                                        deductionTransaction.total = debitDbtrans.total;
                                        deductionTransaction.companyId = debitDbtrans.companyId;
                                        deductionTransaction.currencyId = debitDbtrans.currencyId;
                                        deductionTransaction.isReconciled = debitDbtrans.isReconciled;
                                        deductionTransaction.reconcilationDate = debitDbtrans.reconcilationDate;

                                    }
                                    journalTransactions.Add(deductionTransaction);
                                }
                            }
                        }
                    }
                }
                if (saleReceipt.bankCharges.Count != 0)
                {
                    foreach (var receiptDeduction in saleReceipt.bankCharges)
                    {
                        var deduction = salesReceiptRepo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                        var debitDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);

                        var creditDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.credit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);

                        if (receiptDeduction.Amount != 0)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                if (deduction.chartofAccountId != null)
                                {
                                    JournalTransaction deductionTransaction = new JournalTransaction();

                                    if (debitDbtrans == null)
                                    {
                                        deductionTransaction.accountId = deduction.chartofAccountId;
                                        deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        deductionTransaction.creationDate = saleReceipt.GLPostingDate;
                                        deductionTransaction.debit = receiptDeduction.Amount;
                                        deductionTransaction.credit = 0;
                                        deductionTransaction.userId = saleReceipt.user_Id;
                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                        deductionTransaction.deptId = saleReceipt.department.Id;
                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                        deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                    }
                                    else
                                    {
                                        deductionTransaction.accountId = debitDbtrans.accountId;
                                        deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                        deductionTransaction.creationDate = debitDbtrans.creationDate;
                                        deductionTransaction.debit = debitDbtrans.debit;
                                        deductionTransaction.credit = debitDbtrans.credit;
                                        deductionTransaction.userId = debitDbtrans.userId;
                                        deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                        deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                        deductionTransaction.MER = debitDbtrans.MER;
                                        deductionTransaction.deptId = debitDbtrans.deptId;
                                        deductionTransaction.total = debitDbtrans.total;
                                        deductionTransaction.companyId = debitDbtrans.companyId;
                                        deductionTransaction.currencyId = debitDbtrans.currencyId;
                                        deductionTransaction.isReconciled = debitDbtrans.isReconciled;
                                        deductionTransaction.reconcilationDate = debitDbtrans.reconcilationDate;

                                    }
                                    journalTransactions.Add(deductionTransaction);
                                }
                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                if (creditDbtrans == null)
                                {
                                    deducBankTransaction.accountId = selAccnt.COA_accountId;
                                    deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                    deducBankTransaction.debit = 0;
                                    deducBankTransaction.credit = receiptDeduction.Amount;
                                    //MER = 1,
                                    deducBankTransaction.userId = saleReceipt.user_Id;
                                    deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                    deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                    deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                    deducBankTransaction.deptId = saleReceipt.department.Id;
                                    deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                    deducBankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    deducBankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                }
                                else
                                {
                                    deducBankTransaction = creditDbtrans;
                                }

                                journalTransactions.Add(deducBankTransaction);
                            }

                        }
                    }

                }
                if (saleReceipt.ReceiptBankTaxes.Count > 0)
                {
                    foreach (var receiptBankTax in saleReceipt.ReceiptBankTaxes)
                    {
                        var vatTax = taxRepo.getTaxtById((int)receiptBankTax.taxNameId);
                        var debitDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == receiptBankTax.Amount && x.accountId == vatTax.COA_Id);

                        var creditDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.credit == receiptBankTax.Amount && x.accountId == vatTax.COA_Id);

                        if (receiptBankTax.Amount != 0)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                if (vatTax.COA_Id != null)
                                {
                                    JournalTransaction deductionTransaction = new JournalTransaction();

                                    if (debitDbtrans == null)
                                    {
                                        deductionTransaction.accountId = vatTax.COA_Id;
                                        deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        deductionTransaction.creationDate = saleReceipt.GLPostingDate;
                                        deductionTransaction.debit = receiptBankTax.Amount;
                                        deductionTransaction.credit = 0;
                                        deductionTransaction.userId = saleReceipt.user_Id;
                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                        deductionTransaction.deptId = saleReceipt.department.Id;
                                        deductionTransaction.total = receiptBankTax.Amount - 0;
                                        deductionTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                                    }
                                    else
                                    {
                                        deductionTransaction.accountId = debitDbtrans.accountId;
                                        deductionTransaction.coaTransactionsType = debitDbtrans.coaTransactionsType;
                                        deductionTransaction.creationDate = debitDbtrans.creationDate;
                                        deductionTransaction.debit = debitDbtrans.debit;
                                        deductionTransaction.credit = debitDbtrans.credit;
                                        deductionTransaction.userId = debitDbtrans.userId;
                                        deductionTransaction.transactionRefno = debitDbtrans.transactionRefno;
                                        deductionTransaction.SaleReceiptId = debitDbtrans.SaleReceiptId;
                                        deductionTransaction.MER = debitDbtrans.MER;
                                        deductionTransaction.deptId = debitDbtrans.deptId;
                                        deductionTransaction.total = debitDbtrans.total;
                                        deductionTransaction.companyId = debitDbtrans.companyId;
                                        deductionTransaction.currencyId = debitDbtrans.currencyId;
                                        deductionTransaction.isReconciled = debitDbtrans.isReconciled;
                                        deductionTransaction.reconcilationDate = debitDbtrans.reconcilationDate;

                                    }
                                    journalTransactions.Add(deductionTransaction);
                                }
                            }
                            if (btnPushCredits.IsChecked == true)
                            {
                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                if (creditDbtrans == null)
                                {
                                    deducBankTransaction.accountId = selAccnt.COA_accountId;
                                    deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                    deducBankTransaction.debit = 0;
                                    deducBankTransaction.credit = receiptBankTax.Amount;
                                    //MER = 1,
                                    deducBankTransaction.userId = saleReceipt.user_Id;
                                    deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                    deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                    deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                    deducBankTransaction.deptId = saleReceipt.department.Id;
                                    deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                    deducBankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    deducBankTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;
                                }
                                else
                                {
                                    deducBankTransaction = creditDbtrans;
                                }

                                journalTransactions.Add(deducBankTransaction);
                            }

                        }
                    }

                }
                if (saleReceipt.department.chartofAccountId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {

                        JournalTransaction receivableTransaction = new JournalTransaction();
                        receivableTransaction.accountId = saleReceipt.department.chartofAccountId;
                        receivableTransaction.deptId = saleReceipt.department.Id;

                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                        receivableTransaction.creationDate = saleReceipt.GLPostingDate;
                        receivableTransaction.debit = 0;
                        receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                        receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                        //receivableTransaction.MER = 1;
                        //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                        receivableTransaction.userId = saleReceipt.user_Id;
                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                        receivableTransaction.SaleReceiptId = saleReceipt.Id;
                        receivableTransaction.deptId = saleReceipt.department.Id;
                        receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                        receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                        receivableTransaction.currencyId = (lookupCurrency.SelectedItem as Currency).Id;

                        journalTransactions.Add(receivableTransaction);
                    }
                }

            }


            return journalTransactions;
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

                var item = (AllOrdersView)grid.SelectedItem;

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
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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


                        //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        ////ucFrmAddAccount obj = new ucFrmAddAccount();
                        //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        //updateSaleReceiptObj.enter_receipt_win.Show();

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

            //MessageBox.Show("Mission Successful!");
        }

        private void BtnDeposit_Checked(object sender, RoutedEventArgs e)
        {
            btnPayment.IsChecked = false;
        }

        private void BtnPayment_Checked(object sender, RoutedEventArgs e)
        {
            btnDeposit.IsChecked = false;
        }
       
    }
}
