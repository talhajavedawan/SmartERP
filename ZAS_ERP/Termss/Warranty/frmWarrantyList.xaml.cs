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
using ERP_BL;

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmWarrantyList : Window
    {


        public bool addnew;

        ProcurementRepo repo = new ProcurementRepo();
        List<Warranty> warrantyss = new List<Warranty>();
        Warranty warranty = new Warranty();
        public frmWarrantyList()
        {
            InitializeComponent();
        }

        private void winWarranty_Loaded(object sender, RoutedEventArgs e)
        {
            loadWarrantygrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdWarranty);
        }

        private void loadWarrantygrid()
        {
            warrantyss = repo.GetWarranties();
            this.grdWarranty.ItemsSource = warrantyss;
            grdWarranty.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdWarranty.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdWarranty.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdWarranty.Columns.GetColumnByFieldName("user").Visible = false;
            addnew = false;

            
        }


        public void newWarranty()
        {
            frmWarrantyAdd frmWarrantyadd = new frmWarrantyAdd();
            frmWarrantyadd.ShowDialog();
            loadWarrantygrid();
        }


        private void mbtnNewWarranty_Click(object sender, RoutedEventArgs e)
        {
            newWarranty();

        }

        private void mbtnEditWarranty_Click(object sender, RoutedEventArgs e)
        {
            if (grdWarranty.SelectedItem != null)
            {
                frmWarrantyAdd.warrantyId = (grdWarranty.SelectedItem as Product).Id;
                newWarranty();
            }
            else
            {
                MessageBox.Show("Please select a Warranty to Edit");
            }
        }

        private void grdWarranty_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmWarrantyAdd.warrantyId = (grdWarranty.SelectedItem as Product).Id;
            newWarranty();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WinWarranty_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdWarranty);
        }
    }
}
