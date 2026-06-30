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

namespace ZAS_ERP.Tax
{
    /// <summary>
    /// Interaction logic for ucTaxList.xaml
    /// </summary>
    public partial class ucTaxList : UserControl
    {
        TaxRepo taxRepo = new TaxRepo();
        ucFrmTaxAdd frmTaxAdd = new ucFrmTaxAdd();
        public ucTaxList()
        {
            InitializeComponent();
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
            grdCntrlTaxNameList.ItemsSource = taxRepo.getAllTaxes();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTaxNameList.ItemsSource = taxRepo.getAllTaxes();
        }

        private void AddTypeWindow()
        {
            frmTaxAdd = new ucFrmTaxAdd();

            frmTaxAdd.editFlag = false;

            frmTaxAdd.addTaxWin.Height = 300;
            frmTaxAdd.addTaxWin.Width = 700;
            frmTaxAdd.addTaxWin.ResizeMode = ResizeMode.CanMinimize;
            frmTaxAdd.addTaxWin.Content = frmTaxAdd;
            frmTaxAdd.addTaxWin.Title = "Add new Tax";
            frmTaxAdd.addTaxWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTaxAdd.addTaxWin.ShowDialog();
        }

        private void EditTypeWindow()
        {
            frmTaxAdd = new ucFrmTaxAdd();
            frmTaxAdd.editFlag = true;

            var selectedRow = (TaxName)grdCntrlTaxNameList.SelectedItem;
            if (selectedRow != null)
            {
                frmTaxAdd.taxNameId = selectedRow.Id;
                frmTaxAdd.addTaxWin.Height = 300;
                frmTaxAdd.addTaxWin.Width = 700;
                frmTaxAdd.addTaxWin.ResizeMode = ResizeMode.CanMinimize;
                frmTaxAdd.addTaxWin.Content = frmTaxAdd;
                frmTaxAdd.addTaxWin.Title = "Update Tax";
                frmTaxAdd.addTaxWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmTaxAdd.addTaxWin.ShowDialog();
            }
        }
    }
}
