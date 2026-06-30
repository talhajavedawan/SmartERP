using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddAccount.xaml
    /// </summary>
    public partial class ucFrmAddAccount : UserControl
    {
        public int flag;
        //public string compName;
        //public string deptName;
        public int accntId;
        Account account = new Account();

        public UcListWindow addAccntWin = new UcListWindow();
        public UcListWindow updateAccntWin = new UcListWindow();

        Company company = new Company();

        SalesReceiptRepo repo = new SalesReceiptRepo();
        List<Bank> allBanks = new List<Bank>();
        //List<Company> allCompanies = new List<Company>();
        UsersRepo userRep = new UsersRepo();
        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

        public ucFrmAddAccount()
        {
            InitializeComponent();
            addAccntWin.Closing += AddAccount_Window_Closing;
            updateAccntWin.Closing += UpdateAccnt_Window_Closing;

            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;
            gridDepartment.SelectionChanged += OndeptGridSelectionChanged;
        }

        private void Initialize_AllComboboxes()
        {
            loadIndustryTypes();
            LoadAccountsNatures();
            //Getting All Active Companies, Deparments and currencies

            var currentUserId = SYSTEM_STATIC.currentUser.id;
            var loginUser = userRep.getuser(currentUserId);
            cmbxCompany.ItemsSource = loginUser.employee.Companies;

            //Populating Combobox Account Type
            for (int i = 0; i <= (int)ERP_BL.Enums.AccountsType.Fixed_Term; i++)
            {
                cmbxAccntType.Items.Add(((ERP_BL.Enums.AccountsType)i).ToString());
            }

            //Populating Combobox Account Category
            for (int i = 0; i <= (int)ERP_BL.Enums.AccountsCategory.Customer; i++)
            {
                cmbxCategory.Items.Add(((ERP_BL.Enums.AccountsCategory)i).ToString());
            }

            //Populating Combobox Currency
            CurrencyRepo currencyRepo = new CurrencyRepo();
            cmbxCurrency.ItemsSource = currencyRepo.getAll().ToList();

            cmbxBank.ItemsSource = repo.GetAllBanks();

           

            
            //If Item click "Edit" is clicked
            if (flag == 1 && accntId > 0)
            {
                account = repo.GetAccount(accntId);

                chkIsActive.IsChecked = account.isActive;
                chkIsAdjustment.IsChecked = account.isAdjustmentAccount;

                for (int i = 0; i <= (int)ERP_BL.Enums.AccountsType.Fixed_Term; i++)
                {
                    if (((ERP_BL.Enums.AccountsType)i).ToString() == account.accountType.ToString())
                    {
                        cmbxAccntType.SelectedIndex = i;
                        break;
                    }
                }

                if(account.COA_Type != null)
                    for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
                    {
                        if (((ERP_BL.Enums.COA_AccountType)i).ToString() == account.COA_Type.ToString())
                        {
                            cmbxAccntNature.SelectedIndex = i;
                            break;
                        }
                    }
                else
                {
                    if (account.nature == 0)
                    {
                        cmbxAccntNature.Text = ((ERP_BL.Enums.COA_AccountType)3).ToString();
                    }
                    else
                    {
                        cmbxAccntNature.Text = ((ERP_BL.Enums.COA_AccountType)account.nature).ToString();
                    }
                }

                for (int i = 0; i <= (int)ERP_BL.Enums.AccountsCategory.Customer; i++)
                {
                    if (((ERP_BL.Enums.AccountsCategory)i).ToString() == account.accountsCategory.ToString())
                    {
                        cmbxCategory.SelectedIndex = i;
                        break;
                    }
                }

                //Select the Bank linked with the Account has to be updated
                if (account.mainBankId != null)
                {
                    cmbxBank.EditValue = account.mainBankId;
                }

                //Select the Bank linked with the Account has to be updated
                if (account.bank != null)
                {
                    var branches = (cmbxBranch.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBranch.ItemsSource as List<Bank>;
                    if (branches.FirstOrDefault(x => x.Id == account.bank.Id) == null)
                    {
                        branches.Add(account.bank);
                        cmbxBranch.ItemsSource = branches;
                    }
                        

                    cmbxBranch.EditValue = account.bank.Id;
                    //var bankList = (cmbxBankName.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBankName.ItemsSource as List<Bank>;
                    //int index = 0;
                    //foreach (var _bank in bankList)
                    //{
                    //    if (_bank.Id == account.bank.Id)
                    //    {
                    //        cmbxBankName.SelectedIndex = index;
                    //        index = 0;
                    //        break;
                    //    }
                    //    index++;
                    //}
                    //cmbxBankName.Text = account.bank.BankName;
                }
                    

                //Select Company
                if (account.company != null)
                {
                    var companylist = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                    if (account.company != null && companylist.Find(x => x.Id == account.company.Id) == null)
                    {
                        companylist.Add(account.company);
                        cmbxCompany.ItemsSource = companylist;
                    }
                    cmbxCompany.Text = account.company.CompanyName;
                }

                //Select Currency
                if (account.currency != null)
                    cmbxCurrency.Text = account.currency.CurrencyName;


                //Departments of the Selected Company
                foreach (var _dept in account.departments)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                }

                if(account.COAaccount != null)
                    cmbcoaAccounts.Text = account.COAaccount.accountName;

                if ((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex == ERP_BL.Enums.AccountsCategory.Vendor)
                {
                    layoutCntrlVendor.Visibility = Visibility.Visible;
                    //Select Industry
                    var industryTypeList = (cmbIndustry.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbIndustry.ItemsSource as List<cmbitem>;
                    if (account.industryType != null)
                    {
                        int index = 0;
                        foreach (var _industry in industryTypeList)
                        {
                            if (_industry.id == account.industryType.Id)
                            {
                                cmbIndustry.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    //Select Vendor
                    var vendorList = (cmbxVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : cmbxVendor.ItemsSource as List<Vendor>;
                    if (account.vendor != null)
                    {
                        int index = 0;
                        foreach (var _vendor in vendorList)
                        {
                            if (_vendor.Id == account.vendor.Id)
                            {
                                cmbxVendor.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                }
                
            }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize_AllComboboxes();

        }


        public void loadIndustryTypes()
        {


            List<IndustryType> industryTypes = new List<IndustryType>();
            CompanyRepo repo = new CompanyRepo();
            industryTypes = repo.GetVendorIndustryTypes();
            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }


        private void OndeptgirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                gridDepartment.SelectItem(e.Node.RowHandle);
            else
                gridDepartment.UnselectItem(e.Node.RowHandle);
        }

        private void OndeptGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = griddeptview;
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
                    var selectedRows = gridDepartment.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        //Save Button Click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxBank.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank!");
                    cmbxBank.Focus();
                    return;
                }
                else if (cmbxBranch.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Branch!");
                    cmbxBranch.Focus();
                    return;
                }
                else if(cmbxAccntType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account Type!");
                    cmbxAccntType.Focus();
                    return;
                }
                else if (cmbxCurrency.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Currency!");
                    cmbxCurrency.Focus();
                    return;
                }
                else if (cmbxCompany.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Company!");
                    cmbxCompany.Focus();
                    return;
                }
                else if (gridDepartment.SelectedItems.Count < 1)
                {
                    DXMessageBox.Show("Please select Departments!");
                    return;
                }
                else if (String.IsNullOrEmpty(txtAccntNo.Text))
                {
                    DXMessageBox.Show("Please enter Account Number!");
                    txtAccntNo.Focus();
                    return;
                }
                else if (cmbxCategory.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Account Category!");
                    cmbxCategory.Focus();
                    return;
                }

                if((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex == ERP_BL.Enums.AccountsCategory.Vendor)
                {
                    if(cmbIndustry.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Industry Type!");
                        cmbIndustry.Focus();
                        return;
                    }
                    else if (cmbxVendor.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Vendor!");
                        cmbxVendor.Focus();
                        return;
                    }
                }

                //If click on Add new Account
                if (flag == 0)
                {
                    //Getting the details of the Account to be created from the form 
                    account.mainBankId = (cmbxBank.SelectedItem as MainBank).Id;
                    account.bank = cmbxBranch.SelectedItem as Bank;
                    account.AccountNick = txtAccntNick.Text;
                    account.AccountNo = txtAccntNo.Text;
                    account.company = cmbxCompany.SelectedItem as Company;

                    account.isActive = chkIsActive.IsChecked.Value;
                    account.isAdjustmentAccount = chkIsAdjustment.IsChecked.Value;

                    if (gridDepartment.SelectedItems.Count != 0)
                    { //company.departments = depts;
                        account.departments = new List<Department>();
                        foreach (Department dept in gridDepartment.SelectedItems)
                        {
                            if (!account.departments.Contains(dept))
                            {
                                account.departments.Add(dept);
                            }
                        }
                    }

                    //acc.departments = deptList1;
                    account.currency = cmbxCurrency.SelectedItem as Currency;
                    if(cmbcoaAccounts.SelectedIndex!=-1)
                    {
                        var chartofAccount = cmbcoaAccounts.SelectedItem as ChartofAccount;
                        if (chartofAccount.currencyId == null)
                        {
                            DXMessageBox.Show("COA does not have any Currency Linked!");
                            return;
                        }
                        if (account.currency.Id == chartofAccount.currencyId)
                        {
                            account.COA_accountId = chartofAccount.Id;
                        }
                        else
                        {
                            DXMessageBox.Show("Currency not matching with COA Currency!");
                            return;
                        }
                    }
                    account.accountType = ((ERP_BL.Enums.AccountsType)cmbxAccntType.SelectedIndex);
                    account.COA_Type = ((ERP_BL.Enums.COA_AccountType)cmbxAccntNature.SelectedIndex);
                    //acc.COANo = txtCOAno.Text;
                    account.IBAN = txtIban.Text;


                    account.accountsCategory = ((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex);

                    if ((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex == ERP_BL.Enums.AccountsCategory.Vendor)
                    {
                        account.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                        account.vendor_Id = (cmbxVendor.SelectedItem as Vendor).Id;
                    }
                    else
                    {
                        account.industryTypeId = null;
                        account.vendor_Id = null;
                    }


                    SalesReceiptRepo repo1 = new SalesReceiptRepo();

                    //Adding new Account
                    repo1.addAccount(account);
                    //addAccntFrm = 0;
                    MessageBox.Show("Successfully Added");

                    //Closing window after adding account
                    addAccntWin.Close();
                }

                //If click on Update selected Account
                else if (flag == 1) {
                    //Getting the details of the Account to be updated from the form
                    Account acc = new Account();
                    acc.Id = accntId;
                    acc.mainBankId = (cmbxBank.SelectedItem as MainBank).Id;
                    acc.bank = cmbxBranch.SelectedItem as Bank;
                    acc.AccountNick = txtAccntNick.Text;
                    acc.AccountNo = txtAccntNo.Text;
                    acc.company = cmbxCompany.SelectedItem as Company;
                    acc.isActive = chkIsActive.IsChecked.Value;
                    acc.isAdjustmentAccount = chkIsAdjustment.IsChecked.Value;
                    if (gridDepartment.SelectedItems.Count != 0)
                    {
                        acc.departments = new List<Department>();
                        foreach (Department dept in gridDepartment.SelectedItems)
                        {
                            if (!acc.departments.Contains(dept))
                            {
                                acc.departments.Add(dept);
                            }
                        }
                    }

                    acc.currency = cmbxCurrency.SelectedItem as Currency;
                    if (cmbcoaAccounts.SelectedIndex != -1)
                    {
                        var chartofAccount = cmbcoaAccounts.SelectedItem as ChartofAccount;
                        if(chartofAccount.currencyId == null)
                        {
                            DXMessageBox.Show("COA does not have any Currency Linked!");
                            return;
                        }
                            

                        if (acc.currency.Id == chartofAccount.currencyId)
                        {
                            acc.COA_accountId = chartofAccount.Id;
                        }
                        else
                        {
                            DXMessageBox.Show("Please select a Currency that Matching COA Currency!");
                            return;
                        }
                    }
                    acc.accountType = ((ERP_BL.Enums.AccountsType)cmbxAccntType.SelectedIndex);
                    acc.COA_Type = ((ERP_BL.Enums.COA_AccountType)cmbxAccntNature.SelectedIndex);
                    //acc.COANo = txtCOAno.Text;
                    acc.IBAN = txtIban.Text;

                    acc.accountsCategory = ((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex);

                    if((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex == ERP_BL.Enums.AccountsCategory.Vendor)
                    {
                        acc.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                        acc.vendor_Id = (cmbxVendor.SelectedItem as Vendor).Id;
                    }
                    else
                    {
                        acc.industryTypeId = null;
                        acc.vendor_Id = null;
                    }

                    SalesReceiptRepo repo1 = new SalesReceiptRepo();

                    //Updating Account
                    repo1.updateAccount(acc);
                    //updateAccntFrm = 0;
                    MessageBox.Show("Successfully Updated");                    

                    //Closing window after updating account
                    updateAccntWin.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        //Closing function of Add new account window
        private void AddAccount_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //addAccntFrm = 0;
            e.Cancel = false;
        }

        //Closing function of Update account window
        private void UpdateAccnt_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //updateAccntFrm = 0;
            e.Cancel = false;
        }

        //Combobox Company Selected Index changed
        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            loadDepartments();
        }

        private void loadDepartments()
        {
            company = cmbxCompany.SelectedItem as ERP_BL.Databases.Company;
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                gridDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments)
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    //if (receiptsList != null && receiptsList.Count > 0 && receiptsList[0].Id > 0 && saveEditFlag == 1)
                    //    if (receiptsList[0].department != null && departments.FirstOrDefault(x => x.Id == receiptsList[0].department.Id) == null)
                    //        departments.Add(receiptsList[0].department);

                    gridDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }

        //Got focus funtion of Combobox department
        private void CmbxDeptartment_GotFocus(object sender, RoutedEventArgs e)
        {
            //If no company is selected
            if(cmbxCompany.SelectedIndex < 0)
            {
                MessageBox.Show("Select company first");
            }
        }

        private void CmbxBranch_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxBranch.SelectedIndex > -1)
            {
                var companies = (cmbxBranch.SelectedItem as Bank).companies;
                cmbxCompany.ItemsSource = companies;
            }
        }

        private void CmbxCompany_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxBranch.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank first!");
                cmbxBranch.Focus();
                return;
            }
        }
        private void CmbcoaAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void cmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIndustry.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIndustry.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmIndustryTypeAdd industryadd = new frmIndustryTypeAdd();
                    industryadd.ShowDialog();
                    loadIndustryTypes();
                }

                List<Vendor> vendors = new List<Vendor>();
                VendorRepo vendorRepo = new VendorRepo();
                vendors = vendorRepo.getAllByIndustryType(idd);
                cmbxVendor.ItemsSource = vendors;
            }
        }

        private void CmbxCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var selectedCategory = ((ERP_BL.Enums.AccountsCategory)cmbxCategory.SelectedIndex);
            if(selectedCategory == ERP_BL.Enums.AccountsCategory.Vendor)
            {
                layoutCntrlVendor.Visibility = Visibility.Visible;
            }
            else
            {
                layoutCntrlVendor.Visibility = Visibility.Collapsed;
            }
        }

        private void CmbxVendor_GotFocus(object sender, RoutedEventArgs e)
        {
            if(cmbIndustry.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Industry Type first!");
                cmbIndustry.Focus();
            }
        }
        public void LoadAccountsNatures()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.COA_AccountType.Other_Expense; i++)
            {
              cmbxAccntNature.Items.Add(((ERP_BL.Enums.COA_AccountType)i).ToString());
            }
        }

        private void CmbxAccntNature_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedType = ((ERP_BL.Enums.COA_AccountType)cmbxAccntNature.SelectedIndex);
                chartofAccounts = coaRepo.getChartofAccountsByType(SYSTEM_STATIC.currentUser.id, selectedType);
                
                    cmbcoaAccounts.ItemsSource = chartofAccounts;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbxBank_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var mainBank = cmbxBank.SelectedItem as MainBank;
            allBanks = repo.GetAllBranchesbyUserAndBank(MainWindow.currentUserid, mainBank.Id);
            cmbxBranch.ItemsSource = allBanks;


            //if (allBanks != null)
            //{
            //    if (flag == 1 && account.bank != null && allBanks.FirstOrDefault(x => x.Id == account.bank.Id) == null)
            //        allBanks.Add(account.bank);
            //    cmbxBranch.ItemsSource = allBanks;
            //}

        }
    }
}
