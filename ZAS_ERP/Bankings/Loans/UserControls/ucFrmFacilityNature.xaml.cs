using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
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

namespace ZAS_ERP.Bankings.Loans
{

    /// <summary>
    /// Interaction logic for ucFrmFacilityNature.xaml
    /// </summary>
    public partial class ucFrmFacilityNature : UserControl
    {//comment
        public FacilityNature facilityNature = new FacilityNature();
        LoansRepo loansRepo = new LoansRepo();
        public bool editFlag = false;
        public ucFrmFacilityNature()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {
                txtFacilityNature.Text = facilityNature.NatureName;
                chkIsActive.IsChecked = facilityNature.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsActive.IsChecked == true)
                facilityNature.isActive = true;
            else
                facilityNature.isActive = false;

            facilityNature.NatureName = txtFacilityNature.Text;
            loansRepo = new LoansRepo();
            if(editFlag == false)
            {
                loansRepo.AddFacilityNature(facilityNature);
                DXMessageBox.Show("Added Succesfully!");
            }
                
            else
            {
                loansRepo.UpdateFacilityNature(facilityNature);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
