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

using ERP_BL.Config;
using ERP_BL.Enums;

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmProductNatureList : Window
    {


        List<ProductNature> productNatures= new List<ProductNature>();
        ProductRepo repo = new ProductRepo();

        public frmProductNatureList()
        {
            InitializeComponent();
                       
        }

        private void winProductNatureList_Loaded(object sender, RoutedEventArgs e)
        {
            loadProductNature();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdProductNature);


        }



        private void loadProductNature()
        {
           
            if (MainWindow.currentUserid == 0)
                productNatures = repo.getallnature();
            else
                productNatures = repo.getActiveProductNatures();
                       
            this.grdProductNature.ItemsSource = productNatures;
            grdProductNature.Columns.GetColumnByFieldName("Id").Visible = false;
            grdProductNature.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdProductNature.Columns.GetColumnByFieldName("user").Visible = false;

        }

        public void newProductNature()
        {
            frmProductNatureAdd frmProductNatureadd = new frmProductNatureAdd();
            frmProductNatureadd.ShowDialog();
            loadProductNature();
        }
        

        private void mbtnNewProductNature_Click(object sender, RoutedEventArgs e)
        {
            newProductNature();

        }

        private void mbtnEditProductNature_Click(object sender, RoutedEventArgs e)
        {
            if (grdProductNature.SelectedItem != null)
            {
                Productss.frmProductNatureAdd.natureId = (grdProductNature.SelectedItem as ProductNature).Id;
                newProductNature();
            }
            else
            {
                MessageBox.Show("Please select a Unit Of Measure to Edit");
            }
        }

        private void grdProductNature_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Productss.frmProductNatureAdd.natureId = (grdProductNature.SelectedItem as ProductNature).Id;
            newProductNature();
        }

        private void WinProductNatureList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdProductNature);
        }
    }
}
