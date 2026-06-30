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
    /// Interaction logic for winSelectCompany.xaml
    /// </summary>
    public partial class winSelectCompany : DXWindow
    {
        CoaReportModuleType coaModuleType = new CoaReportModuleType();
        public winSelectCompany()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;
         
        }
        public winSelectCompany(CoaReportModuleType _coaModuleType)
        {
            InitializeComponent();
            coaModuleType = _coaModuleType;
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;

        }
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCompany.SelectItem(e.Node.RowHandle);
            else
                grdCompany.UnselectItem(e.Node.RowHandle);
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdCompany.View;
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
                    var selectedRows = grdCompany.GetSelectedRowHandles();
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
            CompanyRepo companyRepo = new CompanyRepo();
            var userCompanies = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
            if(userCompanies!=null)
            grdCompany.ItemsSource = userCompanies;
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {

            List<Company> companies = new List<Company>();
            var selectedCompanies= grdCompany.SelectedItems;
            if(selectedCompanies!=null)
            foreach(Company company in selectedCompanies)
                {
                    companies.Add(company);
                }
            
            winSelectDepartments winSelect = new winSelectDepartments(companies,coaModuleType);
            winSelect.Show();
            this.Close();

        }
    }
}
