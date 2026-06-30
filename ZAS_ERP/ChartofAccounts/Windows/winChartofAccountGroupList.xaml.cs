using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winChartofAccountGroupList.xaml
    /// </summary>
    public partial class winChartofAccountGroupList : DXWindow
    {
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();

        public winChartofAccountGroupList()
        {
            InitializeComponent();
        }
        private void grdCOAGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Chart of Account Group") != null)
            {
                if (grdCOAGroups.SelectedItem != null)
                {
                    var group = grdCOAGroups.SelectedItem as ChartofAccountGroup;
                    winChartofAccountGroups groupWin = new winChartofAccountGroups(group);
                    groupWin.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission Denied","Information");

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdCOAGroups.ItemsSource= coaRepo.GetAllChartofAccountGroups();
            grdCOAGroupsLinks.ItemsSource= coaRepo.GetAllChartofAccountGroups();

        }

        private void btnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account Group") != null)
            {
                winChartofAccountGroups groupWin = new winChartofAccountGroups();
                groupWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }

        }

        private void grdCOAGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            if(grdCOAGroups.SelectedItem!=null)
            {
                var group = grdCOAGroups.SelectedItem as ChartofAccountGroup;
                gridAllCompany.ItemsSource = group.Companies;
                gridAllDepartment.ItemsSource = group.Departments;
                gridAllEmployee.ItemsSource = group.Employees;
                gridGroupChartofAccounts.ItemsSource = group.ChartofAccounts;
            }
        }

        private void btnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Chart of Account Group") != null)
            {
                winChartofAccountGroups groupWin = new winChartofAccountGroups();
                groupWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void grdCOAGroupsLinks_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            if (grdCOAGroups.SelectedItem != null)
            {
                var group = grdCOAGroups.SelectedItem as ChartofAccountGroup;
                gridGroupChartofAccountsLinks.ItemsSource = group.ChartofAccounts;
            }
        }

        private void grdCOAGroupsLinks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCOAGroupsLinks.SelectedItem != null)
            {
                var group = grdCOAGroupsLinks.SelectedItem as ChartofAccountGroup;
                if (group != null)
                {
                    winCOAGroupAdminPanel adminPanel = new winCOAGroupAdminPanel(group);
                    adminPanel.Show();
                }
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            coaRepo = new ChartofAccountsRepo();
            grdCOAGroups.ItemsSource = coaRepo.GetAllChartofAccountGroups();
            grdCOAGroupsLinks.ItemsSource = coaRepo.GetAllChartofAccountGroups();
        }
    }
}
