using DevExpress.Xpf.Grid;
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

namespace ZAS_ERP.Vendorss
{
    /// <summary>
    /// Interaction logic for frmVendorCenter.xaml
    /// </summary>
    public partial class frmVendorCenter : Window
    {

        public static int Editit;
        public static int vendorid;
        public frmVendorCenter()
        {
            InitializeComponent();
        }

        private void mbtnaddcomp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmVendoradd Vendoradd = new frmVendoradd();
            Vendoradd.ShowDialog();
            loadgrid();
        }
        private void loadgrid()

        {
            Editit = 0;
            Vendor vendor = new Vendor();
            VendorRepo vendorRepo = new VendorRepo();
            //companyRepos = vendorRepo.getAll();
            //this.grdVendorcompanies.ItemsSource = companyRepos;
            if (MainWindow.currentUserid == 0)
            {
                List<ERP_BL.Databases.Vendor> companyRepos = new List<ERP_BL.Databases.Vendor>();

                companyRepos = vendorRepo.getAll();
                this.grdVendorcompanies.ItemsSource = companyRepos;
                return;

            }
            List<Vendor> VendorCompanies = new List<Vendor>();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            //var currentuser =employeeRepo.getuser(MainWindow.currentUserid);
            var currentemployee = employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Vendors)
                    {
                        if (!VendorCompanies.Contains(cust))
                        {
                            VendorCompanies.Add(cust);
                        }
                    }
                }
            }
            else
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Vendors)
                    {
                        if (!VendorCompanies.Contains(cust)&&cust.isActive==true)
                        {
                            VendorCompanies.Add(cust);
                        }
                    }
                }
            }
            
            this.grdVendorcompanies.ItemsSource = VendorCompanies;
            // grdcutomercompanies.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "Id" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "company.CompanyName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.FName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.FName").Header = "First Name";
            //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.LName" });
            //grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.LName").Header = "Last Name";

        }

        private void winVendorCenter_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorcompanies);

        }

        private void btnEditVendor_Click(object sender, RoutedEventArgs e)
        {
            Editit = 1;
            if (grdVendorcompanies.GetFocusedRowCellValue(grdVendorcompanies.Columns.GetColumnByFieldName("Id")) != null)
            {

               vendorid = (int)grdVendorcompanies.GetFocusedRowCellValue(grdVendorcompanies.Columns.GetColumnByFieldName("Id"));

                //MessageBox.Show(empid.ToString());
            }
            frmVendoradd Vendoradd = new frmVendoradd();
            Vendoradd.ShowDialog();
            loadgrid();

        }

        private void btnNewVendor_Click(object sender, RoutedEventArgs e)
        {
            frmVendoradd Vendoradd = new frmVendoradd();
            Vendoradd.ShowDialog();
            loadgrid();

        }

        private void WinVendorCenter_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorcompanies);
        }
    }
}
