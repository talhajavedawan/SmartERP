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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for winFOCSamplingList.xaml
    /// </summary>
    public partial class winFOCSamplingList : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        public winFOCSamplingList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlFOCList.ItemsSource = offerRepo.GetAllFOCSamplings();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FOC/Sampling") != null)
            {
                frmFocSampling frmFocSampling = new frmFocSampling((grdCntrlFOCList.SelectedItem as FOCSampling).Id);
                frmFocSampling.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit FOC/Sampling", "Information");
            }
        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            offerRepo = new OfferRepo();
            grdCntrlFOCList.ItemsSource = offerRepo.GetAllFOCSamplings();
        }
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add FOC/Sampling") != null)
            {
                frmFocSampling frmFocSampling = new frmFocSampling();
                frmFocSampling.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Add FOC/Sampling", "Information");
            }
        }
    }
}
