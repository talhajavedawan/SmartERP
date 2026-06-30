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
    /// Interaction logic for frmPassOn.xaml
    /// </summary>
    public partial class frmPassOn : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        PassOn passOn = new PassOn();
        ChartofAccountsRepo accountsRepo = new ChartofAccountsRepo();
        public frmPassOn()
        {
            InitializeComponent();
        }
        public frmPassOn(int passOnId)
        {
            InitializeComponent();
            passOn.Id = passOnId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (passOn.Id == 0)
            {
                passOn = new PassOn();
                if (!string.IsNullOrEmpty(txtPassOn.Text))
                {
                    passOn.passOnName = txtPassOn.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input FOC sampling name", "Error");
                    txtPassOn.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    passOn.isActive = true;
                }
                else
                {
                    passOn.isActive = false;
                }
                if(lookupCharofAccount.SelectedIndex>-1)
                {
                    passOn.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.AddPassOn(passOn);
                DXMessageBox.Show("PassOn added successfully", "Information");
                this.Close();

            }
            else
            {
                if (!string.IsNullOrEmpty(txtPassOn.Text))
                {
                    passOn.passOnName = txtPassOn.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input PassOn name", "Error");
                    txtPassOn.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    passOn.isActive = true;
                }
                else
                {
                    passOn.isActive = false;
                }
                if (lookupCharofAccount.SelectedIndex > -1)
                {
                    passOn.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.UpdatePassOn(passOn);
                DXMessageBox.Show("PassOn has been updated", "Information");
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lookupCharofAccount.ItemsSource = accountsRepo.GetAll();
            if (passOn.Id != 0)
            {
                passOn = offerRepo.GetPassOn(passOn.Id);
                txtPassOn.Text = passOn.passOnName;
                chkisactive.IsChecked = passOn.isActive;
                if(passOn.chartofAccountId!=null)
                {
                    lookupCharofAccount.Text = passOn.ChartofAccount.accountName;
                }
            }
        }
    }
}
