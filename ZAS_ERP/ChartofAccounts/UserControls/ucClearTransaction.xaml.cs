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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucClearTransaction.xaml
    /// </summary>
    public partial class ucClearTransaction : DXWindow
    {
        public static Company company = new Company();
        public static Department department = new Department();

        public ucClearTransaction()
        {
            InitializeComponent();
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            lookupDepartment.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();

        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company=lookupCompany.SelectedItem as Company;
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
             department=lookupDepartment.SelectedItem as Department;
        }
    }
}
