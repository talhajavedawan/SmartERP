using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget
{
    /// <summary>
    /// Interaction logic for frmBudgetAdd.xaml
    /// </summary>
    public partial class frmBudgetAdd : DXWindow
    {
        Company company = new Company();
        List<Company> currentUserCompanies;
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        Department department = new Department();
        BudgetCostSheet sheet = new BudgetCostSheet();
        List<BudgetCostField> budgetFieldItems = new List<BudgetCostField>();
        List<ViewInfo> views = new List<ViewInfo>();
        UsersRepo UsersRepo = new UsersRepo();
        public int editOrder;
        public BudgetCostSheetStatus checkStatus = new BudgetCostSheetStatus();
        BudgetCostSheetStatus oldStatus = new BudgetCostSheetStatus();

        public int budgetid;

        ProcurementRepo procurementRepo = new ProcurementRepo();

        List<SaleOrder> saleOrders = new List<SaleOrder>();
        List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
        public frmBudgetAdd()
        {
            budgetFieldItems = new List<BudgetCostField>();
            InitializeComponent();
        }
        public frmBudgetAdd(int _orderId)
        {
            budgetFieldItems = new List<BudgetCostField>();
            budgetid = _orderId;
            editOrder = 1;
            InitializeComponent();
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                loaddepartments();
            }
        }
        public void loaddepartments()
        {
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsProcurementType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    lookupDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
            }
            currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupCompany.ItemsSource = currentUserCompanies;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompanies();
            loadBudgetStatus();
            loadCurrencies();
            grdBudgetCostItems.ItemsSource = budgetFieldItems;
            loadFieldHeades();
            LoadBudget();
        }
        public void loadonBudgetdata()
        {
            try
            {


                if (sheet.isVoid == true)
                {
                    //grdVoid.Visibility = Visibility.Visible;
                    //txtVoid.RenderTransform = new RotateTransform(-45);
                }
                else if (sheet.isReApproved == false)
                {
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (sheet.isApproved == true && sheet.stage == "Closed")
                {
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (sheet.isApproved == true && sheet.budgetCostSheetStatus.isActive == false && sheet.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (sheet.isApproved == true && sheet.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (sheet.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (sheet.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (sheet.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                datCreationDate.EditValue = sheet.CreationDate;

                if (sheet.companyId != null || sheet.company != null)
                {
                    company = sheet.company;
                    lookupCompany.Text = sheet.company.CompanyName;
                }
                else
                {
                    lookupCompany.Text = "Select Company";

                }
                if (sheet.deptId != 0 || sheet.Department != null)
                {
                    lookupDepartment.Text = sheet.Department.DeptName;
                    department = sheet.Department;
                }
                else
                {
                    lookupDepartment.Text = "Select Department";
                }
                if (sheet.BudgetMonth != null)
                {
                    budgetMonth.EditValue = sheet.BudgetMonth;
                }
                if (sheet.empId != 0 || sheet.Employee != null)
                {
                    try
                    {
                        var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                        cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == sheet.empId))];

                    }
                    catch
                    {

                    }
                }
                var BudgetSource = (List<cmbitem>)cmbBudgetStatus.Items.SourceCollection;

                if (sheet.budgetCostSheetStatus.isActive == false)
                {
                    try
                    {
                        cmbBudgetStatus.SelectedItem = cmbBudgetStatus.Items[cmbBudgetStatus.Items.IndexOf(BudgetSource.Find(x => x.name == sheet.budgetCostSheetStatus.Status))];
                    }
                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Sale Order status! " + ex.ToString());
                    }
                }
                else
                {
                    cmbBudgetStatus.SelectedItem = cmbBudgetStatus.Items[cmbBudgetStatus.Items.IndexOf(BudgetSource.Find(x => x.name == sheet.budgetCostSheetStatus.Status))];
                }
                if (sheet.currencyId != 0 && sheet.Currency != null)
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == sheet.currencyId))];
                }
                else
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == sheet.company.CurrencyId))];
                }
                datFrom.EditValue = sheet.From;
                datTo.EditValue = sheet.To;
                grdBudgetCostItems.ItemsSource = sheet.budgetCostFields;
                txtBudgetTotalIncome.Text = sheet.TotalIncome.ToString();
                txtBudgetTotalCGS.Text = sheet.TotalCGS.ToString();
                txtBudgetTotalExpense.Text = sheet.TotalExpense.ToString();
                txtBudgetTotalGrossProfit.Text = sheet.TotalGrossProfit.ToString();
                txtBudgetTotalNetProfit.Text = sheet.TotalNetProfit.ToString();
                txtRefNo.Text = sheet.refNo;



                double totalSystemIncome = 0, totalSystemExpense = 0, totalSystemCGS = 0, totalSystemGrossProfit = 0, totalSystemNetProfit = 0;

                foreach (var saleOrder in saleOrders)
                {
                    foreach (var invoice in saleOrder.SaleInvoices)
                    {
                        if (invoice.BudgetSystemCostFields.Count != 0)
                        {
                            var incomeFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isIncome == true).ToList();
                            var expenseFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isExpense == true).ToList();
                            var cgsFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isCGS == true).ToList();
                            totalSystemIncome += incomeFields.Sum(x => x.addedSystemCost);
                            totalSystemExpense += expenseFields.Sum(x => x.addedSystemCost);
                            totalSystemCGS += cgsFields.Sum(x => x.addedSystemCost);
                        }
                        foreach(var receipt in invoice.salesReceipts)
                        {
                            var incomeFields = receipt.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isIncome == true).ToList();
                            var expenseFields = receipt.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isExpense == true).ToList();
                            var cgsFields = receipt.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isCGS == true).ToList();
                            totalSystemIncome += incomeFields.Sum(x => x.addedSystemCost);
                            totalSystemExpense += expenseFields.Sum(x => x.addedSystemCost);
                            totalSystemCGS += cgsFields.Sum(x => x.addedSystemCost);
                        }
                    }
                    foreach (var bill in saleOrder.Bills)
                    {
                        if (bill.BudgetSystemCostFields.Count != 0)
                        {
                            var incomeFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isIncome == true).ToList();
                            var expenseFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isExpense == true).ToList();
                            var cgsFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isCGS == true).ToList();
                            totalSystemIncome += incomeFields.Sum(x => x.addedSystemCost);
                            totalSystemExpense += expenseFields.Sum(x => x.addedSystemCost);
                            totalSystemCGS += cgsFields.Sum(x => x.addedSystemCost);
                        }
                    }

                }
                foreach (var purchaseOrder in purchaseOrders)
                {

                    foreach (var bill in purchaseOrder.Bills)
                    {
                        if (bill.BudgetSystemCostFields.Count != 0)
                        {
                            var incomeFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isIncome == true).ToList();
                            var expenseFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isExpense == true).ToList();
                            var cgsFields = bill.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isCGS == true).ToList();
                            totalSystemIncome += incomeFields.Sum(x => x.addedSystemCost);
                            totalSystemExpense += expenseFields.Sum(x => x.addedSystemCost);
                            totalSystemCGS += cgsFields.Sum(x => x.addedSystemCost);
                        }
                    }
                    foreach (var invoice in purchaseOrder.PurchaseInvoices)
                    {
                        if (invoice.BudgetSystemCostFields.Count != 0)
                        {
                            var incomeFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isIncome == true).ToList();
                            var expenseFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isExpense == true).ToList();
                            var cgsFields = invoice.BudgetSystemCostFields.Where(x => x.BudgetSheettHead.isCGS == true).ToList();
                            totalSystemIncome += incomeFields.Sum(x => x.addedSystemCost);
                            totalSystemExpense += expenseFields.Sum(x => x.addedSystemCost);
                            totalSystemCGS += cgsFields.Sum(x => x.addedSystemCost);
                        }
                    }

                }
                totalSystemGrossProfit = totalSystemIncome - totalSystemCGS;
                totalSystemNetProfit = totalSystemGrossProfit - totalSystemExpense;
                txtSystemTotalIncome.Text = totalSystemIncome.ToString();
                txtSystemTotalExpense.Text = totalSystemExpense.ToString();
                txtSystemTotalCGS.Text = totalSystemCGS.ToString();
                txtSystemTotalGrossProfit.Text = totalSystemGrossProfit.ToString();
                txtSystemTotalNetProfit.Text = totalSystemNetProfit.ToString();

            }
            catch (Exception)
            {

            }
        }
        public void LoadBudget()
        {
            if (editOrder == 1 && budgetid != 0)
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget") == null)
                {
                    sheet = repo.get(budgetid);
                    saleOrders = repo.getBudgetSaleOrders(budgetid);
                    purchaseOrders = repo.getBudgetPurchaseOrders(budgetid);
                    loadonBudgetdata();
                    btnSave.IsEnabled = false;

                    if (sheet.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Budget") != null)
                    {
                        btnSave.IsEnabled = true;
                    }


                }

                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget") != null)
                {
                    sheet = repo.get(budgetid);
                    saleOrders = repo.getBudgetSaleOrders(budgetid);
                    purchaseOrders = repo.getBudgetPurchaseOrders(budgetid);

                    loadonBudgetdata();
                    btnSave.IsEnabled = true;

                }

                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget") != null)
                {
                    sheet = repo.get(budgetid);
                    saleOrders = repo.getBudgetSaleOrders(budgetid);
                    purchaseOrders = repo.getBudgetPurchaseOrders(budgetid);

                    loadonBudgetdata();

                    btnSave.IsEnabled = true;
                    if (sheet != null)
                    {
                        if (sheet.isVoid == true)
                        {
                            //grdVoid.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Budget!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }

            }
            else
            {
            }
        }
        public void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }
        public void loadFieldHeades()
        {
            List<BudgetCostField> fields = new List<BudgetCostField>();

            var heads= repo.getallCostSheetField();
            foreach(var head in heads)
            {
                BudgetCostField field = new BudgetCostField();
                field.Head_Id = head.Id;
                field.BudgetSheettHead = head;
                fields.Add(field);
            }
            grdBudgetCostItems.ItemsSource = fields;





        }
        public void loadBudgetStatus()
        {
            List<BudgetCostSheetStatus> budgetCostStatuses = new List<BudgetCostSheetStatus>();

            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Budget") != null)
            {
                budgetCostStatuses = repo.getAllBudgetCostStatus();
            }
            else
                budgetCostStatuses = repo.getAllActiveBudgetCostStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (BudgetCostSheetStatus status in budgetCostStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                //});
            }
            cmbBudgetStatus.ItemsSource = cmbitems;
        }
        private void cmbBudgetStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                var child = saleOrderRepo.getParentDepart(department.Id);
                if (child != null)
                {
                    lookupDepartment.SelectedItem = null;
                    DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                    lookupDepartment.Focus();
                    return;
                }
                else
                {
                    loademployees();
                }
            }
        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            employees = department.employees;
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbEmployee.ItemsSource = cmbitems;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            try
            {
                if (lookupCompany.SelectedIndex == -1)
                {
                    lookupCompany.Focus();
                    MessageBox.Show("Please Select a company against Budget", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else if (lookupDepartment.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a Department against Budget", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    MessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    MessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbBudgetStatus.SelectedIndex == -1 && sheet.PendingForClosing != true)
                {
                    MessageBox.Show("Please Select Current Status of Budget to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbBudgetStatus.Focus();
                    return;
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Budget Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtRefNo.Text))
                {
                    MessageBox.Show("Please Input Ref No", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtRefNo.Focus();
                    return;
                }
                sheet.CreationDate = (DateTime)datCreationDate.EditValue;
                if (company != null)
                {
                    sheet.companyId = company.Id;
                }
                if (department != null)
                {
                    sheet.deptId = department.Id;
                }
                if (cmbEmployee.SelectedIndex>-1 )
                {
                    sheet.empId = (cmbEmployee.SelectedItem as cmbitem).id;
                }
                if (cmbCurrency.SelectedIndex > -1)
                {
                    sheet.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                }
                if ((cmbBudgetStatus.SelectedItem as cmbitem) != null)
                {
                    var status = repo.getstatus((cmbBudgetStatus.SelectedItem as cmbitem).id);
                    sheet.budgetCostSheetStatus = status;
                }
                sheet.To = (DateTime)datTo.EditValue;
                sheet.From = (DateTime)datFrom.EditValue;
                sheet.budgetCostFields = getBudgetFields();
                sheet.TotalIncome = Math.Round(Convert.ToDouble(txtBudgetTotalIncome.Text), 2);
                sheet.TotalCGS = Math.Round(Convert.ToDouble(txtBudgetTotalCGS.Text), 2);
                sheet.TotalExpense = Math.Round(Convert.ToDouble(txtBudgetTotalExpense.Text), 2);
                sheet.TotalGrossProfit = Math.Round(Convert.ToDouble(txtBudgetTotalExpense.Text), 2);
                sheet.TotalNetProfit = Math.Round(Convert.ToDouble(txtBudgetTotalNetProfit.Text), 2);
                sheet.refNo = txtRefNo.Text;
                if(budgetMonth.EditValue!=null)
                {
                    sheet.BudgetMonth = (DateTime)budgetMonth.EditValue;
                }
                var myWindow = Window.GetWindow(this);


                if (editOrder == 1 && sheet.Id != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Budget") != null))
                {
                    
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null && sheet.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            sheet.stage = TransactionStage.Approved.ToString();

                            sheet.isApproved = true;
                            sheet.ApprovedDate = System.DateTime.Now;
                        }
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without ReApproval") != null && sheet.isReApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Budget is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            sheet.stage = TransactionStage.Approved.ToString();

                            sheet.isReApproved = true;
                            sheet.ReApprovalDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != sheet.budgetCostSheetStatus.Id)
                        {
                            sheet.LastStatusChangeDate = System.DateTime.Now;
                            if (sheet.budgetCostSheetStatus.isActive != true)
                            {
                                sheet.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }

                    repo.update(sheet);

                    if (checkStatus.Id != sheet.budgetCostSheetStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Budget has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), sheet.Id, TransactionItemType.Budget);

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
                        string newStat = sheet.budgetCostSheetStatus.Status;
                        string symbolCurr = "";
                        if (sheet.Currency != null)
                        {
                            symbolCurr = sheet.Currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog();
                      
                       
                            comment.Comment = "Status of Budget having Total Budget Gross Profit (OC): " + sheet.TotalNetProfit.ToString() + " (" + symbolCurr + ")\n "
                        + "Budget Net Profit(OC): " + txtBudgetTotalNetProfit.Text + " (" + symbolCurr + ")"
                        //+ "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                        //+ "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                        + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                        
                        procurementRepo.Add(sheet.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }



                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                        UsersRepo.Add(TransactionInfo.Status_Changed, sheet.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + sheet.budgetCostSheetStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, sheet.Id, 3, frmInputBox.comment);

                    
                }
                else if (editOrder != 1)
                {
                    
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget") != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null /*&& saleOrder.isApproved == false*/)
                        {
                            //if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                sheet.stage = TransactionStage.Approved.ToString();

                                sheet.isApproved = true;
                                sheet.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        else
                        {
                            sheet.stage = TransactionStage.AwaitingFirstReview.ToString();

                            sheet.isApproved = false;
                        }
                      

                        repo.Add(sheet);

                        
                        MessageBox.Show("Budget Added Succesfully");
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        //var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;
                    }
                }
                myWindow.Close();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //SystemLog.LogError(this.GetType(), "SaleOrder Error refrence No= " + saleOrder.referenceNo + " Id=" + saleOrder.Id + ex.ToString());

            }
        }
        public List<BudgetCostField> getBudgetFields()
        {
            List<BudgetCostField> budgetFieldItems = new List<BudgetCostField>();
            List<BudgetCostField> budgetItems = new List<BudgetCostField>();
            ProcurementProduct product = new ProcurementProduct();
            budgetFieldItems = grdBudgetCostItems.ItemsSource as List<BudgetCostField>;     
            return budgetFieldItems;
        }
        
        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editOrder == 1 && sheet != null)
                {

                    repo = new BudgetCostCenterRepo();
                    sheet = new BudgetCostSheet();
                    sheet = repo.get(budgetid);
                    UsersRepo usersRepo = new UsersRepo();

                    if (sheet != null)
                    {
                        if (sheet.isApproved == true)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Budget is Approved, Do you want to UnApprove this Budget?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    sheet.isApproved = false;
                                    sheet.stage = TransactionStage.AwaitingApproval.ToString();
                                    repo.Approve(sheet);
                                    var res1 = MessageBox.Show("Budget has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), sheet.Id, TransactionItemType.Budget);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), sheet.Id, TransactionItemType.Budget);
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
                                    if (sheet.Currency != null)
                                    {
                                        symbolCurr = sheet.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog();

                                    

                                        comment.Comment = "Budget (Net Budget Profit) having value: " + sheet.TotalNetProfit.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "Budget UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                    
                                    procurementRepo.Add(sheet.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Budget are UnApproved (" + sheet.refNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Budget is UnApproved (" + sheet.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Budget Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Budget Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (sheet.isApproved == false)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Budget are Pending for Approval, Do you want to Approve this Budget?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    sheet.isApproved = true;
                                    sheet.stage = TransactionStage.Approved.ToString();
                                    repo.Approve(sheet);

                                    var res1 = MessageBox.Show("Budget has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), sheet.Id, TransactionItemType.Budget);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), sheet.Id, TransactionItemType.Budget);
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
                                    if (sheet.Currency != null)
                                    {
                                        symbolCurr = sheet.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog();
                                    comment.Comment = "Budget (Net Budget Profit) having value: " + sheet.TotalNetProfit.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "Budget UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    procurementRepo.Add(sheet.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Budget is Approved (" + sheet.refNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Budget is Approved (" + sheet.Id + ")");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Budget Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Budget Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }
                        }
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();


            budgetid = sheet.Id;
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null) ? true : false)
            {
                UsersRepo usersRepo = new UsersRepo();
                sheet = repo.get(budgetid);
                var row = sheet;
                if (row.budgetCostSheetStatus != null)
                {
                    oldStatus = row.budgetCostSheetStatus;
                }
                Procurementss.Budget.UserControls.ucStatusChange.inActiveStatuses = 1;

                Procurementss.Budget.UserControls.ucStatusChange.budgetid = (int)budgetid;

                Procurementss.Budget.frmBudgetSatatusChange statusChange = new frmBudgetSatatusChange(repo); 
                var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();
                if (Procurementss.Budget.UserControls.ucStatusChange.sheet.Id != 0)

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null) ? true : false)
                    {
                        Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = false;
                        Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.Approved.ToString();

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingApproval.ToString();
                        if (Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing == null)
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                    }

                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null)
                    {
                        Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing != true)
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                    }
                    else
                    {
                        Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingFirstReview.ToString();

                        Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;
                        usersRepo.Add(TransactionInfo.Closed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                    }
                Procurementss.Budget.UserControls.ucStatusChange.sheet.LastStatusChangeDate = System.DateTime.Now;
                Procurementss.Budget.UserControls.ucStatusChange.sheet.ClosingDate = System.DateTime.Now;
                if (row.budgetCostSheetStatus != Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus)
                    usersRepo.Add(TransactionInfo.Status_Changed, sheet.Id, (int)TransactionItemType.Budget, "While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                UsersRepo userRepo = new UsersRepo();
                //Asking for Tag
                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                var res = MessageBox.Show("Budget has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (row.Department != null && row.Department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.Department.Id, row.company.Id), sheet.Id, TransactionItemType.Budget);
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
                string newStat = Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status;
                string symbolCurr = "";
                if (row.Currency != null)
                {
                    symbolCurr = row.Currency.Abbrivation.ToString();
                }
                CommentLog comment = new CommentLog()
                {
                    Comment = "Status of Budget having Budget Net Profit: " + row.TotalNetProfit.ToString() + " (" + symbolCurr + ")\n "
                        + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                    Timestamp = DateTime.Now,
                    Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                };

                procurementRepo.Add(row.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);

                //Creating notification
                if (tagUsers.Count != 0)
                {
                    foreach (var user in tagUsers)
                    {

                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + row.refNo, row.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);

                    }
                }

                if (ccUsers.Count != 0)
                {
                    foreach (var user in ccUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + row.refNo, row.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                    }
                }
                try
                {
                    row = Procurementss.Budget.UserControls.ucStatusChange.sheet;
                    repo.updateStatusById(row.Id, row.budgetCostSheetStatus.Id);
                }
                catch { }
                MessageBox.Show("Budget status changed to InActive (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                var thisWindow = Window.GetWindow(this);
                thisWindow.Close();
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Budget Directly.");
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), TransactionItemType.Budget);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Budget);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (sheet!= null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && sheet.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, frmInputBox.comment, 0, user.id, "New Comment ", null);

                        }
                    }

                    procurementRepo.Add(sheet.Id, TransactionItemType.Budget, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (sheet.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Budget first to add a comment!");
                }

            }
            loadcomments();
        }
        public void loadcomments()
        {
            try
            {
                if (sheet != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(budgetid, TransactionItemType.Budget);
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
            {
                loadcomments();
            }
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
            }
            else
                grdComments.Visibility = Visibility.Collapsed;

        }

        private void btnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (sheet.Id != 0)
            {
                if (sheet.budgetCostSheetStatus.isActive == false && sheet.PendingForClosing != true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when Budget Closed") != null)
                    {

                        if (grdAttach.Visibility == Visibility.Visible)
                            grdAttach.Visibility = Visibility.Collapsed;
                        else
                        {
                            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                            grdAttach.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required" + " Can attach document when Budget Closed!");
                    }
                }
                else
                {
                    if (grdAttach.Visibility == Visibility.Visible)
                        grdAttach.Visibility = Visibility.Collapsed;
                    else
                    {
                        cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                        grdAttach.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Visible)
            {

                gridTracker.Visibility = Visibility.Collapsed;
            }
            else
           if (gridTracker.Visibility == Visibility.Collapsed && sheet.Id != 0)
            {

                views = UsersRepo.getViwerInfo(sheet.Id, (int)TransactionItemType.Sale_Order);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            GetTotalAmounts();
        }
        public void GetTotalAmounts()
        {
            var fields = grdBudgetCostItems.ItemsSource as List<BudgetCostField>;

            var totalCgs= fields.Where(x => x.BudgetSheettHead.isCGS == true).Sum(x => x.BudgetedCost);
            var totalIncome= fields.Where(x => x.BudgetSheettHead.isIncome == true).Sum(x => x.BudgetedCost);
            var totalExpense= fields.Where(x => x.BudgetSheettHead.isExpense == true).Sum(x => x.BudgetedCost);
            var grossProfit = totalIncome - totalCgs;
            var netProfit = grossProfit - totalExpense;
            txtBudgetTotalIncome.Text = totalIncome.ToString();
            txtBudgetTotalCGS.Text = totalCgs.ToString();
            txtBudgetTotalExpense.Text = totalExpense.ToString();
            txtBudgetTotalGrossProfit.Text = grossProfit.ToString();
            txtBudgetTotalNetProfit.Text = netProfit.ToString();




            var totalRSBCCgs = fields.Where(x => x.BudgetSheettHead.isCGS == true).Sum(x => x.RSBC);
            var totalRSBCIncome = fields.Where(x => x.BudgetSheettHead.isIncome == true).Sum(x => x.RSBC);
            var totalRSBCExpense = fields.Where(x => x.BudgetSheettHead.isExpense == true).Sum(x => x.RSBC);
            var grossRSBCProfit = totalRSBCIncome - totalRSBCCgs;
            var netRSBCProfit = grossRSBCProfit - totalRSBCExpense;
            txtRSBCTotalIncome.Text = totalRSBCIncome.ToString();
            txtRSBCTotalCGS.Text = totalRSBCCgs.ToString();
            txtRSBCTotalExpense.Text = totalRSBCExpense.ToString();
            txtRSBCTotalGrossProfit.Text = grossRSBCProfit.ToString();
            txtRSBCTotalNetProfit.Text = netRSBCProfit.ToString();
        }

        private void txtTotalIncome_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            GetTotalAmounts();
        }

        private void grdBudgetCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "systemCost":
                        double totalSystemCost = 0;
                        var field = grdBudgetCostItems.GetRowByListIndex(e.ListSourceRowIndex) as BudgetCostField;
                        SaleOrderRepo repo = new SaleOrderRepo();
                        if (budgetid != 0)
                        {
                            saleOrders = repo.GetSOBYBudgetId(budgetid);
                            purchaseOrders = repo.GetPOBYBudgetId(budgetid);
                            foreach (var saleOrder in saleOrders)
                            {
                                if (saleOrder.Id != 0)
                                {
                                    foreach (var invoice in saleOrder.SaleInvoices)
                                    {
                                        if (invoice.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += invoice.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                        }
                                        foreach (var receipt in invoice.salesReceipts)
                                        {
                                            if (receipt.BudgetSystemCostFields.Count != 0)
                                            {
                                                totalSystemCost += receipt.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                            }
                                        }
                                    }
                                    foreach (var bill in saleOrder.Bills)
                                    {
                                        if (bill.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += bill.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                        }
                                    }
                                }
                            }
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.Id != 0)
                                {
                                    foreach (var invoice in purchaseOrder.PurchaseInvoices)
                                    {
                                        if (invoice.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += invoice.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                        }
                                        foreach (var payment in invoice.Payments)
                                        {
                                            if (payment.BudgetSystemCostFields.Count != 0)
                                            {
                                                totalSystemCost += payment.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                            }
                                        }
                                    }
                                    foreach (var bill in purchaseOrder.Bills)
                                    {
                                        if (bill.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += bill.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == field.Head_Id).addedSystemCost;
                                        }
                                    }
                                }
                            }
                            e.Value = totalSystemCost;
                        }
                        break;
                }
        }
    }
}
