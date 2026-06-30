using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
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
using ZAS_ERP.Interest.UserControls;

namespace ZAS_ERP.MarginPerc.UserControls
{
    /// <summary>
    /// Interaction logic for ucCashMarginList.xaml
    /// </summary>
    public partial class ucCashMarginList : UserControl
    {
        TaxRepo taxRepo = new TaxRepo();
        ucFrmCashMarginAdd frmCashMarginAdd = new ucFrmCashMarginAdd();
        public ucCashMarginList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTypeWindow();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taxRepo = new TaxRepo();
            grdCntrlInterestNameList.ItemsSource = taxRepo.getAllMarginPerc();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlInterestNameList.ItemsSource = taxRepo.getAllMarginPerc();
        }

        private void AddTypeWindow()
        {
            frmCashMarginAdd = new ucFrmCashMarginAdd();
            frmCashMarginAdd.editFlag = false;

            frmCashMarginAdd.addMarginWin.Height = 300;
            frmCashMarginAdd.addMarginWin.Width = 700;
            frmCashMarginAdd.addMarginWin.ResizeMode = ResizeMode.CanMinimize;
            frmCashMarginAdd.addMarginWin.Content = frmCashMarginAdd;
            frmCashMarginAdd.addMarginWin.Title = "Add new Cash Margin Perc.";
            frmCashMarginAdd.addMarginWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmCashMarginAdd.addMarginWin.ShowDialog();
        }

        private void EditTypeWindow()
        {
            frmCashMarginAdd = new ucFrmCashMarginAdd();
            frmCashMarginAdd.editFlag = true;

            var selectedRow = (MarginPercentage)grdCntrlInterestNameList.SelectedItem;
            //var selectedRow = (MarginPercentage)grdCntrlInterestNameList.SelectedItem;
            if (selectedRow != null)
            {
                frmCashMarginAdd.marginNameId = selectedRow.Id;
                frmCashMarginAdd.addMarginWin.Height = 300;
                frmCashMarginAdd.addMarginWin.Width = 700;
                frmCashMarginAdd.addMarginWin.ResizeMode = ResizeMode.CanMinimize;
                frmCashMarginAdd.addMarginWin.Content = frmCashMarginAdd;
                frmCashMarginAdd.addMarginWin.Title = "Update Cash Margin Perc.";
                frmCashMarginAdd.addMarginWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmCashMarginAdd.addMarginWin.ShowDialog();
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditTypeWindow();
        }
    }
}
