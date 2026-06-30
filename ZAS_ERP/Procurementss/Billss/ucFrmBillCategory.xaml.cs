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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for ucFrmBillCategory.xaml
    /// </summary>
    public partial class ucFrmBillCategory : UserControl
    {//comment
        public BillCategory billCategory = new BillCategory();
        BillRepo billRepo = new BillRepo();
        public bool editFlag = false;
        public ucFrmBillCategory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                txtBillCategory.Text = billCategory.Category;
                chkIsActive.IsChecked = billCategory.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsActive.IsChecked == true)
                billCategory.isActive = true;
            else
                billCategory.isActive = false;

            billCategory.Category = txtBillCategory.Text;
            billRepo = new BillRepo();
            if (editFlag == false)
            {
                billRepo.AddBillCategory(billCategory);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                billRepo.UpdateBillCategory(billCategory);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
