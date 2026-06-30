using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZAS_ERP.FixedAssets.Classes;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucBankList.xaml
    /// </summary>

    public partial class ucBranchList : UserControl
    {
        public int addBankFlag = 0;
        public int updateBankFlag = 0;
        public int addCntctPrsnFlag = 0;
        public int updateCntctPrsnFlag = 0;
        //public int updateCntctPrsnFlag = 0;

        ucFrmAddBranch addBankObj = new ucFrmAddBranch();
        ucFrmAddBranch updateBankObj = new ucFrmAddBranch();
        ucFrmAddContactPerson addCntctPrsnObj = new ucFrmAddContactPerson();
        ucFrmAddContactPerson updateCntctPrsnObj = new ucFrmAddContactPerson();
        List<Bank> banks = new List<Bank>();
        SalesReceiptRepo repo = new SalesReceiptRepo();

        public ucBranchList()
        {
            InitializeComponent();
            //grdCtrlBankList.Columns["SerialNo"].Visible = false;
            //grdCtrlBankList.Columns["ContactPersonId"].Visible = false;


            ////Add New Bank menu Item permission
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add New Banks") != null)
            //{
            //    mbtnAddBank.IsEnabled = true;
            //}
            //else
            //{
            //    mbtnAddBank.IsEnabled = false;
            //}

            ////Update Existing Bank menu Item permission
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update Existing Bank") != null)
            //{
            //    mbtnUpdateBank.IsEnabled = true;
            //}
            //else
            //{
            //    mbtnUpdateBank.IsEnabled = false;
            //}

            ////Add New Contact Person menu Item permission
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add New Contact Person") != null)
            //{
            //    mbtnAddContPerson.IsEnabled = true;
            //}
            //else
            //{
            //    mbtnAddContPerson.IsEnabled = false;
            //}

            //Update Existing Contact Person menu Item permission
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update Existing Contact Person") != null)
            //{
            //    mbtnUpdateContPerson.IsEnabled = true;
            //}
            //else
            //{
            //    mbtnUpdateContPerson.IsEnabled = false;
            //}
        }


        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            EditBank();
        }

        private void EditBank()
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Bank") != null)
                {
                    var selectedRow = (Bank)grdCtrlBankList.SelectedItem;

                    if (selectedRow != null)
                    {
                        updateBankObj = new ucFrmAddBranch();
                        updateBankObj.addEditflag = 1;

                        updateBankObj.bank = repo.GetBank(selectedRow.Id);
                        //updateBankObj.bankId = selectedRow.SerialNo;
                        //updateBankObj.txtBankName.Text = selectedRow.BankName;
                        //updateBankObj.txtBranchCode.Text = selectedRow.BranchCode;
                        //updateBankObj.txtSwiftCode.Text = selectedRow.SwiftCode;
                        //if (selectedRow.Country == "Pakistan")
                        //{
                        //    updateBankObj.cmbxCountryList.SelectedIndex = 0;
                        //    Cities obj = new Cities();
                        //    var cityList = obj.GetPakCityList();
                        //    if (cityList.Contains(selectedRow.City))
                        //    {
                        //        updateBankObj.cmbxCityList.SelectedIndex = cityList.FindIndex(a => a.Contains(selectedRow.City));
                        //    }
                        //}
                        //else if (selectedRow.Country == "UAE")
                        //{
                        //    updateBankObj.cmbxCountryList.SelectedIndex = 1;
                        //    Cities obj = new Cities();
                        //    var cityList = obj.GetUaeCityList();
                        //    if (cityList.Contains(selectedRow.City))
                        //    {
                        //        updateBankObj.cmbxCityList.SelectedIndex = cityList.FindIndex(a => a.Contains(selectedRow.City));
                        //    }
                        //}

                        //updateBankObj.txtAddress.Text = selectedRow.Address;
                        //updateBankObj.txtBankEmail.Text = selectedRow.Email;
                        //updateBankObj.txtPhone1.Text = selectedRow.PhoneNo;
                        //updateBankObj.txtPhone2.Text = selectedRow.SecondaryPhone;
                        //updateBankObj.txtWebsite.Text = selectedRow.Website;
                        updateBankObj.updateBankWin.Title = "Update Bank";
                        updateBankObj.updateBankWin.Content = updateBankObj;
                        updateBankObj.updateBankWin.Height = 550;
                        updateBankObj.updateBankWin.Width = 700;
                        updateBankObj.updateBankWin.ResizeMode = ResizeMode.CanMinimize;
                        updateBankObj.updateBankWin.ShowDialog();

                        //Load_banks();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to update existing Banks!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void Add_New_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddBank();
        }

        private void AddBank()
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Banks") != null)
                {
                    addBankObj = new ucFrmAddBranch();
                    addBankObj.addEditflag = 0;
                    addBankObj.addBankWin.Title = "Add new Bank";
                    addBankObj.addBankWin.Content = addBankObj;
                    addBankObj.addBankWin.Height = 550;
                    addBankObj.addBankWin.Width = 700;
                    addBankObj.addBankWin.ResizeMode = ResizeMode.CanMinimize;
                    addBankObj.addBankWin.ShowDialog();
                    Load_banks();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Banks!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load_banks();
        }

        private void Load_banks()
        {
            repo = new SalesReceiptRepo();
            //GetAllBanks banks = new GetAllBanks();
            banks = repo.GetAllBranches();
            grdCtrlBankList.ItemsSource = banks;
            //grdCtrlBankList.Columns["SerialNo"].Visible = false;
            //grdCtrlBankList.Columns["ContactPersonId"].Visible = false;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCtrlBankList);
        }

        private void Add_Contact_Person_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddContactPerson();
        }

        private void AddContactPerson()
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Contact Person") != null)
                {
                    var selectedRow = (Bank)grdCtrlBankList.SelectedItem;
                    if (selectedRow != null)
                    {
                        addCntctPrsnObj = new ucFrmAddContactPerson();

                        //addCntctPrsnObj.cmbxBanks.SelectedIndex = selectedRow.SerialNo - 1;
                        int bankId = selectedRow.Id;
                        addCntctPrsnObj.selectedBank = repo.GetBank(bankId);
                        banks = repo.GetAllBranches();
                        //int index = -1;
                        //foreach (var _bank in banks)
                        //{
                        //    index++;
                        //    if (_bank.Id == bank.Id)
                        //    {
                        //        addCntctPrsnObj.cmbxBanks.SelectedIndex = index;
                        //        break;
                        //    }
                        //}

                        addCntctPrsnObj.addEditFlag = 0;
                        addCntctPrsnObj.addCntctPrsnWin.Title = "Add Contact Person";
                        addCntctPrsnObj.addCntctPrsnWin.Content = addCntctPrsnObj;
                        addCntctPrsnObj.addCntctPrsnWin.Height = 400;
                        addCntctPrsnObj.addCntctPrsnWin.Width = 700;
                        addCntctPrsnObj.addCntctPrsnWin.ResizeMode = ResizeMode.CanMinimize;
                        addCntctPrsnObj.addCntctPrsnWin.ShowDialog();
                        //Load_banks();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Contact Person!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void Update_Contact_Person_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            UpdateContactPerson();
        }

        private void UpdateContactPerson()
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Contact Person") != null)
                {
                    //var selectedRow = (AllBanks)grdCtrlBankList.SelectedItem;
                    //if (selectedRow != null)
                    //{

                    //    int bankId = selectedRow.SerialNo;
                    //    Bank bank = new Bank();
                    //    repo = new SalesReceiptRepo();
                    //    bank = repo.GetBank(bankId);
                    //    banks = repo.GetAllBank();
                    //    if (bank.contactPersons.Count != 0)
                    //    {
                    //        updateCntctPrsnObj = new ucFrmAddContactPerson();

                    //        updateCntctPrsnObj.contactPersonId = selectedRow.ContactPersonId;
                    //        //updateCntctPrsnObj.cmbxBanks.SelectedIndex = selectedRow.SerialNo -1;
                    //        int index = -1;
                    //        foreach (var _bank in banks)
                    //        {
                    //            index++;
                    //            if (_bank.Id == bank.Id)
                    //            {
                    //                updateCntctPrsnObj.cmbxBanks.SelectedIndex = index;
                    //                break;
                    //            }
                    //        }

                    //        updateCntctPrsnObj.txtPersonName.Text = selectedRow.PersonName;
                    //        updateCntctPrsnObj.txtCnic.Text = selectedRow.CNIC;
                    //        updateCntctPrsnObj.txtDOB.DateTime = selectedRow.DOB;
                    //        updateCntctPrsnObj.txtDesignation.Text = selectedRow.Designation;
                    //        updateCntctPrsnObj.txtPhone.Text = selectedRow.Phone_No;
                    //        updateCntctPrsnObj.txtMobile.Text = selectedRow.MobileNo;
                    //        updateCntctPrsnObj.txtEmail.Text = selectedRow.PersonEmail;
                    //        updateCntctPrsnObj.chkActive.IsChecked = selectedRow.IsActive;

                    //        updateCntctPrsnObj.addEditFlag = 1;
                    //        updateCntctPrsnObj.updateCntctPrsnWin.Title = "Update Contact Person";
                    //        updateCntctPrsnObj.updateCntctPrsnWin.Content = updateCntctPrsnObj;
                    //        updateCntctPrsnObj.updateCntctPrsnWin.Height = 400;
                    //        updateCntctPrsnObj.updateCntctPrsnWin.Width = 700;
                    //        updateCntctPrsnObj.updateCntctPrsnWin.ResizeMode = ResizeMode.CanMinimize;
                    //        updateCntctPrsnObj.updateCntctPrsnWin.ShowDialog();
                    //        Load_banks();

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("This bank has no contact person.");
                    //    }
                    //}
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to update Existing Contact Person!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void MbtnAllContacts_Click(object sender, RoutedEventArgs e)
        {
            Load_banks();
        }

        private void MbtnActiveContacts_Click(object sender, RoutedEventArgs e)
        {
            //var allBanks = repo.GetAllBank();
            //GetAllBanks obj = new GetAllBanks(true, allBanks);
            //grdCtrlBankList.ItemsSource = obj.BanksList;
            //grdCtrlBankList.Columns["SerialNo"].Visible = false;
            //grdCtrlBankList.Columns["ContactPersonId"].Visible = false;
            //SystemLogic.SetUserSettingOfCurrentWindow(grdCtrlBankList);
        }

        private void MbtnInActiveContacts_Click(object sender, RoutedEventArgs e)
        {
            //var allBanks = repo.GetAllBank();
            //GetAllBanks obj = new GetAllBanks(false, allBanks);
            //grdCtrlBankList.ItemsSource = obj.BanksList;
            //grdCtrlBankList.Columns["SerialNo"].Visible = false;
            //grdCtrlBankList.Columns["ContactPersonId"].Visible = false;
            //SystemLogic.SetUserSettingOfCurrentWindow(grdCtrlBankList);
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCtrlBankList);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Load_banks();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCtrlBankList);
        }

        private void Add_New_ContactClick(object sender, RoutedEventArgs e)
        {
            AddContactPerson();
        }
        private void View_ContactPersonsClick(object sender, RoutedEventArgs e)
        {
            var selectedRow = (Bank)grdCtrlBankList.SelectedItem;

            repo = new SalesReceiptRepo();
            ucBankContactPersons ucBank = new ucBankContactPersons();
            ucBank = new ucBankContactPersons();
            ucBank.bank = repo.GetBank(selectedRow.Id);

            Window window = new Window();
            window.Content = ucBank;
            window.Show();
            //UpdateContactPerson();
        }
        private void Edit_BankClick(object sender, RoutedEventArgs e)
        {
            EditBank();
        }

        private void Add_New_BankClick(object sender, RoutedEventArgs e)
        {
            AddBank();
        }

        private void UserControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedRow = (Bank)grdCtrlBankList.SelectedItem;

            repo = new SalesReceiptRepo();
            ucBankContactPersons ucBank = new ucBankContactPersons();
            ucBank = new ucBankContactPersons();
            ucBank.bank =  repo.GetBank(selectedRow.Id);

            Window window = new Window();
            window.Content = ucBank;
            window.Show();
        }

        private void GrdCtrlBankList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if(e.IsGetData && e.Column.FieldName == "Companiess")
            {
                var _bank = grdCtrlBankList.GetRowByListIndex(e.ListSourceRowIndex) as Bank;
                if(_bank != null)
                {
                    var companies = String.Join(" | ", _bank.companies.Select(x => x.CompanyName));
                    e.Value = companies;
                }
                
            }
        }
    }
}
    
