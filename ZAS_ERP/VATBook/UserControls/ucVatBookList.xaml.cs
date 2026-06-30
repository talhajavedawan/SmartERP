using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.VATBook;
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
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;

namespace ZAS_ERP.VATBook.UserControls
{
    /// <summary>
    /// Interaction logic for ucVatBookList.xaml
    /// </summary>
    public partial class ucVatBookList : UserControl
    {
        VATBookRepo vatBookRepo = new VATBookRepo();
        List<ERP_BL.VATBook.VATBook> vatSIBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatPIBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatBillBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatAdminBillBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatIBTBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatIBCTBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatPaymentBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatSaleReceiptBooks = new List<ERP_BL.VATBook.VATBook>();
        string buttonLabel;
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();



        public ucVatBookList()
        {
            InitializeComponent();
        }
        public ucVatBookList(string Label)
        {
            InitializeComponent();
            buttonLabel = Label;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            LoadAllUserReferences();
            //LoadvatBooks();
        }

        private void LoadAllUserReferences()
        {
            switch (buttonLabel)
            {
                case "Active":
                    var activeVatRef = vatBookRepo.GetAllActiveVATBookReferenceNo();
                    activeVatRef = activeVatRef.Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    lookupVatRef.ItemsSource = activeVatRef;
                    break;
                case "InActive":
                    var inActiveVatRef = vatBookRepo.GetAllInActiveVATBookReferenceNo();
                    inActiveVatRef = inActiveVatRef.Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    lookupVatRef.ItemsSource = inActiveVatRef;

                    break;
            }
        }
        private void LoadvatBooks()
        {
            vatBookRepo.FixNullTransactions();
            vatBookRepo = new VATBookRepo();

            switch (buttonLabel)
            {
                case "Active":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view VAT Book Enteries without Company & Departmental Authority") != null)
                    {

                        vatSIBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view VAT Book Enteries without Departmental Authority") != null)
                    {
                        vatSIBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    }
                    else
                    {
                        vatSIBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    }
                    txtHeader.Text = "VAT Book (Active)";
                    break;
                case "InActive":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view VAT Book Enteries without Company & Departmental Authority") != null)
                    {
                        vatSIBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllInActiveWithoutCompDepartmentalCheckByType(TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view VAT Book Enteries without Departmental Authority") != null)
                    {
                        vatSIBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllInActiveWithoutDepartmentalCheckByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    }
                    else
                    {
                        vatSIBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Invoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatBillBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatAdminBillBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Admin_Bill).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBTBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatIBCTBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.InterCompanyBank_Transfer).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPIBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.PurchaseInvoice).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatPaymentBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Payments).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                        vatSaleReceiptBooks = vatBookRepo.GetAllInActiveVATBookByType(SYSTEM_STATIC.currentUser.id, TransactionItemType.Sale_Receipt).Where(x => companyIds.Contains((int)x.companyId)).ToList();
                    }
                    txtHeader.Text = "VAT Book (InActive)";
                    break;
            }
            if (lookupVatRef.SelectedIndex > -1)
            {
                vatSIBooks = vatSIBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatBillBooks = vatBillBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatAdminBillBooks = vatAdminBillBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatIBTBooks = vatIBTBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatIBCTBooks = vatIBCTBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatPIBooks = vatPIBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatPaymentBooks = vatPaymentBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
                vatSaleReceiptBooks = vatSaleReceiptBooks.Where(x => x.VATBookRefNumberRefId == (lookupVatRef.SelectedItem as VATBookRefNumber).Id).ToList();
            }
            grdSIVATList.ItemsSource = vatSIBooks;
            grdBillVATList.ItemsSource = vatBillBooks;
            grdAdminBillVATList.ItemsSource = vatAdminBillBooks;
            grdIBTVATList.ItemsSource = vatIBTBooks;
            grdIBCTVATList.ItemsSource = vatIBCTBooks;
            grdPIVATList.ItemsSource = vatPIBooks;
            grdPaymentVATList.ItemsSource = vatPaymentBooks;
            grdSaleReceiptVATList.ItemsSource = vatSaleReceiptBooks;


