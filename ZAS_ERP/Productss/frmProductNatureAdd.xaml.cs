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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmProductNatureAdd.xaml
    /// </summary>
    public partial class frmProductNatureAdd : Window
    {
        public frmProductNatureAdd()
        {
            InitializeComponent();
        }
        public static int natureId;
        ProductRepo repo = new ProductRepo();
        ProductNature productNature = new ProductNature();
        private void btnNatureSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            productNature.nature = txtNature.Text.Trim();
            if (chkisactive.IsChecked == true)
                productNature.isActive = true;
            else
                productNature.isActive = false;
            if (productNature.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        productNature.user_Id = MainWindow.currentUserid;
                    else
                        productNature.user_Id = null;

                    repo.Addnature(productNature);

                MessageBox.Show("New Productnature " + txtNature.Text + " Added", "Congratulations");
            }
            else
            {
                repo.Updatenature(productNature);

                MessageBox.Show("Productnature " + txtNature.Text + " updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winProductNatureAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            natureId = 0;
        }

        private void winProductNatureAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (natureId != 0)
            {
                productNature = repo.getnature(natureId);
                txtNature.Text = productNature.nature;
                chkisactive.IsChecked = productNature.isActive;
            }
        }
    }
}
