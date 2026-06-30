using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.LoansAdvances;
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
using ERP_BL.Procurements.InterBankTransfers;

namespace ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt
{
    /// <summary>
    /// Interaction logic for ucFrmCompanyLoanSaleReceipt.xaml
    /// </summary>
    public partial class ucFrmCompanyLoanSaleReceipt : UserControl
    {
        List<LoansAdvance> LoansAdvances = new List<LoansAdvance>();
        List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
        UsersRepo UsersRepo = new UsersRepo();
        public int groupId = 0;
        public bool editFlag = false;
        public int receiptId;
        int intGroupId;
        public int loansAdvanceId = 0;

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

        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<Department> deptList = new List<Department>();
        bool? companyChnaged = null;

        public ucFrmCompanyLoanSaleReceipt()
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
               
                if (editFlag == false)
                {

                    datCreationDate.DateTime = DateTime.Now;
                    datglPostingdate.DateTime = DateTime.Now;
                    btnRefresh.IsEnabled = false;

                    if (loansAdvanceId != 0)
                    {
                        btnLoad.IsEnabled = false;
                        AdvanceRepo LAdvanceRepo = new AdvanceRepo();
                        LoansAdvance _LA = new LoansAdvance();
                        List<CompanyLoansSaleReceiptModelView> modelViewList = new List<CompanyLoansSaleReceiptModelView>();


                        _LA = LAdvanceRepo.GetLoansAdvance(loansAdvanceId);
                        LoansAdvances.Add(_LA);

                        if (_LA != null)
                        {

                            int index = 0;
                            //Select Company
                            var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                            if (_LA.company != null)
                            {

                                if (_LA.company != null && companyList.Find(x => x.Id == _LA.companyId) == null)
                                {
                                    companyList.Add(_LA.company);
                                    lookupCompany.ItemsSource = null;
                                    lookupCompany.ItemsSource = companyList;
                                }
                                //lookupCompany.Text = _bill.company.CompanyName;
                                index = 0;
                                foreach (var _company in companyList)
                                {
                                    if (_company.Id == _LA.companyId)
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
                            if (_LA.department != null)
                            {
                                deptList.Add(_LA.department);
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
                            var currencyList = (lookUpCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookUpCurrency.ItemsSource as List<Currency>;
                            if (_LA.currency != null)
                            {

                                index = 0;
                                foreach (var _currency in currencyList)
                                {

                                    if (_currency.Id == _LA.currencyId)
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

                            CompanyLoansSaleReceiptModelView LA = new CompanyLoansSaleReceiptModelView();
                            double amountWithTax = 0;
                            double loanAmount = 0;
                            LA.LoansAdvanceId = _LA.Id;
                            LA.LACreationDate = (DateTime)_LA.CreationDate;


                            if (_LA.currency != null)
                                LA.currency = _LA.currency.CurrencyName;
                            if (_LA.isEmployee == true)
                            {
                                if (_LA.ApplicantEmployee != null)
                                    LA.Employee = _LA.ApplicantEmployee.person.FName + " " + _LA.ApplicantEmployee.person.LName;
                            }
                            else
                            {
                                if (_LA.applicantType != null)
                                    LA.ApplicantType = _LA.applicantType.TypeName;
                                if (_LA.applicant != null)
                                    LA.ApplicantName = _LA.applicant.Name;
                            }

                            LA.SystemReferenceNo = _LA.SystemRef;
                            
                            
                            loanAmount = _LA.LoanAmountOC;
                            LA.LoanAmount = loanAmount;



                            if (_LA.SalesReceipts.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                LA.RemainingAmount = loanAmount;
                            }
                            else
                            {
                                LA.RemainingAmount = loanAmount - _LA.SalesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount);
                            }

                            LA.LoansAdvanceStage = GetLAStatus(_LA);

                            modelViewList.Add(LA);
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


                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment Reference Number in Payments") != null)
                //{

                //    cmbxPettyCashRef.IsEnabled = true;
                //}

                //else
                //{

                //    cmbxPettyCashRef.IsEnabled = false;
                //}



                if (editFlag == true && groupId > 0)
                {
                    saleReceipts = salesReceiptRepo.getReceiptsByGroupIdForLA(groupId);

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
                    cmbxReceiptType.SelectedIndex = 0;




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
                            lookupDepartment1.ItemsSource = null;
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

                    LoadLoansAdvanceData();
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

        private void LoadLoansAdvanceData()
        {
            List<CompanyLoansSaleReceiptModelView> modelViewList = new List<CompanyLoansSaleReceiptModelView>();
            foreach (var _receipt in saleReceipts)
            {
                CompanyLoansSaleReceiptModelView receipt = new CompanyLoansSaleReceiptModelView();
                receipt.Id = _receipt.Id;
                receipt.LoansAdvanceId = (int)_receipt.LoansAdvanceId;
                //payment.GroupId = _payment.adminBill.transactionGroupId;
                receipt.LACreationDate = (DateTime)_receipt.loansAdvance.CreationDate;

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

                if (_receipt.loansAdvance.currency != null)
                    receipt.currency = _receipt.loansAdvance.currency.CurrencyName;
                if (_receipt.loansAdvance.isEmployee == true)
                {
                    if (_receipt.loansAdvance.ApplicantEmployee != null)
                        receipt.Employee = _receipt.loansAdvance.ApplicantEmployee.person.FName + " " + _receipt.loansAdvance.ApplicantEmployee.person.LName;
                }
                else
                {
                    if (_receipt.loansAdvance.applicantType != null)
                        receipt.ApplicantType = _receipt.loansAdvance.applicantType.TypeName;
                    if (_receipt.loansAdvance.applicant != null)
                        receipt.ApplicantName = _receipt.loansAdvance.applicant.Name;
                }

                receipt.SystemReferenceNo = _receipt.loansAdvance.SystemRef;



                receipt.LoanAmount = _receipt.loansAdvance.LoanAmountOC;


                receipt.CreditedAmount = _receipt.CollectionAmount;


                receipt.Total = _receipt.CollectionAmount;


                var amountReceived = Math.Round(_receipt.loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount), 2);
                var remainingAmount = Math.Round(receipt.LoanAmount - amountReceived, 2);


                receipt.RemainingAmount = remainingAmount;



                //bill.AmountToPay = _bill.AmountOC;


                receipt.LoansAdvanceStage = GetLAStatus(_receipt.loansAdvance);
                if (_receipt.transactionHolderId != null)
                {
                    receipt.transactionHolderId = _receipt.transactionHolderId;
                    receipt.holderChangeDate = _receipt.holderChangeDate;
                }
                else
                {
                    receipt.holderChangeDate = DateTime.Now;
                }
                if (_receipt.holderChangeDate != null)
                {
                    var time = DateTime.Now - _receipt.holderChangeDate;
                    txtHolderDays.Text = time.Days.ToString();
                }
                modelViewList.Add(receipt);
            }
            grdCntrlSalesReceipt.ItemsSource = modelViewList;
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

        private void loadReceiptTypes()
        {
            for (int i = 2; i <= (int)ERP_BL.Enums.ReceiptType.Loans_Advances; i++)
            {
                cmbxReceiptType.Items.Add(((ERP_BL.Enums.ReceiptType)i).ToString());
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


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (editFlag == false)
                //{
                //    //datCreationDate.DateTime = DateTime.Now;
                //    GroupIdCalculation();
                //    //txtSystemRef.Text = "Payment-" + intGroupId;
                //    //lblPaymentRefNo.Text = " (Payment-" + intGroupId + ")";
                //}

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

                var tempAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                var totalCollection = Math.Round(double.Parse(tempAmount.ToString()), 2);
                if (totalCollection != Convert.ToDouble(txtCollectionAmnt.Text))
                {
                    DXMessageBox.Show("Collection amount is not matching with Total amount!");
                    return;
                }

                List<CompanyLoansSaleReceiptModelView> selectedItems = new List<CompanyLoansSaleReceiptModelView>();
                foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
                {
                    if ((_item as CompanyLoansSaleReceiptModelView).Total != 0)
                        selectedItems.Add((CompanyLoansSaleReceiptModelView)_item);
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

                    receipt.receiptType = ReceiptType.Loans_Advances;
                    //receipt.paymentLoansAdvancesTemplate = (PaymentLoansAdvancesTemplate)cmbxPaymentType.SelectedIndex;
                    receipt.CreationDate = datCreationDate.DateTime;
                    receipt.GLPostingDate = datglPostingdate.DateTime;
                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        receipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        receipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }

                    //if (editFlag == false)
                    //    receipt.transactionGroupId = intGroupId;

                    receipt.company = lookupCompany.SelectedItem as Company;
                    //payment.dept_Id = (lookupDepartment.SelectedItem as Department).Id;

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

                    if (cmbxPettyCashRef.SelectedIndex > 0)
                        receipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                    else
                        receipt.PettyCashRefId = null;

                    receipt.CreditedDate = datCreditedDate.DateTime;
                    receipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
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

                    //payment.AdminBill_Id = _item.BillId;
                    receipt.LoansAdvanceId = _item.LoansAdvanceId;
                    //receipt. = _item.LACreationDate;
                    //receipt.SystemRefNo = _item.SystemReferenceNo;
                    //payment.SystemRefNo = _item.SystemRefNo;
                    //payment.BillingMonth = _item.BillingMonth;
                    //payment.billda = _item.BillDate;
                    //payment.BillAmount = _item.LoanAmount;
                    //payment.AmountDue = _item.AmountDue;
                    receipt.CollectionAmount = _item.CreditedAmount;

                    if (editFlag == false)
                    {
                        receipt.user_Id = SYSTEM_STATIC.currentUser.id;
                        //receipt.createdFromBill = createdFromBill;
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
                                debit = _item.Total,
                                credit = 0,
                                total = _item.Total - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.loansAdvance.deptId,
                                companyId = receipt.loansAdvance.companyId,
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
                                credit = _item.Total,
                                total = 0 - _item.Total,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = receipt.loansAdvance.deptId,
                                companyId = receipt.loansAdvance.companyId,
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
                                debit = _item.Total,
                                credit = 0,
                                total = _item.Total - 0,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).deptId,
                                companyId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).companyId,
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
                                credit = _item.Total,
                                total = 0 - _item.Total,
                                FinanceRefNo = txtReceiptRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).deptId,
                                companyId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).companyId,
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
                    //comment
                    List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                    var account = lookupAccounts.SelectedItem as Account;
                    var loanAdvance = salesReceiptRepo.getLAForJournalTransactions((int)receipt.LoansAdvanceId);
                    if (editFlag == true)
                    {

                        receipt.isBypassBank = true;
                        receipt.coaAccountId = null;
                        var dbTrans = receipt.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.debit == Convert.ToDouble(txtCollectionAmnt.Text));
                        if (banktransactionFlag == 0 && receipt.isVoid != true)
                        {
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (account != null)
                            {
                                if (account.COA_accountId != null)
                                {
                                    if (btnPushDebits.IsChecked == true)
                                    {

                                        if (dbTrans == null)
                                        {
                                            bankTransaction.accountId = account.COA_accountId;
                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                            bankTransaction.creationDate = receipt.GLPostingDate;
                                            bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                            bankTransaction.credit = 0;
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                            bankTransaction.userId = receipt.user_Id;
                                            bankTransaction.SaleReceiptId = receipt.Id;
                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                            bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                            bankTransaction.deptId = loanAdvance.deptId;
                                            bankTransaction.companyId = loanAdvance.companyId;
                                            bankTransaction.currencyId = loanAdvance.currencyId;

                                        }
                                        else
                                        {
                                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                bankTransaction.accountId = dbTrans.accountId;
                                                bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                                bankTransaction.creationDate = dbTrans.creationDate;
                                                bankTransaction.debit = dbTrans.debit;
                                                bankTransaction.credit = dbTrans.credit;
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                                bankTransaction.userId = dbTrans.userId;
                                                bankTransaction.SaleReceiptId = dbTrans.SaleReceiptId;
                                                bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                                bankTransaction.total = dbTrans.total;
                                                bankTransaction.deptId = dbTrans.deptId;
                                                bankTransaction.companyId = dbTrans.companyId;
                                                bankTransaction.currencyId = dbTrans.currencyId;
                                                bankTransaction.reconcilationDate = null;
                                                bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                bankTransaction.ReconcilationId = null;
                                                bankTransaction.isReconciled = false;

                                            }
                                            else
                                            {
                                                bankTransaction.accountId = dbTrans.accountId;
                                                bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                                bankTransaction.creationDate = dbTrans.creationDate;
                                                bankTransaction.debit = dbTrans.debit;
                                                bankTransaction.credit = dbTrans.credit;
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                                bankTransaction.userId = dbTrans.userId;
                                                bankTransaction.SaleReceiptId = dbTrans.SaleReceiptId;
                                                bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                                bankTransaction.total = dbTrans.total;
                                                bankTransaction.deptId = dbTrans.deptId;
                                                bankTransaction.companyId = dbTrans.companyId;
                                                bankTransaction.currencyId = dbTrans.currencyId;
                                            }
                                        }
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }

                                }
                            }
                        }



                        if (receipt.LoansAdvanceId != null && receipt.isVoid != true)
                        {
                            if (loanAdvance.applicantType != null)
                            {
                                if (loanAdvance.applicantType.account != null)
                                {
                                    if (btnPushCredits.IsChecked == true)
                                    {

                                        var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 loanAdvance.applicantType.accountId &&
                                                 x.credit == _item.CreditedAmount &&
                                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                                 x.deptId == loanAdvance.deptId
                                                 );


                                        if (dbTransaction == null)
                                        {
                                            JournalTransaction BillTransaction = new JournalTransaction()
                                            {
                                                accountId = loanAdvance.applicantType.accountId,
                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                creationDate = receipt.GLPostingDate,
                                                credit = _item.CreditedAmount,
                                                debit = 0,
                                                MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                userId = receipt.user_Id,
                                                SaleReceiptId = receipt.Id,
                                                transactionRefno = txtReceiptRef.Text,
                                                total = 0 - _item.CreditedAmount,
                                                deptId = loanAdvance.deptId,
                                                companyId = loanAdvance.companyId,
                                                currencyId = loanAdvance.currencyId,

                                            };
                                            journalTransactions.Add(BillTransaction);
                                        }
                                        else
                                        {
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction BillTransaction = new JournalTransaction()
                                                {
                                                    accountId = loanAdvance.applicantType.accountId,
                                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                    creationDate = receipt.GLPostingDate,
                                                    credit = _item.CreditedAmount,
                                                    debit = 0,
                                                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                    userId = receipt.user_Id,
                                                    SaleReceiptId = receipt.Id,
                                                    transactionRefno = txtReceiptRef.Text,
                                                    total = 0 - _item.CreditedAmount,
                                                    deptId = loanAdvance.deptId,
                                                    companyId = loanAdvance.companyId,
                                                    currencyId = loanAdvance.currencyId,
                                                    reconcilationDate = null,
                                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                                    ReconcilationId = null,
                                                    isReconciled = false

                                                };
                                                journalTransactions.Add(BillTransaction);
                                            }
                                            else
                                            {
                                                JournalTransaction BillTransaction = new JournalTransaction()
                                                {
                                                    accountId = loanAdvance.applicantType.accountId,
                                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                    creationDate = receipt.GLPostingDate,
                                                    credit = _item.CreditedAmount,
                                                    debit = 0,
                                                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                    userId = receipt.user_Id,
                                                    SaleReceiptId = receipt.Id,
                                                    transactionRefno = txtReceiptRef.Text,
                                                    total = 0 - _item.CreditedAmount,
                                                    deptId = loanAdvance.deptId,
                                                    companyId = loanAdvance.companyId,
                                                    currencyId = loanAdvance.currencyId,
                                                    reconcilationDate = dbTransaction.reconcilationDate,
                                                    reconcilationType = dbTransaction.reconcilationType,
                                                    ReconcilationId = dbTransaction.ReconcilationId,
                                                    isReconciled = dbTransaction.isReconciled,
                                                };
                                                journalTransactions.Add(BillTransaction);
                                            }
                                        }
                                    }
                                }
                            }
                            //else
                            //if (loanAdvance.ApplicantEmployee != null)
                            //{
                            //    if (loanAdvance.ApplicantEmployee.receivableAccount != null)
                            //    {
                            //        if (btnPushCredits.IsChecked == true)
                            //        {
                            //            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                            //                    loanAdvance.ApplicantEmployee.receivableAccountId &&
                            //                    x.credit == _item.CreditedAmount &&
                            //                    x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                            //                    x.deptId == loanAdvance.deptId
                            //                    );

                            //            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            //            {
                            //                JournalTransaction BillTransaction = new JournalTransaction()
                            //                {
                            //                    accountId = loanAdvance.ApplicantEmployee.receivableAccountId,
                            //                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                            //                    creationDate = receipt.GLPostingDate,
                            //                    credit = _item.CreditedAmount,
                            //                    debit = 0,
                            //                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                            //                    userId = receipt.user_Id,
                            //                    SaleReceiptId = receipt.Id,
                            //                    transactionRefno = txtReceiptRef.Text,
                            //                    total =  0- _item.CreditedAmount,
                            //                    deptId = loanAdvance.deptId,
                            //                    companyId = loanAdvance.companyId,
                            //                    currencyId = loanAdvance.currencyId,
                            //                    reconcilationDate = null,
                            //                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                            //                    ReconcilationId = null,
                            //                    isReconciled = false
                            //                };
                            //                journalTransactions.Add(BillTransaction);
                            //            }
                            //            else
                            //            {
                            //                JournalTransaction BillTransaction = new JournalTransaction()
                            //                {
                            //                    accountId = loanAdvance.ApplicantEmployee.receivableAccountId,
                            //                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                            //                    creationDate = receipt.GLPostingDate,
                            //                    credit = _item.CreditedAmount,
                            //                    debit = 0,
                            //                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                            //                    userId = receipt.user_Id,
                            //                    SaleReceiptId = receipt.Id,
                            //                    transactionRefno = txtReceiptRef.Text,
                            //                    total = _item.CreditedAmount - 0,
                            //                    deptId = loanAdvance.deptId,
                            //                    companyId = loanAdvance.companyId,
                            //                    currencyId = loanAdvance.currencyId,
                            //                    reconcilationDate = dbTransaction.reconcilationDate,
                            //                    reconcilationType = dbTransaction.reconcilationType,
                            //                    ReconcilationId = dbTransaction.ReconcilationId,
                            //                    isReconciled = dbTransaction.isReconciled,
                            //                };
                            //                journalTransactions.Add(BillTransaction);

                            //            }
                            //        }
                            //    }
                            //}
                        }
                        receipt.journalTransactions = journalTransactions;
                    }
                    else
                    {
                        if (isBypassCOA.IsChecked == true)
                        {
                            receipt.isBypassBank = true;
                            receipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (receipt.coaAccountId != null)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    bankTransaction.accountId = receipt.coaAccountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    bankTransaction.creationDate = receipt.GLPostingDate;
                                    bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                    bankTransaction.credit = 0;
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                    bankTransaction.userId = receipt.user_Id;
                                    bankTransaction.SaleReceiptId = receipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                    bankTransaction.deptId = loanAdvance.deptId;
                                    bankTransaction.companyId = loanAdvance.companyId;
                                    bankTransaction.currencyId = loanAdvance.currencyId;

                                }
                                journalTransactions.Add(bankTransaction);
                                banktransactionFlag = 1;
                            }
                            if (loanAdvance != null)
                            {


                            }
                        }
                        else
                        {
                            receipt.isBypassBank = false;
                            receipt.coaAccountId = null;
                            if (banktransactionFlag == 0 && receipt.isVoid != true)
                            {
                                JournalTransaction bankTransaction = new JournalTransaction();
                                if (account != null)
                                {
                                    if (account.COA_accountId != null)
                                    {
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            bankTransaction.accountId = account.COA_accountId;
                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                            bankTransaction.creationDate = receipt.GLPostingDate;
                                            bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                            bankTransaction.credit = 0;
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                            bankTransaction.userId = receipt.user_Id;
                                            bankTransaction.SaleReceiptId = receipt.Id;
                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                            bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                            bankTransaction.deptId = loanAdvance.deptId;
                                            bankTransaction.companyId = loanAdvance.companyId;
                                            bankTransaction.currencyId = loanAdvance.currencyId;
                                            journalTransactions.Add(bankTransaction);
                                            banktransactionFlag = 1;
                                        }
                                    }
                                }
                            }
                            if (receipt.LoansAdvanceId != null && receipt.isVoid != true)
                            {
                                if (loanAdvance.applicantType != null)
                                {
                                    if (loanAdvance.applicantType.account != null)
                                    {
                                        if (btnPushCredits.IsChecked == true)
                                        {
                                            JournalTransaction BillTransaction = new JournalTransaction()
                                            {
                                                accountId = loanAdvance.applicantType.accountId,
                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                creationDate = receipt.GLPostingDate,
                                                credit = _item.CreditedAmount,
                                                debit = 0,
                                                MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                userId = receipt.user_Id,
                                                SaleReceiptId = receipt.Id,
                                                transactionRefno = txtReceiptRef.Text,
                                                total = 0 - _item.CreditedAmount,
                                                deptId = loanAdvance.deptId,
                                                companyId = loanAdvance.companyId,
                                                currencyId = loanAdvance.currencyId,

                                            };
                                            journalTransactions.Add(BillTransaction);
                                        }
                                    }
                                }
                            }

                        }
                        receipt.journalTransactions = journalTransactions;




                        if (editFlag == true)
                            saleReceipts[saleReceipts.FindIndex(x => x.Id == receipt.Id)] = receipt;
                        else
                            receiptList.Add(receipt);
                    }
                }
                if (editFlag == true)
                {
                    salesReceiptRepo.UpdateLAReceipt(saleReceipts);

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
                    salesReceiptRepo.AddLAReceipt(receiptList);
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

        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastReceiptId = salesReceiptRepo.GetLastSaleReceiptId();
            if (lastReceiptId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastReceiptId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }

        private void LoadGridData()
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                lookupCompany.Focus();
                return;
            }
            if (deptList != null && deptList.Count > 0)
            {
                DXMessageBox.Show("Please Select Department!");
                lookupDepartment1.Focus();
                return;
            }
            if (lookUpCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Currency!");
                lookUpCurrency.Focus();
                return;
            }