            txtSIDebitOC.Text = Math.Round(vatSIBooks.Sum(x => x.debit), 2).ToString();
            txtSICreditOC.Text = Math.Round(vatSIBooks.Sum(x => x.credit), 2).ToString();
            txtSIBalanceOC.Text = Math.Round(vatSIBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtPIDebitOC.Text = Math.Round(vatPIBooks.Sum(x => x.debit), 2).ToString();
            txtPICreditOC.Text = Math.Round(vatPIBooks.Sum(x => x.credit), 2).ToString();
            txtPIBalanceOC.Text = Math.Round(vatPIBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtBillDebitOC.Text = Math.Round(vatBillBooks.Sum(x => x.debit), 2).ToString();
            txtBillCreditOC.Text = Math.Round(vatBillBooks.Sum(x => x.credit), 2).ToString();
            txtBillBalanceOC.Text = Math.Round(vatBillBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtAdminBillDebitOC.Text = Math.Round(vatAdminBillBooks.Sum(x => x.debit), 2).ToString();
            txtAdminBillCreditOC.Text = Math.Round(vatAdminBillBooks.Sum(x => x.credit), 2).ToString();
            txtAdminBillBalanceOC.Text = Math.Round(vatAdminBillBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtIBTDebitOC.Text = Math.Round(vatIBTBooks.Sum(x => x.debit), 2).ToString();
            txtIBTCreditOC.Text = Math.Round(vatIBTBooks.Sum(x => x.credit), 2).ToString();
            txtIBTBalanceOC.Text = Math.Round(vatIBTBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtIBCTDebitOC.Text = Math.Round(vatIBCTBooks.Sum(x => x.debit), 2).ToString();
            txtIBCTCreditOC.Text = Math.Round(vatIBCTBooks.Sum(x => x.credit), 2).ToString();
            txtIBCTBalanceOC.Text = Math.Round(vatIBCTBooks.Sum(x => x.debit - x.credit), 2).ToString();


            txtPaymentDebitOC.Text = Math.Round(vatPaymentBooks.Sum(x => x.debit), 2).ToString();
            txtPaymentCreditOC.Text = Math.Round(vatPaymentBooks.Sum(x => x.credit), 2).ToString();
            txtPaymentBalanceOC.Text = Math.Round(vatPaymentBooks.Sum(x => x.debit - x.credit), 2).ToString();

            txtSaleReceiptDebitOC.Text = Math.Round(vatSaleReceiptBooks.Sum(x => x.debit), 2).ToString();
            txtSaleReceiptCreditOC.Text = Math.Round(vatSaleReceiptBooks.Sum(x => x.credit), 2).ToString();
            txtSaleReceiptBalanceOC.Text = Math.Round(vatSaleReceiptBooks.Sum(x => x.debit - x.credit), 2).ToString();
            txtTotalDebitAmountOC.Text = (
                Convert.ToDouble(txtSIDebitOC.Text) +
                Convert.ToDouble(txtPIDebitOC.Text) +
                Convert.ToDouble(txtBillDebitOC.Text) +
                Convert.ToDouble(txtAdminBillDebitOC.Text) +
                Convert.ToDouble(txtIBTDebitOC.Text) +
                Convert.ToDouble(txtIBCTDebitOC.Text) +
                Convert.ToDouble(txtPaymentDebitOC.Text) +
                Convert.ToDouble(txtSaleReceiptDebitOC.Text)
                ).ToString();
            txtTotalCreditOC.Text = (
               Convert.ToDouble(txtSICreditOC.Text) +
               Convert.ToDouble(txtPICreditOC.Text) +
               Convert.ToDouble(txtBillCreditOC.Text) +
               Convert.ToDouble(txtAdminBillCreditOC.Text) +
               Convert.ToDouble(txtIBTCreditOC.Text) +
               Convert.ToDouble(txtIBCTCreditOC.Text) +
               Convert.ToDouble(txtPaymentCreditOC.Text) +
               Convert.ToDouble(txtSaleReceiptCreditOC.Text)
               ).ToString();   
            txtTotalBalanceOC.Text = (
               Convert.ToDouble(txtSIBalanceOC.Text) +
               Convert.ToDouble(txtPIBalanceOC.Text) +
               Convert.ToDouble(txtBillBalanceOC.Text) +
               Convert.ToDouble(txtAdminBillBalanceOC.Text) +
               Convert.ToDouble(txtIBTBalanceOC.Text) +
               Convert.ToDouble(txtIBCTBalanceOC.Text) +
               Convert.ToDouble(txtPaymentBalanceOC.Text) +
               Convert.ToDouble(txtSaleReceiptBalanceOC.Text)
               ).ToString();
        }

        private void GrdListTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
        public void DoubleClick(ERP_BL.VATBook.VATBook vatBook)
        {
            switch (vatBook.TransactionType)
            {
                case ERP_BL.Enums.TransactionItemType.InterBank_Transfer:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") == null)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                        if (vatBook.interBankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") == null)
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                return;
                            }
                        }
                    }
                    ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                    ucFrmBankTransfer.editFlag = true;
                    ucFrmBankTransfer.bankTransferId = vatBook.interBankTransferId.Value;
                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                    ucFrmBankTransfer.frmBankTranfer.Show();
                    break;
                case ERP_BL.Enums.TransactionItemType.PurchaseInvoice:
                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)vatBook.purchaseInvoiceId);
                    procurmentPanel.Show();


