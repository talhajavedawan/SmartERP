using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.CreditCards;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ZAS_ERP.CreditCards.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmCreditCardAdd.xaml
    /// </summary>
    public partial class ucFrmCreditCardAdd : UserControl
    {
        CreditCardRepo cardRepo = new CreditCardRepo();
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();

        public CreditCard creditCard = new CreditCard();
        public Window creditCardWindow = new Window();
        public bool editFlag = false;

        public ucFrmCreditCardAdd()
        {
            InitializeComponent();
            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;
            gridDepartment.SelectionChanged += OndeptGridSelectionChanged;
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select type!");
                cmbxType.Focus();
                return;
            }
            if(cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                cmbxCompany.Focus();
                return;
            }
            if(cmbxPrimaryHolder.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Primary Card Holder!");
                cmbxPrimaryHolder.Focus();
                return;
            }
            if (cmbxCardUser.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Card User!");
                cmbxCardUser.Focus();
                return;
            }
            
            
            if (cmbxBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                cmbxBank.Focus();
                return;
            }
            if (datIssueDate.EditValue == null)
            {
                DXMessageBox.Show("Please select Issue Date!");
                return;
            }
            if (datExpDate.EditValue == null)
            {
                DXMessageBox.Show("Please select Expiry Date!");
                datExpDate.Focus();
                return;
            }

            if(cmbxCardType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Type!");
                cmbxCardType.Focus();
                return;
            }

            if (cmbxCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Currency!");
                cmbxCurrency.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtLimit.Text))
            {
                DXMessageBox.Show("Please enter Limt!");
                txtLimit.Focus();
                return;
            }

            if (cmbxType.SelectedIndex == 1)
            {
                if (cmbxPrimaryCardNo.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Primary Card Number!");
                    cmbxPrimaryCardNo.Focus();
                    return;
                }
                if (cmbxSecondaryCardHolder.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Secondary Card holder!");
                    cmbxSecondaryCardHolder.Focus();
                    return;
                }
                
                
            }
            else
            {
                creditCard.SecondaryCardHolder = null;
            }
            
                if (String.IsNullOrEmpty( txtCardNumber.Text))
                {
                    DXMessageBox.Show("Please enter Primary Card Number!");
                    txtCardNumber.Focus();
                    return;
                }
                
            

            if(gridDepartment.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Please select departments!");
                return;
            }

            creditCard.cardHolderType = ((ERP_BL.Enums.CardHolderType)cmbxType.SelectedIndex);
            creditCard.company = cmbxCompany.SelectedItem as Company;
            creditCard.PrimaryCardHolder = cmbxPrimaryHolder.SelectedItem as CardHolder;
            creditCard.CardUser = cmbxCardUser.SelectedItem as CardHolder;

            creditCard.isActive = chkIsActive.IsChecked.Value;

            if (creditCard.cardHolderType == ERP_BL.Enums.CardHolderType.Secondary)
            {
                //creditCard.hasPrimaryCard = true;
                creditCard.PrimaryCardNoId = (cmbxPrimaryCardNo.SelectedItem as cmbitem).id;
                creditCard.SecondaryCardHolder = cmbxSecondaryCardHolder.SelectedItem as CardHolder;

                //creditCard.SecondaryCardNumber = txtSecondaryCardNumber.Text;

                //creditCard.CardNumber = null;

            }
            else
            {
                //creditCard.hasPrimaryCard = false;
                creditCard.PrimaryCardNoId = null;
                //creditCard.SecondaryCardNumber = null;

                //creditCard.CardNumber = txtCardNumber.Text;
            }

            creditCard.CardNumber = txtCardNumber.Text;
            creditCard.bank = cmbxBank.SelectedItem as Bank;
            creditCard.IssueDate = (DateTime)datIssueDate.EditValue;
            creditCard.ExpiryDate = (DateTime)datExpDate.EditValue;
            creditCard.creditCardType = cmbxCardType.SelectedItem as CreditCardType;
            creditCard.currency = cmbxCurrency.SelectedItem as Currency;
            creditCard.LimitAmount = Convert.ToDouble(txtLimit.Text);

            if(!String.IsNullOrEmpty(txtCVV.Text))
                creditCard.CVV = Convert.ToInt32(txtCVV.Text);

            if (gridDepartment.SelectedItems.Count != 0)
            {
                creditCard.departments = new List<Department>();
                foreach (Department dept in gridDepartment.SelectedItems)
                {
                    if (!creditCard.departments.Contains(dept))
                    {
                        creditCard.departments.Add(dept);
                    }
                }
            }


            if (editFlag == true && creditCard.Id > 0)
            {
                cardRepo.updateCreditCard(creditCard);
                DXMessageBox.Show("Successfully Updated!");
            }
            else if(editFlag == false && creditCard.Id == 0)
            {
                cardRepo.addCreditCard(creditCard);
                DXMessageBox.Show("Successfully added");
            }

            creditCardWindow.Close();
        }

        private void loadPrimaryCards()
        {
            var company = cmbxCompany.SelectedItem as Company;
            var bank = cmbxBank.SelectedItem as Bank;
            var primaryHolder = cmbxPrimaryHolder.SelectedItem as CardHolder;
            cardRepo = new CreditCardRepo();
            var primaryCards = cardRepo.GetAllByCompanyBankPrimaryHolder(company.Id, bank.Id, primaryHolder.Id);

            List<cmbitem> cmbitems = new List<cmbitem>();

            foreach (CreditCard _card in primaryCards)
            {
                cmbitems.Add(new cmbitem() { name = _card.CardNumber, id = _card.Id });
            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbxPrimaryCardNo.ItemsSource = cmbitems;
        }

        private void loadCreditCardTypes()
        {
            cardRepo = new CreditCardRepo();
            cmbxCardType.ItemsSource = cardRepo.GetAllCreditCardTypes();
        }

        private void loadCurrencies()
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            cmbxCurrency.ItemsSource = currencyRepo.getAll();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadCreditCardTypes();
            loadCurrencies();
            //loadPrimaryCards();

            for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
            {
                cmbxType.Items.Add(((ERP_BL.Enums.CardHolderType)i).ToString());
            }

            var cardHolders = cardRepo.GetAllCardHolders();

            cmbxPrimaryHolder.ItemsSource = cardHolders;
            cmbxSecondaryCardHolder.ItemsSource = cardHolders;
            cmbxCardUser.ItemsSource = cardHolders;

            if(editFlag == true && creditCard.Id > 0)
            {
                int index = 0;

                

                if (creditCard.IssueDate != null)
                    datIssueDate.EditValue = creditCard.IssueDate;

                if(creditCard.ExpiryDate != null)
                    datExpDate.EditValue = creditCard.ExpiryDate;

                if(!String.IsNullOrEmpty(creditCard.CVV.ToString()))
                    txtCVV.Text = creditCard.CVV.ToString();

                //Select Card Holder Type
                for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
                {
                    if (((ERP_BL.Enums.CardHolderType)i).ToString() == creditCard.cardHolderType.ToString())
                    {
                        cmbxType.SelectedIndex = i;
                        break;
                    }
                }


                chkIsActive.IsChecked = creditCard.isActive;
                

                //Select Bank
                if (creditCard.company != null)
                {
                    index = 0;
                    var companyList = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == creditCard.company.Id)
                        {
                            cmbxCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                foreach (var _dept in creditCard.departments)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                }

                //Select Bank
                if (creditCard.bank != null)
                {
                    index = 0;
                    var bankList = (cmbxBank.ItemsSource as List<Bank>) == null ? new List<Bank>() : cmbxBank.ItemsSource as List<Bank>;
                    foreach (var _bank in bankList)
                    {
                        if (_bank.Id == creditCard.bank.Id)
                        {
                            cmbxBank.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Card Holder
                if (creditCard.PrimaryCardHolder != null)
                {
                    index = 0;
                    var cardHolderList = (cmbxPrimaryHolder.ItemsSource as List<CardHolder>) == null ? new List<CardHolder>() : cmbxPrimaryHolder.ItemsSource as List<CardHolder>;
                    foreach (var _cardHolder in cardHolderList)
                    {
                        if (_cardHolder.Id == creditCard.PrimaryCardHolder.Id)
                        {
                            cmbxPrimaryHolder.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (creditCard.cardHolderType == ERP_BL.Enums.CardHolderType.Secondary)
                {
                    //chkHasPrimaryCreditCard.IsChecked = true;

                    //Select Primary Card Number
                    var primaryCardList = (cmbxPrimaryCardNo.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPrimaryCardNo.ItemsSource as List<cmbitem>;
                    if (creditCard.PrimaryCardNoId != null)
                    {
                        index = 0;
                        foreach (var _cardNo in primaryCardList)
                        {
                            if (_cardNo.id == creditCard.PrimaryCardNoId)
                            {
                                cmbxPrimaryCardNo.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    
                }
               
                    if (creditCard.CardNumber != null)
                        txtCardNumber.Text = creditCard.CardNumber;
                

                //Select Card User
                if (creditCard.CardUser != null)
                {
                    index = 0;
                    var cardUserList = (cmbxCardUser.ItemsSource as List<CardHolder>) == null ? new List<CardHolder>() : cmbxCardUser.ItemsSource as List<CardHolder>;
                    foreach (var _cardUser in cardUserList)
                    {
                        if (_cardUser.Id == creditCard.CardUser.Id)
                        {
                            cmbxCardUser.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Primary Card Holder
                if (creditCard.SecondaryCardHolder != null)
                {
                    index = 0;
                    cmbxSecondaryCardHolder.Visibility = Visibility.Visible;

                    //Select Primary Card Holder
                    var primaryCardHolderList = (cmbxSecondaryCardHolder.ItemsSource as List<CardHolder>) == null ? new List<CardHolder>() : cmbxSecondaryCardHolder.ItemsSource as List<CardHolder>;
                    if (creditCard.SecondaryCardHolder != null)
                    {
                        foreach (var _cardHolder in primaryCardHolderList)
                        {
                            if (_cardHolder.Id == creditCard.SecondaryCardHolder.Id)
                            {
                                cmbxSecondaryCardHolder.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                }

                //Select Card User
                if (creditCard.creditCardType != null)
                {
                    index = 0;
                    var cardTypeList = (cmbxCardType.ItemsSource as List<CreditCardType>) == null ? new List<CreditCardType>() : cmbxCardType.ItemsSource as List<CreditCardType>;
                    foreach (var _cardType in cardTypeList)
                    {
                        if (_cardType.Id == creditCard.creditCardType.Id)
                        {
                            cmbxCardType.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                //Select Currency
                if (creditCard.currency != null)
                {
                    index = 0;
                    var currencyList = (cmbxCurrency.ItemsSource as List<Currency>) == null ? new List<Currency>() : cmbxCurrency.ItemsSource as List<Currency>;
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.Id == creditCard.currency.Id)
                        {
                            cmbxCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtLimit.Text = creditCard.LimitAmount.ToString();


            }
        }

        private void CmbxType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxType.SelectedIndex == 1)
            {
                txtBlkPrimaryCardNo.Visibility = Visibility.Visible;
                cmbxPrimaryCardNo.Visibility = Visibility.Visible;

                //txtBlkCardNo.Visibility = Visibility.Collapsed;
                //txtCardNumber.Visibility = Visibility.Collapsed;

                txtBlckSecondaryHolder.Visibility = Visibility.Visible;
                cmbxSecondaryCardHolder.Visibility = Visibility.Visible;


                //txtSecondaryCardNumber.Visibility = Visibility.Visible;
                //txtBlkSecondaryCardNo.Visibility = Visibility.Visible;
            }
            else if(cmbxType.SelectedIndex == 0)
            {
                txtBlkPrimaryCardNo.Visibility = Visibility.Collapsed;
                cmbxPrimaryCardNo.Visibility = Visibility.Collapsed;

                //txtBlkCardNo.Visibility = Visibility.Visible;
                //txtCardNumber.Visibility = Visibility.Visible;

                txtBlckSecondaryHolder.Visibility = Visibility.Collapsed;
                cmbxSecondaryCardHolder.Visibility = Visibility.Collapsed;

                //txtSecondaryCardNumber.Visibility = Visibility.Collapsed ;
                //txtBlkSecondaryCardNo.Visibility = Visibility.Collapsed;
            }
        }

        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //Getting the Selected Company
            var company = cmbxCompany.SelectedItem as ERP_BL.Databases.Company;

            //Getting the Departments of the selected company
            var allDepts = company.departments.Where(x => x.isActive == true).ToList();

            //Assigning itemsource to Combobox Department 
            this.gridDepartment.ItemsSource = allDepts;

            var banks = company.banks.ToList();
            cmbxBank.ItemsSource = banks;
        }

        private void loadCompanies()
        {
            CompanyRepo repo = new CompanyRepo();
            cmbxCompany.ItemsSource = repo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ChkHasPrimaryCreditCard_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChkHasPrimaryCreditCard_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void CmbxPrimaryCardNo_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxPrimaryCardNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmbxPrimaryHolder_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if(cmbxType.SelectedIndex == 1 && cmbxCompany.SelectedIndex > -1 && cmbxBank.SelectedIndex > -1 && cmbxPrimaryHolder.SelectedIndex > -1)
            {
                loadPrimaryCards();
            }
        }

        private void CmbxCompany_GotFocus(object sender, RoutedEventArgs e)
        {
            if(cmbxType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Holder Type!");
                return;
            }
        }

        private void CmbxBank_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                return;
            }
        }

        private void CmbxPrimaryHolder_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cmbxBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                return;
            }
        }

        private void TxtCardNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            //if(cmbxType.SelectedIndex == 0)
            //{
            //    var cardNo = txtCardNumber.Text;

            //    cardRepo = new CreditCardRepo();
            //    var count = cardRepo.GetAllCreditCards().Where(x => x.CardNumber == cardNo).Count();

            //    if(count > 0)
            //    {
            //        //txtCardNumber.Focus();
            //        txtCardNumber.Background = Brushes.Red;
            //        DXMessageBox.Show("Card Number already exist!");
            //        return;
            //    }
            //    else
            //    {
            //        txtCardNumber.Background = Brushes.Transparent;
            //    }
            //}
            
        }
    }
}
