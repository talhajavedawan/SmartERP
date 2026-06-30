using ERP_BL.Databases;
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

namespace ZAS_ERP.Customerss
{
    /// <summary>
    /// Interaction logic for ucReligionList.xaml
    /// </summary>
    public partial class ucReligionList : UserControl
    {
        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        ucAddReligion ucAddReligion = new ucAddReligion();

        public ucReligionList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlReligionMethodList.ItemsSource = customerCompRepo.GetAllReligion();
            
        }

        private void MbtnAddReligionMethod_Click(object sender, RoutedEventArgs e)
        {
            ucAddReligion = new ucAddReligion();
            ucAddReligion.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
            ucAddReligion.addCategoryWindow.Height = 250;
            ucAddReligion.addCategoryWindow.Width = 350;
            ucAddReligion.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ucAddReligion.editFlag = false;
            ucAddReligion.addCategoryWindow.Content = ucAddReligion;
            ucAddReligion.addCategoryWindow.ShowDialog();
        }

        private void MbtnEditReligionMethod_Click(object sender, RoutedEventArgs e)
        {
            customerCompRepo = new CustomerCompRepo();
            var selectedRow = (grdCntrlReligionMethodList.SelectedItem as Religion);
            if(selectedRow != null)
            {
                ucAddReligion = new ucAddReligion();
                ucAddReligion.ReligionId = selectedRow.Id;
                ucAddReligion.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
                ucAddReligion.addCategoryWindow.Height = 250;
                ucAddReligion.addCategoryWindow.Width = 350;
                ucAddReligion.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucAddReligion.editFlag = true;
                ucAddReligion.addCategoryWindow.Content = ucAddReligion;
                ucAddReligion.addCategoryWindow.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdCntrlReligionMethodList.ItemsSource = customerCompRepo.GetAllReligion(); 
        }
       
    }
}
