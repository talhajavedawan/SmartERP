using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
using ERP_BL.Databases;
using ERP_BL.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZAS_ERP.Bankings.Loans.UserControls;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.Bankings.Loans
{
    /// <summary>
    /// Interaction logic for ucFrmLoans.xaml
    /// </summary>
    public partial class ucFrmLoans : UserControl
    {//comment
        public virtual List<FacilityNature> facilityNatures { get; set; }
        LoansRepo loansRepo = new LoansRepo();
        List<ERP_BL.Bankings.Loans> loans = new List<ERP_BL.Bankings.Loans>();
        List<ERP_BL.Bankings.Loans> removedLoans = new List<ERP_BL.Bankings.Loans>();
        public int groupId = 0;
        UsersRepo usersRepo = new UsersRepo();
        User loginuser = new User();
        CompanyRepo companyRepo = new CompanyRepo();
        public bool editFlag = false;
        int intGroupId = 0;

        string stage;
        bool? isApproved;
        //bool? isReApproved;
        DateTime? approvalDate;

        static LoansStatus statusChanged = new LoansStatus();
        List<ViewInfo> views = new List<ViewInfo>();
        UsersRepo UsersRepo = new UsersRepo();
        List<cmbitem> cmbitems = new List<cmbitem>();
        List<LoansStatus> loansStatuses = new List<LoansStatus>();
        public ucFrmLoans()
        {
            InitializeComponent();
        }
        //comment

        public ucFrmLoans(LoansStatus loansStatus)
        {
            statusChanged = loansStatus;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == false)
            {
                GroupIdCalculation();
                txtSystemRef.Text = "Loans-" + intGroupId;
            }

            if(lookupCompany.SelectedIndex < 0)
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
            if (lookupBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank!");
                lookupBank.Focus();
                return;
            }
            if (lookupAccount.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Account!");
                lookupAccount.Focus();
                return;
            }
            if (cmbCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Currency!");
                cmbCurrency.Focus();
                return;
            }

            if (cmbLoansStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbLoansStatus.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtFinanceRef.Text))
            {
                DXMessageBox.Show("Please enter Finance Ref #!");
                txtFinanceRef.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtSystemRef.Text))
            {
                DXMessageBox.Show("Please enter System Ref #!");
                txtSystemRef.Focus();
                return;
            }
            if (datLimitDate.EditValue == null)
            {
                DXMessageBox.Show("Please enter Limit Date!");
                datLimitDate.Focus();
                return;
            }
            if (datExpiryDate.EditValue == null)
            {
                DXMessageBox.Show("Please enter Expiry Date!");
                datExpiryDate.Focus();
                return;
            }
            if (datExtensionDate.EditValue == null)
            {
                DXMessageBox.Show("Please enter Extension Date!");
                datExtensionDate.Focus();
                return;
            }

            if (editFlag == true && loans.Count > 0)
            {
                //intGroupId = loans[0].transactionGroupId;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null && loans[0].isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Loan is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        stage = TransactionStage.Approved.ToString();
                        isApproved = true;
                        approvalDate = System.DateTime.Now;
                    }
                }
            }

            //int index = 0;
            foreach (var _item in lstBoxFacilityNature.Items)
            {
                var uc = _item as ucLoansRow;
                if(uc.lookupMainLimitNature.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Facility Nature in Row "+(lstBoxFacilityNature.Items.IndexOf(_item)+1));
                    uc.lookupMainLimitNature.Focus();
                    return;
                }
                if(Convert.ToDouble(uc.txtMainLimitAmount.Text) == 0)
                {
                    DXMessageBox.Show("Please enter Amount in Row "+ (lstBoxFacilityNature.Items.IndexOf(_item) + 1));
                    uc.txtMainLimitAmount.Focus();
                    return;
                }
                ERP_BL.Bankings.Loans _loan = new ERP_BL.Bankings.Loans();

                if (editFlag == true)
                {
                    if(Convert.ToInt32(uc.txtId.Text) > 0)
                        _loan = loans[loans.IndexOf(loans.FirstOrDefault(x => x.Id == Convert.ToInt32(uc.txtId.Text)))];
                }
                

                _loan.CreationDate = datCreationDate.DateTime;
                _loan.CompanyId = (lookupCompany.SelectedItem as Company).Id;
                _loan.deptId = (lookupDepartment.SelectedItem as Department).Id;
                _loan.bankId = (lookupBank.SelectedItem as Bank).Id;
                _loan.accountId = (lookupAccount.SelectedItem as Account).Id;
                _loan.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                _loan.statusId = (cmbLoansStatus.SelectedItem as cmbitem).id;
                _loan.MainLimitNatureId = (uc.lookupMainLimitNature.SelectedItem as FacilityNature).Id;
                _loan.MainLimitAmount = Convert.ToDouble(uc.txtMainLimitAmount.Text);
                
                _loan.SystemRefNo = txtSystemRef.Text;
                _loan.FinanceRefNo = txtFinanceRef.Text;

                _loan.ExpiryDate = datExpiryDate.DateTime;
                _loan.LimitDate = datLimitDate.DateTime;
                _loan.ExtensionDate = datExtensionDate.DateTime;






                if (editFlag == true)
                {
                    _loan.transactionGroupId = groupId;
                    _loan.stage = stage;
                    _loan.isApproved = isApproved;
                    _loan.ApprovedDate = approvalDate;

                    if (Convert.ToInt32(uc.txtId.Text) > 0)
                        loans[loans.IndexOf(loans.FirstOrDefault(x => x.Id == Convert.ToInt32(uc.txtId.Text)))] = _loan;
                    else
                    {
                        loans.Add(_loan);
                    }
                    //index++;
                }
                else
                {
                    _loan.transactionGroupId = intGroupId;
                    _loan.creatorId = SYSTEM_STATIC.currentUser.employeeId;
                    _loan.user_Id = SYSTEM_STATIC.currentUser.id;
                    _loan.isApproved = false;
                    loans.Add(_loan);
                }
            }

            if(editFlag == false)
            {
                loansRepo.AddLoans(loans);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                loansRepo.Update(loans, removedLoans);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void loadLoansStatus()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment Statuses") != null)
                loansStatuses = loansRepo.GetAllLoansStatuses();
            else
                loansStatuses = loansRepo.GetAllLoansStatuses().Where(x => x.isActive == true).ToList();
            loansStatuses = loansStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(loansStatuses, delegate (LoansStatus status) 
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbLoansStatus.ItemsSource = cmbitems;
        }
        private void loadLoansStatus(LoansStatus _loansStatus)
        {
            loansStatuses.Add(_loansStatus);
            Parallel.ForEach(loansStatuses, delegate (LoansStatus status)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbLoansStatus.ItemsSource = cmbitems;
        }

        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = loansRepo.GetLastPaymentId();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName;
                AddRow();
            }
            loginuser = usersRepo.getuserForTenant(SYSTEM_STATIC.currentUser.id);
            loadLoansStatus();
            LoadCompanies();
            LoadCurrencies();

            if(editFlag == true)
            {
                views = UsersRepo.getViwerInfo(groupId, (int)TransactionItemType.Loans);
                grdUsers.ItemsSource = views;
                loadcomments();
                LoadAttachments();

                loans = loansRepo.GetAllLoansByGroupId(groupId);

                if (loans[0].Creator != null)
                    txtCreator.Text = loans[0].Creator.person.FName;

                if (loans[0].FinanceRefNo != null)
                    lblLoansRefNo.Text = "("+ loans[0].FinanceRefNo+")";

                if (loans[0].isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    lblStage.Text = "Void";
                }
                else if (loans[0].isReApproved == false)
                {
                    lblStage.Text = "Under Re-Approval";
                }
                else if (loans[0].isApproved == true && loans[0].stage == "Closed")
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (loans[0].isApproved == true && loans[0].Status.isActive == false && loans[0].PendingForClosing != true)
                {
                    lblStage.Text = "Approved and Closed";
                }
                else if (loans[0].isApproved == true && loans[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }
                else if (loans[0].isApproved == true)
                {
                    lblStage.Text = "Approved";
                }
                else if (loans[0].isApproved == false)
                {
                    lblStage.Text = "Under Approval";
                }
                else if (loans[0].PendingForClosing == true)
                {
                    lblStage.Text = "Under Closing Approval";
                }



                if (loans[0].company != null)
                {
                    lookupCompany.Text = loans[0].company.CompanyName;
                }

                if (loans[0].department != null)
                {
                    lookupDepartment.Text = loans[0].department.DeptName;
                }

                if (loans[0].bank != null)
                {
                    lookupBank.Text = loans[0].bank.BankName;
                }

                if (loans[0].account != null)
                {
                    lookupAccount.Text = loans[0].account.AccountNo;
                }

                if (loans[0].SystemRefNo != null)
                {
                    txtSystemRef.Text = loans[0].SystemRefNo;
                }

                if (loans[0].FinanceRefNo != null)
                {
                    txtFinanceRef.Text = loans[0].FinanceRefNo;
                }

                //Select Currency
                var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                if (loans[0].currency != null)
                {
                    int index = 0;
                    foreach (var _currency in currencyList)
                    {
                        if (_currency.id == loans[0].currency.Id)
                        {
                            cmbCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                if (loans[0].Status != null)
                {
                    var disAbleStatus = loansStatuses.FirstOrDefault(x => x.Id == loans[0].Status.Id);
                    if (disAbleStatus == null)
                    {
                        loadLoansStatus(loans[0].Status);
                    }
                }
                //Select Currency
                var statusList = (cmbLoansStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbLoansStatus.ItemsSource as List<cmbitem>;
                if (loans[0].Status != null)
                {

                    int index = 0;
                    foreach (var _status in statusList)
                    {

                        if (_status.id == loans[0].Status.Id)
                        {
                            cmbLoansStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }

                }

                if (loans[0].CreationDate != null)
                {
                    datCreationDate.DateTime = loans[0].CreationDate;
                }

                if (loans[0].LimitDate != null)
                {
                    datLimitDate.DateTime = loans[0].LimitDate.Value;
                }

                if (loans[0].ExpiryDate != null)
                {
                    datExpiryDate.DateTime = loans[0].ExpiryDate.Value;
                }

                if (loans[0].ExtensionDate != null)
                {
                    datExtensionDate.DateTime = loans[0].ExtensionDate.Value;
                }

                foreach (var _loan in loans)
                {
                    AddRow();
                    var _item = lstBoxFacilityNature.Items[lstBoxFacilityNature.Items.Count - 1] as ucLoansRow;

                    _item.txtId.Text = _loan.Id.ToString();
                    if(_loan.MainLimitFacilityNature != null)
                    {
                        _item.lookupMainLimitNature.Text = _loan.MainLimitFacilityNature.NatureName;
                    }
                    _item.txtMainLimitAmount.Text = _loan.MainLimitAmount.ToString();
             
                }

                stage = loans[0].stage;
                isApproved = loans[0].isApproved;
                approvalDate = loans[0].ApprovedDate;
            }
        }

        private void LoadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        private void LoadCompanies()
        {
            lookupCompany.ItemsSource = companyRepo.GetUserCompaniesForChartofAccount(SYSTEM_STATIC.currentUser.id);
        }

        private void BtnAddMore_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            //lstBoxFacilityNature.Items.Remove(lstBoxFacilityNature.SelectedItem);
            var row = (UserControl)lstBoxFacilityNature.SelectedItem;
            if (row != null)
            {
                if (editFlag == true)
                {
                    var loan = (ucLoansRow)row;

                    if (!String.IsNullOrEmpty(loan.txtId.Text))
                    {
                        removedLoans.Add(loans.FirstOrDefault(x => x.Id == Convert.ToInt32(loan.txtId.Text)));
                        loans.Remove(loans.FirstOrDefault(x => x.Id == Convert.ToInt32(loan.txtId.Text)));
                    }

                    lstBoxFacilityNature.Items.Remove(lstBoxFacilityNature.SelectedItem);
                }
                else
                {
                    lstBoxFacilityNature.Items.Remove(lstBoxFacilityNature.SelectedItem);
                }
            }
            else
            {
                DXMessageBox.Show("Please select a Loan to delete!!");
                return;
            }
        }

        void AddRow()
        {
            ucLoansRow uc = new ucLoansRow();
            uc.HorizontalAlignment = HorizontalAlignment.Stretch;
            uc.Name = "ucFacilityNatureRow";
            lstBoxFacilityNature.Items.Add(uc);
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex > -1)
            {
                var selectedCompany = lookupCompany.SelectedItem as Company;
                var departments = selectedCompany.departments;
                List<Department> allowedDepts = new List<Department>();
                foreach (var _dept in departments)
                {
                    if (_dept.isActive == true)
                    {
                        List<int> empIds = new List<int>();
                        foreach (var emp in _dept.employees)
                        {
                            empIds.Add(emp.EmpId);
                        }
                        if (empIds.Contains(loginuser.employeeId))
                            allowedDepts.Add(_dept);
                    }
                }
                lookupDepartment.ItemsSource = allowedDepts;
            }
        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var selectedCompany = lookupCompany.SelectedItem as Company;
            SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
            var bankList = receiptRepo.GetAllBanksByCompany(selectedCompany.Id);
            lookupBank.ItemsSource = bankList;
        }

        private void LookupBank_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupBank.SelectedItem != null)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var bank = lookupBank.SelectedItem as Bank;
                var accntList = receiptRepo.GetAccountsForLaonsByBankId(bank.Id);
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains((lookupDepartment.SelectedItem as Department).Id) && _account.bank.Id == bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                lookupAccount.ItemsSource = allowedAccounts;
            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                loans = loansRepo.GetAllLoansByGroupId(groupId);

                if (loans != null && loans.Count > 0)
                {
                    if (loans[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Loans") != null))
                    {
                        if (DXMessageBox.Show("This Transaction is currently in the list of Void Loans! Do you want to remove it from Void?", "Remove Void Loans", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            loansRepo.SetLoansToVoid(loans, false);
                            grdVoid.Visibility = Visibility.Collapsed;

                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res1 = MessageBox.Show("Loans has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (loans[0].department != null && loans[0].department.Id != 0 && loans[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loans[0].department.Id, loans[0].company.Id), loans[0].transactionGroupId, TransactionItemType.Loans);
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
                                Comment = "Loans having System Ref #: " + loans[0].SystemRefNo + " has been marked as Unvoid",
                                Timestamp = DateTime.Now,
                                Subject = "Loans UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].FinanceRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].FinanceRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Loans") != null)
                    {
                        if (DXMessageBox.Show("This Transaction is not currently in the list of Void Loans! Do you want to move it to Void Loans?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            loansRepo.SetLoansToVoid(loans,true);
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

                            var res1 = MessageBox.Show("Loans has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (loans[0].department != null && loans[0].department.Id != 0 && loans[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loans[0].department.Id, loans[0].company.Id), loans[0].transactionGroupId, TransactionItemType.Loans);
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
                                Comment = "Loans having System Ref #: " + loans[0].SystemRefNo + " has been marked as void",
                                Timestamp = DateTime.Now,
                                Subject = "Loans Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].FinanceRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].FinanceRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to Mark or UnMark a Loans to Void!");
                    }
                }

            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Loans") != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Attached Files!");
            }
        }

        public void LoadAttachments()
        {
            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Loans);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Loans") != null)
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

        public void loadcomments()
        {
            try
            {
                if (groupId != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(groupId, TransactionItemType.Loans);
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

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;

            if (department != null && department.Id != 0 && company?.Id != 0)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Loans);
                inputBox.ShowDialog();

            }

            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (loans != null && loans.Count > 0)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && loans[0].transactionGroupId != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (loans[0].transactionGroupId == 0)
                {
                    DXMessageBox.Show("Kindly save Loans first to add a comment!");
                }

            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ProcurementRepo procurementRepo = new ProcurementRepo();
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (editFlag == true && groupId > 0)
            {
                string previous_status;
                loans = loansRepo.GetAllLoansByGroupId(groupId);


                if (loans != null && loans.Count > 0)
                {
                    previous_status = loans[0].Status.Status;

                    if (loans[0].isApproved == false)
                    {
                        DXMessageBox.Show("Loans are pending for approval!");
                        return;
                    }
                    statusChanged = null;
                    ucFrmLoanDirectClose ucFrmDirectClose = new ucFrmLoanDirectClose();
                    if (loans[0].Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = loans[0].Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(loans[0].Status.backcolor);
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
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Loans has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (loans[0].department != null && loans[0].department.Id != 0 && loans[0].company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment(loans[0].department.Id, loans[0].company.Id), loans[0].transactionGroupId, TransactionItemType.Loans);
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Loans without Approval") != null)
                        {
                            for (int i = 0; i < loans.Count; i++)
                            {
                                loans[i].PendingForClosing = false;
                                loans[i].stage = TransactionStage.Closed.ToString();
                                loans[i].statusId = statusChanged.Id;
                                loans[i].LastStatusChangeDate = System.DateTime.Now;
                                loans[i].ClosingDate = System.DateTime.Now;


                            }
                            loansRepo.UpdateLoans(loans);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Loans, frmInputBox.comment);


                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Loans having system ref #: " + loans[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            if (loans.Count != 0)
                            {
                                procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ",null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            }

                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < loans.Count; i++)
                            {
                                loans[i].PendingForClosing = true;
                                loans[i].stage = TransactionStage.AwaitingApproval.ToString();
                                loans[i].statusId = statusChanged.Id;
                                loans[i].LastStatusChangeDate = System.DateTime.Now;
                                loans[i].ClosingDate = System.DateTime.Now;


                            }
                            loansRepo.UpdateLoans(loans);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Loans, frmInputBox.comment);

                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Loans having system ref #: " + loans[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat ,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            if (loans.Count != 0)
                            {
                                procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
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

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (editFlag == true && loans != null && loans.Count > 0)
                {
                    //SalesReceipt receipt = new SalesReceipt();
                    var loansForApproval = loansRepo.GetAllLoansByGroupId(loans[0].transactionGroupId);

                    if (loansForApproval != null && loansForApproval.Count > 0)
                    {
                        if (loansForApproval[0].isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Loans are Approved, Do you want to UnApprove these Loans?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < loansForApproval.Count; i++)
                                    {
                                        loansForApproval[i].isApproved = false;
                                        loansForApproval[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    }
                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, loans[0].transactionGroupId, (int)TransactionItemType.Loans, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                    loansRepo.UpdateLoans(loansForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>(); 
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Loans has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (loansForApproval[0].department != null && loansForApproval[0].department.Id != 0 && loansForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loansForApproval[0].department.Id, loansForApproval[0].company.Id), loansForApproval[0].transactionGroupId, TransactionItemType.Loans);
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
                                    if (loansForApproval[0].currency != null)
                                    {
                                        symbolCurr = loansForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Loans having System Ref: " + loansForApproval[0].SystemRefNo + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Loans UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loansForApproval[0].FinanceRefNo, loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loansForApproval[0].FinanceRefNo, loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Loans are UnApproved (" + loans[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Loans are UnApproved (" + loans[0].transactionGroupId + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loansForApproval[0].isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Loans are Pending for Approval, Do you want to Approve these Loans?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < loansForApproval.Count; i++)
                                    {
                                        loansForApproval[i].isApproved = true;
                                        loansForApproval[i].stage = TransactionStage.Approved.ToString();
                                    }
                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, loans[0].transactionGroupId, 17, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                    loansRepo.UpdateLoans(loansForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>(); 
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Loans has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (loansForApproval[0].department != null && loansForApproval[0].department.Id != 0 && loansForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loansForApproval[0].department.Id, loansForApproval[0].company.Id), loansForApproval[0].transactionGroupId, TransactionItemType.Loans);
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
                                    if (loansForApproval[0].currency != null)
                                    {
                                        symbolCurr = loansForApproval[0].currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Loans having System Ref #: " + loansForApproval[0].SystemRefNo.ToString() +  " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Loans Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loansForApproval[0].FinanceRefNo, loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }
                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans #" + loansForApproval[0].FinanceRefNo, loansForApproval[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Loans are Approved (" + loans[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Loans are Approved (" + loans[0].transactionGroupId + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }
                        }
                        else if (loans[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Loans") != null) ? true : false)
                            {
                                for (int i = 0; i < loans.Count; i++)
                                {
                                    loansForApproval[i].isReApproved = true;
                                    loansForApproval[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, loans[0].transactionGroupId, (int)TransactionItemType.Loans, frmInputBox.comment);
                                loansRepo.UpdateLoans(loans);                                
                                MessageBox.Show("Loans are Approved (" + loans[0].transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans are Approved (" + loans[0].transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans Directly user id=(" + MainWindow.currentUserid + ")");
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

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

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
                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        //Loader.DeferedVisibility = true;

                        //Percent = LoaderClass.loaderValue;
                        var result = attachment.startDownload(str, TransactionItemType.Loans);
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
                            //this.Dispatcher.Invoke(() =>
                            //{
                            DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            //});
                        }
                        //grdProgressBar.Visibility = Visibility.Collapsed;
                    });
                    thread.Start();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (groupId != 0)
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
                        destination += "Attachments\\Loans\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += groupId + "_" + TransactionItemType.Loans.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Loans);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), groupId, TransactionItemType.Loans, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, groupId, 17, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(groupId, TransactionItemType.Loans);
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
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
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
            if (groupId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, groupId, (int)TransactionItemType.Loans, "Viewed details of Loans");
            }
        }
    }
}
