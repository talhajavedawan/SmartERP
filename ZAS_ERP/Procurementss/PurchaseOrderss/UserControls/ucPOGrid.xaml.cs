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
using ERP_BL.Databases;
using ERP_BL;
using DevExpress.Xpf.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using System.Collections.ObjectModel;
using ERP_BL.Enums;
using System.Windows.Threading;
using ZAS_ERP.Reportss;
using ERP_BL.ExchangeRates;

namespace ZAS_ERP.Procurementss.PurchaseOrderss
{
    /// <summary>
    /// Interaction logic for ucPurchaseOrderGrid.xaml
    /// </summary>
    public partial class ucPurchaseOrderGrid : UserControl
    {
        //commentadded
        public static int statusId;
        public static int AllActive;
        PurchaseOrderStatus oldStatus = new PurchaseOrderStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        //NotificationsRepo notificationsRepo = new NotificationsRepo();
        static List<ExchangeRateGroup> exchangeRateGroups = new List<ExchangeRateGroup>();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        ExchangeRate exchangeRate = null;
        public ucPurchaseOrderGrid()
        {

            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                {
                    ApprovalCount = purchaseOrderrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        ApprovalCount = purchaseOrderrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = purchaseOrderrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) PurchaseOrder List") != null)
                {
                    ReApprovalCount = purchaseOrderrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        ReApprovalCount = purchaseOrderrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = purchaseOrderrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void PurchaseOrders") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = purchaseOrderrepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = purchaseOrderrepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add PurchaseOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        ApproveunapprovedCount = purchaseOrderrepo.getPurchaseRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = purchaseOrderrepo.getPurchaseRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = purchaseOrderrepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = purchaseOrderrepo.getPurchaseRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                {
                    ClosingCount = purchaseOrderrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        ClosingCount = purchaseOrderrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = purchaseOrderrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
            }

            else
            {
                ClosingCount = purchaseOrderrepo.getAllPendingForClosingAdministratorCount();
            }
            //this.DataContext = this;
        }

        PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        public IList<ERP_BL.Databases.PurchaseOrder> purchaseOrders { get; set; }
        //public ObservableCollection<PurchaseOrder> orders { get; set; }
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucpurchaseOrderGrid_Loaded(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
        
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            //SystemLogic.SetUserSettingOfCurrentWindow(grdpurchaseOrderr);
            loadMarketExchangeRates();
            loadPurchaseOrdergrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }
        private void loadMarketExchangeRates()
        {
            //CurrencyRepo currencyRepo = new CurrencyRepo();
            //marketexchangeRates = currencyRepo.getAllMarketExchangeRatesForToday();
            exchangeRateGroups = exchangeRateGroupRepo.GetAllMER();
        }
    
        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            
            {

                var po = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;

                if (e.Column.FieldName == "LotNumberr")
                {
                    string LotNo = "";
                    if (po.lotNumber != null)
                        LotNo = po.lotNumber.LotNo;
                    else if (!String.IsNullOrEmpty(po.lotNo))
                        LotNo = po.lotNo;
                    e.Value = LotNo;
                }

                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if(row != null)
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
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if(row != null)
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
                        //if (deptList.Count == 1)
                        //{
                        //    e.Value = deptList[0].DeptName;
                        //}
                        //if (deptList.Count == 2)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 3)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 4)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 5)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}

                        //int index = 1;
                        //if (index >= 0 && index < deptList.Count)
                        //{

                        //    e.Value = deptList[1].DeptName;
                        //}

                            //if (deptList.Count <= 1)
                            //{
                            //    e.Value = dep;
                            //}
                            //else
                            //{
                            //    int index = 1;
                            //    if (index >= 0 && index < deptList.Count)
                            //    {

                            //        e.Value = deptList[1].DeptName;
                            //        dep = deptList[1].DeptName;
                            //    }
                            //}

                        }

                }
                if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if(row != null)
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

                        //if (deptList.Count == 1)
                        //{
                        //    e.Value = deptList[0].DeptName;
                        //}
                        //if (deptList.Count == 2)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 3)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}
                        //if (deptList.Count == 4)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}
                        //if (deptList.Count == 5)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}


                        //if (deptList.Count <= 2)
                        //{
                        //    e.Value = dep;
                        //}
                        //else
                        //{
                        //    int index = 2;
                        //    if (index >= 0 && index < deptList.Count)
                        //    {

