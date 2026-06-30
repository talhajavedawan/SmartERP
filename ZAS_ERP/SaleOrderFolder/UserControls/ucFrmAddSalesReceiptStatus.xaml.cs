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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddSalesReceiptStatus.xaml
    /// </summary>
    public partial class ucFrmAddSalesReceiptStatus : UserControl
    {
        public Window win = new Window();
        SalesReceiptRepo repo = new SalesReceiptRepo();
        ucSaleReceiptsStatusList ucSaleReceiptsStatusList = new ucSaleReceiptsStatusList();


        public bool saveEditFlag = false;
        public int statusId;
        public ucFrmAddSalesReceiptStatus()
        {
            InitializeComponent();
        }

        private void CpStatus_ColorChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saveEditFlag == true)
                {
                    SalesReceiptStatus status = new SalesReceiptStatus();

                    status.Id = statusId;
                    status.Status = txtStatus.Text;
                    status.backcolor = cpStatus.Text;

                    if (chkisActive.IsChecked == true)
                        status.isActive = true;
                    else
                        status.isActive = false;
                    if (chkDisable.IsChecked == true)
                        status.isDisable = true;
                    else
                        status.isDisable = false;

                    repo.UpdateSaleReceiptStatus(status);
                    MessageBox.Show("Successfully Updated!");
                    win.Close();
                }
                else
                {
                    SalesReceiptStatus status = new SalesReceiptStatus();

                    status.Status = txtStatus.Text;
                    status.backcolor = cpStatus.Text;

                    if (chkisActive.IsChecked == true)
                        status.isActive = true;
                    else
                        status.isActive = false;
                    if (chkDisable.IsChecked == true)
                        status.isDisable = true;
                    else
                        status.isDisable = false;
                    repo.AddSaleReceiptStatus(status);
                    MessageBox.Show("Successfully Added!");
                    win.Close();
                }
            }
            catch
            {

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (statusId != 0)
            {
               var status = repo.GetSaleReceiptStatus(statusId);
                txtStatus.Text = status.Status;
                chkisActive.IsChecked = status.isActive;
                chkDisable.IsChecked = status.isDisable;
            }
        }
    }
}
