using DevExpress.Xpf.Core;
using ERP_BL.Config;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucTargetAdjustment.xaml
    /// </summary>
    public partial class ucTargetAdjustment : DXWindow
    {
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        CustomerCompany customer = new CustomerCompany();
       public AuditYearAdjustment auditYearAdjustment = new AuditYearAdjustment();
        Vendor vendor = new Vendor();
        List<ViewInfo> views = new List<ViewInfo>();
        Principal principal = new Principal();
      public  SaleOrder saleOrder = new SaleOrder();
        double systemMargin = 0;
        public ucTargetAdjustment()
        {
            InitializeComponent();
        }
        public ucTargetAdjustment(SaleOrder _saleOrder)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadcompanies();
                loadinquirytypes();
                loadCurrencies();
                LoadAuditYearAdjustment();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }
        public void LoadAuditYearAdjustment()
        {
            txtCreator.Text = SYSTEM_STATIC.currentUser.userName;
            cmbAdjustmentType.SelectedItem = saleOrder.saleOrdertype.ToString();
            if (saleOrder.company_Id != null || saleOrder.company != null)
            {
                company = saleOrder.company;
                lookupCompany.Text = saleOrder.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";
            }
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
            if (saleOrder.customerCompany.Id != 0 || saleOrder.customerCompany != null)
            {
                lookupCustomer.Text = saleOrder.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(saleOrder.customerCompany);
                customer = saleOrder.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";
            }
            if (saleOrder.allocation_Id != 0 || saleOrder.employee != null)
            {
                foreach (cmbitem cmbitem in cmbEmployee.Items)
                {
                    if (cmbitem.id == saleOrder.allocation_Id)
                    {
                        cmbEmployee.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            if (saleOrder.vendors != null)
            {
                foreach (Vendor vendr in saleOrder.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                }
            }
            if (saleOrder.principal_Id != 0 || saleOrder.principal != null)
            {
                lookupPrincipal.Text = saleOrder.principal.company.CompanyName;
                principal = saleOrder.principal;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";
            }
            if (saleOrder.auditYear != null)
            {
                datAuditYealy.EditValue = saleOrder.auditYear;
            }
            txtAmountSO.Text = saleOrder.totalCFRValue.ToString();
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == saleOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            foreach (cmbitem cmbitem in cmbOriginalCurrency.Items)
            {
                if (cmbitem.id == saleOrder.currency_Id)
                {
                    cmbOriginalCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            if (saleOrder.CostSheet != null)
            {
                txtBudgetCost.Text = saleOrder.CostSheet.TotalBudgetedMargin.ToString();
                txtActualCost.Text = saleOrder.CostSheet.TotalActualMargin.ToString();
            }
            if (saleOrder.CostSheet_Id != null)
            {
                var systemCost = saleOrderRepo.GetSOSystemCost((int)saleOrder.CostSheet_Id);

                txtSystemCost.Text = systemCost.ToString();
            }
            if (saleOrder.auditYearAdjustment_Id != null)
            {
                txtExchangeRate.Text = saleOrder.AuditYearAdjustment.ExhangeRate.ToString();
                txtSOAmountAudit.Text= saleOrder.AuditYearAdjustment.SOAmountAudit.ToString();
                txtRevenue.Text = saleOrder.AuditYearAdjustment.revenue.ToString();
                txtDefferedIncome.Text = saleOrder.AuditYearAdjustment.defferedIncome.ToString();
                txtCGS.Text = saleOrder.AuditYearAdjustment.cgs.ToString();
                txtReceivable.Text = saleOrder.AuditYearAdjustment.accountReceivable.ToString();
                txtPayable.Text = saleOrder.AuditYearAdjustment.accountPayable.ToString();
                txtBank.Text = saleOrder.AuditYearAdjustment.bank.ToString();

                if (saleOrder.AuditYearAdjustment.auditCurrency_Id != null && saleOrder.AuditYearAdjustment.auditCurrency != null)
                {
                    var currencySource = (List<cmbitem>)cmbAuditCurrency.Items.SourceCollection;
                    cmbAuditCurrency.SelectedItem = cmbAuditCurrency.Items[cmbAuditCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.AuditYearAdjustment.auditCurrency_Id))];
                }
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
            var currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupCompany.ItemsSource = currentUserCompanies;
        }
        public void loadinquirytypes()
        {
            Config config = new Config();
            List<string> InquiryType = config.getInquiryType();
            if (InquiryType.Count > 0)
                foreach (string IND in InquiryType)
                {
                    cmbAdjustmentType.Items.Add(IND);
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
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsSaleOrderType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    //if (auditYearAdjustment != null && auditYearAdjustment.Id > 0)
                    //    if (auditYearAdjustment.department != null && departments.FirstOrDefault(x => x.Id == auditYearAdjustment.dept_Id) == null)
                    //        departments.Add(auditYearAdjustment.department);
                    lookupDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
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
            }
        }
        private void loadCurrencies()
        {
            cmbbaseCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbAuditCurrency.ItemsSource= SYSTEM_STATIC.currencySources;
            cmbOriginalCurrency.ItemsSource= SYSTEM_STATIC.currencySources;
        }

     
        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            customer = lookupCustomer.SelectedItem as CustomerCompany;
            if (customer != null)
            {
                //if (auditYearAdjustment.customerCompany_Id != customer.Id)
                //{
                //    var customerCountry = customer.billingAddres.Country;
                //    //txtCustomerCountry.Text = customerCountry;
                //    var child = saleOrderRepo.getParent(customer.Id);
                //    if (child != null)
                //    {
                //        //txtCustomerCountry.Text = "";
                //        lookupCustomer.SelectedItem = null;
                //        DXMessageBox.Show("Selected Customer is parent please select child to add selected Customer register");
                //        lookupDepartment.Focus();
                //        return;
                //    }
                //}
            }
        }
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnauditAdjustmentadd_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo userRepo = new UsersRepo();
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();

            if (lookupCompany.SelectedIndex == -1 )
            {
                lookupCompany.Focus();
                MessageBox.Show("Please Select a company against Audit Adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                return;
            }
            else if (lookupDepartment.SelectedIndex == -1 )
            {
                MessageBox.Show("Please Select a department against Audit Adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                lookupDepartment.Focus();
                return;
            } 
            else if (lookupCustomer.SelectedIndex == -1 )
            {
                MessageBox.Show("Please Select a customer against Audit Adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                lookupCustomer.Focus();
                return;
            }
            else if (datAuditYealy.EditValue ==null )
            {
                MessageBox.Show("Please Select audit Year at Sale order Audit Adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                lookupCustomer.Focus();
                return;
            }
            var user = userRepo.getuserbyUsername(txtCreator.Text);
            auditYearAdjustment.user_Id = user.id;
            auditYearAdjustment.auditYearAdjustmentType = (InquiryType)cmbAdjustmentType.SelectedIndex;
            auditYearAdjustment.company_Id = (lookupCompany.SelectedItem as Company).Id;
            auditYearAdjustment.dept_Id = (lookupDepartment.SelectedItem as Department).Id;
            auditYearAdjustment.customerCompany_Id = (lookupCustomer.SelectedItem as CustomerCompany).Id;
            auditYearAdjustment.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
            auditYearAdjustment.principal_Id = (lookupPrincipal.SelectedItem as ERP_BL.Databases.Principal).Id;
            auditYearAdjustment.auditYear = (DateTime)datAuditYealy.EditValue;
            auditYearAdjustment.SOAmount = Convert.ToDouble(txtAmountSO.Text);
            auditYearAdjustment.SOAmountAudit = Convert.ToDouble(txtSOAmountAudit.Text);
            auditYearAdjustment.ExhangeRate = Convert.ToDouble(txtExchangeRate.Text);
            if ((InquiryType)cmbAdjustmentType.SelectedIndex == InquiryType.Principal)
            {
                auditYearAdjustment.comissionOC =  Convert.ToDouble(txtCommission.Text);
                auditYearAdjustment.netComissionOC =  Convert.ToDouble(txtNetCommission.Text);
                auditYearAdjustment.comissionAudit =  Convert.ToDouble(txtCommissionAudit.Text);
                auditYearAdjustment.netComissionAudit =  Convert.ToDouble(txtNetCommissionAudit.Text);
            }
            else
            {

                auditYearAdjustment.BudgetCost = Convert.ToDouble(txtBudgetCost.Text);
                auditYearAdjustment.ActualCost = Convert.ToDouble(txtActualCost.Text);
                auditYearAdjustment.SystemCost = Convert.ToDouble(txtSystemCost.Text);
                auditYearAdjustment.BudgetCostAudit = Convert.ToDouble(txtBudgetCostAudit.Text);
                auditYearAdjustment.ActualCostAudit = Convert.ToDouble(txtActualCostAudit.Text);
                auditYearAdjustment.SystemCostAudit = Convert.ToDouble(txtSystemCostAudit.Text);







                auditYearAdjustment.BudgetMargin = Convert.ToDouble(txtBMOC.Text);
                auditYearAdjustment.ActualMargin = Convert.ToDouble(txtAMOC.Text);
                auditYearAdjustment.SystemMargin = Convert.ToDouble(txtSMOC.Text);
                auditYearAdjustment.BudgetMarginAudit = Convert.ToDouble(txtBMA.Text);
                auditYearAdjustment.ActualMarginAudit = Convert.ToDouble(txtAMA.Text);
                auditYearAdjustment.SystemMarginAudit = Convert.ToDouble(txtSMA.Text);
            }
            if (cmbAuditCurrency.SelectedIndex > -1)
            {
                auditYearAdjustment.auditCurrency_Id = (cmbAuditCurrency.SelectedItem as cmbitem).id;
            }
            if (string.IsNullOrEmpty(txtRevenue.Text))
            {
                auditYearAdjustment.revenue = 0;
            }
            else
            {
                auditYearAdjustment.revenue = Convert.ToDouble(txtRevenue.Text);
            }
            if (string.IsNullOrEmpty(txtDefferedIncome.Text))
            {
                auditYearAdjustment.defferedIncome = 0;
            }
            else
            {
                auditYearAdjustment.defferedIncome = Convert.ToDouble(txtDefferedIncome.Text);
            }
            if (string.IsNullOrEmpty(txtCGS.Text))
            {
                auditYearAdjustment.cgs = 0;
            }
            else
            {
                auditYearAdjustment.cgs = Convert.ToDouble(txtCGS.Text);
            }
            if (string.IsNullOrEmpty(txtReceivable.Text))
            {
                auditYearAdjustment.accountReceivable = 0;
            }
            else
            {
                auditYearAdjustment.accountReceivable = Convert.ToDouble(txtReceivable.Text);
            }
            if (string.IsNullOrEmpty(txtPayable.Text))
            {
                auditYearAdjustment.accountPayable = 0;
            }
            else
            {
                auditYearAdjustment.accountPayable = Convert.ToDouble(txtPayable.Text);
            }
            if (string.IsNullOrEmpty(txtBank.Text))
            {
                auditYearAdjustment.bank = 0;
            }
            else
            {
                auditYearAdjustment.bank = Convert.ToDouble(txtBank.Text);
            }
            //if (vendor != null)
            //{
            //    auditYearAdjustment.vendors = new List<Vendor>();
            //    auditYearAdjustment.vendors.Add(saleOrderRepo.getVendor(vendor.Id));
            //}
            this.Close();
        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            //  employees = cont1.GetEmployees();
            employees = department.employees;

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbEmployee.ItemsSource = cmbitems;
        }
        public void loadcustomers()
        {
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                        var customers = customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id);
                        if (customers == null || customers.Count == 0)
                        {
                           
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
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                //if (auditYearAdjustment.dept_Id != department.Id)
                //{
                //    var child = saleOrderRepo.getParentDepart(department.Id);
                //    if (child != null)
                //    {
                //        lookupDepartment.SelectedItem = null;
                //        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                //        lookupDepartment.Focus();
                //        return;
                //    }
                //}
                loadcustomers();
                lookupVendor.ItemsSource = department.Vendors;
                PrincipalRepo principalRepo = new PrincipalRepo();
                lookupPrincipal.ItemsSource = principalRepo.getAllByDept(department.Id);
                ProductRepo productRepo = new ProductRepo();
                loademployees();
                if (department.customers.Count == 0)
                {
                    MessageBox.Show("No customer is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Vendors.Count == 0)
                {
                    MessageBox.Show("No Vendor is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.employees.Count == 0)
                {
                    MessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Principals.Count == 0)
                {
                    MessageBox.Show("No Principal is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
            }
        }
        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                MessageBox.Show("Select Department Frist");
                return;
            }
        }
        private void lookupVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            vendor = lookupVendor.SelectedItem as Vendor;
            if (vendor != null)
            {
                var vendorCountry = vendor.billingAddres.Country;
                //txtVendorCountry.Text = vendorCountry;
            }
        }
        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

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
        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            
        }
        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
        }
        private void lookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            principal = lookupPrincipal.SelectedItem as Principal;
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
        private void CmbbaseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtExchangeRate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if ((InquiryType)cmbAdjustmentType.SelectedIndex == InquiryType.Principal)
            {
                if (!string.IsNullOrEmpty(txtCommission.Text) && !string.IsNullOrEmpty(txtNetCommission.Text))
                {
                    var commissionAudit = Convert.ToDouble(txtCommission.Text) * Convert.ToDouble(txtExchangeRate.Text);
                    var netCommissionAudit = Convert.ToDouble(txtNetCommission.Text) * Convert.ToDouble(txtExchangeRate.Text);
            
                    var soAmountAudit = Convert.ToDouble(txtAmountSO.Text) * Convert.ToDouble(txtExchangeRate.Text);
                    txtCommissionAudit.Text = commissionAudit.ToString();
                    txtNetCommissionAudit.Text = netCommissionAudit.ToString();
                    txtSOAmountAudit.Text = soAmountAudit.ToString();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtAmountSO.Text) && !string.IsNullOrEmpty(txtBudgetCost.Text) && !string.IsNullOrEmpty(txtActualCost.Text) && !string.IsNullOrEmpty(txtSystemCost.Text))
                {

                    var budgetCostAudit = Convert.ToDouble(txtBudgetCost.Text) * Convert.ToDouble(txtExchangeRate.Text);
                    var actualCostAudit = Convert.ToDouble(txtActualCost.Text) * Convert.ToDouble(txtExchangeRate.Text);
                    var systemCostAudit = Convert.ToDouble(txtSystemCost.Text) * Convert.ToDouble(txtExchangeRate.Text);


                    txtBudgetCostAudit.Text = budgetCostAudit.ToString();
                    txtActualCostAudit.Text = actualCostAudit.ToString();
                    txtSystemCostAudit.Text = systemCostAudit.ToString();

                    var budgetMargin = Convert.ToDouble(txtAmountSO.Text) - Convert.ToDouble(txtBudgetCost.Text);
                    var actualMargin = Convert.ToDouble(txtAmountSO.Text) - Convert.ToDouble(txtActualCost.Text);
                    var systemMargin = Convert.ToDouble(txtAmountSO.Text) - Convert.ToDouble(txtSystemCost.Text);
                    txtBMOC.Text = budgetMargin.ToString();
                    txtAMOC.Text = actualMargin.ToString();
                    txtSMOC.Text = systemMargin.ToString();



                    var budgetMarginAudit = Convert.ToDouble(budgetMargin * Convert.ToDouble(txtExchangeRate.Text));
                    var actualMarginAudit = Convert.ToDouble(actualMargin) * Convert.ToDouble(txtExchangeRate.Text);
                    var systemMarginAudit = Convert.ToDouble(systemMargin) * Convert.ToDouble(txtExchangeRate.Text);
            


                    txtBMA.Text = budgetMarginAudit.ToString();
                    txtAMA.Text = actualMarginAudit.ToString();
                    txtSMA.Text = systemMarginAudit.ToString();
                    var soAmountAudit = Convert.ToDouble(txtAmountSO.Text) * Convert.ToDouble(txtExchangeRate.Text);

                    txtSOAmountAudit.Text = soAmountAudit.ToString();
                }
            }
        }

        private void cmbAdjustmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((InquiryType)cmbAdjustmentType.SelectedIndex == InquiryType.Principal)
            {
                grpMarginCost.Visibility = Visibility.Collapsed;
                grpMarginAudit.Visibility = Visibility.Collapsed;
                grpComissionCost.Visibility = Visibility.Visible;
                grpComissionAudit.Visibility = Visibility.Visible;
                if (saleOrder.auditYearAdjustment_Id != null)
                {
                   txtCommissionAudit.Text = saleOrder.AuditYearAdjustment.comissionAudit.ToString();
                   txtNetCommissionAudit.Text = saleOrder.AuditYearAdjustment.netComissionAudit.ToString();
                }
                txtCommission.Text = saleOrder.commision.ToString();
                txtNetCommission.Text = saleOrder.commisioninBase.ToString();
            }
        }
    }
}
