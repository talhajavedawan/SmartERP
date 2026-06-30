using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using ZAS_ERP.AssetRentalss.TenantRentals;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTenantRentalAdd.xaml
    /// </summary>
    public partial class ucFrmTenantRentalAdd : UserControl
    {
        TenantRentalRepo tenantRentalRepo = new TenantRentalRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        TenantRental tenant = new TenantRental();
        TenantRental trackingOrder = new TenantRental();

        public int tenantId = 0;
        public bool editFlag = false;
        TenantRentalStatus checkStatus = new TenantRentalStatus();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public ucFrmTenantRentalAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadStatuses();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Tenants") == null)
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
                tenant = tenantRentalRepo.GetTenantRentall(tenantId);

                TenantStages();

                if (tenant.CreationDate != null)
                    datCreationDate.EditValue = tenant.CreationDate;

                txtTenantName.Text = tenant.TenantName;

                if (tenant.company != null)
                    lookupCompany.Text = tenant.company.CompanyName;

                if (tenant.department != null)
                    lookupDepartment.Text = tenant.department.DeptName;

                if (tenant.assetRental != null)
                    lookupAsset.Text = tenant.assetRental.AssetName;

                if (tenant.contact != null)
                {
                    txtContactNo.Text = tenant.contact.ContactNo;
                    txtEmail.Text = tenant.contact.Email;
                }

                txtSystemRef.Text = tenant.SystemRef;

                if (tenant.Creator != null && tenant.Creator.employee != null)
                    txtCreator.Text = tenant.Creator.employee.person.FName + " " + tenant.Creator.employee.person.LName;

                if (tenant.person != null)
                {
                    txtCnic.Text = tenant.person.CNIC;
                    txtPass.Text = tenant.person.PassportNo;
                }

                    if (tenant.address != null)
                {
                    txtAddress.Text = tenant.address.Line1;
                    txtCity.Text = tenant.address.City;
                    txtCountry.Text = tenant.address.Country;
                }

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (tenant.Status != null)
                {
                    checkStatus = tenant.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == tenant.statusId)
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


        private void TenantStages()
        {
            if (tenant.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (tenant.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (tenant.isApproved == true && tenant.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (tenant.isApproved == true && tenant.Status.isActive == false && tenant.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (tenant.isApproved == true && tenant.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (tenant.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (tenant.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (tenant.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void loadStatuses()
        {
            List<TenantRentalStatus> tenantRentalStatuses = new List<TenantRentalStatus>();

            List<cmbitem> cmbitems = new List<cmbitem>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Tenant Statuses") != null)
                tenantRentalStatuses = tenantRentalRepo.GetAllTenantRentalStatuses();
            else
                tenantRentalStatuses = tenantRentalRepo.GetAllTenantRentalStatuses().Where(x => x.isActive == true).ToList();
            tenantRentalStatuses = tenantRentalStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(tenantRentalStatuses, delegate (TenantRentalStatus status) // foreach (BillStatus status in BillStatuses)
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

        private void loadCompanies()
        {
            empUser = tenantRentalRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }

  

        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = tenantRentalRepo.GetLastTransactionId();
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
                txtSystemRef.Text = "Tenant-" + intGroupId;
                lblRefNo.Text = " (Tenant-" + intGroupId + ")";
                tenant.transactionGroupId = intGroupId;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please enter Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTenantName.Text))
            {
                DXMessageBox.Show("Please enter Tenant Name!");
                txtTenantName.Focus();
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
            if (lookupAsset.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Asset!");
                lookupDepartment.Focus();
                return;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbStatus.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtContactNo.Text))
            {
                DXMessageBox.Show("Please enter Contact No!");
                txtContactNo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtEmail.Text))
            {
                DXMessageBox.Show("Please enter Email!");
                txtEmail.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCnic.Text))
            {
                DXMessageBox.Show("Please enter CNIC No!");
                txtCnic.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtPass.Text))
            {
                DXMessageBox.Show("Please enter Passport #!");
                txtEmail.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtAddress.Text))
            {
                DXMessageBox.Show("Please enter Address!");
                txtAddress.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCity.Text))
            {
                DXMessageBox.Show("Please enter City!");
                txtCity.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCountry.Text))
            {
                DXMessageBox.Show("Please enter Country!");
                txtCountry.Focus();
                return;
            }

            tenant.CreationDate = datCreationDate.DateTime;
            tenant.TenantName = txtTenantName.Text;

            tenant.companyId = (lookupCompany.SelectedItem as Company).Id;
            tenant.deptId = (lookupDepartment.SelectedItem as Department).Id;
            tenant.assetRentalId = (lookupAsset.SelectedItem as AssetRental).Id;
            tenant.statusId = (cmbStatus.SelectedItem as cmbitem).id;

            if (tenant.contact == null)
            {
                tenant.contact = new Contact();
            }
            tenant.contact.ContactNo = txtContactNo.Text;
            tenant.contact.Email = txtEmail.Text;

            if (tenant.address == null)
            {
                tenant.address = new Address();
            }
            tenant.address.Line1 = txtAddress.Text;
            tenant.address.City = txtCity.Text;
            tenant.address.Country = txtCountry.Text;

            if (tenant.person == null)
            {
                tenant.person = new Person();
            }
            tenant.person.CNIC = txtCnic.Text;
            tenant.person.PassportNo = txtPass.Text;
            tenant.SystemRef = txtSystemRef.Text;

            if (editFlag == false)
            {
                tenant.creatorId = SYSTEM_STATIC.currentUser.id;
                tenant.isApproved = false;
                tenantRentalRepo.AddTenantRental(tenant);
                DXMessageBox.Show("Successfully Added!");
            }
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without Approval") != null && tenant.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Tenant is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        tenant.stage = TransactionStage.Approved.ToString();
                        tenant.isApproved = true;
                        tenant.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without ReApproval") != null && tenant.isReApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Tenant is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        tenant.stage = TransactionStage.Approved.ToString();
                        tenant.isReApproved = true;
                        tenant.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != tenant.Status.Id)
                    {
                        tenant.LastStatusChangeDate = System.DateTime.Now;
                        if (tenant.Status.isActive != true)
                        {
                            tenant.ClosingDate = System.DateTime.Now;
                        }
                    }
                }

                tenantRentalRepo.UpdateTenantRental(tenant);
                DXMessageBox.Show("Successfully Updated!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CompanyRepo companyRepo = new CompanyRepo();

            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();

            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.isActive == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (tenant != null && editFlag == true)
                        if (tenant.department != null)
                            if (departments.FirstOrDefault(x => x.Id == tenant.department.Id) == null)
                                departments.Add(tenant.department);
                }

                lookupDepartment.ItemsSource = departments;
            }
        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex > -1 && lookupDepartment.SelectedIndex > -1)
            {
                var comp = lookupCompany.SelectedItem as Company;
                var dept = lookupDepartment.SelectedItem as Department;

                lookupAsset.ItemsSource = tenantRentalRepo.GetActiveAssetRentalsByCompDept(comp.Id, dept.Id);
            }
        }

        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
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
                if (editFlag == true && tenant != null)
                {

                    //SalesReceipt receipt = new SalesReceipt();
                    tenant = tenantRentalRepo.GetTenantRentall(tenant.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (tenant != null)
                    {
                        if (tenant.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Tenants") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Tenant is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    tenant.isApproved = false;
                                    tenant.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, tenant.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                                    tenantRentalRepo.UpdateTenantRental(tenant);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();

                                    var res1 = MessageBox.Show("Tenant has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id);

                                            if (userss.Find(x => x.id == tenant.creatorId) == null)
                                                userss.Add(tenant.Creator);

                                            winTagUsers win = new winTagUsers(userss, tenant.Id, TransactionItemType.TenantRental);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;

                                            if (tagUsers.Find(x => x.id == tenant.creatorId) == null)
                                                tagUsers.Add(tenant.Creator);

                                            if (ccUsers.Find(x => x.id == tenant.creatorId) == null)
                                                ccUsers.Add(tenant.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (tenant.currency != null)
                                    //{
                                    //    symbolCurr = tenant.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Tenant having System Ref: " + tenant.SystemRef.ToString() + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Tenant UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers
                                    };
                                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Tenant is UnApproved (" + tenant.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Tenant is UnApproved (" + tenant.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Tenant Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Tenant Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (tenant.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Tenants") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Tenant is Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    tenant.isApproved = true;
                                    tenant.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, tenant.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                                    tenantRentalRepo.UpdateTenantRental(tenant);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();

                                    var res1 = MessageBox.Show("Tenant has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id);

                                            if (userss.Find(x => x.id == tenant.creatorId) == null)
                                                userss.Add(tenant.Creator);

                                            winTagUsers win = new winTagUsers(userss, tenant.Id, TransactionItemType.TenantRental);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;

                                            if (tagUsers.Find(x => x.id == tenant.creatorId) == null)
                                                tagUsers.Add(tenant.Creator);

                                            if (ccUsers.Find(x => x.id == tenant.creatorId) == null)
                                                ccUsers.Add(tenant.Creator);
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
                                        Comment = "Tenant having System Ref: " + tenant.SystemRef.ToString() + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Tenant Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers
                                    };
                                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Tenant is Approved (" + tenant.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Tenant is Approved (" + tenant.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Tenant Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Tenant Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (tenant.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Tenants") != null) ? true : false)
                            {
                                tenant.isReApproved = true;
                                tenant.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, tenant.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                tenantRentalRepo.UpdateTenantRental(tenant);

                                MessageBox.Show("Tenant is Approved (" + tenant.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Tenant is Approved (" + tenant.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Tenants Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Tenants Directly user id=(" + MainWindow.currentUserid + ")");
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

        AssetRentalStatus oldStatus = new AssetRentalStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static TenantRentalStatus statusChanged = new TenantRentalStatus();

        public ucFrmTenantRentalAdd(TenantRentalStatus tenantRentalStatus)
        {
            statusChanged = tenantRentalStatus;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            //loansAdvanceRepo = new LoansAdvanceRepo();

            if (tenant != null && tenantId > 0)
            {
                var previous_status = tenant.Status.Status;
                if (tenant != null)
                {
                    if (tenant.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucTenantRentalStatusChange ucFrmDirectClose = new ucTenantRentalStatusChange();
                    if (tenant.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = tenant.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(tenant.Status.backcolor);
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

                        var res = MessageBox.Show("Tenant has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id);

                                if (userss.Find(x => x.id == tenant.creatorId) == null)
                                    userss.Add(tenant.Creator);


                                winTagUsers win = new winTagUsers(userss, tenant.Id, TransactionItemType.TenantRental);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();

                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;



                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }
                        }


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Tenants without Approval") != null)
                        {

                            tenant.PendingForClosing = false;
                            tenant.stage = TransactionStage.Closed.ToString();
                            tenant.statusId = statusChanged.Id;
                            tenant.LastStatusChangeDate = System.DateTime.Now;
                            tenant.ClosingDate = System.DateTime.Now;



                            tenantRentalRepo.UpdateTenantRental(tenant);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Tenant having system ref #: " + tenant.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers
                                };
                                if (tenant != null)
                                {
                                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            tenant.PendingForClosing = true;
                            tenant.stage = TransactionStage.AwaitingApproval.ToString();
                            tenant.statusId = statusChanged.Id;
                            tenant.LastStatusChangeDate = System.DateTime.Now;
                            tenant.ClosingDate = System.DateTime.Now;

                            tenantRentalRepo.UpdateTenantRental(tenant);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Tenant having system ref #: " + tenant.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers
                                };
                                if (tenant != null)
                                {
                                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
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
            if (editFlag == true && tenant != null)
            {

                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

                //{

                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                //    inputBox.ShowDialog();

                //}
                //                else 

                if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id);

                    if (tenant.Creator != null && userss.Find(x => x.id == tenant.creatorId) == null)
                        userss.Add(tenant.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.TenantRental);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (tenant != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && tenant.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(tenant.Id, TransactionItemType.TenantRental, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant with System Ref #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant with System Ref #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Tenant with System Ref #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Tenant with System Ref #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

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
                    else if (tenant.Id == 0)
                    {
                        DXMessageBox.Show("Kindly save this Transaction first to add a comment!");
                    }

                }
            }
        }

        public void loadcomments()
        {
            try
            {
                if (tenant != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(tenant.Id, TransactionItemType.TenantRental);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
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
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(tenant.Id, TransactionItemType.TenantRental);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (tenant != null && tenant.Id > 0)
            {
                var idd = tenant.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.TenantRental);
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
            if (tenant.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Tenants") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Tenants! Do you want to remove it from Void?", "Remove Void Tenant", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    tenant.isVoid = false;
                    tenantRentalRepo.UpdateTenantRental(tenant);

                    grdVoid.Visibility = Visibility.Collapsed;

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();

                    var res1 = MessageBox.Show("Tenant has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id), tenant.Id, TransactionItemType.TenantRental);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    //string symbolCurr = "";
                    //if (tenant.currency != null)
                    //{
                    //    symbolCurr = tenant.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Tenant having System Ref: " + tenant.SystemRef + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Tenant UnVoided",
                        TaggedList = tagUsers,
                        CCUsersList = ccUsers
                    };
                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant # " + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Tenants") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Tenants! Do you want to move it to Void tenants?", "Add to Void Tenants", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    tenant.isVoid = true;
                    tenantRentalRepo.UpdateTenantRental(tenant);

                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();

                    var res1 = MessageBox.Show("Tenant has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (tenant.department != null && tenant.department.Id != 0 && tenant.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(tenant.department.Id, tenant.company.Id), tenant.Id, TransactionItemType.TenantRental);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
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
                        Comment = "Tenant having System Ref: " + tenant.SystemRef + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Tenant Voided",
                        TaggedList = tagUsers,
                        CCUsersList = ccUsers
                    };
                    procurementRepo.Add(tenant.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Tenant #" + tenant.SystemRef, tenant.Id, TransactionItemType.TenantRental, comment.Comment, 0, user.id, "New Comment ", null);
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
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveTenantAttachmentCategories();
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
                if (tenant.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    List<TreeItem> atachments = SYSTEM_STATIC.GetTenantAttachmentsListByCategory((int)tenant.Id, TransactionItemType.TenantRental);

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
                trackingGrid.itemId = tenant.Id;
                trackingGrid.itemType = TransactionItemType.TenantRental;
                grdTracking.Children.Add(trackingGrid);
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                grdTracking.Children.Clear();
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }

        private void btnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        public void GellAllOrdersTracking()
        {

            //saleOrderRepo = new SaleOrderRepo();
            trackingOrder = tenantRentalRepo.GetTenantRentall(tenant.Id);
            if (trackingOrder != null)
            {
                if (grdTracking.Children != null && grdTracking.Children.Count > 0)
                {
                    ucTrackingGrid uc = grdTracking.Children[0] as ucTrackingGrid;


                    OrderTracking tracking = new OrderTracking();
                    uc.grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.TenantRental);
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
                        if (str.Contains("TenantRental"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.TenantRental);
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
                        else if (str.Contains("Rentals"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.RentalOrder);
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
                if (tenantId != 0)
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
                        destination += "Attachments\\TenantRental\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += tenantId + "_" + TransactionItemType.TenantRental.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.TenantRental);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), tenantId, TransactionItemType.TenantRental, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, tenant.Id, (int)TransactionItemType.TenantRental, "Added a new attachment");

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

        
       


    }
}
