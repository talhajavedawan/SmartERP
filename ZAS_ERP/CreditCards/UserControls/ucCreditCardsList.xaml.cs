using DevExpress.Xpf.Grid;
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
    /// Interaction logic for ucCreditCardsList.xaml
    /// </summary>
    public partial class ucCreditCardsList : UserControl
    {
        CreditCardRepo cardRepo = new CreditCardRepo();

        public ucCreditCardsList()
        {
            InitializeComponent();
        }

        private void GrdCreditCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCreditCard.ItemsSource = cardRepo.GetAllCreditCards();
        }

        private void MbtnAddCreditCard_Click(object sender, RoutedEventArgs e)
        {
            ucFrmCreditCardAdd creditCardAdd = new ucFrmCreditCardAdd();
            creditCardAdd.editFlag = false;
            creditCardAdd.creditCardWindow.Width = 800;
            creditCardAdd.creditCardWindow.Height = 420;
            creditCardAdd.creditCardWindow.Content = creditCardAdd;
            creditCardAdd.creditCardWindow.ResizeMode = ResizeMode.CanMinimize;
            creditCardAdd.creditCardWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            creditCardAdd.creditCardWindow.ShowDialog();
        }

        private void MbtnEditCreditCard_Click(object sender, RoutedEventArgs e)
        {
            cardRepo = new CreditCardRepo();
            ucFrmCreditCardAdd creditCardAdd = new ucFrmCreditCardAdd();
            var selectedRow = grdCreditCard.SelectedItem as CreditCard;

            if (selectedRow != null)
            {
                creditCardAdd.creditCard = new CreditCard();
                creditCardAdd.creditCard = cardRepo.GetCreditCard(selectedRow.Id);
                creditCardAdd.creditCardWindow.Content = creditCardAdd;
                creditCardAdd.editFlag = true;
                creditCardAdd.creditCardWindow.Width = 800;
                creditCardAdd.creditCardWindow.Height = 420;
                creditCardAdd.creditCardWindow.ResizeMode = ResizeMode.CanMinimize;
                creditCardAdd.creditCardWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                creditCardAdd.creditCardWindow.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            cardRepo = new CreditCardRepo();
            grdCreditCard.ItemsSource = cardRepo.GetAllCreditCards();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmCreditCardAdd creditCardAdd = new ucFrmCreditCardAdd();
            creditCardAdd.editFlag = false;
            creditCardAdd.creditCardWindow.Width = 800;
            creditCardAdd.creditCardWindow.Height = 420;
            creditCardAdd.creditCardWindow.Content = creditCardAdd;
            creditCardAdd.creditCardWindow.ResizeMode = ResizeMode.CanMinimize;
            creditCardAdd.creditCardWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            creditCardAdd.creditCardWindow.ShowDialog();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            cardRepo = new CreditCardRepo();
            ucFrmCreditCardAdd creditCardAdd = new ucFrmCreditCardAdd();
            var selectedRow = grdCreditCard.SelectedItem as CreditCard;

            if (selectedRow != null)
            {
                creditCardAdd.creditCard = new CreditCard();
                creditCardAdd.creditCard = cardRepo.GetCreditCard(selectedRow.Id);
                creditCardAdd.creditCardWindow.Content = creditCardAdd;
                creditCardAdd.editFlag = true;
                creditCardAdd.creditCardWindow.Width = 800;
                creditCardAdd.creditCardWindow.Height = 420;
                creditCardAdd.creditCardWindow.ResizeMode = ResizeMode.CanMinimize;
                creditCardAdd.creditCardWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                creditCardAdd.creditCardWindow.ShowDialog();
            }
        }

        private void GrdCreditCard_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {


            //if (e.Column.FieldName == "PrimaryCardNo.CardNumber" && e.IsGetData)
            //{
            //    var primaryCard = e.GetListSourceFieldValue("PrimaryCardNo") as CreditCard;

            //    if (primaryCard != null)
            //    {
            //        e.Value = primaryCard.CardNumber;
            //        //grdCreditCard.SetCellValue(e.ListSourceRowIndex, grdCreditCard.Columns["PrimaryCardNo.CardNumber"], primaryCard.CardNumber);
            //    }

            //}

            //if (e.Column.FieldName == "CardNumber" && e.IsGetData)
            //{
            //    var primaryCardNo = e.GetListSourceFieldValue("PrimaryCardNo.CardNumber");

            //    if (primaryCardNo != null)
            //    {
            //        e.Value = null;
            //    }
            //}
        }

        private void GrdCreditCard_Loaded(object sender, RoutedEventArgs e)
        {
            //List<int> rowHandles = new List<int>();
            //for (int i = 0; i < grdCreditCard.VisibleRowCount; i++)
            //{
            //    int rowHandle = grdCreditCard.GetRowHandleByVisibleIndex(i);
            //    rowHandles.Add(rowHandle);
            //}

            //string cellvalue = "";
            //foreach (int rwHandle in rowHandles)
            //{
            //    var primaryCard = grdCreditCard.GetCellValue(rwHandle, "PrimaryCardNo") as CreditCard;

            //    if(primaryCard != null)
            //    {
            //        cellvalue = primaryCard.CardNumber;
            //        grdCreditCard.SetCellValue(rwHandle, "PrimaryCardNumber", cellvalue);
            //    }
            //    else
            //    {
            //        cellvalue = grdCreditCard.GetCellValue(rwHandle, "CardNumber") as string;
            //        grdCreditCard.SetCellValue(rwHandle, "PrimaryCardNumber", cellvalue);
            //        grdCreditCard.SetCellValue(rwHandle, "CardNumber", null);
            //    }
                
            //}
        }
    }
}
