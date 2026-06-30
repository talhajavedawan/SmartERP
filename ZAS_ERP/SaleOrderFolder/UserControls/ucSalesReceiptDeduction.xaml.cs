using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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
using System.Windows.Shapes;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucSalesReceiptDeduction.xaml
    /// </summary>
    public partial class ucSalesReceiptDeduction : DXWindow
    {
        public double totalDeduction = 0, totalVAT = 0;
        public bool BankCharges = true;
        public List<ReceiptDeduction> receiptDeductions = new List<ReceiptDeduction>();
        public List<ReceiptDeduction> finalDeductions = new List<ReceiptDeduction>();

        public List<ReceiptTax> receiptTaxes = new List<ReceiptTax>();
        public List<ReceiptTax> finalTaxes = new List<ReceiptTax>();
        int receiptId = 0;
        public bool? IsDedAdjusted = null;
        public bool? IsBankAdjusted = null;
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        public ucSalesReceiptDeduction()
        {
            InitializeComponent();
        }
        public ucSalesReceiptDeduction(int _receiptId)
        {
            InitializeComponent();
            receiptId = _receiptId;
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdDeductions.ItemsSource = receiptDeductions;
            SalesReceiptRepo deductionsRepo = new SalesReceiptRepo();
            var deductions = deductionsRepo.GetAllDeductions();
            lookupDeductionsGrid.ItemsSource = deductions;
            grdVAT.ItemsSource = receiptTaxes;
            LoadonReceiptDeductions();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //foreach (var deduction in grdDeductions.ItemsSource as List<ReceiptDeduction>)
            //{
            //    finalDeductions.Add(new ReceiptDeduction()
            //    {
            //        Id = deduction.Id,
            //        deduction_Id = deduction.deduction.Id,
            //        Amount = deduction.Amount
            //    });
            //}

            //totalDeduction = finalDeductions.Sum(x => x.Amount);
            this.Close();
        }

        private void btnAddDeduction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //View.AddNewRow();
        }
        public void LoadonReceiptDeductions()
        {
            try
            {
                if (receiptId != 0)
                {
                    var receipt = receiptRepo.GetSalesReceipt(receiptId);

                    if(BankCharges == true)
                    {
                        if (receipt.IsAdjustedBankVAT != null)
                        {
                            if (receipt.IsAdjustedBankVAT == true)
                                chkIsAdjusted.IsChecked = true;
                            else
                                chkNonAdjusted.IsChecked = true;
                        }
                        grdDeductions.Columns["deduction"].Header = "Bank Charges";
                        lblHeading.Text = "Attach Bank Charges";
                        foreach (var deduction in receipt.bankCharges)
                        {
                            receiptDeductions.Add(new ReceiptDeduction()
                            {
                                Id = deduction.Id,
                                deduction_Id = deduction.deduction_Id,
                                Amount = deduction.Amount,
                                deduction = deduction.deduction
                            });
                        }

                        foreach (var tax in receipt.ReceiptBankTaxes)
                        {
                            receiptTaxes.Add(new ReceiptTax()
                            {
                                Id = tax.Id,
                                taxNameId = tax.taxNameId,
                                Amount = tax.Amount,
                                taxName = tax.taxName
                            });
                        }
                    }
                    else
                    {
                        if (receipt.IsAdjustedDedVAT != null)
                        {
                            if (receipt.IsAdjustedDedVAT == true)
                                chkIsAdjusted.IsChecked = true;
                            else
                                chkNonAdjusted.IsChecked = true;
                        }
                        foreach (var deduction in receipt.receiptDeductions)
                        {
                            grdDeductions.Columns["deduction"].Header = "Deductions";
                            lblHeading.Text = "Attach Sales Receipt Deductions";
                            receiptDeductions.Add(new ReceiptDeduction()
                            {
                                Id = deduction.Id,
                                deduction_Id = deduction.deduction_Id,
                                Amount = deduction.Amount,
                                deduction = deduction.deduction
                            });
                        }

                        foreach (var tax in receipt.ReceiptDeductionTaxes)
                        {
                            receiptTaxes.Add(new ReceiptTax()
                            {
                                Id = tax.Id,
                                taxNameId = tax.taxNameId,
                                Amount = tax.Amount,
                                taxName = tax.taxName
                            });
                        }
                    }
                    grdDeductions.ItemsSource = receiptDeductions;
                    grdVAT.ItemsSource = receiptTaxes;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

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
            if (BankCharges == true)
                IsBankAdjusted = false;
            else
                IsDedAdjusted = false;
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
            if (BankCharges == true)
                IsBankAdjusted = true;
            else
                IsDedAdjusted = true;
            //}
        }

        private void ChkIsAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var deduction in grdDeductions.ItemsSource as List<ReceiptDeduction>)
            {
                finalDeductions.Add(new ReceiptDeduction()
                {
                    Id = deduction.Id,
                    deduction_Id = deduction.deduction.Id,
                    Amount = deduction.Amount
                });
            }

            totalDeduction = finalDeductions.Sum(x => x.Amount);

            foreach (var tax in grdVAT.ItemsSource as List<ReceiptTax>)
            {
                finalTaxes.Add(new ReceiptTax()
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
