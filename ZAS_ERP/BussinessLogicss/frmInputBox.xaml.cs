using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmInputBox.xaml
    /// </summary>
    /// 
    public partial class frmInputBox : Window
    {
        public static string comment;
        public static int? FlagId = null;
        public static bool FlagForTag = true;
        public static bool FlagForCC = false;
        public static bool FlagForRecomended = false;
        //public List<User> taggedUsers = new List<User>();
        //public  List<User> CCUsers = new List<User>();

        public static CommentLog Comment;
        TransactionItemType transactionItemType;
        public  List<ERP_BL.Databases.Employee> Employees{ get; set; }
        public ERP_BL.Databases.Employee Assignee { get; set; }
        SaleOrder saleOrder = new SaleOrder();
        public  static string  billReference;
        public static string  poReference ;
        public bool editFlag = false;
        public static bool commentAdded = false;
        User to = new User();
        CommentLog replyComment = null;
        public frmInputBox()
        {
            InitializeComponent();
            Comment = new CommentLog();
            comment = "";
            lblMessage.Text = "Write a comment Below";
            this.Title = "Comments";
            btnRecomendation.Visibility = Visibility.Collapsed;
        }
        public frmInputBox(string message, string title)
        {
            InitializeComponent();
            lblMessage.Text = message;
            this.Title = title;
            comment = "";

        }
        public frmInputBox(string _comment)
        {
            InitializeComponent();
            lblMessage.Text = "Write a comment Below";
            this.Title = "Comments";
            comment = _comment;
            txtComment.Text = _comment;

        }
      
        public frmInputBox(string message, string title, bool Allowtag, List<User> Users, TransactionItemType _transactionItemType)
        {
            Comment = new CommentLog();

            InitializeComponent();
            lblMessage.Text = message;
            this.Title = title;
            comment = "";
            transactionItemType = _transactionItemType;

            //checkBoxesPanel.Visibility = Visibility.Visible;
            lblFlag.Visibility = Visibility.Visible;
            cmbNotificationFlag.Visibility = Visibility.Visible;
            lblAsignee.Visibility = Visibility.Visible;
            lblCategory.Visibility = Visibility.Visible;
            cmbEmployee.Visibility = Visibility.Visible;
            cmbCategories.Visibility = Visibility.Visible;
            lblSubject.Visibility = Visibility.Visible;
            txtSubject.Visibility = Visibility.Visible;
            if (Allowtag && Users!=null)
            {
                lblTagUser.Visibility = Visibility.Visible;
                cmbTagUsers.Visibility = Visibility.Visible;
                lblCCUser.Visibility = Visibility.Visible;

                cmbCCUsers.Visibility = Visibility.Visible;
                grdRecomendation.Visibility = Visibility.Visible;
                cmbCCUsers.Visibility = Visibility.Visible;


                btnClearTagUser.Visibility = Visibility.Visible;
                loadusers(Users);
               
            }
            else
            {
                //taggedUsers = new List<User>();
            }
            LoadCategories();
            LoadFlags();

            btnRecomendation.Visibility = Visibility.Collapsed;


        }
        public frmInputBox(string message, string title, bool Allowtag, List<User> Users, TransactionItemType _transactionItemType, SaleOrder _saleOrder)
        {
            Comment = new CommentLog();

            InitializeComponent();
            lblMessage.Text = message;
            this.Title = title;
            comment = "";
            transactionItemType = _transactionItemType;

            //checkBoxesPanel.Visibility = Visibility.Visible;
            lblFlag.Visibility = Visibility.Visible;
            cmbNotificationFlag.Visibility = Visibility.Visible;
            lblAsignee.Visibility = Visibility.Visible;
            lblCategory.Visibility = Visibility.Visible;
            cmbEmployee.Visibility = Visibility.Visible;
            cmbCategories.Visibility = Visibility.Visible;
            lblSubject.Visibility = Visibility.Visible;
            txtSubject.Visibility = Visibility.Visible;
            if (Allowtag && Users != null)
            {
                lblTagUser.Visibility = Visibility.Visible;
                cmbTagUsers.Visibility = Visibility.Visible;
                lblCCUser.Visibility = Visibility.Visible;
                grdRecomendation.Visibility = Visibility.Visible;
                cmbCCUsers.Visibility = Visibility.Visible;

                btnClearTagUser.Visibility = Visibility.Visible;
                 loadusers(Users);
            }
            else
            {
                //taggedUsers = new List<User>();
            }
            LoadCategories();
            LoadFlags();
            saleOrder = _saleOrder;
            if (transactionItemType == TransactionItemType.CostCenter)
            {
                loadReferences();
                grdReferences.Visibility = Visibility.Visible;
            }
            else
                grdReferences.Visibility = Visibility.Collapsed;


        }
        private void loadReferences()
        {
            List<String> poReferences = new List<string>();
            List<String> billReferences = new List<string>();
            if (saleOrder.Id != 0 && saleOrder != null)
            {
                if (saleOrder.PurchaseOrders != null)
                {
                    if (saleOrder.PurchaseOrders.Count != 0)
                    {
                        poReferences = saleOrder.PurchaseOrders.Where(x => x.isVoid != true && x.SyetmReferenceNo != null).Select(x => x.SyetmReferenceNo).ToList();
                    }
                }
                if (saleOrder.Bills != null)
                {
                    if (saleOrder.Bills.Count != 0)
                    {
                        billReferences = saleOrder.Bills.Where(x => x.isVoid != true && x.SyetmReferenceNo != null).Select(x => x.SyetmReferenceNo).ToList();
                    }
                    cmbBillReferences.ItemsSource = billReferences;
                    cmbPOReferences.ItemsSource = poReferences;
                }
            }
            

        }
        public frmInputBox(string message, string title, bool Allowtag, List<User> Users,string replyComment)
        {
            InitializeComponent();
            Comment = new CommentLog();

            lblMessage.Text = message;
            this.Title = title;
            comment = "";
            txtComment.Text = replyComment;
            if (Allowtag && Users != null)
            {
                lblTagUser.Visibility = Visibility.Visible;
                cmbTagUsers.Visibility = Visibility.Visible;
            
                btnClearTagUser.Visibility = Visibility.Visible;
                loadusers(Users);
            }
            else
            {
                //taggedUsers = new List<User>();
            }
        }
        public frmInputBox(string message, string title, bool Allowtag, List<User> Users, CommentLog _replyComment, TransactionItemType _transactionItemType)
        {
            InitializeComponent();
            Comment = new CommentLog();
            replyComment = _replyComment;

            txtSubject.Text = replyComment.Subject;
            lblMessage.Text = message;
            this.Title = title;
            comment = "";
            transactionItemType = _transactionItemType;
            txtReply.Text = replyComment.Comment;
            Comment.ReplyCommentId = replyComment.Id;
            //checkBoxesPanel.Visibility = Visibility.Visible;



            lblFlag.Visibility = Visibility.Visible;
            cmbNotificationFlag.Visibility = Visibility.Visible;
            lblAsignee.Visibility = Visibility.Visible;
            lblCategory.Visibility = Visibility.Visible;
            cmbEmployee.Visibility = Visibility.Visible;
            cmbCategories.Visibility = Visibility.Visible;
            lblSubject.Visibility = Visibility.Visible;
            txtSubject.Visibility = Visibility.Visible;
            //if(replyComment.manager!=null)
            //{
            //    lookupSales.Text = replyComment.manager.person.FName;
            //    var image = GetBitmapImageFromByteArray(replyComment.manager.person.Photo);
            //    managerImg.Source = image;
            //}
            //if (replyComment.salesPerson != null)
            //{
            //    lookupSales.Text = replyComment.salesPerson.person.FName;
            //    var image = GetBitmapImageFromByteArray(replyComment.salesPerson.person.Photo);
            //    salesImg.Source = image;
            //}
            //if (replyComment.financePerson != null)
            //{
            //    lookupSales.Text = replyComment.financePerson.person.FName;
            //    var image = GetBitmapImageFromByteArray(replyComment.financePerson.person.Photo);
            //    financeImg.Source = image;
            //}
            if (!string.IsNullOrWhiteSpace(replyComment.Comment))
            {
                lblReply.Visibility = Visibility.Visible;
                txtReply.Visibility = Visibility.Visible;
            }
            if (Allowtag && Users != null)
            {
                lblTagUser.Visibility = Visibility.Visible;
                cmbTagUsers.Visibility = Visibility.Visible;
                lblCCUser.Visibility = Visibility.Visible;
                grdRecomendation.Visibility = Visibility.Visible;
                cmbCCUsers.Visibility = Visibility.Visible;
                btnClearTagUser.Visibility = Visibility.Visible;
                loadusers(Users);
            }
            else
            {
                //taggedUsers = new List<User>();
            }
            LoadCategories();
            LoadFlags();
            
            
            if (replyComment.CategoryId != 0 && replyComment.Category != null)
            {
                var incoSource = (List<cmbitem>)cmbCategories.Items.SourceCollection;
                cmbCategories.SelectedItem = cmbCategories.Items[cmbCategories.Items.IndexOf(incoSource.Find(x => x.id == replyComment.CategoryId))];

            }
            if (replyComment.AssigneeId != 0 && replyComment.Assignee != null)
            {
                
                var incoSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                var index = cmbEmployee.Items.IndexOf(incoSource.Find(x => x.id == replyComment.AssigneeId));
                if(index>-1)
                cmbEmployee.SelectedItem = cmbEmployee.Items[index];

            }
            var taggedRecommendationusersSource = cmbTagRecomendUsers.ItemsSource as List<User>;
            foreach (User cmbitem in taggedRecommendationusersSource)
            {
                if (replyComment.TaggedRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                {
                    cmbTagRecomendUsers.SelectedItems.Add(cmbitem);
                    //break;
                }
            }
            var CCRecomenndationUsersSource = cmbCCRecomendUsers.ItemsSource as List<User>;
            foreach (User cmbitem in CCRecomenndationUsersSource)
            {
                if (replyComment.CCRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                {
                    cmbCCRecomendUsers.SelectedItems.Add(cmbitem);
                    //break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            EmployeeRepo empRepo = new EmployeeRepo();
            var allEmployees=empRepo.GetAllActiveEmployees();
            //lookupManager.ItemsSource = allEmployees.Where(x => x.empFunction?.functionType == FunctionType.Other).ToList();
            //lookupSales.ItemsSource = allEmployees.Where(x => x.empFunction?.functionType == FunctionType.Sales).ToList();
            //lookupFinance.ItemsSource = allEmployees.Where(x => x.empFunction?.functionType == FunctionType.Finance).ToList();
            
            commentAdded = false;
            if(editFlag == true && Comment != null)
            {
                txtComment.IsReadOnly = true;
                txtSubject.IsReadOnly = true;
                txtReply.IsReadOnly = true;
                cmbBillReferences.IsReadOnly = true;
                cmbCategories.IsReadOnly = true;
                cmbCCUsers.IsReadOnly = true;
                cmbTagRecomendUsers.IsReadOnly = true;
                cmbEmployee.IsReadOnly = true;
                cmbPOReferences.IsReadOnly = true;
                cmbTagUsers.IsReadOnly = true;

                if(!String.IsNullOrEmpty( Comment.Comment))
                {
                    txtComment.Text = Comment.Comment;
                }
                if (!String.IsNullOrEmpty(Comment.Subject))
                {
                    txtSubject.Text = Comment.Subject;
                }
                if (Comment.isReply == true)
                {
                    txtReply.Text = Comment.ReplyComment.Comment;
                }
                if(Comment.Category != null)
                {
                    var categories = (List<cmbitem>)cmbCategories.Items.SourceCollection;
                    cmbCategories.SelectedItem = cmbCategories.Items[cmbCategories.Items.IndexOf(categories.Find(x => x.id == Comment.CategoryId))];
                }
                if (Comment.notificationFlag != null)
                {
                    var flagss = (List<cmbitem>)cmbNotificationFlag.Items.SourceCollection;
                    cmbNotificationFlag.SelectedItem = cmbNotificationFlag.Items[cmbNotificationFlag.Items.IndexOf(flagss.Find(x => x.id == Comment.FlagId))];
                }

                if (Comment.CCUsersList != null && Comment.CCUsersList.Count > 0)
                {
                    foreach (var _user in Comment.CCUsersList)
                    {
                        var userList = (cmbCCUsers.ItemsSource as List<User>) == null ? new List<User>() : cmbCCUsers.ItemsSource as List<User>;
                        if (userList.Find(x => x.id == _user.id) == null)
                            userList.Add(_user);

                        cmbCCUsers.ItemsSource = null;
                        cmbCCUsers.ItemsSource = userList;
                    }

                    var CCusersSource = cmbCCUsers.ItemsSource as List<User>;
                    foreach (User cmbitem in CCusersSource)
                    {
                        if (Comment.CCUsersList.Find(x=>x.id == cmbitem.id) != null)
                        {
                            cmbCCUsers.SelectedItems.Add(cmbitem);
                            //break;
                        }
                    }
                }
                if (Comment.TaggedRecomenndedList != null && Comment.TaggedRecomenndedList.Count > 0)
                {
                    foreach (var _user in Comment.TaggedRecomenndedList)
                    {
                        var userList = (cmbTagRecomendUsers.ItemsSource as List<User>) == null ? new List<User>() : cmbTagRecomendUsers.ItemsSource as List<User>;
                        if (userList.Find(x => x.id == _user.id) == null)
                            userList.Add(_user);

                        cmbTagRecomendUsers.ItemsSource = null;
                        cmbTagRecomendUsers.ItemsSource = userList;
                    }

                    var CCusersSource = cmbTagRecomendUsers.ItemsSource as List<User>;
                    foreach (User cmbitem in CCusersSource)
                    {
                        if (Comment.TaggedRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                        {
                            cmbTagRecomendUsers.SelectedItems.Add(cmbitem);
                            //break;
                        }
                    }
                }
                if (Comment.CCRecomenndedList != null && Comment.CCRecomenndedList.Count > 0)
                {
                    foreach (var _user in Comment.CCRecomenndedList)
                    {
                        var userList = (cmbCCRecomendUsers.ItemsSource as List<User>) == null ? new List<User>() : cmbCCRecomendUsers.ItemsSource as List<User>;
                        if (userList.Find(x => x.id == _user.id) == null)
                            userList.Add(_user);

                        cmbCCRecomendUsers.ItemsSource = null;
                        cmbCCRecomendUsers.ItemsSource = userList;
                    }

                    var CCusersSource = cmbCCRecomendUsers.ItemsSource as List<User>;
                    foreach (User cmbitem in CCusersSource)
                    {
                        if (Comment.CCRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                        {
                            cmbCCRecomendUsers.SelectedItems.Add(cmbitem);
                            //break;
                        }
                    }
                }

                if (Comment.employee != null)
                {
                    var employees = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    if(employees.FirstOrDefault(x=>x.id == Comment.employeeId) == null)
                    {
                        employees.Add(new cmbitem {
                            id=Comment.employee.EmpId,
                            name = Comment.employee.person.FName + " " + Comment.employee.person.LName
                        });
                        cmbEmployee.ItemsSource = employees;
                    }
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(employees.Find(x => x.id == Comment.employeeId))];
                }
                if (Comment.TaggedList != null && Comment.TaggedList.Count > 0)
                {
                    foreach(var _user in Comment.TaggedList)
                    {
                        var userList = (cmbTagUsers.ItemsSource as List<User>) == null ? new List<User>() : cmbTagUsers.ItemsSource as List<User>;
                        if (userList.Find(x => x.id == _user.id) == null)
                            userList.Add(_user);
                        cmbTagUsers.ItemsSource = null;
                        cmbTagUsers.ItemsSource = userList;
                    }
                    cmbTagUsers.Text = Comment.TaggedList[0].userName;
                }
               

            }
        }

        public void LoadFlags()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            List<NotificationFlag> allNotificationFlags = new List<NotificationFlag>();
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Notification Flags") != null)
                allNotificationFlags = notificationsRepo.GetAllNotificationFlags();
            else
                allNotificationFlags = notificationsRepo.GetAllOpenFlags();

            if (allNotificationFlags != null)
            {
                foreach(var flag in allNotificationFlags) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {

                    cmbitems.Add
                    (new cmbitem()
                    {
                        name = flag.Flag,
                        id = flag.Id,
                        bcolor = flag.backcolor,
                        fcolor = "#FF000000"
                    });


                }
                cmbNotificationFlag.ItemsSource = cmbitems;
            }
        }

        public void LoadCategories()
        {
            ProcurementRepo repo = new ProcurementRepo();
            var Categorries = repo.getCurrentTransactionCommentCategories(transactionItemType);
            List<cmbitem> items = new List<cmbitem>();

            foreach (var Obj in Categorries)
            {

                items.Add(new cmbitem()
                {
                    id = Obj.Id,
                    name = Obj.category
                });

            }
            cmbCategories.ItemsSource = items;
        }
        public void loadusers(List<User> Users)
        {
            List<cmbitem> items = new List<cmbitem>();
            List<cmbitem> empss = new List<cmbitem>();
            this.DataContext = this;
           // taggedUsers = Users;
            Employees = new List<ERP_BL.Databases.Employee>();

            foreach (User user in Users)
            {
                Employees.Add(user.employee);
                items.Add(new cmbitem()
                {
                    id = user.id,
                    name = user.userName
                });
                empss.Add(new cmbitem()
                {
                    id = user.employee.EmpId,
                    name = user.employee.person.FName +" "+ user.employee.person.LName
                });

            }
            cmbTagUsers.ItemsSource = Users;
            cmbCCUsers.ItemsSource = Users;
            cmbTagRecomendUsers.ItemsSource = Users;
            cmbCCRecomendUsers.ItemsSource = Users;
            cmbEmployee.ItemsSource = empss;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            commentAdded = true;
            if (editFlag == true)
            {
                if (cmbNotificationFlag.SelectedIndex == -1)
                    FlagId = null;
                else
                {
                    FlagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                }
                this.Close();
                return;
            }
            comment = txtComment.Text.Trim();
            if (Comment != null)
            {
                Comment.Comment = comment;
                if (cmbEmployee.SelectedItem != null)
                {
                    Comment.AssigneeId = (cmbEmployee.SelectedItem as cmbitem).id;
                }
                if (cmbCategories.SelectedItem != null)
                {
                    Comment.CategoryId = (cmbCategories.SelectedItem as cmbitem).id;
                }
                Comment.Subject = txtSubject.Text.ToString();

                //if(lookupManager.SelectedIndex>-1)
                //{
                //    Comment.managerId = (lookupManager.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                //}
                //if (lookupSales.SelectedIndex > -1)
                //{
                //    Comment.salesPersonId = (lookupSales.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                //}
                //if (lookupFinance.SelectedIndex > -1)
                //{
                //    Comment.financePersonId = (lookupFinance.SelectedItem as ERP_BL.Databases.Employee).EmpId;
                //}

                Comment.Subject = txtSubject.Text.ToString();

                if (cmbTagUsers.SelectedItems.Count != 0)
                {
                    if (cmbTagUsers.SelectedItem != null)
                    {
                       var taggedUsers = new List<User>();
                        foreach (var user in cmbTagUsers.SelectedItems )
                        {
                            taggedUsers.Add(new User { id = (cmbTagUsers.SelectedItem as User).id, employeeId= (cmbTagUsers.SelectedItem as User).employeeId });

                        }
                        Comment.TaggedList = taggedUsers;
                    }
                }
                else
                {
                    Comment.TaggedList = new List<User>();
                } 
          

                if (cmbCCUsers.SelectedItems.Count != 0)
                {
                    if (cmbCCUsers.SelectedItems != null)
                    {
                      var  CCUsers = new List<User>();
                        foreach (User user in cmbCCUsers.SelectedItems)
                        {
                            CCUsers.Add(new User { id = user.id });

                        }
                        Comment.CCUsersList = CCUsers;

                    }
                }
                else
                {
                    Comment.CCUsersList = new List<User>();
                }
                if (cmbTagRecomendUsers.SelectedItems.Count != 0)
                {
                    if (cmbTagRecomendUsers.SelectedItems != null)
                    {
                        var CCUsers = new List<User>();
                        foreach (User user in cmbTagRecomendUsers.SelectedItems)
                        {
                            CCUsers.Add(new User { id = user.id });

                        }
                        Comment.TaggedRecomenndedList = CCUsers;

                    }
                }
                else
                {
                    Comment.TaggedRecomenndedList = new List<User>();
                }
                if (cmbCCRecomendUsers.SelectedItems.Count != 0)
                {
                    if (cmbCCRecomendUsers.SelectedItems != null)
                    {
                        var CCUsers = new List<User>();
                        foreach (User user in cmbCCRecomendUsers.SelectedItems)
                        {
                            CCUsers.Add(new User { id = user.id });

                        }
                        Comment.CCRecomenndedList = CCUsers;

                    }
                }
                else
                {
                    Comment.CCRecomenndedList = new List<User>();
                }

                if (cmbPOReferences.SelectedIndex!=-1)
                {
                    poReference = cmbPOReferences.EditValue as string;
                }
                if (cmbBillReferences.SelectedIndex != -1)
                {
                    billReference = cmbBillReferences.EditValue as string;
                }

                if (cmbNotificationFlag.SelectedIndex == -1)
                    FlagId = null;
                else
                {
                    FlagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                }

                //if (chkFlagForTag.IsChecked == true)
                //    FlagForTag = true;
                //else
                //    FlagForTag = false;

                //if (chkFlagForCC.IsChecked == true)
                //    FlagForCC = true;
                //else
                //    FlagForCC = false;
            }

            
            this.Close();
        }
        public void loadUsers()
        {

        }

        private void Picker_SelectionChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //txtComment.
            //string str = "";

            //if ( txtComment.LineCount > 1)
            //{
            //    for(int i = 0; i < txtComment.LineCount; i ++)
            //    {
            //        str =str + txtComment.GetLineText(i);
            //    }
            //}
            //else
            //{
            //    str = txtComment.Text;
            //}
            var emoji = emojiPicker.Selection;
            //var lines = txtComment.Text.;
            //TextPointer tp = txtComment.CaretPosition;
            //tp = tp.GetNextInsertionPosition(LogicalDirection.Forward);


            //var document = txtComment.Text.Insert(tp, emoji);
            //txtComment.Text = document + emoji;
            
            txtComment.Text = txtComment.Text.Insert(txtComment.CaretIndex, emoji);
        }

        private void btnClearTagUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbTagUsers.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occur while removing user from tagUser lookup" + ex.Message);
            }
        }
        private void lookupManager_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (lookupManager.SelectedIndex>-1)
            //{
            //    var image = GetBitmapImageFromByteArray((lookupManager.SelectedItem as ERP_BL.Databases.Employee).person.Photo);
            //    managerImg.Source = image;
            //}
        }

        private void lookupSales_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var image = GetBitmapImageFromByteArray((lookupSales.SelectedItem as ERP_BL.Databases.Employee).person.Photo);
            //salesImg.Source = image;
        }

        private void lookupFinance_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //var image = GetBitmapImageFromByteArray((lookupFinance.SelectedItem as ERP_BL.Databases.Employee).person.Photo);
            //financeImg.Source = image;
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

        private void btnClearRecomTagUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbTagRecomendUsers.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occur while removing user from recomendation tagUser lookup" + ex.Message);
            }
        }
        private void btnRecomendation_Click(object sender, RoutedEventArgs e)
        {
            if(cmbTagRecomendUsers.SelectedItems!=null)
            {
                var TagUsersSource = cmbTagUsers.ItemsSource as List<User>;
                foreach (User cmbitem in TagUsersSource)
                {
                    if (replyComment.TaggedRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                    {
                        cmbTagUsers.Text=cmbitem.userName;
                    }
                }
            }
            if (cmbCCRecomendUsers.SelectedItems != null)
            {
                var CCUsersSource = cmbCCUsers.ItemsSource as List<User>;
                foreach (User cmbitem in CCUsersSource)
                {
                    if (replyComment.CCRecomenndedList.Find(x => x.id == cmbitem.id) != null)
                    {
                        cmbCCUsers.SelectedItems.Add(cmbitem);
                    }
                }
            }
        }
    }
}
