using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Tax;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.SaleOrderss;
using ERP_BL.Procurements.Inventories;

using static ZAS_ERP.Inquiriess.frmInquiryadd;
using DevExpress.Xpf.Grid;
using System.Diagnostics;
using ZAS_ERP.Procurementss.Budget;
using System.Printing;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;

namespace ZAS_ERP.Procurementss.PurchaseInvoice.UserControls
{
    /// <summary>
    /// Interaction logic for ucPIAdd.xaml
    /// </summary>
    public partial class ucPIAdd : UserControl
    {
        public static int purchaseInvoiceId;
        public static int purchaseOrderId;
        public bool isloading = false;
        public int InvoiceId;
        public int editInvoice;
        TransactionItemType transactionType;
        public static int editpurchaseInvoice;
        bool addinfo = true;

        UsersRepo usersRepo = new UsersRepo();
        Currency currency = new Currency();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
        PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        VendorRepo vendorRepo = new VendorRepo();
        ProductRepo productrepo = new ProductRepo();
        UsersRepo UsersRepo = new UsersRepo();



        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<Product> products { get; set; }
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        List<ViewInfo> views = new List<ViewInfo>();

        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        ERP_BL.Databases.PurchaseInvoice purchaseInvoice = new ERP_BL.Databases.PurchaseInvoice();
        string symbol;
        private static InquiryType PoType;
        CustomerCompany customer = new CustomerCompany();
        Vendor vendor = new Vendor();
        public PurchaseInvoiceStatus checkStatus = new PurchaseInvoiceStatus();
        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        Department InterDepartment = new Department();
        public double POCFRRemaining = 0;
        PurchaseInvoiceStatus oldStatus = new PurchaseInvoiceStatus();
        TaxName tax = new TaxName();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        List<ERP_BL.Procurements.Inventories.Inventory> inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();
        TaxRepo taxRepo = new TaxRepo();
        TaxName purchaseOrderTax = new TaxName();
        private object bill;
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        OfferRepo offerRepo = new OfferRepo();
        ProductRepo productRepo = new ProductRepo();
        ERP_BL.Databases.PurchaseInvoice  trackingOrder = new ERP_BL.Databases.PurchaseInvoice();
        List<PurchaseInvoiceStatus> PurchaseInvoiceStatuses = new List<PurchaseInvoiceStatus>();


