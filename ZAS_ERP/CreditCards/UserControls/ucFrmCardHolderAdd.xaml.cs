using DevExpress.Xpf.Core;
using ERP_BL.CreditCards;
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

namespace ZAS_ERP.CreditCards.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmCardHolderAdd.xaml
    /// </summary>
    public partial class ucFrmCardHolderAdd : UserControl
    {
        public int editFlag = 0;
        public CardHolder cardHolder = new CardHolder();
        CreditCardRepo cardRepo = new CreditCardRepo();
        public Window cardHolderWindow = new Window();

        public ucFrmCardHolderAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //if(cmbxType.SelectedIndex < 0)
            //{
            //    DXMessageBox.Show("Please select type!");
            //}
            //else 
            if (String.IsNullOrEmpty(txtCardHolder.Text))
            {
                DXMessageBox.Show("Please enter Card holder name!");
            }
            else 
            {
                //cardHolder.cardHolderType = ((ERP_BL.Enums.CardHolderType)cmbxType.SelectedIndex);
                cardHolder.Name = txtCardHolder.Text;

                if (chkIsActive.IsChecked == true)
                    cardHolder.isActive = true;
                else
                    cardHolder.isActive = false;
                        


                //if (cmbxType.SelectedIndex == 1)
                //{
                //    if(cmbxPrimaryCardHolder.SelectedIndex < 0)
                //    {
                //        DXMessageBox.Show("Please enter primary Card holder!");
                //        return;
                //    }
                //    else
                //    {
                //        cardHolder.ParentCardHolder = cmbxPrimaryCardHolder.SelectedItem as CardHolder;   
                //    }
                //}
                //else if(cmbxType.SelectedIndex == 0)
                //{
                //    cardHolder.ParentCardHolder = null;
                //}


                if(editFlag == 1 && cardHolder.Id > 0)
                {
                    cardRepo.updateCardHolder(cardHolder);
                    MessageBox.Show("Successfully Updated!");
                }
                else if(editFlag == 0 && cardHolder.Id == 0)
                {
                    cardRepo.addCardHolder(cardHolder);
                    MessageBox.Show("Successfully Added!");
                }


                cardHolderWindow.Close();
            }
           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
            //{
            //    cmbxType.Items.Add(((ERP_BL.Enums.CardHolderType)i).ToString());
            //}

            //cmbxPrimaryCardHolder.ItemsSource = cardRepo.GetAllCardHolders();

            if(editFlag == 1 && cardHolder.Id > 0)
            {
                txtCardHolder.Text = cardHolder.Name;
                if(cardHolder.isActive == true)
                {
                    chkIsActive.IsChecked = true;
                }

                ////Select Card Holder Type
                //for (int i = 0; i <= (int)ERP_BL.Enums.CardHolderType.Secondary; i++)
                //{
                //    if (((ERP_BL.Enums.CardHolderType)i).ToString() == cardHolder.cardHolderType.ToString())
                //    {
                //        cmbxType.SelectedIndex = i;
                //        break;
                //    }
                //}

                //if(cardHolder.ParentCardHolder != null)
                //{
                //    int index = 0;
                //    cmbxPrimaryCardHolder.Visibility = Visibility.Visible;

                //    //Select Primary Card Holder
                //    var primaryCardHolderList = (cmbxPrimaryCardHolder.ItemsSource as List<CardHolder>) == null ? new List<CardHolder>() : cmbxPrimaryCardHolder.ItemsSource as List<CardHolder>;
                //    if (cardHolder.ParentCardHolder != null)
                //    {
                //        foreach (var _cardHolder in primaryCardHolderList)
                //        {
                //            if (_cardHolder.Id == cardHolder.ParentCardHolder.Id)
                //            {
                //                cmbxPrimaryCardHolder.SelectedIndex = index;
                //                index = 0;
                //                break;
                //            }
                //            index++;
                //        }
                //    }
                //}

            }
        }

        private void CmbxPrimaryCardHolder_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void CmbxPrimaryCardHolder_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void CmbxType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if(cmbxType.SelectedIndex == 1)
            //{
            //    parentCardHolder.Visibility = Visibility.Visible;
            //}
            //else if (cmbxType.SelectedIndex == 0)
            //{
            //    parentCardHolder.Visibility = Visibility.Hidden;
            //}
        }
    }
}
