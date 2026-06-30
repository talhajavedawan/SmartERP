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
    /// Interaction logic for ucAddRentedVehicleOwner.xaml
    /// </summary>
    public partial class ucAddRentedVehicleOwner : UserControl
    {
        public bool editFlag = false;
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        RentedVehicleOwner vehicleOwner = new RentedVehicleOwner();
        public int vehicleOwnerId = 0;
        public ucAddRentedVehicleOwner()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtOwnerName.Text))
            {
                DXMessageBox.Show("Please Enter Owner Name!");
                txtOwnerName.Focus();
                return;
            }

            vehicleOwner.OwnerName = txtOwnerName.Text;
            if (chkIsActive.IsChecked == true)
                vehicleOwner.isActive = true;
            else
                vehicleOwner.isActive = false;

            if (editFlag == false)
            {
                billsRepo.AddRentedVehicleOwner(vehicleOwner);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                billsRepo.UpdateRentedVehicleOwner(vehicleOwner);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                vehicleOwner = billsRepo.GetRentedVehicleOwner(vehicleOwnerId);

                txtOwnerName.Text = vehicleOwner.OwnerName;
                chkIsActive.IsChecked = vehicleOwner.isActive;
            }
        }
    }
}
