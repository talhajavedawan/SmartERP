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
using ZAS_ERP.Tax;

namespace ZAS_ERP.Interest.UserControls
{
    /// <summary>
    /// Interaction logic for ucInterestTypeList.xaml
    /// </summary>
    public partial class ucInterestTypeList : UserControl
    {
        ucFrmInterestType frminterestType = new ucFrmInterestType();
        TaxRepo taxRepo = new TaxRepo();
        public ucInterestTypeList()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllInterestType();
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
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllInterestType();
        }

        private void AddTypeWindow()
        {
            frminterestType = new ucFrmInterestType();
            frminterestType.editFlag = false;
            frminterestType.addInterestTypeWin.Height = 300;
            frminterestType.addInterestTypeWin.Width = 400;
            frminterestType.addInterestTypeWin.ResizeMode = ResizeMode.CanMinimize;
            frminterestType.addInterestTypeWin.Content = frminterestType;
            frminterestType.addInterestTypeWin.Title = "Add Interest Type";
            frminterestType.addInterestTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frminterestType.addInterestTypeWin.ShowDialog();
        }
        private void EditTypeWindow()
        {
            frminterestType = new ucFrmInterestType();
            frminterestType.editFlag = true;
            var selectedRow = (STLInterestType)grdCntrlTaxTypeList.SelectedItem;
            if (selectedRow != null)
            {
                frminterestType.interestTypeId = selectedRow.Id;
                frminterestType.addInterestTypeWin.Height = 300;
                frminterestType.addInterestTypeWin.Width = 400;
                frminterestType.addInterestTypeWin.ResizeMode = ResizeMode.CanMinimize;
                frminterestType.addInterestTypeWin.Content = frminterestType;
                frminterestType.addInterestTypeWin.Title = "Update Interest Type";
                frminterestType.addInterestTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frminterestType.addInterestTypeWin.ShowDialog();
            }
        }
    }
}
