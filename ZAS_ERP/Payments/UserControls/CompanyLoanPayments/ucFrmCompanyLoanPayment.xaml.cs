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
using ERP_BL.Procurements.LoansAdvances;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Payments.ModelViews;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.Payments.UserControls.CompanyLoanPayments
{
    /// <summary>
    /// Interaction logic for ucFrmCompanyLoanPayments.xaml
    /// </summary>
    public partial class ucFrmCompanyLoanPayment : UserControl
    {
        List<Payment> payments = new List<Payment>();
        PaymentRepo paymentRepo = new PaymentRepo();

        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        List<Department> deptList = new List<Department>();
        public bool editFlag = false;
        bool? companyChnaged = null;

        PaymentStatus checkStatus = new PaymentStatus();

        public Window frmPiPaymentWindow = new Window();
        public int groupId = 0;
        int intGroupId;
        UsersRepo UsersRepo = new UsersRepo();

        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static PaymentStatus statusChanged = new PaymentStatus();

        public int loansAdvanceId = 0;
        string stage;
        bool? isApproved;
        bool? underApproval;
        //bool? isReApproved;
        DateTime? approvalDate;

        public bool createdFromBill = false;
        int banktransactionFlag = 0;

        List<LoansAdvance> LoansAdvances = new List<LoansAdvance>();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<PaymentStatus> PaymentStatuses = new List<PaymentStatus>();
        ERP_BL.Databases.Employee insuarenceAppliedEmployee = new ERP_BL.Databases.Employee();
        bool firstInsuranceCheck = false;
        public ucFrmCompanyLoanPayment()
        {
            InitializeComponent();
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
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                loadPaymentTypes();
                loadCompanies();
                loadCurrencies();
                loadPaymentMethods();
                loadPaymentStatus();
                loadPaymentTerms();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Insurance") != null)
                {
                    insuranceGrid.IsEnabled = true;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of Payment") != null)
                {
                    datglPostingdate.IsEnabled = true;
                }
                else
                {
                    datglPostingdate.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass Payment Bank Account") != null)
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

                    if (loansAdvanceId != 0)
                    {
                        btnLoad.IsEnabled = false;
                        AdvanceRepo LAdvanceRepo = new AdvanceRepo();
                        LoansAdvance _LA = new LoansAdvance();
                        List<CompanyLoanPaymentModelView> modelViewList = new List<CompanyLoanPaymentModelView>();


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


                            var departmentList = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;
                            if (_LA.department != null)
                            {

                                if (_LA.department != null && departmentList.Find(x => x.Id == _LA.deptId) == null)
                                {

                                    departmentList.Add(_LA.department);
                                    lookupDepartment1.ItemsSource = null;
                                    lookupDepartment1.ItemsSource = departmentList;
                                }

                            }



                            List<Vendor> vendors = new List<Vendor>();
                            string deptNames = "";

                            if (_LA.department != null)
                            {

                                deptList.Add(_LA.department);
                            }
                            allEmployees.Clear();
                            if (deptList.Count > 0)
                            {

                                foreach (var _dept in deptList)
                                {
                                    deptNames = deptNames + " | " + _dept.DeptName;
                                    if (lookupCompany.SelectedIndex > -1)
                                        foreach (var _vendor in _dept.Vendors)
                                        {
                                            if (!vendors.Contains(_vendor))
                                                vendors.Add(_vendor);
                                        }
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
                            gridInterCompanyDetails.IsEnabled = false;


                            CompanyLoanPaymentModelView LA = new CompanyLoanPaymentModelView();
                            double amountWithTax = 0;
                            double loanAmount = 0;
                            LA.LoansAdvanceId = _LA.Id;
                            LA.LACreationDate = (DateTime)_LA.CreationDate;


                            if (_LA.currency != null)
                                LA.currency = _LA.currency.CurrencyName;


                            LA.SystemReferenceNo = _LA.SystemRef;

                            if (_LA.SalesReceipts != null)
                                loanAmount = _LA.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                            LA.LoanAmount = loanAmount;



                            if (_LA.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                LA.AmountDue = loanAmount;
                            }
                            else
                            {
                                LA.AmountDue = loanAmount - _LA.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                            }

                            LA.LoansAdvanceStage = GetLAStatus(_LA);

                            modelViewList.Add(LA);
                            grdCntrlPayment.ItemsSource = modelViewList;
                        }
                    }
                    if (editFlag == false)
                    {
                        //btnPushDebits.IsChecked = true;
                        //btnPushCredits.IsChecked = true;
                    }
                    else
                    {
                        if (payments[0].isBypassBank == true && payments[0].coaAccountId != null)
                        {
                            isBypassCOA.IsChecked = true;
                            lookupCOA.Text = payments[0].ChartofAccount.accountName;
                        }
                    }
                }



                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment Reference Number in Payments") != null)
                {

                    cmbxPettyCashRef.IsEnabled = true;
                }

                else
                {

                    cmbxPettyCashRef.IsEnabled = false;
                }



                if (editFlag == true && groupId > 0)
                {
                    payments = paymentRepo.GetLApaymentsByGroupId(groupId);

                    if (payments[0].isDeposit == true)
                        btnDeposit.IsChecked = true;
                    else if (payments[0].isDeposit == false)
                        btnPayment.IsChecked = true;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances Payment") == null)
                    {
                        btnSave.IsEnabled = false;

                    }

                    if (payments[0].Status.isActive == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Payment") != null)
                            btnSave.IsEnabled = true;
                        else
                            btnSave.IsEnabled = false;
                    }


                    if (payments[0].isApproved == false)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Payment") == null)
                            btnSave.IsEnabled = false;
                        else
                            btnSave.IsEnabled = true;
                    }



                    if (payments[0].isApproved == false)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Payment") != null)
                        {
                            btnSave.IsEnabled = true;
                        }
                        else
                        {
                            btnSave.IsEnabled = false;
                        }
                    }

                    else if (payments[0].isApproved == true)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Deductions After Approval") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances Payment") == null)
                        {
                            chkInterCompany.IsEnabled = false;
                            gridInterCompanyDetails.IsEnabled = false;
                            lookupDepartment1.IsEnabled = false;
                            lookupCompany.IsEnabled = false;
                            datDebitedDate.IsEnabled = false;
                            cmbxPaymentType.IsEnabled = false;


                            datPaymentDate.IsEnabled = false;
                            txtSystemRef.IsEnabled = false;
                            txtPaymentRef.IsEnabled = false;
                            lookUpCurrency.IsEnabled = false;
                            txtPaymentAmount.IsEnabled = false;
                            cmbPaymentStatus.IsEnabled = false;


                            lookupPaymentMethod.IsEnabled = false;
                            lookupBanks.IsEnabled = false;
                            lookupAccounts.IsEnabled = false;
                            cmbxPettyCashRef.IsEnabled = false;
                            txtInstrumentNo.IsEnabled = false;
                            datInstrumentDate.IsEnabled = false;
                            cmbxApplyToPO.IsEnabled = false;
                            grdCntrlPayment.Columns["DebitedAmount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                            btnSave.IsEnabled = true;
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Deductions After Approval") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances Payment") == null)
                        {
                            btnSave.IsEnabled = false;
                        }
                    }

                    //Payment term Load for sale Order
                    if (payments[0].paymentterm_Id != 0 && payments[0].paymentTerm != null)
                    {
                        var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;
                        //cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == payments[0].paymentterm_Id))];
                        var term = paymentTermSource.Find(x => x.id == payments[0].paymentterm_Id);

                        if (term == null)
                        {
                            paymentTermSource.Add(new cmbitem() { name = payments[0].paymentTerm.term, id = payments[0].paymentTerm.Id });
                            cmbPaymentTerm.ItemsSource = null;
                            cmbPaymentTerm.ItemsSource = paymentTermSource;
                        }
                        cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == payments[0].paymentterm_Id))];
                    }

                    btnLoad.IsEnabled = false;


                    views = UsersRepo.getViwerInfo(groupId, 20);
                    grdUsers.ItemsSource = views;
                    loadcomments();


                    txtSystemRef.Text = payments[0].SystemRefNo;
                    lblPaymentRefNo.Text = " (" + payments[0].SystemRefNo + ")";


                    if (payments[0].isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                        txtVoid.RenderTransform = new RotateTransform(-45);
                        //lblStage.Text = "Void";
                    }
                    else if (payments[0].isReApproved == false)
                    {
                        //lblStage.Text = "Under Re-Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;

                        underApproval = true;
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Reload Payments") != null)
                            btnReLoad.Visibility = Visibility.Visible;
                    }
                    else if (payments[0].isApproved == true && payments[0].stage == "Closed")
                    {
                        //lblStage.Text = "Approved and Closed";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (payments[0].isApproved == true && payments[0].Status.isActive == false && payments[0].PendingForClosing != true)
                    {
                        //lblStage.Text = "Approved and Closed";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (payments[0].isApproved == true && payments[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (payments[0].isApproved == true)
                    {
                        //lblStage.Text = "Approved";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (payments[0].isApproved == false)
                    {
                        //lblStage.Text = "Under Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;

                        underApproval = true;
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Reload Payments") != null)
                            btnReLoad.Visibility = Visibility.Visible;
                    }
                    else if (payments[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }

                    if (payments[0].GLPostingDate != null)
                    {
                        datglPostingdate.EditValue = payments[0].GLPostingDate;
                    }
                    else
                    {
                        datglPostingdate.EditValue = payments[0].CreationDate;
                    }


                    loadcomments();
                    loadAttachments();


                    if (payments[0].createdFromBill == true)
                    {
                        lookupCompany.IsEnabled = false;
                        lookupDepartment1.IsEnabled = false;
                        lookUpCurrency.IsEnabled = false;
                        gridInterCompanyDetails.IsEnabled = false;
                        chkInterCompany.IsEnabled = false;

                    }


                    int index = 0;


                    //Select Template
                    for (int i = 0; i <= (int)ERP_BL.Enums.PaymentLoansAdvancesTemplate.Loans_Advance; i++)
                    {

                        if (((ERP_BL.Enums.PaymentLoansAdvancesTemplate)i).ToString() == payments[0].paymentLoansAdvancesTemplate.ToString())
                        {
                            cmbxPaymentType.SelectedIndex = i;
                            break;
                        }
                    }




                    if (payments[0].CreationDate != null)
                        datCreationDate.EditValue = (DateTime)payments[0].CreationDate;


                    if (payments[0].DebitedDate != null)
                        datDebitedDate.EditValue = (DateTime)payments[0].DebitedDate;


                    if (payments[0].InstrumentDate != null)
                        datInstrumentDate.EditValue = (DateTime)payments[0].InstrumentDate;


                    if (payments[0].PaymentDate != null)
                        datPaymentDate.EditValue = (DateTime)payments[0].PaymentDate;


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

                    if (payments[0].company_Id != null || payments[0].company != null)
                    {

                        var companylist = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                        if (payments[0].company_Id != null && companylist.Find(x => x.Id == payments[0].company_Id) == null)
                        {

                            companylist.Add(payments[0].company);
                            lookupCompany.ItemsSource = null;
                            lookupCompany.ItemsSource = companylist;
                            //lookupCompany.IsEnabled = false;
                        }

                        index = 0;
                        foreach (var _company in companylist)
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

                    else
                    {
                        lookupCompany.Text = "Select Company";
                    }



                    List<Vendor> vendors = new List<Vendor>();
                    string deptNames = "";


                    if (payments[0].departments != null && payments[0].departments.Count > 0)
                    {

                        deptList = payments[0].departments;
                    }


                    allEmployees.Clear();
                    if (deptList.Count > 0)
                    {

                        foreach (var _dept in deptList)
                        {

                            deptNames = deptNames + " | " + _dept.DeptName;
                            if (lookupCompany.SelectedIndex > -1)
                                foreach (var _vendor in _dept.Vendors)
                                {
                                    if (!vendors.Contains(_vendor))
                                        vendors.Add(_vendor);
                                }
                            allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (lookupCompany.SelectedItem as Company).Id) != null));
                        }


                    }
                    loadEmployees();

                    lookupDepartment1.EditValue = deptNames;


                    //Select Payment Reference No
                    var paymentRefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
                    if (payments[0].PaymentRefNoId != null)
                    {

                        index = 0;
                        foreach (var _ref in paymentRefList)
                        {

                            if (_ref.id == payments[0].PaymentRefNoId)
                            {
                                cmbxPettyCashRef.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }




                    if (payments[0].DebitedDate != null)
                        datDebitedDate.DateTime = (DateTime)payments[0].DebitedDate;


                    if (payments[0].PaymentDate != null)
                        datPaymentDate.DateTime = (DateTime)payments[0].PaymentDate;


                    if (payments[0].SystemRefNo != null)
                        txtSystemRef.Text = payments[0].SystemRefNo;


                    if (payments[0].PaymentRefNo != null)
                        txtPaymentRef.Text = payments[0].PaymentRefNo;


                    //Select Currency
                    var currencyList = (lookUpCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookUpCurrency.ItemsSource as List<Currency>;
                    if (payments[0].currency_Id != null)
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

                    txtPaymentAmount.Text = Convert.ToDouble(payments.Sum(x => x.DebitedAmount)).ToString();
                    if (payments[0].Status != null)
                    {
                        var disAbleStatus = PaymentStatuses.FirstOrDefault(x => x.Id == payments[0].Status.Id);
                        if (disAbleStatus == null)
                        {
                            loadPaymentStatus(payments[0].Status);
                        }
                    }
                    //Select Status
                    var statusList = (cmbPaymentStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbPaymentStatus.ItemsSource as List<cmbitem>;
                    if (payments[0].Status != null)
                    {
                        if (payments[0].Status.isPaid == true)
                        {
                            imgPaid.Visibility = Visibility.Visible;
                        }
                        checkStatus = payments[0].Status;
                        index = 0;
                        foreach (var _status in statusList)
                        {

                            if (_status.id == payments[0].statusId)
                            {
                                cmbPaymentStatus.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Payment Method
                    var paymentMethodList = (lookupPaymentMethod.ItemsSource as List<PaymentMethod>) == null ? new List<PaymentMethod>() : lookupPaymentMethod.ItemsSource as List<PaymentMethod>;
                    if (payments[0].paymentMethodId != null)
                    {

                        index = 0;
                        foreach (var _method in paymentMethodList)
                        {

                            if (_method.Id == payments[0].paymentMethodId)
                            {
                                lookupPaymentMethod.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Bank
                    var bankList = (lookupBanks.ItemsSource as List<Bank>) == null ? new List<Bank>() : lookupBanks.ItemsSource as List<Bank>;
                    if (payments[0].bankId != null)
                    {

                        index = 0;
                        foreach (var _bank in bankList)
                        {

                            if (_bank.Id == payments[0].bankId)
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
                    if (payments[0].accountId != null)
                    {

                        index = 0;
                        foreach (var _account in accountList)
                        {

                            if (_account.Id == payments[0].accountId)
                            {
                                lookupAccounts.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    if (payments[0].InstrumentNo != null)
                        txtInstrumentNo.Text = payments[0].InstrumentNo;
                    if (payments[0].VATBookRefId != 0 && payments[0].VATBookRefNumber != null)
                    {
                        var vatBookSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                        var term = vatBookSource.Find(x => x.id == payments[0].VATBookRefId);

                        if (term == null)
                        {
                            vatBookSource.Add(new cmbitem() { name = payments[0].VATBookRefNumber.VATBookReferenceNo, id = payments[0].VATBookRefNumber.Id });
                            cmbxVATBookRef.ItemsSource = null;
                            cmbxVATBookRef.ItemsSource = vatBookSource;
                        }
                        cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == payments[0].VATBookRefId))];
                    }
                    if (payments[0].isVATBookPosted == true)
                        btnVATBookPost.IsChecked = true;

                    if (payments[0].InstrumentDate != null)
                        datInstrumentDate.DateTime = (DateTime)payments[0].InstrumentDate;

                    LoadLoansAdvanceData();
                    if (payments[0].transactionHolderId != null)
                    {
                        try
                        {
                            var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                            cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == payments[0].transactionHolderId))];

                        }
                        catch (Exception ex)
                        {

                        }
                    }
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
                    //btnPushDebits.IsChecked = true;
                    //btnPushCredits.IsChecked = true;
                }
                else
                {
                    if (payments[0].isBypassBank == true && payments[0].coaAccountId != null)
                    {
                        isBypassCOA.IsChecked = true;
                        lookupCOA.Text = payments[0].ChartofAccount.accountName;
                    }

                    if (payments[0].insuranceRequired == true)
                    {
                        btnInsuranceRequired.IsChecked = true;
                    }
                    else
                    {
                        btnInsuranceRequired.IsChecked = false;
                    }
                    if (payments[0].insuranceApplied == true)
                    {
                        btnInsuranceApplied.IsChecked = true;
                    }
                    else
                    {
                        btnInsuranceApplied.IsChecked = false;
                    }

                    if (payments[0].insuranceNotApplicable == true)
                    {
                        btnInsuranceNotApplicable.IsChecked = true;
                    }
                    else
                    {
                        btnInsuranceNotApplicable.IsChecked = false;
                    }
                    if (payments[0].insuranceAppliedBy_Id != 0 || payments[0].insuranceAppliedBy != null)
                    {
                        try
                        {
                            var empSource = (List<cmbitem>)cmbAppliedBy.Items.SourceCollection;
                            cmbAppliedBy.SelectedItem = cmbAppliedBy.Items[cmbAppliedBy.Items.IndexOf(empSource.Find(x => x.id == payments[0].insuranceAppliedBy_Id))];
                            insuarenceAppliedEmployee = payments[0].insuranceAppliedBy;

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
            grdTrackingTree.ExpandAllNodes();
        }

        public void loadPaymentTerms()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForPayments();


                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbPaymentTerm.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }
        }

        private void cmbPaymentTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPaymentTerm.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPaymentTerm.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
                    paymentTerms.ShowDialog();
                    loadPaymentTerms();

                }
            }
        }

        private void LoadLoansAdvanceData()
        {
            List<CompanyLoanPaymentModelView> modelViewList = new List<CompanyLoanPaymentModelView>();
            foreach (var _payment in payments)
            {
                CompanyLoanPaymentModelView payment = new CompanyLoanPaymentModelView();
                payment.Id = _payment.Id;
                payment.LoansAdvanceId = (int)_payment.LoansAdvanceId;
                //payment.GroupId = _payment.adminBill.transactionGroupId;
                payment.LACreationDate = (DateTime)_payment.loansAdvance.CreationDate;

                var creditJournalTransactions = _payment.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = _payment.journalTransactions.Where(x => x.debit != 0).ToList();
                //if (creditJournalTransactions.Count > 0)
                //{
                //    btnPushCredits.IsChecked = true;
                //}
                //if (debitJournalTransactions.Count > 0)
                //{
                //    btnPushDebits.IsChecked = true;
                //}

                if (_payment.loansAdvance.currency != null)
                    payment.currency = _payment.loansAdvance.currency.CurrencyName;
                if (_payment.loansAdvance.isEmployee == true)
                {
                    if (_payment.loansAdvance.ApplicantEmployee != null)
                        payment.Employee = _payment.loansAdvance.ApplicantEmployee.person.FName + " " + _payment.loansAdvance.ApplicantEmployee.person.LName;
                }
                else
                {
                    if (_payment.loansAdvance.applicantType != null)
                        payment.ApplicantType = _payment.loansAdvance.applicantType.TypeName;
                    if (_payment.loansAdvance.applicant != null)
                        payment.ApplicantName = _payment.loansAdvance.applicant.Name;
                }

                payment.SystemReferenceNo = _payment.loansAdvance.SystemRef;



                payment.LoanAmount = Math.Round(_payment.loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(y => y.CollectionAmount), 2);


                payment.DebitedAmount = _payment.DebitedAmount;


                payment.Total = _payment.DebitedAmount;


                var amountPaid = Math.Round(_payment.loansAdvance.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount), 2);
                var remainingAmount = Math.Round(payment.LoanAmount - amountPaid, 2);


                payment.AmountDue = remainingAmount;



                //bill.AmountToPay = _bill.AmountOC;


                payment.LoansAdvanceStage = GetLAStatus(_payment.loansAdvance);
                if (_payment.transactionHolderId != null)
                {
                    payment.transactionHolderId = _payment.transactionHolderId;
                    payment.holderChangeDate = _payment.holderChangeDate;
                }
                modelViewList.Add(payment);
            }
            grdCntrlPayment.ItemsSource = modelViewList;
        }

        private string GetLAStatus(LoansAdvance LA)
        {
            PaymentRepo repo = new PaymentRepo();
            var _LA = repo.GetLoansAdvance(LA.Id);
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

        private void loadPaymentTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.PaymentLoansAdvancesTemplate.Loans_Advance; i++)
            {
                cmbxPaymentType.Items.Add(((ERP_BL.Enums.PaymentLoansAdvancesTemplate)i).ToString());
            }
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Payments);
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
                    comments = procurementRepo.getcommentslogAsc(groupId, TransactionItemType.Payments);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void loadPaymentMethods()
        {
            lookupPaymentMethod.ItemsSource = paymentRepo.GetAllPaymentMethods();
        }

        private void loadPaymentStatus()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment Statuses") != null)
                PaymentStatuses = paymentRepo.GetAllPaymentStatuses();
            else
                PaymentStatuses = paymentRepo.GetAllPaymentStatuses().Where(x => x.isActive == true).ToList();
            PaymentStatuses = PaymentStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(PaymentStatuses, delegate (PaymentStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbPaymentStatus.ItemsSource = cmbitems;
        }
        private void loadPaymentStatus(PaymentStatus _paymentStatus)
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            PaymentStatuses.Add(_paymentStatus);
            Parallel.ForEach(PaymentStatuses, delegate (PaymentStatus status)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbPaymentStatus.ItemsSource = cmbitems;
        }

        private void loadCompanies()
        {
            //EmployeeRepo empRepo = new EmployeeRepo();
            empUser = paymentRepo.GetEmployeeForPayments(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
            lookupInterCompany.ItemsSource = empUser.Companies;
        }

        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll().Where(x => x.isVoid != true).ToList();
            lookUpCurrency.ItemsSource = currencies;
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && payments.Count > 0)
            {

                Company InterCompany = lookupInterCompany.SelectedItem as Company;
                Department InterDepartment = lookupInterDepartment.SelectedItem as Department;
                Company company = lookupCompany.SelectedItem as Company;
                if (deptList != null && deptList.Count > 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                {
                    List<int> deptIds = new List<int>();
                    foreach (var _dept in deptList)
                        deptIds.Add(_dept.Id);
                    deptIds.Add(InterDepartment.Id);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(deptIds, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Payments);
                    inputBox.ShowDialog();

                }
                else if (deptList != null && deptList.Count > 0 && company?.Id != 0 && company != null)
                {
                    List<User> usersList = new List<User>();
                    foreach (var _dept in deptList)
                    {
                        usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                    }
                    var userss = usersList.Where(x => x.employee.Companies.FirstOrDefault(y => y.Id == company.Id) != null).ToList();
                    userss = userss.Distinct().ToList();

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.Payments);
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
                        var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }
                        if (frmInputBox.Comment.TaggedList.Count > 0)
                        {
                            var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                            var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                            cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                        }
                        //procurementRepo.Add(groupId, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
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
                DXMessageBox.Show("You have to save the payments first!");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Payment") != null)
            {
                if (grdAttach.Visibility == Visibility.Visible)
                    grdAttach.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttach.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Attach File!");
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Payment") != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlPayment.SelectedItem != null)
            {
                var idd = (grdCntrlPayment.SelectedItem as CompanyLoanPaymentModelView).Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.Payments);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Select any Payment first!");
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                payments = paymentRepo.GetPaymentsByGroupId(groupId);

                if (payments != null && payments.Count > 0)
                {

                    if (payments[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Payment") != null))
                    {
                        if (DXMessageBox.Show("This Transaction is currently in the list of Void Payments! Do you want to remove it from Void?", "Remove Void Payments", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            grdVoid.Visibility = Visibility.Collapsed;

                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res1 = MessageBox.Show("Payments has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (deptList != null && deptList.Count > 0)
                                {
                                    List<User> usersList = new List<User>();
                                    foreach (var _dept in deptList)
                                    {
                                        usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                                    }
                                    var userss = usersList.Distinct().ToList();

                                    winTagUsers win = new winTagUsers(userss, payments[0].transactionGroupId, TransactionItemType.Payments);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count != 0)
                                    {
                                        for (int i = 0; i < payments.Count; i++)
                                        {
                                            if (payments[i].transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                payments[i].holderChangeDate = DateTime.Now;
                                            }
                                            payments[i].transactionHolderId = win.tagUsers[0].employeeId;
                                        }
                                    }

                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }

                            }
                            paymentRepo.setPaymentsToVoid(payments, groupId, false);


                            string symbolCurr = "";
                            if (payments[0].currency != null)
                            {
                                symbolCurr = payments[0].currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Payments (Amount OC) having value: " + payments[0].PaymentAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                                Timestamp = DateTime.Now,
                                Subject = "Payments UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + payments[0].PaymentRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + payments[0].PaymentRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Payment") != null)
                    {
                        if (DXMessageBox.Show("This Transaction is not currently in the list of Void Payments! Do you want to move it to Void Payments?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {

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

                            var res1 = MessageBox.Show("Payments has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (deptList != null && deptList.Count > 0)
                                {
                                    List<User> usersList = new List<User>();
                                    foreach (var _dept in deptList)
                                    {
                                        usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                                    }
                                    var userss = usersList.Distinct().ToList();

                                    winTagUsers win = new winTagUsers(userss, payments[0].transactionGroupId, TransactionItemType.Payments);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count != 0)
                                    {
                                        for (int i = 0; i < payments.Count; i++)
                                        {
                                            if (payments[i].transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                payments[i].holderChangeDate = DateTime.Now;
                                            }
                                            payments[i].transactionHolderId = win.tagUsers[0].employeeId;
                                        }
                                    }

                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }

                            }
                            paymentRepo.setPaymentsToVoid(payments, groupId, true);

                            string symbolCurr = "";
                            if (payments[0].currency != null)
                            {
                                symbolCurr = payments[0].currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Payments (Amount OC) having value: " + payments[0].PaymentAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                                Timestamp = DateTime.Now,
                                Subject = "Payments Voided",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + payments[0].PaymentRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + payments[0].PaymentRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to Mark or UnMark a Payment to Void!");
                    }
                }

            }
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

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
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
                        destination += "Attachments\\Payments\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Payments.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Payments);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Payments, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 20, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Payments);
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

        public ucFrmCompanyLoanPayment(PaymentStatus paymentStatus)
        {
            statusChanged = paymentStatus;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo = new UsersRepo();
            if (editFlag == true && groupId > 0)
            {
                List<Department> deptss = new List<Department>();
                if (payments != null && payments.Count > 0)
                {
                    if (payments[0].departments != null)
                        deptss = payments[0].departments;
                }

                string previous_status;
                paymentRepo = new PaymentRepo();
                payments = paymentRepo.GetPaymentForApproval(groupId);
                if (payments != null && payments.Count > 0)
                {
                    previous_status = payments[0].Status.Status;
                    var deparmentss = payments[0].departments;
                    var companyy = payments[0].company;

                    if (payments[0].isApproved == false)
                    {
                        DXMessageBox.Show("Payments are pending for approval!");
                        return;
                    }
                    statusChanged = null;
                    ucFrmPaymentDirectClose ucFrmDirectClose = new ucFrmPaymentDirectClose();
                    if (payments[0].Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = payments[0].Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(payments[0].Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
                    ucFrmDirectClose.type = "LoansAdvance";
                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>(); 
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Payments has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (deparmentss != null && deparmentss.Count != 0 && companyy?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                List<User> usersList = new List<User>();
                                foreach (var _dept in deptList)
                                {
                                    usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, companyy.Id));

                                }
                                var userss = usersList.Distinct().ToList();

                                winTagUsers win = new winTagUsers(userss, payments[0].transactionGroupId, TransactionItemType.Payments);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;
                                tagUsersRecommendation = win.tagRecommendationUsers;
                                ccUsersRecommendation = win.ccRecommendationUsers;
                                if (win.tagUsers.Count != 0)
                                {
                                    for (int i = 0; i < payments.Count; i++)
                                    {
                                        if (payments[i].transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            payments[i].holderChangeDate = DateTime.Now;
                                        }
                                        payments[i].transactionHolderId = win.tagUsers[0].employeeId;
                                    }
                                }

                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }
                        }

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Payment without Approval") != null)
                        {
                            for (int i = 0; i < payments.Count; i++)
                            {
                                payments[i].PendingForClosing = false;
                                payments[i].stage = TransactionStage.Closed.ToString();
                                payments[i].statusId = statusChanged.Id;
                                payments[i].LastStatusChangeDate = System.DateTime.Now;
                                payments[i].ClosingDate = System.DateTime.Now;


                            }
                            paymentRepo.ApprovePayment(payments, deptss);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);


                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            if (payments.Count != 0)
                            {
                                procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            }

                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < payments.Count; i++)
                            {
                                payments[i].PendingForClosing = true;
                                payments[i].stage = TransactionStage.AwaitingApproval.ToString();
                                payments[i].statusId = statusChanged.Id;
                                payments[i].LastStatusChangeDate = System.DateTime.Now;
                                payments[i].ClosingDate = System.DateTime.Now;


                            }
                            paymentRepo.ApprovePayment(payments, deptss);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            if (payments.Count != 0)
                            {
                                procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            }

                        }
                        DXMessageBox.Show("Status has been changed to InActive from " + previous_status + " to " + statusChanged.Status);
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();

                    }
                }


            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (editFlag == true && payments != null && payments.Count > 0)
                {

                    List<Department> deptss = new List<Department>();
                    if (payments[0].departments != null)
                        deptss = payments[0].departments;

                    //SalesReceipt receipt = new SalesReceipt();
                    paymentRepo = new PaymentRepo();
                    var paymentsForApproval = paymentRepo.GetPaymentForApproval(payments[0].transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (paymentsForApproval != null && paymentsForApproval.Count > 0)
                    {
                        if (paymentsForApproval[0].isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Payments are Approved, Do you want to UnApprove these Payments?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < paymentsForApproval.Count; i++)
                                    {
                                        paymentsForApproval[i].isApproved = false;
                                        paymentsForApproval[i].stage = TransactionStage.AwaitingApproval.ToString();

                                    }
                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, payments[0].transactionGroupId, 20, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);


                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>(); 
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Payments has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (paymentsForApproval[0].departments != null && paymentsForApproval[0].departments.Count > 0)
                                        {
                                            List<User> usersList = new List<User>();
                                            foreach (var _dept in deptList)
                                            {
                                                usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                                            }
                                            var userss = usersList.Distinct().ToList();

                                            winTagUsers win = new winTagUsers(userss, payments[0].transactionGroupId, TransactionItemType.Payments);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                for (int i = 0; i < paymentsForApproval.Count; i++)
                                                {
                                                    if (paymentsForApproval[i].transactionHolderId != win.tagUsers[0].employeeId)
                                                    {
                                                        paymentsForApproval[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    paymentsForApproval[i].transactionHolderId = win.tagUsers[0].employeeId;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }
                                    paymentRepo.ApprovePayment(paymentsForApproval, deptss);

                                    string symbolCurr = "";
                                    if (paymentsForApproval[0].currency != null)
                                    {
                                        symbolCurr = paymentsForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Payments (Amount OC) having value: " + paymentsForApproval[0].PaymentAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Payments UnApproved",TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + paymentsForApproval[0].PaymentRefNo, paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + paymentsForApproval[0].PaymentRefNo, paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Payments are UnApproved (" + payments[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Payment is UnApproved (" + payments[0].transactionGroupId + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Payment Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payment Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (paymentsForApproval[0].isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Payments are Pending for Approval, Do you want to Approve these Payments?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < paymentsForApproval.Count; i++)
                                    {
                                        paymentsForApproval[i].isApproved = true;
                                        paymentsForApproval[i].stage = TransactionStage.Approved.ToString();
                                        paymentsForApproval[i].ApprovedDate = DateTime.Now;
                                    }
                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, payments[0].transactionGroupId, 20, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>(); 
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Payments has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (paymentsForApproval[0].departments != null && paymentsForApproval[0].departments.Count > 0)
                                        {
                                            List<User> usersList = new List<User>();
                                            foreach (var _dept in deptList)
                                            {
                                                usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                                            }
                                            var userss = usersList.Distinct().ToList();

                                            winTagUsers win = new winTagUsers(userss, paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                for (int i = 0; i < paymentsForApproval.Count; i++)
                                                {
                                                    if (paymentsForApproval[i].transactionHolderId != win.tagUsers[0].employeeId)
                                                    {
                                                        paymentsForApproval[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    paymentsForApproval[i].transactionHolderId = win.tagUsers[0].employeeId;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }
                                    paymentRepo.ApprovePayment(paymentsForApproval, deptss);

                                    string symbolCurr = "";
                                    if (paymentsForApproval[0].currency != null)
                                    {
                                        symbolCurr = paymentsForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Payments (Amount OC) having value: " + paymentsForApproval[0].PaymentAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Payments Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + paymentsForApproval[0].PaymentRefNo, paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Payments #" + paymentsForApproval[0].PaymentRefNo, paymentsForApproval[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Payments are Approved (" + payments[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + payments[0].transactionGroupId + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Payments Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payments Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (payments[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Payment") != null) ? true : false)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    paymentsForApproval[i].isReApproved = true;
                                    paymentsForApproval[i].stage = TransactionStage.Approved.ToString();
                                    paymentsForApproval[i].ReApprovalDate = DateTime.Now;
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, payments[0].transactionGroupId, (int)TransactionItemType.Payments, frmInputBox.comment);
                                paymentRepo.ApprovePayment(paymentsForApproval, deptss);
                                MessageBox.Show("Payments are Approved (" + payments[0].transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + payments[0].transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Payment Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payment Directly user id=(" + MainWindow.currentUserid + ")");
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
                    thread.Start();                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadGridData();
        }

        private void LoadGridData()
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                lookupCompany.Focus();
                return;
            }
            if (deptList.Count == 0)
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


            CompanyLoansRepo LoansAdvanceRepo = new CompanyLoansRepo();

            LoansAdvances = LoansAdvanceRepo.getLoansAdvancebyCompanyDept((lookupCompany.SelectedItem as Company).Id, deptList, (lookUpCurrency.SelectedItem as Currency).Id);

            List<CompanyLoanPaymentModelView> modelViewList = new List<CompanyLoanPaymentModelView>();

            if (editFlag == true && payments != null && payments.Count > 0)
            {

                var billIds = LoansAdvances.Select(x => x.Id).ToList();
                var paymentIds = payments.Select(x => x.LoansAdvanceId.Value).ToList();

                var finalIds = paymentIds.Except(billIds).ToList();

                var transactionGroupId = payments[0].transactionGroupId;


                foreach (var idd in finalIds)
                {
                    CompanyLoanPaymentModelView payment = new CompanyLoanPaymentModelView();

                    var _payment = payments.FirstOrDefault(x => x.Bill_Id == idd);

                    if (_payment != null)
                    {
                        payment.Id = _payment.Id;
                        payment.LoansAdvanceId = (int)_payment.LoansAdvanceId;
                        //payment.GroupId = _payment.adminBill.transactionGroupId;
                        payment.LACreationDate = (DateTime)_payment.loansAdvance.CreationDate;

                        var creditJournalTransactions = _payment.journalTransactions.Where(x => x.credit != 0).ToList();
                        var debitJournalTransactions = _payment.journalTransactions.Where(x => x.debit != 0).ToList();
                        //if (creditJournalTransactions.Count > 0)
                        //{
                        //    btnPushCredits.IsChecked = true;
                        //}
                        //if (debitJournalTransactions.Count > 0)
                        //{
                        //    btnPushDebits.IsChecked = true;
                        //}

                        if (_payment.loansAdvance.currency != null)
                            payment.currency = _payment.loansAdvance.currency.CurrencyName;
                        if (_payment.loansAdvance.isEmployee == true)
                        {
                            if (_payment.loansAdvance.ApplicantEmployee != null)
                                payment.Employee = _payment.loansAdvance.ApplicantEmployee.person.FName + " " + _payment.loansAdvance.ApplicantEmployee.person.LName;
                        }
                        else
                        {
                            if (_payment.loansAdvance.applicantType != null)
                                payment.ApplicantType = _payment.loansAdvance.applicantType.TypeName;
                            if (_payment.loansAdvance.applicant != null)
                                payment.ApplicantName = _payment.loansAdvance.applicant.Name;
                        }
                        payment.SystemReferenceNo = _payment.loansAdvance.SystemRef;
                        payment.LoanAmount = _payment.loansAdvance.LoanAmountOC;
                        payment.DebitedAmount = _payment.DebitedAmount;
                        payment.Total = _payment.DebitedAmount;
                        var amountPaid = Math.Round(_payment.loansAdvance.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount), 2);
                        var remainingAmount = Math.Round(payment.LoanAmount - amountPaid, 2);
                        payment.AmountDue = remainingAmount;
                        payment.LoansAdvanceStage = GetLAStatus(_payment.loansAdvance);
                        if (_payment.transactionHolderId != null)
                        {
                            payment.transactionHolderId = _payment.transactionHolderId;
                            payment.holderChangeDate = _payment.holderChangeDate;
                        }
                        modelViewList.Add(payment);
                    }
                }

                foreach (var _LA in LoansAdvances)
                {
                    CompanyLoanPaymentModelView LA = new CompanyLoanPaymentModelView();

                    var _payment = payments.FirstOrDefault(x => x.LoansAdvanceId == _LA.Id);
                    if (_payment != null)
                    {
                        LA.Id = _payment.Id;
                        LA.LoansAdvanceId = (int)_payment.LoansAdvanceId;
                        LA.LACreationDate = (DateTime)_payment.loansAdvance.CreationDate;
                        var creditJournalTransactions = _payment.journalTransactions.Where(x => x.credit != 0).ToList();
                        var debitJournalTransactions = _payment.journalTransactions.Where(x => x.debit != 0).ToList();
                        //if (creditJournalTransactions.Count > 0)
                        //{
                        //    btnPushCredits.IsChecked = true;
                        //}
                        //if (debitJournalTransactions.Count > 0)
                        //{
                        //    btnPushDebits.IsChecked = true;
                        //}

                        if (_payment.loansAdvance.currency != null)
                            LA.currency = _payment.loansAdvance.currency.CurrencyName;
                        if (_payment.loansAdvance.isEmployee == true)
                        {
                            if (_payment.loansAdvance.ApplicantEmployee != null)
                                LA.Employee = _payment.loansAdvance.ApplicantEmployee.person.FName + " " + _payment.loansAdvance.ApplicantEmployee.person.LName;
                        }
                        else
                        {
                            if (_payment.loansAdvance.applicantType != null)
                                LA.ApplicantType = _payment.loansAdvance.applicantType.TypeName;
                            if (_payment.loansAdvance.applicant != null)
                                LA.ApplicantName = _payment.loansAdvance.applicant.Name;
                        }

                        LA.SystemReferenceNo = _payment.loansAdvance.SystemRef;



                        LA.LoanAmount = _payment.loansAdvance.LoanAmountOC;


                        LA.DebitedAmount = _payment.DebitedAmount;


                        LA.Total = _payment.DebitedAmount;


                        var amountPaid = Math.Round(_payment.loansAdvance.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount), 2);
                        var remainingAmount = Math.Round(LA.LoanAmount - amountPaid, 2);


                        LA.AmountDue = remainingAmount;



                        //bill.AmountToPay = _bill.AmountOC;


                        LA.LoansAdvanceStage = GetLAStatus(_payment.loansAdvance);
                        if (_payment.transactionHolderId != null)
                        {
                            LA.transactionHolderId = _payment.transactionHolderId;
                            LA.holderChangeDate = _payment.holderChangeDate;
                        }
                    }
                    else
                    {
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


                        if (_LA.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                        {
                            LA.AmountDue = loanAmount;
                        }

                        else
                        {

                            LA.AmountDue = loanAmount - _LA.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                        }
                        LA.LoansAdvanceStage = GetLAStatus(_LA);
                    }
                    modelViewList.Add(LA);
                }
            }
            else
            {
                foreach (var _LA in LoansAdvances)
                {
                    CompanyLoanPaymentModelView LA = new CompanyLoanPaymentModelView();
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


                    if (_LA.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                    {
                        LA.AmountDue = loanAmount;
                    }

                    else
                    {

                        LA.AmountDue = loanAmount - _LA.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                    }
                    LA.LoansAdvanceStage = GetLAStatus(_LA);
                    modelViewList.Add(LA);
                }
            }


            grdCntrlPayment.ItemsSource = modelViewList;
        }

        private void GrdCntrlPayment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void GrdCntrlPayment_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void GrdCntrlPayment_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TableViewPayment_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = e.Row as CompanyLoanPaymentModelView;
            var debt = row.DebitedAmount;

            var pymnt = payments.FirstOrDefault(x => x.Id == row.Id);

            if (editFlag == true && pymnt != null)
            {

                var amountPaid = pymnt.loansAdvance.Payments.Where(x => x.isVoid != true && x.Id != pymnt.Id).Sum(y => y.DebitedAmount);
                amountPaid = Math.Round(amountPaid + debt /*+ ded*/, 2);

                if (amountPaid > pymnt.loansAdvance.SalesReceipts?.Where(x=>x.isVoid != null).Sum(y=>y.CollectionAmount))
                {

                    DXMessageBox.Show("Debited Amount cannot exceed Loan Amount!");
                    row.DebitedAmount = pymnt.DebitedAmount;

                    //row.Deductions = pymnt.Deductions;
                    row.Total = pymnt.DebitedAmount;
                    return;
                }


            }

            //if (editFlag == false)
            //{

            if (row.DebitedAmount > Math.Round(row.AmountDue, 2) && editFlag == false)
            {
                DXMessageBox.Show("Debited amount cannot exceed Amount Due!");
                ((DataViewBase)sender).Background = Brushes.LightBlue;
                row.DebitedAmount = 0;
                return;
            }
            //}

            var b = (CompanyLoanPaymentModelView)grdCntrlPayment.GetRow(e.RowHandle);
            b.Total = debt;


            foreach (var _item in grdCntrlPayment.ItemsSource as List<CompanyLoanPaymentModelView>)
            {
                var amount = _item.Total;
            }
        }

        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = paymentRepo.GetLastPaymentId();
            if (lastPaymentId == 0)
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
                var lastId = lastPaymentId/*.transactionGroupId*/;
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == false)
                {
                    //datCreationDate.DateTime = DateTime.Now;
                    GroupIdCalculation();
                    txtSystemRef.Text = "Payment-" + intGroupId;
                    lblPaymentRefNo.Text = " (Payment-" + intGroupId + ")";
                }

                if (cmbxPaymentType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Payment Type!");
                    cmbxPaymentType.Focus();
                    return;
                }
                if (lookupCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company!");
                    lookupCompany.Focus();
                    return;
                }
                if (deptList.Count == 0)
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
                if (datDebitedDate.EditValue == null)
                {
                    DXMessageBox.Show("Please select Debited Date!");
                    datDebitedDate.Focus();
                    return;
                }
                if (datPaymentDate.EditValue == null)
                {
                    DXMessageBox.Show("Please select Payment date!");
                    datPaymentDate.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtPaymentRef.Text))
                {
                    DXMessageBox.Show("Please select Payment Ref #!");
                    txtPaymentRef.Focus();
                    return;
                }
                if (lookUpCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency!");
                    lookUpCurrency.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtPaymentAmount.Text) || Convert.ToDouble(txtPaymentAmount.Text) == 0)
                {
                    DXMessageBox.Show("Please enter Payment amount!");
                    txtPaymentAmount.Focus();
                    return;
                }
                if (cmbPaymentStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Status!");
                    cmbPaymentStatus.Focus();
                    return;
                }
                if (lookupPaymentMethod.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Payment method!");
                    lookupPaymentMethod.Focus();
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
                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                var tempPayment = Convert.ToDouble(grdCntrlPayment.Columns["DebitedAmount"].TotalSummaries[0].Value);
                var totalPayment = Math.Round(double.Parse(tempPayment.ToString()), 2);
                if (totalPayment != Convert.ToDouble(txtPaymentAmount.Text))
                {
                    DXMessageBox.Show("Payment amount is not matching with Total amount!");
                    return;
                }

                List<CompanyLoanPaymentModelView> selectedItems = new List<CompanyLoanPaymentModelView>();
                foreach (var _item in grdCntrlPayment.VisibleItems)
                {
                    if ((_item as CompanyLoanPaymentModelView).Total != 0)
                        selectedItems.Add((CompanyLoanPaymentModelView)_item);
                }

                List<Payment> paymentList = new List<Payment>();
                int index = 0;

                if (editFlag == true && payments.Count > 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null && payments[0].isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Payment is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            stage = TransactionStage.Approved.ToString();
                            isApproved = true;
                            approvalDate = System.DateTime.Now;
                        }
                    }
                }

                foreach (var _item in selectedItems)
                {
                    Payment payment = new Payment();

                    if (editFlag == true)
                    {
                        payment = payments.FirstOrDefault(x => x.LoansAdvanceId == _item.LoansAdvanceId);
                        if (payment == null)
                            payment = new Payment();

                        if (isApproved != null)
                            payment.isApproved = isApproved;
                        else if (underApproval == true)
                            payment.isApproved = false;
                        if (stage != null)
                            payment.stage = stage;
                        if (approvalDate != null)
                            payment.ApprovedDate = approvalDate;
                    }
                    else
                    {
                        payment.isApproved = false;
                        payment.stage = ERP_BL.Enums.TransactionStage.AwaitingApproval.ToString();
                    }
                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        payment.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        payment.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }

                    payment.transactionType = PaymentTransactionType.Loans_Advances;
                    payment.paymentLoansAdvancesTemplate = (PaymentLoansAdvancesTemplate)cmbxPaymentType.SelectedIndex;
                    payment.CreationDate = datCreationDate.DateTime;
                    payment.GLPostingDate = datglPostingdate.DateTime;


                    if (editFlag == false)
                        payment.transactionGroupId = intGroupId;

                    payment.company_Id = (lookupCompany.SelectedItem as Company).Id;
                    //payment.dept_Id = (lookupDepartment.SelectedItem as Department).Id;

                    if (deptList.Count != 0)
                    {
                        payment.departments = new List<Department>();
                        foreach (Department _dept in deptList)
                        {
                            if (!payment.departments.Contains(_dept))
                            {
                                payment.departments.Add(_dept);
                            }
                        }
                    }

                    if (cmbxPettyCashRef.SelectedIndex > 0)
                        payment.PaymentRefNoId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                    else
                        payment.PaymentRefNoId = null;

                    if (cmbPaymentTerm.SelectedItem != null)
                        payment.paymentterm_Id = (cmbPaymentTerm.SelectedItem as cmbitem).id;

                    payment.DebitedDate = datDebitedDate.DateTime;
                    payment.PaymentDate = datPaymentDate.DateTime;
                    payment.SystemRefNo = txtSystemRef.Text;
                    payment.PaymentRefNo = txtPaymentRef.Text;
                    payment.currency_Id = (lookUpCurrency.SelectedItem as Currency).Id;
                    payment.PaymentAmount = Convert.ToDouble(txtPaymentAmount.Text);
                    payment.statusId = (cmbPaymentStatus.SelectedItem as cmbitem).id;
                    payment.paymentMethodId = (lookupPaymentMethod.SelectedItem as PaymentMethod).Id;
                    payment.bankId = (lookupBanks.SelectedItem as Bank).Id;
                    payment.accountId = (lookupAccounts.SelectedItem as Account).Id;
                    payment.InstrumentNo = txtInstrumentNo.Text;
                    payment.InstrumentDate = datInstrumentDate.DateTime;

                    //payment.AdminBill_Id = _item.BillId;
                    payment.LoansAdvanceId = _item.LoansAdvanceId;
                    payment.BillCreationDate = _item.LACreationDate;
                    // payment.SystemRefNo = _item.SystemReferenceNo;
                    //payment.SystemRefNo = _item.SystemRefNo;
                    //payment.BillingMonth = _item.BillingMonth;
                    //payment.billda = _item.BillDate;
                    payment.BillAmount = _item.LoanAmount;
                    //payment.AmountDue = _item.AmountDue;
                    payment.DebitedAmount = _item.DebitedAmount;

                    if (editFlag == false)
                    {
                        payment.user_Id = SYSTEM_STATIC.currentUser.id;
                        payment.createdFromBill = createdFromBill;
                    }

                    List<PettyCash> pettyCashes = new List<PettyCash>();
                    if (editFlag == true && groupId != 0)
                    {
                        if (btnDeposit.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                PaymentId = payment.Id,
                                TransactionType = TransactionItemType.Payments,
                                debit = _item.Total,
                                credit = 0,
                                total = _item.Total - 0,
                                FinanceRefNo = txtPaymentRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = payment.loansAdvance.deptId,
                                companyId = payment.loansAdvance.companyId,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            payment.isDeposit = true;
                        }
                        else if (btnPayment.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                PaymentId = payment.Id,
                                TransactionType = TransactionItemType.Payments,
                                debit = 0,
                                credit = _item.Total,
                                total = 0 - _item.Total,
                                FinanceRefNo = txtPaymentRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = payment.loansAdvance == null ? null : payment.loansAdvance.deptId,
                                companyId = payment.loansAdvance == null ? null : payment.loansAdvance.companyId,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            payment.isDeposit = false;
                        }
                        else
                        {
                            payment.isDeposit = null;
                        }
                    }
                    else
                    {
                        if (btnDeposit.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                PaymentId = 0,
                                TransactionType = TransactionItemType.Payments,
                                debit = _item.Total,
                                credit = 0,
                                total = _item.Total - 0,
                                FinanceRefNo = txtPaymentRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).deptId,
                                companyId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).companyId,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            payment.isDeposit = true;
                        }
                        else if (btnPayment.IsChecked == true)
                        {
                            pettyCashes.Add(new PettyCash()
                            {
                                CreationDate = datCreationDate.DateTime,
                                PaymentId = 0,
                                TransactionType = TransactionItemType.Payments,
                                debit = 0,
                                credit = _item.Total,
                                total = 0 - _item.Total,
                                FinanceRefNo = txtPaymentRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                deptId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).deptId,
                                companyId = LoansAdvances.FirstOrDefault(x => x.Id == _item.LoansAdvanceId).companyId,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            payment.isDeposit = false;
                        }
                        else
                        {
                            payment.isDeposit = null;
                        }

                    }
                    payment.pettyCashes = pettyCashes;
                    if (cmbxVATBookRef.SelectedIndex > -1)
                    {
                        payment.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                    }
                    else
                    {
                        payment.VATBookRefId = null;
                    }
                    List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
                    if (editFlag == true && groupId != 0)
                    {
                        if (btnVATBookPost.IsChecked == true)
                        {
                            foreach (var tax in payment.PaymentTaxes)
                            {
                                vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                {
                                    CreationDate = datCreationDate.DateTime,
                                    paymentId = payment.Id,
                                    TransactionType = TransactionItemType.Payments,
                                    debit = tax.Amount,
                                    credit = 0,
                                    total = tax.Amount - 0,
                                    FinanceRefNo = txtPaymentRef.Text,
                                    SystemRefNo = txtSystemRef.Text,
                                    MER = Math.Round(Convert.ToDouble(payment.loansAdvance.MER), 2),
                                    deptId = payment.loansAdvance.deptId,
                                    companyId = payment.loansAdvance.companyId,
                                    currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                    VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                });
                                payment.isVATBookPosted = true;
                            }
                        }
                    }
                    List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                    var account = lookupAccounts.SelectedItem as Account;
                    var loanAdvance = paymentRepo.getLAForJournalTransactions((int)payment.LoansAdvanceId);
                    if (editFlag == true && payment.Id > 0)
                    {
                        if (isBypassCOA.IsChecked == true)
                        {
                        }
                        else
                        {
                            payment.isBypassBank = true;
                            payment.coaAccountId = null;
                            var dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.credit == Convert.ToDouble(txtPaymentAmount.Text));
                            if (banktransactionFlag == 0 && payment.isVoid != true)
                            {
                                JournalTransaction bankTransaction = new JournalTransaction();
                                if (account != null)
                                {
                                    if (account.COA_accountId != null)
                                    {
                                        if (btnPushCredits.IsChecked == true)
                                        {
                                            if (dbTrans == null)
                                            {
                                                bankTransaction.accountId = account.COA_accountId;
                                                bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                bankTransaction.creationDate = payment.GLPostingDate;
                                                bankTransaction.debit = 0;
                                                bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                                bankTransaction.userId = payment.user_Id;
                                                bankTransaction.PaymentId = payment.Id;
                                                bankTransaction.transactionRefno = txtPaymentRef.Text;
                                                bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
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
                                                    bankTransaction.PaymentId = dbTrans.PaymentId;
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
                                                    bankTransaction.PaymentId = dbTrans.PaymentId;
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
                        }
                        if (payment.LoansAdvanceId != null && payment.isVoid != true)
                        {
                            if (loanAdvance.applicantType != null)
                            {
                                if (loanAdvance.applicantType.account != null)
                                {
                                    if (btnPushDebits.IsChecked == true)
                                    {

                                        var dbTransaction = payment.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                 loanAdvance.applicantType.accountId &&
                                                 x.debit == _item.DebitedAmount &&
                                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                                 x.deptId == loanAdvance.deptId
                                                 );


                                        if (dbTransaction == null)
                                        {
                                            JournalTransaction BillTransaction = new JournalTransaction()
                                            {
                                                accountId = loanAdvance.applicantType.accountId,
                                                coaTransactionsType = coaTransactionsType.Payment,
                                                creationDate = payment.GLPostingDate,
                                                debit = _item.DebitedAmount,
                                                credit = 0,
                                                MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                userId = payment.user_Id,
                                                PaymentId = payment.Id,
                                                transactionRefno = txtPaymentRef.Text,
                                                total = _item.DebitedAmount - 0,
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
                                                    coaTransactionsType = coaTransactionsType.Payment,
                                                    creationDate = payment.GLPostingDate,
                                                    debit = _item.DebitedAmount,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                    userId = payment.user_Id,
                                                    PaymentId = payment.Id,
                                                    transactionRefno = txtPaymentRef.Text,
                                                    total = _item.DebitedAmount - 0,
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
                                                    coaTransactionsType = coaTransactionsType.Payment,
                                                    creationDate = payment.GLPostingDate,
                                                    debit = _item.DebitedAmount,
                                                    credit = 0,
                                                    MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                                    userId = payment.user_Id,
                                                    PaymentId = payment.Id,
                                                    transactionRefno = txtPaymentRef.Text,
                                                    total = _item.DebitedAmount - 0,
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
                        }
                        payment.journalTransactions = journalTransactions;
                    }
                    else
                    {
                        if (isBypassCOA.IsChecked == true)
                        {
                            payment.isBypassBank = true;
                            payment.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (payment.coaAccountId != null)
                            {
                                if (btnPushCredits.IsChecked == true)
                                {
                                    bankTransaction.accountId = payment.coaAccountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                    bankTransaction.creationDate = payment.GLPostingDate;
                                    bankTransaction.debit = 0;
                                    bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                    bankTransaction.userId = payment.user_Id;
                                    bankTransaction.PaymentId = payment.Id;
                                    bankTransaction.transactionRefno = txtPaymentRef.Text;
                                    bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
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
                            payment.isBypassBank = false;
                            payment.coaAccountId = null;
                            if (banktransactionFlag == 0 && payment.isVoid != true)
                            {
                                JournalTransaction bankTransaction = new JournalTransaction();
                                if (account != null)
                                {
                                    if (account.COA_accountId != null)
                                    {
                                        if (btnPushCredits.IsChecked == true)
                                        {

                                            bankTransaction.accountId = account.COA_accountId;
                                            bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            bankTransaction.creationDate = payment.GLPostingDate;
                                            bankTransaction.debit = 0;
                                            bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2);
                                            bankTransaction.userId = payment.user_Id;
                                            bankTransaction.PaymentId = payment.Id;
                                            bankTransaction.transactionRefno = txtPaymentRef.Text;
                                            bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
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
                                if (btnPushDebits.IsChecked == true)
                                {
                                    JournalTransaction BillTransaction = new JournalTransaction()
                                    {
                                        accountId = loanAdvance.applicantType.accountId,
                                        coaTransactionsType = coaTransactionsType.Payment,
                                        creationDate = payment.GLPostingDate,
                                        debit = _item.DebitedAmount,
                                        credit = 0,
                                        MER = Math.Round(Convert.ToDouble(loanAdvance.MER), 2),
                                        userId = payment.user_Id,
                                        PaymentId = payment.Id,
                                        transactionRefno = txtPaymentRef.Text,
                                        total = _item.DebitedAmount - 0,
                                        deptId = loanAdvance.deptId,
                                        companyId = loanAdvance.companyId,
                                        currencyId = loanAdvance.currencyId,

                                    };
                                    journalTransactions.Add(BillTransaction);
                                }
                            }
                        }
                    }
                    payment.journalTransactions = journalTransactions;
                    if (btnInsuranceRequired.IsChecked == true)
                    {
                        payment.insuranceRequired = true;
                    }
                    else
                    {
                        payment.insuranceRequired = false;
                    }
                    if (btnInsuranceApplied.IsChecked == true)
                    {
                        payment.insuranceApplied = true;
                    }
                    else
                    {
                        payment.insuranceApplied = false;
                    }
                    if (btnInsuranceNotApplicable.IsChecked == true)
                    {
                        payment.insuranceNotApplicable = true;
                    }
                    else
                    {
                        payment.insuranceNotApplicable = false;
                    }

                    if (editFlag == true)
                    {
                        if (payment.Id > 0)
                            payments[payments.FindIndex(x => x.Id == payment.Id)] = payment;
                        else
                        {
                            payment.transactionGroupId = payments[0].transactionGroupId;
                            payments.Add(payment);
                        }
                    }
                    else
                        paymentList.Add(payment);


                }
                if (editFlag == true)
                {
                    if (payments.FirstOrDefault(x => x.Id == 0) != null)
                    {
                        GroupIdCalculation();
                        payments.ForEach(x => x.transactionGroupId = intGroupId);
                        payments.ForEach(x => x.SystemRefNo = "Payment-" + intGroupId);

                    }
                    paymentRepo.UpdatePayments(payments);

                    var selectedStatus = cmbPaymentStatus.SelectedItem as cmbitem;
                    if (checkStatus.Id != selectedStatus.id)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>(); 
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();
                        var res = MessageBox.Show("Status of Payment has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            var companyy = lookupCompany.SelectedItem as Company;
                            if (deptList != null && companyy?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                List<User> usersList = new List<User>();
                                foreach (var _dept in deptList)
                                {
                                    usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, companyy.Id));

                                }
                                var userss = usersList.Distinct().ToList();
                                winTagUsers win = new winTagUsers(userss, payments[0].transactionGroupId, TransactionItemType.Payments);
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
                            Comment = "Status of Payment having System Reference: " + txtSystemRef.Text + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(groupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, groupId, (int)TransactionItemType.Payments, "Status Changed from (" + checkStatus.Status + ") to (" + selectedStatus.name + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, groupId, (int)TransactionItemType.Payments, frmInputBox.comment);



                    DXMessageBox.Show("Successfully Updated!");

                }
                else if (editFlag == false)
                {
                    paymentRepo.AddPayment(paymentList);
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

        private void CmbxPaymentType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

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

        private void LookupBanks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList.Count == 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment1.Focus();
                return;
            }
            if (lookUpCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please currency first!");
                lookUpCurrency.Focus();
            }
        }

        private void LookupBanks_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            var curr = lookUpCurrency.SelectedItem as Currency;
            //var department = lookupDepartment.SelectedItem as Department; 

            List<int> dept_ids = new List<int>();
            foreach (var _dept in deptList)
            {
                dept_ids.Add(_dept.Id);
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
                        foreach (var _deptt in _account.departments)
                        {
                            if (_account.currency.Id == curr.Id && dept_ids.Contains(_deptt.Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                            {
                                allowedAccounts.Add(_account);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var _account in accntList)
                    {
                        foreach (var _deptt in _account.departments)
                        {
                            if ((_account.currency.Id == curr.Id || _account.isAdjustmentAccount == true) && dept_ids.Contains(_deptt.Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal || _account.accountsCategory == AccountsCategory.Vendor))
                            {
                                allowedAccounts.Add(_account);
                                break;
                            }
                        }
                    }
                }


                lookupAccounts.ItemsSource = allowedAccounts;
            }
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var company = lookupCompany.SelectedItem as Company;

            CompanyRepo companyRepo = new CompanyRepo();
            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();
            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.IsPaymentType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (payments != null && payments.Count > 0 && editFlag == true)
                        if (payments[0].departments != null)
                            foreach (var _dept in payments[0].departments)
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
                loadVATBookReferenceNo();
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                List<Bank> banks = new List<Bank>();
                if (chkInterCompany.IsChecked == false)
                    banks = receiptRepo.GetBanksbyCompanyForPayments(company);
                else
                {
                    if (lookupInterCompany.SelectedIndex > -1)
                    {
                        banks = receiptRepo.GetBanksbyCompInterCompForPayments(company, lookupInterCompany.SelectedItem as Company);
                    }

                }

                lookupBanks.ItemsSource = banks;
            }
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

            if (chkInterCompany.IsChecked == true)
            {
                if (lookupInterCompany.SelectedIndex > -1)
                    references = BillsRepo.GetAllBillRefNo().Where(x => x.companyId == (lookupInterCompany.SelectedItem as Company).Id || x.companyId == (lookupCompany.SelectedItem as Company).Id).ToList();
            }
            else
            {
                references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);
            }

            if (editFlag == true && payments.Count > 0)
            {
                if (payments[0].PaymentRefNo1 != null && references.FirstOrDefault(x => x.Id == payments[0].PaymentRefNoId) == null)
                    references.Add(payments[0].PaymentRefNo1);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }

            cmbxPettyCashRef.ItemsSource = cmbitems;
        }


        //Only to get gridControl of LookupDepartment1
        GridControl gridControl = new GridControl();
        private void LookupDepartment1_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            string deptNames = "";

            gridControl = lookupDepartment1.GetGridControl();
            var treeView = gridControl.View as TreeListView;

            deptList.Clear();

            CallRecursive(treeView);
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

        private void LookUpVendor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList.Count == 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment1.Focus();
                return;
            }
        }

        private void TreeListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && companyChnaged == false)
            {
                var lookupDepts = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;
                int index = 0;
                if (payments[0].departments != null && payments[0].departments.Count > 0)
                {
                    var grid = lookupDepartment1.GetGridControl();
                    gridControl = lookupDepartment1.GetGridControl();
                    var treeView = gridControl.View as TreeListView;

                    var column = grid.Columns[0];
                    index = 0;
                    deptList = payments[0].departments;

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


        private void TreeListView_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            var grid = lookupDepartment1.GetGridControl();
            var row = grid.GetRow(e.Node.RowHandle) as Department;

            if (e.Node.IsChecked == true)
                deptList.Add(row);
            else
                deptList.Remove(row);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 20, "Viewed details of Payments");
            }
        }

        private void LookupInterCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            Company InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
            {
                loadInterCompanyDepartments();
                if (lookupCompany.SelectedIndex > -1)
                {
                    SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                    var banks = receiptRepo.GetBanksbyCompInterCompForPayments(lookupCompany.SelectedItem as Company, lookupInterCompany.SelectedItem as Company);
                    lookupBanks.ItemsSource = banks;
                }
                loadBillReferenceNo();
            }
        }

        public void loadInterCompanyDepartments()
        {
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupInterDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            Company InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
                if (InterCompany.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (Department dep in InterCompany.departments)
                        foreach (Department empdep in empUser.departments)
                            if (dep.Id == empdep.Id)
                            {
                                departments.Add(dep);
                            }
                    lookupInterDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }

        private void LookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = true;
            if (lookupInterCompany.SelectedIndex > -1 && lookupCompany.SelectedIndex > -1)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompInterCompForPayments(lookupCompany.SelectedItem as Company, lookupInterCompany.SelectedItem as Company);
                lookupBanks.ItemsSource = banks;
            }
            else if (lookupInterCompany.SelectedIndex < 0 && lookupCompany.SelectedIndex > -1)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompanyForPayments(lookupCompany.SelectedItem as Company);
                lookupBanks.ItemsSource = banks;
            }
            else if (lookupInterCompany.SelectedIndex > -1 && lookupCompany.SelectedIndex < 0)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompanyForPayments(lookupInterCompany.SelectedItem as Company);
                lookupBanks.ItemsSource = banks;
            }
            else
            {
                lookupBanks.ItemsSource = null;
            }
            loadBillReferenceNo();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = false;

            if (lookupCompany.SelectedIndex > -1)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompanyForPayments(lookupCompany.SelectedItem as Company);
                lookupBanks.ItemsSource = banks;
            }
            else
            {
                lookupBanks.ItemsSource = null;
            }
            loadBillReferenceNo();
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (grdCommentss.SelectedItem != null)
            {
                var reply = grdCommentss.SelectedItem as CommentLog;
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (payments[0].transactionGroupId > 0)
                {
                    Company InterCompany = lookupInterCompany.SelectedItem as Company;
                    Department InterDepartment = lookupInterDepartment.SelectedItem as Department;
                    Company company = lookupCompany.SelectedItem as Company;
                    if (deptList != null && deptList.Count > 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {
                        List<int> deptIds = new List<int>();
                        foreach (var _dept in deptList)
                            deptIds.Add(_dept.Id);
                        deptIds.Add(InterDepartment.Id);

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(deptIds, new List<int> { company.Id, InterCompany.Id }), reply, TransactionItemType.Payments);
                        inputBox.ShowDialog();

                    }
                    else if (deptList != null && deptList.Count > 0 && company?.Id != 0 && company != null)
                    {
                        List<User> usersList = new List<User>();
                        foreach (var _dept in deptList)
                        {
                            usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, payments[0].company_Id.Value));

                        }
                        var userss = usersList.Where(x => x.employee.Companies.FirstOrDefault(y => y.Id == company.Id) != null).ToList();
                        userss = userss.Distinct().ToList();

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, reply, TransactionItemType.Payments);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (payments[0].transactionGroupId > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && groupId != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Payment #" + txtSystemRef.Text, groupId, TransactionItemType.Payments, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (payments[0].transactionGroupId == 0)
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
                if (deptList != null && deptList.Count > 0)
                {
                    List<User> usersList = new List<User>();
                    foreach (var _dept in deptList)
                    {
                        usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                    }
                    var userss = usersList.Distinct().ToList();

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.Payments);
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
                    DXMessageBox.Show("Kindly save Payment first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (payments != null && payments.Count > 0)
            {
                //payments = paymentRepo.GetPIpaymentsByGroupId(payments[0].transactionGroupId);

                LoadLoansAdvanceData();
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Payment") != null)
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActivePaymentAttachmentCategories();
                    grdAttach1.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Attach File!");
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Payment") != null)
            {
                if (grdAttachments1.Visibility == Visibility.Visible)
                    grdAttachments1.Visibility = Visibility.Collapsed;
                else
                {
                    if (editFlag != false)
                    {
                        List<TreeItem> otherAttachments = new List<TreeItem>();

                        List<TreeItem> atachments = SYSTEM_STATIC.GetPaymentAttachmentsListByCategory(groupId, TransactionItemType.Payments);
                        CompanyLoanPaymentModelView pyment = (grdCntrlPayment.SelectedItem as CompanyLoanPaymentModelView);

                        if (pyment.LoansAdvanceId != 0)
                        {
                            AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                            var loansAdvance = loansAdvanceRepo.GetLoansAdvance(pyment.LoansAdvanceId);
                            //treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoice.Id, TransactionItemType.Sale_Invoice);


                            otherAttachments.AddRange(SYSTEM_STATIC.GetPaymentAttachmentsListByCategory((int)loansAdvance.Id, TransactionItemType.LoansAdvances));


                            if (loansAdvance.Payments.Count != 0)
                                foreach (var payment in loansAdvance.Payments)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetPaymentAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                }

                            //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                            foreach (var cat in atachments)
                            {
                                foreach (var otherCat in otherAttachments)
                                {
                                    if (otherCat.name == cat.name)
                                    {
                                        foreach (var file in otherCat.Items.Distinct())
                                        {
                                            cat.Items.Add(file);
                                        }
                                    }
                                }
                            }
                            treeViewAttachments1.ItemsSource = atachments.Distinct();
                        }
                    }
                    grdAttachments1.Visibility = Visibility.Visible;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                });
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
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
                        destination += "Attachments\\Payments\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Payments.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Payments);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Payments, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 20, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Payments);
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

        private void BtnDeposit_Checked(object sender, RoutedEventArgs e)
        {
            btnPayment.IsChecked = false;
        }

        private void BtnPayment_Checked(object sender, RoutedEventArgs e)
        {
            btnDeposit.IsChecked = false;
        }
        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            //ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            //var userChartofAccounts = chartofAccountsRepo.GetAllforVendorBills(lookupCompany.SelectedItem as Company, lookupDepartment1.SelectedItem as Department, SYSTEM_STATIC.currentUser.id);
            //lookupCOA.ItemsSource = userChartofAccounts;
        }

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void LookupCOA_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void IsBypassCOA_Checked(object sender, RoutedEventArgs e)
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            var userChartofAccounts = chartofAccountsRepo.GetAllforPayment(lookupCompany.SelectedItem as Company, deptList, SYSTEM_STATIC.currentUser.id);
            lookupCOA.ItemsSource = userChartofAccounts;
        }

        private void TableViewPayment_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            switch (e.MenuInfo.Column.FieldName)
            {
                case "Deductions":
                    mbtnAddDeductions.IsVisible = true;
                    break;
                case "VAT":
                    mbtnAddDeductions.IsVisible = true;
                    break;
                default:
                    mbtnAddDeductions.IsVisible = false;
                    break;
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
            List<JournalTransaction> finalJournalTransactions = new List<JournalTransaction>();
            var tempPayment = Convert.ToDouble(grdCntrlPayment.Columns["DebitedAmount"].TotalSummaries[0].Value);
            var totalPayment = Math.Round(double.Parse(tempPayment.ToString()), 2);

            List<CompanyLoanPaymentModelView> selectedItems = new List<CompanyLoanPaymentModelView>();
            foreach (var _item in grdCntrlPayment.VisibleItems)
            {
                if ((_item as CompanyLoanPaymentModelView).Total != 0)
                    selectedItems.Add((CompanyLoanPaymentModelView)_item);
            }

            List<Payment> paymentList = new List<Payment>();
            int index = 0;
            foreach (var _item in selectedItems)
            {
                Payment payment = new Payment();
                if (editFlag == true)
                {
                    payment = payments[index];
                    index++;
                }
                payment.PaymentAmount = Convert.ToDouble(txtPaymentAmount.Text);

                //payment.AdminBill_Id = _item.BillId;
                payment.LoansAdvanceId = _item.LoansAdvanceId;
                payment.BillCreationDate = _item.LACreationDate;
                payment.SystemRefNo = _item.SystemReferenceNo;
                payment.BillAmount = _item.LoanAmount;
                payment.DebitedAmount = _item.DebitedAmount;
                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                var account = lookupAccounts.SelectedItem as Account;
                var loansAdvance = paymentRepo.getLAForJournalTransactions((int)payment.LoansAdvanceId);
                if (editFlag == true)
                {
                    if (isBypassCOA.IsChecked == true)
                    {

                    }
                    else
                    {
                        payment.isBypassBank = true;
                        payment.coaAccountId = null;
                        var dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.credit == Convert.ToDouble(txtPaymentAmount.Text));
                        if (banktransactionFlag == 0 && payment.isVoid != true)
                        {
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (account != null)
                            {
                                if (account.COA_accountId != null)
                                {
                                    if (btnPushCredits.IsChecked == true)
                                    {

                                        if (dbTrans == null)
                                        {
                                            bankTransaction.accountId = account.COA_accountId;
                                            bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            bankTransaction.creationDate = payment.GLPostingDate;
                                            bankTransaction.debit = 0;
                                            bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2);
                                            bankTransaction.userId = payment.user_Id;
                                            bankTransaction.PaymentId = payment.Id;
                                            bankTransaction.transactionRefno = txtPaymentRef.Text;
                                            bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                            bankTransaction.deptId = loansAdvance.deptId;
                                            bankTransaction.companyId = loansAdvance.companyId;
                                            bankTransaction.currencyId = loansAdvance.currencyId;
                                        }
                                        else
                                        {
                                            bankTransaction.accountId = dbTrans.accountId;
                                            bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                            bankTransaction.creationDate = dbTrans.creationDate;
                                            bankTransaction.debit = dbTrans.debit;
                                            bankTransaction.credit = dbTrans.credit;
                                            bankTransaction.MER = dbTrans.MER;
                                            bankTransaction.userId = dbTrans.userId;
                                            bankTransaction.PaymentId = dbTrans.PaymentId;
                                            bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                            bankTransaction.total = dbTrans.total;
                                            bankTransaction.deptId = dbTrans.deptId;
                                            bankTransaction.companyId = dbTrans.companyId;
                                            bankTransaction.currencyId = loansAdvance.currencyId;
                                            bankTransaction.isReconciled = dbTrans.isReconciled;
                                            bankTransaction.reconcilationDate = dbTrans.reconcilationDate;
                                        }
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }

                                }
                            }
                        }

                    }

                    if (payment.loansAdvance.Id != null && payment.isVoid != true)
                    {
                        if (payment.loansAdvance.applicantType != null)
                        {
                            if (payment.loansAdvance.applicantType.account != null)
                            {
                                if (btnPushDebits.IsChecked == true)
                                {
                                    JournalTransaction BillTransaction = new JournalTransaction()
                                    {
                                        accountId = loansAdvance.applicantType.accountId,
                                        coaTransactionsType = coaTransactionsType.Payment,
                                        creationDate = payment.GLPostingDate,
                                        debit = _item.DebitedAmount,
                                        credit = 0,
                                        MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2),
                                        //MER = 1,
                                        userId = payment.user_Id,
                                        PaymentId = payment.Id,
                                        transactionRefno = txtPaymentRef.Text,
                                        total = _item.DebitedAmount - 0,
                                        deptId = payment.loansAdvance.deptId,
                                        companyId = payment.loansAdvance.companyId,
                                        currencyId = payment.loansAdvance.currencyId
                                        //deptId = payment.de.Id
                                    };
                                    journalTransactions.Add(BillTransaction);
                                }
                            }
                        }
                        else
                        {
                            //if(payment.loansAdvance.ApplicantEmployee!=null)
                            //{
                            //    if (payment.loansAdvance.ApplicantEmployee.receivableAccountId != null)
                            //    {
                            //        if (btnPushDebits.IsChecked == true)
                            //        {
                            //            JournalTransaction BillTransaction = new JournalTransaction()
                            //            {
                            //                accountId = loansAdvance.ApplicantEmployee.receivableAccountId,
                            //                coaTransactionsType = coaTransactionsType.Payment,
                            //                creationDate = payment.GLPostingDate,
                            //                debit = _item.DebitedAmount,
                            //                credit = 0,
                            //                MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2),
                            //                //MER = 1,
                            //                userId = payment.user_Id,
                            //                PaymentId = payment.Id,
                            //                transactionRefno = txtPaymentRef.Text,
                            //                total = _item.DebitedAmount - 0,
                            //                deptId = payment.loansAdvance.deptId,
                            //                companyId = payment.loansAdvance.companyId,
                            //                currencyId = payment.loansAdvance.currencyId
                            //                //deptId = payment.de.Id
                            //            };
                            //            journalTransactions.Add(BillTransaction);
                            //        }
                            //    }

                            //}

                        }
                    }


                }
                else
                {
                    if (isBypassCOA.IsChecked == true)
                    {
                        payment.isBypassBank = true;
                        payment.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                        JournalTransaction bankTransaction = new JournalTransaction();
                        if (payment.coaAccountId != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                bankTransaction.accountId = payment.coaAccountId;
                                bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                bankTransaction.creationDate = payment.GLPostingDate;
                                bankTransaction.debit = 0;
                                bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                bankTransaction.MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2);
                                bankTransaction.userId = payment.user_Id;
                                bankTransaction.PaymentId = payment.Id;
                                bankTransaction.transactionRefno = txtPaymentRef.Text;
                                bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                bankTransaction.deptId = loansAdvance.deptId;
                                bankTransaction.companyId = loansAdvance.companyId;
                                bankTransaction.currencyId = loansAdvance.currencyId;

                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                    }
                    else
                    {
                        payment.isBypassBank = false;
                        payment.coaAccountId = null;
                        if (banktransactionFlag == 0 && payment.isVoid != true)
                        {
                            JournalTransaction bankTransaction = new JournalTransaction();
                            if (account != null)
                            {
                                if (account.COA_accountId != null)
                                {
                                    if (btnPushCredits.IsChecked == true)
                                    {

                                        bankTransaction.accountId = account.COA_accountId;
                                        bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                        bankTransaction.creationDate = payment.GLPostingDate;
                                        bankTransaction.debit = 0;
                                        bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2);
                                        bankTransaction.userId = payment.user_Id;
                                        bankTransaction.PaymentId = payment.Id;
                                        bankTransaction.transactionRefno = txtPaymentRef.Text;
                                        bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                        bankTransaction.deptId = loansAdvance.deptId;
                                        bankTransaction.companyId = loansAdvance.companyId;
                                        bankTransaction.currencyId = loansAdvance.currencyId;

                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }
                                }
                            }
                        }

                    }
                    if (payment.LoansAdvanceId != null && payment.isVoid != true)
                    {

                        //if (payment.loansAdvance.ApplicantEmployee.receivableAccountId != null)
                        //{
                        //    if (btnPushDebits.IsChecked == true)
                        //    {
                        //        JournalTransaction BillTransaction = new JournalTransaction()
                        //        {
                        //            accountId = loansAdvance.ApplicantEmployee.receivableAccountId,
                        //            coaTransactionsType = coaTransactionsType.Payment,
                        //            creationDate = payment.GLPostingDate,
                        //            debit = _item.DebitedAmount,
                        //            credit = 0,
                        //            MER = Math.Round(Convert.ToDouble(loansAdvance.MER), 2),
                        //            //MER = 1,
                        //            userId = payment.user_Id,
                        //            PaymentId = payment.Id,
                        //            transactionRefno = txtPaymentRef.Text,
                        //            total = _item.DebitedAmount - 0,
                        //            deptId =loansAdvance.deptId,
                        //            companyId =loansAdvance.companyId,
                        //            currencyId = loansAdvance.currencyId
                        //            //deptId = payment.de.Id
                        //        };
                        //        journalTransactions.Add(BillTransaction);
                        //    }
                        //}
                    }
                }
                finalJournalTransactions.AddRange(journalTransactions);

            }
            banktransactionFlag = 0;
            return finalJournalTransactions;
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editFlag == true)
            {
                var source = grdCntrlPayment.ItemsSource as List<CompanyLoanPaymentModelView>;
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
                grdTrackingTree.ExpandAllNodes();

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
        public void GellAllOrdersTracking()
        {
            if (grdCntrlPayment.SelectedItem != null)
            {
                var idd = (grdCntrlPayment.SelectedItem as CompanyLoanPaymentModelView).Id;
                //var groupIdd = (grdCntrlPayment.SelectedItem as PaymentModelView).GroupId;
                if (idd != 0)
                {

                    OrderTracking tracking = new OrderTracking();
                    grdOrdersTracking.ItemsSource = tracking.getTransactions(idd, TransactionItemType.Payments);
                }
            }
            else
            {
                DXMessageBox.Show("Select any Payment first!");
            }
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
                        ucFrmCompanyLoanPayment frmLAreceipt = new ucFrmCompanyLoanPayment();
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
        private void btnInsuranceRequired_Checked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Visibility = Visibility.Visible;
            imgInsuarance.Background = Brushes.Gray;
            btnInsuranceApplied.IsEnabled = true;
            btnInsuranceNotApplicable.IsEnabled = true;
        }
        private void btnInsuranceRequired_Unchecked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Visibility = Visibility.Collapsed;
        }
        private void btnInsuranceApplied_Checked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Content = "Insurance Applied";
            imgInsuarance.Background = Brushes.Green;
            btnInsuranceNotApplicable.IsChecked = false;
            if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
            {
                insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
            }
            firstInsuranceCheck = true;
        }

        private void btnInsuranceApplied_Unchecked(object sender, RoutedEventArgs e)
        {
            if (btnInsuranceNotApplicable.IsChecked != true)
            {
                imgInsuarance.Content = "Insurance Required";
                imgInsuarance.Background = Brushes.Gray;
                btnInsuranceApplied.IsChecked = false;

                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
            }
        }
        private void btnInsuranceNotApplicable_Checked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Visibility = Visibility.Visible;
            imgInsuarance.Content = "Insurance N/A";
            imgInsuarance.Background = Brushes.Orange;
            btnInsuranceApplied.IsChecked = false;
            if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
            {
                insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
            }
            firstInsuranceCheck = true;
        }
        private void btnInsuranceNotApplicable_Unchecked(object sender, RoutedEventArgs e)
        {
            if (btnInsuranceNotApplicable.IsChecked != true && btnInsuranceApplied.IsChecked != true)
            {
                imgInsuarance.Content = "Insurance Required";
                imgInsuarance.Background = Brushes.Gray;
                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
            }
            else
            if (btnInsuranceNotApplicable.IsChecked != true && btnInsuranceApplied.IsChecked == true)
            {
                imgInsuarance.Content = "Insurance Applied";
                imgInsuarance.Background = Brushes.Green;
                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
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

        private void BtnReLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadGridData();
        }

        private void btnCreateReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Create Direct Receipt from Payments") != null)
            {
                ucFrmDirectReceiptPayment frmDirectReceiptPO = new ucFrmDirectReceiptPayment();
                frmDirectReceiptPO.editFlag = false;
                frmDirectReceiptPO.paymentId = groupId;
                frmDirectReceiptPO.receiptId = 0;
                Window win = new Window();
                win.Content = frmDirectReceiptPO;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Direct Receipt from Payments!");
            }
        }

        private void btnCreateIBT_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers(groupId);
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

        private void btnCreateICBT_Click(object sender, RoutedEventArgs e)
        {
            ucBankTransferInterCompany uc = new ucBankTransferInterCompany(groupId, false);
            Window win = new Window();
            win.Content = uc;

            win.WindowState = WindowState.Maximized;
            win.Show();
        }
        private void cmbPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPaymentStatus.SelectedItem != null)
            {
                var selectedStatus = cmbPaymentStatus.SelectedItem as cmbitem;
                var status = paymentRepo.GetPaymentStatus(selectedStatus.id);
                if (status.isPaid == true)
                {
                    btnPushDebits.IsChecked = true;
                    btnPushCredits.IsChecked = true;
                }
                else
                {
                    btnPushDebits.IsChecked = false;
                    btnPushCredits.IsChecked = false;
                }
            }
        }
    }
}
