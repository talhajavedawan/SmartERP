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
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Config;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid.TreeList;

namespace ZAS_ERP.Companiess
{
   
    /// <summary>
    /// Interaction logic for frmDepartmentAdd.xaml
    /// </summary>
    /// 

    public partial class frmDepartmentAdd : DevExpress.Xpf.Core.ThemedWindow
    {
        DepartmentRepo repo = new DepartmentRepo();
        Department department = new Department();
        ChartofAccount receivableAccount = new ChartofAccount();
        ChartofAccount payableAccount = new ChartofAccount();
        List<Company> companies = new List<Company>();  
        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>(); 
        List<ERP_BL.Databases.Employee> selectedUsers = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> departmentEmployes = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<CustomerCompany> allCustomers = new List<CustomerCompany>();
        List<CustomerCompany> selectedCustomers = new List<CustomerCompany>(); 
        List<Vendor> allVendors = new List<Vendor>();
        List<Vendor> selectedVendors = new List<Vendor>();
        List<Principal> allPrinciples = new List<Principal>();
        List<Principal> selectedPrinciples = new List<Principal>();
        public string charonly { get; set; }
        
        public frmDepartmentAdd()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            gridCompany.SelectionChanged += OnGridSelectionChanged;
        }
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                gridCompany.SelectItem(e.Node.RowHandle);
            else
                gridCompany.UnselectItem(e.Node.RowHandle);
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)gridCompany.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                    {
                        node.IsChecked = true;
                        var company = node.Content as Company;
                        companies.Add(company);

                    }

                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                    {
                        node.IsChecked = false;
                        var company = node.Content as Company;
                        companies.Remove(company);
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = gridCompany.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
        
        private void btnDeptSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtdepartmentname.Text == "")
                {
                    txtdepartmentname.Focus();
                    MessageBox.Show("Please Enter a Department Name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                department.isActive = (chkisActive.IsChecked == true) ? true : false;
                department.applyMERasSER = (chkapplyMERasSER.IsChecked == true) ? true : false;
                department.Abbrivation = txtdepartmentabr.Text.Trim();
                //If department is amnagerial 4 letters code is added at the end of department code

                if(chkisLinkAble.IsChecked==true)
                    department.isLinkable = true;
                else
                    department.isLinkable = false;

                if (chkIsHRM.IsChecked == true)
                {
                    if (!txtdepartmentcode.Text.Trim().Contains("--MGMT"))
                    {

                        var mgmtCode = "--MGMT";
                        department.Code = txtdepartmentcode.Text.Trim() + mgmtCode;
                    }
                }
                else if (chkIsHRM.IsChecked == false)
                    department.Code = txtdepartmentcode.Text.Trim();

                if(chkIsAdminBillType.IsChecked == true)
                    department.IsAdminBillType = true;
                else
                    department.IsAdminBillType = false;

                if (chkIsManagerial.IsChecked == true)
                    department.IsManagerial = true;
                else
                    department.IsManagerial = false;

                if (chkProcurmentModule.IsChecked == true)
                    department.IsProcurementType = true;
                else
                    department.IsProcurementType = false;

                if (chkInquiry.IsChecked == true)
                    department.IsInquiryType = true;
                else
                    department.IsInquiryType = false;

                if (chkOffer.IsChecked == true)
                    department.IsOfferType = true;
                else
                    department.IsOfferType = false;

                if (chkSaleOrder.IsChecked == true)
                    department.IsSaleOrderType = true;
                else
                    department.IsSaleOrderType = false;

                if (chkSaleInvoice.IsChecked == true)
                    department.IsSaleInvoiceType = true;
                else
                    department.IsSaleInvoiceType = false;

                if (chkSaleReceipt.IsChecked == true)
                    department.IsSaleReceiptType = true;
                else
                    department.IsSaleReceiptType = false;

                if (chkPurchaseOrder.IsChecked == true)
                    department.IsPurchaseOrderType = true;
                else
                    department.IsPurchaseOrderType = false;

                if (chkPurchaseInvoice.IsChecked == true)
                    department.IsPurchaseInvoiceType = true;
                else
                    department.IsPurchaseInvoiceType = false;

                if (chkPayments.IsChecked == true)
                    department.IsPaymentType = true;
                else
                    department.IsPaymentType = false;

                if (chkVendorBillType.IsChecked == true)
                    department.IsVendorBillType = true;
                else
                    department.IsVendorBillType = false;

                if (chkInventoryType.IsChecked == true)
                    department.IsInventoryType = true;
                else
                    department.IsInventoryType = false;


                if (chkInterBankTransfer.IsChecked == true)
                    department.IsInterBankTransferType = true;
                else
                    department.IsInterBankTransferType = false;

                if (chkInterCompanyBankTransfer.IsChecked == true)
                    department.IsInterCompTransferType = true;
                else
                    department.IsInterCompTransferType = false;

                if (chkLoansAdvancesTransfer.IsChecked == true)
                {
                    department.IsLoansAdvancesType = true;
                }
                else
                {
                    department.IsLoansAdvancesType = false;
                }
                if (chkTaskType.IsChecked == true)
                {
                    department.IsTaskType = true;
                }
                else
                {
                    department.IsTaskType = false;
                }
                if (chkTravelingRecordType.IsChecked == true)
                {
                    department.IsTravelingRecordType = true;
                }
                else
                {
                    department.IsTravelingRecordType = false;
                }
                if(lookupLevel.SelectedIndex>-1)
                {
                    department.LevelID = (lookupLevel.SelectedItem as ERP_BL.Databases.DepartmentLevel).Id;                       
                }

                if (chkAssets.IsChecked == true)
                    department.IsAssetType = true;
                else
                    department.IsAssetType = false;

                if (chkRentalContract.IsChecked == true)
                    department.IsRentalContractType = true;
                else
                    department.IsRentalContractType = false;

                if (chkRentalOrder.IsChecked == true)
                    department.IsRentalOrderType = true;
                else
                    department.IsRentalOrderType = false;

                if (chkRentalInvoice.IsChecked == true)
                    department.IsRentalInvoiceType = true;
                else
                    department.IsRentalInvoiceType = false;

                if (chkRentalReceipt.IsChecked == true)
                    department.IsRentalReceiptType = true;
                else
                    department.IsRentalReceiptType = false;

                if (chkDocument.IsChecked == true)
                    department.IsDocumentType = true;
                else
                    department.IsDocumentType = false;

                department.DeptName = txtdepartmentname.Text.Trim();  
                if(departmentEmployes.Count != 0)
                {
                    department.employees = new List<ERP_BL.Databases.Employee>();
                    foreach (var emp in departmentEmployes)
                    {
                        if (!department.employees.Contains(emp))
                        {
                            department.employees.Add(emp);
                        }
                    }
                }
                if (gridCompany.SelectedItems.Count != 0)
                    {
                        department.companies = new List<Company>();

                        foreach (Company comp in companies)
                        {
                            if (!department.companies.Contains(comp))
                            {
                                department.companies.Add(comp);
                            }
                        }
                    }
                if (selectedCustomers.Count != 0)
                {
                    department.customers = new List<CustomerCompany>();
                    foreach (var _cus in selectedCustomers)
                    {
                        if (!department.customers.Contains(_cus))
                        {
                            department.customers.Add(_cus);
                        }
                    }
                }
                else
                {
                    department.customers = new List<CustomerCompany>();
                }
                if (selectedVendors.Count != 0)
                {
                    department.Vendors = new List<Vendor>();
                    foreach (var _ven in selectedVendors)
                    {
                        if (!department.Vendors.Contains(_ven))
                        {
                            department.Vendors.Add(_ven);
                        }
                    }
                }
                else
                {
                    department.Vendors = new List<Vendor>();
                }
                if (selectedPrinciples.Count != 0)
                {
                    department.Principals = new List<Principal>();
                    foreach (var _prin in selectedPrinciples)
                    {
                        if (!department.Principals.Contains(_prin))
                        {
                            department.Principals.Add(_prin);
                        }
                    }
                }
                else
                {
                    department.Principals = new List<Principal>();
                }
                if (chkisSubsidaiary.IsChecked == true && dept.Id != 0)
                {
                    department.IsSubsidary = true;
                    department.ParentID = dept.Id;
                }
                else
                {
                    department.IsSubsidary = false;
                    department.ParentID = null;
                    department.parentDepartment = null;
                }
                if (chkIsParent.IsChecked == true)
                {
                    department.IsParent = true;
                }
                else
                {
                    department.IsParent = false;
                }
                if (isReceivableAccount.IsChecked != false)
                {
                    if (lookupReceivableAccounts.SelectedIndex != -1)
                        department.chartofAccountId = receivableAccount.Id;
                    else
                    {
                        department.chartofAccountId = null;
                        department.ChartofAccount = null;
                    }
                }
                else
                {
                    department.ChartofAccount = null;
                    department.chartofAccountId = null;
                }
                if (isPayableAccount.IsChecked != false)
                {
                    if (lookupPayableAccounts.SelectedIndex != -1)
                        department.accountPayableId = (lookupPayableAccounts.SelectedItem as ChartofAccount).Id;
                    else
                    {
                        department.accountPayableId = null;
                        department.AccountPayable = null;
                    }
                }
                else
                {
                    department.AccountPayable = null;
                    department.accountPayableId = null;
                }
                if (frmcompanyCenter.editdept == 1)
                {
                    repo.updateDepartment(department);
                    MessageBox.Show("Department Updated Succesfully");
                }
                else
                {
                    department.Timestamp = dtedeptcreation.DateTime;
                    repo.addDepartment(department);
                    MessageBox.Show("Department added Succesfully");
                }
                
              
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void windowDeptadd_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new DepartmentRepo();
            allEmployees = (List<ERP_BL.Databases.Employee>)repo.GetAllEmployees();
            allCustomers = (List<CustomerCompany>)repo.GetAllCustomers();
            allVendors = repo.GetAllVendors();
            allPrinciples = repo.GetAllPrinciple();
            gridCompany.ItemsSource = repo.getAllCompanies();
            chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Department as InActive") != null) ? true : false;
            LoadDepartmentLevels();
            LoadDepartmentData();
            LoadChartofAccounts();
           
        }
        public void LoadDepartmentLevels()
        {
            lookupLevel.ItemsSource= repo.getAllDepartmentLevels();
        }
        private void LoadChartofAccounts()
        {
            ChartofAccountsRepo repo = new ChartofAccountsRepo();
            var chartofAccounts=repo.GetAllApprovedAcccountforDepartment(SYSTEM_STATIC.currentUser.id);
            lookupReceivableAccounts.ItemsSource = chartofAccounts;
            lookupPayableAccounts.ItemsSource = chartofAccounts;
        }
        private void LoadDepartmentData()
        {
            if (frmcompanyCenter.editdept == 1)
            {
                windowDeptadd.Title = "Edit Department"; 
                
                windowDeptadd.HorizontalAlignment = HorizontalAlignment.Center;
                department = repo.GetDepartment(frmcompanyCenter.departmentId);
                string fullName = "";
                var node = department;
                while (node != null)
                {
                    if (fullName.Length != 0)
                        fullName = " | " + fullName;
                    fullName = node.DeptName + fullName;

                    node = node.parentDepartment;
                }
                lblDeptName.Content = fullName;
                chkisActive.IsChecked = department.isActive;
                chkapplyMERasSER.IsChecked = department.applyMERasSER;
                txtdepartmentabr.Text = department.Abbrivation;
                txtdepartmentcode.Text = department.Code;
                txtdepartmentname.Text = department.DeptName;
                dtedeptcreation.DateTime = department.Timestamp;
                if(department.LevelID!=null)
                {
                    lookupLevel.Text = department.departmentLevel.Title;
                }

                if (department.Code.EndsWith("--MGMT"))
                {
                    chkIsHRM.IsChecked = true;
                }

                if (department.IsAdminBillType == true)
                    chkIsAdminBillType.IsChecked = true;

                if (department.IsManagerial == true)
                    chkIsManagerial.IsChecked = true;

                if (department.IsProcurementType == true)
                    chkProcurmentModule.IsChecked = true;

                if (department.IsInquiryType == true)
                    chkInquiry.IsChecked = true;
                else
                    chkInquiry.IsChecked = false;
                if (department.IsOfferType == true)
                    chkOffer.IsChecked = true;
                else
                    chkOffer.IsChecked = false;
                if (department.IsSaleOrderType == true)
                    chkSaleOrder.IsChecked = true;
                else
                    chkSaleOrder.IsChecked = false;
                if (department.IsSaleInvoiceType == true)
                    chkSaleInvoice.IsChecked = true;
                else
                    chkSaleInvoice.IsChecked = false;
                if (department.IsSaleReceiptType == true)
                    chkSaleReceipt.IsChecked = true;
                else
                    chkSaleReceipt.IsChecked = false;
                if (department.IsPurchaseOrderType == true)
                    chkPurchaseOrder.IsChecked = true;
                else
                    chkPurchaseOrder.IsChecked = false;
                if (department.IsPurchaseInvoiceType == true)
                    chkPurchaseInvoice.IsChecked = true;
                else
                    chkPurchaseInvoice.IsChecked = false;
                if (department.IsPaymentType == true)
                    chkPayments.IsChecked = true;
                else
                    chkPayments.IsChecked = false;


                if (department.IsInventoryType == true)
                    chkInventoryType.IsChecked = true;

                if (department.IsVendorBillType == true)
                    chkVendorBillType.IsChecked = true;

                if (department.IsInterBankTransferType == true)
                    chkInterBankTransfer.IsChecked = true;

                if (department.IsInterCompTransferType == true)
                    chkInterCompanyBankTransfer.IsChecked = true;

                if (department.IsLoansAdvancesType == true)
                    chkLoansAdvancesTransfer.IsChecked = true;

                if (department.IsTaskType == true)
                    chkTaskType.IsChecked = true;

                if (department.IsTravelingRecordType == true)
                    chkTravelingRecordType.IsChecked = true;


                if (department.IsAssetType == true)
                    chkAssets.IsChecked = true;

                if (department.IsRentalContractType == true)
                    chkRentalContract.IsChecked = true;
                else
                    chkRentalContract.IsChecked = false;

                if (department.IsRentalOrderType == true)
                    chkRentalOrder.IsChecked = true;
                else
                    chkRentalOrder.IsChecked = false;

                if (department.IsRentalInvoiceType == true)
                    chkRentalInvoice.IsChecked = true;
                else
                    chkRentalInvoice.IsChecked = false;

                if (department.IsRentalReceiptType == true)
                    chkRentalReceipt.IsChecked = true;
                else
                    chkRentalReceipt.IsChecked = false;

                if (department.IsDocumentType == true)
                    chkDocument.IsChecked = true;
                else
                    chkDocument.IsChecked = false;

                if (department.IsSubsidary == true)
                {// set selected row  in a grid for parent company
                    chkisSubsidaiary.IsChecked = true;
                    if (department.ParentID != null)
                    {
                        dept = department.parentDepartment;
                        //lookupParentDept.SelectedItem = lookupParentDept.GetItemByKeyValue(department.parentDepartment);
                        lookupParentDept.Text = department.parentDepartment.DeptName;
                    }

                }
                else
                {
                    chkisSubsidaiary.IsChecked = false;
                }

                if (department.IsParent == true)
                {
                    chkIsParent.IsChecked = true;
                }
                else
                {
                    chkIsParent.IsChecked = false;
                }

                //foreach (var item in department.employees)
                //{
                // grdEmployee.SelectItem(grdEmployee.FindRowByValue(grdEmployee.Columns.GetColumnByFieldName("EmpId"), item.EmpId));

                foreach (var rrr in department.employees)
                    {
                        departmentEmployes.Add(rrr);
                        //if (departmentEmployes.Contains(rrr))
                        //{
                        //    employes.Remove(rrr);
                        //    selectedUsers.Add(rrr);
                        //}
                    }
                //}
                //grdEmployee.ItemsSource = null;
                //grdEmployee.ItemsSource = employes;
                grdcntrlSelectedEmployee.ItemsSource = null;
                grdcntrlSelectedEmployee.ItemsSource = departmentEmployes;
                foreach (var rrr in departmentEmployes)
                {

                    if (allEmployees.Contains(rrr))
                    {
                        allEmployees.Remove(rrr);
                        
                    }
                }
                grdEmployee.ItemsSource = null;
                grdEmployee.ItemsSource = allEmployees;
                
                foreach (var _customer in department.customers)
                {
                    selectedCustomers.Add(_customer);
                }
                foreach(var _selectedCustomer in selectedCustomers.ToList())
                {
                    if (allCustomers.Contains(_selectedCustomer))
                    {
                        allCustomers.Remove(_selectedCustomer);
                        //var v = _selectedCustomer.parentCompany;
                        //while (v != null)
                        //{

                        //    if (!allCustomers.Contains(_selectedCustomer))
                        //        allCustomers.Add(_selectedCustomer);
                        //    selectedCustomers.Remove(_selectedCustomer);
                        //    //if (!allCustomers.Contains(v))
                        //    //    allCustomers.Add(v);
                        //    var findParetsss = selectedCustomers.Find(x => x.parentCompany == v);
                        //    if (findParetsss == null)
                        //    {
                        //        allCustomers.Remove(v);
                        //    }
                        //    v = v.parentCompany;

                        //}
                        var findParet = allCustomers.Find(x => x.ParentID == _selectedCustomer.Id);
                        if (findParet != null)
                        {

                            //if(!allCustomers.Contains(findParet))
                            allCustomers.Add(_selectedCustomer);
                        }

                    }
                }
                grdCustomers.ItemsSource = null;
                grdCustomers.ItemsSource = allCustomers;
                grdCustomersSelected.ItemsSource = null;
                grdCustomersSelected.ItemsSource = selectedCustomers;

                foreach (var _vendor in department.Vendors)
                {
                    selectedVendors.Add(_vendor);
                }
                foreach (var _selectedVendor in selectedVendors.ToList())
                {
                    if (allVendors.Contains(_selectedVendor))
                    {
                        allVendors.Remove(_selectedVendor);
                        var findParet = allVendors.Find(x => x.ParentID == _selectedVendor.Id);
                        if (findParet != null)
                        {
                            allVendors.Add(_selectedVendor);
                        }
                    }
                }
                grdVendorsSelected.ItemsSource = null;
                grdVendorsSelected.ItemsSource = selectedVendors;
                grdVendor.ItemsSource = null;
                grdVendor.ItemsSource = allVendors;

                foreach (var _princ in department.Principals)
                {
                    selectedPrinciples.Add(_princ);
                }
                foreach (var _selectedPrinc in selectedPrinciples.ToList())
                {
                    if (allPrinciples.Contains(_selectedPrinc))
                    {
                        allPrinciples.Remove(_selectedPrinc);
                        var findParet = allPrinciples.Find(x => x.ParentID == _selectedPrinc.Id);
                        if (findParet != null)
                        {
                            allPrinciples.Add(_selectedPrinc);
                        }
                    }
                }
                grdPrincipleSelected.ItemsSource = null;
                grdPrincipleSelected.ItemsSource = selectedPrinciples;
                grdPrinciple.ItemsSource = null;
                grdPrinciple.ItemsSource = allPrinciples;
                foreach (var item in department.companies)
                {
                    gridCompany.SelectItem(gridCompany.FindRowByValue(gridCompany.Columns.GetColumnByFieldName("Id"), item.Id));

                }
                if(department.ChartofAccount!=null)
                {
                    lookupReceivableAccounts.Text = department.ChartofAccount.accountName;
                    receivableAccount = department.ChartofAccount;
                    isReceivableAccount.IsChecked = true;
                }
                if (department.AccountPayable != null)
                {
                    lookupPayableAccounts.Text = department.AccountPayable.accountName;
                    payableAccount = department.AccountPayable;
                    isPayableAccount.IsChecked = true;
                }
                if(department.isLinkable==true)
                {
                    chkisLinkAble.IsChecked = true;
                }
                else
                {
                    chkisLinkAble.IsChecked = false;
                }
            }
            else
            {
                loadSubsidoryData();
                grdEmployee.ItemsSource = allEmployees;
                grdcntrlSelectedEmployee.ItemsSource = departmentEmployes;
                grdCustomers.ItemsSource = allCustomers;
                grdCustomersSelected.ItemsSource = selectedCustomers;
                grdVendor.ItemsSource = allVendors;
                grdVendorsSelected.ItemsSource = selectedVendors;
                grdPrinciple.ItemsSource = allPrinciples;
                lblDeptName.Content = "Add New Department";
            }    
        }

        private void mapSection_Click(object sender, RoutedEventArgs e)
        {

        }
        public class sectdeptlist
        {
            public string deptname { get; set; }
            public string secname { get; set; }
            public int secid { get; set; }
        }
        private void loadSubsidoryData()
        {
            List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            lookupParentDept.ItemsSource = repo.GetDepartments();
        }
        private void txtdepartmentname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtdepartmentname.Text != "")
            {
                btnDeptSave.IsEnabled = true;

            }
            else
                btnDeptSave.IsEnabled = false;
        }

        private void chkisSubsidaiary_Checked(object sender, RoutedEventArgs e)
        {
            if (lookupParentDept.IsVisible == false)
            {
                lookupParentDept.Visibility = Visibility.Visible;
               lblParentDepartment.Visibility = Visibility.Visible;
                
            }
            loadSubsidoryData();
        }

        private void chkisSubsidaiary_Unchecked(object sender, RoutedEventArgs e)
        {
            if (lookupParentDept.IsVisible == true)
                lookupParentDept.Visibility = Visibility.Collapsed;
            if (lblParentDepartment.IsVisible == true)
                lblParentDepartment.Visibility = Visibility.Collapsed;
        }
        Department dept = new Department();
        private void lookupParentDept_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            dept = lookupParentDept.SelectedItem as Department;
            if (dept != null)
            {
                string selecteddept = dept.DeptName+" ("+ dept.Code +")";
            }
        }

       
   
        private void LookupReceivableAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(lookupReceivableAccounts.SelectedIndex != -1)
            receivableAccount = lookupReceivableAccounts.SelectedItem as ChartofAccount;
        }

        private void IsReceivableAccount_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void IsReceivableAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            receivableAccount = null;
        }

        private void IsPayableAccount_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void IsPayableAccount_Unchecked(object sender, RoutedEventArgs e)
        {
            payableAccount = null;
        }

        private void LookupPayableAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupPayableAccounts.SelectedIndex != -1)
                payableAccount = lookupPayableAccounts.SelectedItem as ChartofAccount;
        }

        private void ChkProcurmentModule_Checked(object sender, RoutedEventArgs e)
        {
            chkInquiry.IsChecked = true;
            chkOffer.IsChecked = true;
            chkSaleOrder.IsChecked = true;
            chkSaleInvoice.IsChecked = true;
            chkSaleReceipt.IsChecked = true;
            chkPurchaseOrder.IsChecked = true;
            chkPurchaseInvoice.IsChecked = true;
            chkPayments.IsChecked = true;
        }

        private void ChkProcurmentModule_Unchecked(object sender, RoutedEventArgs e)
        {
            chkInquiry.IsChecked = false;
            chkOffer.IsChecked = false;
            chkSaleOrder.IsChecked = false;
            chkSaleInvoice.IsChecked = false;
            chkSaleReceipt.IsChecked = false;
            chkPurchaseOrder.IsChecked = false;
            chkPurchaseInvoice.IsChecked = false;
            chkPayments.IsChecked = false;
        }
    

        private void btnLeftMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if(selectedItem != null)
                {
                    
                    allEmployees.Remove(selectedItem);
                    if(!departmentEmployes.Contains(selectedItem))
                        departmentEmployes.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdcntrlSelectedEmployee.RefreshData();
                grdEmployee.RefreshData();
                grdEmployee.SelectedItem = null;
                grdcntrlSelectedEmployee.SelectedItem = null;
            }
            catch (Exception)
            {

                
            }
        }

        private void btnRightMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdcntrlSelectedEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {
                    if (!allEmployees.Contains(selectedItem))
                         allEmployees.Add(selectedItem);
                    departmentEmployes.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdcntrlSelectedEmployee.RefreshData();
                grdEmployee.RefreshData();
                grdEmployee.SelectedItem = null;
                grdcntrlSelectedEmployee.SelectedItem = null;
            }
            catch (Exception)
            {

                
            }
        }

        private void btnRightMoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            int hasParent = 0;
            try
            {
                var selectedItem = grdCustomers.SelectedItem as CustomerCompany;
               
                if (selectedItem != null)
                {
                    var child = repo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Customer is parent please select child to add selected customer register");
                        return;
                    }
                    var node = selectedItem.parentCompany;
                    while (node != null)
                    {
                        hasParent = 1;
                        allCustomers.Remove(selectedItem);
                        if (!selectedCustomers.Contains(selectedItem))
                            selectedCustomers.Add(selectedItem);
                        if (!selectedCustomers.Contains(node))
                            selectedCustomers.Add(node);
                        var findParet = allCustomers.Find(x => x.parentCompany == node);
                        if (findParet == null)
                        {
                            allCustomers.Remove(node);
                        }
                        node = node.parentCompany;
                    }
                 
                    
                  
                 
                    if (hasParent == 0)
                        {
                            allCustomers.Remove(selectedItem);
                            if (!selectedCustomers.Contains(selectedItem))
                                selectedCustomers.Add(selectedItem);
                        }
                   

                    
                  

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Customer List Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
                //if(selectedCustomers.Count > 0)
                //{
                //    foreach (var bgColor in selectedCustomers)
                //    {
                //        if(bgColor.ParentID != null)
                //        {
                //            //grdCustomersSelected.Background = Brushes.Yellow;
                            
                //        }
                //    }
                //}
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            int parent = 0;
            try
            {
                var selectedItem = grdCustomersSelected.SelectedItem as CustomerCompany;

                if (selectedItem != null)
                {
                    var child = repo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Customer is parent please select child to add selected customer register");
                        return;
                    }
                    var node = selectedItem.parentCompany;
                    while (node != null)
                    {
                        parent = 1;
                        if (!allCustomers.Contains(selectedItem))
                            allCustomers.Add(selectedItem);
                        selectedCustomers.Remove(selectedItem);
                        if (!allCustomers.Contains(node))
                            allCustomers.Add(node);
                        var findParet = selectedCustomers.Find(x => x.parentCompany == node);
                        if (findParet == null)
                        {
                            selectedCustomers.Remove(node);
                        }
                        node = node.parentCompany;
                    }
                    if (parent == 0)
                    {
                        if (!allCustomers.Contains(selectedItem))
                            allCustomers.Add(selectedItem);
                        selectedCustomers.Remove(selectedItem);
 
                    }
                    //var node = selectedItem.parentCompany;
                    //while (node != null)
                    //{
                    //    parent = 1;
                    //    if (!allCustomers.Contains(selectedItem))
                    //        allCustomers.Add(selectedItem);
                    //    selectedCustomers.Remove(selectedItem);
                    //    if (!allCustomers.Contains(selectedItem.parentCompany))
                    //        allCustomers.Add(selectedItem.parentCompany);
                    //    //selectedCustomers.Remove(selectedItem.parentCompany);
                    //    //if (!allCustomers.Contains(selectedItem.parentCompany))
                    //    //    allCustomers.Add(selectedItem);
                    //    node = node.parentCompany;
                    //}
                    //if (parent == 0)
                    //{
                    //    if (!allCustomers.Contains(selectedItem))
                    //        allCustomers.Add(selectedItem);
                    //    selectedCustomers.Remove(selectedItem);
                    //    selectedCustomers.Remove(selectedItem.parentCompany);
                    //    //if (!allCustomers.Contains(selectedItem.parentCompany))
                    //    //    allCustomers.Add(selectedItem.parentCompany);
                    //}

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Customer Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);

            }
        }

        private void ChkAssetModule_Checked(object sender, RoutedEventArgs e)
        {
            chkRentalContract.IsChecked = true;
            chkRentalOrder.IsChecked = true;
            chkRentalInvoice.IsChecked = true;
            chkRentalReceipt.IsChecked = true;
        }

        private void ChkAssetModule_Unchecked(object sender, RoutedEventArgs e)
        {
            chkRentalContract.IsChecked = true;
            chkRentalOrder.IsChecked = true;
            chkRentalInvoice.IsChecked = true;
            chkRentalReceipt.IsChecked = true;
        }

        private void btnRightMoveVendor_Click(object sender, RoutedEventArgs e)
        {
            if (grdVendor.SelectedItem != null)
            {
                var vendor = grdVendor.SelectedItem as Vendor;
                if (allVendors.Find(x => x.ParentID == vendor.Id) == null)
                {

                    allVendors.Remove(vendor);
                    if (!selectedVendors.Contains(vendor))
                        selectedVendors.Add(vendor);

                    var parent = vendor.ParentVendor;
                    while (parent != null)
                    {
                        if (allVendors.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedVendors.Contains(parent))
                            {
                                selectedVendors.Add(parent);
                            }
                            var findParet = allVendors.Find(x => x.ParentVendor == parent);
                            if (findParet == null)
                            {
                                allVendors.Remove(parent);
                            }
                        }
                        parent = parent.ParentVendor;
                    }
                    grdVendor.ItemsSource = null;
                    grdVendorsSelected.ItemsSource = null;
                    grdVendor.ItemsSource = allVendors;
                    grdVendorsSelected.ItemsSource = selectedVendors;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Vendor which you want to Insert to Selected Vendor!");
            }
        }
        private void btnLeftMoveVendor_Click(object sender, RoutedEventArgs e)
        {
            if (grdVendorsSelected.SelectedItem != null)
            {
                var vendor = grdVendorsSelected.SelectedItem as Vendor;
                if (selectedVendors.Find(x => x.ParentID == vendor.Id) == null)
                {
                    selectedVendors.Remove(vendor);
                    if (!allVendors.Contains(vendor))
                        allVendors.Add(vendor);
                    var parent = vendor.ParentVendor;
                    while (parent != null)
                    {
                        if (selectedVendors.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allVendors.Contains(parent))
                                allVendors.Add(parent);
                            var findParet = selectedVendors.Find(x => x.ParentVendor == parent);
                            if (findParet == null)
                            {
                                selectedVendors.Remove(parent);
                            }
                        }
                        parent = parent.ParentVendor;
                    }
                    grdVendor.ItemsSource = null;
                    grdVendorsSelected.ItemsSource = null;
                    grdVendor.ItemsSource = allVendors;
                    grdVendorsSelected.ItemsSource = selectedVendors;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Vendor which you want to Remove from Selected Vendor !");
            }
        }

        private void btnRightMovePrinciple_Click(object sender, RoutedEventArgs e)
        {
            if (grdPrinciple.SelectedItem != null)
            {
                var principle = grdPrinciple.SelectedItem as Principal;
                if (allPrinciples.Find(x => x.ParentID == principle.Id) == null)
                {

                    allPrinciples.Remove(principle);
                    if (!selectedPrinciples.Contains(principle))
                        selectedPrinciples.Add(principle);

                    var parent = principle.parentDepartment;
                    while (parent != null)
                    {
                        if (allPrinciples.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedPrinciples.Contains(parent))
                            {
                                selectedPrinciples.Add(parent);
                            }
                            var findParet = allPrinciples.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allPrinciples.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    grdPrinciple.ItemsSource = null;
                    grdPrincipleSelected.ItemsSource = null;
                    grdPrinciple.ItemsSource = allPrinciples;
                    grdPrincipleSelected.ItemsSource = selectedPrinciples;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Principle which you want to Insert to Selected Principle!");
            }
        }

        private void btnLeftMovePrinciple_Click(object sender, RoutedEventArgs e)
        {
            if (grdPrincipleSelected.SelectedItem != null)
            {
                var principle = grdPrincipleSelected.SelectedItem as Principal;
                if (selectedPrinciples.Find(x => x.ParentID == principle.Id) == null)
                {
                    selectedPrinciples.Remove(principle);
                    if (!allPrinciples.Contains(principle))
                        allPrinciples.Add(principle);
                    var parent = principle.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedPrinciples.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allPrinciples.Contains(parent))
                                allPrinciples.Add(parent);
                            var findParet = selectedPrinciples.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedPrinciples.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    grdPrinciple.ItemsSource = null;
                    grdPrincipleSelected.ItemsSource = null;
                    grdPrinciple.ItemsSource = allPrinciples;
                    grdPrincipleSelected.ItemsSource = selectedPrinciples;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Vendor which you want to Remove from Selected Vendor !");
            }
        }

        private void chkIsParent_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkIsParent_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
