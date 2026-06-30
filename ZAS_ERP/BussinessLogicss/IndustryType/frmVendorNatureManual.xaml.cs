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

namespace ZAS_ERP.BussinessLogicss.IndustryType
{
    /// <summary>
    /// Interaction logic for frmVendorNatureManual.xaml
    /// </summary>
    public partial class frmVendorNatureManual : DXWindow
    {
        public frmVendorNatureManual()
        {
            InitializeComponent();
        }
        public static int industryTypeId;
        CompanyRepo repo = new CompanyRepo();
        VendorNatureManual industryType = new VendorNatureManual();
        private void btnIndustryTypeSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                industryType.name = txtIndustryType.Text.Trim();
                industryType.isApproved = false;
                //industryType.user_Id = MainWindow.currentUserid;
                if (chkisactive.IsChecked == true)
                    industryType.isActive = true;


                if (chkisVoid.IsChecked == true)
                    industryType.isVoid = true;
                else
                    industryType.isVoid = false;
                if (industryType.Id == 0)
                {
                    industryType.addedDate = System.DateTime.Now;

                    if (MainWindow.currentUserid != 0)
                        industryType.user_Id = MainWindow.currentUserid;
                    else
                        industryType.user_Id = null;
                    repo.addVendorNaturesManual(industryType);

                    MessageBox.Show("New Vendor Nature " + txtIndustryType.Text + " Added", "Congratulations");
                }
                else
                {
                    repo.updateVendorNaturesManual(industryType);

                    MessageBox.Show("Vendor Nature " + txtIndustryType.Text + " updated", "Congratulations");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winIndustryTypeAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            industryTypeId = 0;
        }

        private void winIndustryTypeAdd_Loaded(object sender, RoutedEventArgs e)
        {
            //if (industryTypeId != 0)
            //{
            //    industryType = repo.GetVendorNaturesManual(industryTypeId);
            //    txtIndustryType.Text = industryType.name;
            //    chkisactive.IsChecked = industryType.isActive;
            //    chkisVoid.IsChecked = industryType.isVoid;
            //}
        }
    }
}
