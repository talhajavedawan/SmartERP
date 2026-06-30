using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.LookUp;
using ERP_BL.AssetsRentals;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.CreditCards;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Fields;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Tax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Adjustments.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmBillAdd.xaml
    /// </summary>
    public partial class ucFrmBillAdd : UserControl
    {
        AdminBillsRepo BillsRepo = new AdminBillsRepo();
        VendorRepo vendorRepo = new VendorRepo();
        ucFrmAddMemo frmAddMemo = new ucFrmAddMemo();
        Window memoWindow = new Window();
        int intGroupId;
        public int groupId = 0;
        List<cmbitem> cmbitems = new List<cmbitem>();
        Department department = new Department();
        Company company = new Company();


        public List<AdminBill> bills = new List<AdminBill>();
        public List<AdminBill> removedBills = new List<AdminBill>();
        public List<AdminBill> addedBills = new List<AdminBill>();
        public bool editFlag = false;

        AdminBill bill = new AdminBill();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        string stage;
        bool? isApproved;
        DateTime? approvalDate;


        AdminBillStatus checkStatus = new AdminBillStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<User> UsersForComments = new List<User>();
        static AdminBillStatus statusChanged = new AdminBillStatus();

        bool isFullyPaid = true;

        List<AdminBillType> adminBillTypes = new List<AdminBillType>();
        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        List<AdminBillNature> adminBillNatures = new List<AdminBillNature>();
        public int loansAdvanceId = 0;
        LoansAdvance loansAdvance = new LoansAdvance();

        public int assetRentalId = 0;
        public AdminBillTypes billTypes = AdminBillTypes.Admin_Bill;
        public bool? isProgressiveCost = null;
        AssetRental assetRental = new AssetRental();

        List<TaxName> taxes = new List<TaxName>();
        List<Payee> payeeList = new List<Payee>();
        List<AdminBillStatus> BillStatuses = new List<AdminBillStatus>();

        public ucFrmBillAdd()
        {
            InitializeComponent();
        }

        private void loadTaxes()
        {
            TaxRepo taxRepo = new TaxRepo();
            taxes = taxRepo.getAllTaxes();
            //ucBillDataRow.lookupVat.ItemsSource = taxes;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            

            
               

                grdProgressBar1.Visibility = Visibility.Visible;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += OnDoWork;
                worker.RunWorkerCompleted += OnRunWorkerCompleted;
                worker.RunWorkerAsync();

                grdProgressBar1.Visibility = Visibility.Collapsed;


                

            
        }

        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(500).Wait();  // Pretend to work
        }


        bool clickCheck = false;
        private void BtnFuel_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {

                var stackPanel = (StackPanel)((SimpleButton)sender).Parent;
                //var stackPanel = (StackPanel)parent.Parent;
                var grid = (Grid)stackPanel.Parent;
                var row = (UserControl)grid.Parent;


                if (row != null)
                {
                    var _bill = (ucBillData)row;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var billId = Convert.ToInt32((billElement.FindName("txtId") as TextBlock).Text);

                        if (billId > 0)
                        {
                            ucAddFuelExpenses ucAddExpenses = new ucAddFuelExpenses(bills.Find(x => x.Id == billId), bills.Find(x => x.Id == billId).payee);
                            //var maintenanceHeads = BillsRepo.GetAllMaintenanceHead();

                            if (billElement.lookupAdminBillType.SelectedIndex > -1 && billElement.cmbxVendor.SelectedIndex > -1 && cmbxCompany.SelectedIndex > -1 && cmbxDepartment.SelectedIndex > -1)
                            {
                                var cmpny = cmbxCompany.SelectedItem as Company;
                                var dept = cmbxDepartment.SelectedItem as Department;
                                var billType = billElement.lookupAdminBillType.SelectedItem as AdminBillType;
                                var vendor = billElement.cmbxVendor.SelectedItem as Vendor;

                                payeeList = BillsRepo.GetAllPayeesOfCompanyDept(cmpny, dept, billType, vendor);
                                ucAddExpenses.lookupPayee.ItemsSource = payeeList;
                            }


                            if (ucAddExpenses.adminBill != null && ucAddExpenses.adminBill.fuelExpenses != null && ucAddExpenses.adminBill.fuelExpenses.Count > 0)
                                ucAddExpenses.editFlag = true;
                            else
                                ucAddExpenses.editFlag = false;

                            ucAddExpenses.ShowDialog();


                            if (ucAddExpenses.saveflag == true)
                            {
                                var _billl = bills.Find(x => x.Id == billId);
                                _billl.fuelExpenses = ucAddExpenses.finalVehicleExpenses;
                                billElement.txtAmountOC.Text = Math.Round((ucAddExpenses.totalExpenses + _billl.vehicleExpenses?.Sum(x => x.ExpenseAmount)).Value, 2).ToString();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Invalid Admin Bill!");
                        }

                    }
                }
                
            }
            else
            {
                DXMessageBox.Show("Please Save this Admin Bill first!");
            }
        }

        private void BtnVehicleExpenses_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {

                var stackPanel = (StackPanel)((SimpleButton)sender).Parent;
                //var stackPanel = (StackPanel)parent.Parent;
                var grid = (Grid)stackPanel.Parent;
                var row = (UserControl)grid.Parent;


                if(row != null)
                {
                    var _bill = (ucBillData)row;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var billId = Convert.ToInt32((billElement.FindName("txtId") as TextBlock).Text);

                        if (billId > 0)
                        {
                            ucAddVehicleExpenses ucAddExpenses = new ucAddVehicleExpenses(bills.Find(x => x.Id == billId), bills.Find(x => x.Id == billId).payee);
                            var maintenanceHeads = BillsRepo.GetAllMaintenanceHead();
                            ucAddExpenses.grdVehicleExpenses.ItemsSource = ucAddExpenses.vehicleExpenses;
                            ucAddExpenses.lookupMaintenanceHead.ItemsSource = maintenanceHeads;

                            if (billElement.lookupAdminBillType.SelectedIndex > -1 && billElement.cmbxVendor.SelectedIndex > -1 && cmbxCompany.SelectedIndex > -1 && cmbxDepartment.SelectedIndex > -1)
                            {
                                var cmpny = cmbxCompany.SelectedItem as Company;
                                var dept = cmbxDepartment.SelectedItem as Department;
                                var billType = billElement.lookupAdminBillType.SelectedItem as AdminBillType;
                                var vendor = billElement.cmbxVendor.SelectedItem as Vendor;

                                payeeList = BillsRepo.GetAllPayeesOfCompanyDept(cmpny, dept, billType, vendor);
                                ucAddExpenses.lookupPayee.ItemsSource = payeeList;
                            }

                            if (ucAddExpenses.adminBill != null && ucAddExpenses.adminBill.vehicleExpenses != null && ucAddExpenses.adminBill.vehicleExpenses.Count > 0)
                                ucAddExpenses.editFlag = true;
                            else
                                ucAddExpenses.editFlag = false;

                            ucAddExpenses.ShowDialog();


                            if (ucAddExpenses.saveflag == true)
                            {
                                var _billl = bills.Find(x => x.Id == billId);
                                _billl.vehicleExpenses = ucAddExpenses.finalVehicleExpenses;
                                billElement.txtAmountOC.Text = Math.Round((ucAddExpenses.totalExpenses + _billl.fuelExpenses?.Sum(x => x.ExpenseAmount)).Value, 2).ToString();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Invalid Admin Bill!");
                        }

                    }
                }
                
                    
                
            }
            else
            {
                DXMessageBox.Show("Please Save this Admin Bill first!");
            }
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
            {
                cmbxHolderType.Items.Add(((ERP_BL.Enums.CardHolderType)i).ToString());
            }

            for (int i = 0; i <= (int)ERP_BL.Enums.AdminBillTypes.Asset; i++)
            {
                cmbxBillType.Items.Add(((ERP_BL.Enums.AdminBillTypes)i).ToString());
            }

            ucBillDataRow.btnMaintenance.Click += BtnVehicleExpenses_Click;
            ucBillDataRow.btnFuel.Click += BtnFuel_Click;
            ucBillDataRow.txtAmountOC.KeyUp += TxtAmountOC_KeyUp;
            ucBillDataRow.txtAmountWithTax.EditValueChanged += TxtAmountWithTax_EditValueChanged;
            ucBillDataRow.txtAmountPaidOC.EditValueChanged += TxtTotalAmountPaid_EditValueChanged;
            ucBillDataRow.txtRemainingAmountOC.EditValueChanged += TxtTotalAmountDue_EditValueChanged;

            ucBillDataRow.cmbxCOA.GotFocus += CmbxCoa_GotFocus;
            ucBillDataRow.cmbxCOA.SelectedIndexChanged += CmbxCoa_SelectedIndexChanged;
            ucBillDataRow.cmbxPayeeName.GotFocus += CmbxPayeeName_GotFocus;
            ucBillDataRow.cmbxVendor.SelectedIndexChanged += CmbxVendor_SelectedIndexChanged;
            ucBillDataRow.lookupAdminBillType.GotFocus += CmbxAdminBillType_GotFocus;
            ucBillDataRow.lookupAdminBillType.SelectedIndexChanged += CmbxAdminBillType_SelectedIndexChanged;
            ucBillDataRow.lookupVat.SelectedIndexChanged += LookupVat_SelectedIndexChanged;
            ucBillDataRow.txtTaxAmount.EditValueChanged += TxtTaxAmount_EditValueChanged;

            loadTaxes();
            loadTemplates();
            loadCompanies();
            loadManagementSummaries();
            loadCurrencies();
            loadBillStatus();
            loadAdminBillTypes();
            ucBillDataRow.lookupAdminBillType.ItemsSource = adminBillTypes;

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                txtPaymentsClosed.Visibility = Visibility.Collapsed;

                if (loansAdvanceId != 0)
                {
                    mbtnAddAdjustment.Visibility = Visibility.Visible;
                    AdjustmentFieldVisibility();
                }

                if(billTypes == AdminBillTypes.Asset && assetRentalId > 0)
                {
                    AssetRentalRepo rentalRepo = new AssetRentalRepo();
                    assetRental = rentalRepo.GetAssetRental(assetRentalId);
                    //Select Company
                    if (assetRental.company != null)
                    {
                        cmbxCompany.EditValue = assetRental.companyId;
                    }

                    //Select Company
                    if (assetRental.department != null)
                    {
                        cmbxDepartment.EditValue = assetRental.deptId;
                    }

                    cmbxCompany.IsReadOnly = true;
                    cmbxDepartment.IsReadOnly = true;
                }
            }

            for (int i = 0; i <= (int)ERP_BL.Enums.AdminBillTypes.Asset; i++)
            {
                if (((ERP_BL.Enums.AdminBillTypes)i).ToString() == billTypes.ToString())
                {
                    cmbxBillType.SelectedIndex = i;
                    break;
                }
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit MER in Admin Bills") != null)
                txtMER.IsReadOnly = false;
            else
                txtMER.IsReadOnly = true;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Admin Bills") != null)
            {
                panelMngmntSummary.Visibility = Visibility.Visible;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
            {
                btnPushDebits.IsEnabled = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
            {
                btnPushCredits.IsEnabled = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of Admin Bills") != null)
            {
                datglPostingdate.IsEnabled = true;
            }
            else
            {
                datglPostingdate.IsEnabled = false;

            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Admin Bill") != null)
            {
                datCreationDate.IsEnabled = true;
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Enable Employee List in Admin Bills") == null)
            {
                chkEmployee.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill Reference Number in Admin Bill") != null)
            {
                cmbxBillRef.IsEnabled = true;
            }
            else
            {
                cmbxBillRef.IsEnabled = false;
            }

            if (editFlag == true && groupId > 0)
            {
                bills = BillsRepo.GetBillsByGroupIdForForm(groupId);

                billTypes = bills[0].billTypes;
                isProgressiveCost = bills[0].isProgressiveCost;

                if(billTypes == AdminBillTypes.Asset)
                {
                    cmbxCompany.IsReadOnly = true;
                    cmbxDepartment.IsReadOnly = true;
                }

                if (bills[0].isAdjustedTax == true)
                    chkAdjusted.IsChecked = true;
                else if (bills[0].isAdjustedTax == false)
                    chkNonAdjusted.IsChecked = true;


                if (bills[0].LoansAdvanceId != null && bills[0].LoansAdvanceId != 0)
                {
                    loansAdvanceId = bills[0].LoansAdvanceId.Value;
                    AdjustmentFieldVisibility();
                    mbtnAddAdjustment.Visibility = Visibility.Visible;
                }

                if (bills[0].Creator != null)
                    txtCreator.Text = bills[0].Creator.person.FName + " " + bills[0].Creator.person.LName;

                if (bills[0].isApproved == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Admin Bills with Value") == null)
                    {
                        txtMER.IsReadOnly = true;
                        ucBillDataRow.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].PendingForClosing == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Pending for Closing Admin Bills with Value") == null)
                    {
                        txtMER.IsReadOnly = true;
                        ucBillDataRow.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].BillStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Closed Admin Bills with Value") == null)
                    {
                        txtMER.IsReadOnly = true;
                        ucBillDataRow.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].isApproved == true && bills[0].PendingForClosing != true && bills[0].BillStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Open Admin Bills with Value") == null)
                    {
                        txtMER.IsReadOnly = true;
                        ucBillDataRow.txtAmountOC.IsReadOnly = true;
                    }
                }

                if (bills[0].BillStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Admin Bill") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Admin Bill Statuses") == null)
                    {
                        cmbBillStatus.IsEnabled = false;
                    }
                }
                if (bills[0].GLPostingDate != null)
                {
                    datglPostingdate.EditValue = bills[0].GLPostingDate;
                }
                else
                {
                    datglPostingdate.EditValue = bills[0].CreationDate;
                }


                var creditJournalTransactions = bills[0].journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = bills[0].journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }


                else if (bills[0].BillStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Admin Bill") == null)
                    {
                        btnSave.IsEnabled = false;
                    }
                }



                if (bills[0].isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (bills[0].isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bills[0].isApproved == true && bills[0].stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bills[0].isApproved == true && bills[0].BillStatus.isActive == false && bills[0].PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (bills[0].isApproved == true && bills[0].PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bills[0].isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bills[0].isApproved == false)
                {
                    //lblStage.Text = "Under Approval";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (bills[0].PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }

                isApproved = bills[0].isApproved;

               
                
                loadonAdminBilldata();



                int index = 0;



                if (bills[0].CreationDate != null)
                    datCreationDate.DateTime = (DateTime)bills[0].CreationDate;

                //Select Template
                if (bills[0].template_Id != null && bills[0].template != null)
                {
                    var templateList = (List<cmbitem>)cmbxBillTemplate.Items.SourceCollection;
                    var term = templateList.Find(x => x.id == bills[0].template_Id);
                    if (term == null)
                    {
                        templateList.Add(new cmbitem() { name = bills[0].template.Name, id = bills[0].template.Id });
                        cmbxBillTemplate.ItemsSource = null;
                        cmbxBillTemplate.ItemsSource = templateList;
                    }

                    cmbxBillTemplate.SelectedItem = cmbxBillTemplate.Items[cmbxBillTemplate.Items.IndexOf(templateList.Find(x => x.id == bills[0].template_Id))];
                }

                //Select Company
                if (bills[0].company_Id != null || bills[0].company != null)
                {
                    var companylist = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                    if (bills[0].company != null && companylist.Find(x => x.Id == bills[0].company_Id) == null)
                    {
                        companylist.Add(bills[0].company);
                        cmbxCompany.ItemsSource = null;
                        cmbxCompany.ItemsSource = companylist;
                    }

                    cmbxCompany.EditValue = bills[0].company_Id;
                }

                //Select Company
                if (bills[0].dept_Id != null || bills[0].department != null)
                {
                    var deptList = (cmbxDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : cmbxDepartment.ItemsSource as List<Department>;
                    if (bills[0].department != null && deptList.Find(x => x.Id == bills[0].dept_Id) == null)
                    {
                        deptList.Add(bills[0].department);
                        cmbxCompany.ItemsSource = null;
                        cmbxDepartment.ItemsSource = deptList;
                    }

                    cmbxDepartment.EditValue = bills[0].dept_Id;
                }

                //Select Employee
                if (bills[0].employee != null)
                {
                    cmbxEmployee.EditValue = bills[0].emp_Id;
                }

                if (bills[0].BillStatus != null)
                {
                    var disAbleStatus = BillStatuses.FirstOrDefault(x => x.Id == bills[0].BillStatus.Id);
                    if (disAbleStatus == null)
                    {
                        loadBillStatus(bills[0].BillStatus);
                    }
                }
                //Select Status
                if (bills[0].BillStatus != null && bills[0].statusId != null)
                {
                    var statusList = (List<cmbitem>)cmbBillStatus.Items.SourceCollection;
                    var term = statusList.Find(x => x.id == bills[0].statusId);
                    if (term == null)
                    {
                        statusList.Add(new cmbitem() { name = bills[0].BillStatus.Status, id = bills[0].BillStatus.Id });
                        cmbBillStatus.ItemsSource = null;
                        cmbBillStatus.ItemsSource = statusList;
                    }

                    cmbBillStatus.SelectedItem = cmbBillStatus.Items[cmbBillStatus.Items.IndexOf(statusList.Find(x => x.id == bills[0].statusId))];
                    checkStatus = bills[0].BillStatus;
                }

                txtSystemRefNo.Text = bills[0].SystemRefNo;
                lblBillRefNo.Text = " (" + bills[0].SystemRefNo + ")";
                txtFinanceRefNo.Text = bills[0].FinanceRefNo;
                txtFinanceRefNo2.Text = bills[0].FinanceRefNo2;


                datTransactionDate.Text = ((DateTime)bills[0].TransactionDate).ToString("dd/MMM/yyyy");


                //Bill ref no
                if (bills[0].BillRefNoId != null && bills[0].BillRefNo != null)
                {
                    var billRefList = (List<cmbitem>)cmbxBillRef.Items.SourceCollection;
                    var term = billRefList.Find(x => x.id == bills[0].BillRefNoId);
                    if (term == null)
                    {
                        billRefList.Add(new cmbitem() { name = bills[0].BillRefNo.BillReferenceNo, id = bills[0].BillRefNo.Id });
                        cmbxBillRef.ItemsSource = null;
                        cmbxBillRef.ItemsSource = billRefList;
                    }

                    cmbxBillRef.SelectedItem = cmbxBillRef.Items[cmbxBillRef.Items.IndexOf(billRefList.Find(x => x.id == bills[0].BillRefNoId))];
                }

                //Select Currency
                if (bills[0].currency != null)
                {
                    cmbxCurrency.EditValue = bills[0].currency_Id;
                }

                if (bills[0].bank != null)
                    lookupBank.EditValue = bills[0].BankId;

                //Select Transfer Type
                for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
                {
                    if (((ERP_BL.Enums.CardHolderType)i).ToString() == bills[0].CardHolderType.ToString())
                    {
                        cmbxHolderType.SelectedIndex = i;
                        break;
                    }
                }

                //Select Primary Card No
                if (bills[0].PrimaryCreditCardNoId != null && bills[0].primaryCreditCardNo != null)
                {
                    var primaryCardList = (List<cmbitem>)cmbxPrimaryCardNo.Items.SourceCollection;
                    var term = primaryCardList.Find(x => x.id == bills[0].PrimaryCreditCardNoId);
                    if (term == null)
                    {
                        primaryCardList.Add(new cmbitem() { name = bills[0].primaryCreditCardNo.CardNumber, id = bills[0].primaryCreditCardNo.Id });
                        cmbxPrimaryCardNo.ItemsSource = null;
                        cmbxPrimaryCardNo.ItemsSource = primaryCardList;
                    }

                    cmbxPrimaryCardNo.SelectedItem = cmbxPrimaryCardNo.Items[cmbxPrimaryCardNo.Items.IndexOf(primaryCardList.Find(x => x.id == bills[0].PrimaryCreditCardNoId))];
                }

                //Select Card User
                var secondaryCardList = (cmbxSecondaryCardNo.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxSecondaryCardNo.ItemsSource as List<cmbitem>;
                if (bills[0].primaryCreditCardNo != null)
                {
                    index = 0;
                    foreach (var _cardNo in secondaryCardList)
                    {
                        if (_cardNo.id == bills[0].SecondaryCreditCardNoId)
                        {
                            cmbxSecondaryCardNo.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (bills[0].EmployeeForEveryBill == true)
                {
                    chkEmployee.IsChecked = true;
                }

                //Select Management Summary
                //if (bills[0].managementSummary != null)
                //{
                //    chkManagementSummary.IsChecked = true;
                //    lookUpManagementSummary.EditValue = bills[0].managementSummary_Id;
                //}
                    

                if (bills[0].isDeposit == true)
                {
                    btnDeposit.IsChecked = true;
                    if (bills[0].isAmountOC == true)
                        chkAmountOC.IsChecked = true;
                    else if (bills[0].isAmountOC == false)
                        chkAmountMER.IsChecked = true;
                }

                else if (bills[0].isDeposit == false)
                {
                    btnPayment.IsChecked = true;
                    if (bills[0].isAmountOC == true)
                        chkAmountOC.IsChecked = true;
                    else if (bills[0].isAmountOC == false)
                        chkAmountMER.IsChecked = true;
                }





                if (bills[0].isVATBookPosted == true)
                {
                    btnVATBookPost.IsChecked = true;
                  if(bills[0].VATBookRefNumber!=null)
                    {
                        var vatBookList = (cmbxVATBookRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxVATBookRef.ItemsSource as List<cmbitem>;
                            index = 0;
                            foreach (var _vatBookNo in vatBookList)
                            {
                                if (_vatBookNo.id == bills[0].VATBookRefId)
                                {
                                    cmbxVATBookRef.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }
                        
                    }
                }
                txtMER.Text = bills[0].MER.ToString();


                //----------------------------------------------------

                int countBills = bills.Count;
                if (countBills > 1)
                {
                    for (int i = 1; i < countBills; i++)
                        AddRow();
                }

                if (bills[0].isVehicleType == true)
                    chkVehicleType.IsChecked = true;
                else if (bills[0].isVehicleType == false)
                    chkVehicleType.IsChecked = false;

                int billsCounter = 0;

                double totalAmountOc = 0;
                double totalAmountMer = 0;
                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            billsCounter++;
                            var billElement = (ucBillData)_bill;

                            (billElement.FindName("txtId") as TextBlock).Text = bills[billsCounter - 1].Id.ToString();
                            (billElement.FindName("txtGroupId") as TextBlock).Text = bills[billsCounter - 1].transactionGroupId.ToString();

                            if (loansAdvanceId != 0 && bills[billsCounter - 1].Adjustments != null)
                            {

                                var aprvdAdjustments = bills[billsCounter - 1].Adjustments.Where(x => x.isApproved == true);
                                var unaprvdAdjustments = bills[billsCounter - 1].Adjustments.Where(x => x.isApproved == false);

                                billElement.adjustments = bills[billsCounter - 1].Adjustments;
                                billElement.txtApprovedLoanAdjustment.Text = aprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                                billElement.txtUnApprovedLoanAdjustment.Text = unaprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                                billElement.txtApprovedAdjustmentCount.Text = aprvdAdjustments.Count().ToString();
                                billElement.txtUnApprovedAdjustmentCount.Text = unaprvdAdjustments.Count().ToString();
                            }

                            //Select COA
                            var coaList = (billElement.FindName("cmbxCOA") as LookUpEdit).ItemsSource as List<ChartofAccount>;
                            if (bills[billsCounter - 1].chartofAccount != null && coaList != null)
                            {
                                if(coaList.FirstOrDefault(x=>x.Id == bills[billsCounter - 1].COA_Id) == null)
                                {
                                    coaList.Add(bills[billsCounter - 1].chartofAccount);

                                    (billElement.FindName("cmbxCOA") as LookUpEdit).ItemsSource = null;
                                    (billElement.FindName("cmbxCOA") as LookUpEdit).ItemsSource = coaList;
                                }
                                (billElement.FindName("cmbxCOA") as LookUpEdit).EditValue = bills[billsCounter - 1].COA_Id;
                            }

                            //Select COA
                            var coaCreditList = (billElement.FindName("cmbxCOAcredit") as LookUpEdit).ItemsSource as List<ChartofAccount>;
                            if (bills[billsCounter - 1].chartofAccountCredit != null && coaCreditList != null)
                            {
                                if (coaCreditList.FirstOrDefault(x => x.Id == bills[billsCounter - 1].CoaCredit_Id) == null)
                                {
                                    coaCreditList.Add(bills[billsCounter - 1].chartofAccountCredit);

                                    (billElement.FindName("cmbxCOAcredit") as LookUpEdit).ItemsSource = null;
                                    (billElement.FindName("cmbxCOAcredit") as LookUpEdit).ItemsSource = coaCreditList;
                                }
                                (billElement.FindName("cmbxCOAcredit") as LookUpEdit).EditValue = bills[billsCounter - 1].CoaCredit_Id;
                            }

                            //Select Admin Bill Type
                            if (bills[billsCounter - 1].AdminBillType != null)
                            {
                                (billElement.FindName("lookupAdminBillType") as LookUpEdit).EditValue = bills[billsCounter - 1].adminBillType_Id;
                            }

                            //Select Vendor 
                            if (bills[billsCounter - 1].vendor != null)
                            {
                                (billElement.FindName("cmbxVendor") as LookUpEdit).EditValue = bills[billsCounter - 1].vendor_Id;
                            }
                            //}


                            //Select Payee Name 
                            //var payeeNameList = (billElement.FindName("cmbxPayeeName") as LookUpEdit).ItemsSource as List<Payee>;
                            if (bills[billsCounter - 1].payee != null /*&& payeeNameList != null*/)
                            {
                                var payeeList = ((billElement.FindName("cmbxPayeeName") as LookUpEdit).ItemsSource as List<Payee>) == null ? new List<Payee>() : (billElement.FindName("cmbxPayeeName") as LookUpEdit).ItemsSource as List<Payee>;
                                if (bills[billsCounter - 1].payee != null && payeeList.Find(x => x.Id == bills[billsCounter - 1].payee_Id) == null)
                                {
                                    payeeList.Add(bills[billsCounter - 1].payee);
                                    (billElement.FindName("cmbxPayeeName") as LookUpEdit).ItemsSource = null;
                                    (billElement.FindName("cmbxPayeeName") as LookUpEdit).ItemsSource = payeeList;
                                }

                                (billElement.FindName("cmbxPayeeName") as LookUpEdit).EditValue = bills[billsCounter - 1].payee_Id;
                            }

                            //Select Admin Bill Nature
                            //var adminBillNatureList = (billElement.FindName("lookupAdminBillNature") as LookUpEdit).ItemsSource as List<AdminBillNature>;
                            if (bills[billsCounter - 1].adminBillNature_Id != null)
                            {
                                (billElement.FindName("lookupAdminBillNature") as LookUpEdit).EditValue = bills[billsCounter - 1].adminBillNature_Id;
                            }

                            if (chkEmployee.IsChecked == true)
                            {
                                //Select Employee for every bill 
                                //var employeeList = (billElement.FindName("cmbxEmployee") as LookUpEdit).ItemsSource as List<ERP_BL.Databases.Employee>;
                                if (bills[billsCounter - 1].employeeForBill != null /*&& employeeList != null*/)
                                {
                                    (billElement.FindName("cmbxEmployee") as LookUpEdit).EditValue = bills[billsCounter - 1].employeeForBill_Id;
                                }
                            }

                            if (bills[billsCounter - 1].Bill_Number != null)
                                (billElement.FindName("txtBillNo") as TextEdit).Text = bills[billsCounter - 1].Bill_Number;

                            if (bills[billsCounter - 1].BillingMonthFrom != null)
                                (billElement.FindName("datBillingMonth") as DateEdit).Text = ((DateTime)bills[billsCounter - 1].BillingMonthFrom).ToString("MMM yyyy");


                            if (bills[billsCounter - 1].DueDate != null)
                                (billElement.FindName("datDueDate") as DateEdit).Text = ((DateTime)bills[billsCounter - 1].DueDate).ToString("dd MMM yyyy");

                            (billElement.FindName("txtAmountOC") as TextEdit).Text = Math.Round(bills[billsCounter - 1].AmountOC, 2).ToString();
                            totalAmountOc = Math.Round(totalAmountOc + bills[billsCounter - 1].AmountOC, 2);

                            if (bills[billsCounter - 1].hasWAT == true)
                            {
                                (billElement.FindName("chkVat") as CheckEdit).IsChecked = true;
                                var vatList = (billElement.lookupVat.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : billElement.lookupVat.ItemsSource as List<TaxName>;
                                if (bills[billsCounter - 1].tax != null)
                                {
                                    if (vatList.Find(x => x.Id == bills[billsCounter - 1].tax.Id) == null)
                                    {
                                        vatList.Add(bills[billsCounter - 1].tax);
                                        billElement.lookupVat.ItemsSource = null;
                                        billElement.lookupVat.ItemsSource = vatList;
                                    }
                                    (billElement.FindName("lookupVat") as LookUpEdit).EditValue = bills[billsCounter - 1].tax.Id;
                                }
                                (billElement.FindName("txtTaxAmount") as TextEdit).Text = Math.Round(bills[billsCounter - 1].TaxAmount, 2).ToString();
                                (billElement.FindName("txtAmountWithTax") as TextEdit).Text = Math.Round(bills[billsCounter - 1].AmountWithTax, 2).ToString();
                            }

                            (billElement.FindName("txtAmountMER") as TextEdit).Text = bills[billsCounter - 1].AmountMER.ToString();
                            totalAmountMer = totalAmountMer + bills[billsCounter - 1].AmountMER;

                            if (bills[billsCounter - 1].Payments == null || bills[billsCounter - 1].Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                txtPaymentsClosed.Visibility = Visibility.Collapsed;
                                var adjustmentAmount = bills[billsCounter - 1].Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                                if (bills[billsCounter - 1].AmountWithTax != 0)
                                    (billElement.FindName("txtRemainingAmountOC") as TextEdit).Text = (bills[billsCounter - 1].AmountWithTax - adjustmentAmount).ToString();
                                else
                                    (billElement.FindName("txtRemainingAmountOC") as TextEdit).Text = (bills[billsCounter - 1].AmountOC - adjustmentAmount).ToString();

                                (billElement.FindName("txtAmountPaidOC") as TextEdit).Text = "0";
                                (billElement.FindName("txtFullyPaid") as TextBlock).Text = "0% Paid";
                                (billElement.FindName("txtFullyPaid") as TextBlock).Foreground = new SolidColorBrush(Colors.Red);
                                //(billElement.FindName("pBarPayment") as ProgressBar).Value = 0;
                            }
                            else
                            {
                                var amountPaid = bills[billsCounter - 1].Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount) /*+ bills[billsCounter - 1].Payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
                                double remainingAmount = 0;
                                var adjustmentAmount = bills[billsCounter - 1].Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                                if (bills[billsCounter - 1].AmountWithTax != 0)
                                    remainingAmount = Math.Round(bills[billsCounter - 1].AmountWithTax, 2) - (amountPaid + adjustmentAmount);
                                else
                                    remainingAmount = bills[billsCounter - 1].AmountOC - (amountPaid + adjustmentAmount);

                                (billElement.FindName("txtAmountPaidOC") as TextEdit).Text = amountPaid.ToString();
                                (billElement.FindName("txtRemainingAmountOC") as TextEdit).Text = remainingAmount.ToString();

                                if (remainingAmount == 0)
                                {
                                    (billElement.FindName("txtFullyPaid") as TextBlock).Text = "100% Paid";
                                    (billElement.FindName("txtFullyPaid") as TextBlock).Foreground = new SolidColorBrush(Colors.Green);

                                    if (bills[billsCounter - 1].Payments.Where(x => x.Status.isActive == true && x.isVoid != true).Count() > 0)
                                    {
                                        billElement.Background = new SolidColorBrush(Colors.Green);
                                        billElement.Foreground = new SolidColorBrush(Colors.Green);
                                        txtPaymentsClosed.Visibility = Visibility.Collapsed;
                                    }
                                    else
                                    {
                                        billElement.Background = new SolidColorBrush(Colors.DarkRed);
                                        billElement.Foreground = new SolidColorBrush(Colors.DarkRed);
                                    }

                                    //(billElement.FindName("pBarPayment") as ProgressBar).Value = 100;
                                    //(billElement.FindName("pBarPayment") as ProgressBar).Foreground = new SolidColorBrush(Colors.Green);
                                }
                                else if (Math.Abs(remainingAmount) > 0)
                                {
                                    double percentage = 0;
                                    if (bills[billsCounter - 1].AmountWithTax != 0)
                                        percentage = Math.Round(((amountPaid / bills[billsCounter - 1].AmountWithTax) * 100), 2);
                                    else
                                        percentage = Math.Round(((amountPaid / bills[billsCounter - 1].AmountOC) * 100), 2);

                                    (billElement.FindName("txtFullyPaid") as TextBlock).Text = percentage + "% Paid";
                                    (billElement.FindName("txtFullyPaid") as TextBlock).Foreground = new SolidColorBrush(Colors.Blue);
                                    txtPaymentsClosed.Visibility = Visibility.Collapsed;
                                    //(billElement.FindName("pBarPayment") as ProgressBar).Value = percentage;
                                    //(billElement.FindName("pBarPayment") as ProgressBar).Foreground = new SolidColorBrush(Colors.Blue);
                                }
                            }

                            if (bills[billsCounter - 1].managementSummary != null)
                                (billElement.FindName("txtManagementSummaryId") as TextEdit).Text = bills[billsCounter - 1].managementSummary_Id.ToString();

                            if (!String.IsNullOrEmpty(bills[billsCounter - 1].Memo))
                                (billElement.FindName("txtMemo") as TextEdit).Text = bills[billsCounter - 1].Memo.ToString();

                            if (!String.IsNullOrEmpty(bills[billsCounter - 1].SummaryMemo))
                                (billElement.FindName("txtSummaryMemo") as TextEdit).Text = bills[billsCounter - 1].SummaryMemo.ToString();

                        }
                    }
                }

                txtTotalAmountOC.Text = totalAmountOc.ToString();
                txtTotalAmountMER.Text = totalAmountMer.ToString();

                if (bills.Count > 0)
                {
                    stage = bills[0].stage;
                    isApproved = bills[0].isApproved;
                    approvalDate = bills[0].ApprovedDate;
                }

            }
            if (editFlag == false)
            {
                btnPushCredits.IsChecked = true;
                btnPushDebits.IsChecked = true;
                datglPostingdate.EditValue = DateTime.Now;
            }

        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //(grdCntrlBillData.CurrentItem as Vendor).company = new Company();
        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastReceipt = BillsRepo.GetLastBill();
            if (lastReceipt == 0)
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
                var lastId = lastReceipt/*.transactionGroupId*/;
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
                    txtSystemRefNo.Text = "Bill-" + intGroupId;
                    lblBillRefNo.Text = " (Bill-" + intGroupId + ")";
                    bills = new List<AdminBill>();
                }

                if (cmbxBillTemplate.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bill template!");
                    cmbxBillTemplate.Focus();
                    return;
                }
                if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company!");
                    cmbxCompany.Focus();
                    return;
                }
                if (cmbxDepartment.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Department!");
                    cmbxDepartment.Focus();
                    return;
                }
                if (cmbxEmployee.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Employee!");
                    cmbxBillTemplate.Focus();
                    return;
                }
                if (cmbBillStatus.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bill status!");
                    cmbBillStatus.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtSystemRefNo.Text))
                {
                    DXMessageBox.Show("Please enter System Ref number!");
                    txtSystemRefNo.Focus();
                    return;
                }
               
                if (String.IsNullOrEmpty(datTransactionDate.Text))
                {
                    DXMessageBox.Show("Please enter transaction date!");
                    datTransactionDate.Focus();
                    return;
                }
                if (cmbxCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency!");
                    cmbxCurrency.Focus();
                    return;
                }

                if (lookupBank.SelectedIndex > -1 && cmbxHolderType.SelectedIndex > -1)
                {
                    if(cmbxHolderType.SelectedIndex == 0)
                    {
                        if (cmbxPrimaryCardNo.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please select Primary Card #!");
                            cmbxPrimaryCardNo.Focus();
                            return;
                        }
                            
                    }
                    else if (cmbxHolderType.SelectedIndex == 1)
                    {
                        if (cmbxSecondaryCardNo.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please select Primary Card #!");
                            cmbxSecondaryCardNo.Focus();
                            return;
                        }
                            
                    }
                   
                }

                if(chkManagementSummary.IsChecked == true)
                {
                    if( lookUpManagementSummary.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Management Summary!");
                        lookUpManagementSummary.Focus();
                        return;
                    }
                }

                int billsCounter = 0;

                if (editFlag == true && bills.Count > 0)
                {
                    addedBills = new List<AdminBill>();
                    intGroupId = bills[0].transactionGroupId;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null && bills[0].isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Admin Bill is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            stage = TransactionStage.Approved.ToString();
                            isApproved = true;
                            approvalDate = System.DateTime.Now;
                        }
                    }
                }

                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            billsCounter++;
                            var billElement = (ucBillData)_bill;

                            if (billElement.lookupAdminBillType.SelectedIndex < 0)
                            {
                                DXMessageBox.Show("Please select Admin Bill Type!", "Bill #" + billsCounter.ToString());
                                billElement.lookupAdminBillType.Focus();
                                return;
                            }
                            if (billElement.cmbxVendor.SelectedIndex < 0)
                            {
                                DXMessageBox.Show("Please select Vendor!", "Bill #" + billsCounter.ToString());
                                billElement.cmbxVendor.Focus();
                                return;
                            }
                            if (billElement.cmbxCOA.SelectedIndex < 0)
                            {
                                DXMessageBox.Show("Please select COA Debit!", "Bill #" + billsCounter.ToString());
                                billElement.cmbxCOA.Focus();
                                return;
                            }
                            if (String.IsNullOrEmpty(billElement.txtBillNo.Text))
                            {
                                DXMessageBox.Show("Please enter Bill Number!", "Bill #" + billsCounter.ToString());
                                billElement.txtBillNo.Focus();
                                return;
                            }
                            if (String.IsNullOrEmpty(billElement.datBillingMonth.Text))
                            {
                                DXMessageBox.Show("Please enter Billing Month!", "Bill #" + billsCounter.ToString());
                                billElement.datBillingMonth.Focus();
                                return;
                            }
                            if (String.IsNullOrEmpty(billElement.datDueDate.Text))
                            {
                                DXMessageBox.Show("Please enter Due date!", "Bill #" + billsCounter.ToString());
                                billElement.datDueDate.Focus();
                                return;
                            }
                            if (Convert.ToDouble(billElement.txtAmountOC.Text) == 0)
                            {
                                DXMessageBox.Show("Please enter Amount OC!", "Bill #" + billsCounter.ToString());
                                billElement.txtAmountOC.Focus();
                                return;
                            }
                            if (billElement.lookupAdminBillNature.SelectedIndex < 0)
                            {
                                DXMessageBox.Show("Please select Admin Bill Nature!", "Bill #" + billsCounter.ToString());
                                billElement.lookupAdminBillNature.Focus();
                                return;
                            }

                            AdminBill bill = new AdminBill();
                            if(editFlag == true)
                            {
                                if (Convert.ToInt32(billElement.txtId.Text) > 0)
                                    bill = bills[bills.IndexOf(bills.FirstOrDefault(x => x.Id == Convert.ToInt32(billElement.txtId.Text)))];
                            }

                            if(billTypes == AdminBillTypes.Asset)
                            {
                                bill.billTypes = billTypes;
                                bill.assetRentalId = assetRentalId;
                            }

                            bill.isProgressiveCost = isProgressiveCost;

                            bill.isVehicleType = chkVehicleType.IsChecked.Value;
                            bill.template_Id = (cmbxBillTemplate.SelectedItem as cmbitem).id;
                            bill.CreationDate = datCreationDate.DateTime;
                            bill.GLPostingDate = datglPostingdate.DateTime;
                            bill.TransactionDate = datTransactionDate.DateTime;
                            bill.company_Id = (cmbxCompany.SelectedItem as Company).Id;
                            bill.dept_Id = (cmbxDepartment.SelectedItem as Department).Id;
                            bill.emp_Id = (cmbxEmployee.SelectedItem as ERP_BL.Databases.Employee).EmpId;

                            if(editFlag == false)
                            {
                                bill.creatorId = SYSTEM_STATIC.currentUser.employeeId;
                                bill.user_Id = SYSTEM_STATIC.currentUser.id;
                                if(loansAdvanceId != 0)
                                {
                                    bill.LoansAdvanceId = loansAdvanceId;
                                }
                            }

                            if (lookupBank.SelectedIndex > -1 && cmbxHolderType.SelectedIndex > -1)
                            {
                                if (cmbxHolderType.SelectedIndex == 0)
                                {
                                    bill.BankId = (lookupBank.SelectedItem as Bank).Id;
                                    bill.CardHolderType =((CardHolderType)cmbxHolderType.SelectedIndex);
                                    bill.PrimaryCreditCardNoId = (cmbxPrimaryCardNo.SelectedItem as cmbitem).id;

                                    bill.SecondaryCreditCardNoId = null;
                                }
                                else if (cmbxHolderType.SelectedIndex == 1)
                                {
                                    bill.BankId = (lookupBank.SelectedItem as Bank).Id;
                                    bill.CardHolderType = ((CardHolderType)cmbxHolderType.SelectedIndex);
                                    bill.SecondaryCreditCardNoId = (cmbxSecondaryCardNo.SelectedItem as cmbitem).id;

                                    if(cmbxPrimaryCardNo.SelectedIndex > -1)
                                        bill.PrimaryCreditCardNoId = (cmbxPrimaryCardNo.SelectedItem as cmbitem).id;
                                }

                            }
                            else
                            {
                                bill.BankId = null;
                                bill.CardHolderType = null;
                                bill.PrimaryCreditCardNoId = null;

                                bill.SecondaryCreditCardNoId = null;
                            }


                            //Receipt Asset Status 
                            if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                            {
                                bill.statusId = (cmbBillStatus.SelectedItem as cmbitem).id;
                            }
                            bill.SystemRefNo = txtSystemRefNo.Text;


                            if (!String.IsNullOrEmpty(txtFinanceRefNo.Text))
                                bill.FinanceRefNo = txtFinanceRefNo.Text;
                            else
                                bill.FinanceRefNo = null;

                            if (!String.IsNullOrEmpty(txtFinanceRefNo2.Text))
                                bill.FinanceRefNo2 = txtFinanceRefNo2.Text;
                            else
                                bill.FinanceRefNo2 = null;

                            bill.currency_Id = (cmbxCurrency.SelectedItem as Currency).Id;

                            if (cmbxBillRef.SelectedIndex > 0)
                            {
                                bill.BillRefNoId = (cmbxBillRef.SelectedItem as cmbitem).id;
                            }
                            else
                            {
                                bill.BillRefNoId = null;
                            }

                            bill.MER = Convert.ToDouble(txtMER.Text);

                            if (billElement.cmbxCOA.SelectedIndex > -1)
                                bill.COA_Id = (billElement.cmbxCOA.SelectedItem as ChartofAccount).Id;

                            if (billElement.cmbxCOAcredit.SelectedIndex > -1)
                                bill.CoaCredit_Id = (billElement.cmbxCOAcredit.SelectedItem as ChartofAccount).Id;

                            bill.adminBillType_Id = (billElement.lookupAdminBillType.SelectedItem as AdminBillType).Id;
                            bill.vendor_Id = (billElement.cmbxVendor.SelectedItem as Vendor).Id;

                            if(billElement.cmbxPayeeName.SelectedIndex > -1)
                            {
                                bill.payee_Id = (billElement.cmbxPayeeName.SelectedItem as Payee).Id;
                            }
                            else
                            {
                                bill.payee_Id = null;
                            }


                            if(chkEmployee.IsChecked == true)
                            {
                                bill.EmployeeForEveryBill = true;
                                if(billElement.cmbxEmployee.SelectedIndex > -1)
                                    bill.employeeForBill_Id = (billElement.cmbxEmployee.SelectedItem as ERP_BL.Databases.Employee).EmpId;

                            }
                            else
                            {
                                bill.EmployeeForEveryBill = false;
                                bill.employeeForBill_Id = null;
                            }
                           
                            if (Convert.ToInt32(billElement.txtManagementSummaryId.Text) > 0)
                            {
                                bill.hasSummary = true;
                                bill.managementSummary_Id = Convert.ToInt32(billElement.txtManagementSummaryId.Text);
                                if (!String.IsNullOrEmpty(billElement.txtSummaryMemo.Text))
                                    bill.SummaryMemo = billElement.txtSummaryMemo.Text;
                            }
                            else
                            {
                                bill.hasSummary = false;
                                bill.managementSummary_Id = null;
                                bill.SummaryMemo = null;
                            }

                            bill.BillingMonthFrom = billElement.datBillingMonth.DateTime;
                            bill.DueDate = billElement.datDueDate.DateTime;
                            bill.AmountOC = Convert.ToDouble(billElement.txtAmountOC.Text);

                            if(loansAdvanceId != 0 && billElement.adjustments != null)
                            {
                                bill.Adjustments = billElement.adjustments;
                                //foreach(var _adj in billElement.adjustments)
                                //{
                                //    if(!bill.Adjustments.Contains(_adj))
                                //        bill.Adjustments.Add(_adj);
                                //}
                                
                            }


                            bill.AmountMER = Convert.ToDouble(billElement.txtAmountMER.Text);
                            bill.TotalAmountOC = Convert.ToDouble(txtTotalAmountOC.Text);
                            bill.Bill_Number = billElement.txtBillNo.Text;

                            //bill.transactionGroupId = intGroupId;

                            bill.Memo = billElement.txtMemo.Text;

                            if(billElement.lookupAdminBillNature.SelectedIndex > -1)
                                bill.adminBillNature_Id = (billElement.lookupAdminBillNature.SelectedItem as AdminBillNature).Id;

                            if (billElement.cmbxCOA.SelectedIndex > -1)
                            {
                                if (billElement.cmbxCOAcredit.SelectedIndex < 0)
                                {
                                    DXMessageBox.Show("Please Select COA Credit!", "Bill #" + billsCounter.ToString());
                                    billElement.cmbxCOAcredit.Focus();
                                    return;
                                }
                                
                                if (!string.IsNullOrEmpty(billElement.txtId.Text))
                                {
                                    bill.journalTransactions = getJournalTransactions(Convert.ToInt32(billElement.txtId.Text), bill.MER, bill.Memo, SYSTEM_STATIC.currentUser.id, bill.AmountOC, (billElement.cmbxCOA.SelectedItem as ChartofAccount).Id, txtFinanceRefNo.Text, department.Id, (billElement.cmbxCOAcredit.SelectedItem as ChartofAccount).Id, (DateTime)datglPostingdate.EditValue);
                                }
                                else
                                {
                                    bill.journalTransactions = getJournalTransactions(0, bill.MER, bill.Memo, SYSTEM_STATIC.currentUser.id, bill.AmountOC, (billElement.cmbxCOA.SelectedItem as ChartofAccount).Id, txtFinanceRefNo.Text, department.Id, (billElement.cmbxCOAcredit.SelectedItem as ChartofAccount).Id, (DateTime)datglPostingdate.EditValue);
                                }                                                                

                            }

                            if (chkAdjusted.IsChecked == true)
                                bill.isAdjustedTax = true;
                            else if (chkNonAdjusted.IsChecked == true)
                                bill.isAdjustedTax = false;
                            else
                                bill.isAdjustedTax = null;

                            if (billElement.chkVat.IsChecked == true)
                            {

                                if (billElement.lookupVat.SelectedIndex < 0)
                                {
                                    DXMessageBox.Show("Select Tax in Bill # " + billsCounter.ToString());
                                    billElement.lookupVat.Focus();
                                    return;
                                }
                                bill.hasWAT = true;
                                bill.tax_Id = (billElement.lookupVat.SelectedItem as TaxName).Id;
                                bill.TaxAmount = Convert.ToDouble(billElement.txtTaxAmount.Text);
                                if (bill.journalTransactions != null)
                                {
                                    if (btnPushDebits.IsChecked == true)
                                    {
                                        var dbTransaction = bill.journalTransactions.FirstOrDefault(x => x.accountId ==
                                                  (billElement.lookupVat.SelectedItem as TaxName).COA_Id &&
                                                  x.debit == bill.TaxAmount &&
                                                  x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                                  x.deptId == department.Id
                                                  );

                                        if (dbTransaction == null)
                                        {
                                            double total = 0;
                                            if (
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Loan ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Equity ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Income ||
                                                   (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Income
                                                   )
                                            {
                                                total = 0- bill.TaxAmount ;
                                            }
                                            else
                                            {
                                                total = bill.TaxAmount - 0;
                                            }
                                            bill.journalTransactions.Add(new JournalTransaction()
                                            {
                                                AdminBillId = bill.Id,
                                                accountId = (billElement.lookupVat.SelectedItem as TaxName).COA_Id,
                                                coaTransactionsType = coaTransactionsType.AdminBill,
                                                creationDate = (DateTime)datglPostingdate.EditValue,
                                                memo = bill.Memo,
                                                transactionRefno = txtFinanceRefNo.Text,
                                                userId = SYSTEM_STATIC.currentUser.id,
                                                debit = bill.TaxAmount,
                                                credit = 0,
                                                total =total,
                                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                deptId = department.Id,
                                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                                
                                            });
                                        }
                                        else
                                        {
                                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                            {
                                                double total = 0;
                                                if (
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Loan ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Equity ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Income ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Income
                                                       )
                                                {
                                                    total = 0 - bill.TaxAmount;
                                                }
                                                else
                                                {
                                                    total = bill.TaxAmount - 0;
                                                }
                                                bill.journalTransactions.Add(new JournalTransaction()
                                                {
                                                    AdminBillId = bill.Id,
                                                    accountId = (billElement.lookupVat.SelectedItem as TaxName).COA_Id,
                                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                                    memo = bill.Memo,
                                                    transactionRefno = txtFinanceRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    debit = bill.TaxAmount,
                                                    credit = 0,
                                                    total = total,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    deptId = department.Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                                    reconcilationDate = null,
                                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                                    ReconcilationId = null,
                                                    isReconciled = false
                                                });
                                            }
                                            else
                                            {
                                                double total = 0;
                                                if (
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Loan ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Equity ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Income ||
                                                       (billElement.lookupVat.SelectedItem as TaxName).chartofAccount.accountType == COA_AccountType.Other_Income
                                                       )
                                                {
                                                    total = 0 - bill.TaxAmount;
                                                }
                                                else
                                                {
                                                    total = bill.TaxAmount - 0;
                                                }
                                                bill.journalTransactions.Add(new JournalTransaction()
                                                {
                                                    AdminBillId = bill.Id,
                                                    accountId = (billElement.lookupVat.SelectedItem as TaxName).COA_Id,
                                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                                    memo = bill.Memo,
                                                    transactionRefno = txtFinanceRefNo.Text,
                                                    userId = SYSTEM_STATIC.currentUser.id,
                                                    debit = bill.TaxAmount,
                                                    credit = 0,
                                                    total = total,
                                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                                    deptId = department.Id,
                                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                                    reconcilationDate = dbTransaction.reconcilationDate,
                                                    reconcilationType = dbTransaction.reconcilationType,
                                                    ReconcilationId = dbTransaction.ReconcilationId,
                                                    isReconciled = dbTransaction.isReconciled,
                                                });
                                            }
                                        }


                                    }
                                }
                                bill.AmountWithTax = Convert.ToDouble(billElement.txtAmountWithTax.Text);
                            }
                            else
                            {
                                bill.hasWAT = false;
                                bill.tax_Id = null;
                                bill.TaxAmount = 0;
                                bill.AmountWithTax = Convert.ToDouble(billElement.txtAmountWithTax.Text);
                            }

                            List<PettyCash> pettyCashes = new List<PettyCash>();
                            if (btnDeposit.IsChecked == true)
                            {
                                pettyCashes.Add(new PettyCash()
                                {
                                    CreationDate = datCreationDate.DateTime,
                                    AdminBillId = string.IsNullOrEmpty(billElement.txtId.Text) == false ? Convert.ToInt32(billElement.txtId.Text) : 0,
                                    TransactionType = TransactionItemType.Admin_Bill,
                                    debit = chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0,
                                    credit = 0,
                                    total = (chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0) - 0,
                                    FinanceRefNo = txtFinanceRefNo.Text,
                                    SystemRefNo = txtSystemRefNo.Text,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    PettyCashRefId = cmbxBillRef.SelectedIndex > -1 ? (cmbxBillRef.SelectedItem as cmbitem).id : (int?)null
                                });
                                bill.isDeposit = true;
                                bill.isAmountOC = chkAmountOC.IsChecked == true ? true : chkAmountMER.IsChecked == true ? false : (bool?)null;
                            }
                            else if (btnPayment.IsChecked == true)
                            {
                                pettyCashes.Add(new PettyCash()
                                {
                                    CreationDate = datCreationDate.DateTime,
                                    AdminBillId = string.IsNullOrEmpty(billElement.txtId.Text) == false ? Convert.ToInt32(billElement.txtId.Text) : 0,
                                    TransactionType = TransactionItemType.Admin_Bill,
                                    debit = 0,
                                    credit = chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0,
                                    total = 0 - (chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0),
                                    FinanceRefNo = txtFinanceRefNo.Text,
                                    SystemRefNo = txtSystemRefNo.Text,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    PettyCashRefId = cmbxBillRef.SelectedIndex > -1 ? (cmbxBillRef.SelectedItem as cmbitem).id : (int?)null
                                });
                                bill.isDeposit = false;
                                bill.isAmountOC = chkAmountOC.IsChecked == true ? true : chkAmountMER.IsChecked == true ? false : (bool?)null;
                            }
                            else
                            {
                                bill.isDeposit = null;
                                bill.isAmountOC = null;
                            }
                            bill.pettyCashes = pettyCashes;
                            if (cmbxVATBookRef.SelectedIndex > -1)
                            {
                                List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
                                if (btnVATBookPost.IsChecked == true)
                                {
                                    vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                    {
                                        CreationDate = datCreationDate.DateTime,
                                        GLPostingDate = datCreationDate.DateTime,
                                        adminBillId = string.IsNullOrEmpty(billElement.txtId.Text) == false ? Convert.ToInt32(billElement.txtId.Text) : 0,
                                        TransactionType = TransactionItemType.Admin_Bill,
                                        debit = bill.TaxAmount,
                                        credit = 0,
                                        total =  bill.TaxAmount - 0,
                                        FinanceRefNo = txtFinanceRefNo.Text,
                                        SystemRefNo = txtSystemRefNo.Text,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                        VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                    });
                                    bill.isVATBookPosted = true;
                                    bill.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                                }
                                //else if (btnVATBookPost.IsChecked == true)
                                //{
                                //    vatBooks.Add(new ERP_BL.VATBook.VATBook()
                                //    {
                                //        CreationDate = datCreationDate.DateTime,
                                //        adminBillId = string.IsNullOrEmpty(billElement.txtId.Text) == false ? Convert.ToInt32(billElement.txtId.Text) : 0,
                                //        TransactionType = TransactionItemType.Admin_Bill,
                                //        debit = 0,
                                //        credit = chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0,
                                //        total = 0 - (chkAmountOC.IsChecked == true ? bill.AmountOC : chkAmountMER.IsChecked == true ? bill.AmountMER : 0),
                                //        FinanceRefNo = txtFinanceRefNo.Text,
                                //        SystemRefNo = txtSystemRefNo.Text,
                                //        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                //        deptId = (cmbxDepartment.SelectedItem as Department).Id,
                                //        companyId = (cmbxCompany.SelectedItem as Company).Id,
                                //        currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                //        VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null
                                //    });
                                //    bill.isVATBookPosted = false;
                                //}
                                else
                                {
                                    bill.isVATBookPosted = null;
                                }
                                bill.VATBooks = vatBooks;
                            }

                            if (editFlag == true)
                            {
                                bill.transactionGroupId = groupId;
                                bill.stage = stage;
                                bill.isApproved = isApproved;
                                bill.ApprovedDate = approvalDate;

                                if (Convert.ToInt32(billElement.txtId.Text) > 0)
                                    bills[bills.IndexOf(bills.FirstOrDefault(x => x.Id == Convert.ToInt32(billElement.txtId.Text)))] = bill;
                                else
                                {
                                    addedBills.Add(bill);
                                }
                                ////index++;
                            }
                            else
                            {
                                bill.transactionGroupId = intGroupId;
                                bill.creatorId = SYSTEM_STATIC.currentUser.employeeId;
                                bill.user_Id = SYSTEM_STATIC.currentUser.id;
                                bill.isApproved = false;
                                bills.Add(bill);
                            }
                        }
                    }
                }
                //BillsRepo = new AdminBillsRepo();

                var newStatus = BillsRepo.GetBillStatus( (cmbBillStatus.SelectedItem as cmbitem).id);
                if (checkStatus.isActive == true && newStatus.isActive == false)
                {
                    foreach (var _billl in bills)
                    {


                        if (_billl.Payments == null || _billl.Payments.Count == 0)
                        {
                            isFullyPaid = false;
                            break;
                        }
                        else if ((_billl.Payments.Sum(x => x.DebitedAmount) - _billl.AmountOC) != 0)
                        {
                            isFullyPaid = false;
                            break;
                        }

                    }

                    if (isFullyPaid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bills without Payments") == null)
                    {
                        DXMessageBox.Show("Permission required to close Unpaid Admin Bills!");
                        return;
                    }

                    if (isFullyPaid == false || (addedBills != null && addedBills.Count > 0))
                    {
                        MessageBoxResult result = DevExpress.Xpf.Core.DXMessageBox.Show("This Admin Bill is not Paid Yet, Do you want to Close it?", "Bill is Unpaid", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                }



                
                if (editFlag == true)
                {
                    BillsRepo.UpdateBillsNew(bills, removedBills, addedBills);

                    var totalAmountOC = bills.Sum(x => x.AmountOC) + addedBills.Sum(x=>x.AmountOC);
                    var selectedStatus = cmbBillStatus.SelectedItem as cmbitem;
                    if (checkStatus.Id != selectedStatus.id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Admin Bill has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), groupId, TransactionItemType.Admin_Bill);
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
                            Comment = "Status of Admin Bill having System Reference: " + txtSystemRefNo.Text + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(groupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, comment.Comment, 0,user.id, "New Comment ", null);
                            }
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, groupId, (int)TransactionItemType.Admin_Bill, "Status Changed from (" + checkStatus.Status + ") to (" + selectedStatus.name + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, groupId, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                    DXMessageBox.Show("Successfully Updated!");
                }
                else if (editFlag == false)
                {
                    BillsRepo.AddBills(bills);
                    DXMessageBox.Show("Successfully Added!");
                }

                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public List<JournalTransaction> getJournalTransactions(int billId, double MER, string Memo, int userId, double AmountOC, int chartofAccountDebitId, string financeRef, int departmentId, int chartofAccountCreditId,DateTime? glDate)
        {
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

            var debitChartofAccount = coaRepo.get(chartofAccountDebitId);
            var creditChartofAccount = coaRepo.get(chartofAccountCreditId);
            double Amount = 0;
            if (AmountOC < 0)
            {
                Amount = AmountOC * (-1);
            }
            else
                Amount = AmountOC;
            if (billId != 0)
            {
                var adminBill = adminBillsRepo.GetBill(billId);
   
                if (debitChartofAccount.Id != 0)
                {
                    if (btnPushDebits.IsChecked == true)
                    {

                        var dbTransaction = adminBill.journalTransactions.FirstOrDefault(x => x.accountId ==
                                             debitChartofAccount.Id &&
                                             x.debit == Amount &&
                                             x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                             x.deptId == department.Id
                                             );



                        if(dbTransaction==null)
                        {
                            double total = 0;
                            if (
                                   debitChartofAccount.accountType == COA_AccountType.Loan ||
                                   debitChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   debitChartofAccount.accountType == COA_AccountType.Equity ||
                                   debitChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   debitChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   debitChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   debitChartofAccount.accountType == COA_AccountType.Income ||
                                   debitChartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0 - Amount ;
                            }
                            else
                            {
                                total = Amount - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                AdminBillId = adminBill.Id,
                                accountId = debitChartofAccount.Id,
                                coaTransactionsType = coaTransactionsType.AdminBill,
                                creationDate = glDate,
                                memo = Memo,
                                transactionRefno = financeRef,
                                userId = userId,
                                debit = Amount,
                                credit = 0,
                                total = total,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                deptId = departmentId,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                            
                            });
                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       debitChartofAccount.accountType == COA_AccountType.Loan ||
                                       debitChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       debitChartofAccount.accountType == COA_AccountType.Equity ||
                                       debitChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       debitChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       debitChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       debitChartofAccount.accountType == COA_AccountType.Income ||
                                       debitChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Amount;
                                }
                                else
                                {
                                    total = Amount - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    AdminBillId = adminBill.Id,
                                    accountId = debitChartofAccount.Id,
                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                    creationDate = glDate,
                                    memo = Memo,
                                    transactionRefno = financeRef,
                                    userId = userId,
                                    debit = Amount,
                                    credit = 0,
                                    total =total,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = departmentId,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    reconcilationDate = null,
                                    ReconcilationId = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                    isReconciled = false
                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       debitChartofAccount.accountType == COA_AccountType.Loan ||
                                       debitChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       debitChartofAccount.accountType == COA_AccountType.Equity ||
                                       debitChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       debitChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       debitChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       debitChartofAccount.accountType == COA_AccountType.Income ||
                                       debitChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Amount;
                                }
                                else
                                {
                                    total = Amount - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    AdminBillId = adminBill.Id,
                                    accountId = debitChartofAccount.Id,
                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                    creationDate = glDate,
                                    memo = Memo,
                                    transactionRefno = financeRef,
                                    userId = userId,
                                    debit = Amount,
                                    credit = 0,
                                    total = total,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = departmentId,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    reconcilationDate = dbTransaction.reconcilationDate,
                                    reconcilationType = dbTransaction.reconcilationType,
                                    ReconcilationId = dbTransaction.ReconcilationId,
                                    isReconciled = dbTransaction.isReconciled,
                                });
                            }
                        }

                       
                    }
                }

                if (creditChartofAccount.Id != 0)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        var dbTransaction = adminBill.journalTransactions.FirstOrDefault(x => x.accountId ==
                                            creditChartofAccount.Id &&
                                             x.credit == Amount &&
                                             x.companyId == (cmbxCompany.SelectedItem as Company).Id &&
                                             x.deptId == department.Id
                                             );
                        if (dbTransaction == null)
                        {
                            double total = 0;
                            if (
                                   creditChartofAccount.accountType == COA_AccountType.Loan ||
                                   creditChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   creditChartofAccount.accountType == COA_AccountType.Equity ||
                                   creditChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   creditChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   creditChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   creditChartofAccount.accountType == COA_AccountType.Income ||
                                   creditChartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = Amount;
                            }
                            else
                            {
                                total = 0 - Amount;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                AdminBillId = adminBill.Id,
                                accountId = creditChartofAccount.Id,
                                coaTransactionsType = coaTransactionsType.AdminBill,
                                creationDate = glDate,
                                memo = Memo,
                                transactionRefno = financeRef,
                                userId = userId,
                                debit = 0,
                                credit = Amount,
                                total = total,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                deptId = departmentId,
                                companyId = (cmbxCompany.SelectedItem as Company).Id,
                                currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                             
                            });
                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       creditChartofAccount.accountType == COA_AccountType.Loan ||
                                       creditChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       creditChartofAccount.accountType == COA_AccountType.Equity ||
                                       creditChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       creditChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       creditChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       creditChartofAccount.accountType == COA_AccountType.Income ||
                                       creditChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Amount;
                                }
                                else
                                {
                                    total = 0 - Amount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    AdminBillId = adminBill.Id,
                                    accountId = creditChartofAccount.Id,
                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                    creationDate = glDate,
                                    memo = Memo,
                                    transactionRefno = financeRef,
                                    userId = userId,
                                    debit = 0,
                                    credit = Amount,
                                    total = total,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = departmentId,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    reconcilationDate = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                    ReconcilationId = null,
                                    isReconciled = false
                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       creditChartofAccount.accountType == COA_AccountType.Loan ||
                                       creditChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       creditChartofAccount.accountType == COA_AccountType.Equity ||
                                       creditChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       creditChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       creditChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       creditChartofAccount.accountType == COA_AccountType.Income ||
                                       creditChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Amount;
                                }
                                else
                                {
                                    total = 0 - Amount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    AdminBillId = adminBill.Id,
                                    accountId = creditChartofAccount.Id,
                                    coaTransactionsType = coaTransactionsType.AdminBill,
                                    creationDate = glDate,
                                    memo = Memo,
                                    transactionRefno = financeRef,
                                    userId = userId,
                                    debit = 0,
                                    credit = Amount,
                                    total = total,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    deptId = departmentId,
                                    companyId = (cmbxCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbxCurrency.SelectedItem as Currency).Id,
                                    reconcilationDate = dbTransaction.reconcilationDate,
                                    reconcilationType = dbTransaction.reconcilationType,
                                    ReconcilationId = dbTransaction.ReconcilationId,
                                    isReconciled = dbTransaction.isReconciled,
                                });
                            }
                        }
                           
                    }
                }

               
            }
            else
            {
                if (debitChartofAccount.Id != 0)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        double total = 0;
                        if (
                               debitChartofAccount.accountType == COA_AccountType.Loan ||
                               debitChartofAccount.accountType == COA_AccountType.Credit_Card ||
                               debitChartofAccount.accountType == COA_AccountType.Equity ||
                               debitChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                               debitChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                               debitChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                               debitChartofAccount.accountType == COA_AccountType.Income ||
                               debitChartofAccount.accountType == COA_AccountType.Other_Income
                               )
                        {
                            total = 0 - Amount;
                        }
                        else
                        {
                            total = Amount - 0;
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            AdminBillId = billId,
                            accountId = debitChartofAccount.Id,
                            coaTransactionsType = coaTransactionsType.AdminBill,
                            creationDate = glDate,
                            memo = Memo,
                            transactionRefno = financeRef,
                            userId = userId,
                            debit = Amount,
                            credit = 0,
                            total = total,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = departmentId,
                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                        });
                    }
                }
                if (creditChartofAccount.Id != 0)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        double total = 0;
                        if (
                               creditChartofAccount.accountType == COA_AccountType.Loan ||
                               creditChartofAccount.accountType == COA_AccountType.Credit_Card ||
                               creditChartofAccount.accountType == COA_AccountType.Equity ||
                               creditChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                               creditChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                               creditChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                               creditChartofAccount.accountType == COA_AccountType.Income ||
                               creditChartofAccount.accountType == COA_AccountType.Other_Income
                               )
                        {
                            total = Amount;
                        }
                        else
                        {
                            total = 0 - Amount;
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            AdminBillId = billId,
                            accountId = creditChartofAccount.Id,
                            coaTransactionsType = coaTransactionsType.AdminBill,
                            creationDate = glDate,
                            memo = Memo,
                            transactionRefno = financeRef,
                            userId = userId,
                            debit = 0,
                            credit = Amount,
                            total = total,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = departmentId,
                            companyId = (cmbxCompany.SelectedItem as Company).Id,
                            currencyId = (cmbxCurrency.SelectedItem as Currency).Id
                        });
                    }
                }
                //}

            }
            return journalTransactions;
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var row = (UserControl)lstBillData.SelectedItem;
            if(row!= null)
            {
                if (editFlag == true)
                {
                        var bill = (ucBillData)row;
                    if(!String.IsNullOrEmpty(bill.txtId.Text))
                    {
                        var _bill = BillsRepo.GetBill(Convert.ToInt32(bill.txtId.Text));
                        if (_bill != null && _bill.Payments != null && _bill.Payments.Where(x=>x.isVoid != true).ToList().Count > 0)
                        {
                            DXMessageBox.Show("This Bill cannot be deleted because it has some active Payments!");
                            return;
                        }
                    }
                    

                        if (!String.IsNullOrEmpty(bill.txtId.Text))
                        {
                            removedBills.Add(bills.FirstOrDefault(x => x.Id == Convert.ToInt32(bill.txtId.Text)));
                            bills.Remove(bills.FirstOrDefault(x => x.Id == Convert.ToInt32(bill.txtId.Text)));
                        }
                    var amountOc = Convert.ToDouble(txtTotalAmountOC.Text) - Convert.ToDouble(bill.txtAmountOC.Text);
                    var amountMer = Convert.ToDouble(txtTotalAmountMER.Text) - Convert.ToDouble(bill.txtAmountMER.Text);

                    txtTotalAmountOC.Text = amountOc.ToString();
                    txtTotalAmountMER.Text = amountMer.ToString();

                    lstBillData.Items.Remove(lstBillData.SelectedItem);
                }
                else
                {
                    lstBillData.Items.Remove(lstBillData.SelectedItem);
                }
            }
            else
            {
                DXMessageBox.Show("Please select a Bill to delete!!");
                return;
            }
            

        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            if (loansAdvanceId == 0)
                AddRow();
        }

        private void BtnAddMore_Click(object sender, RoutedEventArgs e)
        {
            if(loansAdvanceId == 0)
                AddRow();
        }

        void AddRow()
        {
            ucBillData uc = new ucBillData();
            uc.HorizontalAlignment = HorizontalAlignment.Center;
            uc.Name = "ucBillDataRow";

            uc.btnMaintenance.Click += BtnVehicleExpenses_Click;
            uc.btnFuel.Click += BtnFuel_Click;
            uc.txtAmountOC.KeyUp += TxtAmountOC_KeyUp;
            uc.txtAmountWithTax.EditValueChanged += TxtAmountWithTax_EditValueChanged;
            uc.txtAmountPaidOC.EditValueChanged += TxtTotalAmountPaid_EditValueChanged;
            uc.txtRemainingAmountOC.EditValueChanged += TxtTotalAmountDue_EditValueChanged;
            uc.cmbxCOA.GotFocus += CmbxCoa_GotFocus;
            uc.cmbxCOA.SelectedIndexChanged += CmbxCoa_SelectedIndexChanged;
            uc.cmbxPayeeName.GotFocus += CmbxPayeeName_GotFocus;
            uc.cmbxVendor.SelectedIndexChanged += CmbxVendor_SelectedIndexChanged;
            uc.lookupAdminBillType.GotFocus += CmbxAdminBillType_GotFocus;
            uc.lookupAdminBillType.SelectedIndexChanged += CmbxAdminBillType_SelectedIndexChanged;
            uc.lookupVat.SelectedIndexChanged += LookupVat_SelectedIndexChanged;
            uc.txtTaxAmount.EditValueChanged += TxtTaxAmount_EditValueChanged;


            //uc.lookupVat.ItemsSource = taxes;

            if (chkAdjusted.IsChecked == true)
                uc.lookupVat.ItemsSource = taxes.Where(x => x.isAdjusted == true).ToList();
            else if (chkNonAdjusted.IsChecked == true)
                uc.lookupVat.ItemsSource = taxes.Where(x => x.isAdjusted == false).ToList();

            Company cmpny = new Company();
            if (cmbxCompany.SelectedIndex > -1)
                cmpny = cmbxCompany.SelectedItem as Company;

            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            

            if(department != null)
            {
                uc.cmbxCOA.ItemsSource = null;
                uc.cmbxCOA.ItemsSource = chartofAccounts;
                uc.cmbxCOAcredit.ItemsSource = chartofAccounts;
            }
            uc.lookupAdminBillType.ItemsSource = adminBillTypes;
            if(editFlag == true && bills.Count > 0)
            {
                if (bills[0].isApproved == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Admin Bills with Value") == null)
                    {
                        uc.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].PendingForClosing == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Pending for Closing Admin Bills with Value") == null)
                    {
                        uc.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].BillStatus.isActive == false)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Closed Admin Bills with Value") == null)
                    {
                        uc.txtAmountOC.IsReadOnly = true;
                    }
                }
                else if (bills[0].isApproved == true && bills[0].PendingForClosing != true && bills[0].BillStatus.isActive == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Open Admin Bills with Value") == null)
                    {
                        uc.txtAmountOC.IsReadOnly = true;
                    }
                }
            }

            if(company != null)
            {
                var employess = company.AdminBillEmployees;

                if (editFlag == true && bills != null && bills.Count > 0 && bills[0].user != null && employess.FirstOrDefault(x => x.EmpId == bills[0].user?.employeeId) == null)
                    employess.Add(bills[0].user.employee);
                uc.cmbxEmployee.ItemsSource = employess;

                uc.lookupAdminBillNature.ItemsSource = adminBillNatures;
                ucBillDataRow.lookupAdminBillNature.ItemsSource = adminBillNatures;
            }

            if (chkEmployee.IsChecked == true)
                uc.cmbxEmployee.Visibility = Visibility.Visible;
            else
                uc.cmbxEmployee.Visibility = Visibility.Collapsed;

            lstBillData.Items.Add(uc);
            AdjustmentFieldVisibility();
        }

        private void AdjustmentFieldVisibility()
        {
            if(loansAdvanceId != 0)
            {
                    emptyText1.Width = 740;

                    borderApprovedAdjustment.Visibility = Visibility.Visible;
                borderUnApprovedAdjustment.Visibility = Visibility.Visible;
                    foreach (UIElement element in lstBillData.Items)
                    {
                        if (element is UserControl)
                        {
                            var _bill = (UserControl)element;
                            if (_bill.Name == "ucBillDataRow")
                            {
                                var billElement = (ucBillData)_bill;
                                billElement.txtApprovedLoanAdjustment.Visibility = Visibility.Visible;
                            billElement.txtApprovedAdjustmentCount.Visibility = Visibility.Visible;
                            billElement.txtUnApprovedLoanAdjustment.Visibility = Visibility.Visible;
                            billElement.txtUnApprovedAdjustmentCount.Visibility = Visibility.Visible;
                            }
                        }

                    }
                    //if (chkEmployee.IsChecked == false)
                    //    emptyText.Width = 1335;
                    //else
                    //    emptyText.Width = 1470;
            }
            else
            {
                emptyText1.Width = 440;

                borderApprovedAdjustment.Visibility = Visibility.Collapsed;
                borderUnApprovedAdjustment.Visibility = Visibility.Collapsed;
                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            var billElement = (ucBillData)_bill;
                            billElement.txtApprovedLoanAdjustment.Visibility = Visibility.Collapsed;
                            billElement.txtApprovedAdjustmentCount.Visibility = Visibility.Collapsed;
                            billElement.txtUnApprovedLoanAdjustment.Visibility = Visibility.Collapsed;
                            billElement.txtUnApprovedAdjustmentCount.Visibility = Visibility.Collapsed;
                        }
                    }

                }
                //if (chkEmployee.IsChecked == false)
                //    emptyText.Width = 1335;
                //else
                //    emptyText.Width = 1470;
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if(groupId > 0)
            {
                if (gridTracker.Visibility == Visibility.Collapsed)
                {
                    views = UsersRepo.getViwerInfo(groupId, 17);
                    grdUsers.ItemsSource = views;
                    gridTracker.Visibility = Visibility.Visible;
                }
                else
                {
                    gridTracker.Visibility = Visibility.Collapsed;
                }
            }
            
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(editFlag == true && bills != null && bills.Count > 0)
                {
                    var adjustments = bills.SelectMany(x=>x.Adjustments).Where(x=>x.isApproved == false);
                    if (adjustments.Count() > 0)
                    {
                        DXMessageBox.Show("Please Approve all Adjustments first!");
                        return;
                    }

                    AdminBillsRepo billsRepo = new AdminBillsRepo();
                        //SalesReceipt receipt = new SalesReceipt();
                        var billsForApproval = billsRepo.GetBillsByGroupId(bills[0].transactionGroupId);
                        UsersRepo usersRepo = new UsersRepo();

                        if (billsForApproval != null && billsForApproval.Count > 0)
                        {
                            if (billsForApproval[0].isApproved == true)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null) ? true : false)
                                {
                                var res = MessageBox.Show("Admin Bills are Approved, Do you want to UnApprove these Bills?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if(res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < billsForApproval.Count; i++)
                                    {
                                        billsForApproval[i].isApproved = false;
                                        billsForApproval[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, bills[0].transactionGroupId, 17, frmInputBox.comment);

                                    billsRepo.ApproveBill(billsForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Admin Bill has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (billsForApproval[0].department != null && billsForApproval[0].department.Id != 0 && billsForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(billsForApproval[0].department.Id, billsForApproval[0].company.Id), groupId, TransactionItemType.Admin_Bill);
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
                                    if (billsForApproval[0].currency != null)
                                    {
                                        symbolCurr = billsForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Admin Bills (Amount OC) having value: " + billsForApproval[0].TotalAmountOC.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Admin Bill UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + billsForApproval[0].FinanceRefNo, billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + billsForApproval[0].FinanceRefNo, billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Admin Bills are UnApproved (" + bills[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Admin Bill is UnApproved (" + bills[0].transactionGroupId + ")");
                                }
                                
                                }
                                else
                                {
                                    MessageBox.Show("You are not Allowed to Approve Admin Bill Directly");
                                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                    return;
                                }

                            }
                            else if (billsForApproval[0].isApproved == false)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null) ? true : false)
                                {
                                var res = MessageBox.Show("Admin Bills are Pending for Approval, Do you want to Approve these Bills?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < billsForApproval.Count; i++)
                                    {
                                        billsForApproval[i].isApproved = true;
                                        billsForApproval[i].stage = TransactionStage.Approved.ToString();
                                    }
                                    
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, bills[0].transactionGroupId, 17, frmInputBox.comment);

                                    billsRepo.ApproveBill(billsForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Admin Bill has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (billsForApproval[0].department != null && billsForApproval[0].department.Id != 0 && billsForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(billsForApproval[0].department.Id, billsForApproval[0].company.Id), groupId, TransactionItemType.Admin_Bill);
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
                                    if (billsForApproval[0].currency != null)
                                    {
                                        symbolCurr = billsForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Admin Bills (Amount OC) having value: " + billsForApproval[0].TotalAmountOC.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Admin Bill Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + billsForApproval[0].FinanceRefNo, billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + billsForApproval[0].FinanceRefNo, billsForApproval[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Admin Bills are Approved (" + bills[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Admin Bill is Approved (" + bills[0].transactionGroupId + ")");
                                }

                                    
                                }
                                else
                                {
                                    MessageBox.Show("You are not Allowed to Approve Admin Bill Directly");
                                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                    return;
                                }

                            }
                            else if (bills[0].isReApproved == false)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Admin Bill") != null) ? true : false)
                                {
                                    for (int i = 0; i < bills.Count; i++)
                                    {
                                        billsForApproval[i].isReApproved = true;
                                        billsForApproval[i].stage = TransactionStage.Approved.ToString();

                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, bills[0].transactionGroupId, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                    billsRepo.ApproveBill(bills);

                                    MessageBox.Show("Admin Bills are Approved (" + bills[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + bills[0].transactionGroupId + ")");
                                }
                                else
                                {
                                    MessageBox.Show("You are not Allowed to Re-Approve Admin Bill Directly");
                                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
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

        public void loadonAdminBilldata()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Admin_Bill);
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
                    comments = procurementRepo.getcommentslogAsc(groupId, TransactionItemType.Admin_Bill);
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
                if (groupId > 0)
                {
                    if (department != null && department.Id != 0 && company?.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Admin_Bill);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (groupId > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && groupId != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (groupId == 0)
                        {
                            DXMessageBox.Show("Kindly save Admin Bill first to add a comment!");
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
                if (department != null && department.Id != 0 && company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Admin_Bill);
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
                    

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
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

        public void GenerateUsersForComments()
        {
            bool userFlag = false;
            try
            {
                if (bills[0].department != null)
                {
                    var dept = bills[0].department;
                    var users = UsersRepo.getusersByDepartment(dept.Id);
                    if (users != null)
                    {
                        UsersForComments = users;

                        if(bills != null && bills.Count > 0)
                        {
                            if(bills[0].user != null)
                            {
                                foreach (var _user in UsersForComments)
                                {
                                    if (_user.id == bills[0].user_Id)
                                    {
                                        userFlag = true;
                                        break;
                                    }
                                }
                                if(userFlag == false)
                                    UsersForComments.Add(bills[0].user);

                            }
                        }
                    }
                }
            }
            catch { }

        }


        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && bills != null && bills.Count > 0)
            {
                GenerateUsersForComments();

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersForComments, TransactionItemType.Admin_Bill);
                inputBox.ShowDialog();

               

                if (groupId != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && groupId != 0)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(groupId, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Admin Bill #" + txtSystemRefNo.Text, groupId, TransactionItemType.Admin_Bill, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }

                        //procurementRepo.Add(groupId, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        MessageBox.Show("Comment Added!");
                        loadcomments();
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (groupId == 0)
                    {
                        DXMessageBox.Show("Kindly save Admin Bill first to add a comment!");
                    }

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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Admin Bill") != null)
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Admin Bill") != null)
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

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                bills = BillsRepo.GetBillsByGroupId(groupId);

                if(bills != null && bills.Count > 0)
                {
                    if(bills[0].isVoid != true)
                        foreach (var _bill in bills)
                        {
                            if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                            {
                                DXMessageBox.Show("This Admin Bill cannot set to Void because there are bills which have active Payments!");
                                return;
                            }
                        }


                    if (bills[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Admin Bill") != null))
                        {
                            if (DXMessageBox.Show("This Transaction is currently in the list of Void Bills! Do you want to remove it from Void?", "Remove Void Bills", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                BillsRepo.setAdminBillsToVoid(groupId, false);
                                grdVoid.Visibility = Visibility.Collapsed;

                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res1 = MessageBox.Show("Admin Bill has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (bills[0].department != null && bills[0].department.Id != 0 && bills[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), groupId, TransactionItemType.Admin_Bill);
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
                            if (bills[0].currency != null)
                            {
                                symbolCurr = bills[0].currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Admin Bill (Amount OC) having value: " + bills[0].TotalAmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                                Timestamp = DateTime.Now,
                                Subject = "Admin Bill UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + bills[0].FinanceRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ",null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + bills[0].FinanceRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Admin Bill") != null)
                        {
                            if (DXMessageBox.Show("This Transaction is not currently in the list of Void Admin Bills! Do you want to move it to Void Admin Bills?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                BillsRepo.setAdminBillsToVoid(groupId, true);
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

                            var res1 = MessageBox.Show("Admin Bill has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (bills[0].department != null && bills[0].department.Id != 0 && bills[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), groupId, TransactionItemType.Admin_Bill);
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
                            if (bills[0].currency != null)
                            {
                                symbolCurr = bills[0].currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Admin Bill (Amount OC) having value: " + bills[0].TotalAmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                                Timestamp = DateTime.Now,
                                Subject = "Admin Bill Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + bills[0].FinanceRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Admin Bill #" + bills[0].FinanceRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to Mark or UnMark a Bill to Void!");
                        }
                }
                
            }
        }

        private void TxtAmountOC_KeyUp(object sender, KeyEventArgs e)
        {
            //AmountMERcalculations();
        }

        private void TxtAmountWithTax_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            double totalAmountOc = 0;
            double totalAmountMer = 0;
            if (String.IsNullOrEmpty(txtMER.Text) || Convert.ToDouble(txtMER.Text) == 0)
            {
                txtMER.Text = "1.00";
            }

            var MER = Convert.ToDouble(txtMER.Text);
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var AmountWithTax = Convert.ToDouble((billElement.FindName("txtAmountWithTax") as TextEdit).Text);
                        var AmountOC = Convert.ToDouble((billElement.FindName("txtAmountOC") as TextEdit).Text);
                        totalAmountOc = totalAmountOc + AmountOC;

                        var AmountMER = MER * AmountWithTax;
                        totalAmountMer = totalAmountMer + AmountMER;
                        (billElement.FindName("txtAmountMER") as TextEdit).Text = AmountMER.ToString();
                    }
                }
            }
            txtTotalAmountOC.Text = totalAmountOc.ToString();
            txtTotalAmountMER.Text = totalAmountMer.ToString();
        }

        private void TxtMER_KeyUp(object sender, KeyEventArgs e)
        {
            //AmountMERcalculations();
        }

        void AmountMERcalculations()
        {
            double totalAmountOc = 0;
            double totalAmountMer = 0;
            if (String.IsNullOrEmpty(txtMER.Text) || Convert.ToDouble(txtMER.Text) == 0)
            {
                txtMER.Text = "1.00";
            }

            var MER = Convert.ToDouble(txtMER.Text);
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var AmountWithTax = Convert.ToDouble((billElement.FindName("txtAmountWithTax") as TextEdit).Text);
                        var AmountOC = Convert.ToDouble((billElement.FindName("txtAmountOC") as TextEdit).Text);
                        totalAmountOc = totalAmountOc + AmountOC;

                        var AmountMER = MER * AmountWithTax;
                        totalAmountMer = totalAmountMer + AmountMER;
                        (billElement.FindName("txtAmountMER") as TextEdit).Text = AmountMER.ToString();
                    }
                }
            }
            txtTotalAmountOC.Text = totalAmountOc.ToString();
            txtTotalAmountMER.Text = totalAmountMer.ToString();
        }

        private void TxtTotalAmountPaid_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            double totalAmountPaid = 0;

            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var AmountPaid = Convert.ToDouble((billElement.FindName("txtAmountPaidOC") as TextEdit).Text);
                        totalAmountPaid = totalAmountPaid + AmountPaid;

                    }
                }
            }
            txtTotalAmountPaid.Text = totalAmountPaid.ToString();
        }

        private void TxtTotalAmountDue_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            double totalAmountRemaining = 0;

            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        var AmountDue = Convert.ToDouble((billElement.FindName("txtRemainingAmountOC") as TextEdit).Text);
                        totalAmountRemaining = totalAmountRemaining + AmountDue;

                    }
                }
            }
            txtTotalAmountDue.Text = totalAmountRemaining.ToString();
        }

        private void BtnAddLevel_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnExpandLevels_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCollapseLevels_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MbtnAddMemo_Click(object sender, RoutedEventArgs e)
        {
            memoWindow = new Window();
            frmAddMemo = new ucFrmAddMemo();

            memoWindow.Width = 400;
            memoWindow.Height = 300;
            memoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            memoWindow.ResizeMode = ResizeMode.NoResize;
            memoWindow.Title = "Memo";
            frmAddMemo.Padding = new Thickness(5,5,5,5);
            var _bill = (UserControl)lstBillData.SelectedItem;
            if (_bill.Name == "ucBillDataRow")
            {
                var billElement = (ucBillData)_bill;
                var textMemoElement = billElement.FindName("txtMemo") as TextEdit;

                frmAddMemo.txtMemo.Text = textMemoElement.Text;
                frmAddMemo.btnSave.Click += BtnSaveMemo_Click;
            }
            memoWindow.Content = frmAddMemo;
            memoWindow.ShowDialog();
        }

        private void BtnSaveMemo_Click(object sender, RoutedEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;
            if (_bill.Name == "ucBillDataRow")
            {
                var billElement = (ucBillData)_bill;
                if (!String.IsNullOrEmpty(frmAddMemo.txtMemo.Text))
                {
                    (billElement.FindName("txtMemo") as TextEdit).Text = frmAddMemo.txtMemo.Text;
                }   
            }
            memoWindow.Close();
        }

        public void loadTemplates()
        {
            List<Template> templates = new List<Template>();
            FieldsRepo repo = new FieldsRepo();
            templates = repo.GetAllTemplates().Where(x=>x.transactionType == ERP_BL.Enums.TransactionItemType.Admin_Bill).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (Template _template in templates)
            {
                cmbitems.Add(new cmbitem() { name = _template.Name, id = _template.Id });
            }
            cmbxBillTemplate.ItemsSource = cmbitems;

        }

        private void loadBillReferenceNo()
        {
            var references = BillsRepo.GetAllActiveBillReferenceNo(company.Id);
            if(editFlag == true && bills.Count > 0)
            {
                if (bills[0].BillRefNo != null && references.FirstOrDefault(x => x.Id == bills[0].BillRefNoId) == null)
                    references.Add(bills[0].BillRefNo);
            }
            List<cmbitem> cmbitems = new List<cmbitem>();

            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }
            
            cmbxBillRef.ItemsSource = cmbitems;
        }

        private void loadBillStatus()
        {
            //BillRepo billRepo = new BillRepo();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Admin Bill Statuses") != null)
                BillStatuses = BillsRepo.GetAllBillStatuses();
            else
                BillStatuses = BillsRepo.GetAllBillStatuses().Where(x=>x.isActive == true).ToList();
            BillStatuses = BillStatuses.Where(x => x.isDisable != true).ToList();
            foreach (var status in BillStatuses)
            {
                cmbitems.Add(new cmbitem() {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000" });
            }
            cmbBillStatus.ItemsSource = cmbitems;
        }  
        private void loadBillStatus(AdminBillStatus _status)
        {
            BillStatuses.Add(_status);
            foreach(var status in BillStatuses)
            {
                cmbitems.Add(new cmbitem() {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000" });
            }
            cmbBillStatus.ItemsSource = cmbitems;
        }

        private void loadAdminBillTypes()
        {
            adminBillTypes = BillsRepo.GetAllAdminBillTypesForAdminBill();
        }

        private void loadCurrencies()
        {
            CurrencyRepo repo = new CurrencyRepo();
            var currencies = repo.getAll();
            cmbxCurrency.ItemsSource = currencies;
        }

        private void loadManagementSummaries()
        {
            lookUpManagementSummary.ItemsSource = BillsRepo.GetAllManagementSummary();
        }

        private void loadCompanies()
        {
            CompanyRepo repo = new CompanyRepo();
            cmbxCompany.ItemsSource = repo.GetUserAdminBillCompanies(SYSTEM_STATIC.currentUser.id);
        }

        private void CmbxCoa_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var parent = (StackPanel)((LookUpEdit)sender).Parent;
            var grid = (Grid)parent.Parent;
            var row = (UserControl)grid.Parent;
            if(row != null)
            {
                var bill = (ucBillData)row;
                if(bill.cmbxCOA.SelectedIndex > -1)
                {
                    var coa = bill.cmbxCOA.SelectedItem as ChartofAccount;

                    List<AdminBillType> adminBillTypes = new List<AdminBillType>();
                    bill.lookupAdminBillType.ItemsSource = BillsRepo.GetAllAdminBillTypesByCOA(coa);
                }
            }
        }

        private void CmbxAdminBillType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var parent = (StackPanel)((LookUpEdit)sender).Parent;
            var grid = (Grid)parent.Parent;
            var row = (UserControl)grid.Parent;
            if (row != null)
            {
                var bill = (ucBillData)row;
                if (bill.lookupAdminBillType.SelectedIndex > -1)
                {
                    var billType = bill.lookupAdminBillType.SelectedItem as AdminBillType;
                    bill.cmbxVendor.ItemsSource = billType.vendors;
                }
            }
        }

        private void TxtTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var parent = (StackPanel)((TextEdit)sender).Parent;
            var stackPanel = (StackPanel)parent.Parent;
            var grid = (Grid)stackPanel.Parent;
            var row = (UserControl)grid.Parent;

            if (row != null)
            {
                var bill = (ucBillData)row;
                
                    var amountOC = Convert.ToDouble(bill.txtAmountOC.Text);

                    var taxAmount = Convert.ToDouble(bill.txtTaxAmount.Text);

                    bill.txtAmountWithTax.Text = (amountOC + taxAmount).ToString();
                
            }   
        }

        private void LookupVat_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var parent = (StackPanel)((LookUpEdit)sender).Parent;
            var stackPanel = (StackPanel)parent.Parent;
            var grid = (Grid)stackPanel.Parent;
            var row = (UserControl)grid.Parent;

            if (row != null)
            {
                var bill = (ucBillData)row;
                if(bill.lookupVat.SelectedIndex > -1)
                {
                    var tax = bill.lookupVat.SelectedItem as TaxName;
                    if (tax.isManual == true)
                        bill.txtTaxAmount.IsReadOnly = false;
                    else
                        bill.txtTaxAmount.IsReadOnly = true;

                    if (tax != null)
                        if (Convert.ToDouble(bill.txtAmountOC.Text) != 0)
                        {
                            var amountOC = Convert.ToDouble(bill.txtAmountOC.Text);
                            var percent = tax.percentage;

                            var taxAmount = (percent / 100) * amountOC;
                            bill.txtTaxAmount.Text = taxAmount.ToString();

                            bill.txtAmountWithTax.Text = (amountOC + taxAmount).ToString();
                        }
                }
            }

                
        }

        private void CmbxVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var parent = (StackPanel)((LookUpEdit)sender).Parent;
            var grid = (Grid)parent.Parent;
            var row = (UserControl)grid.Parent;
            if (row != null)
            {
                    var bill = (ucBillData)row;
                    if(bill.lookupAdminBillType.SelectedIndex > -1 && bill.cmbxVendor.SelectedIndex > -1 && cmbxCompany.SelectedIndex > -1 && cmbxDepartment.SelectedIndex > -1)
                    {
                        var cmpny = cmbxCompany.SelectedItem as Company;
                        var dept = cmbxDepartment.SelectedItem as Department;
                        var billType = bill.lookupAdminBillType.SelectedItem as AdminBillType;
                        var vendor = bill.cmbxVendor.SelectedItem as Vendor;

                        payeeList = BillsRepo.GetAllPayeesOfCompanyDept(cmpny, dept, billType, vendor);
                        bill.cmbxPayeeName.ItemsSource = payeeList;
                        bill.cmbxPayeeName.SelectedIndex = -1;

                    }   
            }
        }

        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = new Company();
            CompanyRepo companyRepo = new CompanyRepo();
            company = cmbxCompany.SelectedItem as Company;

            adminBillNatures = BillsRepo.GetAllAdminBillNatureByComp(company.Id);
            var loginUser = SYSTEM_STATIC.currentUser;

            //var departments = company.departments;
            var departments = company.departments.Where(x=>x.isActive==true && x.IsAdminBillType == true && x.employees.FirstOrDefault(y=>y.EmpId == loginUser.employeeId) != null).ToList();

            if (editFlag == true && bills.Count > 0 && bills[0].department != null && departments.FirstOrDefault(x=>x.Id == bills[0].dept_Id) == null)
            {
                departments.Add(bills[0].department);
            }
            


            cmbxDepartment.SelectedItem = null;

            DepartmentRepo deptRepo = new DepartmentRepo();
            var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();

            if (deptMngt != null)
                foreach (var _dept in deptMngt)
                {
                    if (departments.Any(x => x.Id == _dept.Id) == false && _dept.companies.FirstOrDefault(x=>x.Id == company.Id) != null)
                        departments.Add(_dept);
                }

            //Only Allowed departments to Employee will show in Dropdown
            cmbxDepartment.ItemsSource = departments;

            loadBillReferenceNo();
            loadVATBookReferenceNo();

            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        billElement.cmbxCOA.ItemsSource = null;
                        billElement.cmbxCOA.SelectedIndex = -1;

                        //loadAdminBillTypes();
                        billElement.lookupAdminBillType.ItemsSource = adminBillTypes;

                        billElement.cmbxEmployee.ItemsSource = null;
                        var employess = company.AdminBillEmployees;
                        if (editFlag == true && bills != null && bills.Count > 0 && bills[0].user != null && employess.FirstOrDefault(x => x.EmpId == bills[0].user.employeeId) == null)
                            employess.Add(bills[0].user.employee);
                        billElement.cmbxEmployee.ItemsSource = employess;

                        billElement.lookupAdminBillNature.SelectedIndex = -1;
                        billElement.lookupAdminBillNature.ItemsSource = null;
                        billElement.lookupAdminBillNature.ItemsSource = adminBillNatures;
                    }
                }
            }

            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            var banks = receiptRepo.GetBanksbyCompanyForPayments(company);
            lookupBank.ItemsSource = banks;
        }

        private void CmbxDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = cmbxDepartment.SelectedItem as Department;
            int billCounter = 0;

            if (department != null)
            {
                cmbxEmployee.ItemsSource = department.employees.Where(x => x.isActive == true).ToList();
                var cmpny = cmbxCompany.SelectedItem as Company;
                ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
                chartofAccounts = coaRepo.GetAllforBills(cmpny, department, SYSTEM_STATIC.currentUser.id);


                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            var billElement = (ucBillData)_bill;
                            if(billElement.lookupAdminBillType.SelectedIndex > -1 && billElement.cmbxVendor.SelectedIndex > -1 && company != null)
                            {
                                var billType = billElement.lookupAdminBillType.SelectedItem as AdminBillType;

                                var vendor = billElement.cmbxVendor.SelectedItem as Vendor;
                                payeeList = BillsRepo.GetAllPayeesOfCompanyDept(cmpny, department, billType, vendor);
                                billElement.cmbxPayeeName.ItemsSource = payeeList;
                            }
                            billElement.cmbxCOA.ItemsSource = null;
                            billElement.cmbxCOA.ItemsSource = chartofAccounts;
                            billElement.cmbxCOAcredit.ItemsSource = chartofAccounts;

                        }
                    }

                    billCounter++;
                }
            }
        }

        private void loadSecondaryCards()
        {
            CreditCardRepo cardRepo = new CreditCardRepo();
            var creditCardList = cardRepo.GetAllSecondaryCardsByCompanyDept(company.Id, department.Id, (lookupBank.SelectedItem as Bank).Id);

            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (CreditCard _card in creditCardList)
            {
                cmbitems.Add(new cmbitem() { name = _card.CardNumber, id = _card.Id });
            }
            cmbxSecondaryCardNo.ItemsSource = cmbitems;
        }


        private void loadPrimaryCards()
        {
            if(company != null && department != null && lookupBank.SelectedIndex > -1)
            {
                CreditCardRepo cardRepo = new CreditCardRepo();
                var creditCardList = cardRepo.GetAllPrimaryCardsByCompanyDept(company.Id, department.Id, (lookupBank.SelectedItem as Bank).Id);

                List<cmbitem> cmbitems = new List<cmbitem>();

                foreach (CreditCard _card in creditCardList)
                {
                    cmbitems.Add(new cmbitem() { name = _card.CardNumber, id = _card.Id });
                }
                cmbxPrimaryCardNo.ItemsSource = cmbitems;
            }
            else
                cmbxPrimaryCardNo.ItemsSource = null;
        }

        private void BtnAddMemo_Click(object sender, RoutedEventArgs e)
        {
            memoWindow = new Window();
            frmAddMemo = new ucFrmAddMemo();

            memoWindow.Width = 400;
            memoWindow.Height = 300;
            memoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            memoWindow.ResizeMode = ResizeMode.NoResize;
            memoWindow.Title = "Memo";
            frmAddMemo.Padding = new Thickness(5, 5, 5, 5);
            var _bill = (UserControl)lstBillData.SelectedItem;

            if(_bill != null)
            {
                if (_bill.Name == "ucBillDataRow")
                {
                    var billElement = (ucBillData)_bill;
                    var textMemoElement = billElement.FindName("txtMemo") as TextEdit;

                        frmAddMemo.txtMemo.Text = textMemoElement.Text;
                        frmAddMemo.btnSave.Click += BtnSaveMemo_Click;
                }

                memoWindow.Content = frmAddMemo;
                memoWindow.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Please select a Bill first!");
            }
            
        }

        private void CmbxBillTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AdjustmentFieldVisibility();
        }

        private void CmbxCoa_GotFocus(object sender, RoutedEventArgs e)
        {
            if(cmbxBillTemplate.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Bill Template!");
                return;
            }
            if(cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                return;
            }
        }

        private void CmbxAdminBillType_GotFocus(object sender, RoutedEventArgs e)
        {
            //var parent = (StackPanel)((ComboBox)sender).Parent;
            //var grid = (Grid)parent.Parent;
            //var row = (UserControl)grid.Parent;
            //if (row != null)
            //{
            //    var bill = (ucBillData)row;
            //    if (bill.cmbxCOA.SelectedIndex < 0)
            //    {
            //        DXMessageBox.Show("Please Select Chart Of Account!");
            //        bill.cmbxCOA.Focus();
            //        return;
            //    }
            //}
        }

        private void CmbxPayeeName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                return;
            }
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Department!");
                return;
            }

            var stackPanel = (StackPanel)((LookUpEdit)sender).Parent;
            var grid = (Grid)stackPanel.Parent;
            var row = (ucBillData)grid.Parent;

            if(row.cmbxVendor.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Vendor!");
                return;
            }
        }

        private void CmbxDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if(cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, 17, "Viewed details of Admin Bills");
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

        private void CmbxEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                return;
            }
        }

        public static double Percent = 10;
        
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
                        destination += "Attachments\\Admin_Bill\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Admin_Bill.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Admin_Bill);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Admin_Bill, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 17, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Admin_Bill);
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

        private void CmbxBillRef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       

        private void CmbxPrimaryCardNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (cmbxPrimaryCardNo.SelectedIndex > -1)
            {
                var cmbitem = cmbxPrimaryCardNo.SelectedItem as cmbitem;

                CreditCardRepo cardRepo = new CreditCardRepo();
                //var creditCardList = cardRepo.GetCardsByPrimaryCard(company.Id, department.Id, cmbitem.id);
                var creditCard = cardRepo.GetCreditCard(cmbitem.id);

                if(creditCard != null)
                {
                    if(creditCard.CardUser != null)
                        txtCardUser.Text = creditCard.CardUser.Name;

                    if(creditCard.creditCardType != null)
                        txtCardType.Text = creditCard.creditCardType.Type;

                    if(creditCard.PrimaryCardHolder != null)
                        txtPrimaryCardHolder.Text = creditCard.PrimaryCardHolder.Name;

                }
            }
        }

        private void CmbxPrimaryCardNo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }

            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                return;
            }

           
            if (lookupBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                return;
            }

            if (cmbxHolderType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Holder Type!");
                return;
            }

            if(cmbxHolderType.SelectedIndex == 1)
            {
                DXMessageBox.Show("You have selected Secondary Holder Type!");
                cmbxHolderType.Focus();
                return;
            }
        }

        private void CmbxCardUser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }

            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                return;
            }
        }

        private void CmbxBank_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                cmbxCompany.Focus();
                return;
            }

            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                cmbxDepartment.Focus();
                return;
            }
        }

        private void CmbxBank_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
          
        }

        private void CmbxSecondaryCardNo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }

            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                return;
            }

            if (lookupBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                return;
            }
            if (cmbxHolderType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Holder Type!");
                return;
            }

            if (cmbxHolderType.SelectedIndex == 0)
            {
                DXMessageBox.Show("You have selected Primary Holder Type!");
                cmbxHolderType.Focus();
                return;
            }
        }

        private void CmbxSecondaryCardNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (cmbxSecondaryCardNo.SelectedIndex > -1)
            {
                CreditCardRepo cardRepo = new CreditCardRepo();
                var cmbitem = cmbxSecondaryCardNo.SelectedItem as cmbitem;
                var creditCard = cardRepo.GetCreditCard(cmbitem.id);

                if (creditCard != null)
                {
                    //Select Primary Card Number
                    var primaryCardList = (cmbxPrimaryCardNo.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPrimaryCardNo.ItemsSource as List<cmbitem>;
                    if (creditCard.PrimaryCardNoId != null)
                    {
                        int index = 0;
                        foreach (var _cardNo in primaryCardList)
                        {
                            if (_cardNo.id == creditCard.PrimaryCardNoId)
                            {
                                cmbxPrimaryCardNo.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    if(cmbxPrimaryCardNo.SelectedIndex > -1)
                    {
                        txtCardType.Text = creditCard.creditCardType.Type;
                        txtCardUser.Text = creditCard.CardUser.Name;
                        txtPrimaryCardHolder.Text = creditCard.PrimaryCardHolder.Name;
                        if (creditCard.cardHolderType == CardHolderType.Secondary)
                            txtSecondaryCardHolder.Text = creditCard.SecondaryCardHolder.Name;
                        else
                            txtSecondaryCardHolder.Text = null;
                    }
                    else
                    {
                        DXMessageBox.Show("No primary card found!");
                        return;
                    }
                    
                }
            }
        }

        private void cmbxHolderType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if(cmbxHolderType.SelectedIndex == 0)
            //{
                loadPrimaryCards();
            //}
             if(cmbxHolderType.SelectedIndex == 1)
            {
                loadSecondaryCards();
            }
            cmbxPrimaryCardNo.SelectedIndex = -1;
            cmbxSecondaryCardNo.SelectedIndex = -1;
            txtCardType.Text = null;
            txtCardUser.Text = null;
            txtPrimaryCardHolder.Text = null;
            txtSecondaryCardHolder.Text = null;
        }

        private void CmbxBank_GotFocus_1(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxType_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }

            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                return;
            }

            if (lookupBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                return;
            }
        }

        private void BtnAddMemo_MouseEnter(object sender, MouseEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;
            if (_bill != null && _bill.Name == "ucBillDataRow")
            {
                var billElement = (ucBillData)_bill;
                var textMemoElement = billElement.FindName("txtMemo") as TextEdit;

                btnAddMemo.ToolTip = textMemoElement.Text;
            }
        }

        private void ChkEmployee_Checked(object sender, RoutedEventArgs e)
        {
            
              
                    emptyText.Width = 1610;
                    //emptyText1.Width = 440;
                    borderEmployee.Visibility = Visibility.Visible;
                    foreach (UIElement element in lstBillData.Items)
                    {
                        if (element is UserControl)
                        {
                            var _bill = (UserControl)element;
                            if (_bill.Name == "ucBillDataRow")
                            {
                                var billElement = (ucBillData)_bill;
                                billElement.cmbxEmployee.Visibility = Visibility.Visible;
                            }
                        }

                    }

            AdjustmentFieldVisibility();
            
        }

        private void ChkEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            
                    emptyText.Width = 1475;
                    //emptyText1.Width = 440;
                    borderEmployee.Visibility = Visibility.Collapsed;
                    foreach (UIElement element in lstBillData.Items)
                    {
                        if (element is UserControl)
                        {
                            var _bill = (UserControl)element;
                            if (_bill.Name == "ucBillDataRow")
                            {
                                var billElement = (ucBillData)_bill;
                                billElement.cmbxEmployee.Visibility = Visibility.Collapsed;
                            }
                        }

                    }

            AdjustmentFieldVisibility();
           
        }

        public ucFrmBillAdd(AdminBillStatus billStatus)
        {
            statusChanged = billStatus;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

            UsersRepo usersRepo = new UsersRepo();
            AdminBillsRepo billsRepo = new AdminBillsRepo();
            if(editFlag == true && groupId > 0)
            {
                string previous_status;
                var bills = billsRepo.GetBillsByGroupId(groupId);
                var totalAmountOC = Math.Round( bills.Sum(x => x.AmountOC),2);

                foreach (var _billl in bills)
                {
                    double amountOC = 0;
                    if (_billl.tax != null)
                        amountOC = _billl.AmountWithTax;
                    else
                        amountOC = _billl.AmountOC;

                    var adjustmentAmount = _billl.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);

                    var finalAmount =Math.Round((_billl.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) +adjustmentAmount) - amountOC, 2);
                    if (_billl.Payments == null || _billl.Payments.Count == 0)
                    {
                        isFullyPaid = false;
                        break;
                    }
                    else if (finalAmount != 0)
                    {
                        isFullyPaid = false;
                        break;
                    }
                    
                }

                if(isFullyPaid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bills without Payments") == null)
                {
                    DXMessageBox.Show("Permission required to close Unpaid Admin Bills!");
                    return;
                }

                if(isFullyPaid == false)
                {
                    MessageBoxResult result = DevExpress.Xpf.Core.DXMessageBox.Show("This Admin Bill is not Paid Yet, Do you want to Close it?", "Bill is Unpaid", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                




                if (bills != null && bills.Count > 0)
                    {
                        previous_status = bills[0].BillStatus.Status;

                        if (bills[0].isApproved == false)
                        {
                            DXMessageBox.Show("Bills are pending for approval!");
                            return;
                        }
                        statusChanged = null;
                        ucFrmBillDirectClose ucFrmDirectClose = new ucFrmBillDirectClose();
                        if (bills[0].BillStatus != null)
                        {
                            ucFrmDirectClose.statusName.Text = bills[0].BillStatus.Status;

                            var brush = new BrushConverter();
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(bills[0].BillStatus.backcolor);
                        }

                        ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
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

                        var res = MessageBox.Show("Admin Bills has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment(department.Id, company.Id), groupId, TransactionItemType.Admin_Bill);
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null)
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].PendingForClosing = false;
                                    bills[i].stage = TransactionStage.Closed.ToString();
                                    bills[i].statusId = statusChanged.Id;
                                    bills[i].LastStatusChangeDate = System.DateTime.Now;
                                    bills[i].ClosingDate = System.DateTime.Now;


                                }
                                billsRepo.ApproveBill(bills);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                               
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:"+totalAmountOC,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (bills.Count != 0)
                                    {
                                        procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0,user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                           
                            //Load_Receipts();
                        }
                            else
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].PendingForClosing = true;
                                    bills[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    bills[i].statusId = statusChanged.Id;
                                    bills[i].LastStatusChangeDate = System.DateTime.Now;
                                    bills[i].ClosingDate = System.DateTime.Now;


                                }
                                billsRepo.ApproveBill(bills);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);
                            
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (bills.Count != 0)
                                    {
                                        procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, 0,user.id, "New Comment ", null);
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

        private void BtnCreatePayment_Click(object sender, RoutedEventArgs e)
        {
            bool isFullyPaid = true;

            if(editFlag == true && bills != null && bills.Count > 0)
            {
                if (bills[0].isVoid == true)
                {
                    DXMessageBox.Show("This Bill is Voided!");
                    return;
                }
                if (bills[0].PendingForClosing == true)
                {
                    DXMessageBox.Show("This Bill is in Pending for Closing State!");
                    return;
                }
                if (bills[0].isApproved == false)
                {
                    DXMessageBox.Show("Bill is under Approval!");
                    return;
                }
                
                if (bills[0].BillStatus.isActive == false)
                {
                    DXMessageBox.Show("This bill is Already closed!");
                    return;
                }

                foreach (var _bill in bills)
                {
                    if(_bill.LoansAdvanceId == null)
                    {
                        if (_bill.Payments.Where(x => x.isVoid != true) == null || _bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                        {
                            isFullyPaid = false;
                            break;
                        }
                        else if ((_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) - _bill.AmountOC) != 0)
                        {
                            isFullyPaid = false;
                            break;
                        }
                    }
                    else
                    {
                        if (_bill.Payments.Where(x => x.isVoid != true) == null || _bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                        {
                            if (_bill.Adjustments == null || _bill.Adjustments.Count == 0)
                            {
                                isFullyPaid = false;
                                break;
                            }
                            else
                            {
                                if ((_bill.Adjustments.Sum(x=>x.AdjustmentAmount) - _bill.AmountOC) != 0)
                                {
                                    isFullyPaid = false;
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            if ((_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) + _bill.Adjustments.Sum(x => x.AdjustmentAmount) - _bill.AmountOC) != 0)
                            {
                                isFullyPaid = false;
                                break;
                            }

                            
                        }
                    }
                    
                }

                if(isFullyPaid == true)
                {
                    DXMessageBox.Show("This Admin Bill is fully Paid!");
                    return;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill Payment") != null)
                {
                    Window moduleWin = new Window();

                    ucFrmPayments frmPayments = new ucFrmPayments();

                    frmPayments.adminBillsGroupId = bills[0].transactionGroupId;
                    frmPayments.createdFromBill = true;
                    moduleWin.Content = frmPayments;
                    moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    moduleWin.WindowState = WindowState.Maximized;
                    moduleWin.Show();

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Payment!");
                }
            }
            
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (lstBillData.SelectedItem != null)
            {
                var idd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtId.Text);
                var groupIdd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtGroupId.Text);
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, groupIdd, TransactionItemType.Admin_Bill);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Select any Bill first!");
            }
        }

        private void BtnSaveManagmentSummaryMemo_Click(object sender, RoutedEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;
            if (_bill.Name == "ucBillDataRow")
            {
                var billElement = (ucBillData)_bill;
                if (!String.IsNullOrEmpty(frmAddMemo.txtMemo.Text))
                {
                    (billElement.FindName("txtSummaryMemo") as TextEdit).Text = frmAddMemo.txtMemo.Text;
                }
            }
            memoWindow.Close();
        }

        private void BtnManagementSummary_Click(object sender, RoutedEventArgs e)
        {
            memoWindow = new Window();
            frmAddMemo = new ucFrmAddMemo();

            memoWindow.Width = 400;
            memoWindow.Height = 300;
            memoWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            memoWindow.ResizeMode = ResizeMode.NoResize;
            memoWindow.Title = "Management Summary";
            frmAddMemo.Padding = new Thickness(5, 5, 5, 5);
            var _bill = (UserControl)lstBillData.SelectedItem;

            if (_bill != null)
            {
                if (_bill.Name == "ucBillDataRow")
                {
                    var billElement = (ucBillData)_bill;
                    var textMemoElement = billElement.FindName("txtSummaryMemo") as TextEdit;

                    frmAddMemo.txtMemo.Text = textMemoElement.Text;
                    frmAddMemo.btnSave.Click += BtnSaveManagmentSummaryMemo_Click;
                }

                memoWindow.Content = frmAddMemo;
                memoWindow.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Please select a Bill first!");
            }
        }

        private void ChkManagementSummary_Checked(object sender, RoutedEventArgs e)
        {
            lookUpManagementSummary.IsEnabled = true;
            btnManagementSummary.IsEnabled = true;
        }

        private void ChkManagementSummary_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpManagementSummary.IsEnabled = false;
            btnManagementSummary.IsEnabled = false;
        }

        private void LookUpManagementSummary_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var managementSummary = lookUpManagementSummary.SelectedItem as ManagementSummary;
            

            var _bill = (UserControl)lstBillData.SelectedItem;

            if (_bill != null)
            {
                if (managementSummary.ParentId == null)
                {
                    DXMessageBox.Show("Parent Summary cannot be selected!");
                    lookUpManagementSummary.Focus();
                    lookUpManagementSummary.SelectedIndex = -1;
                    return;
                }
                if (_bill.Name == "ucBillDataRow")
                {
                    var billElement = (ucBillData)_bill;
                    (billElement.FindName("txtManagementSummaryId") as TextEdit).Text = managementSummary.Id.ToString();
                }
            }
            else
            {
                DXMessageBox.Show("Please select a Bill first!");
            }
        }

        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {
           
            List<JournalTransaction> jTransactions = new List<JournalTransaction>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        var billElement = (ucBillData)_bill;
                        AdminBill bill = new AdminBill();
                        if (editFlag == true)
                        {
                            if (Convert.ToInt32(billElement.txtId.Text) > 0)
                                bill = bills[bills.IndexOf(bills.FirstOrDefault(x => x.Id == Convert.ToInt32(billElement.txtId.Text)))];
                        }
                        bill.AmountOC = Convert.ToDouble(billElement.txtAmountOC.Text);
                        bill.MER = Convert.ToDouble(txtMER.Text);
                        bill.AmountOC = Convert.ToDouble(billElement.txtAmountOC.Text);
                        if (!string.IsNullOrEmpty(billElement.txtId.Text))
                        {
                            jTransactions.AddRange(getJournalTransactions(Convert.ToInt32(billElement.txtId.Text), bill.MER, bill.Memo, SYSTEM_STATIC.currentUser.id, bill.AmountOC, (billElement.cmbxCOA.SelectedItem as ChartofAccount).Id, txtFinanceRefNo.Text, department.Id, (billElement.cmbxCOAcredit.SelectedItem as ChartofAccount).Id, (DateTime)datglPostingdate.EditValue));
                        }
                        else
                            jTransactions.AddRange(getJournalTransactions(0, bill.MER, bill.Memo, SYSTEM_STATIC.currentUser.id, bill.AmountOC, (billElement.cmbxCOA.SelectedItem as ChartofAccount).Id, txtFinanceRefNo.Text, department.Id, (billElement.cmbxCOAcredit.SelectedItem as ChartofAccount).Id, (DateTime)datglPostingdate.EditValue));
                    }
                }
                ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(jTransactions);
                generalJournal.ShowDialog();
               
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
           
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Admin Bill") != null)
            {
                if (grdAttachments1.Visibility == Visibility.Visible)
                    grdAttachments1.Visibility = Visibility.Collapsed;
                else
                {
                    if (editFlag != false)
                    {
                        if (lstBillData.SelectedItem != null)
                        {
                            List<TreeItem> otherAttachments = new List<TreeItem>();
                            List<TreeItem> atachments = SYSTEM_STATIC.GetAdminBillAttachmentsListByCategory(groupId, TransactionItemType.Admin_Bill);
                            var idd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtId.Text);
                            AdminBill bill = BillsRepo.get(idd);
                            if (bill.Payments.Count != 0)
                            {
                                foreach (var payment in bill.Payments)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetAdminBillAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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
                    }
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Admin Bill") != null)
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttach1.Visibility = Visibility.Visible;
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveABAttachmentCategories();
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
                        destination += "Attachments\\Admin_Bill\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Admin_Bill.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Admin_Bill);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Admin_Bill, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 17, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Admin_Bill);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
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

        private void ChkAmountOC_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountMER.IsChecked = false;
        }

        private void ChkAmountMER_Checked(object sender, RoutedEventArgs e)
        {
            chkAmountOC.IsChecked = false;
        }

        

        private void LstBillData_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void MbtnAddAdjustment_Click(object sender, RoutedEventArgs e)
        {
            if(SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Add Adjustments") != null)
            {
                if (loansAdvanceId != 0)
                {
                    ucAdjustmentsList adjustmentWindow = new ucAdjustmentsList();
                    adjustmentWindow.Width = 550;
                    adjustmentWindow.Height = 450;
                    adjustmentWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    var row = (UserControl)lstBillData.SelectedItem;
                    if (row != null)
                    {
                        if (editFlag == true)
                    {
                        
                            var bill = (ucBillData)row;
                            if (!String.IsNullOrEmpty(bill.txtId.Text))
                                adjustmentWindow.billId = Convert.ToInt32(bill.txtId.Text);
                        
                    }
                    adjustmentWindow.ShowDialog();

                    
                        if (editFlag == true)
                        {
                            var bill = (ucBillData)row;
                            bill.adjustments = new List<Adjustment>();
                            foreach (var _adjustment in adjustmentWindow.adjustments)
                            {
                                bill.adjustments.Add(new Adjustment()
                                {
                                    Id = _adjustment.Id,
                                    AdjustmentDate = _adjustment.AdjustmentDate,
                                    adminBillId = Convert.ToInt32(bill.txtId.Text),
                                    AdjustmentAmount = _adjustment.AdjustmentAmount,
                                    ReferenceNo = _adjustment.ReferenceNo,
                                    isApproved = _adjustment.isApproved,
                                    ApprovedDate = _adjustment.ApprovedDate
                                });
                            }
                            var aprvdAdjustments = bill.adjustments.Where(x => x.isApproved == true);
                            var unaprvdAdjustments = bill.adjustments.Where(x => x.isApproved == false);
                            bill.txtApprovedLoanAdjustment.Text = aprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                            bill.txtUnApprovedLoanAdjustment.Text = unaprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                            bill.txtApprovedAdjustmentCount.Text = aprvdAdjustments.Count().ToString();
                            bill.txtUnApprovedAdjustmentCount.Text = unaprvdAdjustments.Count().ToString();
                        }
                        else
                        {
                            var bill = (ucBillData)row;
                            bill.adjustments = new List<Adjustment>();
                            foreach (var _adjustment in adjustmentWindow.adjustments)
                            {
                                bill.adjustments.Add(new Adjustment()
                                {
                                    Id = _adjustment.Id,
                                    adminBillId = 0,
                                    AdjustmentDate = _adjustment.AdjustmentDate,
                                    AdjustmentAmount = _adjustment.AdjustmentAmount,
                                    ReferenceNo = _adjustment.ReferenceNo,
                                    isApproved = _adjustment.isApproved,
                                    ApprovedDate = _adjustment.ApprovedDate
                                });
                            }
                            var aprvdAdjustments = bill.adjustments.Where(x => x.isApproved == true);
                            var unaprvdAdjustments = bill.adjustments.Where(x => x.isApproved == false);
                            bill.txtApprovedLoanAdjustment.Text = aprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                            bill.txtUnApprovedLoanAdjustment.Text = unaprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
                            bill.txtApprovedAdjustmentCount.Text = aprvdAdjustments.Count().ToString();
                            bill.txtUnApprovedAdjustmentCount.Text = unaprvdAdjustments.Count().ToString();
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Please select the Bill to View/Add Adjustments");
                    }
                }
                else
                {
                    DXMessageBox.Show("This Admin Bill is not generated from Loans Advances");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Adjustments!");
            }
        }

        private void btnCreateIBT_Click(object sender, RoutedEventArgs e)
        {
            if (lstBillData.SelectedItems.Count == 1)
            {
                var adminBill = lstBillData.SelectedItem as ucBillData;
                var adminBIllId = Convert.ToInt32(adminBill.txtId.Text);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
                {
                    ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers(TransactionItemType.Admin_Bill, adminBIllId);
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
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("You can only create IBT for 1 admin bill");

            }
        }

        private void ChkAdjusted_Checked(object sender, RoutedEventArgs e)
        {
            chkNonAdjusted.IsChecked = false;
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        billElement.lookupVat.ItemsSource = taxes.Where(x => x.isAdjusted == true).ToList();
                    }
                }

            }
        }

        private void ChkAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {
            if(chkNonAdjusted.IsChecked == false)
                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            var billElement = (ucBillData)_bill;
                            billElement.lookupVat.ItemsSource = null;
                        }
                    }

                }
        }

        private void ChkNonAdjusted_Checked(object sender, RoutedEventArgs e)
        {
            chkAdjusted.IsChecked = false;
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;
                        billElement.lookupVat.ItemsSource = taxes.Where(x => x.isAdjusted == false).ToList();
                    }
                }

            }
        }
        
        private void ChkNonAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkAdjusted.IsChecked == false)
                foreach (UIElement element in lstBillData.Items)
                {
                    if (element is UserControl)
                    {
                        var _bill = (UserControl)element;
                        if (_bill.Name == "ucBillDataRow")
                        {
                            var billElement = (ucBillData)_bill;
                            billElement.lookupVat.ItemsSource = null;
                        }
                    }

                }
        }

        private void CmbxCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            DateTime creationDate = (DateTime)datCreationDate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (cmbxCurrency.SelectedIndex != -1 /*&& cmbbaseCurrency.SelectedIndex != -1*/)
                {
                    if (cmbxCompany.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Company!");
                        cmbxCompany.Focus();
                        return;
                    }


                    var cmpny = cmbxCompany.SelectedItem as Company;
                    //var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbxCurrency.SelectedItem as Currency).Id, cmpny.CurrencyId.Value, creationDate.Year);


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

        private void TxtMER_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            AmountMERcalculations();
        }

        private void LstBillData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdSingleAttach.Visibility == Visibility.Visible)
                grdSingleAttach.Visibility = Visibility.Collapsed;
            if (grdSingleAttachments.Visibility == Visibility.Visible)
                grdSingleAttachments.Visibility = Visibility.Collapsed;

            var _bill = (UserControl)lstBillData.SelectedItem;

            if (_bill != null)
            {
                if (_bill.Name == "ucBillDataRow")
                {
                    var billElement = (ucBillData)_bill;
                    if(Convert.ToInt32( (billElement.FindName("txtManagementSummaryId") as TextEdit).Text) > 0)
                    {
                        var textSummaryId = billElement.FindName("txtManagementSummaryId") as TextEdit;
                        lookUpManagementSummary.EditValue = Convert.ToInt32(textSummaryId.Text);
                        chkManagementSummary.IsChecked = true;
                    }
                }
            }
            //else
            //{
            //    DXMessageBox.Show("Please select a Bill first!");
            //}
        }

        private void LookupBank_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var selectedItem = lookupBank.SelectedItem;

            if(selectedItem == null)
            {
                cmbxHolderType.SelectedIndex = -1;
                cmbxPrimaryCardNo.SelectedIndex = -1;
                cmbxSecondaryCardNo.SelectedIndex = -1;
                txtCardUser.Text = null;
                txtCardType.Text = null;
                txtPrimaryCardHolder.Text = null;
                txtSecondaryCardHolder.Text = null;
            }
        }

        private void ChkVehicleType_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;

                        billElement.btnMaintenance.IsEnabled = true;
                        billElement.btnFuel.IsEnabled = true;
                    }
                }
            }
        }

        private void ChkVehicleType_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in lstBillData.Items)
            {
                if (element is UserControl)
                {
                    var _bill = (UserControl)element;
                    if (_bill.Name == "ucBillDataRow")
                    {
                        var billElement = (ucBillData)_bill;

                        billElement.btnMaintenance.IsEnabled = false;
                        billElement.btnFuel.IsEnabled = false;
                    }
                }
            }
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
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
        public void GellAllOrdersTracking()
        {
            if (lstBillData.SelectedItem != null)
            {
                var idd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtId.Text);
                OrderTracking tracking = new OrderTracking();
                //var orders = tracking.getTransactions(idd, TransactionItemType.Admin_Bill);
                grdOrdersTracking.ItemsSource = tracking.getTransactions(idd, TransactionItemType.Admin_Bill);

                gridOrderStageTrack.Visibility = Visibility.Visible;




                //var groupIdd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtGroupId.Text);
                //if (idd != 0)
                //{
                //    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, groupIdd, TransactionItemType.Admin_Bill);
                //    trackingWindow.ShowDialog();
                //}
            }
            else
            {
                DXMessageBox.Show("Select any Bill first!");
            }
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

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId);

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBillAdd.isProgressiveCost = bill.isProgressiveCost;
                                    frmBillAdd.billTypes = bill.billTypes;
                                    frmBill.Content = frmBillAdd;

                                    frmBill.WindowState = WindowState.Maximized;

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
                                frmBillAdd.isProgressiveCost = bill.isProgressiveCost;
                                frmBillAdd.billTypes = bill.billTypes;
                                frmBill.Content = frmBillAdd;

                                frmBill.WindowState = WindowState.Maximized;
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

        private void btnSingleAttachNew_Click(object sender, RoutedEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;

            if (_bill != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Admin Bill") != null)
                {
                    if (grdSingleAttach.Visibility == Visibility.Visible)
                        grdSingleAttach.Visibility = Visibility.Collapsed;
                    else
                    {
                        grdSingleAttach.Visibility = Visibility.Visible;
                        cmbSingleCategory.ItemsSource = SYSTEM_STATIC.GetActiveABAttachmentCategories();
                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to Attach File!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select a Bill first!");
            }
        }

        private void btnSingleAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;

            if (_bill != null)
            {
                var billElement = (ucBillData)_bill;
                var _billId = Convert.ToInt32(billElement.txtId.Text);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Admin Bill") != null)
                {
                    if (grdSingleAttachments.Visibility == Visibility.Visible)
                        grdSingleAttachments.Visibility = Visibility.Collapsed;
                    else
                    {
                        if (editFlag != false)
                        {
                            if (lstBillData.SelectedItem != null)
                            {
                                List<TreeItem> otherAttachments = new List<TreeItem>();
                                List<TreeItem> atachments = SYSTEM_STATIC.GetAdminBillAttachmentsListByCategory(_billId, TransactionItemType.Admin_Bill);
                                var idd = Convert.ToInt32((lstBillData.SelectedItem as ucBillData).txtId.Text);
                                AdminBill bill = BillsRepo.get(idd);
                                if (bill.Payments.Count != 0)
                                {
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetAdminBillAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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
                                treeViewSingleAttachments.ItemsSource = atachments;
                                grdSingleAttachments.Visibility = Visibility.Visible;
                            }
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to View List of Attached Files!");
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                });
            }
            else
            {
                DXMessageBox.Show("Please select a Bill first!");
            }
        }

        private void btnSingleAttachment_Click(object sender, RoutedEventArgs e)
        {
            var _bill = (UserControl)lstBillData.SelectedItem;
            var billElement = (ucBillData)_bill;
            var _billId = Convert.ToInt32(billElement.txtId.Text);
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbSingleCategory.SelectedItem != null)
            {
                if (_billId != 0)
                {
                    try
                    {
                        int CategoryId = (cmbSingleCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\Admin_Bill\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += _billId + "_" + TransactionItemType.Admin_Bill.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Admin_Bill);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), _billId, TransactionItemType.Admin_Bill, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, _billId, 17, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewSingleAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(_billId, TransactionItemType.Admin_Bill);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
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

        private void BtnSingleDownloadAttachment_Click(object sender, RoutedEventArgs e)
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

        private void btnSingleAttachClose_Click(object sender, RoutedEventArgs e)
        {
            grdSingleAttach.Visibility = Visibility.Collapsed;
        }

        private void btnSingleAttachmentsClose_Click(object sender, RoutedEventArgs e)
        {
            grdSingleAttachments.Visibility = Visibility.Collapsed;
        }
    }
}
