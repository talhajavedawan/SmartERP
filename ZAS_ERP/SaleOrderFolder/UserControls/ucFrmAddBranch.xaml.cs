using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
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
using ZAS_ERP.FixedAssets.Classes;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddBank.xaml
    /// </summary>
    public partial class ucFrmAddBranch : UserControl
    {
        List<string> country_list = new List<string>();

        public UcListWindow addBankWin = new UcListWindow();
        public UcListWindow updateBankWin = new UcListWindow();
        public Bank bank = new Bank();
        UsersRepo userRep = new UsersRepo();
        List<Company> loginUserCompanies = new List<Company>();

        SalesReceiptRepo repo = new SalesReceiptRepo();

        public int addEditflag;
        public int bankId;
        public ucFrmAddBranch()
        {
            InitializeComponent();
            cmbxCountryList.ItemsSource = GetCountryList();
            addBankWin.Closing += AddBank_Window_Closing;
            updateBankWin.Closing += UpdateBank_Window_Closing;

            gridCompview.NodeCheckStateChanged += OncompgirdNodeCheckStateChanged;
            gridCompanies.SelectionChanged += OncompGridSelectionChanged;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUser = userRep.getuser(MainWindow.currentUserid);
            loginUserCompanies = loginUser.employee.Companies;

            CompanyRepo compRepo = new CompanyRepo();
            gridCompanies.ItemsSource = loginUserCompanies; /*compRepo.GetActiveCompanies();*/

            lookupBankName.ItemsSource = repo.GetAllBanks();

            if (addEditflag == 1)
            {
                if (bank.mainBank != null)
                    lookupBankName.Text = bank.mainBank.BankName;

                txtBranchName.Text = bank.BankName;
                txtBranchCode.Text = bank.BranchCode;
                txtSwiftCode.Text = bank.SwiftCode;
                if (bank.address.Country == "Pakistan")
                {
                    cmbxCountryList.SelectedIndex = 0;
                    Cities obj = new Cities();
                    var cityList = obj.GetPakCityList();
                    if (cityList.Contains(bank.address.City))
                    {
                        cmbxCityList.SelectedIndex = cityList.FindIndex(a => a.Contains(bank.address.City));
                    }
                }
                else if (bank.address.Country == "UAE")
                {
                    cmbxCountryList.SelectedIndex = 1;
                    Cities obj = new Cities();
                    var cityList = obj.GetUaeCityList();
                    if (cityList.Contains(bank.address.City))
                    {
                        cmbxCityList.SelectedIndex = cityList.FindIndex(a => a.Contains(bank.address.City));
                    }
                }

                txtAddress.Text = bank.address.Line1;
                txtBankEmail.Text = bank.contact.Email;
                txtPhone1.Text = bank.contact.ContactNo;
                txtPhone2.Text = bank.contact.SecondaryContact;
                txtWebsite.Text = bank.contact.Website;

                foreach (var _comp in bank.companies)
                {
                    gridCompanies.SelectItem(gridCompanies.FindRowByValue(gridCompanies.Columns.GetColumnByFieldName("Id"), _comp.Id));
                }
            }

        }

        public List<string> GetCountryList()
        {
            country_list.Add("Pakistan");
            country_list.Add("UAE");
            return country_list;
            //List<string> cultureList = new List<string>();

            //CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            //foreach (CultureInfo culture in cultures)
            //{
            //    RegionInfo region = new RegionInfo(culture.LCID);

            //    if (!(cultureList.Contains(region.EnglishName)))
            //    {
            //        cultureList.Add(region.EnglishName);
            //    }
            //}
            //return cultureList;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var companies = gridCompanies.SelectedItems;
                //Contact cont1 = new Contact();
                //cont1.ContactNo = txtMobile.Text;
                //cont1.SecondaryContact = txtPhone.Text;
                //cont1.Email = txtPersonEmail.Text;

                //Person personName = new Person();
                //personName.CNIC = txtCnic.Text;
                //personName.FName = txtPersonName.Text;
                //personName.Gender = ERP_BL.Enums.Gender.Male;
                //personName.DOB = txtDob.DateTime;


                //ContactPerson contPerson = new ContactPerson();
                //contPerson.person = personName;
                //contPerson.contact = cont1;
                //contPerson.designation = txtDesignation.Text;

                //ucBankList obj = new ucBankList();
                if(lookupBankName.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Bank!");
                    lookupBankName.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtBranchName.Text))
                {
                    DXMessageBox.Show("Please enter Bank name!");
                    return;
                }
                if (cmbxCountryList.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Coutry!");
                    return;
                }
                else if(cmbxCityList.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select City!");
                    return;
                }
                else if (String.IsNullOrEmpty(txtAddress.Text))
                {
                    DXMessageBox.Show("Please enter Address!");
                    return;
                }

                

               
                if (addEditflag == 0)
                {
                    bank.bankId = (lookupBankName.SelectedItem as MainBank).Id;
                    bank.BankName = txtBranchName.Text;
                    bank.BranchCode = txtBranchCode.Text;
                    bank.SwiftCode = txtSwiftCode.Text;

                    //Address add = new Address();
                    bank.address = new Address();
                    bank.address.Country = cmbxCountryList.EditValue.ToString();
                    bank.address.City = cmbxCityList.EditValue.ToString();
                    bank.address.Line1 = txtAddress.Text;

                    //Contact cont = new Contact();
                    bank.contact = new Contact();
                    bank.contact.ContactNo = txtPhone1.Text;
                    bank.contact.SecondaryContact = txtPhone2.Text;
                    bank.contact.Email = txtBankEmail.Text;
                    bank.contact.Website = txtWebsite.Text;
                    bank.contact.Website = txtWebsite.Text;
                    if (gridCompanies.SelectedItems.Count != 0)
                    {
                        bank.companies = new List<Company>();
                        foreach (Company _comp in gridCompanies.SelectedItems)
                        {
                            if (!bank.companies.Contains(_comp))
                            {
                                bank.companies.Add(_comp);
                            }
                        }
                    }

                    
                    repo.addBank(bank);
                    MessageBox.Show("Successfully Added");
                    //obj.grdCtrlBankList.ItemsSource = banks.BanksList;
                    addBankWin.Close();
                }
                else if (addEditflag == 1)
                {
                    bank.bankId = (lookupBankName.SelectedItem as MainBank).Id;
                    bank.BankName = txtBranchName.Text;
                    bank.BranchCode = txtBranchCode.Text;
                    bank.SwiftCode = txtSwiftCode.Text;

                    //Address add = new Address();
                    bank.address.Country = cmbxCountryList.EditValue.ToString();
                    bank.address.City = cmbxCityList.EditValue.ToString();
                    bank.address.Line1 = txtAddress.Text;

                    //Contact cont = new Contact();
                    bank.contact.ContactNo = txtPhone1.Text;
                    bank.contact.SecondaryContact = txtPhone2.Text;
                    bank.contact.Email = txtBankEmail.Text;
                    bank.contact.Website = txtWebsite.Text;
                    bank.contact.Website = txtWebsite.Text;
                    if (gridCompanies.SelectedItems.Count != 0)
                    {
                        bank.companies = new List<Company>();
                        foreach (Company _comp in gridCompanies.SelectedItems)
                        {
                            if (!bank.companies.Contains(_comp))
                            {
                                bank.companies.Add(_comp);
                            }
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Please select Company!");
                        return;
                    }

                    repo.updateBank(bank);
                    MessageBox.Show("Successfully Updated");
                    updateBankWin.Close();
                    //obj.grdCtrlBankList.ItemsSource = banks.BanksList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbxCountryList_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (cmbxCountryList.SelectedIndex == 0)
            {
                Cities obj = new Cities();
                cmbxCityList.ItemsSource = obj.GetPakCityList();
            }
            else if (cmbxCountryList.SelectedIndex == 1)
            {
                Cities obj = new Cities();
                cmbxCityList.ItemsSource = obj.GetUaeCityList();
            }
        }

        private void AddBank_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void UpdateBank_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void OncompgirdNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                gridCompanies.SelectItem(e.Node.RowHandle);
            else
                gridCompanies.UnselectItem(e.Node.RowHandle);
        }
        private void OncompGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridCompview;
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
                    var selectedRows = gridCompanies.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
    }
}
