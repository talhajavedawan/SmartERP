using DevExpress.Xpf.Core;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Bankings.InterCompanyBankTransfer;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Employee;
using ZAS_ERP.Leaves;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.ToDoTasks.Taskks.UserControls.TaxTasks;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows;
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.AssetRentalss.UserControls;
using ZAS_ERP.AssetRentalss.RentalOrderss.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.FilesAndDocss.Documentss;
using ZAS_ERP.SaleOrderFolder.UserControls.RentalReceipts;
using ZAS_ERP.Memos.PerformanceReview;
using ERP_BL.Procurements.Memos;

namespace ZAS_ERP.Commentss
{
    /// <summary>
    /// Interaction logic for ucCommentsGrid.xaml
    /// </summary>
    public partial class ucCommentsGrid : UserControl
    {
        List<cmbitem> cmbitems = new List<cmbitem>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        List<Notification> notifications = new List<Notification>();
        //public bool? loadSentNotification = false;
        public string notificationsLoaded = "";
        public int unReadCount { get; set; }
        public int pendingCount { get; set; }
        public int counter { get; set; }
        public int selectedCounter { get; set; }
        public MainWindow myParent = null;
        UsersRepo UsersRepo = new UsersRepo();
        Notification notification = new Notification();
        bool? glow = null;
        int checkNotificationIdGlow = 0;

        public bool urgentNotification = false;
        public ucCommentsGrid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            cmbitems = new List<cmbitem>();
            List<NotificationFlag> allNotificationFlags = new List<NotificationFlag>();
            notificationsRepo = new NotificationsRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Notification Flags") != null)
                allNotificationFlags = notificationsRepo.GetAllNotificationFlags();
            else
                allNotificationFlags = notificationsRepo.GetAllOpenFlags();

            if (allNotificationFlags != null)
            {
                cmbitems.Add
                    (new cmbitem()
                    {
                        name = "-- Clear flag --",
                        id = 0,
                        
                        fcolor = "#000000"
                    }
                    );
                Parallel.ForEach(allNotificationFlags, delegate (NotificationFlag flag) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {

                    cmbitems.Add
                    (new cmbitem()
                    {
                        name = flag.Flag,
                        id = flag.Id,
                        bcolor = flag.backcolor,
                        fcolor = "#FF000000"
                    });


                });
                cmbNotificationFlag.ItemsSource = cmbitems;
            }

            if(urgentNotification == false)
            {
                //LoadNotifications();
                LoadAllUnreadNotifications();
            }
            else
            {
                LoadAllUrgentNotifications();
            }

