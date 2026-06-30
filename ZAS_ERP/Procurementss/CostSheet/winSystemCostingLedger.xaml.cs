using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;

namespace ZAS_ERP.Procurementss.CostSheet
{
    /// <summary>
    /// Interaction logic for winSystemCostingLedger.xaml
    /// </summary>
    public partial class winSystemCostingLedger : DXWindow
    {
        int fieldId = 0;
        int costSheetId = 0;
        ProcurementRepo repo = new ProcurementRepo();

        public winSystemCostingLedger()
        {
            InitializeComponent();
        }
        public winSystemCostingLedger(int _fieldId, int _costSheetId, decimal budgetedValue, decimal soTotal, decimal systemValue)
        {
            InitializeComponent();
            fieldId = _fieldId;
            costSheetId = _costSheetId;
            txtTotalBudgtedCost.Text = budgetedValue.ToString();
           
            var percent = (/*soTotal -*/ systemValue) / soTotal * 100;
            var systemValuePerc = decimal.Round(percent, 2);
            txtTotalSystemCost.Text = systemValue.ToString();
            txtTotalSystemCostPerc.Text = systemValuePerc.ToString();
            txtTotalSOCost.Text = soTotal.ToString();


        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<CostSheetFields> fields = new List<CostSheetFields>();
            if(fieldId!=0)
            {
                try
                {
                    ProcurementRepo repo = new ProcurementRepo();
                    BillRepo billRepo = new BillRepo();
                    PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
                    PaymentRepo paymentRepo = new PaymentRepo();
                    SalesReceiptRepo receiptRepo = new SalesReceiptRepo();

                    var billFields = repo.GetSystemBillCostsByFieldId(fieldId, costSheetId);
                    var poFields = repo.GetSystemPOCostsByFieldId(fieldId, costSheetId);
                    var paymentFields = repo.GetSystemPaymentCostsByFieldId(fieldId, costSheetId);
                    var receiptFields = repo.GetSystemReceiptCostsByFieldId(fieldId, costSheetId);
                    var siFields = repo.GetSystemSaleInvoiceCostsByFieldId(fieldId, costSheetId);
                    foreach (var field in billFields)
                    {
                        Bill bill = new Bill();
                        bill=billRepo.GetForCostsheet(field.Bill_Id);

                        fields.Add(new CostSheetFields()
                        {
                            Id = field.Id,
                            Bill_Id = field.Bill_Id,
                            Value = field.Value,
                            timeStamp = field.timeStamp,
                            isBill = true,
                            Field = field.Field,
                            TimeStamp = field.timeStamp,
                            refNo= bill.SalesReferenceNo


                        });
                    }
                    foreach (var field in poFields)
                    {
                        PurchaseOrder po = new PurchaseOrder();
                        po = purchaseOrderRepo.GetForCostsheet(field.PO_Id);
                        fields.Add(new CostSheetFields()
                        {
                            Id = field.Id,
                            PO_Id = field.PO_Id,
                            Value = field.Value,
                            timeStamp = field.timeStamp,
                            isPO = true,
                            Field = field.Field,
                            TimeStamp = field.timeStamp,
                            refNo=po.SalesReferenceNo
                        });
                    }
                    foreach (var field in paymentFields)
                    {
                        Payment payment = new Payment();
                        payment = paymentRepo.GetForCostSheet(field.Payment_Id);
                        fields.Add(new CostSheetFields()
                        {
                            Id = field.Id,
                            Payment_Id = field.Payment_Id,
                            Value = field.Value,
                            timeStamp = field.timeStamp,
                            isPayment = true,
                            Field = field.Field,
                            TimeStamp = field.timeStamp,
                            refNo=payment.PaymentRefNo
                        });
                    }
                    SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
                    foreach (var field in receiptFields)
                    {
                        SalesReceipt receipt = new SalesReceipt();
                        receipt = receiptRepo.GetSalesReceiptForCostSheet(field.Receipt_Id);
                        fields.Add(new CostSheetFields()
                        {
                            Id = field.Id,
                            Receipt_Id = field.Receipt_Id,
                            Value = field.Value,
                            timeStamp = field.timeStamp,
                            isReceipt = true,
                            Field = field.Field,
                            TimeStamp = field.timeStamp,
                            refNo=receipt.ReceiptRefNo
                        });
                    }
                    foreach (var field in siFields)
                    {
                        SaleInvoice invoice = new SaleInvoice();
                        invoice = saleInvoiceRepo.GetSaleInvoice(field.SI_Id);
                        fields.Add(new CostSheetFields()
                        {
                            Id = field.id,
                            SaleInvoiceId = field.SI_Id,
                            Value = field.Value,
                            timeStamp = field.timeStamp,
                            isSaleInvoice = true,
                            Field = field.Field,
                            TimeStamp = field.timeStamp,
                            refNo = invoice.referenceNo
                        });
                    }
                    grdCostItems.ItemsSource = fields;
                }
                catch (Exception)
                {
                } 
            }
        }

