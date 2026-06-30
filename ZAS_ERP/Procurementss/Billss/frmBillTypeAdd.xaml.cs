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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmBillTypeAdd.xaml
    /// </summary>
    public partial class frmBillTypeAdd : Window
    {
        public frmBillTypeAdd()
        {
            InitializeComponent();
        }
        public static int billTypeId;
        BillRepo repo = new BillRepo();
        BillType billType = new BillType();
        private void btnNatureSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            billType.billType = txtNature.Text.Trim();
            if (chkisactive.IsChecked == true)
                billType.isActive = true;
            else
                billType.isActive = false;
            if (billType.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        billType.user_Id = MainWindow.currentUserid;
                    else
                        billType.user_Id = null;

                    repo.AddbillType(billType);

                MessageBox.Show("New billType " + txtNature.Text + " Added", "Congratulations");
            }
            else
            {
                repo.UpdatebillType(billType);

                MessageBox.Show("BillType " + txtNature.Text + " updated", "Congratulations");
            }
            this.Close(); 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winBillTypeAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            billTypeId = 0;
        }

        private void winBillTypeAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (billTypeId != 0)
            {
                billType = repo.getbillType(billTypeId);
                txtNature.Text = billType.billType;
                chkisactive.IsChecked = billType.isActive;
            }
        }
    }
}
