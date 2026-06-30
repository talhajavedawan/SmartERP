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
    /// Interaction logic for ucInterestList.xaml
    /// </summary>
    public partial class ucInterestList : UserControl
    {
        TaxRepo taxRepo = new TaxRepo();
        ucFrmInterestAdd frmInterestAdd = new ucFrmInterestAdd();
        public ucInterestList()
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
            grdCntrlInterestNameList.ItemsSource = taxRepo.getAllInterests();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlInterestNameList.ItemsSource = taxRepo.getAllInterests();
        }

        private void AddTypeWindow()
        {
            frmInterestAdd = new ucFrmInterestAdd();

            frmInterestAdd.editFlag = false;

            frmInterestAdd.addInterestWin.Height = 300;
            frmInterestAdd.addInterestWin.Width = 700;
            frmInterestAdd.addInterestWin.ResizeMode = ResizeMode.CanMinimize;
            frmInterestAdd.addInterestWin.Content = frmInterestAdd;
            frmInterestAdd.addInterestWin.Title = "Add new Interest";
            frmInterestAdd.addInterestWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmInterestAdd.addInterestWin.ShowDialog();
        }

        private void EditTypeWindow()
        {
            frmInterestAdd = new ucFrmInterestAdd();
            frmInterestAdd.editFlag = true;

            var selectedRow = (STLInterest)grdCntrlInterestNameList.SelectedItem;
            if (selectedRow != null)
            {
                frmInterestAdd.interestNameId = selectedRow.Id;
                frmInterestAdd.addInterestWin.Height = 300;
                frmInterestAdd.addInterestWin.Width = 700;
                frmInterestAdd.addInterestWin.ResizeMode = ResizeMode.CanMinimize;
                frmInterestAdd.addInterestWin.Content = frmInterestAdd;
                frmInterestAdd.addInterestWin.Title = "Update Interest";
                frmInterestAdd.addInterestWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmInterestAdd.addInterestWin.ShowDialog();
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditTypeWindow();

        }
    }
}
