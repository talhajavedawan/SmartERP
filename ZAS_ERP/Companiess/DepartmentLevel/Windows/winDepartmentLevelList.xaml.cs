using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Companiess.DepartmentLevel.Windows
{
    /// <summary>
    /// Interaction logic for winDepartmentLevelList.xaml
    /// </summary>
    public partial class winDepartmentLevelList : DXWindow
    {
        DepartmentRepo departmentrepo = new DepartmentRepo();
        public winDepartmentLevelList()
        {
            InitializeComponent();
        }

        private void btnNewDepartmentLevel_Click(object sender, RoutedEventArgs e)
        {

                winfrmDepartmentLevel departmentLevel = new winfrmDepartmentLevel();
                departmentLevel.Show();
        }

        private void btnEditDepartmentLevel_Click(object sender, RoutedEventArgs e)
        {
            
            if (gridDepartmentLevels.SelectedItem != null)
            {
                winfrmDepartmentLevel departmentLevel = new winfrmDepartmentLevel(gridDepartmentLevels.SelectedItem as ERP_BL.Databases.DepartmentLevel);
                departmentLevel.Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridDepartmentLevels.ItemsSource=  departmentrepo.getAllDepartmentLevels();
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            departmentrepo = new DepartmentRepo();
            gridDepartmentLevels.ItemsSource = departmentrepo.getAllDepartmentLevels();
        }
    }
}
