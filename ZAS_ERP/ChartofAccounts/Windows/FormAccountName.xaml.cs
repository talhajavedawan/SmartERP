using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZAS_ERP.ChartofAccounts.UserControls;
using ZAS_ERP.ChartofAccounts.ViewModels;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for FormAccountName.xaml
    /// </summary>
    public partial class formAccountName : DXWindow
    {
        string accountType = null;
        ChartofAccount chartofAccount = new ChartofAccount();
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        formAddOpeningBalance addOpeningBalance = new formAddOpeningBalance();
        Currency selectedCurrency = new Currency();
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        CompanyRepo companyRepo = new CompanyRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        public ucChartofAccountList  myParent = null;
        Currency currency = new Currency();
        Company accountCompany = new Company();
        List<Company> allCompanies = new List<Company>();
        List<Company> finalCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Department> finalDepartments = new List<Department>();      
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> finalEmployees = new List<ERP_BL.Databases.Employee>();



        public formAccountName()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;
            viewTableDepartment.NodeCheckStateChanged += OnNodeCheckStateChangedDepartment;
            grdDepartment.SelectionChanged += OnGridSelectionChangedDepartment;
           
        
            chartofAccount = null;
        }

        public formAccountName(string type)
        {
            InitializeComponent();
            accountType = type;
            LoadParentAccounts();
            LoadCurrencies();
            cmbAccountType.SelectedItem = accountType;
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;
            viewTableDepartment.NodeCheckStateChanged += OnNodeCheckStateChangedDepartment;
            grdDepartment.SelectionChanged += OnGridSelectionChangedDepartment;
        
          
        }

        public formAccountName(ChartofAccount account)
        {
            InitializeComponent();
            chartofAccount = account;
            //LoadAccountData(chartofAccount);
            //accountType = account.accountType.ToString();
            LoadParentAccounts(account.accountType);
            LoadCurrencies();
            //viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            //grdCompany.SelectionChanged += OnGridSelectionChanged;
            //viewTableDepartment.NodeCheckStateChanged += OnNodeCheckStateChangedDepartment;
            //grdDepartment.SelectionChanged += OnGridSelectionChangedDepartment;
          
        }
        private void OnGridSelectionChangedDepartment(object sender, GridSelectionChangedEventArgs e)
        {
            var viewDepartment = (TreeListView)grdDepartment.View;
            var nodeDepartment = viewDepartment.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (nodeDepartment != null)
                        nodeDepartment.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (nodeDepartment != null)
                        nodeDepartment.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdDepartment.GetSelectedRowHandles();
                    viewDepartment.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        nodeDepartment = viewDepartment.GetNodeByRowHandle(rowHandle);
                        nodeDepartment.IsChecked = true;
                    }
                    break;
            }
        }

        private void OnNodeCheckStateChangedDepartment(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdDepartment.SelectItem(e.Node.RowHandle);
            else
                grdDepartment.UnselectItem(e.Node.RowHandle);
        }

        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCompany.SelectItem(e.Node.RowHandle);
            else
                grdCompany.UnselectItem(e.Node.RowHandle);
            
        }

        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdCompany.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdCompany.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
           
        }


        
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetLayout(accountType);
            LoadAccountTypes();
            LoadCurrencies();
            //LoadParentAccounts();
            Loadcompanies();
            LoadAccountData(chartofAccount);
          
        }

        public void Loadcompanies()
        {
            CompanyRepo cont = new CompanyRepo();
            if (MainWindow.currentUserid == 0)
            {
               
                allCompanies= cont.GetCompanies();
                return;
            }
            allCompanies = cont.GetActiveCompaniesByEmpl(SYSTEM_STATIC.currentUser.employee.EmpId);
            lookupAssociatedCompany.ItemsSource = allCompanies;
           //allCompanies =  SYSTEM_STATIC.LoadCurrentUserCompanies();

            //empUser = employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            //grdCompany.ItemsSource = empUser.Companies;
           
        }

        //Opening balance Click
        private void BtnOpeningBalance_Click(object sender, RoutedEventArgs e)
        {
            addOpeningBalance.ShowDialog();
            txtOpeningBalance.Text = addOpeningBalance.txtOpeningBalance.Text;
            lblOpeningDate.Content = addOpeningBalance.datAccountCreation.EditValue;
        }
        //Save Account Click
        private void BtnSaveAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                switch (cmbAccountType.SelectedItem)
                {
                    case "Income":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Income;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.creationDate = DateTime.Now;
                            break;
                        }
                    case "Expense":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Expense;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Fixed Assets":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Fixed_Asset;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Bank":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Bank;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.bankAccountNo = txtBankAccountNo.Text.Trim();
                            chartofAccount.routingNo = txtAccountRoutingNo.Text.Trim();
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Loan":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Loan;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.routingNo = txtAccountRoutingNo.Text.Trim();
                            chartofAccount.accountNo = txtAccountNo.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Credit Card":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Credit_Card;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creditCardNo = txtCreditCardAccountNo.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Equity":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Equity;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Account Receivable":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Account_Receivable;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text;
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Other Current Asset":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Other_Current_Asset;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text;
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Other Asset":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Other_Asset;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.accountNo = txtAccountNo.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Accounts Payable":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Accounts_Payable;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Other Current Liability":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Other_Current_Liability;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.routingNo = txtAccountRoutingNo.Text.Trim();
                            chartofAccount.accountNo = txtAccountNo.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Long term Liability":
                        {
                            chartofAccount.isDebitIncrease = false;
                            chartofAccount.accountType = COA_AccountType.Longterm_Liability;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.accountNo = txtAccountNo.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Cost of Goods Sold":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Cost_of_Goods_Sold;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Other Income":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Other_Income;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                    case "Other Expense":
                        {
                            chartofAccount.isDebitIncrease = true;
                            chartofAccount.accountType = COA_AccountType.Other_Expense;
                            chartofAccount.accountName = txtAccountName.Text.Trim();
                            chartofAccount.isActive = isActive.IsChecked == true ? true : false;
                            chartofAccount.description = txtDescription.Text.Trim();
                            chartofAccount.creationDate = DateTime.Now;
                            chartofAccount.userId = SYSTEM_STATIC.currentUser.id;
                            chartofAccount.isActiveForTrialBalance = isTrialBalance.IsChecked == true ? true : false;

                            break;
                        }
                }
                //if (cmbxCurrency.SelectedIndex == -1)
                //{
                //    DXMessageBox.Show("Please Select Account Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    cmbxCurrency.Focus();
                //    return;
                //}
                
                if (cmbxCurrency.SelectedIndex != -1 )
                {
                    chartofAccount.currencyId = currency.Id;
                }
                if(lookupAssociatedCompany.SelectedIndex>-1)
                {
                    chartofAccount.accociatedCompany = (lookupAssociatedCompany.SelectedItem as Company).CompanyName;
                }
                chartofAccount.Companies = GetCompanies();
                chartofAccount.Departments = GetDepartments();
                chartofAccount.Employees = GetEmployees();


                if (chartofAccount.Id != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Chart of Account") != null))
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null && chartofAccount.isApproved != true)
                    {

                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            chartofAccount.stage = TransactionStage.Approved.ToString();
                            chartofAccount.isApproved = true;
                            chartofAccount.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null && chartofAccount.isReApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            chartofAccount.stage = TransactionStage.Approved.ToString();
                            chartofAccount.isReApproved = true;
                            chartofAccount.ReApprovalDate = System.DateTime.Now;
                        }
                    }
                    if (isSubAccount.IsChecked == false)
                    {
                        chartofAccount.parent = null;
                        chartofAccount.parentId = null;
                    }
                   
                    if (isCurrency.IsChecked == false)
                    {
                        chartofAccount.Currency = null;
                        //chartofAccount.companyId = null;
                    }
                    ModuleLogic.UpdateAccount(chartofAccount);
                    DXMessageBox.Show("Chart of Account has been Updated Successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);

                    myParent.LoadCounts();

                }
                else
                if (chartofAccount.Id == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            DXMessageBox.Show("Please Create another Use Account to Create Chart of Account, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        else
                            chartofAccount.userId = MainWindow.currentUserid;
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null)
                        {
                            
                                chartofAccount.stage = TransactionStage.Approved.ToString();
                                chartofAccount.isApproved = true;
                                chartofAccount.ApprovedDate = System.DateTime.Now;
                            
                        }
                        else
                        {
                            chartofAccount.stage = TransactionStage.AwaitingFirstReview.ToString();

                            chartofAccount.isApproved = false;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null)
                        {

                            chartofAccount.stage = TransactionStage.Approved.ToString();
                            chartofAccount.isReApproved = true;
                            chartofAccount.ReApprovalDate = System.DateTime.Now;

                        }
                        else
                        {
                            chartofAccount.stage = TransactionStage.AwaitingApproval.ToString();
                            chartofAccount.isReApproved = false;
                            

                        }
                        if (isSubAccount.IsChecked == false)
                        {
                            chartofAccount.parent = null;
                            chartofAccount.parentId = null;
                        }
                   
                        if (isCurrency.IsChecked == false)
                        {
                            chartofAccount.Currency = null;
                            chartofAccount.currencyId = null;
                        }
                        ModuleLogic.AddAccount(chartofAccount);
                        DXMessageBox.Show("Chart of Account has been saved successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        myParent.LoadCounts();
                    }
                }
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
      
        private void CmbAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetLayout(cmbAccountType.SelectedItem.ToString());
        }

        private void CmbParentAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
           var parentAccount = cmbParentAccounts.SelectedItem as ChartofAccount;
            if (parentAccount != null)
            {
                string selectedcust = parentAccount.accountName;
                cmbParentAccounts.EditValue = selectedcust;
                chartofAccount.parentId = parentAccount.Id;
            }
        }
       
        //Loading Functions
        public void LoadParentAccounts()
        {
            var type = GetAccountType(accountType);
            cmbParentAccounts.ItemsSource = repo.GetAllApprovedAcccountsbyType(type, SYSTEM_STATIC.currentUser.id);
        }
        public void LoadParentAccounts(COA_AccountType type)
        {
            var coaType = type;
            cmbParentAccounts.ItemsSource = repo.GetAllApprovedAcccountsbyType(type, SYSTEM_STATIC.currentUser.id);
        }
        public void LoadAccountTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
            {
                var type = ((ERP_BL.Enums.COA_AccountType)i).ToString();
                if (type == "Account_Receivable")
                {
                    cmbAccountType.Items.Add("Account Receivable");
                }
                else
                     if (type == "Credit_Card")
                { cmbAccountType.Items.Add("Credit Card"); }
                else
                     if (type == "Other_Current_Asset")
                { cmbAccountType.Items.Add("Other Current Asset"); }
                else
                     if (type == "Other_Asset")
                { cmbAccountType.Items.Add("Other Asset"); }
                else
                     if (type == "Accounts_Payable")
                { cmbAccountType.Items.Add("Accounts Payable"); }
                else
                     if (type == "Other_Current_Liability")
                { cmbAccountType.Items.Add("Other Current Liability"); }
                else
                        if (type == "Longterm_Liability")
                { cmbAccountType.Items.Add("Long term Liability"); }
                else
                          if (type == "Cost_of_Goods_Sold")
                { cmbAccountType.Items.Add("Cost of Goods Sold"); }
                else
                          if (type == "Other_Income")
                {
                    cmbAccountType.Items.Add("Other Income");
                }
                else
                          if (type == "Other_Expense")
                {
                    cmbAccountType.Items.Add("Other Expense");
                }
                else
                          if (type == "Fixed_Asset")
                {
                    cmbAccountType.Items.Add("Fixed Assets");
                }
                else
                    cmbAccountType.Items.Add(type);
            }
        }

        public void LoadCurrencies()
        {
            var currencyRepo = new CurrencyRepo();
            var currencies= currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();
            cmbxCurrency.ItemsSource = currencies;
        }

        public void SetLayout(string type)
        {
            try
            {
                switch(type)
                {
                    case "Income":
                        {
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;

                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;

                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Expense":
                        {
                            btnOpeningBalance.Visibility = Visibility.Collapsed;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Collapsed;
                            lblOpeningDate.Visibility = Visibility.Collapsed;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Collapsed;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Fixed Assets":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Bank":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Visible;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Visible;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Visible;
                            lblBankAccountNo.Visibility = Visibility.Visible;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Loan":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Visible;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Visible;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            break;
                        }
                    case "Credit Card":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Visible;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Visible;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Equity":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Account Receivable":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Collapsed;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Other Current Asset":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Other Asset":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            break;
                        }
                    case "Accounts Payable":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Other Current Liability":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Visible;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Visible;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            break;
                        }
                    case "Long term Liability":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Visible;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Visible;
                            lblAccountNo.Visibility = Visibility.Visible;
                            break;
                        }
                    case "Cost of Goods Sold":
                        {
                            btnOpeningBalance.Visibility = Visibility.Visible;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Visible;
                            lblOpeningDate.Visibility = Visibility.Visible;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Collapsed;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Other Income":
                        {
                            btnOpeningBalance.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Collapsed;
                            lblOpeningDate.Visibility = Visibility.Collapsed;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Collapsed;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }
                    case "Other Expense":
                        {
                            btnOpeningBalance.Visibility = Visibility.Collapsed;
                            lblRoutingNo.Visibility = Visibility.Collapsed;
                            lblCreditCardNo.Visibility = Visibility.Collapsed;
                            lblAsof.Visibility = Visibility.Collapsed;
                            lblOpeningDate.Visibility = Visibility.Collapsed;
                            txtAccountRoutingNo.Visibility = Visibility.Collapsed;
                            txtOpeningBalance.Visibility = Visibility.Collapsed;
                            txtCreditCardAccountNo.Visibility = Visibility.Collapsed;
                            txtBankAccountNo.Visibility = Visibility.Collapsed;
                            lblBankAccountNo.Visibility = Visibility.Collapsed;
                            txtAccountNo.Visibility = Visibility.Collapsed;
                            lblAccountNo.Visibility = Visibility.Collapsed;
                            break;
                        }


                }
                if(chartofAccount.Id!=0)
                { btnVoid.Visibility = Visibility.Visible; }else
                { btnVoid.Visibility = Visibility.Collapsed; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadAccountData(ChartofAccount account)
        {
            try
            {
                if (account.Id != 0)
                {
                    CoaLogicClass obj = new CoaLogicClass();
                    var type = obj.GetAccountType(chartofAccount.accountType);
                    accountType = type;
                    lblStage.Text = account.stage;
                    txtAccountName.Text = chartofAccount.accountName;
                    cmbAccountType.SelectedItem = accountType;
                    if(!string.IsNullOrEmpty(account.accociatedCompany))
                    {
                        lookupAssociatedCompany.Text = account.accociatedCompany;
                    }
                    
                    SetLayout(accountType);
                    if (!string.IsNullOrEmpty(chartofAccount.accountName))
                    {
                        txtAccountName.Text = chartofAccount.accountName;
                    }
                    isActive.IsChecked = chartofAccount.isActive;
                    isActive.IsChecked = chartofAccount.isActiveForTrialBalance;

                    if (chartofAccount.parentId != 0 && chartofAccount.parentId != null)
                    {
                        isSubAccount.IsChecked = true;
                        cmbParentAccounts.SelectedItem = chartofAccount.parent.accountName;
                    }
                    if (chartofAccount.currencyId != null || chartofAccount.Currency != null)
                    {
                        currency = chartofAccount.Currency;
                        if (chartofAccount.Currency != null)
                        {
                            cmbxCurrency.Text = chartofAccount.Currency.CurrencyName;
                            isCurrency.IsChecked = true;
                        }
                    }
                    else
                    {
                        cmbxCurrency.Text = "Select Currency";
                    }
                    if (chartofAccount.parent != null)
                    {
                        isSubAccount.IsChecked = true;
                        cmbParentAccounts.Text = chartofAccount.parent.accountName;

                    }
                    if (!string.IsNullOrEmpty(chartofAccount.description))
                    {
                        txtDescription.Text = chartofAccount.description;
                    }
                    if (!string.IsNullOrEmpty(chartofAccount.bankAccountNo))
                    {
                        txtBankAccountNo.Text = chartofAccount.bankAccountNo;
                    }
                    if (!string.IsNullOrEmpty(chartofAccount.routingNo))
                    {
                        txtAccountRoutingNo.Text = chartofAccount.routingNo;
                    }
                    if (!string.IsNullOrEmpty(chartofAccount.accountNo))
                    {
                        txtAccountNo.Text = chartofAccount.accountNo;
                    }
                    if (!string.IsNullOrEmpty(chartofAccount.creditCardNo))
                    {
                        txtCreditCardAccountNo.Text = chartofAccount.creditCardNo;
                    }
                    if (chartofAccount.Companies.Count != 0)
                    {
                        //foreach (var _comp in chartofAccount.Companies)
                        //{
                        //    grdCompany.SelectItem(grdCompany.FindRowByValue(grdCompany.Columns.GetColumnByFieldName("Id"), _comp.Id));
                        //}
                        foreach (var company in chartofAccount.Companies)
                        {
                            foreach (var acompany in allCompanies)
                            {
                                if (acompany.Id == company.Id)
                                {
                                    finalCompanies.Add(acompany);
                                    allDepartments.AddRange(company.departments.Where(c=>c.isActive == true));
                                    //allCompanies.Remove(acompany);
                                }
                            }
                        }
                     
                    }
                
                    if (chartofAccount.Departments.Count != 0)
                    {
                        //foreach (var _Department in chartofAccount.Departments)
                        //{
                        //    grdDepartment.SelectItem(grdDepartment.FindRowByValue(grdDepartment.Columns.GetColumnByFieldName("Id"), _Department.Id));
                        //}
                        foreach (var dept in chartofAccount.Departments)
                        {
                            foreach (var adept in allDepartments)
                            {
                                if (adept.Id == dept.Id)
                                {
                                    finalDepartments.Add(adept);
                                    allEmployees.AddRange(adept.employees);
                                    //allDepartments.Remove(adept);
                                }
                            }
                        }
                     
                    }
                
                    if (chartofAccount.Employees.Count != 0)
                    {
                        //foreach (var _Employee in chartofAccount.Employees)
                        //{
                        //    grdEmployee.SelectItem(grdEmployee.FindRowByValue(grdEmployee.Columns.GetColumnByFieldName("EmpId"), _Employee.EmpId));
                        //}

                        foreach (var emp in chartofAccount.Employees)
                        {
                            foreach (var aEMP in allEmployees)
                            {
                                if (aEMP.EmpId == emp.EmpId)
                                {
                                    finalEmployees.Add(aEMP);
                                }
                            }
                        }
                       
                    }
                 
                    finalCompanies = finalCompanies.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();

                    allCompanies = allCompanies.Except(finalCompanies).ToList();
                    allCompanies = allCompanies.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
                    grdCompany.ItemsSource = allCompanies;
                    finalDepartments = finalDepartments.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();

                    allDepartments = allDepartments.Except(finalDepartments).ToList();
                    allDepartments = allDepartments.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();


                    allEmployees = allEmployees.Except(finalEmployees).ToList();
                    allEmployees = allEmployees.GroupBy(x => x.EmpId)
                                  .Select(g => g.First())
                                  .ToList();
                    finalEmployees = finalEmployees.GroupBy(x => x.EmpId)
                                  .Select(g => g.First())
                                  .ToList();
                    finalEmployees = finalEmployees.Except(allEmployees).ToList();


                    grdFinalCompanies.ItemsSource = finalCompanies;
                    grdFinalDepartment.ItemsSource = finalDepartments;
                    grdDepartment.ItemsSource = allDepartments;
                    grdFinalEmployee.ItemsSource = finalEmployees;

                    grdEmployee.ItemsSource = allEmployees;
                }
                else
                {
                    chartofAccount = new ChartofAccount();
                    grdCompany.ItemsSource = allCompanies;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public COA_AccountType GetAccountType(string accountType)
        {
            COA_AccountType type = 0;

            switch(accountType)
            {
                case "Income":
                    {
                        type = COA_AccountType.Income;
                        break;
                    }
                case "Expense":
                    {
                        type = COA_AccountType.Expense;
                        break;
                    }
                case "Fixed Assets":
                    {
                        type = COA_AccountType.Fixed_Asset;
                        break;
                    }
                case "Bank":
                    {

                        type = COA_AccountType.Bank;
                        break;
                    }
                case "Loan":
                    {
                        type = COA_AccountType.Loan;
                        break;
                    }
                case "Credit Card":
                    {
                        type = COA_AccountType.Credit_Card;
                        break;
                    }
                case "Equity":
                    {

                        type = COA_AccountType.Equity;
                        break;
                    }
                case "Account Receivable":
                    {
                        type = COA_AccountType.Account_Receivable;
                        break;
                    }
                case "Other Current Asset":
                    {
                        type = COA_AccountType.Other_Current_Asset;
                        break;
                    }
                case "Other Asset":
                    {
                        type = COA_AccountType.Other_Asset;
                        break;
                    }
                case "Accounts Payable":
                    {
                        type = COA_AccountType.Accounts_Payable;
                        break;
                    }
                case "Other Current Liability":
                    {
                        type = COA_AccountType.Other_Current_Liability;
                        break;
                    }
                case "Long term Liability":
                    {
                        type = COA_AccountType.Longterm_Liability;
                        break;
                    }
                case "Cost of Goods Sold":
                    {
                        type = COA_AccountType.Cost_of_Goods_Sold;
                        break;
                    }
                case "Other Income":
                    {
                        type = COA_AccountType.Other_Income;
                        break;
                    }
                case "Other Expense":
                    {
                        type = COA_AccountType.Other_Expense;
                        break;
                    }
            }
            return type;
        }

        private void CmbxCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxCurrency.SelectedItem as Currency != null)
            {
                int idd = (cmbxCurrency.SelectedItem as Currency).Id;
                currency = currencyRepo.get(idd);
            }
        }

        private void BtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (chartofAccount.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Chart of Account") != null))
            {
                if (DXMessageBox.Show("This Chart of Account is currently in the list of Void Chart of Accounts! Do you want to remove it from Void?", "Remove Void Chart of Account", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    chartofAccount.isVoid = false;
                    repo.setChartofAccounttoVoid(chartofAccount.Id, false);
                    DXMessageBox.Show("This Chart of Account is re-moved from void list?", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Chart of Account") != null)
            {
                if (DXMessageBox.Show("This Chart of Account is not currently in the list of Void Chart of Accounts! Do you want to move it to Void Chart of Accounts?", "Add to Void Chart of Accounts", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    chartofAccount.isVoid = true;
                    repo.setChartofAccounttoVoid(chartofAccount.Id, true);
                    DXMessageBox.Show("This Chart of Account is moved to void list?", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        public List<Company> GetCompanies()
        {
            
            List<Company> companies = new List<Company>();
            var grdCompanies = grdFinalCompanies.ItemsSource as List<Company>;
            foreach (Company company in grdCompanies)
            {
                companies.Add(company);
            }
            return companies;
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();

           
                var grdDepartments = grdFinalDepartment.ItemsSource as List<Department>;
                foreach (Department department in grdDepartments)
                {
                    departments.Add(department);
                }
          
            return departments;
        }

        public List<ERP_BL.Databases.Employee> GetEmployees()
        {
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();

            
                var grdEmployees = grdFinalEmployee.ItemsSource as List<ERP_BL.Databases.Employee>;
                foreach (ERP_BL.Databases.Employee employee in grdEmployees)
                {
                    employees.Add(employee);
                }
      
           
            
            return employees;
        }

        private void GrdDepartment_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();

            //if (grdDepartment.SelectedItems != null)
            //{
            //    foreach (Department department in grdDepartment.SelectedItems)
            //    {
            //        employees.AddRange(department.employees.Distinct());
            //    }
            //    grdEmployee.ItemsSource = employees.Distinct();
            //}
            //else
            //{
            //    grdEmployee.ItemsSource = null;
            //    employees.Clear();
            //}
            //if (chartofAccount.Employees.Count != 0)
            //{
            //    foreach (var _Employee in chartofAccount.Employees)
            //    {
            //        grdEmployee.SelectItem(grdEmployee.FindRowByValue(grdEmployee.Columns.GetColumnByFieldName("EmpId"), _Employee.EmpId));
            //    }
            //    isEmployee.IsChecked = true;
            //}
            //else
            //    isEmployee.IsChecked = false;


        }

        private void GrdCompany_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //List<Department> departments = new List<Department>();

            //if (grdCompany.SelectedItems != null)
            //{
            //    foreach (Company company in grdCompany.SelectedItems)
            //    {
            //        departments.AddRange(company.departments.Distinct());
            //    }
            //    grdDepartment.ItemsSource = departments.Distinct();
            //}
            //else
            //{
            //    grdDepartment.ItemsSource = null;
            //    departments.Clear();
            //}
            //if (chartofAccount.Departments.Count != 0)
            //{
            //    foreach (var _Department in chartofAccount.Departments)
            //    {
            //        grdDepartment.SelectItem(grdDepartment.FindRowByValue(grdDepartment.Columns.GetColumnByFieldName("Id"), _Department.Id));
            //    }
            //    isDepartment.IsChecked = true;
            //}
            //else
            //    isDepartment.IsChecked = false;
        }

        private void grdFinalDepartment_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void btnSelectCompany_Click(object sender, RoutedEventArgs e)
        {
            var selectedCompany=grdCompany.SelectedItem as Company;
            if (selectedCompany != null)
            {
                finalCompanies.Add(selectedCompany);
                allCompanies.Remove(selectedCompany);
                finalDepartments.Clear();
                allDepartments.Clear();
                allEmployees.Clear();
                finalEmployees.Clear();
                grdDepartment.ItemsSource = allDepartments;
                grdFinalDepartment.ItemsSource = finalDepartments;
                grdEmployee.ItemsSource = allEmployees;
                grdFinalEmployee.ItemsSource = finalEmployees;
                PopulateData();
            }
        }

        private void btnUnSelectCompany_Click(object sender, RoutedEventArgs e)
        {
          
            var selectedCompany = grdFinalCompanies.SelectedItem as Company;
            if (selectedCompany != null)
            {
                allCompanies.Add(selectedCompany);
                finalCompanies.Remove(selectedCompany);
                finalDepartments.Clear();
                allDepartments.Clear();
                allEmployees.Clear();
                finalEmployees.Clear();
                grdDepartment.ItemsSource = allDepartments;
                grdFinalDepartment.ItemsSource = finalDepartments;
                PopulateData();
            }
        }

        public void PopulateData()
        {
            try
            {
                foreach (var comp in finalCompanies)
                {
                    allDepartments.AddRange(comp.departments.Where(x=>x.isActive == true));

                    if (chartofAccount.Departments.Count != 0)
                    {
                        foreach (var dept in chartofAccount.Departments)
                        {
                            foreach (var adept in allDepartments)
                            {
                                if (adept.Id == dept.Id)
                                {
                                    finalDepartments.Add(adept);
                                    allEmployees.AddRange(adept.employees);
                                    //allDepartments.Remove(adept);
                                }
                            }
                        }
                       
                    }
                   
                    if (chartofAccount.Employees.Count != 0)
                    {
                        foreach (var emp in chartofAccount.Employees)
                        {
                            foreach (var aEMP in allEmployees)
                            {
                                if (aEMP.EmpId == emp.EmpId)
                                {
                                    finalEmployees.Add(aEMP);
                                }
                            }
                        }
                     
                    }
                  
                }
                allCompanies = allCompanies.GroupBy(x => x.Id)
                            .Select(g => g.First())
                            .ToList();
                finalCompanies = finalCompanies.GroupBy(x => x.Id)
                            .Select(g => g.First())
                            .ToList();
                allDepartments = allDepartments.GroupBy(x => x.Id)
                             .Select(g => g.First())
                             .ToList();

                finalDepartments = finalDepartments.GroupBy(x => x.Id)
                            .Select(g => g.First())
                            .ToList();
                allEmployees = allEmployees.GroupBy(x => x.EmpId)
                        .Select(g => g.First())
                        .ToList();

                finalEmployees = finalEmployees.GroupBy(x => x.EmpId)
                            .Select(g => g.First())
                            .ToList();
                allCompanies = allCompanies.Except(finalCompanies).ToList();
                finalCompanies = finalCompanies.Except(allCompanies).ToList();
                allDepartments = allDepartments.Except(finalDepartments).ToList();
                finalDepartments = finalDepartments.Except(allDepartments).ToList();
                //finalEmployees = finalEmployees.Except(allEmployees).ToList();
                allEmployees = allEmployees.Except(finalEmployees).ToList();
                grdCompany.ItemsSource = allCompanies;
                grdFinalCompanies.ItemsSource = finalCompanies;
                allDepartments = allDepartments.Where(x => x.isActive == true).ToList();
                grdDepartment.ItemsSource = allDepartments;
                grdFinalDepartment.ItemsSource = finalDepartments;
                grdEmployee.ItemsSource = allEmployees;
                grdFinalEmployee.ItemsSource = finalEmployees;
            }
            catch (Exception)
            {

             
            }
           
        }
        private void btnSelectDepartment_Click(object sender, RoutedEventArgs e)
        {
            var selectedDepartment = grdDepartment.SelectedItem as Department;
            if (selectedDepartment != null)
            {
                finalDepartments.Add(selectedDepartment);
                allDepartments.Remove(selectedDepartment);

                finalEmployees.Clear();
                allEmployees.Clear();
                grdEmployee.ItemsSource = allEmployees;
                grdFinalEmployee.ItemsSource = finalEmployees;


                PopulateDepartments();


            
            }
        }

        public void PopulateDepartments()
        {
            if (chartofAccount.Departments.Count != 0)
            {
                foreach (var dept in chartofAccount.Departments)
                {
                    foreach (var adept in finalDepartments)
                    {
                       
                            //finalDepartments.Add(adept);
                            allEmployees.AddRange(adept.employees);

                        
                    }
                }
           
            }
            else
            {
                foreach (var adept in finalDepartments)
                {
                    allEmployees.AddRange(adept.employees);
                }

            }
            if (chartofAccount.Employees.Count != 0)
            {
                foreach (var emp in chartofAccount.Employees)
                {
                    foreach (var aEMP in allEmployees)
                    {
                        if (aEMP.EmpId == emp.EmpId)
                        {
                            finalEmployees.Add(aEMP);
                        }
                    }
                }
             
            }
            else
            {

            }
          

            finalDepartments = finalDepartments.GroupBy(x => x.Id)
             .Select(g => g.First())
             .ToList();

            allDepartments = allDepartments.GroupBy(x => x.Id)
                      .Select(g => g.First())
                      .ToList();


            allDepartments = allDepartments.Except(finalDepartments).ToList();
            finalDepartments = finalDepartments.Except(allDepartments).ToList();



            allEmployees = allEmployees.GroupBy(x => x.EmpId)
                    .Select(g => g.First())
                    .ToList();

            finalEmployees = finalEmployees.GroupBy(x => x.EmpId)
                        .Select(g => g.First())
                        .ToList();


            allEmployees = allEmployees.Except(finalEmployees).ToList();
            grdDepartment.ItemsSource = allDepartments;
            grdFinalDepartment.ItemsSource = finalDepartments;
            grdEmployee.ItemsSource = allEmployees;
            grdFinalEmployee.ItemsSource = finalEmployees;
        }
        private void btnUnSelectDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedDepartment = grdFinalDepartment.SelectedItem as Department;
                if (selectedDepartment != null)
                {
                    allDepartments.Add(selectedDepartment);
                    finalDepartments.Remove(selectedDepartment);
                    finalEmployees.Clear();
                    allEmployees.Clear();
                    grdEmployee.ItemsSource = allEmployees;
                    grdFinalEmployee.ItemsSource = finalEmployees;


                    PopulateDepartments();
                }
            }
            catch (Exception)
            {

               
            }

        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEmployee = grdFinalEmployee.SelectedItem as ERP_BL.Databases.Employee;
                if (selectedEmployee != null)
                {
                    allEmployees.Add(selectedEmployee);

                    finalEmployees.Remove(selectedEmployee);
                    allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(g => g.First()).ToList();
                    finalEmployees = finalEmployees.GroupBy(x => x.EmpId).Select(g => g.First()).ToList();

                    finalEmployees = finalEmployees.Except(allEmployees).ToList();
                    allEmployees = allEmployees.Except(finalEmployees).ToList();

                    grdEmployee.ItemsSource = allEmployees;
                    grdFinalEmployee.ItemsSource = finalEmployees;
                }
            }
            catch (Exception)
            {

            }
        
        }

        private void btnSelectEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedEmployee = grdEmployee.SelectedItem as ERP_BL.Databases.Employee;
                if (selectedEmployee != null)
                {
                    finalEmployees.Add(selectedEmployee);
                    allEmployees.Remove(selectedEmployee);
                    allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(g => g.First()).ToList();
                    finalEmployees = finalEmployees.GroupBy(x => x.EmpId).Select(g => g.First()).ToList();
                    finalEmployees = finalEmployees.Except(allEmployees).ToList();
                    allEmployees = allEmployees.Except(finalEmployees).ToList();
                    grdEmployee.ItemsSource = allEmployees;
                    grdFinalEmployee.ItemsSource = finalEmployees;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

