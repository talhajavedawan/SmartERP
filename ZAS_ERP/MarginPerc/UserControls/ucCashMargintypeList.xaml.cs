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
    /// Interaction logic for ucCashMargintypeList.xaml
    /// </summary>
    public partial class ucCashMargintypeList : UserControl
    {
        ucFrmCashMarginType frmCashMarginPercType = new ucFrmCashMarginType();
        TaxRepo taxRepo = new TaxRepo();
        public ucCashMargintypeList()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllMarginPercType();
        }
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddTypeWindow();
        }
        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditTypeWindow();
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taxRepo = new TaxRepo();
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllMarginPercType();
        }

        private void AddTypeWindow()
        {
            frmCashMarginPercType = new ucFrmCashMarginType();
            frmCashMarginPercType.editFlag = false;
            frmCashMarginPercType.addMarginPercTypeWin.Height = 300;
            frmCashMarginPercType.addMarginPercTypeWin.Width = 400;
            frmCashMarginPercType.addMarginPercTypeWin.ResizeMode = ResizeMode.CanMinimize;
            frmCashMarginPercType.addMarginPercTypeWin.Content = frmCashMarginPercType;
            frmCashMarginPercType.addMarginPercTypeWin.Title = "Add Cash Margin Perc Type";
            frmCashMarginPercType.addMarginPercTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmCashMarginPercType.addMarginPercTypeWin.ShowDialog();
        }
        private void EditTypeWindow()
        {
            frmCashMarginPercType = new ucFrmCashMarginType();
            frmCashMarginPercType.editFlag = true;
            var selectedRow = (MarginPercentageType)grdCntrlTaxTypeList.SelectedItem;
            if (selectedRow != null)
            {
                frmCashMarginPercType.marginPercTypeId = selectedRow.Id;
                frmCashMarginPercType.addMarginPercTypeWin.Height = 300;
                frmCashMarginPercType.addMarginPercTypeWin.Width = 400;
                frmCashMarginPercType.addMarginPercTypeWin.ResizeMode = ResizeMode.CanMinimize;
                frmCashMarginPercType.addMarginPercTypeWin.Content = frmCashMarginPercType;
                frmCashMarginPercType.addMarginPercTypeWin.Title = "Update Cash Margin Perc. Type";
                frmCashMarginPercType.addMarginPercTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmCashMarginPercType.addMarginPercTypeWin.ShowDialog();
            }
        }
    }
}
