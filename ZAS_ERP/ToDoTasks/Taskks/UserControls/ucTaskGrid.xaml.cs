using DevExpress.Xpf.Core;
using ERP_BL.ToDoTasks.Taskss;
using System;
using System.Collections.Generic;
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
using ZAS_ERP.Reportss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls.TaxTasks;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucTaskGrid.xaml
    /// </summary>
    public partial class ucTaskGrid : UserControl
    {
        public MainWindow myParent = null;
        TaskRepo taskRepo = new TaskRepo();

        bool byPassMode = false;
        public ucTaskGrid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btnByPassMode.Visibility = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "By Pass Permissions") != null) ? Visibility.Visible : Visibility.Collapsed;

            LoadCounters();
            LoadTaskStatuses();
            grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlTasks);
        }

        private void LoadTaskStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();
            List<TasksStatus> tasksStatuses = new List<TasksStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive User Tasks Statuses") != null)
                tasksStatuses = taskRepo.GetAllTaskStatuses();
            else
                tasksStatuses = taskRepo.GetAllTaskStatuses().Where(x => x.isActive == true).ToList();

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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void GrdCntrlTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
                var selectedRow = grdCntrlTasks.SelectedItem as Tasks;

                if (selectedRow != null)
                {
                    if (selectedRow.taskTemplate != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Tax Tasks") != null)
                        {
                            ucTaxTaskAdd taskAdd = new ucTaxTaskAdd();
                            Window win = new Window();
                            taskAdd.editFlag = true;
                            taskAdd.taskId = selectedRow.Id;
                            //taskAdd.transactionType = selectedRow.transactionType;
                            //taskAdd.transactionId = selectedRow.transactionId;
                            win.Content = taskAdd;
                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                        }
                        else
                        {
                            DXMessageBox.Show("Permission Required to View Tax Tasks!");
                        }
                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View User Task") != null)
                        {

                            ucTaskAdd taskAdd = new ucTaskAdd();
                            Window win = new Window();
                            taskAdd.editFlag = true;
                            taskAdd.taskId = selectedRow.Id;
                            taskAdd.transactionType = selectedRow.transactionType;
                            taskAdd.transactionId = selectedRow.transactionId;
                            win.Content = taskAdd;
                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            win.WindowState = WindowState.Maximized;
                            win.Show();
                        }
                        else
                        {
                            DXMessageBox.Show("Permission Required to View User Tasks!");
                        }
                    }
                }
            
        }

        private void GrdCntrlTasks_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdCntrlTasks.GetRowByListIndex(e.ListSourceRowIndex) as Tasks;
                switch (e.Column.FieldName)
                {
                    case "LotNumberr":
                        string LotNo = "";
                        if (row.lotNumber != null)
                            LotNo = row.lotNumber.LotNo;
                        else
                            LotNo = row.LotNo;
                        e.Value = LotNo;
                        break;
                    case "SDrefNo":
                        string SDref = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:

                                if (row.saleInvoice != null && row.saleInvoice.SaleOrder != null)
                                    SDref = row.saleInvoice.SaleOrder.SalesReferenceNo;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Order:

                                if (row.saleOrder != null)
                                    SDref = row.saleOrder.SalesReferenceNo;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:

                                if (row.purchaseOrder != null && row.purchaseOrder.SaleOrder != null)
                                    SDref = row.purchaseOrder.SaleOrder.SalesReferenceNo;
                                break;

                            default:
                                break;
                        }
                        e.Value = SDref;
                        break;

                    case "SalesReference":
                        string SalesReference = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:

                                if (row.saleInvoice != null && row.saleInvoice.SaleOrder != null)
                                    SalesReference = row.saleInvoice.SaleOrder.referenceNo;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Order:

                                if (row.saleOrder != null)
                                    SalesReference = row.saleOrder.referenceNo;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:

                                if (row.purchaseOrder != null && row.purchaseOrder.SaleOrder != null)
                                    SalesReference = row.purchaseOrder.SaleOrder.referenceNo;
                                break;

                            default:
                                break;
                        }
                        e.Value = SalesReference;
                        break;

                    case "SOamount":
                        Decimal SOamount = 0;
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:

                                if (row.saleInvoice != null && row.saleInvoice.SaleOrder != null)
                                    SOamount = Convert.ToDecimal(row.saleInvoice.SaleOrder.totalCFRValue);
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Order:

                                if (row.saleOrder != null)
                                    SOamount = Convert.ToDecimal(row.saleOrder.totalCFRValue);
                                break;

                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:

                                if (row.purchaseOrder != null && row.purchaseOrder.SaleOrder != null)
                                    SOamount = Convert.ToDecimal(row.purchaseOrder.SaleOrder.totalCFRValue);
                                break;

                            default:
                                break;
                        }
                        e.Value = SOamount;
                        break;

                    case "SOcurrency":
                        string SOcurrency = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:

                                if (row.saleInvoice != null && row.saleInvoice.SaleOrder != null && row.saleInvoice.SaleOrder.currency != null)
                                    SOcurrency = row.saleInvoice.SaleOrder.currency.CurrencyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Order:

                                if (row.saleOrder != null && row.saleOrder.currency != null)
                                    SOcurrency = row.saleOrder.currency.CurrencyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:

                                if (row.purchaseOrder != null && row.purchaseOrder.SaleOrder != null && row.purchaseOrder.SaleOrder.currency != null)
                                    SOcurrency = row.purchaseOrder.SaleOrder.currency.CurrencyName;
                                break;

                            default:
                                break;
                        }
                        e.Value = SOcurrency;
                        break;

                    case "VendorName":
                        string vendorName = "";
                        switch (row.transactionType)
                        {
                            case null:
                                if (row.vendor != null && row.vendor.company != null)
                                    vendorName = row.vendor.company.CompanyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.UnDefined:
                                if (row.vendor != null && row.vendor.company != null)
                                    vendorName = row.vendor.company.CompanyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:

                                if (row.saleInvoice != null && row.saleInvoice.vendors != null && row.saleInvoice.vendors.Count > 0)
                                    vendorName = row.saleInvoice.vendors[0].company.CompanyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Sale_Order:

                                if (row.saleOrder != null && row.saleOrder.vendors != null && row.saleOrder.vendors.Count > 0)
                                    vendorName = row.saleOrder.vendors[0].company.CompanyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:

                                if (row.purchaseOrder != null && row.purchaseOrder.vendors != null && row.purchaseOrder.vendors.Count > 0)
                                    vendorName = row.purchaseOrder.vendors[0].company.CompanyName;
                                break;

                            case ERP_BL.Enums.TransactionItemType.Offer:

                                if (row.offer != null && row.offer.vendors != null && row.offer.vendors.Count > 0)
                                    vendorName = row.offer.vendors[0].company.CompanyName;
                                break;

                            default:
                                break;
                        }
                        e.Value = vendorName;
                        break;

                    case "ModuleType":
                        string type = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.UnDefined:
                                type = "NA";
                                break;
                            default:
                                type = row.transactionType.ToString();
                                break;
                        }
                        e.Value = type;
                        break;
                    case "Customer":
                        string customer= "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Order:
                                if (row.saleOrder != null && row.saleOrder.customerCompany != null)
                                    customer = row.saleOrder.customerCompany.company.CompanyName;
                                else if (row.CustomerCompany != null)
                                    customer = row.CustomerCompany.company.CompanyName;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:
                                if (row.saleInvoice != null && row.saleInvoice.customerCompany != null)
                                    customer = row.saleInvoice.customerCompany.company.CompanyName;
                                else if (row.CustomerCompany != null)
                                    customer = row.CustomerCompany.company.CompanyName;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:
                                if (row.purchaseOrder != null && row.purchaseOrder.customerCompany != null)
                                    customer = row.purchaseOrder.customerCompany.company.CompanyName;
                                else if (row.CustomerCompany != null)
                                    customer = row.CustomerCompany.company.CompanyName;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Offer:
                                if(row.offer != null && row.offer.customerCompany!=null)
                                    customer = row.offer.customerCompany.company.CompanyName;
                                else if (row.CustomerCompany != null)
                                    customer = row.CustomerCompany.company.CompanyName;
                                break;
                            case ERP_BL.Enums.TransactionItemType.UnDefined:
                                if(row.CustomerCompany != null)
                                    customer = row.CustomerCompany.company.CompanyName;
                                break;
                        }
                        e.Value = customer;
                        break;

                    case "SOReferenceNo":
                        string refNo = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Order:
                                if (row.saleOrder != null)
                                    refNo = row.saleOrder.referenceNo;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:
                                if (row.saleInvoice != null && row.saleInvoice.SaleOrder != null)
                                    refNo = row.saleInvoice.SaleOrder.referenceNo;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:
                                if (row.purchaseOrder != null && row.purchaseOrder.SaleOrder != null)
                                    refNo = row.purchaseOrder.SaleOrder.referenceNo;
                                break;
                        }
                        e.Value = refNo;
                        break;

                    case "POReferenceNo":
                        string POrefNo = "";
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:
                                if (row.purchaseOrder != null )
                                    POrefNo = row.purchaseOrder.POReferenceNo;
                                break;
                        }
                        e.Value = POrefNo;
                        break;

                    case "SystemAmount":
                        double systemAmount = 0;
                        switch (row.transactionType)
                        {
                            case ERP_BL.Enums.TransactionItemType.Sale_Order:
                                systemAmount = row.saleOrder.totalCFRValue;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Sale_Invoice:
                                systemAmount = row.saleInvoice.totalInvoiceAmount;
                                break;
                            case ERP_BL.Enums.TransactionItemType.Purchase_Order:
                                systemAmount = row.purchaseOrder.totalCFRValue;
                                break;
                        }
                        e.Value = systemAmount;
                        break;
                    case "SupervisedBy":
                        if (row.supervisedBy != null && row.supervisedBy.employee != null && row.supervisedBy.employee.person != null)
                        {
                            userName = row.supervisedBy.employee.person.FName + " " + row.supervisedBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "Creator":
                        if (row.creator != null && row.creator.employee != null && row.creator.employee.person != null)
                        {
                            userName = row.creator.employee.person.FName + " " + row.creator.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "AssignedBy":
                        if (row.assignedBy != null && row.assignedBy.employee != null && row.assignedBy.employee.person != null)
                        {
                            userName = row.assignedBy.employee.person.FName + " " + row.assignedBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "AssignedTo":
                        if (row.assignedTo != null && row.assignedTo.employee != null && row.assignedTo.employee.person != null)
                        {
                            userName = row.assignedTo.employee.person.FName + " " + row.assignedTo.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "AgingDays":
                        int timeSpan = 0;
                        if (row.TentativeCompletionDate != null)
                        {
                            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            var date = row.TentativeCompletionDate.Value;
                            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            timeSpan = currDate.Subtract(targetDate).Days;
                        }
                        e.Value = timeSpan;
                        break;
                    case "FillerName":
                        if (row.filerEmployee != null)
                        {
                            userName = row.filerEmployee.person.FName + " " + row.filerEmployee.person.LName;
                        }
                        else
                        {
                            userName = row.FilerName;
                        }
                        e.Value = userName;
                        break;
                }
            }
        }

        private void TableViewTasks_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void Btnclose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

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

        private void TableViewTasks_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                var task = grdCntrlTasks.GetFocusedRow() as Tasks;
                if(task != null)
                {
                    if(task.creator != null)
                    {
                        if (task.creator.employee != null)
                        {
                            if (task.creator.employee.person != null)
                            {
                                txtCreator.Text = task.creator.employee.person.FName + " " + task.creator.employee.person.LName;
                                var byteImg = task.creator.employee.person.Photo;
                                if (byteImg != null)
                                {
                                    var image = GetBitmapImageFromByteArray(byteImg);
                                    imgCreator.ImageSource = image;
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        txtCreator.Text = null;
                        imgCreator.ImageSource = null;
                    }

                    if (task.supervisedBy != null)
                    {
                        if (task.supervisedBy.employee != null)
                        {
                            if (task.supervisedBy.employee.person != null)
                            {
                                txtSupervisedBy.Text = task.supervisedBy.employee.person.FName + " " + task.supervisedBy.employee.person.LName;
                                var byteImg = task.supervisedBy.employee.person.Photo;
                                if (byteImg != null)
                                {
                                    var image = GetBitmapImageFromByteArray(byteImg);
                                    imgSupervisedBy.ImageSource = image;
                                }
                            }
                        }
                    }
                    else
                    {
                        txtSupervisedBy.Text = null;
                        imgSupervisedBy.ImageSource = null;
                    }

                    if (task.assignedTo != null)
                    {
                        if (task.assignedTo.employee != null)
                        {
                            if (task.assignedTo.employee.person != null)
                            {
                                txtAssignedTo.Text = task.assignedTo.employee.person.FName + " " + task.assignedTo.employee.person.LName;
                                var byteImg = task.assignedTo.employee.person.Photo;
                                if (byteImg != null)
                                {
                                    var image = GetBitmapImageFromByteArray(byteImg);
                                    imgAssignedTo.ImageSource = image;
                                }
                            }
                        }
                        else
                        {
                            txtAssignedTo.Text = "Nil";
                        }
                    }
                    else
                    {
                        txtAssignedTo.Text = null;
                        imgAssignedTo.ImageSource = null;
                    }

                    if (task.assignedBy != null)
                    {
                        if (task.assignedBy.employee != null)
                        {
                            if (task.assignedBy.employee.person != null)
                            {
                                txtAssignedBy.Text = task.assignedBy.employee.person.FName + " " + task.assignedBy.employee.person.LName;
                                var byteImg = task.assignedBy.employee.person.Photo;
                                if (byteImg != null)
                                {
                                    var image = GetBitmapImageFromByteArray(byteImg);
                                    imgAssignedBy.ImageSource = image;
                                }
                            }
                        }
                        else
                        {
                            txtAssignedBy.Text = "Nil";
                        }
                    }
                    else
                    {
                        txtAssignedBy.Text = null;
                        imgAssignedBy.ImageSource = null;
                    }

                    //Select Status
                    var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                    if (task.Status != null)
                    {
                        int index = 0;
                        foreach (var _status in statusList)
                        {

                            if (_status.id == task.statusId)
                            {
                                cmbStatus.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }

                    txtDescription.Text = task.Description;

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MbtnAddTaxTask_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tax Tasks") != null)
            {
                ucTaxTaskAdd taskAdd = new ucTaxTaskAdd();
                Window win = new Window();
                win.Title = "Tasks";
                win.Content = taskAdd;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Tax Tasks!");
            }

        }

        private void MbtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Task Not referring to Any Module") != null)
            {
                //var selectedRow = grdCntrlTasks.SelectedItem as Tasks;
                //if (selectedRow != null)
                //{
                    ucTaskAdd taskAdd = new ucTaskAdd();
                    taskAdd.transactionId = 0;
                    taskAdd.transactionType = ERP_BL.Enums.TransactionItemType.UnDefined;
                    Window win = new Window();
                    win.Title = "Tasks";
                    win.Content = taskAdd;
                    win.Show();
                //}
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Non-Linked User Tasks!");
            }
            
        }

        private void MbtnUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = grdCntrlTasks.SelectedItem as Tasks;

            if (selectedRow != null)
            {
                if (selectedRow.taskTemplate != null)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Tax Tasks") != null)
                    {
                        ucTaxTaskAdd taskAdd = new ucTaxTaskAdd();
                        Window win = new Window();
                        taskAdd.editFlag = true;
                        taskAdd.taskId = selectedRow.Id;
                        //taskAdd.transactionType = selectedRow.transactionType;
                        //taskAdd.transactionId = selectedRow.transactionId;
                        win.Content = taskAdd;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission Required to View Tax Tasks!");
                    }
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View User Task") != null)
                    {

                        ucTaskAdd taskAdd = new ucTaskAdd();
                        Window win = new Window();
                        taskAdd.editFlag = true;
                        taskAdd.taskId = selectedRow.Id;
                        taskAdd.transactionType = selectedRow.transactionType;
                        taskAdd.transactionId = selectedRow.transactionId;
                        win.Content = taskAdd;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.WindowState = WindowState.Maximized;
                        win.Show();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission Required to View User Tasks!");
                    }
                }
            }

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            LoadCounters();
            LoadTaskStatuses();
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasks();
                    lblHeading.Content = "Open Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Open Tasks";
            }
        }

        private void BtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlTasks);
        }

        private void BtnOpenTasks_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasks();
                    lblHeading.Content = "Open Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Open Tasks";
            }
           
        }

        private void BtnAssignedToMe_Click(object sender, RoutedEventArgs e)
        {
            grdCntrlTasks.ItemsSource = taskRepo.GetAllAssignedToMeTasks(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Assigned To Me Tasks";
        }

        private void BtnTasksRegister_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Task Register";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllTasks();
                    lblHeading.Content = "Task Register";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Task Register";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Task Register";
            }
            
        }

        private void BtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllVoidTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Void Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllVoidTasks();
                    lblHeading.Content = "Void Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllVoidTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Void Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllVoidTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Void Tasks";
            }
           
        }

        private void LoadCounters()
        {
            if (byPassMode == true)
            {
                if(SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name== "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    mbtnOpenTasksCounter.Header = taskRepo.GetAllOpenTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnClosedTasksCounter.Header = taskRepo.GetAllClosedTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnUnderApprovalCounter.Header = taskRepo.GetAllPendingForApprovalTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnTaskRegisterCounter.Header = taskRepo.GetAllTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnVoidCounter.Header = taskRepo.GetAllVoidTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnAssignedToMeCounter.Header = taskRepo.GetAllAssignedToMeTasksCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnPendingForClosedTasksCounter.Header = taskRepo.GetAllPendingForClosingTasksDepartmentalCount(SYSTEM_STATIC.currentUser.id).ToString();
                }
                else if(SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    mbtnOpenTasksCounter.Header = taskRepo.GetAllOpenTasksCount().ToString();
                    mbtnClosedTasksCounter.Header = taskRepo.GetAllClosedTasksCount().ToString();
                    mbtnUnderApprovalCounter.Header = taskRepo.GetAllPendingForApprovalTasksCount().ToString();
                    mbtnTaskRegisterCounter.Header = taskRepo.GetAllTasksCount().ToString();
                    mbtnVoidCounter.Header = taskRepo.GetAllVoidTasksCount().ToString();
                    mbtnAssignedToMeCounter.Header = taskRepo.GetAllAssignedToMeTasksCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnPendingForClosedTasksCounter.Header = taskRepo.GetAllPendingForClosingTasksCount().ToString();
                }
                else
                {
                    mbtnOpenTasksCounter.Header = taskRepo.GetAllOpenTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnClosedTasksCounter.Header = taskRepo.GetAllClosedTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnUnderApprovalCounter.Header = taskRepo.GetAllPendingForApprovalTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnTaskRegisterCounter.Header = taskRepo.GetAllTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnVoidCounter.Header = taskRepo.GetAllVoidTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnAssignedToMeCounter.Header = taskRepo.GetAllAssignedToMeTasksCount(SYSTEM_STATIC.currentUser.id).ToString();
                    mbtnPendingForClosedTasksCounter.Header = taskRepo.GetAllPendingForClosingTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                }
            }
            else
            {
                mbtnOpenTasksCounter.Header = taskRepo.GetAllOpenTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnClosedTasksCounter.Header = taskRepo.GetAllClosedTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnUnderApprovalCounter.Header = taskRepo.GetAllPendingForApprovalTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnTaskRegisterCounter.Header = taskRepo.GetAllTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnVoidCounter.Header = taskRepo.GetAllVoidTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnAssignedToMeCounter.Header = taskRepo.GetAllAssignedToMeTasksCount(SYSTEM_STATIC.currentUser.id).ToString();
                mbtnPendingForClosedTasksCounter.Header = taskRepo.GetAllPendingForClosingTasksAllowedUsersCount(SYSTEM_STATIC.currentUser.id).ToString();
            }
        }

        private void MbtnPendingForClosing_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForClosingTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Pending For Closing Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForClosingTasks();
                    lblHeading.Content = "Pending For Closing Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForClosingTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Pending For Closing Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForClosingTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Pending For Closing Tasks";
            }
        }

        private void BtnClosedTasks_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllClosedTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Closed Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllClosedTasks();
                    lblHeading.Content = "Closed Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllClosedTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Closed Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllClosedTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Closed Tasks";
            }
           
        }

        private void BtnByPassMode_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                byPassMode = false;
                btnByPassMode.Background = Brushes.LightGray;
                btnByPassMode.Content = "By Pass Mode is Off";

            }
            else
            {
                byPassMode = true;
                btnByPassMode.Background = Brushes.DeepSkyBlue;
                btnByPassMode.Content = "By Pass Mode is ON";
            }

            taskRepo = new TaskRepo();
            LoadCounters();
            LoadTaskStatuses();
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasks();
                    lblHeading.Content = "Open Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Open Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllOpenTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Open Tasks";
            }
        }

        private void BtnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            //tabCntrlTaskDetails.Visibility = Visibility.Visible;
            //btnHideDetails.Visibility = Visibility.Visible;
            //btnShowDetails.Visibility = Visibility.Collapsed;
        }

        private void BtnHideDetails_Click(object sender, RoutedEventArgs e)
        {
            //tabCntrlTaskDetails.Visibility = Visibility.Collapsed;
            //btnHideDetails.Visibility = Visibility.Collapsed;
            //btnShowDetails.Visibility = Visibility.Visible;
        }

        private void MbtnPendingForApproval_Click(object sender, RoutedEventArgs e)
        {
            if (byPassMode == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for Allowed Departments (Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForApprovalTasksDepartmental(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Pending For Approval Tasks";
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Tasks for All Departments (Member/Non Member)") != null)
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForApprovalTasks();
                    lblHeading.Content = "Pending For Approval Tasks";
                }
                else
                {
                    grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForApprovalTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "Pending For Approval Tasks";
                }
            }
            else
            {
                grdCntrlTasks.ItemsSource = taskRepo.GetAllPendingForApprovalTasksAllowedUsers(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Pending For Approval Tasks";
            }
        }

        private void mbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Task Report") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("Tasks");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdCntrlTasks, reportName, reportType, reportGroup, lblHeading.Content.ToString(), report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Task Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
