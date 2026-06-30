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
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmDirectClose.xaml
    /// </summary>
    public partial class ucFrmDirectClose : UserControl
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();
        ucSaleReceiptList receiptStatus = new ucSaleReceiptList();
        ucFrmSaleReceipt frmSaleReceipt = new ucFrmSaleReceipt();
        public UcListWindow directCloseWin = new UcListWindow();

        public bool frmFlag = false;
        public string type;

        public ucFrmDirectClose()
        {
            InitializeComponent();

            try
            {
                List<cmbitem> receiptStatusLst = new List<cmbitem>();
                var allReceiptStatus = repo.GetAllCloseSaleReceiptStatus();
                if (allReceiptStatus != null)
                {
                    Parallel.ForEach(allReceiptStatus, delegate (SalesReceiptStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        receiptStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbSaleReceiptStatus.ItemsSource = receiptStatusLst;
                }
            }
            catch
            {

            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbSaleReceiptStatus.SelectedItem as cmbitem) != null)
                {
                    var status = repo.GetSaleReceiptStatus((cmbSaleReceiptStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        if(frmFlag == true)
                        {
                            switch (type)
                            {
                                case "CustomerCredits":
                                    ucFrmCustomerCreditReceipt customCreditStatus = new ucFrmCustomerCreditReceipt(status);
                                    break;
                                case "SaleInvoice":
                                    ucFrmSaleReceipt receiptStatus = new ucFrmSaleReceipt(status);
                                    break;
                                case "LoansAdvances":
                                    ucFrmLoansAdvanceSaleReceiptAdd statusChanged = new ucFrmLoansAdvanceSaleReceiptAdd(status);
                                    break;
                                case "Direct":
                                    ucFrmDirectSaleReceipt directReceipt = new ucFrmDirectSaleReceipt(status);
                                    break;
                            }
                            
                        }
                        else
                        {
                            //receiptStatus.statusChanged = status;
                            ucSaleReceiptList receiptStatus = new ucSaleReceiptList(status);
                        }
                       
                        directCloseWin.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Select a status before saving!");
                }
            }
            catch
            {

            }
          

        }
    }
}
