using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.CreditCards;
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
using System.Collections.ObjectModel;
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
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.ModelViews;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.Procurementss.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmBillPayments.xaml
    /// </summary>
    public partial class ucFrmPayments : UserControl
    {
        List<AdminBill> adminBills = new List<AdminBill>();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        EmployeeRepo empRepo = new EmployeeRepo();
        PaymentRepo paymentRepo = new PaymentRepo();
        int intGroupId;
        public bool editFlag = false;
        bool? companyChnaged = null;
        List<Payment> payments = new List<Payment>();
        public Window frmPaymentWindow = new Window();

        public int groupId = 0;

        static PaymentStatus statusChanged = new PaymentStatus();

        UsersRepo UsersRepo = new UsersRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        PaymentStatus checkStatus = new PaymentStatus();
        public int adminBillsGroupId = 0;
        public bool createdFromBill = false;
        List<Department> deptList = new List<Department>();
        string stage;
        bool? isApproved;
        bool? underApproval;
        //bool? isReApproved;
        DateTime? approvalDate;
        int banktransactionFlag = 0;
        TaxRepo taxRepo = new TaxRepo();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<PaymentStatus> PaymentStatuses = new List<PaymentStatus>();
        ERP_BL.Databases.Employee insuarenceAppliedEmployee = new ERP_BL.Databases.Employee();
        bool firstInsuranceCheck = false;

        public ucFrmPayments()
        {
            InitializeComponent();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 20, "Viewed details of Payments");
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
            cmbAppliedBy.ItemsSource = cmbitems;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                loadPaymentTypes();
                loadCompanies();
                loadCurrencies();
                loadPaymentStatus();
                loadPaymentMethods();
                loadPaymentTerms();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Insurance") != null)
                {
                    insuranceGrid.IsEnabled = true;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment Reference Number in Payments") != null)
                {
                    cmbxPettyCashRef.IsEnabled = true;
                }
                else
                {
                    cmbxPettyCashRef.IsEnabled = false;
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
                    datglPostingdate.EditValue = DateTime.Now;
                    btnRefresh.IsEnabled = false;
                    if (adminBillsGroupId != 0)
                    {
                        btnLoad.IsEnabled = false;


                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        List<PaymentModelView> modelViewList = new List<PaymentModelView>();


                        adminBills = billsRepo.GetBillsByGroupId(adminBillsGroupId);

                        if (adminBills != null && adminBills.Count > 0)
                        {

                            int index = 0;
                            //Select Company
                            var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                            if (adminBills[0].company != null)
                            {

                                index = 0;
                                foreach (var _company in companyList)
                                {

                                    if (_company.Id == adminBills[0].company_Id)
                                    {
                                        lookupCompany.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }

                            }



                            List<Vendor> vendors = new List<Vendor>();
                            string deptNames = "";


                            if (adminBills[0].department != null)
                            {
                                deptList.Add(adminBills[0].department);
                            }


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


                            //var department = lookupDepartment.SelectedItem as Department;
                            //var vendors = department.Vendors.ToList();


                            //lookUpVendor.ItemsSource = vendors;



                            ////Select Vendor
                            //var vendorList = (lookUpVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : lookUpVendor.ItemsSource as List<Vendor>;
                            //if (adminBills[0].vendor != null)
                            //{
                            //    index = 0;
                            //    foreach (var _vendor in vendorList)
                            //    {
                            //        if (_vendor.Id == adminBills[0].vendor_Id)
                            //        {
                            //            lookUpVendor.SelectedIndex = index;
                            //            index = 0;
                            //            break;
                            //        }
                            //        index++;
                            //    }
                            //}


                            //Select Currency
                            var currencyList = (lookUpCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : lookUpCurrency.ItemsSource as List<Currency>;
                            if (adminBills[0].currency != null)
                            {

                                index = 0;
                                foreach (var _currency in currencyList)
                                {

                                    if (_currency.Id == adminBills[0].currency_Id)
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
                            lookUpVendor.IsEnabled = false;
                            lookUpCurrency.IsEnabled = false;


                            foreach (var _bill in adminBills)
                            {
                                PaymentModelView bill = new PaymentModelView();
                                bill.BillId = _bill.Id;
                                bill.GroupId = _bill.transactionGroupId;
                                bill.BillCreationDate = (DateTime)_bill.CreationDate;

                                if (_bill.vendor != null)
                                    bill.vendor = _bill.vendor.company.CompanyName;


                                bill.BillFinanceRefNo = _bill.FinanceRefNo;
                                bill.BillNumber = _bill.Bill_Number;
                                bill.BillingMonth = (DateTime)_bill.BillingMonthFrom;
                                bill.BillDueDate = (DateTime)_bill.DueDate;
                                bill.Memo = _bill.Memo;
                                bill.SystemRefNo = _bill.SystemRefNo;

                                if (_bill.vendor != null)
                                    bill.vendor = _bill.vendor.company.CompanyName;


                                if (_bill.primaryCreditCardNo != null)
                                    bill.PrimaryCardNo = _bill.primaryCreditCardNo.CardNumber;

                                if (_bill.secondaryCreditCardNo != null)
                                    bill.SecondaryCardNo = _bill.secondaryCreditCardNo.CardNumber;


                                if (_bill.chartofAccount != null)
                                    bill.CoaDebit = _bill.chartofAccount.accountName;

                                if (_bill.chartofAccountCredit != null)
                                    bill.CoaCredit = _bill.chartofAccountCredit.accountName;

                                if (_bill.AdminBillType != null)
                                    bill.AdminBillLink = _bill.AdminBillType.name;

                                if (_bill.adminBillNature != null)
                                    bill.AdminBillNature = _bill.adminBillNature.Nature;

                                if (_bill.currency != null)
                                    bill.Currency = _bill.currency.CurrencyName;

                                var adjustedAmount = _bill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                                if (_bill.Payments.Count == 0)
                                {
                                    if (_bill.AmountWithTax != 0)
                                    {
                                        bill.OriginalAmount = _bill.AmountWithTax;
                                        bill.AmountDue = _bill.AmountWithTax - adjustedAmount;
                                    }
                                    else
                                    {
                                        bill.OriginalAmount = _bill.AmountOC;
                                        bill.AmountDue = _bill.AmountOC - adjustedAmount;
                                    }
                                }

                                else
                                {
                                    var amountPaid = _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                                    double remainingAmount = 0;

                                    if (_bill.AmountWithTax != 0)
                                    {
                                        bill.OriginalAmount = _bill.AmountWithTax;
                                        remainingAmount = _bill.AmountWithTax - amountPaid;
                                    }
                                    else
                                    {
                                        bill.OriginalAmount = _bill.AmountOC;
                                        remainingAmount = _bill.AmountOC - amountPaid;
                                    }
                                    bill.AmountDue = remainingAmount - adjustedAmount;
                                }

                                bill.AdminBillStage = GetBillStatus(_bill);
                                modelViewList.Add(bill);
                            }
                            grdCntrlPayment.ItemsSource = modelViewList;
                        }
                    }
                }

                if (editFlag == true && groupId > 0)
                {
                    payments = paymentRepo.GetAdminBillPaymentsByGroupId(groupId);
                    if (payments[0].isDeposit == true)
                        btnDeposit.IsChecked = true;
                    else if (payments[0].isDeposit == false)
                        btnPayment.IsChecked = true;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Admin Bill Payment") == null)
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Deductions After Approval") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Admin Bill Payment") == null)
                        {
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
                            txtDeductionAmount.IsEnabled = true;
                            grdCntrlPayment.Columns["DebitedAmount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                            btnSave.IsEnabled = true;
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Deductions After Approval") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Admin Bill Payment") == null)
                        {
                            btnSave.IsEnabled = false;
                        }
                    }



                    btnLoad.IsEnabled = false;


                    views = UsersRepo.getViwerInfo(groupId, 20);
                    grdUsers.ItemsSource = views;
                    //loadcomments();

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

                    loadcomments();
                    loadAttachments();


                    if (payments[0].createdFromBill == true)
                    {
                        lookupCompany.IsEnabled = false;
                        lookupDepartment1.IsEnabled = false;
                        lookUpVendor.IsEnabled = false;
                        lookUpCurrency.IsEnabled = false;
                    }


                    int index = 0;


                    //Select Template
                    for (int i = 0; i <= (int)ERP_BL.Enums.PaymentAdminBillTemplate.Admin_Bill_Credit_Card; i++)
                    {

                        if (((ERP_BL.Enums.PaymentAdminBillTemplate)i).ToString() == payments[0].paymentAdminBillTemplate.ToString())
                        {
                            cmbxPaymentType.SelectedIndex = i;
                            break;
                        }
                    }




                    if (payments[0].CreationDate != null)
                        datCreationDate.EditValue = (DateTime)payments[0].CreationDate;
                    if (payments[0].GLPostingDate != null)
                    {
                        datglPostingdate.EditValue = payments[0].GLPostingDate;
                    }
                    else
                    {
                        datglPostingdate.EditValue = (DateTime)payments[0].CreationDate;

                    }

                    if (payments[0].DebitedDate != null)
                        datDebitedDate.EditValue = (DateTime)payments[0].DebitedDate;


                    if (payments[0].InstrumentDate != null)
                        datInstrumentDate.EditValue = (DateTime)payments[0].InstrumentDate;


                    if (payments[0].PaymentDate != null)
                        datPaymentDate.EditValue = (DateTime)payments[0].PaymentDate;


                    //Select Company
                    var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                    if (payments[0].company_Id != null)
                    {
                        if (payments[0].company != null && companyList.Find(x => x.Id == payments[0].company_Id) == null)
                        {
                            companyList.Add(payments[0].company);
                            lookupCompany.ItemsSource = null;
                            lookupCompany.ItemsSource = companyList;
                        }
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



                    //Select Department
                    //var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                    //if (payments[0].department != null)
                    //{
                    //    index = 0;
                    //    foreach (var _dept in deptList)
                    //    {
                    //        if (_dept.Id == payments[0].dept_Id)
                    //        {
                    //            lookupDepartment.SelectedIndex = index;
                    //            index = 0;
                    //            break;
                    //        }
                    //        index++;
                    //    }
                    //}


                    //var lookupDepts = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                    //if (payments[0].departments != null && payments[0].departments.Count > 0)
                    //{
                    //    var grid = lookupDepartment1.GetGridControl();
                    //    index = 0;
                    //    deptList = payments[0].departments;


                    //    List<int> deptIds = new List<int>();
                    //    foreach (var _dept in deptList)
                    //        deptIds.Add(_dept.Id);


                    //    foreach (Department _deptt in lookupDepts)
                    //    {
                    //        if (deptIds.Contains(_deptt.Id))
                    //        {
                    //            //var row = grid.GetRow(index);
                    //            lookupDepartment1.SelectedItems.Add(lookupDepartment1.GetItemByKeyValue(index));
                    //        }
                    //        index++;
                    //    }
                    //}


                    List<Vendor> vendors = new List<Vendor>();
                    string deptNames = "";

                    if (payments[0].departments != null && payments[0].departments.Count > 0)
                    {
                        deptList = payments[0].departments;
                    }


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
                        loadEmployees();
                    }

                    lookupDepartment1.EditValue = deptNames;


                    //var department = lookupDepartment.SelectedItem as Department;
                    //var vendors = department.Vendors.ToList();


                    lookUpVendor.ItemsSource = vendors;
                    lookupSelectVendor.ItemsSource = vendors;



                    //Select Vendor
                    var vendorList = (lookUpVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : lookUpVendor.ItemsSource as List<Vendor>;
                    if (payments[0].vendor_Id != null)
                    {

                        index = 0;
                        foreach (var _vendor in vendorList)
                        {

                            if (_vendor.Id == payments[0].vendor_Id)
                            {
                                lookUpVendor.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    else
                    {
                        lookUpVendor.IsEnabled = false;
                    }


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



                    //Select Credit Card Bank
                    var creditCardBankList = (lookupCreditCardBank.ItemsSource as List<Bank>) == null ? new List<Bank>() : lookupCreditCardBank.ItemsSource as List<Bank>;
                    if (payments[0].creditCardBankId != null)
                    {

                        index = 0;
                        foreach (var _bank in creditCardBankList)
                        {

                            if (_bank.Id == payments[0].creditCardBankId)
                            {
                                lookupCreditCardBank.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }

                    }

                    //Select Card User
                    var primaryCardList = (cmbPrimaryCardNo.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbPrimaryCardNo.ItemsSource as List<cmbitem>;
                    if (payments[0].PrimaryCreditCardNoId != null)
                    {

                        index = 0;
                        foreach (var _card in primaryCardList)
                        {

                            if (_card.id == payments[0].PrimaryCreditCardNoId)
                            {
                                cmbPrimaryCardNo.SelectedIndex = index;
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
                    txtDeductionAmount.Text = Convert.ToDouble(payments.Sum(x => x.Deductions) + payments.Sum(x => x.totalVATamount)).ToString();

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
                        if(payments[0].Status.isPaid==true)
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


                    if (payments[0].InstrumentDate != null)
                        datInstrumentDate.DateTime = (DateTime)payments[0].InstrumentDate;
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
                        btnVATBookPost.IsChecked = false;
                    LoadPaymentData();
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

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushDebits.IsEnabled = true;
                    btnPushCredits.IsEnabled = true;

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
                LoadPendingInvoices();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
            grdTrackingTree.ExpandAllNodes();
        }

        public void LoadPendingInvoices()
        {
            try
            {
                if (lookUpVendor.SelectedIndex > -1)
                {
                    lookupSelectVendor.ItemsSource = lookUpVendor.ItemsSource as List<Vendor>;
                    lookupSelectVendor.Text = (lookUpVendor.SelectedItem as Vendor).company.CompanyName;
                }
                LoadVendorProfile("Open");
                btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";

            }
            catch (Exception)
            {
            }
        }
        public void LoadVendorProfile(string _type)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            var type = _type;
            if (!string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case "Open":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetOpenPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }

                            break;
                        }
                    case "Close":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetClosePurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "All":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetAllPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Pending for Approval":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetPendingForApprovalPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                }

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void loadPaymentTerms()
        {
            //cmbPaymentTerm.ItemsSource = SYSTEM_STATIC.paymentTermSource;

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

        private void LoadPaymentData()
        {
            List<PaymentModelView> modelViewList = new List<PaymentModelView>();


            foreach (var _payment in payments)
            {
                PaymentModelView payment = new PaymentModelView();

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

                payment.id = _payment.Id;
                payment.BillId = (int)_payment.AdminBill_Id;
                payment.GroupId = _payment.transactionGroupId;
                payment.BillCreationDate = (DateTime)_payment.adminBill.CreationDate;

                if (_payment.adminBill.vendor != null)
                    payment.vendor = _payment.adminBill.vendor.company.CompanyName;

                if (_payment.adminBill.primaryCreditCardNo != null)
                    payment.PrimaryCardNo = _payment.adminBill.primaryCreditCardNo.CardNumber;

                if (_payment.adminBill.secondaryCreditCardNo != null)
                    payment.SecondaryCardNo = _payment.adminBill.secondaryCreditCardNo.CardNumber;

                if (_payment.adminBill.chartofAccount != null)
                    payment.CoaDebit = _payment.adminBill.chartofAccount.accountName;

                if (_payment.adminBill.chartofAccountCredit != null)
                    payment.CoaCredit = _payment.adminBill.chartofAccountCredit.accountName;

                if (_payment.adminBill.AdminBillType != null)
                    payment.AdminBillLink = _payment.adminBill.AdminBillType.name;

                if (_payment.adminBill.adminBillNature != null)
                    payment.AdminBillNature = _payment.adminBill.adminBillNature.Nature;

                if (_payment.adminBill.currency != null)
                    payment.Currency = _payment.adminBill.currency.CurrencyName;

                payment.BillFinanceRefNo = _payment.adminBill.FinanceRefNo;
                payment.BillNumber = _payment.adminBill.Bill_Number;
                payment.BillingMonth = (DateTime)_payment.adminBill.BillingMonthFrom;
                payment.BillDueDate = (DateTime)_payment.adminBill.DueDate;

                payment.Memo = _payment.adminBill.Memo;
                payment.SystemRefNo = _payment.adminBill.SystemRefNo;

                payment.OriginalAmount = Math.Round(_payment.adminBill.AmountWithTax, 2);
                payment.DebitedAmount = _payment.DebitedAmount;

                payment.Total = _payment.DebitedAmount + _payment.Deductions + _payment.totalVATamount;

                var amountPaid = _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount) /*+ _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
                var adjustedAmount = _payment.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                if (_payment.adminBill.AmountWithTax != 0)
                {
                    var remainingAmount = _payment.adminBill.AmountWithTax - amountPaid;
                    payment.AmountDue = remainingAmount - adjustedAmount;
                }
                else
                {
                    var remainingAmount = _payment.adminBill.AmountOC - amountPaid;
                    payment.AmountDue = remainingAmount - adjustedAmount;
                }

                payment.VAT = _payment.totalVATamount;
                payment.paymentTaxes = _payment.PaymentTaxes;
                payment.IsAdjusted = _payment.IsAdjusted;

                //bill.AmountToPay = _bill.AmountOC;
                payment.Deductions = _payment.Deductions;

                payment.paymentDeductions = _payment.paymentDeductions;
                payment.AdminBillStage = GetBillStatus(_payment.adminBill);
                if (_payment.transactionHolderId != null)
                {
                    payment.transactionHolderId = _payment.transactionHolderId;
                    payment.holderChangeDate = _payment.holderChangeDate;
                }
                modelViewList.Add(payment);
            }

            grdCntrlPayment.ItemsSource = modelViewList;
        }

        private string GetBillStatus(AdminBill _billl)
        {
            PaymentRepo repo = new PaymentRepo();
            var billl = repo.GetAdminBill(_billl.Id);

            if (billl.isVoid == true)
            {
                return "Void";
            }
            else if (billl.isReApproved == false)
            {
                return "Under Re-Approval";
            }
            else if (billl.isApproved == true && billl.stage == "Closed")
            {
                return "Closed";
            }
            else if (billl.isApproved == true && billl.BillStatus.isActive == false && billl.PendingForClosing != true)
            {
                return "Closed";
            }
            else if (billl.isApproved == true && billl.PendingForClosing == true)
            {
                return "Under Closing";
            }
            else if (billl.isApproved == true)
            {
                return "Approved";
            }
            else if (billl.isApproved == false)
            {
                return "Under Approval";
            }
            else if (billl.PendingForClosing == true)
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
                    comments = procurementRepo.getcommentslogAsc(groupId, TransactionItemType.Payments);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        public void loadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Payments);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        private void loadPaymentMethods()
        {
            lookupPaymentMethod.ItemsSource = paymentRepo.GetAllPaymentMethods();
        }

        private void loadPaymentStatus()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
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


        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            var currencies = currencyRepo.getAll().Where(x => x.isVoid != true).ToList();
            lookUpCurrency.ItemsSource = currencies;
        }

        private void loadPaymentTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.PaymentAdminBillTemplate.Admin_Bill_Credit_Card; i++)
            {
                cmbxPaymentType.Items.Add(((ERP_BL.Enums.PaymentAdminBillTemplate)i).ToString());
            }
        }

        private void loadCompanies()
        {
            empUser = paymentRepo.GetEmployeeForPayments(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }

        private void LoadGridData()
        {
            if (cmbxPaymentType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Payment Type");
                cmbxPaymentType.Focus();
                return;
            }
            var paymentType = cmbxPaymentType.SelectedItem.ToString();

            switch (paymentType)
            {
                case "Admin_Bills":
                    LoadAdminBillData();
                    break;
                case "Admin_Bill_Credit_Card":
                    LoadAdminBillByCreditCard();
                    break;



                default:
                    break;
            }




        }

        private void LoadAdminBillByCreditCard()
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
            if (lookupCreditCardBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Bank for Credit Cards!");
                lookupCreditCardBank.Focus();
                return;
            }
            if (cmbPrimaryCardNo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Credit Cards!");
                cmbPrimaryCardNo.Focus();
                return;
            }


            //List<AdminBill> Bills = new List<AdminBill>();
            AdminBillsRepo billsRepo = new AdminBillsRepo();



            adminBills = billsRepo.GetAllBillsByCompDeptPrimaryCard((lookupCompany.SelectedItem as Company).Id, deptList, (cmbPrimaryCardNo.SelectedItem as cmbitem).id, (lookUpCurrency.SelectedItem as Currency).Id);

            List<PaymentModelView> modelViewList = new List<PaymentModelView>();


            foreach (var _bill in adminBills)
            {
                PaymentModelView bill = new PaymentModelView();
                bill.BillId = _bill.Id;
                bill.GroupId = _bill.transactionGroupId;
                bill.BillCreationDate = (DateTime)_bill.CreationDate;

                if (_bill.vendor != null)
                    bill.vendor = _bill.vendor.company.CompanyName;

                if (_bill.primaryCreditCardNo != null)
                    bill.PrimaryCardNo = _bill.primaryCreditCardNo.CardNumber;

                if (_bill.secondaryCreditCardNo != null)
                    bill.SecondaryCardNo = _bill.secondaryCreditCardNo.CardNumber;

                if (_bill.chartofAccount != null)
                    bill.CoaDebit = _bill.chartofAccount.accountName;

                if (_bill.chartofAccountCredit != null)
                    bill.CoaCredit = _bill.chartofAccountCredit.accountName;

                if (_bill.AdminBillType != null)
                    bill.AdminBillLink = _bill.AdminBillType.name;

                if (_bill.adminBillNature != null)
                    bill.AdminBillNature = _bill.adminBillNature.Nature;

                if (_bill.currency != null)
                    bill.Currency = _bill.currency.CurrencyName;

                bill.BillFinanceRefNo = _bill.FinanceRefNo;
                bill.BillNumber = _bill.Bill_Number;
                bill.BillingMonth = (DateTime)_bill.BillingMonthFrom;
                bill.BillDueDate = (DateTime)_bill.DueDate;
                bill.Memo = _bill.Memo;
                bill.SystemRefNo = _bill.SystemRefNo;

                if (_bill.AmountWithTax != 0)
                    bill.OriginalAmount = _bill.AmountWithTax;
                else
                    bill.OriginalAmount = _bill.AmountOC;

                if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    if (_bill.AmountWithTax != 0)
                        bill.AmountDue = _bill.AmountWithTax;
                    else
                        bill.AmountDue = _bill.AmountOC;
                }
                else
                {
                    if (_bill.AmountWithTax != 0)
                        bill.AmountDue = _bill.AmountWithTax - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                    else
                        bill.AmountDue = _bill.AmountOC - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                }
                bill.AdminBillStage = GetBillStatus(_bill);

                modelViewList.Add(bill);
            }

            grdCntrlPayment.ItemsSource = modelViewList;
        }

        private void LoadAdminBillData()
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


            //List<AdminBill> Bills = new List<AdminBill>();
            AdminBillsRepo billsRepo = new AdminBillsRepo();

            if (lookUpVendor.SelectedIndex >= 0)

                adminBills = billsRepo.GetAllBillsByCompDeptVendor((lookupCompany.SelectedItem as Company).Id, deptList, (lookUpVendor.SelectedItem as Vendor).Id, (lookUpCurrency.SelectedItem as Currency).Id);
            else

                adminBills = billsRepo.GetAllBillsByCompDept((lookupCompany.SelectedItem as Company).Id, deptList, (lookUpCurrency.SelectedItem as Currency).Id);

            List<PaymentModelView> modelViewList = new List<PaymentModelView>();

            if (editFlag == true && payments != null && payments.Count > 0)
            {
                var billIds = adminBills.Select(x => x.Id).ToList();
                var paymentIds = payments.Select(x => x.AdminBill_Id.Value).ToList();

                var finalIds = paymentIds.Except(billIds).ToList();

                var transactionGroupId = payments[0].transactionGroupId;


                foreach (var idd in finalIds)
                {
                    PaymentModelView bill = new PaymentModelView();

                    var _payment = payments.FirstOrDefault(x => x.AdminBill_Id == idd);
                    if (_payment != null)
                    {
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

                        bill.id = _payment.Id;
                        bill.BillId = (int)_payment.AdminBill_Id;
                        bill.GroupId = transactionGroupId;
                        bill.BillCreationDate = (DateTime)_payment.CreationDate;

                        if (_payment.adminBill.vendor != null)
                            bill.vendor = _payment.adminBill.vendor.company.CompanyName;

                        if (_payment.adminBill.primaryCreditCardNo != null)
                            bill.PrimaryCardNo = _payment.adminBill.primaryCreditCardNo.CardNumber;

                        if (_payment.adminBill.secondaryCreditCardNo != null)
                            bill.SecondaryCardNo = _payment.adminBill.secondaryCreditCardNo.CardNumber;

                        if (_payment.adminBill.chartofAccount != null)
                            bill.CoaDebit = _payment.adminBill.chartofAccount.accountName;

                        if (_payment.adminBill.chartofAccountCredit != null)
                            bill.CoaCredit = _payment.adminBill.chartofAccountCredit.accountName;

                        if (_payment.adminBill.AdminBillType != null)
                            bill.AdminBillLink = _payment.adminBill.AdminBillType.name;

                        if (_payment.adminBill.adminBillNature != null)
                            bill.AdminBillNature = _payment.adminBill.adminBillNature.Nature;

                        if (_payment.adminBill.currency != null)
                            bill.Currency = _payment.adminBill.currency.CurrencyName;

                        bill.BillFinanceRefNo = _payment.BillFinanceRefNo;
                        bill.BillNumber = _payment.BillNumber;
                        bill.BillingMonth = (DateTime)_payment.BillingMonth;
                        bill.BillDueDate = (DateTime)_payment.BillDueDate;

                        bill.Memo = _payment.adminBill.Memo;
                        bill.SystemRefNo = _payment.adminBill.SystemRefNo;

                        bill.OriginalAmount = Math.Round(_payment.adminBill.AmountWithTax, 2);
                        bill.DebitedAmount = _payment.DebitedAmount;

                        bill.Total = _payment.DebitedAmount + _payment.Deductions + _payment.totalVATamount;

                        var amountPaid = _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount) /*+ _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
                        var adjustedAmount = _payment.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                        if (_payment.adminBill.AmountWithTax != 0)
                        {
                            var remainingAmount = _payment.adminBill.AmountWithTax - amountPaid;
                            bill.AmountDue = remainingAmount - adjustedAmount;
                        }
                        else
                        {
                            var remainingAmount = _payment.adminBill.AmountOC - amountPaid;
                            bill.AmountDue = remainingAmount - adjustedAmount;
                        }

                        bill.VAT = _payment.totalVATamount;
                        bill.paymentTaxes = _payment.PaymentTaxes;
                        bill.IsAdjusted = _payment.IsAdjusted;

                        //bill.AmountToPay = _bill.AmountOC;
                        bill.Deductions = _payment.Deductions;

                        bill.paymentDeductions = _payment.paymentDeductions;
                        bill.AdminBillStage = GetBillStatus(_payment.adminBill);
                        if (_payment.transactionHolderId != null)
                        {
                            bill.transactionHolderId = _payment.transactionHolderId;
                            bill.holderChangeDate = _payment.holderChangeDate;
                        }

                        modelViewList.Add(bill);
                    }
                }

                foreach (var _bill in adminBills)
                {
                    PaymentModelView bill = new PaymentModelView();

                    var _payment = payments.FirstOrDefault(x => x.AdminBill_Id == _bill.Id);
                    if (_payment != null)
                    {
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

                        bill.id = _payment.Id;
                        bill.BillId = (int)_payment.AdminBill_Id;
                        bill.GroupId = transactionGroupId;
                        bill.BillCreationDate = (DateTime)_payment.CreationDate;

                        if (_payment.adminBill.vendor != null)
                            bill.vendor = _payment.adminBill.vendor.company.CompanyName;

                        if (_payment.adminBill.primaryCreditCardNo != null)
                            bill.PrimaryCardNo = _payment.adminBill.primaryCreditCardNo.CardNumber;

                        if (_payment.adminBill.secondaryCreditCardNo != null)
                            bill.SecondaryCardNo = _payment.adminBill.secondaryCreditCardNo.CardNumber;

                        if (_payment.adminBill.chartofAccount != null)
                            bill.CoaDebit = _payment.adminBill.chartofAccount.accountName;

                        if (_payment.adminBill.chartofAccountCredit != null)
                            bill.CoaCredit = _payment.adminBill.chartofAccountCredit.accountName;

                        if (_payment.adminBill.AdminBillType != null)
                            bill.AdminBillLink = _payment.adminBill.AdminBillType.name;

                        if (_payment.adminBill.adminBillNature != null)
                            bill.AdminBillNature = _payment.adminBill.adminBillNature.Nature;

                        if (_payment.adminBill.currency != null)
                            bill.Currency = _payment.adminBill.currency.CurrencyName;

                        bill.BillFinanceRefNo = _payment.BillFinanceRefNo;
                        bill.BillNumber = _payment.BillNumber;
                        bill.BillingMonth = (DateTime)_payment.BillingMonth;
                        bill.BillDueDate = (DateTime)_payment.BillDueDate;

                        bill.Memo = _payment.adminBill.Memo;
                        bill.SystemRefNo = _payment.adminBill.SystemRefNo;

                        bill.OriginalAmount = Math.Round(_payment.adminBill.AmountWithTax, 2);
                        bill.DebitedAmount = _payment.DebitedAmount;

                        bill.Total = _payment.DebitedAmount + _payment.Deductions + _payment.totalVATamount;

                        var amountPaid = _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount) /*+ _payment.adminBill.Payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
                        var adjustedAmount = _payment.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                        if (_payment.adminBill.AmountWithTax != 0)
                        {
                            var remainingAmount = _payment.adminBill.AmountWithTax - amountPaid;
                            bill.AmountDue = remainingAmount - adjustedAmount;
                        }
                        else
                        {
                            var remainingAmount = _payment.adminBill.AmountOC - amountPaid;
                            bill.AmountDue = remainingAmount - adjustedAmount;
                        }

                        bill.VAT = _payment.totalVATamount;
                        bill.paymentTaxes = _payment.PaymentTaxes;
                        bill.IsAdjusted = _payment.IsAdjusted;

                        //bill.AmountToPay = _bill.AmountOC;
                        bill.Deductions = _payment.Deductions;

                        bill.paymentDeductions = _payment.paymentDeductions;
                        bill.AdminBillStage = GetBillStatus(_payment.adminBill);
                        if (_payment.transactionHolderId != null)
                        {
                            bill.transactionHolderId = _payment.transactionHolderId;
                            bill.holderChangeDate = _payment.holderChangeDate;
                        }
                    }
                    else
                    {
                        bill.BillId = _bill.Id;
                        bill.GroupId = transactionGroupId;
                        bill.BillCreationDate = (DateTime)_bill.CreationDate;

                        if (_bill.vendor != null)
                            bill.vendor = _bill.vendor.company.CompanyName;

                        if (_bill.chartofAccount != null)
                            bill.CoaDebit = _bill.chartofAccount.accountName;

                        if (_bill.chartofAccountCredit != null)
                            bill.CoaCredit = _bill.chartofAccountCredit.accountName;

                        if (_bill.AdminBillType != null)
                            bill.AdminBillLink = _bill.AdminBillType.name;

                        if (_bill.adminBillNature != null)
                            bill.AdminBillNature = _bill.adminBillNature.Nature;

                        if (_bill.currency != null)
                            bill.Currency = _bill.currency.CurrencyName;

                        bill.BillFinanceRefNo = _bill.FinanceRefNo;
                        bill.BillNumber = _bill.Bill_Number;
                        bill.BillingMonth = (DateTime)_bill.BillingMonthFrom;
                        bill.BillDueDate = (DateTime)_bill.DueDate;
                        bill.Memo = _bill.Memo;
                        bill.SystemRefNo = _bill.SystemRefNo;

                        if (_bill.AmountWithTax != 0)
                            bill.OriginalAmount = _bill.AmountWithTax;
                        else
                            bill.OriginalAmount = _bill.AmountOC;

                        if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                        {
                            if (_bill.AmountWithTax != 0)
                                bill.AmountDue = _bill.AmountWithTax;
                            else
                                bill.AmountDue = _bill.AmountOC;
                        }
                        else
                        {
                            if (_bill.AmountWithTax != 0)
                                bill.AmountDue = _bill.AmountWithTax - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                            else
                                bill.AmountDue = _bill.AmountOC - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                        }
                        bill.AdminBillStage = GetBillStatus(_bill);
                    }
                    //bill.AmountToPay = _bill.AmountOC;
                    modelViewList.Add(bill);
                }



            }
            else
            {

                foreach (var _bill in adminBills)
                {
                    PaymentModelView bill = new PaymentModelView();
                    bill.BillId = _bill.Id;
                    bill.GroupId = _bill.transactionGroupId;
                    bill.BillCreationDate = (DateTime)_bill.CreationDate;

                    if (_bill.vendor != null)
                        bill.vendor = _bill.vendor.company.CompanyName;

                    if (_bill.chartofAccount != null)
                        bill.CoaDebit = _bill.chartofAccount.accountName;

                    if (_bill.chartofAccountCredit != null)
                        bill.CoaCredit = _bill.chartofAccountCredit.accountName;

                    if (_bill.AdminBillType != null)
                        bill.AdminBillLink = _bill.AdminBillType.name;

                    if (_bill.adminBillNature != null)
                        bill.AdminBillNature = _bill.adminBillNature.Nature;

                    if (_bill.currency != null)
                        bill.Currency = _bill.currency.CurrencyName;

                    bill.BillFinanceRefNo = _bill.FinanceRefNo;
                    bill.BillNumber = _bill.Bill_Number;
                    bill.BillingMonth = (DateTime)_bill.BillingMonthFrom;
                    bill.BillDueDate = (DateTime)_bill.DueDate;
                    bill.Memo = _bill.Memo;
                    bill.SystemRefNo = _bill.SystemRefNo;

                    if (_bill.AmountWithTax != 0)
                        bill.OriginalAmount = _bill.AmountWithTax;
                    else
                        bill.OriginalAmount = _bill.AmountOC;

                    if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                    {
                        if (_bill.AmountWithTax != 0)
                            bill.AmountDue = _bill.AmountWithTax;
                        else
                            bill.AmountDue = _bill.AmountOC;
                    }
                    else
                    {
                        if (_bill.AmountWithTax != 0)
                            bill.AmountDue = _bill.AmountWithTax - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                        else
                            bill.AmountDue = _bill.AmountOC - _bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                    }
                    bill.AdminBillStage = GetBillStatus(_bill);

                    //bill.AmountToPay = _bill.AmountOC;
                    modelViewList.Add(bill);
                }
            }

            grdCntrlPayment.ItemsSource = modelViewList;
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                double debt_amount = Convert.ToDouble(e.GetListSourceFieldValue("DebitedAmount"));
                double ded = Convert.ToDouble(e.GetListSourceFieldValue("Deductions"));

                //Adding the Values of four columns to show in the Total column
                e.Value = debt_amount + ded;

                var a = e.ListSourceRowIndex;
                var b = (PaymentModelView)grdCntrlPayment.GetRow(a);
                var prod = debt_amount + ded;
                b.Total = prod;
            }
        }

        private void GrdCntrlPayments_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void GrdCntrlPayments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        //private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    var department = lookupDepartment.SelectedItem as Department;
        //    var vendors = department.Vendors.ToList();

        //    lookUpVendor.ItemsSource = vendors;
        //}

        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                lookupCompany.Focus();
                return;
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
                    foreach (var _dept in empUser.departments.Where(x => x.IsAdminBillType == true && x.isActive == true))
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
                var banks = receiptRepo.GetBanksbyCompany(company);
                lookupBanks.ItemsSource = banks;
                lookupCreditCardBank.ItemsSource = banks;
            }
        }

        private void loadBillReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            var references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);

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

        private void CmbxBanks_SelectedIndexChanged(object sender, RoutedEventArgs e)
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
                            if ((_account.currency.Id == curr.Id || _account.isAdjustmentAccount == true) && dept_ids.Contains(_deptt.Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
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

        private void CmbxBanks_GotFocus(object sender, RoutedEventArgs e)
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

        private void CmbxAccounts_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupBanks.SelectedIndex < 0)
            {

                DXMessageBox.Show("Please select Bank!");
                lookupBanks.Focus();
                return;
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
                            //payments.All(x=>x.pettyCashes.All(y=>y.isVoid == false));
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

                                    winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
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
                                Subject = "Payments UnVoided",
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
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Payment") != null)
                    {
                        if (DXMessageBox.Show("This Transaction is not currently in the list of Void Payments! Do you want to move it to Void Payments?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            //payments.All(x => x.pettyCashes.All(y => y.isVoid = true));
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

                                    winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
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

                    AdminBillsRepo billsRepo = new AdminBillsRepo();
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

                                            winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
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
                                        Subject = "Payments UnApproved",
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

                                            winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                for (int i = 0; i < paymentsForApproval.Count; i++)
                                                {
                                                    if (paymentsForApproval[0].transactionHolderId != win.tagUsers[0].employeeId)
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
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                paymentRepo.ApprovePayment(paymentsForApproval, deptss);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

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
                    //Load_Receipts();

                }

                //}

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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
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
            if (cmbxPaymentType.SelectedIndex == 1)
            {
                if (cmbPrimaryCardNo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Primary Card No.!");
                    cmbPrimaryCardNo.Focus();
                    return;
                }
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


            var tempDeduction = Convert.ToDouble(grdCntrlPayment.Columns["Deductions"].TotalSummaries[0].Value);
            var tempVAT = Convert.ToDouble(grdCntrlPayment.Columns["VAT"].TotalSummaries[0].Value);
            var totalDeduction = Math.Round(double.Parse((tempDeduction + tempVAT).ToString()), 2);
            if (totalDeduction != Convert.ToDouble(txtDeductionAmount.Text))
            {
                DXMessageBox.Show("Deduction and VAT amount not matching with Total Deduction!");
                return;
            }

            var tempPayment = Convert.ToDouble(grdCntrlPayment.Columns["DebitedAmount"].TotalSummaries[0].Value);
            var totalPayment = Math.Round(double.Parse(tempPayment.ToString()), 2);
            if (totalPayment != Convert.ToDouble(txtPaymentAmount.Text))
            {
                DXMessageBox.Show("Payment amount is not matching with Total amount!");
                return;
            }

            if (btnDeposit.IsChecked == true || btnPayment.IsChecked == true)
            {
                if (cmbxPettyCashRef.SelectedIndex <= 0)
                {
                    DXMessageBox.Show("Please select Petty Cash Reference #!");
                    cmbxPettyCashRef.Focus();
                    return;
                }
            }

            List<PaymentModelView> selectedItems = new List<PaymentModelView>();

            foreach (var _item in grdCntrlPayment.VisibleItems)
            {
                if ((_item as PaymentModelView).Total != 0)
                    selectedItems.Add((PaymentModelView)_item);
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
                    //var allDeductions = paymentRepo.GetDeductions(_item.id);
                    //_item.paymentDeductions = allDeductions;
                    payment = payments.FirstOrDefault(x => x.AdminBill_Id == _item.BillId);
                    if (payment == null)
                        payment = new Payment();
                    //index++;

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

                payment.transactionType = PaymentTransactionType.Admin_Bills;
                payment.paymentAdminBillTemplate = (PaymentAdminBillTemplate)cmbxPaymentType.SelectedIndex;
                payment.CreationDate = datCreationDate.DateTime;
                payment.GLPostingDate = datglPostingdate.DateTime;
                if (editFlag == false)
                    payment.transactionGroupId = intGroupId;
                else
                    payment.transactionGroupId = _item.GroupId;

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

                if ((PaymentAdminBillTemplate)cmbxPaymentType.SelectedIndex == PaymentAdminBillTemplate.Admin_Bill_Credit_Card)
                {
                    if (lookupCreditCardBank.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Credit Card Bank!");
                        lookupCreditCardBank.Focus();
                        return;
                    }
                    if (cmbPrimaryCardNo.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Primary Credit Card!");
                        cmbPrimaryCardNo.Focus();
                        return;
                    }

                    payment.creditCardBankId = (lookupCreditCardBank.SelectedItem as Bank).Id;
                    payment.PrimaryCreditCardNoId = (cmbPrimaryCardNo.SelectedItem as cmbitem).id;
                }

                if (cmbPaymentTerm.SelectedItem != null)
                    payment.paymentterm_Id = (cmbPaymentTerm.SelectedItem as cmbitem).id;

                if (lookUpVendor.SelectedIndex > -1)
                    payment.vendor_Id = (lookUpVendor.SelectedItem as Vendor).Id;

                if (cmbxPettyCashRef.SelectedIndex > 0)
                    payment.PaymentRefNoId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                else
                    payment.PaymentRefNoId = null;

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
                payment.AdminBill_Id = _item.BillId;
                payment.BillCreationDate = _item.BillCreationDate;
                payment.BillFinanceRefNo = _item.BillFinanceRefNo;
                payment.BillNumber = _item.BillNumber;
                payment.BillingMonth = _item.BillingMonth;
                payment.BillDueDate = _item.BillDueDate;
                payment.BillAmount = _item.OriginalAmount;
                //payment.AmountDue = _item.AmountDue;
                payment.DebitedAmount = _item.DebitedAmount;
                payment.Deductions = _item.Deductions;
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    payment.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    payment.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }

                if (editFlag == false)
                {
                    payment.user_Id = SYSTEM_STATIC.currentUser.id;
                    payment.createdFromBill = createdFromBill;
                }
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
                if (insuarenceAppliedEmployee.EmpId != 0)
                {
                    payment.insuranceAppliedBy_Id = insuarenceAppliedEmployee.EmpId;
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
                            MER = Math.Round(Convert.ToDouble(payment.adminBill.MER), 2),
                            deptId = payment.adminBill.dept_Id,
                            companyId = payment.adminBill.company_Id,
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
                            MER = Math.Round(Convert.ToDouble(payment.adminBill.MER), 2),
                            deptId = payment.adminBill.dept_Id,
                            companyId = payment.adminBill.company_Id,
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

                            MER = adminBills.FirstOrDefault(x => x.Id == _item.BillId).MER,
                            deptId = adminBills.FirstOrDefault(x => x.Id == _item.BillId).dept_Id,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
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
                            MER = adminBills.FirstOrDefault(x => x.Id == _item.BillId).MER,
                            deptId = adminBills.FirstOrDefault(x => x.Id == _item.BillId).dept_Id,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
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







                AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
                if (_item.paymentDeductions != null)
                    if (_item.paymentDeductions.Count != 0)
                    {
                        payment.paymentDeductions = _item.paymentDeductions;
                    }
                payment.Deductions = _item.Deductions;


                if (_item.paymentTaxes != null)
                    if (_item.paymentTaxes.Count != 0)
                    {

                        payment.PaymentTaxes = _item.paymentTaxes;
                    }
                payment.totalVATamount = _item.VAT;
                payment.IsAdjusted = _item.IsAdjusted;

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
                                GLPostingDate = datCreationDate.DateTime,
                                paymentId = payment.Id,
                                TransactionType = TransactionItemType.Payments,
                                debit = tax.Amount,
                                credit = 0,
                                total = tax.Amount - 0,
                                FinanceRefNo = txtPaymentRef.Text,
                                SystemRefNo = txtSystemRef.Text,
                                MER = Math.Round(Convert.ToDouble(payment.adminBill.MER), 2),
                                deptId = payment.adminBill.dept_Id,
                                companyId = payment.adminBill.company_Id,
                                currencyId = (lookUpCurrency.SelectedItem as Currency).Id,
                                VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                            });
                            payment.isVATBookPosted = true;
                        }
                        payment.VATBooks = vatBooks;
                    }
                }
                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                var adminBill = adminBillsRepo.get((int)payment.AdminBill_Id);
                var account = lookupAccounts.SelectedItem as Account;
                if (editFlag == true && payment.Id > 0)
                {
                    if (isBypassCOA.IsChecked == true)
                    {
                        payment.isBypassBank = true;
                        payment.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                        var dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == Convert.ToDouble(txtPaymentAmount.Text));
                        JournalTransaction bankTransaction = new JournalTransaction();
                        if (payment.coaAccountId != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                if (dbTrans == null)
                                {
                                    bankTransaction.accountId = payment.coaAccountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                    bankTransaction.creationDate = payment.GLPostingDate;
                                    bankTransaction.debit = 0;
                                    bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                    bankTransaction.userId = payment.user_Id;
                                    bankTransaction.PaymentId = payment.Id;
                                    bankTransaction.transactionRefno = txtPaymentRef.Text;
                                    bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                    bankTransaction.deptId = adminBill.dept_Id;
                                    bankTransaction.companyId = adminBill.company_Id;
                                    bankTransaction.currencyId = adminBill.currency_Id;
                                }
                                else
                                {
                                    if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        bankTransaction.accountId = dbTrans.accountId;
                                        bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                        bankTransaction.creationDate = payment.GLPostingDate;
                                        bankTransaction.debit = dbTrans.debit;
                                        bankTransaction.credit = dbTrans.credit;
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                        bankTransaction.userId = dbTrans.userId;
                                        bankTransaction.PaymentId = dbTrans.PaymentId;
                                        bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                        bankTransaction.total = dbTrans.total;
                                        bankTransaction.deptId = dbTrans.deptId;
                                        bankTransaction.companyId = dbTrans.companyId;
                                        bankTransaction.currencyId = dbTrans.currencyId;
                                        bankTransaction.isReconciled = false;
                                        bankTransaction.reconcilationDate = null;
                                        bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                        bankTransaction.ReconcilationId = null;
                                    }
                                    else
                                    {

                                        bankTransaction.accountId = dbTrans.accountId;
                                        bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                        bankTransaction.creationDate = payment.GLPostingDate;
                                        bankTransaction.debit = dbTrans.debit;
                                        bankTransaction.credit = dbTrans.credit;
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                        bankTransaction.userId = dbTrans.userId;
                                        bankTransaction.PaymentId = dbTrans.PaymentId;
                                        bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                        bankTransaction.total = dbTrans.total;
                                        bankTransaction.deptId = dbTrans.deptId;
                                        bankTransaction.companyId = dbTrans.companyId;
                                        bankTransaction.currencyId = dbTrans.currencyId;
                                        bankTransaction.isReconciled = dbTrans.isReconciled;
                                        bankTransaction.reconcilationDate = dbTrans.reconcilationDate;
                                    }
                                }
                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == deduction.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbDeduction.chartofAccountId && x.debit == deduction.Amount);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbDeduction.chartofAccountId;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = deduction.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = deduction.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = payment.GLPostingDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = false;
                                                    deducTransaction.reconcilationDate = null;
                                                    deducTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                    deducTransaction.ReconcilationId = null;
                                                    journalTransactions.Add(deducTransaction);
                                                }
                                                else
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = payment.GLPostingDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                    deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                    journalTransactions.Add(deducTransaction);
                                                }

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = payment.coaAccountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = deduction.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - deduction.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = creditDbTrans.accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = payment.GLPostingDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = false;
                                                deducBankTransaction.reconcilationDate = null;
                                                deducBankTransaction.ReconcilationId = null;
                                                deducBankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                            else
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = creditDbTrans.accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = payment.GLPostingDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                                deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                        }
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = debitDbTrans.creationDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = false;
                                                    deducTransaction.reconcilationDate = null;
                                                    deducTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                    deducTransaction.ReconcilationId = null;
                                                    journalTransactions.Add(deducTransaction);
                                                }
                                                else
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = debitDbTrans.creationDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                    deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                    journalTransactions.Add(deducTransaction);
                                                }

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            if (creditDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = account.COA_accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = 0 - creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = false;
                                                deducBankTransaction.reconcilationDate = null;
                                                deducBankTransaction.ReconcilationId = null;
                                                deducBankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                            else
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = account.COA_accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = 0 - creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                                deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                            bankTransaction.userId = payment.user_Id;
                                            bankTransaction.PaymentId = payment.Id;
                                            bankTransaction.transactionRefno = txtPaymentRef.Text;
                                            bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                            bankTransaction.deptId = adminBill.dept_Id;
                                            bankTransaction.companyId = adminBill.company_Id;
                                            bankTransaction.currencyId = adminBill.currency_Id;
                                        }
                                        else
                                        {
                                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                bankTransaction.accountId = dbTrans.accountId;
                                                bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                                bankTransaction.creationDate = payment.GLPostingDate;
                                                bankTransaction.debit = dbTrans.debit;
                                                bankTransaction.credit = dbTrans.credit;
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                                bankTransaction.userId = dbTrans.userId;
                                                bankTransaction.PaymentId = dbTrans.PaymentId;
                                                bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                                bankTransaction.total = dbTrans.total;
                                                bankTransaction.deptId = dbTrans.deptId;
                                                bankTransaction.companyId = dbTrans.companyId;
                                                bankTransaction.currencyId = adminBill.currency_Id;
                                                bankTransaction.isReconciled = false;
                                                bankTransaction.reconcilationDate = null;
                                                bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                bankTransaction.ReconcilationId = null;
                                            }
                                            else
                                            {
                                                bankTransaction.accountId = dbTrans.accountId;
                                                bankTransaction.coaTransactionsType = dbTrans.coaTransactionsType;
                                                bankTransaction.creationDate = payment.GLPostingDate;
                                                bankTransaction.debit = dbTrans.debit;
                                                bankTransaction.credit = dbTrans.credit;
                                                bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                                bankTransaction.userId = dbTrans.userId;
                                                bankTransaction.PaymentId = dbTrans.PaymentId;
                                                bankTransaction.transactionRefno = dbTrans.transactionRefno;
                                                bankTransaction.total = dbTrans.total;
                                                bankTransaction.deptId = dbTrans.deptId;
                                                bankTransaction.companyId = dbTrans.companyId;
                                                bankTransaction.currencyId = adminBill.currency_Id;
                                                bankTransaction.isReconciled = dbTrans.isReconciled;
                                                bankTransaction.reconcilationDate = dbTrans.reconcilationDate;
                                            }
                                        }
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }

                                }
                            }
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.credit == deduction.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbDeduction.chartofAccountId && x.debit == deduction.Amount);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbDeduction.chartofAccountId;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = deduction.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = deduction.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = payment.GLPostingDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = false;
                                                    deducTransaction.reconcilationDate = null;
                                                    deducTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                    deducTransaction.ReconcilationId = null;
                                                    journalTransactions.Add(deducTransaction);
                                                }
                                                else
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = payment.GLPostingDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                    deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                    journalTransactions.Add(deducTransaction);
                                                }

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = deduction.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - deduction.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = creditDbTrans.accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = false;
                                                deducBankTransaction.reconcilationDate = null;
                                                deducBankTransaction.ReconcilationId = null;
                                                deducBankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                            else
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = creditDbTrans.accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                                deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                        }
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                if (debitDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = debitDbTrans.creationDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = false;
                                                    deducTransaction.reconcilationDate = null;
                                                    deducTransaction.ReconcilationId = null;
                                                    deducTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                    journalTransactions.Add(deducTransaction);
                                                }
                                                else
                                                {
                                                    deducTransaction.accountId = debitDbTrans.accountId;
                                                    deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                    deducTransaction.creationDate = debitDbTrans.creationDate;
                                                    deducTransaction.debit = debitDbTrans.debit;
                                                    deducTransaction.credit = debitDbTrans.credit;
                                                    deducTransaction.userId = debitDbTrans.userId;
                                                    deducTransaction.MER = debitDbTrans.MER;
                                                    deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                    deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                    deducTransaction.deptId = debitDbTrans.deptId;
                                                    deducTransaction.total = debitDbTrans.total;
                                                    deducTransaction.companyId = debitDbTrans.companyId;
                                                    deducTransaction.currencyId = debitDbTrans.currencyId;
                                                    deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                    deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                    journalTransactions.Add(deducTransaction);
                                                }

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            if (creditDbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = account.COA_accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = 0 - creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = false;
                                                deducBankTransaction.reconcilationDate = null;
                                                deducBankTransaction.ReconcilationId = null;
                                                deducBankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                            else
                                            {
                                                JournalTransaction deducBankTransaction = new JournalTransaction();
                                                deducBankTransaction.accountId = account.COA_accountId;
                                                deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                                deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                                deducBankTransaction.debit = creditDbTrans.debit;
                                                deducBankTransaction.credit = creditDbTrans.credit;
                                                deducBankTransaction.userId = creditDbTrans.userId;
                                                deducBankTransaction.MER = creditDbTrans.MER;
                                                deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                                deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                                deducBankTransaction.deptId = creditDbTrans.deptId;
                                                deducBankTransaction.total = 0 - creditDbTrans.total;
                                                deducBankTransaction.companyId = creditDbTrans.companyId;
                                                deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                                deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                                deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                                journalTransactions.Add(deducBankTransaction);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (payment.AdminBill_Id != null && payment.isVoid != true)
                    {

                        AdminBillsRepo adminBillRepo = new AdminBillsRepo();


                        if (adminBill.CoaCredit_Id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == adminBill.CoaCredit_Id &&

                                      x.debit == _item.DebitedAmount && x.companyId == adminBill.company_Id && x.deptId == adminBill.dept_Id
                                    );
                                if (dbTrans == null)
                                {
                                    JournalTransaction BillTransaction = new JournalTransaction()
                                    {
                                        accountId = adminBill.CoaCredit_Id,
                                        coaTransactionsType = coaTransactionsType.Payment,
                                        creationDate = payment.GLPostingDate,
                                        debit = _item.DebitedAmount,
                                        credit = 0,
                                        MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                        userId = payment.user_Id,
                                        PaymentId = payment.Id,
                                        transactionRefno = txtPaymentRef.Text,
                                        total = _item.DebitedAmount - 0,
                                        deptId = adminBill.dept_Id,
                                        companyId = adminBill.company_Id,
                                        currencyId = adminBill.currency_Id,
                                    };
                                    journalTransactions.Add(BillTransaction);
                                }
                                else
                                {
                                    if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        JournalTransaction BillTransaction = new JournalTransaction()
                                        {
                                            accountId = adminBill.CoaCredit_Id,
                                            coaTransactionsType = coaTransactionsType.Payment,
                                            creationDate = payment.GLPostingDate,
                                            debit = _item.DebitedAmount,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                            //MER = 1,
                                            userId = payment.user_Id,
                                            PaymentId = payment.Id,
                                            transactionRefno = txtPaymentRef.Text,
                                            total = _item.DebitedAmount - 0,
                                            deptId = adminBill.dept_Id,
                                            companyId = adminBill.company_Id,
                                            currencyId = adminBill.currency_Id
                                            //deptId = payment.de.Id
                                        };
                                        journalTransactions.Add(BillTransaction);
                                    }
                                    else
                                    {
                                        dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == adminBill.CoaCredit_Id &&

                                          x.debit == _item.DebitedAmount && x.companyId == adminBill.company_Id && x.deptId == adminBill.dept_Id
                                        );
                                        JournalTransaction BillTransaction = new JournalTransaction()
                                        {
                                            accountId = adminBill.CoaCredit_Id,
                                            coaTransactionsType = coaTransactionsType.Payment,
                                            creationDate = payment.GLPostingDate,
                                            debit = _item.DebitedAmount,
                                            credit = 0,
                                            MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                            //MER = 1,
                                            userId = payment.user_Id,
                                            PaymentId = payment.Id,
                                            transactionRefno = txtPaymentRef.Text,
                                            total = _item.DebitedAmount - 0,
                                            deptId = adminBill.dept_Id,
                                            companyId = adminBill.company_Id,
                                            currencyId = adminBill.currency_Id,
                                            reconcilationDate = dbTrans.reconcilationDate,
                                            reconcilationType = dbTrans.reconcilationType,
                                            ReconcilationId = dbTrans.ReconcilationId,
                                            isReconciled = dbTrans.isReconciled
                                            //deptId = payment.de.Id
                                        };
                                        journalTransactions.Add(BillTransaction);
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
                                bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                bankTransaction.userId = payment.user_Id;
                                bankTransaction.PaymentId = payment.Id;
                                bankTransaction.transactionRefno = txtPaymentRef.Text;
                                bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                bankTransaction.deptId = adminBill.dept_Id;
                                bankTransaction.companyId = adminBill.company_Id;
                                bankTransaction.currencyId = adminBill.currency_Id;

                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            deducTransaction.accountId = dbDeduction.chartofAccountId;
                                            deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducTransaction.creationDate = payment.GLPostingDate;
                                            deducTransaction.debit = deduction.Amount;
                                            deducTransaction.credit = 0;
                                            deducTransaction.userId = payment.user_Id;
                                            deducTransaction.MER = 1;
                                            deducTransaction.PaymentId = payment.Id;
                                            deducTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducTransaction.deptId = adminBill.dept_Id;
                                            deducTransaction.total = deduction.Amount - 0;
                                            deducTransaction.companyId = adminBill.company_Id;
                                            deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducTransaction);
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction deducBankTransaction = new JournalTransaction();
                                        deducBankTransaction.accountId = payment.coaAccountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                        deducBankTransaction.creationDate = payment.GLPostingDate;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = deduction.Amount;
                                        deducBankTransaction.userId = payment.user_Id;
                                        deducBankTransaction.MER = 1;
                                        deducBankTransaction.PaymentId = payment.Id;
                                        deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                        deducBankTransaction.deptId = adminBill.dept_Id;
                                        deducBankTransaction.total = 0 - deduction.Amount;
                                        deducBankTransaction.companyId = adminBill.company_Id;
                                        deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        journalTransactions.Add(deducBankTransaction);
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
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
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                        bankTransaction.userId = payment.user_Id;
                                        bankTransaction.PaymentId = payment.Id;
                                        bankTransaction.transactionRefno = txtPaymentRef.Text;
                                        bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                        bankTransaction.deptId = adminBill.dept_Id;
                                        bankTransaction.companyId = adminBill.company_Id;
                                        bankTransaction.currencyId = adminBill.currency_Id;
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }
                                }
                            }
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            deducTransaction.accountId = dbDeduction.chartofAccountId;
                                            deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducTransaction.creationDate = payment.GLPostingDate;
                                            deducTransaction.debit = deduction.Amount;
                                            deducTransaction.credit = 0;
                                            deducTransaction.userId = payment.user_Id;
                                            deducTransaction.MER = 1;
                                            deducTransaction.PaymentId = payment.Id;
                                            deducTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducTransaction.deptId = adminBill.dept_Id;
                                            deducTransaction.total = deduction.Amount - 0;
                                            deducTransaction.companyId = adminBill.company_Id;
                                            deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducTransaction);
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction deducBankTransaction = new JournalTransaction();
                                        deducBankTransaction.accountId = account.COA_accountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                        deducBankTransaction.creationDate = payment.GLPostingDate;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = deduction.Amount;
                                        deducBankTransaction.userId = payment.user_Id;
                                        deducBankTransaction.MER = 1;
                                        deducBankTransaction.PaymentId = payment.Id;
                                        deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                        deducBankTransaction.deptId = adminBill.dept_Id;
                                        deducBankTransaction.total = 0 - deduction.Amount;
                                        deducBankTransaction.companyId = adminBill.company_Id;
                                        deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        journalTransactions.Add(deducBankTransaction);
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (payment.AdminBill_Id != null && payment.isVoid != true)
                    {

                        AdminBillsRepo adminBillRepo = new AdminBillsRepo();


                        if (adminBill.CoaCredit_Id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction BillTransaction = new JournalTransaction()
                                {
                                    accountId = adminBill.CoaCredit_Id,
                                    coaTransactionsType = coaTransactionsType.Payment,
                                    creationDate = payment.GLPostingDate,
                                    debit = _item.DebitedAmount,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                    //MER = 1,
                                    userId = payment.user_Id,
                                    PaymentId = payment.Id,
                                    transactionRefno = txtPaymentRef.Text,
                                    total = _item.DebitedAmount - 0,
                                    deptId = adminBill.dept_Id,
                                    companyId = adminBill.company_Id,
                                    currencyId = adminBill.currency_Id
                                    //deptId = payment.de.Id
                                };
                                journalTransactions.Add(BillTransaction);
                            }
                        }
                    }
                    payment.journalTransactions = journalTransactions;
                }
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
                paymentRepo.UpdatePayments(paymentList);

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

                            winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
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



            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        public ucFrmPayments(PaymentStatus paymentStatus)
        {
            statusChanged = paymentStatus;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadGridData();
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
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
                        else
                        if (str.Contains("Admin_Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Admin_Bill);
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

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
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

        private void TableViewPayment_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

            var row = e.Row as PaymentModelView;
            var debt = row.DebitedAmount;
            var ded = row.Deductions + row.VAT;

            var pymnt = payments.FirstOrDefault(x => x.Id == row.id);

            if (editFlag == true && pymnt != null)
            {

                var adjustmentAmount = pymnt.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                var amountPaid = pymnt.adminBill.Payments.Where(x => x.isVoid != true && x.Id != pymnt.Id).Sum(y => y.DebitedAmount) + adjustmentAmount;
                amountPaid = Math.Round(amountPaid + debt, 2);

                if (pymnt.adminBill.AmountWithTax != 0)
                {
                    if (Math.Abs(amountPaid) > Math.Abs(pymnt.adminBill.AmountWithTax))
                    {
                        DXMessageBox.Show("Payment cannot exceed Bill Amount!");
                        row.DebitedAmount = pymnt.DebitedAmount;

                        row.Total = pymnt.DebitedAmount + ded;
                        return;
                    }
                }
                else if (Math.Abs(amountPaid) > Math.Abs(pymnt.adminBill.AmountOC))
                {
                    DXMessageBox.Show("Payment cannot exceed Bill Amount!");
                    row.DebitedAmount = pymnt.DebitedAmount;

                    row.Total = pymnt.DebitedAmount + ded;
                    return;
                }
            }


            if (Math.Round(row.DebitedAmount, 2) > Math.Round(row.AmountDue, 2) && editFlag == false)
            {
                DXMessageBox.Show("Debited amount cannot exceed Amount Due!");
                ((DataViewBase)sender).Background = Brushes.LightBlue;
                row.DebitedAmount = 0;

                //row.Deductions = 0;
                return;
            }

            var b = (PaymentModelView)grdCntrlPayment.GetRow(e.RowHandle);
            b.Total = debt + ded;

            foreach (var _item in grdCntrlPayment.ItemsSource as List<PaymentModelView>)
            {
                var amount = _item.Total;
            }
        }

        private void CmbxPaymentType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxPaymentType.SelectedIndex == 0)
            {
                gridVendor.Visibility = Visibility.Visible;
                gridCrediCard.Visibility = Visibility.Collapsed;
            }
            else if (cmbxPaymentType.SelectedIndex == 1)
            {
                gridVendor.Visibility = Visibility.Collapsed;
                gridCrediCard.Visibility = Visibility.Visible;
            }
        }

        private void ChkCreditCard_Checked(object sender, RoutedEventArgs e)
        {
            cmbPrimaryCardNo.IsEnabled = true;
        }

        private void ChkCreditCard_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbPrimaryCardNo.IsEnabled = false;
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

        private void CmbPrimaryCardNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LookupCreditCardBank_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment1.Focus();
                return;
            }
        }

        private void CmbPrimaryCardNo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCreditCardBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                lookupCreditCardBank.Focus();
                return;
            }
        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {

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

            //var department = lookupDepartment.SelectedItem as Department;
            //var vendors = department.Vendors.ToList();

            lookUpVendor.ItemsSource = vendors;
            lookupSelectVendor.ItemsSource = vendors;
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
                    ucFrmDirectClose.type = "AdminBill";
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

                                winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Payments);
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
                                Subject = "Status Changed",
                                TaggedList = tagUsers,
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
                                Subject = "Status Changed",
                                TaggedList = tagUsers,
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

        private void LookupCreditCardBank_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var cmpnyId = (lookupCompany.SelectedItem as Company).Id;
            var bankId = (lookupCreditCardBank.SelectedItem as Bank).Id;

            CreditCardRepo cardRepo = new CreditCardRepo();
            var cardList = cardRepo.GetAllPrimaryCardsByCompanyBank(cmpnyId, bankId);

            List<int> deptIds = new List<int>();
            foreach (var _dept in deptList)
            {
                deptIds.Add(_dept.Id);
            }

            List<CreditCard> creditCards = new List<CreditCard>();
            foreach (var _card in cardList)
            {
                foreach (var _dept in _card.departments)
                {
                    if (deptIds.Contains(_dept.Id))
                    {
                        creditCards.Add(_card);
                        break;
                    }
                }
            }

            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (CreditCard _card in creditCards)
            {
                cmbitems.Add(new cmbitem() { name = _card.CardNumber, id = _card.Id });
            }

            cmbPrimaryCardNo.ItemsSource = cmbitems;
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlPayment.SelectedItem != null)
            {
                var idd = (grdCntrlPayment.SelectedItem as PaymentModelView).id;
                var groupIdd = (grdCntrlPayment.SelectedItem as PaymentModelView).GroupId;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, groupIdd, TransactionItemType.Payments);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Select any Payment first!");
            }
        }
        private void MbtnAddDeductions_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if (grdCntrlPayment.SelectedItem != null)
                {
                    var payment = grdCntrlPayment.SelectedItem as PaymentModelView;
                    ucPaymentDeductions ucPaymentDeductions = new ucPaymentDeductions(payment.id);
                    ucPaymentDeductions.ShowDialog();
                    payment.paymentDeductions = ucPaymentDeductions.finalDeductions;
                    payment.Deductions = ucPaymentDeductions.totalDeduction;
                    grdCntrlPayment.SetFocusedRowCellValue("Deductions", ucPaymentDeductions.totalDeduction);

                    payment.paymentTaxes = ucPaymentDeductions.finalTaxes;
                    payment.VAT = ucPaymentDeductions.totalVAT;
                    payment.IsAdjusted = ucPaymentDeductions.IsAdjusted;
                    grdCntrlPayment.SetFocusedRowCellValue("VAT", ucPaymentDeductions.totalVAT);

                }
                else
                    DXMessageBox.Show("Please Select payment to add deduction.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                if (payments[0].transactionGroupId > 0)
                {
                    if (deptList != null && deptList.Count > 0)
                    {
                        List<User> usersList = new List<User>();
                        foreach (var _dept in deptList)
                        {
                            usersList.AddRange(UsersRepo.getusersByDepartment(_dept.Id));

                        }
                        var userss = usersList.Distinct().ToList();

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, comment, TransactionItemType.Payments);
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
                //payments = paymentRepo.GetAdminBillPaymentsByGroupId(payments[0].transactionGroupId);
                LoadPaymentData();
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
                        PaymentModelView pyment = (grdCntrlPayment.SelectedItem as PaymentModelView);
                        SaleOrder saleOrder = new SaleOrder();
                        if (pyment.BillId != 0)
                        {
                            AdminBillsRepo billRepo = new AdminBillsRepo();
                            AdminBill adminbill = billRepo.get((int)pyment.BillId);
                            if (adminbill.Id != 0)
                            {
                                var _attByGroupId = SYSTEM_STATIC.GetPaymentAttachmentsListByCategory(adminbill.transactionGroupId, TransactionItemType.Admin_Bill);
                                var _attById = SYSTEM_STATIC.GetPaymentAttachmentsListByCategory(adminbill.Id, TransactionItemType.Admin_Bill);

                                if (_attByGroupId.FirstOrDefault(x => x.Items.Count > 0) != null)
                                    otherAttachments.AddRange(_attByGroupId);
                                else
                                    otherAttachments.AddRange(_attById);

                                foreach (var cat in atachments)
                                {
                                    foreach (var otherCat in otherAttachments)
                                    {
                                        if (otherCat.name == cat.name)
                                        {
                                            foreach (var file in otherCat.Items)
                                            {
                                                if (cat.Items.FirstOrDefault(x => x.id == file.id) == null)
                                                    cat.Items.Add(file);
                                            }
                                        }
                                    }
                                }
                                treeViewAttachments1.ItemsSource = atachments;
                            }
                        }
                    }
                    grdAttachments1.Visibility = Visibility.Visible;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    //Mouse.OverrideCursor = Cursors.Arrow;
                });
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
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

                            btnAttachment1.Content = "Uploading File . . .";
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

        private void LookupCOA_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void IsBypassCOA_Checked(object sender, RoutedEventArgs e)
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            var userChartofAccounts = chartofAccountsRepo.GetAllforPayment(lookupCompany.SelectedItem as Company, deptList, SYSTEM_STATIC.currentUser.id);
            lookupCOA.ItemsSource = userChartofAccounts;
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
            List<JournalTransaction> finalJournalTransactions = new List<JournalTransaction>();

            var tempPayment = Convert.ToDouble(grdCntrlPayment.Columns["DebitedAmount"].TotalSummaries[0].Value);


            List<PaymentModelView> selectedItems = new List<PaymentModelView>();

            foreach (var _item in grdCntrlPayment.VisibleItems)
            {
                if ((_item as PaymentModelView).Total != 0)
                    selectedItems.Add((PaymentModelView)_item);
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
                AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                var adminBill = adminBillsRepo.get((int)payment.AdminBill_Id);
                var account = lookupAccounts.SelectedItem as Account;
                payment.transactionType = PaymentTransactionType.Admin_Bills;
                payment.paymentAdminBillTemplate = (PaymentAdminBillTemplate)cmbxPaymentType.SelectedIndex;
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
                payment.PaymentAmount = Convert.ToDouble(txtPaymentAmount.Text);
                payment.bankId = (lookupBanks.SelectedItem as Bank).Id;
                payment.accountId = (lookupAccounts.SelectedItem as Account).Id;
                payment.AdminBill_Id = _item.BillId;
                payment.BillFinanceRefNo = _item.BillFinanceRefNo;
                payment.BillAmount = _item.OriginalAmount;
                payment.DebitedAmount = _item.DebitedAmount;
                payment.Deductions = _item.Deductions;

                if (_item.paymentDeductions != null)
                    if (_item.paymentDeductions.Count != 0)
                    {

                        payment.paymentDeductions = _item.paymentDeductions;
                    }
                payment.Deductions = _item.Deductions;
                if (editFlag == true)
                {

                    if (isBypassCOA.IsChecked == true)
                    {
                        payment.isBypassBank = true;
                        payment.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                        var dbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == Convert.ToDouble(txtPaymentAmount.Text));
                        JournalTransaction bankTransaction = new JournalTransaction();
                        if (payment.coaAccountId != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                if (dbTrans == null)
                                {
                                    bankTransaction.accountId = payment.coaAccountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                    bankTransaction.creationDate = payment.GLPostingDate;
                                    bankTransaction.debit = 0;
                                    bankTransaction.credit = Convert.ToDouble(txtPaymentAmount.Text);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                    bankTransaction.userId = payment.user_Id;
                                    bankTransaction.PaymentId = payment.Id;
                                    bankTransaction.transactionRefno = txtPaymentRef.Text;
                                    bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                    bankTransaction.deptId = adminBill.dept_Id;
                                    bankTransaction.companyId = adminBill.company_Id;
                                    bankTransaction.currencyId = adminBill.currency_Id;
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
                                    bankTransaction.currencyId = dbTrans.currencyId;
                                    bankTransaction.isReconciled = dbTrans.isReconciled;
                                    bankTransaction.reconcilationDate = dbTrans.reconcilationDate;
                                }
                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == deduction.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbDeduction.chartofAccountId && x.debit == deduction.Amount);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbDeduction.chartofAccountId;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = deduction.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = deduction.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = payment.coaAccountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = deduction.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - deduction.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = creditDbTrans.accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                        }
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
                                            bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                            bankTransaction.userId = payment.user_Id;
                                            bankTransaction.PaymentId = payment.Id;
                                            bankTransaction.transactionRefno = txtPaymentRef.Text;
                                            bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                            bankTransaction.deptId = adminBill.dept_Id;
                                            bankTransaction.companyId = adminBill.company_Id;
                                            bankTransaction.currencyId = adminBill.currency_Id;
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
                                            bankTransaction.currencyId = dbTrans.currencyId;
                                            bankTransaction.isReconciled = dbTrans.isReconciled;
                                            bankTransaction.reconcilationDate = dbTrans.reconcilationDate;
                                        }
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }

                                }
                            }
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == account.COA_accountId && x.credit == deduction.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbDeduction.chartofAccountId && x.debit == deduction.Amount);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbDeduction.chartofAccountId;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = deduction.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = deduction.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = deduction.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - deduction.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = creditDbTrans.accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (payment.AdminBill_Id != null && payment.isVoid != true)
                    {

                        AdminBillsRepo adminBillRepo = new AdminBillsRepo();


                        if (adminBill.CoaCredit_Id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction BillTransaction = new JournalTransaction()
                                {
                                    accountId = adminBill.CoaCredit_Id,
                                    coaTransactionsType = coaTransactionsType.Payment,
                                    creationDate = payment.GLPostingDate,
                                    debit = _item.DebitedAmount,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                    //MER = 1,
                                    userId = payment.user_Id,
                                    PaymentId = payment.Id,
                                    transactionRefno = txtPaymentRef.Text,
                                    total = _item.DebitedAmount - 0,
                                    deptId = adminBill.dept_Id,
                                    companyId = adminBill.company_Id,
                                    currencyId = adminBill.currency_Id
                                    //deptId = payment.de.Id
                                };
                                journalTransactions.Add(BillTransaction);
                            }
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
                                bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                bankTransaction.userId = payment.user_Id;
                                bankTransaction.PaymentId = payment.Id;
                                bankTransaction.transactionRefno = txtPaymentRef.Text;
                                bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                bankTransaction.deptId = adminBill.dept_Id;
                                bankTransaction.companyId = adminBill.company_Id;
                                bankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;

                            }
                            journalTransactions.Add(bankTransaction);
                            banktransactionFlag = 1;
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            deducTransaction.accountId = dbDeduction.chartofAccountId;
                                            deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducTransaction.creationDate = payment.GLPostingDate;
                                            deducTransaction.debit = deduction.Amount;
                                            deducTransaction.credit = 0;
                                            deducTransaction.userId = payment.user_Id;
                                            deducTransaction.MER = 1;
                                            deducTransaction.PaymentId = payment.Id;
                                            deducTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducTransaction.deptId = adminBill.dept_Id;
                                            deducTransaction.total = deduction.Amount - 0;
                                            deducTransaction.companyId = adminBill.company_Id;
                                            deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducTransaction);
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction deducBankTransaction = new JournalTransaction();
                                        deducBankTransaction.accountId = payment.coaAccountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                        deducBankTransaction.creationDate = payment.GLPostingDate;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = deduction.Amount;
                                        deducBankTransaction.userId = payment.user_Id;
                                        deducBankTransaction.MER = 1;
                                        deducBankTransaction.PaymentId = payment.Id;
                                        deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                        deducBankTransaction.deptId = adminBill.dept_Id;
                                        deducBankTransaction.total = 0 - deduction.Amount;
                                        deducBankTransaction.companyId = adminBill.company_Id;
                                        deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        journalTransactions.Add(deducBankTransaction);
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
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
                                        bankTransaction.MER = Math.Round(Convert.ToDouble(adminBill.MER), 2);
                                        bankTransaction.userId = payment.user_Id;
                                        bankTransaction.PaymentId = payment.Id;
                                        bankTransaction.transactionRefno = txtPaymentRef.Text;
                                        bankTransaction.total = 0 - Convert.ToDouble(txtPaymentAmount.Text);
                                        bankTransaction.deptId = adminBill.dept_Id;
                                        bankTransaction.companyId = adminBill.company_Id;
                                        bankTransaction.currencyId = adminBill.currency_Id;
                                        journalTransactions.Add(bankTransaction);
                                        banktransactionFlag = 1;
                                    }
                                }
                            }
                        }
                        if (adminBill != null)
                        {

                            if (_item.paymentDeductions != null && _item.paymentDeductions.Count != 0)
                            {

                                foreach (var deduction in _item.paymentDeductions)
                                {
                                    var dbDeduction = paymentRepo.GetDeduction((int)deduction.deduction_id);
                                    if (dbDeduction.chartofAccountId != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            deducTransaction.accountId = dbDeduction.chartofAccountId;
                                            deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducTransaction.creationDate = payment.GLPostingDate;
                                            deducTransaction.debit = deduction.Amount;
                                            deducTransaction.credit = 0;
                                            deducTransaction.userId = payment.user_Id;
                                            deducTransaction.MER = 1;
                                            deducTransaction.PaymentId = payment.Id;
                                            deducTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducTransaction.deptId = adminBill.dept_Id;
                                            deducTransaction.total = deduction.Amount - 0;
                                            deducTransaction.companyId = adminBill.company_Id;
                                            deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducTransaction);
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        JournalTransaction deducBankTransaction = new JournalTransaction();
                                        deducBankTransaction.accountId = account.COA_accountId;
                                        deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                        deducBankTransaction.creationDate = payment.GLPostingDate;
                                        deducBankTransaction.debit = 0;
                                        deducBankTransaction.credit = deduction.Amount;
                                        deducBankTransaction.userId = payment.user_Id;
                                        deducBankTransaction.MER = 1;
                                        deducBankTransaction.PaymentId = payment.Id;
                                        deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                        deducBankTransaction.deptId = adminBill.dept_Id;
                                        deducBankTransaction.total = 0 - deduction.Amount;
                                        deducBankTransaction.companyId = adminBill.company_Id;
                                        deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                        journalTransactions.Add(deducBankTransaction);
                                    }
                                }
                            }
                            if (_item.paymentTaxes != null && _item.paymentTaxes.Count != 0)
                            {

                                foreach (var tax in _item.paymentTaxes)
                                {
                                    var dbTax = taxRepo.getTaxName((int)tax.taxNameId);
                                    var creditDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == payment.coaAccountId && x.credit == tax.Amount);
                                    var debitDbTrans = payment.journalTransactions.FirstOrDefault(x => x.accountId == dbTax.COA_Id && x.debit == tax.Amount);
                                    if (dbTax != null)
                                    {
                                        JournalTransaction deducTransaction = new JournalTransaction();
                                        if (btnPushDebits.IsChecked == true)
                                        {
                                            if (debitDbTrans == null)
                                            {
                                                deducTransaction.accountId = dbTax.COA_Id;
                                                deducTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                                deducTransaction.creationDate = payment.GLPostingDate;
                                                deducTransaction.debit = tax.Amount;
                                                deducTransaction.credit = 0;
                                                deducTransaction.userId = payment.user_Id;
                                                deducTransaction.MER = 1;
                                                deducTransaction.PaymentId = payment.Id;
                                                deducTransaction.transactionRefno = txtPaymentRef.Text;
                                                deducTransaction.deptId = adminBill.dept_Id;
                                                deducTransaction.total = tax.Amount - 0;
                                                deducTransaction.companyId = adminBill.company_Id;
                                                deducTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                                journalTransactions.Add(deducTransaction);
                                            }
                                            else
                                            {
                                                deducTransaction.accountId = debitDbTrans.accountId;
                                                deducTransaction.coaTransactionsType = debitDbTrans.coaTransactionsType;
                                                deducTransaction.creationDate = debitDbTrans.creationDate;
                                                deducTransaction.debit = debitDbTrans.debit;
                                                deducTransaction.credit = debitDbTrans.credit;
                                                deducTransaction.userId = debitDbTrans.userId;
                                                deducTransaction.MER = debitDbTrans.MER;
                                                deducTransaction.PaymentId = debitDbTrans.PaymentId;
                                                deducTransaction.transactionRefno = debitDbTrans.transactionRefno;
                                                deducTransaction.deptId = debitDbTrans.deptId;
                                                deducTransaction.total = debitDbTrans.total;
                                                deducTransaction.companyId = debitDbTrans.companyId;
                                                deducTransaction.currencyId = debitDbTrans.currencyId;
                                                deducTransaction.isReconciled = debitDbTrans.isReconciled;
                                                deducTransaction.reconcilationDate = debitDbTrans.reconcilationDate;
                                                journalTransactions.Add(deducTransaction);

                                            }
                                        }
                                    }
                                    if (btnPushCredits.IsChecked == true)
                                    {
                                        if (creditDbTrans == null)
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = coaTransactionsType.Payment;
                                            deducBankTransaction.creationDate = payment.GLPostingDate;
                                            deducBankTransaction.debit = 0;
                                            deducBankTransaction.credit = tax.Amount;
                                            deducBankTransaction.userId = payment.user_Id;
                                            deducBankTransaction.MER = 1;
                                            deducBankTransaction.PaymentId = payment.Id;
                                            deducBankTransaction.transactionRefno = txtPaymentRef.Text;
                                            deducBankTransaction.deptId = adminBill.dept_Id;
                                            deducBankTransaction.total = 0 - tax.Amount;
                                            deducBankTransaction.companyId = adminBill.company_Id;
                                            deducBankTransaction.currencyId = (lookUpCurrency.SelectedItem as Currency).Id;
                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                        else
                                        {
                                            JournalTransaction deducBankTransaction = new JournalTransaction();
                                            deducBankTransaction.accountId = account.COA_accountId;
                                            deducBankTransaction.coaTransactionsType = creditDbTrans.coaTransactionsType;
                                            deducBankTransaction.creationDate = creditDbTrans.creationDate;
                                            deducBankTransaction.debit = creditDbTrans.debit;
                                            deducBankTransaction.credit = creditDbTrans.credit;
                                            deducBankTransaction.userId = creditDbTrans.userId;
                                            deducBankTransaction.MER = creditDbTrans.MER;
                                            deducBankTransaction.PaymentId = creditDbTrans.PaymentId;
                                            deducBankTransaction.transactionRefno = creditDbTrans.transactionRefno;
                                            deducBankTransaction.deptId = creditDbTrans.deptId;
                                            deducBankTransaction.total = 0 - creditDbTrans.total;
                                            deducBankTransaction.companyId = creditDbTrans.companyId;
                                            deducBankTransaction.currencyId = creditDbTrans.currencyId;
                                            deducBankTransaction.isReconciled = creditDbTrans.isReconciled;
                                            deducBankTransaction.reconcilationDate = creditDbTrans.reconcilationDate;

                                            journalTransactions.Add(deducBankTransaction);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (payment.AdminBill_Id != null && payment.isVoid != true)
                    {

                        AdminBillsRepo adminBillRepo = new AdminBillsRepo();


                        if (adminBill.CoaCredit_Id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                JournalTransaction BillTransaction = new JournalTransaction()
                                {
                                    accountId = adminBill.CoaCredit_Id,
                                    coaTransactionsType = coaTransactionsType.Payment,
                                    creationDate = payment.GLPostingDate,
                                    debit = _item.DebitedAmount,
                                    credit = 0,
                                    MER = Math.Round(Convert.ToDouble(adminBill.MER), 2),
                                    //MER = 1,
                                    userId = payment.user_Id,
                                    PaymentId = payment.Id,
                                    transactionRefno = txtPaymentRef.Text,
                                    total = _item.DebitedAmount - 0,
                                    deptId = adminBill.dept_Id,
                                    companyId = adminBill.company_Id
                                    //deptId = payment.de.Id
                                };
                                journalTransactions.Add(BillTransaction);
                            }
                        }
                    }

                }
                finalJournalTransactions.AddRange(journalTransactions);
            }
            banktransactionFlag = 0;
            return finalJournalTransactions;
        }

        private void BtnCreateIBT_Click(object sender, RoutedEventArgs e)
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

        private void btnCreateJV_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.JV, 0, groupId);
                procurmentPanel.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;

            }
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

        private void btnCreateICBT_Click(object sender, RoutedEventArgs e)
        {
            ucBankTransferInterCompany uc = new ucBankTransferInterCompany(groupId, false);
            Window win = new Window();
            win.Content = uc;

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void btnCreateSTL_Click(object sender, RoutedEventArgs e)
        {
            winSTLAdd sTLAdd = new winSTLAdd(groupId, Convert.ToDouble(txtPaymentAmount.Text), lookUpCurrency.SelectedItem as Currency);
            winSTLAdd.editFlag = false;
            sTLAdd.Show();
            Window win = Window.GetWindow(this);
            win.Close();

        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (editFlag == true)
            {
                var source = grdCntrlPayment.ItemsSource as List<PaymentModelView>;
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }
        public void GellAllOrdersTracking()
        {
            if (grdCntrlPayment.SelectedItem != null)
            {
                var idd = (grdCntrlPayment.SelectedItem as PaymentModelView).id;
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
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt != null)
                {
                    if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Customer_Credits)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credit Receipts") != null)
                        {
                            ucFrmCustomerCreditReceipt frmReceipt = new ucFrmCustomerCreditReceipt();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmReceipt.receiptId = saleReceipt.Id;
                                    frmPiPaymentWindow.Content = frmReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
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
                                frmReceipt.editFlag = true;
                                frmReceipt.groupId = saleReceipt.transactionGroupId;
                                frmReceipt.receiptId = saleReceipt.Id;
                                frmPiPaymentWindow.Content = frmReceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Direct Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Direct Receipts!");
                        }
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Direct_Receipt)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                        {
                            if (saleReceipt.payment != null)
                            {
                                ucFrmDirectReceiptPayment frmLAreceipt = new ucFrmDirectReceiptPayment();
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
                                        frmPiPaymentWindow.Title = "Direct Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                            }
                            else
                            {
                                ucFrmDirectSaleReceipt frmReceipt = new ucFrmDirectSaleReceipt();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                Window frmPiPaymentWindow = new Window();
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                    {
                                        frmReceipt.editFlag = true;
                                        frmReceipt.groupId = saleReceipt.transactionGroupId;
                                        frmPiPaymentWindow.Content = frmReceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Direct Receipts";
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
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                            }


                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Direct Receipts!");
                        }
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Loans_Advances)
                    {

                        switch (saleReceipt.loansAdvance.advanceTemplate)
                        {
                            case LoansAdvanceTemplate.Loan:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                                {
                                    ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
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
                                break;
                            default:
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
                                break;
                        }

                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") == null)
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                                return;
                            }
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (saleReceipt.saleReceiptStatus.isActive == false && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null))
                                {
                                    DXMessageBox.Show("Permission required to View Closed Receipts!");
                                    return;
                                }
                            }
                        }

                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;

                        updateSaleReceiptObj.dateEditcreationDate.EditValue = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;

                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
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

                        if (saleReceipt.principal != null)
                        {
                            PrincipalRepo prinRepo = new PrincipalRepo();
                            var principal = prinRepo.get(saleReceipt.principal.Id);
                        }

                        updateSaleReceiptObj.enter_receipt_win.Show();
                    }
                }
            }
            catch
            {

            }

            //MessageBox.Show("Mission Successful!");
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



        private void btnPOTracking_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Tracking By PO") != null)
            {
                if (gridVendorProfileByPO.Visibility == Visibility.Collapsed)
                {
                    gridVendorProfileByPO.Visibility = Visibility.Visible;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorProfileByPO);

                }
                else
                {
                    gridVendorProfileByPO.Visibility = Visibility.Collapsed;

                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Vendor Tracking By PO");

            }

        }
        private void btnCustomerProfileBySo_Click(object sender, EventArgs e)
        {

        }
        private void btnVendorProfileByPO_Click(object sender, EventArgs e)
        {

        }

        private void grdVendorProfileByPO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdVendorProfileByPO.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (grdVendorProfileByPO.SelectedItem as PendingInvoice).Id);
                procurmentPanel.Show();
            }
        }

        private void lookupSelectVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadVendorProfile("Open");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";
        }
        private void cmbPoProfileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            //LoadVendorProfile();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void btnSavePOLayout_Click(object sender, EventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorProfileByPO);

        }

        private void btnOpenVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Open");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";
        }

        private void btnCloseVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Close");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Close)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Close)";
        }

        private void btnPendingForApprovalVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Pending for Approval");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Pending for Approval)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Pending for Approval)";
        }

        private void btnAllVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("All");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (All)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (All)";
        }

        public class PendingInvoice
        {
            public int Id { get; set; }
            public string refNo { get; set; }
            public string currencyName { get; set; }
            public double invoiceAmount { get; set; }
            public double uninvoiceAmount { get; set; }
            public double collectedAmount { get; set; }
            public double pendingCollectionAmount { get; set; }
            public double paidAmount { get; set; }
            public double unPaidAmount { get; set; }
            public double amountOC { get; set; }
            public double agingDays { get; set; }
            public string customer { get; set; }
            public string vendor { get; set; }
            public string company { get; set; }
            public string department { get; set; }
            public string stage { get; set; }
            public SaleOrderStatus saleInvoiceStatus { get; set; }
            public PurchaseOrderStatus PurchaseInvoiceStatus { get; set; }
        }

        private void cmbPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbPaymentStatus.SelectedItem!=null)
            {
                var selectedStatus= cmbPaymentStatus.SelectedItem as cmbitem;
                var status=paymentRepo.GetPaymentStatus(selectedStatus.id);
                if(status.isPaid==true)
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
