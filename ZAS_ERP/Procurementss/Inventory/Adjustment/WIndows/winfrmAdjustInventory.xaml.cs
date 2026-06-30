using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
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

namespace ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows
{
    /// <summary>
    /// Interaction logic for winfrmAdjustInventory.xaml
    /// </summary>
    public partial class winfrmAdjustInventory : DXWindow
    {
        public int OrderId;
        public int editOrder;
        Company company = new Company();
        Department department = new Department();
        List<ERP_BL.Procurements.Inventories.Inventory> inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();
        InventoryAdjustment adjustment = new InventoryAdjustment();
        UsersRepo UsersRepo = new UsersRepo();
        AdjustmentRepo adjustmentRepo = new AdjustmentRepo();
        ProductRepo productrepo = new ProductRepo();
        public InventoryAdjustmentStatus checkStatus = new InventoryAdjustmentStatus();
        public InventoryAdjustmentStatus oldStatus = new InventoryAdjustmentStatus();
        UsersRepo _usersRepo = new UsersRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public winfrmAdjustInventory()
        {
            InitializeComponent();
            inventories= new List<ERP_BL.Procurements.Inventories.Inventory>();
            grdInventoryItem.ItemsSource = inventories;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lookupCompany.SelectedIndex == -1)
                {
                    lookupCompany.Focus();
                    MessageBox.Show("Please Select a company against adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }
                else
                if (lookupDepartment.SelectedIndex == -1)
                {
                    lookupCompany.Focus();
                    MessageBox.Show("Please Select a department against adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else
                if (cmbEmployee.SelectedIndex == -1)
                {
                    cmbEmployee.Focus();
                    MessageBox.Show("Please Select a creator against adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else
                if (cmbAdjustmentType.SelectedIndex == -1)
                {
                    cmbEmployee.Focus();
                    MessageBox.Show("Please Select a adjustment type adjustment", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else
                  if (string.IsNullOrEmpty(txtAdjustRef.Text))
                {
                    MessageBox.Show("Please add adjustment reference #", "Required field", MessageBoxButton.OK, MessageBoxImage.Stop);

                }
                else
                if (cmbCurrency.SelectedIndex == -1)
                {
                    cmbEmployee.Focus();
                    MessageBox.Show("Please Select adjustment currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else
                if (lookupReceivableAccounts.SelectedIndex == -1)
                {
                    lookupReceivableAccounts.Focus();
                    MessageBox.Show("Please Select adjustment account", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }
                else
                {
                    UsersRepo repo = new UsersRepo();
                    adjustment.ReferenceNo = txtAdjustRef.Text;
                    DateTime? dateTime = null;
                    adjustment.ApprovedDate = (datAdjustdate.Text == "") ? dateTime : datAdjustdate.DateTime;
                    adjustment.company_Id = (lookupCompany.SelectedItem as Company).Id;
                    adjustment.depId_Id = (lookupDepartment.SelectedItem as Department).Id;
                    var user = repo.GetbyEmpId(Convert.ToInt32((cmbEmployee.SelectedItem as cmbitem).id));
                    adjustment.creator_Id = user.id;
                    adjustment.AdjustmentType = (AdjustmentType)cmbAdjustmentType.SelectedIndex;
                    adjustment.AdjustmentDate = (DateTime)datAdjustdate.EditValue;
                    adjustment.CreationDate = (DateTime)datAdjustdate.EditValue;
                    adjustment.chartofAccount_Id = (lookupReceivableAccounts.SelectedItem as ChartofAccount).Id;
                    adjustment.currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                    if ((cmbStatus.SelectedItem as cmbitem) != null)
                    {
                        InventoryAdjustmentStatus status = adjustmentRepo.getstatus((cmbStatus.SelectedItem as cmbitem).id);
                        adjustment.AdjustmentStatus = status;
                    }
                    if ((cmbStatus.SelectedItem as cmbitem) != null)
                    {

                        InventoryAdjustmentStatus status = adjustmentRepo.getstatus((cmbStatus.SelectedItem as cmbitem).id);
                        adjustment.AdjustmentStatus = status;
                    }
                    if ((AdjustmentType)cmbAdjustmentType.SelectedIndex == AdjustmentType.Quantity)
                    {
                        GetQuantityInventories(adjustment);
                    }
                    else
                    if ((AdjustmentType)cmbAdjustmentType.SelectedIndex == AdjustmentType.Amount)
                    {
                        GetAmountInventories(adjustment);
                    }
                    else
                    if ((AdjustmentType)cmbAdjustmentType.SelectedIndex == AdjustmentType.Quantity_and_Amount)
                    {
                        GetAmountAndQuantityInventories(adjustment);
                      
                    }
                    var myWindow = Window.GetWindow(this);
                    if (editOrder == 1 && OrderId != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inventory Adjustment") != null)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Adjustment without Approval") != null && adjustment.isApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Adjustment is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                adjustment.stage = TransactionStage.Approved.ToString();

                                adjustment.isApproved = true;
                                adjustment.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        if (checkStatus != null && checkStatus.Id != 0)
                        {
                            if (checkStatus.Id != adjustment.AdjustmentStatus.Id)
                            {
                                adjustment.LastStatusChangeDate = System.DateTime.Now;
                                if (adjustment.AdjustmentStatus.isActive != true)
                                {
                                    adjustment.ClosingDate = System.DateTime.Now;
                                }
                            }
                        }


                        if (checkStatus.Id != adjustment.AdjustmentStatus.Id)
                        {
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of Inventory Adjustment has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (department != null && department.Id != 0 && company?.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(_usersRepo.getusersByCompanyDepartment(department.Id, company.Id), adjustment.Id, TransactionItemType.InventoryAdjustment);
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
                            string newStat = adjustment.AdjustmentStatus.Status;
                            string symbolCurr = "";

                            if (adjustment.Currency != null)
                            {
                                symbolCurr = adjustment.Currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Adjustment having value: " + adjustment.Inventories.Sum(x => x.Debit).ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(adjustment.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var _user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, _user.id, "New Comment ", null);
                                }
                            }
                            if (ccUsers.Count != 0)
                            {
                                foreach (var _user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, _user.id, "New Comment ", null);
                                }
                            }

                            if (checkStatus != null && checkStatus.Id != 0)
                            {
                                _usersRepo.Add(TransactionInfo.Status_Changed, adjustment.Id, 2, "Status Changed from (" + checkStatus.Status + ") to (" + adjustment.AdjustmentStatus.Status + ")");
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Edited, adjustment.Id, 34, frmInputBox.comment);
                        }

                        adjustmentRepo.update(adjustment);
                        DXMessageBox.Show("Adjustment Updated Succesfully", "Congratulations");
                        myWindow.Close();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment") != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null)
                        {
                            adjustment.stage = TransactionStage.Approved.ToString();
                            adjustment.isApproved = true;
                            adjustment.ApprovedDate = System.DateTime.Now;
                        }
                        else
                        {
                            adjustment.stage = TransactionStage.AwaitingFirstReview.ToString();
                            adjustment.isApproved = false;
                        }
                        adjustmentRepo.Add(adjustment);
                        DXMessageBox.Show("Inventory Adjustment Added Succesfully", "Congratulations");
                                myWindow.Close();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        myWindow.Close();
                        return;
                    }
                
                }
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }
        public void GetQuantityInventories(InventoryAdjustment _adjustment)
        {


            if (editOrder == 0 || adjustment.Inventories.Count == 0)
            {
                adjustment.Inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();

                foreach (var inventory in grdInventoryItem.ItemsSource as List<ERP_BL.Procurements.Inventories.Inventory>)
                {
                    ERP_BL.Procurements.Inventories.Inventory Item = new ERP_BL.Procurements.Inventories.Inventory();
                    Item.creationDate = _adjustment.CreationDate;
                    Item.currencyId = _adjustment.currency_Id;
                    Item.adjustment_Id = _adjustment.Id;
                    Item.AmountOC = inventory.AmountOC;
                    Item.MER = inventory.AmountMER;
                    Item.AmountMER = inventory.AmountOC * inventory.MER;
                    Item.Quantity = inventory.Quantity;
                    Item.Weight = inventory.Weight;
                    Item.prodId = inventory.Product.Id;
                    Item.TransactionsType = InventoryTransactionsType.Adjustment;
                    adjustment.Inventories.Add(Item);
                }
            }
            else
            {
                 var list=grdInventoryItem.ItemsSource as List<ERP_BL.Procurements.Inventories.Inventory>;
            }
        }
        public void GetAmountInventories(InventoryAdjustment _adjustment)
        {
            if (editOrder == 0 || adjustment.Inventories.Count==0)
            {
                adjustment.Inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();
                foreach (var inventory in grdInventoryItem.ItemsSource as List<ERP_BL.Procurements.Inventories.Inventory>)
                {
                    ERP_BL.Procurements.Inventories.Inventory Item = new ERP_BL.Procurements.Inventories.Inventory();
                    Item.creationDate = (DateTime)datAdjustdate.EditValue;
                    Item.currencyId = _adjustment.currency_Id;
                    Item.adjustment_Id = _adjustment.Id;
                    Item.AmountOC = (-inventory.AmountOC);
                    Item.MER = inventory.AmountMER;
                    Item.AmountMER = inventory.AmountOC * inventory.MER;
                    Item.Quantity = inventory.Quantity;
                    Item.Weight = inventory.Weight;
                    Item.prodId = inventory.Product.Id;
                    Item.TransactionsType = InventoryTransactionsType.Adjustment;
                    adjustment.Inventories.Add(Item);
                }
            }
        }
        public  void GetAmountAndQuantityInventories(InventoryAdjustment _adjustment)
        {
            if (editOrder == 0 || adjustment.Inventories.Count == 0)
            {
                adjustment.Inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();

                foreach (var inventory in grdInventoryItem.ItemsSource as List<ERP_BL.Procurements.Inventories.Inventory>)
                {
                    ERP_BL.Procurements.Inventories.Inventory Item = new ERP_BL.Procurements.Inventories.Inventory();
                    Item.creationDate = _adjustment.CreationDate;
                    Item.currencyId = _adjustment.currency_Id;
                    Item.adjustment_Id = _adjustment.Id;
                    Item.AmountOC = inventory.AmountOC;
                    Item.companyId = (lookupCompany.SelectedItem as Company).Id;
                    Item.deptId = (lookupDepartment.SelectedItem as Department).Id;
                    Item.MER = inventory.MER;
                    Item.MER = inventory.AmountMER;
                    Item.AmountMER = inventory.AmountOC * inventory.MER;
                    Item.Quantity = inventory.Quantity;
                    Item.Weight = inventory.Weight;
                    Item.UnitRate = inventory.UnitRate;
                    Item.prodId = inventory.Product.Id;
                    Item.AverageCost = inventory.AverageCost;
                    Item.userId = adjustment.creator_Id;
                    Item.transactionRefno = "Adjustment Entry";
                    Item.TransactionsType = InventoryTransactionsType.Adjustment;
                    adjustment.Inventories.Add(Item);
                }
            }
        }
        private void cmbAdjustmentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0 )
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), TransactionItemType.InventoryAdjustment);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 )
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.InventoryAdjustment);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (adjustment != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && adjustment.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(adjustment.Id, TransactionItemType.InventoryAdjustment, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }

                        //if (frmInputBox.Comment.TaggedList.Count > 0)
                        //{
                        //    var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                        //    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                        //    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                        //}
                    }

                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (adjustment.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Inventory Adjustment first to add a comment!");
                }

            }
            loadcomments();
        }
        public void loadcomments()
        {
            try
            {
                if (adjustment != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(adjustment.Id, TransactionItemType.InventoryAdjustment);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            ProcurementRepo procurementRepo = new ProcurementRepo();

            bool isFullyPaid = true;
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment without Approval") != null) ? true : false)
            {
                UsersRepo usersRepo = new UsersRepo();
             
                adjustment = adjustmentRepo.get(adjustment.Id);

              



              

                var row = adjustment;

                if (row.AdjustmentStatus != null)
                {
                    oldStatus = row.AdjustmentStatus;
                }
                Adjustment.UserControls.ucStatusChange.inActiveStatuses = 1;

                Adjustment.UserControls.ucStatusChange.adjustmentId = (int)adjustment.Id;
                Adjustment.WIndows.frmInventoryAdjustmentStatusChange statusChange = new Adjustment.WIndows.frmInventoryAdjustmentStatusChange(adjustmentRepo);


        var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();
                if  (Adjustment.UserControls.ucStatusChange.adjustment.Id != 0)
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inventory Adjustment") != null) ? true : false)
                    {
                        Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing = false;
                        Adjustment.UserControls.ucStatusChange.adjustment.stage = TransactionStage.Approved.ToString();

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.AdjustmentStatus.Status + ") to (" + Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, Adjustment.UserControls.ucStatusChange.adjustment.Id, (int)TransactionItemType.InventoryAdjustment, frmInputBox.comment);

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inventory Adjustment") != null)
                    {
                        Adjustment.UserControls.ucStatusChange.adjustment.stage = TransactionStage.AwaitingApproval.ToString();
                        if (Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing == null)
                        {
                            Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.AdjustmentStatus.Status + ") to (" + Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Adjustment.UserControls.ucStatusChange.adjustment.Id, (int)TransactionItemType.InventoryAdjustment, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inventory Adjustment") != null)
                    {
                        Adjustment.UserControls.ucStatusChange.adjustment.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing != true)
                        {
                            Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing = true;

                        }

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.AdjustmentStatus.Status + ") to (" + Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Adjustment.UserControls.ucStatusChange.adjustment.Id, (int)TransactionItemType.InventoryAdjustment, frmInputBox.comment);
                    }
                    else
                    {
                        Adjustment.UserControls.ucStatusChange.adjustment.stage = TransactionStage.AwaitingFirstReview.ToString();

                        Adjustment.UserControls.ucStatusChange.adjustment.PendingForClosing = true;
                        usersRepo.Add(TransactionInfo.Closed, Adjustment.UserControls.ucStatusChange.adjustment.Id, (int)TransactionItemType.InventoryAdjustment, frmInputBox.comment);
                    }
                Adjustment.UserControls.ucStatusChange.adjustment.LastStatusChangeDate = System.DateTime.Now;
                Adjustment.UserControls.ucStatusChange.adjustment.ClosingDate = System.DateTime.Now;
                if (row.AdjustmentStatus != Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus)
                    usersRepo.Add(TransactionInfo.Status_Changed, adjustment.Id, (int)TransactionItemType.InventoryAdjustment, "While direct closing Status Changed from (" + row.AdjustmentStatus.Status + ") to (" + Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status + ")");
                if (oldStatus.Status != Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status)
                {
                    UsersRepo userRepo = new UsersRepo();
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Inventory Adjustment has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.Department != null && row.Department.Id != 0 && row.Company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.Department.Id, row.Company.Id), row.Id, TransactionItemType.InventoryAdjustment);
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
                    string newStat = Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status;
                    string symbolCurr = "";
                    if (row.Currency != null)
                    {
                        symbolCurr = row.Currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Adjustment (Amount OC) having value: " + row.Inventories.Sum(x=>x.Debit).ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);

                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + row.ReferenceNo, row.Id, TransactionItemType.InventoryAdjustment, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + row.ReferenceNo, row.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                }




                row = Adjustment.UserControls.ucStatusChange.adjustment;

                adjustmentRepo.updateStatus(row.AdjustmentStatus);
                MessageBox.Show("Adjustment status changed to InActive (" + Adjustment.UserControls.ucStatusChange.adjustment.AdjustmentStatus.Status + ")");
                myWindow = Window.GetWindow(this);
                myWindow.Close();


            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Inventory Adjustment Directly.");
            }
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editOrder == 1 && adjustment != null)
                {

                    adjustmentRepo = new AdjustmentRepo();
                    adjustment = new InventoryAdjustment();
                    adjustment = adjustmentRepo.get(adjustment.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (adjustment != null)
                    {
                        if (adjustment.isApproved == true)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Inventory Adjustment is Approved, Do you want to UnApprove this Inventory Adjustment?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    adjustment.isApproved = false;
                                    adjustment.stage = TransactionStage.AwaitingApproval.ToString();
                                    adjustmentRepo.Approve(adjustment);
                                    var res1 = MessageBox.Show("Adjustment has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), adjustment.Id, TransactionItemType.InventoryAdjustment);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                //if (adjustment.transactionHolderId != win.tagUsers[0].employeeId)
                                                //{
                                                //    adjustment.holderChangeDate = DateTime.Now;
                                                //}
                                                //saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                //saleOrderRepo.update(saleOrder);
                                            }
                                        }
                                        else if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), adjustment.Id, TransactionItemType.InventoryAdjustment);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            //if (win.tagUsers.Count != 0)
                                            //{
                                            //    if (adjustment.transactionHolderId != win.tagUsers[0].employeeId)
                                            //    {
                                            //        adjustment.holderChangeDate = DateTime.Now;
                                            //    }
                                            //    adjustment.transactionHolderId = win.tagUsers[0].employeeId;
                                            //    saleOrderRepo.update(saleOrder);
                                            //}

                                            ccUsers = win.ccUsers;
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";
                                    if (adjustment.Currency != null)
                                    {
                                        symbolCurr = adjustment.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog();

                                
                                        comment.Comment = "Adjustment (Amount OC) having value: " + adjustment.Inventories.Sum(x=>x.Debit).ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "Adjustment UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                    
                                    procurementRepo.Add(adjustment.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Inventory Adjustments are UnApproved (" + adjustment.ReferenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Inventory Adjustment is UnApproved (" + adjustment.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Inventory Adjustment Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Inventory Adjustment Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (adjustment.isApproved == false)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inventory Adjustment") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();
                                var res = MessageBox.Show("Inventory Adjustment are Pending for Approval, Do you want to Approve this Inventory Adjustment?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    adjustment.isApproved = true;
                                    adjustment.stage = TransactionStage.Approved.ToString();
                                    adjustmentRepo.Approve(adjustment);

                                    var res1 = MessageBox.Show("Inventory Adjustment has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id}), adjustment.Id, TransactionItemType.InventoryAdjustment);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                //if (adjustment.transactionHolderId != win.tagUsers[0].employeeId)
                                                //{
                                                //    adjustment.holderChangeDate = DateTime.Now;
                                                //}
                                                //adjustment.transactionHolderId = win.tagUsers[0].employeeId;
                                                adjustmentRepo.update(adjustment);
                                            }
                                        }
                                        else if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), adjustment.Id, TransactionItemType.InventoryAdjustment);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            //if (win.tagUsers.Count != 0)
                                            //{
                                            //    if (adjustment.transactionHolderId != win.tagUsers[0].employeeId)
                                            //    {
                                            //        adjustment.holderChangeDate = DateTime.Now;
                                            //    }
                                            //    adjustment.transactionHolderId = win.tagUsers[0].employeeId;
                                            //    adjustmentRepo.update(saleOrder);
                                            //}
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }

                                    }

                                    string symbolCurr = "";
                                    if (adjustment.Currency != null)
                                    {
                                        symbolCurr = adjustment.Currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Inventory Adjustment (Amount OC) having value: " + adjustment.Inventories.Sum(x=>x.Debit).ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "Inventory Adjustment Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(adjustment.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inventory Adjustment #" + adjustment.ReferenceNo, adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Inventory Adjustment is Approved (" + adjustment.ReferenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Inventory Adjustment is Approved (" + adjustment.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Inventory Adjustment Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Inventory Adjustment Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }


                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    //Load_Receipts();

                }

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inventory Adjustment") != null)
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
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (adjustment.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inventory Adjustments") != null))
            {

                if (DXMessageBox.Show("This Adjustment is currently in the list of Void Adjustment's! Do you want to remove it from Void?", "Remove Void Adjustment", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    adjustment.isVoid = false;
                    adjustmentRepo.setAdjustmenttoVoid(adjustment.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Adjustment has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { adjustment.Department.Id }, new List<int> { adjustment.Company.Id }), adjustment.Id, TransactionItemType.InventoryAdjustment);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(adjustment.Department.Id, adjustment.Company.Id), adjustment.Id, TransactionItemType.InventoryAdjustment);
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
                    if (adjustment.Currency != null)
                    {
                        symbolCurr = adjustment.Currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Adjustmet (Amount OC) having value: " + adjustment.Inventories.Sum(x=>x.AmountOC).ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Adjustmet UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(adjustment.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + adjustment.Inventories.Sum(x=>x.AmountOC), adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + adjustment.Inventories.Sum(x=>x.AmountOC), adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inventory Adjustments") != null)
            {
                if (DXMessageBox.Show("This Adjustment is not currently in the list of Void Adjustment's! Do you want to move it to Void Adjustments?", "Add to Void Adjustments", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    adjustment.isVoid = true;
                    adjustmentRepo.setAdjustmenttoVoid(adjustment.Id, true);

                    grdVoid.Visibility = Visibility.Visible;


                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Adjustment has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { adjustment.Department.Id }, new List<int> { adjustment.Company.Id }), adjustment.Id, TransactionItemType.InventoryAdjustment);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else if (adjustment.Department != null && adjustment.Department.Id != 0 && adjustment.Company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(adjustment.Department.Id, adjustment.Company.Id), adjustment.Id, TransactionItemType.InventoryAdjustment);
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
                    if (adjustment.Currency != null)
                    {
                        symbolCurr = adjustment.Currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Adjustment (Amount(OC)) having value: " + adjustment.Inventories.Sum(x => x.AmountOC).ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "Adjustment Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(adjustment.Id, TransactionItemType.InventoryAdjustment, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + adjustment.Inventories.Sum(x => x.AmountOC).ToString(), adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Adjustment #" + adjustment.Inventories.Sum(x => x.AmountOC).ToString(), adjustment.Id, TransactionItemType.InventoryAdjustment, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }
        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompanies();
            LoadDepartments();
            LoadCurrencies();
            LoadAdjustmentTypes();
            loadStatuses();
            SelectAdjustmentType();
            if (editOrder == 1 && OrderId != 0)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inventory Adjustments") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inventory Adjustments") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                else
                {
                    btnSetVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inventory Adjustment") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inventory Adjustments") == null)
                {
                    adjustment = adjustmentRepo.get(OrderId);
                    //views = _usersRepo.getViwerInfo(OrderId, 7);
                    //grdUsers.ItemsSource = views;
                    LoadInventoryAdjustmentData();
                    //GellAllOrdersTracking();

                    //loadcomments();
                    //btnSave.IsEnabled = false;
                    if (adjustment.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Inventory Adjustment") != null)
                    {
                        btnSave.IsEnabled = true;
                    }


                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inventory Adjustment") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Inventory Adjustment") != null)
                {
                    adjustment = adjustmentRepo.get(OrderId);
                    LoadInventoryAdjustmentData();
                    //GellAllOrdersTracking();
                    //views = usersRepo.getViwerInfo(purchaseInvoice.Id, 7);
                    //grdUsers.ItemsSource = views;
                    //loadcomments();
                    btnSave.IsEnabled = true;

                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inventory Adjustment") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inventory Adjustment") != null)
                {
                    adjustment = adjustmentRepo.get(OrderId);

                    LoadInventoryAdjustmentData();
                    //GellAllOrdersTracking();
                    //views = usersRepo.getViwerInfo(purchaseInvoice.Id, 7);
                    //grdUsers.ItemsSource = views;
                    //loadcomments();

                    btnSave.IsEnabled = true;

                }
                //Setting void stamp
                if (adjustment != null)
                {
                    if (adjustment.isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Inventory Adjustment!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }

            }
           
        }

        public void SelectAdjustmentType()
        {
            cmbAdjustmentType.SelectedIndex = 2;
        }
        public void LoadInventoryAdjustmentData()
        {
           if(adjustment.Id!=0)
            {
                if (adjustment.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (adjustment.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";\
                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (adjustment.isApproved == true && adjustment.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (adjustment.isApproved == true && adjustment.AdjustmentStatus.isActive == false && adjustment.PendingForClosing != true &&  adjustment.PendingForClosing != null)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (adjustment.isApproved == true && adjustment.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (adjustment.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (adjustment.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (adjustment.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                checkStatus = adjustment.AdjustmentStatus;


                if (adjustment.company_Id!=null)
                {
                    lookupCompany.Text = adjustment.Company.CompanyName;
                }
                if (adjustment.depId_Id != null)
                {
                    lookupDepartment.Text = adjustment.Department.DeptName;
                }
                if (adjustment.creator_Id != 0 || adjustment.Creator != null)
                {
                    try
                    {
                        var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                        cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == adjustment.Creator.employeeId))];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (!string.IsNullOrEmpty(adjustment.ReferenceNo))
                {
                    txtAdjustRef.Text = adjustment.ReferenceNo;
                }
                cmbAdjustmentType.SelectedIndex = Convert.ToInt32(adjustment.AdjustmentType);
                if (adjustment.CreationDate!=null)
                {
                    datAdjustdate.EditValue = adjustment.CreationDate.Value;
                }
                var adjustmentSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;

                if (adjustment.AdjustmentStatus != null)
                    if (adjustment.AdjustmentStatus.isActive == false)
                    {
                        try
                        {
                            cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(adjustmentSource.Find(x => x.name == adjustment.AdjustmentStatus.Status))];
                        }
                        catch (Exception ex)
                        {
                            SystemLog.LogError(this.GetType(), "This User cannot see closed Inventory Adjustment status! " + ex.ToString());
                        }
                    }
                    else
                    {
                        cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(adjustmentSource.Find(x => x.name == adjustment.AdjustmentStatus.Status))];
                    }
                if (adjustment.chartofAccount_Id != null)
                {
                    lookupReceivableAccounts.Text = adjustment.ChartofAccount.accountName;
                }
                if (adjustment.currency_Id != 0 && adjustment.Currency != null)
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == adjustment.currency_Id))];
                }
                else
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == adjustment.Company.CurrencyId))];
                }
                if(adjustment.Inventories.Count>0)
                {
                    grdInventoryItem.ItemsSource = adjustment.Inventories;
                }
            }
        }
        public void loadStatuses()
        {
            List<InventoryAdjustmentStatus> InventoryAdjustmentStatuses = new List<InventoryAdjustmentStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Inventory Adjustment") != null)
            {
                InventoryAdjustmentStatuses = adjustmentRepo.getAllInventoryAdjustmentStatus();
            }
            else

                InventoryAdjustmentStatuses = adjustmentRepo.getAllActiveInventoryAdjustmentStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            Parallel.ForEach(InventoryAdjustmentStatuses, delegate (InventoryAdjustmentStatus status)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            });
            cmbStatus.ItemsSource = cmbitems;
        }
        public void LoadAdjustmentTypes()
        {
            List<cmbitem> items = new List<cmbitem>();
            for (int i = 0; i <= (int)ERP_BL.Enums.AdjustmentType.Quantity_and_Amount; i++)
            {
                if(i==0)
                {
                    cmbitem item = new cmbitem();
                    item.id = i;
                    item.name = "Quantity";
                    items.Add(item);

                }
                else
                    if(i==1)
                {
                    cmbitem item = new cmbitem();
                    item.id = i;
                    item.name = "Amount";
                    items.Add(item);
                }
                else
                    if (i == 2)
                {
                    cmbitem item = new cmbitem();
                    item.id = i;
                    item.name = "Quantity_and_Amount";
                    items.Add(item);
                }
                cmbAdjustmentType.ItemsSource=items;
            }
        }
        public void LoadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }
        public void LoadCompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
            }
            var currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupCompany.ItemsSource = currentUserCompanies;
        }
        public void LoadDepartments()
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
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsSaleOrderType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    lookupDepartment.ItemsSource = departments;
                  
                }
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            LoadDepartments();
            LoadChartofAccounts();
        }
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                ProductRepo productRepo = new ProductRepo();
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupInventoryProductsinGrid.ItemsSource = products;
                loademployees();
                if (department.employees.Count == 0)
                {
                    MessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
            }
            LoadChartofAccounts();

        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            //  employees = cont1.GetEmployees();
            employees = department.employees;

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbEmployee.ItemsSource = cmbitems;
        }

        private void cmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbEmployee.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbEmployee.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Employeess.frmEmployeeAdd employeeadd = new Employeess.frmEmployeeAdd();
                    employeeadd.ShowDialog();
                    loademployees();
                 

                }
            }
            LoadChartofAccounts();
        }
        public void LoadChartofAccounts()
        {
            ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
            if (lookupCompany.SelectedIndex > -1 && lookupDepartment.SelectedIndex > -1 && cmbEmployee.SelectedIndex > -1)
            {
                var userChartofAccounts = chartofAccountsRepo.GetAllInventoryAdjustments(lookupCompany.SelectedItem as Company, lookupDepartment.SelectedItem as Department, (cmbEmployee.SelectedItem as cmbitem).id);
                lookupReceivableAccounts.ItemsSource = userChartofAccounts;
            }
        }

        private void grdInventoryItem_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "OnHand")
            {

                var inventory = grdInventoryItem.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.Inventories.Inventory;
                var product = (grdInventoryItem.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.Inventories.Inventory).Product;
                if(product!=null)
                if (product.Inventories != null && product.Inventories.Count != 0)
                {
                    var purchasees = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && (DateTime)datAdjustdate.EditValue < x.PurchaseInvoice.CreationDate.Value && x.PurchaseInvoice?.isVoid != true);
                        purchasees = purchasees.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                            
                    var sales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && (DateTime)datAdjustdate.EditValue < x.SaleInvoice.CreationDate.Value && x.SaleInvoice?.isVoid!=true);
                        sales = sales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        var purchaseQuantity = purchasees.Sum(x => x.Quantity);
                    var saleQuantity = sales.Sum(x => x.Quantity);


                    var adjustmentQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid!=true && (DateTime)datAdjustdate.EditValue < x.InventoryAdjustment?.CreationDate.Value).Sum(x => x.Quantity);
                    purchaseQuantity = purchaseQuantity + adjustmentQuantity;

                    purchaseQuantity = purchaseQuantity + saleQuantity;
                    e.Value = purchaseQuantity;
                }
            }
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            try
            {
                var row = e.Row as ERP_BL.Procurements.Inventories.Inventory;
                row.AmountMER = row.MER * row.AmountOC;
                var purchases = productrepo.getPurchases(row.Product.Id);
                purchases = purchases.Where(x => x.creationDate < (DateTime)datAdjustdate.EditValue).ToList();
                purchases = purchases.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                var adjustments = adjustmentRepo.GetbyProductId(row.Product.Id, (DateTime)datAdjustdate.EditValue);
                var sumAdjustmentsAmount = adjustments.Where(x=>x.InventoryAdjustment.isVoid!=true) .Sum(x => x.AmountOC);
                var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                var sales = productrepo.getSales(row.Product.Id);
                sales = sales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                sales = sales.Where(x => x.creationDate < (DateTime)datAdjustdate.EditValue).ToList();
                var sumPurchase = purchases.Sum(x => x.AmountOC);
                sumPurchase = sumPurchase + sumAdjustmentsAmount;


                var sumSales = sales.Sum(x => x.AmountOC);
                var qSumPurchases = purchases.Sum(x => x.Quantity);
                qSumPurchases = qSumPurchases + sumAdjustmentQuantity;

                var qSumSales = sales.Sum(x => x.Quantity);
                var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);

                row.UnitRate = (row.AmountOC) / (row.Quantity);
                if (Double.IsNaN(averageCost) || Double.IsInfinity(averageCost))
                {
                    row.AverageCost = row.UnitRate;
                    //row.UnitRate = averageCost;
                    //row.AmountMER = row.AmountOC * row.MER;
                    //row.AmountOC = row.AverageCost * row.Quantity;
                }
                else
                {
                    row.AverageCost = averageCost;
                    //row.UnitRate = averageCost;
                    //row.MER = 1;
                    //row.AmountMER = row.AmountOC*row.MER;
                    //row.AmountOC = row.AverageCost * row.Quantity;
                }
            }
            catch (Exception)
            {
                
         
            }
            //if (averageCost == 0 || remainingAmountInven == 0 && remaiInvenQuantity == 0)
            //{
            //    averageCost = Math.Round(procurementProduct.unit * procurementProduct.siQuantity / procurementProduct.siQuantity, 2);
            //}
        }
    }
}