                        //        e.Value = deptList[2].DeptName;
                        //        dep = deptList[2].DeptName;
                        //    }
                        //}


                    }
                  
                }
                if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if(row != null)
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

                        //if (deptList.Count == 1)
                        //{
                        //    e.Value = deptList[0].DeptName;
                        //}
                        //if (deptList.Count == 2)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 3)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}
                        //if (deptList.Count == 4)
                        //{
                        //    e.Value = deptList[3].DeptName;
                        //}
                        //if (deptList.Count == 5)
                        //{
                        //    e.Value = deptList[3].DeptName;
                        //}
                        //if (deptList.Count <= 3)
                        //{
                        //    e.Value = dep;
                        //}
                        //else
                        //{
                        //    int index = 3;
                        //    if (index >= 0 && index < deptList.Count)
                        //    {
                        //        e.Value = deptList[3].DeptName;
                        //        dep = deptList[3].DeptName;
                        //    }
                        //}

                    }
                 
                }
                if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if(row != null)
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


                        //if (deptList.Count == 1)
                        //{
                        //    e.Value = deptList[0].DeptName;
                        //}
                        //if (deptList.Count == 2)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 3)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}
                        //if (deptList.Count == 4)
                        //{
                        //    e.Value = deptList[3].DeptName;
                        //}
                        //if (deptList.Count == 5)
                        //{
                        //    e.Value = deptList[4].DeptName;
                        //}

                        //if (deptList.Count <= 4)
                        //{
                        //    e.Value = dep;
                        //}
                        //else
                        //{
                        //    int index = 4;
                        //    if (index >= 0 && index < deptList.Count)
                        //    {
                        //        e.Value = deptList[4].DeptName;
                        //        dep = deptList[4].DeptName;
                        //    }
                        //}

                    }
                  
                }
                //if (e.Column.FieldName == "PaymentDueAgeingDays" && e.IsGetData)
                //{
                //    if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                //    {
                //        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));

                //        //DateTime date;
                //        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                //        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                //        e.Value = NoDueAgeingDays;
                //    }
                //}
                if (e.Column.FieldName == "SODateAgeing" && e.IsGetData)
                {
                        var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                        if (row.saleOrder_Id != null)
                        {
                            DateTime dateTime = (DateTime)row.SaleOrder.saleOrderDate;
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                    
                }
                if (e.Column.FieldName == "soDeliveryAging" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (row.saleOrder_Id != null)
                    {
                        if (row.SaleOrder.deliveryDate != null)
                        {
                            DateTime dateTime = (DateTime)row.SaleOrder.deliveryDate;
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                    }

                }
                if (e.Column.FieldName == "soDeliveryFinalAging" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (row.saleOrder_Id != null)
                    {
                        if (row.SaleOrder.deliveryDateFinal != null)
                        {
                            DateTime dateTime = (DateTime)row.SaleOrder.deliveryDateFinal;
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                    }

                }
                if (e.Column.FieldName == "revisedShipmentAging" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (row.saleOrder_Id != null)
                    {
                        if (row.RevisedShipmentDate != null)
                        {
                            DateTime dateTime = (DateTime)row.RevisedShipmentDate;
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                    }

                }
                if (e.Column.FieldName == "ocShipmentAging" && e.IsGetData)
                {
                    var row = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (row.saleOrder_Id != null)
                    {
                        if (row.ShipmentDate != null)
                        {
                            DateTime dateTime = (DateTime)row.ShipmentDate;
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                    }

                }


                if (e.Column.FieldName == "Department")

                {

                    // string s = "Test: FieldTwo";

                }
                if (e.Column.FieldName == "CreationAgeing")

                {
                    if (e.GetListSourceFieldValue("CreationDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));

                        Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = CreateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODateAgeing")

                {
                    if (e.GetListSourceFieldValue("purchaseOrderDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("purchaseOrderDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODeliveryAgeing")

                {
                    if (e.GetListSourceFieldValue("deliveryDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalWeight")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        decimal? totalweight = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.Weight != null || pro.inquiryProduct.Weight != 0)
                            {
                                totalweight = pro.inquiryProduct.Weight;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalweight;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalQuantity")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        double totalquantity = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.quantity != 0)
                            {
                                totalquantity = pro.inquiryProduct.quantity;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalquantity;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "BudgetMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount!=0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin;
                            e.Value = result;
                        }
                        else
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin;
                            e.Value = result;
                        }
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {

                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) * exchangerate;
                            e.Value = result;
                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                            e.Value = result;
                        }

                        
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMargininBase"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {

                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) * exchangerate;
                            e.Value = result;
                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                            e.Value = result;
                        }
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(SO.costCenterAmount);
                            var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) / total) * 100;
                            e.Value = result;
                        }
                        else
                        {

                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                            var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) / total) * 100;
                            e.Value = result;
                        }
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMarginPercent"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin;
                            e.Value = result;
                        }
                        else
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin;
                            e.Value = result;
                        }
                           
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargin"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) * exchangerate;
                            e.Value = result;
                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                            e.Value = result;
                        }
                    }
                         
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargininBase"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) * exchangerate;
                            e.Value = result;

                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                            e.Value = result;
                        }
                           
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesActualMargin"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(SO.costCenterAmount);
                            var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) / total) * 100;
                            e.Value = result;
                        }
                        else
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                            var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) / total) * 100;
                            e.Value = result;
                        }
                           
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMarginPercent"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin;
                            e.Value = result;
                        }
                        else
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin;
                            e.Value = result;
                        }
                            
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargin"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) * exchangerate;
                            e.Value = result;
                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                            e.Value = result;
                        }
                            
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargininBase"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) * exchangerate;
                            e.Value = result;
                        }
                        else
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                            e.Value = result;
                        }
                           
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesRevisedMargin"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(SO.costCenterAmount);
                            var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) / total) * 100;
                            e.Value = result;
                        }
                        else
                        {
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                            var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) / total) * 100;
                            e.Value = result;
                        }
                            
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
                    }
                    
                }
                if (e.Column.FieldName == "InvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32( e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var invoicedAmount =Convert.ToDecimal(PO.PurchaseInvoices.Sum(x => x.totalInvoiceAmount));
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0 )
                            if(PO.tax != null)
                            {
                                amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal( PO.tax.percentage)) / 100);
                            }
                            else
                            {
                                amountWithTax = invoicedAmount +Convert.ToDecimal(PO.totaltaxAmount);
                            }
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "UninvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var invoicedAmount = Convert.ToDecimal(PO.PurchaseInvoices.Where(x=>x.isVoid != true).Sum(x => x.totalInvoiceAmount));
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Where(x=>x.isVoid!= true).ToList().Count > 0)
                        {
                            if (PO.tax != null && PO.billWithTax != null)
                            {
                                invoicedAmount = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                                amountWithTax =Convert.ToDecimal(PO.billWithTax.Value) - invoicedAmount;
                            }
                            if (PO.totaltaxAmount!=0)
                            {
                                invoicedAmount = invoicedAmount + Convert.ToDecimal(PO.totaltaxAmount);
                                amountWithTax = Convert.ToDecimal(PO.billWithTax.Value) - invoicedAmount;
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(PO.totalCFRValue) - invoicedAmount;
                            }
                            
                        }
                        else
                        {
                            if (PO.tax != null && PO.billWithTax != null)
                            {
                                amountWithTax = Convert.ToDecimal(PO.billWithTax.Value);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(PO.totalCFRValue);
                            }
                        }  
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "PaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0)
                            foreach(var _PI in PO.PurchaseInvoices)
                            {
                                amountWithTax = amountWithTax + Convert.ToDecimal( _PI.Payments.Where(x => x.isVoid != true).Sum(x=>x.DebitedAmount));
                            }
                            //if (PO.tax != null)
                            //{
                            //    amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                            //}
                            //else
                            //{
                            //    amountWithTax = invoicedAmount;
                            //}
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "UnpaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        decimal unPaidAmount = 0;
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0)
                            foreach (var _PI in PO.PurchaseInvoices)
                            {
                                amountWithTax = Convert.ToDecimal( amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)));
                            }
                        if(PO.billWithTax != null)
                        {
                            unPaidAmount =Convert.ToDecimal( PO.billWithTax) - amountWithTax;
                        }
                        else
                        {
                            unPaidAmount = Convert.ToDecimal(PO.totalCFRValue+PO.totaltaxAmount) - amountWithTax;
                        }
                        e.Value = unPaidAmount;
                    }
                }
                if (e.Column.FieldName == "CMER")
                {
                    var purchaseOrder = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    double todayRate = 0;
                    var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == purchaseOrder.currency_Id && x.base_currency_Id == purchaseOrder.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == purchaseOrder.company_Id);
                        switch (DateTime.Now.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    todayRate = 0;
                                break;
                        }
                    }
                    var total = todayRate;
                    e.Value = total;
                }
                if (e.Column.FieldName == "SOAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var PO = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                        if (PO.saleOrder_Id != null)
                        {
                            e.Value = PO.SaleOrder.totalCFRValue;
                        }
                    }
                }
                if (e.Column.FieldName == "SOInvoiced")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var PO = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;

                        if (PO.saleOrder_Id != null)
                        {
                            e.Value = PO.SaleOrder.SaleInvoices.Sum(x => x.totalInvoiceAmount);
                        }
                    }
                }
                if (e.Column.FieldName == "daysLeft")
                {
                    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    var PO = grdpurchaseOrderr.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (PO.ExpectedPayment != null)
                    {
                        var date = (DateTime)PO.ExpectedPayment;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);
                        e.Value = timeSpan.Days;
                    }
                }
            }
        }
        /// <summary>
        /// Loads data into purchaseOrder Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadPurchaseOrdergrid()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Purchase Orders(Open)" || lblHeading.Text == "Purchase Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
 

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        purchaseOrders = purchaseOrderrepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Orders") != null)
                    {
                        purchaseOrders = purchaseOrderrepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        purchaseOrders = purchaseOrderrepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    purchaseOrders = purchaseOrderrepo.getAllActiveFirst(MainWindow.currentUserid);
                }
                else if (AllActive == 2)
                {
                    purchaseOrders = purchaseOrderrepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    try
                    {

                        lblHeading.Text = "Pending For Approvals (Purchase Orders)";
                        if (MainWindow.currentUserid != 0)
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                {
                                    purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                                }

                                else
                                {
                                    purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);
                                }
                            }
                        else
                            purchaseOrders = purchaseOrderrepo.getAllPendingForAdministrator();
                        //grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
                    }
                    catch (Exception ex)
                    {

                        DXMessageBox.Show(ex.Message, "ucPOGrid Line 666");
                    }
                    //_usersRepo usersRepo = new _usersRepo();

                }
                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Purchase Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                        {
                            purchaseOrders = purchaseOrderrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);
                            }
                        }
                    else
                        purchaseOrders = purchaseOrderrepo.getAllPendingForClosingAdministratorFirst();
                    //grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                purchaseOrders = purchaseOrderrepo.getAllPobyStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdpurchaseOrderr);
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
            RemoveSourceObjects();               
        }
        private void grdpurchaseOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPurchaseOrder();
        }

        private void EditPurchaseOrder()
        {
            if (grdpurchaseOrderr.GetFocusedRowCellValue(grdpurchaseOrderr.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)grdpurchaseOrderr.GetFocusedRowCellValue(grdpurchaseOrderr.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void btnNewPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, 0);
            procurmentPanel.Show();
        }

        private void btnEditPurchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            EditPurchaseOrder();
        }

        private void UcpurchaseOrdergrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdpurchaseOrderr);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdpurchaseOrderr.View.ShowPrintPreview(this);
            //ShowDesigner(tableView);
        }
        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ShowDesigner(tableView);
        }
        // Initializes and runs a Report Designer. 
        public static void ShowDesigner(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        {
            var report = new XtraReport();
            ReportGenerationExtensions<ColumnWrapper, RowBaseWrapper>.Generate(report, factory);
            Reportss.frmReportPanel frmReport = new Reportss.frmReportPanel(report);
            frmReport.Show();
            //var reportDesigner = new ReportDesigner();
            //reportDesigner.Loaded += (s, e) => {
            //    reportDesigner.OpenDocument(report);
            //};
            //reportDesigner.ShowWindow(factory as FrameworkElement);
        }

        private void MbtnReportCreate_Click(object sender, RoutedEventArgs e)
        {
            ShowDesigner(tableView);
        }
        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Purchase Orders Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
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
                        ReportLogic.SaveGridReport(grdpurchaseOrderr, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Purchase Orders Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.RemoveFromVisualTree();
            PurchaseOrderss.ucPurchaseOrderGrid ucPurchaseOrderGrid = new ucPurchaseOrderGrid();
            this.Content = ucPurchaseOrderGrid;
            //InitializeComponent();
            //loadPurchaseOrdergrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdpurchaseOrderr.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdpurchaseOrderr.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null) ? true : false)
            {

                if (grdpurchaseOrderr.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    var row = grdpurchaseOrderr.GetFocusedRow() as PurchaseOrder;
                    if (row.PurchaseOrderStatus != null)
                    {
                        oldStatus = row.PurchaseOrderStatus;
                    }
                    PurchaseOrderss.ucStatuschange.inActiveStatuses = 1;

                    PurchaseOrderss.ucStatuschange.purchaseOrderid = (int)grdpurchaseOrderr.GetFocusedRowCellValue(grdpurchaseOrderr.Columns.GetColumnByFieldName("Id"));
                    PurchaseOrderss.frmPurchaseOrderStatusChange statusChange = new PurchaseOrderss.frmPurchaseOrderStatusChange(purchaseOrderrepo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    //if (PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Id == row.PurchaseOrderStatus.Id)
                    //    return;
                    if (PurchaseOrderss.ucStatuschange.purchaseOrder.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null) ? true : false)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = false;
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.purchaseOrderRepo.update(Inquiriess.ucStatuschange.purchaseOrder);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                            if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing == null)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing != true)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            }
                            //PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                            PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                    //PurchaseOrderss.ucStatuschange.purchaseOrder.user_Id = MainWindow.currentUserid;
                    PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                    PurchaseOrderss.ucStatuschange.purchaseOrder.ClosingDate = System.DateTime.Now;
                    if (row.PurchaseOrderStatus != PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                    //Adding signature (comment)

                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("PO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id),row.Id, TransactionItemType.Purchase_Order);
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
                    string newStat = PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of PO (Amount OC) having value: " + row.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    row = PurchaseOrderss.ucStatuschange.purchaseOrder;
                    grdpurchaseOrderr.RefreshData();
                    purchaseOrderrepo.updateStatus(row.Id, row.PurchaseOrderStatus);
                    //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();//purchaseOrderRepo.update(PurchaseOrderss.ucStatuschange.purchaseOrder);
                    MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");

                }

            }
            else
            {
                MessageBox.Show("You are not Allowed to Close PurchaseOrder Directly.");
            }
        }
        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Purchase Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                {
                    purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);
                    }
                }
            else
                purchaseOrders = purchaseOrderrepo.getAllPendingForAdministratorFirst();
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
            grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
            AllActive = 3;
            statusId = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending for ReApprovals Purchase Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) PurchaseOrder List") != null)
                {
                    purchaseOrders = purchaseOrderrepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForReApproval(MainWindow.currentUserid);
                    }

                    else
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForReApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else
                purchaseOrders = purchaseOrderrepo.getAllPendingForAdministrator();

            grdpurchaseOrderr.ItemsSource = purchaseOrders;

            grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;

            statusId = 0;
            AllActive = 5;
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
            lblHeading.Text = "(Pending for Closing) Purchase Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                {
                    purchaseOrders = purchaseOrderrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        purchaseOrders = purchaseOrderrepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                purchaseOrders = purchaseOrderrepo.getAllPendingForClosingAdministratorFirst();
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
            grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
            AllActive = 4;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null) ? true : false)
            {
                if (grdpurchaseOrderr.GetFocusedRow() != null)
                {
                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                    purchaseOrder = grdpurchaseOrderr.SelectedItem as PurchaseOrder;

                    UsersRepo usersRepo = new UsersRepo();

                    if (purchaseOrder.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null) ? true : false)
                        {
                            purchaseOrder.isApproved = true;
                            purchaseOrder.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                        {
                            purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                            if (purchaseOrder.isApproved == null)
                            {
                                purchaseOrder.isApproved = false;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                        {
                            purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (purchaseOrder.isApproved == null)
                            {
                                purchaseOrder.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else
                        {
                            purchaseOrder.isApproved = false;
                        }
                    else //reapprove when it is approved already
                    {
                        if (purchaseOrder.isReApproved == false)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added PurchaseOrder") != null) ? true : false)
                            {
                                purchaseOrder.isReApproved = true;
                                purchaseOrder.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (purchaseOrder.isReApproved == null)
                                {
                                    purchaseOrder.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (purchaseOrder.isReApproved == null)
                                {
                                    purchaseOrder.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else
                            {
                                purchaseOrder.isReApproved = false;
                            }

                    }


                    purchaseOrderrepo.update(purchaseOrder);
                    MessageBox.Show("PurchaseOrder is Approved (" + purchaseOrder.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "PurchaseOrder is Approved (" + purchaseOrder.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve PurchaseOrder Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve PurchaseOrder Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdpurchaseOrderr);
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
           
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Purchase Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                {
                    purchaseOrders = purchaseOrderrepo.getPurchaseRegisterFirst(MainWindow.currentUserid);
                }

                else
                {
                    purchaseOrders = purchaseOrderrepo.getPurchaseRegisterFirst(MainWindow.currentUserid);
                }

            else
                purchaseOrders = purchaseOrderrepo.getPurchaseRegisterAdministratorFirst();
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
            grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
            statusId = 0;
            AllActive = 6;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Purchase Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void PurchaseOrders") != null)
                {
                    purchaseOrders = purchaseOrderrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    purchaseOrders = purchaseOrderrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                purchaseOrders = purchaseOrderrepo.getVoidRegisterAdministrator();
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
            grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
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
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Purchase Orders(Open)" || lblHeading.Text == "Purchase Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        purchaseOrders = purchaseOrderrepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Orders") != null)
                    {
                        purchaseOrders = purchaseOrderrepo.getAllByDateRange((DateTime)dateFrom.EditValue,(DateTime)dateTo.EditValue,MainWindow.currentUserid);
                    }
                    else
                    {
                        purchaseOrders = purchaseOrderrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    purchaseOrders = purchaseOrderrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    purchaseOrders = purchaseOrderrepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Purchase Orders)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) PurchaseOrder List") != null)
                        {
                            purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }

                            else
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                        purchaseOrders = purchaseOrderrepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    //grdpurchaseOrderr.ItemsSource = purchaseOrders;
                    grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
                }
                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Purchase Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) PurchaseOrder List") != null)
                        {
                            purchaseOrders = purchaseOrderrepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }

                            else
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                        purchaseOrders = purchaseOrderrepo.getAllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    //grdpurchaseOrderr.ItemsSource = purchaseOrders;

                }
                else if (AllActive == 5)
                {

                    lblHeading.Text = "Pending for ReApprovals Purchase Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) PurchaseOrder List") != null)
                        {
                            purchaseOrders = purchaseOrderrepo.getAllPendingForReApprovalDepartmentalByDateRange((DateTime) dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForReApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                            }

                            else
                            {
                                purchaseOrders = purchaseOrderrepo.getAllPendingForReApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);

                            }
                        }
                    else
                        purchaseOrders = purchaseOrderrepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    grdpurchaseOrderr.ItemsSource = purchaseOrders;

                    grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;

                }
                else if (AllActive == 6)
                {

                    lblHeading.Text = "Purchase Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                        {
                            purchaseOrders = purchaseOrderrepo.getPurchaseRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                        }

                        else
                        {
                            //Displaying all PO instead of OWN transcations
                            //purchaseOrders = purchaseOrderrepo.getPurchaseRegisterOwn(MainWindow.currentUserid);
                            purchaseOrders = purchaseOrderrepo.getPurchaseRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);

                        }

                    else
                        purchaseOrders = purchaseOrderrepo.getPurchaseRegisterAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    grdpurchaseOrderr.ItemsSource = purchaseOrders;
                    grdpurchaseOrderr.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                purchaseOrders = purchaseOrderrepo.getAllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            grdpurchaseOrderr.ItemsSource = purchaseOrders;
        }
        private void RemoveSourceObjects()
        {
            

        }

        private void MbtnCopyTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Copy Purchase Order Template") != null)
            {

                if (grdpurchaseOrderr.GetFocusedRowCellValue(grdpurchaseOrderr.Columns.GetColumnByFieldName("Id")) != null)
                {
                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)grdpurchaseOrderr.GetFocusedRowCellValue(grdpurchaseOrderr.Columns.GetColumnByFieldName("Id")),true);
                    procurmentPanel.Show();
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Copy Purchase Order Template", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);

            }
        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
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
                if(cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
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
