using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.AssetsRentals.RentalOrders;
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
using ZAS_ERP.SaleOrderFolder.UserControls.RentalReceipts;

namespace ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalInvoiceAdd.xaml
    /// </summary>
    public partial class ucRentalInvoiceAdd : UserControl
    {
        RentalInvoiceRepo invoiceRepo = new RentalInvoiceRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        RentalInvoice rentalInvoice = new RentalInvoice();
        RentalInvoice trackingOrder = new RentalInvoice();
        public int orderId = 0;
        public int invoiceId = 0;
        public bool editFlag = false;
        RentalInvoiceStatus checkStatus = new RentalInvoiceStatus();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public ucRentalInvoiceAdd()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadStatuses();
            LoadRentalBasis();
            loadCurrencies();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Rental Invoices") == null)
                datCreationDate.IsEnabled = false;
            else
                datCreationDate.IsEnabled = true;

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + ' ' + SYSTEM_STATIC.currentUser.employee.person.LName;

                if (orderId > 0)
                {
                    RentalOrderRepo orderRepo = new RentalOrderRepo();
                    var rentalOrder = orderRepo.GetRentalOrder(orderId);

                    if (rentalOrder.company != null)
                        lookupCompany.Text = rentalOrder.company.CompanyName;

                    if (rentalOrder.department != null)
                        lookupDepartment.Text = rentalOrder.department.DeptName;

                    if (rentalOrder.assetRental != null)
                        lookupRentalAsset.Text = rentalOrder.assetRental.AssetName;

                    if (rentalOrder.tenantRental != null)
                        lookupTenant.Text = rentalOrder.tenantRental.TenantName;

                    if (rentalOrder.currency != null)
                    {
                        var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                        if (rentalOrder.currency != null)
                        {
                            int index = 0;
                            foreach (var _curr in currencyList)
                            {

                                if (_curr.id == rentalOrder.currencyId)
                                {
                                    cmbCurrency.SelectedIndex = index;
                                    index = 0;
                                    break;
                                }
                                index++;
                            }
                        }
                    }

                    cmbRentalBasis.SelectedIndex = Convert.ToInt32(rentalOrder.rentalBasis);

                    if (rentalOrder.fromDate != null)
                        datFromDate.EditValue = rentalOrder.fromDate;

                    if (rentalOrder.toDate != null)
                        datToDate.EditValue = rentalOrder.toDate;

                    if (rentalOrder.RentMonth != null)
                        datRentMonth.EditValue = rentalOrder.RentMonth;
                    //txtRentAmount.Text = rentalOrder.RentAmount.ToString();
                }
            }
            if (editFlag == true)
            {
                rentalInvoice = invoiceRepo.GetRentalInvoice(invoiceId);
                orderId = rentalInvoice.rentalOrderId.Value;

                views = UsersRepo.getViwerInfo(orderId, 22);
                grdUsers.ItemsSource = views;
                loadcomments();
                cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveRentalInvoiceAttachmentCategories();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Rental Invoices") == null)
                {
                    btnSave.IsEnabled = false;
                }

                if (rentalInvoice.CreationDate != null)
                    datCreationDate.EditValue = rentalInvoice.CreationDate;

                if (rentalInvoice.company != null)
                    lookupCompany.Text = rentalInvoice.company.CompanyName;

                if (rentalInvoice.department != null)
                    lookupDepartment.Text = rentalInvoice.department.DeptName;

                if (rentalInvoice.assetRental != null)
                    lookupRentalAsset.Text = rentalInvoice.assetRental.AssetName;

                if (rentalInvoice.tenantRental != null)
                    lookupTenant.Text = rentalInvoice.tenantRental.TenantName;

                if (rentalInvoice.Creator != null)
                    txtCreator.Text = rentalInvoice.Creator.employee.person.FName + " " + rentalInvoice.Creator.employee.person.LName;

                cmbRentalBasis.SelectedIndex = (int)rentalInvoice.rentalBasis;

                txtSystemRef.Text = rentalInvoice.SystemRef;


                if (rentalInvoice.fromDate != null)
                    datFromDate.EditValue = rentalInvoice.fromDate;

                if (rentalInvoice.toDate != null)
                    datToDate.EditValue = rentalInvoice.toDate;

                datRentMonth.EditValue = rentalInvoice.RentMonth;

                var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                if (rentalInvoice.currency != null)
                {
                    int index = 0;
                    foreach (var _curr in currencyList)
                    {

                        if (_curr.id == rentalInvoice.currencyId)
                        {
                            cmbCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtexchangerate.Text = rentalInvoice.MER.ToString();
                txtInvoiceAmount.Text = rentalInvoice.InvoiceAmount.ToString();
                txtInvoiceAmountMER.Text = rentalInvoice.InvoiceAmountMER.ToString();

                txtInvoicedAmount.Text = rentalInvoice.InvoiceAmount.ToString();

                var collectedAmount = Math.Round(rentalInvoice.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);
                txtCollectedAmount.Text = collectedAmount.ToString();

                txtUnCollectedAmount.Text = Math.Round(rentalInvoice.InvoiceAmount - collectedAmount, 2).ToString();

                //var invoicedAmount = rentalInvoice.rentalInvoices?.Where(x => x.isVoid != true).Sum(y => y.InvoiceAmount);
                //txtInvoicedAmount.Text = invoicedAmount.ToString();
                //txtUnInvoicedAmount.Text = (rentalInvoice.RentAmount - invoicedAmount).ToString();

                RentalInvoiceStages();

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (rentalInvoice.Status != null)
                {
                    checkStatus = rentalInvoice.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == rentalInvoice.statusId)
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

        private void RentalInvoiceStages()
        {
            if (rentalInvoice.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (rentalInvoice.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalInvoice.isApproved == true && rentalInvoice.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (rentalInvoice.isApproved == true && rentalInvoice.Status.isActive == false && rentalInvoice.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (rentalInvoice.isApproved == true && rentalInvoice.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalInvoice.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalInvoice.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (rentalInvoice.PendingForClosing == true)
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
            empUser = invoiceRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }



        private void loadStatuses()
        {
            List<RentalInvoiceStatus> rentalInvoiceStatuses = new List<RentalInvoiceStatus>();

            List<cmbitem> cmbitems = new List<cmbitem>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Rental Invoices Statuses") != null)
                rentalInvoiceStatuses = invoiceRepo.GetAllRentalInvoiceStatuses();
            else
                rentalInvoiceStatuses = invoiceRepo.GetAllRentalInvoiceStatuses().Where(x => x.isActive == true).ToList();
            rentalInvoiceStatuses = rentalInvoiceStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(rentalInvoiceStatuses, delegate (RentalInvoiceStatus status) // foreach (BillStatus status in BillStatuses)
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

            var lastPaymentId = invoiceRepo.GetLastTransactionId();
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
                txtSystemRef.Text = "RentalInvoice-" + intGroupId;
                lblRefNo.Text = " (RentalInvoice-" + intGroupId + ")";
                rentalInvoice.transactionGroupId = intGroupId;
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
            if (String.IsNullOrEmpty(txtInvoiceAmount.Text) || Convert.ToDouble(txtInvoiceAmount.Text) == 0)
            {
                DXMessageBox.Show("Please enter Rent Amount!");
                datFromDate.Focus();
                return;
            }


            rentalInvoice.CreationDate = datCreationDate.DateTime;
            rentalInvoice.companyId = (lookupCompany.SelectedItem as Company).Id;
            rentalInvoice.deptId = (lookupDepartment.SelectedItem as Department).Id;
            rentalInvoice.assetRentalId = (lookupRentalAsset.SelectedItem as AssetRental).Id;
            rentalInvoice.TenantRentalId = (lookupTenant.SelectedItem as TenantRental).Id;
            rentalInvoice.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            rentalInvoice.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
            rentalInvoice.rentalBasis = (ERP_BL.Enums.RentalBasis)cmbRentalBasis.SelectedIndex;
            rentalInvoice.SystemRef = txtSystemRef.Text;
            rentalInvoice.fromDate = datFromDate.DateTime;
            rentalInvoice.toDate = datToDate.DateTime;
            rentalInvoice.MER = Convert.ToDouble(txtexchangerate.Text);
            rentalInvoice.RentMonth = datRentMonth.DateTime;
            rentalInvoice.InvoiceAmount = Convert.ToDouble(txtInvoiceAmount.Text);
            rentalInvoice.InvoiceAmountMER = Convert.ToDouble(txtInvoiceAmountMER.Text);
            rentalInvoice.rentalOrderId = orderId;

            if (editFlag == false)
            {
                rentalInvoice.creatorId = SYSTEM_STATIC.currentUser.id;
                rentalInvoice.isApproved = false;
                invoiceRepo.AddRentalInvoice(rentalInvoice);
                DXMessageBox.Show("Successfully Added!");
                UsersRepo.Add(TransactionInfo.Initialized, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, "Rental Invoice Added");
            }
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without Approval") != null && rentalInvoice.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Rental Invoice is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        rentalInvoice.stage = TransactionStage.Approved.ToString();
                        rentalInvoice.isApproved = true;
                        rentalInvoice.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without ReApproval") != null && rentalInvoice.isReApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Rental Invoice is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        rentalInvoice.stage = TransactionStage.Approved.ToString();
                        rentalInvoice.isReApproved = true;
                        rentalInvoice.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != rentalInvoice.Status.Id)
                    {
                        rentalInvoice.LastStatusChangeDate = System.DateTime.Now;
                        if (rentalInvoice.Status.isActive != true)
                        {
                            rentalInvoice.ClosingDate = System.DateTime.Now;
                        }
                    }
                }

                invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                if (checkStatus.Id != rentalInvoice.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();
                    List<User> tagUsersRecommendation = new List<User>();
                    List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Rental Invoice has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            var userss = UsersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id);

                            if (userss.Find(x => x.id == rentalInvoice.creatorId) == null)
                                userss.Add(rentalInvoice.Creator);

                            winTagUsers win = new winTagUsers(userss, rentalInvoice.Id, TransactionItemType.RentalInvoice);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                            tagUsersRecommendation = win.tagRecommendationUsers;
                            ccUsersRecommendation = win.ccRecommendationUsers;

                            if (tagUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                tagUsers.Add(rentalInvoice.Creator);

                            if (ccUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                ccUsers.Add(rentalInvoice.Creator);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = rentalInvoice.Status.Status;
                    string symbolCurr = "";
                    //if (rentalInvoice.currency != null)
                    //{
                    //    symbolCurr = rentalInvoice.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Rental Invoice having System Ref: " + rentalInvoice.SystemRef + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed"; comment.TaggedList = tagUsers;
                    comment.CCUsersList = ccUsers;
                    comment.TaggedRecomenndedList = tagUsersRecommendation;
                    comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, rentalInvoice.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + rentalInvoice.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

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
                    foreach (var _dept in empUser.departments.Where(x => x.isActive == true && x.IsRentalInvoiceType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (rentalInvoice != null && editFlag == true)
                        if (rentalInvoice.department != null)
                            if (departments.FirstOrDefault(x => x.Id == rentalInvoice.department.Id) == null)
                                departments.Add(rentalInvoice.department);
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

                AssetRentalRepo repo = new AssetRentalRepo();
                lookupRentalAsset.ItemsSource = repo.GetActiveAssetRentalsByCompDept(comp.Id, dept.Id);
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

                TenantRentalRepo repo = new TenantRentalRepo();
                lookupTenant.ItemsSource = repo.GetAllTenantsByCompDeptAsset(comp.Id, dept.Id, asset.Id);
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
                if (editFlag == true && rentalInvoice != null)
                {

                    //SalesReceipt receipt = new SalesReceipt();
                    rentalInvoice = invoiceRepo.GetRentalInvoice(rentalInvoice.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalInvoice != null)
                    {
                        if (rentalInvoice.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Invoices") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Rental Invoice is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    rentalInvoice.isApproved = false;
                                    rentalInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                                    invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Rental Invoice has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id);

                                            if (userss.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                userss.Add(rentalInvoice.Creator);

                                            winTagUsers win = new winTagUsers(userss, rentalInvoice.Id, TransactionItemType.RentalInvoice);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                tagUsers.Add(rentalInvoice.Creator);

                                            if (ccUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                ccUsers.Add(rentalInvoice.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    //string symbolCurr = "";
                                    //if (rentalInvoice.currency != null)
                                    //{
                                    //    symbolCurr = rentalInvoice.currency.Abbrivation.ToString();
                                    //}
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Rental Invoice having System Ref: " + rentalInvoice.SystemRef.ToString() + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Rental Invoice UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Rental Invoice is UnApproved (" + rentalInvoice.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Rental Invoice is UnApproved (" + rentalInvoice.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Invoice Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Invoice Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalInvoice.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Invoices") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Rental Invoice is Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    rentalInvoice.isApproved = true;
                                    rentalInvoice.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                                    invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Rental Invoice has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id);

                                            if (userss.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                userss.Add(rentalInvoice.Creator);

                                            winTagUsers win = new winTagUsers(userss, rentalInvoice.Id, TransactionItemType.RentalInvoice);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                tagUsers.Add(rentalInvoice.Creator);

                                            if (ccUsers.Find(x => x.id == rentalInvoice.creatorId) == null)
                                                ccUsers.Add(rentalInvoice.Creator);
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
                                        Comment = "Rental Invoice having System Ref: " + rentalInvoice.SystemRef.ToString() + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Rental Invoice Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Rental Invoice is Approved (" + rentalInvoice.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Rental Invoice is Approved (" + rentalInvoice.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Invoice Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Invoice Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalInvoice.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Invoices") != null) ? true : false)
                            {
                                rentalInvoice.isReApproved = true;
                                rentalInvoice.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                                MessageBox.Show("Rental Invoice is Approved (" + rentalInvoice.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Rental Invoice is Approved (" + rentalInvoice.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Rental Invoices Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Invoices Directly user id=(" + MainWindow.currentUserid + ")");
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


        RentalInvoiceStatus oldStatus = new RentalInvoiceStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static RentalInvoiceStatus statusChanged = new RentalInvoiceStatus();

        public ucRentalInvoiceAdd(RentalInvoiceStatus rentalInvoiceStatus)
        {
            statusChanged = rentalInvoiceStatus;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            //loansAdvanceRepo = new LoansAdvanceRepo();

            if (rentalInvoice != null && orderId > 0)
            {
                var previous_status = rentalInvoice.Status.Status;
                if (rentalInvoice != null)
                {
                    if (rentalInvoice.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucRentalInvoiceStatusChange ucFrmDirectClose = new ucRentalInvoiceStatusChange();
                    if (rentalInvoice.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = rentalInvoice.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(rentalInvoice.Status.backcolor);
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

                        var res = MessageBox.Show("Rental Invoice has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id);

                                if (userss.Find(x => x.id == rentalInvoice.creatorId) == null)
                                    userss.Add(rentalInvoice.Creator);


                                winTagUsers win = new winTagUsers(userss, rentalInvoice.Id, TransactionItemType.RentalInvoice);
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


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Invoices without Approval") != null)
                        {

                            rentalInvoice.PendingForClosing = false;
                            rentalInvoice.stage = TransactionStage.Closed.ToString();
                            rentalInvoice.statusId = statusChanged.Id;
                            rentalInvoice.LastStatusChangeDate = System.DateTime.Now;
                            rentalInvoice.ClosingDate = System.DateTime.Now;



                            invoiceRepo.UpdateRentalInvoice(rentalInvoice);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Invoice having system ref #: " + rentalInvoice.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalInvoice != null)
                                {
                                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            rentalInvoice.PendingForClosing = true;
                            rentalInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                            rentalInvoice.statusId = statusChanged.Id;
                            rentalInvoice.LastStatusChangeDate = System.DateTime.Now;
                            rentalInvoice.ClosingDate = System.DateTime.Now;

                            invoiceRepo.UpdateRentalInvoice(rentalInvoice);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Invoice having system ref #: " + rentalInvoice.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalInvoice != null)
                                {
                                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
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
            if (editFlag == true && rentalInvoice != null)
            {

                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

                //{

                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                //    inputBox.ShowDialog();

                //}
                //                else 

                if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id);

                    if (rentalInvoice.Creator != null && userss.Find(x => x.id == rentalInvoice.creatorId) == null)
                        userss.Add(rentalInvoice.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.RentalInvoice);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (rentalInvoice != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && rentalInvoice.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(rentalInvoice.Id, TransactionItemType.RentalInvoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Rental Invoice with System Ref #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

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
                    else if (rentalInvoice.Id == 0)
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
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(rentalInvoice.Id, TransactionItemType.RentalInvoice);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (rentalInvoice != null && rentalInvoice.Id > 0)
            {
                var idd = rentalInvoice.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.RentalInvoice);
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
            if (rentalInvoice.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Rental Invoices") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Rental Invoices! Do you want to remove it from Void?", "Remove Void Rental Invoice", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    rentalInvoice.isVoid = false;
                    invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                    grdVoid.Visibility = Visibility.Collapsed;

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();
                    List<User> tagUsersRecommendation = new List<User>();
                    List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Rental Invoice has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id), rentalInvoice.Id, TransactionItemType.RentalInvoice);
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
                    //if (rentalInvoice.currency != null)
                    //{
                    //    symbolCurr = rentalInvoice.currency.Abbrivation.ToString();
                    //}
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Rental Invoice having System Ref: " + rentalInvoice.SystemRef + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Rental Invoice UnVoided",
                        TaggedList = tagUsers,
                        CCUsersList = ccUsers,
                        TaggedRecomenndedList = tagUsersRecommendation,
                        CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice # " + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Rental Invoices") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Rental Invoices! Do you want to move it to Void rentalInvoices?", "Add to Void Rental Invoices", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    rentalInvoice.isVoid = true;
                    invoiceRepo.UpdateRentalInvoice(rentalInvoice);

                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);

                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();
                    List<User> tagUsersRecommendation = new List<User>();
                    List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Rental Invoice has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (rentalInvoice.department != null && rentalInvoice.department.Id != 0 && rentalInvoice.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(rentalInvoice.department.Id, rentalInvoice.company.Id), rentalInvoice.Id, TransactionItemType.RentalInvoice);
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
                        Comment = "Rental Invoice having System Ref: " + rentalInvoice.SystemRef + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Rental Invoice Voided",
                        TaggedList = tagUsers,
                        CCUsersList = ccUsers,
                        TaggedRecomenndedList = tagUsersRecommendation,
                        CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(rentalInvoice.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Rental Invoice #" + rentalInvoice.SystemRef, rentalInvoice.Id, TransactionItemType.RentalInvoice, comment.Comment, 0, user.id, "New Comment ", null);
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
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveRentalInvoiceAttachmentCategories();
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
                if (rentalInvoice.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    List<TreeItem> atachments = SYSTEM_STATIC.GetRentalInvoiceAttachmentsListByCategory((int)rentalInvoice.Id, TransactionItemType.RentalInvoice);

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
                trackingGrid.itemId = rentalInvoice.Id;
                trackingGrid.itemType = TransactionItemType.RentalInvoice;
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
            trackingOrder = invoiceRepo.GetRentalInvoice(rentalInvoice.Id);
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
                if (rentalInvoice != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(rentalInvoice.Id, TransactionItemType.RentalInvoice);

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
                            var result = attachment.startDownload(str, TransactionItemType.RentalInvoice);
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
                if (orderId != 0)
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
                        destination += "Attachments\\RentalInvoice\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += orderId + "_" + TransactionItemType.RentalInvoice.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.RentalInvoice);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), orderId, TransactionItemType.RentalInvoice, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, rentalInvoice.Id, (int)TransactionItemType.RentalInvoice, "Added a new attachment");

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
            if (invoiceId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, invoiceId, (int)TransactionItemType.RentalInvoice, "Viewed details of Rental Invoice");
            }
        }

        private void txtInvoiceAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var rentAmount = Convert.ToDouble(txtInvoiceAmount.Text);
            var mer = Convert.ToDouble(txtexchangerate.Text);

            txtInvoiceAmountMER.Text = Math.Round(rentAmount * mer, 2).ToString();
        }

        private void txtMonths_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

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
        }

        private void datFromDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var fromdate = datFromDate.DateTime;
            var toDate = datToDate.DateTime;

            if (toDate < fromdate)
            {
                DXMessageBox.Show("Starting Date cannot exceed End Date!");
                datFromDate.EditValue = null;
                return;
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
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                lookupDepartment.Focus();
            }
        }

        private void txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var rentAmount = Convert.ToDouble(txtInvoiceAmount.Text);
            var mer = Convert.ToDouble(txtexchangerate.Text);

            txtInvoiceAmountMER.Text = Math.Round(rentAmount * mer, 2).ToString();
        }

        private void btnCreateRentalReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (rentalInvoice == null || rentalInvoice.Id == 0)
            {
                DXMessageBox.Show("Please Save this Transaction first!");
                return;
            }
            if (rentalInvoice.isApproved == false)
            {
                DXMessageBox.Show("This Transaction is not Approved yet!");
                return;
            }
            ucFrmRentalReceiptAdd frmRentalReceiptAdd = new ucFrmRentalReceiptAdd();
            frmRentalReceiptAdd.editFlag = false;
            frmRentalReceiptAdd.rentalInvoiceId = rentalInvoice.Id;

            Window win = new Window();
            win.Content = frmRentalReceiptAdd;
            win.Show();

        }

    }
}
