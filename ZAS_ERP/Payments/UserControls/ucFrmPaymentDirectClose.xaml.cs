using ERP_BL.Payments;
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
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmPaymentDirectClose.xaml
    /// </summary>
    public partial class ucFrmPaymentDirectClose : UserControl
    {
        PaymentRepo paymentsRepo = new PaymentRepo();
        ucPaymentList paymentStatus = new ucPaymentList();
        public UcListWindow directCloseWin = new UcListWindow();
        public bool frmFlag = false;
        public string type;


        public ucFrmPaymentDirectClose()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<cmbitem> billStatusLst = new List<cmbitem>();
                var allBillsStatus = paymentsRepo.GetAllClosePaymentStatus();
                if (allBillsStatus != null)
                {
                    Parallel.ForEach(allBillsStatus, delegate (PaymentStatus status) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                    {
                        billStatusLst.Add
                        (new cmbitem()
                        {
                            name = status.Status,
                            id = status.Id,
                            bcolor = status.backcolor,
                            fcolor = "#FF000000"
                        });


                    });
                    cmbBillStatus.ItemsSource = billStatusLst;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Sale Receipt Status 
                if ((cmbBillStatus.SelectedItem as cmbitem) != null)
                {
                    var status = paymentsRepo.GetPaymentStatus((cmbBillStatus.SelectedItem as cmbitem).id);
                    if (status != null)
                    {
                        //receiptStatus.statusChanged = status;
                        if (frmFlag == true)
                        {
                            switch (type)
                            {
                                case "AdminBill":
                                    ucFrmPayments frmPayments = new ucFrmPayments(status);
                                    break;
                                case "Bills":
                                    ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd(status);
                                    break;
                                case "PurchaseInvoice":
                                    ucFrmPInvoicePaymentAdd frmPIpaymentAdd = new ucFrmPInvoicePaymentAdd(status);
                                    break;
                                case "LoansAdvance":
                                    ucFrmLoansAdvancePaymentAdd frmLApaymentAdd = new ucFrmLoansAdvancePaymentAdd(status);
                                    break;
                                case "TargetReward":
                                    ucFrmTargetRewardPayment FrmTargetReward = new ucFrmTargetRewardPayment(status);
                                    break;
                            }
                            
                        }
                        else
                        {
                            ucPaymentList paymentList = new ucPaymentList(status);

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
