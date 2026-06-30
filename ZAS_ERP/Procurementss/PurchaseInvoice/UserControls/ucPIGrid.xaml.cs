using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.PurchaseInvoice.UserControls
{
    /// <summary>
    /// Interaction logic for ucPIGrid.xaml
    /// </summary>
    public partial class ucPIGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        PurchaseInvoiceStatus oldStatus = new PurchaseInvoiceStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static List<MarketExchangeRate> marketexchangeRates = new List<MarketExchangeRate>();
        public ucPIGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseInvoice List") != null)
                {

                    ApprovalCount = purchaseInvoiceRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                        ApprovalCount = purchaseInvoiceRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        ApprovalCount = purchaseInvoiceRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }

                }
            }

            else
            {
                ApprovalCount = purchaseInvoiceRepo.getAllPendingForAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseInvoice List") != null)
                {
                    ClosingCount = purchaseInvoiceRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                        ClosingCount = purchaseInvoiceRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = purchaseInvoiceRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Register") != null)
                {
                    mbtnRegister.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                        ApproveunapprovedCount = purchaseInvoiceRepo.getPurchaseInvoiceRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = purchaseInvoiceRepo.getInvoiceRegisterCountOWn();
                    }
                }
                else
                {
                    mbtnRegister.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ClosingCount = purchaseInvoiceRepo.getAllPendingForClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void PurchaseInvoices") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = purchaseInvoiceRepo.getVoidRegisterCount(MainWindow.currentUserid);

                }
                else
                {
                    VoidCount = purchaseInvoiceRepo.getVoidRegisterAdministratorCount();

                }


            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
        }
        PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
        ERP_BL.Databases.PurchaseInvoice purchaseInvoice = new ERP_BL.Databases.PurchaseInvoice();
        public IList<ERP_BL.Databases.PurchaseInvoice> purchaseInvoices { get; set; }
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }

        private void BtnNewPurchaseInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditPurchaseInvoice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdpurchaseInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPurchaseInvoice();
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    e.Value = deptList[0].DeptName;
                }
            }
            if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[1].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[2].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[3].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[4].DeptName;
                            break;
                    }
                }
            }


            if (e.Column.FieldName == "PaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseInvoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PO.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        if (PO.tax != null)
                        {
                            amountWithTax = paidAmount + (paidAmount * Convert.ToDecimal(PO.tax.percentage) / 100);
                        }
                        else
                        {
                            amountWithTax = paidAmount + Convert.ToDecimal(PO.totaltaxAmount);
                        }
                    e.Value = amountWithTax;
                }
            }

            if (e.Column.FieldName == "UnpaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PI = purchaseInvoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PI.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            //paidAmount = paidAmount + ((paidAmount * Convert.ToDecimal(PI.PurchaseOrder.tax.percentage)) / 100);
                            
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount*PI.PurchaseOrder.tax.percentage)/100) - paidAmount, 2);
                        }
                        else
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + PI.totaltaxAmount) - paidAmount, 2);
                        }
                    }
                    else
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount * PI.PurchaseOrder.tax.percentage) / 100),2);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PI.totalInvoiceAmount+ PI.totaltaxAmount);
                        }
                    }
                    e.Value = amountWithTax;
                }
            }
        }
        private void EditPurchaseInvoice()
        {
            if (grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void mbtnPartially_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;
            //});
            //statusId = 0;
            //AllActive = 8;
            //_usersRepo usersRepo = new _usersRepo();

            //lblHeading.Text = "Partially Invoiced";
            //if (MainWindow.currentUserid != 0)
            //    saleInvoices = saleInvoicerepo.getPartiallyInvoicedFirst(MainWindow.currentUserid);
            //else
            //    saleInvoices = saleInvoicerepo.getVoidRegisterAdministratorFirst();
            //grdsaleInvoice.ItemsSource = saleInvoices;
            //grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    Mouse.OverrideCursor = Cursors.Arrow;
            //});
        }

        private void mbtnFullyInvoiced_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;
            //});
            //statusId = 0;
            //AllActive = 7;
            //_usersRepo usersRepo = new _usersRepo();

            //lblHeading.Text = "Fully Invoiced";
            //if (MainWindow.currentUserid != 0)
            //    purchaseInvoices = purchaseInvoiceRepo.getFullyInvoiced(MainWindow.currentUserid);
            //else
            //    purchaseInvoices = purchaseInvoiceRepo.getVoidRegisterAdministratorFirst();
            //grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            //grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    Mouse.OverrideCursor = Cursors.Wait;
            //});

        }
       

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Purchase Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleInvoices") != null)
                {
                    purchaseInvoices = purchaseInvoiceRepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    purchaseInvoices = purchaseInvoiceRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                purchaseInvoices = purchaseInvoiceRepo.getVoidRegisterAdministrator();
            grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdpurchaseInvoice);

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

            lblHeading.Text = "Purchase Invoice Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                {
                    purchaseInvoices = purchaseInvoiceRepo.getSaleInvoiceRegisterFirst(MainWindow.currentUserid);
                }

                else
                {
                    purchaseInvoices = purchaseInvoiceRepo.getSaleInvoiceRegisterFirst(MainWindow.currentUserid);
                }

            else
                
                purchaseInvoices = purchaseInvoiceRepo.getInvoiceRegisterAdministratorFirst();
            grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
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
            lblHeading.Text = "(Pending for Closing) Purchase Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseInvoice List") != null)
                {
                   ;
                    purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                        
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                       ;
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
         
            else
                
                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingAdministratorFirst();
            grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            
            grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
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
            lblHeading.Text = "Pending Purchase Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseInvoice List") != null)
                {

                    
                    purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                    {
                     
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
        
                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForAdministratorFirst();

            grdpurchaseInvoice.ItemsSource = purchaseInvoices;

            grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null) ? true : false)
            {
                if (grdpurchaseInvoice.GetFocusedRow() != null)
                {
                    ERP_BL.Databases.PurchaseInvoice purchaseInvoice = new ERP_BL.Databases.PurchaseInvoice();
                    purchaseInvoice = grdpurchaseInvoice.SelectedItem as ERP_BL.Databases.PurchaseInvoice;
                    
                    UsersRepo usersRepo = new UsersRepo();

                    if (purchaseInvoice.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null) ? true : false)
                        {
                            purchaseInvoice.isApproved = true;
                            purchaseInvoice.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, purchaseInvoice.Id, 7, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                        {
                            purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                            if (purchaseInvoice.isApproved == null)
                            {
                                purchaseInvoice.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null)
                        {
                            purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (purchaseInvoice.isApproved == null)
                            {
                                purchaseInvoice.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else
                        {
                            purchaseInvoice.isApproved = false;
                        }




                    purchaseInvoiceRepo.updateForDirectClose(purchaseInvoice);
                    MessageBox.Show("PurchaseInvoice is Approved (" + purchaseInvoice.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "PurchaseInvoice is Approved (" + purchaseInvoice.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve PurchaseInvoice Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve PurchaseInvoice Directly user id=(" + MainWindow.currentUserid + ")");

            }
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null) ? true : false)
            {

                if (grdpurchaseInvoice.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    var row = grdpurchaseInvoice.GetFocusedRow() as ERP_BL.Databases.PurchaseInvoice;



                    if (row.PurchaseInvoiceStatus != null)
                    {
                        oldStatus = row.PurchaseInvoiceStatus;
                    }
                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid = (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id"));
                    ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange statusChange = new ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null) ? true : false)
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = false;
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null)
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else
                        {
                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                            ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;
                        }
                    //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.ClosingDate = System.DateTime.Now;
                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.LastStatusChangeDate = System.DateTime.Now;
                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceRepo.updateForDirectClose(ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice);
                    PurchaseOrderss.ucStatuschange.purchaseOrderid = (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("purchaseOrder_Id"));
                    PurchaseOrderss.frmPurchaseOrderStatusChange statChange = new PurchaseOrderss.frmPurchaseOrderStatusChange();
                    PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                    statChange.Owner = myWindow;
                    statChange.ShowDialog();
                    //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                    PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                    PurchaseOrderss.ucStatuschange.UpdatePurchaseOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                    MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                    MessageBox.Show("PurchaseInvoice status changed to InActive (" + ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PurchaseInvoiceStatus.Status + ")");

                    //Adding auto Signature
                    //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                    //{
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Purchase Invoice has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Purchase_Invoice);
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
                    string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Purchase Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close PurchaseInvoice Directly.");
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            grdpurchaseInvoice.ShowLoadingPanel = true;
            
            bw.RunWorkerAsync();

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ucPIGrid ucPIGrid = new ucPIGrid();

            this.Content = ucPIGrid;
            grdpurchaseInvoice.ShowLoadingPanel = false;

        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(6000);
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Purchase Invoices Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdpurchaseInvoice, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Purchase Invoices Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdpurchaseInvoice.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            loadPurchaseInvoicegrid();
            // loadMarketExchangeRates();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

       
        private void loadPurchaseInvoicegrid()
        {
           
          
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Purchase Invoices(Open)" || lblHeading.Text == "Purchase Invoices(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
           
          
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        purchaseInvoices = purchaseInvoiceRepo.getAll();
                       
                        
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Invoices") != null)
                    {
                       
                        purchaseInvoices = purchaseInvoiceRepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                       
                        purchaseInvoices = purchaseInvoiceRepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    
                    purchaseInvoices = purchaseInvoiceRepo.getAllActiveFirst(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    
                    purchaseInvoices = purchaseInvoiceRepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Purchase Invoices)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseInvoice List") != null)
                        {

                            
                            purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                            {
                               
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                            }

                            else
                            {
                                
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    else
                     
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForAdministratorFirst();

         
                    

                
                    //grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }

                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Purchase Invoices";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseInvoice List") != null)
                        {
                            
                            purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                            {
                              
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                              
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                            }
                        }
                   
                    else
                     
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingAdministratorFirst();
                  
                   
                 
                    //grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                
                purchaseInvoices = purchaseInvoiceRepo.getAllPobyStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdpurchaseInvoice);
            grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            RemoveSourceObjects();
            
          
        }
       
        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

       
                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

           
                return;

            }
            else
            {
               

                LoadOrdersByDate();
                RemoveSourceObjects();
                Application.Current.Dispatcher.Invoke(() =>
                {
                   
                    Mouse.OverrideCursor = null;
                });

            }
        }
        public void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Purchase Invoices(Open)" || lblHeading.Text == "Purchase Invoices(Closed)")
            { mbtnExportToReport.IsEnabled = false; }

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                      
                        purchaseInvoices = purchaseInvoiceRepo.getAll();

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Invoices") != null)
                    {
                        purchaseInvoices = purchaseInvoiceRepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                    else
                    {
                        
                        purchaseInvoices = purchaseInvoiceRepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    purchaseInvoices = purchaseInvoiceRepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    purchaseInvoices = purchaseInvoiceRepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

            
                    lblHeading.Text = "Pending For Approvals (Purchase Invoices)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseInvoice List") != null)
                        {

                
                            purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                            {
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }

            
                            else
                            {
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                 
                            }
                        }
                    else
         
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);



                    grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }

                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Purchase Invoices";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseInvoice List") != null)
                        {
                    
                  
                            purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                            {
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

       
                            }
                        }
                    else
                        purchaseInvoices = purchaseInvoiceRepo.getAllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                }
              
                else if (AllActive == 6)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Purchase Invoice Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                        {
                            purchaseInvoices = purchaseInvoiceRepo.getSaleInvoiceRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }

                        else
                        {
                            purchaseInvoices = purchaseInvoiceRepo.getSaleInvoiceRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }

                
                    else
                        purchaseInvoices = purchaseInvoiceRepo.getInvoiceRegisterAdministrator();
                    grdpurchaseInvoice.ItemsSource = purchaseInvoices;
                    grdpurchaseInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                purchaseInvoices = purchaseInvoiceRepo.getAllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdpurchaseInvoice);
            grdpurchaseInvoice.ItemsSource = purchaseInvoices;
            RemoveSourceObjects();
        }
        public void RemoveSourceObjects()
        {
            grdpurchaseInvoice.Columns.GetColumnByFieldName("Id").Visible = false;

            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("company"));


            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("currency"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("PurchaseInvoiceStatus"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("vendor"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("employee"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("department"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("customerCompany"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("company_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("currency_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("allocation_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("dept_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("user_Id"));
            grdpurchaseInvoice.Columns.Remove(grdpurchaseInvoice.Columns.GetColumnByFieldName("user"));
        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
                {
                    switch (cmbxDepartmentFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 0:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 1:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {

                                case 1:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Department Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
                {
                    cmbxDepartmentTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater department!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxDepartmentTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
