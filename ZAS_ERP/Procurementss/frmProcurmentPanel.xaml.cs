using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
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
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Inquiriess;
using ZAS_ERP.Procurementss.Inquiriess.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Procurementss.PurchaseInvoice.UserControls;
using ZAS_ERP.Procurementss.PurchaseOrderss.Cst_Report;
using ZAS_ERP.Procurementss.PurchaseOrderss.UserControls;
using ZAS_ERP.Procurementss.SaleInvoicess;
using ZAS_ERP.Procurementss.SaleInvoicess.UserControls;
using ZAS_ERP.Procurementss.saleOrderss.SNReport;
using ZAS_ERP.Procurementss.SaleOrderss.UserControls;
using ZAS_ERP.Reportss;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptSR;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmProcurmentPanel.xaml
    /// </summary>
    /// 
    public partial class frmProcurmentPanel : DXWindow
    {
        public bool isCommission;
        TransactionItemType transactionType;
        public int transactionId = 0;
        public bool _isGeneratebySOLink = false;
        public List<Inquiry> inq = new List<Inquiry>();
        SaleInvoice saleInvoice = new SaleInvoice();
        SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
        public bool iscopyTemplate = false;
        public int paymentGroupId = 0;
        public int receiptGroupId = 0;

        public frmProcurmentPanel()
        {
            InitializeComponent();
            this.Activate();

        }
        public frmProcurmentPanel(TransactionItemType _transactionType, int _transactionId, int _paymentGroupId)
        {
            InitializeComponent();
            this.Activate();
            this.transactionId = _transactionId;
            this.transactionType = _transactionType;
            paymentGroupId = _paymentGroupId;

            if (transactionId == 0)
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:

                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:

                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnLinkIBT.Visibility = Visibility.Visible;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        btnCreateLoan.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Memorandum_Sale:

                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:

                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Sale_Invoice:

                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice != null)
                        {
                            isCommission = false;
                        }

                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:

                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        if (paymentGroupId != 0)
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher voucher = new ChartofAccounts.UserControls.ucJournalVoucher(paymentGroupId);
                            usercongrid.Children.Add(voucher);
                        }
                        else
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher();
                            usercongrid.Children.Add(ucVoucher);
                        }


                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;


                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPurchaseInvoiceAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPurchaseInvoiceAdd);
                        break;

                }

            }
            else
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        btnCreateOffer.IsEnabled = true;
                        Inquiriess.ucInquiryAdd.editinquiry = 1;
                        Inquiriess.ucInquiryAdd.inquiryid = transactionId;
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        Offerss.ucOfferAdd.editoffer = 1;
                        Offerss.ucOfferAdd.offerid = transactionId;
                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:
                        btnNewInvoice.IsEnabled = true;
                        btnCreatePO.IsEnabled = true;
                        btnCreateBill.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                        SaleOrderss.ucSaleOrderAdd.saleOrderid = transactionId;
                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        break;
                    case TransactionItemType.Memorandum_Sale:
                        MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                        MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = transactionId;
                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:
                        btnCreateBill.IsEnabled = true;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        ucPurchaseOrderAdd.editpurchaseOrder = 1;
                        ucPurchaseOrderAdd.purchaseOrderid = transactionId;
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        break;
                    case TransactionItemType.Sale_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                        SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = transactionId;
                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice.Id != 0)
                        {
                            isCommission = false;
                        }
                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:
                        Billss.ucBillAdd.editbill = 1;
                        Billss.ucBillAdd.billid = transactionId;
                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId.ToString());
                        usercongrid.Children.Add(ucVoucher);
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.editpurchaseInvoice = 1;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.purchaseInvoiceId = transactionId;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPIAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPIAdd);
                        break;
                    case TransactionItemType.InterBank_Transfer:
                        try
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                            {


                                if (transactionId != null)
                                {
                                    ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                    CompanyRepo compRepo = new CompanyRepo();
                                    InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                                    var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(transactionId);
                                    //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                    ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                    if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                        {
                                            ucFrmBankTransfer.editFlag = true;

                                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";


                                            ucFrmBankTransfer.frmBankTranfer.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";


                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }


                                }
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        break;

                }
            }
            switch (transactionId)
            {
                case 0:
                    break;

            }

        }
        public frmProcurmentPanel(TransactionItemType _transactionType, int _transactionId, int _receiptGroupId, bool _isReceipt)
        {
            InitializeComponent();
            this.Activate();
            this.transactionId = _transactionId;
            this.transactionType = _transactionType;
            receiptGroupId = _receiptGroupId;

            if (transactionId == 0)
            {
                switch (transactionType)
                {
                    case TransactionItemType.JV:
                        if (receiptGroupId != 0)
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher voucher = new ChartofAccounts.UserControls.ucJournalVoucher(receiptGroupId,true);
                            usercongrid.Children.Add(voucher);
                        }
                        else
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher();
                            usercongrid.Children.Add(ucVoucher);
                        }
                        break;
                }

            }
            else
            {
                switch (transactionType)
                {
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId.ToString());
                        usercongrid.Children.Add(ucVoucher);
                        break;
                }
            }
            switch (transactionId)
            {
                case 0:
                    break;

            }

        }
        public frmProcurmentPanel(TransactionItemType _transactionType, int _transactionId)
        {
            InitializeComponent();
            this.Activate();
            this.transactionId = _transactionId;
            this.transactionType = _transactionType;
            
            if (transactionId == 0)
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:

                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:

                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnLinkIBT.Visibility = Visibility.Visible;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateLoan.Visibility = Visibility.Visible;
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Memorandum_Sale:

                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:

                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Sale_Invoice:

                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice!=null)
                        {
                            isCommission = false;
                        }

                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:

                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        if(paymentGroupId!=0)
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher voucher = new ChartofAccounts.UserControls.ucJournalVoucher(paymentGroupId);
                            usercongrid.Children.Add(voucher);
                        }
                        else
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher();
                            usercongrid.Children.Add(ucVoucher);
                        }

                        
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;


                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPurchaseInvoiceAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPurchaseInvoiceAdd);
                        break;

                }

            }
            else
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        btnCreateOffer.IsEnabled = true;
                        Inquiriess.ucInquiryAdd.editinquiry = 1;
                        Inquiriess.ucInquiryAdd.inquiryid = transactionId;
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        Offerss.ucOfferAdd.editoffer = 1;
                        Offerss.ucOfferAdd.offerid = transactionId;
                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;
                    case TransactionItemType.ModuleContract:
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd.editModuleContract = 1;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd.ModuleContractid = transactionId;
                        ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd ucModuleContractAdd = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd();
                        usercongrid.Children.Add(ucModuleContractAdd);
                        break;
                    case TransactionItemType.Sale_Order:
                        btnCreateReciept.IsEnabled = true;
                        btnNewInvoice.IsEnabled = true;
                        btnCreatePO.IsEnabled = true;
                        btnCreateBill.IsEnabled = true;
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                        SaleOrderss.ucSaleOrderAdd.saleOrderid = transactionId;
                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateLoan.Visibility = Visibility.Visible;
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Memorandum_Sale:
                        MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                        MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = transactionId;
                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:
                        btnCreateReciept.IsEnabled = true;
                        btnCreateBill.IsEnabled = true;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        ucPurchaseOrderAdd.editpurchaseOrder = 1;
                        ucPurchaseOrderAdd.purchaseOrderid = transactionId;
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        btnCreateAdvance.Visibility = Visibility.Visible;
                        break;
                    case TransactionItemType.Sale_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                        SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = transactionId;
                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if(saleInvoice.Id != 0)
                        {
                            isCommission = false;
                        }
                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:
                        Billss.ucBillAdd.editbill= 1;
                        Billss.ucBillAdd.billid = transactionId;
                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId.ToString());
                        usercongrid.Children.Add(ucVoucher);
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.editpurchaseInvoice = 1;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.purchaseInvoiceId = transactionId;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPIAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPIAdd);
                        break;
                    case TransactionItemType.InterBank_Transfer:
                        try
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                            {


                                if (transactionId != null)
                                {
                                    ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                    CompanyRepo compRepo = new CompanyRepo();
                                    InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                                    var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(transactionId);
                                    //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                    ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                    if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                        {
                                            ucFrmBankTransfer.editFlag = true;

                                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                           
                                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                     

                                            ucFrmBankTransfer.frmBankTranfer.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                     
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                      

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }


                                }
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        break;
                    case TransactionItemType.InterCompanyBank_Transfer:
                        try
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                            {

                              
                                    ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();
                                    CompanyRepo compRepo = new CompanyRepo();
                                    InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();

                                    
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                        {
                                            ucFrmBankTransfer.editFlag = true;
                                            ucFrmBankTransfer.bankTransferId = transactionId;
                                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";

                                            ucFrmBankTransfer.frmBankTranfer.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                            return;
                                        }
                                    
                                  
                                
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        break;

                }
            }
            switch (transactionId)
            {
                case 0:
                    break;

            }

        }
        public frmProcurmentPanel(TransactionItemType _transactionType, int _transactionId,int _parentSoId, bool isParent, bool isParentSo, bool _isGeneratebySOLink )
        {
            InitializeComponent();
            this.Activate();
            this.transactionId = _transactionId;
            this.transactionType = _transactionType;
            this._isGeneratebySOLink = _isGeneratebySOLink;
            if (transactionId == 0)
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:

                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:
                        if(isParentSo==true)
                        {
                            
                            Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd(true,_parentSoId);
                            usercongrid.Children.Add(ucSaleOrderAdd);
                            btnCreatePayment.Visibility = Visibility.Collapsed;
                            btnLinkIBT.Visibility = Visibility.Visible;
                            btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                            usercongrid.Children.Add(ucSaleOrderAdd);
                            btnCreatePayment.Visibility = Visibility.Collapsed;
                            btnLinkIBT.Visibility = Visibility.Visible;
                            btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        }
                        
                        break;
                    case TransactionItemType.Memorandum_Sale:

                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:

                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        break;
                    case TransactionItemType.Sale_Invoice:

                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice!=null)
                        {
                            isCommission = false;
                        }

                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:

                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        if(paymentGroupId!=0)
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher voucher = new ChartofAccounts.UserControls.ucJournalVoucher(paymentGroupId);
                            usercongrid.Children.Add(voucher);
                        }
                        else
                        {
                            ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher();
                            usercongrid.Children.Add(ucVoucher);
                        }

                        
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;


                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPurchaseInvoiceAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPurchaseInvoiceAdd);
                        break;

                }

            }
            else
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        btnCreateOffer.IsEnabled = true;
                        Inquiriess.ucInquiryAdd.editinquiry = 1;
                        Inquiriess.ucInquiryAdd.inquiryid = transactionId;
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        Offerss.ucOfferAdd.editoffer = 1;
                        Offerss.ucOfferAdd.offerid = transactionId;
                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:
                        btnNewInvoice.IsEnabled = true;
                        btnCreatePO.IsEnabled = true;
                        btnCreateBill.IsEnabled = true;
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                        SaleOrderss.ucSaleOrderAdd.saleOrderid = transactionId;
                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd();
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        break;
                    case TransactionItemType.Memorandum_Sale:
                        MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                        MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = transactionId;
                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:
                        btnCreateBill.IsEnabled = true;
                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        ucPurchaseOrderAdd.editpurchaseOrder = 1;
                        ucPurchaseOrderAdd.purchaseOrderid = transactionId;
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        break;
                    case TransactionItemType.Sale_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                        SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = transactionId;
                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if(saleInvoice.Id != 0)
                        {
                            isCommission = false;
                        }
                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:
                        Billss.ucBillAdd.editbill= 1;
                        Billss.ucBillAdd.billid = transactionId;
                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId.ToString());
                        usercongrid.Children.Add(ucVoucher);
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.editpurchaseInvoice = 1;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.purchaseInvoiceId = transactionId;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPIAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPIAdd);
                        break;
                    case TransactionItemType.InterBank_Transfer:
                        try
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                            {


                                if (transactionId != null)
                                {
                                    ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                    CompanyRepo compRepo = new CompanyRepo();
                                    InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                                    var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(transactionId);
                                    //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                    ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                    if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                        {
                                            ucFrmBankTransfer.editFlag = true;

                                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                           
                                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                     

                                            ucFrmBankTransfer.frmBankTranfer.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                     
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                      

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }


                                }
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        break;
                    case TransactionItemType.InterCompanyBank_Transfer:
                        try
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                            {

                              
                                    ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();
                                    CompanyRepo compRepo = new CompanyRepo();
                                    InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();

                                    
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                        {
                                            ucFrmBankTransfer.editFlag = true;
                                            ucFrmBankTransfer.bankTransferId = transactionId;
                                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";

                                            ucFrmBankTransfer.frmBankTranfer.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                            return;
                                        }
                                    
                                  
                                
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        break;

                }
            }
            switch (transactionId)
            {
                case 0:
                    break;

            }

        }

        public frmProcurmentPanel(TransactionItemType _transactionType, int _transactionId, bool isCopy)
        {
            InitializeComponent();
            this.Activate();
            this.transactionId = _transactionId;
            this.transactionType = _transactionType;
            this.iscopyTemplate = isCopy;
            if (transactionId == 0)
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:

                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:

                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd(iscopyTemplate);
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        break;
                    case TransactionItemType.Memorandum_Sale:

                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:

                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd(iscopyTemplate);
                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        break;
                    case TransactionItemType.Sale_Invoice:

                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice != null)
                        {
                            isCommission = false;
                        }

                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:

                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher();
                        usercongrid.Children.Add(ucVoucher);
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;


                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPurchaseInvoiceAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPurchaseInvoiceAdd);
                        break;

                }

            }
            else
            {
                switch (transactionType)
                {
                    case TransactionItemType.Inquiry:
                        btnCreateOffer.IsEnabled = true;
                        Inquiriess.ucInquiryAdd.editinquiry = 1;
                        Inquiriess.ucInquiryAdd.inquiryid = transactionId;
                        Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
                        usercongrid.Children.Add(ucInquiryAdd);
                        break;
                    case TransactionItemType.Offer:
                        btnCreateSO.IsEnabled = true;
                        btnCreateModuleContract.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        Offerss.ucOfferAdd.editoffer = 1;
                        Offerss.ucOfferAdd.offerid = transactionId;
                        Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                        usercongrid.Children.Add(ucOfferAdd);
                        break;

                    case TransactionItemType.Sale_Order:
                        btnNewInvoice.IsEnabled = true;
                        btnCreatePO.IsEnabled = true;
                        btnCreateBill.IsEnabled = true;
                        btnCreateMemorandumSale.IsEnabled = true;
                        SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                        SaleOrderss.ucSaleOrderAdd.saleOrderid = transactionId;
                        Procurementss.SaleOrderss.ucSaleOrderAdd ucSaleOrderAdd = new SaleOrderss.ucSaleOrderAdd(iscopyTemplate);
                        usercongrid.Children.Add(ucSaleOrderAdd);
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        break;
                    case TransactionItemType.Memorandum_Sale:
                        MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                        MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = transactionId;
                        Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                        usercongrid.Children.Add(ucMemorandumSaleAdd);
                        break;
                    case TransactionItemType.Purchase_Order:
                        btnCreateBill.IsEnabled = true;
                        //PurchaseOrderss.ucPurchaseOrderAdd.editpurchaseOrder = 1;
                        //PurchaseOrderss.ucPurchaseOrderAdd.purchaseOrderid = transactionId;
                        //Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd(iscopyTemplate);

                        Procurementss.PurchaseOrderss.ucPurchaseOrderAdd ucPurchaseOrderAdd = new PurchaseOrderss.ucPurchaseOrderAdd(iscopyTemplate);
                        ucPurchaseOrderAdd.editpurchaseOrder = 1;
                        ucPurchaseOrderAdd.purchaseOrderid = transactionId;

                        usercongrid.Children.Add(ucPurchaseOrderAdd);
                        break;
                    case TransactionItemType.Sale_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                        SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = transactionId;
                        Procurementss.SaleInvoicess.ucSaleInvoiceAdd ucSaleInvoiceAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePayment.Visibility = Visibility.Collapsed;
                        saleInvoice = saleInvoiceRepo.getForCommissionInvoice(_transactionId);
                        if (saleInvoice.Id != 0)
                        {
                            isCommission = false;
                        }
                        usercongrid.Children.Add(ucSaleInvoiceAdd);
                        break;
                    case TransactionItemType.Bill:
                        Billss.ucBillAdd.editbill = 1;
                        Billss.ucBillAdd.billid = transactionId;
                        Procurementss.Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateBill.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        usercongrid.Children.Add(ucBillAdd);
                        break;
                    case TransactionItemType.JV:
                        ChartofAccounts.UserControls.ucJournalVoucher ucVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId.ToString());
                        usercongrid.Children.Add(ucVoucher);
                        break;
                    case TransactionItemType.Purchase_Invoice:
                        btnCreateReciept.IsEnabled = true;
                        btnNewInvoice.Visibility = Visibility.Collapsed;
                        btnCreatePO.Visibility = Visibility.Collapsed;
                        btnCreateReciept.Visibility = Visibility.Collapsed;
                        btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                        btnCreateSO.Visibility = Visibility.Collapsed;
                        btnCreateModuleContract.Visibility = Visibility.Collapsed;
                        btnCreateOffer.Visibility = Visibility.Collapsed;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.editpurchaseInvoice = 1;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd.purchaseInvoiceId = transactionId;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd ucPIAdd = new ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucPIAdd();
                        usercongrid.Children.Add(ucPIAdd);
                        break;

                }
            }
            switch (transactionId)
            {
                case 0:
                    break;

            }

        }
        private void btnCreateOffer_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && TransactionItemType.Inquiry == transactionType)
            {
                Inquiriess.ucStatuschange.inquiryid = transactionId;
                Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();

                {
                    usercongrid.Children.Clear();
                    Offerss.ucOfferAdd.inquiryid = transactionId;
                    Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
                    usercongrid.Children.Add(ucOfferAdd);
                    Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                    Procurementss.Offerss.ucOfferAdd.offerid = 0;
                    transactionType = TransactionItemType.Offer;
                }
                btnCreateModuleContract.Visibility = Visibility.Visible;

            }
            else
            {
                MessageBox.Show("Create or load Inquiry First to create Offer!", "Inquiry not Found");
            }
        }

        private void btnNewInquiry_Click(object sender, RoutedEventArgs e)
        {
            transactionId = 0;

            Procurementss.Inquiriess.ucInquiryAdd.editinquiry = 0;
            Procurementss.Inquiriess.ucInquiryAdd.inquiryid = 0;
            usercongrid.Children.Clear();
            Inquiriess.ucInquiryAdd ucInquiryAdd = new Inquiriess.ucInquiryAdd();
            usercongrid.Children.Add(ucInquiryAdd);
            transactionType = TransactionItemType.Inquiry;
        }

        private void btnNewOffer_Click(object sender, RoutedEventArgs e)
        {
      
            transactionId = 0;
           
            Procurementss.Offerss.ucOfferAdd.editoffer = 0;
            Procurementss.Offerss.ucOfferAdd.offerid = 0;
            Procurementss.Offerss.ucOfferAdd.inquiryid = 0;
            usercongrid.Children.Clear();
            Offerss.ucOfferAdd ucOfferAdd = new Offerss.ucOfferAdd();
            usercongrid.Children.Add(ucOfferAdd);
            transactionType = TransactionItemType.Offer;



        }

        private void winProcpanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (e.NewSize.Width <= 1000)
            {
                MyScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                MyScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            if (e.NewSize.Height <= 800)
            {
                MyScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                MyScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }

        }

        private void winProcpanel_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Procurementss.Offerss.ucOfferAdd.editoffer = 0;
            Procurementss.Offerss.ucOfferAdd.offerid = 0;
            Procurementss.Offerss.ucOfferAdd.inquiryid = 0;
            Procurementss.Inquiriess.ucInquiryAdd.inquiryid = 0;
            Procurementss.SaleOrderss.ucSaleOrderAdd.offerid = 0;
            Procurementss.SaleOrderss.ucSaleOrderAdd.moduleContractid = 0;
            Procurementss.SaleOrderss.ucSaleOrderAdd.saleOrderid = 0;
            Procurementss.MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = 0;
            Procurementss.MemorandumSaless.ucMemorandumSaleAdd.offerid = 0;
            Procurementss.PurchaseOrderss.ucPurchaseOrderAdd.saleOrderidd = 0;
            //Procurementss.PurchaseOrderss.ucPurchaseOrderAdd.purchaseOrderid = 0;
            Procurementss.Billss.ucBillAdd.billid = 0;
            Procurementss.Billss.ucBillAdd.saleOrderid = 0;
            Procurementss.Billss.ucBillAdd.purchaseOrderid = 0;
            Procurementss.Billss.ucBillAdd.saleOrderid = 0;
            Procurementss.Billss.ucBillAdd.billid = 0;

        }

        private void btnNewPO_Click(object sender, RoutedEventArgs e)
        {
            transactionId = 0;
            Procurementss.Offerss.ucOfferAdd.editoffer = 0;
            Procurementss.Offerss.ucOfferAdd.offerid = 0;
            Procurementss.Offerss.ucOfferAdd.inquiryid = 0;
            usercongrid.Children.Clear();
            PurchaseOrderss.ucPurchaseOrderAdd ucPOAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
            usercongrid.Children.Clear();
            usercongrid.Children.Add(ucPOAdd);
            transactionType = TransactionItemType.Purchase_Order;

        }

        private void btnCreateSO_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0&& transactionType == TransactionItemType.Offer)
            {

                Offerss.ucStatuschange.offerid = transactionId;
                Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                SaleOrderss.ucSaleOrderAdd ucPOAdd = new SaleOrderss.ucSaleOrderAdd();
                usercongrid.Children.Add(ucPOAdd);
                SaleOrderss.ucSaleOrderAdd.offerid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
            }
            else
            if (transactionId != 0 && transactionType == TransactionItemType.ModuleContract)
            {
                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucStatuschange.ModuleContractid = transactionId;
                ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusChange statusChange = new ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                SaleOrderss.ucSaleOrderAdd ucSOAdd = new SaleOrderss.ucSaleOrderAdd();
                usercongrid.Children.Add(ucSOAdd);
                SaleOrderss.ucSaleOrderAdd.moduleContractid = transactionId;
                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd.editModuleContract = 0;
                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd.ModuleContractid = 0;
            }
            else
            if (transactionId!=0 && transactionType== TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                SaleOrderss.ucStatuschange.saleOrderid = transactionId;

                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                SaleOrderss.ucSaleOrderAdd ucSOAdd = new SaleOrderss.ucSaleOrderAdd();
                usercongrid.Children.Add(ucSOAdd);
                SaleOrderss.ucSaleOrderAdd.parentSaleOrderid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Sale_Order;
            }
            else
            {
                MessageBox.Show("Create or load Offer First to create Sale Order!", "Offer not Found");
            }
        }

        private void btnCreatePO_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType==TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                SaleOrderss.ucStatuschange.saleOrderid = transactionId;
                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                PurchaseOrderss.ucPurchaseOrderAdd ucPOAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                usercongrid.Children.Add(ucPOAdd);
                PurchaseOrderss.ucPurchaseOrderAdd.saleOrderidd = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Purchase_Order;
            }
            else
            {
                MessageBox.Show("Create or load SaleOrder First to create SaleOrder!", "Sale Order not Found");
            }
        }
        private void btnCreateBill_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.isApproved == false)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for Approval stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                

                usercongrid.Children.Clear();
                Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                usercongrid.Children.Add(ucBillAdd);
                Billss.ucBillAdd.purchaseOrderid = 0;
                Billss.ucBillAdd.saleOrderid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Bill;
            }
            else if(transactionId != 0 && transactionType == TransactionItemType.Purchase_Order)
            {
                usercongrid.Children.Clear();
                Billss.ucBillAdd ucBillAdd = new Billss.ucBillAdd();
                usercongrid.Children.Add(ucBillAdd);
                Billss.ucBillAdd.saleOrderid = 0;
                Billss.ucBillAdd.purchaseOrderid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Bill;
            }
            else
            {
                MessageBox.Show("Create or load SaleOrder/PurchaseOrder first to create Bill!", "Sale Order or Purchase Order not Found");
            }
        }

        private void btnCreateSaleinvoice_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0&& transactionType == TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                SaleOrderss.ucStatuschange.saleOrderid = transactionId;
                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange();
                SaleOrderss.ucStatuschange.inActiveStatuses = 0;

                statusChange.Owner = this;
                statusChange.ShowDialog();
               
                usercongrid.Children.Clear();

                SaleInvoicess.ucSaleInvoiceAdd ucSIAdd = new SaleInvoicess.ucSaleInvoiceAdd();
                usercongrid.Children.Add(ucSIAdd);
                SaleInvoicess.ucSaleInvoiceAdd.saleOrderid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Sale_Invoice;
                //isCommission = false;
            }
            else
            {
                MessageBox.Show("Create or load SaleOrder First to create SaleInvoice!", "Sale Order not Found");
            }
        }
        private void BtnNewSO_Click(object sender, RoutedEventArgs e)
        {
            transactionId= 0;
            Procurementss.Offerss.ucOfferAdd.editoffer = 0;
            Procurementss.Offerss.ucOfferAdd.offerid = 0;
            Procurementss.Offerss.ucOfferAdd.inquiryid = 0;
            usercongrid.Children.Clear();
            SaleOrderss.ucSaleOrderAdd ucPOAdd = new SaleOrderss.ucSaleOrderAdd();
            usercongrid.Children.Add(ucPOAdd);
            transactionType = TransactionItemType.Sale_Order;
        }

        private void BtnNewMemorandumSale_Click(object sender, RoutedEventArgs e)
        {
            transactionId = 0;
            usercongrid.Children.Clear();
            Procurementss.MemorandumSaless.ucMemorandumSaleAdd ucMemorandumSaleAdd = new MemorandumSaless.ucMemorandumSaleAdd();
            usercongrid.Children.Add(ucMemorandumSaleAdd);
            transactionType = TransactionItemType.Memorandum_Sale;
        }

        private void BtnCreateMemorandumSale_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Offer)
            {

                Offerss.ucStatuschange.offerid = transactionId;
                Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                MemorandumSaless.ucMemorandumSaleAdd.offerid = transactionId;
                MemorandumSaless.ucMemorandumSaleAdd ucPOAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                usercongrid.Children.Add(ucPOAdd);
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Memorandum_Sale;

            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                SaleOrderss.ucStatuschange.saleOrderid = transactionId;
                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                MemorandumSaless.ucMemorandumSaleAdd.saleOrderid = transactionId;
                MemorandumSaless.ucMemorandumSaleAdd ucPOAdd = new MemorandumSaless.ucMemorandumSaleAdd();
                usercongrid.Children.Add(ucPOAdd);
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
                transactionType = TransactionItemType.Memorandum_Sale;

            }
            else
            {
                DXMessageBox.Show("Create or load Offer/Sale Order first, to create Memorandum Sale!", "Offer/Sale Order not Found",MessageBoxButton.OK,MessageBoxImage.Information);
            }
        }
        private void BtnCreateReportLayout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Purchase Order layout? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (transactionType == TransactionItemType.Purchase_Order)
                    {

                        PurchaseOrderRepo repo = new PurchaseOrderRepo();
                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                        purchaseOrder = repo.get(transactionId);
                        if (purchaseOrder.company.CompanyName == "AL BIJJAR TRADING")
                        {
                            ucPurchaseOrderReport firstPage = new ucPurchaseOrderReport(purchaseOrder);
                            ucPurchaseOrderReport2 secondPage = new ucPurchaseOrderReport2();
                            frmReportPanel panel = new frmReportPanel(firstPage, secondPage);
                            panel.Show();
                        }
                        else
                             if (purchaseOrder.company.CompanyName == "JAZ TRADE International")
                        {
                            ucPurchaseOrderReportJazzTrade firstPage = new ucPurchaseOrderReportJazzTrade(purchaseOrder);
                            ucPurchaseOrderReportJazTradePage2 secondPage = new ucPurchaseOrderReportJazTradePage2();
                            frmReportPanel panel = new frmReportPanel(firstPage, secondPage);
                            panel.Show();
                        }
                        else if (purchaseOrder.company.CompanyName == "ABT Europe Ltd")
                        {
                            ucFrmPoReportsAbtUk ucPoAbtEurope = new ucFrmPoReportsAbtUk(purchaseOrder);
                            ucFrmPoReportsAbtUkPage2 ucPoAbtEuropePage2 = new ucFrmPoReportsAbtUkPage2();
                            frmReportPanel panel = new frmReportPanel(ucPoAbtEurope, ucPoAbtEuropePage2);
                            panel.Show();
                        }
                        else if (purchaseOrder.company.CompanyName == "Competitive Solutions Trading")
                        {
                            ucCstReport frmCstReport = new ucCstReport(purchaseOrder);
                            frmReportPanel panel = new frmReportPanel(frmCstReport);
                            panel.Show();
                        }
                        else if (purchaseOrder.company.CompanyName == "Z&F Corporation")
                        {
                            ucPurchaseOrderReportZFCoorporation firstPage = new ucPurchaseOrderReportZFCoorporation(purchaseOrder);
                            ucPurchaseOrderReportZFCoorporationPage2 secondPage = new ucPurchaseOrderReportZFCoorporationPage2();
                            frmReportPanel panel = new frmReportPanel(firstPage, secondPage);
                            panel.Show();
                        }
                      

                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void WinProcpanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (transactionType == TransactionItemType.Purchase_Order)
                btnCreateReportLayout.IsEnabled = true;
            else
                btnCreateReportLayout.IsEnabled = false;

            if (transactionType == TransactionItemType.Sale_Invoice)
            {
                btnCreateDNReportLayout.IsEnabled = true;
                btnCreateSITaxReport.IsEnabled = true;
            }
            else if (transactionType == TransactionItemType.Sale_Order)
            {
                btnCreateDNReportLayout.IsEnabled = true;
            }
            else
            {
                btnCreateDNReportLayout.IsEnabled = false;
                btnCreateSITaxReport.IsEnabled = false;
            }
            //if (transactionType == TransactionItemType.Sale_Order)
            //{
            //    btnCreateDNReportLayout.IsEnabled = true;
            //}
            //else
            //{
            //    btnCreateDNReportLayout.IsEnabled = false;
            //}
            if (transactionType == TransactionItemType.Sale_Order)
                btnCreatePO.IsEnabled = false;

            if (transactionType == TransactionItemType.Sale_Invoice /*&& saleInvoice.Id!= 0*/)
            { 
               if(isCommission == false)
                {
                    if (saleInvoice.saleInvoicetype == InquiryType.Principal)
                    {
                        btnCreateCommissionSIReport.Visibility = Visibility.Visible;
                    }
                }
               
              
            }
        }

        private void btnCreateReciept_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Invoice)
            {
                SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();
                var SI = invoiceRepo.get(transactionId);

                if (SI.saleInvoiceStatus != null && SI.saleInvoiceStatus.isActive == false)
                {
                    DXMessageBox.Show("You cannot create Sale Receipt of a closed Sale Invoice!", "Closed Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SI.isApproved == false)
                {
                    DXMessageBox.Show("You cannot create Sale Receipt of an UnApproved Sale Invoice!", "UnApproved Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (SI.isVoid == true)
                {
                    DXMessageBox.Show("You cannot create Sale Receipt of a voided Sale Invoice!", "Voided Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                var amount = SI.salesReceipts.Where(x => x.isVoid != true).Sum(x=>x.CollectionAmount);
                if(SI.totalInvoiceAmount == amount)
                {
                    DXMessageBox.Show("You cannot create Sale Recaipt of a Fully Received Sale Invoice!", "Fully Received", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ucFrmSaleReceipt enterSaleReceiptObj = new ucFrmSaleReceipt();
                enterSaleReceiptObj.enterReceiptWindowFlag = true;
                enterSaleReceiptObj.invoiceNo = transactionId;
                enterSaleReceiptObj.enter_receipt_win.Content = enterSaleReceiptObj;
                enterSaleReceiptObj.enter_receipt_win.Title = "Sale Receipt";
                enterSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                enterSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                enterSaleReceiptObj.enter_receipt_win.Show();


                

            }
            
            else
            {
                MessageBox.Show("Create or load Sale Invoice First to create Sale Receipt!", "Invoice not Found");
            }
        }

        private void BtnCreateJV_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Bill)
            {
                usercongrid.Children.Clear();
                ChartofAccounts.UserControls.ucJournalVoucher ucJournalVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId, TransactionItemType.Bill);
                usercongrid.Children.Add(ucJournalVoucher);
                transactionType = TransactionItemType.JV;
            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Purchase_Order)
            {
                usercongrid.Children.Clear();
                ChartofAccounts.UserControls.ucJournalVoucher ucJournalVoucher = new ChartofAccounts.UserControls.ucJournalVoucher(transactionId, TransactionItemType.Purchase_Order);
                usercongrid.Children.Add(ucJournalVoucher);
            }
            else
            {
                MessageBox.Show("Create or load SaleOrder/PurchaseOrder first to create JV!", "Sale Order or Purchase Order not Found");
            }
        }

        private void BtnCreatePurchaseInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Purchase_Order)
            {
                PurchaseOrderRepo POrepo = new PurchaseOrderRepo();
                var _PO = POrepo.getForPI(transactionId);
                if(_PO != null)
                {
                    if(_PO.tax != null)
                    {
                         var PIsum = _PO.PurchaseInvoices.Where(x=>x.isVoid != true).Sum(x=>x.totalInvoiceAmount);
                        PIsum = PIsum + (PIsum * _PO.tax.percentage) / 100;
                        if(PIsum >= _PO.billWithTax)
                        {
                            DXMessageBox.Show("PO cannot be Over Invoiced!");
                            return;
                        }
                    }
                    else
                    {
                        var PIsum = _PO.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                        if (PIsum >= _PO.totalCFRValue)
                        {
                            DXMessageBox.Show("PO cannot be Over Invoiced!");
                            return;
                        }
                    }
                }

                PurchaseOrderss.ucStatuschange.purchaseOrderid = transactionId;
                PurchaseOrderss.frmPurchaseOrderStatusChange statusChange = new PurchaseOrderss.frmPurchaseOrderStatusChange();
                PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;

                statusChange.Owner = this;
                statusChange.ShowDialog();

                usercongrid.Children.Clear();
                btnNewInvoice.Visibility = Visibility.Collapsed;
                btnCreatePO.Visibility = Visibility.Collapsed;
                btnCreateReciept.Visibility = Visibility.Collapsed;
                btnCreatePurchaseInvoice.Visibility = Visibility.Collapsed;
                btnCreateSO.Visibility = Visibility.Collapsed;
                btnCreateModuleContract.Visibility = Visibility.Collapsed;
                btnCreateOffer.Visibility = Visibility.Collapsed;

                ucPIAdd ucPIIAdd = new ucPIAdd();
                usercongrid.Children.Add(ucPIIAdd);
                ucPIAdd.purchaseOrderId = transactionId;
                ucPIAdd.editpurchaseInvoice = 0;


                transactionType = TransactionItemType.Purchase_Invoice;
            }
            else
            {
                DXMessageBox.Show("Create or load Purchase Order First to create Purchase Invoice!", "Purchase Order not found", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCreatePayment_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Purchase_Invoice)
            {
                PurchaseInvoiceRepo PIrepo = new PurchaseInvoiceRepo();
                var PI = PIrepo.getForPayments(transactionId);

                if(PI != null)
                {
                    if (PI.PurchaseInvoiceStatus != null && PI.PurchaseInvoiceStatus.isActive == false)
                    {
                        DXMessageBox.Show("You cannot create payment of a closed Purchase Invoice!", "Closed Purchase Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (PI.isApproved == false)
                    {
                        DXMessageBox.Show("You cannot create Payment of an UnApproved Purchase Invoice!", "UnApproved Purchase Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (PI.isVoid == true)
                    {
                        DXMessageBox.Show("You cannot create Payment of a Voided Purchase Invoice!", "Voided Purchase Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var paidAmount = PI.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                    double invoiceAmount;

                    if (PI.PurchaseOrder.tax != null)
                    {
                        invoiceAmount = PI.totalInvoiceAmount;

                        if (PI.PurchaseOrder.tax.isManual == true)
                        {
                            var percentAmount = PI.totaltaxAmount;
                            invoiceAmount = Math.Round(percentAmount + invoiceAmount, 2);
                        }
                        else
                        {
                            var percentAmount = Math.Round((PI.PurchaseOrder.tax.percentage * invoiceAmount) / 100, 2);
                            invoiceAmount = Math.Round(percentAmount + invoiceAmount, 2);
                        }
                    }
                    else
                        invoiceAmount = PI.totalInvoiceAmount;

                    var percentPaid = Math.Round((paidAmount / Convert.ToDouble(invoiceAmount)) * 100, 2);

                    if (percentPaid == 100)
                    {
                        DXMessageBox.Show("You cannot create payment of a Fully Paid Purchase Invoice!", "Fully Paid", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                

                ucFrmPInvoicePaymentAdd frmPInvoicePayment = new ucFrmPInvoicePaymentAdd();
                frmPInvoicePayment.createdFromBill = true;
                frmPInvoicePayment.frmPiPaymentWindow.Content = frmPInvoicePayment;
                frmPInvoicePayment.frmPiPaymentWindow.Title = "Payment";
                frmPInvoicePayment.frmPiPaymentWindow.ResizeMode = ResizeMode.CanResize;
                frmPInvoicePayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                frmPInvoicePayment.editFlag = false;
                frmPInvoicePayment.purchaseInvoiceId = transactionId;
                frmPInvoicePayment.frmPiPaymentWindow.Show();

                
            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Bill)
            {
                BillRepo billRepo = new BillRepo();
                var bill = billRepo.get(transactionId);
                if (bill != null)
                {
                    if(bill.BillStatus != null && bill.BillStatus.isActive == false)
                    {
                        DXMessageBox.Show("You cannot create payment of a closed Bill!" , "Closed Bill", MessageBoxButton.OK,MessageBoxImage.Warning);
                        return;
                    }

                    if (bill.isApproved == false)
                    {
                        DXMessageBox.Show("You cannot create Payment of an UnApproved Bill!", "UnApproved Bill", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (bill.isVoid == true)
                    {
                        DXMessageBox.Show("You cannot create Payment of a voided Bill!", "Voided Bill", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var paidAmount = bill.Payments.Where(x => x.isVoid != true).Sum(y => y.DebitedAmount);
                    double percentPaid = 0;
                    if (bill.billWithTax != null)
                        percentPaid = Math.Round((paidAmount / Convert.ToDouble(bill.billWithTax)) * 100, 2);
                    else
                        percentPaid = Math.Round((paidAmount / Convert.ToDouble(bill.totalCFRValue)) * 100, 2);

                    if (percentPaid == 100)
                    {
                        DXMessageBox.Show("You cannot create payment of a Fully Paid Bill!", "Fully Paid", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                ucFrmBillPaymentAdd frmBillPayment = new ucFrmBillPaymentAdd();
                frmBillPayment.createdFromBill = true;
                frmBillPayment.frmBillPaymentWindow.Content = frmBillPayment;
                frmBillPayment.frmBillPaymentWindow.Title = "Payment";
                frmBillPayment.frmBillPaymentWindow.ResizeMode = ResizeMode.CanResize;
                frmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                frmBillPayment.editFlag = false;
                frmBillPayment.vendorBillId = transactionId;
                frmBillPayment.frmBillPaymentWindow.Show();


            }
            else
            {
                DXMessageBox.Show("Create or load Purchase Invoice or Vendor Bill First to create Payment!", "Purchase Invoice or Vendor Bill not found", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void BtnCreateDNReportLayout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (transactionType == TransactionItemType.Sale_Invoice)
                {
                    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Sale Invoice D-N? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {


                        SaleInvoiceRepo repo = new SaleInvoiceRepo();
                        SaleInvoice SI = new SaleInvoice();
                        SI = repo.getForSIReports(transactionId);

                        ucSaleInvoiceReport firstPage = new ucSaleInvoiceReport(SI);
                        //ucPurchaseOrderReport2 secondPage = new ucPurchaseOrderReport2();
                        frmReportPanel panel = new frmReportPanel(firstPage);
                        panel.Show();


                    }
                }
                else
                {
                    if (transactionType == TransactionItemType.Sale_Order)
                    {
                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Sale Order D-N? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                        {
                            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                            var saleOrder = saleOrderRepo.get(transactionId);
                            

                            if (saleOrder.company.CompanyName == "S & N TRADING")
                            {
                                ucSNDNReport firstPage = new ucSNDNReport(saleOrder);
                                frmReportPanel panel = new frmReportPanel(firstPage);
                                panel.Show();
                            }


                        }
                    }
                }
                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void BtnCreateSITaxReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Sale Invoice Tax report? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (transactionType == TransactionItemType.Sale_Invoice)
                    {
                        SaleInvoiceRepo repo = new SaleInvoiceRepo();
                        SaleInvoice SI = new SaleInvoice();
                        SI = repo.getForSIReports(transactionId);
                        ucSItaxInvoice ucSItax = new ucSItaxInvoice(SI);
                        frmReportPanel panel = new frmReportPanel(ucSItax);
                        panel.Show();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCreateCommissionSIReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create commission invoice report? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (transactionType == TransactionItemType.Sale_Invoice)
                    {
                        SaleInvoiceRepo repo = new SaleInvoiceRepo();
                        SaleInvoice SI = new SaleInvoice();
                        SI = repo.getForSIReports(transactionId);
                        ucSIcommissionInvoice ucSItax = new ucSIcommissionInvoice(SI);
                        ucCommissionInvoicePage2 page2 = new ucCommissionInvoicePage2(SI);
                        frmReportPanel panel = new frmReportPanel(ucSItax,page2);
                        panel.Show();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PurchaseOrderRepo repo = new PurchaseOrderRepo();
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                purchaseOrder = repo.get(transactionId);
                ucFrmPoReportsAbtUk ucPoAbtEurope = new ucFrmPoReportsAbtUk(purchaseOrder);
                ucFrmPoReportsAbtUkPage2 ucPoAbtEuropePage2 = new ucFrmPoReportsAbtUkPage2();
                frmReportPanel panel = new frmReportPanel(ucPoAbtEurope, ucPoAbtEuropePage2);
                panel.Show();
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
          

        }

        private void BtnLinkIBT_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                var _SO = saleOrderRepo.get(transactionId);
                ucSaleOrdertoIBT ucSaleOrdertoIBT = new ucSaleOrdertoIBT(_SO);
                usercongrid.Children.Clear();
                usercongrid.Children.Add(ucSaleOrdertoIBT);
                transactionType = TransactionItemType.Sale_Order;
            }
            else
            {
                DXMessageBox.Show("!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void BtnCreateTask_Click(object sender, RoutedEventArgs e)
        {
            TaskRepo taskRepo = new TaskRepo();
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                SaleOrderRepo orderRepo = new SaleOrderRepo();
                var SO = orderRepo.get(transactionId);

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Sale Order is in Pending for closing stage!");
                    return;
                }

                if (SO.saleOrderStatus.isActive == false && SO.PendingForClosing == false)
                {
                    DXMessageBox.Show("This Sale Order is Already closed!");
                    return;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Add User Task for Sale Order") != null)
                {
                    //var task = taskRepo.GetSOTask(transactionId);
                    //if (task != null)
                    //{
                    //    DXMessageBox.Show("This Sale Order already has a Task Created!");
                    //    return;
                    //}
                    ucTaskAdd taskAdd = new ucTaskAdd();
                    taskAdd.transactionType = TransactionItemType.Sale_Order;
                    taskAdd.transactionId = transactionId;
                    taskAdd.editFlag = false;
                    taskAdd.taskId = 0;
                    Window win = new Window();
                    win.Content = taskAdd;
                    win.Show();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Add Task for Sale Order!");
                }
            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Purchase_Order)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add User Task for Purchase Order") != null)
                {
                    //var task = taskRepo.GetPOTask(transactionId);
                    //if (task != null)
                    //{
                    //    DXMessageBox.Show("This Purchase Order already has a Task created!");
                    //    return;
                    //}
                    ucTaskAdd taskAdd = new ucTaskAdd();
                    taskAdd.transactionType = TransactionItemType.Purchase_Order;
                    taskAdd.transactionId = transactionId;
                    taskAdd.editFlag = false;
                    taskAdd.taskId = 0;
                    Window win = new Window();
                    win.Content = taskAdd;
                    win.Show();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Add Task for Purchase Order!");
                }
                
            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Sale_Invoice)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add User Task for Sale Invoice") != null)
                {
                    //var task = taskRepo.GetSITask(transactionId);
                    //if (task != null)
                    //{
                    //    DXMessageBox.Show("This Sale Invoice already has a Task created!");
                    //    return;
                    //}
                    ucTaskAdd taskAdd = new ucTaskAdd();
                    taskAdd.transactionType = TransactionItemType.Sale_Invoice;
                    taskAdd.transactionId = transactionId;
                    taskAdd.editFlag = false;
                    taskAdd.taskId = 0;
                    Window win = new Window();
                    win.Content = taskAdd;
                    win.Show();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Add Task for Sale Invoice!");
                }
               
            }
            //else if (transactionId != 0 && transactionType == TransactionItemType.Inquiry)
            //{
            //    var task = taskRepo.GetInquiryTask(transactionId);
            //    if (task != null)
            //    {
            //        DXMessageBox.Show("This Inquiry already has a Task created!");
            //        return;
            //    }
            //    ucTaskAdd taskAdd = new ucTaskAdd();
            //    taskAdd.transactionType = TransactionItemType.Inquiry;
            //    taskAdd.transactionId = transactionId;
            //    taskAdd.editFlag = false;
            //    taskAdd.taskId = 0;
            //    Window win = new Window();
            //    win.Content = taskAdd;
            //    win.Show();
            //}
            else if (transactionId != 0 && transactionType == TransactionItemType.Offer)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add User Task for Offer") != null)
                {
                    //var task = taskRepo.GetOfferTask(transactionId);
                    //if (task != null)
                    //{
                    //    DXMessageBox.Show("This Offer already has a Task created!");
                    //    return;
                    //}
                    ucTaskAdd taskAdd = new ucTaskAdd();
                    taskAdd.transactionType = TransactionItemType.Offer;
                    taskAdd.transactionId = transactionId;
                    taskAdd.editFlag = false;
                    taskAdd.taskId = 0;
                    Window win = new Window();
                    win.Content = taskAdd;
                    win.Show();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to Add Task for Offer!");
                }
                
            }
        }

        private void btnCreateModuleContract_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Offer)
            {
                Offerss.ucStatuschange.offerid = transactionId;
                Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                statusChange.Owner = this;
                statusChange.ShowDialog();
                usercongrid.Children.Clear();
                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd ucModuleContractAdd = new ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd();
                usercongrid.Children.Add(ucModuleContractAdd);
                ZAS_ERP.Procurementss.ModuleContract.UserControls.ucModuleContractAdd.offerid = transactionId;
                Procurementss.Offerss.ucOfferAdd.editoffer = 0;
                Procurementss.Offerss.ucOfferAdd.offerid = 0;
            }
            else
            {
                MessageBox.Show("Create or load Offer First to create Module Contract!", "Offer not Found");
            }
        }
private void btnCreateSaleReceipt_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCreateLoan_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Loans Advances") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmVendorBillLoan frmVendorBillLoan = new ucFrmVendorBillLoan();

                    frmVendorBillLoan.editFlag = false;
                    frmVendorBillLoan.saleOrderId = transactionId;

                    enterPaymentWin.Content = frmVendorBillLoan;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Vendor Bill Loans Advance!");
                }
            }
            else
            {
                MessageBox.Show("Create or load Sale Order First to create Loan!", "Sale Order not Found");
            }
        }

        private void btnCreateAdvance_Click(object sender, RoutedEventArgs e)
        {
            if (transactionId != 0 && transactionType == TransactionItemType.Sale_Order)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Loans Advances") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();

                    frmBillLoansAdvance.editFlag = false;
                    frmBillLoansAdvance.saleOrderId = transactionId;

                    enterPaymentWin.Content = frmBillLoansAdvance;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Vendor Bill Advance!");
                }

            }
            else if (transactionId != 0 && transactionType == TransactionItemType.Purchase_Order)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Loans Advances") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();

                    frmBillLoansAdvance.editFlag = false;
                    frmBillLoansAdvance.purchaseOrderId = transactionId;

                    enterPaymentWin.Content = frmBillLoansAdvance;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Vendor Bill Advance!");
                }

            }
            else
            {
                MessageBox.Show("Create or load Sale Order First to create Loan!", "Sale Order not Found");
            }
        }
    }
}
