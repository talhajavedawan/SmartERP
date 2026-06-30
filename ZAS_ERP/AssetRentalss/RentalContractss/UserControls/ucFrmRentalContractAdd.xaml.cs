using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZAS_ERP.AssetRentalss.RentalOrderss.UserControls;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmRentalContractAdd.xaml
    /// </summary>
    public partial class ucFrmRentalContractAdd : UserControl
    {
        RentalContractRepo rentalRepo = new RentalContractRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        RentalContract trackingOrder = new RentalContract();
        RentalContract rentalContract = new RentalContract();
        public int contractId = 0;
        public bool editFlag = false;
        RentalContractStatus checkStatus = new RentalContractStatus();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public ucFrmRentalContractAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadStatuses();
            LoadRentalBasis();
            loadCurrencies();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Rental Contracts") == null)
                datCreationDate.IsEnabled = false;
            else
                datCreationDate.IsEnabled = true;

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + ' ' + SYSTEM_STATIC.currentUser.employee.person.LName;
            }
            if (editFlag == true)
            {
                rentalContract = rentalRepo.GetRentalContract(contractId);

                views = UsersRepo.getViwerInfo(contractId, 22);
                grdUsers.ItemsSource = views;
                loadcomments();
                cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveRentalContractAttachmentCategories();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Rental Contracts") == null)
                {
                    btnSave.IsEnabled = false;
                }

                if (rentalContract.CreationDate != null)
                    datCreationDate.EditValue = rentalContract.CreationDate;

                if (rentalContract.company != null)
                    lookupCompany.Text = rentalContract.company.CompanyName;

                if (rentalContract.department != null)
                    lookupDepartment.Text = rentalContract.department.DeptName;

                if (rentalContract.assetRental != null)
                    lookupRentalAsset.Text = rentalContract.assetRental.AssetName;

                if (rentalContract.tenantRental != null)
                    lookupTenant.Text = rentalContract.tenantRental.TenantName;

                if (rentalContract.Creator != null)
                    txtCreator.Text = rentalContract.Creator.employee.person.FName + " " + rentalContract.Creator.employee.person.LName;

                cmbRentalBasis.SelectedIndex = (int) rentalContract.rentalBasis;

                txtSystemRef.Text = rentalContract.SystemRef;


                if (rentalContract.fromDate != null)
                    datFromDate.EditValue = rentalContract.fromDate;

                if (rentalContract.toDate != null)
                    datToDate.EditValue = rentalContract.toDate;

                var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                if (rentalContract.currency != null)
                {
                    int index = 0;
                    foreach (var _curr in currencyList)
                    {

                        if (_curr.id == rentalContract.currencyId)
                        {
                            cmbCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtRentAmount.Text = rentalContract.RentAmount.ToString();
                txtMonths.Text = rentalContract.NumberOfMonths.ToString();
                txtTotalRentAmount.Text = rentalContract.TotalRentAmount.ToString();

                

               

                RentalContractStages();

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (rentalContract.Status != null)
                {
                    checkStatus = rentalContract.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == rentalContract.statusId)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

            }
        }

        private void RentalContractStages()
        {
            if (rentalContract.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (rentalContract.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalContract.isApproved == true && rentalContract.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (rentalContract.isApproved == true && rentalContract.Status.isActive == false && rentalContract.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (rentalContract.isApproved == true && rentalContract.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalContract.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalContract.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalContract.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void LoadRentalBasis()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.RentalBasis.Yearly; i++)
            {
                cmbRentalBasis.Items.Add(((ERP_BL.Enums.RentalBasis)i).ToString());
            }
        }

        private void loadCompanies()
        {
            empUser = rentalRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }

       

        private void loadStatuses()
        {
            List<RentalContractStatus> rentalContractStatuses = new List<RentalContractStatus>();

            List<cmbitem> cmbitems = new List<cmbitem>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment Statuses") != null)
                rentalContractStatuses = rentalRepo.GetAllRentalContractStatuses();
            else
                rentalContractStatuses = rentalRepo.GetAllRentalContractStatuses().Where(x => x.isActive == true).ToList();
            rentalContractStatuses = rentalContractStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(rentalContractStatuses, delegate (RentalContractStatus status) // foreach (BillStatus status in BillStatuses)
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

        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = rentalRepo.GetLastTransactionId();
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


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                GroupIdCalculation();
                txtSystemRef.Text = "RentalContract-" + intGroupId;
                lblRefNo.Text = " (RentalContract-" + intGroupId + ")";
                rentalContract.transactionGroupId = intGroupId;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please enter Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment.Focus();
                return;
            }
            if (lookupRentalAsset.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset!");
                lookupRentalAsset.Focus();
                return;
            }
            if (lookupTenant.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Tenant!");
                lookupTenant.Focus();
                return;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbStatus.Focus();
                return;
            }
            if (cmbCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Currency!");
                cmbCurrency.Focus();
                return;
            }
            if (datFromDate.DateTime == null)
            {
                DXMessageBox.Show("Please enter From Date!");
                datFromDate.Focus();
                return;
            }
            if (datToDate.DateTime == null)
            {
                DXMessageBox.Show("Please enter To Date!");
                datFromDate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtRentAmount.Text) || Convert.ToDouble(txtRentAmount.Text) == 0)
            {
                DXMessageBox.Show("Please enter Rent Amount!");
                datFromDate.Focus();
                return;
            }
            

            rentalContract.CreationDate = datCreationDate.DateTime;
            rentalContract.companyId = (lookupCompany.SelectedItem as Company).Id;
            rentalContract.deptId = (lookupDepartment.SelectedItem as Department).Id;
            rentalContract.assetRentalId = (lookupRentalAsset.SelectedItem as AssetRental).Id;
            rentalContract.tenantRentalId = (lookupTenant.SelectedItem as TenantRental).Id;
            rentalContract.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            rentalContract.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
            rentalContract.rentalBasis = (ERP_BL.Enums.RentalBasis)cmbRentalBasis.SelectedIndex;
            rentalContract.SystemRef = txtSystemRef.Text;
            rentalContract.fromDate = datFromDate.DateTime;
            rentalContract.toDate = datToDate.DateTime;
            rentalContract.RentAmount = Convert.ToDouble(txtRentAmount.Text);
            rentalContract.NumberOfMonths = Convert.ToDouble(txtMonths.Text);
            rentalContract.TotalRentAmount = Convert.ToDouble(txtTotalRentAmount.Text);


            if (editFlag == false)
            {
                rentalContract.creatorId = SYSTEM_STATIC.currentUser.id;
                rentalContract.isApproved = false;
                rentalRepo.AddRentalContract(rentalContract);
                DXMessageBox.Show("Successfully Added!");
                UsersRepo.Add(TransactionInfo.Initialized, rentalContract.Id, (int)TransactionItemType.RentalContract, "Rental Contract Added");
            }
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without Approval") != null && rentalContract.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Rental Contract is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        rentalContract.stage = TransactionStage.Approved.ToString();
                        rentalContract.isApproved = true;
                        rentalContract.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without ReApproval") != null && rentalContract.isReApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Rental Contract is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        rentalContract.stage = TransactionStage.Approved.ToString();
                        rentalContract.isReApproved = true;
                        rentalContract.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != rentalContract.Status.Id)
                    {
                        rentalContract.LastStatusChangeDate = System.DateTime.Now;
                        if (rentalContract.Status.isActive != true)
                        {
                            rentalContract.ClosingDate = System.DateTime.Now;
                        }
                    }
                }

                rentalRepo.UpdateRentalContract(rentalContract);

                if (checkStatus.Id != rentalContract.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Rental Contract has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {

                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            var userss = UsersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id);

                            if (userss.Find(x => x.id == rentalContract.creatorId) == null)
                                userss.Add(rentalContract.Creator);

                            winTagUsers win = new winTagUsers(userss, rentalContract.Id, TransactionItemType.RentalContract);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                            if (tagUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                tagUsers.Add(rentalContract.Creator);

                            if (ccUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                ccUsers.Add(rentalContract.Creator);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = rentalContract.Status.Status;
                    string symbolCurr = "";
                    //if (rentalContract.currency != null)
                    //{
                    //    symbolCurr = rentalContract.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Rental Contract having System Ref: " + rentalContract.SystemRef + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, rentalContract.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + rentalContract.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, rentalContract.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                DXMessageBox.Show("Successfully Updated!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CompanyRepo companyRepo = new CompanyRepo();

            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();

            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.isActive == true && x.IsRentalContractType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (rentalContract != null && editFlag == true)
                        if (rentalContract.department != null)
                            if (departments.FirstOrDefault(x => x.Id == rentalContract.department.Id) == null)
                                departments.Add(rentalContract.department);
                }

                lookupDepartment.ItemsSource = departments;
            }
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex > -1 && lookupDepartment.SelectedIndex > -1)
            {
                var comp = lookupCompany.SelectedItem as Company;
                var dept = lookupDepartment.SelectedItem as Department;

                
                lookupRentalAsset.ItemsSource = rentalRepo.GetActiveAssetRentalsByCompDept(comp.Id, dept.Id);
            }
        }

        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void lookupRentalAsset_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex > -1 && lookupDepartment.SelectedIndex > -1 && lookupRentalAsset.SelectedIndex > -1)
            {
                var comp = lookupCompany.SelectedItem as Company;
                var dept = lookupDepartment.SelectedItem as Department;
                var asset = lookupRentalAsset.SelectedItem as AssetRental;

                lookupTenant.ItemsSource = rentalRepo.GetAllTenantsByCompDeptAsset(comp.Id, dept.Id, asset.Id);
            }
        }

        private void lookupRentalAsset_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void lookupTenant_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void grdRentalPeriodDetail_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void grdRentalPeriodDetail_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void tableViewRentalPeriodDetail_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void tableViewRentalPeriodDetail_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
        {

        }

        private void tableViewRentalPeriodDetail_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {

        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void btnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == true && rentalContract != null)
                {

                    //SalesReceipt receipt = new SalesReceipt();
                    rentalContract = rentalRepo.GetRentalContract(rentalContract.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalContract != null)
                    {
                        if (rentalContract.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Contracts") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Rental Contract is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    rentalContract.isApproved = false;
                                    rentalContract.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, rentalContract.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                                    rentalRepo.UpdateRentalContract(rentalContract);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Rental Contract has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id);

                                            if (userss.Find(x => x.id == rentalContract.creatorId) == null)
                                                userss.Add(rentalContract.Creator);

                                            winTagUsers win = new winTagUsers(userss, rentalContract.Id, TransactionItemType.RentalContract);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                                tagUsers.Add(rentalContract.Creator);

                                            if (ccUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                                ccUsers.Add(rentalContract.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (rentalContract.currency != null)
                                    //{
                                    //    symbolCurr = rentalContract.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Rental Contract having System Ref: " + rentalContract.SystemRef.ToString() + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Rental Contract UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Rental Contract is UnApproved (" + rentalContract.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Rental Contract is UnApproved (" + rentalContract.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Contract Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Contract Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Contracts") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Rental Contract is Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    rentalContract.isApproved = true;
                                    rentalContract.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, rentalContract.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                                    rentalRepo.UpdateRentalContract(rentalContract);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Rental Contract has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id);

                                            if (userss.Find(x => x.id == rentalContract.creatorId) == null)
                                                userss.Add(rentalContract.Creator);

                                            winTagUsers win = new winTagUsers(userss, rentalContract.Id, TransactionItemType.RentalContract);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                                tagUsers.Add(rentalContract.Creator);

                                            if (ccUsers.Find(x => x.id == rentalContract.creatorId) == null)
                                                ccUsers.Add(rentalContract.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (loansAdvance.currency != null)
                                    //{
                                    //    symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Rental Contract having System Ref: " + rentalContract.SystemRef.ToString() + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Rental Contract Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Rental Contract is Approved (" + rentalContract.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Rental Contract is Approved (" + rentalContract.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Contract Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Contract Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Contracts") != null) ? true : false)
                            {
                                rentalContract.isReApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, rentalContract.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                rentalRepo.UpdateRentalContract(rentalContract);

                                MessageBox.Show("Rental Contract is Approved (" + rentalContract.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Rental Contract is Approved (" + rentalContract.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Rental Contracts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Contracts Directly user id=(" + MainWindow.currentUserid + ")");
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


        RentalContractStatus oldStatus = new RentalContractStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static RentalContractStatus statusChanged = new RentalContractStatus();

        public ucFrmRentalContractAdd(RentalContractStatus rentalContractStatus)
        {
            statusChanged = rentalContractStatus;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            //loansAdvanceRepo = new LoansAdvanceRepo();

            if (rentalContract != null && contractId > 0)
            {
                var previous_status = rentalContract.Status.Status;
                if (rentalContract != null)
                {
                    if (rentalContract.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucRentalContractStatusChange ucFrmDirectClose = new ucRentalContractStatusChange();
                    if (rentalContract.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = rentalContract.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(rentalContract.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
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

                        var res = MessageBox.Show("Rental Contract has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id);

                                if (userss.Find(x => x.id == rentalContract.creatorId) == null)
                                    userss.Add(rentalContract.Creator);


                                winTagUsers win = new winTagUsers(userss, rentalContract.Id, TransactionItemType.RentalContract);
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


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Contracts without Approval") != null)
                        {

                            rentalContract.PendingForClosing = false;
                            rentalContract.stage = TransactionStage.Closed.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateRentalContract(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Contract having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            rentalContract.PendingForClosing = true;
                            rentalContract.stage = TransactionStage.AwaitingApproval.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;

                            rentalRepo.UpdateRentalContract(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Contract having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && rentalContract != null)
            {

                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

                //{

                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                //    inputBox.ShowDialog();

                //}
                //                else 

                if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id);

                    if (rentalContract.Creator != null && userss.Find(x => x.id == rentalContract.creatorId) == null)
                        userss.Add(rentalContract.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.RentalContract);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (rentalContract != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && rentalContract.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(rentalContract.Id, TransactionItemType.RentalContract, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Rental Contract with System Ref #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }


                            //foreach (var user in frmInputBox.Comment.TaggedList)
                            //{
                            //    if (frmInputBox.FlagForTag == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", null);

                            //}
                            //foreach (var user in frmInputBox.Comment.CCUsersList)
                            //{
                            //    if (frmInputBox.FlagForCC == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //}
                        }

                        //procurementRepo.Add(loansAdvance.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (rentalContract.Id == 0)
                    {
                        DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                    }

                }
            }
        }

        private void btnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void btnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(rentalContract.Id, TransactionItemType.RentalContract);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (rentalContract != null && rentalContract.Id > 0)
            {
                var idd = rentalContract.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.RentalContract);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Please save this Transaction first!");
            }
        }

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (rentalContract.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Rental Contracts") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Rental Contracts! Do you want to remove it from Void?", "Remove Void Rental Contract", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    rentalContract.isVoid = false;
                    rentalRepo.UpdateRentalContract(rentalContract);

                    grdVoid.Visibility = Visibility.Collapsed;

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Rental Contract has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id), rentalContract.Id, TransactionItemType.RentalContract);
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

                    //string symbolCurr = "";
                    //if (rentalContract.currency != null)
                    //{
                    //    symbolCurr = rentalContract.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Rental Contract having System Ref: " + rentalContract.SystemRef + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Rental Contract UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract # " + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Rental Contracts") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Rental Contracts! Do you want to move it to Void rentalContracts?", "Add to Void Rental Contracts", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    rentalContract.isVoid = true;
                    rentalRepo.UpdateRentalContract(rentalContract);

                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Rental Contract has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (rentalContract.department != null && rentalContract.department.Id != 0 && rentalContract.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(rentalContract.department.Id, rentalContract.company.Id), rentalContract.Id, TransactionItemType.RentalContract);
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

                    //string symbolCurr = "";
                    //if (loansAdvance.currency != null)
                    //{
                    //    symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Rental Contract having System Ref: " + rentalContract.SystemRef + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Rental Contract Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void btnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveRentalContractAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void btnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (rentalContract.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    List<TreeItem> atachments = SYSTEM_STATIC.GetRentalContractAttachmentsListByCategory((int)rentalContract.Id, TransactionItemType.RentalContract);

                    foreach (var cat in atachments)
                    {
                        foreach (var otherCat in otherAttachments)
                        {
                            if (otherCat.name == cat.name)
                            {
                                foreach (var file in otherCat.Items)
                                {
                                    cat.Items.Add(file);
                                }
                            }
                        }
                    }
                    treeViewAttachments1.ItemsSource = atachments;
                }
                grdAttachments1.Visibility = Visibility.Visible;

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                ucTrackingGrid trackingGrid = new ucTrackingGrid();
                trackingGrid.itemId = rentalContract.Id;
                trackingGrid.itemType = TransactionItemType.RentalContract;
                grdTracking.Children.Add(trackingGrid);
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                grdTracking.Children.Clear();
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }

        public void GellAllOrdersTracking()
        {

            //saleOrderRepo = new SaleOrderRepo();
            trackingOrder = rentalRepo.GetRentalContract(rentalContract.Id);
            if (trackingOrder != null)
            {
                if (grdTracking.Children != null && grdTracking.Children.Count > 0)
                {
                    ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;


                    OrderTracking tracking = new OrderTracking();
                    uc.grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.RentalContract);
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            if (grdTracking.Children != null && grdTracking.Children.Count > 0)
            {
                ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;
                uc.grdOrdersTracking.ShowLoadingPanel = true;
                (uc.grdOrdersTracking.View as TreeListView).ExpandAllNodes();
                uc.grdOrdersTracking.ShowLoadingPanel = false;
            }
        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            if (grdTracking.Children != null && grdTracking.Children.Count > 0)
            {
                ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;
                uc.grdOrdersTracking.ShowLoadingPanel = true;

                (uc.grdOrdersTracking.View as TreeListView).CollapseAllNodes();
                uc.grdOrdersTracking.ShowLoadingPanel = false;
            }

        }


        public void loadcomments()
        {
            try
            {
                if (rentalContract != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(rentalContract.Id, TransactionItemType.RentalContract);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void btnDownloadAttachment_Click(object sender, RoutedEventArgs e)
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

                        if (str.Contains("Rentals"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.RentalContract);
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
                        if (str.Contains("LoansAdvances"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.LoansAdvances);
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

        private void btnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (contractId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Multiselect = false;
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\RentalContract\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += contractId + "_" + TransactionItemType.RentalContract.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.RentalContract);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), contractId, TransactionItemType.RentalContract, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, rentalContract.Id, (int)TransactionItemType.RentalContract, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.LoansAdvances);
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

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (contractId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, contractId, (int)TransactionItemType.RentalContract, "Viewed details of Rental Contract");
            }
        }

        private void txtRentAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var months = Convert.ToDouble(txtMonths.Text);
            var rent = Convert.ToDouble(txtRentAmount.Text);

            txtTotalRentAmount.Text = Math.Round(months * rent, 2).ToString();
        }

        private void txtMonths_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var months = Convert.ToDouble(txtMonths.Text);
            var rent = Convert.ToDouble(txtRentAmount.Text);

            txtTotalRentAmount.Text = Math.Round(months * rent, 2).ToString();
        }

        private void datToDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime? fromdate = datFromDate.DateTime;
            DateTime? toDate = datToDate.DateTime;

            if (toDate < fromdate)
            {
                DXMessageBox.Show("End Date cannot be less than Starting Date!");
                datToDate.EditValue = DBNull.Value;
                return;
            }

            if (fromdate != null && toDate != null)
            {
                var dateSpan = ((toDate.Value.Year - fromdate.Value.Year) * 12) + toDate.Value.Month - fromdate.Value.Month /*DateTimeSpan.CompareDates(fromdate.Value, toDate.Value)*/;
                txtMonths.Text = dateSpan.ToString();
            }
        }

        private void datFromDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var fromdate = datFromDate.DateTime;
            var toDate = datToDate.DateTime;

            if(toDate < fromdate)
            {
                DXMessageBox.Show("Starting Date cannot exceed End Date!");
                datFromDate.EditValue = null;
                return;
            }

            if(fromdate != null && toDate != null)
            {
                var dateSpan = DateTimeSpan.CompareDates(fromdate, toDate);
                txtMonths.Text = dateSpan.Months.ToString();
            }
        }

        private void datToDate_GotFocus(object sender, RoutedEventArgs e)
        {
            var fromdate = datFromDate.EditValue;

            if (fromdate == null)
            {
                DXMessageBox.Show("Please Select Starting Date first");
                datFromDate.Focus();
                return;
            }
        }

        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            DateTime creationDate = (DateTime)datCreationDate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (cmbCurrency.SelectedIndex != -1 /*&& cmbbaseCurrency.SelectedIndex != -1*/)
                {
                    if (lookupCompany.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Company!");
                        lookupCompany.Focus();
                        return;
                    }


                    var cmpny = lookupCompany.SelectedItem as Company;
                    //var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCurrency.SelectedItem as cmbitem).id, cmpny.CurrencyId.Value, creationDate.Year);


                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (creationDate.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJan.ToString();
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateFeb.ToString();
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateMar.ToString();
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateApr.ToString();
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateMay.ToString();
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJun.ToString();
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJul.ToString();
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateAug.ToString();
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateSep.ToString();
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateOct.ToString();
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateNov.ToString();
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateDec.ToString();
                                break;
                            default:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = 0.ToString();
                                break;
                        }
                    }

                }
            }
        }

       

        private void lookupBank_GotFocus(object sender, RoutedEventArgs e)
        {
            if(lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment.Focus();
            }
        }

        private void btnCreateRentalOrder_Click(object sender, RoutedEventArgs e)
        {
            if(rentalContract == null || rentalContract.Id == 0)
            {
                DXMessageBox.Show("Please Save this Contract first!");
                return;
            }
            if(rentalContract.isApproved == false)
            {
                DXMessageBox.Show("This Contract is not Approved yet!");
                return;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Rental Order From Rental Contract") != null)
            {
                Window moduleWin = new Window();

                ucRentalOrderAdd rentalOrderAdd = new ucRentalOrderAdd();
                rentalOrderAdd.editFlag = false;
                rentalOrderAdd.contractId = rentalContract.Id;
                moduleWin.Content = rentalOrderAdd;
                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.WindowState = WindowState.Maximized;
                moduleWin.Show();

                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                DXMessageBox.Show("Permission required to Create Rental Order from Rental Contract!");
            }
        }
    }


    public struct DateTimeSpan
    {
        public int Years { get; }
        public int Months { get; }
        public int Days { get; }
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }
        public int Milliseconds { get; }

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        enum Phase { Years, Months, Days, Done }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }
    }
}
