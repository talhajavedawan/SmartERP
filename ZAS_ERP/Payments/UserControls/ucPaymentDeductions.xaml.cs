using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Payments;
using ERP_BL.Tax;
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

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucPaymentDeductions.xaml
    /// </summary>
    public partial class ucPaymentDeductions : DXWindow
    {


        public double totalDeduction =0, totalVAT= 0;
        public List<PaymentDeduction> paymentDeductions = new List<PaymentDeduction>();
        public List<PaymentDeduction> finalDeductions = new List<PaymentDeduction>();

        public List<PaymentTax> paymentTaxes = new List<PaymentTax>();
        public List<PaymentTax> finalTaxes = new List<PaymentTax>();
        int paymentId=0 ;
        public bool? IsAdjusted = null;
        PaymentRepo paymentRepo = new PaymentRepo();
        public ucPaymentDeductions()
        {
            InitializeComponent();
        }
        public ucPaymentDeductions(int _paymentId)
        {
            InitializeComponent();
            paymentId = _paymentId;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdDeductions.ItemsSource = paymentDeductions;
            SalesReceiptRepo deductionsRepo = new SalesReceiptRepo();
            var deductions=deductionsRepo.GetAllDeductions();
            lookupDeductionsGrid.ItemsSource = deductions;

            grdVAT.ItemsSource = paymentTaxes;
            

            LoadonPaymentDeductions();

            //grdDeductions.ItemsSource = deductions;
        }
        public void LoadonPaymentDeductions()
        {
            if (paymentId != 0)
            {
               var payment= paymentRepo.GetPayment(paymentId);

                if(payment.IsAdjusted != null)
                {
                    if (payment.IsAdjusted == true)
                        chkIsAdjusted.IsChecked = true;
                    else
                        chkNonAdjusted.IsChecked = true;
                }
                foreach (var deduction in payment.paymentDeductions)
                {
                    paymentDeductions.Add(new PaymentDeduction()
                    {
                        Id = deduction.Id,
                        deduction_id =deduction.deduction_id,
                        Amount= deduction.Amount,
                        deduction= deduction.deduction
                    });
                }
                grdDeductions.ItemsSource = paymentDeductions;

                foreach (var tax in payment.PaymentTaxes)
                {
                    paymentTaxes.Add(new PaymentTax()
                    {
                        Id = tax.Id,
                        taxNameId = tax.taxNameId,
                        Amount = tax.Amount,
                        taxName = tax.taxName
                    });
                }
                grdVAT.ItemsSource = paymentTaxes;
            }
          
        } 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           

            this.Close();
        }

        private void btnAddDeduction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //View.AddNewRow();
        }

        private void TblViewVAT_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "IsAdjusted":
                    
                    break;
                case "NonAdjusted":
                    //var value1 = Convert.ToBoolean(grdVAT.GetCellValue(e.RowHandle, e.Column));
                    //if (value1 == true)
                    //{
                    //    grdVAT.SetCellValue(e.RowHandle, e.Column, 0);
                    //    TaxRepo taxRepo = new TaxRepo();
                    //    var taxes = taxRepo.getAllUnAdjustedTaxes();
                    //    lookupVAT.ItemsSource = taxes;

                    //}
                    break;
                
            }
        }

        private void ChkNonAdjusted_Checked(object sender, RoutedEventArgs e)
        {
            //grdVAT.SetCellValue(tblViewVAT.FocusedRowHandle, grdVAT.Columns["IsAdjusted"], false);
            //var value = grdVAT.GetCellValue(tblViewVAT.FocusedRowHandle, grdVAT.Columns["IsAdjusted"]);
            chkIsAdjusted.IsChecked = false;
            TaxRepo taxRepo = new TaxRepo();
            var taxes = taxRepo.getAllUnAdjustedTaxes();
            lookupVAT.ItemsSource = taxes;
            IsAdjusted = false;
        }

        private void ChkNonAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ChkIsAdjusted_Checked(object sender, RoutedEventArgs e)
        {
            //var value = Convert.ToBoolean(grdVAT.GetCellValue(tblViewVAT.FocusedRowHandle, grdVAT.Columns["IsAdjusted"]));
            //if (value == true)
            //{
            //    grdVAT.SetCellValue(tblViewVAT.FocusedRowHandle, grdVAT.Columns["NonAdjusted"], false);

            //var value = grdVAT.GetCellValue(tblViewVAT.FocusedRowHandle, grdVAT.Columns["IsAdjusted"]);
            chkNonAdjusted.IsChecked = false;
                TaxRepo taxRepo = new TaxRepo();
                var taxes = taxRepo.getAllAdjustedTaxes();
                lookupVAT.ItemsSource = taxes;
            IsAdjusted = true;

            //}
        }

        private void ChkIsAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {

        }

       

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var deduction in grdDeductions.ItemsSource as List<PaymentDeduction>)
            {
                finalDeductions.Add(new PaymentDeduction()
                {
                    Id = deduction.Id,
                    deduction_id = deduction.deduction.Id,
                    Amount = deduction.Amount
                });
            }

            totalDeduction = finalDeductions.Sum(x => x.Amount);

            foreach (var tax in grdVAT.ItemsSource as List<PaymentTax>)
            {
                finalTaxes.Add(new PaymentTax()
                {
                    Id = tax.Id,
                    taxNameId = tax.taxName.Id,
                    Amount = tax.Amount
                });
            }

            totalVAT = finalTaxes.Sum(x => x.Amount);
        }
    }
}
