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
    /// Interaction logic for frmUnitOfMeasureAdd.xaml
    /// </summary>
    public partial class frmUnitOfMeasureAdd : Window
    {
        public frmUnitOfMeasureAdd()
        {
            InitializeComponent();
        }
        public static int unitOfMeasureId;
        ProductRepo repo = new ProductRepo();
        UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
        private void btnMeasureSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            unitOfMeasure.unitOfMeasure = txtMeasure.Text.Trim();
            if (chkisactive.IsChecked == true)
                unitOfMeasure.isActive = true;
            else
                unitOfMeasure.isActive = false;
            if (unitOfMeasure.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        unitOfMeasure.user_Id = MainWindow.currentUserid;
                    else
                        unitOfMeasure.user_Id = null;
                repo.AddUnitOfMeasure(unitOfMeasure);

                MessageBox.Show("New Unit Of Measure (" + txtMeasure.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.UpdateUnitOfMeasure(unitOfMeasure);

                MessageBox.Show("Unit Of Measure (" + txtMeasure.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winUnitOfMeasureAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            unitOfMeasureId = 0;
        }

        private void winUnitOfMeasureAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (unitOfMeasureId != 0)
            {
                unitOfMeasure = repo.getUnitOfMeasure(unitOfMeasureId);
                txtMeasure.Text = unitOfMeasure.unitOfMeasure;
                chkisactive.IsChecked = unitOfMeasure.isActive;
            }
        }
    }
}
