using DevExpress.Xpf.Core;
using ERP_BL.Countryy;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.FilesAndDocs;
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
using ZAS_ERP.Procurementss;


namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTravelingRecord.xaml
    /// </summary>
    public partial class ucFrmTravelingRecord : UserControl
    {
        List<VisitingCountry> visitingCountries = new List<VisitingCountry>();

        TravelingRecords travelingRecord = new TravelingRecords();
        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public int recordId = 0;
        public bool editFlag = false;

        public TravelingStatus checkStatus = new TravelingStatus();
        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        public ucFrmTravelingRecord()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            visitingCountries = new List<VisitingCountry>();
            LoadStatuses();
            LoadCompanies();
            lookupTravelerName.ItemsSource = recordRepo.GetAllTravelers();

            //grdVisitingRecords.GroupBy("sequence");
            if (editFlag == false && recordId == 0)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                travelingRecord.visitingCountries = new List<VisitingCountry>();
            }
            else if (editFlag == true && recordId > 0)
            {
                RefreshData();
            }
            grdVisitingRecords.ItemsSource = visitingCountries;
        }

        private void RefreshData()
        {
            itineraryId = 0;
            recordRepo = new VisitingRecordRepo();
            travelingRecord = recordRepo.GetTravelingRecord(recordId);

            if (travelingRecord.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (travelingRecord.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (travelingRecord.isApproved == true && travelingRecord.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (travelingRecord.isApproved == true && travelingRecord.Status.isActive == false && travelingRecord.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (travelingRecord.isApproved == true && travelingRecord.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (travelingRecord.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (travelingRecord.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (travelingRecord.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }


            views = UsersRepo.getViwerInfo(recordId, (int)TransactionItemType.TravelingRecord);
            grdUsers.ItemsSource = views;

            if (travelingRecord.visitingCountries != null)
                visitingCountries = travelingRecord.visitingCountries;
            visitingCountries = travelingRecord.visitingCountries;

            if (travelingRecord.CreationDate != null)
            {
                datCreationDate.DateTime = travelingRecord.CreationDate.Value;
            }

            if (travelingRecord.companyId != null)
                lookupCompany.EditValue = travelingRecord.companyId;

            if (travelingRecord.deptId != null)
                lookupDepartment.EditValue = travelingRecord.deptId;

            if (travelingRecord.travelerName != null)
            {
                lookupTravelerName.Text = travelingRecord.travelerName.Name;
            }
            if (travelingRecord.residentYear != null)
            {
                datResidentYear.DateTime = travelingRecord.residentYear.Value;
            }

            if (travelingRecord.Creator != null)
            {
                txtCreator.Text = travelingRecord.Creator.employee.person.FName + " " + travelingRecord.Creator.employee.person.LName;
            }
            if (!String.IsNullOrEmpty(travelingRecord.SystemRefNo))
            {
                txtSystemRef.Text = travelingRecord.SystemRefNo;
            }

            var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
            if (travelingRecord.Status != null)
            {
                int index = 0;
                foreach (var _status in statusList)
                {

                    if (_status.id == travelingRecord.statusId)
                    {
                        cmbStatus.SelectedIndex = index;
                        index = 0;
                        break;
                    }
                    index++;
                }
                checkStatus = travelingRecord.Status;
            }
            if (travelingRecord.residentFromDate != null)
            {
                datResidentFromDate.SelectedDate = travelingRecord.residentFromDate;
            }
            if (travelingRecord.residentToDate != null)
            {
                datResidentToDate.SelectedDate = travelingRecord.residentToDate;
            }
            if (travelingRecord.residentCountry != null)
            {
                lookupResidentCountry.EditValue = travelingRecord.residentCountryId;
                txtResidentDays.Text = travelingRecord.residentCountry.country.ResidentDays.ToString();
            }

            grdVisitingRecords.ItemsSource = visitingCountries;
            grdVisitingRecords.RefreshData();

            if (editFlag == true && travelingRecord != null && travelingRecord.residentCountry != null)
            {
                var countries = visitingCountries.Where(x => x.LeavingDate != null && x.DepartureDate != null);
                    var totalDays = countries.Sum(x => x.LeavingDate?.Subtract(x.DepartureDate.Value).TotalDays);
                    var residentCountryDays = countries.Where(x => x.visitingCountry?.Id == travelingRecord.residentCountry.countryId).ToList().Sum(x => x.LeavingDate?.Subtract(x.DepartureDate.Value).TotalDays);
                    var otherCountriesDays = countries.Where(x => x.visitingCountry?.Id != travelingRecord.residentCountry.countryId).ToList().Sum(x => x.LeavingDate?.Subtract(x.DepartureDate.Value).TotalDays);

                    txtResidentCountryDays.Text = residentCountryDays.ToString();
                    txtOtherCountryDays.Text = otherCountriesDays.ToString();
                    txtTotalDays.Text = totalDays.ToString();
                
            }

            //for (int i = 0; i < grdVisitingRecords.VisibleRowCount; i++)
            //{
            //    //var _row = grdCntrlStepsForRegister.GetRow(i);
            //    int rowHandle = grdVisitingRecords.GetRowHandleByVisibleIndex(i);
            //    var _row = grdVisitingRecords.GetRow(rowHandle);
            //    var _visitRecord = _row as VisitingCountry;
            //    if (rowHandle >= 0 && _visitRecord != null && _visitRecord.Id > 0)
            //    {
            //        if(itineraryId != _visitRecord.transactionGroupId)
            //        {
            //            itineraryId = _visitRecord.transactionGroupId;
            //            grdVisitingRecords.SetCellValue(rowHandle, grdVisitingRecords.Columns["TicketCostt"], _visitRecord.TicketCost);
            //            grdVisitingRecords.SetCellValue(rowHandle, grdVisitingRecords.Columns["MERR"], _visitRecord.MER);
            //            grdVisitingRecords.SetCellValue(rowHandle, grdVisitingRecords.Columns["CostMERR"], _visitRecord.TicketCostMER);
            //        }
                 
            //    }
            //}
        }
        private void LoadTravelers()
        {
            lookupTravelerName.ItemsSource = recordRepo.GetAllTravelers();
        }

        private void LoadStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();
            List<TravelingStatus> travelingStatuses = new List<TravelingStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Traveling Statuses") != null)
                travelingStatuses = recordRepo.GetAllTravelingStatuses();
            else
                travelingStatuses = recordRepo.GetAllTravelingStatuses().Where(x => x.isActive == true).ToList();

            Parallel.ForEach(travelingStatuses, delegate (TravelingStatus status) // foreach (BillStatus status in BillStatuses)
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

      

       

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (datCreationDate.EditValue == null)
            {
                DXMessageBox.Show("Please select Creation Date!");
                datCreationDate.Focus();
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
            if (lookupTravelerName.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Traveler!");
                lookupTravelerName.Focus();
                return;
            }
            if (datResidentYear.EditValue == null)
            {
                DXMessageBox.Show("Please select Resident Year!");
                datResidentYear.Focus();
                return;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbStatus.Focus();
                return;
            }
            if(datResidentFromDate.SelectedDate == null)
            {
                DXMessageBox.Show("Please select Resident from Date!");
                datResidentFromDate.Focus();
                return;
            }
            if (datResidentToDate.SelectedDate == null)
            {
                DXMessageBox.Show("Please select Resident To Date!");
                datResidentToDate.Focus();
                return;
            }
            if (lookupResidentCountry.SelectedIndex < 0)
            {
                DXMessageBox.Show("There is no Resident Country selected");
                lookupResidentCountry.Focus();
                return;
            }

            if (editFlag == true)
                if (visitingCountries == null || visitingCountries.Count == 0)
                {
                    DXMessageBox.Show("Please Add at least one visit!");
                    return;
                }

            travelingRecord.CreationDate = datCreationDate.DateTime;
            travelingRecord.companyId = (lookupCompany.SelectedItem as Company).Id;
            travelingRecord.deptId = (lookupDepartment.SelectedItem as Department).Id;
            travelingRecord.travelerNameId = (lookupTravelerName.SelectedItem as Traveler).Id;
            travelingRecord.residentYear = datResidentYear.DateTime;
            travelingRecord.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            travelingRecord.residentFromDate = datResidentFromDate.SelectedDate;
            travelingRecord.residentToDate = datResidentToDate.SelectedDate;
            travelingRecord.residentCountryId = (lookupResidentCountry.SelectedItem as ResidentCountry).Id;

            travelingRecord.DaysInResidentCountry = Convert.ToDouble(txtResidentCountryDays.Text);
            travelingRecord.DaysInOtherCountries = Convert.ToDouble(txtOtherCountryDays.Text);
            travelingRecord.TotalDays = Convert.ToDouble(txtTotalDays.Text);
            travelingRecord.RequiredResidentDays = Convert.ToDouble(txtResidentDays.Text);

            if (visitingCountries != null)
            {
                foreach (var _visit in visitingCountries)
                {
                    if (!travelingRecord.visitingCountries.Contains(_visit))
                    {
                        travelingRecord.visitingCountries.Add(_visit);
                    }

                }
            }

            if (editFlag == false)
            {
                travelingRecord.creatorId = SYSTEM_STATIC.currentUser.id;
                travelingRecord.isApproved = false;
                recordRepo.AddTravelingRecord(travelingRecord);
                DXMessageBox.Show("Added Successfully!");

                DXMessageBox.Show("You should add new records to complete the process!");
                recordId = travelingRecord.Id;
                editFlag = true;
                RefreshData();
            }
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without Approval") != null && travelingRecord.isApproved == false)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Travel Record is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        travelingRecord.stage = TransactionStage.Approved.ToString();
                        travelingRecord.isApproved = true;
                        travelingRecord.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without ReApproval") != null && travelingRecord.isReApproved == false)
                {                   
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Travel Record is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        travelingRecord.stage = TransactionStage.Approved.ToString();
                        travelingRecord.isReApproved = true;
                        travelingRecord.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != travelingRecord.Status.Id)
                    {
                        travelingRecord.LastStatusChangeDate = System.DateTime.Now;
                        if (travelingRecord.Status.isActive != true)
                        {
                            travelingRecord.ClosingDate = System.DateTime.Now;
                        }
                    }
                }
                recordRepo.UpdateTravelingRecord(travelingRecord);

                if (checkStatus.Id != travelingRecord.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Travel Record has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            var userss = UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                            if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                                userss.Add(travelingRecord.Creator);

                            winTagUsers win = new winTagUsers(userss, travelingRecord.Id, TransactionItemType.TravelingRecord);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                            if (tagUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                tagUsers.Add(travelingRecord.Creator);

                            if (ccUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                ccUsers.Add(travelingRecord.Creator);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = travelingRecord.Status.Status;
                   
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Travel Record with System Ref # " + travelingRecord.SystemRefNo+ "\n "
               + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Travel Record with System Ref #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Travel Record with System Ref #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, travelingRecord.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + travelingRecord.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, travelingRecord.Id, 25, frmInputBox.comment);                
                
                DXMessageBox.Show("Updated Successfully!");
                UsersRepo.Add(TransactionInfo.Initialized, travelingRecord.Id, (int)TransactionItemType.TravelingRecord, "");
                Window myWin = Window.GetWindow(this);
                myWin.Close();
            }

           
        }

        private void LoadCompanies()
        {
            CompanyRepo repo = new CompanyRepo();
            lookupCompany.ItemsSource = repo.GetUserAdminBillCompanies(SYSTEM_STATIC.currentUser.id);
        }

       

        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }

        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var department = lookupDepartment.SelectedItem as Department;

            //if (department != null)
            //{
            //    lookupEmployee.ItemsSource = department.employees.Where(x => x.isActive == true).ToList();
            //}
        }

       

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
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

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == true && travelingRecord != null)
                {
                    //SalesReceipt receipt = new SalesReceipt();
                    travelingRecord = recordRepo.GetTravelingRecord(travelingRecord.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (travelingRecord != null)
                    {
                        if (travelingRecord.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Traveling Record") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Traveling Record is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    travelingRecord.isApproved = false;
                                    travelingRecord.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, travelingRecord.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                                    recordRepo.UpdateTravelingRecord(travelingRecord);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Traveling Record has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                                            if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                                                userss.Add(travelingRecord.Creator);

                                            winTagUsers win = new winTagUsers(userss, travelingRecord.Id, TransactionItemType.TravelingRecord);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                                tagUsers.Add(travelingRecord.Creator);

                                            if (ccUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                                ccUsers.Add(travelingRecord.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Traveling Record with System Ref #: " + travelingRecord.SystemRefNo + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Traveling Record UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Traveling Record are UnApproved (" + travelingRecord.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Traveling Record is UnApproved (" + travelingRecord.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Traveling Record Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Traveling Record Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (travelingRecord.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Traveling Record") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Traveling Record are Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    travelingRecord.isApproved = true;
                                    travelingRecord.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, travelingRecord.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                                    recordRepo.UpdateTravelingRecord(travelingRecord);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Traveling Record has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                                            if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                                                userss.Add(travelingRecord.Creator);

                                            winTagUsers win = new winTagUsers(userss, travelingRecord.Id, TransactionItemType.TravelingRecord);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                                tagUsers.Add(travelingRecord.Creator);

                                            if (ccUsers.Find(x => x.id == travelingRecord.creatorId) == null)
                                                ccUsers.Add(travelingRecord.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Traveling Record with System Ref #: " + travelingRecord.SystemRefNo + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Traveling Record Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Traveling Record is Approved (" + travelingRecord.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Traveling Record is Approved (" + travelingRecord.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Traveling Record Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Traveling Record Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (travelingRecord.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Traveling Record") != null) ? true : false)
                            {
                                travelingRecord.isReApproved = true;
                                travelingRecord.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, travelingRecord.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                recordRepo.UpdateTravelingRecord(travelingRecord);

                                MessageBox.Show("Traveling Record is Approved (" + travelingRecord.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Traveling Record is Approved (" + travelingRecord.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Traveling Record Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Traveling Record Directly user id=(" + MainWindow.currentUserid + ")");
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


        TravelingStatus oldStatus = new TravelingStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static TravelingStatus statusChanged = new TravelingStatus();
        public ucFrmTravelingRecord(TravelingStatus travelingStatus)
        {
            statusChanged = travelingStatus;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();

            if (travelingRecord != null && recordId > 0)
            {
                var previous_status = travelingRecord.Status.Status;
                if (travelingRecord != null)
                {
                    if (travelingRecord.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmTravelingStatusChange ucFrmDirectClose = new frmTravelingStatusChange();
                    if (travelingRecord.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = travelingRecord.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(travelingRecord.Status.backcolor);
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

                        var res = MessageBox.Show("Traveling Record has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                                if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                                    userss.Add(travelingRecord.Creator);


                                winTagUsers win = new winTagUsers(userss, travelingRecord.Id, TransactionItemType.TravelingRecord);
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


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Traveling Record without Approval") != null)
                        {

                            travelingRecord.PendingForClosing = false;
                            travelingRecord.stage = TransactionStage.Closed.ToString();
                            travelingRecord.statusId = statusChanged.Id;
                            travelingRecord.LastStatusChangeDate = System.DateTime.Now;
                            travelingRecord.ClosingDate = System.DateTime.Now;



                            recordRepo.UpdateTravelingRecord(travelingRecord);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Traveling Record having system ref #: " + travelingRecord.SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (travelingRecord != null)
                                {
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            travelingRecord.PendingForClosing = true;
                            travelingRecord.stage = TransactionStage.AwaitingApproval.ToString();
                            travelingRecord.statusId = statusChanged.Id;
                            travelingRecord.LastStatusChangeDate = System.DateTime.Now;
                            travelingRecord.ClosingDate = System.DateTime.Now;



                            recordRepo.UpdateTravelingRecord(travelingRecord);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Traveling Record having system ref #: " + travelingRecord.SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (travelingRecord != null)
                                {
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Records #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && travelingRecord != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();

                if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                    if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                        userss.Add(travelingRecord.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.TravelingRecord);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (travelingRecord != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && travelingRecord.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }


                            //foreach (var user in frmInputBox.Comment.TaggedList)
                            //{
                            //    if (frmInputBox.FlagForTag == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + TravelingRecord.SalesReferenceNo, TravelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + TravelingRecord.SalesReferenceNo, TravelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", null);

                            //}
                            //foreach (var user in frmInputBox.Comment.CCUsersList)
                            //{
                            //    if (frmInputBox.FlagForCC == true)
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + TravelingRecord.SalesReferenceNo, TravelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            //    else
                            //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + TravelingRecord.SalesReferenceNo, TravelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //}
                        }

                        //procurementRepo.Add(TravelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (travelingRecord.Id == 0)
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
                if (travelingRecord != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(travelingRecord.Id, TransactionItemType.TravelingRecord);

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
                if (travelingRecord.Id != 0)
                {
                    //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    //{
                    //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Bill);
                    //    inputBox.ShowDialog();
                    //}
                    //else 
                    if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                    {
                        var userss = UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id);

                        if (userss.Find(x => x.id == travelingRecord.creatorId) == null)
                            userss.Add(travelingRecord.Creator);

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, comment, TransactionItemType.TravelingRecord);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (travelingRecord != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && travelingRecord.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.travelingRecord, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.travelingRecord, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.travelingRecord, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.travelingRecord, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.travelingRecord, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (travelingRecord.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Traveling Record first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
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
                        else if (str.Contains("TravelingRecord"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.TravelingRecord);
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

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if(grdVisitingRecords.SelectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Traveling Record") != null)
                {
                    if (grdAttach.Visibility == Visibility.Visible)
                        grdAttach.Visibility = Visibility.Collapsed;
                    else
                    {
                        grdAttach.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to Attach File!");
                }
            }
            
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            var visitingCountry = grdVisitingRecords.SelectedItem as VisitingCountry;

            if(visitingCountry != null)
            {
                var Idd = visitingCountry.Id;
                string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
                string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
                if (cmbCategory1.SelectedItem != null)
                {
                    if (Idd != 0)
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
                            destination += "Attachments\\TravelingRecord\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += Idd + "_" + TransactionItemType.TravelingRecord.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.TravelingRecord);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), Idd, TransactionItemType.TravelingRecord, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, travelingRecord.Id, (int)TransactionItemType.TravelingRecord, "Added a new attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
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

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            var visitingCountry = grdVisitingRecords.SelectedItem as VisitingCountry;

            if (visitingCountry != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(visitingCountry.Id, TransactionItemType.TravelingRecord);
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (travelingRecord.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Traveling Record") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Traveling Record! Do you want to remove it from Void?", "Remove Void Traveling Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    travelingRecord.isVoid = false;
                    recordRepo.UpdateTravelingRecord(travelingRecord);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Traveling Record has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        
                        if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id), travelingRecord.Id, TransactionItemType.TravelingRecord);
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

                   
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Traveling Record with System Ref #: " + travelingRecord.SystemRefNo + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Traveling Record UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Traveling Record") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Traveling Record! Do you want to move it to Void Traveling Record?", "Add to Void Traveling Records", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    travelingRecord.isVoid = true;
                    recordRepo.UpdateTravelingRecord(travelingRecord);

                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Traveling Record has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                       

                        if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id), travelingRecord.Id, TransactionItemType.TravelingRecord);
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

                  
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Traveling Record with System Ref #: " + travelingRecord.SystemRefNo + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Traveling Record Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, 0, user.id, "New Comment ", null);
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
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveTravelingRecordAttachmentCategories();
                //var pos = System.Windows.Input.Mouse.GetPosition(this);
                //grdAttach1.TranslatePoint(pos, grdVisitingRecords);
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupCompany.SelectedItem as Company;
               
                if (company != null)
                    if (company.departments != null)
                    {
                        List<Department> departments = new List<Department>();
                        //departments = SYSTEM_STATIC.currentUser.employee.departments /*companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id)*/;
                        foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsTravelingRecordType == true))
                        {
                            if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                                departments.Add(_dept);
                        }
                        if (travelingRecord != null && travelingRecord.Id > 0 && editFlag == true)
                            if (travelingRecord.department != null && departments.FirstOrDefault(x => x.Id == travelingRecord.deptId) == null)
                                departments.Add(travelingRecord.department);

                        lookupDepartment.ItemsSource = departments;

                        if (departments.Count == 0)
                        {
                            MessageBox.Show("This company dosen't contain any department mapped with the current User");
                        }
                    }
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message + "Invalid Company");
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == false)
            {
                DXMessageBox.Show("Please Save before adding new records!");
                return;
            }

            if(lookupResidentCountry.SelectedIndex < 0)
            {
                DXMessageBox.Show("Resident Country should be selected!");
                lookupResidentCountry.Focus();
                return;
            }

            DateTime? lastArrivalDate = null;
            if(visitingCountries != null && visitingCountries.Count > 0)
            {
                var groupIds = visitingCountries.Select(item => item.transactionGroupId).ToList();
                groupIds = groupIds.Distinct().ToList();

                foreach (var groupId in groupIds)
                {
                    if (visitingCountries.FirstOrDefault(x => x.transactionGroupId == groupId && x.End == true) == null)
                    {
                        DXMessageBox.Show("Itinerary having Id: " + groupId + " is not ended yet!");
                        return;
                    }
                }

                var lastVisit = visitingCountries.Where(x=>x.transactionGroupId == groupIds.Max()).OrderByDescending(q => q.sequence).FirstOrDefault();
                if(lastVisit.ArrivalDate == null)
                {
                    DXMessageBox.Show("Arrival date in Visit having Itinerary # " + lastVisit.transactionGroupId + " and "+ " Sequence # " + lastVisit.sequence + " is not selected!");
                    return;
                }
                lastArrivalDate = lastVisit.ArrivalDate;
            }
            

            ucVisitingRecord visitingRecord = new ucVisitingRecord();

            visitingRecord.groupId = 0;
            visitingRecord.recordId = recordId;
            visitingRecord.editFlag = false;
            visitingRecord.lastArrivalDate = lastArrivalDate;
            visitingRecord.residentCountry = lookupResidentCountry.SelectedItem as ResidentCountry;

            DXWindow window = new DXWindow();
            window.Title = "Staying Record";
            window.Content = visitingRecord;
            window.Height = 400;
            window.Width = 700;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();

            RefreshData();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lookupResidentCountry.SelectedIndex < 0)
            {
                DXMessageBox.Show("Resident Country should be selected!");
                lookupResidentCountry.Focus();
                return;
            }

            var row = grdVisitingRecords.SelectedItem as VisitingCountry;
            if(row != null)
            {
                ucVisitingRecord visitingRecord = new ucVisitingRecord();

                visitingRecord.groupId = row.transactionGroupId;
                visitingRecord.editFlag = true;
                visitingRecord.recordId = recordId;
                visitingRecord.residentCountry = lookupResidentCountry.SelectedItem as ResidentCountry;

                DXWindow window = new DXWindow();
                window.Title = "Staying Record";
                window.Content = visitingRecord;
                window.Height = 400;
                window.Width = 700;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                RefreshData();
            }
        }

        private void DatResidentFromDate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupTravelerName.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Traveler!");
                lookupTravelerName.Focus();
                return;
            }
        }

        private void DatResidentToDate_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupTravelerName.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Traveler!");
                lookupTravelerName.Focus();
                return;
            }
        }

        private void DatResidentFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datResidentToDate.SelectedDate != null)
            {
                if (datResidentToDate.SelectedDate < datResidentFromDate.SelectedDate)
                {
                    DXMessageBox.Show("From Date cannot be greater than To Date!");
                    datResidentFromDate.SelectedDate = null;
                    return;
                }

                var residentCountries = (lookupTravelerName.SelectedItem as Traveler).ResidentCountries;

                foreach(var _country in residentCountries)
                {
                    if (_country != null)
                    {
                        if (_country.fromDate <= datResidentFromDate.SelectedDate && _country.toDate >= datResidentToDate.SelectedDate)
                        {
                            lookupResidentCountry.EditValue = _country.Id;
                            return;
                        }
                    }
                }
                lookupResidentCountry.Text = null;

            }
        }

        private void DatResidentToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datResidentFromDate.SelectedDate != null)
            {
                if (datResidentToDate.SelectedDate < datResidentFromDate.SelectedDate)
                {
                    DXMessageBox.Show("From Date cannot be greater than To Date!");
                    datResidentToDate.SelectedDate = null;
                    return;
                }

                var residentCountries = (lookupTravelerName.SelectedItem as Traveler).ResidentCountries;

                foreach (var _country in residentCountries)
                {
                    if (_country != null)
                    {
                        if(_country.fromDate <= datResidentFromDate.SelectedDate && _country.toDate >= datResidentToDate.SelectedDate)
                        {
                            lookupResidentCountry.EditValue = _country.Id;
                            return;
                        }
                    }
                }
                lookupResidentCountry.Text =  null;
            }
        }

        private void LookupTravelerName_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var residentCountries = (lookupTravelerName.SelectedItem as Traveler).ResidentCountries;
            lookupResidentCountry.ItemsSource = residentCountries;
        }

        private void GrdVisitingRecords_Loaded(object sender, RoutedEventArgs e)
        {
           
            
        }

        int itineraryId = 0;
        private void GrdVisitingRecords_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var visit = grdVisitingRecords.GetRowByListIndex(e.ListSourceRowIndex) as VisitingCountry;

            switch (e.Column.FieldName)
            {
                case "NumberOfDays":
                    if (visit != null && visit.DepartureDate != null && visit.LeavingDate != null)
                        e.Value = (visit.LeavingDate.Value - visit.DepartureDate.Value).TotalDays;
                    break;

                case "TicketCostt":
                    //if (visit.transactionGroupId != itineraryId)
                    //{
                    //    itineraryId = visit.transactionGroupId;
                        e.Value =  "Ticket Cost =" + visit.TicketCost + ",  MER =" + visit.MER + ",  Cost(MER) =" + visit.TicketCostMER;
                    //}
                    break;
                //case "MERR":
                //    if (visit.transactionGroupId != itineraryId)
                //    {
                //        itineraryId = visit.transactionGroupId;
                //        e.Value = visit.MER;
                //    }
                //    break;
                //case "CostMERR":
                //    if (visit.transactionGroupId != itineraryId)
                //    {
                //        itineraryId = visit.transactionGroupId;
                //        e.Value = visit.TicketCostMER;
                //    }
                //    break;
            }
        }
       
        private void GrdVisitingRecords_ColumnsPopulated(object sender, RoutedEventArgs e)
        {
            var colAttach = grdVisitingRecords.Columns["AttachNew"];
            colAttach.CellTemplate = (DataTemplate)this.Resources["AttachmentButton"];
            colAttach.Width = 70;
            colAttach.Name = "AttachNew";

            var colAttachList = grdVisitingRecords.Columns["AttachmentList"];
            colAttachList.CellTemplate = (DataTemplate)this.Resources["AttachmentListButton"];
            colAttachList.Width = 70;
            colAttachList.Name = "AttachmentList";
        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
        }

        private void BtnCloseAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
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
                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                //{
                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.TravelingRecord);
                //    inputBox.editFlag = true;
                //    frmInputBox.Comment = comment;
                //    inputBox.ShowDialog();
                //}
                //else 
                if (travelingRecord.department != null && travelingRecord.department.Id != 0 && travelingRecord.company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(travelingRecord.department.Id, travelingRecord.company.Id), TransactionItemType.TravelingRecord);
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

                if (frmInputBox.commentAdded == true && travelingRecord.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (travelingRecord.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Traveling Record first to add a comment!");
                }
            }
            loadcomments();
        }

        private void datResidentYear_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var residentYear = datResidentYear.DateTime;
            int count = grdVisitingRecords.VisibleItems.Count;
            for (int i = 0; i < count; i++)
            {
                grdVisitingRecords.SetCellValue(i, grdVisitingRecords.Columns["travelingRecord.residentYear"], residentYear);
            }
        }
    }
}
