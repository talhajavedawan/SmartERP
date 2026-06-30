using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ERP_BL.Procurements;
using ERP_BL.Payments;
using ZAS_ERP.Bankings;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectReceiptPayment;

namespace ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment
{
    /// <summary>
    /// Interaction logic for ucFrmDirectReceiptPO.xaml
    /// </summary>
    public partial class ucFrmDirectReceiptPayment : UserControl
    {
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        SalesReceiptRepo salesReceiptRepo = new SalesReceiptRepo();
        List<SalesReceipt> saleReceipts = new List<SalesReceipt>();

        public int groupId = 0;
        public bool editFlag = false;
        public int receiptId, paymentId;
        static SalesReceiptStatus statusChanged = new SalesReceiptStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        SalesReceiptStatus checkStatus = new SalesReceiptStatus();
        UsersRepo UsersRepo = new UsersRepo();
        int banktransactionFlag = 0;

        string stage;
        bool? isApproved;
        //bool? isReApproved;
        DateTime? approvalDate;
        List<SalesReceiptStatus> ReceiptStatuses = new List<SalesReceiptStatus>();

        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<Department> deptList = new List<Department>();
        bool? companyChnaged = null;
        public ucFrmDirectReceiptPayment()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //grdSaleReceipt.ItemsSource = saleReceipts;
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
                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass SaleReceipt Bank Account") != null)
                //{
                //    grdBypass.IsEnabled = true;
                //}
                //else
                //{
                //    grdBypass.IsEnabled = false;
                //}

