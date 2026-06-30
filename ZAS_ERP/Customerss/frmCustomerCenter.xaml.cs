using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls;

namespace ZAS_ERP.Customerss
{
    /// <summary>
    /// Interaction logic for frmCustomerCenter.xaml
    /// </summary>
    /// 
    public partial class frmCustomerCenter : DXWindow
    {
        public static int Editit;
        public static int customerid;
        Procurementss.Inquiriess.ucInquiryGrid ucInquirygrid;//= new Procurementss.Inquiriess.ucInquiryGrid(true);
        Procurementss.Offerss.ucOfferGrid ucOffergrid;//= new Procurementss.Offerss.ucOfferGrid();
        Procurementss.ModuleContract.UserControls.ucModuleContractGrid ucModuleContractgrid;
        Procurementss.SaleOrderss.ucSaleOrderGrid saleOrderGrid;//= new Procurementss.SaleOrderss.ucSaleOrderGrid();
        Procurementss.SaleInvoicess.ucSaleInvoiceGrid saleInvoiceGrid;// = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
        Procurementss.PurchaseInvoice.UserControls.ucPIGrid pIGrid;
        ucInventoryAdjustmentGrid iAGrid;
        Procurementss.Billss.ucBillGrid billGrid;
        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid purchaseOrderGrid;// = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
        Procurementss.MemorandumSaless.ucMemorandumSaleGrid memorandumSaleGrid;// = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
        Procurementss.PurchaseInvoice.UserControls.ucPIGrid ucPIGrid;
        ChartofAccounts.UserControls.ucTransactions jvGrid = new ChartofAccounts.UserControls.ucTransactions();
        ucContactPerson ucContactPerson = new ucContactPerson();
        CustomerCompany customerCompany = new CustomerCompany();
        CustomerCompRepo CustomerCompRepo = new CustomerCompRepo();
        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid ucModulegrid;
        public frmCustomerCenter()
        {
            InitializeComponent();
            this.Activate();

            //SystemLog.LogInfo(this.GetType(), "Form Intialized");
            //PopulateTransactionPanel();
        }

