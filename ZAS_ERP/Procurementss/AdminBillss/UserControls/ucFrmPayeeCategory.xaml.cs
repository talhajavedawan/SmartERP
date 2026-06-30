using DevExpress.Xpf.Core;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmPayeeCategory.xaml
    /// </summary>
    public partial class ucFrmPayeeCategory : UserControl
    {
        public Window addCategoryWindow = new Window();
        AdminBillsRepo newBillsRepo = new AdminBillsRepo();
        public PayeeCategory payeeCategory = new PayeeCategory();
        public bool editFlag = false;

        public ucFrmPayeeCategory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            populatefIelds();
        }

        public void populatefIelds()
        {
            if (editFlag == true)
            {
                txtCategoryName.Text = payeeCategory.Name;

                if (payeeCategory.isActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCategoryName.Text))
            {
                DXMessageBox.Show("Please enter Category Name!");
                txtCategoryName.Focus();
                return;
            }

            payeeCategory.Name = txtCategoryName.Text;
            if (chkIsActive.IsChecked == true)
                payeeCategory.isActive = true;
            else
                payeeCategory.isActive = false;
                    

            if (editFlag == false && payeeCategory.Id == 0)
            {
                newBillsRepo.AddPayeeCategpry(payeeCategory);
                DXMessageBox.Show("Successfully Added!");
                addCategoryWindow.Close();
            }
            else if (editFlag == true && payeeCategory.Id != 0)
            {
                newBillsRepo.UpdatePayeeCategory(payeeCategory);
                DXMessageBox.Show("Updated Successfully!");
                addCategoryWindow.Close();
            }
        }

        
    }
}
