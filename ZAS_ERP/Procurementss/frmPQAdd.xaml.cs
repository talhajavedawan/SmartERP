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

namespace ZAS_ERP.Procurementss.CostSheet
{
    /// <summary>
    /// Interaction logic for frmPQAdd.xaml
    /// </summary>
    public partial class frmPQAdd : UserControl
    {
        public bool editFlag = false;
        public SaleOrderRepo repo = new SaleOrderRepo();
        public PQDocument document = new PQDocument();

        public frmPQAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
          
            if (String.IsNullOrEmpty(txtPQName.Text))
            {
                DXMessageBox.Show("Please enter Document Name!");
                txtPQName.Focus();
                return;
            }
            document.Name = txtPQName.Text;
            if (editFlag == false)
            {
                repo.AddPQ(document);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true)
            {
                repo.UpdatePQ(document);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                if (document.Name != null)
                {
                    txtPQName.Text = document.Name;
                }
            }
        }
    }
}
