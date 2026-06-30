using DevExpress.Xpf.Core;
using ERP_BL.CashBook;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.Windows;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ERP_BL.Payments;
using ZAS_ERP.SaleOrderFolder.UserControls;

namespace ZAS_ERP.Procurementss.LoanAdvance.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmLoansAdvances.xaml
    /// </summary>
    public partial class ucFrmLoansAdvances : UserControl
    {
        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
        LoansAdvance loansAdvance = new LoansAdvance();
        public int loansAdvanceId = 0;
        public bool editFlag = false;
        public LoansAdvanceStatus checkStatus = new LoansAdvanceStatus();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        int saleInvoiceId = 0;
        TransactionItemType type=TransactionItemType.UnDefined;
        public ucFrmLoansAdvances()
        {
            InitializeComponent();
        }
        public ucFrmLoansAdvances(TransactionItemType _type, int _saleInvoiceId)
        {
            InitializeComponent();
            type = _type;
            saleInvoiceId = _saleInvoiceId;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompanies();
            LoadApplicantTypes();
            LoadStatuses();
            loadCurrencies();
            LoadTypes();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit MER in Loans Advances") != null)
                txtexchangerate.IsReadOnly = false;
            else
                txtexchangerate.IsReadOnly = true;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Loans and Advances") == null)
                datCreationDate.IsEnabled = false;
            else
                datCreationDate.IsEnabled = true;

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + ' ' + SYSTEM_STATIC.currentUser.employee.person.LName;
                cmbType.SelectedIndex = 0;
            }
            if (editFlag == true && loansAdvanceId > 0)
            {
                loansAdvance = loansAdvanceRepo.GetLoansAdvance(loansAdvanceId);
                views = UsersRepo.getViwerInfo(loansAdvanceId, 25);
                grdUsers.ItemsSource = views;
                loadcomments();
                cmbType.SelectedIndex = 0;

                cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances") == null)
                {
                    btnSave.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loan Approved Amount") != null)
                {
                    txtApprovedAmountOC.IsReadOnly = false;
                }
                LoanAdvanceStages();

                DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0,0);
                var date = loansAdvance.LoanReturnDate.Value;
                DateTime returnDate = new DateTime(date.Year, date.Month, date.Day, 0, 0 ,0);
                var timeSpan = currDate.Subtract(returnDate);
                txtAgingDays.Text = timeSpan.Days.ToString();

                if (loansAdvance.CreationDate != null)
                    datCreationDate.DateTime = loansAdvance.CreationDate.Value;

                if (loansAdvance.companyId != null)
                {
                    var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                    if (loansAdvance.company != null && companyList.Find(x => x.Id == loansAdvance.companyId) == null)
                    {
                        companyList.Add(loansAdvance.company);
                        lookupCompany.ItemsSource = null;
                        lookupCompany.ItemsSource = companyList;
                    }
                    lookupCompany.EditValue = loansAdvance.companyId;
                }

                if (loansAdvance.deptId != null)
                    lookupDepartment.EditValue = loansAdvance.deptId;

                if (loansAdvance.applicantTypeId != null)
                    lookupApplicantType.EditValue = loansAdvance.applicantTypeId;

                if (loansAdvance.Creator != null)
                    txtCreator.Text = loansAdvance.Creator.employee.person.FName +" "+ loansAdvance.Creator.employee.person.LName;

                if (loansAdvance.applicant != null)
                    lookupApplicant.EditValue = loansAdvance.applicantId;

                if(loansAdvance.isEmployee == true)
                {
                    chkEmployee.IsChecked = true;
                    lookupEmployee.EditValue = loansAdvance.employeeId;
                }
                else if(loansAdvance.isEmployee == false)
                {
                    chkEmployee.IsChecked = false;
                }
                if (loansAdvance.applicantTypeId == null)
                {
                    lookupApplicantType.EditValue = loansAdvance.applicantTypeId;

                }
                if (loansAdvance.applicantId == null)
                {
                    lookupApplicant.EditValue = loansAdvance.applicantId;
                }

                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (loansAdvance.Status != null)
                {
                    int index = 0;
                    foreach (var _status in statusList)
                    {

                        if (_status != null && _status.id == loansAdvance.statusId)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                    checkStatus = loansAdvance.Status;
                }

                var currencyList = (cmbCurrency.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbCurrency.ItemsSource as List<cmbitem>;
                if (loansAdvance.currency != null)
                {
                    int index = 0;
                    foreach (var _curr in currencyList)
                    {

                        if (_curr.id == loansAdvance.currencyId)
                        {
                            cmbCurrency.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                txtAppliedAmountOC.Text = loansAdvance.AppliedAmountOC.ToString();
                txtApprovedAmountOC.Text = loansAdvance.LoanAmountOC.ToString();
                txtLoanTenure.Text = loansAdvance.LoanTenureDays.ToString();
                txtPurpose.Text = loansAdvance.Purpose;
                txtSystemRef.Text = loansAdvance.SystemRef;
                txtexchangerate.Text = loansAdvance.MER.ToString();
                lblLoansAdvancesRefNo.Text = loansAdvance.SystemRef;

                double paidAmount = 0, receivedAmount = 0, adjustedAmount = 0;
                if (loansAdvance.Payments != null && loansAdvance.Payments.Where(x=>x.isVoid != true).ToList().Count > 0)
                {
                    paidAmount = loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                    txtTotalLoanPaid.Text = paidAmount.ToString();
                }

                if (loansAdvance.SalesReceipts != null && loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count > 0)
                {
                    receivedAmount = loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                    txtLoanReturned.Text = receivedAmount.ToString();
                }

                if (loansAdvance.AdminBills != null && loansAdvance.AdminBills.Where(x=>x.isVoid != true).ToList().Count > 0)
                {
                    foreach(var adminBill in loansAdvance.AdminBills.Where(x => x.isVoid != true))
                    {
                        adjustedAmount = adjustedAmount + adminBill.Adjustments.Where(x=>x.isApproved == true).Sum(x=>x.AdjustmentAmount);
                    }
                    txtAdjustedAmount.Text = adjustedAmount.ToString();
                }
                txtBalanceAmountOC.Text =Math.Round( paidAmount - adjustedAmount - receivedAmount, 2).ToString();

                //comment

                if (loansAdvance.LoanReturnDate != null)
                    datLoanReturnDate.DateTime = loansAdvance.LoanReturnDate.Value;

                //Select Payment Reference No
                var paymentRefList = (cmbxPettyCashRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxPettyCashRef.ItemsSource as List<cmbitem>;
                if (loansAdvance.PettyCashRefId != null)
                {

                    int index = 0;
                    foreach (var _ref in paymentRefList)
                    {

                        if (_ref.id == loansAdvance.PettyCashRefId)
                        {
                            cmbxPettyCashRef.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }

                }

                if (loansAdvance.isDeposit == true)
                    btnDeposit.IsChecked = true;
                else if (loansAdvance.isDeposit == false)
                    btnPayment.IsChecked = true;


            }
            GellAllOrdersTracking();

        }


        private void LoadTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.LoansAdvanceType.Vendor_Bill; i++)
            {
                cmbType.Items.Add(((ERP_BL.Enums.LoansAdvanceType)i).ToString());
            }
        }

        private void loadBillReferenceNo()
        {
            AdminBillsRepo BillsRepo = new AdminBillsRepo();
            List<BillRefNumber> references = new List<BillRefNumber>();

            references = BillsRepo.GetAllActiveBillReferenceNo((lookupCompany.SelectedItem as Company).Id);

            if (editFlag == true && loansAdvance != null)
            {
                if (loansAdvance.PettyCashRef != null && references.FirstOrDefault(x => x.Id == loansAdvance.PettyCashRefId) == null)
                    references.Add(loansAdvance.PettyCashRef);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (BillRefNumber _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.BillReferenceNo, id = _ref.Id });
            }

            cmbxPettyCashRef.ItemsSource = cmbitems;
        }

        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = loansAdvanceRepo.GetLastTransactionId();
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
                txtSystemRef.Text = "LoansAdvances-" + intGroupId;
                lblLoansAdvancesRefNo.Text = " (LoansAdvances-" + intGroupId + ")";
                loansAdvance.transactionGroupId = intGroupId;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please Select Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if(lookupCompany.SelectedIndex < 0)
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
            if (chkEmployee.IsChecked == true)
            {
                if(lookupEmployee.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Employee!");
                    lookupEmployee.Focus();
                    return;
                }
            }
            else
            {
                if(lookupApplicantType.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Applicant Type!");
                    lookupApplicantType.Focus();
                    return;
                }
                if (lookupApplicant.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Applicant!");
                    lookupApplicant.Focus();
                    return;
                }
            }
            if(cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Status!");
                cmbStatus.Focus();
                return;
            }
            if(cmbCurrency.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Currency!");
                cmbCurrency.Focus();
                return;
            }
            if(saleInvoiceId!=0 && type == TransactionItemType.Sale_Invoice)
            {
                loansAdvance.SaleInvoiceId = saleInvoiceId;
            }
            loansAdvance.CreationDate = datCreationDate.DateTime;
            loansAdvance.advanceTemplate = LoansAdvanceTemplate.Advance;
            loansAdvance.loansAdvanceType = LoansAdvanceType.Admin_Bill;
            loansAdvance.companyId = (lookupCompany.SelectedItem as Company).Id;
            loansAdvance.deptId = (lookupDepartment.SelectedItem as Department).Id;
            
            if(chkEmployee.IsChecked == true)
            {
                loansAdvance.isEmployee = true;
                loansAdvance.employeeId = (lookupEmployee.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                //loansAdvance.applicantTypeId = null;
                //loansAdvance.applicantId = null;
            }
            else
            {
                loansAdvance.isEmployee = false;
                loansAdvance.employeeId = null;
              
            }
            if(lookupApplicantType.SelectedIndex>-1)
            {
                loansAdvance.applicantTypeId = (lookupApplicantType.SelectedItem as LoanApplicantType).Id;
            }
            if (lookupApplicant.SelectedIndex > -1)
            {
                loansAdvance.applicantId = (lookupApplicant.SelectedItem as LoanApplicant).Id;
            }

            loansAdvance.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            loansAdvance.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
            loansAdvance.AppliedAmountOC = Convert.ToDouble(txtAppliedAmountOC.Text);
            loansAdvance.LoanAmountOC = Convert.ToDouble(txtApprovedAmountOC.Text);
            loansAdvance.MER = Convert.ToDouble(txtexchangerate.Text);
            loansAdvance.LoanAmountMER = Convert.ToDouble(txtAmoutMER.Text);
            loansAdvance.Purpose = txtPurpose.Text;
            loansAdvance.LoanTenureDays = Convert.ToInt32(txtLoanTenure.Text);
            loansAdvance.LoanReturnDate = datLoanReturnDate.DateTime;
            loansAdvance.SystemRef = txtSystemRef.Text;


            if (cmbxPettyCashRef.SelectedIndex > 0)
                loansAdvance.PettyCashRefId = (cmbxPettyCashRef.SelectedItem as cmbitem).id;
            else
                loansAdvance.PettyCashRefId = null;

            List<PettyCash> pettyCashes = new List<PettyCash>();
            if (editFlag == true && loansAdvanceId != 0)
            {
                if (btnDeposit.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = datCreationDate.DateTime,
                        LoansAdvanceId = loansAdvance.Id,
                        TransactionType = TransactionItemType.LoansAdvances,
                        debit = loansAdvance.LoanAmountOC,
                        credit = 0,
                        total = loansAdvance.LoanAmountOC - 0,
                        SystemRefNo = txtSystemRef.Text,
                        deptId = loansAdvance.deptId,
                        companyId = loansAdvance.companyId,
                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    loansAdvance.isDeposit = true;
                }
                else if (btnPayment.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = datCreationDate.DateTime,
                        LoansAdvanceId = loansAdvance.Id,
                        TransactionType = TransactionItemType.LoansAdvances,
                        debit = 0,
                        credit = loansAdvance.LoanAmountOC,
                        total = 0 - loansAdvance.LoanAmountOC,
                        SystemRefNo = txtSystemRef.Text,
                        deptId = loansAdvance.deptId,
                        companyId =loansAdvance.companyId,
                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    loansAdvance.isDeposit = false;
                }
                else
                {
                    loansAdvance.isDeposit = null;
                }
            }
            else
            {
                if (btnDeposit.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = datCreationDate.DateTime,
                        LoansAdvanceId = 0,
                        TransactionType = TransactionItemType.LoansAdvances,
                        debit = loansAdvance.LoanAmountOC,
                        credit = 0,
                        total = loansAdvance.LoanAmountOC - 0,
                        SystemRefNo = txtSystemRef.Text,
                        deptId = (lookupDepartment.SelectedItem as Department).Id,
                        companyId = (lookupCompany.SelectedItem as Company).Id,
                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    loansAdvance.isDeposit = true;
                }
                else if (btnPayment.IsChecked == true)
                {
                    pettyCashes.Add(new PettyCash()
                    {
                        CreationDate = datCreationDate.DateTime,
                        LoansAdvanceId = 0,
                        TransactionType = TransactionItemType.LoansAdvances,
                        debit = 0,
                        credit = loansAdvance.LoanAmountOC,
                        total = 0 - loansAdvance.LoanAmountOC,
                        SystemRefNo = txtSystemRef.Text,
                        deptId = (lookupDepartment.SelectedItem as Department).Id,
                        companyId = (lookupCompany.SelectedItem as Company).Id,
                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                        PettyCashRefId = cmbxPettyCashRef.SelectedIndex > -1 ? (cmbxPettyCashRef.SelectedItem as cmbitem).id : (int?)null
                    });
                    loansAdvance.isDeposit = false;
                }
                else
                {
                    loansAdvance.isDeposit = null;
                }

            }
            loansAdvance.pettyCashes = pettyCashes;

            if (editFlag == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without Approval") != null && loansAdvance.isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Loans and Advances is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (Convert.ToDouble(txtApprovedAmountOC.Text) == 0)
                        {
                            DXMessageBox.Show("Please enter some Loan Approved Amount before Approval!");
                            txtApprovedAmountOC.Focus();
                            return;
                        }

                        loansAdvance.stage = TransactionStage.Approved.ToString();
                        loansAdvance.isApproved = true;
                        loansAdvance.ApprovedDate = System.DateTime.Now;
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without ReApproval") != null && loansAdvance.isReApproved != true)
                {
                    if (Convert.ToDouble(txtApprovedAmountOC.Text) == 0)
                    {
                        DXMessageBox.Show("Please enter some Loan Approved Amount before Re-Approval!");
                        txtApprovedAmountOC.Focus();
                        return;
                    }

                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Loans and Advances is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        loansAdvance.stage = TransactionStage.Approved.ToString();
                        loansAdvance.isReApproved = true;
                        loansAdvance.ReApprovalDate = System.DateTime.Now;
                    }
                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    if (checkStatus.Id != loansAdvance.Status.Id)
                    {
                        loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                        if (loansAdvance.Status.isActive != true)
                        {
                            loansAdvance.ClosingDate = System.DateTime.Now;
                        }
                    }
                }
                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                if (checkStatus.Id != loansAdvance.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Loans Advance has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            var userss = UsersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                            if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                userss.Add(loansAdvance.Creator);

                            winTagUsers win = new winTagUsers(userss, loansAdvance.Id, TransactionItemType.LoansAdvances);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                            if (tagUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                tagUsers.Add(loansAdvance.Creator);

                            if (ccUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                ccUsers.Add(loansAdvance.Creator);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = loansAdvance.Status.Status;
                    string symbolCurr = "";
                    if (loansAdvance.currency != null)
                    {
                        symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog();
                    
                        comment.Comment = "Status of Loans Advance having Amount (OC): " + loansAdvance.LoanAmountOC.ToString() + " (" + symbolCurr + ")\n "
                   + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                        comment.Timestamp = DateTime.Now;
                        comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                    
                   
                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ",null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, loansAdvance.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + loansAdvance.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, loansAdvance.Id, 25, frmInputBox.comment);

                DXMessageBox.Show("Updated Successfully!");
                Window window = Window.GetWindow(this);
                window.Close();
            }
            else
            {
                loansAdvance.creatorId = SYSTEM_STATIC.currentUser.id;
                loansAdvance.isApproved = false;
                loansAdvanceRepo.AddLoansAdvance(loansAdvance);
                DXMessageBox.Show("Added Successfully!");
                UsersRepo.Add(TransactionInfo.Initialized, loansAdvance.Id, (int)TransactionItemType.LoansAdvances, "");
                Window window = Window.GetWindow(this);
                window.Close();
            }
            
        }

        private void LoadCompanies()
        {
            CompanyRepo repo = new CompanyRepo();
            lookupCompany.ItemsSource = repo.GetUserAdminBillCompanies(SYSTEM_STATIC.currentUser.id);
        }

        private void LoadApplicantTypes()
        {
            lookupApplicantType.ItemsSource = loansAdvanceRepo.GetAllApplicantTypes();
        }

        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        public void LoadStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            List<LoansAdvanceStatus> Statuses = new List<LoansAdvanceStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Loans and Advances Statuses") != null)
                Statuses = loansAdvanceRepo.GetAllloansAdvanceStatuses();
            else
                Statuses = loansAdvanceRepo.GetAllloansAdvanceStatuses().Where(x => x.isActive == true).ToList();

            Parallel.ForEach(Statuses, delegate (LoansAdvanceStatus status) 
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
                if (editFlag == true && loansAdvance != null)
                {
                    if(Convert.ToDouble(txtApprovedAmountOC.Text) == 0)
                    {
                        DXMessageBox.Show("Please enter some Loan Approved Amount before Approve!");
                        txtApprovedAmountOC.Focus();
                        return;
                    }

                    if(loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0)
                    {
                        if(loansAdvance.AdminBills.FirstOrDefault(x=>x.isApproved == false) != null)
                        {
                            DXMessageBox.Show("Please approve all Admin Bills linked with this Transaction!");
                            return;
                        }
                    }

                    //SalesReceipt receipt = new SalesReceipt();
                    loansAdvance = loansAdvanceRepo.GetLoansAdvance(loansAdvance.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (loansAdvance != null)
                    {
                        if (loansAdvance.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans and Advances") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Loans and Advances is Approved, Do you want to UnApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                        loansAdvance.isApproved = false;
                                        loansAdvance.stage = TransactionStage.AwaitingApproval.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, loansAdvance.Id, 25, frmInputBox.comment);

                                    loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Loans and Advances has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                                            if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                                userss.Add(loansAdvance.Creator);

                                            winTagUsers win = new winTagUsers(userss, loansAdvance.Id, TransactionItemType.LoansAdvances);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                                tagUsers.Add(loansAdvance.Creator);

                                            if (ccUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                                ccUsers.Add(loansAdvance.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (loansAdvance.currency != null)
                                    {
                                        symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Loans and Advance (Amount OC) having value: " + loansAdvance.LoanAmountOC.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Loans and Advance UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Loans and Advances are UnApproved (" + loansAdvance.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Loans and Advance is UnApproved (" + loansAdvance.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans and Advance Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans and Advance Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loansAdvance.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans and Advances") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Loans and Advances are Pending for Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    
                                        loansAdvance.isApproved = true;
                                        loansAdvance.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, loansAdvance.Id, 25, frmInputBox.comment);

                                    loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Loans and Advances has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0)
                                        {
                                            var userss = usersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                                            if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                                userss.Add(loansAdvance.Creator);

                                            winTagUsers win = new winTagUsers(userss, loansAdvance.Id, TransactionItemType.LoansAdvances);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;

                                            if (tagUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                                tagUsers.Add(loansAdvance.Creator);

                                            if (ccUsers.Find(x => x.id == loansAdvance.creatorId) == null)
                                                ccUsers.Add(loansAdvance.Creator);
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (loansAdvance.currency != null)
                                    {
                                        symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Loans and Advances (Amount OC) having value: " + loansAdvance.LoanAmountOC.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Loans and Advance Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Loans and Advances is Approved (" + loansAdvance.Id + ")");
                                    SystemLog.LogInfo(this.GetType(), "Loans and Advances is Approved (" +loansAdvance.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans and Advances Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans and Advances Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loansAdvance.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Loans and Advances") != null) ? true : false)
                            {
                                    loansAdvance.isReApproved = true;
                                    loansAdvance.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, loansAdvance.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                                MessageBox.Show("Loans and Advances are Approved (" + loansAdvance.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans and Advances is Approved (" + loansAdvance.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Loans and Advances Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans and Advances Directly user id=(" + MainWindow.currentUserid + ")");
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
        LoansAdvanceStatus oldStatus = new LoansAdvanceStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        static LoansAdvanceStatus statusChanged = new LoansAdvanceStatus();
        public ucFrmLoansAdvances(LoansAdvanceStatus loansAdvanceStatus)
        {
            statusChanged = loansAdvanceStatus;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            //loansAdvanceRepo = new LoansAdvanceRepo();

            if (loansAdvance != null && loansAdvanceId > 0)
            {
                var previous_status = loansAdvance.Status.Status;
                if (loansAdvance != null)
                {
                    if (loansAdvance.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmLoansAdvanceStatusChange ucFrmDirectClose = new frmLoansAdvanceStatusChange();
                    if (loansAdvance.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = loansAdvance.Status.Status;

                        var brush = new BrushConverter();

                        if (!String.IsNullOrEmpty(loansAdvance.Status.backcolor))
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(loansAdvance.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.loansAdvanceTemplate = LoansAdvanceTemplate.Advance;
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

                        var res = MessageBox.Show("Loans Advance has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                var userss = usersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                                if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                                    userss.Add(loansAdvance.Creator);


                                winTagUsers win = new winTagUsers(userss, loansAdvance.Id, TransactionItemType.LoansAdvances);
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


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Loans and Advances") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Loans and Advances without Approval") != null)
                        {

                            loansAdvance.PendingForClosing = false;
                            loansAdvance.stage = TransactionStage.Closed.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans and Advances having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            loansAdvance.PendingForClosing = true;
                            loansAdvance.stage = TransactionStage.AwaitingApproval.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans and Advances having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
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
            if(editFlag == true && loansAdvance != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();

                //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)

                //{

                //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Bill);
                //    inputBox.ShowDialog();

                //}
                //                else 

                if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {

                    var userss = UsersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                    if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                        userss.Add(loansAdvance.Creator);

                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, TransactionItemType.LoansAdvances);
                    inputBox.ShowDialog();

                }

                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (loansAdvance != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && loansAdvance.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

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
                    else if (loansAdvance.Id == 0)
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
                if (loansAdvance != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(loansAdvance.Id, TransactionItemType.LoansAdvances);

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
                if (loansAdvance.Id != 0)
                {
                    //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    //{
                    //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Bill);
                    //    inputBox.ShowDialog();
                    //}
                    //else 
                    if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                    {
                        var userss = UsersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id);

                        if (userss.Find(x => x.id == loansAdvance.creatorId) == null)
                            userss.Add(loansAdvance.Creator);

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, comment, TransactionItemType.LoansAdvances);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (loansAdvance != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && loansAdvance.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Loans Advance with System Ref #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + loansAdvance.SalesReferenceNo, loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (loansAdvance.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Loans and Advance first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
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
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(loansAdvance.Id, TransactionItemType.LoansAdvances);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (loansAdvance != null && loansAdvance.Id > 0)
            {
                var idd = loansAdvance.Id;
                if (idd != 0)
                {
                    frmTrackingWindow trackingWindow = new frmTrackingWindow(idd, TransactionItemType.LoansAdvances);
                    trackingWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Please save this Transaction first!");
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (loansAdvance.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Loans and Advances") != null))
            {
                if (DXMessageBox.Show("This is currently in the list of Void Loans and Advances! Do you want to remove it from Void?", "Remove Void Loans", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    loansAdvance.isVoid = false;
                    loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Loans and Advance has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        //if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 && loansAdvance.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && loansAdvance.InterDepartment != null && loansAdvance.InterDepartment.Id != 0)
                        //{
                        //    winTagUsers win = new winTagUsers(_usersRepo.getusersByDepartmentIdsList(new List<int> { loansAdvance.department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //    //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                        //    //inputBox.ShowDialog();
                        //}
                        //                        else 

                        if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id), loansAdvance.Id, TransactionItemType.LoansAdvances);
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
                    if (loansAdvance.currency != null)
                    {
                        symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Loans and Advance (Amount OC) having value: " + loansAdvance.LoanAmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Loans and Advance UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Loans and Advances") != null)
            {
                if (DXMessageBox.Show("This is not currently in the list of Void Loans and Advances! Do you want to move it to Void Loans and Advances?", "Add to Void Loans and Advances", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    loansAdvance.isVoid = true;
                    loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

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

                    var res1 = MessageBox.Show("Loans and Advance has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        //if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 && loansAdvance.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && loansAdvance.InterDepartment != null && loansAdvance.InterDepartment.Id != 0)
                        //{
                        //    winTagUsers win = new winTagUsers(_usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //    //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                        //    //inputBox.ShowDialog();
                        //}
                        //                        else

                        if (loansAdvance.department != null && loansAdvance.department.Id != 0 && loansAdvance.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*//*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(loansAdvance.department.Id, loansAdvance.company.Id), loansAdvance.Id, TransactionItemType.LoansAdvances);
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
                    if (loansAdvance.currency != null)
                    {
                        symbolCurr = loansAdvance.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Loans and Advance (Amount OC) having value: " + loansAdvance.LoanAmountOC.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Loans and Advance Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Loans and Advance #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void LoanAdvanceStages()
        {
            if (loansAdvance.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (loansAdvance.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (loansAdvance.isApproved == true && loansAdvance.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (loansAdvance.isApproved == true && loansAdvance.Status.isActive == false && loansAdvance.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (loansAdvance.isApproved == true && loansAdvance.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (loansAdvance.isApproved == true)
            {
                //lblStage.Text = "Approved";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (loansAdvance.isApproved == false)
            {
                //lblStage.Text = "Under Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (loansAdvance.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveLAAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
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

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (loansAdvanceId != 0)
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
                        destination += "Attachments\\LoansAdvances\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += loansAdvanceId + "_" + TransactionItemType.LoansAdvances.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.LoansAdvances);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), loansAdvanceId, TransactionItemType.LoansAdvances, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, loansAdvance.Id, (int)TransactionItemType.LoansAdvances, "Added a new attachment");

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

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupCompany.SelectedItem as Company;

                if (company != null)
                {

                    var loginUser = SYSTEM_STATIC.currentUser;
                    var departments = company.departments;
                    departments = departments.Where(x => x.isActive == true && x.IsAdminBillType == true).ToList();
                    departments = departments.Where(x => x.employees.FirstOrDefault(y => y.EmpId == loginUser.employeeId) != null).ToList();

                    if (editFlag == true  && loansAdvance.department != null && departments.FirstOrDefault(x => x.Id == loansAdvance.deptId) == null)
                    {
                        departments.Add(loansAdvance.department);
                    }



                    lookupDepartment.SelectedItem = null;

                    DepartmentRepo deptRepo = new DepartmentRepo();
                    var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();

                    if (deptMngt != null)
                        foreach (var _dept in deptMngt)
                        {
                            if (departments.Any(x => x.Id == _dept.Id) == false && _dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                                departments.Add(_dept);
                        }

                    //DepartmentRepo deptRepo = new DepartmentRepo();
                    //var deptMngt = deptRepo.GetActiveDepartments().Where(x => x.IsManagerial == true).ToList();

                    //if (deptMngt != null)
                    //    foreach (var _dept in deptMngt)
                    //    {
                    //        if (departments.Any(x => x.Id == _dept.Id) == false && _dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                    //            departments.Add(_dept);
                    //    }

                    //Only Allowed departments to Employee will show in Dropdown

                    lookupDepartment.ItemsSource = departments;
                    loadBillReferenceNo();
                    var employees = company.employees;
                    lookupEmployee.ItemsSource = employees;
                }
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message + "Invalid Company");
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

        private void ChkEmployee_Checked(object sender, RoutedEventArgs e)
        {
            grdEmployee.Visibility = Visibility.Visible;
            //grdApplicantType.Visibility = Visibility.Collapsed;
            //grdApplicant.Visibility = Visibility.Collapsed;
        }

        private void ChkEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            grdEmployee.Visibility = Visibility.Collapsed;
            //grdApplicantType.Visibility = Visibility.Visible;
            //grdApplicant.Visibility = Visibility.Visible;
        }

        

        private void BtnPayment_Checked(object sender, RoutedEventArgs e)
        {
            btnDeposit.IsChecked = false;
        }

        private void BtnDeposit_Checked(object sender, RoutedEventArgs e)
        {
            btnPayment.IsChecked = false;
        }

        private void lookupApplicantType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var typeId = (lookupApplicantType.SelectedItem as LoanApplicantType).Id;
            var compId = (lookupCompany.SelectedItem as Company).Id;
            var deptId = (lookupDepartment.SelectedItem as Department).Id;

            var applicants = loansAdvanceRepo.GetApplicantsByTypeCompDept(typeId, compId, deptId);
            lookupApplicant.ItemsSource = applicants;
        }

        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
                
        }

        private void lookupApplicantType_GotFocus(object sender, RoutedEventArgs e)
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

        private void lookupApplicant_GotFocus(object sender, RoutedEventArgs e)
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
            if (lookupApplicantType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Applicant Type first!");
                return;
            }
        }

        private void lookupEmployee_GotFocus(object sender, RoutedEventArgs e)
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

        private void Txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amountOC = Math.Round( Convert.ToDouble( txtAmountOC.Text),2);
            var MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
            var amountMER = Math.Round(amountOC * MER, 2);
            txtAmoutMER.Text = amountMER.ToString();
        }

        private void TxtAmountOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amountOC = Math.Round(Convert.ToDouble(txtAmountOC.Text), 2);
            var MER = Math.Round(Convert.ToDouble(txtexchangerate.Text), 2);
            var amountMER = Math.Round(amountOC * MER, 2);
            txtAmoutMER.Text = amountMER.ToString();
        }

        private void TxtLoanTenure_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
             var days =Convert.ToInt32( txtLoanTenure.Text);
            DateTime creationDate = datCreationDate.DateTime;
            DateTime returnDate = creationDate.AddDays(days);

            datLoanReturnDate.DateTime = returnDate;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (loansAdvanceId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, loansAdvanceId, 25, "Viewed details of Loans and Advances");
            }
        }

        private void TxtApprovedAmountOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var approvedAmount =Convert.ToDouble( txtApprovedAmountOC.Text);
            txtAmountOC.Text = approvedAmount.ToString();
        }

        private void BtnCreatePayment_Click(object sender, RoutedEventArgs e)
        {
            bool isFullyPaid = true;

            if (editFlag == true && loansAdvance != null)
            {
                if (loansAdvance.isVoid == true)
                {
                    DXMessageBox.Show("This Transaction is Voided!");
                    return;
                }
                if (loansAdvance.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Transaction is in Pending for Closing State!");
                    return;
                }
                if (loansAdvance.isApproved == false)
                {
                    DXMessageBox.Show("Transaction is under Approval!");
                    return;
                }

                if (loansAdvance.Status.isActive == false)
                {
                    DXMessageBox.Show("This Transaction is Already closed!");
                    return;
                }

                
                if (loansAdvance.Payments.Where(x => x.isVoid != true) == null || loansAdvance.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    isFullyPaid = false;
                }
                else if ((loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) - loansAdvance.LoanAmountOC) != 0)
                {
                    isFullyPaid = false;
                }
                

                if (isFullyPaid == true)
                {
                    DXMessageBox.Show("This Transaction is fully Paid!");
                    return;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances Payment") != null)
                {
                    Window moduleWin = new Window();

                    ucFrmLoansAdvancePaymentAdd frmPayments = new ucFrmLoansAdvancePaymentAdd();

                    frmPayments.loansAdvanceId = loansAdvance.Id;
                    frmPayments.createdFromBill = true;
                    moduleWin.Content = frmPayments;
                    moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    moduleWin.WindowState = WindowState.Maximized;
                    moduleWin.Show();

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Payment!");
                }
            }
        }

        private void BtnCreateReceipt_Click(object sender, RoutedEventArgs e)
        {
            bool isFullyReceipt = true;

            if (editFlag == true && loansAdvance != null)
            {
                if (loansAdvance.isVoid == true)
                {
                    DXMessageBox.Show("This Transaction is Voided!");
                    return;
                }
                if (loansAdvance.PendingForClosing == true)
                {
                    DXMessageBox.Show("This Transaction is in Pending for Closing State!");
                    return;
                }
                if (loansAdvance.isApproved == false)
                {
                    DXMessageBox.Show("Transaction is under Approval!");
                    return;
                }

                if (loansAdvance.Status.isActive == false)
                {
                    DXMessageBox.Show("This Transaction is Already closed!");
                    return;
                }


                if (loansAdvance.SalesReceipts.Where(x => x.isVoid != true) == null || loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count == 0)
                {
                    isFullyReceipt = false;
                }
                else if ((loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount)) - (loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) != 0)
                {
                    isFullyReceipt = false;
                }


                if (isFullyReceipt == true)
                {
                    DXMessageBox.Show("This Transaction is fully Received!");
                    return;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances Sale Receipts") != null)
                {
                    Window moduleWin = new Window();

                    ucFrmLoansAdvanceSaleReceiptAdd frmPayments = new ucFrmLoansAdvanceSaleReceiptAdd();

                    frmPayments.loansAdvanceId = loansAdvance.Id;
                    //frmPayments.createdFromBill = true;
                    moduleWin.Content = frmPayments;
                    moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    moduleWin.WindowState = WindowState.Maximized;
                    moduleWin.Show();

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Sale Receipt!");
                }
            }
        }

        private void BtnCreateAdminBill_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Admin Bill From Loans Advances") != null)
            {
                Window moduleWin = new Window();

                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                frmBillAdd.editFlag = false;
                frmBillAdd.loansAdvanceId = loansAdvance.Id;
                moduleWin.Content = frmBillAdd;
                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.WindowState = WindowState.Maximized;
                moduleWin.Show();

                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                DXMessageBox.Show("Permission required to Create Admin Bills from Loans Advances!");
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

            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (department != null && department.Id != 0 && company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.LoansAdvances);
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

                if (frmInputBox.commentAdded == true && loansAdvance.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    

                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.PurchaseInvoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (loansAdvance.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Loans Advance first to add a comment!");
                }
            }
            loadcomments();
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
        public void GellAllOrdersTracking()
        {
            
            if (loansAdvance.Id != 0)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(loansAdvance.Id, TransactionItemType.LoansAdvances);
            }
        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
            if (grid.SelectedItem != null)
            {

                var item = grid.SelectedItem as AllOrdersView;

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

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId);

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;

                                    frmBill.WindowState = WindowState.Maximized;

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

                                frmBill.WindowState = WindowState.Maximized;
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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }
    }
}