                if (editFlag == false)
                {
                    datCreationDate.DateTime = DateTime.Now;
                    datglPostingdate.DateTime = DateTime.Now;
                    btnRefresh.IsEnabled = false;

                    btnPushDebits.IsChecked = true;
                    btnPushCredits.IsChecked = true;


                    if (paymentId != 0)
                    {
                        btnLoad.IsEnabled = false;
                        PaymentRepo paymentRepo = new PaymentRepo();
                        List<Payment> payments = new List<Payment>();
                        List<DirectReceiptForPaymentModelView> modelViewList = new List<DirectReceiptForPaymentModelView>();


                        payments = paymentRepo.GetPaymentsByGroupId(paymentId);

                        int index = 0;
                        //Select Company
                        var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                        if (payments[0].company != null)
                        {

                            if (payments[0].company != null && companyList.Find(x => x.Id == payments[0].company_Id) == null)
                            {
                                companyList.Add(payments[0].company);
                                lookupCompany.ItemsSource = null;
                                lookupCompany.ItemsSource = companyList;
                            }
                            //lookupCompany.Text = _bill.company.CompanyName;
                            index = 0;
                            foreach (var _company in companyList)
                            {
                                if (_company.Id == payments[0].company_Id)
                                {
                                    lookupCompany.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }
                        }


                        //var departmentList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                        //if (_LA.department != null)
                        //{

                        //    if (_LA.department != null && departmentList.Find(x => x.Id == _LA.deptId) == null)
                        //    {

                        //        departmentList.Add(_LA.department);
                        //        lookupDepartment.ItemsSource = null;
                        //        lookupDepartment.ItemsSource = departmentList;
                        //    }

                        //    index = 0;
                        //    foreach (var _dept in departmentList)
                        //    {
                        //        if (_dept.Id == _LA.deptId)
                        //        {
                        //            lookupDepartment.SelectedIndex = index;
                        //            index = 0;
                        //            break;
                        //        }
                        //        index++;
                        //    }

                        //}


                        string deptNames = "";
                        if (payments[0].departments != null)
                        {
                            deptList = payments[0].departments;
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
                        LoadCOAaccounts();

                        //Select Currency
                        var currencyList = (lookUpCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookUpCurrency.ItemsSource as List<Currency>;
                        if (payments[0].currency != null)
                        {

                            index = 0;
                            foreach (var _currency in currencyList)
                            {

                                if (_currency.Id == payments[0].currency_Id)
                                {
                                    lookUpCurrency.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }

                        }

                        lookupCompany.IsEnabled = false;
                        lookupDepartment1.IsEnabled = false;
                        lookUpCurrency.IsEnabled = false;

                        foreach (var _payment in payments)
                        {
                            if (_payment != null)
                            {

                                

                                DirectReceiptForPaymentModelView LA = new DirectReceiptForPaymentModelView();
                                double amountWithTax = 0;
                                double ReceiptAmount = 0;
                                LA.PaymentId = _payment.Id;
                                LA.PaymentCreationDate = (DateTime)_payment.CreationDate;

                                switch (_payment.transactionType)
                                {
                                    case PaymentTransactionType.Admin_Bills:
                                        if (_payment.adminBill != null && _payment.adminBill.vendor != null && _payment.adminBill.vendor.company != null)
                                            LA.vendor = _payment.adminBill.vendor.company.CompanyName;
                                        break;
                                    case PaymentTransactionType.Loans_Advances:
                                        if (_payment.loansAdvance != null && _payment.loansAdvance.vendor != null && _payment.loansAdvance.vendor.company != null)
                                            LA.vendor = _payment.loansAdvance.vendor.company.CompanyName;
                                        break;
                                    case PaymentTransactionType.Purchase_Invoice:
                                        if (_payment.purchaseInvoice != null && _payment.purchaseInvoice.Vendor != null && _payment.purchaseInvoice.Vendor.company != null)
                                            LA.vendor = _payment.purchaseInvoice.Vendor.company.CompanyName;
                                        break;
                                    case PaymentTransactionType.Target_Reward:
                                        break;
                                    case PaymentTransactionType.Vendor_Bills:
                                        if (_payment.Bill != null && _payment.Bill.vendor != null && _payment.Bill.vendor.company != null)
                                            LA.vendor = _payment.Bill.vendor.company.CompanyName;
                                        break;
                                }

                                

                                if (_payment.currency != null)
                                    LA.currency = _payment.currency.CurrencyName;

                                LA.PaymentRefNo = _payment.PaymentRefNo;
                                LA.SystemRefNo = _payment.SystemRefNo;


                                if (_payment.salesReceipts != null && _payment.salesReceipts.Count > 0)
                                    ReceiptAmount = _payment.salesReceipts.Where(y => y.isVoid != true).Sum(x => x.CollectionAmount);
                                LA.PaymentAmount = _payment.DebitedAmount;

                                if (_payment.salesReceipts != null && _payment.salesReceipts.Where(x => x.isVoid != true).ToList().Count == 0)
                                {
                                    LA.RemainingAmount = _payment.DebitedAmount;
                                }
                                else
                                {
                                    LA.RemainingAmount = _payment.DebitedAmount - _payment.salesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount);
                                }

                                LA.PaymentStage = GetPaymentStatus(_payment);

                                modelViewList.Add(LA);
                                
                            }
                        }
                        grdSaleReceipt.ItemsSource = modelViewList;

                    }

                }

                if (editFlag == true && groupId > 0)
                {
                    tblViewSaleReceipt.NewItemRowPosition = NewItemRowPosition.None;
                    saleReceipts = salesReceiptRepo.getReceiptsByGroupId(groupId);

                    cmbxReceiptType.SelectedIndex = 0;

                    if (saleReceipts[0].isBypassBank == true && saleReceipts[0].coaAccountId != null)
                    {
                        isBypassCOA.IsChecked = true;
                        lookupCOA.Text = saleReceipts[0].ChartofAccount.accountName;
                    }

                    if (saleReceipts[0].isDeposit == true)
                        btnDeposit.IsChecked = true;
                    else if (saleReceipts[0].isDeposit == false)
                        btnPayment.IsChecked = true;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances Sale Receipts") == null)
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


                    views = UsersRepo.getViwerInfo(groupId, 20);
                    grdUsers.ItemsSource = views;
                    loadcomments();


                    txtSystemRef.Text = saleReceipts[0].SystemRefNo;
                    lblPaymentRefNo.Text = " (" + saleReceipts[0].SystemRefNo + ")";


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

                    var recDeductions = saleReceipts.SelectMany(x => x.receiptDeductions).ToList();
                    var bankCharges = saleReceipts.SelectMany(x => x.bankCharges).ToList();
                    var bankTaxes = saleReceipts.SelectMany(x => x.ReceiptBankTaxes).ToList();
                    var dedTaxes = saleReceipts.SelectMany(x => x.ReceiptDeductionTaxes).ToList();
                    txtDeductionAmount.Text = Convert.ToDouble(recDeductions.Sum(x => x.Amount)).ToString();
                    txtBankCharges.Text = Convert.ToDouble(bankCharges.Sum(x => x.Amount)).ToString();

                    txtDedVAT.Text = dedTaxes.Sum(x => x.Amount).ToString();
                    txtBankChargesVAT.Text = bankTaxes.Sum(x => x.Amount).ToString();
                    if (saleReceipts[0].DeductionExchangeRate == 0)
                        txtDedER.Text = 1.ToString();
                    else
                        txtDedER.Text = saleReceipts[0].DeductionExchangeRate.ToString();


                    //if (saleReceipts[0].createdFromBill == true)
                    //{
                    lookupCompany.IsEnabled = false;
                    lookupDepartment1.IsEnabled = false;
                    lookUpCurrency.IsEnabled = false;
                    grpInterCompany.IsEnabled = false;
                    chkInterCompany.IsEnabled = false;

                    //}


                    int index = 0;


                    //Select Template
                    //for (int i = 0; i <= (int)ERP_BL.Enums.ReceiptType.Loans_Advances; i++)
                    //{

                    //    if (((ERP_BL.Enums.ReceiptType)i).ToString() == saleReceipts[0].receiptType.ToString())
                    //    {
                    //        cmbxReceiptType.SelectedIndex = i;
                    //        break;
                    //    }
                    //}

                    


                    if (saleReceipts[0].CreationDate != null)
                        datCreationDate.EditValue = (DateTime)saleReceipts[0].CreationDate;


                    if (saleReceipts[0].CreditedDate != null)
                        datCreditedDate.EditValue = (DateTime)saleReceipts[0].CreditedDate;


                    if (saleReceipts[0].InstrumentDate != null)
                        datInstrumentDate.EditValue = (DateTime)saleReceipts[0].InstrumentDate;


                    if (saleReceipts[0].DepositedDate != null)
                        datDepositedDate.EditValue = (DateTime)saleReceipts[0].DepositedDate;


                    //Select Company
                    //var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                    //if (payments[0].company != null)
                    //{
                    //    index = 0;
                    //    foreach (var _company in companyList)
                    //    {
                    //        if (_company.Id == payments[0].company_Id)
                    //        {
                    //            lookupCompany.SelectedIndex = index;
                    //            index = 0;
                    //            break;
                    //        }
                    //        index++;
                    //    }
                    //}


                    // Select Company

                    if (saleReceipts[0].company != null)
                    {

                        var companylist = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                        if (saleReceipts[0].company != null && companylist.Find(x => x.Id == saleReceipts[0].company.Id) == null)
                        {

                            companylist.Add(saleReceipts[0].company);
                            lookupCompany.ItemsSource = null;
                            lookupCompany.ItemsSource = companylist;
                            //lookupCompany.IsEnabled = false;
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
                            lookupDepartment1.ItemsSource = departmentlist;
                        }
                        deptList.Add(saleReceipts[0].department);
                    }
                    else if (saleReceipts[0].Departments != null && saleReceipts[0].Departments.Count > 0)
                    {
                        deptList = saleReceipts[0].Departments; 
                    }


                    if (deptList.Count > 0)
                    {
                        foreach (var _dept in deptList)
                        {
                            deptNames = deptNames + " | " + _dept.DeptName;
                            //if (lookupCompany.SelectedIndex > -1)
                            //    foreach (var _vendor in _dept.Vendors)
                            //    {
                            //        if (!vendors.Contains(_vendor))
                            //            vendors.Add(_vendor);
                            //    }
                            allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                        }
                        loadEmployees();
                    }
                    lookupDepartment1.EditValue = deptNames;
                    LoadCOAaccounts();


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
                        datCreditedDate.DateTime = (DateTime)saleReceipts[0].CreditedDate;

                    if (saleReceipts[0].DepositedDate != null)
                        datDepositedDate.DateTime = (DateTime)saleReceipts[0].DepositedDate;

                    if (saleReceipts[0].SystemRefNo != null)
                        txtSystemRef.Text = saleReceipts[0].SystemRefNo;

                    if (saleReceipts[0].ReceiptRefNo != null)
                        txtReceiptRef.Text = saleReceipts[0].ReceiptRefNo;

                    //Select Currency
                    var currencyList = (lookUpCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookUpCurrency.ItemsSource as List<Currency>;
                    if (saleReceipts[0].Currency != null)
                    {
                        index = 0;
                        foreach (var _currency in currencyList)
                        {
                            if (_currency.Id == saleReceipts[0].Currency.Id)
                            {
                                lookUpCurrency.SelectedIndex = index;
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


                    List<DirectReceiptForPaymentModelView> modelViewList = new List<DirectReceiptForPaymentModelView>();
                    foreach (var _receipt in saleReceipts)
                    {
                        if (_receipt != null)
                        {
                            DirectReceiptForPaymentModelView LA = new DirectReceiptForPaymentModelView();
                            double amountWithTax = 0;
                            double ReceiptAmount = 0;
                            LA.Id = _receipt.Id;
                            LA.PaymentId = _receipt.paymentId.Value;
                            LA.PaymentCreationDate = (DateTime)_receipt.payment.CreationDate;

                            switch (_receipt.payment.transactionType)
                            {
                                case PaymentTransactionType.Admin_Bills:
                                    if (_receipt.payment.adminBill != null && _receipt.payment.adminBill.vendor != null && _receipt.payment.adminBill.vendor.company != null)
                                        LA.vendor = _receipt.payment.adminBill.vendor.company.CompanyName;
                                    break;
                                case PaymentTransactionType.Loans_Advances:
                                    if (_receipt.payment.loansAdvance != null && _receipt.payment.loansAdvance.vendor != null && _receipt.payment.loansAdvance.vendor.company != null)
                                        LA.vendor = _receipt.payment.loansAdvance.vendor.company.CompanyName;
                                    break;
                                case PaymentTransactionType.Purchase_Invoice:
                                    if (_receipt.payment.purchaseInvoice != null && _receipt.payment.purchaseInvoice.Vendor != null && _receipt.payment.purchaseInvoice.Vendor.company != null)
                                        LA.vendor = _receipt.payment.purchaseInvoice.Vendor.company.CompanyName;
                                    break;
                                case PaymentTransactionType.Target_Reward:
                                    break;
                                case PaymentTransactionType.Vendor_Bills:
                                    if (_receipt.payment.Bill != null && _receipt.payment.Bill.vendor != null && _receipt.payment.Bill.vendor.company != null)
                                        LA.vendor = _receipt.payment.Bill.vendor.company.CompanyName;
                                    break;
                            }

                            LA.COAcredit = _receipt.COAcredit;

                            if (_receipt.payment.currency != null)
                                LA.currency = _receipt.payment.currency.CurrencyName;

                            LA.PaymentRefNo = _receipt.payment.PaymentRefNo;
                            LA.SystemRefNo = _receipt.payment.SystemRefNo;

                            LA.Description = _receipt.Description;

                            if (_receipt.payment.salesReceipts != null && _receipt.payment.salesReceipts.Count > 0)
                                ReceiptAmount = _receipt.payment.salesReceipts.Where(y => y.isVoid != true).Sum(x => x.CollectionAmount);
                            LA.PaymentAmount = _receipt.payment.DebitedAmount;

                            if (_receipt.payment.salesReceipts != null && _receipt.payment.salesReceipts.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                LA.RemainingAmount = _receipt.payment.DebitedAmount;
                            }
                            else
                            {
                                LA.RemainingAmount = _receipt.payment.DebitedAmount - _receipt.payment.salesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount);
                            }

                            LA.CreditedAmount = _receipt.CollectionAmount;
                            LA.Total = _receipt.CollectionAmount;

                            LA.PaymentStage = GetPaymentStatus(_receipt.payment);

                            modelViewList.Add(LA);

                        }
                    }
                    grdSaleReceipt.ItemsSource = modelViewList;



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
                    if (saleReceipts[0].holderChangeDate != null)
                    {
                        var time = DateTime.Now - saleReceipts[0].holderChangeDate;
                        txtHolderDays.Text = time.Days.ToString();
                    }
                    var creditJournalTransactions = saleReceipts[0].journalTransactions.Where(x => x.credit != 0).ToList();
                    var debitJournalTransactions = saleReceipts[0].journalTransactions.Where(x => x.debit != 0).ToList();
                    if (creditJournalTransactions.Count > 0)
                    {
                        btnPushCredits.IsChecked = true;
                    }
                    if (debitJournalTransactions.Count > 0)
                    {
                        btnPushDebits.IsChecked = true;
                    }

                    if (saleReceipts[0].GLPostingDate != null)
                    {
                        datglPostingdate.EditValue = saleReceipts[0].GLPostingDate;
                    }
                    else
                    {
                        datglPostingdate.EditValue = saleReceipts[0].CreationDate;
                    }
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
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private string GetPaymentStatus(Payment payment)
        {
            PaymentRepo paymentRepo = new PaymentRepo();
            var _payment = paymentRepo.GetPayment(payment.Id);

            if (_payment.isVoid == true)
            {
                return "Void";
            }
            else if (_payment.isReApproved == false)
            {
                return "Under Re-Approval";
            }
            else if (_payment.isApproved == true && _payment.stage == "Closed")
            {
                return "Closed";
            }
            else if (_payment.isApproved == true && _payment.Status.isActive == false && _payment.PendingForClosing != true)
            {
                return "Closed";
            }
            else if (_payment.isApproved == true && _payment.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else if (_payment.isApproved == true)
            {
                return "Approved";
            }
            else if (_payment.isApproved == false)
            {
                return "Under Approval";
            }
            else if (_payment.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else
            {
                return "No Status";
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

        private void LoadReceiptData()
        {
            //var receiptCOAs = saleReceipts.Select(x=>x.receiptCOA).ToList();
            grdSaleReceipt.ItemsSource = saleReceipts;
        }

        private string GetPOStatus(PurchaseOrder PO)
        {
            var _PO = salesReceiptRepo.GetLoansAdvance(PO.Id);
            if (_PO.isVoid == true)
            {
                return "Void";
            }
            else if (_PO.isReApproved == false)
            {
                return "Under Re-Approval";
            }
            else if (_PO.isApproved == true && _PO.stage == "Closed")
            {
                return "Closed";
            }
            else if (_PO.isApproved == true && _PO.Status.isActive == false && _PO.PendingForClosing != true)
            {
                return "Closed";
            }
            else if (_PO.isApproved == true && _PO.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else if (_PO.isApproved == true)
            {
                return "Approved";
            }
            else if (_PO.isApproved == false)
            {
                return "Under Approval";
            }
            else if (_PO.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else
            {
                return "No Status";
            }
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


        private void loadReceiptTypes()
        {
            for (int i = 3; i <= (int)ERP_BL.Enums.ReceiptType.Direct_Receipt; i++)
            {
                cmbxReceiptType.Items.Add(((ERP_BL.Enums.ReceiptType)i).ToString());
            }
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        private void loadCompanies()
        {
            //EmployeeRepo empRepo = new EmployeeRepo();
            empUser = salesReceiptRepo.GetEmployeeForSaleReceipt(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
            lookupInterCompany.ItemsSource = empUser.Companies;
        }

        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll().Where(x => x.isVoid != true).ToList();
            lookUpCurrency.ItemsSource = currencies;
        }

        private void loadCollectionMethods()
        {
            lookupCollectionMethod.ItemsSource = salesReceiptRepo.GetAllCollectionMethods();
        }



        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
                    DXMessageBox.Show("Please select Deparment!");
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
                if (lookUpCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency!");
                    lookUpCurrency.Focus();
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
                var tempDeduction = Convert.ToDouble(grdSaleReceipt.Columns["Deductions"].TotalSummaries[0].Value);
                var tempVAT = Convert.ToDouble(grdSaleReceipt.Columns["VAT"].TotalSummaries[0].Value);
                var tempCharges = Convert.ToDouble(grdSaleReceipt.Columns["BankCharges"].TotalSummaries[0].Value);
                var totalDeduction = Math.Round(double.Parse((tempDeduction + tempVAT + tempCharges).ToString()), 2);

                var dedAmount = Math.Round(Convert.ToDouble(txtDedTotal.Text) + Convert.ToDouble(txtTotalBankCharges.Text), 2);
                if (totalDeduction != dedAmount)
                {
                    DXMessageBox.Show("Deduction, VAT and Bank Charges amount not matching with Total Deduction!");
                    return;
                }

                //Total column summary
                var temp = Convert.ToDouble(grdSaleReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                var total = Math.Round(double.Parse(temp.ToString()), 2);

                if (string.IsNullOrEmpty(txtCollectionAmnt.Text) || string.IsNullOrWhiteSpace(txtCollectionAmnt.Text))
                {
                    DXMessageBox.Show("Please enter Collection amount!");
                    return;
                }

                if (total != double.Parse(txtCollectionAmnt.Text))
                {
                    DXMessageBox.Show("Collection amount is wrong!");
                    return;
                }

                List<SalesReceipt> receiptList = new List<SalesReceipt>();
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


                int index = 0;
                foreach (var row in grdSaleReceipt.VisibleItems)
                {
                    var _receipt = row as DirectReceiptForPaymentModelView;

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

                    //receipt = _receipt;
                    receipt.receiptType = ReceiptType.Direct_Receipt;
                    receipt.CreationDate = datCreationDate.DateTime;
                    //receipt.GLPostingDate = datglPostingdate.DateTime;
                    receipt.companyId = (lookupCompany.SelectedItem as Company).Id;
                    //receipt.department = lookupDepartment.SelectedItem as Department;

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

                    receipt.COAcredit_Id = _receipt.COAcredit?.Id;
                    if (cmbxPettyCashRef.SelectedIndex > 0)
                        receipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                    else
                        receipt.PettyCashRefId = null;

                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        receipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        if (datHolderDate.EditValue != null)
                            receipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }

                    receipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                    receipt.CreditedDate = datCreditedDate.DateTime;
                    receipt.DepositedDate = datDepositedDate.DateTime;
                    receipt.SystemRefNo = txtSystemRef.Text;
                    receipt.ReceiptRefNo = txtReceiptRef.Text;
                    receipt.CurrencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                    receipt.paymentId = _receipt.PaymentId;
                    receipt.CollectionAmount = _receipt.CreditedAmount;
                    receipt.saleReceiptStatus = salesReceiptRepo.GetSaleReceiptStatus((cmbStatus.SelectedItem as cmbitem).id);
                    receipt.collectionMethodId = (lookupCollectionMethod.SelectedItem as CollectionMethod).Id;
                    receipt.BankId = (lookupBanks.SelectedItem as Bank).Id;
                    receipt.AccountId = (lookupAccounts.SelectedItem as Account).Id;
                    receipt.InstrumentNo = txtInstrumentNo.Text;
                    receipt.InstrumentDate = datInstrumentDate.DateTime;


                    PaymentRepo paymentRepo = new PaymentRepo();
                    var payment = paymentRepo.GetPayment(_receipt.PaymentId);
                    int? deptId = 0;

                    switch (payment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:
                            deptId = payment.adminBill?.dept_Id;
                            break;
                        case PaymentTransactionType.Loans_Advances:
                            deptId = payment.loansAdvance?.deptId;
                            break;
                        case PaymentTransactionType.Purchase_Invoice:
                            deptId = payment.purchaseInvoice?.dept_Id;
                            break;
                        case PaymentTransactionType.Target_Reward:
                            //deptId = payment.adminBill?.dept_Id;
                            break;
                        case PaymentTransactionType.Vendor_Bills:
                            deptId = payment.Bill?.dept_Id;
                            break;
                    }
                    //comment

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
                                debit = _receipt.CreditedAmount,
                                credit = 0,
                                total = _receipt.CreditedAmount - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = deptId,
                                companyId = receipt.company.Id,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
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
                                credit = _receipt.CreditedAmount,
                                total = 0 - _receipt.CreditedAmount,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = deptId,
                                companyId = receipt.company.Id,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
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
                                debit = _receipt.CreditedAmount,
                                credit = 0,
                                total = _receipt.CreditedAmount - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = deptId,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
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
                                credit = _receipt.CreditedAmount,
                                total = 0 - _receipt.CreditedAmount,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = deptId,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
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
                    List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

                    if (banktransactionFlag == 0)
                    {
                        

                        if ((lookupAccounts.SelectedItem as Account).COA_accountId != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction bankTransaction = new JournalTransaction();

                                if (receipt.journalTransactions != null)
                                {
                                    var dbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(txtCollectionAmnt.Text) && x.accountId == (lookupAccounts.SelectedItem as Account).COA_accountId);
                                    var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                     (lookupAccounts.SelectedItem as Account).COA_accountId &&
                                     x.debit == Convert.ToDouble(txtCollectionAmnt.Text) &&
                                     x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                     x.deptId == deptId
                                     );
                                    if (dbtrans != null && dbtrans.debit == Convert.ToDouble(txtCollectionAmnt.Text))
                                    {
                                        bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                        bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                        bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        bankTransaction.debit = dbtrans.debit;
                                        bankTransaction.MER = Convert.ToDouble(txtMER.Text);
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
                                            bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                            bankTransaction.credit = 0;
                                            bankTransaction.userId = receipt.user_Id;
                                            bankTransaction.SaleReceiptId = receipt.Id;
                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                            bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                            bankTransaction.deptId = deptId;
                                            bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
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
                                        bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                        bankTransaction.credit = 0;
                                        bankTransaction.userId = receipt.user_Id;
                                        bankTransaction.SaleReceiptId = receipt.Id;
                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                        bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                        bankTransaction.deptId = deptId;
                                        bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;


                                    }
                                    journalTransactions.Add(bankTransaction);
                                }
                                else
                                {
                                    bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                    bankTransaction.credit = 0;
                                    bankTransaction.userId = receipt.user_Id;
                                    bankTransaction.SaleReceiptId = receipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                    bankTransaction.deptId = deptId;
                                    bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                    journalTransactions.Add(bankTransaction);

                                }
                            }
                            banktransactionFlag = 1;
                        }

                    }
                    if (_receipt.COAcredit != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {

                            if (receipt.journalTransactions != null)
                            {
                                var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                            receipt.COAcredit_Id &&
                                            x.credit == Math.Round(Convert.ToDouble(_receipt.CreditedAmount)) &&
                                            x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                            x.deptId == deptId
                                            );

                                if (dbTransaction == null)
                                {
                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                    receivableTransaction.accountId = _receipt.COAcredit.Id;
                                    receivableTransaction.deptId = deptId;
                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    receivableTransaction.debit = 0;
                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                    receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                                    //receivableTransaction.MER = 1;
                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    receivableTransaction.userId = receipt.user_Id;
                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                    receivableTransaction.SaleReceiptId = receipt.Id;
                                    receivableTransaction.deptId = deptId;
                                    receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                    receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;

                                    journalTransactions.Add(receivableTransaction);
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction receivableTransaction = new JournalTransaction();
                                        receivableTransaction.accountId = _receipt.COAcredit.Id;
                                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        receivableTransaction.debit = 0;
                                        receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);

                                        receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                                        //receivableTransaction.MER = 1;
                                        //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                        receivableTransaction.userId = receipt.user_Id;
                                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                        receivableTransaction.SaleReceiptId = receipt.Id;
                                        receivableTransaction.deptId = deptId;
                                        receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                        receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        receivableTransaction.isReconciled = false;
                                        receivableTransaction.reconcilationDate = null;
                                        receivableTransaction.ReconcilationId = null;
                                        receivableTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                        journalTransactions.Add(receivableTransaction);
                                    }
                                    else
                                    {
                                        JournalTransaction receivableTransaction = new JournalTransaction();
                                        receivableTransaction.accountId = _receipt.COAcredit.Id;
                                        receivableTransaction.deptId = deptId;

                                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        receivableTransaction.debit = 0;
                                        receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);

                                        receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);

                                        receivableTransaction.userId = receipt.user_Id;
                                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                        receivableTransaction.SaleReceiptId = receipt.Id;
                                        receivableTransaction.deptId = deptId;
                                        receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                        receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        receivableTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                        receivableTransaction.reconcilationType = dbTransaction.reconcilationType;
                                        receivableTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                        receivableTransaction.isReconciled = dbTransaction.isReconciled;
                                        journalTransactions.Add(receivableTransaction);

                                    }
                                }
                            }
                            else
                            {
                                JournalTransaction receivableTransaction = new JournalTransaction();
                                receivableTransaction.accountId = _receipt.COAcredit.Id;
                                receivableTransaction.deptId = deptId;
                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                receivableTransaction.debit = 0;
                                receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                                //receivableTransaction.MER = 1;
                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                receivableTransaction.userId = receipt.user_Id;
                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                receivableTransaction.SaleReceiptId = receipt.Id;
                                receivableTransaction.deptId = deptId;
                                receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;

                                journalTransactions.Add(receivableTransaction);
                            }


                        }
                    }

                    receipt.journalTransactions = journalTransactions;



                    receipt.CollectionAmount = _receipt.CreditedAmount;
                    //receipt.COAdebit_Id = _receipt.COAdebit.Id;
                    //receipt.COAcredit_Id = _receipt.COAcredit.Id;
                    receipt.Description = _receipt.Description;

                    receipt.DeductionExchangeRate = _receipt.ExchangeRate;
                    receipt.DeductionSOC = _receipt.DeductionSOC;
                    receipt.receiptDeductions = _receipt.receiptDeductions;
                    receipt.ReceiptDeductionTaxes = _receipt.receiptDedTaxes;

                    receipt.IsAdjustedDedVAT = _receipt.IsDedAdjusted;
                    receipt.bankCharges = _receipt.bankChargess;
                    receipt.ReceiptBankTaxes = _receipt.receiptBankTaxes;
                    receipt.IsAdjustedBankVAT = _receipt.IsBankAdjusted;
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

                                if(receipt.payment.transactionType== PaymentTransactionType.Admin_Bills)
                                {
                                    company=receipt.payment.adminBill.company;
                                    department=receipt.payment.adminBill.department;
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
                                    currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                    VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                });
                            }
                            receipt.VATBooks = vatBooks;
                        }
                    }

                    if (editFlag == true && saleReceipts.Find(x => x.Id == receipt.Id) != null)
                    {
                        saleReceipts[saleReceipts.FindIndex(x => x.Id == receipt.Id)] = receipt;
                    }
                    else
                    {
                        receiptList.Add(receipt);
                    }
                }
                if (editFlag == true)
                {
                    salesReceiptRepo.UpdateDirectReceipt(saleReceipts);

                    var selectedStatus = cmbStatus.SelectedItem as cmbitem;
                    if (checkStatus.Id != selectedStatus.id)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Receipt has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
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
                    salesReceiptRepo.AddDirectReceiptForPayment(receiptList);
                    DXMessageBox.Show("Successfully Added!");
                }

                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
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

                    if (saleReceipts[0].Departments != null && saleReceipts[0].Departments.Count > 0)
                    {

                        List<User> usersList = new List<User>();
                        foreach (var _dept in saleReceipts[0].Departments)
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



        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 11, "Viewed details of Receipt");
            }
            //SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSaleReceipt);
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
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

        public ucFrmDirectReceiptPayment(SalesReceiptStatus status)
        {
            statusChanged = status;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
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
                            ucFrmDirectClose.type = "LoansAdvances";

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

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
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

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    List<TreeItem> atachments = SYSTEM_STATIC.GetSRAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && groupId > 0)
            {
                if (grdSaleReceipt.SelectedItem != null)
                {
                    var idd = (grdSaleReceipt.SelectedItem as SalesReceipt).Id;
                    if (idd != 0)
                    {
                        frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.Sale_Receipt);
                        trackingWindow.ShowDialog();
                    }
                }
                else
                {
                    DXMessageBox.Show("Select any Receipt first!");
                }
            }

        }


        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                SalesReceiptRepo repo = new SalesReceiptRepo();
                List<SalesReceipt> salesReceipts = new List<SalesReceipt>();
                salesReceipts = repo.getReceiptsByGroupId(groupId);

                if (salesReceipts[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleReceipt") != null))
                {
                    if (DXMessageBox.Show("This Receipt is currently in the list of Void Receipts! Do you want to remove it from Void?", "Remove Void Receipt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        foreach (var _receipt in salesReceipts)
                        {
                            _receipt.isVoid = false;
                            repo.setSaleReceipttoVoid(_receipt.Id, false);
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

                        var res1 = MessageBox.Show("Receipt has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (salesReceipts[0].department != null && salesReceipts[0].department.Id != 0 && salesReceipts[0].company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(salesReceipts[0].department.Id, salesReceipts[0].company.Id), saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;
                                tagUsersRecommendation = win.tagRecommendationUsers;
                                ccUsersRecommendation = win.ccRecommendationUsers;
                                for (int i = 0; i < salesReceipts.Count; i++)
                                {
                                    if (tagUsers.Count > 0)
                                    {
                                        if (salesReceipts[i].transactionHolderId != tagUsers[0].employeeId)
                                        {
                                            salesReceipts[i].holderChangeDate = DateTime.Now;
                                        }
                                        salesReceipts[i].transactionHolderId = tagUsers[0].employeeId;
                                        repo.UpdateSaleReceipt(salesReceipts[i]);
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
                        if (salesReceipts[0].Currency != null)
                        {
                            symbolCurr = salesReceipts[0].Currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Receipt (Amount OC) having value: " + salesReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "Receipt UnVoided",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleReceipt") != null)
                {
                    if (DXMessageBox.Show("This Receipt is not currently in the list of Void Receipts! Do you want to move it to Void SaleReceipts?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        foreach (var _receipt in salesReceipts)
                        {
                            _receipt.isVoid = true;
                            repo.setSaleReceipttoVoid(_receipt.Id, true);
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

                        var res1 = MessageBox.Show("Receipt has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (salesReceipts[0].department != null && salesReceipts[0].department.Id != 0 && salesReceipts[0].company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(salesReceipts[0].department.Id, salesReceipts[0].company.Id), saleReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;
                                tagUsersRecommendation = win.tagRecommendationUsers;
                                ccUsersRecommendation = win.ccRecommendationUsers;
                                for (int i = 0; i < salesReceipts.Count; i++)
                                {
                                    if (tagUsers.Count > 0)
                                    {
                                        if (salesReceipts[i].transactionHolderId != tagUsers[0].employeeId)
                                        {
                                            salesReceipts[i].holderChangeDate = DateTime.Now;
                                        }
                                        salesReceipts[i].transactionHolderId = tagUsers[0].employeeId;
                                        repo.UpdateSaleReceipt(salesReceipts[i]);
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
                        if (salesReceipts[0].Currency != null)
                        {
                            symbolCurr = salesReceipts[0].Currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Receipt (Amount OC) having value: " + salesReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                            Timestamp = DateTime.Now,
                            Subject = "Receipt Voided",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
            }
        }


        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if (editFlag != false)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSRAttachmentCategories();
                }
                grdAttach1.Visibility = Visibility.Visible;
            }
        }


        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {

                if (grdSaleReceipt.SelectedItem != null)
                {


                    List<TreeItem> atachments = SYSTEM_STATIC.GetSRAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);




                    treeViewAttachments1.ItemsSource = atachments;
                    grdAttachments1.Visibility = Visibility.Visible;

                }


            }
        }

        private void LookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

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

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
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

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
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

        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
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
            var references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);

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
            var company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
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

       


        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var company = lookupCompany.SelectedItem as Company;
            //var dept = lookupDepartment.SelectedItem as Department;


            //var chartofAccounts = salesReceiptRepo.GetChartofAccountsByCompanyDept(SYSTEM_STATIC.currentUser.id, company, dept);
            //lookupCOACreditGrid.ItemsSource = chartofAccounts;
            ////lookupCOAdebitGrid.ItemsSource = chartofAccounts;
            //ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            ////  employees = cont1.GetEmployees();
            //employees = dept.employees;

            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (ERP_BL.Databases.Employee employee in employees)
            //{
            //    cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //cmbTransactionHolder.ItemsSource = cmbitems;

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
                    //if (lookupCompany.SelectedIndex > -1)
                    //    foreach (var _customer in _dept.customers)
                    //    {
                    //        if (!customers.Contains(_customer))
                    //            customers.Add(_customer);
                    //    }
                    allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                }
            }
            loadEmployees();
            LoadCOAaccounts();

            lookupDepartment1.EditValue = deptNames;

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

        }

        private void LoadCOAaccounts()
        {
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
            var company = lookupCompany.SelectedItem as Company;

            foreach (var _dept in deptList)
            {
                chartofAccounts.AddRange(  salesReceiptRepo.GetChartofAccountsByCompanyDept(SYSTEM_STATIC.currentUser.id, company, _dept));
            }
            chartofAccounts = chartofAccounts.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            lookupCOACreditGrid.ItemsSource = chartofAccounts;
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


        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void LookupCOA_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void LookupBanks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment1.Focus();
                return;
            }
            if (lookUpCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please currency first!");
            }
        }

        private void LookupBanks_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var curr = lookUpCurrency.SelectedItem as Currency; 
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();

            List<int> dept_Ids = new List<int>();
            foreach (var _dept in deptList)
            {
                dept_Ids.Add(_dept.Id);
            }

            if (lookupBanks.SelectedItem != null)
            {
                var bank = lookupBanks.SelectedItem as Bank;
                var accntList = salesReceiptRepo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();/*.Where(x => x.departments.Contains(department) && x.company.Id == (cmbxCompany.SelectedItem as Company).Id && x.accountsCategory == AccountsCategory.Company).ToList()*/
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

        private void LookupAccounts_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupBanks.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                lookupBanks.Focus();
                return;
            }
        }

        private void GrdCntrlSalesReceipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }



        private void TableViewSaleReceipt_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void TableViewSaleReceipt_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {

        }

        private void IsBypassCOA_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeposit_Checked(object sender, RoutedEventArgs e)
        {
            btnPayment.IsChecked = false;
        }

        private void BtnPayment_Checked(object sender, RoutedEventArgs e)
        {
            btnDeposit.IsChecked = false;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }



        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {

        }

        private void GrdCntrlSalesReceipt_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdSaleReceipt.GetRowByListIndex(e.ListSourceRowIndex) as DirectReceiptForPaymentModelView;
            if (e.IsGetData)
            {
                switch (e.Column.FieldName)
                {
                    case "COAdebit":
                        if (lookupAccounts.SelectedIndex > -1)
                        {
                            var accnt = lookupAccounts.SelectedItem as Account;

                            if (accnt.COAaccount != null)
                                e.Value = accnt.COAaccount.accountName;
                        }

                        break;
                    case "Deductions":
                        if (row != null)
                            e.Value = Math.Round(row.receiptDeductions == null ? 0 : row.receiptDeductions.Sum(x => x.Amount), 2);
                        break;

                    case "VAT":
                        if (row != null)
                            e.Value = Math.Round((row.receiptBankTaxes == null ? 0 : row.receiptBankTaxes.Sum(x => x.Amount)) + (row.receiptDedTaxes == null ? 0 : row.receiptDedTaxes.Sum(x => x.Amount)), 2);
                        break;

                    case "BankCharges":
                        if (row != null)
                            e.Value = Math.Round(row.bankChargess == null ? 0 : row.bankChargess.Sum(x => x.Amount), 2);
                        break;

                    case "Total":
                        if (row != null)
                        {
                            if (row.receiptDeductions != null)
                            {
                                e.Value = Math.Round((row.CreditedAmount == null ? 0 : row.CreditedAmount) + (row.receiptDeductions == null ? 0 : row.receiptDeductions.Sum(x => x.Amount)) + (row.receiptDedTaxes == null ? 0 : row.receiptDedTaxes.Sum(x => x.Amount)), 2);
                            }
                            else
                            {
                                e.Value = Math.Round(row.CreditedAmount + row.receiptDeductions.Sum(x => x.Amount), 2);

                            }
                        }
                        break;
                    case "BankChargesSOC":
                        if (row != null)
                            e.Value = Math.Round(((row.bankChargess == null ? 0 : row.bankChargess.Sum(x => x.Amount)) + (row.receiptBankTaxes == null ? 0 : row.receiptBankTaxes.Sum(x => x.Amount))) * row.ExchangeRate, 2);
                        break;

                    case "DeductionSOCC":
                        if (row != null)
                            e.Value = Math.Round(((row.receiptDeductions == null ? 0 : row.receiptDeductions.Sum(x => x.Amount)) + (row.receiptDedTaxes == null ? 0 : row.receiptDedTaxes.Sum(x => x.Amount))) * row.ExchangeRate, 2);
                        break;

                    case "TotalDeductionSOC":
                        if (row != null)
                            e.Value = Math.Round(((row.receiptDeductions == null ? 0 : row.receiptDeductions.Sum(x => x.Amount)) + (row.receiptBankTaxes == null ? 0 : row.receiptBankTaxes.Sum(x => x.Amount)) + (row.receiptDedTaxes == null ? 0 : row.receiptDedTaxes.Sum(x => x.Amount)) + (row.bankChargess == null ? 0 : row.bankChargess.Sum(x => x.Amount))) * row.ExchangeRate, 2);
                        break;
                }

            }

        }

        private void MbtnAddDeductions_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (grdSaleReceipt.SelectedItem != null)
                    {
                        var receipt = grdSaleReceipt.SelectedItem as DirectReceiptForPaymentModelView;
                        ucSalesReceiptDeduction ucSalesReceiptDeduction = new ucSalesReceiptDeduction(receipt.Id);
                        ucSalesReceiptDeduction.BankCharges = false;
                        ucSalesReceiptDeduction.ShowDialog();
                        receipt.receiptDeductions = ucSalesReceiptDeduction.finalDeductions;
                        receipt.Deductions = ucSalesReceiptDeduction.totalDeduction;
                        grdSaleReceipt.SetFocusedRowCellValue("Deductions", ucSalesReceiptDeduction.totalDeduction);

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
                DXMessageBox.Show("Please Save this Receipt first!");
                return;
            }
        }

        private void MbtnAddBankCharges_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    if (grdSaleReceipt.SelectedItem != null)
                    {
                        var receipt = grdSaleReceipt.SelectedItem as DirectReceiptForPaymentModelView;
                        ucSalesReceiptDeduction ucSalesReceiptDeduction = new ucSalesReceiptDeduction(receipt.Id);
                        ucSalesReceiptDeduction.BankCharges = true;
                        ucSalesReceiptDeduction.ShowDialog();
                        receipt.bankChargess = ucSalesReceiptDeduction.finalDeductions;
                        receipt.BankCharges = ucSalesReceiptDeduction.totalDeduction;
                        grdSaleReceipt.SetFocusedRowCellValue("BankCharges", ucSalesReceiptDeduction.totalDeduction);

                        receipt.receiptBankTaxes = ucSalesReceiptDeduction.finalTaxes;
                        receipt.bankVAT = ucSalesReceiptDeduction.totalVAT;
                        receipt.VAT = receipt.dedVAT + receipt.bankVAT;
                        receipt.TotalDeductionSOC = receipt.Deductions + receipt.BankCharges + receipt.VAT;
                        receipt.IsBankAdjusted = ucSalesReceiptDeduction.IsBankAdjusted;

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
                DXMessageBox.Show("Please Save this Receipt first!");
                return;
            }
        }


