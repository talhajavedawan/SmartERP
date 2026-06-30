using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
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
using ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls;
using ZAS_ERP.AssetRentalss.RentalOrderss.UserControls;
using ZAS_ERP.AssetRentalss.UserControls;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.RentalReceipts;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.AssetRentalss
{
    /// <summary>
    /// Interaction logic for ucTrackingGrid.xaml
    /// </summary>
    public partial class ucTrackingGrid : UserControl
    {
        public TransactionItemType? itemType;
        public int itemId = 0;
        public ucTrackingGrid()
        {
            InitializeComponent();
        }

        public ucTrackingGrid(int Id, TransactionItemType type)
        {
            InitializeComponent();

            Id = itemId;
            itemType = type;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GellAllOrdersTracking();
        }

        public void GellAllOrdersTracking()
        {

            //saleOrderRepo = new SaleOrderRepo();
            if (itemId > 0 && itemType != null) 
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(itemId, itemType.Value);
            }

        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
           

            if (grid.SelectedItem != null)
            {

                var item = grid.SelectedItem as AllOrdersView;

                if (item.transactionType == TransactionItemType.RentalInvoice)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Invoices") != null)
                    {
                        RentalInvoiceRepo rentalInvoiceRepo = new RentalInvoiceRepo();
                        var selectedStl = rentalInvoiceRepo.GetRentalInvoice(item.Id);
                        ucRentalInvoiceAdd ucRentalInvoice = new ucRentalInvoiceAdd();
                        ucRentalInvoice.editFlag = true;
                        ucRentalInvoice.invoiceId = selectedStl.Id;

                        Window win = new Window();
                        win.Content = ucRentalInvoice;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Assets");
                    }
                }
                else if(item.transactionType == TransactionItemType.RentalOrder)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Orders") != null)
                    {
                        RentalOrderRepo rentalOrderRepo = new RentalOrderRepo();
                        var selectedStl = rentalOrderRepo.GetRentalOrder(item.Id);
                        ucRentalOrderAdd ucRentalOrder = new ucRentalOrderAdd();
                        ucRentalOrder.editFlag = true;
                        ucRentalOrder.orderId = selectedStl.Id;

                        Window win = new Window();
                        win.Content = ucRentalOrder;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Assets");
                    }
                }
                else if(item.transactionType == TransactionItemType.RentalContract)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Contracts") != null)
                    {
                        RentalContractRepo rentalContractRepo = new RentalContractRepo();
                        var selectedStl = rentalContractRepo.GetRentalContract(item.Id);
                        ucFrmRentalContractAdd frmRentalContractAdd = new ucFrmRentalContractAdd();
                        frmRentalContractAdd.editFlag = true;
                        frmRentalContractAdd.contractId = selectedStl.Id;

                        Window win = new Window();
                        win.Content = frmRentalContractAdd;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Assets");
                    }
                }
                else if (item.transactionType == TransactionItemType.AssetRental)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Assets") != null)
                    {
                        AssetRentalRepo tenantRentalRepo = new AssetRentalRepo();
                        var selectedStl = tenantRentalRepo.GetAssetRental(item.Id);
                        ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
                        frmAddAssetRental.editFlag = true;
                        frmAddAssetRental.assetId = selectedStl.Id;

                        Window win = new Window();
                        win.Content = frmAddAssetRental;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Assets");
                    }
                }
                else if (item.transactionType == TransactionItemType.TenantRental)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Tenants") != null)
                    {
                        TenantRentalRepo tenantRentalRepo = new TenantRentalRepo();
                        var selectedStl = tenantRentalRepo.GetTenantRentall(item.Id);
                        ucFrmTenantRentalAdd frmTenantRentalAdd = new ucFrmTenantRentalAdd();
                        frmTenantRentalAdd.editFlag = true;
                        frmTenantRentalAdd.tenantId = selectedStl.Id;

                        Window win = new Window();
                        win.Content = frmTenantRentalAdd;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Tenants");
                    }
                }
                else if (item.transactionType == TransactionItemType.STL)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                    {
                        STLRepo sTLRepo = new STLRepo();

                        var selectedStl = sTLRepo.Get(Convert.ToInt32(item.Id));
                        winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                        stl.stl = selectedStl;
                        stl.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else if (item.transactionType == TransactionItemType.InterBank_Transfer)
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(item.Id);

                            if (selectedBankTransfer != null)
                            {
                                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                CompanyRepo compRepo = new CompanyRepo();
                                bankTransRepo = new InterBankTransRepo();

                                //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

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
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

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
                }
                else
                {
                    if (item.transactionType == TransactionItemType.Sale_Receipt)
                    {
                        GrdSaleReceiptListLoad(item.Id);
                        return;
                    }

                    if (item.transactionType == TransactionItemType.Tasks)
                    {
                        ucTaskAdd taskAdd = new ucTaskAdd();
                        taskAdd.taskId = Convert.ToInt32(item.Id);
                        taskAdd.editFlag = true;
                        Window win = new Window();
                        win.Content = taskAdd;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.LoansAdvances)
                    {
                        AdvanceRepo advanceRepo = new AdvanceRepo();


                        var advance = advanceRepo.GetLoansAdvance(item.Id);


                        switch (advance.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                    frmLoansAdvances.loansAdvanceId = advance.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                    frmBillLoansAdvance.loansAdvanceId = advance.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }


                        return;
                    }

                    if (item.transactionType == TransactionItemType.TargetReward)
                    {
                        ucFrmBasicTargetRewards frmTargetRewards = new ucFrmBasicTargetRewards();
                        frmTargetRewards.rewardId = Convert.ToInt32(item.Id);
                        //frmTargetRewards.editFlag = true;
                        Window win = new Window();
                        win.Content = frmTargetRewards;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.transactionType == TransactionItemType.Admin_Bill)
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(item.Id);

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId);

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;

                                    frmBill.WindowState = WindowState.Maximized;

                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId;
                                frmBill.Content = frmBillAdd;

                                frmBill.WindowState = WindowState.Maximized;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.transactionType == TransactionItemType.Payments)
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(item.Id);



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = payment.transactionGroupId;
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
                                    frmLApayment.groupId = payment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Target_Reward:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRpayment.editFlag = true;
                                        frmTRpayment.groupId = payment.transactionGroupId;
                                        frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                        frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRpayment.editFlag = true;
                                    frmTRpayment.groupId = payment.transactionGroupId;
                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRpayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
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
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }
                        return;
                    }
                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.transactionType.ToString()), item.Id);
                    procurmentPanele.Show();
                }
            }

        }


        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt != null)
                {
                    if (saleReceipt.receiptType == ReceiptType.Rental_Receipt)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Receipts") != null)
                        {
                            ucFrmRentalReceiptAdd frmRentalReceipt = new ucFrmRentalReceiptAdd();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmRentalReceipt.editFlag = true;
                                    frmRentalReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmRentalReceipt.receiptId = saleReceipt.Id;
                                    frmPiPaymentWindow.Content = frmRentalReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Rental Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmRentalReceipt.editFlag = true;
                                frmRentalReceipt.groupId = saleReceipt.transactionGroupId;
                                frmRentalReceipt.receiptId = saleReceipt.Id;
                                frmPiPaymentWindow.Content = frmRentalReceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Rental Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Rental Receipts!");
                        }
                        return;
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Customer_Credits)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credit Receipts") != null)
                        {
                            ucFrmCustomerCreditReceipt frmReceipt = new ucFrmCustomerCreditReceipt();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmReceipt.receiptId = saleReceipt.Id;
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
                                frmReceipt.receiptId = saleReceipt.Id;
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
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Direct_Receipt)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                        {
                            if (saleReceipt.payment != null)
                            {
                                ucFrmDirectReceiptPayment frmLAreceipt = new ucFrmDirectReceiptPayment();
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
                                        frmPiPaymentWindow.Title = "Direct Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                            }
                            else
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


                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Direct Receipts!");
                        }
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Loans_Advances)
                    {

                        switch (saleReceipt.loansAdvance.advanceTemplate)
                        {
                            case LoansAdvanceTemplate.Loan:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                                {
                                    ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
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
                                break;
                            default:
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
                                break;
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



                //if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                //{
                //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                //    {
                //        ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                //        //paymentRepo = new PaymentRepo();
                //        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                //        Window frmPiPaymentWindow = new Window();
                //        if (saleReceipt.saleReceiptStatus.isActive == false)
                //        {
                //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                //            {
                //                frmLAreceipt.editFlag = true;
                //                frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                //                frmPiPaymentWindow.Content = frmLAreceipt;
                //                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                //                frmPiPaymentWindow.Title = "Sale Receipts";
                //                frmPiPaymentWindow.Show();
                //            }
                //            else
                //            {
                //                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                //                return;
                //            }
                //        }
                //        else
                //        {
                //            frmLAreceipt.editFlag = true;
                //            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                //            frmPiPaymentWindow.Content = frmLAreceipt;
                //            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                //            frmPiPaymentWindow.Title = "Sale Receipts";
                //            frmPiPaymentWindow.Show();
                //        }

                //    }
                //    else
                //    {
                //        DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                //    }
                //}
                //else
                //{
                //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                //    {
                //        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                //        updateSaleReceiptObj.saveEditFlag = 1;


                //        if (saleReceipt == null)
                //        {
                //            return;
                //        }

                //        if (saleReceipt.saleReceiptStatus != null)
                //        {
                //            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                //            updateSaleReceiptObj.selectedStatus = status;
                //        }

                //        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                //        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                //        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                //        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                //        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                //        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                //        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                //        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;
                //        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
                //        updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                //        updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                //        updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                //        updateSaleReceiptObj.enterReceiptWindowFlag = true;

                //        if (saleReceipt.CreditedDate != null)
                //            updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                //        if (saleReceipt.DepositedDate != null)
                //            updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                //        if (saleReceipt.InstrumentDate != null)
                //            updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                //        if (saleReceipt.InstrumentNo != null)
                //            updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                //        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                //        //Load_Receipts();
                //    }
                //    else
                //    {
                //        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                //        return;
                //    }
                //}



            }
            catch
            {

            }
        }

     
    }
}
