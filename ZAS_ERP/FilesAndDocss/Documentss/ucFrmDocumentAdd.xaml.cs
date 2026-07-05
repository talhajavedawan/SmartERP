using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.LookUp;
using ERP_BL.Countryy;
using ERP_BL.Databases;
using ERP_BL.Documents;
using ERP_BL.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{ /*comment*/
    /// <summary>
    /// Interaction logic for ucFrmDocumentAdd.xaml
    /// </summary>
    public partial class ucFrmDocumentAdd : UserControl
    {
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        List< ERP_BL.Documents.Document> documents = new List<ERP_BL.Documents.Document>();
        public int groupId = 0;
        public bool editFlag = false;
        DocumentStatus checkStatus = new DocumentStatus();
        DocumentRepo documentRepo = new DocumentRepo();
        List<DocumentStatus> DocumentStatuses = new List<DocumentStatus>();

        static DocumentStatus statusChanged = new DocumentStatus();

        string stage;
        bool? isApproved;
        //bool? isReApproved;
        DateTime? approvalDate;

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucFrmDocumentAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdDocumentData.ItemsSource = documents;
            LoadCompanies();
            LoadCountries();
            LoadEmployees();
            LoadPaymentStatus();

            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
            }

                
            if (editFlag == true && groupId > 0)
            {
                documents = documentRepo.GetDocumentsByGroupId(groupId);
                lblRefNo.Text = documents[0].SystemRefNo;
                if (documents[0].isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (documents[0].isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (documents[0].isApproved == true && documents[0].stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (documents[0].isApproved == true && documents[0].Status.isActive == false && documents[0].PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (documents[0].isApproved == true && documents[0].PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (documents[0].isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (documents[0].isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (documents[0].PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }


                if (documents[0].CreationDate != null)
                    datCreationDate.EditValue = (DateTime)documents[0].CreationDate;


                

                int index = 0;
                // Select Company
                if (documents[0].company != null)
                {

                    var companylist = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                    if (documents[0].company != null && companylist.Find(x => x.Id == documents[0].company.Id) == null)
                    {

                        companylist.Add(documents[0].company);
                        lookupCompany.ItemsSource = null;
                        lookupCompany.ItemsSource = companylist;
                        //lookupCompany.IsEnabled = false;
                    }

                    index = 0;
                    foreach (var _company in companylist)
                    {
                        if (_company.Id == documents[0].company.Id)
                        {
                            lookupCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    lookupCompany.Text = "Select Company";
                }


                if (documents[0].department != null)
                {

                    var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                    if (documents[0].department != null && deptList.Find(x => x.Id == documents[0].department.Id) == null)
                    {

                        deptList.Add(documents[0].department);
                        lookupDepartment.ItemsSource = null;
                        lookupDepartment.ItemsSource = deptList;
                        //lookupCompany.IsEnabled = false;
                    }

                    index = 0;
                    foreach (var _dept in deptList)
                    {
                        if (_dept.Id == documents[0].department.Id)
                        {
                            lookupDepartment.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    lookupDepartment.Text = "Select Department";
                }

                if (documents[0].documentTemplate != null)
                {
                    lookupDocumentTemplate.Text = documents[0].documentTemplate.TemplateName;
                }


                if (documents[0].employee != null)
                {
                    chkEmployee.IsChecked = true;
                    lookupEmployee.Text = documents[0].employee.person.FullName;
                }
                else
                {
                    chkEmployee.IsChecked = false;
                    txtEmployee.Text = documents[0].Employee;
                }

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (documents[0].Status != null)
                {
                    checkStatus = documents[0].Status;
                    index = 0;
                    foreach (var _status in statusList)
                    {

                        if (_status.id == documents[0].Status.Id)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (documents[0].SystemRefNo != null)
                    txtSystemRef.Text = "Document-" + documents[0].transactionGroupId;

                if (documents[0].creator != null)
                    txtCreator.Text = documents[0].creator.employee.person.FullName;

                grdDocumentData.ItemsSource = documents;


            }
        }

        public void LoadCompanies()
        {
            empUser = documentRepo.GetEmployeeForDocuments(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
        }

        public void LoadCountries()
        {
            CountryRepo countryRepo = new CountryRepo();
            lookupCountry.ItemsSource = countryRepo.GetAllCountries();
        }

        private void LoadEmployees()
        {
            EmployeeRepo empRepo = new EmployeeRepo();
            var employees = empRepo.GetAllEmployees();
            lookupEmployee.ItemsSource = employees;
        }

        private void LoadPaymentStatus()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Document Statuses") != null)
                DocumentStatuses = documentRepo.GetAllDocumentStatuses();
            else
                DocumentStatuses = documentRepo.GetAllDocumentStatuses().Where(x => x.isActive == true).ToList();
            DocumentStatuses = DocumentStatuses.Where(x => x.isDisable != true).ToList();

            Parallel.ForEach(DocumentStatuses, delegate (DocumentStatus status) // foreach (BillStatus status in BillStatuses)
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

        
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var company = lookupCompany.SelectedItem as Company;

            CompanyRepo companyRepo = new CompanyRepo();

            Company company = companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);

            List<Department> departments = new List<Department>();

            if (company != null)
            {
                if (company.departments != null)
                {
                    foreach (var _dept in empUser.departments.Where(x => x.IsDocumentType == true && x.isActive == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (documents != null && documents.Count > 0 && editFlag == true)
                        if (documents[0].department != null)
                                if (departments.FirstOrDefault(x => x.Id == documents[0].department.Id) == null)
                                    departments.Add(documents[0].department);
                }
                lookupDepartment.ItemsSource = departments;
            }
        }

        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var company = lookupCompany.SelectedItem as Company;
            var dept = lookupDepartment.SelectedItem as Department;

            if (company != null && dept != null)
            {
                var templates = documentRepo.GetAllDocumentTemplate().Where(x => x.companies.FirstOrDefault(y => y.Id == company.Id) != null && x.departments.FirstOrDefault(y => y.Id == dept.Id) != null).ToList();
                lookupDocumentTemplate.ItemsSource = templates;
            }
        }

        private void lookupEmployee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveDocumentAttachmentCategories();
                //var pos = System.Windows.Input.Mouse.GetPosition(this);
                //grdAttach1.TranslatePoint(pos, grdVisitingRecords);
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            var document = grdDocumentData.SelectedItem as Document;

            if (document != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetDocumentAttachmentsListByCategory(document.Id, TransactionItemType.Document);
                    grdAttachments.Visibility = Visibility.Visible;
                }
            }
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
                if (editFlag == true && documents != null && documents.Count > 0)
                {


                    documentRepo = new DocumentRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var documentsForApproval = documentRepo.GetDocumentsByGroupId(documents[0].transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (documentsForApproval != null && documentsForApproval.Count > 0)
                    {
                        if (documentsForApproval[0].isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Document") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Documents are Approved, Do you want to UnApprove these Bills?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < documentsForApproval.Count; i++)
                                    {
                                        documentsForApproval[i].isApproved = false;
                                        documentsForApproval[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, documents[0].transactionGroupId, (int)TransactionItemType.Document, frmInputBox.comment);

                                    documentRepo.UpdateDocument(documentsForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Document has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if( documentsForApproval[0].department != null && documentsForApproval[0].department.Id != 0 && documentsForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(documentsForApproval[0].department.Id, documentsForApproval[0].company.Id), groupId, TransactionItemType.Document);
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
                                        Comment = "Documents having Ref # " + documentsForApproval[0].SystemRefNo.ToString() + " has been UnApproved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Document UnApproved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document # " + documentsForApproval[0].SystemRefNo, documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document # " + documentsForApproval[0].SystemRefNo, documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Documents are UnApproved (" + documents[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Document is UnApproved (" + documents[0].transactionGroupId + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Document Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Document Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (documentsForApproval[0].isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Document") != null) ? true : false)
                            {
                                var res = MessageBox.Show("Documents are Pending for Approval, Do you want to Approve these documents?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    for (int i = 0; i < documentsForApproval.Count; i++)
                                    {
                                        documentsForApproval[i].isApproved = true;
                                        documentsForApproval[i].stage = TransactionStage.Approved.ToString();
                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, documents[0].transactionGroupId, (int)TransactionItemType.Document, frmInputBox.comment);

                                    documentRepo.UpdateDocument(documentsForApproval);

                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
                                    List<User> ccUsers = new List<User>();
                                    List<User> tagUsersRecommendation = new List<User>();
                                    List<User> ccUsersRecommendation = new List<User>();

                                    var res1 = MessageBox.Show("Document has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (documentsForApproval[0].department != null && documentsForApproval[0].department.Id != 0 && documentsForApproval[0].company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(documentsForApproval[0].department.Id, documentsForApproval[0].company.Id), groupId, TransactionItemType.Document);
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
                                        Comment = "Document having Ref # " + documentsForApproval[0].SystemRefNo + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Document Approved",
                                        TaggedList = tagUsers,
                                        CCUsersList = ccUsers,
                                        TaggedRecomenndedList = tagUsersRecommendation,
                                        CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document #" + documentsForApproval[0].SystemRefNo, documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document #" + documentsForApproval[0].SystemRefNo, documentsForApproval[0].transactionGroupId, TransactionItemType.Document, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Documents are Approved (" + documents[0].transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Document is Approved (" + documents[0].transactionGroupId + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Document Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Document Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (documents[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Document") != null) ? true : false)
                            {
                                for (int i = 0; i < documents.Count; i++)
                                {
                                    documentsForApproval[i].isReApproved = true;
                                    documentsForApproval[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, documents[0].transactionGroupId, (int)TransactionItemType.Document, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                documentRepo.UpdateDocument(documentsForApproval);

                                MessageBox.Show("Documents are Approved (" + documents[0].transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Document is Approved (" + documents[0].transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Document Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Document Directly user id=(" + MainWindow.currentUserid + ")");
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


        public ucFrmDocumentAdd(DocumentStatus documentStatus)
        {
            statusChanged = documentStatus;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            if (editFlag == true && documents != null && documents.Count > 0)
            {
                string previous_status;
                previous_status = documents[0].Status.Status;
                if (documents[0].isApproved == false)
                {
                    DXMessageBox.Show("Documents are pending for approval!");
                    return;
                }

                statusChanged = null;
                ucFrmDocumentDirectClose ucFrmDirectClose = new ucFrmDocumentDirectClose();
                if (documents[0].Status != null)
                {
                    ucFrmDirectClose.statusName.Text = documents[0].Status.Status;

                    var brush = new BrushConverter();
                    ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(documents[0].Status.backcolor);
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

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Document without Approval") != null)
                    {
                        for (int i = 0; i < documents.Count; i++)
                        {
                            documents[i].PendingForClosing = false;
                            documents[i].stage = TransactionStage.Closed.ToString();
                            documents[i].status_Id = statusChanged.Id;
                            documents[i].LastStatusChangeDate = System.DateTime.Now;
                            documents[i].ClosingDate = System.DateTime.Now;


                        }
                        documentRepo.UpdateDocument(documents);
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Document, frmInputBox.comment);

                        if (previous_status != statusChanged.Status)
                        {
                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Document having system ref #: " + documents[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            if (documents.Count != 0)
                            {
                                procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);
                                    }
                                }
                            }
                        }
                        //Load_Receipts();
                    }
                    else
                    {
                        for (int i = 0; i < documents.Count; i++)
                        {
                            documents[i].PendingForClosing = true;
                            documents[i].stage = TransactionStage.AwaitingApproval.ToString();
                            documents[i].status_Id = statusChanged.Id;
                            documents[i].LastStatusChangeDate = System.DateTime.Now;
                            documents[i].ClosingDate = System.DateTime.Now;


                        }
                        documentRepo.UpdateDocument(documents);
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Document, frmInputBox.comment);

                        if (previous_status != statusChanged.Status)
                        {
                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Documents having system ref #: " + documents[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:",
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            if (documents.Count != 0)
                            {
                                procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);
                                    }
                                }
                            }
                        }
                    }
                }
                DXMessageBox.Show("Status has been changed to InActive from " + previous_status + " to " + statusChanged.Status);
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (documents != null && documents.Count > 0)
            {
                UsersRepo UsersRepo = new UsersRepo();

                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (documents[0].department != null && documents[0].department.Id != 0 && documents[0].company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(documents[0].department.Id, documents[0].company.Id), TransactionItemType.Document);
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && documents[0].transactionGroupId != 0)
                {
                    var commentId = procurementRepo.AddCommentLinkNotification(documents[0].transactionGroupId, TransactionItemType.Document, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                    if (commentId != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                        }

                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Document # " + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Document # " + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                        }
                    }

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (documents[0].transactionGroupId == 0)
                {
                    DXMessageBox.Show("Kindly save Document first to add a comment!");
                }


            }
            
        }
        public void loadcomments()
        {
            try
            {
                if (documents != null && documents.Count>0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(documents[0].transactionGroupId, TransactionItemType.Document);

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
            if (grdDocumentData.SelectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Document") != null)
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

        private void btnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (groupId > 0)
            {
                documents = documentRepo.GetDocumentsByGroupId(groupId);

                if (documents != null && documents.Count > 0)
                {                   


                    if (documents[0].isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Document") != null))
                    {
                        if (DXMessageBox.Show("This Transaction is currently in the list of Void documents! Do you want to remove it from Void?", "Remove Void documents", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            documentRepo.SetDocumentsToVoid(groupId, false);
                            grdVoid.Visibility = Visibility.Collapsed;

                            NotificationsRepo notificationsRepo = new NotificationsRepo();
                            ProcurementRepo procurementRepo = new ProcurementRepo();
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
                            List<User> ccUsers = new List<User>();
                            List<User> tagUsersRecommendation = new List<User>();
                            List<User> ccUsersRecommendation = new List<User>();

                            var res1 = MessageBox.Show("Document has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (documents[0].department != null && documents[0].department.Id != 0 && documents[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(documents[0].department.Id, documents[0].company.Id), groupId, TransactionItemType.Document);
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
                                Comment = "Document having Ref # " + documents[0].SystemRefNo.ToString() + " has been marked as Unvoid",
                                Timestamp = DateTime.Now,
                                Subject = "Document UnVoided",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Document") != null)
                    {
                        if (DXMessageBox.Show("This Transaction is not currently in the list of Void documents! Do you want to move it to Void documents?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            documentRepo.SetDocumentsToVoid(groupId, true);
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

                            var res1 = MessageBox.Show("Document has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                            {
                                if (documents[0].department != null && documents[0].department.Id != 0 && documents[0].company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(documents[0].department.Id, documents[0].company.Id), groupId, TransactionItemType.Document);
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
                                Comment = "Document having Ref # " + documents[0].SystemRefNo + " has been marked as void",
                                Timestamp = DateTime.Now,
                                Subject = "Document Voided",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document # " + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Document # " + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to Mark or UnMark a Document to Void!");
                    }
                }

            }
        }

        private void btnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

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

        private void btnCloseAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
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
                        if (str.Contains("Document"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Document);
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

        private void btnAttachment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
        }

       

        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastDocumentId = documentRepo.GetLastTransactionId();
            if (lastDocumentId == 0)
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
                var lastId = lastDocumentId/*.transactionGroupId*/;
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

        private void NewGroupIdCalculation()
        {
            var lastDocumentId = documentRepo.GetLargestTransactionId();

            intGroupId = lastDocumentId + 1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                NewGroupIdCalculation();
                txtSystemRef.Text = "Document-" + intGroupId;
                lblRefNo.Text = " (Document-" + intGroupId + ")";
                //loansAdvance.transactionGroupId = intGroupId;
            }
            if (lookupDocumentTemplate.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Document Template!");
                lookupDocumentTemplate.Focus();
                return;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please Select Creation Date!");
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
            if (chkEmployee.IsChecked == true)
            {
                if (lookupEmployee.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Employee!");
                    lookupEmployee.Focus();
                    return;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(txtEmployee.Text))
                {
                    DXMessageBox.Show("Please Enter Employee!");
                    txtEmployee.Focus();
                    return;
                }
            }
            
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Status!");
                cmbStatus.Focus();
                return;
            }


            if (editFlag == true && documents.Count > 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without Approval") != null && documents[0].isApproved != true)
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Document is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        stage = TransactionStage.Approved.ToString();
                        isApproved = true;
                        approvalDate = System.DateTime.Now;
                    }
                }
            }

            // Cast ItemsSource to a List of your class type
            var itemsSourceList = grdDocumentData.ItemsSource as List<Document>;
            //int index = 0;
            if (itemsSourceList != null)
                for (int i = 0; i < itemsSourceList.Count; i++)
                {
                    var doc = itemsSourceList[i];

                    if (editFlag == true)
                    {
                        //doc = documents[index];
                        //index++;

                        if (isApproved != null)
                            doc.isApproved = isApproved;
                        if (stage != null)
                            doc.stage = stage;
                        if (approvalDate != null)
                            doc.ApprovedDate = approvalDate;
                    }
                    else
                    {
                        doc.isApproved = false;
                        doc.stage = ERP_BL.Enums.TransactionStage.AwaitingApproval.ToString();
                    }

                    //receipt = _receipt;
                    doc.CreationDate = datCreationDate.DateTime;
                    //receipt.GLPostingDate = datglPostingdate.DateTime;
                    doc.documentTemplate_Id = (lookupDocumentTemplate.SelectedItem as DocumentTemplate).Id;
                    doc.company_Id = (lookupCompany.SelectedItem as Company).Id;
                    doc.dept_Id = (lookupDepartment.SelectedItem as Department).Id;

                    if (chkEmployee.IsChecked == true)
                    {
                        doc.employee_Id = (lookupEmployee.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                        doc.Employee = null;
                    }
                    else
                    {
                        doc.employee_Id = null;
                        doc.Employee = txtEmployee.Text;
                    }



                    doc.status_Id = (cmbStatus.SelectedItem as cmbitem).id;
                    //doc.documentType_Id = _doc.documentType?.Id;
                    //doc.country_Id = _doc.country?.Id;
                    //doc.IssueDate = _doc.IssueDate;
                    //doc.ExpiryDate = _doc.ExpiryDate;
                    //doc.isOpen = _doc.isOpen;
                    doc.SystemRefNo = txtSystemRef.Text;

                    if (editFlag == false)
                    {
                        doc.creator_Id = SYSTEM_STATIC.currentUser.id;
                        doc.transactionGroupId = intGroupId;
                    }


                    if (editFlag == true)
                    {
                        doc.isVoid = documents[0].isVoid;
                        doc.isReviewed = documents[0].isReviewed;
                        doc.needReview = documents[0].needReview;
                        doc.PendingForClosing = documents[0].PendingForClosing;
                        if (isApproved != null)
                            doc.isApproved = isApproved;
                        else
                            doc.isApproved = documents[0].isApproved;

                        if (stage != null)
                            doc.stage = stage;
                        else
                            doc.stage = documents[0].stage;

                        if (approvalDate != null)
                            doc.ApprovedDate = approvalDate;
                        else
                            doc.ApprovedDate = documents[0].ApprovedDate;

                        doc.ReApprovalDate = documents[0].ReApprovalDate;
                        doc.isReApproved = documents[0].isReApproved;
                        doc.ClosingDate = documents[0].ClosingDate;
                        doc.LastStatusChangeDate = documents[0].LastStatusChangeDate;

                        doc.transactionGroupId = documents[0].transactionGroupId;
                        doc.creator_Id = documents[0].creator_Id;

                        //if (documents.Find(x => x.Id == doc.Id) != null)
                        //    documents[documents.FindIndex(x => x.Id == doc.Id)] = doc;
                        //else
                        //    _doc = doc;

                    }
                    //else if(editFlag == false)
                    //{

                    //    row = doc as GridRow;
                    //}

                    grdDocumentData.RefreshRow(i);
                }

            if (editFlag == true)
            {
                //if(documents.Where(x=>x.Id == 0).Count() > 0)
                //{
                //    GroupIdCalculation();
                //    documents.ForEach(cc => cc.transactionGroupId = intGroupId);
                //    documents.ForEach(cc => cc.SystemRefNo = "Document-" + intGroupId);
                //}
                documentRepo.UpdateDocument(documents);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                documentRepo.AddDocument(documents);
                DXMessageBox.Show("Added Successfully!");
            }
            Window myWin = Window.GetWindow(this);
            myWin.Close();
        }
        private void btnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            var document = grdDocumentData.SelectedItem as Document;

            if (document != null)
            {
                var Idd = document.Id;
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
                            destination += "Attachments\\Document\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += Idd + "_" + TransactionItemType.Document.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.Document);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), Idd, TransactionItemType.Document, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, document.Id, (int)TransactionItemType.Document, "Added a new attachment");

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
        private void chkEmployee_Checked(object sender, RoutedEventArgs e)
        {
            txtEmployee.Visibility = Visibility.Collapsed;
            lookupEmployee.Visibility = Visibility.Visible;
        }
        private void chkEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            txtEmployee.Visibility = Visibility.Visible;
            lookupEmployee.Visibility = Visibility.Collapsed;
        }

        private void cmbStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void View_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {

        }

        private void grdDocumentData_ColumnsPopulated(object sender, RoutedEventArgs e)
        {
            var colAttach = grdDocumentData.Columns["AttachNew"];
            colAttach.CellTemplate = (DataTemplate)this.Resources["AttachmentButton"];
            colAttach.Width = 70;
            colAttach.Name = "AttachNew";

            var colAttachList = grdDocumentData.Columns["AttachmentList"];
            colAttachList.CellTemplate = (DataTemplate)this.Resources["AttachmentListButton"];
            colAttachList.Width = 70;
            colAttachList.Name = "AttachmentList";
        }

        private void lookupDocTypes_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var lookUpEdit = sender as LookUpEdit;
            if (lookUpEdit != null)
            {
                var selectedItem = lookUpEdit.EditValue;

                // Access the current row's data context
                var rowData = (sender as FrameworkElement)?.DataContext as Document;
                var row = sender as GridRow;
                if (rowData != null)
                {
                    //row.c
                    //// Update other fields in rowData based on the selected item
                    //rowData. = GetUpdatedItemsBasedOnSelection(selectedItem);

                    //// Optionally refresh the GridControl’s data
                    //gridControl.RefreshData();

                    //e.
                }
            }
        }

        private void lookupDocumentTemplate_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var company = lookupCompany.SelectedItem as Company;
            var dept = lookupDepartment.SelectedItem as Department;
            var template = lookupDocumentTemplate.SelectedItem as DocumentTemplate;

            if (company != null && dept != null && template != null)
            {
                var types = documentRepo.GetAllDocumentType();
                lookupDocumentType.ItemsSource = types.Where(x => x.companies.FirstOrDefault(y => y.Id == company.Id) != null && x.departments.FirstOrDefault(y => y.Id == dept.Id) != null && x.DocumentTemplates.FirstOrDefault(y => y.Id == template.Id) != null).ToList();
            }

            if (lookupDocumentTemplate.SelectedIndex >= 0)
            {
                //var types = documentRepo.GetAllDocumentType().Where(x => x.documentTemplate_Id == template.Id).ToList();
                //lookupDocumentType.ItemsSource = types;

                var authorities = documentRepo.GetAllDocumentAuthority().Where(x => x.documentTemplate_Id == template.Id).ToList();
                lookupDocumentAuthority.ItemsSource = authorities;
            }
        }

        private void grdDocumentData_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            
            

            switch (e.Column.FieldName)
            {
                case "NoOfAttachments":
                    var row = grdDocumentData.GetRowByListIndex(e.ListSourceRowIndex) as Document;
                    var NoOfAttach = SYSTEM_STATIC.GetDocumentAttachmentsListByCategory(row.Id, TransactionItemType.Document);
                    var countt = NoOfAttach.Sum(x => x.Items.Count);
                    e.Value = countt;
                    break;
            }
        }

        //comment
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var fileName = (sender as TextBlock).Tag.ToString(); // Replace with your data model
            //if (attachment == null) return;

            // Fetch the preview content
            
            var preview = GetFilePreview(fileName);

            if (preview is BitmapImage image)
            {
                PreviewImage.Source = image;
                PreviewImage.Visibility = Visibility.Visible;
                PreviewText.Visibility = Visibility.Collapsed;
            }
            else if (preview is string text)
            {
                PreviewText.Text = text;
                PreviewText.Visibility = Visibility.Visible;
                PreviewImage.Visibility = Visibility.Collapsed;
            }

            PreviewPopup.IsOpen = true;
        }

        private void AttachmentList_MouseLeave(object sender, MouseEventArgs e)
        {
            PreviewPopup.IsOpen = false;
        }

        private object GetFilePreview(string fileName)
        {
            string ftpPath = "";
            if (SYSTEM_STATIC.server == "Fsociety96")
                //For LAN
                ftpPath = $"ftp://192.168.10.91/{fileName}";
            else if (SYSTEM_STATIC.server == "119.156.232.242,1411")
                //For WAN
                ftpPath = $"ftp://119.156.232.242/{fileName}";

             
            string ftpUsername = "ZAS@ZASErpProd";
            string ftpPassword = "0zLjmxumEdR9*ls8";

            string downloadsFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string outputFileName = "first-page.png"; // Change file name if needed
            string outputImagePath = System.IO.Path.Combine(downloadsFolderPath, outputFileName);

            try
            {
                using (WebClient client = new WebClient())
                {
                    // Set FTP credentials
                    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    // Download the file into memory
                    byte[] fileData = client.DownloadData(ftpPath);

                    // Check file type and process accordingly
                    if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    {

                        // Preview PDF - Render the first page as an image
                        RenderFirstPageFromFtp(ftpPath, ftpUsername, ftpPassword, outputImagePath);

                        // Ensure the file exists
                        if (!File.Exists(outputImagePath))
                            throw new FileNotFoundException("The specified PNG file was not found.", outputImagePath);

                        // Create a BitmapImage
                        BitmapImage bitmapImage = new BitmapImage();

                        // Open the file stream
                        using (FileStream stream = new FileStream(outputImagePath, FileMode.Open, FileAccess.Read))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Ensures the file is fully loaded into memory
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                        }

                        bitmapImage.Freeze(); // Make it cross-thread accessible
                        return bitmapImage;
                    }
                    else if (fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        // Preview Text - Read a snippet of the text
                        return PreviewTextFile(fileData);
                    }
                    else if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    {
                        //return new BitmapImage(new Uri(ftpPath));

                        // Load the image data into a BitmapImage
                        using (var stream = new MemoryStream(fileData))
                        {
                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = stream;
                            bitmap.EndInit();
                            return bitmap; // Return the preview image
                        }
                    }
                    else
                    {
                        return null; // Unsupported file type
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching file preview: {ex.Message}");
                return null;
            }
        }

        // Preview the first few lines of a text file
        private string PreviewTextFile(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            using (var reader = new StreamReader(stream))
            {
                // Read the first 500 characters or so
                char[] buffer = new char[500];
                int readCount = reader.Read(buffer, 0, buffer.Length);
                return new string(buffer, 0, readCount) + (readCount == 500 ? "..." : "");
            }
        }


        //comment
        public static void RenderFirstPageFromFtp(string ftpUrl, string ftpUsername, string ftpPassword, string outputImagePath)
        {
            try
            {
                // Create the FTP request to get the file stream
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                // Get the FTP response stream
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Load the PDF directly from the stream
                    using (var pdfDocument = new Aspose.Pdf.Document(responseStream))
                    {
                        if (pdfDocument.Pages.Count == 0)
                            throw new Exception("The PDF document has no pages.");

                        var page = pdfDocument.Pages[1];
                        using (MemoryStream imageStream = new MemoryStream())
                        {
                            var resolution = new Aspose.Pdf.Devices.Resolution(300);
                            var pngDevice = new Aspose.Pdf.Devices.PngDevice(resolution);

                            pngDevice.Process(page, imageStream);
                            using (var image = System.Drawing.Image.FromStream(imageStream))
                            {
                                image.Save(outputImagePath, System.Drawing.Imaging.ImageFormat.Png);
                            }
                        }
                    }
                }
                Console.WriteLine("First page rendered successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


    }
}
