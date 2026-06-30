using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss.SaleInvoicess
{
    /// <summary>
    /// Interaction logic for winSICustomerCredits.xaml
    /// </summary>
    public partial class winSICustomerCredits : DXWindow
    {
        public int saleInvoiceId = 0;
        public winSICustomerCredits()
        {
            InitializeComponent();
        }
        
        public winSICustomerCredits(int _saleInvoiceId)
        {
            InitializeComponent();
            saleInvoiceId = _saleInvoiceId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SaleInvoiceRepo repo = new SaleInvoiceRepo(); 
            if(saleInvoiceId!=0)
            {
                grdCustomerCredits.ItemsSource = repo.GetAllSaleInvoiceCustomerCredits(saleInvoiceId);
            }
            else
            {
                grdCustomerCredits.ItemsSource = repo.GetAllCustomerCredits();
            }
        }

        private void grdCustomerCredits_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCustomerCredits.SelectedItem != null && (grdCustomerCredits.SelectedItem as ERP_BL.Databases.CustomerCredit).SaleInvoiceId!=null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)(grdCustomerCredits.SelectedItem as ERP_BL.Databases.CustomerCredit).SaleInvoiceId);
                procurmentPanel.Show();
            }
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var cusCredit = grdCustomerCredits.GetRowByListIndex(e.ListSourceRowIndex) as CustomerCredit;
            if (e.Column.FieldName == "EdDaysLeft")
            {
                if (cusCredit.SaleInvoice.ExpectedDiscountDate != null)
                {
                    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime date = cusCredit.SaleInvoice.ExpectedDiscountDate.Value;
                    DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    var timeSpan = targetDate.Subtract(currDate);
                    e.Value = timeSpan.Days.ToString();
                }

            }

            
            var saleInvoice = cusCredit.SaleInvoice;

            if (cusCredit != null) 
            {
                if (e.Column.FieldName == "collectedCustomerCredit")
                {
                    if (saleInvoice != null && saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count > 0)
                    {
                        var collectedAmount = saleInvoice.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        e.Value = collectedAmount;
                    }
                    else
                    {
                        e.Value = 0;
                    }
                }
                if (e.Column.FieldName == "balanceCustomerCredit")
                {
                    if (saleInvoice != null && (saleInvoice.salesReceipts == null || saleInvoice.salesReceipts.Count == 0))
                    {
                        //var collectedAmount = saleInvoice.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        e.Value = cusCredit.creditAmount;
                    }
                    else if (saleInvoice != null && saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count > 0)
                    {
                        var collectedAmount = saleInvoice.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        e.Value = cusCredit.creditAmount - collectedAmount;
                    }
                    else
                    {
                        e.Value = 0;
                    }
                }
            }
        }
    }
}
