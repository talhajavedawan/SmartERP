using ERP_BL.Bankings;
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

namespace ZAS_ERP.Bankings.Loans
{
    /// <summary>
    /// Interaction logic for ucFacilityNatureList.xaml
    /// </summary>
    public partial class ucFacilityNatureList : UserControl
    {//comment
        LoansRepo loansRepo = new LoansRepo();
        public ucFacilityNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAddFacilityNature_Click(object sender, RoutedEventArgs e)
        {
            ucFrmFacilityNature frmFacilityNature = new ucFrmFacilityNature();
            Window win = new Window();
            frmFacilityNature.editFlag = false;
            win.Content = frmFacilityNature;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditFacilityNature_Click(object sender, RoutedEventArgs e)
        {
            if(grdCntrlFacilityNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlFacilityNatureList.SelectedItem as FacilityNature;
                ucFrmFacilityNature frmFacilityNature = new ucFrmFacilityNature();
                Window win = new Window();
                frmFacilityNature.facilityNature = selectedRow;
                frmFacilityNature.editFlag = true;
                win.Content = frmFacilityNature;
                win.Width = 300;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loansRepo = new LoansRepo();
            grdCntrlFacilityNatureList.ItemsSource = loansRepo.GetAllFacilityNatures();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loansRepo = new LoansRepo();
            grdCntrlFacilityNatureList.ItemsSource = loansRepo.GetAllFacilityNatures();
        }
    }
}
