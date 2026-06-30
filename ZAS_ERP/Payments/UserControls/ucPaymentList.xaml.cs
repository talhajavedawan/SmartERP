using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucPaymentList.xaml
    /// </summary>
    public partial class ucPaymentList : UserControl
    {
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }

        PaymentRepo paymentRepo = new PaymentRepo();
        //ucFrmPayments frmPayments = new ucFrmPayments();
        List<Payment> payments = new List<Payment>();

        SaleOrderRepo saleOrderRepo = new SaleOrderRepo(); 
        static PaymentStatus statusChanged = new PaymentStatus();
        
       
        public ucPaymentList()
        {
            this.DataContext = this;
            InitializeComponent();
            LoadPaymentCounters();
        }

        private void LoadPaymentCounters()
        {
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Payment List") != null)
                {
                    ApprovalCount = paymentRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Payment") != null)
                    {
                        ApprovalCount = paymentRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = paymentRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Payment List") != null)
                {
                    ReApprovalCount = paymentRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Payment") != null)
                    {
                        ReApprovalCount = paymentRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = paymentRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Payment") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = paymentRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = paymentRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payment") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = paymentRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
                    //}
                    //else
                    //{
                    //    ApproveunapprovedCount = bankTransRepo.getBankTransferRegisterCountOWn(MainWindow.currentUserid);
                    //}
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = paymentRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = paymentRepo.getPaymentAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Payment List") != null)
                {
                    ClosingCount = paymentRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Payment") != null)
                    {
                        ClosingCount = paymentRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = paymentRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = paymentRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            payments = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
            grdPaymentRegister.ItemsSource = payments;
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            GridControlSetUserSettings();
            this.DataContext = this;
            LoadPaymentCounters();
        }

        public void GridControlSetUserSettings()
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPaymentRegister);
        }


        private void GrdPaymentList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            switch (row.transactionType)
            {
                case PaymentTransactionType.Admin_Bills:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();

                            if(row.adminBill != null)
                            {
                                var node = row.adminBill.department;

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

                    }
                    if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                    break;
                case PaymentTransactionType.Loans_Advances:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                    break;
                case PaymentTransactionType.Purchase_Invoice:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                    break;
                case PaymentTransactionType.Vendor_Bills:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                    break;
            }

            //if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First(); //get first item from department list

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }
            //        }
            //        deptList.Reverse();
            //        e.Value = deptList[0].DeptName;
            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }


            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //        }
            //        //if (deptList.Count == 1)
            //        //{
            //        //    e.Value = deptList[0].DeptName;
            //        //}
            //        //if (deptList.Count == 2)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 3)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 4)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 5)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}

            //        //int index = 1;
            //        //if (index >= 0 && index < deptList.Count)
            //        //{

            //        //    e.Value = deptList[1].DeptName;
            //        //}

            //        //if (deptList.Count <= 1)
            //        //{
            //        //    e.Value = dep;
            //        //}
            //        //else
            //        //{
            //        //    int index = 1;
            //        //    if (index >= 0 && index < deptList.Count)
            //        //    {

            //        //        e.Value = deptList[1].DeptName;
            //        //        dep = deptList[1].DeptName;
            //        //    }
            //        //}

            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }
            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //        }

            //        //if (deptList.Count == 1)
            //        //{
            //        //    e.Value = deptList[0].DeptName;
            //        //}
            //        //if (deptList.Count == 2)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 3)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}
            //        //if (deptList.Count == 4)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}
            //        //if (deptList.Count == 5)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}


            //        //if (deptList.Count <= 2)
            //        //{
            //        //    e.Value = dep;
            //        //}
            //        //else
            //        //{
            //        //    int index = 2;
            //        //    if (index >= 0 && index < deptList.Count)
            //        //    {

            //        //        e.Value = deptList[2].DeptName;
            //        //        dep = deptList[2].DeptName;
            //        //    }
            //        //}


            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }
            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //        }

            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {

            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }







            //            //foreach (var _dept in fDepartm) 
            //            //{
            //            //    var ddept = _dept.parentDepartment;

            //            //    while (ddept != null)
            //            //    {
            //            //        if (ddept.ParentID != null)
            //            //        {
            //            //            if (ddept.ParentID != ddept.Id)
            //            //            {
            //            //                //this will add current node to department list and tranverse to its parent
            //            //                deptList.Add(ddept);
            //            //                ddept = ddept.parentDepartment;
            //            //            }
            //            //            else
            //            //            {
            //            //                //when node is parent to itself
            //            //                deptList.Add(ddept);
            //            //                break;
            //            //            }
            //            //        }
            //            //        else
            //            //        {
            //            //            //parent with parent id is null
            //            //            deptList.Add(ddept);
            //            //            break;
            //            //        }
            //            //    }
            //            //}
            //        }



            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[4].DeptName;
            //                break;
            //        }

            //    }

            //}


            if (e.Column.FieldName == "Depts" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string deptNames = "";

                if (pymnt.departments != null && pymnt.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", pymnt.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }
            if (e.Column.FieldName == "Vendor1" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string vendor = "";

                if (pymnt.vendor != null && pymnt.vendor.company != null)
                    vendor = pymnt.vendor.company.CompanyName;
                else
                {
                    if (pymnt.transactionType == PaymentTransactionType.Admin_Bills && pymnt.adminBill != null && pymnt.adminBill.vendor != null)
                        vendor = pymnt.adminBill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.vendor != null)
                        vendor = pymnt.Bill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.vendors != null && pymnt.purchaseInvoice.PurchaseOrder.vendors.Count >  0)
                        vendor = pymnt.purchaseInvoice.PurchaseOrder.vendors[0].company.CompanyName;
                }

                e.Value = vendor;
            }
            if (e.Column.FieldName == "Customer" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string customer = "";


                if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.customerCompany != null)
                    customer = pymnt.Bill.customerCompany.company.CompanyName;
                else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.customerCompany != null )
                    customer = pymnt.purchaseInvoice.PurchaseOrder.customerCompany.company.CompanyName;


                e.Value = customer;
            }
            if (e.Column.FieldName == "SystemCost" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                if (pymnt.CostSheetId != null)
                {
                    var systemCost = saleOrderRepo.GetPaymentSystemCost((int)pymnt.CostSheetId,pymnt.Id);
                    e.Value = systemCost;
                }
              
            }
        }

        public void update_Payment(int transaction_id)
        {           
                var pymnt = paymentRepo.GetPaymentByGroupId(transaction_id);

                if(pymnt != null)
                {
                    switch (pymnt.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                        {
                            ucFrmPayments frmPayments = new ucFrmPayments();
                            
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

                            if (pymnt!= null)
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
                }
            }
        }

        private void GrdPaymentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedPayment = grdPaymentRegister.SelectedItem as Payment;

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

                            switch (selectedPayment.loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Loan:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
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
                                default:
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
                            


                            break;
                        case PaymentTransactionType.Target_Reward:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                            {
                                ucFrmTargetRewardPayment frmTRPayment = new ucFrmTargetRewardPayment();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRPayment.editFlag = true;
                                        frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                        frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                        frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRPayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRPayment.editFlag = true;
                                    frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                    frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                    frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRPayment.frmPiPaymentWindow.Show();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                Window moduleWin = new Window();

                ucFrmModuleSelect frmModuleSelect = new ucFrmModuleSelect();
                //ucFrmPayments frmBillPayments = new ucFrmPayments();

                moduleWin.Content = frmModuleSelect;

                //enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //enterPaymentWin.WindowState = WindowState.Maximized;

                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.Width = 400;
                moduleWin.Height = 250;
                moduleWin.ResizeMode = ResizeMode.CanMinimize;
                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Payment!");
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedPayment = grdPaymentRegister.SelectedItem as Payment;

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

                            switch (selectedPayment.loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Loan:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
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
                                default:
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



                            break;
                        case PaymentTransactionType.Target_Reward:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                            {
                                ucFrmTargetRewardPayment frmTRPayment = new ucFrmTargetRewardPayment();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRPayment.editFlag = true;
                                        frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                        frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                        frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRPayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRPayment.editFlag = true;
                                    frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                    frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                    frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRPayment.frmPiPaymentWindow.Show();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdPaymentRegister.GetFocusedRow() != null)
                {
                    paymentRepo = new PaymentRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (Payment)grdPaymentRegister.GetFocusedRow();
                    var payments = paymentRepo.GetPaymentForApproval(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();
                    
                    if (payments != null && payments.Count > 0)
                    {
                        List<Department> deptss = new List<Department>();
                        if (selectedRow.departments != null)
                            deptss = selectedRow.departments;

                        if (payments[0].isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null) ? true : false)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].isApproved = true;
                                    payments[i].stage = TransactionStage.Approved.ToString();
                                    payments[i].ApprovedDate = DateTime.Now;
                                }
                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, 17, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                paymentRepo.ApprovePayment(payments, deptss);

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Payments are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Payments Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payments Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (payments[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Payment") != null) ? true : false)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].isReApproved = true;
                                    payments[i].stage = TransactionStage.Approved.ToString();
                                    payments[i].ReApprovalDate = DateTime.Now;
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Payments, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                paymentRepo.ApprovePayment(payments, deptss);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Payments are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Payments Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payments Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                    }
                    //Load_Receipts();
                }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Payment List") != null)
            {
                //paymentRepo = new PaymentRepo();
                lblHeading.Text = "Pending for Approval Payments";
                payments = paymentRepo.GetAllPendingForApprovalPayments(SYSTEM_STATIC.currentUser.id);
                grdPaymentRegister.ItemsSource = payments;
                //grdPaymentRegister.RefreshData();
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Payments!!");
            }
        }


        public ucPaymentList(PaymentStatus status)
        {
            statusChanged = status;
            //InitializeComponent();
            //receipt_register_win.Closing += ReceiptRegister_Window_Closing;
            //grdSaleReceiptList.Columns["SerialNo"].Visible = false;
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            ProcurementRepo procurementRepo = new ProcurementRepo();
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            UsersRepo usersRepo = new UsersRepo();
            paymentRepo = new PaymentRepo();
            var selectedRow = (Payment)grdPaymentRegister.GetFocusedRow();


            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var payments = paymentRepo.GetPaymentForApproval(selectedRow.transactionGroupId);
                if (payments != null && payments.Count > 0)
                {
                    List<Department> deptss = new List<Department>();
                    if (selectedRow.departments != null)
                        deptss = selectedRow.departments;

                    if (payments[0].isApproved == false)
                    {
                        DXMessageBox.Show("Payments are pending for approval!");
                        return;
                    }
                    statusChanged = null;
                    ucFrmPaymentDirectClose ucFrmDirectClose = new ucFrmPaymentDirectClose();
                    if (selectedRow.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = selectedRow.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Payment without Approval") != null)
                        {
                            for (int i = 0; i < payments.Count; i++)
                            {
                                payments[i].PendingForClosing = false;
                                payments[i].stage = TransactionStage.Closed.ToString();
                                payments[i].statusId = statusChanged.Id;
                                payments[i].LastStatusChangeDate = System.DateTime.Now;
                                payments[i].ClosingDate = System.DateTime.Now;


                            }
                            paymentRepo.ApprovePayment(payments, deptss);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (payments.Count != 0)
                                {
                                    procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payemnts #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ",null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < payments.Count; i++)
                            {
                                payments[i].PendingForClosing = true;
                                payments[i].stage = TransactionStage.AwaitingApproval.ToString();
                                payments[i].statusId = statusChanged.Id;
                                payments[i].LastStatusChangeDate = System.DateTime.Now;
                                payments[i].ClosingDate = System.DateTime.Now;


                            }
                            paymentRepo.ApprovePayment(payments, deptss);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (payments.Count != 0)
                                {
                                    procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPaymentRegister);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            paymentRepo = new PaymentRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                var payments = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
                grdPaymentRegister.ItemsSource = payments;
            }
            else
            {
                grdPaymentRegister.ItemsSource = null;
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPaymentRegister);
            lblHeading.Text = "Payments";
            ucPaymentList paymentList = new ucPaymentList();
            this.DataContext = paymentList;
           // LoadPaymentCounters();
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Payments Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
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
                        ReportLogic.SaveGridReport(grdPaymentRegister, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission Export Payments Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        
        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            //paymentRepo = new PaymentRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Payment List") != null)
            {
                lblHeading.Text = "Pending for ReApproval Payments";
                payments = paymentRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);

                grdPaymentRegister.ItemsSource = payments;
                //grdPaymentRegister.RefreshData();
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Payments!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Payment List") != null)
            {
                payments = paymentRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Payments";
                grdPaymentRegister.ItemsSource = payments;
                //grdPaymentRegister.RefreshData();
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Payments!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                UsersRepo usersRepo = new UsersRepo();                
                lblHeading.Text = "Payment Register";
                if (MainWindow.currentUserid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Payment") != null)
                    {
                        payments = paymentRepo.getPaymentRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        payments = paymentRepo.getPaymentRegister(MainWindow.currentUserid);
                    }
                }

                else
                {
                    payments = paymentRepo.getPaymentAdministrator();
                }
                grdPaymentRegister.ItemsSource = payments;
                //grdPaymentRegister.RefreshData();
            }
            else
            {
                MessageBox.Show("Permission required to View list of Payments!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            //paymentRepo = new PaymentRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Payment") != null)
            {
                lblHeading.Text = "Void Payments";

                payments = paymentRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdPaymentRegister.ItemsSource = payments;
                //grdPaymentRegister.RefreshData();
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Payments!!");
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
    }
}
