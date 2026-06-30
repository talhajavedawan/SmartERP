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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for ucSTAdd.xaml
    /// </summary>
    public partial class ucSTAdd : UserControl
    {
        public bool editFlag = false;
        public SaleOrderRepo repo = new SaleOrderRepo();
        public ShippingTerm term = new ShippingTerm();
        public ucSTAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                if (term.Name != null)
                {
                    txtSTName.Text = term.Name;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSTName.Text))
            {
                DXMessageBox.Show("Please enter Document Name!");
                txtSTName.Focus();
                return;
            }
            term.Name = txtSTName.Text;
            if (editFlag == false)
            {
                repo.AddST(term);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true)
            {
                repo.UpdateST(term);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }
    }
}
