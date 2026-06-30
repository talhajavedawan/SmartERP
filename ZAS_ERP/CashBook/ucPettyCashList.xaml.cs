using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.CashBook;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.CashBook
{
    /// <summary>
    /// Interaction logic for ucPettyCashList.xaml
    /// </summary>
    public partial class ucPettyCashList : UserControl
    {
        PettyCashRepo pettyCashRepo = new PettyCashRepo();
        List<PettyCash> pettyCashes = new List<PettyCash>();
        string buttonLabel;
        public ucPettyCashList()
        {
            InitializeComponent();
        }

        public ucPettyCashList(string Label)
        {
            InitializeComponent();
            buttonLabel = Label;                
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            LoadPettyCashes();
        }

        private void LoadPettyCashes()
        {
            pettyCashRepo.FixNullTransactions();
            pettyCashRepo = new PettyCashRepo();
            //pettyCashes = pettyCashRepo.GetAllPettyCash(SYSTEM_STATIC.currentUser.id);
            //txtHeader.Text = "Petty Cash (Active)";
            //grdPettyCash.ItemsSource = pettyCashes;
            switch (buttonLabel)
            {
                case "Active":
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Petty Cash Enteries without Company & Departmental Authority") != null)
                        pettyCashes = pettyCashRepo.GetAllActiveWithoutCompDepartmentalCheck();
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Petty Cash Enteries without Departmental Authority") != null)
                        pettyCashes = pettyCashRepo.GetAllActiveWithoutDepartmentalCheck(SYSTEM_STATIC.currentUser.id);
                    else
                        pettyCashes = pettyCashRepo.GetAllActivePettyCash(SYSTEM_STATIC.currentUser.id);

                    txtHeader.Text = "Petty Cash (Active)";
                    grdPettyCash.ItemsSource = pettyCashes;
                    break;
                case "InActive":

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Petty Cash Enteries without Company & Departmental Authority") != null)
                        pettyCashes = pettyCashRepo.GetAllInActiveWithoutCompDepartmentalCheck();
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Petty Cash Enteries without Departmental Authority") != null)
                        pettyCashes = pettyCashRepo.GetAllInActiveWithoutDepartmentalCheck(SYSTEM_STATIC.currentUser.id);
                    else
                        pettyCashes = pettyCashRepo.GetAllInActivePettyCash(SYSTEM_STATIC.currentUser.id);

                    txtHeader.Text = "Petty Cash (InActive)";
                    grdPettyCash.ItemsSource = pettyCashes;
                    break;
            }
        }

        private void GrdListTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var pettyCash = grdPettyCash.SelectedItem as PettyCash;

            switch (pettyCash.TransactionType)
            {
                case ERP_BL.Enums.TransactionItemType.Bill:
                    if (pettyCash.billId != null)
                    {

                        Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, pettyCash.billId.Value);
                        procurmentPanel.Show();

                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.InterBank_Transfer:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") == null)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                        if (pettyCash.interBankTransfer.interBankTransStatus.isActive == false)
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
                    ucFrmBankTransfer.bankTransferId = pettyCash.interBankTransferId.Value;
                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                    ucFrmBankTransfer.frmBankTranfer.Show();
                    break;

                case ERP_BL.Enums.TransactionItemType.InterCompanyBank_Transfer:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") == null)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                        if (pettyCash.InterCompanyTransfer.interBankTransStatus.isActive == false)
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
                    frmCompTransfer.bankTransferId = pettyCash.InterCompanyId.Value;
                    frmCompTransfer.frmBankTranfer.Content = frmCompTransfer;
                    frmCompTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                    frmCompTransfer.frmBankTranfer.Title = "Inter-Company Bank Transfer";
                    frmCompTransfer.frmBankTranfer.Show();
                    break;

                case ERP_BL.Enums.TransactionItemType.LoansAdvances:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") == null)
                        {
                            DXMessageBox.Show("Permission required to View Loans and Advances!");
                            return;
                        }
                    }

                    switch (pettyCash.loansAdvance.loansAdvanceType)
                    {
                        case ERP_BL.Enums.LoansAdvanceType.Admin_Bill:
                            ucFrmLoansAdvances frmLoansAdvancesAdd = new ucFrmLoansAdvances();
                            Window win = new Window();
                            win.WindowState = WindowState.Maximized;
                            win.Title = "Update Loans Advances";
                            frmLoansAdvancesAdd.editFlag = true;
                            frmLoansAdvancesAdd.loansAdvanceId = pettyCash.loansAdvance.Id;
                            win.Content = frmLoansAdvancesAdd;
                            win.Show();
                            break;
                        case ERP_BL.Enums.LoansAdvanceType.Vendor_Bill:
                            ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                            Window wind = new Window();
                            wind.WindowState = WindowState.Maximized;
                            wind.Title = "Update Loans Advances";
                            frmBillLoansAdvance.editFlag = true;
                            frmBillLoansAdvance.loansAdvanceId = pettyCash.loansAdvance.Id;
                            wind.Content = frmBillLoansAdvance;
                            wind.Show();
                            break;
                    }
                    

                    break;

                case ERP_BL.Enums.TransactionItemType.Admin_Bill:
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
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
                    frmBillAdd.groupId = pettyCash.adminBill.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();    
                    
                    break;

                case ERP_BL.Enums.TransactionItemType.Payments:
                    if (pettyCash.Payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Admin_Bills)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Admin Bill!s");
                                return;
                            }
                            if (pettyCash.Payment.Status.isActive == false)
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
                        if (pettyCash.Payment != null)
                        {
                            frmPayments.editFlag = true;
                            frmPayments.groupId = pettyCash.Payment.transactionGroupId;
                            frmPayments.frmPaymentWindow.Content = frmPayments;
                            frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                            frmPayments.frmPaymentWindow.Title = "Payments";
                            frmPayments.frmPaymentWindow.Show();
                        }
                    }
                    else if (pettyCash.Payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Purchase_Invoice)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Purchase Invoice");
                                return;
                            }
                            if (pettyCash.Payment.Status.isActive == false)
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
                        frmPIpayment.groupId = pettyCash.Payment.transactionGroupId;
                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                        frmPIpayment.frmPiPaymentWindow.Show();
                    }
                    else if (pettyCash.Payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Vendor_Bills)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Vendor Bills!");
                                return;
                            }
                            if (pettyCash.Payment.Status.isActive == false)
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
                        frmBillPayments.groupId = pettyCash.Payment.transactionGroupId;
                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                        frmBillPayments.frmBillPaymentWindow.Show();

                    }
                    else if (pettyCash.Payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Loans_Advances)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Loans and Advances!");
                                return;
                            }
                            if (pettyCash.Payment.Status.isActive == false)
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
                        frmLAPayments.groupId = pettyCash.Payment.transactionGroupId;
                        frmLAPayments.frmPiPaymentWindow.Content = frmLAPayments;
                        frmLAPayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmLAPayments.frmPiPaymentWindow.Title = "Payments";
                        frmLAPayments.frmPiPaymentWindow.Show();

                    }
                    else if (pettyCash.Payment.transactionType == ERP_BL.Enums.PaymentTransactionType.Target_Reward)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") == null)
                            {
                                DXMessageBox.Show("Permission required to View Existing Payments for Target Rewards!");
                                return;
                            }
                            if (pettyCash.Payment.Status.isActive == false)
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
                        frmTRPayments.groupId = pettyCash.Payment.transactionGroupId;
                        frmTRPayments.frmPiPaymentWindow.Content = frmTRPayments;
                        frmTRPayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmTRPayments.frmPiPaymentWindow.Title = "Payments";
                        frmTRPayments.frmPiPaymentWindow.Show();

                    }

                    break;

                case ERP_BL.Enums.TransactionItemType.Sale_Receipt:
                    var saleReceipt = pettyCash.SalesReceipt;
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
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
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
            }
        }

        private void GrdListTransactions_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                //if (e.Column.FieldName == "balanceAmount")
                //{
                //    var grid = (sender as GridControl).View;
                //    if (e.ListSourceRowIndex == 0)
                //        e.Value = e.GetListSourceFieldValue("total");
                //    else
                //    {
                //        var total = Convert.ToDecimal(e.GetListSourceFieldValue("total"));
                //        var balanceAmount = Convert.ToDecimal(e.GetListSourceFieldValue(e.ListSourceRowIndex - 1, "balanceAmount"));
                //        e.Value = total + balanceAmount;
                //    }
                //}

                GridControl view = (GridControl)sender;
                if (e.Column.FieldName == "balanceAmount" & e.IsGetData)
                {
                    double total = 0;
                    int rHandle = view.GetRowHandleByListIndex( e.ListSourceRowIndex);
                    for (int i = -1; i <= rHandle - 1; i++)
                    {
                        total += Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                    }
                    e.Value = total;
                }
            }
            catch (StackOverflowException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void View1_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
        {
            //if (e.Column.FieldName != "balanceAmount")
            //{
            //    string fieldName = e.Column.FieldName;
            //    if(fieldName.Contains("."))
            //    {
            //        int index = fieldName.IndexOf('.');
            //        if (index >= 0)
            //        {
            //            fieldName = fieldName.Substring(0, index);
            //        }
            //    }
            //    var list = (grdPettyCash.ItemsSource as List<PettyCash>) == null ? new List<PettyCash>() : grdPettyCash.ItemsSource as List<PettyCash>;
            //    var sortedList = list.OrderBy(x => x.GetPropertyValue(fieldName)).ToList();
            //    grdPettyCash.ItemsSource = sortedList;
            //    pettyCashes = pettyCashes;
            //} 
            
        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            LoadPettyCashes();
        }

        private void View1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

        }

        double debitTotal = 0, creditTotal = 0;
        private void GrdPettyCash_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                GridControl gridControl = sender as GridControl;

                switch (e.SummaryProcess)
                {
                    case CustomSummaryProcess.Start:
                        debitTotal = 0;
                        creditTotal = 0;
                        break;
                    case CustomSummaryProcess.Calculate:
                        debitTotal += Convert.ToDouble(grdPettyCash.GetCellValue(e.RowHandle, grdPettyCash.Columns["debit"]));
                        creditTotal += Convert.ToDouble(grdPettyCash.GetCellValue(e.RowHandle, grdPettyCash.Columns["credit"]));
                        
                        //Total = debitTotal - creditTotal;
                        break;
                    case CustomSummaryProcess.Finalize:

                        e.TotalValue = debitTotal - creditTotal;
                        break;
                }
            

                
            }
        }
    }
}
