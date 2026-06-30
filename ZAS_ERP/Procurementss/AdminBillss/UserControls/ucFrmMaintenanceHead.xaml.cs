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
    /// Interaction logic for ucFrmMaintenanceHead.xaml
    /// </summary>
    public partial class ucFrmMaintenanceHead : UserControl
    {

        public bool editFlag = false;
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        MaintenanceHead maintenanceHead = new MaintenanceHead();
        public int maintenanceHeadId = 0;
        public ucFrmMaintenanceHead()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtMaintenanceHead.Text))
            {
                DXMessageBox.Show("Please Enter Owner Name!");
                txtMaintenanceHead.Focus();
                return;
            }

            maintenanceHead.HeadName = txtMaintenanceHead.Text;
            if (chkIsActive.IsChecked == true)
                maintenanceHead.isActive = true;
            else
                maintenanceHead.isActive = false;

            if (editFlag == false)
            {
                billsRepo.AddMaintenanceHead(maintenanceHead);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                billsRepo.UpdateMaintenanceHead(maintenanceHead);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                maintenanceHead = billsRepo.GetMaintenanceHead(maintenanceHeadId);

                txtMaintenanceHead.Text = maintenanceHead.HeadName;
                chkIsActive.IsChecked = maintenanceHead.isActive;
            }
        }
    }
}
