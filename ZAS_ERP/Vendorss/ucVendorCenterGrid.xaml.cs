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

namespace ZAS_ERP.Vendorss
{
    /// <summary>
    /// Interaction logic for ucVendorCenterGrid.xaml
    /// </summary>
    public partial class ucVendorCenterGrid : UserControl
    {
        public static int Editit;
        public static int vendorid;
        public ucVendorCenterGrid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadgrid();
        }

        private void BtnCollaps_Click(object sender, RoutedEventArgs e)
        {
            grdvendorGrid.ShowLoadingPanel = true;
            treeListView.CollapseAllNodes();
            grdvendorGrid.ShowLoadingPanel = false;
        }

        private void BtnExpand_Click(object sender, RoutedEventArgs e)
        {
            grdvendorGrid.ShowLoadingPanel = true;
            treeListView.ExpandAllNodes();
            grdvendorGrid.ShowLoadingPanel = false;
        }
        //ribbon button click
        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            addVendor();
        }

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            EditVendor();
        }
        private void MbtnAddNew_ItemClick(object sender, RoutedEventArgs e)
        {
            addVendor();
        }

        private void MbtnUpdate_ItemClick(object sender, RoutedEventArgs e)
        {
            EditVendor();
        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditVendor();
        }

        private void TreeListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "linkedDepartmentsss" && e.IsGetData)
            {
                var dept = grdvendorGrid.GetRow(e.Node.RowHandle) as Vendor;
                string deptNames = "";

                if (dept.departments != null && dept.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", dept.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }

        }
        private void loadgrid()
        {
            Editit = 0;
            VendorRepo vendorRepo = new VendorRepo();
            if (MainWindow.currentUserid == 0)
            {
                List<ERP_BL.Databases.Vendor> companyRepos = new List<ERP_BL.Databases.Vendor>();
                companyRepos = vendorRepo.getAll();
                this.grdvendorGrid.ItemsSource = companyRepos;
                return;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {
                this.grdvendorGrid.ItemsSource = vendorRepo.getAllbyDepartment(SYSTEM_STATIC.currentUser.employee);
                
                return;
            }
            else
            {
               this.grdvendorGrid.ItemsSource = vendorRepo.getAllActivebyDepartment(SYSTEM_STATIC.currentUser.employee);
            }
            this.grdvendorGrid.RefreshData();
        }


        private void addVendor()
        {
            frmVendoradd Vendoradd = new frmVendoradd();
            Vendoradd.ShowDialog();
            loadgrid();
        }
        private void EditVendor()
        {
              Editit = 1;
            if (grdvendorGrid.GetFocusedRowCellValue(grdvendorGrid.Columns.GetColumnByFieldName("Id")) != null)
            {
                vendorid = (int)grdvendorGrid.GetFocusedRowCellValue(grdvendorGrid.Columns.GetColumnByFieldName("Id"));
            }
            frmVendoradd Vendoradd = new frmVendoradd();
            Vendoradd.ShowDialog();
            loadgrid();
        }
    }
}
