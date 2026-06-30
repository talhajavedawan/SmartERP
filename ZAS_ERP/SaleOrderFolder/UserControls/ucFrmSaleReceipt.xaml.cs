using DevExpress.Data;
using DevExpress.DataProcessing;
using DevExpress.PivotGrid.OLAP;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.Budget;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptSR;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.Windows;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmSaleReceipt.xaml
    /// </summary>
    public partial class ucFrmSaleReceipt : UserControl
    {
        Window win = new Window();
        Grid grid = new Grid();
        LayoutControl layoutControl = new LayoutControl();
        List<List<double>> deductionValues = new List<List<double>>();
        int countDeductions;
        double TotalAmount = 0;
        List<SaleReceipts> _changedItems = new List<SaleReceipts>();
        List<SaleReceipts> _visibleItems = new List<SaleReceipts>();
        List<Deduction> listDeductions = new List<Deduction>();
        int row = -1;
        bool deductionEditFlag = false, saveClickFlag = false;
        List<int> changedIdList = new List<int>();
        public int SreceiptId = 0;
        NotificationsRepo notificationsRepo = new NotificationsRepo();


        GridColumn column = new GridColumn();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        List<Department> deptList = new List<Department>();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        bool? companyChnaged = null;
        public int saveEditFlag = 0;
        public bool enterReceiptWindowFlag = false;
        public UcListWindow enter_receipt_win = new UcListWindow();
        public UcListWindow deductionWin = new UcListWindow();
        public int receiptId;
        public int invoiceNo = 0;
        public int groupId = 0;
        JournalEntryRepo journalTransactionRepo = new JournalEntryRepo();

        CustomerCompany customer = new CustomerCompany();
        SaleInvoice invoices = new SaleInvoice();
        Company company = new Company();
        Department department = new Department();
        Bank bank = new Bank();
        public SalesReceiptStatus selectedStatus = new SalesReceiptStatus();
        List<CustomerCompany> custCompList = new List<CustomerCompany>();

        List<Account> accntList = new List<Account>();
        List<Bank> bankList = new List<Bank>();
        List<Bank> allBanks = new List<Bank>();
        //List<Company> allCompanies = new List<Company>();
        List<Account> allAccounts = new List<Account>();
        List<SaleInvoice> allInvoices = new List<SaleInvoice>();
        List<CollectionMethod> allCollectionMethods = new List<CollectionMethod>();
        public SalesReceiptStatus oldStatus = new SalesReceiptStatus();

        public List<SalesReceipt> receiptsList = new List<SalesReceipt>();
        public SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();

        SalesReceiptRepo repo = new SalesReceiptRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        UsersRepo userRep = new UsersRepo();
        UsersRepo UsersRepo = new UsersRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        List<Department> loginUserDepts = new List<Department>();
        List<Company> loginUserCompanies = new List<Company>();
        SalesReceiptStatus checkStatus = new SalesReceiptStatus();
        ProductRepo productrepo = new ProductRepo();
        double collectionAmountChk = 0;

        TaxRepo taxRepo = new TaxRepo();


        string stage;
        bool? isApproved;
        bool? isReApproved;
        DateTime? approvalDate;
        DateTime? reApprovalDate;

        bool? PendingForClosing;
        DateTime? ClosingDate;
        int banktransactionFlag = 0;

        static SalesReceiptStatus statusChanged = new SalesReceiptStatus();
        List<cmbitem> receiptStatusLst = new List<cmbitem>();
        List<SalesReceiptStatus> allReceiptStatus = new List<SalesReceiptStatus>();
        public ucFrmSaleReceipt()
        {
            InitializeComponent();

            //grdCntrlSalesReceipt.Columns["TotalAmount"].Visible = false;
            //column.Visible = false;
            enter_receipt_win.Closing += EnterReceipt_Window_Closing;

            //Getting all Collection Methods
            allCollectionMethods = repo.GetAllCollectionMethods();
            cmbxCollectionMethod.ItemsSource = allCollectionMethods;

            //Setting current date in DateEdit
            dateEditcreationDate.DateTime = DateTime.Now;
            dateEditcreationDate.IsEnabled = false;

            //Getting all currencies
            var allCurrencies = currencyRepo.getAll();
            allCurrencies = allCurrencies.Where(x => x.isVoid != true).ToList();
            cmbxCurrency.ItemsSource = allCurrencies;

            //Getting all Receipt Types
            for (int i = 0; i <= (int)ERP_BL.Enums.ReceiptType.Sales_Department; i++)
            {
                cmbxReceiptType.Items.Add(((ERP_BL.Enums.ReceiptType)i).ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize_AllComboboxes();
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
            grdTrackingTree.ExpandAllNodes();
        }
        public void loademployees()
        {
            //ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            ////  employees = cont1.GetEmployees();
            //employees = department.employees;

            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (ERP_BL.Databases.Employee employee in employees)
            //{
            //    cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //cmbTransactionHolder.ItemsSource = cmbitems;

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

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlSalesReceipt.SelectedItem != null)
            {
                var idd = (grdCntrlSalesReceipt.SelectedItem as SaleReceipts).Id;
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

        public void loadReceiptStatuses()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipt Statuses") != null)
                allReceiptStatus = repo.GetAllSaleReceiptStatus();
            else
                allReceiptStatus = repo.GetAllOpenSaleReceiptStatus();
                allReceiptStatus = allReceiptStatus.Where(x => x.isDisable != true).ToList();

            if (allReceiptStatus != null)
            {
                foreach (var _status in allReceiptStatus)
                {
                    receiptStatusLst.Add(new cmbitem()
                    {
                        name = _status.Status,
                        id = _status.Id,
                        bcolor = _status.backcolor,
                        fcolor = "#FF000000"
                    });
                }

                cmbSaleReceiptStatus.ItemsSource = receiptStatusLst;
            }
        }
        public void loadReceiptStatuses(SalesReceiptStatus status)
        {
            allReceiptStatus.Add(status);
            if (allReceiptStatus != null)
            {
                foreach (var _status in allReceiptStatus)
                {
                    receiptStatusLst.Add(new cmbitem()
                    {
                        name = _status.Status,
                        id = _status.Id,
                        bcolor = _status.backcolor,
                        fcolor = "#FF000000"
                    });
                }

                cmbSaleReceiptStatus.ItemsSource = receiptStatusLst;
            }
        }

        private void Initialize_AllComboboxes()
        {
            listDeductions = repo.GetAllDeductions();
            countDeductions = listDeductions.Count;

            //Getting companies and departments of the Login User
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            var loginUser = userRep.getuser(currentUserId);
            //loginUserDepts = loginUser.employee.departments;
            empUser = loginUser.employee;
            loginUserCompanies = empUser.Companies;

            //Populating cmbxCompnay
            cmbxCompany.ItemsSource = loginUserCompanies;
            loadReceiptStatuses();
            if (saveEditFlag == 0 && invoiceNo != 0)
            {
                txtDedER.Text = "1";
                txtBankChargesER.Text = "1";
                invoiceRepo = new SaleInvoiceRepo();
                SaleInvoice _invoice = invoiceRepo.get(invoiceNo);

                //Select the Company linked with the Sale Rceipt has to be updated
                if (_invoice.company != null)
                {
                    cmbxCompany.Text = _invoice.company.CompanyName;
                }

                //Select Department
                //if (_invoice.department != null)
                //{
                //    lookupDepartment.Text = _invoice.department.DeptName;
                //}

                List<CustomerCompany> customers = new List<CustomerCompany>();

                string deptNames = "";
                if (_invoice.department != null)
                {
                    deptList.Add(_invoice.department);
                }

                if (deptList.Count > 0)
                {
                    foreach (var _dept in deptList)
                    {
                        deptNames = deptNames + " | " + _dept.DeptName; if (cmbxCompany.SelectedIndex > -1)
                            foreach (var _customer in _dept.customers)
                            {
                                if (!customers.Contains(_customer))
                                    customers.Add(_customer);
                            }
                        foreach (var _customer in _dept.disableCustomers)
                        {
                            if (!customers.Contains(_customer))
                                customers.Add(_customer);
                        }

                        allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (cmbxCompany.SelectedItem as Company).Id) != null));
                    }
                }
                loademployees();

                lookupDepartment1.EditValue = deptNames;

                cmbxCustomers.ItemsSource = customers;

                PrincipalRepo principalRepo = new PrincipalRepo();
                var principalList = principalRepo.getAllByMultiDept(deptList);
                lookupPrincipal.ItemsSource = principalList;


                //Select Currency
                if (_invoice.currency != null)
                {
                    cmbxCurrency.Text = _invoice.currency.CurrencyName;
                }

                //Select Customer
                if (_invoice.customerCompany != null)
                {
                    cmbxCustomers.Text = _invoice.customerCompany.company.CompanyName;
                }



                List<SaleReceipts> invoices = new List<SaleReceipts>();
                SaleReceipts invoice = new SaleReceipts();

                invoice.Id = _invoice.Id;
                invoice.Date = _invoice.ApprovedDate;
                invoice.Customer = _invoice.customerCompany.company.CompanyName;
                invoice.SoNumber = _invoice.SaleOrderId;
                invoice.FinanceRefNo = _invoice.FinanceRefrenceNo;
                invoice.Currency = _invoice.currency.CurrencyName;

                double invoiceAmount = 0;
                if (_invoice.saleInvoicetype == InquiryType.DistributionBiz)
                    invoiceAmount =  Math.Round( _invoice.BookerStatementItems.Sum(x=>x.siNetAmount), 2);
                else
                    invoiceAmount = _invoice.totalInvoiceAmount;

                invoice.OriginalAmount = invoiceAmount;


                var reciepts = _invoice.salesReceipts;
                if (reciepts != null)
                {
                    var result = Math.Round( reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                    invoice.AmountDue = invoiceAmount - result;
                }


                invoice.InvoiceNo = _invoice.Id;

                invoices.Add(invoice);

                grdCntrlSalesReceipt.ItemsSource = invoices;
                btnLoad.IsEnabled = false;
                datglPostingdate.EditValue = System.DateTime.Now;

            }
            else if(saveEditFlag == 0 && invoiceNo == 0)
            {
                datglPostingdate.EditValue = System.DateTime.Now;
            }

            //Checking if the existing sale receipt is to be updated
            if (saveEditFlag == 1 && groupId > 0)
            {
                btnRefresh.IsEnabled = true;
                receiptsList = repo.getReceiptsByGroupId(groupId);

                if (receiptsList[0].isDeposit == true)
                    btnDeposit.IsChecked = true;
                else if (receiptsList[0].isDeposit == false)
                    btnPayment.IsChecked = true;

                if (receiptsList[0].saleReceiptStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleReceipt") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipt Statuses") == null)
                    {
                        cmbSaleReceiptStatus.IsEnabled = false;
                    }
                }
                else if (receiptsList[0].saleReceiptStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Receipt") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                }             

                if (receiptsList[0].isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Collection Amount value in SaleReceipt Under Approval") == null)
                {
                    txtCollectionAmnt.IsReadOnly = true;
                }
                else if(receiptsList[0].isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Collection Amount value in SaleReceipt After Approval") == null)
                {
                    txtCollectionAmnt.IsReadOnly = true;
                }
             
                allAccounts = repo.GetAllAccounts();

                loadcomments();
                loadonSaleInvoicedata();
                views = UsersRepo.getViwerInfo(groupId, 11);
                grdUsers.ItemsSource = views;

                btnLoad.IsEnabled = false;
                double collectedAmount = 0;

                if(receiptsList[0].isApproved == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Receipt") != null)
                    {
                        btnSave.IsEnabled = true;
                    }
                    else
                    {
                        btnSave.IsEnabled = false;
                    }
                }

                if (receiptsList != null && receiptsList.Count > 0)
                {
                    if (receiptsList[0].saleReceiptStatus != null)
                    {
                        checkStatus = receiptsList[0].saleReceiptStatus;
                    }

                    if (receiptsList[0].ReceiptRefNo != null)
                    {
                        lblReceiptRefNo.Text = " (" + receiptsList[0].ReceiptRefNo + ")";
                    }

                    if (receiptsList[0].isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                        txtVoid.RenderTransform = new RotateTransform(-45);
                        //lblStage.Text = "Void";
                    }
                    else if (receiptsList[0].isReApproved == false)
                    {
                        //lblStage.Text = "Under Re-Approval";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (receiptsList[0].isApproved == true && receiptsList[0].stage == "Closed")
                    {
                        //lblStage.Text = "Approved and Closed";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (receiptsList[0].isApproved == true && receiptsList[0].saleReceiptStatus.isActive == false && receiptsList[0].PendingForClosing != true)
                    {
                        //lblStage.Text = "Approved and Closed";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.DeepSkyBlue;
                    }
                    else if (receiptsList[0].isApproved == true && receiptsList[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";

                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (receiptsList[0].isApproved == true)
                    {
                        //lblStage.Text = "Approved";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (receiptsList[0].isApproved == false)
                    {
                        //lblStage.Text = "Under Approval";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.LightGray;
                        grdUnderClosing.Background = Brushes.LightGray;
                        grdClosed.Background = Brushes.LightGray;
                    }
                    else if (receiptsList[0].PendingForClosing == true)
                    {
                        //lblStage.Text = "Under Closing Approval";
                        grdUnderApproval.Background = Brushes.DeepSkyBlue;
                        grdApproved.Background = Brushes.DeepSkyBlue;
                        grdUnderClosing.Background = Brushes.DeepSkyBlue;
                        grdClosed.Background = Brushes.LightGray;
                    }
             

                }

                List<SaleReceipts> invoices = new List<SaleReceipts>();
                foreach (var _receipt in receiptsList)
                {
                    SaleReceipts recpt = new SaleReceipts();
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
                    recpt.Customer = _invoice.customerCompany.company.CompanyName;
                    recpt.SoNumber = _invoice.SaleOrderId;
                    recpt.FinanceRefNo = _invoice.FinanceRefrenceNo;
                    recpt.Currency = _invoice.currency.CurrencyName;

                    double invoiceAmount = 0;
                    if (_invoice.saleInvoicetype == InquiryType.DistributionBiz)
                        invoiceAmount = Math.Round(_invoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                    else
                        invoiceAmount = _invoice.totalInvoiceAmount;

                    recpt.OriginalAmount = invoiceAmount;

                    recpt.InvoiceStage = GetInvoiceStatus(_invoice);
                    var invoice1 = invoiceRepo.GetSaleInvoice(_invoice.Id);
                    var reciepts = invoice1.salesReceipts;
                    var result = Math.Round( reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                    recpt.AmountDue = invoiceAmount - result;
                    recpt.InvoiceNo = _invoice.Id;

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
                    if(_receipt.transactionHolderId!=null)
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


                txtCollectionAmnt.Text = collectedAmount.ToString();
                collectionAmountChk = double.Parse(txtCollectionAmnt.Text);

                var recDeductions = receiptsList.SelectMany(x=>x.receiptDeductions).ToList();
                var bankCharges = receiptsList.SelectMany(x=>x.bankCharges).ToList();
                var bankTaxes = receiptsList.SelectMany(x => x.ReceiptBankTaxes).ToList();
                var dedTaxes = receiptsList.SelectMany(x=>x.ReceiptDeductionTaxes).ToList();
                txtDeductionAmount.Text = Convert.ToDouble(recDeductions.Sum(x => x.Amount)).ToString();
                txtBankCharges.Text = Convert.ToDouble(bankCharges.Sum(x => x.Amount)).ToString();

                txtDedVAT.Text = dedTaxes.Sum(x => x.Amount).ToString();
                txtBankChargesVAT.Text = bankTaxes.Sum(x => x.Amount).ToString();
                if (receiptsList[0].DeductionExchangeRate == 0)
                    txtDedER.Text = 1.ToString();
                else
                    txtDedER.Text = receiptsList[0].DeductionExchangeRate.ToString();
                //txtDedSOC.Text = Convert.ToDouble(receiptsList.Sum(x => x.DeductionSOC)).ToString();
                //txtDedSOC.Text = Math.Round(recDeductions.Sum(x => x.Amount) * receiptsList[0].DeductionExchangeRate,2).ToString() ;
                //txtBankChargesSOC.Text = Math.Round(bankCharges.Sum(x => x.Amount) * receiptsList[0].DeductionExchangeRate, 2).ToString();

                //Select the Company linked with the Sale Rceipt has to be updated
                if (receiptsList[0].company != null)
                {
                    cmbxCompany.Text = receiptsList[0].company.CompanyName;
                }

                ////Populating Combobox Department 
                //if (receiptsList[0].department != null)
                //{
                //    lookupDepartment.Text = receiptsList[0].department.DeptName;
                //}

                List<CustomerCompany> customers = new List<CustomerCompany>();
                string deptNames = "";

                if (receiptsList[0].department != null)
                {
                    var departmentlist = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;

                    if (receiptsList[0].department != null && departmentlist.Find(x => x.Id == receiptsList[0].deptId) == null)
                    {
                        departmentlist.Add(receiptsList[0].department);
                        lookupDepartment1.ItemsSource = null;
                        lookupDepartment1.ItemsSource = departmentlist;
                    }
                    deptList.Add(receiptsList[0].department);
                }
                else if (receiptsList[0].Departments != null && receiptsList[0].Departments.Count > 0)
                {
                    deptList = receiptsList[0].Departments;
                }

                if (deptList.Count > 0)
                {
                    foreach (var _dept in deptList)
                    {
                        deptNames = deptNames + " | " + _dept.DeptName; if (cmbxCompany.SelectedIndex > -1)
                        foreach (var _customer in _dept.customers)
                            {
                                if (!customers.Contains(_customer))
                                    customers.Add(_customer);
                            }
                        foreach (var _customer in _dept.disableCustomers)
                        {
                            if (!customers.Contains(_customer))
                                customers.Add(_customer);
                        }
                        allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (cmbxCompany.SelectedItem as Company).Id) != null));
                    }
                }
                loademployees();

                lookupDepartment1.EditValue = deptNames;

                cmbxCustomers.ItemsSource = customers;

                PrincipalRepo principalRepo = new PrincipalRepo();
                var principalList = principalRepo.getAllByMultiDept(deptList);
                lookupPrincipal.ItemsSource = principalList;

                if (receiptsList[0].saleReceiptStatus != null)
                {
                    var disAbleStatus = allReceiptStatus.FirstOrDefault(x => x.Id == receiptsList[0].saleReceiptStatus.Id);
                    if (disAbleStatus == null)
                    {
                        loadReceiptStatuses(receiptsList[0].saleReceiptStatus);
                    }
                }
                if (receiptsList[0].saleReceiptStatus != null)
                {
                    int index = 0;
                    var statusList = (cmbSaleReceiptStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbSaleReceiptStatus.ItemsSource as List<cmbitem>;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == receiptsList[0].saleReceiptStatus.Id)
                        {
                            cmbSaleReceiptStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Payment Reference No
                var pettyCashRefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
                if (receiptsList[0].PettyCashRefId != null)
                {
                    int index = 0;
                    foreach (var _ref in pettyCashRefList)
                    {
                        if (_ref.id == receiptsList[0].PettyCashRefId)
                        {
                            cmbxPettyCashRef.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                if (receiptsList[0].VATBookRefId != 0 && receiptsList[0].VATBookRefNumber != null)
                {
                    var vatBookSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                    var term = vatBookSource.Find(x => x.id == receiptsList[0].VATBookRefId);

                    if (term == null)
                    {
                        vatBookSource.Add(new cmbitem() { name = receiptsList[0].VATBookRefNumber.VATBookReferenceNo, id = receiptsList[0].VATBookRefNumber.Id });
                        cmbxVATBookRef.ItemsSource = null;
                        cmbxVATBookRef.ItemsSource = vatBookSource;
                    }
                    cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == receiptsList[0].VATBookRefId))];
                }
                else
                    btnVATBookPost.IsChecked = false;

                if (receiptsList[0].receiptType == 0)
                {
                    cmbxReceiptType.SelectedIndex = 0;
                    grdCustomer.Visibility = Visibility.Visible;
                    grdCntrlSalesReceipt.Columns["Customer"].Visible = false;

                    int index = 0;
                    var custCompanies = (cmbxCustomers.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : cmbxCustomers.ItemsSource as List<CustomerCompany>;
                    foreach (var _cust in custCompanies)
                    {
                        if (_cust.Id == receiptsList[0].Customer.Id)
                        {
                            cmbxCustomers.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    cmbxReceiptType.SelectedIndex = 1;
                    if (receiptsList[0].principal != null)
                    {
                        lookupPrincipal.Text = receiptsList[0].principal.company.CompanyName;
                    }

                    grdPrincipal.Visibility = Visibility.Visible;

                    cmbxCustomers.Visibility = Visibility.Collapsed;
                    grdCustomer.Visibility = Visibility.Collapsed;
                    grdCntrlSalesReceipt.Columns["Customer"].Visible = true;
                }

                //Selecting the Collection Method linked with the Sale Rceipt has to be updated
                if (receiptsList[0].collectionMethod != null)
                {
                    cmbxCollectionMethod.Text = receiptsList[0].collectionMethod.MethodName;
                }

                //Selecting the Currency linked with the Sale Rceipt has to be updated
                if (receiptsList[0].Currency != null)
                {
                    cmbxCurrency.Text = receiptsList[0].Currency.CurrencyName;
                }

                //Populating Combobox Banks
                if (receiptsList[0].bank != null)
                {
                    cmbxBanks.Text = receiptsList[0].bank.BankName;
                }

                //Selecting the Account linked with the Sale Rceipt has to be updated
                if (receiptsList[0].account != null)
                {
                    //cmbxAccounts.Text = receiptsList[0].account.AccountNo;

                    var accntSource = (cmbxAccounts.ItemsSource as List<Account>) == null ? new List<Account>() : cmbxAccounts.ItemsSource as List<Account>;

                    if (receiptsList[0].account != null && accntSource.Find(x => x.Id == receiptsList[0].account.Id) == null)
                    {
                        accntSource.Add(receiptsList[0].account);
                        cmbxAccounts.ItemsSource = null;
                        cmbxAccounts.ItemsSource = accntSource;
                    }
                    cmbxAccounts.Text = receiptsList[0].account.AccountNo;
                }

            }
            if (saveEditFlag == 1)
            {
                if (receiptsList[0].isBypassBank == true && receiptsList[0].coaAccountId != null )
                {
                    isBypassCOA.IsChecked = true;
                    lookupCOA.Text = receiptsList[0].ChartofAccount.accountName;
                }
                if (receiptsList[0].transactionHolderId != null)
                {
                    try
                    {
                        var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                        cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == receiptsList[0].transactionHolderId))];

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            if (saveEditFlag == 0)
            {
                btnPushDebits.IsChecked = true;
                btnPushCredits.IsChecked = true;
            }
        }

        private void TblViewLandTypeLst_Loaded(object sender, RoutedEventArgs e)
        {
            //Making the Columns Editable when Loaded
            grdCntrlSalesReceipt.Columns["CreditedAmount"].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            //grdCntrlSalesReceipt.Columns["TotalAmount"].Visible = false;
            //column.Visible = false;
            grdCntrlSalesReceipt.Columns["Id"].Visible = false;
        }

        //Hiding the Customer Column on Selection of ReceiptType_Customer
        private void ComboBoxEdit_ReceiptType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //grdCntrlSalesReceipt.Columns["TotalAmount"].Visible = false;
            //column.Visible = false;

            if (cmbxReceiptType.SelectedIndex == 0)
            {
                grdPrincipal.Visibility = Visibility.Collapsed;

                grdCustomer.Visibility = Visibility.Visible;
                cmbxCustomers.Visibility = Visibility.Visible;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = false;
                cmbxCustomers.Visibility = Visibility.Visible;
            }
            else
            {
                grdPrincipal.Visibility = Visibility.Visible;

                grdCustomer.Visibility = Visibility.Collapsed;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = true;
                cmbxCustomers.Visibility = Visibility.Collapsed;
            }
        }

        //Gridcontrol Loaded function
        private void GrdCntrlSalesReceipt_Loaded(object sender, RoutedEventArgs e)
        {
            //Adding total column
            //column.FieldName = "Total";
            ////column.VisibleIndex = 11;
            ////column.Visible = false;

            //column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            //grdCntrlSalesReceipt.Columns.Add(column);

            //Best Fit.
            var tableView = sender as TableView;
            if (tableView != null)
            {
                foreach (var item in tableView.VisibleColumns)
                {
                    tableView.BestFitColumn(item);
                }
            }


            //-----------Calculations of the columns--------------

            //long creditedAmount;
            //var temp = grdCntrlSalesReceipt.Columns["Total"].TotalSummaries[0].Value;
            //creditedAmount = Int64.Parse(temp.ToString());
            //txtBlckAmntCollected.Text = temp.ToString();


            //temp = grdCntrlSalesReceipt.Columns["Discount"].TotalSummaries[0].Value;
            //creditedAmount = creditedAmount - Int64.Parse(temp.ToString());
            //txtBlckDicount.Text = "-" + temp.ToString();

            //temp = grdCntrlSalesReceipt.Columns["LD_Charges"].TotalSummaries[0].Value;
            //creditedAmount = creditedAmount - Int64.Parse(temp.ToString());
            //txtBlckLDchrgs.Text = "-" + temp.ToString();


            //temp = grdCntrlSalesReceipt.Columns["BankCharges"].TotalSummaries[0].Value;
            //creditedAmount = creditedAmount - Int64.Parse(temp.ToString());
            //txtBlckBankChrgs.Text = "-"+temp.ToString();

            //txtBlckCredit.Text = creditedAmount.ToString();
        }
        
        //Total Column calculations
        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            var row = grdCntrlSalesReceipt.GetRowByListIndex(e.ListSourceRowIndex) as SaleReceipts;
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
                //double cred_amount = Convert.ToDouble(e.GetListSourceFieldValue("CreditedAmount"));
                //double ded = Convert.ToDouble(e.GetListSourceFieldValue("Deductions"));

                ////Adding the Values of four columns to show in the Total column
                //e.Value = cred_amount + ded;

                //var a = e.ListSourceRowIndex;
                //var b = (SaleReceipts)grdCntrlSalesReceipt.GetRow(a);
                //var prod = cred_amount + ded;
                //b.TotalAmount = prod;
            }

            if (e.IsGetData && e.Column.FieldName == "VAT")
            {
                var rcpt = grdCntrlSalesReceipt.GetRowByListIndex(e.ListSourceRowIndex) as SaleReceipts;

                //Adding the Values of four columns to show in the Total column
                e.Value = rcpt.dedVAT + rcpt.bankVAT;
            }
            //column.Visible = false;
        }

        private void loadBillReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();

            
                references = BillsRepo.GetAllActiveBillReferenceNo((cmbxCompany.SelectedItem as Company).Id);


            if (saveEditFlag == 1 && receiptsList.Count > 0)
            {
                if (receiptsList[0].PettyCashRef != null && references.FirstOrDefault(x => x.Id == receiptsList[0].PettyCashRefId) == null)
                    references.Add(receiptsList[0].PettyCashRef);
            }


            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }

            cmbxPettyCashRef.ItemsSource = cmbitems;
        }



        //Save Button Click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Checking if any field is empty
                if (cmbxReceiptType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Receipt type!");
                    cmbxReceiptType.Focus();
                    return;
                }
                if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company!");
                    cmbxCompany.Focus();
                    return;
                }
                if (deptList == null || deptList.Count == 0)
                {
                    DXMessageBox.Show("Please select Department!");
                    lookupDepartment1.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtSystemRef.Text))
                {
                    DXMessageBox.Show("Please enter System reference number!");
                    txtSystemRef.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtReceiptRef.Text))
                {
                    DXMessageBox.Show("Please enter Receipt reference number!");
                    txtReceiptRef.Focus();
                    return;
                }
                if (cmbxCollectionMethod.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Collection method!");
                    cmbxCollectionMethod.Focus();
                    return;
                }
                if (cmbxBanks.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank!");
                    cmbxBanks.Focus();
                    return;
                }
                if (cmbxAccounts.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account!");
                    cmbxAccounts.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtCollectionAmnt.Text))
                {
                    DXMessageBox.Show("Please enter Collection amount!");
                    txtCollectionAmnt.Focus();
                    return;
                }
                if (cmbSaleReceiptStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Status!");
                    cmbSaleReceiptStatus.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtInstrumentNo.Text))
                {
                    DXMessageBox.Show("Please enter Instrument #!");
                    txtInstrumentNo.Focus();
                    return;
                }
                if (dateEditcreditedDate.EditValue == null)
                {
                    DXMessageBox.Show("Please enter Credited date!");
                    dateEditcreditedDate.Focus();
                    return;
                }
                if (datInstrumentDate.EditValue == null)
                {
                    DXMessageBox.Show("Please enter Instrument date!");
                    datInstrumentDate.Focus();
                    return;
                }
                if (datDepositedDate.EditValue == null)
                {
                    DXMessageBox.Show("Please enter Deposited date!");
                    datDepositedDate.Focus();
                    return;
                }
                if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
               
                    SalesReceipt saleReceipt = new SalesReceipt();


                    var tempDeduction = Convert.ToDouble(grdCntrlSalesReceipt.Columns["Deductions"].TotalSummaries[0].Value);
                    var tempVAT = Convert.ToDouble(grdCntrlSalesReceipt.Columns["VAT"].TotalSummaries[0].Value);
                    var tempCharges = Convert.ToDouble(grdCntrlSalesReceipt.Columns["BankCharges"].TotalSummaries[0].Value);
                    var totalDeduction = Math.Round(double.Parse((tempDeduction + tempVAT + tempCharges).ToString()), 2);
                   
                    var dedAmount = Math.Round( Convert.ToDouble(txtDedTotal.Text) + Convert.ToDouble(txtTotalBankCharges.Text), 2);
                    if (totalDeduction != dedAmount)
                    {
                        DXMessageBox.Show("Deduction, VAT and Bank Charges amount not matching with Total Deduction!");
                        return;
                    }

                    //Total column summary
                    var temp = Convert.ToDouble(grdCntrlSalesReceipt.Columns["Total"].TotalSummaries[0].Value);
                    var total = Math.Round(double.Parse(temp.ToString()), 2);

                    var selBank = cmbxBanks.SelectedItem as Bank;
                    var selAccnt = cmbxAccounts.SelectedItem as Account;

                    if (saveEditFlag != 0)
                    {
                        if (saveEditFlag == 1)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Receipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Receipt") != null)
                            {
                                _visibleItems = new List<SaleReceipts>();
                                foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
                                {
                                    _visibleItems.Add((SaleReceipts)_item);

                                }


                                //Checking if Collection Amount field is empty
                                if (string.IsNullOrEmpty(txtCollectionAmnt.Text) || string.IsNullOrWhiteSpace(txtCollectionAmnt.Text))
                                {
                                    DXMessageBox.Show("Please enter Collection amount!");
                                }
                                else
                                {

                                    if (total == double.Parse(txtCollectionAmnt.Text))
                                    {

                                        if (cmbxReceiptType.SelectedIndex == 0)
                                        {
                                            if (cmbxCustomers.SelectedIndex < 0)
                                            {
                                                DXMessageBox.Show("Please select Customer!");
                                            }
                                            else
                                            {
                                                var _vitem = _visibleItems[0];
                                                saleReceipt = new SalesReceipt();
                                                saleReceipt = repo.GetSalesReceipt(_vitem.Id);

                                                //------------------Shifted Part------------------------------
                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null && saleReceipt.isApproved != true)
                                                {
                                                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Receipt is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                    {
                                                        stage = TransactionStage.Approved.ToString();
                                                        isApproved = true;
                                                        approvalDate = System.DateTime.Now;
                                                    }
                                                }
                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null && saleReceipt.isReApproved == false)
                                                {
                                                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This SaleReceipt is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                    {
                                                        stage = TransactionStage.Approved.ToString();
                                                        isReApproved = true;
                                                        reApprovalDate = System.DateTime.Now;
                                                    }
                                                }
                                            //else if (collectionAmountChk != double.Parse(txtCollectionAmnt.Text) && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Collection Amount value in SaleReceipt") != null)

                                            //{
                                            //    stage = TransactionStage.AwaitingApproval.ToString();
                                            //    isReApproved = false;
                                            //}
                                            //Saving the Selected Customer from the List
                                            //var cmpny = custCompList[cmbxCustomers.SelectedIndex];

                                            List<SalesReceipt> finalReceipts = new List<SalesReceipt>();
                                                for (int i = 0; i < _visibleItems.Count; i++)
                                                {
                                                    var _item = _visibleItems[i];
                                                    saleReceipt = new SalesReceipt();
                                                    saleReceipt = repo.GetSalesReceipt(_item.Id);
                                                    saleReceipt.Id = _item.Id;

                                                    if (cmbTransactionHolder.SelectedIndex != -1)
                                                    {
                                                        saleReceipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                                                        saleReceipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                                                    }

                                                    saleReceipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                                                    saleReceipt.receiptType = ((ERP_BL.Enums.ReceiptType)cmbxReceiptType.SelectedIndex);
                                                    saleReceipt.CreationDate = dateEditcreationDate.DateTime;
                                                    saleReceipt.company = loginUserCompanies[cmbxCompany.SelectedIndex];
                                                    //saleReceipt.department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

                                                    if (deptList.Count != 0)
                                                    {
                                                        saleReceipt.Departments = new List<Department>();
                                                        foreach (Department _dept in deptList)
                                                        {
                                                            if (!saleReceipt.Departments.Contains(_dept))
                                                            {
                                                                saleReceipt.Departments.Add(_dept);
                                                            }
                                                        }
                                                    saleReceipt.department = null;
                                                    }

                                                    saleReceipt.Customer = cmbxCustomers.SelectedItem as CustomerCompany;
                                                    saleReceipt.SystemRefNo = txtSystemRef.Text;
                                                    saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                                                    saleReceipt.Currency = cmbxCurrency.SelectedItem as ERP_BL.Databases.Currency;
                                                    saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                                                    saleReceipt.collectionMethod = cmbxCollectionMethod.SelectedItem as ERP_BL.Databases.CollectionMethod;
                                                    saleReceipt.bank = selBank;
                                                    saleReceipt.BankId = selBank.Id;
                                                    saleReceipt.account = selAccnt;
                                                

                                                var saleInvoice = invoiceRepo.GetSaleInvoice(_item.InvoiceNo);
                                                saleReceipt.saleInvoice = saleInvoice;


                                                    saleReceipt.AccountId = selAccnt.Id;
                                                    saleReceipt.transactionGroupId = groupId;

                                                    saleReceipt.CreditedDate = dateEditcreditedDate.DateTime;
                                                    saleReceipt.DepositedDate = datDepositedDate.DateTime;
                                                    saleReceipt.InstrumentNo = txtInstrumentNo.Text;
                                                    saleReceipt.InstrumentDate = datInstrumentDate.DateTime;
                                                    saleReceipt.principal = lookupPrincipal.SelectedItem as Principal;

                                                    if(cmbxPettyCashRef.SelectedIndex > -1)
                                                        saleReceipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;

                                                    //Receipt Asset Status 
                                                    if ((cmbSaleReceiptStatus.SelectedItem as cmbitem) != null)
                                                    {
                                                        var status = repo.GetSaleReceiptStatus((cmbSaleReceiptStatus.SelectedItem as cmbitem).id);
                                                        if (status != null)
                                                        {
                                                            saleReceipt.saleReceiptStatus = status;
                                                        }
                                                    }

                                                    //Adding Sale receipt to the Database

                                                    //var tempList = deductionValues[i];
                                                    //saleReceipt.receiptDeductions.Clear();

                                                    //saleReceipt.receiptDeductions = _visibleItems[i].receiptDeductions;
                                                    saleReceipt.bankCharges = _visibleItems[i].bankChargess;

                                                    saleReceipt.DeductionExchangeRate = _visibleItems[i].ExchangeRate;
                                                    saleReceipt.DeductionSOC = _visibleItems[i].TotalDeductionSOC;

                                                    //for (int j = 0; j < tempList.Count; j++)
                                                    //{
                                                    //    ReceiptDeduction receiptDeduction = new ReceiptDeduction();
                                                    //    receiptDeduction.Amount = tempList[j];
                                                    //    receiptDeduction.deduction = listDeductions[j];
                                                    //    saleReceipt.receiptDeductions.Add(receiptDeduction);




                                                    //}

                                                    if (MainWindow.currentUserid == 0)
                                                    {

                                                    }
                                                    else if (saleReceipt.user_Id == null)
                                                        saleReceipt.user_Id = MainWindow.currentUserid;


                                                    if (stage != null)
                                                        saleReceipt.stage = stage;

                                                    if (isApproved != null)
                                                        saleReceipt.isApproved = isApproved;

                                                    if (isReApproved != null)
                                                        saleReceipt.isReApproved = isReApproved;

                                                    if (approvalDate != null)
                                                        saleReceipt.ApprovedDate = approvalDate;

                                                    if (reApprovalDate != null)
                                                        saleReceipt.ReApprovalDate = reApprovalDate;

                                                    if (checkStatus != null && checkStatus.Id != 0)
                                                    {
                                                        if (checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                                        {
                                                            saleReceipt.LastStatusChangeDate = System.DateTime.Now;
                                                            if (saleReceipt.saleReceiptStatus.isActive != true)
                                                            {
                                                                saleReceipt.ClosingDate = System.DateTime.Now;
                                                            }
                                                        }
                                                    }
                                                    saleReceipt.GLPostingDate = (DateTime)datglPostingdate.EditValue;

                                                    saleReceipt.receiptDeductions = _item.receiptDeductions;
                                                    saleReceipt.bankCharges = _item.bankChargess;
                                                    saleReceipt.IsAdjustedBankVAT = _item.IsBankAdjusted;
                                                    saleReceipt.IsAdjustedDedVAT = _item.IsDedAdjusted;
                                                    saleReceipt.ReceiptDeductionTaxes = _item.receiptDedTaxes;
                                                    saleReceipt.ReceiptBankTaxes = _item.receiptBankTaxes;

                                                    List<PettyCash> pettyCashes = new List<PettyCash>();
                                                    if (saveEditFlag == 1 && groupId != 0)
                                                    {
                                                        if (btnDeposit.IsChecked == true)
                                                        {
                                                            pettyCashes.Add(new PettyCash()
                                                            {
                                                                CreationDate = dateEditcreationDate.DateTime,
                                                                SaleReceiptId = saleReceipt.Id,
                                                                TransactionType = TransactionItemType.Sale_Receipt,
                                                                debit = Math.Round(_item.TotalAmount, 2),
                                                                credit = 0,
                                                                total = Math.Round(_item.TotalAmount, 2) - 0,
                                                                FinanceRefNo = txtReceiptRef.Text,
                                                                SystemRefNo = txtSystemRef.Text,
                                                                MER = 0/*Math.Round(Convert.ToDouble(payment.Bill.ExchangeRate), 2)*/,
                                                         
                                                                deptId = saleInvoice?.department?.Id,
                                                                companyId = saleReceipt.company.Id,
                                                                currencyId = saleReceipt.Currency.Id,
                                                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                            });
                                                            saleReceipt.isDeposit = true;
                                                        }
                                                        else if (btnPayment.IsChecked == true)
                                                        {
                                                            pettyCashes.Add(new PettyCash()
                                                            {
                                                                CreationDate = dateEditcreationDate.DateTime,
                                                                SaleReceiptId = saleReceipt.Id,
                                                                TransactionType = TransactionItemType.Sale_Receipt,
                                                                debit = 0,
                                                                credit = Math.Round(_item.TotalAmount, 2),
                                                                total = 0 - Math.Round(_item.TotalAmount, 2),
                                                                FinanceRefNo = txtReceiptRef.Text,
                                                                SystemRefNo = txtSystemRef.Text,
                                                                MER = 0 /*Math.Round(Convert.ToDouble(payment.Bill.ExchangeRate), 2)*/,
                                                             
                                                                deptId = saleInvoice?.department?.Id,
                                                                companyId = saleReceipt.company.Id,
                                                                currencyId = saleReceipt.Currency.Id,
                                                                PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                            });
                                                            saleReceipt.isDeposit = false;
                                                        }
                                                        else
                                                        {
                                                            saleReceipt.isDeposit = null;
                                                        }
                                                    }
                                                   
                                                    saleReceipt.pettyCashes = pettyCashes;
                                              


                                                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

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
                                                                    var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId== (lookupCOA.SelectedItem as ChartofAccount).Id);
                                                                    JournalTransaction bankTransaction = new JournalTransaction();
                                                                    if (dbtrans == null)
                                                                    {
                                                                        bankTransaction.accountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                                                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                        bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                        bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                        bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        bankTransaction.credit = 0;
                                                                        bankTransaction.userId = saleReceipt.user_Id;
                                                                        bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                        
                                                                        bankTransaction.deptId = saleInvoice?.department.Id;
                                                                        bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                        
                                                                    }
                                                                    else
                                                                    {
                                                                        var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                        saleReceipt.isBypassBank = false;
                                                        saleReceipt.coaAccountId = null;
                                                        if (banktransactionFlag == 0)
                                                        {
                                                            if (saleReceipt.account.COA_accountId != null)
                                                            {
                                                                if (btnPushDebits.IsChecked == true)
                                                                {
                                                                    var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId== (cmbxAccounts.SelectedItem as Account).COA_accountId);
                                                                    JournalTransaction bankTransaction = new JournalTransaction();
                                                                    var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                                     (cmbxAccounts.SelectedItem as Account).COA_accountId &&
                                                                     x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) &&
                                                                     x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                                     
                                                                     x.deptId == saleInvoice?.department?.Id
                                                                     );
                                                                    if (dbtrans != null && dbtrans.debit== Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value))
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

                                                                        if (dbTransaction != null && dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                                                        {
                                                                            bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                            bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            bankTransaction.credit = 0;
                                                                            bankTransaction.userId = saleReceipt.user_Id;
                                                                            bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                            
                                                                            bankTransaction.deptId = saleInvoice?.department.Id;
                                                                            bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                            bankTransaction.isReconciled = false;
                                                                            bankTransaction.reconcilationDate = null;
                                                                            bankTransaction.ReconcilationId = null;
                                                                            bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                       
                                                                            bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                            bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            bankTransaction.credit = 0;
                                                                            bankTransaction.userId = saleReceipt.user_Id;
                                                                            bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                            
                                                                            bankTransaction.deptId = saleInvoice?.department?.Id;
                                                                            bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                          

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
                                                            var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                                                            var debitDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId== deduction.chartofAccountId);
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
                                                                            deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            deductionTransaction.debit = receiptDeduction.Amount;
                                                                            deductionTransaction.credit = 0;
                                                                            deductionTransaction.userId = saleReceipt.user_Id;
                                                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            
                                                                            deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                            deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                            deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                         
                                                                        }
                                                                        else
                                                                        {
                                                                            var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                                //        deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                //        deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                                                //        deducBankTransaction.debit = 0;
                                                                //        deducBankTransaction.credit = receiptDeduction.Amount;
                                                                //        //MER = 1,
                                                                //        deducBankTransaction.userId = saleReceipt.user_Id;
                                                                //        deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                //        deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                //        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                //        deducBankTransaction.deptId = saleReceipt.department.Id;
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
                                                    if (saleReceipt.bankCharges.Count != 0)
                                                    {
                                                        foreach (var receiptDeduction in saleReceipt.bankCharges)
                                                        {
                                                            var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
                                                            var debitDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == receiptDeduction.Amount && x.accountId == deduction.chartofAccountId);

                                                            var creditDbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.credit == receiptDeduction.Amount && x.accountId== deduction.chartofAccountId);

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
                                                                            deductionTransaction.userId = saleReceipt.user_Id;
                                                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            
                                                                            deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                            deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                            deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                        }
                                                                        else
                                                                        {
                                                                            var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                                        deducBankTransaction.userId = saleReceipt.user_Id;
                                                                        deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        
                                                                        deducBankTransaction.deptId = saleInvoice?.department?.Id;
                                                                        deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                                                        deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                                    if (saleReceipt.ReceiptBankTaxes != null && saleReceipt.ReceiptBankTaxes.Count > 0)
                                                    {
                                                        foreach (var receiptBankTax in saleReceipt.ReceiptBankTaxes)
                                                        {
                                                            var vatTax= taxRepo.getTaxtById((int)receiptBankTax.taxNameId);
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
                                                                            deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            deductionTransaction.debit = receiptBankTax.Amount;
                                                                            deductionTransaction.credit = 0;
                                                                            deductionTransaction.userId = saleReceipt.user_Id;
                                                                            deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            
                                                                            deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                            deductionTransaction.total = receiptBankTax.Amount - 0;
                                                                            deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                        }
                                                                        else
                                                                        {
                                                                            var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                                        deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                                                        deducBankTransaction.debit = 0;
                                                                        deducBankTransaction.credit = receiptBankTax.Amount;
                                                                        //MER = 1,
                                                                        deducBankTransaction.userId = saleReceipt.user_Id;
                                                                        deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                       
                                                                        deducBankTransaction.deptId = saleInvoice?.department.Id;
                                                                        deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                                                        deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                                    //comment
                                               
                                                List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
                                                if (saveEditFlag==1 && groupId != 0)
                                                {
                                                    if (cmbxVATBookRef.SelectedIndex > -1)
                                                    {
                                                        saleReceipt.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                                                    }
                                                    if (btnVATBookPost.IsChecked == true)
                                                    {
                                                        foreach (var tax in saleReceipt.bankCharges)
                                                        {
                                                            Company company = null;
                                                            Department department = null;
                                                            double exchangeRate = 0;

                                                            if (saleReceipt.payment.transactionType == PaymentTransactionType.Admin_Bills)
                                                            {
                                                                company = saleReceipt.payment.adminBill.company;
                                                                department = saleReceipt.payment.adminBill.department;
                                                                exchangeRate = saleReceipt.payment.adminBill.MER;

                                                            }
                                                            else
                                                            if (saleReceipt.payment.transactionType == PaymentTransactionType.Loans_Advances)
                                                            {
                                                                company = saleReceipt.payment.loansAdvance.company;
                                                                department = saleReceipt.payment.loansAdvance.department;
                                                                exchangeRate = saleReceipt.payment.loansAdvance.MER;

                                                            }
                                                            else
                                                            if (saleReceipt.payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                                                            {
                                                                company = saleReceipt.payment.purchaseInvoice.company;
                                                                department = saleReceipt.payment.purchaseInvoice.department;
                                                                exchangeRate = saleReceipt.payment.purchaseInvoice.exchangeRate;

                                                            }
                                                            //else
                                                            //if (receipt.payment.transactionType == PaymentTransactionType.Target_Reward)
                                                            //{
                                                            //    company = receipt.payment.tar.company;
                                                            //    department = receipt.payment.adminBill.department;

                                                            //}
                                                            else
                                                            if (saleReceipt.payment.transactionType == PaymentTransactionType.Vendor_Bills)
                                                            {
                                                                company = saleReceipt.payment.Bill.company;
                                                                department = saleReceipt.payment.Bill.department;
                                                                exchangeRate = Convert.ToDouble(saleReceipt.payment.Bill.ExchangeRate);

                                                            }
                                                            vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                                            {
                                                                CreationDate = dateEditcreationDate.DateTime,
                                                                saleReceiptId = saleReceipt.Id,
                                                                TransactionType = TransactionItemType.Sale_Receipt,
                                                                debit = tax.Amount,
                                                                credit = 0,
                                                                total = tax.Amount - 0,
                                                                FinanceRefNo = txtReceiptRef.Text,
                                                                SystemRefNo = txtSystemRef.Text,
                                                                MER = Math.Round(exchangeRate, 2),
                                                                deptId = department.Id,
                                                                companyId = company.Id,
                                                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                                                VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                                            });
                                                        }
                                                        saleReceipt.VATBooks = vatBooks;
                                                    }
                                                }



                                                if (saleInvoice.department != null && saleInvoice.department.chartofAccountId != null)
                                                    {
                                                        if (btnPushCredits.IsChecked == true)
                                                        {
                                                            var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                                               
                                                                               saleInvoice?.department.chartofAccountId &&
                                                                               x.credit == Math.Round(_item.TotalAmount, 2) &&
                                                                               x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                                               
                                                                               x.deptId == saleInvoice?.department.Id
                                                                               );

                                                            if(dbTransaction==null)
                                                            {
                                                                JournalTransaction receivableTransaction = new JournalTransaction();
                                                                
                                                                receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                                receivableTransaction.deptId = saleInvoice?.department.Id;

                                                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                receivableTransaction.debit = 0;
                                                                receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                                //receivableTransaction.MER = 1;
                                                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                                receivableTransaction.userId = saleReceipt.user_Id;
                                                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                                receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                                
                                                                receivableTransaction.deptId = saleInvoice?.department.Id;
                                                                receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                                receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                journalTransactions.Add(receivableTransaction);
                                                            }
                                                            else
                                                            {
                                                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                                                {
                                                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                                                    
                                                                    receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                                    receivableTransaction.deptId = saleInvoice?.department.Id;

                                                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                    receivableTransaction.debit = 0;
                                                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                    receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                                    //receivableTransaction.MER = 1;
                                                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                                    receivableTransaction.userId = saleReceipt.user_Id;
                                                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    
                                                                    receivableTransaction.deptId = saleInvoice?.department.Id;
                                                                    receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                                    receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                    receivableTransaction.isReconciled = false;
                                                                    receivableTransaction.reconcilationDate = null;
                                                                    receivableTransaction.ReconcilationId = null;
                                                                    receivableTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                                    journalTransactions.Add(receivableTransaction);
                                                                }
                                                                else
                                                                {
                                                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                                                    
                                                                    receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                                    receivableTransaction.deptId = saleInvoice?.department.Id;

                                                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                    receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                    receivableTransaction.debit = 0;
                                                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                    receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                                    //receivableTransaction.MER = 1;
                                                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                                    receivableTransaction.userId = saleReceipt.user_Id;
                                                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    
                                                                    receivableTransaction.deptId = saleInvoice?.department.Id;
                                                                    receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                                    receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                    receivableTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                                    receivableTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                                    receivableTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                                    receivableTransaction.isReconciled = dbTransaction.isReconciled;
                                                                    journalTransactions.Add(receivableTransaction);

                                                                }
                                                            }

                                                        }
                                                    }
                                                    saleReceipt.journalTransactions = journalTransactions;

                                                finalReceipts.Add(saleReceipt);
                                                    
                                                }
                                            repo.updateSalesReceipt(finalReceipts);

                                            if (checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                            {
                                                List<User> tagUsers = new List<User>();
                                                List<User> ccUsers = new List<User>();
                                                List<User> tagUsersRecommendation = new List<User>();
                                                List<User> ccUsersRecommendation = new List<User>();

                                                var res = MessageBox.Show("Status of Sale Receipt has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                                if (res == MessageBoxResult.Yes)
                                                {
                                                    
                                                    
                                                    //var dept = lookupDepartment.SelectedItem as Department;
                                                    if (deptList != null && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                                    {
                                                        
                                                        List<User> usersList = new List<User>();
                                                        foreach (var _dept in deptList)
                                                        {
                                             
                                                            usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, company.Id));

                                                     
                                                        }
                                                        var userss = usersList.Distinct().ToList();

                                                        winTagUsers win = new winTagUsers(userss, saleReceipt.transactionGroupId, TransactionItemType.Sale_Receipt);
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
                                                string oldStat = selectedStatus.Status;
                                                string newStat = saleReceipt.saleReceiptStatus.Status;
                                                CommentLog comment = new CommentLog()
                                                {
                                                    Comment = "Status of Receipt having Collection Amount: " + txtCollectionAmnt.Text + " (" + saleReceipt.Currency.Abbrivation.ToString() + ")" + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                                    Timestamp = DateTime.Now,
                                                    Subject = "Status Changed",
                                                    TaggedList = tagUsers,
                                                    CCUsersList = ccUsers,
                                                    TaggedRecomenndedList = tagUsersRecommendation,
                                                    CCRecomenndedList = ccUsersRecommendation
                                                };
                                                procurementRepo.Add(saleReceipt.transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                                if (checkStatus != null && checkStatus.Id != 0 && checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                                {
                                                    //billRepo.Add(billid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + bill.BillStatus.Status + ")");
                                                    UsersRepo.Add(TransactionInfo.Status_Changed, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, "Status Changed from (" + checkStatus.Status + ") to (" + saleReceipt.saleReceiptStatus.Status + ")");
                                                }
                                                frmInputBox inputBox = new frmInputBox();
                                                inputBox.ShowDialog();
                                                UsersRepo.Add(TransactionInfo.Edited, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                                //if (saleReceipt.saleReceiptStatus.Id != oldStatus.Id)
                                                //{
                                                //    string oldStat = oldStatus.Status;
                                                //    string newStat = saleReceipt.saleReceiptStatus.Status;
                                                //    if (SreceiptId != 0)
                                                //    {
                                                //        CommentLog comment = new CommentLog()
                                                //        {
                                                //            Comment = "Status of Sale Reciept Id # " + SreceiptId.ToString() + " has been changed from Status: "+ oldStat + " to Status: "+newStat,
                                                //            Timestamp = DateTime.Now,
                                                //            Subject = "Status Changes"
                                                //        };

                                                //        procurementRepo.Add(SreceiptId, TransactionItemType.Sale_Receipt, frmInputBox.Comment, SystemLogic.currentUser.employeeId);
                                                //       // notificationsRepo.Add(SystemLogic.currentUser.userName + " mentioned you in Sale Receipt #" + txtSystemRef.Text, groupId, TransactionItemType.Sale_Receipt, frmInputBox.comment, user.id, "New Comment ");

                                                //    }

                                                //}


                                                DXMessageBox.Show("Updated Successfully!");
                                                enter_receipt_win.Close();
                                                enterReceiptWindowFlag = false;
                                            }
                                        }
                                        else
                                        {
                                            if (lookupPrincipal.SelectedIndex < 0)
                                            {
                                                DXMessageBox.Show("Please select Principal!");
                                                return;
                                            }

                                            var _vitem = _visibleItems[0];
                                            saleReceipt = new SalesReceipt();
                                            saleReceipt = repo.GetSalesReceipt(_vitem.Id);

                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null && saleReceipt.isApproved != true)
                                            {
                                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Receipt is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                {
                                                    stage = TransactionStage.Approved.ToString();
                                                    isApproved = true;
                                                    approvalDate = System.DateTime.Now;
                                                }
                                            }


                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null && saleReceipt.isReApproved == false)
                                            {
                                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This SaleReceipt is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                {
                                                    stage = TransactionStage.Approved.ToString();
                                                    isReApproved = true;
                                                    reApprovalDate = System.DateTime.Now;
                                                }
                                            }
                                            //else if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null && collectionAmountChk != double.Parse(txtCollectionAmnt.Text))
                                            //{
                                            //    if (saleReceipt.isReApproved == false)
                                            //    {
                                            //        if (DevExpress.Xpf.Core.DXMessageBox.Show("This SaleReceipt is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                            //        {
                                            //            stage = TransactionStage.Approved.ToString();
                                            //            isReApproved = true;
                                            //            reApprovalDate = System.DateTime.Now;
                                            //        }
                                            //    }
                                            //    //else
                                            //    //{
                                            //    //    stage = TransactionStage.Approved.ToString();
                                            //    //    isReApproved = true;
                                            //    //    reApprovalDate = System.DateTime.Now;
                                            //    //}

                                            //}
                                            //else if (collectionAmountChk != double.Parse(txtCollectionAmnt.Text) && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Collection Amount value in SaleReceipt") != null)
                                            //{
                                            //    stage = TransactionStage.AwaitingApproval.ToString();
                                            //    isReApproved = false;
                                            //}

                                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null) && saleReceipt.PendingForClosing == true)
                                            {
                                                if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Receipt is in Pending for closing State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                {
                                                    stage = TransactionStage.Approved.ToString();
                                                    PendingForClosing = false;
                                                    ClosingDate = System.DateTime.Now;
                                                }
                                            }

                                        List<SalesReceipt> finalReceipt = new List<SalesReceipt>();
                                            for (int i = 0; i < _visibleItems.Count; i++)
                                            {
                                                var _item = _visibleItems[i];
                                                saleReceipt = new SalesReceipt();
                                                saleReceipt = repo.GetSalesReceipt(_item.Id);
                                                saleReceipt.Id = _item.Id;

                                                saleReceipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                                                saleReceipt.receiptType = ((ERP_BL.Enums.ReceiptType)cmbxReceiptType.SelectedIndex);
                                                saleReceipt.CreationDate = dateEditcreationDate.DateTime;
                                                saleReceipt.GLPostingDate = (DateTime)datglPostingdate.EditValue;
                                                saleReceipt.company = loginUserCompanies[cmbxCompany.SelectedIndex];
                                                //saleReceipt.department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

                                                if (deptList.Count != 0)
                                                {
                                                    saleReceipt.Departments = new List<Department>();
                                                    foreach (Department _dept in deptList)
                                                    {
                                                        if (!saleReceipt.Departments.Contains(_dept))
                                                        {
                                                            saleReceipt.Departments.Add(_dept);
                                                        }
                                                    }
                                                saleReceipt.department = null;
                                                }

                                                saleReceipt.Customer = cmbxCustomers.SelectedItem as CustomerCompany;
                                                saleReceipt.SystemRefNo = txtSystemRef.Text;
                                                saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                                                saleReceipt.Currency = cmbxCurrency.SelectedItem as ERP_BL.Databases.Currency;
                                                saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                                                saleReceipt.collectionMethod = cmbxCollectionMethod.SelectedItem as ERP_BL.Databases.CollectionMethod;
                                                saleReceipt.bank = selBank;
                                                saleReceipt.BankId = selBank.Id;
                                                saleReceipt.account = selAccnt;
                                                

                                            var saleInvoice = invoiceRepo.GetSaleInvoice(_item.InvoiceNo);
                                            saleReceipt.saleInvoice = saleInvoice;


                                                saleReceipt.AccountId = selAccnt.Id;
                                                saleReceipt.transactionGroupId = groupId;

                                                saleReceipt.CreditedDate = dateEditcreditedDate.DateTime;
                                                saleReceipt.DepositedDate = datDepositedDate.DateTime;
                                                saleReceipt.InstrumentNo = txtInstrumentNo.Text;
                                                saleReceipt.InstrumentDate = datInstrumentDate.DateTime;
                                                saleReceipt.principal = lookupPrincipal.SelectedItem as Principal;
                                                if (cmbTransactionHolder.SelectedIndex != -1)
                                                {
                                                    saleReceipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                                                    saleReceipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                                                }
                                                if (cmbxPettyCashRef.SelectedIndex > -1)
                                                    saleReceipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                                                //Receipt Asset Status 
                                                if ((cmbSaleReceiptStatus.SelectedItem as cmbitem) != null)
                                                {
                                                    var status = repo.GetSaleReceiptStatus((cmbSaleReceiptStatus.SelectedItem as cmbitem).id);
                                                    if (status != null)
                                                    {
                                                        saleReceipt.saleReceiptStatus = status;
                                                    }
                                                }

                                                if (changedIdList.Contains(_item.Id))
                                                {
                                                    var index = changedIdList.IndexOf(_item.Id);
                                                    var tempList = deductionValues[index];
                                                    for (int j = 0; j < tempList.Count; j++)
                                                    {
                                                        ReceiptDeduction receiptDeduction = new ReceiptDeduction();

                                                        receiptDeduction.Amount = tempList[j];
                                                        receiptDeduction.deduction = listDeductions[j];

                                                        saleReceipt.receiptDeductions.Add(receiptDeduction);
                                                    }
                                                }

                                               

                                                if (MainWindow.currentUserid == 0)
                                                {

                                                }
                                                else if (saleReceipt.user_Id == null)
                                                    saleReceipt.user_Id = MainWindow.currentUserid;

                                                if (stage != null)
                                                    saleReceipt.stage = stage;

                                                if (isApproved != null)
                                                    saleReceipt.isApproved = isApproved;

                                                if (isReApproved != null)
                                                    saleReceipt.isReApproved = isReApproved;

                                                if (approvalDate != null)
                                                    saleReceipt.ApprovedDate = approvalDate;

                                                if (reApprovalDate != null)
                                                    saleReceipt.ReApprovalDate = reApprovalDate;

                                                if (PendingForClosing != null)
                                                    saleReceipt.PendingForClosing = PendingForClosing;

                                                if (ClosingDate != null)
                                                    saleReceipt.ClosingDate = ClosingDate;

                                                if (checkStatus != null && checkStatus.Id != 0)
                                                {
                                                    if (checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                                    {
                                                        saleReceipt.LastStatusChangeDate = System.DateTime.Now;
                                                        if (saleReceipt.saleReceiptStatus.isActive != true)
                                                        {
                                                            saleReceipt.ClosingDate = System.DateTime.Now;
                                                        }
                                                    }
                                                }
                                                saleReceipt.receiptDeductions = _item.receiptDeductions;
                                                saleReceipt.bankCharges = _item.bankChargess;
                                                saleReceipt.IsAdjustedBankVAT = _item.IsBankAdjusted;
                                                saleReceipt.IsAdjustedDedVAT = _item.IsDedAdjusted;
                                                saleReceipt.ReceiptDeductionTaxes = _item.receiptDedTaxes;
                                                saleReceipt.ReceiptBankTaxes = _item.receiptBankTaxes;

                                                saleReceipt.DeductionExchangeRate = _item.ExchangeRate;
                                                saleReceipt.DeductionSOC = _item.TotalDeductionSOC;



                                                List<PettyCash> pettyCashes = new List<PettyCash>();
                                                 if (btnDeposit.IsChecked == true)
                                                    {
                                                        pettyCashes.Add(new PettyCash()
                                                        {
                                                            CreationDate = dateEditcreationDate.DateTime,
                                                            SaleReceiptId = saleReceipt.Id,
                                                            TransactionType = TransactionItemType.Sale_Receipt,
                                                            debit = Math.Round(_item.TotalAmount, 2),
                                                            credit = 0,
                                                            total = Math.Round(_item.TotalAmount, 2) - 0,
                                                            FinanceRefNo = txtReceiptRef.Text,
                                                            SystemRefNo = txtSystemRef.Text,
                                                            MER = 0/*Math.Round(Convert.ToDouble(payment.Bill.ExchangeRate), 2)*/,
                                                            
                                                            deptId = saleInvoice?.department.Id,
                                                            companyId = saleReceipt.company.Id,
                                                            currencyId = saleReceipt.Currency.Id,
                                                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                        });
                                                        saleReceipt.isDeposit = true;
                                                    }
                                                    else if (btnPayment.IsChecked == true)
                                                    {
                                                        pettyCashes.Add(new PettyCash()
                                                        {
                                                            CreationDate = dateEditcreationDate.DateTime,
                                                            SaleReceiptId  = saleReceipt.Id,
                                                            TransactionType = TransactionItemType.Sale_Receipt,
                                                            debit = 0,
                                                            credit = Math.Round(_item.TotalAmount, 2),
                                                            total = 0 - Math.Round(_item.TotalAmount, 2),
                                                            FinanceRefNo = txtReceiptRef.Text,
                                                            SystemRefNo = txtSystemRef.Text,
                                                            MER = 0 /*Math.Round(Convert.ToDouble(payment.Bill.ExchangeRate), 2)*/,
                                                            
                                                            deptId = saleInvoice?.department.Id,
                                                            companyId = saleReceipt.company.Id,
                                                            currencyId = saleReceipt.Currency.Id,
                                                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                        });
                                                        saleReceipt.isDeposit = false;
                                                    }
                                                    else
                                                    {
                                                        saleReceipt.isDeposit = null;
                                                    }
                                                
                                                
                                                saleReceipt.pettyCashes = pettyCashes;


                                                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

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
                                                                    bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                    bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                    bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                    bankTransaction.credit = 0;
                                                                    bankTransaction.userId = saleReceipt.user_Id;
                                                                    bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                    
                                                                    bankTransaction.deptId = saleInvoice?.department?.Id;
                                                                    bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                }
                                                                else
                                                                {
                                                                    var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                    saleReceipt.isBypassBank = false;
                                                    saleReceipt.coaAccountId = null;
                                                    if (banktransactionFlag == 0)
                                                    {
                                                        if (saleReceipt.account.COA_accountId != null)
                                                        {
                                                            if (btnPushDebits.IsChecked == true)
                                                            {
                                                                var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (cmbxAccounts.SelectedItem as Account).COA_accountId);
                                                                JournalTransaction bankTransaction = new JournalTransaction();
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

                                                                }
                                                                else
                                                                {
                                                                    var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                                  (cmbxAccounts.SelectedItem as Account).COA_accountId &&
                                                                  x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) &&
                                                                  x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                                  
                                                                  x.deptId == saleInvoice?.department.Id
                                                                  );

                                                                    if(dbTransaction==null)
                                                                    {
                                                                        bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                                                        bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                        bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                        bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                        bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        bankTransaction.credit = 0;
                                                                        bankTransaction.userId = saleReceipt.user_Id;
                                                                        bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                        
                                                                        bankTransaction.deptId = saleInvoice?.department.Id;
                                                                        bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                      

                                                                    }
                                                                    else
                                                                    {
                                                                        if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                                                        {
                                                                            bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                            bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            bankTransaction.credit = 0;
                                                                            bankTransaction.userId = saleReceipt.user_Id;
                                                                            bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                            
                                                                            bankTransaction.deptId = saleInvoice?.department?.Id;
                                                                            bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                            bankTransaction.isReconciled = false;
                                                                            bankTransaction.reconcilationDate = null;
                                                                            bankTransaction.ReconcilationId = null;
                                                                            bankTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                                                        }
                                                                        else
                                                                        {
                                                                            bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                                                            bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                            bankTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                            bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                                            bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                            bankTransaction.credit = 0;
                                                                            bankTransaction.userId = saleReceipt.user_Id;
                                                                            bankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                            bankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                            bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                                                            
                                                                            bankTransaction.deptId = saleInvoice?.department?.Id;
                                                                            bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                            bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                            bankTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                                            bankTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                                            bankTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                                            bankTransaction.isReconciled = dbTransaction.isReconciled;
                                                                        }

                                                                    }

                                                                   


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
                                                        var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                                                        deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                        deductionTransaction.debit = receiptDeduction.Amount;
                                                                        deductionTransaction.credit = 0;
                                                                        deductionTransaction.userId = saleReceipt.user_Id;
                                                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        
                                                                        deductionTransaction.deptId = saleInvoice?.department?.Id;
                                                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                    }
                                                                    else
                                                                    {
                                                                        var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                            //        deducBankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                            //        deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                                            //        deducBankTransaction.debit = 0;
                                                            //        deducBankTransaction.credit = receiptDeduction.Amount;
                                                            //        //MER = 1,
                                                            //        deducBankTransaction.userId = saleReceipt.user_Id;
                                                            //        deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                            //        deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                            //        deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                            //        deducBankTransaction.deptId = saleReceipt.department.Id;
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
                                                if (saleReceipt.bankCharges.Count != 0)
                                                {
                                                    foreach (var receiptDeduction in saleReceipt.bankCharges)
                                                    {
                                                        var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                                                        deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                        deductionTransaction.debit = receiptDeduction.Amount;
                                                                        deductionTransaction.credit = 0;
                                                                        deductionTransaction.userId = saleReceipt.user_Id;
                                                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        
                                                                        deductionTransaction.deptId = saleInvoice?.department?.Id;
                                                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                    }
                                                                    else
                                                                    {
                                                                        var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                                    deducBankTransaction.userId = saleReceipt.user_Id;
                                                                    deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                    deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    
                                                                    deducBankTransaction.deptId = saleInvoice?.department?.Id;
                                                                    deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                                if (saleReceipt.ReceiptBankTaxes != null && saleReceipt.ReceiptBankTaxes.Count > 0)
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
                                                                        deductionTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                        deductionTransaction.debit = receiptBankTax.Amount;
                                                                        deductionTransaction.credit = 0;
                                                                        deductionTransaction.userId = saleReceipt.user_Id;
                                                                        deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                        deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                        deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                        
                                                                        deductionTransaction.deptId = saleInvoice?.department?.Id;
                                                                        deductionTransaction.total = receiptBankTax.Amount - 0;
                                                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                                    }
                                                                    else
                                                                    {
                                                                        var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
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
                                                                    deducBankTransaction.creationDate = saleReceipt.GLPostingDate;
                                                                    deducBankTransaction.debit = 0;
                                                                    deducBankTransaction.credit = receiptBankTax.Amount;
                                                                    //MER = 1,
                                                                    deducBankTransaction.userId = saleReceipt.user_Id;
                                                                    deducBankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                    deducBankTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    deducBankTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    
                                                                    deducBankTransaction.deptId = saleInvoice?.department?.Id;
                                                                    deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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

                                                
                                                if (saleInvoice?.department != null && saleInvoice.department?.chartofAccountId != null)
                                                {
                                                    if (btnPushCredits.IsChecked == true)
                                                    {
                                                        var dbTransaction = saleReceipt.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                                           
                                                                           saleInvoice?.department.chartofAccountId &&
                                                                           x.credit == Math.Round(_item.TotalAmount, 2) &&
                                                                           x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                                           
                                                                           x.deptId == saleInvoice?.department.Id
                                                                           );

                                                        if (dbTransaction == null)
                                                        {
                                                            JournalTransaction receivableTransaction = new JournalTransaction();
                                                            
                                                            
                                                            receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                            receivableTransaction.deptId = saleInvoice?.department?.Id;

                                                            receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                            receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                            receivableTransaction.debit = 0;
                                                            receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                            receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                            //receivableTransaction.MER = 1;
                                                            //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                            receivableTransaction.userId = saleReceipt.user_Id;
                                                            receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                            receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                            
                                                            receivableTransaction.deptId = saleInvoice?.department.Id;
                                                            receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                            receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                            receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                                                            journalTransactions.Add(receivableTransaction);
                                                        }
                                                        else
                                                        {
                                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                                            {
                                                                JournalTransaction receivableTransaction = new JournalTransaction();
                                                                
                                                                receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                                receivableTransaction.deptId = saleInvoice?.department.Id;

                                                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                receivableTransaction.debit = 0;
                                                                receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                                //receivableTransaction.MER = 1;
                                                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                                receivableTransaction.userId = saleReceipt.user_Id;
                                                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                                receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                                
                                                                receivableTransaction.deptId = saleInvoice?.department?.Id;
                                                                receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                                receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                receivableTransaction.isReconciled = false;
                                                                receivableTransaction.reconcilationDate = null;
                                                                receivableTransaction.ReconcilationId = null;
                                                                receivableTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                                                journalTransactions.Add(receivableTransaction);
                                                            }
                                                            else
                                                            {
                                                                JournalTransaction receivableTransaction = new JournalTransaction();
                                                                
                                                                receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                                receivableTransaction.deptId = saleInvoice?.department.Id;

                                                                receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                receivableTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                                                receivableTransaction.debit = 0;
                                                                receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                                //receivableTransaction.MER = 1;
                                                                //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                                receivableTransaction.userId = saleReceipt.user_Id;
                                                                receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                                receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                                
                                                                receivableTransaction.deptId = saleInvoice?.department?.Id;
                                                                receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                                receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                receivableTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                                                receivableTransaction.reconcilationType = dbTransaction.reconcilationType;
                                                                receivableTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                                                receivableTransaction.isReconciled = dbTransaction.isReconciled;
                                                                journalTransactions.Add(receivableTransaction);

                                                            }
                                                        }

                                                    }
                                                }
                                                saleReceipt.journalTransactions = journalTransactions;
                                            finalReceipt.Add(saleReceipt);
                                            }
                                        repo.updateSalesReceipt(finalReceipt);


                                            
                                        if (checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                        {
                                            List<User> tagUsers = new List<User>();
                                            List<User> ccUsers = new List<User>();
                                            List<User> tagUsersRecommendation = new List<User>();
                                            List<User> ccUsersRecommendation = new List<User>();

                                            var res = MessageBox.Show("Status of Sale Receipt has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                            if (res == MessageBoxResult.Yes)
                                            {
                                                
                                                if (deptList != null && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                                {
                                                   
                                                    List<User> usersList = new List<User>();
                                                    foreach (var _dept in deptList)
                                                    {
                                                        
                                                        usersList.AddRange(UsersRepo.getusersByCompanyDepartment(_dept.Id, company.Id));

                                                    }
                                                    var userss = usersList.Distinct().ToList();

                                                    winTagUsers win = new winTagUsers(userss, groupId, TransactionItemType.Sale_Receipt);
                                                    win.ShowDialog();
                                                    tagUsers = win.tagUsers;
                                                    ccUsers = win.ccUsers;
                                                    tagUsersRecommendation = win.tagRecommendationUsers;
                                                    ccUsersRecommendation = win.ccRecommendationUsers;
                                                    if (win.tagUsers.Count > 0)
                                                    {
                                                        var receipts = repo.getReceiptsByGroupId(saleReceipt.transactionGroupId);

                                                        foreach (var receipt in receipts)
                                                        {
                                                            
                                                            if (receipt.transactionHolderId != win.tagUsers[0].employeeId)
                                                            {
                                                                
                                                                receipt.holderChangeDate = DateTime.Now;
                                                            }
                                                            receipt.transactionHolderId = win.tagUsers[0].employeeId;
                                                            repo.updateSalesRecpt(receipt);
                                                        }
                                                    }
                                                    
                                                }
                                                
                                                else
                                                {
                                      
                                                    winTagUsers win = new winTagUsers();
                                                    win.ShowDialog();
                                                }
                                            }
                                            string oldStat = selectedStatus.Status;
                                            string newStat = saleReceipt.saleReceiptStatus.Status;
                                            CommentLog comment = new CommentLog()
                                            {
                                                Comment = "Status of Receipt having Collection Amount: " + txtCollectionAmnt.Text + " (" + saleReceipt.Currency.Abbrivation.ToString() + ")" + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                                Timestamp = DateTime.Now,
                                                Subject = "Status Changed",
                                                TaggedList = tagUsers,
                                                CCUsersList = ccUsers,
                                                TaggedRecomenndedList = tagUsersRecommendation,
                                                CCRecomenndedList = ccUsersRecommendation
                                            };
                                            procurementRepo.Add(saleReceipt.transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                            if (checkStatus != null && checkStatus.Id != 0 && checkStatus.Id != saleReceipt.saleReceiptStatus.Id)
                                            {
                                                //billRepo.Add(billid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + bill.BillStatus.Status + ")");
                                                UsersRepo.Add(TransactionInfo.Status_Changed, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, "Status Changed from (" + checkStatus.Status + ") to (" + saleReceipt.saleReceiptStatus.Status + ")");
                                            }
                                            frmInputBox inputBox = new frmInputBox();
                                            inputBox.ShowDialog();
                                            UsersRepo.Add(TransactionInfo.Edited, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                            DXMessageBox.Show("Updated Successfully!");
                                            enter_receipt_win.Close();
                                            enterReceiptWindowFlag = false;
                                        }
                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Collection amount is wrong!");
                                    }
                                }




                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Sale Receipt!");

                                enter_receipt_win.Close();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            DXMessageBox.Show("Please Create another Account to Create Sale Receipt, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                            return;
                        }

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt") != null)
                        {
                            //Checking if Collection Amount field is empty
                            if (string.IsNullOrEmpty(txtCollectionAmnt.Text) || string.IsNullOrWhiteSpace(txtCollectionAmnt.Text))
                            {
                                DXMessageBox.Show("Enter Collection amount!");
                            }
                            else
                            {
                                //Checking if Collection amount is equal to Total column summary amount
                                if (total == double.Parse(txtCollectionAmnt.Text))
                                {
                                    //Getting all the Data from the Form to save in the Database

                                    List<SaleReceipts> visibleItems = new List<SaleReceipts>();
                                    List<SaleReceipts> changedItems = new List<SaleReceipts>();
                                    foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
                                    {
                                        var item = (SaleReceipts)_item;
                                        visibleItems.Add(item);
                                    }

                                    foreach (var _item in visibleItems)
                                    {
                                        if (_item.CreditedAmount != 0 || _item.Deductions != 0)
                                        {
                                            changedItems.Add(_item);
                                        }
                                    }


                                    //GroupIdCalculation();

                                    if (cmbxReceiptType.SelectedIndex == 0)
                                    {
                                        if (cmbxCustomers.SelectedIndex < 0)
                                        {
                                            DXMessageBox.Show("Please select Customer!");
                                        }
                                        else
                                        {
                                            List<SalesReceipt> saleReceiptList = new List<SalesReceipt>();
                                            //Saving the Selected Customer from the List

                                            for (int i = 0; i < changedItems.Count; i++)
                                            {
                                                var _item = changedItems[i];
                                                saleReceipt = new SalesReceipt();
                                                saleReceipt.user_Id = MainWindow.currentUserid;

                                                saleReceipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                                                saleReceipt.receiptType = ((ERP_BL.Enums.ReceiptType)cmbxReceiptType.SelectedIndex);
                                                saleReceipt.CreationDate = dateEditcreationDate.DateTime;
                                                saleReceipt.GLPostingDate =(DateTime) datglPostingdate.EditValue;
                                                saleReceipt.company = loginUserCompanies[cmbxCompany.SelectedIndex];
                                                //saleReceipt.department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

                                                if (deptList.Count != 0)
                                                {
                                                    saleReceipt.Departments = new List<Department>();
                                                    foreach (Department _dept in deptList)
                                                    {
                                                        if (!saleReceipt.Departments.Contains(_dept))
                                                        {
                                                            saleReceipt.Departments.Add(_dept);
                                                        }
                                                    }
                                                }

                                                saleReceipt.Customer = cmbxCustomers.SelectedItem as CustomerCompany;
                                                saleReceipt.SystemRefNo = txtSystemRef.Text;
                                                saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                                                saleReceipt.Currency = cmbxCurrency.SelectedItem as ERP_BL.Databases.Currency;
                                                saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                                                saleReceipt.collectionMethod = cmbxCollectionMethod.SelectedItem as ERP_BL.Databases.CollectionMethod;
                                                saleReceipt.bank = selBank;
                                                saleReceipt.BankId = selBank.Id;
                                                saleReceipt.account = selAccnt;
                                                saleReceipt.saleInvoice = invoiceRepo.GetSaleInvoice(_item.InvoiceNo);
                                            var saleInvoice = invoiceRepo.GetSaleInvoice(saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id);


                                                
                                            saleReceipt.AccountId = selAccnt.Id;
                                                //saleReceipt.transactionGroupId = Convert.ToInt32(intGroupId);

                                                saleReceipt.CreditedDate = dateEditcreditedDate.DateTime;
                                                saleReceipt.DepositedDate = datDepositedDate.DateTime;
                                                saleReceipt.InstrumentNo = txtInstrumentNo.Text;
                                                saleReceipt.InstrumentDate = datInstrumentDate.DateTime;
                                                saleReceipt.principal = lookupPrincipal.SelectedItem as Principal;
                                                if (cmbTransactionHolder.SelectedIndex != -1)
                                                {
                                                    saleReceipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                                                    saleReceipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                                                }
                                                if (cmbxPettyCashRef.SelectedIndex > -1)
                                                    saleReceipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;

                                                //Receipt Asset Status 
                                                if ((cmbSaleReceiptStatus.SelectedItem as cmbitem) != null)
                                                {
                                                    var status = repo.GetSaleReceiptStatus((cmbSaleReceiptStatus.SelectedItem as cmbitem).id);
                                                    if (status != null)
                                                    {
                                                        saleReceipt.saleReceiptStatus = status;
                                                    }
                                                }
                                                if (changedIdList.Contains(_item.Id))
                                                {
                                                    var index = changedIdList.IndexOf(_item.Id);
                                                    var tempList = deductionValues[index];
                                                    for (int j = 0; j < tempList.Count; j++)
                                                    {
                                                        ReceiptDeduction receiptDeduction = new ReceiptDeduction();

                                                        receiptDeduction.Amount = tempList[j];
                                                        receiptDeduction.deduction = listDeductions[j];

                                                        saleReceipt.receiptDeductions.Add(receiptDeduction);
                                                    }
                                                }
                                                else
                                                {
                                                    //var tempList = deductionValues[index];
                                                    for (int j = 0; j < listDeductions.Count; j++)
                                                    {
                                                        ReceiptDeduction receiptDeduction = new ReceiptDeduction();
                                                        receiptDeduction.Amount = 0;
                                                        receiptDeduction.deduction = listDeductions[j];

                                                        saleReceipt.receiptDeductions.Add(receiptDeduction);
                                                    }
                                                }

                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null)
                                                {
                                                    saleReceipt.stage = TransactionStage.Approved.ToString();
                                                    saleReceipt.isApproved = true;
                                                    saleReceipt.ApprovedDate = System.DateTime.Now;
                                                }
                                                else
                                                {
                                                    saleReceipt.stage = TransactionStage.AwaitingFirstReview.ToString();
                                                    saleReceipt.isApproved = false;
                                                }

                                                saleReceipt.receiptDeductions = _item.receiptDeductions;
                                                saleReceipt.bankCharges = _item.bankChargess;
                                                saleReceipt.IsAdjustedBankVAT = _item.IsBankAdjusted;
                                                saleReceipt.IsAdjustedDedVAT = _item.IsDedAdjusted;
                                                saleReceipt.ReceiptDeductionTaxes = _item.receiptDedTaxes;
                                                saleReceipt.ReceiptBankTaxes = _item.receiptBankTaxes;

                                                saleReceipt.DeductionExchangeRate = _item.ExchangeRate;
                                                saleReceipt.DeductionSOC = _item.TotalDeductionSOC;

                                                List<PettyCash> pettyCashes = new List<PettyCash>();
                                              
                                                    if (btnDeposit.IsChecked == true)
                                                    {
                                                        pettyCashes.Add(new PettyCash()
                                                        {
                                                            CreationDate = dateEditcreationDate.DateTime,
                                                            SaleReceiptId = 0,
                                                            TransactionType = TransactionItemType.Sale_Receipt,
                                                            debit = Math.Round(_item.TotalAmount, 2),
                                                            credit = 0,
                                                            total = Math.Round(_item.TotalAmount, 2) - 0,
                                                            FinanceRefNo = txtReceiptRef.Text,
                                                            SystemRefNo = txtSystemRef.Text,
                                                            MER = 0 /*Math.Round(Convert.ToDouble(Bills.FirstOrDefault(x => x.Id == _item.BillId).ExchangeRate), 2)*/,
                                                            
                                                            deptId = saleInvoice?.department.Id,
                                                            companyId = saleReceipt.company.Id,
                                                            currencyId = saleReceipt.Currency.Id,
                                                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                        });
                                                        saleReceipt.isDeposit = true;
                                                    }
                                                    else if (btnPayment.IsChecked == true)
                                                    {
                                                        pettyCashes.Add(new PettyCash()
                                                        {
                                                            CreationDate = dateEditcreationDate.DateTime,
                                                            SaleReceiptId = 0,
                                                            TransactionType = TransactionItemType.Sale_Receipt,
                                                            debit = 0,
                                                            credit = Math.Round(_item.TotalAmount, 2),
                                                            total = 0 - Math.Round(_item.TotalAmount, 2),
                                                            FinanceRefNo = txtReceiptRef.Text,
                                                            SystemRefNo = txtSystemRef.Text,
                                                            MER = 0 /*Math.Round(Convert.ToDouble(Bills.FirstOrDefault(x => x.Id == _item.BillId).ExchangeRate), 2)*/,
                                                            
                                                            deptId = saleInvoice?.department.Id,
                                                            companyId = saleReceipt.company.Id,
                                                            currencyId = saleReceipt.Currency.Id,
                                                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                        });
                                                        saleReceipt.isDeposit = false;
                                                    }
                                                    else
                                                    {
                                                        saleReceipt.isDeposit = null;
                                                    }

                                                
                                                saleReceipt.pettyCashes = pettyCashes;

                                                List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

                                                if (isBypassCOA.IsChecked == true)
                                                {
                                                    saleReceipt.isBypassBank = true;
                                                    if (lookupCOA.SelectedIndex != -1)
                                                    {
                                                        saleReceipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                                                        if (banktransactionFlag == 0)
                                                        {
                                                            //var totalCreditedAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                            
                                                                if (btnPushDebits.IsChecked == true)
                                                                {
                                                                    JournalTransaction bankTransaction = new JournalTransaction()
                                                                    {
                                                                        accountId = (lookupCOA.SelectedItem as ChartofAccount).Id,
                                                                        coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                        creationDate = saleReceipt.GLPostingDate,
                                                                        debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value),
                                                                        MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                        credit = 0,
                                                                        //MER = 1,
                                                                        //AmountMER= Convert.ToDouble(txtCollectionAmnt.Text)*1,
                                                                        userId = saleReceipt.user_Id,
                                                                        SaleReceiptId = saleReceipt.Id,
                                                                        transactionRefno = txtReceiptRef.Text,
                                                                        total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0,
                                                                        
                                                                        deptId = saleInvoice?.department.Id,
                                                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                                    };
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
                                                    saleReceipt.isBypassBank = false;
                                                    saleReceipt.coaAccountId = null;
                                                    if (banktransactionFlag == 0)
                                                    {
                                                        //var totalCreditedAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                        if (saleReceipt.account.COA_accountId != null)
                                                        {
                                                            if (btnPushDebits.IsChecked == true)
                                                            {
                                                                JournalTransaction bankTransaction = new JournalTransaction()
                                                                {
                                                                    accountId = selAccnt.COA_accountId,
                                                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                    creationDate = saleReceipt.GLPostingDate,
                                                                    debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value),
                                                                    MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                    credit = 0,
                                                                    //MER = 1,
                                                                    //AmountMER= Convert.ToDouble(txtCollectionAmnt.Text)*1,
                                                                    userId = saleReceipt.user_Id,
                                                                    SaleReceiptId = saleReceipt.Id,
                                                                    transactionRefno = txtReceiptRef.Text,
                                                                    total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0,
                                                                    
                                                                    deptId = saleInvoice?.department?.Id,
                                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                                };
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
                                                        if (receiptDeduction.Amount != 0)
                                                        {
                                                            if (receiptDeduction.deduction.chartofAccountId != null)
                                                            {
                                                                if (btnPushDebits.IsChecked == true)
                                                                {
                                                                    JournalTransaction deductionTransaction = new JournalTransaction();

                                                                    deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                                                    deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                    deductionTransaction.creationDate = saleReceipt.GLPostingDate;
                                                                    deductionTransaction.debit = receiptDeduction.Amount;
                                                                    deductionTransaction.credit = 0;
                                                                    //deductionTransaction.MER = 1;
                                                                    //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                                                    deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                                    deductionTransaction.userId = saleReceipt.user_Id;
                                                                    deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                    deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                    
                                                                    deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                    deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                    deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                    journalTransactions.Add(deductionTransaction);
                                                                }
                                                            }
                                                            if (btnPushCredits.IsChecked == true)
                                                            {
                                                                JournalTransaction deducBankTransaction = new JournalTransaction()
                                                                {
                                                                    accountId = selAccnt.COA_accountId,
                                                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                    creationDate = saleReceipt.GLPostingDate,
                                                                    debit = 0,
                                                                    credit = receiptDeduction.Amount,
                                                                    //MER = 1,
                                                                    userId = saleReceipt.user_Id,
                                                                    MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                    SaleReceiptId = saleReceipt.Id,
                                                                    transactionRefno = txtReceiptRef.Text,
                                                                    
                                                                    deptId = saleInvoice?.department.Id,
                                                                    total = 0 - receiptDeduction.Amount,
                                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                                                };
                                                                journalTransactions.Add(deducBankTransaction);
                                                            }
                                                        }

                                                    }

                                                }
                                                if (saleReceipt.bankCharges != null && saleReceipt.bankCharges.Count != 0)
                                                {
                                                    
                                                foreach (var receiptDeduction in saleReceipt.bankCharges)
                                                    {
                                                        var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                                                        
                                                                        deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                                                    
                                                                    deducBankTransaction.deptId = saleInvoice?.department.Id;
                                                                    deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                                if (saleReceipt.ReceiptBankTaxes != null && saleReceipt.ReceiptBankTaxes.Count > 0)
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
                                                                        
                                                                        deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                        deductionTransaction.total = receiptBankTax.Amount - 0;
                                                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                                                    
                                                                    deducBankTransaction.deptId = saleInvoice?.department.Id;
                                                                    deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                                
                                                if (saleInvoice.department != null && saleInvoice.department.chartofAccountId != null)
                                                {
                                                    
                                                
                                                if (btnPushCredits.IsChecked == true)
                                                    {
                                                        JournalTransaction receivableTransaction = new JournalTransaction();
                                                        
                                                        receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                        receivableTransaction.deptId = saleInvoice?.department.Id;
                                                        receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                        receivableTransaction.creationDate = saleReceipt.GLPostingDate;
                                                        receivableTransaction.debit = 0;
                                                        receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                        //receivableTransaction.MER = 1;
                                                        //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                        receivableTransaction.userId = saleReceipt.user_Id;
                                                        receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                        receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                        receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                        
                                                        receivableTransaction.deptId = saleInvoice?.department.Id;
                                                        receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                        receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                        receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                        journalTransactions.Add(receivableTransaction);
                                                    }
                                                }
                                                saleReceipt.journalTransactions = journalTransactions;
                                                saleReceiptList.Add(saleReceipt);
                                            }
                                            repo.addSalesReceipt(saleReceiptList);
                                            UsersRepo.Add(TransactionInfo.Initialized, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, "");
                                            //_usersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Bill genrated on this Offer");

                                            SystemLog.LogInfo(this.GetType(), "SaleReceipt Added Succesfully refrence No= " + saleReceipt.ReceiptRefNo + " TransactionGroupId=" + saleReceipt.transactionGroupId);

                                            DXMessageBox.Show("Successfully Added");
                                            enter_receipt_win.Close();
                                            enterReceiptWindowFlag = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lookupPrincipal.SelectedIndex < 0)
                                        {
                                            DXMessageBox.Show("Please select Principal!");
                                            return;
                                        }
                                        List<SalesReceipt> saleReceiptList = new List<SalesReceipt>();
                                        for (int i = 0; i < changedItems.Count; i++)
                                        {
                                            var _item = changedItems[i];
                                            saleReceipt = new SalesReceipt();
                                            saleReceipt.user_Id = MainWindow.currentUserid;

                                            saleReceipt.TotalCollectionAmount = Convert.ToDouble(txtCollectionAmnt.Text);
                                            saleReceipt.receiptType = ((ERP_BL.Enums.ReceiptType)cmbxReceiptType.SelectedIndex);
                                            saleReceipt.CreationDate = dateEditcreationDate.DateTime;
                                            saleReceipt.GLPostingDate = (DateTime)datglPostingdate.EditValue;
                                            saleReceipt.company = loginUserCompanies[cmbxCompany.SelectedIndex];

                                            //saleReceipt.department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

                                            if (deptList.Count != 0)
                                            {
                                                saleReceipt.Departments = new List<Department>();
                                                foreach (Department _dept in deptList)
                                                {
                                                    if (!saleReceipt.Departments.Contains(_dept))
                                                    {
                                                        saleReceipt.Departments.Add(_dept);
                                                    }
                                                }
                                            }

                                            saleReceipt.Customer = cmbxCustomers.SelectedItem as CustomerCompany;
                                            saleReceipt.SystemRefNo = txtSystemRef.Text;
                                            saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                                            saleReceipt.Currency = cmbxCurrency.SelectedItem as ERP_BL.Databases.Currency;
                                            saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                                            saleReceipt.collectionMethod = cmbxCollectionMethod.SelectedItem as ERP_BL.Databases.CollectionMethod;
                                            saleReceipt.bank = selBank;
                                            saleReceipt.BankId = selBank.Id;
                                            saleReceipt.account = selAccnt;
                                            
                                            var saleInvoice = invoiceRepo.GetSaleInvoice(_item.InvoiceNo);
                                        saleReceipt.saleInvoice = saleInvoice;

                                            saleReceipt.AccountId = selAccnt.Id;
                                            //saleReceipt.transactionGroupId = Convert.ToInt32(intGroupId);


                                            saleReceipt.CreditedDate = dateEditcreditedDate.DateTime;
                                            saleReceipt.DepositedDate = datDepositedDate.DateTime;
                                            saleReceipt.InstrumentNo = txtInstrumentNo.Text;
                                            saleReceipt.InstrumentDate = datInstrumentDate.DateTime;
                                            saleReceipt.principal = lookupPrincipal.SelectedItem as Principal;
                                            if (cmbTransactionHolder.SelectedIndex != -1)
                                            {
                                                saleReceipt.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                                                saleReceipt.holderChangeDate = (DateTime)datHolderDate.EditValue;
                                            }
                                            saleReceipt.DeductionExchangeRate = _item.ExchangeRate;
                                            saleReceipt.DeductionSOC = _item.TotalDeductionSOC;

                                            if (cmbxPettyCashRef.SelectedIndex > -1)
                                                saleReceipt.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;

                                            //Receipt Asset Status 
                                            if ((cmbSaleReceiptStatus.SelectedItem as cmbitem) != null)
                                            {
                                                var status = repo.GetSaleReceiptStatus((cmbSaleReceiptStatus.SelectedItem as cmbitem).id);
                                                if (status != null)
                                                {
                                                    saleReceipt.saleReceiptStatus = status;
                                                }
                                            }

                                            if (changedIdList.Contains(_item.Id))
                                            {
                                                var index = changedIdList.IndexOf(_item.Id);
                                                var tempList = deductionValues[index];
                                                for (int j = 0; j < tempList.Count; j++)
                                                {
                                                    ReceiptDeduction receiptDeduction = new ReceiptDeduction();
                                                    receiptDeduction.Amount = tempList[j];
                                                    receiptDeduction.deduction = listDeductions[j];
                                                    saleReceipt.receiptDeductions.Add(receiptDeduction);
                                                }
                                            }
                                            else
                                            {
                                                //var tempList = deductionValues[index];
                                                for (int j = 0; j < listDeductions.Count; j++)
                                                {
                                                    ReceiptDeduction receiptDeduction = new ReceiptDeduction();
                                                    receiptDeduction.Amount = 0;
                                                    receiptDeduction.deduction = listDeductions[j];
                                                    saleReceipt.receiptDeductions.Add(receiptDeduction);
                                                }
                                            }


                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null)
                                            {
                                                saleReceipt.stage = TransactionStage.Approved.ToString();
                                                saleReceipt.isApproved = true;
                                                saleReceipt.ApprovedDate = System.DateTime.Now;
                                            }
                                            else
                                            {
                                                saleReceipt.stage = TransactionStage.AwaitingFirstReview.ToString();
                                                saleReceipt.isApproved = false;
                                            }


                                            List<PettyCash> pettyCashes = new List<PettyCash>();
                                           
                                            
                                                if (btnDeposit.IsChecked == true)
                                                {
                                                    pettyCashes.Add(new PettyCash()
                                                    {
                                                        CreationDate = dateEditcreationDate.DateTime,
                                                        SaleReceiptId = 0,
                                                        TransactionType = TransactionItemType.Sale_Receipt,
                                                        debit = Math.Round(_item.TotalAmount, 2),
                                                        credit = 0,
                                                        total = Math.Round(_item.TotalAmount, 2) - 0,
                                                        FinanceRefNo = txtReceiptRef.Text,
                                                        SystemRefNo = txtSystemRef.Text,
                                                        MER = 0 /*Math.Round(Convert.ToDouble(Bills.FirstOrDefault(x => x.Id == _item.BillId).ExchangeRate), 2)*/,
                                                        
                                                        deptId = saleInvoice?.department.Id,
                                                        companyId = saleReceipt.company.Id,
                                                        currencyId = saleReceipt.Currency.Id,
                                                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                    });
                                                    saleReceipt.isDeposit = true;
                                                }
                                                else if (btnPayment.IsChecked == true)
                                                {
                                                    pettyCashes.Add(new PettyCash()
                                                    {
                                                        CreationDate = dateEditcreationDate.DateTime,
                                                        SaleReceiptId = 0,
                                                        TransactionType = TransactionItemType.Sale_Receipt,
                                                        debit = 0,
                                                        credit = Math.Round(_item.TotalAmount, 2),
                                                        total = 0 - Math.Round(_item.TotalAmount, 2),
                                                        FinanceRefNo = txtReceiptRef.Text,
                                                        SystemRefNo = txtSystemRef.Text,
                                                        MER = 0 /*Math.Round(Convert.ToDouble(Bills.FirstOrDefault(x => x.Id == _item.BillId).ExchangeRate), 2)*/,
                                                        
                                                        deptId = saleInvoice?.department.Id,
                                                        companyId = saleReceipt.company.Id,
                                                        currencyId = saleReceipt.Currency.Id,
                                                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                                                    });
                                                    saleReceipt.isDeposit = false;
                                                }
                                                else
                                                {
                                                    saleReceipt.isDeposit = null;
                                                }

                                            
                                            saleReceipt.pettyCashes = pettyCashes;

                                            //Adding Sale receipt to the Database
                                            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();

                                            if (isBypassCOA.IsChecked == true)
                                            {
                                                saleReceipt.isBypassBank = true;
                                                if (lookupCOA.SelectedIndex != -1)
                                                {
                                                    saleReceipt.coaAccountId = (lookupCOA.SelectedItem as ChartofAccount).Id;
                                                    if (banktransactionFlag == 0)
                                                    {
                                                        //var totalCreditedAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                        
                                                            if (btnPushDebits.IsChecked == true)
                                                            {
                                                                JournalTransaction bankTransaction = new JournalTransaction()
                                                                {
                                                                    accountId = (lookupCOA.SelectedItem as ChartofAccount).Id,
                                                                    coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                    creationDate = saleReceipt.GLPostingDate,
                                                                    debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value),
                                                                    MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                    credit = 0,
                                                                    //MER = 1,
                                                                    //AmountMER= Convert.ToDouble(txtCollectionAmnt.Text)*1,
                                                                    userId = saleReceipt.user_Id,
                                                                    SaleReceiptId = saleReceipt.Id,
                                                                    transactionRefno = txtReceiptRef.Text,
                                                                    total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0,
                                                                    
                                                                    deptId = saleInvoice?.department.Id,
                                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                                };
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
                                                saleReceipt.isBypassBank = false;
                                                saleReceipt.coaAccountId = null;
                                                if (banktransactionFlag == 0)
                                                {
                                                    //var totalCreditedAmount = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                                    if (saleReceipt.account.COA_accountId != null)
                                                    {
                                                        if (btnPushDebits.IsChecked == true)
                                                        {
                                                            JournalTransaction bankTransaction = new JournalTransaction()
                                                            {
                                                                accountId = selAccnt.COA_accountId,
                                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                creationDate = saleReceipt.GLPostingDate,
                                                                debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value),
                                                                MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                credit = 0,
                                                                //MER = 1,
                                                                //AmountMER= Convert.ToDouble(txtCollectionAmnt.Text)*1,
                                                                userId = saleReceipt.user_Id,
                                                                SaleReceiptId = saleReceipt.Id,
                                                                transactionRefno = txtReceiptRef.Text,
                                                                total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0,
                                                                
                                                                deptId = saleInvoice?.department.Id,
                                                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id

                                                            };
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
                                                    if (receiptDeduction.Amount != 0)
                                                    {
                                                        if (receiptDeduction.deduction.chartofAccountId != null)
                                                        {
                                                            if (btnPushDebits.IsChecked == true)
                                                            {
                                                                JournalTransaction deductionTransaction = new JournalTransaction();

                                                                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                                                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                                deductionTransaction.creationDate = saleReceipt.GLPostingDate;
                                                                deductionTransaction.debit = receiptDeduction.Amount;
                                                                deductionTransaction.credit = 0;
                                                                //deductionTransaction.MER = 1;
                                                                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                                                deductionTransaction.userId = saleReceipt.user_Id;
                                                                deductionTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);

                                                                deductionTransaction.transactionRefno = txtReceiptRef.Text;
                                                                deductionTransaction.SaleReceiptId = saleReceipt.Id;
                                                                
                                                                deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                                journalTransactions.Add(deductionTransaction);
                                                            }
                                                        }
                                                        if (btnPushCredits.IsChecked == true)
                                                        {
                                                            JournalTransaction deducBankTransaction = new JournalTransaction()
                                                            {
                                                                accountId = selAccnt.COA_accountId,
                                                                coaTransactionsType = coaTransactionsType.SaleReceipt,
                                                                creationDate = saleReceipt.GLPostingDate,
                                                                debit = 0,
                                                                credit = receiptDeduction.Amount,
                                                                //MER = 1,
                                                                userId = saleReceipt.user_Id,
                                                                MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2),
                                                                SaleReceiptId = saleReceipt.Id,
                                                                transactionRefno = txtReceiptRef.Text,
                                                                
                                                                deptId = saleInvoice?.department.Id,
                                                                total = 0 - receiptDeduction.Amount,
                                                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                                                            };
                                                            journalTransactions.Add(deducBankTransaction);
                                                        }
                                                    }
                                                }

                                            }
                                            if (saleReceipt.bankCharges != null && saleReceipt.bankCharges.Count != 0)
                                            {
                                                foreach (var receiptDeduction in saleReceipt.bankCharges)
                                                {
                                                    var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                                                    
                                                                    deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                    deductionTransaction.total = receiptDeduction.Amount - 0;
                                                                    deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                                                
                                                                deducBankTransaction.deptId = saleInvoice?.department.Id;
                                                                deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                                                deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                            if (saleReceipt.ReceiptBankTaxes != null && saleReceipt.ReceiptBankTaxes.Count > 0)
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
                                                                    
                                                                    deductionTransaction.deptId = saleInvoice?.department.Id;
                                                                    deductionTransaction.total = receiptBankTax.Amount - 0;
                                                                    deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                    deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                                                
                                                                deducBankTransaction.deptId = saleInvoice?.department.Id;
                                                                deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                                                deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                                deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                            
                                            if (saleInvoice?.department.chartofAccountId != null)
                                            {
                                                if (btnPushCredits.IsChecked == true)
                                                {
                                                    JournalTransaction receivableTransaction = new JournalTransaction();
                                                    
                                                    receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                                                    receivableTransaction.deptId = saleInvoice?.department.Id;
                                                    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                                    receivableTransaction.creationDate = saleReceipt.GLPostingDate;
                                                    receivableTransaction.debit = 0;
                                                    receivableTransaction.credit = Math.Round(_item.TotalAmount, 2);
                                                    //receivableTransaction.MER = 1;
                                                    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                                    receivableTransaction.userId = saleReceipt.user_Id;
                                                    receivableTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                                    receivableTransaction.transactionRefno = txtReceiptRef.Text;
                                                    receivableTransaction.SaleReceiptId = saleReceipt.Id;
                                                    
                                                    receivableTransaction.deptId = saleInvoice?.department.Id;
                                                    receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                                                    receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                                    receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
                                                    journalTransactions.Add(receivableTransaction);
                                                }
                                            }
                                                saleReceipt.journalTransactions = journalTransactions;
                                            saleReceiptList.Add(saleReceipt);
                                        }
                                        repo.addSalesReceipt(saleReceiptList);
                                        UsersRepo.Add(TransactionInfo.Initialized, saleReceipt.transactionGroupId, (int)TransactionItemType.Sale_Receipt, "");
                                        //_usersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Bill genrated on this Offer");

                                        SystemLog.LogInfo(this.GetType(), "SaleReceipt Added Succesfully refrence No= " + saleReceipt.ReceiptRefNo + " TransactionGroupId=" + saleReceipt.transactionGroupId);

                                        DXMessageBox.Show("Successfully Added!");
                                        enter_receipt_win.Close();
                                        enterReceiptWindowFlag = false;
                                    }
                                }
                                else
                                {
                                    DXMessageBox.Show("Collection amount is wrong!");
                                }
                            }
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Sale Receipt!");

                            enter_receipt_win.Close();
                            return;
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Port_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int x);
        }

        //Got focus funtion for Department Combobox
        private void CmbxDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Select Company first");
            }
        }

        private void EnterReceipt_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            enterReceiptWindowFlag = false;
            e.Cancel = false;
        }

        private void Deduction_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
        }

        //Combobox Company Selected Index Changed Function
        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            loadDepartments();
            loadBillReferenceNo();
            loadVATBookReferenceNo();
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

        private void loadDepartments()
        {
            List<Department> departments = new List<Department>();
            company = cmbxCompany.SelectedItem as ERP_BL.Databases.Company;
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
                    if (receiptsList != null && receiptsList.Count > 0 && saveEditFlag == 1)
                        if (receiptsList[0].Departments != null)
                            foreach (var _dept in receiptsList[0].Departments)
                            {
                                if (departments.FirstOrDefault(x => x.Id == _dept.Id) == null)
                                    departments.Add(_dept);
                            }
                }

                deptList.Clear();
                lookupDepartment1.Text = "";

                if (saveEditFlag == 1 && companyChnaged == null)
                    companyChnaged = false;
                else if (saveEditFlag == 1 && companyChnaged == false)
                    companyChnaged = true;

                lookupDepartment1.ItemsSource = departments;

                loadBillReferenceNo();

                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var banks = receiptRepo.GetBanksbyCompany(company);
                cmbxBanks.ItemsSource = banks;
            }
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
                    if (cmbxCompany.SelectedIndex > -1)
                    {

                        foreach (var _customer in _dept.customers)
                        {
                            if (!customers.Contains(_customer))
                                customers.Add(_customer);
                        }
                        foreach (var _customer in _dept.disableCustomers)
                        {
                            if (!customers.Contains(_customer))
                                customers.Add(_customer);
                        }
                    }
                    allEmployees.AddRange(_dept.employees.Where(x => x.Companies.Find(y => y.Id == (cmbxCompany.SelectedItem as Company).Id) != null));
                }
            }
            loademployees();

            lookupDepartment1.EditValue = deptNames;
            cmbxCustomers.ItemsSource = customers;


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
            if (saveEditFlag == 1 && companyChnaged == false)
            {
                var lookupDepts = (lookupDepartment1.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment1.ItemsSource as List<Department>;
                int index = 0;
                if (receiptsList[0].department != null && deptList != null && deptList.Count > 0)
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
                else if (receiptsList[0].Departments != null && receiptsList[0].Departments.Count > 0)
                {
                    var grid = lookupDepartment1.GetGridControl();
                    gridControl = lookupDepartment1.GetGridControl();
                    var treeView = gridControl.View as TreeListView;

                    var column = grid.Columns[0];
                    index = 0;
                    deptList = receiptsList[0].Departments;

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

        //private void CmbxDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    //Saving the Selected Company
        //    department = new Department();
        //    department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department;

        //    //Clearing Bank and Account list on Selected Department changed
        //    bank = new Bank();
        //    bankList.Clear();
        //    accntList.Clear();
        //    cmbxAccounts.ItemsSource = null;
        //    cmbxBanks.ItemsSource = null;

        //    //Getting all customers linked with selected department
        //    cmbxCustomers.ItemsSource = department.customers.Where(x=>x.isActive == true).ToList();

        //    PrincipalRepo principalRepo = new PrincipalRepo();
        //    var allPrincipals = principalRepo.getAll();

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
        //    lookupPrincipal.ItemsSource = principalList;




        //        var accounts = department.accounts;
        //        allAccounts.Clear();

        //        foreach (var _account in accounts)
        //        {
        //            allAccounts.Add(_account);

        //            if (!bankList.Contains(_account.bank))
        //                bankList.Add(_account.bank);
        //        }

        //        cmbxBanks.ItemsSource = bankList;
        //    loademployees();

        //}

        private void CmbxCustomers_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Select Department First");
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

        //Load Button Click event
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxReceiptType.SelectedIndex == 0)
            {
                grdCustomer.Visibility = Visibility.Visible;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = false;
            }
            else
            {
                grdCustomer.Visibility = Visibility.Collapsed;
                grdCntrlSalesReceipt.Columns["Customer"].Visible = true;
            }

            //Condition to check the Receipt Type
            if (cmbxReceiptType.SelectedIndex == 0)
            {
                if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Company");
                }
                else if (deptList.Count == 0)
                {
                    DXMessageBox.Show("Please Select Department!");
                    lookupDepartment1.Focus();
                    return;
                }
                else if (cmbxCustomers.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Customer");
                }
                else if (cmbxCurrency.SelectedIndex < 0)
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
                    var custCompany = cmbxCustomers.SelectedItem as CustomerCompany;
                    List<SaleReceipts> invoices = new List<SaleReceipts>();

                    //Getting All active Statuses
                    var statuses = invoiceRepo.getAllActiveSaleInvoiceStatus();
                    List<string> statusList = new List<string>();

                    foreach (var _status in statuses)
                    {
                        statusList.Add(_status.Status);
                    }

                    var selectedCurrency = cmbxCurrency.SelectedItem as Currency;
                    allInvoices = invoiceRepo.getAllInvoicesByCustomerId(company.Id, selectedCurrency.Id, deptList, custCompany.Id);

                    if (allInvoices.Count > 0)
                    {
                        // Matching all invoices one by one
                        foreach (var _invoice in allInvoices)
                        {
                            SaleReceipts invoice = new SaleReceipts();
                            if (statusList.Contains(_invoice.saleInvoiceStatus.Status))
                            {
                                //if ((_invoice.customerCompany.company.Id == custCompany.Id) && (_invoice.currency.Id == selectedCurrency.Id) && (_invoice.department.Id == department.Id))
                                //{
                                invoice.Id = _invoice.Id;
                                invoice.Date = _invoice.ApprovedDate;
                                invoice.Customer = _invoice.customerCompany.company.CompanyName;
                                invoice.SoNumber = _invoice.SaleOrderId;
                                invoice.FinanceRefNo = _invoice.FinanceRefrenceNo;
                                invoice.Currency = _invoice.currency.CurrencyName;

                                double invoiceAmount = 0;
                                if (_invoice.saleInvoicetype == InquiryType.DistributionBiz)
                                    invoiceAmount = Math.Round(_invoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                                else
                                    invoiceAmount = _invoice.totalInvoiceAmount;

                              

                                invoice.OriginalAmount = invoiceAmount;

                                invoice.InvoiceStage = GetInvoiceStatus(_invoice);

                                var reciepts = _invoice.salesReceipts;
                                if (reciepts != null)
                                {
                                    var result =Math.Round( reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                                    invoice.AmountDue = invoiceAmount - result;
                                }


                                invoice.InvoiceNo = _invoice.Id;

                                invoices.Add(invoice);
                                //}
                            }
                        }
                        grdCntrlSalesReceipt.ItemsSource = invoices;
                    }
                    else
                    {
                        DXMessageBox.Show("No invoice found!");
                    }
                }
            }
            else
            {
                if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Company");
                }
                else if (deptList.Count == 0)
                {
                    DXMessageBox.Show("Please Select Department!");
                    lookupDepartment1.Focus();
                    return;
                }
                else if (lookupPrincipal.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Select Principal");
                }
                else if (cmbxCurrency.SelectedIndex < 0)
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
                    List <SaleReceipts> invoices = new List<SaleReceipts>();

                    //Getting All active Statuses
                    var statuses = invoiceRepo.getAllActiveSaleInvoiceStatus();
                    List<string> statusList = new List<string>();

                    foreach (var _status in statuses)
                    {
                        statusList.Add(_status.Status);
                    }
                    var selectedCurrency = cmbxCurrency.SelectedItem as Currency;
                    allInvoices = invoiceRepo.getAllInvoicesByDepartmentId(company.Id, selectedCurrency.Id, deptList, (lookupPrincipal.SelectedItem as Principal).Id);

                    if (allInvoices.Count > 0)
                    {
                        // Matching all invoices one by one
                        foreach (var _invoice in allInvoices)
                        {
                            SaleReceipts invoice = new SaleReceipts();

                            if (statusList.Contains(_invoice.saleInvoiceStatus.Status))
                            {
                                //if ((customerNamesList.Contains(_invoice.customerCompany.company.CompanyName)) && (_invoice.currency.Id == selectedCurrency.Id) && (_invoice.department.Id == department.Id))
                                //{
                                invoice.Id = _invoice.Id;
                                invoice.Date = _invoice.ApprovedDate;
                                invoice.Customer = _invoice.customerCompany.company.CompanyName;
                                invoice.SoNumber = _invoice.SaleOrderId;
                                invoice.InvoiceNo = _invoice.Id;
                                invoice.FinanceRefNo = _invoice.FinanceRefrenceNo;
                                invoice.Currency = _invoice.currency.CurrencyName;

                                double invoiceAmount = 0;
                                if (_invoice.saleInvoicetype == InquiryType.DistributionBiz)
                                    invoiceAmount = Math.Round(_invoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                                else
                                    invoiceAmount = _invoice.totalInvoiceAmount;

                                invoice.OriginalAmount = invoiceAmount;

                                invoice.InvoiceStage = GetInvoiceStatus(_invoice);

                                var reciepts = _invoice.salesReceipts;
                                if (reciepts != null)
                                {
                                    var result =Math.Round( reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                                    invoice.AmountDue = invoiceAmount - result;
                                }
                                invoice.InvoiceNo = _invoice.Id;
                                invoices.Add(invoice);

                                //}
                            }
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
            //grdCntrlSalesReceipt.Columns["TotalAmount"].Visible = false;
            //column.Visible = false;
            grdCntrlSalesReceipt.Columns["Id"].Visible = false;
            grdCntrlSalesReceipt.Columns.Remove(column);

            //column = new GridColumn();
            //column.FieldName = "Total";
            //column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;

            //grdCntrlSalesReceipt.Columns.Add(column);

            //grdCntrlSalesReceipt.Columns["Total"].UnboundType = DevExpress.Data.UnboundColumnType.Decimal;

            _changedItems = new List<SaleReceipts>();
            _visibleItems = new List<SaleReceipts>();
            foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
            {
                var item = (SaleReceipts)_item;
                _visibleItems.Add(item);
            }

            foreach (var item in tblViewLandTypeLst.VisibleColumns)
            {
                tblViewLandTypeLst.BestFitColumn(item);
            }

        }

        private void CmbxCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
        }

        private void CmbxBanks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (deptList == null || deptList.Count == 0)
            {
                DXMessageBox.Show("Select Department First");
                lookupDepartment1.Focus();
                return;
            }
            if (cmbxCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please currency first!");
                cmbxCurrency.Focus();
            }
        }

        private void CmbxAccounts_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxBanks.SelectedIndex < 0)
            {
                DXMessageBox.Show("Select Bank First");
            }
        }

        private void CmbxBanks_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //allAccounts = repo.GetAllAccounts();
            //accntList.Clear();
            //cmbxAccounts.ItemsSource = null;
            //bank = new Bank();
            //bank = cmbxBanks.SelectedItem as ERP_BL.Databases.Bank;
            //var curr = cmbxCurrency.SelectedItem as Currency;



            //foreach (var _account in allAccounts)
            //{
            //    if (_account.bank.Id == bank.Id && (_account.currency.Id == curr.Id || _account.isAdjustmentAccount == true))
            //    {
            //        accntList.Add(_account);
            //    }
            //}
            //cmbxAccounts.ItemsSource = accntList;



            var curr = cmbxCurrency.SelectedItem as Currency;
            //var department = lookupDepartment.SelectedItem as Department; 

            List<int> dept_ids = new List<int>();
            foreach (var _dept in deptList)
            {
                dept_ids.Add(_dept.Id);
            }

            if (cmbxBanks.SelectedItem != null)
            {
                var bank = cmbxBanks.SelectedItem as Bank;
                var accntList = repo.GetAllAccountsByBankId(bank.Id).Where(x => x.isActive == true).ToList();/*.Where(x => x.departments.Contains(department) && x.company.Id == (cmbxCompany.SelectedItem as Company).Id && x.accountsCategory == AccountsCategory.Company).ToList()*/
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    foreach (var _deptt in _account.departments)
                    {
                        if (_account.bank.Id == bank.Id && (_account.currency.Id == curr.Id || _account.isAdjustmentAccount == true) && dept_ids.Contains(_deptt.Id))
                        {
                            allowedAccounts.Add(_account);
                            break;
                        }
                    }
                }

                cmbxAccounts.ItemsSource = allowedAccounts;
            }
        }

        private void TblViewLandTypeLst_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Save_click(object sender, RoutedEventArgs e)
        {
            var contnt = win.Content;
            List<double> tempList = new List<double>();
            foreach (UIElement element in layoutControl.Children)
            {
                if (element is TextEdit)
                {
                    var _child = (TextEdit)element;
                    if (_child.Text == "")
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add(Convert.ToDouble(_child.Text));
                        TotalAmount = TotalAmount + Convert.ToDouble(_child.Text);
                    }
                }
            }

            if (deductionEditFlag == false)
            {
                deductionValues.Add(tempList);
            }
            else
            {
                deductionValues[row] = tempList;
            }
            saveClickFlag = true;
            win.Close();
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            
            if (saveEditFlag == 1)
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

        public void loadonSaleInvoicedata()
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

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
          
                if (receiptsList[0].transactionGroupId > 0)
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

                    
                    if (receiptsList[0].transactionGroupId > 0)
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
                         
                        else if (receiptsList[0].transactionGroupId == 0)
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

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (saveEditFlag == 1 && groupId > 0)
                {

                     
                    SalesReceiptRepo repo = new SalesReceiptRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                     
                    receiptsList = repo.getReceiptsByGroupId(groupId);
                    UsersRepo usersRepo = new UsersRepo();

                     
                    if (receiptsList != null && receiptsList.Count > 0)
                    {
                        
                        if (receiptsList[0].isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Sale Receipts are Approved, Do you want to UnApprove these Receipts?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    
                                    for (int i = 0; i < receiptsList.Count; i++)
                                    {
                                        
                                        receiptsList[i].isApproved = false;
                                        receiptsList[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    
                                    usersRepo.Add(TransactionInfo.Approved_Adding, receiptsList[0].transactionGroupId, 11, frmInputBox.comment);

                                    
                                    repo.ApproveReceipts(receiptsList);

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

                                            winTagUsers win = new winTagUsers(usersList, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                            win.ShowDialog();

                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            for (int i = 0; i < receiptsList.Count; i++)
                                            {
                                                if (tagUsers.Count > 0)
                                                {
                                                    
                                                    if (receiptsList[i].transactionHolderId != tagUsers[0].employeeId)
                                                    {
                                                        
                                                        receiptsList[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    
                                                    receiptsList[i].transactionHolderId = tagUsers[0].employeeId;
                                                    repo.updateSalesRecpt(receiptsList[i]);

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
                                    
                                    if (receiptsList[0].Currency != null)
                                    {
                                        
                                        symbolCurr = receiptsList[0].Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        
                                        Comment = "Sale Receipt (Amount OC) having value: " + receiptsList[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                   
                                        Subject = "Sale Receipt UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    
                                    procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + receiptsList[0].ReceiptRefNo, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + receiptsList[0].ReceiptRefNo, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }


                                    
                                    MessageBox.Show("Sale Receipts are UnApproved (" + receiptsList[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Receipts is UnApproved (" + receiptsList[0].transactionGroupId + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Receipts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Receipts Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        
                        else if (receiptsList[0].isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Sale Receipts are Pending for Approval, Do you want to Approve these Receipts?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    
                                    for (int i = 0; i < receiptsList.Count; i++)
                                    {
                                        
                                        receiptsList[i].isApproved = true;
                                        receiptsList[i].stage = TransactionStage.Approved.ToString();
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    
                                    usersRepo.Add(TransactionInfo.Approved_Adding, receiptsList[0].transactionGroupId, 17, frmInputBox.comment);

                                    
                                    repo.ApproveReceipts(receiptsList);

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

                                            winTagUsers win = new winTagUsers(usersList, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt);
                                            win.ShowDialog();

                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            for (int i = 0; i < receiptsList.Count; i++)
                                            {
                                                if (tagUsers.Count > 0)
                                                {
                                                    
                                                    if (receiptsList[i].transactionHolderId != tagUsers[0].employeeId)
                                                    {
                                                        
                                                        receiptsList[i].holderChangeDate = DateTime.Now;
                                                    }
                                                    
                                                    receiptsList[i].transactionHolderId = tagUsers[0].employeeId;
                                                    repo.updateSalesRecpt(receiptsList[i]);

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
                                    
                                    if (receiptsList[0].Currency != null)
                                    {

                                        symbolCurr = receiptsList[0].Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        
                                        Comment = "Sale Receipt (Amount OC) having value: " + receiptsList[0].CollectionAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                     
                                        Subject = "Sale Receipt Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    
                                    procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + receiptsList[0].ReceiptRefNo, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Sale Receipt #" + receiptsList[0].ReceiptRefNo, receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    
                                    MessageBox.Show("Sale Receipts are Approved (" + receiptsList[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Receipts is Approved (" + receiptsList[0].transactionGroupId + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Receipts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Receipts Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        
                        else if (receiptsList[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null) ? true : false)
                            {
                                
                                for (int i = 0; i < receiptsList.Count; i++)
                                {
                                    
                                    receiptsList[i].isReApproved = true;
                                    receiptsList[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                
                                usersRepo.Add(TransactionInfo.Approved_Adding, receiptsList[0].transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                
                                repo.ApproveReceipts(receiptsList);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                
                                MessageBox.Show("Sale Receipts are Approved (" + receiptsList[0].transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + receiptsList[0].transactionGroupId + ")");
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

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
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

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 11, "Viewed details of Sale Receipt");
            }
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlSalesReceipt);
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
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
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
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
                            Subject = "Sale Receipt UnVoided",TaggedList = tagUsers,
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
                            repo.RemoveReceiptSystemCost(_receipt.Id, true);
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
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
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
                            Subject = "Sale Receipt Voided",TaggedList = tagUsers,
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

        private void MbtnAddDeductions_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if(saveEditFlag == 1)
            {
                try
                {
                    if (grdCntrlSalesReceipt.SelectedItem != null)
                    {
                        var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
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
                        //grdCntrlSalesReceipt.SetFocusedRowCellValue("dedVAT", ucSalesReceiptDeduction.totalVAT);

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
            if (saveEditFlag == 1)
            {
                try
                {
                    if (grdCntrlSalesReceipt.SelectedItem != null)
                    {
                        var receipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
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

        private void CmbxPrincipal_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void LookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
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

        private void GrdCntrlSalesReceipt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void GrdCommentss_Loaded(object sender, RoutedEventArgs e)
        {
            // this.GrdCommenttableView.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Green);
        }

        public ucFrmSaleReceipt(SalesReceiptStatus status)
        {
            statusChanged = status;
            //InitializeComponent();
            //receipt_register_win.Closing += ReceiptRegister_Window_Closing;
            //grdSaleReceiptList.Columns["SerialNo"].Visible = false;
        }

        private void BtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            try
            {

                if (saveEditFlag == 1 && groupId > 0)
                {
                    
                    SalesReceiptRepo repo = new SalesReceiptRepo();
                    var SR = repo.GetSaleReceipt(groupId);

                    if (SR != null)
                    {
                        var previous_status = SR.saleReceiptStatus.Status;
                        
                        receiptsList = repo.getReceiptsByGroupId(SR.transactionGroupId);

                        
                        if (receiptsList.Count > 0 && receiptsList[0].isApproved == false)
                        {
                            
                            if (MessageBox.Show("Sale Receipt is Under Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null))
                                {
                                    
                                    receiptsList.ForEach(z => z.isApproved = true);
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
                        
                        else if (receiptsList.Count > 0 && receiptsList[0].isReApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under ReApproval, Do you want to ReApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null))
                                {
                                    
                                    receiptsList.ForEach(z => z.isReApproved = true);
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
                            ucFrmDirectClose.type = "SaleInvoice";

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
                                  
                                    foreach (var _receipt in receiptsList)
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
                                        
                                        if (receiptsList.Count != 0)
                                        {
                                            
                                            procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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
                                    
                                    foreach (var _receipt in receiptsList)
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
                                        
                                        if (receiptsList.Count != 0)
                                        {
                                            
                                            procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                    
                                    foreach (var _receipt in receiptsList)
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
                                        
                                        if (receiptsList.Count != 0)
                                        {
                                            
                                            procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

                                    
                                    foreach (var _receipt in receiptsList)
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
                                        
                                        if (receiptsList.Count != 0)
                                        {
                                            
                                            procurementRepo.Add(receiptsList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
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

        private void BtnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {
            var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
            if (selectedReceipt != null)
            {
                SalesReceipt dbreceipt = repo.GetSalesReceiptForCostSheet(selectedReceipt.Id);
                var deductionAmont = dbreceipt.receiptDeductions.Sum(x => x.Amount);
                var amountSOC = selectedReceipt.TotalDeductionSOC;


                //if (amountSOC != 0)
                //{

                    if(dbreceipt.saleInvoice.saleInvoicetype==InquiryType.DistributionBiz)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                        {
                            SaleOrder saleOrder = new SaleOrder();
                            saleOrder = dbreceipt.saleInvoice.SaleOrder;
                            if (dbreceipt.BudgetSystemCostFields.Count != 0)
                            {
                                winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, true, Convert.ToDouble(amountSOC), dbreceipt,saleOrder);
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
                        if ((grdCntrlSalesReceipt.SelectedItem as SaleReceipts).CostSheetId == null)
                        {
                            repo.updateForCostSheet(dbreceipt.Id, (int)dbreceipt.saleInvoice.SaleOrder.CostSheet_Id);
                            try
                            {
                                var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Receipt?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update System Cost from Sales Receipt") != null)

                                    {
                                        if (dbreceipt.saleInvoice.SaleOrderId != 0  && dbreceipt.Id != 0)
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
                                        if (dbreceipt.saleInvoice.SaleOrderId != 0  && dbreceipt.Id != 0)
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



                    
                //}
                //else
                //{
                //    DXMessageBox.Show("No deduction found into selected Sales Receipt, Please Select different Sales Receipt.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;

                //}
            }
            
        }

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            string currency=null;
            string incoTerm = null;
            string creationDate = null;
            string paymentTerm = null;
            string maker = null;
            string origin = null;
            string packing = null;
            string Warranty = null;
            var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
            if (selectedReceipt.Id != 0)
            {
                SalesReceipt dbreceipt = repo.GetSalesReceiptForCostSheet(selectedReceipt.Id);


                if(dbreceipt.saleInvoice.saleInvoicetype==InquiryType.DistributionBiz)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
                    {

                        SaleOrder saleOrder = new SaleOrder();
                        saleOrder= dbreceipt.saleInvoice.SaleOrder;

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
                    if ((grdCntrlSalesReceipt.SelectedItem as SaleReceipts).CostSheetId == 0)
                    {
                        repo.updateForCostSheet(dbreceipt.Id, (int)dbreceipt.saleInvoice.SaleOrder.CostSheet_Id);
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

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if (saveEditFlag!= 0)
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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (saveEditFlag == 1 && groupId > 0)
            {
                receiptsList = repo.getReceiptsByGroupId(groupId);
                double collectedAmount = 0;
                List<SaleReceipts> invoices = new List<SaleReceipts>();
                foreach (var _receipt in receiptsList)
                {
                    SaleReceipts recpt = new SaleReceipts();
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
                    recpt.Customer = _invoice.customerCompany.company.CompanyName;
                    recpt.SoNumber = _invoice.SaleOrderId;
                    recpt.FinanceRefNo = _invoice.FinanceRefrenceNo;
                    recpt.Currency = _invoice.currency.CurrencyName;

                    double invoiceAmount = 0;
                    if (_invoice.saleInvoicetype == InquiryType.DistributionBiz)
                        invoiceAmount = Math.Round(_invoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                    else
                        invoiceAmount = _invoice.totalInvoiceAmount;

                    recpt.OriginalAmount = invoiceAmount;

                    invoiceRepo = new SaleInvoiceRepo();
                    var invoice1 = invoiceRepo.GetSaleInvoice(_invoice.Id);
                    var reciepts = invoice1.salesReceipts;
                    recpt.InvoiceStage = GetInvoiceStatus(invoice1);
                    
                    var result =Math.Round( reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                    recpt.AmountDue = invoiceAmount - result;
                    recpt.InvoiceNo = _invoice.Id;

                    recpt.receiptDeductions = _receipt.receiptDeductions;
                    if (_receipt.receiptDeductions.Count != 0)
                    {
                        recpt.Deductions = _receipt.receiptDeductions.Sum(x => x.Amount);
                    }
                    recpt.CreditedAmount = _receipt.CollectionAmount - recpt.Deductions;

                    invoices.Add(recpt);
                }

                grdCntrlSalesReceipt.ItemsSource = invoices;
            }
        }

        private void LookupCOA_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
          
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            //var userChartofAccounts = chartofAccountsRepo.GetAllforVendorBills(cmbxCompany.SelectedItem as Company, lookupDepartment.SelectedItem as Department, SYSTEM_STATIC.currentUser.id);
            //lookupCOA.ItemsSource = userChartofAccounts;

            List<ChartofAccount> userChartofAccounts = new List<ChartofAccount>();

            foreach (var _dept in deptList)
            {
                userChartofAccounts.AddRange(chartofAccountsRepo.GetAllforVendorBills(cmbxCompany.SelectedItem as Company,  _dept, SYSTEM_STATIC.currentUser.id));
            }

            userChartofAccounts = userChartofAccounts.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            lookupCOA.ItemsSource = userChartofAccounts;
        }

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
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
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            List<SaleReceipts> _visibleItems = new List<SaleReceipts>();
            _visibleItems = new List<SaleReceipts>();
            foreach (var _item in grdCntrlSalesReceipt.VisibleItems)
            {
                _visibleItems.Add((SaleReceipts)_item);

            }
            var _vitem = _visibleItems[0];
            SalesReceipt saleReceipt = new SalesReceipt();
            saleReceipt = repo.GetSalesReceipt(_vitem.Id);
            var selBank = cmbxBanks.SelectedItem as Bank;
            var selAccnt = cmbxAccounts.SelectedItem as Account;
            for (int i = 0; i < _visibleItems.Count; i++)
            {
                var _item = _visibleItems[i];
                saleReceipt = new SalesReceipt();
                saleReceipt = repo.GetSalesReceipt(_item.Id);
                saleReceipt.Id = _item.Id;
                saleReceipt.receiptType = ((ERP_BL.Enums.ReceiptType)cmbxReceiptType.SelectedIndex);
                saleReceipt.CreationDate = dateEditcreationDate.DateTime;
                saleReceipt.company = loginUserCompanies[cmbxCompany.SelectedIndex];
                //saleReceipt.department = lookupDepartment.SelectedItem as ERP_BL.Databases.Department; It has to be resolved
                saleReceipt.Customer = cmbxCustomers.SelectedItem as CustomerCompany;
                saleReceipt.SystemRefNo = txtSystemRef.Text;
                saleReceipt.ReceiptRefNo = txtReceiptRef.Text;
                saleReceipt.Currency = cmbxCurrency.SelectedItem as ERP_BL.Databases.Currency;
                saleReceipt.CollectionAmount = Math.Round(_item.TotalAmount, 2);
                saleReceipt.bank = selBank;
                saleReceipt.BankId = selBank.Id;
                saleReceipt.account = selAccnt;
                
                
                var saleInvoice = invoiceRepo.GetSaleInvoice(_item.InvoiceNo); ;
                saleReceipt.saleInvoice = saleInvoice;

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
                                 
                                    bankTransaction.deptId = saleInvoice?.department.Id;
                                    bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                var dbtrans = saleReceipt.journalTransactions.FirstOrDefault(x => x.debit == Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) && x.accountId == (cmbxAccounts.SelectedItem as Account).COA_accountId);
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
                                    bankTransaction.accountId = (cmbxAccounts.SelectedItem as Account).COA_accountId;
                                    bankTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    bankTransaction.creationDate = saleReceipt.GLPostingDate;
                                    bankTransaction.debit = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value);
                                    bankTransaction.MER = Math.Round(Convert.ToDouble(saleReceipt.saleInvoice.exchangeRate), 2);
                                    bankTransaction.credit = 0;
                                    bankTransaction.userId = saleReceipt.user_Id;
                                    bankTransaction.SaleReceiptId = saleReceipt.Id;
                                    bankTransaction.transactionRefno = txtReceiptRef.Text;
                                    bankTransaction.total = Convert.ToDouble(grdCntrlSalesReceipt.Columns["CreditedAmount"].TotalSummaries[0].Value) - 0;
                                    
                                    bankTransaction.deptId = saleInvoice?.department.Id;
                                    bankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                    bankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                        var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                        
                                        deductionTransaction.deptId = saleInvoice?.department.Id;
                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                        var deduction = repo.GetDeductionById(receiptDeduction.deduction_Id.Value);
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
                                        
                                        deductionTransaction.deptId = saleInvoice?.department.Id;
                                        deductionTransaction.total = receiptDeduction.Amount - 0;
                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                    
                                    deducBankTransaction.deptId = saleInvoice?.department.Id;
                                    deducBankTransaction.total = 0 - receiptDeduction.Amount;
                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                                        
                                        deductionTransaction.deptId = saleInvoice?.department.Id;
                                        deductionTransaction.total = receiptBankTax.Amount - 0;
                                        deductionTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                        deductionTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

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
                                    
                                    deducBankTransaction.deptId = saleInvoice?.department.Id;
                                    deducBankTransaction.total = 0 - receiptBankTax.Amount;
                                    deducBankTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                                    deducBankTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;
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
                
                if (saleInvoice?.department.chartofAccountId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {

                        JournalTransaction receivableTransaction = new JournalTransaction();
                      
                        receivableTransaction.accountId = saleInvoice?.department.chartofAccountId;
                        receivableTransaction.deptId = saleInvoice?.department.Id;

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
                        
                        receivableTransaction.deptId = saleInvoice?.department.Id;
                        receivableTransaction.total = 0 - Math.Round(_item.TotalAmount, 2);
                        receivableTransaction.companyId = (cmbxCompany.SelectedItem as Company).Id;
                        receivableTransaction.currencyId = (cmbxCurrency.SelectedItem as Currency).Id;

                        journalTransactions.Add(receivableTransaction);
                    }
                }
               
            }


            return journalTransactions;
        }

        private void BtnCreateIBT_Click(object sender, RoutedEventArgs e)
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
            ucBankTransferInterCompany uc = new ucBankTransferInterCompany(groupId,true);
            Window win = new Window();
            win.Content = uc;
            
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void btnCostSheetPunching_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                var selectedReceipt = grdCntrlSalesReceipt.SelectedItem as SaleReceipts;
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

        private void TxtDeductionAmount_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            var dedAmount = Convert.ToDouble(txtDeductionAmount.Text);
            var exchangeRate = Convert.ToDouble(txtDedER.Text);
            var VAT = Convert.ToDouble(txtDedVAT.Text);
            txtDedTotal.Text = Math.Round(dedAmount + VAT,2).ToString();
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
            var Total = Math.Round((dedAmount +VAT) * exchangeRate, 2);
            txtDedSOC.Text = Total.ToString();

            int count = grdCntrlSalesReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdCntrlSalesReceipt.SetCellValue(i, grdCntrlSalesReceipt.Columns["ExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }

        private void TblViewLandTypeLst_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            var row = e.Row as SaleReceipts;
            var ded = row.Deductions + row.VAT + row.BankCharges;
            row.ExchangeRate = Convert.ToDouble(txtDedER.Text);

            var rcpt = receiptsList.FirstOrDefault(x => x.Id == row.Id);

            if (saveEditFlag == 1 && rcpt != null)
            {
                var credt = row.CreditedAmount;
                //var adjustmentAmount = pymnt.adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                var amountReceived = rcpt.saleInvoice.salesReceipts.Where(x => x.isVoid != true && x.Id != rcpt.Id).Sum(y => y.CollectionAmount) /*+ adjustmentAmount*/;
                amountReceived = Math.Round(amountReceived + credt, 2);

                double invoiceAmount = 0;
                if (rcpt.saleInvoice.saleInvoicetype == InquiryType.DistributionBiz)
                    invoiceAmount = Math.Round(rcpt.saleInvoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                else
                    invoiceAmount = rcpt.saleInvoice.totalInvoiceAmount;

               if (Math.Abs(amountReceived) > Math.Abs(invoiceAmount))
                {
                    DXMessageBox.Show("Payment cannot exceed Bill Amount!");
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

            if (Math.Round(row.CreditedAmount, 2) > Math.Round(row.AmountDue, 2) && saveEditFlag == 0)
            {
                DXMessageBox.Show("Credited amount cannot exceed Amount Due!");
                ((DataViewBase)sender).Background = Brushes.LightBlue;
                row.CreditedAmount = 0;

                //row.Deductions = 0;
                return;
            }

            if (row.receiptBankTaxes != null)
                row.BankChargesSOC = (row.BankCharges + row.receiptBankTaxes.Sum(x => x.Amount)) *row.ExchangeRate;
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
                row.TotalAmount = row.CreditedAmount + row.Deductions ;

            }
            row.TotalDeductionSOC = Math.Round(row.ExchangeRate * ded, 2);

            

            

            //row.TotalAmount = row.CreditedAmount + ded;
        }

        private void TblViewLandTypeLst_ShowGridMenu(object sender, GridMenuEventArgs e)
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

            int count = grdCntrlSalesReceipt.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdCntrlSalesReceipt.SetCellValue(i, grdCntrlSalesReceipt.Columns["ExchangeRate"], exchangeRate);
                //_item.ExchangeRate = exchangeRate;
            }
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (saveEditFlag != 0)
            {
                var source = grdCntrlSalesReceipt.ItemsSource as List<SaleReceipts>;
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
                var idd = (grdCntrlSalesReceipt.SelectedItem as SaleReceipts).Id;
               
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

            //MessageBox.Show("Mission Successful!");
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void btnCreateReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Create Direct Receipt from Receipts") != null)
            {
                ucFrmDirectReceiptSR frmDirectReceiptSO = new ucFrmDirectReceiptSR();
                frmDirectReceiptSO.editFlag = false;
                frmDirectReceiptSO.linkedReceiptId = receiptId;
                frmDirectReceiptSO.receiptId = 0;
                Window win = new Window();
                win.Content = frmDirectReceiptSO;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Direct Receipt from Sale Receipts!");
            }
        }

      

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;

            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;

        }
    }

    public class SaleReceipts
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Customer { get; set; }
        public int? SoNumber { get; set; }
        public int InvoiceNo { get; set; }
        public string FinanceRefNo { get; set; }
        public string Currency { get; set; }
        public double OriginalAmount { get; set; }
        public double AmountDue { get; set; }
        public double CreditedAmount { get; set; }
        public double Deductions { get; set; }
        public double VAT { get; set; }
        public double BankCharges { get; set; }

        public double ExchangeRate { get; set; }
        public double DeductionSOC { get; set; }
        public double BankChargesSOC { get; set; }
        public double TotalDeductionSOC { get; set; }
        public double TotalAmount { get; set; }
        public Button SyetemCost { get; set; }
        public int CostSheetId { get; set; }
        public Button CostSheet { get; set; }
        public string InvoiceStage { get; set; }
        public List<ReceiptDeduction> receiptDeductions = new List<ReceiptDeduction>();
        public List<ReceiptDeduction> bankChargess = new List<ReceiptDeduction>();
        public List<ReceiptTax> receiptDedTaxes { get; set; }
        public List<ReceiptTax> receiptBankTaxes { get; set; }

        public double dedVAT { get; set; }
        public double bankVAT { get; set; }
        public bool? IsBankAdjusted { get; set; }
        public bool? IsDedAdjusted { get; set; }
        public  List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }

    }

    public class GetAllReceipts
    {
        public virtual bool IsLoading { get; set; }
        public List<SaleReceipts> ReceiptList { get; private set; }

        public GetAllReceipts()
        {
            IsLoading = true;
            List<SaleReceipts> receipts = new List<SaleReceipts>();
            SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();

            ReceiptList = receipts;
            IsLoading = false;
            //IsLoading = false;
        }
    }
}