                    break;

                case ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") == null)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                        if (vatBook.interCompanyTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") == null)
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                return;
                            }
                        }
                    }
                    ucBankTransferInterCompany frmCompTransfer = new ucBankTransferInterCompany();
                    frmCompTransfer.editFlag = true;
                    frmCompTransfer.bankTransferId = vatBook.interCompanyId.Value;
                    frmCompTransfer.frmBankTranfer.Content = frmCompTransfer;
                    frmCompTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                    frmCompTransfer.frmBankTranfer.Title = "Inter-Company Bank Transfer";
                    frmCompTransfer.frmBankTranfer.Show();
                    break;

                case ERP_BL.Enums.TransactionItemType.LoansAdvances:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") == null)
                        {
                            DXMessageBox.Show("Permission required to View Loans and Advances!");
                            return;
                        }
                    }

                    switch (vatBook.loansAdvance.loansAdvanceType)
                    {
                        case ERP_BL.Enums.LoansAdvanceType.Admin_Bill:
                            ucFrmLoansAdvances frmLoansAdvancesAdd = new ucFrmLoansAdvances();
                            Window win = new Window();
                            win.WindowState = WindowState.Maximized;
                            win.Title = "Update Loans Advances";
                            frmLoansAdvancesAdd.editFlag = true;
                            frmLoansAdvancesAdd.loansAdvanceId = vatBook.loansAdvance.Id;
                            win.Content = frmLoansAdvancesAdd;
                            win.Show();
                            break;
                        case ERP_BL.Enums.LoansAdvanceType.Vendor_Bill:
                            ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                            Window wind = new Window();
                            wind.WindowState = WindowState.Maximized;
                            wind.Title = "Update Loans Advances";
                            frmBillLoansAdvance.editFlag = true;
                            frmBillLoansAdvance.loansAdvanceId = vatBook.loansAdvance.Id;
                            wind.Content = frmBillLoansAdvance;
                            wind.Show();
                            break;
                    }


                    break;

                case ERP_BL.Enums.TransactionItemType.Admin_Bill:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") == null)
                        {
                            DXMessageBox.Show("Permission required to View Existing Bill!");
                            return;
                        }
                    }
                    ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                    Window frmBill = new Window();
                    frmBill.WindowState = WindowState.Maximized;
                    frmBill.Title = "Update Bills";
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = vatBook.adminBill.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();

                    break;
                case ERP_BL.Enums.TransactionItemType.Bill:
                    Procurementss.frmProcurmentPanel procurmentPanelBill = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, (int)vatBook.vendorBillId);
                    procurmentPanelBill.Show();
                    break;

                case ERP_BL.Enums.TransactionItemType.Payments:
                    if (vatBook.payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Admin_Bills)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Admin Bill!s");
                                return;
                            }
                            if (vatBook.payment.Status.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                        }
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        PaymentRepo paymentRepo = new PaymentRepo();
                        if (vatBook.payment != null)
                        {
                            frmPayments.editFlag = true;
                            frmPayments.groupId = vatBook.payment.transactionGroupId;
                            frmPayments.frmPaymentWindow.Content = frmPayments;
                            frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                            frmPayments.frmPaymentWindow.Title = "Payments";
                            frmPayments.frmPaymentWindow.Show();
                        }
                    }
                    else if (vatBook.payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Purchase_Invoice)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Purchase Invoice");
                                return;
                            }
                            if (vatBook.payment.Status.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                        }
                        ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();
                        frmPIpayment.editFlag = true;
                        frmPIpayment.groupId = vatBook.payment.transactionGroupId;
                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                        frmPIpayment.frmPiPaymentWindow.Show();
                    }
                    else if (vatBook.payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Vendor_Bills)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Vendor Bills!");
                                return;
                            }
                            if (vatBook.payment.Status.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                        }
                        ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();
                        frmBillPayments.editFlag = true;
                        frmBillPayments.groupId = vatBook.payment.transactionGroupId;
                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                        frmBillPayments.frmBillPaymentWindow.Show();

                    }
                    else if (vatBook.payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Loans_Advances)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Loans and Advances!");
                                return;
                            }
                            if (vatBook.payment.Status.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                        }
                        ucFrmLoansAdvancePaymentAdd frmLAPayments = new ucFrmLoansAdvancePaymentAdd();
                        frmLAPayments.editFlag = true;
                        frmLAPayments.groupId = vatBook.payment.transactionGroupId;
                        frmLAPayments.frmPiPaymentWindow.Content = frmLAPayments;
                        frmLAPayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmLAPayments.frmPiPaymentWindow.Title = "Payments";
                        frmLAPayments.frmPiPaymentWindow.Show();

                    }
                    else if (vatBook.payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Target_Reward)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Target Rewards!");
                                return;
                            }
                            if (vatBook.payment.Status.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                        }
                        ucFrmTargetRewardPayment frmTRPayments = new ucFrmTargetRewardPayment();
                        frmTRPayments.editFlag = true;
                        frmTRPayments.groupId = vatBook.payment.transactionGroupId;
                        frmTRPayments.frmPiPaymentWindow.Content = frmTRPayments;
                        frmTRPayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmTRPayments.frmPiPaymentWindow.Title = "Payments";
                        frmTRPayments.frmPiPaymentWindow.Show();

                    }

                    break;

                case ERP_BL.Enums.TransactionItemType.Sale_Receipt:
                    var saleReceipt = vatBook.salesReceipt;
                    if (saleReceipt != null)
                    {
                        if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Direct_Receipt)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                            {
                                ucFrmDirectSaleReceipt frmReceipt = new ucFrmDirectSaleReceipt();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                Window frmPiPaymentWindow = new Window();
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                    {
                                        frmReceipt.editFlag = true;
                                        frmReceipt.groupId = saleReceipt.transactionGroupId;
                                        frmPiPaymentWindow.Content = frmReceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Direct Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Direct Receipts!");
                            }
                        }
                        else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Loans_Advances)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                            {
                                ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                Window frmPiPaymentWindow = new Window();
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                    {
                                        frmLAreceipt.editFlag = true;
                                        frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                        frmPiPaymentWindow.Content = frmLAreceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Sale Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                            }
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") == null)
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                                    return;
                                }
                                if (saleReceipt.saleReceiptStatus.isActive == false)
                                {
                                    if (saleReceipt.saleReceiptStatus.isActive == false && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null))
                                    {
                                        DXMessageBox.Show("Permission required to View Closed Receipts!");
                                        return;
                                    }
                                }
                            }

                            ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                            updateSaleReceiptObj.saveEditFlag = 1;

                            updateSaleReceiptObj.dateEditcreationDate.EditValue = saleReceipt.CreationDate;
                            updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                            updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                            updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                            updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                            updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                            updateSaleReceiptObj.receiptId = saleReceipt.Id;
                            updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;

                            updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                            updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                            updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                            updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                            updateSaleReceiptObj.enterReceiptWindowFlag = true;


                            if (saleReceipt.CreditedDate != null)
                                updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                            if (saleReceipt.DepositedDate != null)
                                updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                            if (saleReceipt.InstrumentDate != null)
                                updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                            if (saleReceipt.InstrumentNo != null)
                                updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                            if (saleReceipt.principal != null)
                            {
                                PrincipalRepo prinRepo = new PrincipalRepo();
                                var principal = prinRepo.get(saleReceipt.principal.Id);
                            }

                            updateSaleReceiptObj.enter_receipt_win.Show();
                        }
                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.Sale_Invoice:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from VAT Book") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Invoice") == null)
                        {
                            DXMessageBox.Show("Permission required to View Sale Invoice!");
                            return;
                        }
                    }
                    //if (grdvatBook.SelectedItem != null)
                    //{
                    //    var book = grdvatBook.SelectedItem as ERP_BL.VATBook.VATBook;
                    //    Procurementss.frmProcurmentPanel procurmentPanel1 = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)book.saleInvoiceId);

                    //    if (book.saleInvoice.saleInvoicetype == InquiryType.DistributionBiz_CustomerCredit)
                    //    {
                    //        procurmentPanel1.btnCreateReciept.Visibility = Visibility.Collapsed;
                    //    }
                    //    procurmentPanel1.Show();
                    //}
                    break;

            }
        }
        private void GrdListTransactions_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //try
            //{
            //    var row = grdvatBook.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.VATBook.VATBook;

            //    GridControl view = (GridControl)sender;
            //    if (e.Column.FieldName == "vatBookRef.VATBookReferenceNo" & e.IsGetData)
            //    {
            //        var reference= vatBookRepo.GetVATBookReferenceNo((int)(grdvatBook.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.VATBook.VATBook).VATBookRefNumberRefId);
            //        e.Value = reference.VATBookReferenceNo;
            //    }
            //    if (e.Column.FieldName == "balanceAmount" & e.IsGetData)
            //    { 
            //        double total = 0;
            //        int rHandle = view.GetRowHandleByListIndex(e.ListSourceRowIndex);
            //        for (int i = -1; i <= rHandle - 1; i++)
            //        {
            //            total += Convert.ToDouble(view.GetCellValue(i + 1, "total"));
            //        }
            //        e.Value = total;
            //    }
            //    if (e.Column.FieldName == "Vendor" & e.IsGetData)
            //    {

            //        switch (row.TransactionType)
            //        {
            //            case TransactionItemType.Sale_Invoice:
            //                {
            //                    e.Value = row.saleInvoice.vendors[0].company.CompanyName;
            //                    break;
            //                }
            //            case TransactionItemType.Sale_Receipt:
            //                {
            //                    e.Value = row.salesReceipt.saleInvoice.vendors[0].company.CompanyName;
            //                    break;
            //                }
            //            case TransactionItemType.Purchase_Invoice:
            //                {
            //                    e.Value = row.purchaseInvoice.vendors[0].company.CompanyName;


            //                    break;
            //                }
            //            case TransactionItemType.Payments:
            //                {
            //                    e.Value = row.payment.vendor.company.CompanyName;

            //                    break;
            //                }
            //            case TransactionItemType.Admin_Bill:
            //                {
            //                    e.Value = row.adminBill.vendor.company.CompanyName;
            //                    break;
            //                }
            //            case TransactionItemType.Bill:
            //                {
            //                    e.Value = row.vendorBill.vendor.company.CompanyName;

            //                    break;
            //                }
            //            case TransactionItemType.LoansAdvances:
            //                {
            //                    e.Value = row.loansAdvance.vendor.company.CompanyName;

            //                    break;
            //                }
            //            case TransactionItemType.InterBank_Transfer:
            //                {
            //                    e.Value = row.interBankTransfer.vendor.company.CompanyName;

            //                    break;
            //                } 
            //            case TransactionItemType.InterCompanyBank_Transfer:
            //                {
            //                    //e.Value = row.interCompanyTransfer.vendor.company.CompanyName;

            //                    break;
            //                }
                       
            //            default: break;
            //        }
            //    }
            //    if (e.Column.FieldName == "Customer" & e.IsGetData)
            //    {
            //        switch (row.TransactionType)
            //        {
            //            case TransactionItemType.Sale_Invoice:
            //                {
            //                    e.Value = row.saleInvoice.customerCompany.company.CompanyName;
            //                    break;
            //                }
            //            case TransactionItemType.Sale_Receipt:
            //                {
            //                    e.Value = row.salesReceipt.saleInvoice.customerCompany.company.CompanyName;
            //                    break;
            //                }
            //            case TransactionItemType.Purchase_Invoice:
            //                {
            //                    e.Value = row.purchaseInvoice.customerCompany.company.CompanyName;


            //                    break;
            //                }
            //            case TransactionItemType.Payments:
            //                {
            //                    string customer = "";
            //                    if(row.payment.purchaseInvoice!=null)
            //                    {
            //                        e.Value = row.payment.purchaseInvoice.customerCompany.company.CompanyName;

            //                    }
            //                    else
            //                         if (row.payment.adminBill != null)
            //                    {
            //                        e.Value = "";

            //                    }
            //                    else
            //                         if (row.payment.Bill != null)
            //                    {
            //                        e.Value = row.payment.Bill.customerCompany.company.CompanyName;

            //                    }
            //                    else
            //                         if (row.payment.loansAdvance != null)
            //                    {
            //                        //e.Value = row.payment.loansAdvance.customerCompany.company.CompanyName;

            //                    }


            //                    break;
            //                }
            //            case TransactionItemType.Admin_Bill:
            //                {
            //                    e.Value = "";
            //                    break;
            //                }
            //            case TransactionItemType.Bill:
            //                {
            //                    e.Value = row.vendorBill.customerCompany.company.CompanyName;

            //                    break;
            //                }
            //            case TransactionItemType.LoansAdvances:
            //                {
            //                    e.Value = "";

            //                    break;
            //                }
            //            case TransactionItemType.InterBank_Transfer:
            //                {
            //                    e.Value = "";

            //                    break;
            //                }

            //            default: break;
            //        }

            //    }
            //}
            //catch (StackOverflowException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }
        private void btnLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadvatBooks();
        }

        private void grdIBCTVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdIBCTVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdAdminBillVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdAdminBillVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdPaymentVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdPaymentVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdSaleReceiptVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdSaleReceiptVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdIBTVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdIBTVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdPIVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdPIVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdBillVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdBillVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }

        private void grdSIVATList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DoubleClick(grdSIVATList.SelectedItem as ERP_BL.VATBook.VATBook);
        }
    }
}
