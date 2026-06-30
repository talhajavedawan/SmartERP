using DevExpress.CodeParser;
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

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucSelectParentSOKey.xaml
    /// </summary>
    public partial class ucSelectParentSOKey : DXWindow
    {
        List<int> companyIds = new List<int>();
        List<int> deptIds = new List<int>();
        List<SaleOrdeRrefKey> finalKeys = new List<SaleOrdeRrefKey>();
        SaleOrderRepo saleorderRepo= new SaleOrderRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        public int soReftId;
        public ucSelectParentSOKey()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);
            var userCompanies = companyRepo.GetUserCompanies(user.id);
            foreach (var company in userCompanies)
                companyIds.Add(company.Id);

            var keys = saleorderRepo.GetAllSORef(companyIds,deptIds);



            foreach(var key in keys)
            {
                bool approved = saleorderRepo.checkKey(key.Id);
                if(approved)
                {
                    finalKeys.Add(key);
                }
            }
            grdRefKeys.ItemsSource = finalKeys;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            soReftId = Convert.ToInt32((grdRefKeys.SelectedItem as SaleOrdeRrefKey).key);
            this.Close();
        }
    }
}
