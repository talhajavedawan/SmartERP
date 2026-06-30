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
    /// Interaction logic for winClaimList.xaml
    /// </summary>
    public partial class winClaimList : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        public winClaimList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlClaimList.ItemsSource = offerRepo.GetAllClaimDiscounts();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Claim/Discount") != null)
            {
                frmClaimDiscount frmClaimDiscount = new frmClaimDiscount((grdCntrlClaimList.SelectedItem as ClaimDiscount).Id);
                frmClaimDiscount.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Edit Claim/Discount", "Information");
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            offerRepo = new OfferRepo();
            grdCntrlClaimList.ItemsSource = offerRepo.GetAllClaimDiscounts();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Claim/Discount") != null)
            {
                frmClaimDiscount frmClaimDiscount = new frmClaimDiscount();
                frmClaimDiscount.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Add Claim/Discount", "Information");
            }
        }
    }
}
