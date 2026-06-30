using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Printing;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ZAS_ERP.Reportss;


namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for ucSaleOrderGrid.xaml
    /// </summary>
    public partial class ucSaleOrderGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        ProcurementRepo procurementRepo = new ProcurementRepo();
        SaleOrderStatus oldStatus = new SaleOrderStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsSER = new List<ExchangeRateGroup>();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateSER = null;
        ExchangeRate exchangeRateMER = null;
        ExchangeRate exchangeRateCMER = null;


        public ucSaleOrderGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                {
                    ApprovalCount = saleOrderrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        ApprovalCount = saleOrderrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = saleOrderrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                {
                    ReApprovalCount = saleOrderrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        ReApprovalCount = saleOrderrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = saleOrderrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleOrders") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;
                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = saleOrderrepo.getVoidRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        VoidCount = saleOrderrepo.getVoidRegisterAdministratorCount();
                    }
                }
                else
                {
                    mbtnVoid.Visibility = Visibility.Collapsed;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleOrder without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        ApproveunapprovedCount = saleOrderrepo.getSaleRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = saleOrderrepo.getSaleRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }

            }

            else
            {
                ApprovalCount = saleOrderrepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = saleOrderrepo.getSaleRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                {
                    ClosingCount = saleOrderrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        ClosingCount = saleOrderrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = saleOrderrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }
            else
            {
                ClosingCount = saleOrderrepo.getAllPendingForClosingAdministratorCount();
            }
        }

        SaleOrderRepo saleOrderrepo = new SaleOrderRepo();
        SaleOrder saleOrder = new SaleOrder();
   
        //public IList<ERP_BL.Databases.SaleOrder> saleOrders { get; set; }
        List<ERP_BL.Databases.SaleOrder> saleOrders = new List<ERP_BL.Databases.SaleOrder>();

        //public ObservableCollection<SaleOrder> orders { get; set; }
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
        private void ucsaleOrderGrid_Loaded(object sender, RoutedEventArgs e)
        {

            //SystemLogic.SetUserSettingOfCurrentWindow(grdsaleOrder);
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            loadSaleOrdergrid();
            LoadSERGroups();
            LoadMERGroups();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }
        public void LoadSERGroups()
        {
            exchangeRateGroupsSER = exchangeRateGroupRepo.GetAllSER();
        }
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
        }
        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            try
            {
                var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                switch (e.Column.FieldName)
                {
                    case "departmentLevel1":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                            e.Value = deptList[0].Title;
                        }
                        break;
                    case "departmentLevel2":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                                    e.Value = deptList[0].Title;
                                    break;
                                case 2:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 3:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 4:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 5:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 6:
                                    e.Value = deptList[1].Title;
                                    break;
                            }
                        }
                        break;
                    case "departmentLevel3":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                                    e.Value = deptList[0].Title;
                                    break;
                                case 2:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 3:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 4:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 5:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 6:
                                    e.Value = deptList[2].Title;
                                    break;
                            }
                        }
                        break;
                    case "departmentLevel4":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                                    e.Value = deptList[0].Title;
                                    break;
                                case 2:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 3:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 4:
                                    e.Value = deptList[3].Title;
                                    break;
                                case 5:
                                    e.Value = deptList[3].Title;
                                    break;
                                case 6:
                                    e.Value = deptList[3].Title;
                                    break;
                            }
                        }
                        break;
                    case "departmentLevel5":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                                    e.Value = deptList[0].Title;
                                    break;
                                case 2:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 3:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 4:
                                    e.Value = deptList[3].Title;
                                    break;
                                case 5:
                                    e.Value = deptList[4].Title;
                                    break;
                                case 6:
                                    e.Value = deptList[4].Title;
                                    break;
                            }
                        }
                        break;
                    case "departmentLevel6":
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                                    e.Value = deptList[0].Title;
                                    break;
                                case 2:
                                    e.Value = deptList[1].Title;
                                    break;
                                case 3:
                                    e.Value = deptList[2].Title;
                                    break;
                                case 4:
                                    e.Value = deptList[3].Title;
                                    break;
                                case 5:
                                    e.Value = deptList[4].Title;
                                    break;
                                case 6:
                                    e.Value = deptList[5].Title;
                                    break;
                            }
                        }
                        break;
                }

                //if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }

                //        }
                //        deptList.Reverse();
                //        e.Value = deptList[0].Title;
                //    }
                //}
                //if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }
                //        }
                //        deptList.Reverse();
                //        switch (deptList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = deptList[0].Title;
                //                break;
                //            case 2:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 3:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 4:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 5:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 6:
                //                e.Value = deptList[1].Title;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }
                //        }
                //        deptList.Reverse();
                //        switch (deptList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = deptList[0].Title;
                //                break;
                //            case 2:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 3:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 4:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 5:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 6:
                //                e.Value = deptList[2].Title;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;

                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and tranverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }

                //        }
                //        deptList.Reverse();
                //        switch (deptList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = deptList[0].Title;
                //                break;
                //            case 2:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 3:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 4:
                //                e.Value = deptList[3].Title;
                //                break;
                //            case 5:
                //                e.Value = deptList[3].Title;
                //                break;
                //            case 6:
                //                e.Value = deptList[3].Title;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;

                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and tranverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }
                //        }
                //        deptList.Reverse();

                //        switch (deptList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = deptList[0].Title;
                //                break;
                //            case 2:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 3:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 4:
                //                e.Value = deptList[3].Title;
                //                break;
                //            case 5:
                //                e.Value = deptList[4].Title;
                //                break;
                //            case 6:
                //                e.Value = deptList[4].Title;
                //                break;
                //        }
                //    }
                //}

                //if (e.Column.FieldName == "departmentLevel6" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var deptList = new List<DepartmentLevel>();
                //        var node = row.department.departmentLevel;

                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and tranverse to its parent
                //                    deptList.Add(node);
                //                    node = node.parentLevel;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    deptList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                deptList.Add(node);
                //                break;
                //            }
                //        }
                //        deptList.Reverse();

                //        switch (deptList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = deptList[0].Title;
                //                break;
                //            case 2:
                //                e.Value = deptList[1].Title;
                //                break;
                //            case 3:
                //                e.Value = deptList[2].Title;
                //                break;
                //            case 4:
                //                e.Value = deptList[3].Title;
                //                break;
                //            case 5:
                //                e.Value = deptList[4].Title;
                //                break;
                //            case 6:
                //                e.Value = deptList[5].Title;
                //                break;
                //        }
                //    }
                //}


                switch (e.Column.FieldName)
                {
                    case "customerLevel1":
                        if (row != null)
                        {
                            var customerList = new List<CustomerCompany>();
                            var node = row.customerCompany;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        customerList.Add(node);
                                        node = node.parentCompany;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        customerList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    customerList.Add(node);
                                    break;
                                }

                            }
                            customerList.Reverse();
                            e.Value = customerList[0].company.CompanyName;
                        }
                        break;
                    case "customerLevel2":
                        if (row != null)
                        {
                            var customerList = new List<CustomerCompany>();
                            var node = row.customerCompany;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        customerList.Add(node);
                                        node = node.parentCompany;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        customerList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            customerList.Reverse();
                            switch (customerList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = customerList[0].company.CompanyName;
                                    break;
                                case 2:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 3:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 4:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 5:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                            }
                        }
                        break;
                    case "customerLevel3":
                        if (row != null)
                        {
                            var customerList = new List<CustomerCompany>();
                            var node = row.customerCompany;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        customerList.Add(node);
                                        node = node.parentCompany;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        customerList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            customerList.Reverse();
                            switch (customerList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = customerList[0].company.CompanyName;
                                    break;
                                case 2:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 3:
                                    e.Value = customerList[2].company.CompanyName;
                                    break;
                                case 4:
                                    e.Value = customerList[2].company.CompanyName;
                                    break;
                                case 5:
                                    e.Value = customerList[2].company.CompanyName;
                                    break;
                            }
                        }
                        break;
                    case "customerLevel4":
                        if (row != null)
                        {
                            var customerList = new List<CustomerCompany>();
                            var node = row.customerCompany;

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        customerList.Add(node);
                                        node = node.parentCompany;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        customerList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    customerList.Add(node);
                                    break;
                                }

                            }
                            customerList.Reverse();
                            switch (customerList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = customerList[0].company.CompanyName;
                                    break;
                                case 2:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 3:
                                    e.Value = customerList[2].company.CompanyName;
                                    break;
                                case 4:
                                    e.Value = customerList[3].company.CompanyName;
                                    break;
                                case 5:
                                    e.Value = customerList[3].company.CompanyName;
                                    break;
                            }
                        }
                        break;
                    case "customerLevel5":
                        if (row != null)
                        {
                            var customerList = new List<CustomerCompany>();
                            var node = row.customerCompany;

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        customerList.Add(node);
                                        node = node.parentCompany;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        customerList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            customerList.Reverse();

                            switch (customerList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = customerList[0].company.CompanyName;
                                    break;
                                case 2:
                                    e.Value = customerList[1].company.CompanyName;
                                    break;
                                case 3:
                                    e.Value = customerList[2].company.CompanyName;
                                    break;
                                case 4:
                                    e.Value = customerList[3].company.CompanyName;
                                    break;
                                case 5:
                                    e.Value = customerList[4].company.CompanyName;
                                    break;
                            }
                        }
                        break;
                }

                //if (e.Column.FieldName == "customerLevel1" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var customerList = new List<CustomerCompany>();
                //        var node = row.customerCompany;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    customerList.Add(node);
                //                    node = node.parentCompany;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    customerList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                customerList.Add(node);
                //                break;
                //            }

                //        }
                //        customerList.Reverse();
                //        e.Value = customerList[0].company.CompanyName;
                //    }
                //}
                //if (e.Column.FieldName == "customerLevel2" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var customerList = new List<CustomerCompany>();
                //        var node = row.customerCompany;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    customerList.Add(node);
                //                    node = node.parentCompany;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    customerList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                customerList.Add(node);
                //                break;
                //            }
                //        }
                //        customerList.Reverse();
                //        switch (customerList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = customerList[0].company.CompanyName;
                //                break;
                //            case 2:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 3:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 4:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 5:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "customerLevel3" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var customerList = new List<CustomerCompany>();
                //        var node = row.customerCompany;
                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and transverse to its parent
                //                    customerList.Add(node);
                //                    node = node.parentCompany;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    customerList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                customerList.Add(node);
                //                break;
                //            }
                //        }
                //        customerList.Reverse();
                //        switch (customerList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = customerList[0].company.CompanyName;
                //                break;
                //            case 2:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 3:
                //                e.Value = customerList[2].company.CompanyName;
                //                break;
                //            case 4:
                //                e.Value = customerList[2].company.CompanyName;
                //                break;
                //            case 5:
                //                e.Value = customerList[2].company.CompanyName;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "customerLevel4" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var customerList = new List<CustomerCompany>();
                //        var node = row.customerCompany;

                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and tranverse to its parent
                //                    customerList.Add(node);
                //                    node = node.parentCompany;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    customerList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                customerList.Add(node);
                //                break;
                //            }

                //        }
                //        customerList.Reverse();
                //        switch (customerList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = customerList[0].company.CompanyName;
                //                break;
                //            case 2:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 3:
                //                e.Value = customerList[2].company.CompanyName;
                //                break;
                //            case 4:
                //                e.Value = customerList[3].company.CompanyName;
                //                break;
                //            case 5:
                //                e.Value = customerList[3].company.CompanyName;
                //                break;
                //        }
                //    }
                //}
                //if (e.Column.FieldName == "customerLevel5" && e.IsGetData)
                //{
                //    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                //    if (row != null)
                //    {
                //        var customerList = new List<CustomerCompany>();
                //        var node = row.customerCompany;

                //        while (node != null)
                //        {
                //            if (node.ParentID != null)
                //            {
                //                if (node.ParentID != node.Id)
                //                {
                //                    //this will add current node to department list and tranverse to its parent
                //                    customerList.Add(node);
                //                    node = node.parentCompany;
                //                }
                //                else
                //                {
                //                    //when node is parent to itself
                //                    customerList.Add(node);
                //                    break;
                //                }
                //            }
                //            else
                //            {
                //                //parent with parent id is null
                //                customerList.Add(node);
                //                break;
                //            }
                //        }
                //        customerList.Reverse();

                //        switch (customerList.Count)
                //        {
                //            case 0:

                //                break;
                //            case 1:
                //                e.Value = customerList[0].company.CompanyName;
                //                break;
                //            case 2:
                //                e.Value = customerList[1].company.CompanyName;
                //                break;
                //            case 3:
                //                e.Value = customerList[2].company.CompanyName;
                //                break;
                //            case 4:
                //                e.Value = customerList[3].company.CompanyName;
                //                break;
                //            case 5:
                //                e.Value = customerList[4].company.CompanyName;
                //                break;
                //        }
                //    }
                //}


                if (e.IsGetData)
                    switch (e.Column.FieldName)
                    {

                        case "AmountPERsum":
                            var SO = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (SO.SplitPERs.Count != 0)
                            {
                                if (dateMonthSplitPER.EditValue != null && datYearSplitPER.EditValue != null)
                                {
                                    var year = ((DateTime)datYearSplitPER.EditValue).Year;
                                    var month = ((DateTime)dateMonthSplitPER.EditValue).Month;

                                    var splits = SO.SplitPERs.Where(x =>
                                    x.Month.Value.Month == month &&
                                    x.Month.Value.Year == year &&
                                    x.Year.Value.Year == year).ToList();
                                    e.Value = Math.Round(splits.Sum(x => x.Amount), 2);
                                }
                                else if (dateMonthSplitPER.EditValue == null && datYearSplitPER.EditValue != null)
                                {
                                    var year = ((DateTime)datYearSplitPER.EditValue).Year;
                                    var splits = SO.SplitPERs.Where(x =>
                                    x.Month.Value.Year == year &&
                                    x.Year.Value.Year == year).ToList();
                                    e.Value = Math.Round(splits.Sum(x => x.Amount), 2);
                                }
                            }
                            break;
                        case "Vendorss":
                            if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                            {
                                var _SO = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                                string vendorNames = "";

                                if (_SO.vendors != null && _SO.vendors.Count > 0)
                                {
                                    vendorNames = String.Join(" | ", _SO.vendors.Select(x => x.company.CompanyName));
                                }
                                e.Value = vendorNames;
                            }
                            break;
                        case "PaymentDueAgeingDays":
                            if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                            {
                                DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));
                                Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                                e.Value = NoDueAgeingDays;
                            }
                            break;
                        case "Department":
                            break;
                        case "CreationAgeing":
                            if (e.GetListSourceFieldValue("CreationDate") != null)
                            {
                                DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));


                                Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                                e.Value = CreateAgeingDays;
                            }
                            break;
                        case "SODateAgeing":
                            if (e.GetListSourceFieldValue("saleOrderDate") != null)
                            {
                                DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("saleOrderDate"));


                                Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                                e.Value = SoDateAgeingDays;
                            }
                            break;
                        case "SODeliveryAgeing":
                            if (e.GetListSourceFieldValue("deliveryDate") != null)
                            {
                                DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));


                                Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                                e.Value = SoDateAgeingDays;
                            }
                            break;
                        case "totalWeight":
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
                                e.Value = totalweight;
                            }

                            break;
                        case "totalQuantity":
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
                                e.Value = totalquantity;
                            }

                            break;
                        case "BudgetMarginOC":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result1 = total - Cost.TotalBudgetedMargin;
                                e.Value = result1;
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                            }
                            break;

                        case "BudgetMarginSER":
                            var selectedRow = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var budgetCost = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));
                            if (selectedRow.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = budgetCost * selectedRow.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(budgetCost) * Convert.ToDouble(selectedRow.marginExchangeRate);
                            }

                            break;
                        case "BudgetMarginMER":
                            var selectedRow1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var budgetCostMER = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));

                            if (selectedRow1.saleOrdertype == InquiryType.SupplyCCC)
                            {

                                e.Value = budgetCostMER * selectedRow1.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(budgetCostMER) * Convert.ToDouble(selectedRow1.exchangeRate);
                            }

                            break;
                        case "RevisedMarginSER":
                            var selectedRow2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var revisedCost = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                            if (selectedRow2.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = revisedCost * selectedRow2.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(revisedCost) * Convert.ToDouble(selectedRow2.marginExchangeRate);
                            }

                            break;
                        case "RevisedMarginMER":
                            var selectedRow3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var revisedCostMER = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                            if (selectedRow3.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = revisedCostMER * selectedRow3.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(revisedCostMER) * Convert.ToDouble(selectedRow3.exchangeRate);
                            }

                            break;
                        case "actualMarginSER":
                            var selectedRow4 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var actualMargin = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                            if (selectedRow4.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = actualMargin * selectedRow4.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(actualMargin) * Convert.ToDouble(selectedRow4.marginExchangeRate);
                            }

                            break;
                        case "actualMarginMER":
                            var selectedRow5 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var actualMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                            if (selectedRow5.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = actualMarginMER * selectedRow5.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(actualMarginMER) * Convert.ToDouble(selectedRow5.exchangeRate);
                            }

                            break;
                        case "SalesSystemMargin1":
                            var selectedRow6 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var systemMargin = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                            if (selectedRow6.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = systemMargin * selectedRow6.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(systemMargin) * Convert.ToDouble(selectedRow6.marginExchangeRate);
                            }

                            break;
                        case "SalesMarketMargin1":
                            var selectedRow7 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var systemMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                            if (selectedRow7.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = systemMarginMER * selectedRow7.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(systemMarginMER) * Convert.ToDouble(selectedRow7.exchangeRate);
                            }

                            break;



                        case "BudgetedMarginPercentAge":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result4 = ((total - Cost.TotalBudgetedMargin) / total) * 100;
                                e.Value = result4;
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMarginPercent"));
                            }
                            break;
                        case "ActualMarginOC":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }

                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result5 = total - Cost.TotalActualMargin;
                                e.Value = result5;
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargin"));
                            }
                            break;

                        case "ActualMarginPercentAge":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result8 = ((total - Cost.TotalActualMargin) / total) * 100;
                                e.Value = result8;
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMarginPercent"));
                            }
                            break;
                        case "RevisedMarginOC":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result9 = total - Cost.TotalRevisedMargin;
                                e.Value = result9;


                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargin"));
                            }
                            break;

                        case "RevisedMarginPercentAge":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                                if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                                }
                                else
                                {
                                    total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                                }
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                                var result12 = ((total - Cost.TotalRevisedMargin) / total) * 100;
                                e.Value = result12;
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
                            }
                            break;
                        case "invoicedTotalOC":
                            var saleOrder = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            e.Value = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                            break;
                        case "recieptTotalOC":
                            var saleOrder1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            e.Value = saleOrder1.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                            break;
                        case "totalDeductionsOC":
                            var saleOrder2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            e.Value = saleOrder2.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                            break;
                        case "totalCreditedOC":
                            var saleOrder3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            var result = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                            var collection = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                            e.Value = collection - result;
                            break;
                        case "remainingCollectionOC":
                            var saleOrder4 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (saleOrder4.saleOrdertype == InquiryType.DistributionBiz)
                            {
                                var invoicedAmount = Math.Round((saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount))).Value, 2);
                                var collected = Math.Round(Convert.ToDouble(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount))), 2);
                                e.Value = Math.Round((Convert.ToDouble(invoicedAmount) - collected), 2);
                            }
                            else if (saleOrder4.saleOrdertype == InquiryType.DistributionBiz_CustomerCredit)
                            {
                                var invoicedAmount = Math.Round((saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount))).Value, 2);
                                var collected = Math.Round(Convert.ToDouble(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount))), 2);
                                e.Value = Math.Round((Convert.ToDouble(invoicedAmount) - collected), 2);
                            }
                            else if (saleOrder4.saleOrdertype == InquiryType.Principal)
                            {
                                if (saleOrder4.SaleInvoices != null && e.GetListSourceFieldValue("commision") != null)
                                {
                                    var total = Convert.ToDecimal(e.GetListSourceFieldValue("commision"));
                                    var result13 = Convert.ToDecimal(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                    e.Value = total - result13;
                                }
                            }
                            else
                            {
                                if (saleOrder4.SaleInvoices != null && e.GetListSourceFieldValue("totalCFRValue") != null)
                                {
                                    var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                                    var result13 = Convert.ToDecimal(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                    e.Value = total - result13;
                                }
                            }

                            break;
                        case "SystemCost":
                            var saleOrder5 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (saleOrder5.CostSheet_Id != null)
                            {
                                var systemCost = saleOrderRepo.GetSOSystemCost((int)saleOrder5.CostSheet_Id);

                                e.Value = systemCost;
                            }
                            break;

                        case "budgetCost":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("exchangeRate"));
                                var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                                var fields = Cost.FieldValues.Where(x => x.Type == 1).ToList();
                                decimal totalFieldValue = fields.Sum(x => x.Value);
                                var result3 = totalFieldValue /** exchangerate*/;
                                e.Value = Math.Round(result3, 2);
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                            }
                            break;

                        case "adjCost":
                            {
                                var adjCost = saleOrderRepo.GetAdjustmentCost(Convert.ToInt32(e.GetListSourceFieldValue("Id")), Convert.ToInt32(e.GetListSourceFieldValue("CostSheet_Id")));
                                e.Value = adjCost;
                            }
                            break;
                        case "SER":
                            {
                                var saleOrderSER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                                double todayRate = 0;
                                var exchangeRateGroupSER = exchangeRateGroupsSER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderSER.currency_Id && x.base_currency_Id == saleOrderSER.company.CurrencyId && x.TargetYear == saleOrderSER.CreationDate.Value.Year);
                                if (exchangeRateGroupSER != null)
                                {
                                    exchangeRateSER = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderSER.company_Id);
                                    switch (saleOrderSER.CreationDate.Value.Month)
                                    {
                                        case 1:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateJan;
                                            break;
                                        case 2:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateFeb;
                                            break;
                                        case 3:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateMar;
                                            break;
                                        case 4:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateApr;
                                            break;
                                        case 5:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateMay;
                                            break;
                                        case 6:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateJun;
                                            break;
                                        case 7:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateJul;
                                            break;
                                        case 8:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateAug;
                                            break;
                                        case 9:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateSep;
                                            break;
                                        case 10:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateOct;
                                            break;
                                        case 11:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateNov;
                                            break;
                                        case 12:
                                            if (exchangeRateSER != null)
                                                todayRate = exchangeRateSER.rateDec;
                                            break;
                                        default:
                                            if (exchangeRateSER != null)
                                                todayRate = 0;
                                            break;
                                    }
                                }
                                var total = todayRate;
                                e.Value = total;
                            }
                            break;
                        case "MER":
                            {
                                var saleOrderMER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                                double todayRate = 0;
                                var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderMER.currency_Id && x.base_currency_Id == saleOrderMER.company.CurrencyId && x.TargetYear == saleOrderMER.CreationDate.Value.Year);
                                if (exchangeRateGroupMER != null)
                                {
                                    exchangeRateMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderMER.company_Id);
                                    switch (saleOrderMER.CreationDate.Value.Month)
                                    {
                                        case 1:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateJan;
                                            break;
                                        case 2:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateFeb;
                                            break;
                                        case 3:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateMar;
                                            break;
                                        case 4:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateApr;
                                            break;
                                        case 5:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateMay;
                                            break;
                                        case 6:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateJun;
                                            break;
                                        case 7:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateJul;
                                            break;
                                        case 8:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateAug;
                                            break;
                                        case 9:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateSep;
                                            break;
                                        case 10:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateOct;
                                            break;
                                        case 11:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateNov;
                                            break;
                                        case 12:
                                            if (exchangeRateMER != null)
                                                todayRate = exchangeRateMER.rateDec;
                                            break;
                                        default:
                                            if (exchangeRateMER != null)
                                                todayRate = 0;
                                            break;
                                    }
                                }
                                var total = todayRate;
                                e.Value = total;
                            }
                            break;
                        case "CMER":
                            {
                                var SOCMER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                                double todayRate = 0;
                                var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == SOCMER.currency_Id && x.base_currency_Id == SOCMER.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                                if (exchangeRateGroupMER != null)
                                {
                                    exchangeRateCMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == SOCMER.company_Id);
                                    switch (DateTime.Now.Month)
                                    {
                                        case 1:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateJan;
                                            break;
                                        case 2:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateFeb;
                                            break;
                                        case 3:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateMar;
                                            break;
                                        case 4:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateApr;
                                            break;
                                        case 5:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateMay;
                                            break;
                                        case 6:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateJun;
                                            break;
                                        case 7:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateJul;
                                            break;
                                        case 8:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateAug;
                                            break;
                                        case 9:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateSep;
                                            break;
                                        case 10:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateOct;
                                            break;
                                        case 11:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateNov;
                                            break;
                                        case 12:
                                            if (exchangeRateCMER != null)
                                                todayRate = exchangeRateCMER.rateDec;
                                            break;
                                        default:
                                            if (exchangeRateCMER != null)
                                                todayRate = 0;
                                            break;
                                    }
                                }
                                var total = todayRate;
                                e.Value = total;
                            }
                            break;
                        case "supervisorPoints":
                            var order = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (order.PerformanceSheet_Id != null)
                            {
                                e.Value = order.PerformanceSheet.performanceSheetFields.Sum(x => x.point);
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "depHeadPoints":
                            var order1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (order1.PerformanceSheet_Id != null)
                            {
                                e.Value = order1.PerformanceSheet.performanceSheetFields.Sum(x => x.revisedPoint);
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "finalPoints":
                            var order2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (order2.PerformanceSheet_Id != null)
                            {
                                e.Value = order2.PerformanceSheet.totalPoints;
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "totalPoints":
                            var order3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (order3.PerformanceSheet_Id != null)
                            {
                                e.Value = order3.PerformanceSheet.performanceSheetFields.Sum(x => x.PerformanceSheetHead.totalPoints);
                            }
                            break;
                        case "remainingInvoiced":
                            var so = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            if (so.SaleInvoices != null)
                            {
                                if (so.SaleInvoices.Count != 0)
                                {
                                    var totalInvoiced = so.SaleInvoices.Sum(x => x.totalInvoiceAmount);
                                    var totalSOAmount = so.totalCFRValue;
                                    var value = totalSOAmount - totalInvoiced;
                                    e.Value = value;
                                }
                            }
                         
                            break;
                    }
            }
            catch (Exception)
            {

               
            }
         
        }
        /// <summary>
        /// Loads data into saleOrder Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSaleOrdergrid()
        {
            
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Sale Orders(Open)" || lblHeading.Text == "Sale Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        saleOrders = saleOrderrepo.getAll();
                       
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Orders") != null)
                    {
                       
                        saleOrders = saleOrderrepo.getFirstAll(MainWindow.currentUserid);
                    }
                    else
                    {
                        
                        //saleOrders = saleOrderrepo.getAllActive(MainWindow.currentUserid);
                        saleOrders = saleOrderrepo.getAllFirstActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    
                    //saleOrders = saleOrderrepo.getAllActive(MainWindow.currentUserid);
                    saleOrders = saleOrderrepo.getAllFirstActive(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    
                    //saleOrders = saleOrderrepo.getAllInActive(MainWindow.currentUserid);
                    saleOrders = saleOrderrepo.getAllFirstInActive(MainWindow.currentUserid);

                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Sale Orders)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                        {
                            
                            //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);

                            saleOrders = saleOrderrepo.getAllFirstPendingForApprovalDepartmental(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                
                               
                                saleOrders = saleOrderrepo.getAllFirstPendingForApproval(MainWindow.currentUserid);
                            }
                            else
                            {
                               
                              
                                saleOrders = saleOrderrepo.getAllFirstPendingForApprovalOwn(MainWindow.currentUserid);

                            }
                        }
                    else
                       
                    saleOrders = saleOrderrepo.getAllFirstPendingForAdministrator();

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Sale Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                        {
                            
                            saleOrders = saleOrderrepo.getAllFirstPendingForClosingDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                               
                                saleOrders = saleOrderrepo.getAllFirstPendingForClosing(MainWindow.currentUserid);
                            }
                            else
                            {
                               
                                saleOrders = saleOrderrepo.getAllFirstPendingForClosingOwn(MainWindow.currentUserid);
                            }
                        }
                    
                  
                    else
                       
                        saleOrders = saleOrderrepo.getAllFirstPendingForClosingAdministrator();
                    grdsaleOrder.ItemsSource = saleOrders;
                   
       
                    grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {

              
                saleOrders = saleOrderrepo.getAllFirstPobyStatusId(MainWindow.currentUserid, statusId);

            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleOrder);
            grdsaleOrder.ItemsSource = saleOrders;
            
            RemoveSourceObjects();


        }
        private void grdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();
        }
        private void RemoveSourceObjects()
        {
                   
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm"));
                 
                    
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentTerm"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("saleOrderStatus"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendor"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("employee"));
                   //// grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("department"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm_Id"));
                   
                    
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentterm_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("allocation_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("dept_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user_Id"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentId"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentStatus"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheetId"));
                   // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheet"));
                    //grdsaleOrder.Columns.GetColumnByFieldName("referenceNo").Header = "SO Refrence";
           
}
        private void EditSaleOrder()
        {
            
            //if (grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")) != null)
            //{
            if(grdsaleOrder.SelectedItem != null)
            {

                
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (grdsaleOrder.SelectedItem as SaleOrder).Id);
                procurmentPanel.Show();
            }

            
            //}
        }

        private void btnNewSaleOrder_Click(object sender, RoutedEventArgs e)
        {

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, 0);
            procurmentPanel.Show();
        }

        private void btnEditSaleOrder_Click(object sender, RoutedEventArgs e)
        {
            EditSaleOrder();

        }

        private void UcsaleOrdergrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdsaleOrder);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdsaleOrder.View.ShowPrintPreview(this);
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
  
        }

        private void MbtnReportCreate_Click(object sender, RoutedEventArgs e)
        {
            ShowDesigner(tableView);
        }
        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Sale Orders Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
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
                        ReportLogic.SaveGridReport(grdsaleOrder, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Orders Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.RemoveFromVisualTree();
            SaleOrderss.ucSaleOrderGrid ucSaleOrderGrid = new ucSaleOrderGrid();
            this.Content = ucSaleOrderGrid;
            //InitializeComponent();
            //loadSaleOrdergrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleOrder.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleOrder.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null) ? true : false)
            {
                
                if (grdsaleOrder.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    var row = grdsaleOrder.GetFocusedRow() as SaleOrder;

                    var total = row.totalCFRValue; //Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                    var result = Convert.ToDouble(row.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                    var remainingCollection = total - result;
                    remainingCollection = Math.Round(remainingCollection, 2);
                    if (row.saleOrdertype == InquiryType.Principal)
                    {
                        var invoicedAmount = row.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                        var collected = row.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        remainingCollection = Convert.ToDouble(invoicedAmount) - Convert.ToDouble(collected);
                        remainingCollection = Math.Round(remainingCollection, 2);
                    }
                    if (remainingCollection != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without receiving fully Collection") != null)
                        {
                            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale order without complete collection?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
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
                                SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                                SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id"));
                                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderrepo);
                                var myWindow = Window.GetWindow(this);
                                statusChange.Owner = myWindow;
                                statusChange.ShowDialog();
                                //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                                //    return;
                                if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)
                                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                        //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                                    }
                                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                        if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                        }
                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                        if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                        }
                                        //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                    else
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                        usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                                SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                                if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                                    usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
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
                                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Order);
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
                                string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
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
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                                    }
                                }


                                //}

                                //SaleOrderss.ucStatuschange.Updatestatus();
                                try
                                {
                                    row = SaleOrderss.ucStatuschange.saleOrder;
                                    grdsaleOrder.RefreshData();
                                    saleOrderrepo.updateStatusById(row.Id, row.saleOrderStatus);
                                }
                                catch { }

                                //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                                MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            }
                            else
                                return;
                        }
                        else
                            DXMessageBox.Show("Collection is not fully received yet, or you need permission to Close Sale Order without receiving fully Collection","Permission Denied",MessageBoxButton.OK,MessageBoxImage.Stop);
                    }
                    else
                    {
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
                        SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                        SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id"));
                        SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderrepo);
                        var myWindow = Window.GetWindow(this);
                        statusChange.Owner = myWindow;
                        statusChange.ShowDialog();
                        //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                        //    return;
                        if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                }
                                //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                        //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                        SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                        SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                        if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                            usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
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
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Order);
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
                        string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
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
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                            }
                        }


                        //}

                        //SaleOrderss.ucStatuschange.Updatestatus();
                        try
                        {
                            row = SaleOrderss.ucStatuschange.saleOrder;
                            grdsaleOrder.RefreshData();
                            saleOrderrepo.updateStatusById(row.Id, row.saleOrderStatus);
                        }
                        catch { }

                        //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                        MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close SaleOrder Directly.");
            }
        }
        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
           
            statusId = 0;
            lblHeading.Text = "Pending Sale Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                {
                    
                    //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);

                    saleOrders = saleOrderrepo.getAllFirstPendingForApprovalDepartmental(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        
                        //saleOrders = saleOrderrepo.getAllPendingForApproval(MainWindow.currentUserid);
                        saleOrders = saleOrderrepo.getAllFirstPendingForApproval(MainWindow.currentUserid);
                    }

                    else
                    {
                        
                        saleOrders = saleOrderrepo.getAllFirstPendingForApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else
               
                saleOrders = saleOrderrepo.getAllFirstPendingForAdministrator();

            grdsaleOrder.ItemsSource = saleOrders;
          

            //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
           
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
            statusId = 0;
            AllActive = 5;
            lblHeading.Text = "Pending for ReApprovals Sale Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                {
                   
                    saleOrders = saleOrderrepo.getAllFirstPendingForReApprovalDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                       
                        saleOrders = saleOrderrepo.getAllFirstPendingForReApproval(MainWindow.currentUserid);
                    }

                    else
                    {
                        
                        saleOrders = saleOrderrepo.getAllFirstPendingForReApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else
              
                saleOrders = saleOrderrepo.getAllFirstPendingForAdministrator();
            grdsaleOrder.ItemsSource = saleOrders;
     
            //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
         

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
            lblHeading.Text = "(Pending for Closing) Sale Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                {
                    
                    saleOrders = saleOrderrepo.getAllFirstPendingForClosingDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                       
                        saleOrders = saleOrderrepo.getAllFirstPendingForClosing(MainWindow.currentUserid);
                    }

                    else
                    {
                      
                        saleOrders = saleOrderrepo.getAllFirstPendingForClosingOwn(MainWindow.currentUserid);

                    }
                }
            //saleOrders = saleOrderrepo.getAllPendingForApproval(MainWindow.currentUserid);
            else
                
                saleOrders = saleOrderrepo.getAllFirstPendingForClosingAdministrator();
            grdsaleOrder.ItemsSource = saleOrders;
            //grdsaleOrder.ClearGrouping();
            //grdsaleOrder.FilterString = "";
            //grdsaleOrder.ItemsSource = saleOrders;
            //grdsaleOrder.Columns["stage"].GroupIndex = 0;
            //grdsaleOrder.GroupBy("stage");
            //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;

            //grdsaleOrder.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);

            //grdsaleOrder.View =new CardView();

            AllActive = 4;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null) ? true : false)
            {
                if (grdsaleOrder.GetFocusedRow() != null)
                {
                    SaleOrder saleOrder = new SaleOrder();
                    //Inquiriess.ucStatuschange.saleOrderid = (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id"));
                    saleOrder = grdsaleOrder.SelectedItem as SaleOrder;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    UsersRepo usersRepo = new UsersRepo();

                    if (saleOrder.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                        {
                            saleOrder.isApproved = true;
                            saleOrder.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                        {
                            saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                            if (saleOrder.isApproved == null)
                            {
                                saleOrder.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                        {
                            saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (saleOrder.isApproved == null)
                            {
                                saleOrder.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                        }
                        else
                        {
                            saleOrder.isApproved = false;
                        }
                    else //reapprove when it is approved already
                    {
                        if (saleOrder.isReApproved == false)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null) ? true : false)
                            {
                                saleOrder.isReApproved = true;
                                saleOrder.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (saleOrder.isReApproved == null)
                                {
                                    saleOrder.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                            {
                                saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (saleOrder.isReApproved == null)
                                { 
                                    saleOrder.isReApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else
                            {
                                saleOrder.isReApproved = false;
                            }

                    }


                    saleOrderrepo.update(saleOrder);
                    MessageBox.Show("SaleOrder is Approved (" + saleOrder.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "SaleOrder is Approved (" + saleOrder.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve SaleOrder Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleOrder Directly user id=(" + MainWindow.currentUserid + ")");

            }

        }
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdsaleOrder);
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
            lblHeading.Text = "Sale Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                {
                    saleOrders = saleOrderrepo.getFirstSaleRegister(MainWindow.currentUserid);
                }
                else
                {
                    //All bills instead of own bills
                    //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                    saleOrders = saleOrderrepo.getFirstSaleRegister(MainWindow.currentUserid);
                }
            else
                saleOrders = saleOrderrepo.getSaleRegisterAdministrator();
            grdsaleOrder.ItemsSource = saleOrders;
            //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
            statusId = 0;
            AllActive = 6;
            mbtnExportToReport.IsEnabled = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            //_usersRepo usersRepo = new _usersRepo();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "Void Sale Orders";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleOrders") != null)
                {
                    saleOrders = saleOrderrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    saleOrders = saleOrderrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                saleOrders = saleOrderrepo.getVoidRegisterAdministrator();
            grdsaleOrder.ItemsSource = saleOrders;
            //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnLoadMoreSO_Click(object sender, RoutedEventArgs e)
        {

            //var priorDateRealTrades = grdsaleOrder.ItemsSource as List<SaleOrder>;
            //var minSample = priorDateRealTrades.Where(s => s.CreationDate == priorDateRealTrades.Min(x => x.CreationDate))
            //            .FirstOrDefault();

            //var date = minSample.CreationDate;


        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        Mouse.OverrideCursor = Cursors.Wait;
        //    }); 
        ///*    LoadMoreSO((DateTime)date*/);
        //    //grdNotifications.RefreshData();
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        Mouse.OverrideCursor = null;
        //    });
            RemoveSourceObjects();
        }
        //private void LoadMoreSO(DateTime _date)
        //{
        //    lblHeading.Text = SYSTEM_STATIC.gridTitle;
        //    if (lblHeading.Text == "Sale Orders(Open)" || lblHeading.Text == "Sale Orders(Closed)")
        //    { mbtnExportToReport.IsEnabled = false; }
        //    if (statusId == 0)
        //    {
        //        if (AllActive == 0)
        //        {
        //            if (MainWindow.currentUserid == 0)
        //            {
        //                saleOrders = saleOrderrepo.getAll();
        //            }
        //            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Orders") != null)
        //            {
        //                saleOrderrepo = new SaleOrderRepo();
        //                var moreSaleOrders = saleOrderrepo.loadMoreActiveInActiveSo(_date, MainWindow.currentUserid);
        //                saleOrders.AddRange(moreSaleOrders.Distinct());
        //                if (date.Month == 1)
        //                {
        //                    date.AddYears(-1);
        //                }
        //                date = _date.AddMonths(-1);
        //            }
        //            else
        //            {
        //                var moreSaleOrders = saleOrderrepo.loadMoreAllActive(_date, MainWindow.currentUserid);
        //                saleOrders.AddRange(moreSaleOrders.Distinct());
        //                if (date.Month == 1)
        //                {
        //                    date.AddYears(-1);
        //                }
        //                date = _date.AddMonths(-1);
        //            }
        //        }
        //        else if (AllActive == 1)
        //        {
        //            var moreSaleOrders = saleOrderrepo.loadMoreAllActive(_date, MainWindow.currentUserid);
        //            saleOrders.AddRange(moreSaleOrders.Distinct());
        //            if (date.Month == 1)
        //            {
        //                date.AddYears(-1);
        //            }
        //            date = _date.AddMonths(-1);

        //        }
        //        else if (AllActive == 2)
        //        {
        //            var moreSaleOrders = saleOrderrepo.loadMoreAllInActive(_date, MainWindow.currentUserid);
        //            saleOrders.AddRange(moreSaleOrders.Distinct());
        //            if (date.Month == 1)
        //            {
        //                date.AddYears(-1);
        //            }
        //            date = _date.AddMonths(-1);
        //        }
        //        else if (AllActive == 3)
        //        {
        //            _usersRepo usersRepo = new _usersRepo();

        //            lblHeading.Text = "Pending For Approvals (Sale Orders)";
        //            if (MainWindow.currentUserid != 0)
        //                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
        //                {
        //                    //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
        //                    var moreSaleOrders = saleOrderrepo.LoadMoreAllPendingForApprovalDepartmental(_date, MainWindow.currentUserid);
        //                    saleOrders.AddRange(moreSaleOrders.Distinct());
        //                    if (date.Month == 1)
        //                    {
        //                        date.AddYears(-1);
        //                    }
        //                    date = _date.AddMonths(-1);
        //                }
        //                else
        //                {
        //                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
        //                    {
        //                        //saleOrders = saleOrderrepo.getAllPendingForApproval(MainWindow.currentUserid);
        //                        var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForApproval(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                        if (date.Month == 1)
        //                        {
        //                            date.AddYears(-1);
        //                        }
        //                        date = _date.AddMonths(-1);

        //                    }

        //                    else
        //                    {
        //                        var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForApprovalOwn(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                    }
        //                }
        //            else
        //            {
        //                saleOrders = saleOrderrepo.loadMoreAllPendingForAdministrator(_date);
        //                if (date.Month == 1)
        //                {
        //                    date.AddYears(-1);
        //                }
        //                date = _date.AddMonths(-1);
        //            }
        //            //grdsaleOrder.ItemsSource = saleOrders;
        //            grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;

        //        }
        //        if (AllActive == 4)
        //        {
        //            lblHeading.Text = "(Pending for Closing) Sale Orders";
        //            if (MainWindow.currentUserid != 0)
        //                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
        //                {
        //                    var moreSaleOrders = saleOrderrepo.loadMorePendingForClosingDepartmental(_date, MainWindow.currentUserid);
        //                    saleOrders.AddRange(moreSaleOrders.Distinct());
        //                    if (date.Month == 1)
        //                    {
        //                        date.AddYears(-1);
        //                    }
        //                    date = _date.AddMonths(-1);
        //                }
        //                else
        //                {
        //                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
        //                    {
        //                        var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForClosing(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                        if (date.Month == 1)
        //                        {
        //                            date.AddYears(-1);
        //                        }
        //                        date = _date.AddMonths(-1);
        //                    }

        //                    else
        //                    {
        //                        var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForClosingOwn(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                        if (date.Month == 1)
        //                        {
        //                            date.AddYears(-1);
        //                        }
        //                        date = _date.AddMonths(-1);
        //                    }
        //                }
        //            else
        //            {
        //                var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForClosingAdministrator(_date);
        //                saleOrders.AddRange(moreSaleOrders.Distinct());
        //                if (date.Month == 1)
        //                {
        //                    date.AddYears(-1);
        //                }
        //                date = _date.AddMonths(-1);
        //            }
        //            grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
        //        }
        //        else
        //        if (AllActive == 5)
        //        {
        //            _usersRepo usersRepo = new _usersRepo();

        //            lblHeading.Text = "Pending for ReApprovals Sale Orders";
        //            if (MainWindow.currentUserid != 0)
        //                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
        //                {
        //                    var moreSaleOrders = saleOrderrepo.loadMorePendingForReApprovalDepartmental(_date, MainWindow.currentUserid);
        //                    saleOrders.AddRange(moreSaleOrders.Distinct());
        //                    if (date.Month == 1)
        //                    {
        //                        date.AddYears(-1);
        //                    }
        //                    date = _date.AddMonths(-1);
        //                }
        //                else
        //                {
        //                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
        //                    {
        //                        var moreSaleOrders = saleOrderrepo.loadMoreFirstPendingForReApproval(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                        if (date.Month == 1)
        //                        {
        //                            date.AddYears(-1);
        //                        }
        //                        date = _date.AddMonths(-1);
        //                    }

        //                    else
        //                    {
        //                        var moreSaleOrders = saleOrderrepo.loadMoreFirstPendingForReApprovalOwn(_date, MainWindow.currentUserid);
        //                        saleOrders.AddRange(moreSaleOrders.Distinct());
        //                        if (date.Month == 1)
        //                        {
        //                            date.AddYears(-1);
        //                        }
        //                        date = _date.AddMonths(-1);
        //                    }
        //                }
        //            else
        //            {
        //                var moreSaleOrders = saleOrderrepo.loadMoreAllPendingForAdministrator(_date);
        //                saleOrders.AddRange(moreSaleOrders.Distinct());
        //                if (date.Month == 1)
        //                {
        //                    date.AddYears(-1);
        //                }
        //                date = _date.AddMonths(-1);
        //            }

        //            grdsaleOrder.ItemsSource = saleOrders;
        //            grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
        //            AllActive = 5;
        //        }
        //        else
        //            if (AllActive == 6)
        //        {
        //            var moreSaleOrders = saleOrderrepo.loadMoreSaleRegister(_date, MainWindow.currentUserid);
        //            saleOrders.AddRange(moreSaleOrders.Distinct());
        //            if (date.Month == 1)
        //            {
        //                date.AddYears(-1);
        //            }
        //            date = _date.AddMonths(-1);
        //        }
        //    }
        //    else
        //    {
        //        var moreSaleOrders = saleOrderrepo.loadMoreAllPobyStatusId(_date, MainWindow.currentUserid, statusId);
        //        saleOrders.AddRange(moreSaleOrders.Distinct());
        //        if(date.Month == 1)
        //        {
        //            date.AddYears(-1);
        //        }
        //        date = _date.AddMonths(-1);
        //    }
            
        //    grdsaleOrder.ItemsSource = saleOrders.Distinct();
        //    grdsaleOrder.RefreshData();
        //}
        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Sale Orders(Open)" || lblHeading.Text == "Sale Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        saleOrders = saleOrderrepo.getAll();
                        //this.grdsaleOrder.ItemsSource = saleOrders;
                        //MessageBox.Show("You are not Authorized");

                        //return; 
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Orders") != null)
                    {
                        saleOrders = saleOrderrepo.getAll(MainWindow.currentUserid);
                        //saleOrders = saleOrderrepo.getFirstAll(MainWindow.currentUserid);
                    }
                    else
                    {
                        saleOrders = saleOrderrepo.getAllActive(MainWindow.currentUserid);
                        //saleOrders = saleOrderrepo.getAllFirstActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    saleOrders = saleOrderrepo.getAllActive(MainWindow.currentUserid);
                    //saleOrders = saleOrderrepo.getAllFirstActive(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    saleOrders = saleOrderrepo.getAllInActive(MainWindow.currentUserid);
                    //saleOrders = saleOrderrepo.getAllFirstInActive(MainWindow.currentUserid);

                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Sale Orders)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                        {
                            saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);

                            //saleOrders = saleOrderrepo.getAllFirstPendingForApprovalDepartmental(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders = saleOrderrepo.getAllPendingForApproval(MainWindow.currentUserid);
                                //saleOrders = saleOrderrepo.getAllFirstPendingForApproval(MainWindow.currentUserid);
                            }
                            else
                            {
                                saleOrders = saleOrderrepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);
                                //saleOrders = saleOrderrepo.getAllFirstPendingForApprovalOwn(MainWindow.currentUserid);

                            }
                        }
                    else
                        saleOrders = saleOrderrepo.getAllPendingForAdministrator();
                        //saleOrders = saleOrderrepo.getAllFirstPendingForAdministrator();

                    grdsaleOrder.ItemsSource = saleOrders;
                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;

                }
                else
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Sale Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                        {
                            saleOrders = saleOrderrepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                            //saleOrders = saleOrderrepo.getAllFirstPendingForClosingDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders = saleOrderrepo.getAllPendingForClosing(MainWindow.currentUserid);
                                //saleOrders = saleOrderrepo.getAllFirstPendingForClosing(MainWindow.currentUserid);

                            }

                            else
                            {
                                saleOrders = saleOrderrepo.getAllPendingForClosingOwn(MainWindow.currentUserid);
                                //saleOrders = saleOrderrepo.getAllFirstPendingForClosingOwn(MainWindow.currentUserid);


                            }
                        }
                    //saleOrders = saleOrderrepo.getAllPendingForApproval(MainWindow.currentUserid);
                    else
                        saleOrders = saleOrderrepo.getAllPendingForClosingAdministrator();
                        //saleOrders = saleOrderrepo.getAllFirstPendingForClosingAdministrator();
                    grdsaleOrder.ItemsSource = saleOrders;

                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                }
                else
                if (AllActive == 5)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending for ReApprovals Sale Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                        {
                            saleOrders = saleOrderrepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders= saleOrderrepo.getAllPendingForReApproval(MainWindow.currentUserid);
                            }

                            else
                            {
                                saleOrders = saleOrderrepo.getAllPendingForReApprovalOwn(MainWindow.currentUserid);
                                
                            }
                        }
                    else
                    {
                        saleOrders = saleOrderrepo.getAllPendingForAdministrator();
                    }

                    grdsaleOrder.ItemsSource = saleOrders;
                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                    AllActive = 5;
                }
            }
            else
            {
                //saleOrders = saleOrderrepo.getAllPobyStatusId(MainWindow.currentUserid, statusId);
                saleOrders = saleOrderrepo.getAllPobyStatusId(MainWindow.currentUserid, statusId);

            }
            grdsaleOrder.ItemsSource = saleOrders;
            {

                System.Threading.Thread th2 = new System.Threading.Thread(() =>
                {
                    if (grdsaleOrder.Dispatcher.CheckAccess())
                    {
                        // The calling thread owns the dispatcher, and hence the UI element
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleOrder);
                    }
                    else
                    {
                        // Invokation required
                        grdsaleOrder.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleOrder); }));
                    }


                });
                th2.Start();

                System.Threading.Thread th3 = new System.Threading.Thread(() =>
                {

                    recheckInnoke:

                    if (grdsaleOrder.Dispatcher.CheckAccess())
                    {
                        // The calling thread owns the dispatcher, and hence the UI element
                        grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                        //grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm"));
                        //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentTerm"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("saleOrderStatus"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendor"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("employee"));
                       // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("department"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm_Id"));
                        //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentterm_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("allocation_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("dept_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user_Id"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentId"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentStatus"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheetId"));
                        grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheet"));
                        grdsaleOrder.Columns.GetColumnByFieldName("referenceNo").Header = "SO Refrence";
                    }
                    else
                    {
                        // Invokation required
                        grdsaleOrder.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                            //grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm"));
                            //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentTerm"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("saleOrderStatus"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendor"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("employee"));
                           // grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("department"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm_Id"));
                            //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentterm_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("allocation_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("dept_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user_Id"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentId"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentStatus"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheetId"));
                            grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheet"));
                            grdsaleOrder.Columns.GetColumnByFieldName("referenceNo").Header = "SO Refrence";
                        }));
                    }
                });
                th3.Start();
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
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
            else if (dateTo.EditValue == null)
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
                if (SYSTEM_STATIC.currentUser.employee != null && SYSTEM_STATIC.currentUser.employee.SODataRetrievalDate != null)
                {
                    if(SYSTEM_STATIC.currentUser.employee.SODataRetrievalDate > dateFrom.DateTime)
                    {
                        if(SYSTEM_STATIC.currentUser.employee.AllowOpenTransactions != true)
                        {
                            DXMessageBox.Show("Date from cannot be less than " + SYSTEM_STATIC.currentUser.employee.SODataRetrievalDate);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = null;
                            });
                            return;
                        }
                    }

                    if (SYSTEM_STATIC.currentUser.employee.SODataRetrievalDate > dateTo.DateTime)
                    {
                        if (SYSTEM_STATIC.currentUser.employee.AllowOpenTransactions != true)
                        {
                            DXMessageBox.Show("Date To cannot be less than " + SYSTEM_STATIC.currentUser.employee.SODataRetrievalDate);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = null;
                            });
                            return;
                        }
                    }
                }

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
            if (lblHeading.Text == "Sale Orders(Open)" || lblHeading.Text == "Sale Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        saleOrders = saleOrderrepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Orders") != null)
                    {
                        saleOrderrepo = new SaleOrderRepo();
                        saleOrders = saleOrderrepo.ActiveInActiveSoByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                
                    }
                    else
                    {
                        saleOrders = saleOrderrepo.AllActivebyDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                       

                    }
                }
                else if (AllActive == 1)
                {
                    saleOrders = saleOrderrepo.AllActivebyDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                  

                }
                else if (AllActive == 2)
                {
                    saleOrders = saleOrderrepo.AllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                   

                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Sale Orders)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleOrder List") != null)
                        {
                            //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            saleOrders = saleOrderrepo.AllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                          
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders = saleOrderrepo.AllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                               
                            }
                            else
                            {
                                saleOrders = saleOrderrepo.AllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                          
                            }
                        }
                    else
                    {
                        saleOrders = saleOrderrepo.AllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    }
                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;

                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Sale Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleOrder List") != null)
                        {
                            saleOrders = saleOrderrepo.PendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                           
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders = saleOrderrepo.AllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                               

                            }
                            else
                            {
                                saleOrders = saleOrderrepo.AllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                               
                            }
                        }
                    else
                    {
                        saleOrders = saleOrderrepo.AllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                        
  
                    }
                   // grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                }
                else
                if (AllActive == 5)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending for ReApprovals Sale Orders";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleOrder List") != null)
                        {
                            saleOrders = saleOrderrepo.PendingForReApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrders = saleOrderrepo.PendingForReApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                
                            }

                            else
                            {
                                saleOrders = saleOrderrepo.PendingForReApprovalOwnByDateRamge((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                
                            }
                        }
                    else
                    {
                        saleOrders = saleOrderrepo.AllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                        
                    }

                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                    AllActive = 5;
                }
                else
                    if (AllActive == 6)
                {

                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Sale Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                        {
                            saleOrders = saleOrderrepo.SaleRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            //All bills instead of own bills
                            //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                            saleOrders = saleOrderrepo.SaleRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    else
                        saleOrders = saleOrderrepo.getSaleRegisterAdministrator();
                    //grdsaleOrder.ItemsSource = saleOrders;
                    //grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
                    statusId = 0;
                    AllActive = 6;
                }
            }
            else
            {
                saleOrders = saleOrderrepo.AllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
          
            }

            grdsaleOrder.ItemsSource = saleOrders.Distinct();
            grdsaleOrder.RefreshData();
        }

        private void BtnDateFilterSplitPER_Click(object sender, RoutedEventArgs e)

        {
            if(datYearSplitPER.EditValue != null && dateMonthSplitPER.EditValue != null)
            {
                var year = datYearSplitPER.DateTime.Year;
                var month = dateMonthSplitPER.DateTime.Month;

                //grdSplitPER.FilterString = "([Year] Between " + year + "#)" + " AND ([Year] >= #1/1/" + year + "#)";
                grdSplitPER.FilterString = "((([Year] >= #1/1/"+year+"#)" + " AND ([Year] <= #12/31/" + (year+1) + "#))"
                    + " AND (([Month] >= #"+month+"/1/" + year + "#)" + " AND ([Month] <= #" + month +"/28/" + year + "#)))";
                grdsaleOrder.RefreshRow(0);
                //grdSplitPER.FilterString = "[Year] between ( " + year + "and" + year + " ')" /*+ "([Month]=" + month + ")"*/;
                // grdSplitPER.FilterString = String.Format("Start > #{0:MM/dd/yyyy}# AND [Finish] < #{1:MM/dd/yyyy}#", deStart.DateTime, deFinish.DateTime); xx;
            }
            else if (datYearSplitPER.EditValue != null && dateMonthSplitPER.EditValue == null)
            {
                var year = datYearSplitPER.DateTime.Year;
                var month = dateMonthSplitPER.DateTime.Month;

                //grdSplitPER.FilterString = "([Year] Between " + year + "#)" + " AND ([Year] >= #1/1/" + year + "#)";
                grdSplitPER.FilterString = "(([Year] >= #1/1/" + year + "#)" + " AND ([Year] <= #12/31/" + (year + 1) + "#))";
                grdsaleOrder.RefreshRow(0);
                //grdSplitPER.FilterString = "[Year] between ( " + year + "and" + year + " ')" /*+ "([Month]=" + month + ")"*/;
                // grdSplitPER.FilterString = String.Format("Start > #{0:MM/dd/yyyy}# AND [Finish] < #{1:MM/dd/yyyy}#", deStart.DateTime, deFinish.DateTime); xx;
            }
        }

        private void GrdSplitPER_FilterChanged(object sender, RoutedEventArgs e)
        {
            var filter = grdSplitPER.FilterString;
            
            var fltr = filter;
        }

        private void MbtnCopyTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Copy Sale Order Template") != null)
            {

                if (grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")) != null)
                {

                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")), true);


                    procurmentPanel.Show();

                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Copy Sale Order Template", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);

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

        private void GrdsaleOrder_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void grdsaleOrder_Loaded(object sender, RoutedEventArgs e)
        {
            //var soAmountValue = grdsaleOrder.Columns["SOAmountMER"].TotalSummaries[0].Value;
            //var bmGrossProfitValue = grdsaleOrder.Columns["BMgrossProfitSE"].TotalSummaries[0].Value;

            //if (soAmountValue != null && bmGrossProfitValue != null && Convert.ToDouble(soAmountValue) != 0)
            //{
            //    var SOamountMER = Convert.ToDouble(soAmountValue);
            //    var BMgrossProfitSE = Convert.ToDouble(bmGrossProfitValue);

            //    var percentage = (BMgrossProfitSE / SOamountMER) * 100;

            //    // Assign percentage value to another column's summary
            //    grdsaleOrder.Columns["BMgrossProfitSEPercent"].TotalSummaries.Clear(); // Clear existing summaries if needed
            //    grdsaleOrder.Columns["BMgrossProfitSEPercent"].TotalSummaries.Add(new GridSummaryItem()
            //    {
            //        SummaryType = DevExpress.Data.SummaryItemType.Custom, // Or another type if needed
            //        DisplayFormat = $"{percentage:F2}%", // Format to show percentage
            //        Tag = "CustomPercentage" // Optional, to identify this summary if needed
            //    });
            //}
            //else
            //{
            //    Console.WriteLine("Invalid data or SOamountMER is zero.");
            //}

        }


        double soAmountSE = 0, soAmountME = 0;
        double bmGrossProfitSE = 0, bmGrossProfitME = 0; // Example for an additional column
        double smGrossProfitSE = 0, smGrossProfitME = 0;

        private void dateFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
        }

        private void dateTo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        double amGrossProfitSE = 0, amGrossProfitME = 0;
        double percentageTotal = 0;
        private void grdsaleOrder_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            //if (e.IsTotalSummary)
            //{
            //    GridControl gridControl = sender as GridControl;

            //    switch (e.SummaryProcess)
            //    {
            //        case CustomSummaryProcess.Start:
            //            soAmountSE = 0;
            //            bmGrossProfitSE = 0;
            //            break;
            //        case CustomSummaryProcess.Calculate:
            //            soAmountSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["SOAmountSER"]));
            //            bmGrossProfitSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["BMgrossProfitSE"]));

            //            //Total = debitTotal - creditTotal;
            //            break;
            //        case CustomSummaryProcess.Finalize:

            //            e.TotalValue = (bmGrossProfitSE / soAmountSE) * 100;
            //            break;
            //    }
            //}


            if (e.IsTotalSummary)
            {
                // Reset totals at the start of the summary process
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    soAmountSE = 0;
                    soAmountME = 0;
                    bmGrossProfitSE = 0;
                    bmGrossProfitME = 0;
                    smGrossProfitSE = 0;
                    smGrossProfitME = 0;
                    amGrossProfitSE = 0;
                    amGrossProfitME = 0;
                }

                // Perform calculations for each row during the summary process
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    // Access cell values for specific columns
                    if (e.RowHandle != GridControl.InvalidRowHandle)
                    {
                        soAmountSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["SOAmountSER"]));
                        soAmountME += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["SOAmountMER"]));

                        bmGrossProfitSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["BMgrossProfitSE"]));
                        bmGrossProfitME += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["BMgrossProfitME"]));

                        smGrossProfitSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["SalesSystemMargin1"]));
                        smGrossProfitME += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["SalesMarketMargin1"]));

                        amGrossProfitSE += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["actualMarginSER"]));
                        amGrossProfitME += Convert.ToDouble(grdsaleOrder.GetCellValue(e.RowHandle, grdsaleOrder.Columns["actualMarginMER"]));
                    }
                }

                // Finalize the calculation based on the summary item's FieldName
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    var summaryItem = e.Item as GridSummaryItem; // Cast the item to GridSummaryItem
                    if (summaryItem != null)
                    {
                        if (summaryItem.FieldName == "BMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (bmGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "BMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (bmGrossProfitME / soAmountME) * 100 : 0;
                        }
                        if (summaryItem.FieldName == "SMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (smGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "SMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (smGrossProfitME / soAmountME) * 100 : 0;
                        }
                        if (summaryItem.FieldName == "AMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (amGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "AMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (amGrossProfitME / soAmountME) * 100 : 0;
                        }
                    }
                }
            }
        }
    }
}