        public ucPIAdd()
        {
            InitializeComponent();
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
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
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InvoiceId = purchaseInvoiceId;
                editInvoice = editpurchaseInvoice;
                grdPIItems.ItemsSource = procurementProducts;
                //products = SYSTEM_STATIC.GetItemsForCurrentUser();
                //lookupProductsinGrid.ItemsSource = products;
                //PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
                //purchaseOrder = purchaseOrderRepo.get(purchaseOrderId);
                //grdPIItems.ItemsSource = purchaseOrder.products.ToList();
                loadBookerItemsSources();
                Loadcompanies();
                cmbPurchaseInvoiceType.ItemsSource = SYSTEM_STATIC.loadPurchaseOrdertypes();
                loadCurrencies();
                loadPurchaseInvoiceStatus();
                loadVendorPaymentStatus();
                loadIncoterms();
                loadPaymentTerms();
                TaxRepo taxRepo = new TaxRepo();
                var taxes = taxRepo.getAllTaxes();
                lookUpTax.ItemsSource = taxes;
                //lookUpWHT.ItemsSource = taxes;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of PurchaseInvoice") != null)
                {
                    datpiCreationdate.IsEnabled = true;
                }
                else
                {
                    datpiCreationdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of PurchaseInvoice") != null)
                {
                    datglPostingdate.IsEnabled = true;
                }
                else
                {
                    datglPostingdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with PurchaseInvoice") != null)
                {
                    //btnAttachNew.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachNew.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with PurchaseInvoice") != null)
                {
                    btnAttachmentList.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachmentList.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void PurchaseInvoice") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                if (editInvoice == 1 && purchaseInvoiceId != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Invoice") == null)
                    {
                        isloading = true;
                        purchaseInvoice = purchaseInvoiceRepo.get(purchaseInvoiceId);
                        views = usersRepo.getViwerInfo(purchaseInvoice.Id, 7);
                        grdUsers.ItemsSource = views;
                        loadonPurchaseInvoiceData();
                        GellAllOrdersTracking();

                        loadcomments();
                        btnSave.IsEnabled = false;
                        if (purchaseInvoice.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Invoice") != null)
                        {
                            btnSave.IsEnabled = true;
                        }


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Invoice") != null)
                    {
                        isloading = true;
                        purchaseInvoice = purchaseInvoiceRepo.get(purchaseInvoiceId);
                        loadonPurchaseInvoiceData();
                        GellAllOrdersTracking();
                        views = usersRepo.getViwerInfo(purchaseInvoice.Id, 7);
                        grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = true;

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Invoice") != null)
                    {
                        isloading = true;
                        purchaseInvoice = purchaseInvoiceRepo.get(purchaseInvoiceId);

                        loadonPurchaseInvoiceData();
                        GellAllOrdersTracking();
                        views = usersRepo.getViwerInfo(purchaseInvoice.Id, 7);
                        grdUsers.ItemsSource = views;
                        loadcomments();

                        btnSave.IsEnabled = true;

                    }
                    //Setting void stamp
                    if (purchaseInvoice != null)
                    {
                        if (purchaseInvoice.isVoid == true)
                        {
                            grdVoid.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Purchase Invoice!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }

                }
                else if (editInvoice == 0 && purchaseOrderId != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice") != null)
                    {
                        isloading = true;
                        purchaseOrder = purchaseOrderRepo.get(purchaseOrderId);
                        loadOnPurchaseOrderData();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Purchase Invoice!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }


                }
                else
                {
                    //cmbcaption1.SelectedIndex = 0;
                    //cmbcaption2.SelectedIndex = 1;
                }
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                {
                    CalculateBookerTotal();
                }
                else
                {
                    calculatetotal();
                }
                //if (purchaseInvoice.totaltaxAmount != 0)
                //{
                    
                //    if (purchaseOrder.isAdjustedTax == true)
                //    {
                //        chkAdjustedTax.IsChecked = true;
                //    }
                //    else
                //    {
                //        chkTax.IsChecked = true;
                //    }
                //    txtTaxAmount.Text = purchaseInvoice.totaltaxAmount.ToString();
                //}
                //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPIItems);
                isloading = false;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Market Exchange Rate in Purchase Invoice") != null)
                {
                    lblexchangerate.Visibility = Visibility.Visible;
                    txtexchangerate.Visibility = Visibility.Visible;
                }
                else
                {
                    lblexchangerate.Visibility = Visibility.Collapsed;
                    txtexchangerate.Visibility = Visibility.Collapsed;
                }
                if(company.currency!=null)
                txtBaseCurrency.Text = company.currency.CurrencyName;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushCredits.IsEnabled = true;
                }
                else
                {
                    btnPushCredits.IsEnabled = true;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushDebits.IsEnabled = true;
                }
                else
                {
                    btnPushDebits.IsEnabled = true;
                }
                //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPIItems);
                //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPIItems);
                LoadCreator();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            grdTrackingTree.ExpandAllNodes();
        }
        private void LoadCreator()
        {
            if (editInvoice == 0)
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            else if (purchaseInvoice.user != null)
                txtCreator.Text = purchaseInvoice.user.employee.person.FName + " " + purchaseInvoice.user.employee.person.LName;
        }
        public void loadBookerItemsSources()
        {
            BookerStatementItems = new List<BookerStatementItem>();
            grdBokkerItems.ItemsSource = BookerStatementItems;
            //lookupBookerFOCSampling.ItemsSource = offerRepo.GetAllActiveFOCSamplings();
            //lookupBookerClaimDiscount.ItemsSource = offerRepo.GetAllActiveClaimDiscounts();
            //lookupBookerProductsinGrid.ItemsSource = productRepo.getAllUserProducts(SYSTEM_STATIC.currentUser.id);
            lookupBookerGST.ItemsSource = taxRepo.getAllTaxes();
            //lookupPassOn.ItemsSource = offerRepo.GetAllPassOns();
        }
        public void loadonPurchaseInvoiceData()
        {
            if (purchaseInvoice.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (purchaseInvoice.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";\
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseInvoiceStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseInvoice.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseInvoice.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseInvoice.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }

            PaymentRepo paymentRepo = new PaymentRepo();
            var creditJournalTransactions = purchaseInvoice.journalTransactions.Where(x => x.credit != 0).ToList();
            var debitJournalTransactions = purchaseInvoice.journalTransactions.Where(x => x.debit != 0).ToList();
            if (creditJournalTransactions.Count > 0)
            {
                btnPushCredits.IsChecked = true;
            }
            if (debitJournalTransactions.Count > 0)
            {
                btnPushDebits.IsChecked = true;
            }
            if (purchaseInvoice.GLPostingDate != null)
            {
                datglPostingdate.EditValue = purchaseInvoice.GLPostingDate;
            }
            else
            {
                datglPostingdate.EditValue = purchaseInvoice.CreationDate;
            }
            var payments = paymentRepo.GetAllPaymentsByPIId(purchaseInvoiceId);
            var paidAmount = Math.Round( payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount),2)/* + payments.Where(x => x.isVoid != true).Sum(y => y.Deductions)*/;
            double invoiceAmount;
            var purchaseInvoices = purchaseInvoiceRepo.getbyPOID(purchaseInvoice.purchaseOrder_Id.Value);
            double PISum = 0, remainingPoAmount = 0, POamount = 0, totalInvoicedAmount = 0;


            if (purchaseInvoice.tax != null && purchaseInvoice.tax.percentage > 0)
            {
                
                POamount =  purchaseInvoice.PurchaseOrder.totalCFRValue;
                POamount = Math.Round(POamount + ((purchaseInvoice.tax.percentage * POamount) / 100), 2);
                invoiceAmount = Math.Round(purchaseInvoice.totalInvoiceAmount + ((purchaseInvoice.tax.percentage * purchaseInvoice.totalInvoiceAmount) / 100), 2);
                totalInvoicedAmount = purchaseInvoices.Sum(x => x.totalInvoiceAmount);
                totalInvoicedAmount =  Math.Round(totalInvoicedAmount + ((purchaseInvoice.tax.percentage * totalInvoicedAmount) / 100), 2);
                //PISum = Math.Round(purchaseInvoices.Sum(x => x.totalInvoiceAmount), 2);
                //PISum = Math.Round(PISum + ((purchaseInvoice.tax.percentage * PISum) / 100), 2);
                remainingPoAmount = Math.Round( Math.Round(POamount, 2) - totalInvoicedAmount , 2);

            }
            else
            {
                invoiceAmount = purchaseInvoice.totalInvoiceAmount +purchaseInvoice.totaltaxAmount;
                PISum = purchaseInvoices.Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                remainingPoAmount = purchaseInvoice.PurchaseOrder.totalCFRValue + purchaseInvoice.PurchaseOrder.totaltaxAmount - PISum;
                if (purchaseInvoice.isAdjustedTax == true)
                {
                    chkAdjustedTax.IsChecked = true;
                    lookUpTax.Text = purchaseInvoice.tax?.Name;
                }
                if (purchaseInvoice.totaltaxAmount != 0)
                {
                    if (purchaseInvoice.isAdjustedTax == true)
                    {
                    }
                    else
                    {
                        chkTax.IsChecked = true;
                    }
                }
           


                txtTaxAmount.Text = purchaseInvoice.totaltaxAmount.ToString();
            }
                
            var percentPaid = Math.Round((paidAmount / Convert.ToDouble(invoiceAmount)) * 100, 2);
            pbarTarget.Value = percentPaid;

            if (percentPaid == 100)
            {
                grdFullyPaid.Visibility = Visibility.Visible;
                txtFullyPaid.RenderTransform = new RotateTransform(-35);
            }

            txtPaidAmountOC.Text = paidAmount.ToString();
            txtBalanceAmountOC.Text = (invoiceAmount - paidAmount).ToString();
            txtPOremainingcfr.Text = remainingPoAmount.ToString();


            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            if (purchaseInvoice.purchaseOrder_Id != null && purchaseInvoice.PurchaseOrder != null)
                purchaseOrder = purchaseInvoice.PurchaseOrder;


            if (purchaseInvoice.InterCompany_Id != null || purchaseInvoice.InterCompany != null)
            {
                var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                if (purchaseInvoice.InterCompany != null && companylist.Find(x => x.Id == purchaseInvoice.InterCompany_Id) == null)
                {
                    companylist.Add(purchaseInvoice.InterCompany);
                    lookupInterCompany.ItemsSource = companylist;
                }
                InterCompany = purchaseInvoice.InterCompany;
                lookupInterCompany.Text = purchaseInvoice.InterCompany?.CompanyName;
                chkInterCompany.IsChecked = true;
            }
            if (purchaseInvoice.InterDepartment_Id != null || purchaseInvoice.InterDepartment != null)
            {
                var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                if (purchaseInvoice.InterDepartment != null && deptlist.Find(x => x.Id == purchaseInvoice.InterDepartment_Id) == null)
                {
                    deptlist.Add(purchaseInvoice.InterDepartment);
                    lookupInterDepartment.ItemsSource = deptlist;
                }
                lookupInterDepartment.Text = purchaseInvoice.InterDepartment?.DeptName;
                InterDepartment = purchaseInvoice.InterDepartment;
            }
            cmbPurchaseInvoiceType.Text = purchaseInvoice.PurchaseInvoicetype.ToString();
            PoType = purchaseInvoice.PurchaseInvoicetype;
            datpiCreationdate.EditValue = purchaseInvoice.CreationDate;
            // Select Company
            if (purchaseInvoice.company_Id != null || purchaseInvoice.company != null)
            {
                company = purchaseInvoice.company;
                lookupCompany.Text = purchaseInvoice.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";
            }
            // Select Department
            if (purchaseInvoice.dept_Id != 0 || purchaseInvoice.department != null)
            {
                lookupDepartment.Text = purchaseInvoice.department.DeptName;
                department = purchaseInvoice.department;
                //lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (purchaseInvoice.customerCompany.Id != 0 || purchaseInvoice.customerCompany != null)
            {
                lookupCustomer.Text = purchaseInvoice.customerCompany.company.CompanyName;
                //lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(purchaseInvoice.customerCompany);

                customer = purchaseInvoice.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            // Select Employee 
            if (purchaseInvoice.allocation_Id != 0 || purchaseInvoice.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == purchaseInvoice.allocation_Id))];

                }
                catch (Exception ex)
                {

                }
            }
            //Select vendor
            if (purchaseInvoice.vendors.Count != 0)
            {

                foreach (Vendor vendr in purchaseInvoice.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
                //lookupVendor.Text = vendor.company.CompanyName;
                //lookupVendor.SelectedItem = lookupVendor.GetItemByKeyValue(vendor.company.CompanyName);
            }
            if (!string.IsNullOrEmpty(purchaseInvoice.VendorName))
            {
                txtVendorName.Text = purchaseInvoice.VendorName;

            }




            txtPOReferenceNumber.Text = purchaseInvoice.POReferenceNo;
            txtSalesref.Text = purchaseInvoice.SalesReferenceNo;
            datPoDate.EditValue = purchaseInvoice.PODate;
            datPODeliverydate.EditValue = purchaseInvoice.PODeliveryDate;
            txtsaleOrderref.Text = purchaseInvoice.SOReferenceNo;
            datsaleOrderdate.EditValue = purchaseInvoice.SODate;
            datsoDeliverydate.EditValue = purchaseInvoice.SODeliveryDate;
            datLastStatusChangeDate.EditValue = purchaseInvoice.LastStatusChangeDate;
            if (purchaseInvoice.PurchaseInvoiceStatus != null)
            {
                var disAbleStatus = PurchaseInvoiceStatuses.FirstOrDefault(x => x.Id == purchaseInvoice.PurchaseInvoiceStatus.Id);
                if (disAbleStatus == null)
                {
                    loadPurchaseInvoiceStatus(purchaseInvoice.PurchaseInvoiceStatus);
                }
            }
            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;
            // Select SaleInvoice Status 
            if (purchaseInvoice.PurchaseInvoiceStatus != null)
                if (purchaseInvoice.PurchaseInvoiceStatus.isActive == false)
                {
                    try
                    {
                        cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseInvoice.PurchaseInvoiceStatus.Status))];
                    }
                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Invoice status! " + ex.ToString());

                    }
                }
                else
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseInvoice.PurchaseInvoiceStatus.Status))];
                }
            checkStatus = purchaseInvoice.PurchaseInvoiceStatus;
            txtFinanceRef.Text = purchaseInvoice.FinanceRefrenceNo;
            if (purchaseInvoice.currency_Id != 0 && purchaseInvoice.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbPICurrency.Items.SourceCollection;
                cmbPICurrency.SelectedItem = cmbPICurrency.Items[cmbPICurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseInvoice.currency_Id))];
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.currency_Id))];
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbPICurrency.Items.SourceCollection;
                cmbPICurrency.SelectedItem = cmbPICurrency.Items[cmbPICurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseInvoice.company.CurrencyId))];
            }
          
            txtPIReferenceNumber.Text = purchaseInvoice.PIReferenceNo;

            var statusSource = (List<cmbitem>)cmbVendorPaymentStatusChange.Items.SourceCollection;

            if (purchaseInvoice.vendorPaymentStatus != null)
                if (purchaseInvoice.vendorPaymentStatus.isActive == false)
                {
                    try
                    {
                        cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(statusSource.Find(x => x.name == purchaseInvoice.vendorPaymentStatus.Status))];
                    }
                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Invoice vendor payment status! " + ex.ToString());

                    }
                }
                else
                {
                    cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(statusSource.Find(x => x.name == purchaseInvoice.vendorPaymentStatus.Status))];
                }




            txtcfr.Text = purchaseInvoice.totalInvoiceAmount.ToString();
            //txtPIAmount1.Text = purchaseInvoice.totalInvoiceAmount.ToString();

            txtBasetotalcfr.Text = purchaseInvoice.totalBaseAmount.ToString();
            lblStage.Text = (purchaseInvoice.stage != null) ? purchaseInvoice.stage : "";

            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();
            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                if (purchaseInvoice.BookerStatementItems.Count > 0)
                {
                    grdBokkerItems.ItemsSource = purchaseInvoice.BookerStatementItems;
                }
            }
            else
            {
                if (purchaseInvoice.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdInvoiceItems.ItemsSource = saleOrder.products;
                    foreach (var procurementProduct in purchaseInvoice.products)
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


                                    isActive = procurementProduct.inquiryProduct.product.isActive,
                                    journalTransactions = procurementProduct.inquiryProduct.product.journalTransactions,
                                    cgsAccount = procurementProduct.inquiryProduct.product.cgsAccount,

                                    //accountReceivable = procurementProduct.inquiryProduct.product.accountReceivable,

                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = procurementProduct.InvoicedQuantity,
                            TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                            UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = procurementProduct.InvoicedWeight,
                            TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                            totalCommission = (purchaseOrder.Commision != null) ? (Convert.ToDouble(purchaseOrder.Commision)) : ((procurementProduct.totalCommission != 0) ? procurementProduct.totalCommission : 0),


                            value2 = procurementProduct.value2,
                            totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                            NowAmount = procurementProduct.NowAmount,
                            priority = procurementProduct.priority
                            //caption2 = cmbcaption2.Text.Trim(),

                            //UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,

                            //UnInvoicedWeight = (procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,


                            ////UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                            //InvoicedQuantity = procurementProduct.InvoicedQuantity,
                            ////UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                            //InvoicedWeight = procurementProduct.InvoicedWeight,
                            //value1 = procurementProduct.value1,
                            //value2 = procurementProduct.value2,
                            //caption1 = cmbcaption1.Text.Trim(),

                            //caption2 = cmbcaption2.Text.Trim()


                        });


                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdPIItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdPIItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdPIItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdPIItems.ItemsSource = procurementProducts;
                }
            }

            if(purchaseInvoice.tax!=null)
            {
                tax = purchaseInvoice.tax;
            }


            txtexchangerate.Text = purchaseInvoice.exchangeRate.ToString();

            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
            if (purchaseInvoice.isApproved != true)
            {
                if((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Inventory)
                grdPIItems.Columns.GetColumnByName("colNowInvoiceAmount").ReadOnly = false;
                grdPIItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Invoice") != null)
                {
                    grdPIItems.IsEnabled = true;
                }
                else
                    grdPIItems.IsEnabled = false;
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FOB and CFR value for products in PurchaseInvoice") == null)
            {
                grdPIItems.Columns.GetColumnByName("colNowInvoiceAmount").ReadOnly = true;
                grdPIItems.Columns.GetColumnByFieldName("value2").ReadOnly = true;
            }
            if (purchaseInvoice.PurchaseInvoiceStatus != null)
                if (purchaseInvoice.PurchaseInvoiceStatus.isActive == false && MainWindow.currentUserid != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed PurchaseInvoice") == null)
                {
                    grdPIItems.IsEnabled = false;
                    btnAttachment.IsEnabled = false;

                    labeltopStatus.Visibility = Visibility.Visible;
                    labeltopStatus.Text = purchaseInvoice.PurchaseInvoiceStatus.Status;
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(purchaseInvoice.PurchaseInvoiceStatus.backcolor);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    labeltopStatus.Foreground = new SolidColorBrush(newColor);
                    var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                    var rt = (RotateTransform)labeltopStatus.RenderTransform;
                    rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                }

            if (purchaseInvoice.tax_Id != null)
            {
                TaxRepo repo = new TaxRepo();
                tax = repo.getTaxtById(Convert.ToInt32(purchaseInvoice.tax_Id));

                if (purchaseInvoice.isAdjustedTax == true)
                {
                    chkAdjustedTax.IsChecked = true;
                }
                else
                {
                    chkTax.IsChecked = true;
                }
                //tax = purchaseOrder.tax;
                lookUpTax.Text = tax.Name;
            }
            txtSOCER.Text = purchaseInvoice.POCER.ToString();
            txtPIAmountSOC.Text = purchaseInvoice.PIAmuontSOC.ToString();
            if (purchaseInvoice.POPaymentterm_Id != 0 && purchaseInvoice.POPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPOPaymentTerm.Items.SourceCollection;
                //cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseInvoice.POPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseInvoice.POPaymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseInvoice.POPaymentTerm.term, id = purchaseInvoice.POPaymentTerm.Id });
                    cmbPOPaymentTerm.ItemsSource = null;
                    cmbPOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseInvoice.POPaymentTerm.Id))];
            }
            if (purchaseInvoice.incoterm_Id != 0 && purchaseInvoice.POIncoTerm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == purchaseInvoice.incoterm_Id))];

            }
            //select Captions for Item Value 1
            if (purchaseInvoice.TitleValue1Id != 0 && purchaseInvoice.TitleValue1 != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == purchaseInvoice.TitleValue1Id))];

            }


            //select Captions for Item Value 2
            if (purchaseInvoice.TitleValue2Id != 0 && purchaseInvoice.TitleValue2 != null)
                {
                    var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                    cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == purchaseInvoice.TitleValue2Id))];


                }
            if (purchaseInvoice.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == purchaseInvoice.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPIItems);
            if (purchaseInvoice.holderChangeDate != null)
            {
                var time = DateTime.Now - purchaseInvoice.holderChangeDate;
                txtHolderDays.Text = time.Days.ToString();
            }
            loadVATBookReferenceNo();
            if (purchaseInvoice.VATBookRefId != 0 && purchaseInvoice.VATBookRefNumber != null)
            {
                var vatSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatSource.Find(x => x.id == purchaseInvoice.VATBookRefId))];

            }
        }



        public void loadOnPurchaseOrderData()
        {
            if (purchaseOrder.isApproved != true)
            {
                DXMessageBox.Show("This order is not approved, Invoice can't be created at this time.", "Get Approval for PurchaseOrder", MessageBoxButton.OK, MessageBoxImage.Stop);
                var myWindow = Window.GetWindow(this);

                myWindow.Close();
                return;
            }
            if (purchaseOrder.PurchaseOrderStatus.isActive != true)
            {
                DXMessageBox.Show("This order is closed, Invoice can't be created at this PO.", "Closed PurchaseOrder", MessageBoxButton.OK, MessageBoxImage.Stop);
                var myWindow = Window.GetWindow(this);

                myWindow.Close();
                return;
            }
            //if (purchaseOrder.InvoiceStage == InvoiceStage.Fully.ToString())
            //{
            //    DXMessageBox.Show("This Purchase Order has been Invoiced Fully!", "Purchase Order fully Invoiced", MessageBoxButton.OK, MessageBoxImage.Information);
            //    var myWindow = Window.GetWindow(this);
            //    myWindow.Close();

            //    return;

            //}
            if (purchaseOrder.POPaymentterm_Id != 0 && purchaseOrder.POPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPOPaymentTerm.Items.SourceCollection;
                cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseOrder.POPaymentTerm.term, id = purchaseOrder.POPaymentTerm.Id });
                    cmbPOPaymentTerm.ItemsSource = null;
                    cmbPOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentTerm.Id))];
            }
            if (purchaseOrder.incoterm_Id != 0 && purchaseOrder.incoterm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.incoterm_Id))];
            }
            cmbPurchaseInvoiceType.Text = purchaseOrder.PurchaseOrdertype.ToString();
            PoType = purchaseOrder.PurchaseOrdertype;
            txtPOReferenceNumber.Text = purchaseOrder.POReferenceNo;
            datPODeliverydate.EditValue = purchaseOrder.DeliveryDate;

            /////
            datPoDate.EditValue = purchaseOrder.PurchaseOrderDate;

            POCFRRemaining = purchaseOrder.RemainingCFRValue;

            if (purchaseOrder.RemainingCFRValue == 0)

                POCFRRemaining = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;
            else
                POCFRRemaining = purchaseOrder.RemainingCFRValue;

            if (purchaseOrder.SaleOrder != null)
            {
                datsoDeliverydate.EditValue = purchaseOrder.SaleOrder.deliveryDate;
                txtsaleOrderref.Text = purchaseOrder.SaleOrder.referenceNo;
                txtSalesref.Text = purchaseOrder.SaleOrder.SalesReferenceNo;
                datsaleOrderdate.EditValue = purchaseOrder.SaleOrder.saleOrderDate;
            }
            txtFinanceRef.Text = purchaseOrder.FinanceRefrenceNo;

            datpiCreationdate.EditValue = System.DateTime.Now;
            datglPostingdate.EditValue = System.DateTime.Now;
            txtSalesref.Text = purchaseOrder.SalesReferenceNo;

            //datBillOfLaddingdate.EditValue = saleOrder.billLaddingDate;
            //datLCDatedate.EditValue = saleOrder.lCDate;
            //datMaterialReciptdate.EditValue = saleOrder.materialReciptDate;
            //datPaymentDueFrom.EditValue = saleOrder.PaymentDueStartDate;

            //txtOfferRefNo.Text = saleOrder.offerReferenceNo;
            //txtsaleInvoiceref.Text = saleOrder.referenceNo;

            if (purchaseOrder.PurchaseOrdertype == InquiryType.Principal)
            {


            }
            else
            {

            }








            lblStage.Text = (purchaseOrder.stage != null) ? purchaseOrder.stage : "";

         
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
            //select currency
            if (purchaseOrder.currency_Id != 0 && purchaseOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbPICurrency.Items.SourceCollection;
                cmbPICurrency.SelectedItem = cmbPICurrency.Items[cmbPICurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.currency_Id))];
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.currency_Id))];
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbPICurrency.Items.SourceCollection;
                cmbPICurrency.SelectedItem = cmbPICurrency.Items[cmbPICurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.company.CurrencyId))];
            }
            // Select Department
            if (purchaseOrder.dept_Id != 0 || purchaseOrder.department != null)
            {
                lookupDepartment.Text = purchaseOrder.department.DeptName;

                department = purchaseOrder.department;

                //lookupCustomer.ItemsSource = department.customers;
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
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            }
            var statusSource = (List<cmbitem>)cmbVendorPaymentStatusChange.Items.SourceCollection;

            if (purchaseOrder.vendorPaymentStatus != null)
                if (purchaseOrder.vendorPaymentStatus.isActive == false)
                {
                    try
                    {
                        cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(statusSource.Find(x => x.name == purchaseOrder.vendorPaymentStatus.Status))];
                    }
                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Invoice vendor payment status! " + ex.ToString());

                    }
                }
                else
                {
                    cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(statusSource.Find(x => x.name == purchaseOrder.vendorPaymentStatus.Status))];
                }
            if(!string.IsNullOrEmpty(purchaseOrder.VendorName))
            {
                txtVendorName.Text = purchaseOrder.VendorName;

            }
            ////Select Principal
            //if (purchaseOrder.SaleOrder.principal_Id != 0 || purchaseOrder.SaleOrder.principal != null)
            //{
            //    lookupPrincipal.Text = saleOrder.principal.company.CompanyName;
            //    principal = saleOrder.principal;
            //}
            //else
            //{
            //    lookupPrincipal.Text = "Select Vendor";

            //}
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
            if (purchaseOrder.tax != null)
            {
                if (purchaseOrder.isAdjustedTax == true)
                {
                    chkAdjustedTax.IsChecked = true;
                }
                else
                {
                    chkTax.IsChecked = true;
                }
                tax = purchaseOrder.tax;
                lookUpTax.Text = purchaseOrder.tax.Name;
            }

            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();

            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {

                grdBokkerItems.ItemsSource = purchaseOrder.BookerStatementItems;
            }
            else
            {
                if (purchaseOrder.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();

                    foreach (var procurementProduct in purchaseOrder.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {

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
                                    isActive = procurementProduct.inquiryProduct.product.isActive,
                                    cgsAccount = procurementProduct.inquiryProduct.product.cgsAccount
                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,

                            //UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            //InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            InvoicedQuantity = CalculateUninvoicedQty(procurementProduct) != 0 ? CalculateUninvoicedQty(procurementProduct) :
                            (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,


                            UnInvoicedQuantity = (CalculateUninvoicedQty(procurementProduct)) != 0 ?
                            (procurementProduct.inquiryProduct.quantity - CalculateUninvoicedQty(procurementProduct)) :
                            ((procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity),


                            // UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            //InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            InvoicedWeight = CalculateUninvoicedWeight(procurementProduct) != 0 ? CalculateUninvoicedWeight(procurementProduct) : (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,

                            UnInvoicedWeight = CalculateUninvoicedWeight(procurementProduct) != 0 ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - CalculateUninvoicedWeight(procurementProduct) :
                            (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,

                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            priority = procurementProduct.priority,
                            //caption1 = cmbcaption1.Text.Trim(),
                            //caption2 = cmbcaption2.Text.Trim(),
                            //UnInvoicedSoAmount =  procurementProduct.UnInvoicedSoAmount
                            UnInvoicedSoAmount = (CalculateInvoicedSoAmount(procurementProduct) != 0) ? (procurementProduct.value2 != 0 ? procurementProduct.value2 : procurementProduct.value1) - (CalculateInvoicedSoAmount(procurementProduct)) : procurementProduct.UnInvoicedSoAmount



                        });


                    }

                    grdPIItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdPIItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdPIItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdPIItems.ItemsSource = procurementProducts;
                }
            }

            // selected currency of customer company
            //foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            //{

            //    if (cmbitem.id == saleOrder.company.CurrencyId)
            //    {
            //        cmbbaseCurrency.SelectedItem = cmbitem;
            //        break;
            //    }
            //}
            txtexchangerate.Text = purchaseOrder.ExchangeRate.ToString();

            // txtMarginexchangerate.Text = saleOrder.marginExchangeRate.ToString();
            //select Captions for Item Value 1

            //if (purchaseOrder.TitleValue1Id != 0 && purchaseOrder.TitleValue1 != null)
            //{
            //    var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
            //    cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == purchaseOrder.TitleValue1Id))];

            //}
            ////select Captions for Item Value 2
            //if (purchaseOrder.TitleValue2Id != 0 && purchaseOrder.TitleValue2 != null)
            //{
            //    var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
            //    cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == purchaseOrder.TitleValue2Id))];
            //}

            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
            btnPushDebits.IsChecked = true;
            btnPushCredits.IsChecked = true;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPIItems);
        }
        public void loadcomments()
        {
            try
            {
                if (purchaseInvoice != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        public void Loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                companyRepo = new CompanyRepo();
                this.lookupCompany.ItemsSource = companyRepo.GetCompanies();
                return;

            }
            empUser = employeeRepo.GetEmployeeForPayments(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
            lookupInterCompany.ItemsSource = empUser.Companies;
        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            cmbPICurrency.ItemsSource = cmbitems;
            cmbPOCurrency.ItemsSource = cmbitems;

        }
        public void loadPurchaseInvoiceStatus()
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed PurchaseInvoice") != null)
            {
                PurchaseInvoiceStatuses = purchaseInvoiceRepo.getAllPurchaseInvoiceStatus();

            }
            else

                PurchaseInvoiceStatuses = purchaseInvoiceRepo.getAllActivePurchaseInvoiceStatus();
                PurchaseInvoiceStatuses = PurchaseInvoiceStatuses.Where(x => x.isDisable != true).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();
            Parallel.ForEach(PurchaseInvoiceStatuses, delegate (PurchaseInvoiceStatus status)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });


            });
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbStatus.ItemsSource = cmbitems;

        }
        public void loadPurchaseInvoiceStatus(PurchaseInvoiceStatus _status)
        {

            List<cmbitem> cmbitems = new List<cmbitem>();
            PurchaseInvoiceStatuses.Add(_status);
            Parallel.ForEach(PurchaseInvoiceStatuses, delegate (PurchaseInvoiceStatus status)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });


            });
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbStatus.ItemsSource = cmbitems;

        }
        private void lookupInterCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
            {
                loadInterCompanyDepartments();
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

                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsProcurementType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == InterCompany.Id) != null)
                            departments.Add(_dept);
                    }
                    if (purchaseInvoice != null && purchaseInvoice.Id > 0 && editInvoice == 1)
                        if (purchaseInvoice.InterDepartment != null && departments.FirstOrDefault(x => x.Id == purchaseInvoice.InterDepartment_Id) == null)
                            departments.Add(purchaseInvoice.InterDepartment);

                    //foreach (Department dep in InterCompany.departments)
                    //    foreach (Department empdep in SYSTEM_STATIC.currentUser.employee.departments)
                    //        if (dep.Id == empdep.Id)
                    //        {
                    //            departments.Add(dep);
                    //        }
                    lookupInterDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        private void lookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterDepartment = lookupInterDepartment.SelectedItem as Department;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = false;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = true;
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                if (department.ParentID == null && department.subDepartments.Count != 0)
                {
                    DXMessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                //string selecteddept = department.DeptName + " (" + department.Code + ")";
                //lookupDepartment.EditValue = selecteddept;


                loadcustomers();
                ProductRepo productRepo = new ProductRepo();
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupProductsinGrid.ItemsSource = products;
                lookupBookerProductsinGrid.ItemsSource = products;
                lookupVendor.ItemsSource = department.Vendors;
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

            }
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
        public void loadcustomers()
        {
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                        List<CustomerCompany> customers = /*customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id)*/ department.customers.ToList();

                        if (editInvoice == 1)
                        {
                            if (purchaseInvoice.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == purchaseInvoice.customerCompany_Id);
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
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                MessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }

        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            customer = lookupCustomer.SelectedItem as CustomerCompany;
            if (customer != null)
            {
                string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
                var customerCountry = customer.billingAddres.Country;
                txtCustomerCountry.Text = customerCountry;
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

        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                MessageBox.Show("Select Department Frist");
                lookupDepartment.Focus();
                return;
            }
        }

        private void lookupVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            vendor = lookupVendor.SelectedItem as Vendor;
            if (vendor != null)
            {
                string selectedvend = vendor.company.CompanyName + " (" + vendor.contactPerson.FName + ")";
                var vendorCountry = vendor.billingAddres.Country;
                txtVendorCountry.Text = vendorCountry;
            }
        }
        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DXMessageBox.Show("Select Department Frist");
                //lookupDepartment.Focus();
                return;
            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPICurrency.SelectedItem != null)
            {
                MarketExchangeRate ExchangeRate = new MarketExchangeRate();
                SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                ExchangeRate = currencyRepo.getMarketexchangerate(company.Id, (cmbPICurrency.SelectedItem as cmbitem).id);
                salesExchangeRate = currencyRepo.getsalesexchangerate(company.Id, (cmbPICurrency.SelectedItem as cmbitem).id);
                if (ExchangeRate != null)
                {
                    txtexchangerate.Text = ExchangeRate.exchangerate.ToString();
                }
                else
                {
                    txtexchangerate.Text = "1";
                }

            }
            if (cmbPICurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbPICurrency.SelectedItem as cmbitem).id;
                currency = currencyRepo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbPICurrency.SelectedItem as cmbitem).name;
                    string caption1 = "";
                    string caption2 = "";
                    //if (cmbcaption1.SelectedItem != null)
                    //    caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    //if (cmbcaption2.SelectedItem != null)
                    //    caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                    symbol = name.Substring(name.IndexOf("("));
                    //txtBaseCurrency.Text = name;
                    if (grdPIItems.Columns.Count != 0)
                    {
                        //grdPIItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                        //grdPIItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
                        lblTotal.Text = "P.I Amount" + symbol;
                        //lblCFRTotal.Text = "P.O Amount" + symbol;


                        string stri1 = lblbaseTotal.Text;
                        if (-1 != stri1.IndexOf("("))
                            lblbaseTotal.Text = (stri1.Substring(0, stri1.IndexOf("("))) + symbol;
                        else
                            lblbaseTotal.Text = lblbaseTotal.Text + " " + symbol;
                        //string strig = lblSaleAMargin.Text;
                        //if (-1 != strig.IndexOf("("))
                        //    lblSaleAMargin.Text = (strig.Substring(0, strig.IndexOf("("))) + symbol;
                        //else
                        //    lblSaleAMargin.Text = lblSaleAMargin.Text + " " + symbol;
                    }
                }
            }
        }

        private void Txtcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtcfr.Text = string.IsNullOrEmpty(txtcfr.Text) ? 0.ToString() : txtcfr.Text;
            var amount = Convert.ToDouble(txtcfr.Text);
            var taxTotalAmount = Math.Round(amount + Convert.ToDouble(txtTaxAmount.Text), 2);
            txtTotalTaxAmount.Text = taxTotalAmount.ToString();
            //if (purchaseOrder.tax_Id != null)
            //{
            //    saleOrderTax = taxRepo.getTaxtById((int)saleOrder.taxNameId);
            //    lookupSalesTax.Text = saleOrderTax.Name;
            //    isSalesTax.IsChecked = true;
            //    if (saleOrderTax != null)
            //    {
            //        txttax.Text = (saleOrderTax.percentage / 100 * Convert.ToDouble(txtcfr.Text)).ToString();
            //    }
            //}
            //else
            //{
            //    lookupSalesTax.Text = null;
            //    isSalesTax.IsChecked = false;
            //    txttax.Text = 0.ToString();
            //}
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
            if (purchaseInvoice.isApproved != true)
            {
                 if (cmbEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select allocated to", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null)
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.isApproved = true;
                    purchaseInvoice.stage = TransactionStage.Approved.ToString();
                    purchaseInvoiceRepo.update(purchaseInvoice);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Purchase Invoice has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && purchaseInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseInvoice.InterDepartment != null && purchaseInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                        }
                        else if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (purchaseInvoice.currency != null)
                    {
                        symbolCurr = purchaseInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Purchase Invoice (Amount OC) having value: " + purchaseInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "PI Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Adding, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = false;
                    purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();

                    purchaseInvoiceRepo.update(purchaseInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = true;
                    purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();

                    purchaseInvoiceRepo.update(purchaseInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
            else if (purchaseInvoice.isApproved == true)
            {
                if (cmbEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select allocated to", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null)
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.isApproved = false;
                    purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                    purchaseInvoiceRepo.update(purchaseInvoice);


                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Purchase Invoice has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && purchaseInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseInvoice.InterDepartment != null && purchaseInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                        }
                        else if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (purchaseInvoice.currency != null)
                    {
                        symbolCurr = purchaseInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Purchase Invoice (Amount OC) having value: " + purchaseInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "PI UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }


                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Adding, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
            }
            else if (purchaseInvoice.PendingForClosing == true)
            {
                if (cmbEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select allocated to", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null)
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.PendingForClosing = false;
                    purchaseInvoice.stage = TransactionStage.Closed.ToString();
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = false;
                    purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();

                    purchaseInvoiceRepo.update(purchaseInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = true;
                    purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();

                    purchaseInvoiceRepo.update(purchaseInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Invoice);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Purchase_Invoice);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (purchaseInvoice != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                if (frmInputBox.comment != "" && purchaseInvoice.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);
                            }
                        }
                    }
                    if (frmInputBox.Comment.TaggedList.Count > 0)
                    {
                        var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                        var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                        cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                    }
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (purchaseInvoice.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Purchase Invoice first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
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

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(InvoiceId, TransactionItemType.Purchase_Invoice);
                trackingWindow.ShowDialog();
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            //var SRcount = saleInvoice.salesReceipts.Where(x => x.isVoid != true).Count();
            //if (SRcount > 0)
            //{
            //    DXMessageBox.Show("This Sale Invoice cannot be Voided because it has Active Sale Receipts!");
            //    return;
            //}
            if (purchaseInvoice.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void PurchaseInvoice") != null))
            {
                if (DXMessageBox.Show("This Invoice is currently in the list of Void Purchase Invoices! Do you want to remove it from Void?", "Remove Void Purchase Invoice", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    purchaseInvoice.isVoid = false;
                    purchaseInvoiceRepo.setPurchaseInvoicetoVoid(purchaseInvoice.Id, false);

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
                        if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && purchaseInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseInvoice.InterDepartment != null && purchaseInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                        }
                        else if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.update(purchaseInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (purchaseInvoice.currency != null)
                    {
                        symbolCurr = purchaseInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Purchase Invoice (Amount OC) having value: " + purchaseInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "PI UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ",null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0, user.id, "New Comment ",null);
                        }
                    }
                }

                loadcomments();
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void PurchaseInvoice") != null)
            {
                if (DXMessageBox.Show("This Invoice is not currently in the list of Void Purchase Invoices! Do you want to move it to Void Purchaseinvoices?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    purchaseInvoice.isVoid = true;
                    purchaseInvoiceRepo.setPurchaseInvoicetoVoid(purchaseInvoice.Id, true);

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
                        if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && purchaseInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseInvoice.InterDepartment != null && purchaseInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.updatePI(purchaseInvoice);
                            }
                        }
                        else if (purchaseInvoice.department != null && purchaseInvoice.department.Id != 0 && purchaseInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseInvoice.holderChangeDate = DateTime.Now;
                                }
                                purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseInvoiceRepo.update(purchaseInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (purchaseInvoice.currency != null)
                    {
                        symbolCurr = purchaseInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Purchase Invoice (Amount OC) having value: " + purchaseInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "PI Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ",null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PI #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0, user.id, "New Comment ",null);
                        }
                    }
                }

                loadcomments();
            }
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {

                foreach (cmbitem cmbitem in cmbPICurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                    {
                        cmbPICurrency.SelectedItem = cmbitem;
                        break;
                    }
                }

                loaddepartments();
            }
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
                    //departments = SYSTEM_STATIC.currentUser.employee.departments /*companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id)*/;
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsPurchaseInvoiceType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (purchaseInvoice != null && purchaseInvoice.Id > 0 && editInvoice == 1)
                        if (purchaseInvoice.department != null && departments.FirstOrDefault(x => x.Id == purchaseInvoice.dept_Id) == null)
                            departments.Add(purchaseInvoice.department);

                    lookupDepartment.ItemsSource = departments;
                    //if (lookupInterDepartment.ItemsSource == null)
                    //    lookupInterDepartment.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            Companiess.frmcompanyCenter.Editit = 1;
            Companiess.frmcompanyCenter.companyId = company.Id;
            frmcompanyadd.ShowDialog();
            loaddepartments();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customerss.frmCustomeradd customeradd = new Customerss.frmCustomeradd();
            customeradd.ShowDialog();
            loadcustomers();
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                CalculateBookerTotal();
                var row = e.Row as BookerStatementItem;
                row.siAmount = row.unit * row.siQuantity;
                row.siNetAmount = row.siAmount + row.siAmountGST - row.siPassOnValue - row.siClaimDiscountValue - row.siFocValue;
                row.amount = row.unit * row.quantity;
                row.netAmount = row.amount + row.amountGST - row.passOnValue - row.claimDiscountValue - row.focValue;

            }
            else
           if (cmbPurchaseInvoiceType.SelectedItem.ToString() == InquiryType.Inventory.ToString())
            {
                var colum = e.Column;
                var row = e.Row as ProcurementProduct;
                if(colum!=null)
                if (row != null && colum.FieldName != "NowAmount")
                    row.NowAmount = row.InvoicedQuantity * row.unitPrice;
                lookUpTax.Text = "";
                calculatetotal();
            }
            else
            {
                lookUpTax.Text = "";
                calculatetotal();
            }
            
        }
        private void calculatetotal()
        {
            double sumfob = 0;
            double sumcfr = 0;
            decimal? weight = 0;
            double Quantity = 0;
            double sumPoAmount = 0;

            if (grdPIItems.ItemsSource != null)
                foreach (var item in grdPIItems.ItemsSource as List<ProcurementProduct>)
                {
                    {
                        if (item.InvoicedWeight != 0)
                        {
                            weight += (item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0) * Convert.ToDecimal(item.inquiryProduct.quantity);

                        }
                        else
                        {
                            weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;

                        }

                         Quantity += item.inquiryProduct.quantity;

                        sumcfr += item.NowAmount;
                        //sumcfr += item.value2;
                    }
                }
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtcfr.Text = sumcfr.ToString();
            //txtPIAmount1.Text = sumcfr.ToString();




            if (purchaseOrder.Id != 0 && editpurchaseInvoice == 0)
            {
                if (purchaseOrder.tax_Id != null)
                {
                    TaxRepo taxRepo = new TaxRepo();
                    tax = taxRepo.getTaxtById((int)purchaseOrder.tax_Id);
                    if (purchaseOrder.isAdjustedTax == true)
                    {
                        chkAdjustedTax.IsChecked = true;
                    }
                    else
                    {
                        chkTax.IsChecked = true;
                    }
                    lookUpTax.Text = tax.Name;
                    sumPoAmount = Convert.ToDouble(purchaseOrder.billWithTax);
                }
                else 
                {
                    txtTaxAmount.Text = purchaseOrder.totaltaxAmount.ToString();
                    chkTax.IsChecked = true;
                    sumPoAmount = purchaseOrder.totalCFRValue + purchaseOrder.totaltaxAmount;
                    txtTotalTaxAmount.Text = (purchaseOrder.totaltaxAmount + purchaseOrder.totalCFRValue).ToString();
                }
                txtPOtotalcfr.Text = sumPoAmount.ToString();





                //if (txtcfr.Text=="0")
                //{
                //}
                //else
                //{
                //    remainingPoAmount -= Convert.ToDouble(txtcfr.Text);
                //    txtPOremainingcfr.Text = remainingPoAmount.ToString();
                //}


            }
            else if(purchaseOrder != null && editpurchaseInvoice == 1)
            {
                if (purchaseInvoice.tax_Id != null)
                {
                   TaxRepo taxRepo = new TaxRepo();
                    tax = taxRepo.getTaxtById((int)purchaseInvoice.tax_Id);
                    if (purchaseInvoice.isAdjustedTax == true)
                    {
                        chkAdjustedTax.IsChecked = true;
                    }
                    else
                    {
                        chkTax.IsChecked = true;
                    }
                    if(tax!=null)
                    lookUpTax.Text = tax.Name;
                    sumPoAmount = Convert.ToDouble(purchaseOrder.billWithTax);
                }
                else
                {
                    sumPoAmount = purchaseOrder.totalCFRValue + purchaseOrder.totaltaxAmount;
                    txtTotalTaxAmount.Text = (Convert.ToDouble(txtcfr.Text) + Convert.ToDouble( txtTaxAmount.Text)).ToString();
                }
                txtPOtotalcfr.Text = sumPoAmount.ToString();
            }


            decimal exchangeRate = 1;
            if (txtexchangerate.Text != "")
                exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
            decimal marginexchangeRate = 1;
            decimal totalcfr = Convert.ToDecimal(txtcfr.Text);

          
            if (txtexchangerate.Text != "")
            {
                marginexchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                txtBasetotalcfr.Text = (marginexchangeRate * totalcfr).ToString();
            }
            //if (!string.IsNullOrEmpty(txtPOremainingcfr.Text) && !string.IsNullOrEmpty(txtPOtotalcfr.Text))
            //    if (Convert.ToDouble(txtPOremainingcfr.Text) < 0 || Convert.ToDouble(txtPOtotalcfr.Text) < Convert.ToDouble(txtPOremainingcfr.Text))
            //    {
            //        DXMessageBox.Show("Purchase Order can not be over invoiced, Please check again", "Remaining amount cant be less than 0.00 or greater than PO amount");
            //    }
           
         


            //txtcfr.Text = sumcfr.ToString();
            //txtPoAmount.Text = sumcfr.ToString();
            //POCFRRemaining = sumPoAmount;
            //txtPOtotalcfr.Text = sumPoAmount.ToString();
            //decimal cfr/*, tax = 0*/;
            //cfr = Convert.ToDecimal(txtcfr.Text);
           

        }
        private void CalculateBookerTotal()
        {
            double sumfob = 0;
            double sumcfr = 0;
            double weight = 0;
            double Quantity = 0;
            double sumPoAmount = 0;

            if (grdBokkerItems.ItemsSource != null)
                foreach (var item in grdBokkerItems.ItemsSource as List<BookerStatementItem>)
                {
                        weight += item.siWeight;
                        Quantity += item.siQuantity;
                        sumcfr += item.siAmount;
                }
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtcfr.Text = sumcfr.ToString();
            if (purchaseOrder.Id != 0 && editpurchaseInvoice == 0)
            {
                txtTaxAmount.Text = purchaseOrder.totalAmountGST.ToString();
                chkTax.IsChecked = true;
                sumPoAmount = Convert.ToDouble(purchaseOrder.totalAmount + purchaseOrder.totalAmountGST);
                txtTotalTaxAmount.Text = sumPoAmount.ToString();
                txtPOtotalcfr.Text = sumPoAmount.ToString();
            }
            else if (purchaseOrder != null && editpurchaseInvoice == 1)
            {
                txtTaxAmount.Text = purchaseOrder.totalAmountGST.ToString();
                chkTax.IsChecked = true;
                sumPoAmount = Convert.ToDouble(purchaseOrder.totalAmount + purchaseOrder.totalAmountGST);
                txtTotalTaxAmount.Text = sumPoAmount.ToString();
                txtPOtotalcfr.Text = sumPoAmount.ToString();
            }
            decimal exchangeRate = 1;
            if (txtexchangerate.Text != "")
                exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
            decimal marginexchangeRate = 1;
            decimal totalcfr = Convert.ToDecimal(txtcfr.Text);
            if (txtexchangerate.Text != "")
            {
                marginexchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                txtBasetotalcfr.Text = (marginexchangeRate * totalcfr).ToString();
            }
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
        public List<JournalTransaction> getJournalTransactions()
        {
            if (purchaseInvoice.journalTransactions == null)
            {
                var procurementProducts = grdPIItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Inventory)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Income
                                       )
                                {
                                    total = 0-procurementProduct.NowAmount ;
                                }
                                else
                                {
                                    total = procurementProduct.NowAmount - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cAssetAccount.Id,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanceRef.Text,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = procurementProduct.NowAmount,
                                    credit = 0,
                                    deptId = department.Id,
                                    total =total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id
                                });

                            }
                        }
                    }
                    else
                  if (dbProduct.cgsAccount != null)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0- procurementProduct.NowAmount;
                            }
                            else
                            {
                                total = procurementProduct.NowAmount - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                prodId = dbProduct.Id,
                                accountId = dbProduct.cgsAccount.Id,
                                coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                PurchaseInvoiceId = purchaseInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtFinanceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = procurementProduct.NowAmount,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPICurrency.SelectedItem as cmbitem).id
                            });
                        }

                    }
                }
                if (department.accountPayableId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        double total = 0;
                        if (
                               department.AccountPayable.accountType == COA_AccountType.Loan ||
                               department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                               department.AccountPayable.accountType == COA_AccountType.Equity ||
                               department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                               department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                               department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                               department.AccountPayable.accountType == COA_AccountType.Income ||
                               department.AccountPayable.accountType == COA_AccountType.Other_Income
                               )
                        {
                            total =  Convert.ToDouble(txtTotalTaxAmount.Text);
                        }
                        else
                        {
                            total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = department.accountPayableId,
                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                            deptId = department.Id,
                            total = total,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                        });
                    }
                }
                if (Convert.ToDouble(txtPOtotalcfr.Text) != 0)
                {
                    if (lookUpTax.SelectedIndex != -1)
                    {
                        var purchaseInvoiceTax = taxRepo.getTaxtById((lookUpTax.SelectedItem as TaxName).Id);
                        if (purchaseInvoiceTax.chartofAccount != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                double total = 0;
                                if (
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0-Convert.ToDouble(txtTaxAmount.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txtTaxAmount.Text) - 0;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = purchaseInvoiceTax.COA_Id;
                                    taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanceRef.Text;
                                    taxTransaction.userId = purchaseInvoice.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                        }
                    }
                   
                }
            }
            else
            {
                var procurementProducts = grdPIItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Inventory)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount != null)
                            {
                                var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                  dbProduct.cAssetAccount.Id &&
                                  x.debit == procurementProduct.NowAmount &&
                                  x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                  x.deptId == department.Id
                                  );

                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0-procurementProduct.NowAmount ;
                                    }
                                    else
                                    {
                                        total = procurementProduct.NowAmount - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cAssetAccount.Id,
                                        coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                        PurchaseInvoiceId = purchaseInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanceRef.Text,
                                        MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = procurementProduct.NowAmount,
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                   
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income
                                               )
                                        {
                                            total = 0- procurementProduct.NowAmount ;
                                        }
                                        else
                                        {
                                            total = procurementProduct.NowAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.NowAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0- procurementProduct.NowAmount ;
                                        }
                                        else
                                        {
                                            total = procurementProduct.NowAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.NowAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }


                              
                            }
                        }
                    }
                    else
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cgsAccount != null)
                            {
                                var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                 dbProduct.cgsAccount.Id &&
                                 x.debit == procurementProduct.NowAmount &&
                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                 x.deptId == department.Id
                                 );
                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0- procurementProduct.NowAmount ;
                                    }
                                    else
                                    {
                                        total = procurementProduct.NowAmount - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cgsAccount.Id,
                                        coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                        PurchaseInvoiceId = purchaseInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanceRef.Text,
                                        MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = procurementProduct.NowAmount,
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                     
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0- procurementProduct.NowAmount - 0;
                                        }
                                        else
                                        {
                                            total = procurementProduct.NowAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.NowAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0- procurementProduct.NowAmount ;
                                        }
                                        else
                                        {
                                            total = procurementProduct.NowAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.NowAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }

                                    

                            }
                        }
                    }
                }
                if (department.accountPayableId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                               department.accountPayableId &&
                               x.credit == Convert.ToDouble(txtTotalTaxAmount.Text) &&
                               x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                               x.deptId == department.Id
                               );

                        if(dbTransaction==null)
                        {
                            double total = 0;
                            if (
                                   department.AccountPayable.accountType == COA_AccountType.Loan ||
                                   department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                   department.AccountPayable.accountType == COA_AccountType.Equity ||
                                   department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                   department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                   department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                   department.AccountPayable.accountType == COA_AccountType.Income ||
                                   department.AccountPayable.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total =  Convert.ToDouble(txtTotalTaxAmount.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = department.accountPayableId,
                                coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                PurchaseInvoiceId = purchaseInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtFinanceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                    
                            });

                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       department.AccountPayable.accountType == COA_AccountType.Loan ||
                                       department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                       department.AccountPayable.accountType == COA_AccountType.Equity ||
                                       department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                       department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Income ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.accountPayableId,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                    isReconciled = false,
                                    reconcilationDate = null,
                                    ReconcilationId = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions
                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       department.AccountPayable.accountType == COA_AccountType.Loan ||
                                       department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                       department.AccountPayable.accountType == COA_AccountType.Equity ||
                                       department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                       department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Income ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.accountPayableId,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                    reconcilationDate = dbTransaction.reconcilationDate,
                                    reconcilationType = dbTransaction.reconcilationType,
                                    ReconcilationId = dbTransaction.ReconcilationId,
                                    isReconciled = dbTransaction.isReconciled
                                });
                            }
                        }

                        

                    }
                }
                if (lookUpTax.SelectedIndex != -1)
                {
                    var purchaseInvoiceTax = taxRepo.getTaxtById((lookUpTax.SelectedItem as TaxName).Id);
                    if (btnPushDebits.IsChecked == true)
                    {
                        if (purchaseInvoiceTax.chartofAccount != null)
                        {
                            var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                            purchaseInvoiceTax.COA_Id &&
                            x.debit == Convert.ToDouble(txtTaxAmount.Text) &&
                            x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                            x.deptId == department.Id
                            );

                            if (dbTransaction == null)
                            {
                                double total = 0;
                                if (
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total =0- Convert.ToDouble(txtTaxAmount.Text) ;
                                }
                                else
                                {
                                    total = Convert.ToDouble(txtTaxAmount.Text) - 0;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = purchaseInvoiceTax.COA_Id;
                                    taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanceRef.Text;
                                    taxTransaction.userId = purchaseInvoice.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                             
                                }
                                journalTransactions.Add(taxTransaction);


                            }
                            else
                            {
                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    double total = 0;
                                    if (
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Income ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Income
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
                                        taxTransaction.accountId = purchaseInvoiceTax.COA_Id;
                                        taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                        taxTransaction.deptId = department.Id;
                                        taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                        taxTransaction.transactionRefno = txtFinanceRef.Text;
                                        taxTransaction.userId = purchaseInvoice.user_Id;
                                        taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                        taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                        taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        taxTransaction.total = total;
                                        taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                                        taxTransaction.isReconciled = false;
                                        taxTransaction.reconcilationDate = null;
                                        taxTransaction.ReconcilationId = null;
                                        taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                    }
                                    journalTransactions.Add(taxTransaction);
                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Income ||
                                           purchaseInvoiceTax.chartofAccount.accountType == COA_AccountType.Other_Income
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
                                        taxTransaction.accountId = purchaseInvoiceTax.COA_Id;
                                        taxTransaction.debit = Convert.ToDouble(txtTaxAmount.Text);
                                        taxTransaction.deptId = department.Id;
                                        taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                        taxTransaction.transactionRefno = txtFinanceRef.Text;
                                        taxTransaction.userId = purchaseInvoice.user_Id;
                                        taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                        taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                        taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        taxTransaction.total = total;
                                        taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                                        taxTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                        taxTransaction.reconcilationType = dbTransaction.reconcilationType;
                                        taxTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                        taxTransaction.isReconciled = dbTransaction.isReconciled;
                                    }
                                    journalTransactions.Add(taxTransaction);
                                }
                            }
                        }
                    }
                }
            }
            return journalTransactions;
        }

        public List<JournalTransaction> getBookerJournalTransactions()
        {
            if (purchaseInvoice.journalTransactions == null)
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0- procurementProduct.siAmount ;
                                }
                                else
                                {
                                    total = procurementProduct.siAmount - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cAssetAccount.Id,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    transactionRefno = txtFinanceRef.Text,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = procurementProduct.siAmount,
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id
                                });

                            }
                        }
                    }
                    else
                  if (dbProduct.cgsAccount != null)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                   dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total =0- procurementProduct.siAmount ;
                            }
                            else
                            {
                                total = procurementProduct.siAmount - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                prodId = dbProduct.Id,
                                accountId = dbProduct.cgsAccount.Id,
                                coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                PurchaseInvoiceId = purchaseInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                transactionRefno = txtFinanceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = procurementProduct.siAmount,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPICurrency.SelectedItem as cmbitem).id
                            });
                        }

                    }
                }
                if (department.accountPayableId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        double total = 0;
                        if (
                               department.AccountPayable.accountType == COA_AccountType.Loan ||
                               department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                               department.AccountPayable.accountType == COA_AccountType.Equity ||
                               department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                               department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                               department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                               department.AccountPayable.accountType == COA_AccountType.Income ||
                               department.AccountPayable.accountType == COA_AccountType.Other_Income
                               )
                        {
                            total = Convert.ToDouble(txtTotalTaxAmount.Text);
                        }
                        else
                        {
                            total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = department.accountPayableId,
                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = 0,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                            deptId = department.Id,
                            total = total,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                        });
                    }
                }
                if (Convert.ToDouble(txtPOtotalcfr.Text) != 0)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        var gstAccount = procurementProducts[0].TaxName.COA_Id;
                        var totalGST = procurementProducts.Sum(x => x.siAmountGST);
                        double total = 0;
                        if (
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                               )
                        {
                            total =0- totalGST;
                        }
                        else
                        {
                            total = totalGST - 0;
                        }
                        JournalTransaction taxTransaction = new JournalTransaction();
                        {
                            taxTransaction.accountId = gstAccount;
                            taxTransaction.debit = totalGST;
                            taxTransaction.deptId = department.Id;
                            taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                            taxTransaction.transactionRefno = txtFinanceRef.Text;
                            taxTransaction.userId = purchaseInvoice.user_Id;
                            taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                            taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                            taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                            taxTransaction.total =total ;
                            taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                            taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                        }
                        journalTransactions.Add(taxTransaction);
                    }
                }
            }
            else
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount != null)
                            {
                                var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                              dbProduct.cAssetAccount.Id &&
                              x.debit == procurementProduct.siAmount &&
                              x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                              x.deptId == department.Id

                              );
                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0-procurementProduct.siAmount ;
                                    }
                                    else
                                    {
                                        total = procurementProduct.siAmount - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cAssetAccount.Id,
                                        coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                        PurchaseInvoiceId = purchaseInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        transactionRefno = txtFinanceRef.Text,
                                        MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = procurementProduct.siAmount,
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                          
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                                dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = procurementProduct.siAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.siAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = procurementProduct.siAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.siAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }

                                
                            }
                        }
                    }
                    else
                    {
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cgsAccount != null)
                            {
                                var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                    dbProduct.cgsAccount.Id &&
                                    x.debit == procurementProduct.siAmount &&
                                    x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                    x.deptId == department.Id
                                    );
                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - procurementProduct.siAmount ;
                                    }
                                    else
                                    {
                                        total = procurementProduct.siAmount - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cgsAccount.Id,
                                        coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                        PurchaseInvoiceId = purchaseInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        transactionRefno = txtFinanceRef.Text,
                                        MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = procurementProduct.siAmount,
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {

                                        double total = 0;
                                        if (
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = procurementProduct.siAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.siAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = procurementProduct.siAmount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsAccount.Id,
                                            coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                            PurchaseInvoiceId = purchaseInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            transactionRefno = txtFinanceRef.Text,
                                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = procurementProduct.siAmount,
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }

                            }
                        }
                    }
                }
                if (department.accountPayableId != null)
                {
                    if (btnPushCredits.IsChecked == true)
                    {
                        var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                     department.accountPayableId &&
                                     x.credit == Convert.ToDouble(txtTotalTaxAmount.Text) &&
                                     x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                     x.deptId == department.Id
                                     );
                        if (dbTransaction == null)
                        {
                            double total = 0;
                            if (
                                   department.AccountPayable.accountType == COA_AccountType.Loan ||
                                   department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                   department.AccountPayable.accountType == COA_AccountType.Equity ||
                                   department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                   department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                   department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                   department.AccountPayable.accountType == COA_AccountType.Income ||
                                   department.AccountPayable.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = Convert.ToDouble(txtTotalTaxAmount.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = department.accountPayableId,
                                coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                PurchaseInvoiceId = purchaseInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                transactionRefno = txtFinanceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                 
                            });
                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       department.AccountPayable.accountType == COA_AccountType.Loan ||
                                       department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                       department.AccountPayable.accountType == COA_AccountType.Equity ||
                                       department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                       department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Income ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.accountPayableId,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    transactionRefno = txtFinanceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                    isReconciled = false,
                                    reconcilationDate = null,
                                    ReconcilationId = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions
                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       department.AccountPayable.accountType == COA_AccountType.Loan ||
                                       department.AccountPayable.accountType == COA_AccountType.Credit_Card ||
                                       department.AccountPayable.accountType == COA_AccountType.Equity ||
                                       department.AccountPayable.accountType == COA_AccountType.Accounts_Payable ||
                                       department.AccountPayable.accountType == COA_AccountType.Longterm_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.AccountPayable.accountType == COA_AccountType.Income ||
                                       department.AccountPayable.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txtTotalTaxAmount.Text);
                                }
                                
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.accountPayableId,
                                    coaTransactionsType = coaTransactionsType.PurchaseInvoice,
                                    PurchaseInvoiceId = purchaseInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    transactionRefno = txtFinanceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                                    credit = Convert.ToDouble(txtTotalTaxAmount.Text),
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                                    reconcilationDate = dbTransaction.reconcilationDate,
                                    reconcilationType = dbTransaction.reconcilationType,
                                    ReconcilationId = dbTransaction.ReconcilationId,
                                    isReconciled = dbTransaction.isReconciled
                                });
                            }
                        }
                    }
                }


                if (btnPushDebits.IsChecked == true)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        var gstAccount = procurementProducts[0].TaxName.COA_Id;
                        var totalGST = procurementProducts.Sum(x => x.siAmountGST);
                        var dbTransaction = purchaseInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                     gstAccount &&
                                     x.debit == totalGST &&
                                     x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                     x.deptId == department.Id
                                     );
                        if (dbTransaction == null)
                        {
                            double total = 0;
                            if (
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income ||
                                   procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income
                                   )
                            {
                                total = 0- totalGST ;
                            }
                            else
                            {
                                total = totalGST - 0;
                            }
                            JournalTransaction taxTransaction = new JournalTransaction();
                            {
                                taxTransaction.accountId = gstAccount;
                                taxTransaction.debit = totalGST;
                                taxTransaction.deptId = department.Id;
                                taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                taxTransaction.transactionRefno = txtFinanceRef.Text;
                                taxTransaction.userId = purchaseInvoice.user_Id;
                                taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                taxTransaction.total =total ;
                                taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                      
                            }
                            journalTransactions.Add(taxTransaction);
                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                   double total = 0;
                                if (
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0- totalGST ;
                                }
                                else
                                {
                                    total = totalGST - 0;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = gstAccount;
                                    taxTransaction.debit = totalGST;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanceRef.Text;
                                    taxTransaction.userId = purchaseInvoice.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total = totalGST - 0;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                                    taxTransaction.isReconciled = false;
                                    taxTransaction.reconcilationDate = null;
                                    taxTransaction.ReconcilationId = null;
                                    taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                            else
                            {
                                       double total = 0;
                                    if (
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0- totalGST ;
                                }
                                else
                                {
                                    total = totalGST - 0;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = gstAccount;
                                    taxTransaction.debit = totalGST;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.PurchaseInvoiceId = purchaseInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanceRef.Text;
                                    taxTransaction.userId = purchaseInvoice.user_Id;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
                                    taxTransaction.coaTransactionsType = coaTransactionsType.PurchaseInvoice;
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total =total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
                                    taxTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                    taxTransaction.reconcilationType = dbTransaction.reconcilationType;
                                    taxTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                    taxTransaction.isReconciled = dbTransaction.isReconciled;
                                }
                                journalTransactions.Add(taxTransaction);
                            }
                        }
                    }
                }
            }
            return journalTransactions;
        }
        public ICollection<ERP_BL.Procurements.Inventories.Inventory> getInventories()
        {
            AdjustmentRepo adjustmentRepo = new AdjustmentRepo();


            if (purchaseInvoice.Inventories==null && editInvoice==0)
            {
                var procurementProducts = grdPIItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        //var purchases = productrepo.getPurchases(dbProduct.Id);
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        //foreach(var purchase in allPurcashes)
                        //{
                        //    if(purchase.PurchaseInvoice.isVoid!=true)
                        //    {
                        //        purchases.Add(purchase);
                        //    }
                        //}
                        purchases = purchases.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        //var adjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpiCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();

                        //foreach (var adjustment in allAdjustments)
                        //{
                        //    if (adjustment.InventoryAdjustment.isVoid != true)
                        //    {
                        //        adjustments.Add(adjustment);
                        //    }
                        //}



                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);

                        //var sales = productrepo.getSales(dbProduct.Id);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice?.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC)+ Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity, 2);

                        sumPurchase = sumPurchase + sumAdjustmentsAmount;

                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity)+ procurementProduct.InvoicedQuantity;

                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;



                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales) ;
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        //var averageCost = remainingAmountInven / remaiInvenQuantity;

                        if (averageCost == 0 || remainingAmountInven == 0 || remaiInvenQuantity == 0)
                        {
                            averageCost = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity / procurementProduct.InvoicedQuantity, 2);
                        }

                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = procurementProduct.InvoicedQuantity,
                            Weight = procurementProduct.InvoicedWeight,
                            UnitRate = procurementProduct.unitPrice,
                            TransactionsType = InventoryTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datpiCreationdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            deptId = department.Id,
                            AmountOC = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity, 2),
                            //AmountOC = procurementProduct.unitPrice * procurementProduct.InvoicedQuantity,
                            AmountMER = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity, 2) * Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            //AmountMER = procurementProduct.unitPrice * procurementProduct.InvoicedQuantity* Convert.ToDouble(txtexchangerate.Text),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                    
                }
            }
            else
            {
                var procurementProducts = grdPIItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        //var purchases = productrepo.getPurchases(dbProduct.Id);
                        //var purchases = productrepo.getPurchases(dbProduct.Id);
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        //foreach(var purchase in allPurcashes)
                        //{
                        //    if(purchase.PurchaseInvoice.isVoid!=true)
                        //    {
                        //        purchases.Add(purchase);
                        //    }
                        //}
                        purchases = purchases.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        //var adjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpiCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();

                        //foreach (var adjustment in allAdjustments)
                        //{
                        //    if (adjustment.InventoryAdjustment.isVoid != true)
                        //    {
                        //        adjustments.Add(adjustment);
                        //    }
                        //}



                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);

                        //var sales = productrepo.getSales(dbProduct.Id);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate <= purchaseInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC) ;
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;


                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;



                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if(remaiInvenQuantity==0)
                        {
                             averageCost = procurementProduct.unitPrice;
                        }
                        //var averageCost = remainingAmountInven / remaiInvenQuantity;

                        if (averageCost==0  || remainingAmountInven==0 || remaiInvenQuantity==0)
                        {
                            averageCost = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity / procurementProduct.InvoicedQuantity, 2);
                        }

                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = procurementProduct.InvoicedQuantity,
                            Weight = procurementProduct.InvoicedWeight,
                            UnitRate = procurementProduct.unitPrice,
                            TransactionsType = InventoryTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datpiCreationdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            deptId = department.Id,
                            AmountOC = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity, 2),
                            //AmountOC = procurementProduct.unitPrice * procurementProduct.InvoicedQuantity,
                            AmountMER = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity, 2) * Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            //AmountMER = procurementProduct.unitPrice * procurementProduct.InvoicedQuantity * Convert.ToDouble(txtexchangerate.Text),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                }
            }

            return inventories;
        }
        public ICollection<ERP_BL.Procurements.Inventories.Inventory> getBookerInventories()
        {
            AdjustmentRepo adjustmentRepo = new AdjustmentRepo();


            if (purchaseInvoice.Inventories == null && editInvoice == 0)
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {

                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
  
                        purchases = purchases.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpiCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();

                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);

                        //var sales = productrepo.getSales(dbProduct.Id);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice?.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC) + Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity) + procurementProduct.quantity;

                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;



                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        //var averageCost = remainingAmountInven / remaiInvenQuantity;

                        if (averageCost == 0 || remainingAmountInven == 0 || remaiInvenQuantity == 0)
                        {
                            averageCost = Math.Round(procurementProduct.unit * procurementProduct.siQuantity / procurementProduct.siQuantity, 2);
                        }
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = procurementProduct.siQuantity,
                            Weight = procurementProduct.siWeight,
                            UnitRate = procurementProduct.unit,
                            TransactionsType = InventoryTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datpiCreationdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            deptId = department.Id,
                            AmountOC = Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2),
                            AmountMER = Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2) * Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }

                }
            }
            else
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpiCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice?.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < (DateTime)datpiCreationdate.EditValue).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC) + Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity) + procurementProduct.quantity;
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (averageCost == 0 || remainingAmountInven == 0 || remaiInvenQuantity == 0)
                        {
                            averageCost = Math.Round(procurementProduct.unit * procurementProduct.siQuantity / procurementProduct.siQuantity, 2);
                        }
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = procurementProduct.siQuantity,
                            Weight = procurementProduct.siWeight,
                            UnitRate = procurementProduct.unit,
                            TransactionsType = InventoryTransactionsType.PurchaseInvoice,
                            PurchaseInvoiceId = purchaseInvoice.Id,
                            creationDate = (DateTime)datpiCreationdate.EditValue,
                            transactionRefno = txtFinanceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            deptId = department.Id,
                            AmountOC = Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2),
                            AmountMER = Math.Round(procurementProduct.unit * procurementProduct.siQuantity, 2) * Math.Round(Convert.ToDouble(txtexchangerate.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbPICurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                }
            }

            return inventories;
        }
        public List<ERP_BL.VATBook.VATBook> GetVATBooks()
        {
            List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
            ERP_BL.VATBook.VATBook book = new ERP_BL.VATBook.VATBook();
            book.CreationDate = datpiCreationdate.DateTime;
            book.GLPostingDate = datpiCreationdate.DateTime;
            book.purchaseInvoiceId = purchaseInvoice.Id;
            book.TransactionType = TransactionItemType.PurchaseInvoice;
            book.debit = Convert.ToDouble(txtTaxAmount.Text);
            book.total = Convert.ToDouble(txtTaxAmount.Text)-0;
            book.FinanceRefNo = txtFinanceRef.Text;
            book.SystemRefNo = txtPIReferenceNumber.Text;
            book.deptId = purchaseInvoice.dept_Id;
            book.companyId = purchaseInvoice.company_Id;
            book.currencyId = (cmbPICurrency.SelectedItem as cmbitem).id;
            book.VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null;
            vatBooks.Add(book);
            return vatBooks;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                {
                    lookupVendor.Focus();
                    MessageBox.Show("Please Select a vendor against PI", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                {
                    MessageBox.Show("Please Select a Customer for whom PI is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCustomer.Focus();
                    return;
                }
                else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    MessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    MessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbEmployee.SelectedIndex == -1 && empUser.EmpId == 0)
                {
                    MessageBox.Show("Please Select an Employee to whom this Purchase Invoice will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                else if (cmbStatus.SelectedIndex == -1 && purchaseInvoice.PendingForClosing != true)
                {
                    MessageBox.Show("Please Select Current Status of Purchase Invoice to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbStatus.Focus();
                    return;
                }
                else if (cmbPICurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Purchase Invoices Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbPICurrency.Focus();
                    return;
                }
                else if (cmbPOCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Purchase Invoices Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbPOCurrency.Focus();
                    return;
                }
                else if (Convert.ToDouble(txtcfr.Text) == 0)
                {
                    MessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    grdPIItems.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtSOCER.Text))
                {
                    MessageBox.Show("Please Enter PO amount exchange rate", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtSOCER.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPIAmountSOC.Text))
                {
                    MessageBox.Show("Please Enter PI amount SOC", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtPIAmountSOC.Focus();
                    return;
                }
                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                else if (cmbEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select allocated to", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                {

                    purchaseInvoice.BookerStatementItems = getBookerStatementItems();
                    purchaseInvoice.journalTransactions = getBookerJournalTransactions();
                    purchaseInvoice.Inventories = getBookerInventories();
                }
                else
                {
                    purchaseInvoice.products = getProductsdata();
                    purchaseInvoice.journalTransactions = getJournalTransactions();

                    if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Inventory)
                        purchaseInvoice.Inventories = getInventories();
                    if (purchaseInvoice.products.Count == 0)
                    {
                        MessageBox.Show("Please Select items against which you want to create a Purchase Invoice", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                }
               
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    purchaseInvoice.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    purchaseInvoice.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                DateTime? dateTime = null;
                if (cmbcaption1.SelectedItem != null)
                    purchaseInvoice.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                if (cmbcaption2.SelectedItem != null)
                    purchaseInvoice.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                purchaseInvoice.POCER = Convert.ToDecimal(txtSOCER.Text);
                purchaseInvoice.PIAmuontSOC = Convert.ToDecimal(txtPIAmountSOC.Text);
                if (cmbPOPaymentTerm.SelectedItem != null)
                    purchaseInvoice.POPaymentterm_Id = (cmbPOPaymentTerm.SelectedItem as cmbitem).id;
                if (cmbIncoterm.SelectedItem != null)
                    purchaseInvoice.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;
                if (datglPostingdate.EditValue != null)
                {
                    purchaseInvoice.GLPostingDate = (DateTime)datglPostingdate.EditValue;
                }
                else
                {
                    purchaseInvoice.GLPostingDate = null;
                }


                //if (btnPushtoGL.IsChecked == true)
                //{
              
                //}
              
                if (purchaseInvoice.PurchaseOrder == null && purchaseOrder != null)
                {
                    purchaseInvoice.purchaseOrder_Id = purchaseOrder.Id;

                }
                if (chkInterCompany.IsChecked == true)
                {
                    if (InterCompany != null && InterCompany.Id != 0)
                    {

                        purchaseInvoice.InterCompany_Id = InterCompany.Id;
                    }
                    if (InterDepartment != null && InterDepartment.Id != 0)
                    {

                        purchaseInvoice.InterDepartment_Id = InterDepartment.Id;
                    }
                    purchaseInvoice.isInterCompany = true;

                }
                else
                {
                    purchaseInvoice.isInterCompany = false;
                    purchaseInvoice.InterCompany_Id = null;
                    purchaseInvoice.InterDepartment_Id = null;
                }


                //Basic Information
                purchaseInvoice.PurchaseInvoicetype = (InquiryType)cmbPurchaseInvoiceType.SelectedIndex;
                purchaseInvoice.CreationDate = (datpiCreationdate.Text == "") ? dateTime : datpiCreationdate.DateTime;
                // selected company
                if (company != null)
                {

                    purchaseInvoice.company_Id = company.Id;
                }

                // selected Department
                if (department != null)
                {

                    purchaseInvoice.dept_Id = department.Id;
                }
                //selected customer
                if (customer != null)
                {

                    purchaseInvoice.customerCompany_Id = customer.Id;
                }
                purchaseInvoice.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                // Selected Vendor 

                if (vendor != null)
                {

                    purchaseInvoice.vendors = new List<Vendor>();
                    purchaseInvoice.vendors.Add(purchaseInvoiceRepo.getVendor(vendor.Id));
                }
               if(!string.IsNullOrEmpty(txtVendorName.Text))
                {
                    purchaseInvoice.VendorName = txtVendorName.Text;
                }
                if (cmbxVATBookRef.SelectedIndex > -1)
                {
                    purchaseInvoice.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                    if (lookUpTax.SelectedIndex > -1)
                    {
                        purchaseInvoice.VATBooks = new List<ERP_BL.VATBook.VATBook>();
                        purchaseInvoice.VATBooks = GetVATBooks();
                    }
                }

                //PO/SO Informationn
                purchaseInvoice.POReferenceNo = txtPOReferenceNumber.Text;
                purchaseInvoice.SalesReferenceNo = txtSalesref.Text.Trim();
                purchaseInvoice.PODate = (datPoDate.Text == "") ? dateTime : datPoDate.DateTime;
                purchaseInvoice.PODeliveryDate = (datPODeliverydate.Text == "") ? dateTime : datPODeliverydate.DateTime;
                purchaseInvoice.SOReferenceNo = txtsaleOrderref.Text;
                purchaseInvoice.SODate = (datsaleOrderdate.Text == "") ? dateTime : datsaleOrderdate.DateTime;
                purchaseInvoice.SODeliveryDate = (datsoDeliverydate.Text == "") ? dateTime : datsoDeliverydate.DateTime;

                if ((cmbStatus.SelectedItem as cmbitem) != null)
                {
                    PurchaseInvoiceStatus status = purchaseInvoiceRepo.getstatus((cmbStatus.SelectedItem as cmbitem).id);
                    purchaseInvoice.PurchaseInvoiceStatus = status;
                }
                purchaseInvoice.FinanceRefrenceNo = txtFinanceRef.Text;
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    purchaseInvoice.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    purchaseInvoice.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    purchaseInvoice.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    purchaseInvoice.TotalQuantity = null;
                }
                // selected currency
                if (currency != null)
                {
                    purchaseInvoice.currency_Id = currency.Id;
                }
              
                purchaseInvoice.PIReferenceNo = txtPIReferenceNumber.Text.Trim();
                // Vendor Payment Status 
                if ((cmbVendorPaymentStatusChange.SelectedItem as cmbitem) != null)
                {

                    purchaseInvoice.vendorPaymentId = (cmbVendorPaymentStatusChange.SelectedItem as cmbitem).id;

                }
                
                if(lookUpTax.SelectedIndex>-1 && tax!=null)
                {
                    if(chkAdjustedTax.IsChecked==true)
                    {
                        purchaseInvoice.isAdjustedTax = true;
                        purchaseInvoice.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                    }
                    else
                    {
                        purchaseInvoice.isAdjustedTax = false;
                    }
                    purchaseInvoice.tax_Id = tax.Id;

                }





                purchaseInvoice.totalInvoiceAmount = Convert.ToDouble(txtcfr.Text);

                purchaseInvoice.POCFRValue = Math.Round( Convert.ToDouble(txtPOtotalcfr.Text), 1);

                if (purchaseInvoice.PurchaseOrder != null)
                {
                    purchaseInvoice.PurchaseOrder.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                    if (purchaseInvoice.PurchaseOrder.RemainingFOBValue == 0 && purchaseInvoice.PurchaseOrder.RemainingCFRValue == 0)
                    {
                        purchaseInvoice.PurchaseOrder.InvoiceStage = InvoiceStage.Fully.ToString();
                    }
                    else if (purchaseInvoice.PurchaseOrder.RemainingFOBValue != 0 && purchaseInvoice.PurchaseOrder.RemainingCFRValue != 0 && purchaseInvoice.PurchaseOrder.RemainingCFRValue < purchaseInvoice.POCFRValue)
                    {
                        purchaseInvoice.PurchaseOrder.InvoiceStage = InvoiceStage.Partialy.ToString();
                    }
                    else
                    {
                        purchaseInvoice.PurchaseOrder.InvoiceStage = InvoiceStage.None.ToString();

                    }
                }
                else
                {
                    if(!string.IsNullOrEmpty( txtPOremainingcfr.Text))
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                    if (purchaseOrder.RemainingFOBValue == 0 && purchaseOrder.RemainingCFRValue == 0)
                    {
                        purchaseOrder.InvoiceStage = InvoiceStage.Fully.ToString();
                    }
                    else if (purchaseOrder.RemainingFOBValue != 0 && purchaseOrder.RemainingCFRValue != 0 && purchaseOrder.RemainingCFRValue < purchaseInvoice.POCFRValue)
                    {
                        purchaseOrder.InvoiceStage = InvoiceStage.Partialy.ToString();
                    }
                    else
                    {
                        purchaseOrder.InvoiceStage = InvoiceStage.None.ToString();

                    }
                }

                purchaseInvoice.totalBaseAmount = Convert.ToDouble(txtBasetotalcfr.Text);


                //purchaseInvoice.SoAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);
                purchaseInvoice.exchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());
                purchaseInvoice.marginExchangeRate = Convert.ToDecimal(txtexchangerate.Text.Trim());

                if (!string.IsNullOrEmpty(txtTaxAmount.Text))
                {
                    purchaseInvoice.totaltaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                }
                var myWindow = Window.GetWindow(this);

                if (editInvoice == 1 && InvoiceId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Invoice") != null))
                {

                    if (MainWindow.currentUserid == 0)
                    {

                    }
                    else if (purchaseInvoice.user_Id == null)
                        purchaseInvoice.user_Id = MainWindow.currentUserid;

                    if (purchaseInvoice.CostSheet != null && purchaseInvoice.CostSheet.Timestamp != null)
                    {
                        purchaseInvoice.CostSheet.Timestamp = System.DateTime.Now;
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null && purchaseInvoice.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Invoice is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            purchaseInvoice.stage = TransactionStage.Approved.ToString();

                            purchaseInvoice.isApproved = true;
                            purchaseInvoice.ApprovedDate = System.DateTime.Now;
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != purchaseInvoice.PurchaseInvoiceStatus.Id)
                        {
                            purchaseInvoice.LastStatusChangeDate = System.DateTime.Now;
                        }
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);

                    if (checkStatus.Id != purchaseInvoice.PurchaseInvoiceStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Purchase Invoice has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                if (win.tagUsers.Count > 0)
                                {
                                    if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        purchaseInvoice.holderChangeDate = DateTime.Now;
                                    }
                                    purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                    purchaseInvoiceRepo.update(purchaseInvoice);

                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }

                        }

                        string oldStat = checkStatus.Status;
                        string newStat = purchaseInvoice.PurchaseInvoiceStatus.Status;
                        string symbolCurr = "";

                        if (purchaseInvoice.currency != null)
                        {
                            symbolCurr = purchaseInvoice.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Purchase Invoice(Amount OC) having value: " + purchaseInvoice.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation


                        };
                        procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ",null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + purchaseInvoice.SalesReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0,user.id, "New Comment ",null);
                            }

                        }


                        if (checkStatus != null && checkStatus.Id != 0)
                        {

                            usersRepo.Add(TransactionInfo.Status_Changed, purchaseInvoice.Id, 5, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseInvoice.PurchaseInvoiceStatus.Status + ")");
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Edited, purchaseInvoice.Id, 5, frmInputBox.comment);

                        MessageBox.Show("PurhchaseInvoice Updated Succesfully");
                        SystemLog.LogInfo(this.GetType(), "PurhchaseInvoice Updated Succesfully refrence No= " + purchaseInvoice.PIReferenceNo + " Id=" + purchaseInvoice.Id);

                    }
                    else if (editInvoice != 1)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice") != null)
                        {
                            if (MainWindow.currentUserid == 0)
                            {
                                MessageBox.Show("Please Create another Account to Create Purchase Invoice, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                                return;
                            }
                            else
                                purchaseInvoice.user_Id = MainWindow.currentUserid;


                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null)
                            {

                                {

                                    purchaseInvoice.stage = TransactionStage.Approved.ToString();

                                    purchaseInvoice.isApproved = true;
                                    purchaseInvoice.ApprovedDate = System.DateTime.Now;
                                }
                            }
                            else
                            {
                                purchaseInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                                purchaseInvoice.isApproved = false;
                            }
                            purchaseInvoiceRepo.Add(purchaseInvoice);
                            if (purchaseInvoice.PurchaseOrder == null && purchaseOrder != null)
                                purchaseInvoiceRepo.update(purchaseInvoice);
                            //PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                            //PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;
                            //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();
                            usersRepo.Add(TransactionInfo.Initialized, purchaseOrder.Id, 7, "");
                            usersRepo.Add(TransactionInfo.Created_Invoice, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, 6, "Purchase Invoice genrated on this Purchase Order");

                            SystemLog.LogInfo(this.GetType(), "PurchaseInvoice Added Succesfully refrence No= " + purchaseInvoice.PIReferenceNo + " Id=" + purchaseInvoice.Id);


                            MessageBox.Show("PurchaseInvoice Added Succesfully");
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");

                            myWindow.Close();
                            return;
                        }
                    DevExpress.Xpf.Core.DXMessageBox.Show("Purchase Invoice has been updated successfully!","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                    myWindow.Close();
                }
                else if (editInvoice != 1)
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            MessageBox.Show("Please Create another Account to Create Purchase Invoice, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                            return;
                        }
                        else
                            purchaseInvoice.user_Id = MainWindow.currentUserid;
                        if (purchaseInvoice.CostSheet != null)
                        {
                            purchaseInvoice.CostSheet.Timestamp = System.DateTime.Now;
                            purchaseInvoice.CostSheet.TransactionId = purchaseInvoice.Id;
                            purchaseInvoice.CostSheet.TransactionType = 2;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null)
                        {

                            {

                                purchaseInvoice.stage = TransactionStage.Approved.ToString();

                                purchaseInvoice.isApproved = true;
                                purchaseInvoice.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        else
                        {
                            purchaseInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                            purchaseInvoice.isApproved = false;
                        }
                        purchaseInvoiceRepo.Add(purchaseInvoice);
                        if (purchaseInvoice.PurchaseOrder == null && purchaseOrder != null)
                            purchaseInvoiceRepo.update(purchaseOrder);
                        //PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                        //PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;
                        //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();
                        usersRepo.Add(TransactionInfo.Initialized, purchaseInvoice.Id, 7, "");
                        //usersRepo.Add(TransactionInfo.Created_Invoice, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, 6, "Purchase Invoice genrated on this PurchaseOrder");

                        SystemLog.LogInfo(this.GetType(), "PurchaseInvoice Added Succesfully refrence No= " + purchaseInvoice.PIReferenceNo + " Id=" + purchaseInvoice.Id);


                        MessageBox.Show("PurchaseInvoice Added Succesfully");
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
                MessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "PurchaseInvoice Error refrence No= " + purchaseInvoice.PIReferenceNo + " Id=" + purchaseInvoice.Id + ex.ToString());

            }
        }
        public List<BookerStatementItem> getBookerStatementItems()
        {
            List<BookerStatementItem> bookeritems = new List<BookerStatementItem>();
            List<BookerStatementItem> bookerProducts = new List<BookerStatementItem>();

            bookerProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;

            foreach (var item in bookerProducts)
            {
                if (editInvoice != 0)
                {
                    if (item.Id != 0)
                    {
                        item.purchaseInvoiceId = InvoiceId;
                        item.unit = item.unit;

                        item.amount = item.amount;
                        item.amountGST = item.amountGST;
                        item.focValue = item.focValue;
                        item.netAmount = item.netAmount;
                        item.passOnValue = item.passOnValue;
                        item.weight = item.weight;
                        item.quantity = item.quantity;
                        item.claimDiscountValue = item.claimDiscountValue;
                        item.focValue = item.focValue;
                        item.passOnValue = item.passOnValue;




                        item.siAmount = item.siAmount;
                        item.siAmountGST = item.siAmountGST;
                        item.siFocValue = item.siFocValue;
                        item.siNetAmount = item.siNetAmount;
                        item.siPassOnValue = item.siPassOnValue;
                        item.siQuantity = item.siQuantity;
                        item.siWeight = item.siWeight;
                        item.siClaimDiscountValue = item.siClaimDiscountValue;
                        item.siFocValue = item.siFocValue;
                        item.siPassOnValue = item.siPassOnValue;
                        if (item.product.Id != 0)
                        {
                            item.product_Id = productRepo.get((int)item.product.Id).Id;
                        }
                        if (item.TaxName != null)
                        {
                            var tax = taxRepo.getTaxtById((int)item.TaxName.Id);
                            if (item.taxNameId == null)
                                item.TaxName = null;
                            item.taxNameId = tax.Id;


                        }

                        if (item.ClaimDiscount != null)
                        {
                            var claim = offerRepo.GetClaimDiscount((int)item.ClaimDiscount.Id);
                            if (item.claimDiscountId == null)
                                item.ClaimDiscount = null;
                            item.claimDiscountId = claim.Id;

                        }
                        if (item.FOCSampling != null)
                        {
                            var foc = offerRepo.GetFOCSampling((int)item.FOCSampling.Id);
                            if (item.focSamplingId == null)
                                item.FOCSampling = null;
                            item.focSamplingId = foc.Id;

                        }
                        if (item.PassOn != null)
                        {
                            var passOn = offerRepo.GetPassOn((int)item.PassOn.Id);
                            if (item.passOnId == null)
                                item.PassOn = null;
                            item.passOnId = passOn.Id;

                        }
                        bookeritems.Add(item);
                    }
                    else
                    {
                        BookerStatementItem bItem = new BookerStatementItem();
                        item.purchaseInvoiceId = InvoiceId;
                        bItem.unit = item.unit;
                        bItem.amount = item.amount;
                        bItem.amountGST = item.amountGST;
                        bItem.netAmount = item.netAmount;
                        bItem.weight = item.weight;
                        bItem.quantity = item.quantity;
                        bItem.passOnValue = item.passOnValue;
                        bItem.claimDiscountValue = item.claimDiscountValue;
                        bItem.focValue = item.focValue;



                        bItem.siAmount = item.siAmount;
                        bItem.siAmountGST = item.siAmountGST;
                        bItem.siFocValue = item.siFocValue;
                        bItem.siNetAmount = item.siNetAmount;
                        bItem.siPassOnValue = item.siPassOnValue;
                        bItem.siQuantity = item.siQuantity;
                        bItem.siWeight = item.siWeight;
                        bItem.siPassOnValue = item.siPassOnValue;
                        bItem.siClaimDiscountValue = item.siClaimDiscountValue;
                        bItem.siFocValue = item.siFocValue;



                        if (item.ClaimDiscount != null)
                        {
                            bItem.claimDiscountId = offerRepo.GetFOCSampling((int)item.ClaimDiscount.Id).Id;
                        }
                        if (item.FOCSampling != null)
                        {
                            bItem.focSamplingId = offerRepo.GetFOCSampling((int)item.FOCSampling.Id).Id;
                        }

                        if (item.PassOn != null)
                        {
                            bItem.passOnId = offerRepo.GetPassOn((int)item.PassOn.Id).Id;
                        }
                        if (item.product != null)
                        {
                            bItem.product_Id = productRepo.get((int)item.product.Id).Id;
                        }
                        if (item.TaxName != null)
                        {
                            bItem.taxNameId = taxRepo.getTaxtById((int)item.TaxName.Id).Id;
                        }

                        bookeritems.Add(bItem);
                    }
                }
                else
                {
                    BookerStatementItem bItem = new BookerStatementItem();
                    item.purchaseInvoiceId = InvoiceId;
                    bItem.unit = item.unit;
                    bItem.amount = item.amount;
                    bItem.amountGST = item.amountGST;
                    bItem.netAmount = item.netAmount;
                    bItem.weight = item.weight;
                    bItem.quantity = item.quantity;
                    bItem.passOnValue = item.passOnValue;
                    bItem.claimDiscountValue = item.claimDiscountValue;
                    bItem.focValue = item.focValue;



                    bItem.siAmount = item.siAmount;
                    bItem.siAmountGST = item.siAmountGST;
                    bItem.siFocValue = item.siFocValue;
                    bItem.siNetAmount = item.siNetAmount;
                    bItem.siPassOnValue = item.siPassOnValue;
                    bItem.siQuantity = item.siQuantity;
                    bItem.siWeight = item.siWeight;
                    bItem.siPassOnValue = item.siPassOnValue;
                    bItem.siClaimDiscountValue = item.siClaimDiscountValue;
                    bItem.siFocValue = item.siFocValue;

                    if (item.ClaimDiscount != null)
                    {
                        bItem.claimDiscountId = offerRepo.GetFOCSampling((int)item.ClaimDiscount.Id).Id;
                    }
                    if (item.FOCSampling != null)
                    {
                        bItem.focSamplingId = offerRepo.GetFOCSampling((int)item.FOCSampling.Id).Id;
                    }

                    if (item.passOnId != null)
                    {
                        bItem.passOnId = offerRepo.GetPassOn((int)item.passOnId).Id;
                    }
                    if (item.product != null)
                    {
                        bItem.product_Id = productRepo.get((int)item.product.Id).Id;
                    }
                    if (item.TaxName != null)
                    {
                        bItem.taxNameId = taxRepo.getTaxtById((int)item.TaxName.Id).Id;
                    }
                    bookeritems.Add(bItem);
                }
            }
            return bookeritems;
        }
        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> SOProducts = new List<ProcurementProduct>();

            List<ProcurementProduct> purchaseInvoiceItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdPIItems.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {

                            purchaseInvoiceItems.Add(new ProcurementProduct()
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
                                    product_Id = procurementProduct.inquiryProduct.product.Id,
                                    
                                    

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                                totalCommission = procurementProduct.totalCommission,

                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                            purchaseInvoiceItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,

                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,
                                totalCommission = procurementProduct.totalCommission,


                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                    }
                    if (purchaseOrder.products != null)

                        foreach (var PurchaseOrderProduct in purchaseOrder.products)
                        {
                            if (PurchaseOrderProduct.product_Id == procurementProduct.product_Id || PurchaseOrderProduct.inquiryProduct == procurementProduct.inquiryProduct)
                            {
                                //if (procurementProduct.inquiryProduct.Id == 0)
                                {
                                    if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                                    {
                                        PurchaseOrderProduct.UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity;
                                        PurchaseOrderProduct.TotalInvoicedQuantity = procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity;
                                        //SaleOrderProduct.InvoicedQuantity = procurementProduct.InvoicedQuantity;
                                        PurchaseOrderProduct.UnInvoicedWeight = procurementProduct.UnInvoicedWeight;
                                        PurchaseOrderProduct.TotalInvoicedWeight = Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight;

                                        PurchaseOrderProduct.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                                        PurchaseOrderProduct.totalInvoicedSoAmount = (procurementProduct.totalCommission == 0 || cmbPurchaseInvoiceType.SelectedItem.ToString() != InquiryType.Principal.ToString()) ? procurementProduct.value2 - procurementProduct.UnInvoicedSoAmount : procurementProduct.totalCommission - procurementProduct.UnInvoicedSoAmount;

                                    }
                                }
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

                            purchaseInvoiceItems.Add(new ProcurementProduct()
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

                                        isActive = procurementProduct.inquiryProduct.product.isActive
                                    }
                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,
                                totalCommission = procurementProduct.totalCommission,


                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority


                            });
                        }
                    }
                    else
                    {
                        // if user reloaded he saleOrder and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            purchaseInvoiceItems.Add(new ProcurementProduct()
                            {
                                Id=procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription,
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,

                                    product_Id = procurementProduct.inquiryProduct.product.Id

                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                                totalCommission = procurementProduct.totalCommission,

                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority


                            });

                        }
                    }

                    if (purchaseOrder.products != null && purchaseInvoice.products != null)
                        foreach (var PurchaseInvoiceProduct in purchaseInvoice.products)
                        {
                            if (PurchaseInvoiceProduct.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id)
                                foreach (var PurchaseOrderProduct in purchaseOrder.products)
                                {
                                    if (PurchaseOrderProduct.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id || PurchaseOrderProduct.inquiryProduct == procurementProduct.inquiryProduct)
                                    {
                                        //if (procurementProduct.inquiryProduct.Id == 0)
                                        {
                                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                                            {
                                                PurchaseOrderProduct.UnInvoicedQuantity = PurchaseOrderProduct.UnInvoicedQuantity + PurchaseInvoiceProduct.InvoicedQuantity - procurementProduct.InvoicedQuantity;
                                                PurchaseOrderProduct.TotalInvoicedQuantity = PurchaseOrderProduct.TotalInvoicedQuantity - PurchaseInvoiceProduct.InvoicedQuantity + procurementProduct.InvoicedQuantity;
                                                //SaleOrderProduct.InvoicedQuantity = procurementProduct.InvoicedQuantity;
                                                PurchaseOrderProduct.UnInvoicedWeight = PurchaseOrderProduct.UnInvoicedWeight + PurchaseInvoiceProduct.InvoicedWeight - procurementProduct.InvoicedWeight;
                                                PurchaseOrderProduct.TotalInvoicedWeight = PurchaseOrderProduct.TotalInvoicedWeight - PurchaseInvoiceProduct.InvoicedWeight + procurementProduct.InvoicedWeight;

                                                PurchaseOrderProduct.UnInvoicedSoAmount = PurchaseOrderProduct.totalInvoicedSoAmount + PurchaseInvoiceProduct.NowAmount - procurementProduct.NowAmount;
                                                PurchaseOrderProduct.totalInvoicedSoAmount = PurchaseOrderProduct.totalInvoicedSoAmount - PurchaseInvoiceProduct.NowAmount + procurementProduct.NowAmount;
                                            }
                                        }
                                    }
                                }
                        }
                }
            }


            return purchaseInvoiceItems;
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
                if (purchaseInvoice.Id != 0)
                {

                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Purchase_Invoice);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Purchase_Invoice);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (purchaseInvoice != null)
                    {
                        if (frmInputBox.commentAdded == true && purchaseInvoice.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PI #" + purchaseInvoice.PIReferenceNo, purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (purchaseInvoice.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
            loadcomments();
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

        private void Txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
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

        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            Vendorss.frmVendoradd vendoradd = new Vendorss.frmVendoradd();
            vendoradd.ShowDialog();
            loadvendors();
        }
       
        public void loadvendors()
        {
            lookupVendor.ItemsSource = department.Vendors;
        }

        private void cmbInquiryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Tender)
            {
                PoType = InquiryType.Tender;
                GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                nouwAmount.ReadOnly = false;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;
            }
            else
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Supply)
            {
                PoType = InquiryType.Supply;
                GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                nouwAmount.ReadOnly = false;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;

            }
            else
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Principal)
            {
                PoType = InquiryType.Principal;
                GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                nouwAmount.ReadOnly = false;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;

            }
            else
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Bill)
            {
                PoType = InquiryType.Bill;
                GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                nouwAmount.ReadOnly = false;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;
            }
            else
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Standard)
            {
                PoType = InquiryType.Standard;
                GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                nouwAmount.ReadOnly = false;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;
            }
            else
                if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.Inventory)
            { 
                PoType = InquiryType.Inventory;
                grdDistributionItems.Visibility = Visibility.Collapsed;
                grpProcItems.Visibility = Visibility.Visible;
                //GridColumn nouwAmount = grdPIItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                //nouwAmount.ReadOnly = true;
                btnBudgetSystemCostSheet.Visibility = Visibility.Collapsed;
                btnCostSheet.Visibility = Visibility.Collapsed;
            }
            else
                 if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                PoType = InquiryType.DistributionBiz;
                grdDistributionItems.Visibility = Visibility.Visible;
                grpProcItems.Visibility = Visibility.Collapsed;
                btnBudgetSystemCostSheet.Visibility = Visibility.Visible;
                btnCostSheet.Visibility = Visibility.Visible;

            }

            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null) ? true : false)
            {   
                if (cmbEmployee.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select allocated to", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if (purchaseInvoiceId == 0)
                {
                    return;
                }
                UsersRepo usersRepo = new UsersRepo();

                var row = purchaseInvoice;
                if (row.PurchaseInvoiceStatus != null)
                {
                    oldStatus = row.PurchaseInvoiceStatus;
                }
                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid = purchaseInvoiceId;
                ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange statusChange = new ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange();
                var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();
                if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid != 0)
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null) ? true : false)
                    {
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = false;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.Approved.ToString();

                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                        if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null)
                    {
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                        }

                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                    }
                    else
                    {
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;
                    }
                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.ClosingDate = System.DateTime.Now;
                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.LastStatusChangeDate = System.DateTime.Now;
                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceRepo.updateForDirectClose(ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice);
               
                
                
                //PurchaseOrderss.ucStatuschange.purchaseOrderid = (int)purchaseInvoice.purchaseOrder_Id;
                //PurchaseOrderss.frmPurchaseOrderStatusChange statChange = new PurchaseOrderss.frmPurchaseOrderStatusChange();
                //PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active PO Statusses

                //statChange.Owner = myWindow;
                //statChange.ShowDialog();
                //PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrderStatusforInvoice();
                //MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                MessageBox.Show("PurchaseInvoice status changed to InActive (" + ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PurchaseInvoiceStatus.Status + ")");

                //Adding auto Signature

                UsersRepo userRepo = new UsersRepo();
                //Asking for Tag
                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                var res = MessageBox.Show("Purchase Invocie has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (row.department != null && row.department.Id != 0 && row.company?.Id != 0)
                    {
                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        if (win.tagUsers.Count > 0)
                        {
                            if (purchaseInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                            {
                                purchaseInvoice.holderChangeDate = DateTime.Now;
                            }
                            purchaseInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                            purchaseInvoiceRepo.updatePI(purchaseInvoice);
                        }
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
                string newStat = ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PurchaseInvoiceStatus.Status;
                string symbolCurr = "";
                if (row.currency != null)
                {
                    symbolCurr = row.currency.Abbrivation.ToString();
                }
                CommentLog comment = new CommentLog()
                {
                    Comment = "Status of Purchase Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                    Timestamp = DateTime.Now,
                    Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                };
                procurementRepo.Add(row.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                //Creating Comments
                if (tagUsers.Count != 0)
                {
                    foreach (var user in tagUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);
                    }
                }

                if (ccUsers.Count != 0)
                {
                    foreach (var user in ccUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, 0,user.id, "New Comment ", null);
                    }
                }

                var thisWindow = Window.GetWindow(this);
                thisWindow.Close();
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close PurchaseInvoice Directly.");
            }
        }
        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (purchaseInvoiceId != 0)
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
                        destination += "Attachments\\Purchase_Invoice\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += purchaseInvoiceId + "_" + TransactionItemType.Purchase_Invoice.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Purchase_Invoice);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), purchaseInvoiceId, TransactionItemType.Purchase_Invoice, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        usersRepo.Add(TransactionInfo.Attachment_Uploaded, purchaseInvoice.Id, 7, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseInvoiceId, TransactionItemType.Purchase_Invoice);
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
        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            if ((InquiryType)cmbPurchaseInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
            }
            else
            {
                (grdPIItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            }

        }

        private void ColNowInvoiceAmount_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            if (cmbPurchaseInvoiceType.SelectedItem != null)
            {
                if (cmbPurchaseInvoiceType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                {
                    var Total = Convert.ToDouble(((ProcurementProduct)e.Row).totalCommission);
                    var TotalInvoiced = ((ProcurementProduct)e.Row).totalInvoicedSoAmount;
                    var unInvoiced = ((ProcurementProduct)e.Row).UnInvoicedSoAmount;
                    var invoiced = Convert.ToDouble(e.Value);
                    if (Total < invoiced)
                    {

                        //if (!(invoicedqty > 0 && discount <= 30))
                        {
                            e.IsValid = false;
                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            if (invoiced < 0)
                            {
                                e.ErrorContent = string.Format("The invoiced amount can't be negative!",
                                                            Convert.ToDouble(e.CellValue));
                                return;
                            }
                            e.ErrorContent = string.Format(
                               "The Invoiced amount cannot be greater than (${0}). Please correct the Invoiced amount.",
                               Convert.ToDouble(e.CellValue));
                        }
                    }
                    else
                    {
                        var UnInv = Total - (TotalInvoiced + invoiced);
                        grdPIItems.SetFocusedRowCellValue("UnInvoicedPoAmount", UnInv);
                    }
                }
                else if (cmbPurchaseInvoiceType.SelectedItem.ToString() != InquiryType.Principal.ToString())
                {
                    var Total = Convert.ToDouble(((ProcurementProduct)e.Row).value2);
                    var TotalInvoiced = ((ProcurementProduct)e.Row).totalInvoicedSoAmount;
                    var unInvoiced = ((ProcurementProduct)e.Row).UnInvoicedSoAmount;
                    var invoiced = Convert.ToDouble(e.Value);
                    if (Total < invoiced)
                    {

                        //if (!(invoicedqty > 0 && discount <= 30))
                        {
                            e.IsValid = false;
                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            if (invoiced < 0)
                            {
                                e.ErrorContent = string.Format("The invoiced amount can't be negative!",
                                                            Convert.ToDouble(e.CellValue));
                                return;
                            }
                            e.ErrorContent = string.Format(
                               "The Invoiced amount cannot be greater than (${0}). Please correct the Invoiced amount.",
                               Convert.ToDouble(e.CellValue));
                        }
                    }
                    else
                    {
                        var UnInv = Total - (TotalInvoiced + invoiced);
                        grdPIItems.SetFocusedRowCellValue("UnInvoicedPoAmount", UnInv);
                    }
                }

            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            editInvoice = 0;
            purchaseOrderId = 0;
            purchaseInvoiceId = 0;
            if (purchaseInvoice != null && purchaseInvoice.Id != 0)
                usersRepo.Add(TransactionInfo.viewed, purchaseInvoice.Id, 7, "Viewed details of Purchase Invoice");
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPIItems);
        }

        private void TxtPIReferenceNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblPIrefrence.Text = txtPIReferenceNumber.Text;
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (purchaseInvoice.isApproved != true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null)
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.isApproved = false;
                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approver_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment,null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = false;
                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        usersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = false;

                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        usersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment, null);

                }
            }
            else if (purchaseInvoice.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null)
                {
                    purchaseInvoice.isReviewed = true;
                    {
                        purchaseInvoice.PendingForClosing = true;

                    }
                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approver_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null))
                {
                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = false;
                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null))
                {

                    purchaseInvoice.isReviewed = true;
                    purchaseInvoice.needReview = true;
                    purchaseInvoice.stage = TransactionStage.Rejected.ToString();

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseInvoice.Id, (int)TransactionItemType.Purchase_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseInvoiceRepo.update(purchaseInvoice);
                    notificationsRepo.Add("PurchaseInvoice Rejected", purchaseInvoice.Id, TransactionItemType.Purchase_Invoice, "PurchaseInvoice with refrence # " + purchaseInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseInvoice" + frmInputBox.comment, null);

                }
            }
        }

        private void CmbVendorPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbVendorPaymentStatusChange.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbVendorPaymentStatusChange.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Vendorss.frmVendorPaymentStatusAdd VendorPaymentStatuss = new Vendorss.frmVendorPaymentStatusAdd();
                    VendorPaymentStatuss.ShowDialog();
                    loadVendorPaymentStatus();

                }
            }
        }
        public void loadVendorPaymentStatus()
        {

            List<VendorPaymentStatus> VendorPaymentStatuss = new List<VendorPaymentStatus>();

            VendorPaymentStatuss = vendorRepo.getAllActiveVendorPaymentStatus();


            List<cmbitem> cmbitemsChange = new List<cmbitem>();


            foreach (VendorPaymentStatus status in VendorPaymentStatuss)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitemsChange.Add(new cmbitem() { name = status.Status, id = status.Id, fcolor = "#FF000000" });


            }
            cmbitemsChange.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbVendorPaymentStatusChange.ItemsSource = cmbitemsChange;

        }


        private void LookUpTax_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var item = lookUpTax.SelectedItem as TaxName;



            if (item != null)
            {

                if (item.isManual != false)
                {
                    txtTaxAmount.IsReadOnly = false;

                }
                else
                {
                    txtTaxAmount.IsReadOnly = true;
                    var percent = item.percentage;
                    txtcfr.Text = string.IsNullOrEmpty(txtcfr.Text) ? 0.ToString() : txtcfr.Text;
                    //txtPIAmount1.Text = string.IsNullOrEmpty(txtcfr.Text) ? 0.ToString() : txtcfr.Text;

                    var amount = Convert.ToDouble(txtcfr.Text);

                    var percentAmount = Math.Round((percent / 100) * amount, 2);
                    txtTaxAmount.Text = percentAmount.ToString();
                    //amount = amount + percentAmount;
                    var taxTotalAmount = Math.Round(amount + percentAmount, 2);
                    txtTotalTaxAmount.Text = taxTotalAmount.ToString();
                }

               
                tax = item;
                //txtBillWithTax.Text = amount.ToString();

                //if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
                //{
                //    var amount1 = Convert.ToDouble(txtBillWithTax.Text);
                //    var item1 = lookUpWHT.SelectedItem as TaxName;
                //    var percent1 = item1.percentage;
                //    var percentAmount1 = (percent1 / 100) * amount1;
                //    txtWhtAmount.Text = percentAmount1.ToString();
                //    amount1 = amount1 - percentAmount1;
                //    txtBillAfterTax.Text = amount1.ToString();
                //}
                //else
                //{
                //    txtBillAfterTax.Text = amount.ToString();
                //}
            }
        }

        private void ChkTax_Checked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = true;

            chkAdjustedTax.IsChecked = false;
            lookUpTax.ItemsSource = taxRepo.getAllAdjustedTaxes();
            //if (chkWHT.IsChecked == true)
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
            //    txtBillAfterTax.Text = txtBillWithTax.Text;
            //}
        }

        private void ChkTax_Unchecked(object sender, RoutedEventArgs e)
        {
            purchaseInvoice.tax = null;
            chkTax.IsChecked = false;
            tax = null;
            if(editpurchaseInvoice ==0)
                txtTaxAmount.Text = "0";
        }
        private double CalculateUninvoicedQty(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceQty = product.InvoicedQuantity;
                        result = result + invoiceQty;
                    }
                }

            }
            return result;

        }
        private double CalculateUninvoicedWeight(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceWeight = product.InvoicedWeight;
                        result = result + invoiceWeight;
                    }
                }
            }
            return result;
        }

        private double CalculateInvoicedSoAmount(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceAmount = product.NowAmount;
                        result = result + invoiceAmount;
                    }
                }
            }
            return result;
        }

        private void CmbPOCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TxtPER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSOCER.Text))
            {
                //var eRate = Math.Round(Convert.ToDecimal(txtSOCER.Text), 2);
                var PER = Convert.ToDecimal(txtSOCER.Text);// Convert.ToDecimal(txtSOCER.Text);
                var PERAmount = Convert.ToDecimal(txtcfr.Text);
                txtPIAmountSOC.Text = Math.Round(PER * PERAmount, 2).ToString();
            }
            else
                txtPIAmountSOC.Text = txtcfr.Text;

        }

       

        //private void BtnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        //{
        //    //btnCostSheet.IsEnabled = false;

        //    //try
        //    //{
        //    //    var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Bill?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
        //    //    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //    //    {
        //    //        //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Bill") != null)
        //    //        if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Bill") != null)

        //    //        {
        //    //            if (purchaseOrder.SaleOrder != null && purchaseOrder.SaleOrder.Id != 0 && !string.IsNullOrEmpty(txtPIAmountSOC.Text) && purchaseInvoice.Id != 0)
        //    //            {
        //    //                winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(purchaseOrder.SaleOrder, Convert.ToDecimal(txtPIAmountSOC.Text), purchaseInvoice.Id);
        //    //                winSelectCostSheetFields.ShowDialog();
        //    //            }
        //    //        }
        //    //        else
        //    //            DXMessageBox.Show("You do not have permisssion Update CostSheet from Bill", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.Message);
        //    //}
        //}

        private void TxtPOAmountSOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPIAmountSOC.Text))
            {
                var PER = Convert.ToDouble(txtPIAmountSOC.Text);
                var PERAmount = Convert.ToDouble(txtcfr.Text);
                txtSOCER.Text = Math.Round( (PER / PERAmount),2).ToString();
            }
            else
                txtSOCER.Text = txtcfr.Text;

        }
        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            //btnCostSheetPunching.IsEnabled = false;

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
                //if (bill?.SaleOrder == null)
                //{
                //    return;
                //}
                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                var saleOrder=saleOrderRepo.get((int)purchaseOrder.saleOrder_Id);
                frmCostSheet frmCostSheet = new frmCostSheet(saleOrder, symbol, inco, purchaseOrder.CreationDate.ToString(), paymentterm, purchaseOrder.maker, purchaseOrder.origin, views, (InquiryType)cmbPurchaseInvoiceType.SelectedIndex,purchaseOrder.packing, purchaseOrder.POWarranty.name, purchaseInvoice.isApproved, false);
                frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                //frmCostSheet.lookupVendors.IsEnabled = false;
                //frmCostSheet.lookupOC.IsEnabled = false;
                frmCostSheet.ShowDialog();
              
                //{
                //    if (frmCostSheet.costSheet != null)
                //    {
                //        bill.CostSheet = frmCostSheet.costSheet;
                //        txtBudgetMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalBudgetedMargin).ToString();
                //        txtRevisedMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalRevisedMargin).ToString();

                //        txtActualMargin.Text = (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) != (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(bill.SaleOrder.totalCFRValue) - bill.CostSheet.TotalActualMargin).ToString() : "0";
                //        if ((bill.isReApproved != false) && frmCostSheet.isReApproved == false)
                //            bill.stage = TransactionStage.AwaitingFirstReview.ToString();
                //        bill.isReApproved = frmCostSheet.isReApproved;

                //    }
                //}

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
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
        public void loadPaymentTerms()
        {
            PaymentTermRepo TermRepo = new PaymentTermRepo();
            List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
            paymentTerms = TermRepo.getAllForPO();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PaymentTerm paymentTerm in paymentTerms)
            {
                cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbPOPaymentTerm.ItemsSource = cmbitems;

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
            IncotermRepo termRepo = new IncotermRepo();
            List<Incoterm> incoterms = new List<Incoterm>();
            incoterms = termRepo.getAll();
            loadCaptions(incoterms);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Incoterm incoterm in incoterms)
            {
                cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            }

            //cmbcaption1.ItemsSource = cmbitems;
            //cmbcaption2.ItemsSource = cmbitems;
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbIncoterm.ItemsSource = cmbitems;
        }
        public void loadCaptions(List<Incoterm> incoterms)
        {

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Incoterm incoterm in incoterms)
            {
                cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            }

            cmbcaption1.ItemsSource = cmbitems;
            cmbcaption2.ItemsSource = cmbitems;

        }

        private void cmbcaption1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdPIItems != null && grdPIItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdPIItems.Columns.GetColumnByFieldName("value1").Header = ((cmbitem)cmbcaption1.SelectedItem).name + symbol;

        }

        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (grdPIItems != null && grdPIItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdPIItems.Columns.GetColumnByFieldName("value1").Header = ((cmbitem)cmbcaption2.SelectedItem).name + symbol;
        }

        private void TxtTotalTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtPIAmount1.Text = txtTotalTaxAmount.Text;
            txtPoAmount.Text = txtTotalTaxAmount.Text;
        }
        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                if((InquiryType)cmbPurchaseInvoiceType.SelectedIndex==InquiryType.DistributionBiz )
                {
                    ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getBookerJournalTransactions());
                    generalJournal.ShowDialog();
                }
                else
                {
                    ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getJournalTransactions());
                    generalJournal.ShowDialog();
                }


             
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseInvoice.PurchaseInvoiceStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when Purchase Invoice Closed") != null)
                {
                    if (grdAttach.Visibility == Visibility.Visible)
                        grdAttach1.Visibility = Visibility.Collapsed;
                    else
                    {
                        if (purchaseInvoice.Id != 0)
                        {
                            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActivePIAttachmentCategories();
                        }
                        grdAttach1.Visibility = Visibility.Visible;

                    }
                }
                else
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required " + "Can attach document when Purchase Invoice Closed!");
            }
            else
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    if (purchaseInvoice.Id != 0)
                    {
                        cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActivePIAttachmentCategories();
                    }
                    grdAttach1.Visibility = Visibility.Visible;

                }
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
                List<TreeItem> otherAttachments = new List<TreeItem>();

                if (purchaseInvoice.Id != 0)
                {
                    //treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoice.Id, TransactionItemType.Sale_Invoice);

                    if (purchaseInvoice.PurchaseOrder.SaleOrder != null)
                    {
                        var saleOrder = purchaseInvoice.PurchaseOrder.SaleOrder;
                        List<TreeItem> atachments = SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Purchase_Invoice);
                        if (saleOrder.offer != null)
                        {
                            if (saleOrder.offer_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                            if (saleOrder.offer.inquiry_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                        }

                        if (saleOrder.SaleInvoices.Count != 0)
                        {
                            foreach (var invoice in saleOrder.SaleInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                                if (invoice.salesReceipts.Count != 0)
                                    foreach (var receipt in invoice.salesReceipts)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                    }
                            }
                        }
                        if (saleOrder.PurchaseOrders.Count != 0)
                        {
                            foreach (var pO in saleOrder.PurchaseOrders)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                                if (pO.PurchaseInvoices.Count != 0)
                                    foreach (var pI in pO.PurchaseInvoices)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                        if (pI.Payments.Count != 0)
                                            foreach (var payment in pI.Payments)
                                            {
                                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                            }
                                    }
                            }
                        }
                        if (saleOrder.Bills.Count != 0)
                        {
                            foreach (var bill in saleOrder.Bills)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                if (bill.Payments.Count != 0)
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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

                    }
                    else
                    {
                       
                        List<TreeItem> atachments = SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)purchaseInvoice.Id, TransactionItemType.Purchase_Invoice);
                        otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)purchaseInvoice.purchaseOrder_Id, TransactionItemType.Purchase_Order));
                        if (purchaseInvoice.PurchaseOrder.Bills.Count != 0)
                            foreach (var bill in purchaseInvoice.PurchaseOrder.Bills)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                if (bill.Payments.Count != 0)
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                    }
                            }

                        if (purchaseInvoice.Payments.Count != 0)
                            foreach (var payment in purchaseInvoice.Payments)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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
                    }
                   
                    //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
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
                if (purchaseInvoiceId != 0)
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
                        destination += "Attachments\\Purchase_Invoice\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += purchaseInvoiceId + "_" + TransactionItemType.Purchase_Invoice.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Purchase_Invoice);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), purchaseInvoiceId, TransactionItemType.Purchase_Invoice, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        usersRepo.Add(TransactionInfo.Attachment_Uploaded, purchaseInvoice.Id, 7, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseInvoiceId, TransactionItemType.Purchase_Invoice);
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

        private void TxtTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
           
            txtcfr.Text = string.IsNullOrEmpty(txtcfr.Text) ? 0.ToString() : txtcfr.Text;
            var amount = Convert.ToDouble(txtcfr.Text);
            var taxTotalAmount = Math.Round(amount + Convert.ToDouble(txtTaxAmount.Text), 2);
            txtTotalTaxAmount.Text = taxTotalAmount.ToString();
        }

        private void chkAdjustedTax_Checked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = true;
            chkTax.IsChecked = false;
            lookUpTax.ItemsSource = taxRepo.getAllAdjustedTaxes();
        }

        private void chkAdjustedTax_Unchecked(object sender, RoutedEventArgs e)
        {
            purchaseInvoice.tax = null;
            tax = null;
            txtTaxAmount.Text = "0";
            lookUpTax.SelectedIndex = -1;
        }

        private void btnBudgetCostSheet_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnBudgetSystemCostSheet_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseInvoice.purchaseOrder_Id != null)
            {
                if (purchaseInvoice.PurchaseOrder.Budget_Id != null)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                    {
                        if (purchaseInvoice.BudgetSystemCostFields.Count != 0)
                        {
                            winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)purchaseOrder.Budget_Id, true, Convert.ToDouble(txtcfr.Text), purchaseOrder);
                            winAddBudgetSystemCost.costSheetFields = purchaseInvoice.BudgetSystemCostFields;
                            systemCost.ShowDialog();
                            purchaseInvoice.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;
                        }
                        else
                        {
                            if (purchaseOrder.Budget_Id != null)
                            {
                                winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)purchaseOrder.Budget_Id, false, Convert.ToDouble(txtcfr.Text), purchaseOrder);
                                systemCost.ShowDialog();
                                purchaseInvoice.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;
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
                    DXMessageBox.Show("Please attach budget with purchase order first");
                }
            }
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (purchaseInvoice.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (purchaseInvoice.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = purchaseInvoice.holderChangeDate;
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
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Invoice);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Purchase_Invoice);
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

                if (frmInputBox.commentAdded == true && purchaseInvoice.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.PurchaseInvoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (purchaseInvoice.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Purchase Invoice first to add a comment!");
                }
            }
            loadcomments();
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
        public void GellAllOrdersTracking()
        {
            trackingOrder = purchaseInvoiceRepo.get(purchaseInvoice.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Purchase_Invoice);
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
