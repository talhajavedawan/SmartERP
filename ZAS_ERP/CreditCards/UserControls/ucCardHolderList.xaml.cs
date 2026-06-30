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
    /// Interaction logic for ucCardHolderList.xaml
    /// </summary>
    public partial class ucCardHolderList : UserControl
    {
        CreditCardRepo cardRepo = new CreditCardRepo();
        public ucCardHolderList()
        {
            InitializeComponent();
        }

        private void GrdCardHolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCardHolder.ItemsSource = cardRepo.GetAllCardHolders();
        }

        private void Add_New_CardHolderClick(object sender, RoutedEventArgs e)
        {
            ucFrmCardHolderAdd cardHolderAdd = new ucFrmCardHolderAdd();
            cardHolderAdd.editFlag = 0;
            cardHolderAdd.cardHolderWindow.Width = 350;
            cardHolderAdd.cardHolderWindow.Height = 250;
            cardHolderAdd.cardHolderWindow.Content = cardHolderAdd;
            cardHolderAdd.cardHolderWindow.ResizeMode = ResizeMode.CanMinimize;
            cardHolderAdd.cardHolderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cardHolderAdd.cardHolderWindow.ShowDialog();
        }

        private void Edit_CardHolderClick(object sender, RoutedEventArgs e)
        {
            ucFrmCardHolderAdd cardHolderAdd = new ucFrmCardHolderAdd();
            var selectedRow = grdCardHolder.SelectedItem as CardHolder;

            if (selectedRow != null)
            {
                cardHolderAdd.cardHolder = new CardHolder();
                cardHolderAdd.cardHolder = cardRepo.GetCardHolder(selectedRow.Id);
                cardHolderAdd.cardHolderWindow.Content = cardHolderAdd;
                cardHolderAdd.editFlag = 1;
                cardHolderAdd.cardHolderWindow.Width = 350;
                cardHolderAdd.cardHolderWindow.Height = 250;
                cardHolderAdd.cardHolderWindow.ResizeMode = ResizeMode.CanMinimize;
                cardHolderAdd.cardHolderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cardHolderAdd.cardHolderWindow.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdCardHolder.ItemsSource = cardRepo.GetAllCardHolders();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmCardHolderAdd cardHolderAdd = new ucFrmCardHolderAdd();
            cardHolderAdd.editFlag = 0;
            cardHolderAdd.cardHolderWindow.Width = 350;
            cardHolderAdd.cardHolderWindow.Height = 250;
            cardHolderAdd.cardHolderWindow.Content = cardHolderAdd;
            cardHolderAdd.cardHolderWindow.ResizeMode = ResizeMode.CanMinimize;
            cardHolderAdd.cardHolderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cardHolderAdd.cardHolderWindow.ShowDialog();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ucFrmCardHolderAdd cardHolderAdd = new ucFrmCardHolderAdd();
            var selectedRow = grdCardHolder.SelectedItem as CardHolder;

            if (selectedRow != null)
            {
                cardHolderAdd.cardHolder = new CardHolder();
                cardHolderAdd.cardHolder = cardRepo.GetCardHolder(selectedRow.Id);
                cardHolderAdd.cardHolderWindow.Content = cardHolderAdd;
                cardHolderAdd.editFlag = 1;
                cardHolderAdd.cardHolderWindow.Width = 350;
                cardHolderAdd.cardHolderWindow.Height = 250;
                cardHolderAdd.cardHolderWindow.ResizeMode = ResizeMode.CanMinimize;
                cardHolderAdd.cardHolderWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cardHolderAdd.cardHolderWindow.ShowDialog();
            }
        }
    }
}
