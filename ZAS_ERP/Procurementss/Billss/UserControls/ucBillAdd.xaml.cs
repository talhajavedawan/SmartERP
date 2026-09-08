using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System.Data;
using ZAS_ERP.Procurementss.SaleOrderss;
using System.Text.RegularExpressions;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Core;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.Diagnostics;
using ZAS_ERP.Utils;
using ERP_BL.CreditCards;
using ERP_BL.Tax;
using ERP_BL.ChartofAccounts;
using System.Threading.Tasks;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ERP_BL.Procurements.Bill;
using ZAS_ERP.Bankings;
using ZAS_ERP.Procurementss.Budget;
using ERP_BL.Payments;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.Procurementss.Adjustments.UserControls;
using ERP_BL.Procurements.LoansAdvances;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ERP_BL.CashBook;
using DevExpress.Data.Filtering;

namespace ZAS_ERP.Procurementss.Billss
{


    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucBillAdd : UserControl
    {
        public static int editbill;
        public static int billid;
        public int OrderId;
        public int editOrder;
        public static int saleOrderid;
        public static int purchaseOrderid;
        SaleOrder saleOrder = new SaleOrder();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        BillRepo billRepo = new BillRepo();
        Bill bill = new Bill();
        Vendor vendor = new Vendor();
        Vendor billVendor = new Vendor();
        Vendor POVendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        Department InterDepartment = new Department();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        CustomerCompany customer = new CustomerCompany();
        public bool isloading = false;
        public virtual List<ProcurementProduct> procurementProducts { get; set; }

        public virtual List<VendorBillAdjustment> adjustments { get; set; }
        public virtual List<Product> products { get; set; }
        Principal principal = new Principal();
        Currency currency = new Currency();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        UsersRepo UsersRepo = new UsersRepo();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();
        public BillStatus checkStatus = new BillStatus();
        public double POCFRRemaining = 0;
        BillStatus oldStatus = new BillStatus();
        public virtual List<CostSheetField> costSheetFields { get; set; }
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        List<Company> currentUserCompanies;

        Window memoWindow = new Window();
        ucFrmAddMemo frmAddMemo = new ucFrmAddMemo();
        ChartofAccountsRepo chartofAccountRepo = new ChartofAccountsRepo();
        public virtual List<BillItem> billItems { get; set; }
        TaxRepo taxRepo = new TaxRepo();
        List<CostSheetBillField> costSheetBillFields = new List<CostSheetBillField>();
        List<CostSheetSOField> costSheetSOFields = new List<CostSheetSOField>();
        List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        List<CostFieldValues> costfieldCheckedValues = new List<CostFieldValues>();
        public SaleOrderRepo repo = new SaleOrderRepo();
        Bill trackingOrder = new Bill();
        List<BillStatus> BillStatuses = new List<BillStatus>();

        public int loansAdvanceId = 0;
        public ucBillAdd()
        {
            adjustments = new List<VendorBillAdjustment>();
            procurementProducts = new List<ProcurementProduct>();
            billItems = new List<BillItem>();
            products = new List<Product>();
            costSheetFields = new List<CostSheetField>();
            InitializeComponent();
            symbol = "";
        }
        private void winBilladd_Loaded(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            grdTrackingTree.ExpandAllNodes();
            try
            {
                OrderId = billid;
                editOrder = editbill;
                if (OrderId == 0)
                {
                    BillRepo billsrepo = new BillRepo();
                    string today = DateTime.Today.ToString("ddMMyyyy");
                    int seq = billsrepo.getTodayBillCount();
                    seq++;
                    txtSystemRef.Text = string.Format("Bill-{0}-{1}", today, seq.ToString("D4"));

                }
                grdBillItems.ItemsSource = billItems;
                grdPOItems.ItemsSource = procurementProducts;
                
                costSheetFields = SYSTEM_STATIC.GetCostSheetFields();
                lookupCostSheetFieldsinGrid.ItemsSource = costSheetFields;

                lookupBillCostSheetFieldsinGrid.ItemsSource = costSheetFields;

                loadCardUsers();
                loadcompanies();
                loadBillTypes();
                var templateList = cmbBillType.ItemsSource as List<cmbitem>;
                cmbBillType.SelectedItem = templateList.FirstOrDefault(x => x.name == "New-Bills-Direct COA");
                datglPostingdate.EditValue = System.DateTime.Now;
                loadWarrantys();
                loadPaymentTerms();
                loadIncoterms();

                loadCurrencies();
                loadBillStatus();
                loadVendorPaymentStatus();
                loadBillCategories();
                loadManagementSummaries();
                TaxRepo taxRepo = new TaxRepo();
                var taxes = taxRepo.getAllTaxes();
                //lookUpTax.ItemsSource = taxes;
                lookUpWHT.ItemsSource = taxes;
                CheckPermissions();

            }
            //comment
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                DXMessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void CheckPermissions()
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Reference Number in Vendor Bill") != null)
            {
                cmbxBillRef.IsEnabled = true;
            }
            else
            {
                cmbxBillRef.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Bill") != null)
            {
                datpoCreationdate.IsEnabled = true;
            }
            else
            {
                datpoCreationdate.IsEnabled = false;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of VendorBill") != null)
            {
                datglPostingdate.IsEnabled = true;
            }
            else
            {
                datglPostingdate.IsEnabled = false;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Bill") != null)
            {
                //btnAttachNew.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachNew.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Bill") != null)
            {
                btnAttachmentList.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachmentList.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Marketing Cost in Summary Sheet") != null)
            {
                txtNetBaseCommission.Visibility = Visibility.Visible;
                txtNetCommision.Visibility = Visibility.Visible;
                lblNetCommision.Visibility = Visibility.Visible;
                txtNetSalesCommision.Visibility = Visibility.Visible;
            }
            else
            {
                txtNetBaseCommission.Visibility = Visibility.Collapsed;
                txtNetCommision.Visibility = Visibility.Collapsed;
                lblNetCommision.Visibility = Visibility.Collapsed;
                txtNetSalesCommision.Visibility = Visibility.Collapsed;
            }
            if (editbill == 1 && billid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill") == null)
                {
                    isloading = true;
                    bill = billRepo.get(billid);
                    loadonBilldata();
                    GellAllOrdersTracking();
                    //views = _usersRepo.getViwerInfo(bill.Id, (int)TransactionItemType.Bill);
                    //grdUsers.ItemsSource = views;
                    //loadcomments();
                    btnSave.IsEnabled = false;
                    if (bill.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Bill") != null)
                    {
                        btnSave.IsEnabled = true;
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill") != null)
                {
                    isloading = true;
                    bill = billRepo.get(billid);
                    loadonBilldata();
                    GellAllOrdersTracking();
                    //views = _usersRepo.getViwerInfo(bill.Id, (int)TransactionItemType.Bill);
                    //grdUsers.ItemsSource = views;
                    //loadcomments();
                    btnSave.IsEnabled = true;
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill") != null)
                {
                    isloading = true;
                    bill = billRepo.get(billid);
                    loadonBilldata();
                    GellAllOrdersTracking();
                    //views = _usersRepo.getViwerInfo(bill.Id, (int)TransactionItemType.Bill);
                    //grdUsers.ItemsSource = views;
                    //loadcomments();
                    btnSave.IsEnabled = true;

                }
                //Setting void stamp
                if (bill != null)
                {
                    if (bill.isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Bill !");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else if (editbill == 0 && saleOrderid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill") != null)
                {
                    isloading = true;
                    saleOrder = saleOrderRepo.get(saleOrderid);
                    loadonSaleOrderdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Bill !");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else if (editbill == 0 && purchaseOrderid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill") != null)
                {
                    isloading = true;
                    purchaseOrder = purchaseOrderRepo.get(purchaseOrderid);
                    loadonPurchaseOrderdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Bill !");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else
            {
                datpoCreationdate.EditValue = System.DateTime.Now;

                cmbcaption1.SelectedIndex = 0;
                cmbcaption2.SelectedIndex = 1;
            }
            //calculatetotal();
            isloading = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Market Exchange Rate in Bill") != null)
            {
                lblexchangerate.Visibility = Visibility.Visible;
                txtexchangerate.Visibility = Visibility.Visible;
            }
            else
            {
                lblexchangerate.Visibility = Visibility.Collapsed;
                txtexchangerate.Visibility = Visibility.Collapsed;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Bill") != null)
            {
                btnSetVoid.Visibility = Visibility.Visible;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Move Bill to Inter Company") != null)
            {
                gridInterCompanyDetails.IsEnabled = true;
                chkInterCompany.IsEnabled = true;
            }
            else
            {
                gridInterCompanyDetails.IsEnabled = false;
                chkInterCompany.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
            {
                btnPushDebits.IsEnabled = true;
                btnPushCredits.IsEnabled = true;
            }
        }
        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();

                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;

            }
            currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupCompany.ItemsSource = currentUserCompanies;
            lookupInterCompany.ItemsSource = currentUserCompanies;
            lookupLoanAdvanceCompany.ItemsSource = currentUserCompanies;
        }
        public void loadvendors()
        {
            lookupPayeeVendor.ItemsSource = department.Vendors;
            lookupBillVendor.ItemsSource = department.Vendors;
        }
        public void loadcustomers()
        {
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                        List<CustomerCompany> customers = /*customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id)*/ department.customers.ToList();
                        if (editbill == 1)
                        {
                            if (bill.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == bill.customerCompany_Id);
                                if (customer != null)
                                {
                                    customers.AddRange(department.disableCustomers);
                                }
                            }
                        }
                        if (customers == null || customers.Count == 0)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select a different Department and Company! No customer is mapped to this department or Company.", "Select another Department or Company", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else
                        {
                            lookupCustomer.ItemsSource = customers;
                            return;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Department First!", "Select Department to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Company First!", "Select Company to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
        }
        public void loadCardUsers()
        {
            CreditCardRepo cardRepo = new CreditCardRepo();
            cmbxCardUser.ItemsSource = cardRepo.GetAllCardHolders();
        }
        public void loadBillTypes()
        {
            cmbBillType.ItemsSource = SYSTEM_STATIC.billTypesSource;
        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            employees = department.employees;
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbEmployee.ItemsSource = cmbitems;
            cmbTransactionHolder.ItemsSource = cmbitems;

        }
        public void loaddepartments()
        {
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsVendorBillType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (bill != null && bill.Id > 0 && editbill == 1)
                        if (bill.department != null && departments.FirstOrDefault(x => x.Id == bill.dept_Id) == null)
                            departments.Add(bill.department);

                    lookupDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
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
            if (InterCompany != null)
                if (InterCompany.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsVendorBillType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == InterCompany.Id) != null)
                            departments.Add(_dept);
                    }
                    if (bill != null && bill.Id > 0 && editbill == 1)
                        if (bill.InterDepartment != null && departments.FirstOrDefault(x => x.Id == bill.InterDepartment_Id) == null)
                            departments.Add(bill.InterDepartment);
                    lookupInterDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }


        private void MbtnAddAdjustment_Click(object sender, RoutedEventArgs e)
        {
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Adjustments") != null)
            //{
            //    if (loansAdvanceId != 0)
            //    {
            //        ucVendorBillAdjustments adjustmentWindow = new ucVendorBillAdjustments();
            //        adjustmentWindow.Width = 550;
            //        adjustmentWindow.Height = 450;
            //        adjustmentWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    
            //        if (bill != null)
            //        {
            //            if (editbill == 1)
            //            {

                           
            //                if (bill.Id > 0)
            //                    adjustmentWindow.billId = Convert.ToInt32(bill.Id);

            //            }
            //            adjustmentWindow.ShowDialog();


            //            if (editbill == 1)
            //            {
            //                bill.Adjustments = new List<Adjustment>();
            //                foreach (var _adjustment in adjustmentWindow.adjustments)
            //                {
            //                    bill.Adjustments.Add(new Adjustment()
            //                    {
            //                        Id = _adjustment.Id,
            //                        AdjustmentDate = _adjustment.AdjustmentDate,
            //                        billId = bill.Id,
            //                        AdjustmentAmount = _adjustment.AdjustmentAmount,
            //                        ReferenceNo = _adjustment.ReferenceNo,
            //                        isApproved = _adjustment.isApproved,
            //                        ApprovedDate = _adjustment.ApprovedDate,
            //                        loansAdvanceType= LoansAdvanceType.Vendor_Bill
            //                    });
            //                }
            //                var aprvdAdjustments = bill.Adjustments.Where(x => x.isApproved == true);
            //                var unaprvdAdjustments = bill.Adjustments.Where(x => x.isApproved == false);
            //                txtApprovedLoanAdjustment.Text = aprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
            //                txtUnApprovedLoanAdjustment.Text = unaprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
            //                txtApprovedAdjustmentCount.Text = aprvdAdjustments.Count().ToString();
            //                txtUnApprovedAdjustmentCount.Text = unaprvdAdjustments.Count().ToString();
            //            }
            //            else
            //            {
                            
            //                bill.Adjustments = new List<Adjustment>();
            //                foreach (var _adjustment in adjustmentWindow.adjustments)
            //                {
            //                    bill.Adjustments.Add(new Adjustment()
            //                    {
            //                        Id = _adjustment.Id,
            //                        billId = 0,
            //                        AdjustmentDate = _adjustment.AdjustmentDate,
            //                        AdjustmentAmount = _adjustment.AdjustmentAmount,
            //                        ReferenceNo = _adjustment.ReferenceNo,
            //                        isApproved = _adjustment.isApproved,
            //                        ApprovedDate = _adjustment.ApprovedDate
            //                    });
            //                }
            //                var aprvdAdjustments = bill.Adjustments.Where(x => x.isApproved == true);
            //                var unaprvdAdjustments = bill.Adjustments.Where(x => x.isApproved == false);
            //                txtApprovedLoanAdjustment.Text = aprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
            //                txtUnApprovedLoanAdjustment.Text = unaprvdAdjustments.Sum(x => x.AdjustmentAmount).ToString();
            //                txtApprovedAdjustmentCount.Text = aprvdAdjustments.Count().ToString();
            //                txtUnApprovedAdjustmentCount.Text = unaprvdAdjustments.Count().ToString();
            //            }
            //        }
            //        else
            //        {
            //            DXMessageBox.Show("Please select the Bill to View/Add Adjustments");
            //        }
            //    }
            //    else
            //    {
            //        DXMessageBox.Show("This Bill is not generated from Loans Advances");
            //    }
            //}
            //else
            //{
            //    DXMessageBox.Show("Permission Required to Add Adjustments!");
            //}
        }
        public void loadonPurchaseOrderdata()
        {
            if (saleOrder.Bills != null)
                lblPOrefrence.Text = purchaseOrder.SOReferenceNo + " - " + (purchaseOrder.Bills?.Count() + 1);
            else
                lblPOrefrence.Text = purchaseOrder.SOReferenceNo + " - " + 1;
            bill.saleOrder_Id = purchaseOrder.saleOrder_Id;
            bill.purchaseOrder_Id = purchaseOrder.Id;
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseOrder.Id, TransactionItemType.Purchase_Order);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            bill.CostSheet_Id = purchaseOrder.CostSheet_Id;
            lblLastStatusChangeDate.EditValue = purchaseOrder.LastStatusChangeDate;
            datsaleOrderdate.EditValue = purchaseOrder.PurchaseOrderDate;
            datpoCreationdate.EditValue = purchaseOrder.CreationDate;
            //datglPostingdate.EditValue = purchaseOrder.CreationDate;
            datShipmentdate.EditValue = purchaseOrder.ShipmentDate;
            datRevisedShipmentDate.EditValue = purchaseOrder.RevisedShipmentDate;
            datExcpectedPaymentDate.EditValue = purchaseOrder.ExpectedPayment;
            datPaymentDueTill.EditValue = purchaseOrder.PaymentDueAgeing;
            datDeliverydate.EditValue = purchaseOrder.DeliveryDate;
            datOrderConfirmationdate.EditValue = purchaseOrder.OrderConfirmationDate;
            datBillOfLaddingdate.EditValue = purchaseOrder.BillOfLaddingDate;
            datLCDatedate.EditValue = purchaseOrder.lCDate;
            datMaterialReciptdate.EditValue = purchaseOrder.MaterialReciptDate;
            datPaymentDueFrom.EditValue = purchaseOrder.PaymentDueStartDate;
            txtSalesref.Text = purchaseOrder.SalesReferenceNo;
            txtOfferRefNo.Text = purchaseOrder.OfferReferenceNo;
            txtsaleOrderref.Text = purchaseOrder.SOReferenceNo;
            {
                datLCShipmentDate.EditValue = purchaseOrder.LCShipmentDate;
                datLCRevisedShipmentDate.EditValue = purchaseOrder.LCShipmentAmendmentDate;
                datLCExpiryDate.EditValue = purchaseOrder.LCExpiryDate;
                datLCExpiryDate.EditValue = purchaseOrder.LCExpiryAmedmentDate;
                txtLCAmedmentno.Text = (string.IsNullOrEmpty(purchaseOrder.LCAmedmentNo)) ? "" : purchaseOrder.LCAmedmentNo;
                txtPacking.Text = (string.IsNullOrEmpty(purchaseOrder.packing)) ? "" : purchaseOrder.packing;
                if (purchaseOrder.transshipment == true)
                    cmbTransshipment.SelectedIndex = 0;
                if (purchaseOrder.transshipment == false)
                    cmbTransshipment.SelectedIndex = 1;
            }
            double invoiced = 0;
            if (purchaseOrder.PurchaseOrdertype == InquiryType.Principal)
            {
                POCFRRemaining = Convert.ToDouble(purchaseOrder.Commision);
                if (purchaseOrder.InvoiceStage == null)
                {
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(purchaseOrder.Commision);
                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;
                }
                else if (purchaseOrder.InvoiceStage == InvoiceStage.None.ToString() && purchaseOrder.RemainingCFRValue == 0)
                {
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(purchaseOrder.Commision);
                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;
                }
                else
                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;
            }
            else
            {
                POCFRRemaining = purchaseOrder.totalCFRValue;
                if (purchaseOrder.InvoiceStage == null)
                {
                    purchaseOrder.RemainingCFRValue = purchaseOrder.totalCFRValue;
                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;
                }
                else if (purchaseOrder.InvoiceStage == InvoiceStage.None.ToString() && purchaseOrder.RemainingCFRValue == 0)
                {
                    purchaseOrder.RemainingCFRValue = purchaseOrder.totalCFRValue;
                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;
                }
                else
                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;
            }
            txtSOCER.Text = purchaseOrder.SOC_ER.ToString();
            txttotalfob.Text = purchaseOrder.totalFOBValue.ToString();
            txtPOremainingcfr.Text = Math.Round(purchaseOrder.RemainingCFRValue, 2).ToString();
            txttotalcfr.Text = purchaseOrder.totalCFRValue.ToString();
            txtBasetotalfob.Text = purchaseOrder.totalBaseFOBValue.ToString();
            txtBasetotalcfr.Text = purchaseOrder.totalBaseCFRValue.ToString();
            txtSalestotalCfr.Text = purchaseOrder.POAmountSER.ToString();
            txtMaker.Text = purchaseOrder.maker;
            txtOrigin.Text = purchaseOrder.origin;
            txtOwnDescription.Text = purchaseOrder.OwnDescription;
            txtPOVendorName.Text = purchaseOrder.VendorName;
            txtFinanaceRef.Text = purchaseOrder.FinanceRefrenceNo;
            txtComments.Text = purchaseOrder.comments;
            txtPaymentDueDays.Text = purchaseOrder.CreditDays.ToString();
            //lblStage.Text = (purchaseOrder.stage != null) ? purchaseOrder.stage : "";
            txtLCNumber.Text = purchaseOrder.LCnumber;
            if (purchaseOrder.incoterm_Id != 0 || purchaseOrder.incoterm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.incoterm_Id))];
            }
            //Warranty
            if (purchaseOrder.SOWarrantyId != null /*|| purchaseOrder.SOWarranty != null*/)
            {
                var incoSource = (List<cmbitem>)cmbSOWarranty.Items.SourceCollection;
                cmbSOWarranty.SelectedItem = cmbSOWarranty.Items[cmbSOWarranty.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.SOWarrantyId))];
            }
            if (purchaseOrder.POWarrantyId != 0 || purchaseOrder.POWarranty != null)
            {
                var incoSource = (List<cmbitem>)cmbPOWarranty.Items.SourceCollection;
                cmbPOWarranty.SelectedItem = cmbPOWarranty.Items[cmbPOWarranty.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.POWarrantyId))];
            }
            //select Captions for Item Value 1
            if (purchaseOrder.TitleValue1Id != 0 || purchaseOrder.TitleValue1 != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == purchaseOrder.TitleValue1Id))];
            }
            //select Captions for Item Value 2
            if (purchaseOrder.TitleValue2Id != 0 || purchaseOrder.TitleValue2 != null)
            {
                var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == purchaseOrder.TitleValue2Id))];
            }
            //Payment term Load for sale Order
            if (purchaseOrder.SoPaymentterm_Id != 0 && purchaseOrder.SoPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbSOPaymentTerm.Items.SourceCollection;
                //cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseOrder.SoPaymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseOrder.SoPaymentTerm.term, id = purchaseOrder.SoPaymentTerm.Id });
                    cmbSOPaymentTerm.ItemsSource = null;
                    cmbSOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(term)];
            }
            //Payment term Load for sale Order
            if (purchaseOrder.POPaymentterm_Id != 0 || purchaseOrder.POPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPOPaymentTerm.Items.SourceCollection;
                //cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseOrder.SoPaymentTerm.term, id = purchaseOrder.SoPaymentTerm.Id });
                    cmbSOPaymentTerm.ItemsSource = null;
                    cmbSOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(term)];
            }
            if (purchaseOrder.SOCurrency_Id != 0 || purchaseOrder.SOCurrency != null)
            {
                var currencySource = (List<cmbitem>)cmbSOCurrency.Items.SourceCollection;
                cmbSOCurrency.SelectedItem = cmbSOCurrency.Items[cmbSOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.SOCurrency_Id))];
            }
            if (purchaseOrder.currency_Id != 0 || purchaseOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.currency_Id))];
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.company.CurrencyId))];
            }
            if (purchaseOrder.saleOrder_Id != null)
                saleOrder = purchaseOrder.SaleOrder;

            // Select Company
            if (purchaseOrder.company_Id != null || purchaseOrder.company != null)
            {
                company = purchaseOrder.company;
                lookupCompany.Text = purchaseOrder.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";
            }
            // Select Department
            if (purchaseOrder.dept_Id != 0 || purchaseOrder.department != null)
            {
                lookupDepartment.Text = purchaseOrder.department.DeptName;
                department = purchaseOrder.department;

                lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (purchaseOrder.customerCompany.Id != 0 || purchaseOrder.customerCompany != null)
            {
                lookupCustomer.Text = purchaseOrder.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(purchaseOrder.customerCompany);

                customer = purchaseOrder.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";
            }
            //Select vendor
            if (purchaseOrder.vendors != null)
            {
                foreach (Vendor vendr in purchaseOrder.vendors)
                {
                    lookupPOVendor.Text = vendr.company.CompanyName;
                    POVendor = vendr;
                }
            }
            // Select Employee 
            if (purchaseOrder.allocation_Id != 0 || purchaseOrder.AllocateTo != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == purchaseOrder.allocation_Id))];
                }
                catch (Exception ex)
                {
                }
            }
            if (purchaseOrder.bid_Id != null && purchaseOrder.bid_Id != 0 && purchaseOrder.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = purchaseOrder.bid.issueDate;
                txtBidBondRefno.Text = purchaseOrder.bid.refNo;
                txtBidBonValue.Text = purchaseOrder.bid.value;
                datBidBondSubmitdate.DateTime = purchaseOrder.bid.submitDate;
                datBidBondExpirydate.DateTime = purchaseOrder.bid.expireDate;
                txtIssuingbank.Text = purchaseOrder.bid.bankName;
            }
            if (purchaseOrder.vendorPaymentId != null || purchaseOrder.vendorPaymentStatus != null)
            {
                var currencySource = (List<cmbitem>)cmbVendorPaymentStatus.Items.SourceCollection;
                cmbVendorPaymentStatus.SelectedItem = cmbVendorPaymentStatus.Items[cmbVendorPaymentStatus.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.vendorPaymentId))];
            }
            List<DataGridItem> datagriditems = new List<DataGridItem>();
            if (purchaseOrder.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                foreach (var procurementProduct in purchaseOrder.products)
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
                            },
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
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim(),
                        UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                        AmountSOC = procurementProduct.AmountSOC
                    });
                }
                grdPOItems.ItemsSource = procurementProducts;
                int x = 0;
                foreach (var pro in grdPOItems.ItemsSource as List<ProcurementProduct>)
                {
                    grdPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                    x++;
                }
                lookupProductsinGrid.DisplayMember = "code";
            }
            else
            {
                grdPOItems.ItemsSource = procurementProducts;
            }
            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == purchaseOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            txtexchangerate.Text = purchaseOrder.ExchangeRate.ToString();
            txtMarginexchangerate.Text = purchaseOrder.marginExchangeRate.ToString();
            txtCommision.Text = purchaseOrder.Commision.ToString();
            txtBaseCommission.Text = purchaseOrder.commisioninBase.ToString();
            //Margin
            txtBudgetMargin.Text = purchaseOrder.margin.ToString();
            txtRevisedMargin.Text = purchaseOrder.RevisedMargin.ToString();
            txtBaseRevisedMargin.Text = purchaseOrder.RevisedMargininBase.ToString();
            txtSaleRevisedMargin.Text = purchaseOrder.SalesRevisedMargin.ToString();
            txtActualMargin.Text = purchaseOrder.ActualMargin.ToString();
            txtBaseBudgetMargin.Text = purchaseOrder.BudgetedMargininBase.ToString();
            txtBaseActualMargin.Text = purchaseOrder.ActualMargininBase.ToString();
            txtSaleBudgetMargin.Text = purchaseOrder.SalesBudgetedMargin.ToString();
            txtSaleAMargin.Text = purchaseOrder.SalesActualMargin.ToString();
            txtNetCommision.Text = (purchaseOrder.NetCommision).ToString();
            calculatetotal();
            btnPushCredits.IsChecked = true;
            btnPushDebits.IsChecked = true;
            lookupDepartment.IsEnabled = false;
            lookupCompany.IsEnabled = false;
            lookupCustomer.IsEnabled = false;
            var templateList = cmbBillType.ItemsSource as List<cmbitem>;
            cmbBillType.SelectedItem = templateList.FirstOrDefault(x => x.name == "Linked-Bills-Direct COA");
            cmbBillType.IsEnabled = false;
        }
        public void loadonSaleOrderdata()
        {
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            bill.CostSheet_Id = saleOrder.CostSheet_Id;
            datpoCreationdate.EditValue = System.DateTime.Now;
            datsaleOrderdate.EditValue = saleOrder.saleOrderDate;
            datDeliverydate.EditValue = saleOrder.deliveryDate;
            txtSalesref.Text = saleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = saleOrder.offerReferenceNo;
            txtFinanaceRef.Text = saleOrder.FinanceRefrenceNo;
            txtsaleOrderref.Text = saleOrder.referenceNo;
            datglPostingdate.EditValue = System.DateTime.Now;
            if (saleOrder.Bills != null)
                lblPOrefrence.Text = saleOrder.referenceNo + " - " + (saleOrder.Bills?.Count() + 1);
            else
                lblPOrefrence.Text = saleOrder.referenceNo + " - " + 1;
            txtOwnDescription.Text = saleOrder.OwnDescription;
            //Warranty
            if (saleOrder.WarrantyId != null && saleOrder.Warranty != null)
            {
                var incoSource = (List<cmbitem>)cmbSOWarranty.Items.SourceCollection;
                cmbSOWarranty.SelectedItem = cmbSOWarranty.Items[cmbSOWarranty.Items.IndexOf(incoSource.Find(x => x.id == saleOrder.WarrantyId))];
            }
            //select Captions for Item Value 1
            if (saleOrder.TitleValue1Id != null || saleOrder.TitleValue1 != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == saleOrder.TitleValue1Id))];
            }
            //select Captions for Item Value 2
            if (saleOrder.TitleValue2Id != null || saleOrder.TitleValue2 != null)
            {
                var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == saleOrder.TitleValue2Id))];
            }
            //Payment term Load for sale Order
            if (saleOrder.paymentterm_Id != 0 && saleOrder.paymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbSOPaymentTerm.Items.SourceCollection;
                //cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id))];
                var term = paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleOrder.paymentTerm.term, id = saleOrder.paymentterm_Id });
                    cmbSOPaymentTerm.ItemsSource = null;
                    cmbSOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(term)];
            }
            if (saleOrder.currency_Id != 0 && saleOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbSOCurrency.Items.SourceCollection;
                cmbSOCurrency.SelectedItem = cmbSOCurrency.Items[cmbSOCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.currency_Id))];
                cmbSOCurrency.IsEnabled = false;
            }
            // Select Company
            if (saleOrder.company_Id != null || saleOrder.company != null)
            {
                company = saleOrder.company;
                lookupCompany.Text = saleOrder.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (saleOrder.dept_Id != 0 || saleOrder.department != null)
            {
                lookupDepartment.Text = saleOrder.department.DeptName;
                department = saleOrder.department;

                lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (saleOrder.customerCompany?.Id != 0 || saleOrder.customerCompany != null)
            {
                lookupCustomer.Text = saleOrder.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(saleOrder.customerCompany);
                customer = saleOrder.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            //if (saleOrder.vendors != null)
            //{

            //    foreach (Vendor vendr in saleOrder.vendors)
            //    {
            //        lookupVendor.Text = vendr.company.CompanyName;
            //        vendor = vendr;
            //    }
            //}

            if (saleOrder.allocation_Id != 0 || saleOrder.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == saleOrder.allocation_Id))];
                }
                catch (Exception ex)
                {
                }
            }
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();

            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == saleOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            //calculatetotal();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Commision Field without Approval") == null)
            {
                txtCommision.IsReadOnly = true;
            }
            grdPOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
            grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
            btnPushDebits.IsChecked = true;
            btnPushCredits.IsChecked = true;
            lookupDepartment.IsEnabled = false;
            lookupCompany.IsEnabled = false;
            lookupCustomer.IsEnabled = false;
            var templateList = cmbBillType.ItemsSource as List<cmbitem>;
            cmbBillType.SelectedItem = templateList.FirstOrDefault(x => x.name == "Linked-Bills-Direct COA");
            cmbBillType.IsEnabled = false;
        }

        private void BillStages()
        {
            if (bill.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (bill.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (bill.isApproved == true && bill.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (bill.isApproved == true && bill.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (bill.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (bill.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (bill.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void LoadPaymentsStatuses()
        {
            List<cmbitem> pymntStages = new List<cmbitem>();
            foreach (var _pymnt in bill.Payments)
            {
                if (_pymnt.isApproved == true && _pymnt.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Closed)"
                    });
                }
                else if (_pymnt.isApproved == true && _pymnt.Status.isActive == false && _pymnt.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Closed)"
                    });
                }
                else if (_pymnt.isApproved == true && _pymnt.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Under Closing)"
                    });
                }
                else if (_pymnt.isApproved == true)
                {
                    //lblStage.Text = "Approved";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Approved)"
                    });
                }
                else if (_pymnt.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Under Approval)"
                    });
                }
                else if (_pymnt.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";
                    pymntStages.Add(new cmbitem()
                    {
                        id = _pymnt.transactionGroupId,
                        name = _pymnt.SystemRefNo + " (Under Closing)"
                    });
                }
            }

            cmbPaymentsStages.ItemsSource = pymntStages;
        }

        public void loadonBilldata()
        {
            BillStages();
            LoadPaymentsStatuses();

            //Select Company
            var summaryList = (lookUpManagementSummary.ItemsSource as List<ManagementSummary>) == null ? new List<ManagementSummary>() : lookUpManagementSummary.ItemsSource as List<ManagementSummary>;
            if (bill.managementSummary_Id != null)
            {
                int indexx = 0;
                foreach (var _summary in summaryList)
                {
                    if (_summary.Id == bill.managementSummary_Id)
                    {
                        lookUpManagementSummary.SelectedIndex = indexx;
                        chkManagementSummary.IsChecked = true;
                        indexx = 0;
                        break;
                    }
                    indexx++;
                }
            }

            

            if (bill.LoansAdvanceId != null && bill.LoansAdvanceId != 0)
            {
                loansAdvanceId = bill.LoansAdvanceId.Value;

            
                loansAdvanceId = bill.LoansAdvanceId.Value;

                adjustments = bill.Adjustments.ToList();
            }


            var creditJournalTransactions = bill.journalTransactions.Where(x => x.credit != 0).ToList();
            var debitJournalTransactions = bill.journalTransactions.Where(x => x.debit != 0).ToList();
            if (creditJournalTransactions.Count > 0)
            {
                btnPushCredits.IsChecked = true;
            }
            if (debitJournalTransactions.Count > 0)
            {
                btnPushDebits.IsChecked = true;
            }
            if (bill.GLPostingDate != null)
            {
                datglPostingdate.EditValue = bill.GLPostingDate;
            }
            else
            {
                datglPostingdate.EditValue = bill.CreationDate;
            }
            //if (bill.journalTransactions!=null && bill.journalTransactions.Count>0)
            //{
            //    btnPushtoGL.IsChecked = true;
            //}

            if (!String.IsNullOrEmpty(bill.SummaryMemo))
                txtSummaryMemo.Text = bill.SummaryMemo.ToString();

            if (bill.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Vendor Bill Reference Number After Approval") == null)
            {
                cmbxBillRef.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Vendor Bills") != null)
            {
                grdMangementSummaryMemo.Visibility = Visibility.Visible;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Link Vendor Bill with Loan Advance") != null)
            {
                layoutGrpLoanAdvanceLinking.IsEnabled = true;
            }

            if (bill.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Vendor After Approval") == null)
            {
                lookupPayeeVendor.IsEnabled = false;
                //txtVendorName.IsEnabled = false;

                lookupBillVendor.IsEnabled = false;
            }
            else if (bill.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Vendor After Approval") != null)
            {
                lookupPayeeVendor.IsEnabled = true;
                //txtVendorName.IsEnabled = true;
                lookupBillVendor.IsEnabled = true;
            }


            var paidAmount = Math.Round( bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount), 2) /*+ bill.Payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
            var adjustedAmount = Math.Round(bill.Adjustments.Where(x => x.isApproved == true).Sum(y => y.AdjustmentAmount), 2);
            double percentPaid = 0;
            if (bill.billWithTax != null && bill.billWithTax != 0)
                percentPaid = Math.Round((paidAmount / Convert.ToDouble(bill.billWithTax)) * 100, 2);
            else if (bill.totalCFRValue != 0)
                percentPaid = Math.Round((paidAmount / Convert.ToDouble(bill.totalCFRValue)) * 100, 2);
            pbarTarget.Value = percentPaid;
            if (percentPaid == 100)
            {
                grdFullyPaid.Visibility = Visibility.Visible;
                txtFullyPaid.RenderTransform = new RotateTransform(-35);
            }

            if (bill.billCategory != null)
            {
                var billCategories = (lookupBillCategory.ItemsSource as List<BillCategory>) == null ? new List<BillCategory>() : lookupBillCategory.ItemsSource as List<BillCategory>;
                if (bill.billCategory != null)
                {
                    int indexx = 0;
                    foreach (var _category in billCategories)
                    {
                        if (_category.Id == bill.billCategoryId)
                        {
                            lookupBillCategory.SelectedIndex = indexx;
                            indexx = 0;
                            break;
                        }
                        indexx++;
                    }
                }
                //lookupBillCategory.Text = bill.billCategory.Category;
            }

            txtPaidAmount.Text = paidAmount.ToString();
            txtAdjustedAmount.Text = adjustedAmount.ToString();

            double unpaidAmount = 0;
            if (bill.billWithTax != null)
                unpaidAmount = Math.Round((bill.billWithTax.Value - paidAmount - adjustedAmount), 2);
            else
                unpaidAmount = Math.Round((bill.totalCFRValue - paidAmount - adjustedAmount), 2);

            txtUnPaidAmount.Text = unpaidAmount.ToString();

            if (unpaidAmount <= 0 && bill.Payments != null && bill.Payments.Count > 0)
            {
                var paymentCount = bill.Payments.Where(x => x.isVoid != true).Count();

                if (bill.Payments.Where(x => x.isApproved == true && x.isVoid != true).Count() == paymentCount)
                {
                    scrolBillData.Background = Brushes.Green;
                }
                if (bill.Payments.Where(x => x.Status.isActive == false && x.isVoid != true).Count() == paymentCount)
                {
                    scrolBillData.Background = Brushes.Red;
                }
            }


            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bill.Id, TransactionItemType.Bill);
            //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            if (bill.saleOrder_Id != null)
            {
                saleOrder = bill.SaleOrder;

            }
            else
                if (bill.purchaseOrder_Id != null)
            {
                if (bill.PurchaseOrder.SaleOrder != null)
                    saleOrder = bill.PurchaseOrder.SaleOrder;
            }


            lblLastStatusChangeDate.EditValue = bill.LastStatusChangeDate;
            datsaleOrderdate.EditValue = bill.BillDate;
            datpoCreationdate.EditValue = bill.CreationDate;
            datShipmentdate.EditValue = bill.ShipmentDate;
            datRevisedShipmentDate.EditValue = bill.RevisedShipmentDate;
            datExcpectedPaymentDate.EditValue = bill.ExpectedPayment;
            datPaymentDueTill.EditValue = bill.PaymentDueAgeing;
            datDeliverydate.EditValue = bill.DeliveryDate;
            datOrderConfirmationdate.EditValue = bill.OrderConfirmationDate;
            datBillOfLaddingdate.EditValue = bill.BillOfLaddingDate;
            datLCDatedate.EditValue = bill.lCDate;
            datMaterialReciptdate.EditValue = bill.MaterialReciptDate;
            datPaymentDueFrom.EditValue = bill.PaymentDueStartDate;
            txtSalesref.Text = bill.SalesReferenceNo;
            txtOfferRefNo.Text = bill.OfferReferenceNo;
            txtsaleOrderref.Text = bill.SOReferenceNo;
            lblPOrefrence.Text = bill.POReferenceNo;
            if (!string.IsNullOrEmpty(bill.SyetmReferenceNo))
                txtSystemRef.Text = bill.SyetmReferenceNo;
            txtPoReferenceNo.Text = bill.POReferenceNo;

            txtSupplierRef.Text = bill.SupplierReferenceNo;
            datSupplierdate.EditValue = bill.SupplyDate;
            {
                datLCShipmentDate.EditValue = bill.LCShipmentDate;
                datLCRevisedShipmentDate.EditValue = bill.LCShipmentAmendmentDate;
                datLCExpiryDate.EditValue = bill.LCExpiryDate;
                datLCExpiryDate.EditValue = bill.LCExpiryAmedmentDate;
                txtLCAmedmentno.Text = (string.IsNullOrEmpty(bill.LCAmedmentNo)) ? "" : bill.LCAmedmentNo;
                txtPacking.Text = (string.IsNullOrEmpty(bill.packing)) ? "" : bill.packing;

                if (bill.transshipment == true)
                    cmbTransshipment.SelectedIndex = 0;
                if (bill.transshipment == false)
                    cmbTransshipment.SelectedIndex = 1;
            }
            chkInterCompany.IsChecked = bill.isInterCompany;
            double invoiced = 0;
            POCFRRemaining = bill.totalCFRValue;
            if (bill.InvoiceStage == null)
            {
                bill.RemainingCFRValue = bill.totalCFRValue;
                invoiced = bill.totalCFRValue - bill.RemainingCFRValue;

            }
            else if (bill.InvoiceStage == InvoiceStage.None.ToString() && bill.RemainingCFRValue == 0)
            {
                bill.RemainingCFRValue = bill.totalCFRValue;

                invoiced = bill.totalCFRValue - bill.RemainingCFRValue;
            }
            else
                invoiced = bill.totalCFRValue - bill.RemainingCFRValue;


            txttotalfob.Text = bill.totalFOBValue.ToString();
            txtPOremainingcfr.Text = Math.Round(bill.RemainingCFRValue, 2).ToString();
            txttotalcfr.Text = bill.totalCFRValue.ToString();
            txtBasetotalfob.Text = bill.totalBaseFOBValue.ToString();
            txtSalestotalCfr.Text = bill.POAmountSER.ToString();
            txtMaker.Text = bill.maker;
            txtOrigin.Text = bill.origin;
            txtOwnDescription.Text = bill.OwnDescription;
            txtVendorName.Text = bill.VendorName;

            if (bill.billVendorName != null)
                txtBillVendorName.Text = bill.billVendorName;

            txtFinanaceRef.Text = bill.FinanceRefrenceNo;
            txtComments.Text = bill.comments;
            txtPaymentDueDays.Text = bill.CreditDays.ToString();

            //lblStage.Text = (bill.stage != null) ? bill.stage : "";
            txtLCNumber.Text = bill.LCnumber;
            if (bill.isPercentTax == true)
                txttax.Text = bill.salesTax.ToString() + "%";
            else
                txttax.Text = bill.salesTax.ToString();
            //inco term for po
            if (bill.billType_Id != null || bill.Billtype != null)
            {
                var incoSource = (List<cmbitem>)cmbBillType.Items.SourceCollection;
                cmbBillType.SelectedItem = cmbBillType.Items[cmbBillType.Items.IndexOf(incoSource.Find(x => x.id == bill.billType_Id))];
            }
            //Warranty
            if (bill.SOWarrantyId != null /*|| bill.SOWarranty != null*/)
            {
                var incoSource = (List<cmbitem>)cmbSOWarranty.Items.SourceCollection;
                cmbSOWarranty.SelectedItem = cmbSOWarranty.Items[cmbSOWarranty.Items.IndexOf(incoSource.Find(x => x.id == bill.SOWarrantyId))];

            }
            if (bill.POWarrantyId != null || bill.POWarranty != null)
            {
                var incoSource = (List<cmbitem>)cmbPOWarranty.Items.SourceCollection;
                cmbPOWarranty.SelectedItem = cmbPOWarranty.Items[cmbPOWarranty.Items.IndexOf(incoSource.Find(x => x.id == bill.POWarrantyId))];

            }
            if ((bill.SOCurrency_Id != null || bill.SOCurrency != null))
            {
                var currencySource = (List<cmbitem>)cmbSOCurrency.Items.SourceCollection;
                cmbSOCurrency.SelectedItem = cmbSOCurrency.Items[cmbSOCurrency.Items.IndexOf(currencySource.Find(x => x.id == bill.SOCurrency_Id))];

                if (cmbBillType.SelectedIndex == 4)
                    cmbSOCurrency.IsEnabled = false;
            }
            if (bill.currency_Id != 0 || bill.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == bill.currency_Id))];
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == bill.company.CurrencyId))];
            }
            if (bill.SaleOrder != null)
            {
                if (bill.SaleOrder.costCentercurrency_Id != null)
                {
                    lblCosheetCurrency.Visibility = Visibility.Visible;
                    cmbCostSheetCurrency.Visibility = Visibility.Visible;
                    var currencySource = (List<cmbitem>)cmbCostSheetCurrency.Items.SourceCollection;
                    cmbCostSheetCurrency.SelectedItem = cmbCostSheetCurrency.Items[cmbCostSheetCurrency.Items.IndexOf(currencySource.Find(x => x.id == bill.SaleOrder.costCentercurrency_Id))];
                }
            }
            
            if (bill.saleOrder_Id != null)
                saleOrder = bill.SaleOrder;

            // Select Company
            if (bill.company_Id != null || bill.company != null)
            {
                var companylist = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;

                if (bill.company != null && companylist.Find(x => x.Id == bill.company_Id) == null)
                {
                    companylist.Add(bill.company);
                    lookupCompany.ItemsSource = null;
                    lookupCompany.ItemsSource = companylist;
                }
                company = bill.company;
                lookupCompany.Text = bill.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";
            }


            //Select Payment Reference No
            var paymentRefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
            if (bill.PettyCashRefId != null)
            {
                int ind = 0;
                foreach (var _ref in paymentRefList)
                {

                    if (_ref.id == bill.PettyCashRefId)
                    {
                        cmbxPettyCashRef.SelectedIndex = ind;
                        ind = 0;
                        break;
                    }
                    ind++;
                }
            }

            if (bill.isDeposit == true)
                btnDeposit.IsChecked = true;
            else if (bill.isDeposit == false)
                btnPayment.IsChecked = true;


            if (bill.vendorBillNature != null)
            {
                lookupBillNature.Text = bill.vendorBillNature.Nature;
            }

            //Select Bill Reference No
            var billRefList = (cmbxBillRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxBillRef.ItemsSource as List<cmbitem>;
            if (bill.BillRefNo != null)
            {
                int ind = 0;
                foreach (var _ref in billRefList)
                {
                    if (_ref.id == bill.BillRefNoId)
                    {
                        cmbxBillRef.SelectedIndex = ind;
                        ind = 0;
                        break;
                    }
                    ind++;
                }
            }

            if (bill.InterCompany_Id != null || bill.InterCompany != null)
            {
                var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                if (bill.InterCompany != null && companylist.Find(x => x.Id == bill.InterCompany_Id) == null)
                {
                    companylist.Add(bill.InterCompany);
                    lookupInterCompany.ItemsSource = null;
                    lookupInterCompany.ItemsSource = companylist;
                }
                InterCompany = bill.InterCompany;
                lookupInterCompany.Text = bill.InterCompany.CompanyName;
            }
            if (bill.InterDepartment_Id != null || bill.InterDepartment != null)
            {
                var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                if (bill.InterDepartment != null && deptlist.Find(x => x.Id == bill.InterDepartment_Id) == null)
                {
                    deptlist.Add(bill.InterDepartment);
                    lookupInterDepartment.ItemsSource = null;
                    lookupInterDepartment.ItemsSource = deptlist;
                }
                lookupInterDepartment.Text = bill.InterDepartment.DeptName;
                InterDepartment = bill.InterDepartment;
            }
            // Select Department
            if (bill.dept_Id != 0 || bill.department != null)
            {
                var deptlist = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;

                if (bill.department != null && deptlist.Find(x => x.Id == bill.dept_Id) == null)
                {
                    deptlist.Add(bill.department);
                    lookupDepartment.ItemsSource = null;
                    lookupDepartment.ItemsSource = deptlist;

                }
                lookupDepartment.Text = bill.department.DeptName;
                department = bill.department;

                lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }


            // Select Company
            if (bill.loanAdvanceCompany_Id != null && bill.loanAdvanceCompany != null)
            {
                var companylist = (lookupLoanAdvanceCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupLoanAdvanceCompany.ItemsSource as List<Company>;

                if (bill.loanAdvanceCompany != null && companylist.Find(x => x.Id == bill.loanAdvanceCompany_Id) == null)
                {
                    companylist.Add(bill.loanAdvanceCompany);
                    lookupLoanAdvanceCompany.ItemsSource = null;
                    lookupLoanAdvanceCompany.ItemsSource = companylist;
                }
                lookupLoanAdvanceCompany.Text = bill.loanAdvanceCompany.CompanyName;
            }
            else
            {
                lookupLoanAdvanceCompany.Text = "Select Company";
            }

            if (bill.loanAdvanceDept_Id != null && bill.loanAdvanceDepartment != null)
            {
                var deptlist = (lookupLoanAdvanceDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupLoanAdvanceDepartment.ItemsSource as List<Department>;

                if (bill.loanAdvanceDepartment != null && deptlist.Find(x => x.Id == bill.loanAdvanceDept_Id) == null)
                {
                    deptlist.Add(bill.loanAdvanceDepartment);
                    lookupLoanAdvanceDepartment.ItemsSource = null;
                    lookupLoanAdvanceDepartment.ItemsSource = deptlist;

                }
                lookupLoanAdvanceDepartment.Text = bill.loanAdvanceDepartment.DeptName;
            }
            else
            {
                lookupLoanAdvanceDepartment.Text = "Select Department";
            }

            if (bill.LoansAdvanceId != null && bill.loansAdvance != null)
            {
                var loanAdvances = (lookupLoanAdvance.ItemsSource as List<LoansAdvance>) == null ? new List<LoansAdvance>() : lookupLoanAdvance.ItemsSource as List<LoansAdvance>;

                if (bill.loansAdvance != null && loanAdvances.Find(x => x.Id == bill.LoansAdvanceId) == null)
                {
                    loanAdvances.Add(bill.loansAdvance);
                    lookupLoanAdvance.ItemsSource = null;
                    lookupLoanAdvance.ItemsSource = loanAdvances;

                }
                lookupLoanAdvance.Text = bill.loansAdvance.SystemRef;
            }
            else
            {
                lookupLoanAdvance.Text = "Select Loans Advance";
            }

            //Select Customer
            if (bill.customerCompany?.Id != 0 || bill.customerCompany != null)
            {
                lookupCustomer.Text = bill.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(bill.customerCompany);

                customer = bill.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (bill.vendor_Id != null || bill.vendor != null)
            {


                lookupPayeeVendor.Text = bill.vendor.company.CompanyName;
                vendor = bill.vendor;

            }

            //Select Bill vendor
            if (bill.billVendor_Id != null || bill.billVendor != null)
            {
                var vendorList = (lookupBillVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : lookupBillVendor.ItemsSource as List<Vendor>;

                if (bill.billVendor != null && vendorList.Find(x => x.Id == bill.billVendor_Id) == null)
                {
                    vendorList.Add(bill.billVendor);
                }

                lookupBillVendor.ItemsSource = null;
                lookupBillVendor.ItemsSource = vendorList;

                lookupBillVendor.EditValue = bill.billVendor_Id;
                billVendor = bill.billVendor;

            }

            //Select PO vendor
            if (bill.POVendor_Id != null || bill.POVendor != null)
            {


                lookupPOVendor.Text = bill.POVendor.company.CompanyName;
                POVendor = bill.POVendor;

            }

            // Select Employee 
            if (bill.allocation_Id != 0 || bill.AllocateTo != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == bill.allocation_Id))];

                }
                catch (Exception ex)
                {

                }
            }
            if (bill.bid_Id != null && bill.bid_Id != 0 && bill.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = bill.bid.issueDate;
                txtBidBondRefno.Text = bill.bid.refNo;
                txtBidBonValue.Text = bill.bid.value;
                datBidBondSubmitdate.DateTime = bill.bid.submitDate;
                datBidBondExpirydate.DateTime = bill.bid.expireDate;
                txtIssuingbank.Text = bill.bid.bankName;
            }
            if (bill.BillStatus != null)
            {
                var disAbleStatus = BillStatuses.FirstOrDefault(x => x.Id == bill.BillStatus.Id);
                if (disAbleStatus == null)
                {
                    loadBillStatus(bill.BillStatus);
                }
            }
            var POSource = (List<cmbitem>)cmbBillStatus.Items.SourceCollection;

            // Select Bill Status 
            if (bill.BillStatus?.isActive == false)
            {
                try
                {
                    cmbBillStatus.SelectedItem = cmbBillStatus.Items[cmbBillStatus.Items.IndexOf(POSource.Find(x => x.name == bill.BillStatus.Status))];
                }
                catch (Exception ex)
                {

                    SystemLog.LogError(this.GetType(), "This User cannot see closed Bill status! " + ex.ToString());
                }
            }
            else
            {
                try
                {
                    cmbBillStatus.SelectedItem = cmbBillStatus.Items[cmbBillStatus.Items.IndexOf(POSource.Find(x => x.name == bill.BillStatus.Status))];

                }
                catch (Exception ex)
                {
                    DXMessageBox.Show("Error loading closed Bill statuses!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            checkStatus = bill.BillStatus;
            if (bill.vendorPaymentId != null || bill.vendorPaymentStatus != null)
            {
                var currencySource = (List<cmbitem>)cmbVendorPaymentStatus.Items.SourceCollection;
                cmbVendorPaymentStatus.SelectedItem = cmbVendorPaymentStatus.Items[cmbVendorPaymentStatus.Items.IndexOf(currencySource.Find(x => x.id == bill.vendorPaymentId))];

            }
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();


            if (bill.products.Count != 0)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                foreach (var procurementProduct in bill.products)
                {
                    ProcurementProduct procProduct = new ProcurementProduct();
                    procProduct.Id = procurementProduct.Id;
                    procProduct.inquiryProduct = new InquiryProduct()
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
                        },
                        product_Id = procurementProduct.inquiryProduct.product.Id
                    };
                    procProduct.product_Id = procurementProduct.inquiryProduct.Id;
                    procProduct.unitPrice = procurementProduct.unitPrice;
                    procProduct.UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity;
                    procProduct.InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity;
                    procProduct.UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight;
                    procProduct.InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight;
                    procProduct.value1 = procurementProduct.value1;
                    procProduct.value2 = procurementProduct.value2;
                    procProduct.caption1 = cmbcaption1.Text.Trim();
                    procProduct.caption2 = cmbcaption2.Text.Trim();
                    procProduct.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                    procProduct.AmountSOC = procurementProduct.AmountSOC;
                    if (procurementProduct.costSheetField != null)
                    {
                        procProduct.costSheetField = new CostSheetField()
                        {
                            Id = procurementProduct.costSheetField.Id,
                            AddedbyUserId = procurementProduct.costSheetField.AddedbyUserId,

                            //creditCoaId = procurementProduct.costSheetField.creditCoaId,
                            //debitCoaId = procurementProduct.costSheetField.debitCoaId,
                            isActive = procurementProduct.costSheetField.isActive,
                            Timestamp = procurementProduct.costSheetField.Timestamp,
                            SortId = procurementProduct.costSheetField.SortId,
                            Title = procurementProduct.costSheetField.Title,
                            Type = procurementProduct.costSheetField.Type,
                        };
                    }
                    if (procurementProduct.debitAccount != null)
                    {
                        procProduct.debitAccount = procurementProduct.debitAccount;
                    }
                    if (procurementProduct.creditAccount != null)
                    {
                        procProduct.creditAccount = procurementProduct.creditAccount;
                    }
                    procurementProducts.Add(procProduct);
                }

                grdPOItems.ItemsSource = procurementProducts;
                int x = 0;
                foreach (var pro in grdPOItems.ItemsSource as List<ProcurementProduct>)
                {
                    grdPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                    grdPOItems.SetCellValue(x, "costSheetField", pro.costSheetField);
                    x++;
                }
                lookupProductsinGrid.DisplayMember = "code";
                lookupCostSheetFieldsinGrid.DisplayMember = "Title";
            }
            else
            {

                List<BillItem> procItems = new List<BillItem>();
                foreach (var item in bill.billItems)
                {
                    BillItem procProduct = new BillItem();
                    procProduct.Id = item.Id;


                    if (item.costSheetField != null)
                    {
                        procProduct.costSheetField = item.costSheetField;
                    }
                    if (item.debitAccount != null)
                    {
                        procProduct.debitAccount = item.debitAccount;
                    }
                    if (item.creditAccount != null)
                    {
                        procProduct.creditAccount = item.creditAccount;
                    }
                    procProduct.AmountSOC = item.AmountSOC;
                    procProduct.BillAmount = item.BillAmount;
                    procItems.Add(procProduct);
                }

                grdBillItems.ItemsSource = procItems;
            }
            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == bill.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            if (bill.tax_Id != null)
            {
                var tax = taxRepo.getTaxtById((int)bill.tax_Id);
                if (tax.isAdjusted == true)
                {
                    chkAdjustedTax.IsChecked = true;
                    if (tax.isManual == true)
                    {
                        txtTaxAmount.IsReadOnly = false;
                    }
                    lookUpTax.Text = tax.Name;
                }
                else
                {
                    chkUnAdjustedTax.IsChecked = true;
                    if (tax.isManual == true)
                    {
                        txtTaxAmount.IsReadOnly = false;
                    }
                    lookUpTax.Text = tax.Name;
                }
                txtTaxAmount.Text = bill.taxAmount.ToString();
            }



            int index = 0;

            //if (bill.hasTax == true)
            //{
            //    chkTax.IsChecked = true;

            //    //Select Company
            //    var taxList = (lookUpTax.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : lookUpTax.ItemsSource as List<TaxName>;
            //    if (bill.tax_Id != null)
            //    {
            //        index = 0;
            //        foreach (var _tax in taxList)
            //        {
            //            if (_tax.Id == bill.tax_Id)
            //            {
            //                lookUpTax.SelectedIndex = index;
            //                index = 0;
            //                break;
            //            }
            //            index++;
            //        }
            //    }
            //    else
            //    {
            //        lookUpTax.Text = "Select Tax";
            //    }
            //    txtTaxAmount.Text = bill.taxAmount.ToString();
            //    //txtBillWithTax.Text = bill.billWithTax.ToString();
            //}

            if (bill.hasWHT == true)
            {
                chkWHT.IsChecked = true;

                //Select WHT
                var whtList = (lookUpWHT.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : lookUpWHT.ItemsSource as List<TaxName>;
                if (bill.WHT_Id != null)
                {
                    index = 0;
                    foreach (var _WHT in whtList)
                    {
                        if (_WHT.Id == bill.WHT_Id)
                        {
                            lookUpWHT.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    lookUpWHT.Text = "Select WHT";
                }

                //txtBillAfterTax.Text = bill.billAfterTax.ToString();
            }
            txtexchangerate.Text = bill.ExchangeRate.ToString();
            txtBasetotalcfr.Text = bill.totalBaseCFRValue.ToString();
            txtMarginexchangerate.Text = bill.marginExchangeRate.ToString();
            txtCommision.Text = bill.Commision.ToString();
            txtBaseCommission.Text = bill.commisioninBase.ToString();

            var SOCER = bill.SOC_ER.ToString();
            txtSOCER.Text = SOCER;
            //Margin
            if (bill.SaleOrder != null && bill.CostSheet != null)
            {
                txtBudgetMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalBudgetedMargin).ToString();
                txtRevisedMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalRevisedMargin).ToString();
                txtActualMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalActualMargin).ToString();
            }
            else
            {
                txtBudgetMargin.Text = bill.margin.ToString();
                txtRevisedMargin.Text = bill.RevisedMargin.ToString();
                txtBaseRevisedMargin.Text = bill.RevisedMargininBase.ToString();
                txtSaleRevisedMargin.Text = bill.SalesRevisedMargin.ToString();
                txtActualMargin.Text = bill.ActualMargin.ToString();
                txtBaseBudgetMargin.Text = bill.BudgetedMargininBase.ToString();
                txtBaseActualMargin.Text = bill.ActualMargininBase.ToString();
                txtSaleBudgetMargin.Text = bill.SalesBudgetedMargin.ToString();
                txtSaleAMargin.Text = bill.SalesActualMargin.ToString();
            }
            txtNetCommision.Text = (bill.NetCommision).ToString();
            calculatetotal();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Commision Field without Approval") == null)
            {
                txtCommision.IsReadOnly = true;

            }
            if (bill.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill Amount and Bill Amount (SOC) value Under Approval for products in Bill") == null)
            {
                grdBillItems.Columns.GetColumnByFieldName("BillAmount").ReadOnly = true;
                grdBillItems.Columns.GetColumnByFieldName("AmountSOC").ReadOnly = true;

                grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = true;
                grdPOItems.Columns.GetColumnByFieldName("AmountSOC").ReadOnly = true;
            }
            else if (bill.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill Amount and Bill Amount (SOC) value After Approval for products in Bill") == null)
            {
                grdBillItems.Columns.GetColumnByFieldName("BillAmount").ReadOnly = true;
                grdBillItems.Columns.GetColumnByFieldName("AmountSOC").ReadOnly = true;

                grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = true;
                grdPOItems.Columns.GetColumnByFieldName("AmountSOC").ReadOnly = true;
            }

            if (bill.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Bill") != null)
                {
                    grdbilldata.IsEnabled = true;
                }
                else
                    grdbilldata.IsEnabled = false;
            }
            if (bill.BillStatus.isActive == false && MainWindow.currentUserid != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Bill") == null || (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit (Pending for closing) Bill") == null && bill.PendingForClosing == false)))
            {
                grdbilldata.IsEnabled = false;
                btnAttachment.IsEnabled = false;

                labeltopStatus.Visibility = Visibility.Visible;
                labeltopStatus.Text = bill.BillStatus.Status;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(bill.BillStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                labeltopStatus.Foreground = new SolidColorBrush(newColor);
                var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                var rt = (RotateTransform)labeltopStatus.RenderTransform;
                rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet when Bill closed") != null && bill.PendingForClosing != true && bill.Id != 0)
                {
                    grdbilldata.IsEnabled = true;
                    grdBilRef.IsEnabled = false;
                    grdBillStage.IsEnabled = false;
                    grdTemplate.IsEnabled = false;
                    grdCreationDate.IsEnabled = false;
                    grdStatus.IsEnabled = false;
                    grdVendorPayment.IsEnabled = false;
                    grdCompany.IsEnabled = false;
                    grdDepartment.IsEnabled = false;
                    grdCustomer.IsEnabled = false;
                    grdAllocatedTo.IsEnabled = false;
                    grdPayeeVendor.IsEnabled = false;
                    grdPayeeVendorName.IsEnabled = false;
                    grdBillDate.IsEnabled = false;
                    grdBillDueDate.IsEnabled = false;
                    grdFinanceRef.IsEnabled = false;
                    grdOwnDescription.IsEnabled = false;
                    grdCurrency.IsEnabled = false;
                    grdBillRef12.IsEnabled = false;
                    billRef34.IsEnabled = false;
                    grdSystemRef.IsEnabled = false;
                    grdGridControl.IsEnabled = false;
                    grdBillAmountMER.IsEnabled = false;
                    lblbaseTotal.IsEnabled = false;
                    txtSoAmount.IsEnabled = false;
                    lblPERAmount.IsEnabled = false;
                    cmbSOCurrency.IsEnabled = false;
                    lblPER.IsEnabled = false;
                    grdExchangeRates.IsEnabled = false;
                    lblSoAmountPER.IsEnabled = false;
                    txtSoAmountSOC.IsEnabled = false;
                    grdAmount.IsEnabled = false;
                    btnCostSheet.IsEnabled = true;
                    btnCostSheetPunching.IsEnabled = true;
                }
            }

            if (bill.Billtype.billType == "Linked-Bills-Direct COA" || bill.Billtype.billType == "Linked Bill")            {                lookupDepartment.IsEnabled = false;                lookupCompany.IsEnabled = false;                lookupCustomer.IsEnabled = false;                cmbBillType.IsEnabled = false;            }



            if (saleOrder != null && saleOrder.CostSheet != null && saleOrder.CostSheet.FieldValues != null)
            {
                foreach (var item in repo.getActiveCostSheetFields())
                {
                    costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
                }



                grdCostItems.ItemsSource = costFieldValue;

                loadValues();
            }
            if (saleOrder.Id != 0)
            {
                btnBudgetCostSheet.Visibility = Visibility.Visible;
                btnBudgetCostSheetPunching.Visibility = Visibility.Visible;
            }
            else
            if (purchaseOrder.Id != 0)
            {
                btnBudgetCostSheet.Visibility = Visibility.Visible;
                btnBudgetCostSheetPunching.Visibility = Visibility.Visible;
            }
            if (bill.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == bill.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            loadVATBookReferenceNo();
            if (bill.VATBookRefId != 0 && bill.VATBookRefNumber != null)
            {
                var vatSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatSource.Find(x => x.id == bill.VATBookRefId))];
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

        private void loadPettyCashReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();

            references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);

            if (editbill == 1 && bill != null)
            {
                if (bill.PettyCashRef != null && references.FirstOrDefault(x => x.Id == bill.PettyCashRefId) == null)
                    references.Add(bill.PettyCashRef);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }

            cmbxPettyCashRef.ItemsSource = cmbitems;
        }


        public void loadValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            costSheetBillFields = new List<CostSheetBillField>();
            costSheetSOFields = new List<CostSheetSOField>();

            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            if (billid != 0 && saleOrder.CostSheet.Id != 0)
            {
                costSheetBillFields = repo.GetCostSheetBillFields(billid, (int)saleOrder.CostSheet_Id);
            }
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0)
            {
                costSheetSOFields = repo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
            }

            foreach (var item in costfieldValues)
            {
                foreach (var field in saleOrder.CostSheet.FieldValues)
                {
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                        else if (field.Type == 3)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                var value = repo.GetSystemCost(saleOrder.CostSheet.Id, item.Id);
                                // item.systemValue = field.Value;
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        else if (field.Type == 7)
                        {
                            if (costSheetBillFields.Count != 0)
                            {
                                var dbField = costSheetBillFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Bill_Cost && x.CostSheetId == saleOrder.CostSheet_Id && x.Bill_Id == billid);
                                if (dbField != null)
                                {
                                    var billFieldAmount = dbField.Value;
                                    if (billFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.billAmount = billFieldAmount;
                                        item.addedSystemValue = billFieldAmount;
                                    }
                                }
                            }
                        }

                        else if (field.Type == 10)
                        {
                            if (costSheetSOFields.Count != 0)
                            {
                                var dbField = costSheetSOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet.Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                        item.addedSRBCValue = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                var value = repo.GetSBRC(saleOrder.CostSheet.Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;

                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        else if (field.Type == 33)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet.Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }

                    }
                }
                costfields.Add(item);
            }
            grdCostItems.ItemsSource = costfields;
            List<CostFieldValues> finalCostfields = new List<CostFieldValues>();
            costfieldCheckedValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            //costfields = costfieldCheckedValues.Where(x => x.actualValue > 0||x.SRBC>0  || x.budgetedValue > 0 || x.revisedValue > 0 || /*x.systemPayment > 0 ||*/ x.systemValue != 0 || x.vendor != null || x.billAmount > 0).ToList();
            finalCostfields = costfieldCheckedValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();

            grdCostItems.ItemsSource = finalCostfields;
        }
        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> billItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdPOItems.ItemsSource as List<ProcurementProduct>;

            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            ProcurementProduct proProduct = new ProcurementProduct();
                            proProduct.inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product_Id = procurementProduct.inquiryProduct.product.Id
                            };
                            if (procurementProduct.costSheetField != null)
                            {

                                proProduct.fieldId = procurementProduct.costSheetField.Id;
                            }
                            if (procurementProduct.creditAccount != null)
                            {
                                var creditAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.creditAccountId = creditAccount.Id;
                            }
                            if (procurementProduct.debitAccount != null)
                            {
                                var debitAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.debitAccountId = debitAccount.Id;
                            }

                            proProduct.product_Id = procurementProduct.inquiryProduct.Id;
                            proProduct.unitPrice = procurementProduct.unitPrice;
                            proProduct.value1 = procurementProduct.value1;
                            proProduct.value2 = procurementProduct.value2;
                            proProduct.AmountSOC = procurementProduct.AmountSOC;
                            proProduct.caption1 = cmbcaption1.Text.Trim();
                            proProduct.caption2 = cmbcaption2.Text.Trim();
                            billItems.Add(proProduct);
                        }
                    }
                    else
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            ProcurementProduct proProduct = new ProcurementProduct();
                            proProduct.inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product_Id = procurementProduct.inquiryProduct.product.Id
                            };
                            if (procurementProduct.costSheetField != null)
                            {
                                proProduct.fieldId = procurementProduct.costSheetField.Id;
                            }
                            if (procurementProduct.creditAccount != null)
                            {
                                var creditAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.creditAccountId = creditAccount.Id;
                            }
                            if (procurementProduct.debitAccount != null)
                            {
                                var debitAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.debitAccountId = debitAccount.Id;
                            }
                            proProduct.product_Id = procurementProduct.inquiryProduct.Id;
                            proProduct.unitPrice = procurementProduct.unitPrice;
                            proProduct.value1 = procurementProduct.value1;
                            proProduct.value2 = procurementProduct.value2;
                            proProduct.caption1 = cmbcaption1.Text.Trim();
                            proProduct.caption2 = cmbcaption2.Text.Trim();
                            billItems.Add(proProduct);

                        }
                    }

                }
                else
                {
                    // if user is adding completely new prodcut first time.
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            ProcurementProduct proProduct = new ProcurementProduct();
                            proProduct.inquiryProduct = new InquiryProduct()
                            {

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
                                    isActive = procurementProduct.inquiryProduct.product.isActive
                                }
                            };
                            if (procurementProduct.costSheetField != null)
                            {
                                proProduct.fieldId = procurementProduct.costSheetField.Id;
                            }
                            proProduct.product_Id = procurementProduct.inquiryProduct.Id;
                            proProduct.unitPrice = procurementProduct.unitPrice;
                            proProduct.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                            proProduct.UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity;
                            proProduct.UnInvoicedWeight = procurementProduct.UnInvoicedWeight;
                            proProduct.value1 = procurementProduct.value1;
                            proProduct.value2 = procurementProduct.value2;
                            proProduct.caption1 = cmbcaption1.Text.Trim();
                            proProduct.caption2 = cmbcaption2.Text.Trim();
                            proProduct.AmountSOC = procurementProduct.AmountSOC;
                            billItems.Add(proProduct);
                        }
                    }
                    else
                    {
                        // if user reloaded he offer and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            ProcurementProduct proProduct = new ProcurementProduct();
                            proProduct.inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product_Id = procurementProduct.inquiryProduct.product.Id
                            };
                            if (procurementProduct.costSheetField != null)
                            {
                                proProduct.fieldId = procurementProduct.costSheetField.Id;
                            }
                            if (procurementProduct.creditAccount != null)
                            {
                                var creditAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.creditAccountId = creditAccount.Id;
                            }
                            if (procurementProduct.debitAccount != null)
                            {
                                var debitAccount = chartofAccountRepo.get(procurementProduct.creditAccount.Id);
                                proProduct.debitAccountId = debitAccount.Id;
                            }
                            proProduct.product_Id = procurementProduct.inquiryProduct.Id;
                            proProduct.unitPrice = procurementProduct.unitPrice;
                            proProduct.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                            proProduct.UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity;
                            proProduct.UnInvoicedWeight = procurementProduct.UnInvoicedWeight;
                            proProduct.value1 = procurementProduct.value1;
                            proProduct.value2 = procurementProduct.value2;
                            proProduct.caption1 = cmbcaption1.Text.Trim();
                            proProduct.caption2 = cmbcaption2.Text.Trim();
                            proProduct.AmountSOC = procurementProduct.AmountSOC;
                            billItems.Add(proProduct);
                        }
                    }
                }
            }
            return billItems;
        }
        public List<BillItem> getBIllItemsdata()
        {
            List<BillItem> billItems = new List<BillItem>();

            List<BillItem> items = new List<BillItem>();
            billItems = grdBillItems.ItemsSource as List<BillItem>;

            foreach (var item in billItems)
            {
                if (item.Id == 0)
                {

                    BillItem proProduct = new BillItem();

                    if (item.costSheetField != null)
                    {
                        proProduct.fieldId = item.costSheetField.Id;
                    }
                    if (item.creditAccount != null)
                    {
                        var creditAccount = chartofAccountRepo.get(item.creditAccount.Id);
                        proProduct.creditAccountId = creditAccount.Id;
                    }
                    if (item.debitAccount != null)
                    {
                        var debitAccount = chartofAccountRepo.get(item.debitAccount.Id);
                        proProduct.debitAccountId = debitAccount.Id;
                    }
                    proProduct.AmountSOC = item.AmountSOC;
                    proProduct.BillAmount = item.BillAmount;

                    items.Add(proProduct);


                }
                else
                {


                    BillItem proProduct = billRepo.GetBillItem(item.Id);
                    if (item.costSheetField != null)
                    {
                        proProduct.fieldId = item.costSheetField.Id;
                    }
                    if (item.creditAccount != null)
                    {
                        var creditAccount = chartofAccountRepo.get(item.creditAccount.Id);
                        proProduct.creditAccountId = creditAccount.Id;
                    }
                    if (item.debitAccount != null)
                    {
                        var debitAccount = chartofAccountRepo.get(item.debitAccount.Id);
                        proProduct.debitAccountId = debitAccount.Id;
                    }
                    proProduct.BillAmount = item.BillAmount;
                    proProduct.AmountSOC = item.AmountSOC;
                    proProduct.Id = item.Id;

                    items.Add(proProduct);

                }
            }
            return items;
        }
        private void btnBillSave_Click(object sender, RoutedEventArgs e)

        {
            try
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                cmbitem billType = new cmbitem();
                if (cmbBillType.SelectedIndex > -1)
                {
                    billType = cmbBillType.SelectedItem as cmbitem;
                    if (billType.name == "Credit Cards")
                    {
                        if (cmbxCardUser.SelectedIndex > -1)
                        {
                            bill.CardUserId = (cmbxCardUser.SelectedItem as CardHolder).Id;
                        }
                        else
                        {
                            DXMessageBox.Show("Please Select Card User", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                            return;
                        }

                        if (cmbxCreditCardNo.SelectedIndex > -1)
                        {
                            bill.CreditCardNoId = (cmbxCreditCardNo.SelectedItem as cmbitem).id;
                        }
                        else
                        {
                            DXMessageBox.Show("Please Select Credit Card", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                            return;
                        }
                    }
                    else
                    {
                        bill.CardUserId = null;
                        bill.CreditCardNoId = null;
                    }
                }
                else
                {
                    cmbBillType.Focus();
                    DXMessageBox.Show("Please Select Bill Type", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }



                if (lookupPayeeVendor.SelectedIndex == -1 && vendor.Id == 0)
                {
                    lookupPayeeVendor.Focus();
                    DXMessageBox.Show("Please Select a Vendor against Bill", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                {
                    DXMessageBox.Show("Please Select a Customer for whom Bill is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCustomer.Focus();
                    return;
                }
                else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    DXMessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    DXMessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                {
                    DXMessageBox.Show("Please Select an Employee to whom this Bill will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                else if (cmbBillStatus.SelectedIndex == -1 && bill.PendingForClosing != true)
                {
                    DXMessageBox.Show("Please Select Current Status of Bill to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbBillStatus.Focus();
                    return;
                }
                else if (cmbPOCurrency.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Please Select Bills Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbPOCurrency.Focus();
                    return;
                }
                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }

                if (cmbxBillRef.SelectedIndex > 0)
                {
                    bill.BillRefNoId = (cmbxBillRef.SelectedItem as cmbitem).id;
                }
                else
                {
                    bill.BillRefNoId = null;
                }

                //------------------Changed----------------------
                if (billType.name != "New Bill" && billType.name != "Credit Cards")
                {
                    if (cmbSOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select SaleOrder's Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbSOCurrency.Focus();
                        return;
                    }
                }

                if (chkManagementSummary.IsChecked == true)
                {
                    if (lookUpManagementSummary.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Management Summary!");
                        lookUpManagementSummary.Focus();
                        return;
                    }
                }
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    bill.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    bill.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                if (chkManagementSummary.IsChecked == true)
                {
                    bill.hasSummary = true;
                    if (lookUpManagementSummary.SelectedIndex > -1)
                    {
                        bill.managementSummary_Id = (lookUpManagementSummary.SelectedItem as ManagementSummary).Id;
                        if (!String.IsNullOrEmpty(txtSummaryMemo.Text))
                            bill.SummaryMemo = txtSummaryMemo.Text;
                    }
                }
                else
                {
                    bill.hasSummary = false;
                    if (lookUpManagementSummary.SelectedIndex > -1)
                    {
                        bill.managementSummary_Id = null;
                        bill.SummaryMemo = null;
                    }
                }

                if (!string.IsNullOrEmpty(txtSystemRef.Text))
                    bill.SyetmReferenceNo = txtSystemRef.Text;

                if (cmbBillType.SelectedItem != null)
                {
                    if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                        (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                    {
                        bill.billItems = getBIllItemsdata();
                    }
                    else
                    {
                        bill.products = getProductsdata();
                    }
                }

                if(adjustments != null && adjustments.Count > 0)
                {
                    adjustments.ForEach(cc => cc.bill_Id = bill.Id);
                    adjustments.ForEach(cc => cc.loansAdvanceType = LoansAdvanceType.Vendor_Bill);
                    bill.Adjustments = adjustments;
                }


                if (cmbxPettyCashRef.SelectedIndex > 0)
                    bill.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
                else
                    bill.PettyCashRefId = null;

                List<PettyCash> pettyCashes = new List<PettyCash>();
                if (editbill == 1 && billid != 0)
                {
                    if (btnDeposit.IsChecked == true)
                    {
                        pettyCashes.Add(new PettyCash()
                        {
                            CreationDate = datpoCreationdate.DateTime,
                            billId = bill.Id,
                            TransactionType = TransactionItemType.Bill,
                            debit = bill.totalCFRValue,
                            credit = 0,
                            total = bill.totalCFRValue - 0,
                            SystemRefNo = txtSystemRef.Text,
                            deptId = bill.dept_Id,
                            companyId = bill.company_Id,
                            currencyId = (cmbSOCurrency.SelectedItem as cmbitem).id,
                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                        });
                        bill.isDeposit = true;
                    }
                    else if (btnPayment.IsChecked == true)
                    {
                        pettyCashes.Add(new PettyCash()
                        {
                            CreationDate = datpoCreationdate.DateTime,
                            billId = bill.Id,
                            TransactionType = TransactionItemType.Bill,
                            debit = 0,
                            credit = bill.totalCFRValue,
                            total = 0 - bill.totalCFRValue,
                            SystemRefNo = txtSystemRef.Text,
                            deptId = bill.dept_Id,
                            companyId = bill.company_Id,
                            currencyId = (cmbSOCurrency.SelectedItem as cmbitem).id,
                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                        });
                        bill.isDeposit = false;
                    }
                    else
                    {
                        bill.isDeposit = null;
                    }
                }
                else
                {
                    if (btnDeposit.IsChecked == true)
                    {
                        pettyCashes.Add(new PettyCash()
                        {
                            CreationDate = datpoCreationdate.DateTime,
                            billId = 0,
                            TransactionType = TransactionItemType.Bill,
                            debit = bill.totalCFRValue,
                            credit = 0,
                            total = bill.totalCFRValue - 0,
                            SystemRefNo = txtSystemRef.Text,
                            deptId = (lookupDepartment.SelectedItem as Department).Id,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbSOCurrency.SelectedItem as cmbitem).id,
                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                        });
                        bill.isDeposit = true;
                    }
                    else if (btnPayment.IsChecked == true)
                    {
                        pettyCashes.Add(new PettyCash()
                        {
                            CreationDate = datpoCreationdate.DateTime,
                            billId = 0,
                            TransactionType = TransactionItemType.Bill,
                            debit = 0,
                            credit = bill.totalCFRValue,
                            total = 0 - bill.totalCFRValue,
                            SystemRefNo = txtSystemRef.Text,
                            deptId = (lookupDepartment.SelectedItem as Department).Id,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbSOCurrency.SelectedItem as cmbitem).id,
                            PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                        });
                        bill.isDeposit = false;
                    }
                    else
                    {
                        bill.isDeposit = null;
                    }

                }
                bill.pettyCashes = pettyCashes;



                if (datglPostingdate.EditValue != null)
                {
                    bill.GLPostingDate = (DateTime)datglPostingdate.EditValue;
                }
                else
                {
                    bill.GLPostingDate = null;
                }

                //if (btnPushtoGL.IsChecked == true)
                //{

                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {
                    bill.journalTransactions = getJournalTransactions();
                }
                //}

                //if (bill.products.Count == 0)
                //{
                //    DXMessageBox.Show("Please Select items against which you want to create a Bill", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    return;
                //}
                Bill order;
                order = billRepo.get(txtSalesref.Text.Trim());

                DateTime? dateTime = null;
                bill.BillDate = (datsaleOrderdate.Text == "") ? dateTime : datsaleOrderdate.DateTime;
                bill.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;

                if (loansAdvanceId > 0)
                    bill.LoansAdvanceId = loansAdvanceId;

          

                if (cmbSOPaymentTerm.SelectedItem != null)
                    bill.SoPaymentterm_Id = (cmbSOPaymentTerm.SelectedItem as cmbitem).id;
                if (cmbPOPaymentTerm.SelectedItem != null)
                    bill.POPaymentterm_Id = (cmbPOPaymentTerm.SelectedItem as cmbitem).id;
                if (cmbIncoterm.SelectedItem != null)
                    bill.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;
                if (cmbcaption1.SelectedItem != null)
                    bill.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                if (cmbcaption2.SelectedItem != null)
                    bill.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;

                bill.maker = txtMaker.Text.Trim();
                if (cmbSOWarranty.SelectedItem != null)
                    bill.SOWarrantyId = (cmbSOWarranty.SelectedItem as cmbitem).id;
                if (cmbPOWarranty.SelectedItem != null)
                    bill.POWarrantyId = (cmbPOWarranty.SelectedItem as cmbitem).id;

                bill.origin = txtOrigin.Text.Trim();
                bill.ShipmentDate = (datShipmentdate.Text == "") ? dateTime : datShipmentdate.DateTime;
                bill.POReferenceNo = txtPoReferenceNo.Text.ToString();

                bill.SupplierReferenceNo = txtSupplierRef.Text;
                bill.SupplyDate = (datSupplierdate.Text == "") ? dateTime : datSupplierdate.DateTime;

                bill.DeliveryDate = (datDeliverydate.Text == "") ? dateTime : datDeliverydate.DateTime;
                bill.OrderConfirmationDate = (datOrderConfirmationdate.Text == "") ? dateTime : datOrderConfirmationdate.DateTime;
                bill.RevisedShipmentDate = (datRevisedShipmentDate.Text == "") ? dateTime : datRevisedShipmentDate.DateTime;
                bill.BillOfLaddingDate = (datBillOfLaddingdate.Text == "") ? dateTime : datBillOfLaddingdate.DateTime;
                bill.MaterialReciptDate = (datMaterialReciptdate.Text == "") ? dateTime : datMaterialReciptdate.DateTime;
                bill.ExpectedPayment = (datExcpectedPaymentDate.Text == "") ? dateTime : datExcpectedPaymentDate.DateTime;
                bill.OwnDescription = txtOwnDescription.Text;
                bill.FinanceRefrenceNo = txtFinanaceRef.Text;
                bill.comments = txtComments.Text.Trim();
                bill.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                bill.LCnumber = txtLCNumber.Text.Trim();
                bill.SalesReferenceNo = txtSalesref.Text.Trim();
                bill.SOReferenceNo = txtsaleOrderref.Text.Trim();
                bill.OfferReferenceNo = txtOfferRefNo.Text.Trim();
                bill.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                bill.billType_Id = (cmbBillType.SelectedItem as cmbitem).id;
                bill.PaymentDueStartDate = (datPaymentDueFrom.Text == "") ? dateTime : datPaymentDueFrom.DateTime;
                bill.PaymentDueAgeing = (datPaymentDueTill.Text == "") ? dateTime : datPaymentDueTill.DateTime;
                bill.CreditDays = Convert.ToInt32(txtPaymentDueDays.Text.Trim());

                bill.packing = txtPacking.Text;
                bill.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                bill.SoAmountSOC_ER = (string.IsNullOrEmpty(txtSoAmountSOC.Text.Trim())) ? 0 : Convert.ToDouble(txtSoAmountSOC.Text.Trim());
                if (bill.SoAmountSOC_ER == 0 && bill.Id != 0)
                {
                    DXMessageBox.Show("Please set amount Bill Amount(SOC) to continue", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (lookupBillCategory.SelectedIndex > -1)
                {
                    bill.billCategoryId = (lookupBillCategory.SelectedItem as BillCategory).Id;
                }
                if (txtCommision.Text != "" && txtBaseCommission.Text != "")
                {
                    bill.Commision = Convert.ToDouble(txtCommision.Text.Trim());
                    bill.commisioninBase = Convert.ToDouble(txtBaseCommission.Text.Trim());
                }
                bill.NetCommision = (string.IsNullOrEmpty(txtNetCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetCommision.Text.Trim());


                if (!string.IsNullOrEmpty(txtTaxAmount.Text))
                {
                    bill.taxAmount = Convert.ToDouble(txtTaxAmount.Text);
                }
                if (txtBudgetMargin.Text != "")
                {
                    bill.margin = Convert.ToDouble(txtBudgetMargin.Text.Trim());
                    bill.BudgetedMargininBase = Convert.ToDouble(txtBaseBudgetMargin.Text.Trim());
                    bill.SalesBudgetedMargin = Convert.ToDouble(txtSaleBudgetMargin.Text.Trim());
                }

                if (txtActualMargin.Text != "")
                {
                    bill.ActualMargin = Convert.ToDouble(txtActualMargin.Text.Trim());
                    bill.ActualMargininBase = Convert.ToDouble(txtBaseActualMargin.Text.Trim());
                    bill.SalesActualMargin = Convert.ToDouble(txtSaleAMargin.Text.Trim());
                }
                if (txtRevisedMargin.Text != "")
                {
                    bill.RevisedMargin = (string.IsNullOrEmpty(txtRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtRevisedMargin.Text.Trim());
                    bill.RevisedMargininBase = (string.IsNullOrEmpty(txtBaseRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtBaseRevisedMargin.Text.Trim());
                    bill.SalesRevisedMargin = (string.IsNullOrEmpty(txtSaleRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtSaleRevisedMargin.Text.Trim());
                }
                bill.RevisedMarginPercent = (string.IsNullOrEmpty(txtRevisedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtRevisedPercent.Text, "[^0-9.]", ""));
                bill.ActualMarginPercent = (string.IsNullOrEmpty(txtActualPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtActualPercent.Text, "[^0-9.]", ""));
                bill.BudgetedMarginPercent = (string.IsNullOrEmpty(txtBudgetedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtBudgetedPercent.Text, "[^0-9.]", ""));
                bill.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);
                bill.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                bill.totalFOBValue = Convert.ToDouble(txttotalfob.Text);

                if (string.IsNullOrEmpty(txtBasetotalcfr.Text))
                {
                    txtBasetotalcfr.Text = 0.ToString();
                }
                bill.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                bill.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);
                bill.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);
                bill.ExchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());
                bill.marginExchangeRate = Convert.ToDouble(txtMarginexchangerate.Text.Trim());
                bill.SOC_ER = Convert.ToDouble(txtSOCER.Text.Trim());

                if (lookupBillNature.SelectedIndex > -1)
                    bill.vendorBillNature_Id = (lookupBillNature.SelectedItem as VendorBillNature).Id;

                if (!String.IsNullOrEmpty(txtSoAmountSOC.Text))
                    bill.SoAmountSOC_ER = Convert.ToDouble(txtSoAmountSOC.Text.Trim());
                string str = txttax.Text.Trim();
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    bill.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    bill.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    bill.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    bill.TotalQuantity = null;
                }
                if (str.IndexOf("%") != -1)
                {
                    bill.isPercentTax = true;
                    bill.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                }
                else
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        bill.salesTax = Convert.ToDouble(str);
                        bill.isPercentTax = false;
                    }
                }
                if (saleOrder.Id != 0 && saleOrder != null)
                {
                    bill.saleOrder_Id = saleOrder.Id;
                }
                else
                {
                    bill.saleOrder_Id = null;
                }
                if (purchaseOrder?.Id != 0)
                {
                    bill.purchaseOrder_Id = purchaseOrder.Id;
                }
                if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                {

                    BillStatus status = billRepo.getstatus((cmbBillStatus.SelectedItem as cmbitem).id);
                    bill.BillStatus = status;
                }
                //Selected Bill Status
                if ((cmbVendorPaymentStatus.SelectedItem as cmbitem) != null)
                {
                    bill.vendorPaymentId = (cmbVendorPaymentStatus.SelectedItem as cmbitem).id;
                }
                // Selected Vendor 
                if (lookupPayeeVendor.SelectedIndex > -1)
                {
                    bill.vendor_Id = (lookupPayeeVendor.SelectedItem as Vendor).Id;
                    bill.VendorName = txtVendorName.Text;

                }
                //if (vendor != null)
                //{
                //    bill.vendor_Id = vendor.Id;
                //    bill.VendorName = txtVendorName.Text;
                //}
                // Selected Payee Vendor 
                if (lookupBillVendor.SelectedIndex > -1)
                {
                    bill.billVendor_Id = (lookupBillVendor.SelectedItem as Vendor).Id;
                    bill.billVendorName = txtBillVendorName.Text;
                }
                // Selected PO Vendor 
                if (lookupPOVendor.SelectedIndex > -1)
                {
                    bill.POVendor_Id = (lookupPOVendor.SelectedItem as Vendor).Id;
                    bill.POVendorName = txtPOVendorName.Text;
                }
                // selected Department
                if (department != null)
                {
                    bill.dept_Id = department.Id;
                }
                //selected customer
                if (customer != null)
                {
                    bill.customerCompany_Id = customer.Id;
                }
                // selected company
                if (company != null)
                {
                    bill.company_Id = company.Id;
                }
                if (chkInterCompany.IsChecked == true)
                {
                    if (InterCompany != null && InterCompany.Id != 0)
                    {
                        bill.InterCompany_Id = InterCompany.Id;
                    }
                    if (InterDepartment != null && InterDepartment.Id != 0)
                    {
                        bill.InterDepartment_Id = InterDepartment.Id;
                    }
                    bill.isInterCompany = true;
                }
                else
                {
                    bill.isInterCompany = false;
                    bill.InterCompany_Id = null;
                    bill.InterDepartment_Id = null;
                }
                // selected currency
                if (currency != null)
                {
                    bill.currency_Id = currency.Id;
                }
                if (billType.name != "New Bill" && billType.name != "Credit Cards")
                {
                    bill.SOCurrency_Id = (cmbSOCurrency.SelectedItem as cmbitem).id;
                }
                if (lookUpTax.SelectedIndex > -1)
                {

                    //if (lookUpTax.SelectedIndex < 0)
                    //{
                    //    DXMessageBox.Show("Please Select Tax!");
                    //    lookUpTax.Focus();
                    //    return;
                    //}
                    bill.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                    bill.billWithTax = Convert.ToDouble(txtBillWithTax.Text);
                }
                else
                {
                    bill.billWithTax = null;
                }

                // Selected Company 
                if (lookupLoanAdvanceCompany.SelectedIndex > -1)
                {
                    bill.loanAdvanceCompany_Id = (lookupLoanAdvanceCompany.SelectedItem as Company).Id;
                }
                // Selected Department
                if (lookupLoanAdvanceDepartment.SelectedIndex > -1)
                {
                    bill.loanAdvanceDept_Id = (lookupLoanAdvanceDepartment.SelectedItem as Department).Id;
                }
                // Selected Loans Advance
                if (lookupLoanAdvance.SelectedIndex > -1)
                {
                    bill.LoansAdvanceId = (lookupLoanAdvance.SelectedItem as LoansAdvance).Id;
                }


                //if (chkTax.IsChecked == true)
                //{

                //}
                //else
                //{
                //    bill.hasTax = false;
                //    bill.tax_Id = null;
                //    bill.billWithTax = Convert.ToDouble(txtcfr.Text);
                //}

                if (chkWHT.IsChecked == true)
                {
                    bill.hasWHT = true;
                    if (lookUpWHT.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please Select WHT!");
                        lookUpWHT.Focus();
                        return;
                    }
                    bill.WHT_Id = (lookUpWHT.SelectedItem as TaxName).Id;
                    bill.billAfterTax = Convert.ToDouble(txtBillAfterTax.Text);
                }
                else
                {
                    bill.hasWHT = false;
                    bill.WHT_Id = null;
                    bill.billAfterTax = Convert.ToDouble(txtBillWithTax.Text);
                }
                if (cmbxVATBookRef.SelectedIndex > -1)
                {
                    bill.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                    if (lookUpTax.SelectedIndex > -1)
                    {
                        bill.VATBooks = new List<ERP_BL.VATBook.VATBook>();
                        bill.VATBooks = GetVATBooks();
                    }
                }
                var myWindow = Window.GetWindow(this);
                if (editOrder == 1 && OrderId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Bill") != null))
                {
                    if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                    {
                        if (bill.bid_Id == 0 && bill.bid == null)
                        {
                            Bid bid = new Bid()
                            {
                                issueDate = datBidBondIssuedate.DateTime,
                                refNo = txtBidBondRefno.Text.Trim(),
                                value = txtBidBonValue.Text.Trim(),
                                submitDate = datBidBondSubmitdate.DateTime,
                                expireDate = datBidBondExpirydate.DateTime,
                                bankName = txtIssuingbank.Text.Trim()
                            };
                            bill.bid = bid;
                        }
                        else
                        { //get form data for bid
                            bill.bid.issueDate = datBidBondIssuedate.DateTime;
                            bill.bid.refNo = txtBidBondRefno.Text.Trim();
                            bill.bid.value = txtBidBonValue.Text.Trim();
                            bill.bid.submitDate = datBidBondSubmitdate.DateTime;
                            bill.bid.expireDate = datBidBondExpirydate.DateTime;
                            bill.bid.bankName = txtIssuingbank.Text.Trim();
                        }
                    }
                    if (MainWindow.currentUserid == 0)
                    {

                    }
                    else if (bill.user_Id == null)
                        bill.user_Id = MainWindow.currentUserid;

                    if (bill.CostSheet != null && bill.CostSheet.Timestamp != null)
                    {
                        bill.CostSheet.Timestamp = System.DateTime.Now;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null && bill.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Bill is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            bill.stage = TransactionStage.Approved.ToString();
                            bill.isApproved = true;
                            bill.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null && bill.isReApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Bill is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            bill.stage = TransactionStage.Approved.ToString();
                            bill.isReApproved = true;
                            bill.ReApprovalDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != bill.BillStatus.Id)
                        {
                            bill.LastStatusChangeDate = System.DateTime.Now;
                            if (bill.BillStatus.isActive != true)
                            {
                                bill.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }

                    if (checkStatus.Id != bill.BillStatus.Id)
                    {

                        if (bill.BillStatus.isActive == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null) ? true : false)
                            {
                                bill.PendingForClosing = false;
                                bill.stage = TransactionStage.Approved.ToString();
                                //  usersRepo.Add(TransactionInfo.Approved_Closing, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                            {
                                bill.stage = TransactionStage.AwaitingApproval.ToString();
                                if (bill.PendingForClosing == null)
                                {
                                    bill.PendingForClosing = true;
                                }
                                //usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                            {
                                bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (bill.PendingForClosing != true)
                                {
                                    bill.PendingForClosing = true;
                                }
                                //Billss.ucStatuschange.bill.PendingForClosing = true;


                                //usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else
                            {
                                bill.stage = TransactionStage.AwaitingFirstReview.ToString();
                                bill.PendingForClosing = true;
                                //usersRepo.Add(TransactionInfo.Closed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                        }
                    }

                    
                    

                        billRepo.update(bill);
                    if (checkStatus.Id != bill.BillStatus.Id)
                    {


                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Bill has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), bill.Id, TransactionItemType.Bill);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;
                                tagUsersRecommendation = win.tagRecommendationUsers;
                                ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                            {

                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), bill.Id, TransactionItemType.Bill);
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
                        else
                        {

                        }
                        string oldStat = checkStatus.Status;
                        string newStat = bill.BillStatus.Status;
                        string symbolCurr = "";

                        if (bill.currency != null)
                        {
                            symbolCurr = bill.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Bill (Amount OC) having value: " + bill.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(bill.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                        loadcomments();
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, bill.Id, (int)TransactionItemType.Bill, "Status Changed from (" + checkStatus.Status + ") to (" + bill.BillStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);

                    DXMessageBox.Show("Bill Updated Succesfully");
                    SystemLog.LogInfo(this.GetType(), "Bill Updated Succesfully refrence No= " + bill.SOReferenceNo + " Id=" + bill.Id);
                }
                else if (editOrder != 1)
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            DXMessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        else
                            bill.user_Id = MainWindow.currentUserid;
                        if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                        {
                            Bid bid = new Bid()
                            {
                                issueDate = datBidBondIssuedate.DateTime,
                                refNo = txtBidBondRefno.Text.Trim(),
                                value = txtBidBonValue.Text.Trim(),
                                submitDate = datBidBondSubmitdate.DateTime,
                                expireDate = datBidBondExpirydate.DateTime,
                                bankName = txtIssuingbank.Text.Trim()
                            };
                            bill.bid = bid;
                        }
                        else
                            bill.bid_Id = null;
                        if (bill.CostSheet != null)
                        {
                            bill.CostSheet.Timestamp = System.DateTime.Now;
                            bill.CostSheet.TransactionId = bill.Id;
                            bill.CostSheet.TransactionType = 2;
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null /*&& bill.isApproved == false*/)
                        {
                            bill.stage = TransactionStage.Approved.ToString();
                            bill.isApproved = true;
                            bill.ApprovedDate = System.DateTime.Now;
                        }
                        else
                        {
                            bill.stage = TransactionStage.AwaitingFirstReview.ToString();
                            bill.isApproved = false;
                        }
                        billRepo.Add(bill);
                        UsersRepo.Add(TransactionInfo.Initialized, bill.Id, (int)TransactionItemType.Bill, "");
                        UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Bill genrated on this Offer");
                        SystemLog.LogInfo(this.GetType(), "Bill Added Succesfully refrence No= " + bill.SOReferenceNo + " Id=" + bill.Id);
                        DXMessageBox.Show("Bill Added Succesfully");
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        myWindow.Close();
                        return;
                    }
                myWindow.Close();

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "Bill Error refrence No= " + bill.SOReferenceNo + " Id=" + bill.Id + ex.ToString());
            }
        }
        private void loadCurrencies()
        {
            cmbPOCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbSOCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            if (cmbbaseCurrency.ItemsSource == null)
                cmbbaseCurrency.ItemsSource = SYSTEM_STATIC.currencySources; 
            if (cmbCostSheetCurrency.ItemsSource == null)
                cmbCostSheetCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }
        public void loadVendorPaymentStatus()
        {
            cmbVendorPaymentStatus.ItemsSource = SYSTEM_STATIC.vendorPaymentStatusSource;
        }
        public void loadBillCategories()
        {
            billRepo = new BillRepo();
            lookupBillCategory.ItemsSource = billRepo.GetAllBillCategories();
        }
        public void loadBillStatus()
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Bill") != null)
            {
                BillStatuses = billRepo.getAllBillStatus();
            }
            else
                BillStatuses = billRepo.getAllActiveBillStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            BillStatuses = BillStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(BillStatuses, delegate (BillStatus status)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            });

            cmbBillStatus.ItemsSource = cmbitems;
        } 
        public void loadBillStatus(BillStatus _status)
        {
            BillStatuses.Add(_status);
            List<cmbitem> cmbitems = new List<cmbitem>();

            Parallel.ForEach(BillStatuses, delegate (BillStatus status)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            });

            cmbBillStatus.ItemsSource = cmbitems;
        }
        private void cmbBillStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbBillStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbBillStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.Billss.frmBillStatusAdd statusAdd = new Procurementss.Billss.frmBillStatusAdd();
                    statusAdd.ShowDialog();
                    loadBillStatus();
                }
            }
        }
        private void cmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbEmployee.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbEmployee.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Employeess.frmEmployeeAdd employeeadd = new Employeess.frmEmployeeAdd();
                    employeeadd.ShowDialog();
                    loademployees();
                }
            }
        }
        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            customer = lookupCustomer.SelectedItem as CustomerCompany;
            if (customer != null)
            {
                string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
            }
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                    {
                        cmbbaseCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
                loaddepartments();
                loadBillReferenceNo();
                loadPettyCashReferenceNo();

                var billNatures = billRepo.GetAllVendorBillNatureByComp(company.Id);
                lookupBillNature.ItemsSource = billNatures;
            }
        }
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                if (department.ParentID == null && department.subDepartments.Count != 0)
                {
                    DXMessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                loadcustomers();
                lookupPayeeVendor.ItemsSource = department.Vendors;
                lookupBillVendor.ItemsSource = department.Vendors;
                lookupPOVendor.ItemsSource = department.Vendors;
                ProductRepo productRepo = new ProductRepo();
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupProductsinGrid.ItemsSource = products;
                loademployees();
                if (department.customers.Count == 0)
                {
                    DXMessageBox.Show("No customer is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Vendors.Count == 0)
                {
                    DXMessageBox.Show("No Vendor is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.employees.Count == 0)
                {
                    DXMessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }

                var userChartofAccounts = chartofAccountsRepo.GetAllforVendorBills(lookupCompany.SelectedItem as Company, lookupDepartment.SelectedItem as Department, SYSTEM_STATIC.currentUser.id);

                lookupdebitAccountsinGrid.ItemsSource = userChartofAccounts;
                lookupcreditAccountsinGrid.ItemsSource = userChartofAccounts;
            }
        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customerss.frmCustomeradd customeradd = new Customerss.frmCustomeradd();
            customeradd.ShowDialog();
            loadcustomers();
        }
        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            Companiess.frmcompanyCenter.Editit = 1;
            Companiess.frmcompanyCenter.companyId = company.Id;
            frmcompanyadd.ShowDialog();
            loaddepartments();
        }
        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            frmcompanyadd.ShowDialog();
            loadcompanies();

        }

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            Vendorss.frmVendoradd vendoradd = new Vendorss.frmVendoradd();
            vendoradd.ShowDialog();
            loadvendors();
        }

        private void lookupVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
           var  billVendor = lookupPayeeVendor.SelectedItem as Vendor;
            if (billVendor != null)
            {
                if (datpoCreationdate.EditValue != null && (DateTime)datpoCreationdate.EditValue > new DateTime(2025, 2, 17) && billVendor.isBlackList == true)
                {
                    lookupBillVendor.SelectedItem = null;
                    return; // or whatever action you need to take
                }
                else
                if (billVendor.isBlackList == true)
                {

                    lookupBillVendor.Background = Brushes.DarkRed;
                    lookupBillVendor.Foreground = Brushes.White;
                }
                else
                {
                    lookupBillVendor.Background = Brushes.Transparent;
                    lookupBillVendor.Foreground = Brushes.Black;
                }
                var vendorCountry = billVendor.billingAddres.Country;
                txtBillVendorName.Text = billVendor.company.CompanyName;
            }
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbPOCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbPOCurrency.SelectedItem as cmbitem).id;
                currency = currencyRepo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                    string caption1 = "";
                    string caption2 = "";
                    if (cmbcaption1.SelectedItem != null)
                        caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    if (cmbcaption2.SelectedItem != null)
                        caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                    symbol = name.Substring(name.IndexOf("("));
                    if (grdPOItems.Columns.Count != 0)
                    {
                        grdPOItems.Columns[6].Header = caption1 + symbol;
                        grdPOItems.Columns[7].Header = caption2 + symbol;
                        lblTotal.Text = "Total " + symbol;
                        lblCFRTotal.Text = "CFR Total " + symbol;
                        string str = lblCommision.Text;
                        if (-1 != str.IndexOf("("))
                            lblCommision.Text = (str.Substring(0, str.IndexOf("("))) + symbol;
                        else
                            lblCommision.Text = lblCommision.Text + " " + symbol;
                    }

                }
            }
        }
        string symbol;
        private static InquiryType PoType;



        private void txttax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {

                    CalculateGlTotal();
                }
                else
                {
                    if (txttax.Text != "")
                    {
                        calculatetotal();
                    }
                }
            }
        }
        private void calculatetotal()
        {
            double sumfob = 0;
            double sumcfr = 0;
            decimal? weight = 0;
            double Quantity = 0;
            double sumSoc = 0;


            if (grdPOItems.ItemsSource != null)
                foreach (var item in grdPOItems.ItemsSource as List<ProcurementProduct>)
                {

                    {
                        if (item.inquiryProduct.quantity != 0)
                        {
                            weight += (item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0) * Convert.ToDecimal(item.inquiryProduct.quantity);
                        }
                        else
                        {
                            weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;
                        }

                        Quantity += item.inquiryProduct.quantity;
                        sumfob += item.value1;
                        sumcfr += item.value2;
                        if (item.AmountSOC != 0)
                            sumSoc += item.AmountSOC;
                    }
                }
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtfob.Text = sumfob.ToString();
            txtcfr.Text = sumcfr.ToString();
            if (sumSoc != 0)
            {
                txtSoAmountSOC.Text = sumSoc.ToString();
            }
            else
                txtSoAmountSOC.Text = bill.SoAmountSOC_ER.ToString();

            decimal fob, cfr, tax = 0;
            fob = Convert.ToDecimal(txtfob.Text);

            cfr = Convert.ToDecimal(txtBillWithTax.Text);
            string str = txttax.Text.ToString();

            if (txttax.Text != null && txttax.Text != "")
            {
                if (str.IndexOf("%") == -1)
                    tax = Convert.ToDecimal(str);
                else
                    tax = Convert.ToDecimal(str.Substring(0, str.IndexOf("%")));
            }
            decimal ExchangeRate = 1;
            if (txttotalcfr.Text != "")
            {

                decimal totalcfr1 = Convert.ToDecimal(txtBillWithTax.Text);
                if (txtexchangerate.Text != "")
                {
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                    txtBasetotalcfr.Text = (ExchangeRate * totalcfr1).ToString();
                }
            }
            if (str.IndexOf("%") != -1)
            {
                txttotalfob.Text = (fob + (fob * tax / 100)).ToString();
                txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                txtBasetotalfob.Text = ((fob * ExchangeRate) + (fob * tax / 100)).ToString();

            }
            else
            {
                txttotalfob.Text = (fob + tax).ToString();
                txttotalcfr.Text = (cfr + tax).ToString();

                txtBasetotalfob.Text = ((fob * ExchangeRate) + tax).ToString();

            }
            Double remain = 0;
            if (!String.IsNullOrEmpty(txtPOremainingcfr.Text))
                remain = Math.Round(Convert.ToDouble(txtPOremainingcfr.Text), 2);

            if (cmbBillType.SelectedItem != null)
                if (cmbBillType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                {
                    if (!string.IsNullOrEmpty(txtCommision.Text))
                    {
                        var comm = Math.Round(Convert.ToDouble(txtCommision.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) +/* Math.Round(*/remain/*)*/, 2);
                        txtPOremainingcfr.Text = comm.ToString();
                        POCFRRemaining = Convert.ToDouble(txtCommision.Text);

                    }

                }

                else
                {
                    if (!string.IsNullOrEmpty(txttotalcfr.Text))
                    {
                        var remaining = Math.Round(Convert.ToDouble(txttotalcfr.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) + remain, 2).ToString();
                        txtPOremainingcfr.Text = remaining;
                        POCFRRemaining = Convert.ToDouble(txttotalcfr.Text);
                    }
                }
            decimal marginExchangeRate = 1;
            if (txttotalcfr.Text != "")
            {
                decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalestotalCfr.Text = (marginExchangeRate * totalcfr).ToString();
                }
            }

        }

        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            calculatetotal();
        }

        private void cmbPaymentTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as cmbitem) != null)
            {
                int idd = (sender as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
                    paymentTerms.ShowDialog();
                    loadPaymentTerms();

                }



            }
        }
        private void cmbIncoterm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIncoterm.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIncoterm.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmIncotermAdd incoterms = new Termss.frmIncotermAdd();
                    incoterms.ShowDialog();
                    loadIncoterms();
                }
            }
        }
        public void loadIncoterms()
        {
            cmbIncoterm.ItemsSource = SYSTEM_STATIC.incoTermSource;
            loadCaptions();
        }
        public void loadCaptions()
        {

            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (Incoterm incoterm in incoterms)
            //{
            //    cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            //}

            //cmbcaption1.ItemsSource = cmbitems;
            //cmbcaption2.ItemsSource = cmbitems;
            cmbcaption1.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbcaption2.ItemsSource = SYSTEM_STATIC.incoTermSource;

        }
        public void loadWarrantys()
        {
            cmbPOWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
            cmbSOWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
        }
        public void loadPaymentTerms()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForPO();


                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbPOPaymentTerm.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }

            try
            {
                cmbitems = new List<cmbitem>();

                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForSO();


                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbSOPaymentTerm.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }
        }

        private void txttax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9|%]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void cmbcaption1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdPOItems != null && grdPOItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdPOItems.Columns.GetColumnByFieldName("value1").Header = ((cmbitem)cmbcaption1.SelectedItem).name + symbol;

        }

        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (grdPOItems != null && grdPOItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdPOItems.Columns.GetColumnByFieldName("value1").Header = ((cmbitem)cmbcaption2.SelectedItem).name + symbol;
        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                DXMessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }

        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DXMessageBox.Show("Select Department Frist");
                lookupDepartment.Focus();
                return;
            }
        }

        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DXMessageBox.Show("Select Department Frist");
                return;
            }
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

            calculatetotal();

        }
        private void datBillOfLaddingdate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Items") != null)
            {
                Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
                frmItemadd.ShowDialog();
                //products = SYSTEM_STATIC.GetItemsForCurrentUser();
                //lookupProductsinGrid.ItemsSource = products;
            }
            else
            {
                DXMessageBox.Show("Permission required (Add New Items) to add new item!");
            }


        }
        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            (grdPOItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();


        }
        private void winBilladd_Unloaded(object sender, RoutedEventArgs e)
        {
            editbill = 0;
            saleOrderid = 0;
            billid = 0;
            if (bill != null && bill.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, bill.Id, (int)TransactionItemType.Bill, "Viewed details of Bill");

            frmCostSheet.costSheet = new ERP_BL.Databases.CostSheet();


        }

        private void CmbVendorPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbVendorPaymentStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbVendorPaymentStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Vendorss.frmVendorPaymentStatusAdd VendorPaymentStatuss = new Vendorss.frmVendorPaymentStatusAdd();
                    VendorPaymentStatuss.ShowDialog();
                    loadVendorPaymentStatus();

                }



            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {

            /// Working One
            //if ((cmbBillType.SelectedItem as cmbitem).name == "Bills-Direct COA" && cmbBillType.SelectedItem != null)
            //{
            //    CalculateGlTotal();
            //}
            //else
            {

                if (cmbPOCurrency.SelectedItem != null)
                {

                    MarketExchangeRate ExchangeRate = new MarketExchangeRate();
                    SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                    ExchangeRate = currencyRepo.getMarketexchangerate(company.Id, (cmbPOCurrency.SelectedItem as cmbitem).id);
                    salesExchangeRate = currencyRepo.getsalesexchangerate(company.Id, (cmbPOCurrency.SelectedItem as cmbitem).id);
                    if (ExchangeRate != null)
                    {

                        txtexchangerate.Text = ExchangeRate.exchangerate.ToString();
                    }

                    else if (cmbbaseCurrency.SelectedItem != null)
                        if ((cmbbaseCurrency.SelectedItem as cmbitem).id == (cmbPOCurrency.SelectedItem as cmbitem).id)
                        {
                            txtexchangerate.Text = "1";
                            txtMarginexchangerate.Text = "1";
                        }
                    if (salesExchangeRate != null)
                    {
                        txtMarginexchangerate.Text = salesExchangeRate.exchangerate.ToString();
                    }
                }

                if (cmbPOCurrency.SelectedItem as cmbitem != null)
                {

                    int idd = (cmbPOCurrency.SelectedItem as cmbitem).id;
                    currency = currencyRepo.get(idd);
                    if (idd == 0)
                    {

                        BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                        frmCurrency.ShowDialog();
                        loadCurrencies();
                    }
                    else
                    {
                        string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                        string caption1 = "";
                        string caption2 = "";
                        if (cmbcaption1.SelectedItem != null)
                            caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                        if (cmbcaption2.SelectedItem != null)
                            caption2 = (cmbcaption2.SelectedItem as cmbitem).name;


                        symbol = name.Substring(name.IndexOf("("));
                        txtBaseCurrency.Text = "Pakistani Rupee";
                        if (grdPOItems.Columns.Count != 0)
                        {
                            grdPOItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                            grdPOItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
                            lblTotal.Text = "Bill Amount" + symbol;
                            lblCFRTotal.Text = "Bill Amount" + symbol;

                            string str = lblCommision.Text;
                            if (-1 != str.IndexOf("("))
                                lblCommision.Text = (str.Substring(0, str.IndexOf("("))) + symbol;
                            else
                                lblCommision.Text = lblCommision.Text + " " + symbol;
                            string stri = lblActualMargin.Text;
                            if (-1 != stri.IndexOf("("))
                                lblActualMargin.Text = (stri.Substring(0, stri.IndexOf("("))) + symbol;
                            else
                                lblActualMargin.Text = lblActualMargin.Text + " " + symbol;
                            string stringg = lblBudgetdMargin.Text;
                            if (-1 != stringg.IndexOf("("))
                                lblBudgetdMargin.Text = (stringg.Substring(0, stringg.IndexOf("("))) + symbol;
                            else
                                lblBudgetdMargin.Text = lblBudgetdMargin.Text + " " + symbol;
                        }
                        calculateBaseCommision();
                        calculateBaseBudgetMargin();
                        calculateBaseActualMargin();
                        calculatetotal();
                    }

                }
            }



        }

        private void CmbbaseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPOCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbbaseCurrency.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbbaseCurrency.SelectedItem as cmbitem).name;

                }
                calculateBaseBudgetMargin();
                calculateBaseActualMargin();
            }
        }

        private void TxtBudgetMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            calculateBaseBudgetMargin();
        }

        private void TxtActualMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateBaseActualMargin();
        }
        public void calculateBaseActualMargin()
        {
            if (txtActualMargin.Text != "")
            {
                decimal sumbudg = 0;

                sumbudg = Convert.ToDecimal(txtActualMargin.Text);
                var Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtActualPercent.Text = decimal.Round(percent, 2).ToString();
                    txtActualPercent.Text += "%";
                }
                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                decimal margin = Convert.ToDecimal(txtActualMargin.Text);
                txtBaseActualMargin.Text = (margin * ExchangeRate).ToString();
                txtSaleAMargin.Text = (marginExchangeRate * margin).ToString();
            }
        }
        public void calculateBaseRevisedMargin()
        {
            if (txtRevisedMargin.Text != "")
            {

                var sumbudg = Convert.ToDecimal(txtRevisedMargin.Text);
                decimal Sototal = 0;
                if (Sototal != 0)
                {
                    Sototal = Convert.ToDecimal(txtSoAmount.Text);
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtRevisedPercent.Text = decimal.Round(percent, 2).ToString();
                    txtRevisedPercent.Text += "%";
                }

                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                decimal margin = (string.IsNullOrEmpty(txtRevisedMargin.Text)) ? 0 : Convert.ToDecimal(txtRevisedMargin.Text);
                txtBaseRevisedMargin.Text = (margin * ExchangeRate).ToString();
                txtSaleRevisedMargin.Text = (marginExchangeRate * margin).ToString();
            }
        }
        public void calculateBaseBudgetMargin()
        {
            if (txtBudgetMargin.Text != "")
            {
                decimal sumbudg = 0;

                sumbudg = Convert.ToDecimal(txtBudgetMargin.Text);
                if (!String.IsNullOrEmpty(txtSoAmount.Text))
                {
                    var Sototal = Convert.ToDecimal(txtSoAmount.Text);
                    if (Sototal != 0)
                    {
                        var percent = (((sumbudg) / Sototal) * 100);
                        txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                        txtBudgetedPercent.Text += "%";
                    }
                }

                decimal ExchangeRate = 1;
                decimal marginExchangeRate = 1;
                decimal margin = Convert.ToDecimal(txtBudgetMargin.Text);//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtexchangerate.Text != "")
                {
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                    txtBaseBudgetMargin.Text = (margin * ExchangeRate).ToString();

                }

                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSaleBudgetMargin.Text = (marginExchangeRate * margin).ToString();
                }
            }
        }
        public void calculateBaseCommision()
        {
            if (txtCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);

                decimal margin = Convert.ToDecimal(txtCommision.Text);

                txtBaseCommission.Text = (margin * ExchangeRate).ToString();
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalesCommision.Text = (marginExchangeRate * margin).ToString();
                }
            }
        }
        public void calculateNetBaseCommision()
        {
            if (txtNetCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);

                decimal margin = Convert.ToDecimal(txtNetCommision.Text);
                txtNetBaseCommission.Text = (margin * ExchangeRate).ToString();
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtNetSalesCommision.Text = (marginExchangeRate * margin).ToString();
                }
            }
        }
        private void TxtCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (Convert.ToDouble(txtCommision.Text) != bill.Commision)
            {
                foreach (var item in grdPOItems.ItemsSource as List<ProcurementProduct>)
                {
                    item.totalCommission = Convert.ToDouble(txtCommision.Text);
                    item.UnInvoicedSoAmount = Convert.ToDouble(Convert.ToDecimal(txtCommision.Text) - Convert.ToDecimal(bill.Commision));
                }
                grdPOItems.RefreshData();
            }
            calculateBaseCommision();
            calculatetotal();
        }

        private void DatPaymentDueFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            setPaymentTillDate();
        }

        private void TxtPaymentDueDays_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtPaymentDueDays.Text != "")
            {
                setPaymentTillDate();
            }
        }
        public void setPaymentTillDate()
        {
            if (datPaymentDueFrom.Text != "" && txtPaymentDueDays.Text != "")
                datPaymentDueTill.DateTime = datPaymentDueFrom.DateTime.AddDays(Convert.ToInt32(txtPaymentDueDays.Text));//new DateTime(datPaymentDueFrom.DateTime.Year, datPaymentDueFrom.DateTime.Month, datPaymentDueFrom.DateTime.Day+Convert.ToInt32(txtPaymentDueDays.Text));
            else
                datPaymentDueTill.Text = "";
        }

        private void Txttotalcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        private void TxtMarginexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            calculatetotal();
            calculateBaseCommision();
            calculateBaseBudgetMargin();
            calculateBaseActualMargin();
            calculateBaseRevisedMargin();

        }


        private void Txtexchangerate_KeyUp(object sender, KeyEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                       (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {
                    CalculateGlTotal();
                    calculateBaseCommision();
                    calculateBaseBudgetMargin();
                    calculateBaseRevisedMargin();
                    calculateBaseActualMargin();
                }
                else
                {
                    calculatetotal();
                    calculateBaseCommision();
                    calculateBaseBudgetMargin();
                    calculateBaseRevisedMargin();
                    calculateBaseActualMargin();
                }
            }

        }

        private void Txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //calculatetotal();
            //calculateBaseCommision();
            //calculateBaseBudgetMargin();
            //calculateBaseRevisedMargin();
            //calculateBaseActualMargin();
        }

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            btnCostSheetPunching.IsEnabled = false;

            try
            {
                string inco = "";
                string paymentterm = "";
                if (cmbPOPaymentTerm.SelectedItem != null)
                {
                    paymentterm = (cmbPOPaymentTerm.SelectedItem as cmbitem).name;
                }
                if (cmbIncoterm.SelectedItem != null)
                {
                    inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                }
                if (bill?.SaleOrder == null)
                {
                    return;
                }
                frmCostSheet frmCostSheet = new frmCostSheet(bill.SaleOrder, symbol, inco, datpoCreationdate.Text, paymentterm, txtMaker.Text, txtOrigin.Text, views, (InquiryType)cmbBillType.SelectedIndex, txtPacking.Text, cmbPOWarranty.Text, bill.isApproved, bill.isReApproved);
                frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                //frmCostSheet.lookupVendors.IsEnabled = false;
               // frmCostSheet.lookupOC.IsEnabled = false;
                frmCostSheet.ShowDialog();

                if (frmCostSheet.costSheet != null)
                {
                    bill.CostSheet = frmCostSheet.costSheet;
                    txtBudgetMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalBudgetedMargin).ToString();
                    txtRevisedMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalRevisedMargin).ToString();

                    txtActualMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) != (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalActualMargin).ToString() : "0";
                    if ((bill.isReApproved != false) && frmCostSheet.isReApproved == false)
                        bill.stage = TransactionStage.AwaitingFirstReview.ToString();
                    bill.isReApproved = frmCostSheet.isReApproved;
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
                views = UsersRepo.getViwerInfo(bill.Id, (int)TransactionItemType.Bill);
                grdUsers.ItemsSource = views;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

            if (bill.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null)
                {
                    bill.isReviewed = true;
                    bill.isApproved = true;
                    bill.stage = TransactionStage.Approved.ToString();
                    billRepo.update(bill);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && bill.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && bill.InterDepartment != null && bill.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), bill.Id, TransactionItemType.Bill);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), bill.Id, TransactionItemType.Bill);
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
                    if (bill.currency != null)
                    {
                        symbolCurr = bill.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Bill (Amount OC) having value: " + bill.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Bill Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bill.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
                {
                    bill.isReviewed = true;
                    bill.needReview = false;
                    bill.stage = TransactionStage.AwaitingApproval.ToString();

                    billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
                {
                    bill.isReviewed = true;
                    bill.needReview = true;
                    bill.stage = TransactionStage.AwaitingSecondReview.ToString();

                    billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }


            else if (bill.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null)
                {
                    bill.isReviewed = true;
                    bill.isApproved = false;
                    bill.stage = TransactionStage.AwaitingApproval.ToString();
                    billRepo.update(bill);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && bill.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && bill.InterDepartment != null && bill.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), bill.Id, TransactionItemType.Bill);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), bill.Id, TransactionItemType.Bill);
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
                    if (bill.currency != null)
                    {
                        symbolCurr = bill.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Bill (Amount OC) having value: " + bill.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "Bill UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bill.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }

            if (bill.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null)
                {
                    bill.isReviewed = true;
                    bill.PendingForClosing = false;
                    bill.stage = TransactionStage.Closed.ToString();
                    billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Closing, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
                {
                    bill.isReviewed = true;
                    bill.needReview = false;
                    bill.stage = TransactionStage.AwaitingApproval.ToString();

                    billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
                {
                    bill.isReviewed = true;
                    bill.needReview = true;
                    bill.stage = TransactionStage.AwaitingSecondReview.ToString();

                    billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }

            loadcomments();
        }
        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {

                string fname = viewInfo.User.employee.person.FName;
                string lname = viewInfo.User.employee.person.LName;
                e.Value = fname + " " + lname;

            }
        }
        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (bill.isApproved != true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null)
                {
                    bill.isReviewed = true;
                    bill.isApproved = false;
                    bill.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
                {
                    bill.isReviewed = false;
                    bill.stage = TransactionStage.Rejected.ToString();
                    //if(bill.isApproved == null)
                    //{
                    //    bill.isApproved = false;

                    //}
                    //billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
                {
                    bill.isReviewed = false;

                    bill.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

                }
            }
            else if (bill.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null)
                {
                    bill.isReviewed = true;
                    //if (bill.PendingForClosing == null)
                    {
                        bill.PendingForClosing = true;

                    }
                    //bill.PendingForClosing = false;
                    bill.stage = TransactionStage.Rejected.ToString();
                    //billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
                {
                    bill.isReviewed = true;
                    bill.needReview = false;
                    bill.stage = TransactionStage.Rejected.ToString();
                    //if (bill.PendingForClosing == null)
                    //{
                    //    bill.PendingForClosing = true;

                    //}
                    //billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
                {
                    //if (bill.PendingForClosing == null)
                    //{
                    //    bill.PendingForClosing = true;

                    //}
                    bill.isReviewed = true;
                    bill.needReview = true;
                    bill.stage = TransactionStage.Rejected.ToString();

                    //billRepo.update(bill);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, bill.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    billRepo.update(bill);
                    notificationsRepo.Add("Bill Rejected", bill.Id, TransactionItemType.Bill, "Bill with refrence # " + bill.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", bill.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Bill" + frmInputBox.comment, null);

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

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                inputBox.ShowDialog();

            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Bill);
                inputBox.ShowDialog();

            }

            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (bill != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && bill.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(bill.Id, TransactionItemType.Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }


                        //foreach (var user in frmInputBox.Comment.TaggedList)
                        //{
                        //    if (frmInputBox.FlagForTag == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", null);

                        //}
                        //foreach (var user in frmInputBox.Comment.CCUsersList)
                        //{
                        //    if (frmInputBox.FlagForCC == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        //}
                    }

                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bill.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Bill first to add a comment!");
                }

            }
        }
        public void loadcomments()
        {
            try
            {
                if (bill != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(bill.Id, TransactionItemType.Bill);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void BtnSummarySheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inco = "";
                string paymentterm = "";
                if (cmbPOPaymentTerm.SelectedItem != null)
                {
                    paymentterm = (cmbPOPaymentTerm.SelectedItem as cmbitem).name;
                }
                if (cmbIncoterm.SelectedItem != null)
                {
                    inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                }
                frmCommissionSummarySheet frmSummarySheet = new frmCommissionSummarySheet(bill.SaleOrder, txtFinanaceRef.Text, txtsaleOrderref.Text, txtOfferRefNo.Text, lookupCustomer.Text, (cmbPOCurrency.SelectedItem as cmbitem).id, txtCommision.Text, datsaleOrderdate.Text, paymentterm, (InquiryType)cmbBillType.SelectedIndex, /*lookupPOPrincipal.Text*/"", txttotalcfr.Text, txttotalfob.Text, txtPacking.Text, inco, datDeliverydate.DateTime, cmbTransshipment.Text, datpoCreationdate.Text, txtSalesref.Text, views);
                frmSummarySheet.ShowDialog();
                if ((InquiryType)cmbBillType.SelectedIndex == InquiryType.Principal)
                {
                    if (frmCommissionSummarySheet.summarySheet != null)
                    {
                        bill.CostSheet = null;
                        bill.CommissionSummarySheet = frmCommissionSummarySheet.summarySheet;
                        txtCommision.Text = (bill.CommissionSummarySheet.SOCommission).ToString();
                        txtNetCommision.Text = (bill.CommissionSummarySheet.netSOCommission).ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }



        private void LstComments_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //int index = lstComments.IndexFromPoint(e.Location);
            //lstComments.SelectedIndex = index;
        }

        private void TxtNetCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateNetBaseCommision();

        }
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bill.Id, TransactionItemType.Bill);
                grdAttachments.Visibility = Visibility.Visible;
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
                        if (str.Contains("Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Bill);
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
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (OrderId != 0)
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
                        destination += "Attachments\\Bill\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Bill.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Bill);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Bill, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bill.Id, (int)TransactionItemType.Bill, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Bill);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
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


                            //DXMessageBox.Show("Attachment Uploaded");


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

        private void TxtRevisedMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateBaseRevisedMargin();
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OrderId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(OrderId, TransactionItemType.Bill);
                trackingWindow.ShowDialog();
            }
        }

        private void TxtPER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {

                    CalculateGlTotal();
                }
                //else 
                //{
                if (!String.IsNullOrEmpty(txtSOCER.Text) && !String.IsNullOrEmpty(txtBillWithTax.Text))
                {
                    var PER = Convert.ToDouble(txtSOCER.Text);


                    var PERAmount = Convert.ToDouble(txtBillWithTax.Text);
                    txtSoAmountSOC.Text = (PER * PERAmount).ToString();
                }
                else

                    txtSoAmountSOC.Text = txtBillWithTax.Text;
                //}
            }


        }
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {

            if (bill.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Bill") != null))
            {
                if (DXMessageBox.Show("This So is currently in the list of Void Bill s! Do you want to remove it from Void?", "Remove Void Bill", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bill.isVoid = false;
                    billRepo.setSotoVoid(bill.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && bill.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && bill.InterDepartment != null && bill.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), bill.Id, TransactionItemType.Bill);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), bill.Id, TransactionItemType.Bill);
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
                    if (bill.currency != null)
                    {
                        symbolCurr = bill.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Bill (Amount OC) having value: " + bill.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Bill UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bill.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Bill") != null)
            {
                if (DXMessageBox.Show("This So is not currently in the list of Void Bill s! Do you want to move it to Void Saleorders?", "Add to Void Saleorders", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    bill.isVoid = true;
                    billRepo.setSotoVoid(bill.Id, true);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && bill.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && bill.InterDepartment != null && bill.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), bill.Id, TransactionItemType.Bill);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else if (bill.department != null && bill.department.Id != 0 && bill.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), bill.Id, TransactionItemType.Bill);
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
                    if (bill.currency != null)
                    {
                        symbolCurr = bill.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Bill (Amount OC) having value: " + bill.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Bill Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(bill.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + bill.SalesReferenceNo, bill.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
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
                if (bill.Id != 0)
                {
                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Bill);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Bill);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (bill != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && bill.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(bill.Id, TransactionItemType.Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Vendor Bill with System Ref #" + bill.SyetmReferenceNo, bill.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Bill, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Bill, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Bill, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (bill.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Bill first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void cmbBillType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbBillType.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbBillType.SelectedItem as cmbitem).id;
                string typeName = (cmbBillType.SelectedItem as cmbitem).name;

                if (idd == 0)
                {
                    Procurementss.frmBillTypeAdd typeadd = new frmBillTypeAdd();
                    typeadd.ShowDialog();
                    loadBillTypes();
                }

                setVisibilityFields();

            }
        }

        private void setVisibilityFields()
        {
            if ((cmbBillType.SelectedItem as cmbitem).name == "New Bill")
            {
                cmbSOCurrency.IsEnabled = true;
                lblPoReferenceNo.Visibility = Visibility.Collapsed;
                txtPoReferenceNo.Visibility = Visibility.Collapsed;
                lblOfferRefNo.Visibility = Visibility.Collapsed;
                txtOfferRefNo.Visibility = Visibility.Collapsed;
                lblSoPaymentTerms.Visibility = Visibility.Collapsed;
                cmbSOPaymentTerm.Visibility = Visibility.Collapsed;
                lblSalesRefNo.Visibility = Visibility.Collapsed;
                txtSalesref.Visibility = Visibility.Collapsed;
                lblSoRefNo.Visibility = Visibility.Collapsed;
                txtsaleOrderref.Visibility = Visibility.Collapsed;

                lblPoPaymentTerms.Visibility = Visibility.Collapsed;
                cmbPOPaymentTerm.Visibility = Visibility.Collapsed;
                lblIncoTerms.Visibility = Visibility.Collapsed;
                cmbIncoterm.Visibility = Visibility.Collapsed;
                lblTotalWeight.Visibility = Visibility.Collapsed;
                txtWeight.Visibility = Visibility.Collapsed;
                lblTotalQuantity.Visibility = Visibility.Collapsed;
                txtQuantity.Visibility = Visibility.Collapsed;
                lblPERAmount.Visibility = Visibility.Collapsed;
                cmbSOCurrency.Visibility = Visibility.Collapsed;
                lblPER.Visibility = Visibility.Collapsed;
                txtSOCER.Visibility = Visibility.Collapsed;
                lblSoAmountPER.Visibility = Visibility.Collapsed;
                txtSoAmountSOC.Visibility = Visibility.Collapsed;
                grpLCInfo.Visibility = Visibility.Collapsed;
                grpShipmentInfo.Visibility = Visibility.Collapsed;
                grpbondinfo.Visibility = Visibility.Collapsed;
                grpMargindetails.Visibility = Visibility.Collapsed;
                lblShipmentDate.Visibility = Visibility.Collapsed;
                datShipmentdate.Visibility = Visibility.Collapsed;
                lblReceivedDate.Visibility = Visibility.Collapsed;
                datOrderConfirmationdate.Visibility = Visibility.Collapsed;
                lblRevisedShipmentDate.Visibility = Visibility.Collapsed;
                datRevisedShipmentDate.Visibility = Visibility.Collapsed;

                lblWarratnty.Visibility = Visibility.Collapsed;
                cmbSOWarranty.Visibility = Visibility.Collapsed;
                lblPOWarratnty.Visibility = Visibility.Collapsed;
                cmbPOWarranty.Visibility = Visibility.Collapsed;
                grpComments.Visibility = Visibility.Collapsed;

                btnCostSheet.Visibility = Visibility.Collapsed;

                lblCardUser.Visibility = Visibility.Collapsed;
                cmbxCardUser.Visibility = Visibility.Collapsed;
                lblCreditCardNo.Visibility = Visibility.Collapsed;
                cmbxCreditCardNo.Visibility = Visibility.Collapsed;
                lblSecondaryCardNo.Visibility = Visibility.Collapsed;
                txtSecondaryCardNo.Visibility = Visibility.Collapsed;
                lblPrimaryCardHolder.Visibility = Visibility.Collapsed;
                txtPrimaryCardHolder.Visibility = Visibility.Collapsed;
                lblSecondaryCardHolder.Visibility = Visibility.Collapsed;
                txtSecondaryCardHolder.Visibility = Visibility.Collapsed;
                txtSOCER1.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                grdBillGridControl.Visibility = Visibility.Collapsed;
            }

            else if ((cmbBillType.SelectedItem as cmbitem).name == "Linked Bill")
            {

                cmbSOCurrency.IsEnabled = false;

                lblPoReferenceNo.Visibility = Visibility.Visible;
                txtPoReferenceNo.Visibility = Visibility.Visible;



                lblOfferRefNo.Visibility = Visibility.Collapsed;
                txtOfferRefNo.Visibility = Visibility.Collapsed;

                lblSoPaymentTerms.Visibility = Visibility.Collapsed;
                cmbSOPaymentTerm.Visibility = Visibility.Collapsed;

                lblSalesRefNo.Visibility = Visibility.Collapsed;
                txtSalesref.Visibility = Visibility.Collapsed;

                lblSoRefNo.Visibility = Visibility.Collapsed;
                txtsaleOrderref.Visibility = Visibility.Collapsed;


                lblPoPaymentTerms.Visibility = Visibility.Collapsed;
                cmbPOPaymentTerm.Visibility = Visibility.Collapsed;

                lblIncoTerms.Visibility = Visibility.Collapsed;
                cmbIncoterm.Visibility = Visibility.Collapsed;

                lblTotalWeight.Visibility = Visibility.Collapsed;
                txtWeight.Visibility = Visibility.Collapsed;

                lblTotalQuantity.Visibility = Visibility.Collapsed;
                txtQuantity.Visibility = Visibility.Collapsed;

                lblPERAmount.Visibility = Visibility.Visible;
                cmbSOCurrency.Visibility = Visibility.Visible;
                lblPER.Visibility = Visibility.Visible;
                txtSOCER.Visibility = Visibility.Visible;
                lblSoAmountPER.Visibility = Visibility.Visible;
                txtSoAmountSOC.Visibility = Visibility.Visible;

                grpLCInfo.Visibility = Visibility.Collapsed;
                grpShipmentInfo.Visibility = Visibility.Collapsed;
                grpbondinfo.Visibility = Visibility.Collapsed;
                grpMargindetails.Visibility = Visibility.Collapsed;

                lblShipmentDate.Visibility = Visibility.Collapsed;
                datShipmentdate.Visibility = Visibility.Collapsed;

                lblReceivedDate.Visibility = Visibility.Collapsed;
                datOrderConfirmationdate.Visibility = Visibility.Collapsed;

                lblRevisedShipmentDate.Visibility = Visibility.Collapsed;
                datRevisedShipmentDate.Visibility = Visibility.Collapsed;


                lblWarratnty.Visibility = Visibility.Collapsed;
                cmbSOWarranty.Visibility = Visibility.Collapsed;

                lblPOWarratnty.Visibility = Visibility.Collapsed;
                cmbPOWarranty.Visibility = Visibility.Collapsed;

                grpComments.Visibility = Visibility.Collapsed;



                btnCostSheet.Visibility = Visibility.Visible;

                lblCardUser.Visibility = Visibility.Collapsed;
                cmbxCardUser.Visibility = Visibility.Collapsed;
                lblCreditCardNo.Visibility = Visibility.Collapsed;
                cmbxCreditCardNo.Visibility = Visibility.Collapsed;
                lblSecondaryCardNo.Visibility = Visibility.Collapsed;
                txtSecondaryCardNo.Visibility = Visibility.Collapsed;
                lblPrimaryCardHolder.Visibility = Visibility.Collapsed;
                txtPrimaryCardHolder.Visibility = Visibility.Collapsed;
                lblSecondaryCardHolder.Visibility = Visibility.Collapsed;
                txtSecondaryCardHolder.Visibility = Visibility.Collapsed;

                txtSOCER1.Visibility = Visibility.Visible;
                grdGridControl.Visibility = Visibility.Visible;
                grdBillGridControl.Visibility = Visibility.Collapsed;

            }
            else if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
            {
                grdBillGridControl.Visibility = Visibility.Visible;
                cmbSOCurrency.IsEnabled = true;
            }
            else
            {
                grdBillGridControl.Visibility = Visibility.Collapsed;

                cmbSOCurrency.IsEnabled = true;

                cmbBillType.SelectedItem = null;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = false;

        }

        private void lookupInterCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
            {
                loadInterCompanyDepartments();
            }
        }

        private void lookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterDepartment = lookupInterDepartment.SelectedItem as Department;

        }

        private void txtSoAmountSOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {

                    CalculateGlTotal();
                }
                //else
                //{
                if (!string.IsNullOrEmpty(txtSoAmountSOC.Text))
                {
                    var PER = Convert.ToDouble(txtSoAmountSOC.Text);


                    if (!String.IsNullOrEmpty(txtBillWithTax.Text))
                    {

                        var PERAmount = Convert.ToDouble(txtBillWithTax.Text);
                        //var PERAmount = Convert.ToDouble(txttotalcfr.Text);
                        txtSOCER.Text = (Math.Round(PER / PERAmount, 4)).ToString();
                        txtSOCER1.Text = (Math.Round(PERAmount / PER, 4)).ToString();
                    }

                }
                else

                    txtSOCER.Text = txtBillWithTax.Text;
                //}
            }

        }

        private void CmbxCardUser_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
        }

        private void CmbxCreditCardNo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxCreditCardNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            BillRepo billrepo = new BillRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            bool isFullyPaid = true;
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null) ? true : false)
            {
                UsersRepo usersRepo = new UsersRepo();
                if (billid == 0)
                {
                    return;
                }
                bill = billRepo.get(billid);

                if (bill.isApproved == false)
                {
                    DXMessageBox.Show("Bill is pending for approval!");
                    return;
                }

                if (bill.Payments == null || bill.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    isFullyPaid = false;
                }
                else
                {
                    var pymntAmount = Math.Round((bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)), 2);
                    if (bill.billWithTax != null)
                    {
                        if ((pymntAmount - bill.billWithTax) != 0)
                            isFullyPaid = false;
                    }
                    else if ((pymntAmount - bill.totalCFRValue) != 0)
                        isFullyPaid = false;
                }

                if (isFullyPaid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Vendor Bills without Payments") == null)
                {
                    DXMessageBox.Show("Permission required to close Unpaid Bills!");
                    return;
                }


                if (isFullyPaid == false)
                {
                    MessageBoxResult result = DevExpress.Xpf.Core.DXMessageBox.Show("This Bill is not Paid Yet, Do you want to Close it?", "Bill is Unpaid", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                var row = bill;

                if (row.BillStatus != null)
                {
                    oldStatus = row.BillStatus;
                }
                Billss.ucStatuschange.inActiveStatuses = 1;

                Billss.ucStatuschange.billid = (int)billid;
                Billss.frmBillStatusChange statusChange = new Billss.frmBillStatusChange(billrepo);
                var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();
                if (Billss.ucStatuschange.bill.Id != 0)
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null) ? true : false)
                    {
                        Billss.ucStatuschange.bill.PendingForClosing = false;
                        Billss.ucStatuschange.bill.stage = TransactionStage.Approved.ToString();

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingApproval.ToString();
                        if (Billss.ucStatuschange.bill.PendingForClosing == null)
                        {
                            Billss.ucStatuschange.bill.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                    {
                        Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (Billss.ucStatuschange.bill.PendingForClosing != true)
                        {
                            Billss.ucStatuschange.bill.PendingForClosing = true;

                        }
                        //Billss.ucStatuschange.bill.PendingForClosing = true;

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                    }
                    else
                    {
                        Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingFirstReview.ToString();

                        Billss.ucStatuschange.bill.PendingForClosing = true;
                        usersRepo.Add(TransactionInfo.Closed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                    }
                Billss.ucStatuschange.bill.LastStatusChangeDate = System.DateTime.Now;
                Billss.ucStatuschange.bill.ClosingDate = System.DateTime.Now;
                if (row.BillStatus != Billss.ucStatuschange.bill.BillStatus)
                    usersRepo.Add(TransactionInfo.Status_Changed, bill.Id, (int)TransactionItemType.Bill, "While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                if (oldStatus.Status != Billss.ucStatuschange.bill.BillStatus.Status)
                {
                    UsersRepo userRepo = new UsersRepo();
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Bill has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), bill.Id, TransactionItemType.Bill);
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

                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = Billss.ucStatuschange.bill.BillStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Bill (Amount OC) having value: " + row.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);

                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + row.SalesReferenceNo, row.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + row.SalesReferenceNo, row.Id, TransactionItemType.Bill, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                }




                row = Billss.ucStatuschange.bill;

                billrepo.updateStatus(row.Id, row.BillStatus);
                MessageBox.Show("Bill status changed to InActive (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                myWindow = Window.GetWindow(this);
                myWindow.Close();


            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Bill Directly.");
            }
        }

        private void ChkWHT_Checked(object sender, RoutedEventArgs e)
        {
            lookUpWHT.IsEnabled = true;

            if (lookUpWHT.SelectedIndex > -1)
            {
                var amount = Convert.ToDouble(txtBillWithTax.Text);
                var item = lookUpWHT.SelectedItem as TaxName;
                var percent = item.percentage;
                var percentAmount = (percent / 100) * amount;
                txtWhtAmount.Text = percentAmount.ToString();
                amount = amount - percentAmount;
                txtBillAfterTax.Text = amount.ToString();
            }
        }

        private void ChkWHT_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpWHT.IsEnabled = false;
            txtBillAfterTax.Text = txtBillWithTax.Text;
        }


        public void LoadTaxes()
        {

        }
        private void ChkTax_Checked(object sender, RoutedEventArgs e)
        {


            chkUnAdjustedTax.IsChecked = false;
            lookUpTax.IsEnabled = true;
            lookUpTax.ItemsSource = taxRepo.getAllAdjustedTaxes();





            //lookUpTax.IsEnabled = true;

            //if (lookUpTax.SelectedIndex > -1)
            //{
            //    var amount = Convert.ToDouble(txtcfr.Text);
            //    var item = lookUpTax.SelectedItem as TaxName;
            //    var percent = item.percentage;
            //    var percentAmount = (percent / 100) * amount;
            //    txtTaxAmount.Text = percentAmount.ToString();
            //    amount = amount + percentAmount;
            //    txtBillWithTax.Text = amount.ToString();
            //    if ((lookUpTax.SelectedItem as TaxName).isManual != false)
            //    {
            //        txtTaxAmount.IsReadOnly = false;
            //        txtTaxAmount.Text = bill.taxAmount.ToString();
            //    }
            //    else
            //    {
            //        txtTaxAmount.IsReadOnly = true;
            //    }
            //}

            if (chkWHT.IsChecked == true)
            {
                var item = lookUpWHT.SelectedItem as TaxName;
                var percent = item.percentage;
                var amount = Convert.ToDouble(txtBillWithTax.Text);

                var percentAmount = (percent / 100) * amount;
                txtWhtAmount.Text = percentAmount.ToString();
                amount = amount - percentAmount;
                txtBillAfterTax.Text = amount.ToString();
            }
        }
        private void ChkTax_Unchecked(object sender, RoutedEventArgs e)
        {
            //lookUpTax.IsEnabled = false;
            //txtBillWithTax.Text = txtcfr.Text;
            lookUpTax.IsEnabled = false;




            if (chkWHT.IsChecked == true)
            {
                var item = lookUpWHT.SelectedItem as TaxName;
                var percent = item.percentage;
                var amount = Convert.ToDouble(txtBillWithTax.Text);

                var percentAmount = (percent / 100) * amount;
                txtWhtAmount.Text = percentAmount.ToString();
                amount = amount - percentAmount;
                txtBillAfterTax.Text = amount.ToString();
            }

        }

        private void LookUpTax_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CalculateGlTotal();
            var item = lookUpTax.SelectedItem as TaxName;
            var percent = item.percentage;
            var amount = Convert.ToDouble(txtcfr.Text);

            var percentAmount = (percent / 100) * amount;
            txtTaxAmount.Text = percentAmount.ToString();
            amount = amount + percentAmount;
            //txtBillWithTax.Text = amount.ToString();

            if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
            {
                var amount1 = Convert.ToDouble(txtBillWithTax.Text);
                var item1 = lookUpWHT.SelectedItem as TaxName;
                var percent1 = item1.percentage;
                var percentAmount1 = percent1 / 100 * amount1;
                txtWhtAmount.Text = percentAmount1.ToString();
                amount1 = amount1 - percentAmount1;
                //txtBillAfterTax.Text = amount1.ToString();
            }

            if ((lookUpTax.SelectedItem as TaxName).isManual != false)
            {
                txtTaxAmount.IsReadOnly = false;
                //txtTaxAmount.Text = bill.taxAmount.ToString();
            }
            else
            {
                txtTaxAmount.IsReadOnly = true;
            }


        }


        private void LookUpWHT_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {


            CalculateGlTotal();



            var item = lookUpWHT.SelectedItem as TaxName;
            var percent = item.percentage;
            var amount = Convert.ToDouble(txtBillWithTax.Text);
            var percentAmount = (percent / 100) * amount;
            txtWhtAmount.Text = percentAmount.ToString();
            amount = amount - percentAmount;
            //txtBillAfterTax.Text = amount.ToString();


        }

        private void Txtcfr_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void Txtcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {
                    CalculateGlTotal();
                }
                else
                {







                    //if (chkTax.IsChecked == true && lookUpTax.SelectedIndex > -1)
                    //{
                    //    var item = lookUpTax.SelectedItem as TaxName;
                    //    var percent = item.percentage;
                    //    var percentAmount = (percent / 100) * amount;
                    //    txtTaxAmount.Text = percentAmount.ToString();
                    //    amount = amount + percentAmount;
                    //    txtBillWithTax.Text = amount.ToString();
                    //    if ((lookUpTax.SelectedItem as TaxName).isManual != false)
                    //    {
                    //        txtTaxAmount.IsReadOnly = false;
                    //        txtTaxAmount.Text = bill.taxAmount.ToString();
                    //    }
                    //    else
                    //    {
                    //        txtTaxAmount.IsReadOnly = true;
                    //    }
                    //}
                    //else
                    //{
                    //    txtBillWithTax.Text = amount.ToString();
                    //}

                    //if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
                    //{
                    //    var item = lookUpWHT.SelectedItem as TaxName;
                    //    var percent = item.percentage;
                    //    var percentAmount = (percent / 100) * amount;
                    //    txtWhtAmount.Text = percentAmount.ToString();
                    //    amount = amount - percentAmount;
                    //    txtBillAfterTax.Text = amount.ToString();
                    //}
                    //else
                    //{
                    //    txtBillAfterTax.Text = txtBillWithTax.Text;
                    //}
                }
                var amount = Convert.ToDouble(txtcfr.Text);
                var taxAmount = Convert.ToDouble(txtTaxAmount.Text);
                var taxWHTAmount = Convert.ToDouble(txtWhtAmount.Text);
                txtBillWithTax.Text = Math.Round((amount + taxAmount), 2).ToString();
                txtBillAfterTax.Text = Math.Round((amount + taxAmount - taxWHTAmount), 2).ToString();
            }



        }
        private void TxtBasetotalcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {

                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                      (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {

                    CalculateGlTotal();
                }
                else
                {
                    decimal TotalAmount = 1;

                    if (txtBillWithTax.Text != "" && txtBillWithTax.Text != "0")
                    {
                        decimal totalcfr1 = Convert.ToDecimal(txtBillWithTax.Text);
                        if (txtexchangerate.Text != "")
                        {
                            TotalAmount = Math.Round(Convert.ToDecimal(txtBasetotalcfr.Text), 2);
                            txtexchangerate.Text = (Math.Round(TotalAmount / totalcfr1, 2)).ToString();
                        }
                    }
                }
            }

        }

        private void BtnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {

            btnCostSheet.IsEnabled = false;

            try
            {
                var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Bill?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Bill") != null)

                    {
                        if (saleOrder.Id != 0 && !string.IsNullOrEmpty(txtSoAmountSOC.Text) && bill.Id != 0)
                        {
                            winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(saleOrder, Convert.ToDecimal(txtSoAmountSOC.Text), bill.Id);
                            winSelectCostSheetFields.ShowDialog();
                        }
                    }
                    else
                        DXMessageBox.Show("You do not have permisssion Update CostSheet from Bill", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnAddItem_Click_1(object sender, RoutedEventArgs e)
        {
            frmCostSheetFieldAdd productCostSheetFieldAdd = new frmCostSheetFieldAdd();
            productCostSheetFieldAdd.ShowDialog();
        }
        public List<JournalTransaction> getJournalTransactions()
        {
            var procurementProducts = grdBillItems.ItemsSource as List<BillItem>;
            if (bill.journalTransactions == null || bill.journalTransactions.Count == 0)
            {

                foreach (var procurementProduct in procurementProducts)
                {

                    if (procurementProduct.creditAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Loan ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Credit_Card ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Equity ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Income ||
                                   procurementProduct.creditAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = procurementProduct.BillAmount;
                            }
                            else
                            {
                                total = 0 - procurementProduct.BillAmount;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {

                                accountId = procurementProduct.creditAccount.Id,
                                coaTransactionsType = coaTransactionsType.Bill,
                                Bill_Id = bill.Id,
                                creationDate = bill.GLPostingDate,
                                memo = txtFinanaceRef.Text,
                                transactionRefno = txtFinanaceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                credit = procurementProduct.BillAmount,
                                total = total,
                                deptId = department.Id,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id
                            });
                        }


                    }
                    if (procurementProduct.debitAccount != null)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                  procurementProduct.debitAccount.accountType == COA_AccountType.Loan ||
                                   procurementProduct.debitAccount.accountType == COA_AccountType.Credit_Card ||
                                  procurementProduct.debitAccount.accountType == COA_AccountType.Equity ||
                                   procurementProduct.debitAccount.accountType == COA_AccountType.Accounts_Payable ||
                                  procurementProduct.debitAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   procurementProduct.debitAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                  procurementProduct.debitAccount.accountType == COA_AccountType.Income ||
                                   procurementProduct.debitAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0 - procurementProduct.BillAmount;
                            }
                            else
                            {
                                total = procurementProduct.BillAmount - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {

                                accountId = procurementProduct.debitAccount.Id,
                                coaTransactionsType = coaTransactionsType.Bill,
                                Bill_Id = bill.Id,
                                creationDate = bill.GLPostingDate,
                                memo = txtFinanaceRef.Text,
                                transactionRefno = txtFinanaceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                debit = procurementProduct.BillAmount,
                                credit = 0,
                                total = total,
                                deptId = department.Id,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id
                            });
                        }
                    }
                }
                if (lookUpTax.SelectedIndex != -1)
                {
                    var billTax = taxRepo.getTaxtById((lookUpTax.SelectedItem as TaxName).Id);
                    if (billTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                  billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0 - Convert.ToDouble(txtTaxAmount.Text);
                            }
                            else
                            {
                                total = Convert.ToDouble(txtTaxAmount.Text) - 0;
                            }
                            JournalTransaction taxTransaction = new JournalTransaction();
                            {
                                taxTransaction.accountId = billTax.COA_Id;
                                taxTransaction.credit = 0;
                                taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                taxTransaction.deptId = department.Id;
                                taxTransaction.Bill_Id = bill.Id;
                                taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                taxTransaction.userId = bill.user_Id;
                                taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                taxTransaction.creationDate = bill.GLPostingDate;
                                taxTransaction.total = total;
                                taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                            }
                            journalTransactions.Add(taxTransaction);
                        }
                    }
                }
                if (lookUpWHT.SelectedIndex != -1)
                {
                    var billTax = taxRepo.getTaxtById((lookUpWHT.SelectedItem as TaxName).Id);
                    if (billTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                  billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                  billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                   billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = Convert.ToDouble(txtWhtAmount.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txtWhtAmount.Text);
                            }
                            JournalTransaction taxTransaction = new JournalTransaction();
                            {
                                taxTransaction.accountId = billTax.COA_Id;
                                taxTransaction.credit = Convert.ToDouble(txtWhtAmount.Text);
                                taxTransaction.debit = 0;
                                taxTransaction.deptId = department.Id;
                                taxTransaction.Bill_Id = bill.Id;
                                taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                taxTransaction.userId = bill.user_Id;
                                taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                taxTransaction.creationDate = bill.GLPostingDate;
                                taxTransaction.total = total;
                                taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                            }
                            journalTransactions.Add(taxTransaction);
                        }
                    }

                    journalTransactions.Where(w => w.credit != 0 && w.credit != Convert.ToDouble(txtWhtAmount.Text)).ToList().ForEach(w => w.credit = Convert.ToDouble(txtBillAfterTax.Text));
                    journalTransactions.Where(w => w.debit != 0 && w.credit != Convert.ToDouble(txtWhtAmount.Text)).ToList().ForEach(w => w.debit = Convert.ToDouble(txtBillAfterTax.Text));

                }
            }
            else
            {
                var dbTrans = bill.journalTransactions.First();

                foreach (var item in procurementProducts)
                {

                    if (item.creditAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                      item.creditAccount.accountType == COA_AccountType.Loan ||
                                       item.creditAccount.accountType == COA_AccountType.Credit_Card ||
                                      item.creditAccount.accountType == COA_AccountType.Equity ||
                                       item.creditAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      item.creditAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       item.creditAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      item.creditAccount.accountType == COA_AccountType.Income ||
                                       item.creditAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = item.BillAmount;
                                }
                                else
                                {
                                    total = 0 - item.BillAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {

                                    accountId = item.creditAccount.Id,
                                    coaTransactionsType = coaTransactionsType.Bill,
                                    Bill_Id = bill.Id,
                                    creationDate = bill.GLPostingDate,
                                    memo = txtFinanaceRef.Text,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = item.BillAmount,
                                    total = total,
                                    deptId = department.Id,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id,
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
                                      item.creditAccount.accountType == COA_AccountType.Loan ||
                                       item.creditAccount.accountType == COA_AccountType.Credit_Card ||
                                      item.creditAccount.accountType == COA_AccountType.Equity ||
                                       item.creditAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      item.creditAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       item.creditAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      item.creditAccount.accountType == COA_AccountType.Income ||
                                       item.creditAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = item.BillAmount;
                                }
                                else
                                {
                                    total = 0 - item.BillAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {

                                    accountId = item.creditAccount.Id,
                                    coaTransactionsType = coaTransactionsType.Bill,
                                    Bill_Id = bill.Id,
                                    creationDate = bill.GLPostingDate,
                                    memo = txtFinanaceRef.Text,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = item.BillAmount,
                                    total = total,
                                    deptId = department.Id,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id
                                });

                            }

                        }
                    }
                    if (item.debitAccount != null)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                      item.debitAccount.accountType == COA_AccountType.Loan ||
                                       item.debitAccount.accountType == COA_AccountType.Credit_Card ||
                                      item.debitAccount.accountType == COA_AccountType.Equity ||
                                       item.debitAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      item.debitAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       item.debitAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      item.debitAccount.accountType == COA_AccountType.Income ||
                                       item.debitAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - item.BillAmount;
                                }
                                else
                                {
                                    total = item.BillAmount - 0;
                                }

                                journalTransactions.Add(new JournalTransaction()
                                {

                                    accountId = item.debitAccount.Id,
                                    coaTransactionsType = coaTransactionsType.Bill,
                                    Bill_Id = bill.Id,
                                    creationDate = bill.GLPostingDate,
                                    memo = txtFinanaceRef.Text,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    debit = item.BillAmount,
                                    credit = 0,
                                    total = total,
                                    deptId = department.Id,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions,
                                    ReconcilationId = null,
                                    reconcilationDate = null,
                                    isReconciled = false

                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                      item.debitAccount.accountType == COA_AccountType.Loan ||
                                       item.debitAccount.accountType == COA_AccountType.Credit_Card ||
                                      item.debitAccount.accountType == COA_AccountType.Equity ||
                                       item.debitAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      item.debitAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       item.debitAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      item.debitAccount.accountType == COA_AccountType.Income ||
                                       item.debitAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - item.BillAmount;
                                }
                                else
                                {
                                    total = item.BillAmount - 0;
                                }

                                journalTransactions.Add(new JournalTransaction()
                                {

                                    accountId = item.debitAccount.Id,
                                    coaTransactionsType = coaTransactionsType.Bill,
                                    Bill_Id = bill.Id,
                                    creationDate = bill.GLPostingDate,
                                    memo = txtFinanaceRef.Text,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    debit = item.BillAmount,
                                    credit = 0,
                                    total = total,
                                    deptId = department.Id,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id
                                });
                            }
                        }
                    }
                }
                if (lookUpTax.SelectedIndex != -1)
                {
                    var billTax = taxRepo.getTaxtById((lookUpTax.SelectedItem as TaxName).Id);
                    if (billTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                      billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txtTaxAmount.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txtTaxAmount.Text) - 0;
                                }

                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = billTax.COA_Id;
                                    taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                    taxTransaction.credit = 0;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.Bill_Id = bill.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = bill.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                    taxTransaction.creationDate = bill.GLPostingDate;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                                    taxTransaction.reconcilationDate = null;
                                    taxTransaction.ReconcilationId = null;
                                    taxTransaction.isReconciled = false;
                                    taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                            else
                            {
                                double total = 0;
                                if (
                                      billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txtTaxAmount.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txtTaxAmount.Text) - 0;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = billTax.COA_Id;
                                    taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                    taxTransaction.credit = 0;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.Bill_Id = bill.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = bill.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                    taxTransaction.creationDate = bill.GLPostingDate;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                        }
                    }
                }
                if (lookUpWHT.SelectedIndex != -1)
                {
                    var billTax = taxRepo.getTaxtById((lookUpWHT.SelectedItem as TaxName).Id);
                    if (billTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbTrans.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                      billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtWhtAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtWhtAmount.Text);
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = billTax.COA_Id;
                                    taxTransaction.credit = Convert.ToDouble(txtWhtAmount.Text);
                                    taxTransaction.debit = 0;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.Bill_Id = bill.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = bill.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                    taxTransaction.creationDate = bill.GLPostingDate;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                            else
                            {
                                double total = 0;
                                if (
                                      billTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                      billTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       billTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtWhtAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtWhtAmount.Text);
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = billTax.COA_Id;
                                    taxTransaction.credit = Convert.ToDouble(txtWhtAmount.Text);
                                    taxTransaction.debit = 0;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.Bill_Id = bill.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = bill.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.Bill;
                                    taxTransaction.creationDate = bill.GLPostingDate;
                                    taxTransaction.total = 0 - Convert.ToDouble(txtWhtAmount.Text);
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
                                    taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                    taxTransaction.reconcilationDate = null;
                                    taxTransaction.ReconcilationId = null;
                                    taxTransaction.isReconciled = false;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                        }
                    }
                    journalTransactions.Where(w => w.credit != 0 && w.credit != Convert.ToDouble(txtWhtAmount.Text)).ToList().ForEach(w => w.credit = Convert.ToDouble(txtBillAfterTax.Text));
                    //journalTransactions.Where(w => w.debit != 0 && w.credit != Convert.ToDouble(txtWhtAmount.Text)).ToList().ForEach(w => w.debit = Convert.ToDouble(txtBillAfterTax.Text));
                }
            }
            return journalTransactions;
        }
        private void TxtBillWithTax_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (cmbBillType.SelectedItem != null)
            {
                if ((cmbBillType.SelectedItem as cmbitem).name == "Linked-Bills-Direct COA" && cmbBillType.SelectedItem != null ||
                       (cmbBillType.SelectedItem as cmbitem).name == "New-Bills-Direct COA" && cmbBillType.SelectedItem != null)
                {
                    CalculateGlTotal();
                }
                else
                {
                    txtSoAmount.Text = txtBillWithTax.Text;
                    txtBillAmountOC.Text = txtBillWithTax.Text;
                }
            }

        }

        private void LookupPayeeVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
          var  billVendor = lookupBillVendor.SelectedItem as Vendor;
            if (billVendor != null)
            {
                if (datpoCreationdate.EditValue != null && (DateTime)datpoCreationdate.EditValue > new DateTime(2025, 2, 17) && billVendor.isBlackList == true)
                {
                    lookupBillVendor.SelectedItem = null;
                    return; // or whatever action you need to take
                }
                else
                if (billVendor.isBlackList == true)
                {

                    lookupBillVendor.Background = Brushes.DarkRed;
                    lookupBillVendor.Foreground = Brushes.White;
                }
                else
                {
                    lookupBillVendor.Background = Brushes.Transparent;
                    lookupBillVendor.Foreground = Brushes.Black;
                }
                var vendorCountry = billVendor.billingAddres.Country;
                txtBillVendorName.Text = billVendor.company.CompanyName;
            }

        }

        private void LookupPOVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
          var  billVendor = lookupPOVendor.SelectedItem as Vendor;
            if (billVendor != null)
            {
                if (datpoCreationdate.EditValue != null && (DateTime)datpoCreationdate.EditValue > new DateTime(2025, 2, 17) && billVendor.isBlackList == true)
                {
                    lookupPOVendor.SelectedItem = null;
                    return; // or whatever action you need to take
                }
                else
                if (billVendor.isBlackList == true)
                {
                    lookupPOVendor.Background = Brushes.DarkRed;
                    lookupPOVendor.Foreground = Brushes.White;
                }
                else
                {
                    lookupPOVendor.Background = Brushes.Transparent;
                    lookupPOVendor.Foreground = Brushes.Black;
                }
                var vendorCountry = billVendor.billingAddres.Country;
                txtPOVendorName.Text = billVendor.company.CompanyName;
            }
        }

        private void BtnSaveManagmentSummaryMemo_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(frmAddMemo.txtMemo.Text))
            {
                txtSummaryMemo.Text = frmAddMemo.txtMemo.Text;
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


            frmAddMemo.txtMemo.Text = txtSummaryMemo.Text;
            frmAddMemo.btnSave.Click += BtnSaveManagmentSummaryMemo_Click;

            memoWindow.Content = frmAddMemo;
            memoWindow.ShowDialog();
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
            if (managementSummary.ParentId == null)
            {
                DXMessageBox.Show("Parent Summary cannot be selected!");
                lookUpManagementSummary.Focus();
                lookUpManagementSummary.SelectedIndex = -1;
                return;
            }
        }

        private void loadManagementSummaries()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            lookUpManagementSummary.ItemsSource = BillsRepo.GetAllManagementSummary();
        }
        private void ViewBillItem_InitNewRow(object sender, InitNewRowEventArgs e)
        {

        }

        private void ViewBillItem_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            CalculateGlTotal();
        }
        public void CalculateGlTotal()
        {
            double sumTotal = 0;
            double sumAmountSOC = 0;
            if (grdPOItems.ItemsSource != null)
                foreach (var item in grdBillItems.ItemsSource as List<BillItem>)
                {
                    if (item.AmountSOC != 0)
                    sumAmountSOC += item.AmountSOC;
                    sumTotal += item.BillAmount;

                }
            if (sumTotal != 0)
            {
                txtSoAmountSOC.Text = sumAmountSOC.ToString();
                txtcfr.Text = sumTotal.ToString();
                txtSoAmount.Text = sumTotal.ToString();
                txtBillAmountOC.Text = sumTotal.ToString();

                //    if (!string.IsNullOrEmpty(txtTaxAmount.Text))
                //    {
                //        var whtAmount = Convert.ToDouble(txtTaxAmount.Text);
                //        var totalAmountWt= sumTotal + whtAmount;
                //        txtBillWithTax.Text = totalAmountWt.ToString();
                //    }
                //    else
                //    {
                //        txtBillWithTax.Text = sumTotal.ToString();
                //    }
                //    if (!string.IsNullOrEmpty(txtBillAfterTax.Text))
                //    {
                //        var whtAmount = Convert.ToDouble(txtBillAfterTax.Text);
                //        var totalAmountWHt = sumTotal + whtAmount;
                //        txtBillWithTax.Text = totalAmountWHt.ToString();
                //    }
                //    else
                //    {
                //        txtBillWithTax.Text = sumTotal.ToString();
                //    }
                //    var totalAmouutMER = Convert.ToDouble(txtexchangerate.Text) * sumTotal;
                //    txtBasetotalcfr.Text= totalAmouutMER.ToString();

                //}
                //else
                //    txtSoAmountSOC.Text = sumAmountSOC.ToString();


                //if (lookUpTax.SelectedIndex != -1)
                //{
                //    var item = lookUpTax.SelectedItem as TaxName;
                //    var percent = item.percentage;
                //    var amount = Convert.ToDouble(txtcfr.Text);

                //    var percentAmount = (percent / 100) * amount;
                //    txtTaxAmount.Text = percentAmount.ToString();
                //    amount = amount + percentAmount;
                //    txtBillWithTax.Text = amount.ToString();
                //    if ((lookUpTax.SelectedItem as TaxName).isManual != false)
                //    {
                //        txtTaxAmount.IsReadOnly = false;
                //        txtTaxAmount.Text = bill.taxAmount.ToString();
                //    }
                //    else
                //    {
                //        txtTaxAmount.IsReadOnly = true;
                //    }
                //    if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
                //    {
                //        var amount1 = Convert.ToDouble(txtBillWithTax.Text);
                //        var item1 = lookUpWHT.SelectedItem as TaxName;
                //        var percent1 = item1.percentage;
                //        var percentAmount1 = percent1 / 100 * amount1;
                //        txtWhtAmount.Text = percentAmount1.ToString();
                //        amount1 = amount1 - percentAmount1;
                //        txtBillAfterTax.Text = amount1.ToString();
                //    }
                //}
                //else
                //{
                //    txtBillWithTax.Text= sumTotal.ToString();
                //}


                //if (lookUpWHT.SelectedIndex != -1)
                //{
                //    var item = lookUpWHT.SelectedItem as TaxName;
                //    var percent = item.percentage;
                //    var amount = Convert.ToDouble(txtBillWithTax.Text);

                //    var percentAmount = (percent / 100) * amount;
                //    txtWhtAmount.Text = percentAmount.ToString();
                //    amount = amount - percentAmount;
                //    txtBillAfterTax.Text = amount.ToString();
                //}
                //else
                //{
                //    txtBillAfterTax.Text = sumTotal.ToString();
                //}
            }
        }

        private void loadBillReferenceNo()
        {
            //BillsRepo = new AdminBillsRepo();
            var references = billRepo.GetAllBillReferenceNo().Where(x => x.companyId == company.Id && x.isActive == true).ToList();

            if (editbill == 1 && bill != null)
            {
                if (bill.BillRefNo != null && references.FirstOrDefault(x => x.Id == bill.BillRefNoId) == null)
                    references.Add(bill.BillRefNo);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();

            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (VendorBillReference _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.Reference, id = _ref.Id });
            }

            cmbxBillRef.ItemsSource = cmbitems;
        }

        private void CmbxBillRef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveVBAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }

        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                //treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(bill.Id, TransactionItemType.Bill);
                List<TreeItem> atachments = SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill);

                if (bill.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    if (bill.SaleOrder != null)
                    {
                        saleOrder = bill.SaleOrder;
                        if (saleOrder.offer != null)
                        {
                            if (saleOrder.offer_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                            if (saleOrder.offer.inquiry_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                        }

                        if (saleOrder.SaleInvoices.Count != 0)
                        {
                            foreach (var invoice in saleOrder.SaleInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                                if (invoice.salesReceipts.Count != 0)
                                    foreach (var receipt in invoice.salesReceipts)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                    }
                            }
                        }
                        if (saleOrder.PurchaseOrders.Count != 0)
                        {
                            foreach (var pO in saleOrder.PurchaseOrders)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                                if (pO.PurchaseInvoices.Count != 0)
                                    foreach (var pI in pO.PurchaseInvoices)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                        if (pI.Payments.Count != 0)
                                            foreach (var payment in pI.Payments)
                                            {
                                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                            }
                                    }
                            }
                        }
                        if (bill.Payments.Count != 0)
                            foreach (var payment in bill.Payments)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                            }
                        //if (saleOrder.Bills.Count != 0)
                        //{
                        //    foreach (var bill in saleOrder.Bills)
                        //    {
                        //        otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                        //        if (bill.Payments.Count != 0)
                        //            foreach (var payment in bill.Payments)
                        //            {
                        //                otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                        //            }
                        //    }
                        //}

                        //foreach (var cat in atachments)
                        //{
                        //    foreach (var otherCat in otherAttachments)
                        //    {
                        //        if (otherCat.name == cat.name)
                        //        {
                        //            foreach (var file in otherCat.Items)
                        //            {
                        //                cat.Items.Add(file);
                        //            }
                        //        }
                        //    }
                        //}
                        //treeViewAttachments1.ItemsSource = atachments;
                    }
                    else
                    if (bill.PurchaseOrder != null)
                    {
                        otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)bill.PurchaseOrder.Id, TransactionItemType.Purchase_Order));
                        if (bill.PurchaseOrder.PurchaseInvoices.Count != 0)
                            foreach (var pI in bill.PurchaseOrder.PurchaseInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                if (pI.Payments.Count != 0)
                                    foreach (var payment in pI.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                    }
                            }

                        otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                        if (bill.Payments.Count != 0)
                            foreach (var payment in bill.Payments)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetVBAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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




                    //foreach (var bill in saleOrder.Bills)
                    //{
                    //    otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                    //    if (bill.Payments.Count != 0)
                    //        foreach (var payment in bill.Payments)
                    //        {
                    //            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                    //        }
                    //}
                }
                grdAttachments1.Visibility = Visibility.Visible;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (OrderId != 0)
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
                        destination += "Attachments\\Bill\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Bill.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Bill);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Bill, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, bill.Id, (int)TransactionItemType.Bill, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Bill);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
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

                            //DXMessageBox.Show("Attachment Uploaded");


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

        private void TxtTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amount = Convert.ToDouble(txtcfr.Text);
            var taxAmount = Convert.ToDouble(txtTaxAmount.Text);
            var taxWHTAmount = Convert.ToDouble(txtWhtAmount.Text);
            txtBillWithTax.Text = Math.Round((amount + taxAmount), 2).ToString();
            txtBillAfterTax.Text = Math.Round((amount + taxAmount - taxWHTAmount), 2).ToString();
        }

        private void TxtWhtAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amount = Convert.ToDouble(txtcfr.Text);
            var taxAmount = Convert.ToDouble(txtTaxAmount.Text);
            var taxWHTAmount = Convert.ToDouble(txtWhtAmount.Text);
            txtBillWithTax.Text = Math.Round((amount + taxAmount), 2).ToString();
            txtBillAfterTax.Text = Math.Round((amount + taxAmount - taxWHTAmount), 2).ToString();
        }

        private void btnCreateIBT_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers(TransactionItemType.Bill, billid);
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                ucFrmBankTransfer.frmBankTranfer.Show();

            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfe" +
                    "r!");
                return;

            }
        }

        private void chkUnAdjustedTax_Checked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = true;
            lookUpTax.ItemsSource = taxRepo.getAllUnAdjustedTaxes();
            chkAdjustedTax.IsChecked = false;
        }

        private void chkUnAdjustedTax_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = false;
        }

        private void btnCostSheetPunching_MouseEnter(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    ProcurementRepo repo = new ProcurementRepo();
            //    string systemCost = "";
            //    List<CostSheetBillField> costSheetBillValues = repo.GetSystemBillCosts(billid);
            //    foreach (CostSheetBillField field in costSheetBillValues)
            //    {
            //        if (field.Value != 0)
            //        {
            //            systemCost += field.Field.Title + " " + " " + field.Value + "\n";


            //        }


            //    }
            //    btnCostSheetPunching.ToolTip = systemCost;
            //}
            //catch (Exception)
            //{
            //}

        }

        private void view_CellValueChanged_1(object sender, CellValueChangedEventArgs e)
        {

        }

        private void btnBudgetCostSheet_Click(object sender, RoutedEventArgs e)
        {
            if (bill.saleOrder_Id != null)
            {
                if (bill.SaleOrder.Budget_Id != null)
                {
                    frmBudgetAdd budget = new frmBudgetAdd((int)bill.SaleOrder.Budget_Id);
                    budget.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("Please attach budget with sale order first", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            if (bill.purchaseOrder_Id != null)
            {
                if (bill.SaleOrder.Budget_Id != null)
                {
                    frmBudgetAdd budget = new frmBudgetAdd((int)bill.PurchaseOrder.Budget_Id);
                    budget.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("Please attach budget with purchase order first", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

            }


        }

        private void btnBudgetCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {
            if (bill.saleOrder_Id != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                {
                    if (bill.BudgetSystemCostFields.Count != 0)
                    {
                        winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, true, Convert.ToDouble(txtcfr.Text), saleOrder);
                        winAddBudgetSystemCost.costSheetFields = bill.BudgetSystemCostFields;
                        systemCost.ShowDialog();
                        bill.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;
                    }
                    else
                    {
                        if (saleOrder.Budget_Id != null)
                        {
                            winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, false, Convert.ToDouble(txtcfr.Text), saleOrder);
                            systemCost.ShowDialog();
                            bill.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;

                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("Punch Budget System Cost", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
           if (bill.purchaseOrder_Id != null)
            {
                if (bill.PurchaseOrder.Budget_Id != null)
                {

                }
                else
                {
                    DXMessageBox.Show("Please attach budget with purchase order first");

                }

            }
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bill.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (saleOrder.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = bill.holderChangeDate;
                    }
                }
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
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

                if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Bill);
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

                if (frmInputBox.commentAdded == true && bill.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (bill.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Vendor Bill first to add a comment!");
                }
            }
            loadcomments();
        }
        public void GellAllOrdersTracking()
        {
            trackingOrder = billRepo.get(bill.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Bill);
            }

        }
        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
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
                        Window win = new Window();
                        AdvanceRepo advanceRepo = new AdvanceRepo();


                        var loansAdvance = advanceRepo.GetLoansAdvance(item.Id);


                        switch (loansAdvance.advanceTemplate)
                        {
                            case LoansAdvanceTemplate.Advance:
                                switch (loansAdvance.loansAdvanceType)
                                {
                                    case LoansAdvanceType.Admin_Bill:
                                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                        frmLoansAdvances.loansAdvanceId = item.Id;
                                        frmLoansAdvances.editFlag = true;

                                        win.Content = frmLoansAdvances;
                                        win.WindowState = WindowState.Maximized;
                                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                        win.Show();
                                        break;
                                    case LoansAdvanceType.Vendor_Bill:
                                        ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                        frmBillLoansAdvance.loansAdvanceId = item.Id;
                                        frmBillLoansAdvance.editFlag = true;

                                        win.Content = frmBillLoansAdvance;
                                        win.WindowState = WindowState.Maximized;
                                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                        win.Show();
                                        break;
                                }
                                break;
                            case LoansAdvanceTemplate.Loan:
                                switch (loansAdvance.loansAdvanceType)
                                {
                                    case LoansAdvanceType.Admin_Bill:
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                        {
                                            ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                            frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                            frmCompanyLoan.editFlag = true;
                                            Window wind = new Window();
                                            wind.Content = frmCompanyLoan;
                                            wind.WindowState = WindowState.Maximized;
                                            wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            wind.Show();
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission denied!");
                                        }
                                        break;
                                    case LoansAdvanceType.Vendor_Bill:
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                        {
                                            ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                            frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                            frmCompanyLoan.editFlag = true;
                                            Window wind = new Window();
                                            wind.Content = frmCompanyLoan;
                                            wind.WindowState = WindowState.Maximized;
                                            wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            wind.Show();
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission denied!");
                                        }
                                        break;
                                }
                                break;
                        }


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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
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
        public List<ERP_BL.VATBook.VATBook> GetVATBooks()
        {
            List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
            ERP_BL.VATBook.VATBook book = new ERP_BL.VATBook.VATBook();
            book.CreationDate = datpoCreationdate.DateTime;
            book.GLPostingDate = datpoCreationdate.DateTime;
            book.vendorBillId = bill.Id;
            book.TransactionType = TransactionItemType.Bill;
            book.debit = Convert.ToDouble(txtTaxAmount.Text);
            book.credit = 0;
            book.total = Convert.ToDouble(txtTaxAmount.Text)-0;
            book.FinanceRefNo = txtFinanaceRef.Text;
            book.SystemRefNo = txtSystemRef.Text;
            book.deptId = bill.dept_Id;
            book.companyId = bill.company_Id;
            book.currencyId = (cmbPOCurrency.SelectedItem as cmbitem).id;
            book.VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null;
            vatBooks.Add(book);
            return vatBooks;
        }
        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void lookupLoanAdvanceCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupLoanAdvanceCompany.SelectedItem as Company;

                if (company != null)
                {
                    var depts = company.departments;

                    List<Department> departments = new List<Department>();

                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsLoansAdvancesType == true).ToList())
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }

                    lookupLoanAdvanceDepartment.ItemsSource = departments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Invalid Company");
            }
        }

        private void lookupLoanAdvanceDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            AdvanceRepo advanceRepo = new AdvanceRepo();
            var company = lookupLoanAdvanceCompany.SelectedItem as Company;
            var department = lookupLoanAdvanceDepartment.SelectedItem as Department;


            lookupLoanAdvance.ItemsSource = advanceRepo.GetAllLoansAdvancesByCompDept(company.Id, department.Id);
        }

        private void lookupLoanAdvanceDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            var company = lookupLoanAdvanceCompany.SelectedItem as Company;

            if (company == null)
            {
                DXMessageBox.Show("Please Select Company first!");
                lookupLoanAdvanceCompany.Focus();
                return;
            }
           
        }

        private void lookupLoanAdvance_GotFocus(object sender, RoutedEventArgs e)
        {
            var company = lookupLoanAdvanceCompany.SelectedItem as Company;
            var department = lookupLoanAdvanceDepartment.SelectedItem as Department;

            if (company == null)
            {
                DXMessageBox.Show("Please Select Company first!");
                lookupLoanAdvanceCompany.Focus();
                return;
            }

            if (department == null)
            {
                DXMessageBox.Show("Please Select Department first!");
                lookupLoanAdvanceDepartment.Focus();
                return;
            }
        }

        private void grdAdjustments_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }

        private void tblView_InitNewRow(object sender, InitNewRowEventArgs e)
        {

        }

        private void View_CellValueChanged_2(object sender, CellValueChangedEventArgs e)
        {
            var row = e.Row as VendorBillAdjustment;

            var adj = adjustments.FirstOrDefault(x => x.Id == row.Id);

            if(adj != null)
            {
                if (adj.isApproved == true)
                    adj.ApprovedDate = DateTime.Now;
                else
                    adj.ApprovedDate = null;
            }
            
        }

        private void btnApprovedAdjustments_Click(object sender, RoutedEventArgs e)
        {
            //grdAdjustments.ItemsSource = bill.Adjustments.Where(x => x.isApproved == true).ToList();
        }

        private void btnPendingAdjustments_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