        public frmCustomerCenter(int Tabindex, TransactionItemType SelectedItem, DataType datatype)
        {

            InitializeComponent();
            this.Activate();

            SystemLog.LogInfo(this.GetType(), "Form Intialized");

            if (Tabindex == 0)
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "View Customer Detail Info") != null))
                {
                    tabCustomers.IsSelected = true;
                    
                }
                else
                {
                    return;
                }


                
            }
            else if (Tabindex == 1)
            {
                tabTransactions.IsSelected = true;
                switch ((SelectedItem))
                {
                    case (TransactionItemType.Inquiry):
                        {
                            var item = "Inquiries";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForReApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Offer):
                        {
                            string item = "Offers";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.ModuleContract):
                        {
                            string item = "Module Contract";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Sale_Order):
                        {
                            string item = "Sale Orders";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Memorandum_Sale):
                        {
                            string item = "Memorandum Sales";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Sale_Invoice):
                        {
                            string item = "Sale Invoices";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Purchase_Order):
                        {
                            string item = "Purchase Orders";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForReApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.Bill):
                        {
                            string item = "Bills";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;

                    case (TransactionItemType.JV):
                        {
                            string item = "jv";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;


                    case (TransactionItemType.Employee):
                        {
                            string item = "employee";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;

                            }
                        }
                        break;
                    case (TransactionItemType.Purchase_Invoice):
                        {
                            string item = "Purchase Invoices";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;
                    case (TransactionItemType.InventoryAdjustment):
                        {
                            string item = "Inventory Adjustment";
                            switch (datatype)
                            {
                                case DataType.All:
                                    LoadTransactionGrid(item);
                                    break;
                                case DataType.Open:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");

                                    break;
                                case DataType.Closed:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForApproval:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                                case DataType.PendingForClosing:
                                    LoadTransactionGrid($"{item}({datatype.ToString()})");
                                    break;
                            }
                        }
                        break;

                }
            }

        }

        private void mbtnaddcomp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddCompany();

        }

        private void AddCompany()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Customer") != null || MainWindow.currentUserid == 0)
            {
                frmCustomeradd customeradd = new frmCustomeradd();

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Info") != null))
                {
                    customeradd.tabCustomerInfo.IsEnabled = true;
                    customeradd.txtBussinesName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Company Name") != null) ? true : false;
                    customeradd.cmbIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Industry") != null) ? true : false;
                    customeradd.cmbBussinesType.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Business Type") != null) ? true : false;
                    customeradd.cmbCurrency.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Currency") != null) ? true : false;
                    customeradd.lookupCustomer.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Company Name") != null) ? true : false;
                    //billing
                    customeradd.txtAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address line1") != null) ? true : false;
                    customeradd.txtAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address line2") != null) ? true : false;
                    customeradd.txtCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address City") != null) ? true : false;
                    customeradd.txtState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address State") != null) ? true : false;
                    customeradd.txtCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Country") != null) ? true : false;
                    customeradd.txtZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Zip") != null) ? true : false;
                    customeradd.txtRegionbil.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Region") != null) ? true : false;
                    //Shipping
                    customeradd.txtsAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address line1") != null) ? true : false;
                    customeradd.txtsAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address line2") != null) ? true : false;
                    customeradd.txtsCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address City") != null) ? true : false;
                    customeradd.txtsState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address State") != null) ? true : false;
                    customeradd.txtsCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Country") != null) ? true : false;
                    customeradd.txtsZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Zip") != null) ? true : false;
                    customeradd.txtRegionship.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Region") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }


                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Tax Info") != null))
                {
                    customeradd.tabTaxInformationInfo.IsEnabled = true;
                    customeradd.txtVAT.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vat Number") != null) ? true : false;
                    customeradd.txtEIN.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add NTN number") != null) ? true : false;
                    customeradd.txtRegistrationTaxNumber.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Customer sale tax registration Number") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer tax info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {
                    customeradd.tabContactInfo.IsEnabled = true;
                   //customeradd.tabContactInfo.IsEnabled = true;
                    customeradd.txtfirstName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add First Name") != null) ? true : false;
                    customeradd.txtlastName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Last Name") != null) ? true : false;
                    customeradd.txtPhonenum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Default Phone Number") != null) ? true : false;
                    customeradd.txtFaxnum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add FAX Number") != null) ? true : false;
                    customeradd.txtEmail.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Default Email") != null) ? true : false;
                    customeradd.txtWebsite.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Website") != null) ? true : false;
                    customeradd.txtLink1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link1") != null) ? true : false;
                    customeradd.txtLink2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link2") != null) ? true : false;
                    customeradd.txtLink3.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link3") != null) ? true : false;

                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer contact info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Department Info") != null))
                {
                    customeradd.tabDepartmentInfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer department info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Company Info") != null))
                    customeradd.tabCompanynfo.IsEnabled = true;
                else
                    DXMessageBox.Show("Permission required to Add new Customer company info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                


                customeradd.ShowDialog();
                loadgrid();
                grdcutomercompanies.RefreshData();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        private void loadgrid()
        {

            Editit = 0;

            CustomerCompRepo compRepo = new CustomerCompRepo();

            EmployeeRepo employeeRepo = new EmployeeRepo();
            if (MainWindow.currentUserid == 0)
            {

                this.grdcutomercompanies.ItemsSource = compRepo.getAll();
                return;

            }

            List<CustomerCompany> customerCompanies = new List<CustomerCompany>();
            //var currentuser =employeeRepo.getuser(MainWindow.currentUserid);
            //var currentemployee = employeeRepo.GetEmployee(SystemLogic.currentUser.employeeId);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {

                customerCompanies = employeeRepo.GetAllCustomersByUserId(MainWindow.currentUserid);
                this.grdcutomercompanies.ItemsSource = customerCompanies;
            }
            else
            {
                customerCompanies = employeeRepo.GetActiveCustomersByUserId(MainWindow.currentUserid);
                this.grdcutomercompanies.ItemsSource = customerCompanies;
            }



        }

        private void loadContactPerson()
        {
            try
            {
                var custId = (grdcutomercompanies.SelectedItem as CustomerCompany).Id;
                var contactPerson = CustomerCompRepo.GetAllActiveContactPersonsByCustomer(custId);
                if (contactPerson != null)
                {
                    //grdContactPersonsGrid.ItemsSource = contactPerson;
                    grdContactPersonsList.ItemsSource = contactPerson;
                }
            }
            catch (Exception)
            {

       
            }
        }

        private void WinCustomerCenter_Loaded(object sender, RoutedEventArgs e)
        {
            //PopulateTransactionPanel();
            //loadContactPerson();
            treeViewTransactions.ItemsSource = SYSTEM_STATIC.statusSources;
            loadgrid();
            if (tabTransactions.IsSelected)
            {
                gridTransactions.Visibility = Visibility.Visible;
                gridCustomerinfo.Visibility = Visibility.Hidden;
                gridcustomertabss.Visibility = Visibility.Hidden;
                horSlider.Visibility = Visibility.Hidden;

            }
            else if (tabCustomers.IsSelected)
            {
                gridTransactions.Visibility = Visibility.Collapsed;
                gridCustomerinfo.Visibility = Visibility.Visible;
                gridcustomertabss.Visibility = Visibility.Visible;
                horSlider.Visibility = Visibility.Visible;
            }

        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            EditCustomer();
        }

        private void EditCustomer()
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Customer") != null))
            {
                frmCustomeradd customeradd = new frmCustomeradd();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a =>a.Name == "Edit Customer Info") != null))
                {
                    customeradd.tabCustomerInfo.IsEnabled = true;
                    customeradd.txtBussinesName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Company Name") != null) ? true : false;
                    customeradd.cmbIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Industry") != null) ? true : false;
                    customeradd.cmbBussinesType.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Business Type") != null) ? true : false;
                    customeradd.cmbCurrency.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Currency") != null) ? true : false;
                    customeradd.lookupCustomer.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Company Name") != null) ? true : false;
                    //billing
                    customeradd.txtAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address line1") != null) ? true : false;
                    customeradd.txtAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address line2") != null) ? true : false;
                    customeradd.txtCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address City") != null) ? true : false;
                    customeradd.txtState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address State") != null) ? true : false;
                    customeradd.txtCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Country") != null) ? true : false;
                    customeradd.txtZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Zip") != null) ? true : false;
                    customeradd.txtRegionbil.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Region") != null) ? true : false;
                    //Shipping
                    customeradd.txtsAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address line1") != null) ? true : false;
                    customeradd.txtsAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address line2") != null) ? true : false;
                    customeradd.txtsCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address City") != null) ? true : false;
                    customeradd.txtsState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address State") != null) ? true : false;
                    customeradd.txtsCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Country") != null) ? true : false;
                    customeradd.txtsZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Zip") != null) ? true : false;
                    customeradd.txtRegionship.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Region") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Tax Info") != null))
                {
                    customeradd.tabTaxInformationInfo.IsEnabled = true;
                    customeradd.txtVAT.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vat Number") != null) ? true : false;
                    customeradd.txtEIN.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit NTN number") != null) ? true : false;
                    customeradd.txtRegistrationTaxNumber.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Customer sale tax registration Number") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer tax info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Contact Info") != null))
                {
                    customeradd.tabContactInfo.IsEnabled = true;
                    customeradd.txtfirstName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit First Name") != null) ? true : false;
                    customeradd.txtlastName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Last Name") != null) ? true : false;
                    customeradd.txtPhonenum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Default Phone Number") != null) ? true : false;
                    customeradd.txtFaxnum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FAX Number") != null) ? true : false;
                    customeradd.txtEmail.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Default Email") != null) ? true : false;
                    customeradd.txtWebsite.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Website") != null) ? true : false;
                    customeradd.txtLink1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link1") != null) ? true : false;
                    customeradd.txtLink2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link2") != null) ? true : false;
                    customeradd.txtLink3.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link3") != null) ? true : false;

                    //customeradd.btnAddmorePhoneNum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit More Phone Number") != null) ? true : false;
                    //customeradd.btnAddmoreEmails.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit More Email") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer contact info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Department Info") != null))
                {
                    customeradd.tabDepartmentInfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer department info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Company Info") != null))
                {
                    customeradd.tabCompanynfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer company info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                Editit = 1;
                if (grdcutomercompanies.GetFocusedRowCellValue(grdcutomercompanies.Columns.GetColumnByFieldName("Id")) != null)
                {

                    customerid = (int)grdcutomercompanies.GetFocusedRowCellValue(grdcutomercompanies.Columns.GetColumnByFieldName("Id"));

                }
               
                customeradd.ShowDialog();
                loadgrid();
                grdcutomercompanies.RefreshData();

            }
            else
            {
                DXMessageBox.Show("Permission required to edit Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCompany();
        }


        private void tabTransactions_LostFocus(object sender, RoutedEventArgs e)
        {
            //

        }

        private void tabTransactions_GotFocus(object sender, RoutedEventArgs e)
        {
            //DateTime dateTime = System.DateTime.Now;


            if (gridTransactions.Children.Count == 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
            {
                SYSTEM_STATIC.gridTitle = "Inquiries";
                Procurementss.Inquiriess.ucInquiryGrid ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(true);
                gridTransactions.Children.Add(ucInquirygrid);
            }

            gridTransactions.Visibility = Visibility.Visible;
            gridCustomerinfo.Visibility = Visibility.Hidden;
            gridcustomertabss.Visibility = Visibility.Hidden;
            horSlider.Visibility = Visibility.Hidden;


        }


        private void tabCustomers_GotFocus(object sender, RoutedEventArgs e)
        {
            gridTransactions.Visibility = Visibility.Hidden;
            gridCustomerinfo.Visibility = Visibility.Visible;
            gridcustomertabss.Visibility = Visibility.Visible;
            horSlider.Visibility = Visibility.Visible;
        }


        private void mbtnnewtransaction_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, 0);
            procurmentPanel.Show();
        }

        private void btnEditTransaction_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (treeViewTransactions.SelectedItem != null)
            {

                var item = treeViewTransactions.SelectedItem;
                var type = item.GetType();
                string select;
                if (type.Name == "TreeItem")
                {
                    select = (treeViewTransactions.SelectedItem as TreeItem).name.ToString();
                }
                else
                {
                    select = (treeViewTransactions.SelectedItem as cmbitem).description.ToString();
                }

                switch (select)
                {
                    case "Inquiries":
                        {
                            if (ucInquirygrid.grdinquiry.GetFocusedRowCellValue(ucInquirygrid.grdinquiry.Columns.GetColumnByFieldName("Id")) != null)
                            {
                                Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, (int)ucInquirygrid.grdinquiry.GetFocusedRowCellValue(ucInquirygrid.grdinquiry.Columns.GetColumnByFieldName("Id")));
                                procurmentPanele.Show();

                            }
                            break;
                        }
                    case "Offers":

                        Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Offer, (int)ucOffergrid.grdoffer.GetFocusedRowCellValue(ucOffergrid.grdoffer.Columns.GetColumnByFieldName("Id")));
                        procurmentPanel.Show();

                        break;
                    case "Sale Orders":

                        Procurementss.frmProcurmentPanel procurmentPane = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (int)saleOrderGrid.grdsaleOrder.GetFocusedRowCellValue(saleOrderGrid.grdsaleOrder.Columns.GetColumnByFieldName("Id")));
                        procurmentPane.Show();
                        break;
                        //case "Purchase Orders":
                        //    Procurementss.frmProcurmentPanel procurmentPan = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)purchaseOrderGrid.grdpurchaseOrder.GetFocusedRowCellValue(purchaseOrderGrid.grdpurchaseOrder.Columns.GetColumnByFieldName("Id")));
                        //    procurmentPan.Show();
                        //    break;


                }

            }
        }

        private void btnnewoffers_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Offer, 0);
            procurmentPanel.Show();
        }

        private void btnnewPO_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, 0);
            procurmentPanel.Show();

        }

        private void grdcutomercompanies_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            CustomerCompRepo compRepo = new CustomerCompRepo();

            if (grdcutomercompanies.SelectedItem != null)
                grdDepartment.ItemsSource = compRepo.GetCustomerDepartments((grdcutomercompanies.SelectedItem as CustomerCompany).Id);
        }

        private void WinCustomerCenter_Unloaded(object sender, RoutedEventArgs e)
        {
            Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
            Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 0;

            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
            Procurementss.Offerss.ucOfferGrid.statusId = 0;
            Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;

        }

        private void BtnPrint_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var gridObject = gridTransactions.Children;

        }



        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var item = treeViewTransactions.SelectedItem;
            var type = item.GetType();

            if (type.Name == "TreeItem")
            {
                if (gridTransactions != null)
                    gridTransactions.Children.Clear();
                TreeItem treeItem = (TreeItem)treeViewTransactions.SelectedItem;
                SYSTEM_STATIC.gridTitle = treeItem.name;

            }
            else if (type.Name == "cmbitem")
            {
                if (gridTransactions != null)
                    gridTransactions.Children.Clear();
                cmbitem cmbitem = (cmbitem)treeViewTransactions.SelectedItem;
                SYSTEM_STATIC.gridTitle = cmbitem.description + " (" + cmbitem.name + ")";
                if (cmbitem.id == 0)
                {
                    LoadTransactionGrid(cmbitem.name);
                }
                switch (cmbitem.description)
                {
                    case "Inquiries":
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = cmbitem.id;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                            break;
                        }
                    case "Offers":

                        Procurementss.Offerss.ucOfferGrid.statusId = cmbitem.id;
                        ucOffergrid = new Procurementss.Offerss.ucOfferGrid();
                        gridTransactions.Children.Add(ucOffergrid);
                        break;
                    case "Module Contract":

                        Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = cmbitem.id;
                        ucModuleContractgrid = new Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                        gridTransactions.Children.Add(ucModuleContractgrid);
                        break;
                    case "Sale Orders":
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = cmbitem.id;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                        break;
                    case "Memorandum Sales":
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = cmbitem.id;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                        break;
                    case "Sale Invoices":
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = cmbitem.id;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                        break;

                    case "Purchase Orders":
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = cmbitem.id;
                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                        break;
                    case "Bills":
                        Procurementss.Billss.ucBillGrid.statusId = cmbitem.id;
                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                        break;
                    case "Purchase Invoices":
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = cmbitem.id;
                        ucPIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(ucPIGrid);
                        break;

                }
            }
           
        }
        /// <summary>
        /// Load Transaction Grids, Inquiries, Offers, SaleOrder, Memorandum Sale, Purchase Order, Sale Invoice 
        /// Only Select Open /Closed or All Orders
        /// </summary>
        /// <param name="Name">string</param>
        private void LoadTransactionGrid(string Name)
        {
            SYSTEM_STATIC.gridTitle = Name;
            switch (Name)
            {
                case "Inquiries":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 0;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;
                    }

                case "Inquiries(Open)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 1;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;
                    }
                case "Inquiries(Closed)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 2;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;

                    }
                case "Inquiries(PendingForApproval)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 3;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;
                    }
                case "PendingForReapproval":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 4;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;
                    }
                case "Inquiries(PendingForClosing)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                        {
                            Procurementss.Inquiriess.ucInquiryGrid.statusid = 0;
                            Procurementss.Inquiriess.ucInquiryGrid.AllActive = 5;
                            ucInquirygrid = new Procurementss.Inquiriess.ucInquiryGrid(false);
                            gridTransactions.Children.Add(ucInquirygrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Inquiries");
                        }
                        break;
                    }

                case "Offers":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                    {
                        Procurementss.Offerss.ucOfferGrid.statusId = 0;
                        Procurementss.Offerss.ucOfferGrid.AllActive = 0;
                        ucOffergrid = new Procurementss.Offerss.ucOfferGrid();
                        gridTransactions.Children.Add(ucOffergrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Offers");
                    }
                    break;
                case "Offers(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                    {
                        Procurementss.Offerss.ucOfferGrid.statusId = 0;
                        Procurementss.Offerss.ucOfferGrid.AllActive = 1;
                        ucOffergrid = new Procurementss.Offerss.ucOfferGrid();
                        gridTransactions.Children.Add(ucOffergrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Offers");
                    }

                    break;
                case "Offers(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                    {
                        Procurementss.Offerss.ucOfferGrid.statusId = 0;
                        Procurementss.Offerss.ucOfferGrid.AllActive = 2;
                        ucOffergrid = new Procurementss.Offerss.ucOfferGrid();
                        gridTransactions.Children.Add(ucOffergrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Offers");
                    }
                    break;
                case "Module Contract":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContract") != null)
                    {
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = 0;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.AllActive = 0;
                        ucModulegrid = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                        gridTransactions.Children.Add(ucModulegrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Module Contract");
                    }
                    break;
                case "Module Contract(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContract") != null)
                    {
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = 0;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.AllActive = 1;
                        ucModulegrid = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                        gridTransactions.Children.Add(ucModulegrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Module Contract");
                    }

                    break;
                case "Module Contract(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContract") != null)
                    {
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = 0;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.AllActive = 2;
                        ucModulegrid = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                        gridTransactions.Children.Add(ucModulegrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Module Contract");
                    }
                    break;

                case "Module Contract(PendingForApproval)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContract") != null)
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = 0;
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.AllActive = 3;
                            ucModulegrid = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                            gridTransactions.Children.Add(ucModulegrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Module Contract");
                        }
                        break;
                    }

                case "Module Contract(PendingForClosing)":
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ModuleContract") != null)
                        {
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.statusId = 0;
                            ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid.AllActive = 4;
                            ucModulegrid = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractGrid();
                            gridTransactions.Children.Add(ucModulegrid);
                        }
                        else
                        {
                            DXMessageBox.Show("You need permission to view Module Contract");
                        }
                        break;
                    }

                case "Sale Orders":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                    {
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
                        Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 0;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Orders");
                    }
                    break;
                case "Sale Orders(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                    {
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
                        Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 1;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Orders");
                    }
                    break;
                case "Sale Orders(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                    {
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
                        Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 2;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Orders");
                    }
                    break;

                case "Sale Orders(PendingForApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                    {
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
                        Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 3;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Orders");
                    }
                    break;
                case "Sale Orders(PendingForClosing)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                    {
                        Procurementss.SaleOrderss.ucSaleOrderGrid.statusId = 0;
                        Procurementss.SaleOrderss.ucSaleOrderGrid.AllActive = 4;
                        saleOrderGrid = new Procurementss.SaleOrderss.ucSaleOrderGrid();
                        gridTransactions.Children.Add(saleOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Orders");
                    }
                    break;

                //sale_invoice start************************************************************************************************ AllActive is 0 to 4

                case "Sale Invoices":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                    {
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = 0;
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.AllActive = 0;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Invoices");
                    }
                    break;
                case "Sale Invoices(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                    {
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = 0;
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.AllActive = 1;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Invoices");
                    }
                    break;
                case "Sale Invoices(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                    {
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = 0;
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.AllActive = 2;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Invoices");

                    }
                    break;
                case "Sale Invoices(PendingForApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                    {
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = 0;
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.AllActive = 3;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Invoices");
                    }
                    break;
                case "Sale Invoices(PendingForClosing)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                    {
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.statusId = 0;
                        Procurementss.SaleInvoicess.ucSaleInvoiceGrid.AllActive = 4;
                        saleInvoiceGrid = new Procurementss.SaleInvoicess.ucSaleInvoiceGrid();
                        gridTransactions.Children.Add(saleInvoiceGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Sale Invoices");
                    }
                    break;

                //Memorandum Sales start************************************************************************************************

                case "Memorandum Sales":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Memorandum Sales") != null)
                    {
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = 0;
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.AllActive = 0;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Memorandum Sales");
                    }
                    break;
                case "Memorandum Sales(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Memorandum Sales") != null)
                    {
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = 0;
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.AllActive = 1;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Memorandum Sales");
                    }
                    break;
                case "Memorandum Sales(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Memorandum Sales") != null)
                    {
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = 0;
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.AllActive = 2;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Memorandum Sales");
                    }
                    break;
                case "Memorandum Sales(PendingForApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Memorandum Sales") != null)
                    {
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = 0;
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.AllActive = 3;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Memorandum Sales");
                    }
                    break;
                case "Memorandum Sales(PendingForClosing)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Memorandum Sales") != null)
                    {
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.statusId = 0;
                        Procurementss.MemorandumSaless.ucMemorandumSaleGrid.AllActive = 4;
                        memorandumSaleGrid = new Procurementss.MemorandumSaless.ucMemorandumSaleGrid();
                        gridTransactions.Children.Add(memorandumSaleGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Memorandum Sales");
                    }
                    break;

                //Purchase Orders start************************************************************************************************

                case "Purchase Orders":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 0;
                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }

                    break;
                case "Purchase Orders(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 1;

                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }
                    break;
                case "Purchase Orders(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 2;

                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }
                    break;
                case "Purchase Orders(PendingForApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 3;

                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }
                    break;
                case "Purchase Orders(PendingForClosing)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 4;

                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }
                    break;
                case "Purchase Orders(PendingForReApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                    {
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.statusId = 0;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderGrid.AllActive = 5;

                        purchaseOrderGrid = new Procurementss.PurchaseOrderss.ucPurchaseOrderGrid();
                        gridTransactions.Children.Add(purchaseOrderGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Orders");
                    }
                    break;
                   



                case "Bills":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null)
                    {
                        Procurementss.Billss.ucBillGrid.statusId = 0;
                        Procurementss.Billss.ucBillGrid.AllActive = 0;
                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view List of Bills");
                    }
                    break;
                case "Bills(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null)
                    {
                        Procurementss.Billss.ucBillGrid.statusId = 0;
                        Procurementss.Billss.ucBillGrid.AllActive = 1;
                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view List of Bills");
                    }

                    break;
                case "Bills(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bills") != null))
                    {
                        Procurementss.Billss.ucBillGrid.statusId = 0;
                        Procurementss.Billss.ucBillGrid.AllActive = 2;

                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view List of closed Bills");
                    }
                    break;
                case "Bills(PendingForApproval)":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null)
                    {
                        Procurementss.Billss.ucBillGrid.statusId = 0;
                        Procurementss.Billss.ucBillGrid.AllActive = 3;
                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view List of Bills");
                    }

                    break;
                case "Bills(PendingForClosing)":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null)
                    {
                        Procurementss.Billss.ucBillGrid.statusId = 0;
                        Procurementss.Billss.ucBillGrid.AllActive = 4;
                        billGrid = new Procurementss.Billss.ucBillGrid();
                        gridTransactions.Children.Add(billGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view List of Bills");
                    }

                    break;

                case "jv":

                    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    ChartofAccounts.UserControls.ucTransactions.AllActive = 0;
                    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    gridTransactions.Children.Add(jvGrid);

                    break;
                case "jv(Open)":

                    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    ChartofAccounts.UserControls.ucTransactions.AllActive = 1;
                    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    gridTransactions.Children.Add(jvGrid);


                    break;
                case "jv(Closed)":

                    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    ChartofAccounts.UserControls.ucTransactions.AllActive = 2;
                    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    gridTransactions.Children.Add(jvGrid);

                    break;
                case "jv(PendingForApproval)":


                    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    ChartofAccounts.UserControls.ucTransactions.AllActive = 3;
                    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    gridTransactions.Children.Add(jvGrid);



                    break;
                case "jv(PendingForClosing)":
                    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    ChartofAccounts.UserControls.ucTransactions.AllActive = 4;
                    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    gridTransactions.Children.Add(jvGrid);


                    break;
                case "Purchase Invoices":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                    {
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = 0;
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.AllActive = 0;
                        pIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(pIGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Invoices");
                    }
                    break;
                case "Purchase Invoices(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                    {
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = 0;
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.AllActive = 1;
                        pIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(pIGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Invoices");
                    }
                    break;
                case "Purchase Invoices(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                    {
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = 0;
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.AllActive = 2;
                        pIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(pIGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Invoices");

                    }
                    break;
                case "Purchase Invoices(PendingForApproval)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                    {
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = 0;
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.AllActive = 3;
                        pIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(pIGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Invoices");
                    }
                    break;
                case "Purchase Invoices(PendingForClosing)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                    {
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.statusId = 0;
                        Procurementss.PurchaseInvoice.UserControls.ucPIGrid.AllActive = 4;
                        pIGrid = new Procurementss.PurchaseInvoice.UserControls.ucPIGrid();
                        gridTransactions.Children.Add(pIGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Purchase Invoices");
                    }
                    break;


                    //case "employee":
                    //    Employee.ucEmployeeInfo.
                    //   // Employee.UserControls.ucTransactions.statusId = 0;
                    //    ChartofAccounts.UserControls.ucTransactions.AllActive = 0;
                    //    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    //    gridTransactions.Children.Add(jvGrid);

                    //    break;
                    //case "employee(Open)":

                    //    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    //    ChartofAccounts.UserControls.ucTransactions.AllActive = 1;
                    //    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    //    gridTransactions.Children.Add(jvGrid);


                    //    break;
                    //case "employee(Closed)":

                    //    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    //    ChartofAccounts.UserControls.ucTransactions.AllActive = 2;
                    //    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    //    gridTransactions.Children.Add(jvGrid);

                    //    break;
                    //case "employee(PendingForApproval)":


                    //    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    //    ChartofAccounts.UserControls.ucTransactions.AllActive = 3;
                    //    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    //    gridTransactions.Children.Add(jvGrid);



                    //    break;
                    //case "employee(PendingForClosing)":
                    //    ChartofAccounts.UserControls.ucTransactions.statusId = 0;
                    //    ChartofAccounts.UserControls.ucTransactions.AllActive = 4;
                    //    jvGrid = new ChartofAccounts.UserControls.ucTransactions();
                    //    gridTransactions.Children.Add(jvGrid);


                    //    break;
                    case "Inventory Adjustment":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inventory Adjustment") != null)
                    {
                        ucInventoryAdjustmentGrid.statusId = 0;
                        ucInventoryAdjustmentGrid.AllActive = 0;
                        iAGrid = new ucInventoryAdjustmentGrid();
                        gridTransactions.Children.Add(iAGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Inventory Adjustments");
                    }
                    break;
                case "Inventory Adjustment(Open)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inventory Adjustment") != null)
                    {
                        ucInventoryAdjustmentGrid.statusId = 0;
                        ucInventoryAdjustmentGrid.AllActive = 1;
                        iAGrid = new ucInventoryAdjustmentGrid();
                        gridTransactions.Children.Add(iAGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Inventory Adjustments");
                    }
                    break;
                case "Inventory Adjustment(Closed)":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inventory Adjustment") != null)
                    {
                        ucInventoryAdjustmentGrid.statusId = 0;
                        ucInventoryAdjustmentGrid.AllActive = 2;
                        iAGrid = new ucInventoryAdjustmentGrid();
                        gridTransactions.Children.Add(iAGrid);
                    }
                    else
                    {
                        DXMessageBox.Show("You need permission to view Inventory Adjustments");

                    }
                    break;


            }
        }

        private void btnnewSO_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, 0);
            procurmentPanel.Show();
        }

        private void MbtnExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            colSide.Width = new GridLength(250, GridUnitType.Pixel);
        }
        private void MbtnCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            colSide.Width = new GridLength(10, GridUnitType.Pixel);
        }

        private void TreeListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditCustomer();
        }

        private void Grdcutomercompanies_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "allDepts")

            {
                CustomerCompany customer = grdcutomercompanies.GetRow(e.ListSourceRowIndex) as CustomerCompany;
                if (customer.departments != null)
                {

                    //var departments = user1.employee.departments; //*/e.GetListSourceFieldValue("employee.departments") as List<Department>;
                    string depts = "";
                    //if (departments != null)
                    foreach (var department in customer.departments)
                    {
                        if (department.parentDepartment != null)
                        {
                            depts += department.parentDepartment.DeptName + " " + department.DeptName + " , ";
                        }
                        else
                            depts += department.DeptName + " , ";
                    }
                    e.Value = depts;
                }

            }
        }

        private void Add_New_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {

                    var selectedItem = grdcutomercompanies.SelectedItem as CustomerCompany;
                    ucContactPerson = new ucContactPerson();
                    ucContactPerson.ContactPersonId = selectedItem.Id;
                   
                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = false;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {

             
            }
        }

      

        private void Edit_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Update Contact Person Detail") != null))
                {
                    //var selectedContact = grdContactPersonsGrid.SelectedItem as ContactPerson;
                    var selectedContact = grdContactPersonsList.SelectedItem as ContactPerson;
                    ucContactPerson = new ucContactPerson();
                    if (selectedContact != null)
                    {
                        ucContactPerson.ContactPersonId = selectedContact.Id;
                    }
                    else
                    {
                        return;
                    }
                 
                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = true;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Update Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {

               
            }
           
               
        }


     
       

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadContactPerson();
        }

        private void GrdContactPerson_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }

        private void Grdcutomercompanies_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
           
            if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "View Contact Detail Info") != null))
            {
                tabContact.IsEnabled = true;
                loadContactPerson();
            }
            else
            { 
                return;
            }

        }

        private void DXTabItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DXTabItem_MouseUp(object sender, MouseButtonEventArgs e)
        {

           

        }

        private void GrdContactPersonsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Update Contact Person Detail") != null))
                {
                    //var selectedContact = grdContactPersonsGrid.SelectedItem as ContactPerson;
                    var selectedContact = grdContactPersonsList.SelectedItem as ContactPerson;
                    ucContactPerson = new ucContactPerson();
                    if (selectedContact != null)
                    {
                        ucContactPerson.ContactPersonId = selectedContact.Id;
                    }
                    else
                    {
                        return;
                    }

                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = true;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Update Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {
                

            }
        }
    }
}
