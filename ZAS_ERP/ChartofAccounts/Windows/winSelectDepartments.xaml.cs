using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for winSelectDepartments.xaml
    /// </summary>
    public partial class winSelectDepartments : DXWindow
    {
        List<Company> companies = new List<Company>();
        List<Department> departments = new List<Department>();
        List<int> departmentIds = new List<int>();
        CoaReportModuleType coaModuleType = new CoaReportModuleType();

        public winSelectDepartments()
        {
            InitializeComponent();
           

        }
        public winSelectDepartments( List<Company> _companies, CoaReportModuleType _coaModuleType)
        {
            InitializeComponent();
            coaModuleType = _coaModuleType;
            companies = _companies;
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdDepartments.SelectionChanged += OnGridSelectionChanged;

        }
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdDepartments.SelectItem(e.Node.RowHandle);
            else
                grdDepartments.UnselectItem(e.Node.RowHandle);
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdDepartments.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdDepartments.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var company in companies)
            {
                foreach (var companyDepartment in company.departments)
                {
                    if (departmentIds.Contains(companyDepartment.Id))
                        continue;
                    else
                    {
                        departments.Add(companyDepartment);
                        departmentIds.Add(companyDepartment.Id);
                    }
                }
            }
            grdDepartments.ItemsSource = departments;
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            //List<int> departmentIds = new List<int>();
            //var selectedDepartments = grdDepartments.SelectedItems;
            //if (selectedDepartments != null)
            //    foreach (Department department in selectedDepartments)
            //    {
            //        departmentIds.Add(department.Id);
            //    }
            //if (coaModuleType == CoaReportModuleType.Profit_and_Loss)
            //{
            //    winProfitandLoss winProfitandLoss = new winProfitandLoss(companies, departmentIds);
            //    winProfitandLoss.Show();
            //}
            //else
            //  if (coaModuleType == CoaReportModuleType.BalanceSheet)
            //{
            //    winBalanceSheet balanceSheet = new winBalanceSheet(companies, departmentIds);
            //    balanceSheet.Show();
            //}

            //    this.Close();
        }
    }
}
