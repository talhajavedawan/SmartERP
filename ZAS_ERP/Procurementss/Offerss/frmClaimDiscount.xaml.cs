using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
    /// Interaction logic for frmClaimDiscount.xaml
    /// </summary>
    public partial class frmClaimDiscount : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        ClaimDiscount claimDiscount = new ClaimDiscount();
        ChartofAccountsRepo accountsRepo = new ChartofAccountsRepo();
        public frmClaimDiscount()
        {
            InitializeComponent();
        }
        public frmClaimDiscount( int claimId)
        {
            InitializeComponent();
            claimDiscount.Id = claimId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (claimDiscount.Id == 0)
            {
                claimDiscount = new ClaimDiscount();
                if (!string.IsNullOrEmpty(txtClaimDiscount.Text))
                {
                    claimDiscount.discountName = txtClaimDiscount.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input Claim/Discount name", "Error");
                    txtClaimDiscount.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    claimDiscount.isActive = true;
                }
                else
                {
                    claimDiscount.isActive = false;
                }
                if (lookupCharofAccount.SelectedIndex > -1)
                {
                    claimDiscount.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.AddClaimDiscount(claimDiscount);
                DXMessageBox.Show("Claim/Discount added successfully", "Information");
                this.Close();

            }
            else
            {
                if (!string.IsNullOrEmpty(txtClaimDiscount.Text))
                {
                    claimDiscount.discountName = txtClaimDiscount.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input Claim/Discount name", "Error");
                    txtClaimDiscount.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    claimDiscount.isActive = true;
                }
                else
                {
                    claimDiscount.isActive = false;
                }
                if (lookupCharofAccount.SelectedIndex > -1)
                {
                    claimDiscount.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.UpdateClaimDiscount(claimDiscount);
                DXMessageBox.Show("Claim/Discount has been updated", "Information");
                this.Close();
            }
        }



        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lookupCharofAccount.ItemsSource = accountsRepo.GetAll();
            if (claimDiscount.Id != 0)
            {
                claimDiscount = offerRepo.GetClaimDiscount(claimDiscount.Id);
                txtClaimDiscount.Text = claimDiscount.discountName;
                chkisactive.IsChecked = claimDiscount.isActive;
                if (claimDiscount.chartofAccountId != null)
                {
                    lookupCharofAccount.Text = claimDiscount.ChartofAccount.accountName;
                }
            }
        }
    }
}
