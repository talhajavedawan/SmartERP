using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucTaskAdd.xaml
    /// </summary>
    public partial class ucTaskAdd : UserControl
    {
        public TransactionItemType? transactionType = new TransactionItemType();
        public int transactionId = 0;
        public bool editFlag = false;
        public int taskId = 0;
        Tasks task = new Tasks();
        TaskRepo taskRepo = new TaskRepo();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();

        public TasksStatus checkStatus = new TasksStatus();
        TasksStatus oldStatus = new TasksStatus();
        static TasksStatus statusChanged = new TasksStatus();
        List<User> AllAllowedUser = new List<User>();
        List<User> SelectedAllowedUser = new List<User>();

        List<TaskEfficiency> taskEfficiencies = new List<TaskEfficiency>();
        Tasks trackingOrder = new Tasks();
        List<TasksStatus> tasksStatuses = new List<TasksStatus>();

        public ucTaskAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            LoadModuleTypes();
            LoadCompanies();
            LoadWareHouse();
            LoadCurrencies();
            //LoadTaskStatuses();
            GellAllOrdersTracking();

            cmbxModuleType.IsReadOnly = true;
            if(editFlag == false && taskId == 0)
            {
                btnSaveExecutionStyle.IsEnabled = false;
                btnSaveTracking.IsEnabled = false;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                datCreationDate.DateTime = DateTime.Now;
                datCompletionDate.IsEnabled = false;

                if(transactionType != null && transactionId != 0)
                {
                    var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                    List<CustomerCompany> customerList = new List<CustomerCompany>();
                    switch (transactionType)
                    {
                        case TransactionItemType.Sale_Order:
                            SaleOrder saleOrder = new SaleOrder();
                            saleOrder = taskRepo.GetSaleOrder(transactionId);

                            cmbxModuleType.Text = TransactionItemType.Sale_Order.ToString();
                            lookupCompany.EditValue = saleOrder.company_Id;
                            //lookupDepartment.EditValue = saleOrder.dept_Id;

                            var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            if (saleOrder.dept_Id != 0)
                            {
                                if (deptList.Find(x => x.Id == saleOrder.dept_Id) == null)
                                {
                                    deptList.Add(saleOrder.department);
                                    lookupDepartment.ItemsSource = null;
                                    lookupDepartment.ItemsSource = deptList;
                                }

                                lookupDepartment.EditValue = saleOrder.dept_Id;
                            }

                            customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                            if (saleOrder.customerCompany_Id != 0)
                            {
                                if (customerList.Find(x => x.Id == saleOrder.customerCompany_Id) == null)
                                {
                                    customerList.Add(saleOrder.customerCompany);
                                    lookupCustomer.ItemsSource = customerList;
                                }
                                lookupCustomer.EditValue = saleOrder.customerCompany_Id;
                            }

                            if (saleOrder.currency_Id != 0)
                            {
                                int index = 0;
                                foreach (var _curr in currencyList)
                                {

                                    if (_curr.id == saleOrder.currency_Id)
                                    {
                                        cmbCurrency.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }
                            }

                            txtSystemAmount.Text = saleOrder.totalCFRValue.ToString();

                            break;
                        case TransactionItemType.Purchase_Order:
                            PurchaseOrder PO = new PurchaseOrder();
                            PO = taskRepo.GetPurchaseOrder(transactionId);

                            cmbxModuleType.Text = TransactionItemType.Purchase_Order.ToString();
                            lookupCompany.EditValue = PO.company_Id;
                            var deptList1 = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            if (PO.dept_Id != null)
                            {
                                if (deptList1.Find(x => x.Id == PO.dept_Id) == null)
                                {
                                    deptList1.Add(PO.department);
                                    lookupDepartment.ItemsSource = null;
                                    lookupDepartment.ItemsSource = deptList1;
                                }

                                lookupDepartment.EditValue = PO.dept_Id;
                            }
                            customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                            if (PO.customerCompany_Id != 0)
                            {
                                if (customerList.Find(x => x.Id == PO.customerCompany_Id) == null)
                                {
                                    customerList.Add(PO.customerCompany);
                                    lookupCustomer.ItemsSource = customerList;
                                }
                                lookupCustomer.EditValue = PO.customerCompany_Id;
                            }

                            if (PO.currency_Id != 0)
                            {
                                int index = 0;
                                foreach (var _curr in currencyList)
                                {

                                    if (_curr.id == PO.currency_Id)
                                    {
                                        cmbCurrency.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }
                            }

                            txtSystemAmount.Text = PO.totalCFRValue.ToString();

                            break;

                        case TransactionItemType.Sale_Invoice:
                            SaleInvoice SI = new SaleInvoice();
                            SI = taskRepo.GetSaleInvoice(transactionId);

                            cmbxModuleType.Text = TransactionItemType.Sale_Invoice.ToString();
                            lookupCompany.EditValue = SI.company_Id;
                            var deptList2 = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            if (SI.dept_Id != 0)
                            {
                                if (deptList2.Find(x => x.Id == SI.dept_Id) == null)
                                {
                                    deptList2.Add(SI.department);
                                    lookupDepartment.ItemsSource = null;
                                    lookupDepartment.ItemsSource = deptList2;
                                }

                                lookupDepartment.EditValue = SI.dept_Id;
                            }
                            customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                            if (SI.customerCompany_Id != 0)
                            {
                                if (customerList.Find(x => x.Id == SI.customerCompany_Id) == null)
                                {
                                    customerList.Add(SI.customerCompany);
                                    lookupCustomer.ItemsSource = customerList;
                                }
                                lookupCustomer.EditValue = SI.customerCompany_Id;
                            }

                            if (SI.currency_Id != 0)
                            {
                                int index = 0;
                                foreach (var _curr in currencyList)
                                {

                                    if (_curr.id == SI.currency_Id)
                                    {
                                        cmbCurrency.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }
                            }

                            txtSystemAmount.Text = SI.totalInvoiceAmount.ToString();

                            break;

                        case TransactionItemType.Offer:
                            Offer offer = new Offer();
                            offer = taskRepo.GetOffer(transactionId);

                            cmbxModuleType.Text = TransactionItemType.Offer.ToString();
                            lookupCompany.EditValue = offer.company_Id;

                            var deptList3 = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            if (offer.dept_Id != 0)
                            {
                                if (deptList3.Find(x => x.Id == offer.dept_Id) == null)
                                {
                                    deptList3.Add(offer.department);
                                    lookupDepartment.ItemsSource = null;
                                    lookupDepartment.ItemsSource = deptList3;
                                }

                                lookupDepartment.EditValue = offer.dept_Id;
                            }

                            customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                            if (offer.customerCompany_Id != 0)
                            {
                                if (customerList.Find(x => x.Id == offer.customerCompany_Id) == null)
                                {
                                    customerList.Add(offer.customerCompany);
                                    lookupCustomer.ItemsSource = customerList;
                                }
                                lookupCustomer.EditValue = offer.customerCompany_Id;
                            }

                            if (offer.currency_Id != 0)
                            {
                                int index = 0;
                                foreach (var _curr in currencyList)
                                {

                                    if (_curr.id == offer.currency_Id)
                                    {
                                        cmbCurrency.SelectedIndex = index;
                                        index = 0;
                                        break;
                                    }
                                    index++;
                                }
                            }

                            txtSystemAmount.Text = offer.totalCFRValue.ToString();

                            break;

                        case TransactionItemType.Inquiry:
                            Inquiry inquiry = new Inquiry();
                            inquiry = taskRepo.GetInquiry(transactionId);

                            cmbxModuleType.Text = TransactionItemType.Sale_Invoice.ToString();
                            lookupCompany.EditValue = inquiry.company_Id;
                            var deptList4 = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                            if (inquiry.dept_Id != 0)
                            {
                                if (deptList4.Find(x => x.Id == inquiry.dept_Id) == null)
                                {
                                    deptList4.Add(inquiry.department);
                                    lookupDepartment.ItemsSource = null;
                                    lookupDepartment.ItemsSource = deptList4;
                                }

                                lookupDepartment.EditValue = inquiry.dept_Id;
                            }
                            if (inquiry.customerCompany_Id != 0)
                                lookupCustomer.EditValue = inquiry.customerCompany.Id;

                            //if (inquiry.currency_Id != 0)
                            //{
                            //    int index = 0;
                            //    foreach (var _curr in currencyList)
                            //    {

                            //        if (_curr.id == SI.currency_Id)
                            //        {
                            //            cmbCurrency.SelectedIndex = index;
                            //            index = 0;
                            //            break;
                            //        }
                            //        index++;
                            //    }
                            //}

                            //txtSystemAmount.Text = inquiry..ToString();

                            break;
                        case TransactionItemType.UnDefined:
                            cmbxModuleType.Text = "NA";
                            break;
                    }
                }
                else
                {
                    cmbxModuleType.Text = "NA";
                }
                grdCntrlUsersSelected.ItemsSource = SelectedAllowedUser;
            }

            if(editFlag == true && taskId > 0)
            {
                task = taskRepo.GetTask(taskId);

                btnSaveTracking.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Tracking Details") != null) ? true : false;


                if (task.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit User Task Under Approval") != null)
                {
                    btnSave.IsEnabled = true;
                }   
                else if (task.isApproved != false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit User Task") != null)
                {
                    btnSave.IsEnabled = true;
                }
                else
                {
                    btnSave.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit and Save Task Execution Details") == null)
                {
                    btnSaveExecutionStyle.IsEnabled = false;
                }

                

                if (task.transactionType == TransactionItemType.Purchase_Order)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Checklist") != null)
                        tabChecklist.Visibility = Visibility.Visible;
                }
                    

                if(SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Efficiency Points in Tasks") != null)
                    btnAddEfficiencyPoints.Visibility = Visibility.Visible;

                taskEfficiencies = task.TaskEfficiencies;
                if(taskEfficiencies != null)
                {
                    var achievedPoints = taskEfficiencies.Sum(x => x.AchievedPoints);
                    var totalPoints = taskEfficiencies.Where(x => x.TotalPoints >= 0).Sum(x => x.TotalPoints);
                    txtEfficiencyPoints.Text = achievedPoints + "/" + totalPoints;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Users in Tasks (Not creator of the Task)") != null || task.creatorId == SYSTEM_STATIC.currentUser.id)
                {
                    tabUsers.Visibility = Visibility.Visible;
                }
                else
                {
                    tabUsers.Visibility = Visibility.Collapsed;
                }

                transactionId = task.transactionId;
                transactionType = task.transactionType;

                //if (task.isVoid == true)
                //{
                //    grdVoid.Visibility = Visibility.Visible;
                //    txtVoid.RenderTransform = new RotateTransform(-45);
                //}
                //else if(task.Status.isActive == true)
                //{
                //    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                //}
                //else if (task.Status.isActive == false)
                //{
                //    grdApproved.Background = Brushes.DeepSkyBlue;
                //    grdClosed.Background = Brushes.DeepSkyBlue;
                //}

                if (task.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (task.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved != false && task.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (task.isApproved != false && task.Status?.isActive == false && task.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (task.isApproved != false && task.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved != false)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }


                grdTaskTracking.ItemsSource = task.TaskTrackings;

                lblRefNo.Text = task.SystemRef;
                txtLotNo.Text = task.LotNo;

                if (task.ImportBillOfEntry == true)
                    btnImportBillOfEntry.IsChecked = true;
                else
                    btnImportBillOfEntry.IsChecked = false;

                if (task.ExportBillOfEntry == true)
                    btnExportBillOfEntry.IsChecked = true;
                else
                    btnExportBillOfEntry.IsChecked = false;

                if (task.isCompleted == true)
                {
                    var curDate = task.StartDate.Value; new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime startDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0);
                    var date = task.CompletionDate.Value;
                    DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    var timeSpan = startDate.Subtract(targetDate);


                    txtDays.Text = "Completed In";
                    txtDays.FontSize = 18;
                    txtDaysRemaining.Text = System.Math.Abs(timeSpan.Days).ToString() + " Days";
                    txtDaysRemaining.FontSize = 20;
                    txtDaysRemaining.VerticalAlignment = VerticalAlignment.Center;
                    txtDaysIndicator.Background = Brushes.DeepSkyBlue;

                }
                else
                {
                    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    if(task.TentativeCompletionDate != null)
                    {
                        var date = task.TentativeCompletionDate.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);

                        if (timeSpan.Days <= 0)
                        {
                            txtDays.Text = System.Math.Abs(timeSpan.Days).ToString();
                            txtDaysRemaining.Text = "Days Remaining";
                            txtDaysIndicator.Background = Brushes.DeepSkyBlue;
                        }
                        else
                        {
                            txtDays.Text = System.Math.Abs(timeSpan.Days).ToString();
                            txtDaysRemaining.Text = "Days Passed";
                            txtDaysIndicator.Background = Brushes.Red;
                        }
                    }
                    
                }

                


                if (task.creationDate != null)
                    datCreationDate.DateTime = task.creationDate.Value;
                if (task.transactionType != null && task.transactionType != TransactionItemType.UnDefined)
                    cmbxModuleType.Text = transactionType.Value.ToString();
                else
                    cmbxModuleType.SelectedIndex = 0;

                var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                if (task.companyId != null )
                {
                    if(companyList.Find(x => x.Id == task.companyId) == null)
                    {
                        companyList.Add(task.company);
                        lookupCompany.ItemsSource = null;
                        lookupCompany.ItemsSource = companyList;
                    }
                        
                    
                    lookupCompany.EditValue = task.companyId;
                }


                var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                if (task.deptId != null)
                {
                    if (deptList.Find(x => x.Id == task.deptId) == null)
                    {
                        deptList.Add(task.department);
                        lookupDepartment.ItemsSource = null;
                        lookupDepartment.ItemsSource = deptList;
                    }
                        
                    lookupDepartment.EditValue = task.deptId;
                }

                if (task.warehouse != null)
                {
                    lookupWarehouse.Text = task.warehouse.WarehouseName;
                }

                if (task.lotNumber != null)
                {
                    lookupLotNumbers.Text = task.lotNumber.LotNo;
                }

                if (task.Status != null)
                {
                    var disAbleStatus = tasksStatuses.FirstOrDefault(x => x.Id == task.statusId);
                    if (disAbleStatus == null)
                    {
                       LoadTaskStatuses(task.Status);
                    }
                }
                //Select Status
                //var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                //if (task.Status != null)
                //{
                //    checkStatus = task.Status;
                //    int index = 0;
                //    foreach (var _status in statusList)
                //    {

                //        if (_status != null && _status.id == task.statusId)
                //        {
                //            cmbStatus.SelectedIndex = index;
                //            index = 0;
                //            break;
                //        }
                //        index++;
                //    }
                //}

                



                txtSystemRef.Text = task.SystemRef;
                txtTaskRef.Text = task.TaskRef;
                //txtEfficiencyPoints.Text = task.EfficiencyPoints.ToString();
                if (task.creator != null)
                    txtCreator.Text = task.creator.employee.person.FName + " " + task.creator.employee.person.LName;
                if (task.supervisedBy != null)
                    lookUpSupervisedBy.EditValue = task.supervisedBy.id;
                if (task.assignedBy != null)
                    lookUpAssignedBy.EditValue = task.assignedBy.id;
                if (task.assignedTo != null)
                    lookUpAssignedTo.EditValue = task.assignedTo.id;
                if (task.taskType != null)
                    lookupTaskType.EditValue = task.taskType.Id;


                if (task.Status != null)
                {
                    checkStatus = task.Status;
                    var StatusSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;
                    //if (task.Status.isActive == false)
                    //{
                    //    try
                    //    {
                    //        cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(StatusSource.Find(x => x.name == task.Status.Status))];
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        SystemLog.LogError(this.GetType(), "This User cannot see closed Task status! " + ex.ToString());
                    //    }
                    //}
                    //else
                    //{
                    if(StatusSource != null && StatusSource.Count > 0)
                    {
                        var taskStatus = StatusSource.Find(x => x.id == task.Status.Id);
                        if (taskStatus == null)
                        {
                            taskStatus = new cmbitem()
                            {
                                name = task.Status.Status,
                                id = task.Status.Id,
                                bcolor = task.Status.backcolor,
                                fcolor = "#FF000000"
                            };
                            StatusSource.Add(taskStatus);
                            cmbStatus.ItemsSource = null;
                            cmbStatus.ItemsSource = StatusSource;
                        }
                        cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(taskStatus)];
                    }
                    //}
                }


                foreach (var _selectedUser in task.AllowedUsers)
                {
                    SelectedAllowedUser.Add(_selectedUser);

                }
                grdCntrlUsersSelected.ItemsSource = null;
                grdCntrlUsersSelected.ItemsSource = SelectedAllowedUser;

                foreach (var rrr in SelectedAllowedUser)
                {
                    if (AllAllowedUser.Contains(rrr))
                    {
                        AllAllowedUser.Remove(rrr);
                    }
                }
                grdCntrlUsers.ItemsSource = null;
                grdCntrlUsers.ItemsSource = AllAllowedUser;

                //old Code for selected user
                //foreach (User _user in task.AllowedUsers)
                //    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));

                //Select Currency
                var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                if (task.currency != null)
                {
                    int index = 0;
                    foreach (var _curr in currencyList)
                    {

                        if (_curr.id == task.currencyId)
                        {
                            cmbCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (task.saleOrder != null)
                {
                    txtSystemAmount.Text = task.saleOrder.totalCFRValue.ToString();
                    var customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                    if (task.saleOrder.customerCompany_Id != 0)
                    {
                        if (customerList.Find(x => x.Id == task.saleOrder.customerCompany_Id) == null)
                        {
                            customerList.Add(task.saleOrder.customerCompany);
                            lookupCustomer.ItemsSource = null;
                            lookupCustomer.ItemsSource = customerList;
                        }
                        lookupCustomer.EditValue = task.saleOrder.customerCompany_Id;
                    }

                    txtSDrefNo.Text = task.saleOrder.SalesReferenceNo;
                    txtSalesRef.Text = task.saleOrder.referenceNo;

                    if(task.saleOrder.vendors != null && task.saleOrder.vendors.Count > 0)
                        txtVendorName.Text = task.saleOrder.vendors[0].company.CompanyName;
                }
                else if (task.saleInvoice != null)
                {
                    txtSystemAmount.Text = task.saleInvoice.totalInvoiceAmount.ToString();
                    var customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                    if (task.saleInvoice.customerCompany_Id != 0)
                    {
                        if (customerList.Find(x => x.Id == task.saleInvoice.customerCompany_Id) == null)
                        {
                            customerList.Add(task.saleInvoice.customerCompany);
                            lookupCustomer.ItemsSource = null;
                            lookupCustomer.ItemsSource = customerList;
                        }
                        lookupCustomer.EditValue = task.saleInvoice.customerCompany_Id;
                    }

                    if(task.saleInvoice.SaleOrder != null)
                    {
                        txtSDrefNo.Text = task.saleInvoice.SaleOrder.SalesReferenceNo;
                        txtSalesRef.Text = task.saleInvoice.SaleOrder.referenceNo;

                        if (task.saleInvoice.SaleOrder.vendors != null && task.saleInvoice.SaleOrder.vendors.Count > 0)
                            txtVendorName.Text = task.saleInvoice.SaleOrder.vendors[0].company.CompanyName;

                        txtSOamount.Text = task.saleInvoice.SaleOrder.totalCFRValue.ToString();
                        txtSOcurrency.Text = task.saleInvoice.SaleOrder.currency?.CurrencyName;
                    }
                    
                }
                else if (task.purchaseOrder != null)
                {
                    txtSystemAmount.Text = task.purchaseOrder.totalCFRValue.ToString();
                    var customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                    if (task.purchaseOrder.customerCompany_Id != 0)
                    {
                        if (customerList.Find(x => x.Id == task.purchaseOrder.customerCompany_Id) == null)
                        {
                            customerList.Add(task.purchaseOrder.customerCompany);
                            lookupCustomer.ItemsSource = null;
                            lookupCustomer.ItemsSource = customerList;
                        }
                        lookupCustomer.EditValue = task.purchaseOrder.customerCompany_Id;
                    }

                    if (task.purchaseOrder.SaleOrder != null)
                    {
                        txtSDrefNo.Text = task.purchaseOrder.SaleOrder.SalesReferenceNo;
                        txtSalesRef.Text = task.purchaseOrder.SaleOrder.referenceNo;

                        if (task.purchaseOrder.vendors != null && task.purchaseOrder.vendors.Count > 0)
                            txtVendorName.Text = task.purchaseOrder.vendors[0].company.CompanyName;

                        txtSOamount.Text = task.purchaseOrder.SaleOrder.totalCFRValue.ToString();
                        txtSOcurrency.Text = task.purchaseOrder.SaleOrder.currency?.CurrencyName;
                    }
                }
                else if (task.offer != null)
                {
                    txtSystemAmount.Text = task.offer.totalCFRValue.ToString();
                    var customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                    if (task.offer.customerCompany_Id != 0)
                    {
                        if (customerList.Find(x => x.Id == task.offer.customerCompany_Id) == null)
                        {
                            customerList.Add(task.offer.customerCompany);
                            lookupCustomer.ItemsSource = null;
                            lookupCustomer.ItemsSource = customerList;
                        }
                        lookupCustomer.EditValue = task.offer.customerCompany_Id;
                    }
                }
                else
                {
                    var customerList = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                    if (task.customerId != null)
                    {
                        if (customerList.Find(x => x.Id == task.customerId) == null)
                        {
                            customerList.Add(task.CustomerCompany);
                            lookupCustomer.ItemsSource = null;
                            lookupCustomer.ItemsSource = customerList;
                        }
                        lookupCustomer.EditValue = task.customerId;
                    }
                }

                txtManualAmount.Text = task.ManualAmount.ToString();

                if (task.StartDate != null)
                    datStartDate.EditValue = task.StartDate.Value;
                if (task.TentativeCompletionDate != null)
                    datTentativeCompletionDate.EditValue = task.TentativeCompletionDate.Value;

                if (task.isCompleted == true)
                    chkIsCompleted.IsChecked = true;
                else
                    chkIsCompleted.IsChecked = false;

                if (task.CompletionDate != null)
                    datCompletionDate.EditValue = task.CompletionDate.Value;
                txtDescription.Text = task.Description;


                if (task.TrackingNoIn != null)
                    txtTrackingNoIn.Text = task.TrackingNoIn;
                if (task.ETDin != null)
                    datETDin.EditValue = task.ETDin.Value;
                if (task.ETAin != null)
                    datETAin.EditValue = task.ETAin.Value;
                if (task.ADDin != null)
                    datADDin.EditValue = task.ADDin.Value;

                if (task.TrackingNoOut != null)
                    txtTrackingNoOut.Text = task.TrackingNoOut;
                if (task.ETDout != null)
                    datETDout.EditValue = task.ETDout.Value;
                if (task.ETAout != null)
                    datETAout.EditValue = task.ETAout.Value;
                if (task.ADDout != null)
                    datADDout.EditValue = task.ADDout.Value;

                chkOfficeSupportRequired.IsChecked = task.OfficeSupportRequired;
                chkLogisticSupportRequired.IsChecked = task.LositicSupportRequired;

                txtLogisticAreaFrom.Text = task.LogisticAreaFrom;
                txtLogisticAreaTo.Text = task.LogisticAreaTo;

                chkSelfLogistics.IsChecked = task.inHouseLogistics;
                chkOutsourceLogistics.IsChecked = task.outsourceLogistics;

                if (task.PlannedExecutionDate != null)
                    datPlannedExecution.EditValue = task.PlannedExecutionDate.Value;
                if (task.FinalExecutionDate != null)
                    datFinalExecution.EditValue = task.FinalExecutionDate.Value;
            }
        }

        private void LoadTaskStatuses()
        {
            var taskType = lookupTaskType.SelectedItem as TaskType;

            if (taskType != null)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                //BillRepo billRepo = new BillRepo();
                List<TasksStatus> tasksStatuses = new List<TasksStatus>();


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive User Tasks Statuses") != null)
                {
                    //tasksStatuses = taskRepo.GetAllTaskStatuses();
                    tasksStatuses = taskType.TaskStatusess;
                }
                else
                {
                    if (taskType.TaskStatusess.Count > 0)
                        //tasksStatuses = taskRepo.GetAllTaskStatuses().Where(x => x.isActive == true).ToList();
                        tasksStatuses = taskType.TaskStatusess.Where(x => x.isActive == true).ToList();
                }

                foreach(var status in tasksStatuses) // foreach (BillStatus status in BillStatuses)
                {
                    cmbitems.Add(new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                }
                

                if (editFlag == true && task.Status != null)
                {
                    if (tasksStatuses.Count == 0 || tasksStatuses.FirstOrDefault(x => x.Id == task.statusId) == null)
                        cmbitems.Add(new cmbitem()
                        {
                            name = task.Status.Status,
                            id = task.Status.Id,
                            bcolor = task.Status.backcolor,
                            fcolor = "#FF000000"
                        });
                }

                cmbStatus.ItemsSource = cmbitems;
            }
        }
        private void LoadTaskStatuses(TasksStatus _status)
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            tasksStatuses.Add(_status);
            Parallel.ForEach(tasksStatuses, delegate (TasksStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbStatus.ItemsSource = cmbitems;
        }

        private void LoadTaskTypes()
        {
            var _item = (TransactionItemType)cmbxModuleType.SelectedIndex;

            if (_item == TransactionItemType.UnDefined)
            {
                var compId = (lookupCompany.SelectedItem as Company).Id;
                var deptId = (lookupDepartment.SelectedItem as Department).Id;

                var taskTypes = taskRepo.GetTaskTypesCompDept(compId, deptId);
                lookupTaskType.ItemsSource = taskTypes;
              
            }
            else
            {
                var taskTypes = taskRepo.GetProcurementTaskTypes(_item);
                lookupTaskType.ItemsSource = taskTypes;

            }
        }

        private void LoadModuleTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TransactionItemType.Payments; i++)
            {
                if (i == 0)
                    cmbxModuleType.Items.Add("NA");
                else
                    cmbxModuleType.Items.Add(((ERP_BL.Enums.TransactionItemType)i).ToString());
            }
        }

        private void LoadCompanies()
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
        }

        private void LoadWareHouse()
        {
            lookupWarehouse.ItemsSource = taskRepo.GetAllWarehouse();
        }

        private void LoadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        private void LookupAssignedTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupCompany.SelectedItem as Company;
                var depts = company.departments;
                List<Department> departments = new List<Department>();
                foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x=>x.IsTaskType == true).ToList())
                {
                    if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                        departments.Add(_dept);
                }
                //Only Allowed departments to Employee will show in Dropdown
                lookupDepartment.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Invalid Company");
            }
        }

        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;

            if (company != null && department != null)
            {


                var users = taskRepo.getusersByCompanyDepartment(department.Id, company.Id);
                AllAllowedUser = taskRepo.getusersByCompanyDepartment(department.Id, company.Id);

                lookUpAssignedTo.ItemsSource = users;
                lookUpSupervisedBy.ItemsSource = users;
                lookUpAssignedBy.ItemsSource = users;
                grdCntrlUsers.ItemsSource = AllAllowedUser;


                var lotNumbers = taskRepo.GetAllLotNumberForCompDept(company.Id, department.Id);
                lookupLotNumbers.ItemsSource = lotNumbers;
            }
            loadcustomers();

            if (cmbxModuleType.SelectedIndex == 0)
                LoadTaskTypes();
        }


        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = taskRepo.GetLastTransactionId();
            if (lastPaymentId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastPaymentId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                GroupIdCalculation();
                txtSystemRef.Text = "Task-" + intGroupId;
                lblRefNo.Text = " (Task-" + intGroupId + ")";
                task.SystemId = intGroupId;
            }

            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please Enter Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if(cmbxModuleType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Module Type");
                cmbxModuleType.Focus();
                return;
            }
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Department!");
                lookupDepartment.Focus();
                return;
            }
            if(cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Status!");
                cmbStatus.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtSystemRef.Text))
            {
                DXMessageBox.Show("Please Enter System Ref!");
                txtSystemRef.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTaskRef.Text))
            {
                DXMessageBox.Show("Please Enter Task Ref!");
                txtTaskRef.Focus();
                return;
            }
            if (lookUpSupervisedBy.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Supervisor!");
                lookUpSupervisedBy.Focus();
                return;
            }
            if (lookUpAssignedBy.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Assignee!");
                lookUpAssignedBy.Focus();
                return;
            }
            if (lookUpAssignedTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Assigned To!");
                lookUpAssignedTo.Focus();
                return;
            }
            if (cmbCurrency.SelectedIndex < 0 && cmbxModuleType.SelectedIndex > 0)
            {
                DXMessageBox.Show("Please Select Currency!");
                cmbCurrency.Focus();
                return;
            }
            if (String.IsNullOrEmpty( txtSystemAmount.Text))
            {
                DXMessageBox.Show("Please Enter System Amount!");
                txtSystemAmount.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtManualAmount.Text))
            {
                DXMessageBox.Show("Please Enter Manual Amount!");
                txtManualAmount.Focus();
                return;
            }
            if (datStartDate.EditValue == null)
            {
                DXMessageBox.Show("Please Enter Start Date!");
                datStartDate.Focus();
                return;
            }
            if (datTentativeCompletionDate.EditValue == null)
            {
                DXMessageBox.Show("Please Enter Target Date!");
                datTentativeCompletionDate.Focus();
                return;
            }
            
            if (String.IsNullOrEmpty(txtDescription.Text))
            {
                DXMessageBox.Show("Please Enter Description!");
                txtDescription.Focus();
                return;
            }
            if(chkIsCompleted.IsChecked == true && datCompletionDate.EditValue == null)
            {
                DXMessageBox.Show("Please Select Completion Date!");
                datCompletionDate.Focus();
                return;
            }


            task.creationDate = datCreationDate.DateTime;
            task.transactionId = transactionId;
            if (!string.IsNullOrEmpty(txtLotNo.Text))
                task.LotNo = txtLotNo.Text;
            else
                task.LotNo = null;

            if (lookupLotNumbers.SelectedIndex > -1)
                task.lotNumberId = (lookupLotNumbers.SelectedItem as LotNumber).Id;
            else
                task.lotNumberId = null;

            task.transactionId = transactionId;

            task.transactionType = (TransactionItemType)cmbxModuleType.SelectedIndex;

            if(chkOfficeSupportRequired.IsChecked == true)
            {
                task.OfficeSupportRequired = true;
                task.LositicSupportRequired = false;
            }
            else if(chkLogisticSupportRequired.IsChecked == true)
            {
                task.OfficeSupportRequired = false;
                task.LositicSupportRequired = true;

                if (String.IsNullOrEmpty(txtLogisticAreaFrom.Text))
                {
                    DXMessageBox.Show("Please enter location!");
                    txtLogisticAreaFrom.Focus();
                    return;
                }
                if (String.IsNullOrEmpty(txtLogisticAreaTo.Text))
                {
                    DXMessageBox.Show("Please enter location!");
                    txtLogisticAreaTo.Focus();
                    return;
                }

                task.LogisticAreaFrom = txtLogisticAreaFrom.Text;
                task.LogisticAreaTo = txtLogisticAreaTo.Text;

                if (chkSelfLogistics.IsChecked == true)
                    task.inHouseLogistics = true;
                else
                    task.inHouseLogistics = false;

                if (chkOutsourceLogistics.IsChecked == true)
                    task.outsourceLogistics = true;
                else
                    task.outsourceLogistics = false;
            }

            if (datPlannedExecution.DateTime != null)
                task.PlannedExecutionDate = datPlannedExecution.DateTime;
            if (datFinalExecution.DateTime != null)
                task.FinalExecutionDate = datFinalExecution.DateTime;

            //if (transactionType != null)
            //    task.transactionType = transactionType.Value;
            //else
            //    task.transactionType = null;

            task.companyId = (lookupCompany.SelectedItem as Company).Id;
            task.deptId = (lookupDepartment.SelectedItem as Department).Id;
            task.supervisedById = (lookUpSupervisedBy.SelectedItem as User).id;
            task.assignedToId = (lookUpAssignedTo.SelectedItem as User).id;
            task.assignedById = (lookUpAssignedBy.SelectedItem as User).id;



            if(lookupTaskType.SelectedIndex > -1)
                task.taskTypeId = (lookupTaskType.SelectedItem as TaskType).Id;

            if (lookupWarehouse.SelectedIndex > -1)
                task.warehouseId = (lookupWarehouse.SelectedItem as Warehouse).Id;

            if (cmbCurrency.SelectedItem as cmbitem != null)
                task.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;

            if (lookupCustomer.SelectedItem as CustomerCompany != null)
                task.customerId = (lookupCustomer.SelectedItem as CustomerCompany).Id;

            task.SystemRef = txtSystemRef.Text;
            task.TaskRef = txtTaskRef.Text;
            task.StartDate = datStartDate.DateTime;
            task.TentativeCompletionDate = datTentativeCompletionDate.DateTime;

            if(chkIsCompleted.IsChecked == true)
            {
                task.isCompleted = true;
                task.CompletionDate = datCompletionDate.DateTime; 
            }
            else
            {
                task.isCompleted = false;
                task.CompletionDate = null;
            }

            if (btnImportBillOfEntry.IsChecked == true)
                task.ImportBillOfEntry = true;
            else
                task.ImportBillOfEntry = false;

            if (btnExportBillOfEntry.IsChecked == true)
                task.ExportBillOfEntry = true;
            else
                task.ExportBillOfEntry = false;

            task.ManualAmount = Convert.ToDouble(txtManualAmount.Text);
            AssignTransactionId();
            task.Description = txtDescription.Text;

            task.statusId = (cmbStatus.SelectedItem as cmbitem).id;

            //task.EfficiencyPoints = Convert.ToDouble(txtEfficiencyPoints.Text);

            if (SelectedAllowedUser.Count != 0)
            {
                task.AllowedUsers = new List<User>();
                foreach (var _user in SelectedAllowedUser)
                {
                    if (!task.AllowedUsers.Contains(_user))
                    {
                        task.AllowedUsers.Add(_user);
                    }
                }
            }

            if (!String.IsNullOrEmpty(txtTrackingNoIn.Text))
                task.TrackingNoIn = txtTrackingNoIn.Text;
            else
                task.TrackingNoIn = null;
            if (datETDin.EditValue != null)
                task.ETDin = datETDin.DateTime;
            else
                task.ETDin = null;
            if (datETAin.EditValue != null)
                task.ETAin = datETAin.DateTime;
            else
                task.ETAin = null;
            if (datADDin.EditValue != null)
                task.ADDin = datADDin.DateTime;
            else
                task.ADDin = null;

            if (!String.IsNullOrEmpty(txtTrackingNoOut.Text))
                task.TrackingNoOut = txtTrackingNoOut.Text;
            else
                task.TrackingNoOut = null;
            if (datETDout.EditValue != null)
                task.ETDout = datETDout.DateTime;
            else
                task.ETDout = null;
            if (datETAout.EditValue != null)
                task.ETAout = datETAout.DateTime;
            else
                task.ETAout = null;
            if (datADDout.EditValue != null)
                task.ADDout = datADDout.DateTime;
            else
                task.ADDout = null;

            //task.AllowedUsers = new List<User>();
            //foreach(User _user in grdCntrlUsers.SelectedItems)
            //{
            //    if (!task.AllowedUsers.Contains(_user))
            //        task.AllowedUsers.Add(_user);
            //}

            if (editFlag==false)
            {
                task.isApproved = false;
                task.creatorId = SYSTEM_STATIC.currentUser.id;
                taskRepo.AddTask(task, SYSTEM_STATIC.currentUser.id);
                DXMessageBox.Show("Successfully Added!");
            }
            else
            {
                task.TaskEfficiencies = new List<TaskEfficiency>();
                if (taskEfficiencies != null)
                {
                    foreach(var _eff in taskEfficiencies)
                    {
                        task.TaskEfficiencies.Add(new TaskEfficiency
                        {
                            Id = 0,
                            efficiencyPoints_Id = _eff.efficiencyPoints_Id,
                            tasksId = task.Id,
                            AchievedPoints = _eff.AchievedPoints,
                            TotalPoints = _eff.TotalPoints
                        });
                    }
                }
                taskRepo.UpdateTask(task);


                if (checkStatus.Id != task.statusId)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Task has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);

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

                    string oldStat = checkStatus.Status;
                    string newStat = task.Status.Status;
                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Task having System Ref # " + task.SystemRef + " (" + symbolCurr + ")\n "
               + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, task.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + task.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, task.Id, (int)TransactionItemType.Tasks, frmInputBox.comment);

                DXMessageBox.Show("Successfully Updated!");
            }
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void AssignTransactionId()
        {
            switch (transactionType)
            {
                case TransactionItemType.Sale_Order:
                    task.saleOrderId = transactionId;
                    break;
                case TransactionItemType.Purchase_Order:
                    task.purchaseOrderId = transactionId;
                    break;
                case TransactionItemType.Sale_Invoice:
                    task.saleInvoiceId = transactionId;
                    break;
                case TransactionItemType.Offer:
                    task.offerId = transactionId;
                    break;
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                views = UsersRepo.getViwerInfo(taskId, (int)TransactionItemType.Tasks);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (task.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new Added Task") != null)
                {
                    task.isReviewed = true;
                    task.isApproved = true;
                    task.stage = TransactionStage.Approved.ToString();
                    taskRepo.UpdateTask(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
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
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Task Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Task") != null))
                {
                    task.isReviewed = true;
                    task.needReview = false;
                    task.stage = TransactionStage.AwaitingApproval.ToString();

                    taskRepo.UpdateTask(task);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Task") != null))
                {
                    task.isReviewed = true;
                    task.needReview = true;
                    task.stage = TransactionStage.AwaitingSecondReview.ToString();

                    taskRepo.UpdateTask(task);
                }
            }


            else if (task.isApproved == true || task.isApproved == null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new Added Task") != null)
                {
                    task.isReviewed = true;
                    task.isApproved = false;
                    task.stage = TransactionStage.AwaitingApproval.ToString();
                    taskRepo.UpdateTask(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
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
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }

            loadcomments();
        }

        public ucTaskAdd(TasksStatus tasksStatus)
        {
            statusChanged = tasksStatus;
        }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo = new UsersRepo();

            if (editFlag == true && taskId > 0)
            {
                string previous_status;
                taskRepo = new TaskRepo();
                task = taskRepo.GetTask(taskId);
                if (task != null)
                {
                    if(task.isApproved == false)
                    {
                        DXMessageBox.Show("This task cannot be Closed because it is Underl Approval Stage!");
                        return;
                    }
                    previous_status = task.Status.Status;
                    var deparment = task.department;
                    var company = task.company;

                    statusChanged = null;
                    ucFrmTasksDirectClose ucFrmDirectClose = new ucFrmTasksDirectClose();
                    if (task.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = task.Status.Status;

                        var brush = new BrushConverter();
                        if(!String.IsNullOrEmpty( task.Status.backcolor))
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(task.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
                    ucFrmDirectClose.template = "NA";
                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Task has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (deparment != null && company?.Id != 0)
                            {
                                //var userss = _usersRepo.getusersByCompanyDepartment(deparment.Id, company.Id);

                                winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing User Task") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close User Tasks without Approval") != null)
                        {
                            
                                task.PendingForClosing = false;
                            task.stage = TransactionStage.Closed.ToString();
                            task.statusId = statusChanged.Id;
                            task.LastStatusChangeDate = System.DateTime.Now;
                            task.ClosingDate = System.DateTime.Now;



                            taskRepo.UpdateTask(task);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Tasks, frmInputBox.comment);


                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Task having system ref #: " + task.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            
                                procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            

                            //Load_Receipts();
                        }
                        else
                        {
                            
                                task.PendingForClosing = true;
                            task.stage = TransactionStage.AwaitingApproval.ToString();
                            task.statusId = statusChanged.Id;
                            task.LastStatusChangeDate = System.DateTime.Now;
                            task.ClosingDate = System.DateTime.Now;


                            taskRepo.UpdateTask(task);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Task having system ref #: " + task.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            
                                procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            

                        }
                        DXMessageBox.Show("Status has been changed to InActive from " + previous_status + " to " + statusChanged.Status);
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();

                    }
                }


            }
        }

        

        public void loadcomments()
        {
            try
            {
                if (task != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(task.Id, TransactionItemType.Tasks);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(task.Id, TransactionItemType.Tasks);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {

                string fname = viewInfo.User.employee.person.FName;
                string lname = viewInfo.User.employee.person.LName;
                e.Value = fname + " " + lname;

            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Bill);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Tasks"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Tasks);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }

                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
                if (taskId != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(taskId, TransactionItemType.Tasks);
                    trackingWindow.ShowDialog();
                }   
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (task.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void User Tasks") != null))
            {
                if (DXMessageBox.Show("This Task is currently in the list of Void Tasks s! Do you want to remove it from Void?", "Remove Void Tasks", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    task.isVoid = false;
                    taskRepo.UpdateTask(task);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
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
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task (Amount OC) having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void User Tasks") != null && task.isVoid != true)
            {
                if (DXMessageBox.Show("This Task is not currently in the list of Void Tasks s! Do you want to move it to Void Tasks?", "Add to Void Tasks", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    task.isVoid = true;
                    taskRepo.UpdateTask(task);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0 )
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
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
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task (Amount OC) having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Task Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveTaskAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;

            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (department != null && department.Id != 0 && company?.Id != 0)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, task.AllowedUsers, TransactionItemType.Tasks);
                inputBox.ShowDialog();

            }

            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (task != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && task.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }


                        //foreach (var user in frmInputBox.Comment.TaggedList)
                        //{
                        //    if (frmInputBox.FlagForTag == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + task.SalesReferenceNo, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + task.SalesReferenceNo, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null);

                        //}
                        //foreach (var user in frmInputBox.Comment.CCUsersList)
                        //{
                        //    if (frmInputBox.FlagForCC == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + task.SalesReferenceNo, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + task.SalesReferenceNo, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        //}
                    }

                    //procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (task.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Task first to add a comment!");
                }

            }
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (task.Id != 0)
                {
                    if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, task.AllowedUsers, comment, TransactionItemType.Tasks);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (task != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && task.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Tasks, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (task.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Task first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, task.AllowedUsers, TransactionItemType.Tasks);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && task.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(travelingRecord.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (task.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Task first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (taskId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, taskId, (int)TransactionItemType.Tasks, "Viewed details of Task");
            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GrdTaskTracking_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdTaskTracking.GetRowByListIndex(e.ListSourceRowIndex) as TaskTracking;
                switch (e.Column.FieldName)
                {
                    case "UpdatedByy":
                        if (row.UpdatedBy != null && row.UpdatedBy.employee != null && row.UpdatedBy.employee.person != null)
                        {
                            userName = row.UpdatedBy.employee.person.FName + " " + row.UpdatedBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                   
                }
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (taskId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\Tasks\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += taskId + "_" + TransactionItemType.Tasks.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Tasks);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), taskId, TransactionItemType.Tasks, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, task.Id, (int)TransactionItemType.Tasks, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Tasks);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
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


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
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

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (taskId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\Tasks\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += taskId + "_" + TransactionItemType.Tasks.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Tasks);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), taskId, TransactionItemType.Tasks, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, task.Id, (int)TransactionItemType.Tasks, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Tasks);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
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

                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
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

        private void LookUpAssignedTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var user = lookUpAssignedTo.SelectedItem as User;
            if (user != null)
            {
                if (user.employee != null)
                {
                    if (user.employee.person != null)
                    {
                        var byteImg = user.employee.person.Photo;
                        if (byteImg != null)
                        {
                            var image = GetBitmapImageFromByteArray(byteImg);
                            imgAssignedTo.ImageSource = image;
                        }
                    }
                }
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

        private void ChkIsCompleted_Checked(object sender, RoutedEventArgs e)
        {
            datCompletionDate.IsEnabled = true;
        }

        private void ChkIsCompleted_Unchecked(object sender, RoutedEventArgs e)
        {
            datCompletionDate.IsEnabled = false;
        }

        private void LookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void LookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        public void loadcustomers()
        {
            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                        var customers = customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id);
                        if (customers == null || customers.Count == 0)
                        {
                            //DevExpress.Xpf.Core.DXMessageBox.Show("Please select a different Department and Company! No customer is mapped to this department or Company.", "Select another Department or Company", MessageBoxButton.OK, MessageBoxImage.Information);
                            //return;
                        }
                        else
                        {
                            lookupCustomer.ItemsSource = customers;
                            return;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Department First!", "Select Department to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Company First!", "Select Company to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                }

        }

        private void TabUsers_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TabUsers_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveTracking_Click(object sender, RoutedEventArgs e)
        {
            
                Window win = new Window();
                ucFrmTaskTracking frmTaskTracking = new ucFrmTaskTracking();
                win.Content = frmTaskTracking;
                win.ResizeMode = ResizeMode.NoResize;
                win.Width = 400;
                win.Height = 300;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();

                TaskTracking tracking = new TaskTracking();
                tracking.Description = frmTaskTracking.txtDetails.Text;
                tracking.UpdateDateTime = DateTime.Now;
                tracking.UpdatedById = SYSTEM_STATIC.currentUser.id;

                if (task.TaskTrackings != null)
                    task.TaskTrackings.Add(tracking);
            
            taskRepo.UpdateTask(task);

            DXMessageBox.Show("Tracking Added Successfully!");

            //List<User> tagUsers = new List<User>();
            //List<User> ccUsers = new List<User>();

            //if(task.assignedTo.id == SYSTEM_STATIC.currentUser.id)
            //{
            //    tagUsers.Add(task.assignedBy);
            //    ccUsers.Add(task.supervisedBy);
            //}
            //else if (task.assignedBy.id == SYSTEM_STATIC.currentUser.id)
            //{
            //    tagUsers.Add(task.assignedTo);
            //    ccUsers.Add(task.supervisedBy);
            //}
            //else if (task.supervisedBy.id == SYSTEM_STATIC.currentUser.id)
            //{
            //    tagUsers.Add(task.assignedTo);
            //    ccUsers.Add(task.assignedBy);
            //}
            //else
            //{
            //    tagUsers.Add(task.assignedTo);
            //    ccUsers.Add(task.assignedBy);
            //    ccUsers.Add(task.supervisedBy);
            //}

            //CommentLog Comment = new CommentLog
            //{
            //    Subject = "Tracking Added",
            //    Comment = "Task having System Ref # " + task.SystemRef + " has been Updated\n" + tracking.Description,
            //    TaggedList = tagUsers,
            //    CCUsersList = ccUsers
            //};

            //if (Comment.TaggedList != null || Comment.CCUsersList != null)
            //{
            //    foreach (var user in Comment.TaggedList)
            //    {
            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, Comment.Comment, user.id, "New Comment ", null);
            //    }
            //    foreach (var user in Comment.CCUsersList)
            //    {
            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, Comment.Comment, 0, user.id, "New Comment ", null);
            //    }
            //}

            

            //procurementRepo.Add(task.Id, TransactionItemType.Tasks, Comment , SYSTEM_STATIC.currentUser.employeeId);
        }

        private void imgLeftToRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 25;
            imgLeftToRight.Width = 25;
        }

        private void imgLeftToRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 32;
            imgLeftToRight.Width = 32;
            try
            {
                var selectedItem = grdCntrlUsers.SelectedItem as User;

                if (selectedItem != null)
                {

                    AllAllowedUser.Remove(selectedItem);
                    if (!SelectedAllowedUser.Contains(selectedItem))
                        SelectedAllowedUser.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 32;
            imgRightToLeft.Height = 32;

            try
            {
                var selectedItem = grdCntrlUsersSelected.SelectedItem as User;

                if (selectedItem != null)
                {
                    if (!AllAllowedUser.Contains(selectedItem))
                        AllAllowedUser.Add(selectedItem);
                    SelectedAllowedUser.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void btnLeftMoveCustomer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 25;
            imgRightToLeft.Height = 25;
        }

        private void GrdCntrlUsers_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var row = grdCntrlUsers.GetRowByListIndex(e.ListSourceRowIndex) as User;
            
            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlUsersSelected_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var row = grdCntrlUsersSelected.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlSupervisedBy_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpSupervisedBy.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlAssignedBy_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpAssignedBy.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlAssignedTo_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpAssignedTo.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlUsersSelected_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }

        private void BtnAddEfficiencyPoints_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    ucFrmEfficiencyPoints ucFrmEfficiency = new ucFrmEfficiencyPoints(task.Id);
                    ucFrmEfficiency.ShowDialog();
                    taskEfficiencies =  ucFrmEfficiency.finalTaskEfficiencies;
                    var achievedPoints = taskEfficiencies.Sum(x => x.AchievedPoints);
                    var totalPoints = taskEfficiencies.Where(x => x.TotalPoints >= 0).Sum(x => x.TotalPoints);
                    txtEfficiencyPoints.Text = achievedPoints + "/" + totalPoints;   
                }
                catch (Exception ex)
                {   
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Task first!");
                return;
            }

        }

        private void lookupTaskType_GotFocus(object sender, RoutedEventArgs e)
        {
            if(lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company first!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Department first!");
                lookupDepartment.Focus();
                return;
            }
        }

        private void CmbxModuleType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if(cmbxModuleType.SelectedIndex > 0)
                {
                    LoadTaskTypes();
                }
            }
            catch (Exception ex)
            {

                DXMessageBox.Show("");
            }
        }

        private void ChkOfficeSupportRequired_Checked(object sender, RoutedEventArgs e)
        {
            grdLogisticSupportRequired.IsEnabled = true;
            chkLogisticSupportRequired.IsChecked = false;
        }

        private void ChkOfficeSupportRequired_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void ChkOutsourceLogistics_Checked(object sender, RoutedEventArgs e)
        {
            chkSelfLogistics.IsChecked = false;
        }

        private void ChkSelfLogistics_Checked(object sender, RoutedEventArgs e)
        {
            chkOutsourceLogistics.IsChecked = false;
        }

        private void ChkLogisticSupportRequired_Checked(object sender, RoutedEventArgs e)
        {
            chkOfficeSupportRequired.IsChecked = false;
            logisticPanel.IsEnabled = true;
        }

        private void ChkLogisticSupportRequired_Unchecked(object sender, RoutedEventArgs e)
        {
            logisticPanel.IsEnabled = false;
        }

        private void BtnSaveExecutionStyle_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                if (chkOfficeSupportRequired.IsChecked == true)
                {
                    task.OfficeSupportRequired = true;
                    task.LositicSupportRequired = false;
                }
                else if (chkLogisticSupportRequired.IsChecked == true)
                {
                    task.OfficeSupportRequired = false;
                    task.LositicSupportRequired = true;

                    if (String.IsNullOrEmpty(txtLogisticAreaFrom.Text))
                    {
                        DXMessageBox.Show("Please enter location!");
                        txtLogisticAreaFrom.Focus();
                        return;
                    }
                    if (String.IsNullOrEmpty(txtLogisticAreaTo.Text))
                    {
                        DXMessageBox.Show("Please enter location!");
                        txtLogisticAreaTo.Focus();
                        return;
                    }

                    task.LogisticAreaFrom = txtLogisticAreaFrom.Text;
                    task.LogisticAreaTo = txtLogisticAreaTo.Text;

                    if (chkSelfLogistics.IsChecked == true)
                        task.inHouseLogistics = true;
                    else
                        task.inHouseLogistics = false;

                    if (chkOutsourceLogistics.IsChecked == true)
                        task.outsourceLogistics = true;
                    else
                        task.outsourceLogistics = false;
                }

                taskRepo.UpdateTask(task);

                DXMessageBox.Show("Task Execution Details Updated!");
                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();

        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;

            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }
        public void GellAllOrdersTracking()
        {
            trackingOrder = taskRepo.GetTask(taskId);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Tasks);
            }

        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
            if (grid.SelectedItem != null)
            {

                var item = (AllOrdersView)grid.SelectedItem;

                if (item.transactionType == TransactionItemType.STL)
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
                else

                if (item.transactionType == TransactionItemType.InterBank_Transfer)
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
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id);
                        frmLoansAdvances.editFlag = true;
                        Window win = new Window();
                        win.Content = frmLoansAdvances;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
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

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmBill.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                frmBill.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
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
                        //var payments = paymentRepo.GetPaymentsByGroupId(payment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;



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

                if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
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
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;


                        if (saleReceipt == null)
                        {
                            return;
                        }

                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;
                        }

                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;


                        //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        ////ucFrmAddAccount obj = new ucFrmAddAccount();
                        //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        //updateSaleReceiptObj.enter_receipt_win.Show();

                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
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

                        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }



            }
            catch
            {

            }

            //MessageBox.Show("Mission Successful!");
        }

        private void LookupLotNumbers_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        

        private void TabChecklist_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (editFlag == true && task != null)
            {
                ucCheckListAdd ucCheckList = new ucCheckListAdd();

                var department = lookupDepartment.SelectedItem as Department;

                if (department != null)
                {
                    var products = taskRepo.getAllDepartmentProducts(department.Id);
                    ucCheckList.lookupProductsinGrid.ItemsSource = products;
                }

                ucCheckList.lookupPackingStyle.ItemsSource = taskRepo.GetAllPackingStyles();
                ucCheckList.lookupGoodReceiveNote.ItemsSource = taskRepo.GetAllGoodReceiveNotes();

                if(task.TaskComments == null)
                {
                    task.TaskComments = new List<TaskComment>();
                    ucCheckList.grdTaskComments.ItemsSource = task.TaskComments;
                }
                else
                {
                    ucCheckList.grdTaskComments.ItemsSource = task.TaskComments;
                }

                if (task.checklist == null)
                {
                    task.checklist = new Checklist();
                    ucCheckList.checklist = task.checklist;

                    ucCheckList.checklist.creatorId = SYSTEM_STATIC.currentUser.id;
                    ucCheckList.checklist.TransactionType = TransactionItemType.Purchase_Order;
                    if (task.purchaseOrder.products != null)
                        ucCheckList.checklist.products = task.purchaseOrder.products;
                    ucCheckList.checklist.creationDate = DateTime.Now;


                    ucCheckList.txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                    ucCheckList.txtModule.Text = TransactionItemType.Purchase_Order.ToString();

                    if (ucCheckList.checklist.products != null)
                        ucCheckList.grdCheckListItems.ItemsSource = ucCheckList.checklist.products;
                    ucCheckList.datCreationDate.DateTime = DateTime.Now;
                }
                else
                {
                    ucCheckList.checklist = task.checklist;

                    if (ucCheckList.checklist.creator != null)
                        ucCheckList.txtCreator.Text = ucCheckList.checklist.creator.employee.person.FName + " " + ucCheckList.checklist.creator.employee.person.LName;
                    ucCheckList.txtModule.Text = ucCheckList.checklist.TransactionType.ToString();
                    if (ucCheckList.checklist.products != null)
                        ucCheckList.grdCheckListItems.ItemsSource = ucCheckList.checklist.products;
                    ucCheckList.datCreationDate.EditValue = ucCheckList.checklist.creationDate;
                }

                if(task.purchaseOrder != null)
                {
                    ucCheckList.txtSDrefNo.Text = task.purchaseOrder.SalesReferenceNo;
                    if (task.purchaseOrder.vendors != null && task.purchaseOrder.vendors.Count > 0 && task.purchaseOrder.vendors[0].company != null)
                        ucCheckList.txtVendor.Text = task.purchaseOrder.vendors[0].company.CompanyName;

                    if (task.purchaseOrder.SaleOrder != null)
                    {
                        ucCheckList.txtCusRefNo.Text = task.purchaseOrder.SaleOrder.referenceNo;
                        if (task.purchaseOrder.SaleOrder.customerCompany != null && task.purchaseOrder.SaleOrder.customerCompany.company != null)
                            ucCheckList.txtCustomer.Text = task.purchaseOrder.SaleOrder.customerCompany.company.CompanyName;
                    }
                }

                Window window = new Window();
                window.Title = "Checklist";
                window.Content = ucCheckList;
                window.HorizontalContentAlignment = HorizontalAlignment.Center;
                int height = System.Windows.Forms.SystemInformation.WorkingArea.Height;
                height = Convert.ToInt32( System.Windows.SystemParameters.MaximizedPrimaryScreenHeight);
                window.Height = height;
                window.Width = 660;
                window.ResizeMode = ResizeMode.CanMinimize;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        private void LookupTaskType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadTaskStatuses();
        }
    }

    public class TaskEfficiencyPoints
    {
        public int Id { get; set; }

        public double TotalPoints { get; set; }
        public double AchievedPoints { get; set; }

        public List<TaskEfficiency> taskEfficiencies = new List<TaskEfficiency>();
    }
}