        private void GrdCostItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var field=  grdCostItems.SelectedItem as CostSheetFields;
            if (field.Bill_Id != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, (int)field.Bill_Id);
                procurmentPanel.Show();
            }
            else
            if (field.PO_Id != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)field.PO_Id);
                procurmentPanel.Show();
            }
            else
            if (field.Payment_Id != null)
            {
                PaymentRepo paymentRepo = new PaymentRepo();
                var selectedPayment = paymentRepo.GetPayment((int)field.Payment_Id);
                if (selectedPayment != null)
                {
                    CompanyRepo compRepo = new CompanyRepo();
                    switch (selectedPayment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                            {
                                ucFrmPayments frmPayments = new ucFrmPayments();

                                paymentRepo = new PaymentRepo();

                                frmPayments = new ucFrmPayments();
                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;


                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = selectedPayment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = selectedPayment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                            }

                            break;

                        case PaymentTransactionType.Vendor_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                            {
                                ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                //paymentRepo = new PaymentRepo();
                                //frmBillPayments.payments = paymentRepo.GetVendorBillPaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment != null)
                                {
                                    if (selectedPayment.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmBillPayments.editFlag = true;
                                            frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                            frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                            frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                            frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                            frmBillPayments.frmBillPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Vendor Bill Payment!");
                            }

                            break;

                        case PaymentTransactionType.Purchase_Invoice:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                            {
                                ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                        case PaymentTransactionType.Loans_Advances:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                            {
                                ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = selectedPayment.transactionGroupId;
                                        frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                        frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                        frmLApayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLApayment.editFlag = true;
                                    frmLApayment.groupId = selectedPayment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                    }


                }
            }
            else
            if (field.Receipt_Id != null)
            {
                try
                {
                    CurrencyRepo currencyRepo = new CurrencyRepo();
                    CompanyRepo compRepo = new CompanyRepo();
                    SalesReceiptRepo repo = new SalesReceiptRepo();
                    DepartmentRepo deptRepo = new DepartmentRepo();
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;
                        var saleReceiptListByGroupId = repo.getReceiptsByGroupId((int)field.Receipt_Id);
                        var saleReceipt = repo.GetSalesReceipt(saleReceiptListByGroupId[0].Id);
                        if (saleReceipt == null)
                        {
                            return;
                        }
                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;

                            if (status.isActive == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null)
                            {
                                DXMessageBox.Show("Permission required to Edit or View Closed Receipts!");
                                return;
                            }
                        }


                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;

                        if (saleReceipt.CreditedDate != null)
                            updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                        if (saleReceipt.DepositedDate != null)
                            updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                        if (saleReceipt.InstrumentDate != null)
                            updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                        if (saleReceipt.InstrumentNo != null)
                            updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;


                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                        updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        updateSaleReceiptObj.enterReceiptWindowFlag = true;

                        updateSaleReceiptObj.enter_receipt_win.Show();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }
                catch
                {

                }
            }
            else
            if (field.SaleInvoiceId != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)field.SaleInvoiceId);
                procurmentPanel.Show();
            }
        }
    }
    public class CostSheetFields
    {
        public int Id { get; set; }

        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        public int? Bill_Id { get; set; }
        public int? PO_Id { get; set; }
        public int? Payment_Id { get; set; }
        public int? Receipt_Id { get; set; }
        public int? SaleInvoiceId { get; set; }
        public decimal Value { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isPO { get; set; }
        public bool isBill { get; set; }
        public string refNo { get; set; }
        public bool isReceipt { get; set; }
        public bool isSaleInvoice { get; set; }
        public bool isPayment { get; set; }
        public DateTime TimeStamp { get; set; }

    }

}
