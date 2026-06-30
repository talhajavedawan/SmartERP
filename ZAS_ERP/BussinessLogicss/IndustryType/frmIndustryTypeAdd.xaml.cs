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
using DevExpress.Xpf.Core;
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmIndustryTypeAdd.xaml
    /// </summary>
    public partial class frmIndustryTypeAdd : DXWindow
    {
        public frmIndustryTypeAdd()
        {
            InitializeComponent();
        }
        public static int industryTypeId;
        CompanyRepo repo = new CompanyRepo();
        IndustryType industryType = new IndustryType();
        private void btnIndustryTypeSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            industryType.name = txtIndustryType.Text.Trim();

                
                
                
                industryType.isApproved = false;
                //industryType.user_Id = MainWindow.currentUserid;
                if (chkisactive.IsChecked == true)
                    industryType.isActive = true;

                if (chkIsVendorType.IsChecked == true)
                    industryType.isVendorType = true;
                else
                    industryType.isVendorType = false;
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
                repo.addIndustryType(industryType);

                MessageBox.Show("New IndustryType " + txtIndustryType.Text + " Added", "Congratulations");
            }
            else
            {
                repo.updateIndustryType(industryType);

                MessageBox.Show("IndustryType " + txtIndustryType.Text + " updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
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
            if (industryTypeId != 0)
            {
                industryType = repo.GetIndustryType(industryTypeId);
                txtIndustryType.Text = industryType.name;
                chkisactive.IsChecked = industryType.isActive;
                chkisVoid.IsChecked = industryType.isVoid;
                chkIsVendorType.IsChecked = industryType.isVendorType;
            }
        }
    }
}
