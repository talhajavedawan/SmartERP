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

namespace ZAS_ERP.Principalss
{
    /// <summary>
    /// Interaction logic for ucPrincipalRegister.xaml
    /// </summary>
    public partial class ucPrincipalRegister : UserControl
    {
        public static int Editit;
        public static int principalid;
        public ucPrincipalRegister()
        {
            InitializeComponent();
        }
        private void loadgrid()

        {
            Editit = 0;

            PrincipalRepo principalRepo = new PrincipalRepo();
          
            if (MainWindow.currentUserid == 0)
            {
                List<ERP_BL.Databases.Principal> companyRepos = new List<ERP_BL.Databases.Principal>();

                companyRepos = principalRepo.getAll();
                this.grdPrincipalcompanies.ItemsSource = companyRepos;
                return;

            }
            List<Principal> PrincipalCompanies = new List<Principal>();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            var currentemployee = employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Principal") != null)
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Principals)
                    {
                        if (!PrincipalCompanies.Contains(cust))
                        {
                            PrincipalCompanies.Add(cust);
                        }
                    }
                }
            }
            else
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Principals)
                    {
                        if (!PrincipalCompanies.Contains(cust) && cust.isActive == true)
                        {
                            PrincipalCompanies.Add(cust);
                        }
                    }
                }
            }
            this.grdPrincipalcompanies.ItemsSource = PrincipalCompanies;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPrincipalcompanies); //comment by waqas
        }

        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();
        }

        private void mbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Editit = 1;
            if (grdPrincipalcompanies.GetFocusedRowCellValue(grdPrincipalcompanies.Columns.GetColumnByFieldName("Id")) != null)
            {

                principalid = (int)grdPrincipalcompanies.GetFocusedRowCellValue(grdPrincipalcompanies.Columns.GetColumnByFieldName("Id"));

                //MessageBox.Show(empid.ToString());
            }
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();
        }

        private void mbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadgrid();
        }

        private void btnCollaps_Click(object sender, RoutedEventArgs e)
        {
            grdPrincipalcompanies.ShowLoadingPanel = true;
            treeListView1.CollapseAllNodes();
            grdPrincipalcompanies.ShowLoadingPanel = false;
        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            grdPrincipalcompanies.ShowLoadingPanel = true;
            treeListView1.ExpandAllNodes();
            grdPrincipalcompanies.ShowLoadingPanel = false;
        }

        private void TreeListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void TreeListView1_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {

        }

        private void MbtnAddNew_ItemClick(object sender, RoutedEventArgs e)
        {
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();
        }

        private void mbtnUpdate_ItemClick(object sender, RoutedEventArgs e)
        {
            Editit = 1;
            if (grdPrincipalcompanies.GetFocusedRowCellValue(grdPrincipalcompanies.Columns.GetColumnByFieldName("Id")) != null)
            {

                principalid = (int)grdPrincipalcompanies.GetFocusedRowCellValue(grdPrincipalcompanies.Columns.GetColumnByFieldName("Id"));

                //MessageBox.Show(empid.ToString());
            }
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();
        }
    }
}