            AdvanceRepo loansAdvanceRepo = new AdvanceRepo();



            LoansAdvances = loansAdvanceRepo.getLoansAdvanceForSR((lookupCompany.SelectedItem as Company).Id, deptList, (lookUpCurrency.SelectedItem as Currency).Id);


            List<CompanyLoansSaleReceiptModelView> modelViewList = new List<CompanyLoansSaleReceiptModelView>();

            foreach (var _LA in LoansAdvances)
            {
                CompanyLoansSaleReceiptModelView LA = new CompanyLoansSaleReceiptModelView();
                double amountWithTax = 0;
                double loanAmount = 0;
                LA.LoansAdvanceId = _LA.Id;
                LA.LACreationDate = (DateTime)_LA.CreationDate;



                if (_LA.currency != null)
                    LA.currency = _LA.currency.CurrencyName;
                if (_LA.isEmployee == true)
                {
                    if (_LA.ApplicantEmployee != null)
                        LA.Employee = _LA.ApplicantEmployee.person.FName + " " + _LA.ApplicantEmployee.person.LName;
                }
                else
                {
                    if (_LA.applicantType != null)
                        LA.ApplicantType = _LA.applicantType.TypeName;
                    if (_LA.applicant != null)
                        LA.ApplicantName = _LA.applicant.Name;
                }

                LA.SystemReferenceNo = _LA.SystemRef;


                loanAmount = _LA.Payments.Sum(x => x.DebitedAmount);
                LA.LoanAmount = loanAmount;


                if (_LA.SalesReceipts.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    LA.RemainingAmount = loanAmount;
                }

                else
                {

                    LA.RemainingAmount = loanAmount - _LA.SalesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount);
                }
                LA.LoansAdvanceStage = GetLAStatus(_LA);
                modelViewList.Add(LA);
            }
            grdCntrlSalesReceipt.ItemsSource = modelViewList;
        }

        private string GetLAStatus(LoansAdvance LA)
        {

            var _LA = salesReceiptRepo.GetLoansAdvance(LA.Id);
            if (_LA.isVoid == true)
            {
                return "Void";
            }
            else if (_LA.isReApproved == false)
            {
                return "Under Re-Approval";
            }
            else if (_LA.isApproved == true && _LA.stage == "Closed")
            {
                return "Closed";
            }
            else if (_LA.isApproved == true && _LA.Status.isActive == false && _LA.PendingForClosing != true)
            {
                return "Closed";
            }
            else if (_LA.isApproved == true && _LA.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else if (_LA.isApproved == true)
            {
                return "Approved";
            }
            else if (_LA.isApproved == false)
            {
                return "Under Approval";
            }
            else if (_LA.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else
            {
                return "No Status";
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadGridData();
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

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
                UsersRepo.Add(TransactionInfo.viewed, groupId, 11, "Viewed details of Sale Receipt");
            }
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlSalesReceipt);
        }

        public ucFrmCompanyLoanSaleReceipt(SalesReceiptStatus status)
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

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
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

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {




                List<TreeItem> atachments = SYSTEM_STATIC.GetSRAttachmentsListByCategory(groupId, TransactionItemType.Sale_Receipt);




                treeViewAttachments1.ItemsSource = atachments;
                grdAttachments1.Visibility = Visibility.Visible;




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
                    if (DXMessageBox.Show("This Receipt is currently in the list of Void Sale Receipts! Do you want to remove it from Void?", "Remove Void Sale Receipt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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

                        var res1 = MessageBox.Show("Sale Receipt has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
                            Comment = "Sale Receipt (Amount OC) having value: " + salesReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "Sale Receipt UnVoided",
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
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleReceipt") != null)
                {
                    if (DXMessageBox.Show("This Receipt is not currently in the list of Void Sale Receipts! Do you want to move it to Void SaleReceipts?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

                        var res1 = MessageBox.Show("Sale Receipt has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
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
                            Comment = "Sale Receipt (Amount OC) having value: " + salesReceipts[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                            Timestamp = DateTime.Now,
                            Subject = "Sale Receipt Voided",
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
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + salesReceipts[0].ReceiptRefNo, salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                }
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

        //Only to get gridControl of LookupDepartment1
        GridControl gridControl = new GridControl();
        private void LookupDepartment1_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            List<Vendor> vendors = new List<Vendor>();
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

                    allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                }
            }
            loadEmployees();

            lookupDepartment1.EditValue = deptNames;

            //var department = lookupDepartment.SelectedItem as Department;
            //var vendors = department.Vendors.ToList();

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
                else if(saleReceipts[0].Departments != null && saleReceipts[0].Departments.Count > 0)
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

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //Saving the Selected Company
            //var department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;


            //ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();

            //employees = department.employees;

            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (ERP_BL.Databases.Employee employee in employees)
            //{
            //    cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //cmbTransactionHolder.ItemsSource = cmbitems;

        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //CompanyRepo companyRepo = new CompanyRepo();
            //Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);
            //List<Department> departments = new List<Department>();

            //if (company != null)
            //{
            //    if (company.departments != null)
            //    {
            //        foreach (var _dept in empUser.departments.Where(x => x.IsProcurementType == true && x.isActive == true))
            //        {
            //            if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
            //                departments.Add(_dept);
            //        }
            //        if (saleReceipts != null && saleReceipts.Count > 0 && editFlag == true)
            //            if (saleReceipts[0].department != null)
            //                if (departments.FirstOrDefault(x => x.Id == saleReceipts[0].department.Id) == null)
            //                    departments.Add(saleReceipts[0].department);
            //    }

            //    lookupDepartment.ItemsSource = departments;

            //    loadBillReferenceNo();

            //    SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            //    var banks = receiptRepo.GetBanksbyCompany(company);
            //    lookupBanks.ItemsSource = banks;
            //}


            CompanyRepo companyRepo = new CompanyRepo();

            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();

            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.IsProcurementType == true && x.isActive == true))
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
                //lookupCreditCardBank.ItemsSource = banks;
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

        private void LookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
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

        private void ComboBoxEdit_ReceiptType_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void IsBypassCOA_Checked(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex > -1 && deptList != null && deptList.Count > 0)
            {
                ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
                List<ChartofAccount> userChartofAccounts = new List<ChartofAccount>();

                foreach (var _dept in deptList)
                {
                    userChartofAccounts.AddRange(chartofAccountsRepo.GetChartofAccountsByCompanyDept(SYSTEM_STATIC.currentUser.id, lookupCompany.SelectedItem as Company, _dept));
                }

                userChartofAccounts = userChartofAccounts.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                lookupCOA.ItemsSource = userChartofAccounts;
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

        private void LookupAccounts_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupBanks.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                lookupBanks.Focus();
                return;
            }
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadLoansAdvanceData();
        }

        private void GrdCntrlSalesReceipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void GrdCntrlSalesReceipt_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void GrdCntrlPayment_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TableViewSaleReceipt_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as CompanyLoansSaleReceiptModelView;
            var debt = row.CreditedAmount;

            var rcpt = saleReceipts.FirstOrDefault(x => x.Id == row.Id);

            if (editFlag == true)
            {
                var amountReceived = rcpt.loansAdvance.SalesReceipts.Where(x => x.isVoid != true && x.Id != rcpt.Id).Sum(y => y.CollectionAmount);
                amountReceived = Math.Round(amountReceived + debt /*+ ded*/, 2);

                if (amountReceived > rcpt.loansAdvance.LoanAmountOC)
                {
                    DXMessageBox.Show("Credited Amount cannot exceed Loan Amount!");
                    row.CreditedAmount = rcpt.CollectionAmount;

                    //row.Deductions = pymnt.Deductions;
                    row.Total = rcpt.CollectionAmount;
                    return;
                }
            }

            if (row.CreditedAmount > Math.Round(row.RemainingAmount, 2) && editFlag == false)
            {
                DXMessageBox.Show("Credited amount cannot exceed Amount Due!");
                ((DataViewBase)sender).Background = Brushes.LightBlue;
                row.CreditedAmount = 0;
                return;
            }

            var b = (CompanyLoansSaleReceiptModelView)grdCntrlSalesReceipt.GetRow(e.RowHandle);
            b.Total = debt;

            foreach (var _item in grdCntrlSalesReceipt.ItemsSource as List<CompanyLoansSaleReceiptModelView>)
            {
                var amount = _item.Total;
            }
        }

        private void TableViewSaleReceipt_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && groupId > 0)
            {
                if (grdCntrlSalesReceipt.SelectedItem != null)
                {
                    var idd = (grdCntrlSalesReceipt.SelectedItem as CompanyLoansSaleReceiptModelView).Id;
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

        }

        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
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
        public List<JournalTransaction> getJournalTransactions()
        {
            List<CompanyLoansSaleReceiptModelView> selectedItems = new List<CompanyLoansSaleReceiptModelView>();
            foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
            {
                if ((_item as CompanyLoansSaleReceiptModelView).Total != 0)
                    selectedItems.Add((CompanyLoansSaleReceiptModelView)_item);
            }
            List<JournalTransaction> finalJournalTransactions = new List<JournalTransaction>();
            List<SalesReceipt> receiptList = new List<SalesReceipt>();
            int index = 0;
            foreach (var _item in selectedItems)
            {
                SalesReceipt receipt = new SalesReceipt();
                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                receipt = salesReceiptRepo.GetSaleReceiptById(_item.Id);

                var account = lookupAccounts.SelectedItem as Account;
                var loanAdvance = salesReceiptRepo.getLAForJournalTransactions((int)receipt.LoansAdvanceId);
                if (editFlag == true)
                {

                    receipt.isBypassBank = true;
                    receipt.coaAccountId = null;
                    var dbTrans = receipt.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.debit == Convert.ToDouble(txtCollectionAmnt.Text));
                    if (banktransactionFlag == 0 && receipt.isVoid != true)
                    {
                        JournalTransaction bankTransaction = new JournalTransaction();
                        if (account != null)
                        {
                            if (account.COA_accountId != null)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {

                                    if (dbTrans == null)
                                    {
                                        bankTransaction.accountId = account.COA_accountId;
                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        bankTransaction.creationDate = receipt.GLPostingDate;
                                        bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                        bankTransaction.credit = 0;
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                        bankTransaction.userId = receipt.user_Id;
                                        bankTransaction.SaleReceiptId = receipt.Id;
                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                        bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                        bankTransaction.deptId = loanAdvance.deptId;
                                        bankTransaction.companyId = loanAdvance.companyId;
                                        bankTransaction.currencyId = loanAdvance.currencyId;

                                    }
                                    else
                                    {
                                        if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                        {
                                            bankTransaction.accountId = dbTrans.accountId;
                                            bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                            bankTransaction.creationDate = dbTrans.creationDate;
                                            bankTransaction.debit = dbTrans.debit;
                                            bankTransaction.credit = dbTrans.credit;
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                            bankTransaction.userId = dbTrans.userId;
                                            bankTransaction.SaleReceiptId = dbTrans.SaleReceiptId;
                                            bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                            bankTransaction.total = dbTrans.total;
                                            bankTransaction.deptId = dbTrans.deptId;
                                            bankTransaction.companyId = dbTrans.companyId;
                                            bankTransaction.currencyId = dbTrans.currencyId;
                                            bankTransaction.reconcilationDate = null;
                                            bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                            bankTransaction.ReconcilationId = null;
                                            bankTransaction.isReconciled = false;

                                        }
                                        else
                                        {
                                            bankTransaction.accountId = dbTrans.accountId;
                                            bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                            bankTransaction.creationDate = dbTrans.creationDate;
                                            bankTransaction.debit = dbTrans.debit;
                                            bankTransaction.credit = dbTrans.credit;
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                            bankTransaction.userId = dbTrans.userId;
                                            bankTransaction.SaleReceiptId = dbTrans.SaleReceiptId;
                                            bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                            bankTransaction.total = dbTrans.total;
                                            bankTransaction.deptId = dbTrans.deptId;
                                            bankTransaction.companyId = dbTrans.companyId;
                                            bankTransaction.currencyId = dbTrans.currencyId;
                                        }
                                    }
                                    journalTransactions.Add(bankTransaction);
                                    banktransactionFlag = 1;
                                }

                            }
                        }
                    }



                    if (receipt.LoansAdvanceId != null && receipt.isVoid != true)
                    {
                        if (loanAdvance.applicantType != null)
                        {
                            if (loanAdvance.applicantType.account != null)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {

                                    var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                             loanAdvance.applicantType.accountId &&
                                             x.credit == _item.CreditedAmount &&
                                             x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                             x.deptId == loanAdvance.deptId
                                             );


                                    if (dbTransaction == null)
                                    {
                                        JournalTransaction BillTransaction = new JournalTransaction()
                                        {
                                            accountId = loanAdvance.applicantType.accountId,
                                            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                            creationDate = receipt.GLPostingDate,
                                            credit = _item.CreditedAmount,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                            userId = receipt.user_Id,
                                            SaleReceiptId = receipt.Id,
                                            transactionRefno = txtReceiptRef.Text,
                                            total = 0 - _item.CreditedAmount,
                                            deptId = loanAdvance.deptId,
                                            companyId = loanAdvance.companyId,
                                            currencyId = loanAdvance.currencyId,

                                        };
                                        journalTransactions.Add(BillTransaction);
                                    }
                                    else
                                    {
                                        if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                        {
                                            JournalTransaction BillTransaction = new JournalTransaction()
                                            {
                                                accountId = loanAdvance.applicantType.accountId,
                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                creationDate = receipt.GLPostingDate,
                                                credit = _item.CreditedAmount,
                                                debit = 0,
                                                MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                userId = receipt.user_Id,
                                                SaleReceiptId = receipt.Id,
                                                transactionRefno = txtReceiptRef.Text,
                                                total = 0 - _item.CreditedAmount,
                                                deptId = loanAdvance.deptId,
                                                companyId = loanAdvance.companyId,
                                                currencyId = loanAdvance.currencyId,
                                                reconcilationDate = null,
                                                reconcilationType = ReconcilationType.Uncleared_Transactions,
                                                ReconcilationId = null,
                                                isReconciled = false

                                            };
                                            journalTransactions.Add(BillTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction BillTransaction = new JournalTransaction()
                                            {
                                                accountId = loanAdvance.applicantType.accountId,
                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                creationDate = receipt.GLPostingDate,
                                                credit = _item.CreditedAmount,
                                                debit = 0,
                                                MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                userId = receipt.user_Id,
                                                SaleReceiptId = receipt.Id,
                                                transactionRefno = txtReceiptRef.Text,
                                                total = 0 - _item.CreditedAmount,
                                                deptId = loanAdvance.deptId,
                                                companyId = loanAdvance.companyId,
                                                currencyId = loanAdvance.currencyId,
                                                reconcilationDate = dbTransaction.reconcilationDate,
                                                reconcilationType = dbTransaction.reconcilationType,
                                                ReconcilationId = dbTransaction.ReconcilationId,
                                                isReconciled = dbTransaction.isReconciled,
                                            };
                                            journalTransactions.Add(BillTransaction);
                                        }
                                    }
                                }
                            }
                        }
                        //else
                        //if (loanAdvance.ApplicantEmployee != null)
                        //{
                        //    if (loanAdvance.ApplicantEmployee.receivableAccount != null)
                        //    {
                        //        if (btnPushCredits.IsChecked == true)
                        //        {
                        //            var dbTransaction = receipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                        //                    loanAdvance.ApplicantEmployee.receivableAccountId &&
                        //                    x.credit == _item.CreditedAmount &&
                        //                    x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                        //                    x.deptId == loanAdvance.deptId
                        //                    );

                        //            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                        //            {
                        //                JournalTransaction BillTransaction = new JournalTransaction()
                        //                {
                        //                    accountId = loanAdvance.ApplicantEmployee.receivableAccountId,
                        //                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                        //                    creationDate = receipt.GLPostingDate,
                        //                    credit = _item.CreditedAmount,
                        //                    debit = 0,
                        //                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                        //                    userId = receipt.user_Id,
                        //                    SaleReceiptId = receipt.Id,
                        //                    transactionRefno = txtReceiptRef.Text,
                        //                    total = 0 - _item.CreditedAmount,
                        //                    deptId = loanAdvance.deptId,
                        //                    companyId = loanAdvance.companyId,
                        //                    currencyId = loanAdvance.currencyId,
                        //                    reconcilationDate = null,
                        //                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                        //                    ReconcilationId = null,
                        //                    isReconciled = false
                        //                };
                        //                journalTransactions.Add(BillTransaction);
                        //            }
                        //            else
                        //            {
                        //                JournalTransaction BillTransaction = new JournalTransaction()
                        //                {
                        //                    accountId = loanAdvance.ApplicantEmployee.receivableAccountId,
                        //                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                        //                    creationDate = receipt.GLPostingDate,
                        //                    credit = _item.CreditedAmount,
                        //                    debit = 0,
                        //                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                        //                    userId = receipt.user_Id,
                        //                    SaleReceiptId = receipt.Id,
                        //                    transactionRefno = txtReceiptRef.Text,
                        //                    total = _item.CreditedAmount - 0,
                        //                    deptId = loanAdvance.deptId,
                        //                    companyId = loanAdvance.companyId,
                        //                    currencyId = loanAdvance.currencyId,
                        //                    reconcilationDate = dbTransaction.reconcilationDate,
                        //                    reconcilationType = dbTransaction.reconcilationType,
                        //                    ReconcilationId = dbTransaction.ReconcilationId,
                        //                    isReconciled = dbTransaction.isReconciled,
                        //                };
                        //                journalTransactions.Add(BillTransaction);

                        //            }
                        //        }
                        //    }
                        //}
                    }
                    receipt.journalTransactions = journalTransactions;
                }
                else
                {
                    if (isBypassCOA.IsChecked == true)
                    {
                        receipt.isBypassBank = true;
                        receipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                        JournalTransaction bankTransaction = new JournalTransaction();
                        if (receipt.coaAccountId != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                bankTransaction.accountId = receipt.coaAccountId;
                                bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                bankTransaction.creationDate = receipt.GLPostingDate;
                                bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                bankTransaction.credit = 0;
                                bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                bankTransaction.userId = receipt.user_Id;
                                bankTransaction.SaleReceiptId = receipt.Id;
                                bankTransaction.transactionRefno = txtReceiptRef.Text;
                                bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                bankTransaction.deptId = loanAdvance.deptId;
                                bankTransaction.companyId = loanAdvance.companyId;
                                bankTransaction.currencyId = loanAdvance.currencyId;

                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                        if (loanAdvance != null)
                        {


                        }
                    }
                    else
                    {
                        receipt.isBypassBank = false;
                        receipt.coaAccountId = null;
                        if (banktransactionFlag == 0 && receipt.isVoid != true)
                        {
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (account != null)
                            {
                                if (account.COA_accountId != null)
                                {
                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        bankTransaction.accountId = account.COA_accountId;
                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                        bankTransaction.creationDate = receipt.GLPostingDate;
                                        bankTransaction.debit = Convert.ToDouble(txtCollectionAmnt.Text);
                                        bankTransaction.credit = 0;
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                        bankTransaction.userId = receipt.user_Id;
                                        bankTransaction.SaleReceiptId = receipt.Id;
                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                        bankTransaction.total = Convert.ToDouble(txtCollectionAmnt.Text) - 0;
                                        bankTransaction.deptId = loanAdvance.deptId;
                                        bankTransaction.companyId = loanAdvance.companyId;
                                        bankTransaction.currencyId = loanAdvance.currencyId;
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }
                                }
                            }
                        }
                        if (loanAdvance != null)
                        {


                        }
                    }
                    if (loanAdvance.applicantType != null)
                    {
                        if (loanAdvance.applicantType.account != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {

                                JournalTransaction BillTransaction = new JournalTransaction()
                                {
                                    accountId = loanAdvance.applicantType.accountId,
                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    creationDate = receipt.GLPostingDate,
                                    credit = _item.CreditedAmount,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                    userId = receipt.user_Id,
                                    SaleReceiptId = receipt.Id,
                                    transactionRefno = txtReceiptRef.Text,
                                    total = 0 - _item.CreditedAmount,
                                    deptId = loanAdvance.deptId,
                                    companyId = loanAdvance.companyId,
                                    currencyId = loanAdvance.currencyId,

                                };
                                journalTransactions.Add(BillTransaction);
                            }
                        }
                    }
                }
                finalJournalTransactions.AddRange(journalTransactions);
            }
            return finalJournalTransactions;
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editFlag == true)
            {
                var source = grdCntrlSalesReceipt.ItemsSource as List<CompanyLoansSaleReceiptModelView>;
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
            if (grdCntrlSalesReceipt.SelectedItem != null)
            {
                var idd = (grdCntrlSalesReceipt.SelectedItem as CompanyLoansSaleReceiptModelView).Id;

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
    }
}
