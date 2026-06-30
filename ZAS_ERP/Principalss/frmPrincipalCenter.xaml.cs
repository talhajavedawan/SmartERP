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

namespace ZAS_ERP.Principalss
{
    /// <summary>
    /// Interaction logic for frmPrincipalCenter.xaml
    /// </summary>
    public partial class frmPrincipalCenter : Window
    {
        public static int Editit;
        public static int principalid;
        public frmPrincipalCenter()
        {
            InitializeComponent();
        }

        private void mbtnaddcomp_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPrincipalcompanies);
        }
        private void loadgrid()

        {
            Editit = 0;
            //Principal principal = new Principal();
            PrincipalRepo principalRepo = new PrincipalRepo();
            //List<ERP_BL.Databases.Principal> companyRepos = new List<ERP_BL.Databases.Principal>();
            //companyRepos = principalRepo.getAll();
            //this.grdPrincipalcompanies.ItemsSource = companyRepos;
            if (MainWindow.currentUserid == 0)
            {
                List<ERP_BL.Databases.Principal> companyRepos = new List<ERP_BL.Databases.Principal>();

                companyRepos = principalRepo.getAll();
                this.grdPrincipalcompanies.ItemsSource = companyRepos;
                return;

            }
            List<Principal> PrincipalCompanies = new List<Principal>();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            //var currentuser =employeeRepo.getuser(MainWindow.currentUserid);
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
                        if (!PrincipalCompanies.Contains(cust)&&cust.isActive==true)
                        {
                            PrincipalCompanies.Add(cust);
                        }
                    }
                }
            }
            this.grdPrincipalcompanies.ItemsSource = PrincipalCompanies;
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

        private void winPrincipalCenter_Loaded(object sender, RoutedEventArgs e)
        {

            loadgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPrincipalcompanies);
        }

        private void btnEditPrincipal_Click(object sender, RoutedEventArgs e)
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

        private void btnNewPrincipal_Click(object sender, RoutedEventArgs e)
        {
            frmPrincipaladd Principaladd = new frmPrincipaladd();
            Principaladd.ShowDialog();
            loadgrid();

        }

        private void WinPrincipalCenter_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPrincipalcompanies);
        }
    }
}
