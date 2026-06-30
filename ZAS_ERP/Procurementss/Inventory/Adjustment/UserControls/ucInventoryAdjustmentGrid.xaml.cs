using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Procurements.Inventories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows;

namespace ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls
{
    /// <summary>
    /// Interaction logic for ucInventoryAdjustmentGrid.xaml
    /// </summary>
    public partial class ucInventoryAdjustmentGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        InventoryAdjustmentStatus oldStatus = new InventoryAdjustmentStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        AdjustmentRepo adjustmentRepo = new AdjustmentRepo();
        InventoryAdjustment adjustment = new InventoryAdjustment();
        public IList<InventoryAdjustment> adjustments { get; set; }
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        public ucInventoryAdjustmentGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inventory Adjustment List") != null)
                {

                    ApprovalCount = adjustmentRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {
                        ApprovalCount = adjustmentRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = adjustmentRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
            }
            else
            {
                ApprovalCount = adjustmentRepo.getAllPendingForAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inventory Adjustment List") != null)
                {
                    ClosingCount = adjustmentRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {
                        ClosingCount = adjustmentRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = adjustmentRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inventory Adjustment Register") != null)
                {
                    mbtnRegister.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {
                        ApproveunapprovedCount = adjustmentRepo.getInventoryAdjustmentRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = adjustmentRepo.getInventoryAdjustmentRegisterCountOWn();
                    }
                }
                else
                {
                    mbtnRegister.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ClosingCount = adjustmentRepo.getAllPendingForClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inventory Adjustment") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = adjustmentRepo.getVoidRegisterCount(MainWindow.currentUserid);
                }
                else
                {
                    VoidCount = adjustmentRepo.getVoidRegisterAdministratorCount();
                }
            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadInventoryAdjustmentgrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void loadInventoryAdjustmentgrid()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Inventory Adjustment(Open)" || lblHeading.Text == "Inventory Adjustment(Closed)")
            { mbtnExportToReport.IsEnabled = false; }


            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        adjustments = adjustmentRepo.getAll();


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inventory Adjustment") != null)
                    {

                        adjustments = adjustmentRepo.getAll(MainWindow.currentUserid);
                    }
                    else
                    {

                        adjustments = adjustmentRepo.getAllActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {

                    adjustments = adjustmentRepo.getAllActive(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {

                    adjustments = adjustmentRepo.getAllInActive(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Pending For Approvals (Inventory Adjustment)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inventory Adjustment List") != null)
                        {
                            adjustments = adjustmentRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustments") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                            {
                                adjustments = adjustmentRepo.getAllPendingForApproval(MainWindow.currentUserid);
                            }
                            else
                            {
                                adjustments = adjustmentRepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);
                            }
                        }
                    else

                        adjustments = adjustmentRepo.getAllPendingForAdministrator(MainWindow.currentUserid);
                }
                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Inventory Adjustment";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inventory Adjustment List") != null)
                        {
                            adjustments = adjustmentRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                            {
                                adjustments = adjustmentRepo.getAllPendingForClosing(MainWindow.currentUserid);
                            }
                            else
                            {
                                adjustments = adjustmentRepo.getAllPendingForClosingOwn(MainWindow.currentUserid);
                            }
                        }
                    else
                        adjustments = adjustmentRepo.getAllPendingForClosingAdministrator();
                }
            }
            else
            {

                adjustments = adjustmentRepo.getAllAdjustmentbyStatusId(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdInventoryAdjustments);
            grdInventoryAdjustments.ItemsSource = adjustments;
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            PrintableControlLink link = new PrintableControlLink((TableView)grdInventoryAdjustments.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            grdInventoryAdjustments.ShowLoadingPanel = true;
            bw.RunWorkerAsync();
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ucInventoryAdjustmentGrid ucPIGrid = new ucInventoryAdjustmentGrid();
            this.Content = ucPIGrid;
            grdInventoryAdjustments.ShowLoadingPanel = false;

        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(6000);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
            AllActive = 3;
            statusId = 0;
            lblHeading.Text = "Inventory Adjustments";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inventory Adjustment List") != null)
                {


                    adjustments = adjustmentRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {

                        adjustments = adjustmentRepo.getAllPendingForApproval(MainWindow.currentUserid);
                    }

                    else
                    {

                        adjustments = adjustmentRepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else

                adjustments = adjustmentRepo.getAllPendingForAdministrator(MainWindow.currentUserid);

            grdInventoryAdjustments.ItemsSource = adjustments;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            statusId = 0;
            AllActive = 4;
            lblHeading.Text = "(Pending for Closing) Inventory Adjustment";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inventory Adjustments List") != null)
                {
                    adjustments = adjustmentRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustments without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {
                        adjustments = adjustmentRepo.getAllPendingForClosing(MainWindow.currentUserid);
                    }
                    else
                    {
                        adjustments = adjustmentRepo.getAllPendingForClosingOwn(MainWindow.currentUserid);
                    }
                }

            else

                adjustments = adjustmentRepo.getAllPendingForClosingAdministrator();
            grdInventoryAdjustments.ItemsSource = adjustments;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            AllActive = 6;
            statusId = 0;
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Inventory Adjustments Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustments without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                {
                    adjustments = adjustmentRepo.getAdjustmentRegister(MainWindow.currentUserid);
                }

                else
                {
                    adjustments = adjustmentRepo.getAdjustmentRegister(MainWindow.currentUserid);
                }
            else
                adjustments = adjustmentRepo.getAdjustmentRegisterAdministrator();
            grdInventoryAdjustments.ItemsSource = adjustments;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdInventoryAdjustments);

        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Inventory Adjustment";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inventory Adjustment") != null)
                {
                    adjustments = adjustmentRepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    adjustments = adjustmentRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                adjustments = adjustmentRepo.getVoidRegisterAdministrator();
            grdInventoryAdjustments.ItemsSource = adjustments;
        }

        private void grdInventoryAdjustments_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (grdInventoryAdjustments.SelectedItem != null)
                {
                    winfrmAdjustInventory adjustment = new winfrmAdjustInventory();
                    adjustment.OrderId = (grdInventoryAdjustments.SelectedItem as InventoryAdjustment).Id;
                    adjustment.editOrder = 1;
                    adjustment.Show();
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnNewAdjustment_Click(object sender, RoutedEventArgs e)
        {
            winfrmAdjustInventory winfrmAdjustInventory = new winfrmAdjustInventory();
            winfrmAdjustInventory.Show();
        }
    }
}
