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
    /// Interaction logic for frmFocSampling.xaml
    /// </summary>
    public partial class frmFocSampling : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        FOCSampling sampling = new FOCSampling();
        ChartofAccountsRepo accountsRepo = new ChartofAccountsRepo();
        public frmFocSampling()
        {
            InitializeComponent();
        }
        public frmFocSampling(int samplingId)
        {
            InitializeComponent();
            sampling.Id = samplingId;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(sampling.Id==0)
            {
                sampling = new FOCSampling(); 
               if(!string.IsNullOrEmpty(txtFocSampling.Text))
                {
                    sampling.samplingtName = txtFocSampling.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input FOC sampling name","Error");
                    txtFocSampling.Focus();
                    return;

                }
               if(chkisactive.IsChecked==true)
                {
                    sampling.isActive = true;
                }
               else
                {
                    sampling.isActive = false;
                }
                if (lookupCharofAccount.SelectedIndex > -1)
                {
                    sampling.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.AddFOCSampling(sampling);
                DXMessageBox.Show("FOC/sampling added successfully","Information");
                this.Close();

            }
            else
            {
                if (!string.IsNullOrEmpty(txtFocSampling.Text))
                {
                    sampling.samplingtName = txtFocSampling.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input FOC sampling name", "Error");
                    txtFocSampling.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    sampling.isActive = true;
                }
                else
                {
                    sampling.isActive = false;
                }
                if (lookupCharofAccount.SelectedIndex > -1)
                {
                    sampling.chartofAccountId = (lookupCharofAccount.SelectedItem as ChartofAccount).Id;
                }
                offerRepo.UpdateFOCSampling(sampling);
                DXMessageBox.Show("FOC/sampling has been updated", "Information");
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lookupCharofAccount.ItemsSource = accountsRepo.GetAll();
            if (sampling.Id!=0)
            {
                sampling=offerRepo.GetFOCSampling(sampling.Id);
                txtFocSampling.Text = sampling.samplingtName;
                chkisactive.IsChecked = sampling.isActive;
                if (sampling.chartofAccountId != null)
                {
                    lookupCharofAccount.Text = sampling.ChartofAccount.accountName;
                }
            }
        }
    }
}
