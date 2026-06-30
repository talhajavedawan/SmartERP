using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Reports;
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

namespace ZAS_ERP.Procurementss.SharedReports
{
    /// <summary>
    /// Interaction logic for SharedGridReport.xaml
    /// </summary>
    public partial class SharedGridReport : ThemedWindow
    {
       public GridReport gridReport = new GridReport();
        UsersRepo usersRepo = new UsersRepo();
        SharedGridGroup parentGroup = new SharedGridGroup();
        public SharedGridReport()
        {
            InitializeComponent();
        }
        public SharedGridReport(SharedGridGroup _parentGroup)
        {
            InitializeComponent();
            parentGroup = _parentGroup;
        }
        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SharedReportsStatic.selectedCompanies=null;
            SharedReportsStatic.selectedDepartments = null;
            SharedReportsStatic.selectedEmployees=null;
            SharedReportsStatic.companies= SYSTEM_STATIC.LoadCurrentUserCompanies();
            GridReportRepo repo = new GridReportRepo();
            lookupGroup.ItemsSource = repo.GetAllSharedGroups();
            if (parentGroup.Id != 0)
            {
                lookupGroup.Text = parentGroup.groupName;
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            SharedGridGroup group = new SharedGridGroup();
            var selectedCompanies = SharedReportsStatic.selectedCompanies;
            var selectedDepartments = SharedReportsStatic.selectedDepartments;
            var selectedEmployees = SharedReportsStatic.selectedEmployees;
            if(string.IsNullOrEmpty(txtGroupName.Text))
            {
                DXMessageBox.Show("Please give Group Name", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                txtGroupName.Focus();
                return;
            }
            else
            if(selectedCompanies==null)
            {
                DXMessageBox.Show("Please select atleast one company", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                txtGroupName.Focus();
                return;
            }
            group.userId = SYSTEM_STATIC.currentUser.id;
            group.Companies = selectedCompanies;
            group.groupName=txtGroupName.Text;
            group.Departments = selectedDepartments;
            group.Employees = selectedEmployees;
            group.parentId = parentGroup.Id;
            repo.AddSharedGroup(group);
            DXMessageBox.Show("Shared Group Added Successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void LookupGroup_PopupContentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parentGroup = lookupGroup.SelectedItem as SharedGridGroup;
        }
    }
}