        private void TxtDeductionAmount_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();
        }

        private void TxtExchangeRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            txtBankChargesER.Text = txtDedER.Text;
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();

            int count = grdSaleReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdSaleReceipt.SetCellValue(i, grdSaleReceipt.Columns["DeductionExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }


        private void TxtTotalAmount_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            var Total = Math.Round((dedAmount + VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();
        }

        private void TxtVAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT, 2).ToString();
        }

        private void TxtBankCharges_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();
        }

        private void TxtBankChargesVAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
        }

        private void TxtTotalBankCharges_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();
        }

        private void TxtBankChargesER_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            txtDedER.Text = txtBankChargesER.Text;
            var bankCharges = Convert.ToDouble(txtBankCharges.Text);
            var exchangeRate = Convert.ToDouble(txtBankChargesER.Text);
            var VAT = Convert.ToDouble(txtBankChargesVAT.Text);
            txtTotalBankCharges.Text = Math.Round(bankCharges + VAT, 2).ToString();
            var Total = Math.Round((bankCharges + VAT) * exchangeRate, 2);
            txtBankChargesSOC.Text = Total.ToString();

            int count = grdSaleReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdSaleReceipt.SetCellValue(i, grdSaleReceipt.Columns["DeductionExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }

        private void TblViewSaleReceipt_ShowGridMenu(object sender, GridMenuEventArgs e)
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
        private void cmbTransactionHolder_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (editFlag == true)
            {
                var source = grdSaleReceipt.ItemsSource as List<SalesReceipt>;

                if (source != null)
                {
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
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
        }

        private void lookUpCurrency_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            DateTime creationDate = (DateTime)datCreationDate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (lookUpCurrency.SelectedIndex != -1 && lookupCompany.SelectedIndex != -1)
                {
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((lookUpCurrency.SelectedItem as Currency).Id, Convert.ToInt32((lookupCompany.SelectedItem as Company).CurrencyId), creationDate.Year);

                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
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
                    else
                    {
                        txtMER.Text = 1.ToString();
                    }
                }
            }
        }

        private void btnGJournal_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

            foreach (var row in grdSaleReceipt.VisibleItems)
            {
                var _receipt = row as DirectReceiptForPaymentModelView;

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

                //receipt = _receipt;
                receipt.receiptType = ReceiptType.Direct_Receipt;
                receipt.CreationDate = datCreationDate.DateTime;
                //receipt.GLPostingDate = datglPostingdate.DateTime;
                receipt.company = lookupCompany.SelectedItem as Company;

                if(_receipt.payment != null)
                {
                    switch (_receipt.payment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:
                            receipt.deptId = _receipt.payment.adminBill?.dept_Id;
                            break;
                        case PaymentTransactionType.Loans_Advances:
                            receipt.deptId = _receipt.payment.loansAdvance?.deptId;
                            break;
                        case PaymentTransactionType.Purchase_Invoice:
                            receipt.deptId = _receipt.payment.purchaseInvoice?.dept_Id;
                            break;
                        case PaymentTransactionType.Target_Reward:
                            
                            break;
                        case PaymentTransactionType.Vendor_Bills:
                            receipt.deptId = _receipt.payment.Bill?.dept_Id;
                            break;
                    }
                }
                
                receipt.COAcredit_Id = _receipt.COAcredit.Id;
                if (cmbxPettyCashRef.SelectedIndex > 0)
                    receipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                else
                    receipt.PettyCashRefId = null;

                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    receipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    receipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                receipt.CreditedDate = datCreditedDate.DateTime;
                receipt.DepositedDate = datDepositedDate.DateTime;
                receipt.SystemRefNo = txtSystemRef.Text;
                receipt.ReceiptRefNo = txtReceiptRef.Text;
                receipt.Currency = lookUpCurrency.SelectedItem as Currency;
                //receipt.CollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                receipt.saleReceiptStatus = salesReceiptRepo.GetSaleReceiptStatus((cmbStatus.SelectedItem as cmbitem).id);
                receipt.collectionMethod = lookupCollectionMethod.SelectedItem as CollectionMethod;
                receipt.BankId = (lookupBanks.SelectedItem as Bank).Id;
                receipt.AccountId = (lookupAccounts.SelectedItem as Account).Id;
                receipt.InstrumentNo = txtInstrumentNo.Text;
                receipt.InstrumentDate = datInstrumentDate.DateTime;

                if (banktransactionFlag == 0)
                {
                    if ((lookupAccounts.SelectedItem as Account).COA_accountId != null)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            JournalTransaction bankTransaction = new JournalTransaction();

                            if (receipt.journalTransactions != null)
                            {
                                var dbtrans = receipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(txtCollectionAmnt.Text) && x.accountId == (lookupAccounts.SelectedItem as Account).COA_accountId);
                                var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                 (lookupAccounts.SelectedItem as Account).COA_accountId &&
                                 x.debit == Convert.ToDouble(txtCollectionAmnt.Text) &&
                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                 x.deptId == receipt.department.Id
                                 );
                                if (dbtrans != null && dbtrans.debit == Convert.ToDouble(txtCollectionAmnt.Text))
                                {
                                    bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                    bankTransaction.coaTransactionsType = dbtrans.coaTransactionsType;
                                    bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    bankTransaction.debit = dbtrans.debit;
                                    bankTransaction.MER = Convert.ToDouble(txtMER.Text);
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
                                        bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                        bankTransaction.credit = 0;
                                        bankTransaction.userId = receipt.user_Id;
                                        bankTransaction.SaleReceiptId = receipt.Id;
                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                        bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                        bankTransaction.deptId = receipt.department.Id;
                                        bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
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
                                    bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                    bankTransaction.credit = 0;
                                    bankTransaction.userId = receipt.user_Id;
                                    bankTransaction.SaleReceiptId = receipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                    bankTransaction.deptId = receipt.department.Id;
                                    bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;


                                }
                                journalTransactions.Add(bankTransaction);
                            }
                            else
                            {
                                bankTransaction.accountId = (lookupAccounts.SelectedItem as Account).COA_accountId;
                                bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                bankTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                bankTransaction.credit = 0;
                                bankTransaction.userId = receipt.user_Id;
                                bankTransaction.SaleReceiptId = receipt.Id;
                                bankTransaction.transactionRefno = txtReceiptRef.Text;
                                bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                bankTransaction.deptId = receipt.department.Id;
                                bankTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                journalTransactions.Add(bankTransaction);

                            }
                        }
                        banktransactionFlag = 1;
                    }

                }
                if (_receipt.COAcredit != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        if (receipt.journalTransactions != null)
                        {
                            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                        receipt.COAcredit_Id &&
                                        x.credit == Math.Round(Convert.ToDouble(_receipt.CreditedAmount)) &&
                                        x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                        x.deptId == receipt.department.Id
                                        );

                            if (dbTransaction == null)
                            {
                                JournalTransaction receivableTransaction = new JournalTransaction();
                                receivableTransaction.accountId = _receipt.COAcredit.Id;
                                receivableTransaction.deptId = receipt.department.Id;
                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                receivableTransaction.debit = 0;
                                receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                                receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                                //receivableTransaction.MER = 1;
                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                receivableTransaction.userId = receipt.user_Id;
                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                receivableTransaction.SaleReceiptId = receipt.Id;
                                receivableTransaction.deptId = receipt.department.Id;
                                receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;

                                journalTransactions.Add(receivableTransaction);
                            }
                            else
                            {
                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                    receivableTransaction.accountId = _receipt.COAcredit.Id;
                                    receivableTransaction.deptId = receipt.department.Id;

                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    receivableTransaction.debit = 0;
                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);

                                    receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                                    //receivableTransaction.MER = 1;
                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    receivableTransaction.userId = receipt.user_Id;
                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                    receivableTransaction.SaleReceiptId = receipt.Id;
                                    receivableTransaction.deptId = receipt.department.Id;
                                    receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                    receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                    receivableTransaction.isReconciled = false;
                                    receivableTransaction.reconcilationDate = null;
                                    receivableTransaction.ReconcilationId = null;
                                    receivableTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                    journalTransactions.Add(receivableTransaction);
                                }
                                else
                                {
                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                    receivableTransaction.accountId = _receipt.COAcredit.Id;
                                    receivableTransaction.deptId = receipt.department.Id;

                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    receivableTransaction.debit = 0;
                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);

                                    receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);

                                    receivableTransaction.userId = receipt.user_Id;
                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                    receivableTransaction.SaleReceiptId = receipt.Id;
                                    receivableTransaction.deptId = receipt.department.Id;
                                    receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                                    receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                    receivableTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                    receivableTransaction.reconcilationType = dbTransaction.reconcilationType;
                                    receivableTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                    receivableTransaction.isReconciled = dbTransaction.isReconciled;
                                    journalTransactions.Add(receivableTransaction);

                                }
                            }
                        }
                        else
                        {
                            JournalTransaction receivableTransaction = new JournalTransaction();
                            receivableTransaction.accountId = _receipt.COAcredit.Id;
                            receivableTransaction.deptId = receipt.department.Id;
                            receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                            receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                            receivableTransaction.debit = 0;
                            receivableTransaction.MER = Math.Round(Convert.ToDouble(Convert.ToDouble(txtMER.Text)), 2);
                            receivableTransaction.credit = Convert.ToDouble(_receipt.CreditedAmount);
                            //receivableTransaction.MER = 1;
                            //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                            receivableTransaction.userId = receipt.user_Id;
                            receivableTransaction.transactionRefno = txtReceiptRef.Text;
                            receivableTransaction.SaleReceiptId = receipt.Id;
                            receivableTransaction.deptId = receipt.department.Id;
                            receivableTransaction.total = 0 - Math.Round(Convert.ToDouble(_receipt.CreditedAmount), 2);
                            receivableTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                            receivableTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;

                            journalTransactions.Add(receivableTransaction);
                        }


                    }
                }

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(journalTransactions);
                generalJournal.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
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
        public void GellAllOrdersTracking()
        {
            if (grdSaleReceipt.SelectedItem != null)
            {
                var idd = (grdSaleReceipt.SelectedItem as DirectReceiptForPaymentModelView).Id;

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

    }
}
