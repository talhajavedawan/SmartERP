using ERP_BL.Databases;
using ERP_BL.Procurements.InterBankTransfers;
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

namespace ZAS_ERP.Bankings.InterCompanyBankTransfer.Windows
{
    /// <summary>
    /// Interaction logic for winICBTbankCharges.xaml
    /// </summary>
    public partial class winICBTbankCharges : Window
    {
        public double totalBankCharges = 0, totalVAT = 0;
        public bool BankChargesFrom = true;
        public List<InterCompBankTransferCharges> ICBTbankCharges = new List<InterCompBankTransferCharges>();
        public List<InterCompBankTransferCharges> finalBankCharges = new List<InterCompBankTransferCharges>();

        public List<InterCompBankTransferVAT> ICBTvat = new List<InterCompBankTransferVAT>();
        public List<InterCompBankTransferVAT> finalVATs = new List<InterCompBankTransferVAT>();
        int IBTid = 0;
        public bool? IsAdjustedFrom = null;
        public bool? IsAdjustedTo = null;
        InterCompanyBankTransferRepo ICBTrepo = new InterCompanyBankTransferRepo();
        public winICBTbankCharges()
        {
            InitializeComponent();
        }

        public winICBTbankCharges(int _ibtId)
        {
            InitializeComponent();
            IBTid = _ibtId;
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdDeductions.ItemsSource = ICBTbankCharges;
            SalesReceiptRepo deductionsRepo = new SalesReceiptRepo();
            var deductions = deductionsRepo.GetAllDeductions();
            lookupDeductionsGrid.ItemsSource = deductions;
            grdVAT.ItemsSource = ICBTvat;
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
                if (IBTid != 0)
                {
                    var ICBT = ICBTrepo.GetInterCompanyBankTransfer(IBTid);

                    if (BankChargesFrom == true)
                    {
                        if (ICBT.IsAdjustedVATfrom != null)
                        {
                            if (ICBT.IsAdjustedVATfrom == true)
                                chkIsAdjusted.IsChecked = true;
                            else
                                chkNonAdjusted.IsChecked = true;
                        }
                        foreach (var deduction in ICBT.bankChargesFrom)
                        {
                            ICBTbankCharges.Add(new InterCompBankTransferCharges()
                            {
                                Id = deduction.Id,
                                deduction_Id = deduction.deduction_Id,
                                Amount = deduction.Amount,
                                deduction = deduction.deduction
                            });
                        }

                        foreach (var tax in ICBT.InterCompBankTransferVATFrom)
                        {
                            ICBTvat.Add(new InterCompBankTransferVAT()
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
                        if (ICBT.IsAdjustedVATto != null)
                        {
                            if (ICBT.IsAdjustedVATto == true)
                                chkIsAdjusted.IsChecked = true;
                            else
                                chkNonAdjusted.IsChecked = true;
                        }
                        foreach (var deduction in ICBT.bankChargesTo)
                        {

                            ICBTbankCharges.Add(new InterCompBankTransferCharges()
                            {
                                Id = deduction.Id,
                                deduction_Id = deduction.deduction_Id,
                                Amount = deduction.Amount,
                                deduction = deduction.deduction
                            });
                        }

                        foreach (var tax in ICBT.InterCompBankTransferVATto)
                        {
                            ICBTvat.Add(new InterCompBankTransferVAT()
                            {
                                Id = tax.Id,
                                taxNameId = tax.taxNameId,
                                Amount = tax.Amount,
                                taxName = tax.taxName
                            });
                        }
                    }
                    grdDeductions.ItemsSource = ICBTbankCharges;
                    grdVAT.ItemsSource = ICBTvat;
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
            if (BankChargesFrom == true)
                IsAdjustedFrom = false;
            else
                IsAdjustedTo = false;
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
            if (BankChargesFrom == true)
                IsAdjustedFrom = true;
            else
                IsAdjustedTo = true;
            //}
        }

        private void ChkIsAdjusted_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var deduction in grdDeductions.ItemsSource as List<InterCompBankTransferCharges>)
            {
                finalBankCharges.Add(new InterCompBankTransferCharges()
                {
                    Id = deduction.Id,
                    deduction_Id = deduction.deduction.Id,
                    Amount = deduction.Amount
                });
            }

            totalBankCharges = finalBankCharges.Sum(x => x.Amount);

            foreach (var tax in grdVAT.ItemsSource as List<InterCompBankTransferVAT>)
            {
                finalVATs.Add(new InterCompBankTransferVAT()
                {
                    Id = tax.Id,
                    taxNameId = tax.taxName.Id,
                    Amount = tax.Amount
                });
            }

            totalVAT = finalVATs.Sum(x => x.Amount);
        }
    }
}
