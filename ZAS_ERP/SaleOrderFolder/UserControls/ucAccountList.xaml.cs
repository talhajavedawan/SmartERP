using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucAccountList.xaml
    /// </summary>
    public partial class ucAccountList : UserControl
    {
        ucFrmAddAccount addAccntObj = new ucFrmAddAccount();
        ucFrmAddAccount updateAccntObj = new ucFrmAddAccount();
        SalesReceiptRepo repo = new SalesReceiptRepo();

        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        public List<Company> companyList = new List<Company>();
        public int saveEditFlag;
        //List<Departments> dept = new List<Departments>();
        public ucAccountList()
        {
            InitializeComponent();
            //grdCntrlAccountList.Columns["BankName"].Visible = false;
        }


        public void LoadAllDepts()
        {
            DepartmentRepo depts = new DepartmentRepo();
            try
            {
                var allDepts = depts.GetActiveDepartments();
                //gridDepartment.ItemsSource = allDepts;
            }
            catch
            {

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllDepts();
            Load_accounts();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
        }

        private void Add_New_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            

        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
        }

        private void Load_accounts()
        {
            repo = new SalesReceiptRepo();
            var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
            //List<Account> accounts = new List<Account>();
            //foreach (var _accnt in accntList)
            //{
            //    if (((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory) == ERP_BL.Enums.AccountsCategory.Company)
            //    {
            //        accounts.Add(_accnt);
            //    }
            //}

            //comment for commit
            //GetAllAccounts accnts = new GetAllAccounts(accounts);
            grdCntrlAccountList.ItemsSource = accntList;
            //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
            lblHeading.Text = "Accounts List (Company)";

            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
        }


        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load_accounts();
        }

        private void BtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlAccountList);
        }

        private void Add_New_AccountClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Account") != null)
                {
                    addAccntObj = new ucFrmAddAccount();
                    //addAccntObj.addAccntFrm = 1;
                    addAccntObj.flag = 0;
                    saveEditFlag = 0;
                    addAccntObj.addAccntWin.Width = 800;
                    addAccntObj.addAccntWin.Height = 580;
                    addAccntObj.addAccntWin.ResizeMode = ResizeMode.CanMinimize;
                    addAccntObj.addAccntWin.Content = addAccntObj;
                    addAccntObj.addAccntWin.Title = "Add Bank Account";
                    addAccntObj.addAccntWin.ShowDialog();
                    //Load_accounts();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Account!");
                    return;
                }
            }
            catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void Edit_AccountClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Account") != null)
                {

                    var selectedRow = grdCntrlAccountList.SelectedItem as Account;


                    if (selectedRow != null)
                    {
                        //if(updateAccntObj.updateAccntFrm == 0)
                        //{
                        updateAccntObj = new ucFrmAddAccount();

                        //updateAccntObj.updateAccntFrm = 1;

                        updateAccntObj.flag = 1;
                        saveEditFlag = 1;


                        updateAccntObj.accntId = selectedRow.Id;
                        //updateAccntObj.cmbxBankName.SelectedIndex = accnt.bank.Id;

                        //if(chartofAccount != null)
                        //updateAccntObj.cmbcoaAccounts.SelectedItem = chartofAccount.accountName;
                        //frm.cmbxCompany.SelectedIndex = company.Id;
                        //updateAccntObj.compName = company.CompanyName;


                        //var deptList = accnt.departments;                //----FIXIT---
                        //updateAccntObj.deptName = dept.DeptName;
                        //  updateAccntObj.selDept = dept;
                        //------------------------
                        //frm.cmbxDeptartment.SelectedIndex = dept.Id - 1;
                        updateAccntObj.txtAccntNo.Text = selectedRow.AccountNo;
                        updateAccntObj.txtIban.Text = selectedRow.IBAN;
                        updateAccntObj.txtAccntNick.Text = selectedRow.AccountNick;

                        //frm.cmbxAccntNature.SelectedIndex = index;
                        //updateAccntObj.txtCOAno.Text = selectedRow.COA;


                        //updateAccntObj.chkIsPersonal.IsChecked = selectedRow.isPersonal;
                        



                        updateAccntObj.updateAccntWin.Width = 800;
                        updateAccntObj.updateAccntWin.Height = 580;
                        updateAccntObj.updateAccntWin.ResizeMode = ResizeMode.CanMinimize;
                        updateAccntObj.updateAccntWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        //ucFrmAddAccount obj = new ucFrmAddAccount();
                        updateAccntObj.updateAccntWin.Title = "Update Account";
                        updateAccntObj.updateAccntWin.Content = updateAccntObj;
                        updateAccntObj.updateAccntWin.ShowDialog();
                        //Load_accounts();

                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to update existing Account!");
                    return;
                }
            }
            catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlAccountList);
        }

        private void MbtnCompanyAccounts_Click(object sender, RoutedEventArgs e)
        {
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Company Accounts") != null)
            {
                var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
                //List<Account> accounts = new List<Account>();
                //foreach (var _accnt in accntList)
                //{
                //    if (((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory) == ERP_BL.Enums.AccountsCategory.Company)
                //    {
                //        accounts.Add(_accnt);
                //    }
                //}


                //GetAllAccounts accnts = new GetAllAccounts(accounts);
                grdCntrlAccountList.ItemsSource = accntList.Where(x => x.accountsCategory == ERP_BL.Enums.AccountsCategory.Company).ToList();
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
                lblHeading.Text = "Accounts List (Company)";

                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Company Accounts!");
            }
                
        }

        private void MbtnPersonalAccounts_Click(object sender, RoutedEventArgs e)
        {
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Personal Accounts") != null)
            {
                var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
                //List<Account> accounts = new List<Account>();
                //foreach (var _accnt in accntList)
                //{
                //    if (((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory) == ERP_BL.Enums.AccountsCategory.Personal)
                //    {
                //        accounts.Add(_accnt);
                //    }
                //}


                //GetAllAccounts accnts = new GetAllAccounts(accounts);
                grdCntrlAccountList.ItemsSource = accntList.Where(x=>x.accountsCategory == ERP_BL.Enums.AccountsCategory.Personal).ToList();
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
                lblHeading.Text = "Accounts List (Personal)";

                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Personal Accounts!");
            }
            
        }

        private void MbtnVendorAccounts_Click(object sender, RoutedEventArgs e)
        {
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Vendor Accounts") != null)
            {
                var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
                //List<Account> accounts = new List<Account>();
                //foreach (var _accnt in accntList)
                //{
                //    if (((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory) == ERP_BL.Enums.AccountsCategory.Vendor)
                //    {
                //        accounts.Add(_accnt);
                //    }
                //}


                //GetAllAccounts accnts = new GetAllAccounts(accounts);
                grdCntrlAccountList.ItemsSource = accntList.Where(x => x.accountsCategory == ERP_BL.Enums.AccountsCategory.Vendor).ToList();
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
                lblHeading.Text = "Accounts List (Vendor)";

                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Vendor Accounts!");
            }
           
        }

        private void MbtnCustomerAccounts_Click(object sender, RoutedEventArgs e)
        {
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Customer Accounts") != null)
            {
                var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
                //List<Account> accounts = new List<Account>();
                //foreach (var _accnt in accntList)
                //{
                //    if (((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory) == ERP_BL.Enums.AccountsCategory.Customer)
                //    {
                //        accounts.Add(_accnt);
                //    }
                //}


                //GetAllAccounts accnts = new GetAllAccounts(accounts);
                grdCntrlAccountList.ItemsSource = accntList.Where(x => x.accountsCategory == ERP_BL.Enums.AccountsCategory.Customer).ToList();
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
                lblHeading.Text = "Accounts List (Customer)";

                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Customer Accounts!");
            }
            
        }

        private void MbtnAllAccounts_Click(object sender, RoutedEventArgs e)
        {
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of All Accounts") != null)
            {
                var accntList = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
                //GetAllAccounts accnts = new GetAllAccounts(accntList);
                grdCntrlAccountList.ItemsSource = accntList;
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
                lblHeading.Text = "Accounts List (All)";
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAccountList);
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of All Accounts!");
            }
            

        }

        private void GrdCntrlAccountList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData && e.Column.FieldName == "Departmentss")
            {
                var _accnt = grdCntrlAccountList.GetRow(e.ListSourceRowIndex) as Account;
                if(_accnt != null)
                {
                    var depts = String.Join(" | ", _accnt.departments.Select(x => x.DeptName));
                    e.Value = depts;
                }
            }

            if (e.IsGetData && e.Column.FieldName == "accountNature")
            {
                var _accnt = grdCntrlAccountList.GetRow(e.ListSourceRowIndex) as Account;

                if (_accnt != null)
                {
                    if (_accnt.COA_Type != null)
                    {
                        e.Value = ((ERP_BL.Enums.COA_AccountType)_accnt.COA_Type).ToString();
                    }
                    else
                    {
                        if (_accnt.nature == 0)
                        {
                            e.Value = ((ERP_BL.Enums.COA_AccountType)3).ToString();
                        }
                        else
                        {
                            e.Value = ((ERP_BL.Enums.COA_AccountType)_accnt.nature).ToString();
                        }
                    }
                }
            }
        }
    }
    //public class AllAccounts
    //{
    //    public int SerialNo { get; set; }
    //    public string BankName { get; set; }
    //    //public string Location { get; set; }
    //    public string Company { get; set; }
    //    public string Department { get; set; }
    //    public string AccountType { get; set; }
    //    public string Currency { get; set; }
    //    public string AccountNo { get; set; }
    //    public string IBAN { get; set; }
    //    public string AccountName { get; set; }
    //    public string AccountNature { get; set; }
    //    public string COA { get; set; }
    //    public string Category { get; set; }
    //    public string IndustryType { get; set; }
    //    public string Vendor { get; set; }
    //}

    //public class GetAllAccounts
    //{
    //    public List<AllAccounts> AccountList { get; private set; }
    //    List<Account> allAccnts = new List<Account>();

    //    //public event PropertyChangedEventHandler PropertyChanged;
    //    public GetAllAccounts()
    //    {
    //        //try
    //        //{
    //        //    List<AllAccounts> accounts = new List<AllAccounts>();
    //        //    allAccnts = new List<Account>();
    //        //    SalesReceiptRepo repo = new SalesReceiptRepo();
    //        //    //var allAccnts = repo.GetAllAccounts();
    //        //    allAccnts = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
    //        //    //allAccnts = accntList;
    //        //    if (allAccnts.Count > 0)
    //        //    {
    //        //        foreach (var _accnt in allAccnts)
    //        //        {
    //        //            AllAccounts accnt = new AllAccounts();

    //        //            accnt.SerialNo = _accnt.Id;
    //        //            accnt.BankName = _accnt.bank.BankName + " (" + _accnt.bank.BranchCode + ")";
    //        //            //accnt.Location = _accnt.bank.Location;
    //        //            accnt.Company = _accnt.company.CompanyName;
    //        //            //-----FIXIT----
    //        //            if (_accnt.departments.Count != 0)
    //        //            {
    //        //                List<string> _deptList = new List<string>();
    //        //                foreach (var _dept in _accnt.departments)
    //        //                {
    //        //                    if (accnt.Department == null)
    //        //                    {
    //        //                        accnt.Department = _dept.DeptName;
    //        //                    }
    //        //                    else if (_dept.parentDepartment != null)
    //        //                    {
    //        //                        accnt.Department = accnt.Department + ", " + _dept.parentDepartment.DeptName + " " + _dept.DeptName;
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        accnt.Department = accnt.Department + ", " + _dept.DeptName;
    //        //                    }

    //        //                }
    //        //            }

    //        //            //accnt.Department = _accnt.departments;
    //        //            accnt.AccountType = ((ERP_BL.Enums.AccountsType)_accnt.accountType).ToString();
    //        //            accnt.Currency = _accnt.currency.CurrencyName;
    //        //            //accnt.chartofAccount = _accnt.COAaccount;

    //        //            accnt.AccountNo = _accnt.AccountNo;
    //        //            accnt.IBAN = _accnt.IBAN;
    //        //            accnt.AccountNature = ((ERP_BL.Enums.AccountsNature)_accnt.nature).ToString();
    //        //            accnt.AccountName = _accnt.AccountNick;


    //        //            accnt.Category = ((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory).ToString();


    //        //            if (_accnt.COAaccount != null)
    //        //            {
    //        //                accnt.COA = _accnt.COAaccount.accountName;
    //        //            }

    //        //            //accnt.isPersonal = _accnt.isPersonal;

    //        //            accounts.Add(accnt);
    //        //        }
    //        //    }
    //        //    AccountList = accounts;
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    DXMessageBox.Show(ex.Message);
    //        //}
    //    }

    //    public GetAllAccounts(List<Account> accntList)
    //    {
    //        try
    //        {
    //            List<AllAccounts> accounts = new List<AllAccounts>();
    //            allAccnts = new List<Account>();
    //            SalesReceiptRepo repo = new SalesReceiptRepo();
    //            //var allAccnts = repo.GetAllAccounts();
    //            //allAccnts = repo.GetAllAccountsByUserId(MainWindow.currentUserid);
    //            allAccnts = accntList;
    //            if (allAccnts.Count > 0)
    //            {
    //                foreach (var _accnt in allAccnts)
    //                {
    //                    AllAccounts accnt = new AllAccounts();

    //                    accnt.SerialNo = _accnt.Id;
    //                    accnt.BankName = _accnt.bank.BankName + " (" + _accnt.bank.BranchCode + ")";
    //                    //accnt.Location = _accnt.bank.Location;
    //                    accnt.Company = _accnt.company.CompanyName;
    //                    //-----FIXIT----
    //                    if (_accnt.departments.Count != 0)
    //                    {
    //                        List<string> _deptList = new List<string>();
    //                        foreach (var _dept in _accnt.departments)
    //                        {
    //                            if (accnt.Department == null)
    //                            {
    //                                accnt.Department = _dept.DeptName;
    //                            }
    //                            else if (_dept.parentDepartment != null)
    //                            {
    //                                accnt.Department = accnt.Department + ", " + _dept.parentDepartment.DeptName + " " + _dept.DeptName;
    //                            }
    //                            else
    //                            {
    //                                accnt.Department = accnt.Department + ", " + _dept.DeptName;
    //                            }

    //                        }
    //                    }

    //                    //accnt.Department = _accnt.departments;
    //                    accnt.AccountType = ((ERP_BL.Enums.AccountsType)_accnt.accountType).ToString();
    //                    accnt.Currency = _accnt.currency.CurrencyName;
    //                    //accnt.chartofAccount = _accnt.COAaccount;

    //                    accnt.AccountNo = _accnt.AccountNo;
    //                    accnt.IBAN = _accnt.IBAN;

    //                    if(_accnt.COA_Type != null)
    //                    {
    //                        accnt.AccountNature = ((ERP_BL.Enums.COA_AccountType)_accnt.COA_Type).ToString();
    //                    }
    //                    else
    //                    {
    //                        if (_accnt.nature == 0)
    //                        {
    //                            accnt.AccountNature = ((ERP_BL.Enums.COA_AccountType)3).ToString();
    //                        }
    //                        else
    //                        {
    //                            accnt.AccountNature = ((ERP_BL.Enums.COA_AccountType)_accnt.nature).ToString();
    //                        }
                            
    //                    }
                        
    //                    accnt.AccountName = _accnt.AccountNick;

                        
    //                    accnt.Category = ((ERP_BL.Enums.AccountsCategory)_accnt.accountsCategory).ToString();


    //                    if (_accnt.COAaccount!=null)
    //                    {
    //                        accnt.COA = _accnt.COAaccount.accountName;
    //                    }

    //                    if(_accnt.industryType != null && _accnt.vendor != null)
    //                    {
    //                        accnt.IndustryType = _accnt.industryType.name;
    //                        accnt.Vendor = _accnt.vendor.company.CompanyName;
    //                    }
                        
    //                    //accnt.isPersonal = _accnt.isPersonal;

    //                    accounts.Add(accnt);
    //                }
    //            }
    //            AccountList = accounts;
    //        }
    //        catch(Exception ex)
    //        {
    //            DXMessageBox.Show(ex.Message);
    //        }
    //    }
    //}
}
