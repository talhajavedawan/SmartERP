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
    /// Interaction logic for ucTaxTypeList.xaml
    /// </summary>
    public partial class ucTaxTypeList : UserControl
    {
        ucFrmTaxType frmTaxType = new ucFrmTaxType();
        TaxRepo taxRepo = new TaxRepo();
      
        public ucTaxTypeList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllTaxType();
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
            grdCntrlTaxTypeList.ItemsSource = taxRepo.getAllTaxType();
        }

        private void AddTypeWindow()
        {
            frmTaxType = new ucFrmTaxType();

            frmTaxType.editFlag = false;

            frmTaxType.addTaxTypeWin.Height = 300;
            frmTaxType.addTaxTypeWin.Width = 400;
            frmTaxType.addTaxTypeWin.ResizeMode = ResizeMode.CanMinimize;
            frmTaxType.addTaxTypeWin.Content = frmTaxType;
            frmTaxType.addTaxTypeWin.Title = "Add Tax Type";
            frmTaxType.addTaxTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmTaxType.addTaxTypeWin.ShowDialog();
        }

        private void EditTypeWindow()
        {
            frmTaxType = new ucFrmTaxType();

            frmTaxType.editFlag = true;

            var selectedRow = (TaxType)grdCntrlTaxTypeList.SelectedItem;
            if (selectedRow != null)
            {
                frmTaxType.taxTypeId = selectedRow.Id;
                frmTaxType.addTaxTypeWin.Height = 300;
                frmTaxType.addTaxTypeWin.Width = 400;
                frmTaxType.addTaxTypeWin.ResizeMode = ResizeMode.CanMinimize;
                frmTaxType.addTaxTypeWin.Content = frmTaxType;
                frmTaxType.addTaxTypeWin.Title = "Update Tax Type";
                frmTaxType.addTaxTypeWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmTaxType.addTaxTypeWin.ShowDialog();
            }
        }
    }
}
