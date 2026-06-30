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

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmWarrantyAdd.xaml
    /// </summary>
    public partial class frmWarrantyAdd : Window
    {
        public frmWarrantyAdd()
        {
            InitializeComponent();
        }
        public static int warrantyId;
        ProcurementRepo repo = new ProcurementRepo();
        Warranty warranty = new Warranty();
        private void btnWarrantySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            warranty.name = txtWarranty.Text.Trim();

                
                
                
                warranty.isApproved = false;
                //warranty.user_Id = MainWindow.currentUserid;
                if (chkisactive.IsChecked == true)
                warranty.isActive = true;
            else
                warranty.isActive = false;
            if (warranty.Id == 0)
            {
                    warranty.addedDate = System.DateTime.Now;

                    if (MainWindow.currentUserid != 0)
                        warranty.user_Id = MainWindow.currentUserid;
                    else
                        warranty.user_Id = null;
                repo.addWarranty(warranty);

                MessageBox.Show("New Warranty " + txtWarranty.Text + " Added", "Congratulations");
            }
            else
            {
                repo.updateWarranty(warranty);

                MessageBox.Show("Warranty " + txtWarranty.Text + " updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winWarrantyAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            warrantyId = 0;
        }

        private void winWarrantyAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (warrantyId != 0)
            {
                warranty = repo.GetWarranty(warrantyId);
                txtWarranty.Text = warranty.name;
                chkisactive.IsChecked = warranty.isActive;
            }
        }
    }
}