            //loadSentNotification = null;
            notificationsLoaded = "Unread";
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdNotifications);
            //ThemeManager.SetThemeName(this, "Office2010Silver");
            getCountAllUnreadNotifications();
        }
        private void TableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (grdNotifications.GetFocusedRow() != null)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                if (notification != null && notification.TransactionId != 0 && notification.TransactionType != 0)
                {
                    DXWindow win = new DXWindow();
                    switch (notification.TransactionType)
                    {
                        case TransactionItemType.PerformanceReview:
                            MemoRepo memoRepo = new MemoRepo();
                            var review = memoRepo.GetPerformanceReview(notification.TransactionId);

                            if (review != null)
                            {
                                var editWindow = new PerformanceReviewWindow();
                                editWindow.reviewIdToEdit = notification.TransactionId;
                                editWindow.editFlag = true;
                                editWindow.memoId = review.MemoId.Value;
                                editWindow.Show();
                            }
                            break;
                        case TransactionItemType.AssetRental:
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Assets") != null)
                            {


                                ucFrmAddAssetRental frmAddAssetRentalss = new ucFrmAddAssetRental();
                                frmAddAssetRentalss.editFlag = true;
                                frmAddAssetRentalss.assetId = notification.TransactionId;

                                win.Content = frmAddAssetRentalss;
                                win.WindowState = WindowState.Maximized;
                                win.Show();
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to view Asset!");
                            }
                            break;
                        case TransactionItemType.RentalOrder:
                            ucRentalOrderAdd rentalOrderAdd = new ucRentalOrderAdd();
                            rentalOrderAdd.editFlag = true;
                            rentalOrderAdd.orderId = notification.TransactionId;

                            win.Content = rentalOrderAdd;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                            break;
                        case TransactionItemType.RentalContract:
                            ucFrmRentalContractAdd frmAddAssetRental = new ucFrmRentalContractAdd();
                            frmAddAssetRental.editFlag = true;
                            frmAddAssetRental.contractId = notification.TransactionId;

                            win.Content = frmAddAssetRental;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                            break;
                        case TransactionItemType.TargetReward:
                            ToDoTaskRepo toDoTaskRepo = new ToDoTaskRepo();
                            var reward = toDoTaskRepo.GetTargetReward(notification.TransactionId);
                            if (reward.isApplied == false)
                            {
                                ucFrmBasicTargetRewards ucFrmTarget = new ucFrmBasicTargetRewards();
                                ucFrmTarget.rewardId = reward.Id;

                                DXWindow win1 = new DXWindow();
                                win1.WindowState = WindowState.Maximized;
                                win1.Content = ucFrmTarget;
                                win1.Title = "Target Reward";
                                win1.Show();
                            }
                            else if (reward.isApplied == true)
                            {
                                ucFrmTargetRewardAdd ucFrmTarget = new ucFrmTargetRewardAdd();
                                ucFrmTarget.rewardId = reward.Id;

                                DXWindow win1 = new DXWindow();
                                win1.WindowState = WindowState.Maximized;
                                win1.Content = ucFrmTarget;
                                win1.Title = "Target Reward";
                                win1.Show();
                            }
                            
                            break;
                        case TransactionItemType.Tasks:
                            TaskRepo taskRepo = new TaskRepo();
                            var task = taskRepo.GetTask(notification.TransactionId);

                            if(task.taskTemplate == TaskTemplate.Tax_Record)
                            {
                                ucTaxTaskAdd taskAdd = new ucTaxTaskAdd();
                                taskAdd.editFlag = true;
                                taskAdd.taskId = notification.TransactionId;
                                //taskAdd.transactionType = selectedRow.transactionType;
                                //taskAdd.transactionId = selectedRow.transactionId;
                                win.Content = taskAdd;
                                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                win.WindowState = WindowState.Maximized;
                                win.Show();
                            }
                            else
                            {
                                ucTaskAdd taskAdd = new ucTaskAdd();
                                taskAdd.taskId = notification.TransactionId;
                                taskAdd.editFlag = true;
                                win.Content = taskAdd;
                                win.WindowState = WindowState.Maximized;
                                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                win.Show();
                            }
                            
                            break;
                        case TransactionItemType.LoansAdvances:

                            AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                            LoansAdvance loansAdvance = new LoansAdvance();
                            loansAdvance = loansAdvanceRepo.GetLoansAdvance(notification.TransactionId);

                            switch (loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Advance:
                                    switch (loansAdvance.loansAdvanceType)
                                    {
                                        case LoansAdvanceType.Admin_Bill:
                                            ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                            frmLoansAdvances.loansAdvanceId = notification.TransactionId;
                                            frmLoansAdvances.editFlag = true;

                                            win.Content = frmLoansAdvances;
                                            win.WindowState = WindowState.Maximized;
                                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            win.Show();
                                            break;
                                        case LoansAdvanceType.Vendor_Bill:
                                            ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                            frmBillLoansAdvance.loansAdvanceId = notification.TransactionId;
                                            frmBillLoansAdvance.editFlag = true;

                                            win.Content = frmBillLoansAdvance;
                                            win.WindowState = WindowState.Maximized;
                                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                            win.Show();
                                            break;
                                    }
                                    break;
                                case LoansAdvanceTemplate.Loan:
                                    switch (loansAdvance.loansAdvanceType)
                                    {
                                        case LoansAdvanceType.Admin_Bill:
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                            {
                                                ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                                frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                                frmCompanyLoan.editFlag = true;
                                                Window wind = new Window();
                                                wind.Content = frmCompanyLoan;
                                                wind.WindowState = WindowState.Maximized;
                                                wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                                wind.Show();
                                            }
                                            else
                                            {
                                                DXMessageBox.Show("Permission denied!");
                                            }
                                            break;
                                        case LoansAdvanceType.Vendor_Bill:
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                            {
                                                ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                                frmCompanyLoan.loansAdvanceId = loansAdvance.Id;
                                                frmCompanyLoan.editFlag = true;
                                                Window wind = new Window();
                                                wind.Content = frmCompanyLoan;
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
                                    break;
                            }
                            break;
                                    
                        case TransactionItemType.ToDo_Task:
                            ucFrmAddTask frmAddTask = new ucFrmAddTask();
                            frmAddTask.taskId = notification.TransactionId;
                            frmAddTask.editFlag = true;

                            win.Content = frmAddTask;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                            break;
                        case TransactionItemType.TargetStep:
                            ucBasicTargetForm ucBasicTarget = new ucBasicTargetForm();
                            ucBasicTarget.stepId = notification.TransactionId;
                            ucBasicTarget.editFlag = true;
                            win.Content = ucBasicTarget;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                            break;
                        case TransactionItemType.Payments:
                            PaymentRepo paymentRepo = new PaymentRepo();
                            Payment pymnt = new Payment();
                            pymnt = paymentRepo.GetPaymentByGroupId(notification.TransactionId);

                            if (pymnt != null)
                            {
                                switch (pymnt.transactionType)
                                {
                                    case PaymentTransactionType.Admin_Bills:

                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                                        {
                                            ucFrmPayments frmPayments = new ucFrmPayments();

                                            //payments = paymentRepo.GetAdminBillPaymentsByGroupId(notification.TransactionId);


                                            if (pymnt.Status.isActive == false)
                                            {
                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                {
                                                    frmPayments.editFlag = true;
                                                    frmPayments.groupId = pymnt.transactionGroupId;
                                                    frmPayments.frmPaymentWindow.Content = frmPayments;

                                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;

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
                                                frmPayments.groupId = pymnt.transactionGroupId;
                                                frmPayments.frmPaymentWindow.Content = frmPayments;

                                                frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;

                                                frmPayments.frmPaymentWindow.Title = "Payments";
                                                frmPayments.frmPaymentWindow.Show();
                                            }

                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission required to View Admin Bill Payment!");
                                        }

                                        break;

                                    case PaymentTransactionType.Vendor_Bills:

                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                                        {
                                            ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                            //payments = paymentRepo.GetVendorBillPaymentsByGroupId(notification.TransactionId);

                                            if (pymnt != null)
                                            {
                                                if (pymnt.Status.isActive == false)
                                                {
                                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                    {
                                                        frmBillPayments.editFlag = true;
                                                        frmBillPayments.groupId = pymnt.transactionGroupId;
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
                                                    frmBillPayments.groupId = pymnt.transactionGroupId;
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
                                            ucFrmPInvoicePaymentAdd frmPIpayments = new ucFrmPInvoicePaymentAdd();

                                            //payments = paymentRepo.GetPIpaymentsByGroupId(notification.TransactionId);
                                            //frmPIpayments.payments = new List<Payment>();
                                            //frmPIpayments.payments = payments; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                            if (pymnt != null)
                                            {
                                                if (pymnt.Status.isActive == false)
                                                {
                                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                    {
                                                        frmPIpayments.editFlag = true;
                                                        frmPIpayments.groupId = pymnt.transactionGroupId;
                                                        frmPIpayments.frmPiPaymentWindow.Content = frmPIpayments;

                                                        frmPIpayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                        frmPIpayments.frmPiPaymentWindow.Title = "Payments";
                                                        frmPIpayments.frmPiPaymentWindow.Show();
                                                    }
                                                    else
                                                    {
                                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    frmPIpayments.editFlag = true;
                                                    frmPIpayments.groupId = pymnt.transactionGroupId;
                                                    frmPIpayments.frmPiPaymentWindow.Content = frmPIpayments;
                                                    frmPIpayments.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                    frmPIpayments.frmPiPaymentWindow.Title = "Payments";
                                                    frmPIpayments.frmPiPaymentWindow.Show();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission required to View Vendor Bill Payment!");
                                        }
                                        break;
                                    case PaymentTransactionType.Loans_Advances:
                                        switch (pymnt.loansAdvance.advanceTemplate)
                                        {
                                            case LoansAdvanceTemplate.Loan:
                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                                {
                                                    ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
                                                    //paymentRepo = new PaymentRepo();
                                                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                                    if (pymnt != null)
                                                    {
                                                        if (pymnt.Status.isActive == false)
                                                        {
                                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                            {
                                                                frmLApayment.editFlag = true;
                                                                frmLApayment.groupId = pymnt.transactionGroupId;
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
                                                            frmLApayment.groupId = pymnt.transactionGroupId;
                                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                            frmLApayment.frmPiPaymentWindow.Show();
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    DXMessageBox.Show("Permission required to View Loans and Advance Payment!");
                                                }
                                                break;
                                            default:
                                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                                {
                                                    ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                                                    //paymentRepo = new PaymentRepo();
                                                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                                    if (pymnt != null)
                                                    {
                                                        if (pymnt.Status.isActive == false)
                                                        {
                                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                            {
                                                                frmLApayment.editFlag = true;
                                                                frmLApayment.groupId = pymnt.transactionGroupId;
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
                                                            frmLApayment.groupId = pymnt.transactionGroupId;
                                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                            frmLApayment.frmPiPaymentWindow.Show();
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    DXMessageBox.Show("Permission required to View Loans and Advance Payment!");
                                                }
                                                break;
                                        }
                                        
                                        break;

                                    case PaymentTransactionType.Target_Reward:

                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                                        {
                                            ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                                            //paymentRepo = new PaymentRepo();
                                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                            if (pymnt != null)
                                            {
                                                if (pymnt.Status.isActive == false)
                                                {
                                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                                    {
                                                        frmTRpayment.editFlag = true;
                                                        frmTRpayment.groupId = pymnt.transactionGroupId;
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
                                                    frmTRpayment.groupId = pymnt.transactionGroupId;
                                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                                    frmTRpayment.frmPiPaymentWindow.Show();
                                                }
                                            }

                                        }
                                        else
                                        {
                                            DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                        }
                                        break;
                                }
                            }
                            break;
                        case TransactionItemType.InterCompanyBank_Transfer:
                            ucInterCompBankTransRegister bankTransRegister = new ucInterCompBankTransRegister();
                            bankTransRegister.update_interCompanyBankTransfer(notification.TransactionId);
                            break;
                        case TransactionItemType.Admin_Bill:
                            ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                            frmBillAdd.editFlag = true;
                            frmBillAdd.groupId = notification.TransactionId;
                            //ucBillList billList = new ucBillList();
                            win.WindowState = WindowState.Maximized;
                            win.Title = "Update Bills";
                            win.Content = frmBillAdd;
                            win.Show();
                            break;
                        case TransactionItemType.InterBank_Transfer:
                            ucBankTransferRegister ucBankTransfer = new ucBankTransferRegister();
                            ucBankTransfer.update_interBankTransfer(notification.TransactionId);
                            break;
                        case TransactionItemType.Sale_Receipt:
                            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                            var saleReceipt = receiptRepo.GetSaleReceipt(notification.TransactionId);
                            //if (saleReceipt.receiptType == ReceiptType.Direct_Receipt)
                            //{
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
                                                        frmLAreceipt.receiptId = saleReceipt.Id;
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
                                                    frmLAreceipt.receiptId = saleReceipt.Id;
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
                                                        frmLAreceipt.receiptId = saleReceipt.Id;
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
                                                    frmLAreceipt.receiptId = saleReceipt.Id;
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
                            //}
                            //else
                            //{
                            //    ucSaleReceiptList ucSaleReceiptList = new ucSaleReceiptList();
                            //    ucSaleReceiptList.Update_SaleReceipt(notification.TransactionId);
                            //}
                            break;
                        case TransactionItemType.Employee:
                            ucEmployeeInfo ucEmployeeInfo = new ucEmployeeInfo();
                            ucEmployeeInfo.Update_Employee(notification.TransactionId);
                            break;
                        case TransactionItemType.Leave:
                            frmLeaveApplication uc = new frmLeaveApplication();
                            uc.isEdit = true;
                            uc.isNotif = true;
                            uc.leaveIdNotif = notification.TransactionId;
                            uc.leaveId = notification.TransactionId;
                            //frmLeaveApplication frm = new frmLeaveApplication();
                            win.Title = "Leave Application Form";
                            win.Content = uc;
                            win.ShowDialog();
                            break;
                        case TransactionItemType.CostCenter:
                            try
                            {
                                List<ViewInfo> views = new List<ViewInfo>();
                                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                                var saleOrder = saleOrderRepo.GetSaleOrderbyCostSheetId(notification.TransactionId);
                                string inco = "";
                                string paymentterm = "";
                                if (saleOrder.paymentTerm != null)
                                {
                                    paymentterm = saleOrder.paymentTerm.term;
                                }
                                if (saleOrder.incoterm != null)
                                {
                                    inco = saleOrder.incoterm.term;
                                }
                                var soCreationDate = (DateTime)saleOrder.CreationDate;
                                var totalCFR = saleOrder.totalCFRValue.ToString();
                                var soDeliveryDate = (DateTime)saleOrder.deliveryDate;
                                var soDate = (DateTime)saleOrder.saleOrderDate;
                                var financeRef = saleOrder.FinanceRefrenceNo;
                                var salesRef = saleOrder.SalesReferenceNo;

                                frmCostSheet frmCostSheet = new frmCostSheet(
                                    saleOrder,
                                    saleOrder.FinanceRefrenceNo,
                                    saleOrder.SalesReferenceNo,
                                    saleOrder.department.DeptName,
                                    saleOrder.customerCompany.company.CompanyName,
                                    saleOrder.currency.Symbol,
                                    totalCFR,
                                    inco,
                                    soCreationDate.ToString(),
                                    saleOrder.paymentTerm.term,
                                    saleOrder.maker,
                                    saleOrder.origin,
                                    views,
                                    saleOrder.saleOrdertype,
                                    saleOrder.principal.company.CompanyName,
                                    saleOrder.packing,
                                    soDeliveryDate,
                                    saleOrder.Warranty.name,
                                    saleOrder.referenceNo,
                                    soDate.ToString(),
                                    saleOrder.isApproved,
                                    saleOrder.isReApproved);

                                frmCostSheet.ShowDialog();
                                //if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Principal)
                                //{
                                //    if (frmCostSheet.costSheet != null)
                                //    {
                                //        saleOrder.CostSheet = frmCostSheet.costSheet;
                                //        txtCommision.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();

                                //    }
                                //}
                                //else
                                {
                                    if (frmCostSheet.costSheet != null)
                                    {
                                        //saleOrder.CostSheet = frmCostSheet.costSheet;
                                        //txtBudgetMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                                        //txtRevisedMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();

                                        //txtActualMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) != (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin).ToString() : "0";
                                        if ((saleOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                                            saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                                        saleOrder.isReApproved = frmCostSheet.isReApproved;
                                        //saleOrder.SystemMargin = calculateSystemMargins();
                                        saleOrder.SystemMargin = Convert.ToDecimal(frmCostSheet.txtTotalSystemMargin.Text);
                                        saleOrderRepo.update(saleOrder);
                                        //DXMessageBox.Show("Sale Order is Updated successfully","Information", MessageBoxButton.OK, MessageBoxImage.Information);


                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                DXMessageBox.Show(ex.ToString());
                            }
                            break;
                        case TransactionItemType.Budget:
                            {
                                try
                                {
                                    Procurementss.Budget.frmBudgetAdd frmBudget = new ZAS_ERP.Procurementss.Budget.frmBudgetAdd(notification.TransactionId);
                                    frmBudget.Show();
                                }
                                catch
                                {

                                }
                                break;
                            }
                        case TransactionItemType.STL:
                            {
                                try
                                {
                                    STLRepo sTLRepo = new STLRepo();
                                    var selectedStl = sTLRepo.Get(Convert.ToInt32(notification.TransactionId));
                                    winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                                    stl.Show();
                                }
                                catch
                                {

                                }
                                break;
                            }
                        case TransactionItemType.InventoryAdjustment:
                            {
                                
                                try
                                {
                                    winfrmAdjustInventory adjustment = new winfrmAdjustInventory();
                                    adjustment.OrderId = (Convert.ToInt32(notification.TransactionId));
                                    adjustment.editOrder = 1;
                                    adjustment.Show();
                                    
                                }
                                catch (Exception)
                                {
                                }



                                break;
                            }
                        case TransactionItemType.Document:
                            {

                                try
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Document") != null)
                                    {
                                        ucFrmDocumentAdd frmDocuments = new ucFrmDocumentAdd();
                                        frmDocuments.groupId = (Convert.ToInt32(notification.TransactionId));
                                        frmDocuments.editFlag = true;
                                        Window docWin = new Window();
                                        docWin.Content = frmDocuments;
                                        docWin.WindowState = WindowState.Maximized;
                                        docWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                        docWin.Show();
                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission denied!");
                                    }

                                }
                                catch (Exception)
                                {
                                }



                                break;
                            }

                        default:
                            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(notification.TransactionType, notification.TransactionId);
                            procurmentPanel.ShowActivated = true;

                            procurmentPanel.Show();
                            break;

                    }
                    


                    if (notification.isRead == false && (MainWindow.currentUserid != notification.SendingUserId || (MainWindow.currentUserid == notification.SendingUserId && notification.SendingUserId == notification.UserId)))
                    {
                        notificationsRepo.MarkasRead(notification.Id);
                    }
                }
            }
        }

        private void MbtnRefreshNotification_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ReloadNotificationData();
                ChartofAccountsRepo chartOfAccountRepo = new ChartofAccountsRepo();
                chartOfAccountRepo.FixNullTransactions();
                PettyCashRepo pettyCashRepo = new PettyCashRepo();
                pettyCashRepo.FixNullTransactions();
                AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
                adminBillsRepo.FixNullTransactions();

                grdNotifications.RefreshData();
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Information",
                    Message = "Data has been Refresh!",
                    Type = Notifications.Wpf.NotificationType.Information
                });
                myParent.getCountAllUnreadNotifications();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                txtSelected.Text = "0";
                txtFiltered.Text = (grdNotifications.VisibleRowCount - 1).ToString();
            }
            catch (Exception ex)
            {
            }
        }

        private void ReloadNotificationData()
        {
            notificationsRepo = new NotificationsRepo();
            getCountAllUnreadNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadNotifications();
                    break;
                case "Sent":
                    LoadSentNotifications();
                    break;
                case "Unread":
                    LoadAllUnreadNotifications();
                    break;
                case "Pending":
                    LoadAllPendingNotifications();
                    break;
            }
            //if (notificationsLoaded == "Inbox")
            //    LoadNotifications();
            //else if (notificationsLoaded == "Sent")
            //    LoadSentNotifications();
            //else if (notificationsLoaded == "Unread")
            //    LoadAllUnreadNotifications();
        }

        private void getCountAllUnreadNotifications()
        {
            unReadCount = notificationsRepo.CountAllUnreadNotifications(MainWindow.currentUserid);
            txtUnReadcount.Text = unReadCount.ToString();

            pendingCount = notificationsRepo.CountAllPendingNotifications(MainWindow.currentUserid);
            txtPendingCount.Text = pendingCount.ToString();

            //mbtnNotifications.Content = "(" + unReadCount.ToString() + ")";
        }

        private void MbtnSentNotification_Click(object sender, RoutedEventArgs e)
        {
            imgGlowOff.Visibility = Visibility.Collapsed;
            imgGlowOn.Visibility = Visibility.Collapsed;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            notificationsLoaded = "Sent";
            LoadSentNotifications();
            setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void LoadSentNotifications()
        {
            var SentNotifications = notificationsRepo.SentNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            
        }
        private void mbtnInboxNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = false;
            notificationsLoaded = "Inbox";
            LoadNotifications();
            setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }
        private void mbtnAllUnreadNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = null;
            notificationsLoaded = "Unread";
            LoadAllUnreadNotifications();
            if(button != null)
                setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }


        private void LoadAllUnreadNotifications()
        {
            var SentNotifications = notificationsRepo.AllUnreadNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        private void LoadAllUrgentNotifications()
        {
            var SentNotifications = notificationsRepo.AllUrgentNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        private void MbtnLoadMoreNotification_Click(object sender, RoutedEventArgs e)
        {
            //var selectedItems = grdNotifications.SelectedItems;
            //List<Notification> notifications = new List<Notification>();
            //foreach(var _item in selectedItems)
            //{
            //    notifications.Add(_item as Notification);
            //}
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            LoadMoreNotifications();
            grdNotifications.RefreshData();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleRowCount - 1).ToString();
        }

        private void setButtonsColor(string buttonName)
        {   
            switch (buttonName)
            {
                case "Inbox":
                    mbtnInboxNotification.Background = Brushes.LightBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Unread":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.LightBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Sent":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.LightBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Pending":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.LightBlue;
                    break;
            }
        }
        

        public void LoadNotifications()
        {
            try
            {
          
                
                //grdNotifications.View.FocusedRowHandle = -1;
                
                notifications = new List<Notification>();
                notificationsRepo = new NotificationsRepo();
                //System.Threading.Thread thread = new Thread(() =>
                //{
                    notifications = notificationsRepo.getUsersNotificationOrderDsc(MainWindow.currentUserid);
                    //this.Dispatcher.Invoke((Action)(() =>
                    //{
                        grdNotifications.ItemsSource = notifications;
                        counter = notifications.Count;
                        txtCounter.Text = counter.ToString();

                        SetImageforUnreadNotifications();
                    //}));
                //});
                //thread.Start();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
           
        }

        private void SetImageforUnreadNotifications()
        {
            //if (notifications.Find(x => x.isRead == false) != null)
            //{
            //    mbtnNotifications.Glyph = mbtnNotifications.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/images/NotificationOn.png", UriKind.RelativeOrAbsolute));
            //}
            //else
            //{
            //    mbtnNotifications.Glyph = mbtnNotifications.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/images/noify1.png", UriKind.RelativeOrAbsolute));
            //}
        }

        public void LoadMoreInboxNotifications()
        {
            var MoreNotifications = notificationsRepo.getMoreUsersNotificationOrderByDate(datFrom.DateTime, datTo.DateTime, MainWindow.currentUserid);
            notifications = MoreNotifications;

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadMoreSentNotifications()
        {
            var MoreNotifications = notificationsRepo.getMoreSentNotificationsByDate(datFrom.DateTime, datTo.DateTime, MainWindow.currentUserid);
            notifications = MoreNotifications;

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadMoreNotifications()
        {
            //if (loadSentNotification == false)
            //    LoadMoreInboxNotifications();
            //else if (loadSentNotification == true)
            //    LoadMoreSentNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadMoreInboxNotifications();
                    break;
                case "Sent":
                    LoadMoreSentNotifications();
                    break;
              
            }
        }

        public void LoadAllNotifications()
        {
            //if (loadSentNotification == false)
            //    LoadAllInboxNotifications();
            //else if (loadSentNotification == true)
            //    LoadAllSentNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadAllInboxNotifications();
                    break;
                case "Sent":
                    LoadAllSentNotifications();
                    break;

            }
        }

        public void LoadAllInboxNotifications()
        {
            notificationsRepo = new NotificationsRepo();
            notifications = notificationsRepo.getAllInboxNotifications(MainWindow.currentUserid);
            

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadAllSentNotifications()
        {
            notificationsRepo = new NotificationsRepo();
            notifications = notificationsRepo.getAllSentNotifications(MainWindow.currentUserid);
            

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        private void LoadNotificationData(Notification notification)
        {
            if (notification != null)
            {
                if (notification.SendingUser != null)
                {
                    if (notification.SendingUser.employee != null)
                    {
                        if (notification.SendingUser.employee.person != null)
                            txtUserName.Text = notification.SendingUser.employee.person.FName + " " + notification.SendingUser.employee.person.LName;
                        txtDesignation.Text = notification.SendingUser.employee.DesignationTitle;
                    }
                    else
                    {
                        txtUserName.Text = "Nil";
                        txtDesignation.Text = "Nil";
                    }

                    //Select Status
                    if (notification.notificationFlag != null)
                    {
                        int index = 0;
                        //oldStatus = bankTransfer.interBankTransStatus;
                        foreach (var _status in cmbitems)
                        {
                            if (_status.id == notification.notificationFlag.Id)
                            {
                                cmbNotificationFlag.SelectedIndex = index;

                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        cmbNotificationFlag.SelectedIndex = -1;
                    }
                }

                txtTransactionType.Text = notification.TransactionType.ToString();
                txtComment.Text = notification.Description;

                if (notification.SendingUser != null)
                {
                    var byteImg = notification.SendingUser.employee.person.Photo;
                    if (byteImg != null)
                    {
                        var image = GetBitmapImageFromByteArray(byteImg);
                        UserImage.ImageSource = image;
                    }
                }
            }
        }

        private void LoadModuleData(Notification notification)
        {
            
            if (notification != null && notification.TransactionId != 0)
            {
                switch (notification.TransactionType)
                {
                    //case TransactionItemType.Inquiry:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadInquiryData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Offer:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadOfferData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Sale_Receipt:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadSaleReceiptData(notification.TransactionId);
                    //    break;
                    case TransactionItemType.Sale_Order:
                        grdSOamounts.Visibility = Visibility.Visible;
                        grdPOamounts.Visibility = Visibility.Collapsed;
                        LoadSaleOrderData(notification.TransactionId);
                        break;
                    //case TransactionItemType.Sale_Invoice:
                    //    grdSOamounts.Visibility = Visibility.Visible;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadSaleInvoiceData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Purchase_Order:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Visible;
                    //    LoadPurchaseOrderData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Purchase_Invoice:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadPurchaseInvoiceData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Bill:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadVendorBillData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.InterBank_Transfer:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadIBTData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Admin_Bill:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadAdminBillData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Payments:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadPaymentData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.InterCompanyBank_Transfer:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadInterCompanyBankTransferData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.ToDo_Task:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadToDoTask(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.LoansAdvances:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadLoansAdvanceData(notification.TransactionId);
                    //    break;
                    //case TransactionItemType.Tasks:
                    //    grdSOamounts.Visibility = Visibility.Collapsed;
                    //    grdPOamounts.Visibility = Visibility.Collapsed;
                    //    LoadTaskData(notification.TransactionId);
                    //    break;
                }
            }
        }

        private void LoadTaskData(int taskId)
        {
            txtModuleName.Text = "Task";
            TaskRepo taskRepo = new TaskRepo();
            var task = taskRepo.GetTask(taskId);
            if (task.isVoid == true)
                lblStage.Text = "Void";
            else if (task.Status.isActive == true)
                lblStage.Text = "Open";
            else if (task.Status.isActive == false)
                lblStage.Text = "Closed";

            if (task.creationDate != null)
                txtCreationDate.Text = task.creationDate.ToString();
            if (task.company != null)
                txtCompanyName.Text = task.company.CompanyName;

            if (task.department != null)
                txtDeptName.Text = task.department.DeptName;

            switch (task.transactionType)
            {
                case ERP_BL.Enums.TransactionItemType.Sale_Order:
                    if (task.saleOrder != null)
                    {
                        if (task.saleOrder.customerCompany != null)
                            txtCustomerName.Text = task.saleOrder.customerCompany.company.CompanyName;
                        else if (task.CustomerCompany != null)
                            txtCustomerName.Text = task.CustomerCompany.company.CompanyName;

                        if (task.saleOrder.principal != null && task.saleOrder.principal.company != null)
                            txtPrincipalName.Text = task.saleOrder.principal.company.CompanyName;
                        else
                            txtPrincipalName.Text = "";
                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.Sale_Invoice:
                    if (task.saleInvoice != null)
                    {
                        if (task.saleInvoice.customerCompany != null)
                            txtCustomerName.Text = task.saleInvoice.customerCompany.company.CompanyName;
                        else if (task.CustomerCompany != null)
                            txtCustomerName.Text = task.CustomerCompany.company.CompanyName;

                        if (task.saleInvoice.principal != null && task.saleInvoice.principal.company != null)
                            txtPrincipalName.Text = task.saleInvoice.principal.company.CompanyName;
                        else
                            txtPrincipalName.Text = "";
                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.Purchase_Order:
                    if (task.purchaseOrder != null)
                    {
                        if (task.purchaseOrder.customerCompany != null)
                            txtCustomerName.Text = task.purchaseOrder.customerCompany.company.CompanyName;
                        else if (task.CustomerCompany != null)
                            txtCustomerName.Text = task.CustomerCompany.company.CompanyName;

                        txtPrincipalName.Text = "";
                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.Offer:
                    if (task.offer != null)
                    {
                        if (task.offer.customerCompany != null)
                            txtCustomerName.Text = task.offer.customerCompany.company.CompanyName;
                        else if (task.CustomerCompany != null)
                            txtCustomerName.Text = task.CustomerCompany.company.CompanyName;

                        if (task.offer.principal != null)
                            txtPrincipalName.Text = task.offer.principal.company.CompanyName;
                        else
                            txtPrincipalName.Text = "";
                    }
                    break;
                case ERP_BL.Enums.TransactionItemType.UnDefined:
                    if (task.CustomerCompany != null)
                        txtCustomerName.Text = task.CustomerCompany.company.CompanyName;
                    break;
            }

            List<TasksStatus> taskStatuses = new List<TasksStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive User Tasks Statuses") != null)
            {
                taskStatuses = taskRepo.GetAllTaskStatuses();
            }
            else
                taskStatuses = taskRepo.GetAllActiveTaskStatuses();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (TasksStatus status in taskStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (task.Status.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == task.Status.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Task status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == task.Status.Status))];
            }
        }

        private void LoadLoansAdvanceData(int LAid)
        {
            txtModuleName.Text = "Loans Advances";
            AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
            var loansAdvance = loansAdvanceRepo.GetLoansAdvance(LAid);
            if (loansAdvance.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (loansAdvance.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (loansAdvance.isApproved == true && loansAdvance.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (loansAdvance.isApproved == true && loansAdvance.Status.isActive == false && loansAdvance.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (loansAdvance.isApproved == true && loansAdvance.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (loansAdvance.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (loansAdvance.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (loansAdvance.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }

            if (loansAdvance.CreationDate != null)
                txtCreationDate.Text = loansAdvance.CreationDate.ToString();
            if (loansAdvance.company != null)
                txtCompanyName.Text = loansAdvance.company.CompanyName;

            if (loansAdvance.department != null)
                txtDeptName.Text = loansAdvance.department.DeptName;

            txtCustomerName.Text = "N/A";
            txtPrincipalName.Text = "N/A";

            List<LoansAdvanceStatus> statuses = new List<LoansAdvanceStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Loans and Advances Statuses") != null)
            {
                statuses = loansAdvanceRepo.GetAllloansAdvanceStatuses();
            }
            else
                statuses = loansAdvanceRepo.GetAllOpenStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (LoansAdvanceStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (loansAdvance.Status.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == loansAdvance.Status.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Loans Advance status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == loansAdvance.Status.Status))];
            }
        }

        private void LoadToDoTask(int taskId)
        {
            txtModuleName.Text = "To Do Task";
            ToDoTaskRepo taskRepo = new ToDoTaskRepo();
            var task = taskRepo.GetTask(taskId);
            if (task.isVoid == true)
                lblStage.Text = "Void";
            else if (task.Status.isActive == true)
                lblStage.Text = "Open";
            else if (task.Status.isActive == false)
                lblStage.Text = "Closed";

            if (task.creationDate != null)
                txtCreationDate.Text = task.creationDate.ToString();
            if (task.taskGroup.Companies != null && task.taskGroup.Companies.Count > 0) 
            {
                string compNames = "";
                compNames = String.Join(" | ", task.taskGroup.Companies.Select(x => x.CompanyName));
                txtCompanyName.Text = compNames;
            }

            if (task.taskGroup.Departments != null && task.taskGroup.Departments.Count > 0)
            {
                string deptNames = "";
                deptNames = String.Join(" | ", task.taskGroup.Departments.Select(x => x.DeptName));
                txtDeptName.Text = deptNames;
            }

            txtCustomerName.Text = "N/A";
            txtPrincipalName.Text = "N/A";

            List<ToDoTaskStatus> statuses = new List<ToDoTaskStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Statuses in Tasks") != null)
            {
                statuses = taskRepo.GetAllTaskStatuses();
            }
            else
                statuses = taskRepo.GetAllTaskActiveStatuses();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ToDoTaskStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (task.Status.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == task.Status.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Loans Advance status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == task.Status.Status))];
            }
        }

        private void LoadInterCompanyBankTransferData(int interCompBankTransId)
        {
            txtModuleName.Text = "Inter-Company Bank Transfer";
            InterCompanyBankTransferRepo compBankTransferRepo = new InterCompanyBankTransferRepo();
            InterBankTransRepo bankTransferRepo = new InterBankTransRepo();
            var bankTransfer = compBankTransferRepo.GetInterCompanyBankTransfer(interCompBankTransId);
            if (bankTransfer.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (bankTransfer.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.interBankTransStatus.isActive == false && bankTransfer.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (bankTransfer.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (bankTransfer.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (bankTransfer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (bankTransfer.CreationDate != null)
                txtCreationDate.Text = bankTransfer.CreationDate.ToString();
            if (bankTransfer.companyFrom != null)
                txtCompanyName.Text = bankTransfer.companyFrom.CompanyName;
            if (bankTransfer.departmentFrom != null)
                txtDeptName.Text = bankTransfer.departmentFrom.DeptName;

            txtCustomerName.Text = "N/A";
            txtPrincipalName.Text = "N/A";

            List<InterBankTransferStatus> statuses = new List<InterBankTransferStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") != null)
            {
                statuses = bankTransferRepo.GetAllInterBankTransStatus();
            }
            else
                statuses = bankTransferRepo.GetAllOpenInterBankTransferStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (InterBankTransferStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (bankTransfer.interBankTransStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bankTransfer.interBankTransStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Loans Advance status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bankTransfer.interBankTransStatus.Status))];
            }
        }

        private void LoadPaymentData(int GroupId)
        {
            txtModuleName.Text = "Payment";
            PaymentRepo paymentRepo = new PaymentRepo();
            var payments = paymentRepo.GetPaymentsByGroupId(GroupId);

            if (payments != null && payments.Count > 0)
            {
                if (payments[0].isVoid == true)
                {
                    lblStage.Text = "Void";
                }
                else if (payments[0].isReApproved == false)
                {
                    lblStage.Text = "Under Re-Approval";
                }
                else if (payments[0].isApproved == true && payments[0].stage == "Closed")
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (payments[0].isApproved == true && payments[0].Status.isActive == false && payments[0].PendingForClosing != true)
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (payments[0].isApproved == true && payments[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
                else if (payments[0].isApproved == true)
                {
                    lblStage.Text = "Approved";
                }
                else if (payments[0].isApproved == false)
                {
                    lblStage.Text = "Under Approval";
                }
                else if (payments[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
            }

            if (payments[0].CreationDate != null)
                txtCreationDate.Text = payments[0].CreationDate.ToString();
            if (payments[0].company != null)
                txtCompanyName.Text = payments[0].company.CompanyName;
            if (payments[0].departments != null && payments[0].departments.Count > 0)
            {
                string deptNames = "";
                deptNames = String.Join(" | ", payments[0].departments.Select(x => x.DeptName));
                txtDeptName.Text = deptNames;
            }

            switch (payments[0].transactionType)
            {
                case PaymentTransactionType.Admin_Bills:
                    txtCustomerName.Text = "N/A";
                    txtPrincipalName.Text = "N/A";
                    break;
                case PaymentTransactionType.Loans_Advances:
                    txtCustomerName.Text = "N/A";
                    txtPrincipalName.Text = "N/A";
                    break;
                case PaymentTransactionType.Purchase_Invoice:
                    if (payments[0].purchaseInvoice != null && payments[0].purchaseInvoice.customerCompany != null && payments[0].purchaseInvoice.customerCompany.company != null)
                        txtCustomerName.Text = payments[0].purchaseInvoice.customerCompany.company.CompanyName;
                    txtPrincipalName.Text = "N/A";
                    break;
                case PaymentTransactionType.Vendor_Bills:
                    if (payments[0].Bill != null && payments[0].Bill.customerCompany != null && payments[0].Bill.customerCompany.company != null)
                        txtCustomerName.Text = payments[0].Bill.customerCompany.company.CompanyName;
                    txtPrincipalName.Text = "N/A";
                    break;
            }


            List<PaymentStatus> statuses = new List<PaymentStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment Statuses") != null)
            {
                statuses = paymentRepo.GetAllPaymentStatuses();
            }
            else
                statuses = paymentRepo.GetAllOpenPaymentStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PaymentStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (payments[0].Status.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == payments[0].Status.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Payment status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == payments[0].Status.Status))];
            }
        }

        private void LoadAdminBillData(int GroupId)
        {
            txtModuleName.Text = "Admin Bill";
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            var bills = adminBillsRepo.GetBillsByGroupIdForForm(GroupId);

            if (bills != null && bills.Count > 0)
            {
                if (bills[0].isVoid == true)
                {
                    lblStage.Text = "Void";
                }
                else if (bills[0].isReApproved == false)
                {
                    lblStage.Text = "Under Re-Approval";
                }
                else if (bills[0].isApproved == true && bills[0].stage == "Closed")
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (bills[0].isApproved == true && bills[0].BillStatus.isActive == false && bills[0].PendingForClosing != true)
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (bills[0].isApproved == true && bills[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
                else if (bills[0].isApproved == true)
                {
                    lblStage.Text = "Approved";
                }
                else if (bills[0].isApproved == false)
                {
                    lblStage.Text = "Under Approval";
                }
                else if (bills[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
            }

            txtCustomerName.Text = "N/A";
            txtPrincipalName.Text = "N/A";

            if (bills[0].CreationDate != null)
                txtCreationDate.Text = bills[0].CreationDate.ToString();
            if (bills[0].company != null)
                txtCompanyName.Text = bills[0].company.CompanyName;
            if (bills[0].department != null)
                txtDeptName.Text = bills[0].department.DeptName;

            List<AdminBillStatus> statuses = new List<AdminBillStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Admin Bill Statuses") != null)
            {
                statuses = adminBillsRepo.GetAllBillStatuses();
            }
            else
                statuses = adminBillsRepo.GetAllOpenBillStatuses();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (AdminBillStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (bills[0].BillStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bills[0].BillStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Admin Bill status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bills[0].BillStatus.Status))];
            }
        }

        private void LoadIBTData(int IBTid)
        {
            txtModuleName.Text = "Inter-Bank Transfer";
            InterBankTransRepo bankTransRepo = new InterBankTransRepo();
            var bankTransfer = bankTransRepo.GetInterBankTransfer(IBTid);
            if (bankTransfer.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (bankTransfer.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.interBankTransStatus.isActive == false && bankTransfer.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bankTransfer.isApproved == true && bankTransfer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (bankTransfer.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (bankTransfer.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (bankTransfer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (bankTransfer.CreationDate != null)
                txtCreationDate.Text = bankTransfer.CreationDate.ToString();
            if (bankTransfer.company != null)
                txtCompanyName.Text = bankTransfer.company.CompanyName;
            if (bankTransfer.department != null)
                txtDeptName.Text = bankTransfer.department.DeptName;

            txtCustomerName.Text = "N/A";
            txtPrincipalName.Text = "N/A";

            List<InterBankTransferStatus> statuses = new List<InterBankTransferStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer Statuses") != null)
            {
                statuses = bankTransRepo.GetAllInterBankTransStatus();
            }
            else
                statuses = bankTransRepo.GetAllOpenBankTransferStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (InterBankTransferStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (bankTransfer.interBankTransStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bankTransfer.interBankTransStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Inter-Bank Transfer status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bankTransfer.interBankTransStatus.Status))];
            }
        }

        private void LoadSaleReceiptData(int GroupId)
        {
            txtModuleName.Text = "Sale Receipt";
            SalesReceiptRepo salesReceiptRepo = new SalesReceiptRepo();
            var receiptsList = salesReceiptRepo.getReceiptsByGroupId(GroupId);

            if(receiptsList != null && receiptsList.Count > 0)
            {
                if (receiptsList[0].isVoid == true)
                {
                    lblStage.Text = "Void";
                }
                else if (receiptsList[0].isReApproved == false)
                {
                    lblStage.Text = "Under Re-Approval";
                }
                else if (receiptsList[0].isApproved == true && receiptsList[0].stage == "Closed")
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (receiptsList[0].isApproved == true && receiptsList[0].saleReceiptStatus.isActive == false && receiptsList[0].PendingForClosing != true)
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (receiptsList[0].isApproved == true && receiptsList[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
                else if (receiptsList[0].isApproved == true)
                {
                    lblStage.Text = "Approved";
                }
                else if (receiptsList[0].isApproved == false)
                {
                    lblStage.Text = "Under Approval";
                }
                else if (receiptsList[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
            }
            
            if (receiptsList[0].CreationDate != null)
                txtCreationDate.Text = receiptsList[0].CreationDate.ToString();
            if (receiptsList[0].company != null)
                txtCompanyName.Text = receiptsList[0].company.CompanyName;
            if (receiptsList[0].department != null)
                txtDeptName.Text = receiptsList[0].department.DeptName;

            if (receiptsList[0].Customer != null && receiptsList[0].Customer.company != null)
                txtCustomerName.Text = receiptsList[0].Customer.company.CompanyName;
            if (receiptsList[0].principal != null && receiptsList[0].principal.company != null)
                txtPrincipalName.Text = receiptsList[0].principal.company.CompanyName;

            List<SalesReceiptStatus> statuses = new List<SalesReceiptStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipt Statuses") != null)
            {
                statuses = salesReceiptRepo.GetAllSaleReceiptStatus();
            }
            else
                statuses = salesReceiptRepo.GetAllOpenSaleReceiptStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SalesReceiptStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (receiptsList[0].saleReceiptStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == receiptsList[0].saleReceiptStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Sale Receipt status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == receiptsList[0].saleReceiptStatus.Status))];
            }
        }

        private void LoadVendorBillData(int BillId)
        {
            txtModuleName.Text = "Vendor Bill";
            BillRepo billRepo = new BillRepo();
            var bill = billRepo.get(BillId);
            if (bill.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (bill.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (bill.isApproved == true && bill.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (bill.isApproved == true && bill.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (bill.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (bill.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (bill.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (bill.CreationDate != null)
                txtCreationDate.Text = bill.CreationDate.ToString();
            if (bill.company != null)
                txtCompanyName.Text = bill.company.CompanyName;
            if (bill.department != null)
                txtDeptName.Text = bill.department.DeptName;

            if (bill.customerCompany != null && bill.customerCompany.company != null)
                txtCustomerName.Text = bill.customerCompany.company.CompanyName;

            txtPrincipalName.Text = "N/A";

            List<BillStatus> statuses = new List<BillStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bill Statuses") != null)
            {
                statuses = billRepo.getAllBillStatus();
            }
            else
                statuses = billRepo.getAllActiveBillStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (BillStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (bill.BillStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bill.BillStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Vendor Bill status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == bill.BillStatus.Status))];
            }
        }

        private void LoadInquiryData(int InquiryId)
        {
            txtModuleName.Text = "Inquiry";
            InquiryRepo inquiryRepo = new InquiryRepo();
            var inquiry = inquiryRepo.get(InquiryId);
            if (inquiry.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (inquiry.isApproved == true && inquiry.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (inquiry.isApproved == true && inquiry.inquiryStatus.isActive == false && inquiry.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (inquiry.isApproved == true && inquiry.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (inquiry.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (inquiry.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (inquiry.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (inquiry.CreationDate != null)
                txtCreationDate.Text = inquiry.CreationDate.ToString();
            if (inquiry.company != null)
                txtCompanyName.Text = inquiry.company.CompanyName;
            if (inquiry.department != null)
                txtDeptName.Text = inquiry.department.DeptName;

            if (inquiry.customerCompany != null && inquiry.customerCompany.company != null)
                txtCustomerName.Text = inquiry.customerCompany.company.CompanyName;

            txtPrincipalName.Text = "N/A";

            List<InquiryStatus> statuses = new List<InquiryStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiry Statuses") != null)
            {
                statuses = inquiryRepo.getAllInquiryStatus();
            }
            else
                statuses = inquiryRepo.getAllActiveStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (InquiryStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (inquiry.inquiryStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == inquiry.inquiryStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Inquiry status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == inquiry.inquiryStatus.Status))];
            }
        }

        private void LoadOfferData(int OfferId)
        {
            txtModuleName.Text = "Offer";
            OfferRepo offerRepo = new OfferRepo();
            var offer = offerRepo.get(OfferId);
            if (offer.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (offer.isApproved == true && offer.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (offer.isApproved == true && offer.offerStatus.isActive == false && offer.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (offer.isApproved == true && offer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (offer.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (offer.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (offer.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (offer.CreationDate != null)
                txtCreationDate.Text = offer.CreationDate.ToString();
            if (offer.company != null)
                txtCompanyName.Text = offer.company.CompanyName;
            if (offer.department != null)
                txtDeptName.Text = offer.department.DeptName;

            if (offer.customerCompany != null && offer.customerCompany.company != null)
                txtCustomerName.Text = offer.customerCompany.company.CompanyName;

            if (offer.principal != null && offer.principal.company != null)
                txtPrincipalName.Text = offer.principal.company.CompanyName;

            List<OfferStatus> statuses = new List<OfferStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Offer Statuses") != null)
            {
                statuses = offerRepo.getAllOfferStatus();
            }
            else
                statuses = offerRepo.getAllActiveStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (OfferStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (offer.offerStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == offer.offerStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Offer status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == offer.offerStatus.Status))];
            }
        }

        private void LoadSaleOrderData(int SaleOrderId )
        {
            txtModuleName.Text = "Sale Order";
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            var saleOrder = saleOrderRepo.get(SaleOrderId);
            if (saleOrder.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (saleOrder.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";

            }
            else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (saleOrder.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (saleOrder.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (saleOrder.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            if (saleOrder.CreationDate != null)
                txtCreationDate.Text = saleOrder.CreationDate.ToString();
            if(saleOrder.company != null)
                txtCompanyName.Text = saleOrder.company.CompanyName;
            if (saleOrder.department != null)
                txtDeptName.Text = saleOrder.department.DeptName;

            if (saleOrder.customerCompany != null && saleOrder.customerCompany.company != null)
                txtCustomerName.Text = saleOrder.customerCompany.company.CompanyName;

            if (saleOrder.principal != null && saleOrder.principal.company != null)
                txtPrincipalName.Text = saleOrder.principal.company.CompanyName;


            List<SaleOrderStatus> SaleOrderStatuses = new List<SaleOrderStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null)
            {
                SaleOrderStatuses = saleOrderRepo.getAllSaleOrderStatus();
            }
            else
                SaleOrderStatuses = saleOrderRepo.getAllActiveSaleOrderStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (saleOrder.saleOrderStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == saleOrder.saleOrderStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Sale Order status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == saleOrder.saleOrderStatus.Status))];
            }

            grdRemainingSIamount.Visibility = Visibility.Collapsed;
            grdReceivedAmount.Visibility = Visibility.Collapsed;
            txtInvoiced.Text = "Total Invoiced";
            if (saleOrder?.SaleInvoices != null && saleOrder?.SaleInvoices.Count > 0)
            {
                var invoicedAmount = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                invoicedAmount = Math.Round(Convert.ToDouble(invoicedAmount), 2);
                var fobValue = Convert.ToDouble(saleOrder.totalCFRValue);
                txtRemainingSOamount.Text = (fobValue - invoicedAmount).ToString();
                txtSOamount.Text = fobValue.ToString();
                if (saleOrder.Id != 0 && saleOrder.saleOrdertype == InquiryType.Principal && saleOrder.commision != 0)
                {
                    txtRemainingSOamount.Text = (Convert.ToDouble(saleOrder.commision) - invoicedAmount).ToString();

                }
                txtInvoicedAmount.Text = invoicedAmount.ToString();
                //txtSOremainingcfr.Text = Math.Round(saleOrder.RemainingCFRValue, 2).ToString();
            }
            else
            {
                txtSOamount.Text = Convert.ToDouble(saleOrder.totalCFRValue).ToString();
                txtRemainingSOamount.Text = Convert.ToDouble(saleOrder.totalCFRValue).ToString();
                txtInvoicedAmount.Text = "0";
            }
        }

        private void LoadSaleInvoiceData(int SaleInvoiceId)
        {
            txtModuleName.Text = "Sale Invoice";
            SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
            var saleInvoice = saleInvoiceRepo.get(SaleInvoiceId);
            if (saleInvoice.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (saleInvoice.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (saleInvoice.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (saleInvoice.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";               
            }

            if (saleInvoice.CreationDate != null)
                txtCreationDate.Text = saleInvoice.CreationDate.ToString();
            if (saleInvoice.company != null)
                txtCompanyName.Text = saleInvoice.company.CompanyName;
            if (saleInvoice.department != null)
                txtDeptName.Text = saleInvoice.department.DeptName;

            if (saleInvoice.customerCompany != null && saleInvoice.customerCompany.company != null)
                txtCustomerName.Text = saleInvoice.customerCompany.company.CompanyName;

            if (saleInvoice.principal != null && saleInvoice.principal.company != null)
                txtPrincipalName.Text = saleInvoice.principal.company.CompanyName;

            List<SaleInvoiceStatus> statuses = new List<SaleInvoiceStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoice Statuses") != null)
            {
                statuses = saleInvoiceRepo.getAllSaleInvoiceStatus();
            }
            else
                statuses = saleInvoiceRepo.getAllActiveSaleInvoiceStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleInvoiceStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (saleInvoice.saleInvoiceStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == saleInvoice.saleInvoiceStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Offer status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == saleInvoice.saleInvoiceStatus.Status))];
            }

            grdRemainingSIamount.Visibility = Visibility.Visible;
            grdReceivedAmount.Visibility = Visibility.Visible;
            txtInvoicedAmount.Text = saleInvoice.totalInvoiceAmount.ToString();
            txtInvoiced.Text = "SI Amount";

            if (saleInvoice.salesReceipts != null)
            {

                var collected = saleInvoice.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                txtReceivedAmount.Text = collected.ToString();
                txtRemainingInvoice.Text = (saleInvoice.totalInvoiceAmount - collected).ToString();
            }


            if (saleInvoice.SaleOrder?.SaleInvoices != null) // load info from saleorder
            {
                var soAmount = saleInvoice?.SaleOrder?.commision != 0 ? Convert.ToDouble(saleInvoice?.SaleOrder?.commision) : saleInvoice.SaleOrder.totalCFRValue;
                var sumInvocies = Math.Round(Convert.ToDouble(saleInvoice.SaleOrder?.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount)), 2, MidpointRounding.AwayFromZero);
                txtSOamount.Text = soAmount.ToString();
                txtRemainingSOamount.Text = (Math.Round(soAmount - sumInvocies, 2)).ToString();
                //SOCFRRemaining = Math.Round(Convert.ToDouble(soAmount - sumInvocies) + saleInvoice.totalInvoiceAmount, 2);

            }
            else
            {
                //txtSOtotalfob.Text = saleInvoice.SOFOBValue.ToString();
                txtSOamount.Text = saleInvoice.SOCFRValue.ToString();
                //txtSOremainingfob.Text = saleInvoice.SaleOrder.RemainingFOBValue.ToString();
                txtRemainingSOamount.Text = saleInvoice.SaleOrder.RemainingCFRValue.ToString();
                //SOFobRemaining = saleInvoice.SaleOrder.RemainingFOBValue + saleInvoice.totalFOBValue;
                //SOCFRRemaining = Math.Round(saleInvoice.SaleOrder.RemainingCFRValue + saleInvoice.totalInvoiceAmount, 2);
            }
        }

        private void LoadPurchaseOrderData(int PurchaseOrderId)
        {
            txtModuleName.Text = "Purchase Order";
            PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
            var purchaseOrder = purchaseOrderRepo.get(PurchaseOrderId);
            if (purchaseOrder.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (purchaseOrder.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.PurchaseOrderStatus.isActive == false && purchaseOrder.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (purchaseOrder.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (purchaseOrder.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (purchaseOrder.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }

            if (purchaseOrder.CreationDate != null)
                txtCreationDate.Text = purchaseOrder.CreationDate.ToString();
            if (purchaseOrder.company != null)
                txtCompanyName.Text = purchaseOrder.company.CompanyName;
            if (purchaseOrder.department != null)
                txtDeptName.Text = purchaseOrder.department.DeptName;

            if (purchaseOrder.customerCompany != null && purchaseOrder.customerCompany.company != null)
                txtCustomerName.Text = purchaseOrder.customerCompany.company.CompanyName;

            txtPrincipalName.Text = "N/A";

            List<PurchaseOrderStatus> statuses = new List<PurchaseOrderStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Order Statuses") != null)
            {
                statuses = purchaseOrderRepo.getAllPurchaseOrderStatus();
            }
            else
                statuses = purchaseOrderRepo.getAllActivePurchaseOrderStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PurchaseOrderStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (purchaseOrder.PurchaseOrderStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed PO status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];
            }


            var paidAmount = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));

            
            
            if (purchaseOrder.tax != null)
            {
                txtPOAmount.Text = purchaseOrder.billWithTax.ToString();
                var invoicedAmount1 = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                var amountWithTax = invoicedAmount1 + ((invoicedAmount1 * purchaseOrder.tax.percentage) / 100);
                txtPOInvoiceAmount.Text = Math.Round(amountWithTax, 2).ToString();
                txtPOUnInvoiceAmount.Text = Math.Round(purchaseOrder.billWithTax.Value - amountWithTax, 2).ToString();


                txtPaid.Text = paidAmount.ToString();
                txtUnpaid.Text = Math.Round(amountWithTax - paidAmount, 2).ToString();
            }
            else
            {
                var invoicedAmount = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                txtPOAmount.Text = purchaseOrder.totalCFRValue.ToString();
                txtPOInvoiceAmount.Text = invoicedAmount.ToString();
                txtPOUnInvoiceAmount.Text = Math.Round(purchaseOrder.totalCFRValue + purchaseOrder.totaltaxAmount - invoicedAmount, 2).ToString();
                txtPaid.Text = paidAmount.ToString();
                txtUnpaid.Text = Math.Round(invoicedAmount - paidAmount, 2).ToString();
            }
        }

        private void LoadPurchaseInvoiceData(int PurchaseInvoiceId)
        {
            txtModuleName.Text = "Purchase Invoice";
            PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
            var purchaseInvoice = purchaseInvoiceRepo.get(PurchaseInvoiceId);
            if (purchaseInvoice.isVoid == true)
            {
                lblStage.Text = "Void";
            }
            else if (purchaseInvoice.isReApproved == false)
            {
                lblStage.Text = "Under Re-Approval";
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseInvoiceStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
            {
                lblStage.Text = "Approved and Closed";
            }
            else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }
            else if (purchaseInvoice.isApproved == true)
            {
                lblStage.Text = "Approved";
            }
            else if (purchaseInvoice.isApproved == false)
            {
                lblStage.Text = "Under Approval";
            }
            else if (purchaseInvoice.PendingForClosing == true)
            {
                lblStage.Text = "Under Closing Approval";
            }

            if (purchaseInvoice.CreationDate != null)
                txtCreationDate.Text = purchaseInvoice.CreationDate.ToString();
            if (purchaseInvoice.company != null)
                txtCompanyName.Text = purchaseInvoice.company.CompanyName;
            if (purchaseInvoice.department != null)
                txtDeptName.Text = purchaseInvoice.department.DeptName;

            if (purchaseInvoice.customerCompany != null && purchaseInvoice.customerCompany.company != null)
                txtCustomerName.Text = purchaseInvoice.customerCompany.company.CompanyName;

            txtPrincipalName.Text = "N/A";

            List<PurchaseInvoiceStatus> statuses = new List<PurchaseInvoiceStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Invoice Statuses") != null)
            {
                statuses = purchaseInvoiceRepo.getAllPurchaseInvoiceStatus();
            }
            else
                statuses = purchaseInvoiceRepo.getAllActivePurchaseInvoiceStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PurchaseInvoiceStatus status in statuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbStatus.ItemsSource = cmbitems;

            var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (purchaseInvoice.PurchaseInvoiceStatus.isActive == false)
            {
                try
                {
                    cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseInvoice.PurchaseInvoiceStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Invoice status! " + ex.ToString());
                }
            }
            else
            {
                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == purchaseInvoice.PurchaseInvoiceStatus.Status))];
            }
        }

        private void TableView_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                if (notification != null)
                {
                    if(notificationsLoaded == "Sent")
                    {
                        imgGlowOn.Visibility = Visibility.Collapsed;
                        imgGlowOff.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (notification.notificationFlag != null)
                        {
                            if (notification.notificationFlag.canGlow == true && notification.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        else
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = null;
                        }
                    }
                    
                    
                    if (notification.TransactionType == TransactionItemType.Sale_Order)
                    {
                        tabModuleDetails.IsEnabled = true;
                    }
                    else
                    {
                        tabModuleDetails.IsEnabled = false;
                        tabNotification.IsSelected = true;
                    }

                    if (tabNotification.IsSelected == true)
                    {
                        LoadNotificationData(notification);
                    }
                    else if (tabModuleDetails.IsSelected == true)
                    {
                        LoadModuleData(notification);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private void MbtnMarkAsRead_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasReadNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Read Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnMarkAsUnread_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasUnReadNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Unread Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdNotifications);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void CmbNotificationFlag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(grdNotifications.GetFocusedRow() == null)
            {
                DXMessageBox.Show("Please Select Notification first!");
                return;
            }
            if (cmbNotificationFlag.SelectedIndex > 0)
            {
                var flagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                var flag = notificationsRepo.GetNotificationFlag(flagId);

                var not = grdNotifications.GetFocusedRow() as Notification;

                if(flag.canGlow == true)
                {
                    if(not.Id != checkNotificationIdGlow)
                    {
                        checkNotificationIdGlow = not.Id;
                        if (notificationsLoaded == "Sent")
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = true;
                        }
                        else
                        {
                            if (not.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        
                    }
                    else
                    {
                        if (notificationsLoaded == "Sent")
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = true;
                        }
                        else
                        {
                            if (not.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        
                    }
                    
                }
                else
                {
                    checkNotificationIdGlow = not.Id;
                    imgGlowOn.Visibility = Visibility.Collapsed;
                    imgGlowOff.Visibility = Visibility.Collapsed;
                    glow = false;
                }
            }
            else
            {
                imgGlowOn.Visibility = Visibility.Collapsed;
                imgGlowOff.Visibility = Visibility.Collapsed;
                glow = null;
            }

        }

        

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbNotificationFlag.SelectedIndex > -1)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;

                if (cmbNotificationFlag.SelectedIndex == 0)
                    notification.FlagId = null;
                else
                {
                    var flagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                    var flag = notificationsRepo.GetNotificationFlag(flagId);

                    if (flag.canGlow == true)
                    {
                        notification.Glow = glow.Value;
                    }
                    notification.FlagId = flagId;
                }

                notificationsRepo.Update(notification);

                var myWindow = Window.GetWindow(this) as MainWindow;
                myWindow.UrgentNotificationGlow();

                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "Information",
                    Message = "Notification Updated!",
                    Type = Notifications.Wpf.NotificationType.Success
                });
            }
        }

        private void MbtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            LoadAllNotifications();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            myParent.FavouriteItems.Children.Remove(this);
        }

        private void MbtnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdNotifications_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdNotifications.GetRowByListIndex(e.ListSourceRowIndex) as Notification;
                switch (e.Column.FieldName)
                {
                    case "FromUser":
                        if (row.SendingUser != null && row.SendingUser.employee != null && row.SendingUser.employee.person != null)
                        {
                            userName = row.SendingUser.employee.person.FName + " " + row.SendingUser.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "ToUser":
                        if (row.User != null && row.User.employee != null && row.User.employee.person != null)
                        {
                            userName = row.User.employee.person.FName + " " + row.User.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "CCuser":
                        if (row.CcUser != null && row.CcUser.employee != null && row.CcUser.employee.person != null)
                        {
                            userName = row.CcUser.employee.person.FName + " " + row.CcUser.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                }
            }
        }

        private void GrdNotifications_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            selectedCounter = grdNotifications.SelectedItems.Count;
            txtSelected.Text = selectedCounter.ToString();
        }

        private void DXTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {
            notification = grdNotifications.GetFocusedRow() as Notification;
            if (tabNotification.IsSelected == true)
                LoadNotificationData(notification);
            else if (tabModuleDetails.IsSelected == true)
                LoadModuleData(notification);
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                switch (notification.TransactionType)
                {
                    case TransactionItemType.Sale_Order:
                        ApproveSaleOrder(notification.TransactionId);
                        break;
                }
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                switch (notification.TransactionType)
                {
                    case TransactionItemType.Sale_Order:
                        AddCommentSaleOrder(notification.TransactionId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                switch (notification.TransactionType)
                {
                    case TransactionItemType.Sale_Order:
                        DirectCloseSaleOrder(notification.TransactionId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                switch (notification.TransactionType)
                {
                    case TransactionItemType.Sale_Order:
                        VoidSaleOrder(notification.TransactionId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                switch (notification.TransactionType)
                {
                    case TransactionItemType.Sale_Order:
                        AttachNewSaleOrder(notification.TransactionId);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AttachNewSaleOrder(int transactionId)
        {
            if (notification.TransactionId != 0)
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    var saleOrder = saleOrderRepo.get(transactionId);
                    if (saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when SO Closed") != null)
                        {
                            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                            grdAttach1.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required" + " Can attach document when SO Closed!");
                        }
                    }
                    else
                    {


                        if (grdAttach1.Visibility == Visibility.Visible)
                            grdAttach1.Visibility = Visibility.Collapsed;
                        else
                        {
                            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                            grdAttach1.Visibility = Visibility.Visible;
                        }
                    }
                        
                }
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            int OrderId = notification.TransactionId;
           
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (OrderId != 0)
                {
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    var saleOrder = saleOrderRepo.get(OrderId);
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\SaleOrder\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew1.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew1.ToolTip = "Uploading";
                            btnAttachNew1.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Sale_Order.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Order);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Sale_Order, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, saleOrder.Id, 3, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {

                                            //treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Sale_Order);
                                            imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew1.ToolTip = "Attach";
                                            btnAttachNew1.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
                                        });



                                    }
                                });
                                thread.Start();

                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }
                            //MessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void VoidSaleOrder(int transactionId)
        {
            if(transactionId != 0)
            {
                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                var saleOrder = saleOrderRepo.get(transactionId);
                var SIcount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).ToList().Count;
                var purchaseOrders = saleOrder.PurchaseOrders;
                var bills = saleOrder.Bills;

                if (SIcount != 0)
                {
                    DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Sale Invoices!");
                    return;
                }
                if (purchaseOrders != null)
                {
                    var POcount = purchaseOrders.Where(x => x.isVoid != true).ToList();
                    if (POcount.Count > 0)
                    {
                        DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Purchase Orders!");
                        return;
                    }
                }
                if (bills != null)
                {
                    var billCount = bills.Where(x => x.isVoid != true).ToList();
                    if (billCount.Count > 0)
                    {
                        DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Bills!");
                        return;
                    }
                }


                if (saleOrder.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleOrder") != null))
                {

                    if (DXMessageBox.Show("This SO is currently in the list of Void Sale Orders! Do you want to remove it from Void?", "Remove Void Sale Order", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        saleOrder.isVoid = false;
                        saleOrderRepo.setSotoVoid(saleOrder.Id, false);

                        //grdVoid.Visibility = Visibility.Collapsed;

                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("SO has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && saleOrder.isInterCompany == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment.Id }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                //inputBox.ShowDialog();
                            }
                            else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.isInterCompany != true/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }

                        }

                        string symbolCurr = "";
                        if (saleOrder.currency != null)
                        {
                            symbolCurr = saleOrder.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                            Timestamp = DateTime.Now,
                            Subject = "SO UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }

                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleOrder") != null)
                {
                    if (DXMessageBox.Show("This So is not currently in the list of Void Sale Orders! Do you want to move it to Void Saleorders?", "Add to Void Saleorders", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        saleOrder.isVoid = true;
                        saleOrderRepo.setSotoVoid(saleOrder.Id, true);

                        //grdVoid.Visibility = Visibility.Visible;


                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        ProcurementRepo procurementRepo = new ProcurementRepo();
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res1 = MessageBox.Show("SO has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {
                            if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && saleOrder.isInterCompany == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment.Id }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                //inputBox.ShowDialog();
                            }
                            else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.isInterCompany != true/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }

                        }

                        string symbolCurr = "";
                        if (saleOrder.currency != null)
                        {
                            symbolCurr = saleOrder.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                            Timestamp = DateTime.Now,
                            Subject = "SO Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
            }
           
        }

        private void ApproveSaleOrder(int transactionId)
        {
            if (transactionId > 0)
            {

                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                //SalesReceipt receipt = new SalesReceipt();
                SaleOrder saleOrder = new SaleOrder();
                saleOrder = saleOrderRepo.get(transactionId);
                UsersRepo usersRepo = new UsersRepo();

                if (saleOrder != null)
                {
                    if (saleOrder.isApproved == true)
                    {

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                        {
                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sale Order is Approved, Do you want to UnApprove this Sale Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                            if (res == MessageBoxResult.Yes)
                            {

                                saleOrder.isApproved = false;
                                saleOrder.stage = TransactionStage.AwaitingApproval.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();

                                //frmInputBox inputBox = new frmInputBox();
                                //inputBox.ShowDialog();
                                //usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                //saleOrderrepo = new SaleOrderRepo();
                                saleOrderRepo.Approve(saleOrder);

                                var res1 = MessageBox.Show("SO has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res1 == MessageBoxResult.Yes)
                                {
                                    if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && saleOrder.isInterCompany == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment_Id.Value }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                        //inputBox.ShowDialog();
                                    }
                                    else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.isInterCompany != true/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();
                                    }
                                }

                                string symbolCurr = "";
                                if (saleOrder.currency != null)
                                {
                                    symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog();

                                if (saleOrder.saleOrdertype == InquiryType.Principal)
                                {
                                    comment.Comment = "SO (Comission) having value: " + saleOrder.commision.ToString() + "(" + symbolCurr + ") " + " has been UnApproved" + "\n"
                                        + "SO(Net Comission) having value: " + saleOrder.netCommision.ToString()
                                       ;
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "SO UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                }
                                else
                                {

                                    comment.Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "SO UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                }
                                procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating Comments
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Sale Order are UnApproved (" + saleOrder.referenceNo + ")");
                                SystemLog.LogInfo(this.GetType(), "Sale Order is UnApproved (" + saleOrder.Id + ")");
                            }

                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Approve Sale Oder Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Oder Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }

                    }
                    else if (saleOrder.isApproved == false)
                    {

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                        {
                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sale Order are Pending for Approval, Do you want to Approve this Sale Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                            if (res == MessageBoxResult.Yes)
                            {

                                saleOrder.isApproved = true;
                                saleOrder.stage = TransactionStage.Approved.ToString();
                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();

                                //frmInputBox inputBox = new frmInputBox();
                                //inputBox.ShowDialog();
                                //usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                saleOrderRepo.Approve(saleOrder);

                                var res1 = MessageBox.Show("SO has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res1 == MessageBoxResult.Yes)
                                {
                                    if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && saleOrder.isInterCompany == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment.Id }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                        //inputBox.ShowDialog();
                                    }
                                    else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.isInterCompany != true/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();
                                    }

                                }

                                string symbolCurr = "";
                                if (saleOrder.currency != null)
                                {
                                    symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                    Timestamp = DateTime.Now,
                                    Subject = "SO Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating Comments
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Sale Oder is Approved (" + saleOrder.referenceNo + ")");
                                SystemLog.LogInfo(this.GetType(), "Sale Order is Approved (" + saleOrder.Id + ")");
                            }


                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Approve Sale Order Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Order Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }

                    }


                    //var myWindow = Window.GetWindow(this);
                    //myWindow.Close();
                }
                //Load_Receipts();

            }
        }

        private void DirectCloseSaleOrder(int transactionId)
        {
            var saleOrderid = transactionId;
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null) ? true : false)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                var saleOrder = saleOrderRepo.get(saleOrderid);
                SaleOrderStatus oldStatus = new SaleOrderStatus();
                oldStatus = saleOrder.saleOrderStatus;
                //return;
                var total = saleOrder.totalCFRValue; //Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                var result = Convert.ToDouble(saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));


                double remainingCollection = Math.Round(total - result, 2);
                if (saleOrder.saleOrdertype == InquiryType.Principal)
                {
                    var invoicedAmount = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                    var collected = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                    remainingCollection = Convert.ToDouble(invoicedAmount) - Convert.ToDouble(collected);
                }
                if (remainingCollection != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without receiving fully Collection") != null)
                    {
                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale order without complete collection?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                        {

                            UsersRepo usersRepo = new UsersRepo();

                            

                            var row = saleOrder;

                            decimal budgetMarginOC = 0;
                            decimal actualMarginOC = 0;

                            decimal totalCFRValue = 0;
                            decimal TotalBudgetedMargin = 0;
                            decimal margin = 0;

                            decimal TotalActualMargin = 0;
                            decimal ActualMargin = 0;

                            if (row.totalCFRValue != 0)
                            {
                                totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                            }
                            if (row.margin != 0)
                            {
                                margin = Convert.ToDecimal(row.margin);
                            }
                            if (row.ActualMargin != 0)
                            {
                                ActualMargin = Convert.ToDecimal(row.ActualMargin);
                            }
                            if (row.CostSheet != null)
                            {

                                TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                                TotalActualMargin = row.CostSheet.TotalActualMargin;
                            }

                            if (row.CostSheet != null)
                            {

                                budgetMarginOC = totalCFRValue - TotalBudgetedMargin;

                            }
                            else
                            {
                                budgetMarginOC = margin;
                            }



                            if (row.CostSheet != null)
                            {

                                actualMarginOC = totalCFRValue - TotalActualMargin;

                            }
                            else
                            {
                                actualMarginOC = ActualMargin;
                            }



                            if (row.saleOrderStatus != null)
                            {
                                oldStatus = row.saleOrderStatus;
                            }
                            ucStatuschange.inActiveStatuses = 1;

                            ucStatuschange.saleOrderid = (int)saleOrderid;
                            frmSaleOrderStatusChange statusChange = new frmSaleOrderStatusChange(saleOrderRepo);
                            var myWindow = Window.GetWindow(this);
                            statusChange.Owner = myWindow;
                            statusChange.ShowDialog();
                            //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                            //    return;
                            if (ucStatuschange.saleOrder.Id != 0)

                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                {
                                    ucStatuschange.saleOrder.PendingForClosing = false;
                                    ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Closing, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                                }

                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (ucStatuschange.saleOrder.PendingForClosing == null)
                                    {
                                        ucStatuschange.saleOrder.PendingForClosing = true;

                                    }
                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }

                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                {
                                    ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (ucStatuschange.saleOrder.PendingForClosing != true)
                                    {
                                        ucStatuschange.saleOrder.PendingForClosing = true;

                                    }
                                    //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }
                                else
                                {
                                    ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                    ucStatuschange.saleOrder.PendingForClosing = true;
                                    usersRepo.Add(TransactionInfo.Closed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }
                            //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                            ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                            if (row.saleOrderStatus != ucStatuschange.saleOrder.saleOrderStatus)
                                usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            //Adding auto Signature
                            //if (row.saleOrderStatus.Status != saleOrder.saleOrderStatus.Id)
                            //{
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                    //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();

                                }

                            }
                            string oldStat = "";
                            if (oldStatus != null)
                            {
                                oldStat = oldStatus.Status;
                            }
                            string newStat = ucStatuschange.saleOrder.saleOrderStatus.Status;
                            string symbolCurr = "";
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                            //CommentLog comment = new CommentLog()
                            //{
                            //    Comment = "Status of SO having value: " + row.totalFOBValue.ToString()  +"(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            //    Timestamp = DateTime.Now,
                            //    Subject = "Status Changed from Direct Close"
                            //};

                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of SO having SO Amount (OC): " + row.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                    + "Budget Margin(OC): " + budgetMarginOC.ToString() + " (" + symbolCurr + ")"
                                    + "\nActal Margin(OC): " + actualMarginOC.ToString() + " (" + symbolCurr + ")"
                                    + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };

                            procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {

                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {


                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }


                            //}

                            //SaleOrderss.ucStatuschange.Updatestatus();
                            try
                            {
                                row = ucStatuschange.saleOrder;

                                saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                            }
                            catch { }

                            //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                            MessageBox.Show("SaleOrder status changed to InActive (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                        }
                        else
                            return;
                    }
                    else
                    {
                        DXMessageBox.Show("Collection is not fully received yet, or you need permission to Close Sale Order without receiving fully Collection", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                else
                {
                    UsersRepo usersRepo = new UsersRepo();
                    saleOrder = saleOrderRepo.get(saleOrderid);

                    var row = saleOrder;

                    decimal budgetMarginOC = 0;
                    decimal actualMarginOC = 0;

                    decimal totalCFRValue = 0;
                    decimal TotalBudgetedMargin = 0;
                    decimal margin = 0;

                    decimal TotalActualMargin = 0;
                    decimal ActualMargin = 0;

                    if (row.totalCFRValue != 0)
                    {
                        totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                    }
                    if (row.margin != 0)
                    {
                        margin = Convert.ToDecimal(row.margin);
                    }
                    if (row.ActualMargin != 0)
                    {
                        ActualMargin = Convert.ToDecimal(row.ActualMargin);
                    }
                    if (row.CostSheet != null)
                    {

                        TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                        TotalActualMargin = row.CostSheet.TotalActualMargin;
                    }

                    if (row.CostSheet != null)
                    {

                        budgetMarginOC = totalCFRValue - TotalBudgetedMargin;

                    }
                    else
                    {
                        budgetMarginOC = margin;
                    }



                    if (row.CostSheet != null)
                    {

                        actualMarginOC = totalCFRValue - TotalActualMargin;

                    }
                    else
                    {
                        actualMarginOC = ActualMargin;
                    }



                    if (row.saleOrderStatus != null)
                    {
                        oldStatus = row.saleOrderStatus;
                    }
                    ucStatuschange.inActiveStatuses = 1;

                    ucStatuschange.saleOrderid = (int)saleOrderid;
                    frmSaleOrderStatusChange statusChange = new frmSaleOrderStatusChange(saleOrderRepo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                    //    return;
                    if (ucStatuschange.saleOrder.Id != 0)

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                        {
                            ucStatuschange.saleOrder.PendingForClosing = false;
                            ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                        }

                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                        {
                            ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ucStatuschange.saleOrder.PendingForClosing == null)
                            {
                                ucStatuschange.saleOrder.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                        }

                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                        {
                            ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ucStatuschange.saleOrder.PendingForClosing != true)
                            {
                                ucStatuschange.saleOrder.PendingForClosing = true;

                            }
                            //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                        }
                        else
                        {
                            ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                            ucStatuschange.saleOrder.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                        }
                    //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                    ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                    ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                    if (row.saleOrderStatus != ucStatuschange.saleOrder.saleOrderStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                    //Adding auto Signature
                    //if (row.saleOrderStatus.Status != saleOrder.saleOrderStatus.Id)
                    //{
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();

                        }

                    }
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = ucStatuschange.saleOrder.saleOrderStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    //CommentLog comment = new CommentLog()
                    //{
                    //    Comment = "Status of SO having value: " + row.totalFOBValue.ToString()  +"(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                    //    Timestamp = DateTime.Now,
                    //    Subject = "Status Changed from Direct Close"
                    //};

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of SO having SO Amount (OC): " + row.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                            + "Budget Margin(OC): " + budgetMarginOC.ToString() + " (" + symbolCurr + ")"
                            + "\nActal Margin(OC): " + actualMarginOC.ToString() + " (" + symbolCurr + ")"
                            + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };

                    procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }


                    //}

                    //SaleOrderss.ucStatuschange.Updatestatus();
                    try
                    {
                        row = ucStatuschange.saleOrder;

                        saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                    MessageBox.Show("SaleOrder status changed to InActive (" + ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                }

                //var thisWindow = Window.GetWindow(this);
                //thisWindow.Close();
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close SaleOrder Directly.");
            }
        }

        private void AddCommentSaleOrder(int transactionId)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            var saleOrder = saleOrderRepo.get(transactionId);
            var department = saleOrder.department;
            var company = saleOrder.company;
            var InterCompany = saleOrder.InterCompany;
            var InterDepartment = saleOrder.InterDepartment;
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && saleOrder.isInterCompany == true && InterDepartment != null && InterDepartment.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Order);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && saleOrder.isInterCompany != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Order);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (saleOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && saleOrder.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if(frmInputBox.FlagForCC==true)
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", null);

                        }
                    }

                    procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    //frmInputBox.taggedUsers = new List<User>();
                    //loadcomments();
                }
                else if (saleOrder.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }

            }
            //loadcomments();
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdNotifications_FilterChanged(object sender, RoutedEventArgs e)
        {
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void BtnBudgertPunching_Click(object sender, RoutedEventArgs e)
        {
            if (notification.TransactionId != 0)
            {
                btnCostSheet.IsEnabled = false;

                try
                {
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    var saleOrder = saleOrderRepo.get(notification.TransactionId);
                    var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sale Order?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {
                        //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Bill") != null)

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit RSBC from Sale Order") != null)

                        {
                            if (saleOrder.Id != 0)
                            {
                                winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(saleOrder, true);
                                winSelectCostSheetFields.ShowDialog();
                            }
                        }
                        else
                            DXMessageBox.Show("You do not have permisssion Edit RSBC from Sale Order", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            if(notification.TransactionId != 0)
            {
                var views = UsersRepo.getViwerInfo(notification.TransactionId, (int)TransactionItemType.Sale_Order);
                btnBudgertPunching.IsEnabled = false;
                try
                {
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    var saleOrder = saleOrderRepo.get(notification.TransactionId);
                    string inco = "";
                    string paymentterm = "";
                    if (saleOrder.paymentTerm != null)
                    {
                        paymentterm = saleOrder.paymentTerm.term;
                    }
                    if (saleOrder.incoterm != null)
                    {
                        inco = saleOrder.incoterm.term;
                    }
                    frmCostSheet frmCostSheet = new frmCostSheet(saleOrder, saleOrder.FinanceRefrenceNo, saleOrder.SalesReferenceNo, saleOrder.department.DeptName, saleOrder.customerCompany.company.CompanyName, saleOrder.currency.Symbol, saleOrder.totalCFRValue.ToString(), inco, saleOrder.CreationDate.Value.ToString(), paymentterm, saleOrder.maker, saleOrder.origin, views, saleOrder.saleOrdertype, saleOrder.principal.company.CompanyName, saleOrder.packing, saleOrder.deliveryDate.Value, saleOrder.Warranty.name, saleOrder.referenceNo, saleOrder.saleOrderDate.Value.ToString(), saleOrder.isApproved, saleOrder.isReApproved);
                    if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
                    {
                        //frmCostSheet.stackPanelCheckBoxes.IsEnabled = false; 
                        //frmCostSheet.gridUpper.IsEnabled = false;
                        frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        //frmCostSheet.lookupVendors.IsEnabled = false;
                        //frmCostSheet.lookupOC.IsEnabled = false;
                        //frmCostSheet.gridLower.IsEnabled = false;
                    }

                    frmCostSheet.ShowDialog();
                    //if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Principal)
                    //{
                    //    if (frmCostSheet.costSheet != null)
                    //    {
                    //        saleOrder.CostSheet = frmCostSheet.costSheet;
                    //        txtCommision.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();

                    //    }
                    //}
                    //else
                    {
                        //if (frmCostSheet.costSheet != null)
                        //{
                        //    saleOrder.CostSheet = frmCostSheet.costSheet;
                        //    txtBudgetMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                        //    txtRevisedMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();
                        //    if (Convert.ToDecimal(txttotalcfr.Text) != Convert.ToDecimal(txttotalcfr.Text))
                        //    {
                        //        txtActualMargin.Text = saleOrder.CostSheet.TotalActualMargin.ToString();
                        //    }
                        //    else
                        //    {
                        //        txtActualMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                        //    }

                        //    if ((saleOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                        //        saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                        //    saleOrder.isReApproved = frmCostSheet.isReApproved;

                        //}
                    }

                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.ToString());
                }
            }
            
        }

        private void ImgGlowOn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if(grdNotifications.GetFocusedRow() != null)
                {
                    //notification = grdNotifications.GetFocusedRow() as Notification;
                    //notification.Glow = false;
                    //notificationsRepo.Update(notification);
                    glow = false;

                    imgGlowOn.Visibility = Visibility.Collapsed;
                    imgGlowOff.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Error Ocuurred on Image Glow-On button");
            }
        }

        private void ImgGlowOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (grdNotifications.GetFocusedRow() != null)
                {
                    //notification = grdNotifications.GetFocusedRow() as Notification;
                    //notification.Glow = true;
                    //notificationsRepo.Update(notification);
                    glow = true;

                    imgGlowOn.Visibility = Visibility.Visible;
                    imgGlowOff.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Error Ocuurred on Image Glow-Off button");
            }
        }

        private void MbtnAllPendingNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = null;
            notificationsLoaded = "Pending";
            LoadAllPendingNotifications();
            if (button != null)
                setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void LoadAllPendingNotifications()
        {
            var SentNotifications = notificationsRepo.AllPendingNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        

        private void MbtnMarkAsPending_Click(object sender, RoutedEventArgs e)
        {
            if(notificationsLoaded == "Sent")
            {
                DXMessageBox.Show("Sent notifications cannot be Mark/UnMark as Pending!");
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasPendingNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Pending Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnUnMarkAsPending_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsLoaded == "Sent")
            {
                DXMessageBox.Show("Sent notifications cannot be Mark/UnMark as Pending!");
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.UnMarkasPendingNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "UnMarked Pending Notifications Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }
    }
}
