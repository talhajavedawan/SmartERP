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
    /// Interaction logic for frmUniqueNumber.xaml
    /// </summary>
    public partial class frmUniqueNumber : DXWindow
    {
        OfferRepo offerRepo = new OfferRepo();
        UniqueNumber uniqueNumber = new UniqueNumber();
        public frmUniqueNumber()
        {
            InitializeComponent();
        }
        public frmUniqueNumber(int uniqueNumberId)
        {
            InitializeComponent();
            uniqueNumber.Id = uniqueNumberId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (uniqueNumber.Id == 0)
            {
                uniqueNumber = new UniqueNumber();
                if (!string.IsNullOrEmpty(txtUniqueNumber.Text))
                {
                    uniqueNumber.UniqueName = txtUniqueNumber.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input Unique Number", "Error");
                    txtUniqueNumber.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    uniqueNumber.isActive = true;
                }
                else
                {
                    uniqueNumber.isActive = false;
                }
                if (datFrom != null)
                {
                    uniqueNumber.From = (DateTime)datFrom.EditValue;
                }
                else
                {
                    DXMessageBox.Show("Please input date from", "Error");
                    return;
                }
                if (datTo != null)
                {
                    uniqueNumber.To = (DateTime)datTo.EditValue;
                }
                else
                {
                    DXMessageBox.Show("Please input date To", "Error");
                    return;
                }

                offerRepo.AddUniqueNUmber(uniqueNumber);
                DXMessageBox.Show("Unique Number added successfully", "Information");
                this.Close();

            }
            else
            {
                if (!string.IsNullOrEmpty(txtUniqueNumber.Text))
                {
                    uniqueNumber.UniqueName = txtUniqueNumber.Text;
                }
                else
                {
                    DXMessageBox.Show("Please input Unique Number", "Error");
                    txtUniqueNumber.Focus();
                    return;

                }
                if (chkisactive.IsChecked == true)
                {
                    uniqueNumber.isActive = true;
                }
                else
                {
                    uniqueNumber.isActive = false;
                }
                uniqueNumber.From = (DateTime)datFrom.EditValue;
                uniqueNumber.To = (DateTime)datTo.EditValue;
                offerRepo.UpdateUniqueNumber(uniqueNumber);
                DXMessageBox.Show("Unique Number has been updated", "Information");
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (uniqueNumber.Id != 0)
            {
                uniqueNumber = offerRepo.GetUniqueNumber(uniqueNumber.Id);
                txtUniqueNumber.Text = uniqueNumber.UniqueName;
                chkisactive.IsChecked = uniqueNumber.isActive;
                datFrom.EditValue = uniqueNumber.From;
                datTo.EditValue = uniqueNumber.To;
            }
        }
    }
}
