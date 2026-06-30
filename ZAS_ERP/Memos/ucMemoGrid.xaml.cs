using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
using ERP_BL.Procurements.Memos;
using Microsoft.Win32;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Memos.PerformanceReview;

namespace ZAS_ERP.Memos
{
    /// <summary>
    /// Interaction logic for ucMemoGrid.xaml
    /// </summary>
    public partial class ucMemoGrid : UserControl
    {
        bool isCommunicationTabSelected;
        MemoRepo memoRepo = new MemoRepo();
        Memo memo = new Memo();
        UsersRepo UsersRepo = new UsersRepo();

        List<cmbitem> cmbitems = new List<cmbitem>();
        MemoNotificationsRepo notificationsRepo = new MemoNotificationsRepo();
        List<Notification> notifications = new List<Notification>();
        //public bool? loadSentNotification = false;
        public string notificationsLoaded = "";
        public int unReadCount { get; set; }
        public int pendingCount { get; set; }
        public int counter { get; set; }
        public int selectedCounter { get; set; }
        public MainWindow myParent = null;
        Notification notification = new Notification();
        bool? glow = null;
        int checkNotificationIdGlow = 0;

        public bool urgentNotification = false;
        public ucMemoGrid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdMemo.ItemsSource = memoRepo.GetAllMemos(SYSTEM_STATIC.currentUser.id);
            LoadMemoCounters();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdMemo);
            lblMemoHeading.Content = "Memo Register";

            LoadNotificationsData();
            isCommunicationTabSelected = true;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Memos") == null)
            {
                tabMemoOriginator.IsEnabled = false;
            }
            
        }

        private void LoadNotificationsData()
        {
            cmbitems = new List<cmbitem>();
            List<NotificationFlag> allNotificationFlags = new List<NotificationFlag>();
            notificationsRepo = new MemoNotificationsRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Notification Flags") != null)
                allNotificationFlags = notificationsRepo.GetAllNotificationFlags();
            else
                allNotificationFlags = notificationsRepo.GetAllOpenFlags();

            if (allNotificationFlags != null)
            {
                cmbitems.Add
                    (new cmbitem()
                    {
                        name = "-- Clear flag --",
                        id = 0,

                        fcolor = "#000000"
                    }
                    );
                Parallel.ForEach(allNotificationFlags, delegate (NotificationFlag flag) // foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
                {

                    cmbitems.Add
                    (new cmbitem()
                    {
                        name = flag.Flag,
                        id = flag.Id,
                        bcolor = flag.backcolor,
                        fcolor = "#FF000000"
                    });


                });
                cmbNotificationFlag.ItemsSource = cmbitems;
            }

            if (urgentNotification == false)
            {
                //LoadNotifications();
                LoadAllUnreadNotifications();
            }
            else
            {
                LoadAllUrgentNotifications();
            }

            //loadSentNotification = null;
            notificationsLoaded = "Unread";
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdNotifications);
            //ThemeManager.SetThemeName(this, "Office2010Silver");
            getCountAllUnreadNotifications();
        }


         private void TableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (grdNotifications.GetFocusedRow() != null)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                if (notification != null && notification.TransactionId != 0 && notification.TransactionType != 0)
                {
                    DXWindow win = new DXWindow();
                    switch (notification.TransactionType)
                    {
                        //case TransactionItemType.RentalOrder:
                        //    ucRentalOrderAdd rentalOrderAdd = new ucRentalOrderAdd();
                        //    rentalOrderAdd.editFlag = true;
                        //    rentalOrderAdd.orderId = notification.TransactionId;

                        //    win.Content = rentalOrderAdd;
                        //    win.WindowState = WindowState.Maximized;
                        //    win.Show();
                        //    break;
                        
                        //default:
                        //    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(notification.TransactionType, notification.TransactionId);
                        //    procurmentPanel.ShowActivated = true;

                        //    procurmentPanel.Show();
                        //    break;

                    }
                    


                    if (notification.isRead == false && (MainWindow.currentUserid != notification.SendingUserId || (MainWindow.currentUserid == notification.SendingUserId && notification.SendingUserId == notification.UserId)))
                    {
                        notificationsRepo.MarkasRead(notification.Id);
                    }
                }
            }
        }

        private void MbtnRefreshNotification_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ReloadNotificationData();

                grdNotifications.RefreshData();
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "Information",
                    Message = "Data has been Refresh!",
                    Type = Notifications.Wpf.NotificationType.Information
                });
                myParent.getCountAllUnreadMemoNotifications();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                txtSelected.Text = "0";
                txtFiltered.Text = (grdNotifications.VisibleRowCount - 1).ToString();
            }
            catch (Exception ex)
            {
            }
        }

        private void ReloadNotificationData()
        {
            notificationsRepo = new MemoNotificationsRepo();
            getCountAllUnreadNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadNotifications();
                    break;
                case "Sent":
                    LoadSentNotifications();
                    break;
                case "Unread":
                    LoadAllUnreadNotifications();
                    break;
                case "Pending":
                    LoadAllPendingNotifications();
                    break;
            }
            //if (notificationsLoaded == "Inbox")
            //    LoadNotifications();
            //else if (notificationsLoaded == "Sent")
            //    LoadSentNotifications();
            //else if (notificationsLoaded == "Unread")
            //    LoadAllUnreadNotifications();
        }

        private void getCountAllUnreadNotifications()
        {
            unReadCount = notificationsRepo.CountAllUnreadNotifications(MainWindow.currentUserid);
            txtUnReadcount.Text = unReadCount.ToString();

            pendingCount = notificationsRepo.CountAllPendingNotifications(MainWindow.currentUserid);
            txtPendingCount.Text = pendingCount.ToString();

            //mbtnNotifications.Content = "(" + unReadCount.ToString() + ")";
        }

        private void MbtnSentNotification_Click(object sender, RoutedEventArgs e)
        {
            imgGlowOff.Visibility = Visibility.Collapsed;
            imgGlowOn.Visibility = Visibility.Collapsed;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            notificationsLoaded = "Sent";
            LoadSentNotifications();
            setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void LoadSentNotifications()
        {
            var SentNotifications = notificationsRepo.SentNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            
        }
        private void mbtnInboxNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = false;
            notificationsLoaded = "Inbox";
            LoadNotifications();
            setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }
        private void mbtnAllUnreadNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = null;
            notificationsLoaded = "Unread";
            LoadAllUnreadNotifications();
            if(button != null)
                setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }


        private void LoadAllUnreadNotifications()
        {
            var SentNotifications = notificationsRepo.AllUnreadNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        private void LoadAllUrgentNotifications()
        {
            var SentNotifications = notificationsRepo.AllUrgentNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        private void MbtnLoadMoreNotification_Click(object sender, RoutedEventArgs e)
        {
            //var selectedItems = grdNotifications.SelectedItems;
            //List<Notification> notifications = new List<Notification>();
            //foreach(var _item in selectedItems)
            //{
            //    notifications.Add(_item as Notification);
            //}
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            LoadMoreNotifications();
            grdNotifications.RefreshData();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleRowCount - 1).ToString();
        }

        private void setButtonsColor(string buttonName)
        {   
            switch (buttonName)
            {
                case "Inbox":
                    mbtnInboxNotification.Background = Brushes.LightBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Unread":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.LightBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Sent":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.LightBlue;
                    mbtnAllPendingNotification.Background = Brushes.AliceBlue;
                    break;
                case "Pending":
                    mbtnInboxNotification.Background = Brushes.AliceBlue;
                    mbtnAllUnreadNotification.Background = Brushes.AliceBlue;
                    mbtnSentNotification.Background = Brushes.AliceBlue;
                    mbtnAllPendingNotification.Background = Brushes.LightBlue;
                    break;
            }
        }
        

        public void LoadNotifications()
        {
            try
            {
          
                
                //grdNotifications.View.FocusedRowHandle = -1;
                
                notifications = new List<Notification>();
                notificationsRepo = new MemoNotificationsRepo();
                //System.Threading.Thread thread = new Thread(() =>
                //{
                    notifications = notificationsRepo.getUsersNotificationOrderDsc(MainWindow.currentUserid);
                    //this.Dispatcher.Invoke((Action)(() =>
                    //{
                        grdNotifications.ItemsSource = notifications;
                        counter = notifications.Count;
                        txtCounter.Text = counter.ToString();

                        SetImageforUnreadNotifications();
                    //}));
                //});
                //thread.Start();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
           
        }

        private void SetImageforUnreadNotifications()
        {
            //if (notifications.Find(x => x.isRead == false) != null)
            //{
            //    mbtnNotifications.Glyph = mbtnNotifications.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/images/NotificationOn.png", UriKind.RelativeOrAbsolute));
            //}
            //else
            //{
            //    mbtnNotifications.Glyph = mbtnNotifications.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/images/noify1.png", UriKind.RelativeOrAbsolute));
            //}
        }

        public void LoadMoreInboxNotifications()
        {
            var MoreNotifications = notificationsRepo.getMoreUsersNotificationOrderByDate(datFrom.DateTime, datTo.DateTime, MainWindow.currentUserid);
            notifications = MoreNotifications;

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadMoreSentNotifications()
        {
            var MoreNotifications = notificationsRepo.getMoreSentNotificationsByDate(datFrom.DateTime, datTo.DateTime, MainWindow.currentUserid);
            notifications = MoreNotifications;

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadMoreNotifications()
        {
            //if (loadSentNotification == false)
            //    LoadMoreInboxNotifications();
            //else if (loadSentNotification == true)
            //    LoadMoreSentNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadMoreInboxNotifications();
                    break;
                case "Sent":
                    LoadMoreSentNotifications();
                    break;
              
            }
        }

        public void LoadAllNotifications()
        {
            //if (loadSentNotification == false)
            //    LoadAllInboxNotifications();
            //else if (loadSentNotification == true)
            //    LoadAllSentNotifications();

            switch (notificationsLoaded)
            {
                case "Inbox":
                    LoadAllInboxNotifications();
                    break;
                case "Sent":
                    LoadAllSentNotifications();
                    break;

            }
        }

        public void LoadAllInboxNotifications()
        {
            notificationsRepo = new MemoNotificationsRepo();
            notifications = notificationsRepo.getAllInboxNotifications(MainWindow.currentUserid);
            

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        public void LoadAllSentNotifications()
        {
            notificationsRepo = new MemoNotificationsRepo();
            notifications = notificationsRepo.getAllSentNotifications(MainWindow.currentUserid);
            

            grdNotifications.ItemsSource = notifications;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
            SetImageforUnreadNotifications();
        }

        private void LoadNotificationData(Notification notification)
        {
            if (notification != null)
            {
                if (notification.SendingUser != null)
                {
                    if (notification.SendingUser.employee != null)
                    {
                        if (notification.SendingUser.employee.person != null)
                            txtUserName.Text = notification.SendingUser.employee.person.FName + " " + notification.SendingUser.employee.person.LName;
                        txtDesignation.Text = notification.SendingUser.employee.DesignationTitle;
                    }
                    else
                    {
                        txtUserName.Text = "Nil";
                        txtDesignation.Text = "Nil";
                    }

                    //Select Status
                    if (notification.notificationFlag != null)
                    {
                        int index = 0;
                        //oldStatus = bankTransfer.interBankTransStatus;
                        foreach (var _status in cmbitems)
                        {
                            if (_status.id == notification.notificationFlag.Id)
                            {
                                cmbNotificationFlag.SelectedIndex = index;

                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        cmbNotificationFlag.SelectedIndex = -1;
                    }
                }

                txtTransactionType.Text = notification.TransactionType.ToString();
                txtComment.Text = notification.Description;

                if (notification.SendingUser != null)
                {
                    var byteImg = notification.SendingUser.employee.person.Photo;
                    if (byteImg != null)
                    {
                        var image = GetBitmapImageFromByteArray(byteImg);
                        UserImage.ImageSource = image;
                    }
                }
            }
        }

        private void TableView_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            try
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                if (notification != null)
                {
                    if(notificationsLoaded == "Sent")
                    {
                        imgGlowOn.Visibility = Visibility.Collapsed;
                        imgGlowOff.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (notification.notificationFlag != null)
                        {
                            if (notification.notificationFlag.canGlow == true && notification.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        else
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = null;
                        }
                    }
                    
                    
                   

                   
                        LoadNotificationData(notification);
                    loadcomments();
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
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

        private void MbtnMarkAsRead_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasReadNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Read Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnMarkAsUnread_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasUnReadNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Unread Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdNotifications);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void CmbNotificationFlag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(grdNotifications.GetFocusedRow() == null)
            {
                DXMessageBox.Show("Please Select Notification first!");
                return;
            }
            if (cmbNotificationFlag.SelectedIndex > 0)
            {
                var flagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                var flag = notificationsRepo.GetNotificationFlag(flagId);

                var not = grdNotifications.GetFocusedRow() as Notification;

                if(flag.canGlow == true)
                {
                    if(not.Id != checkNotificationIdGlow)
                    {
                        checkNotificationIdGlow = not.Id;
                        if (notificationsLoaded == "Sent")
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = true;
                        }
                        else
                        {
                            if (not.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        
                    }
                    else
                    {
                        if (notificationsLoaded == "Sent")
                        {
                            imgGlowOn.Visibility = Visibility.Collapsed;
                            imgGlowOff.Visibility = Visibility.Collapsed;
                            glow = true;
                        }
                        else
                        {
                            if (not.Glow == true)
                            {
                                imgGlowOn.Visibility = Visibility.Visible;
                                imgGlowOff.Visibility = Visibility.Collapsed;
                                glow = true;
                            }
                            else
                            {
                                imgGlowOn.Visibility = Visibility.Collapsed;
                                imgGlowOff.Visibility = Visibility.Visible;
                                glow = false;
                            }
                        }
                        
                    }
                    
                }
                else
                {
                    checkNotificationIdGlow = not.Id;
                    imgGlowOn.Visibility = Visibility.Collapsed;
                    imgGlowOff.Visibility = Visibility.Collapsed;
                    glow = false;
                }
            }
            else
            {
                imgGlowOn.Visibility = Visibility.Collapsed;
                imgGlowOff.Visibility = Visibility.Collapsed;
                glow = null;
            }

        }

        

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbNotificationFlag.SelectedIndex > -1)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;

                if (cmbNotificationFlag.SelectedIndex == 0)
                    notification.FlagId = null;
                else
                {
                    var flagId = (cmbNotificationFlag.SelectedItem as cmbitem).id;
                    var flag = notificationsRepo.GetNotificationFlag(flagId);

                    if (flag.canGlow == true)
                    {
                        notification.Glow = glow.Value;
                    }
                    notification.FlagId = flagId;
                }

                notificationsRepo.Update(notification);

                var myWindow = Window.GetWindow(this) as MainWindow;
                myWindow.UrgentNotificationGlow();

                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "Information",
                    Message = "Notification Updated!",
                    Type = Notifications.Wpf.NotificationType.Success
                });
            }
        }

        private void MbtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            LoadAllNotifications();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            myParent.FavouriteItems.Children.Remove(this);
        }

        private void MbtnLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdNotifications_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdNotifications.GetRowByListIndex(e.ListSourceRowIndex) as Notification;
                switch (e.Column.FieldName)
                {
                    case "FromUser":
                        if (row.SendingUser != null && row.SendingUser.employee != null && row.SendingUser.employee.person != null)
                        {
                            userName = row.SendingUser.employee.person.FName + " " + row.SendingUser.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "ToUser":
                        if (row.User != null && row.User.employee != null && row.User.employee.person != null)
                        {
                            userName = row.User.employee.person.FName + " " + row.User.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "CCuser":
                        if (row.CcUser != null && row.CcUser.employee != null && row.CcUser.employee.person != null)
                        {
                            userName = row.CcUser.employee.person.FName + " " + row.CcUser.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "CreatedFor":
                        if (row.TransactionId != 0 && row.TransactionType == TransactionItemType.Memo)
                        {
                            var memo = memoRepo.GetMemo(row.TransactionId);
                            e.Value = memo.createdFor?.employee?.person?.FName + " " +memo.createdFor?.employee?.person?.LName;
                        }
                        
                        break;
                }
            }
        }

        private void GrdNotifications_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            selectedCounter = grdNotifications.SelectedItems.Count;
            txtSelected.Text = selectedCounter.ToString();
        }
       
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdNotifications_FilterChanged(object sender, RoutedEventArgs e)
        {
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void ImgGlowOn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if(grdNotifications.GetFocusedRow() != null)
                {
                    //notification = grdNotifications.GetFocusedRow() as Notification;
                    //notification.Glow = false;
                    //notificationsRepo.Update(notification);
                    glow = false;

                    imgGlowOn.Visibility = Visibility.Collapsed;
                    imgGlowOff.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Error Ocuurred on Image Glow-On button");
            }
        }

        private void ImgGlowOff_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (grdNotifications.GetFocusedRow() != null)
                {
                    //notification = grdNotifications.GetFocusedRow() as Notification;
                    //notification.Glow = true;
                    //notificationsRepo.Update(notification);
                    glow = true;

                    imgGlowOn.Visibility = Visibility.Visible;
                    imgGlowOff.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Error Ocuurred on Image Glow-Off button");
            }
        }

        private void MbtnAllPendingNotification_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var stackPanel = (sender as SimpleButton).Content as StackPanel;
            var button = stackPanel.Children[1] as TextBlock;
            //loadSentNotification = null;
            notificationsLoaded = "Pending";
            LoadAllPendingNotifications();
            if (button != null)
                setButtonsColor(button.Text);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
            txtSelected.Text = "0";
            txtFiltered.Text = (grdNotifications.VisibleItems.Count).ToString();
        }

        private void LoadAllPendingNotifications()
        {
            var SentNotifications = notificationsRepo.AllPendingNotifications(MainWindow.currentUserid);
            grdNotifications.ItemsSource = SentNotifications;
            SetImageforUnreadNotifications();
            notifications = SentNotifications as List<Notification>;
            counter = notifications.Count;
            txtCounter.Text = counter.ToString();
        }

        

        private void MbtnMarkAsPending_Click(object sender, RoutedEventArgs e)
        {
            if(notificationsLoaded == "Sent")
            {
                DXMessageBox.Show("Sent notifications cannot be Mark/UnMark as Pending!");
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.MarkasPendingNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "Marked as Pending Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }

        private void MbtnUnMarkAsPending_Click(object sender, RoutedEventArgs e)
        {
            if (notificationsLoaded == "Sent")
            {
                DXMessageBox.Show("Sent notifications cannot be Mark/UnMark as Pending!");
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            if (grdNotifications.SelectedItems.Count > 0)
            {
                List<Notification> notificationsList = new List<Notification>();
                //var cmitem = item as cmbitem;
                foreach (var _item in grdNotifications.SelectedItems)
                {
                    var noti = _item as Notification;
                    notificationsList.Add(notifications.Find(x => x.Id == noti.Id));
                }

                if (notificationsList != null && notificationsList.Count > 0)
                {
                    notificationsRepo.UnMarkasPendingNotificationList(notificationsList);
                    grdNotifications.UnselectAll();
                    NotificationManager notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Information",
                        Message = "UnMarked Pending Notifications Successfully!",
                        Type = Notifications.Wpf.NotificationType.Information
                    });
                    //ReloadNotificationData();
                }
                txtSelected.Text = "0";
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });
        }


        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            memoRepo = new MemoRepo();
            grdMemo.ItemsSource = memoRepo.GetAllMemos(SYSTEM_STATIC.currentUser.id);
            LoadMemoCounters();
            lblMemoHeading.Content = "Memo Register";
        }

        private void LoadMemoCounters()
        {
            mbtnVoidCounter.Header = memoRepo.GetAllVoidMemosCount(SYSTEM_STATIC.currentUser.id).ToString();
            mbtnMemoRegisterCounter.Header = memoRepo.GetAllMemosCount(SYSTEM_STATIC.currentUser.id).ToString();

        }

        private void grdMemo_FilterChanged(object sender, RoutedEventArgs e)
        {

        }

        private void grdMemo_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdMemo.GetRowByListIndex(e.ListSourceRowIndex) as Memo;
                switch (e.Column.FieldName)
                {
                    case "CreatedByy":
                        if (row.createdBy != null && row.createdBy.employee != null && row.createdBy.employee.person != null)
                        {
                            userName = row.createdBy.employee.person.FName + " " + row.createdBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                    case "CreatedForr":
                        if (row.createdFor != null && row.createdFor.employee != null && row.createdFor.employee.person != null)
                        {
                            userName = row.createdFor.employee.person.FName + " " + row.createdFor.employee.person.LName;
                        }
                        e.Value = userName;
                        break;
                }
            }
        }

        
        private void grdMemo_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            memo = grdMemo.SelectedItem as Memo;
            loadcomments();
        }
        private void loadcomments()
        {
            try
            {
                int memoId;
                if (isCommunicationTabSelected == true)
                {
                    if (grdNotifications.GetFocusedRow() == null)
                        memoId = 0;
                    else
                        memoId = notification.TransactionId;

                    memo = memoRepo.GetMemo(memoId);
                }
                else if (isCommunicationTabSelected == false)
                {
                    memo = grdMemo.SelectedItem as Memo;
                }

                if (memo != null)
                {
                    lblMemoTitle.Content = memo.Subject;
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(memo.Id, TransactionItemType.Memo);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void mbtnAddMemo_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Memo") != null)
            {
                //var selectedRow = grdCntrlTasks.SelectedItem as Tasks;
                //if (selectedRow != null)
                //{
                ucSelectMemoType memoAdd = new ucSelectMemoType();
               
                Window win = new Window();
                win.Height = 250;
                win.Width = 400;
                win.Title = "Memo";
                win.Content = memoAdd;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
                //}
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add New Memo!");
            }
        }

        private void mbtnUpdateMemo_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = grdMemo.SelectedItem as Memo;

            if (selectedRow != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memo") != null)
                {
                    ucFrmMemoAdd memoAdd = new ucFrmMemoAdd();
                    Window win = new Window();
                    memoAdd.editFlag = true;
                    memoAdd.memoId = selectedRow.Id;
                    win.Height = 500;
                    win.Width = 400;
                    win.Content = memoAdd;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Show();
                }
                else
                {
                    DXMessageBox.Show("Permission Required to View Memo!");
                }
            }

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //int memoId;
            //if (isCommunicationTabSelected == true)
            //{
            //    notification = grdNotifications.GetFocusedRow() as Notification;
            //    memoId = notification.TransactionId;
            //    memo = memoRepo.GetMemo(memoId);
            //}
            //else if (isCommunicationTabSelected == false)
            //{
            //    memo = grdMemo.SelectedItem as Memo;
            //}

            if (memo != null)
            {
                if (memo.Id != 0)
                {
                    UsersRepo.Add(TransactionInfo.viewed, memo.Id, (int)TransactionItemType.Memo, "Viewed details of Memo");
                }
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int memoId;
            if (isCommunicationTabSelected == true)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                memoId = notification.TransactionId;
                memo = memoRepo.GetMemo(memoId);
            }
            else if (isCommunicationTabSelected == false)
            {
                memo = grdMemo.SelectedItem as Memo;
            }

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (memo != null)
                {
                    if (memo.memoType == MemoType.Group && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Comment in Group Memos") == null)
                    {
                        DXMessageBox.Show("Permission required to Add Comment in Group Memos!");
                        return;
                    }

                    if (memo.memoType == MemoType.Non_Linked)
                    {
                        UsersRepo usersRepo = new UsersRepo();

                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.Add(memo.createdFor);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);

                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();

                        //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.Memo);

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, comment, TransactionItemType.Memo);
                        
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.txtReply.Text = comment.Comment;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;

                        inputBox.ShowDialog();

                    }
                    else
                    if (memo.memoType == MemoType.Group && memo.taskGroup != null && memo.taskGroup.users != null && memo.taskGroup.users.Count > 0)
                    {
                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.AddRange(memo.taskGroup.users);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);

                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, memo.taskGroup.users,comment, TransactionItemType.Memo);
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.txtReply.Text = comment.Comment;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagUser.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCUser.Visibility = Visibility.Collapsed;

                        inputBox.btnClearTagUser.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                        inputBox.ShowDialog();

                    }
                    else if (memo.memoType == MemoType.Linked && memo.createdBy != null && memo.createdFor != null)
                    {
                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.Add(memo.createdFor);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);
                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users,comment, TransactionItemType.Memo);
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.txtReply.Text = comment.Comment;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }



                    if (memo != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && memo.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(memo.Id, TransactionItemType.Memo, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo with Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Admin_Bill, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (memo.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Memo first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int memoId;
                if (isCommunicationTabSelected == true)
                {
                    notification = grdNotifications.GetFocusedRow() as Notification;
                    memoId = notification.TransactionId;
                    memo = memoRepo.GetMemo(memoId);
                }
                else if (isCommunicationTabSelected == false)
                {
                    memo = grdMemo.SelectedItem as Memo;
                }

                if (memo != null)
                {
                    if (memo.memoType == MemoType.Group && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Comment in Group Memos") == null)
                    {
                        DXMessageBox.Show("Permission required to Add Comment in Group Memos!");
                        return;
                    }

                    if (memo.memoType == MemoType.Non_Linked)
                    {
                        UsersRepo usersRepo = new UsersRepo();

                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.Add(memo.createdFor);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);

                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.Memo);
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;

                        inputBox.ShowDialog();

                    }
                    else
                    if (memo.memoType == MemoType.Group && memo.taskGroup != null && memo.taskGroup.users != null && memo.taskGroup.users.Count > 0)
                    {
                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.AddRange(memo.taskGroup.users);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);

                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, memo.taskGroup.users, TransactionItemType.Memo);
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagUser.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCUser.Visibility = Visibility.Collapsed;

                        inputBox.btnClearTagUser.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                        inputBox.ShowDialog();

                    }
                    else if (memo.memoType == MemoType.Linked && memo.createdBy != null && memo.createdFor != null)
                    {
                        List<ERP_BL.Databases.User> users = new List<ERP_BL.Databases.User>();
                        users.Add(memo.createdBy);
                        users.Add(memo.createdFor);
                        var CCusers = memo.CCUsersList;
                        if (CCusers != null && CCusers.Count > 0)
                            users.AddRange(CCusers);
                        users = users.GroupBy(x => x).Select(d => d.First()).ToList();
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, users, TransactionItemType.Memo);
                        inputBox.txtSubject.Text = memo.Subject;
                        inputBox.txtSubject.IsReadOnly = true;

                        inputBox.cmbNotificationFlag.Visibility = Visibility.Collapsed;
                        inputBox.lblFlag.Visibility = Visibility.Collapsed;

                        inputBox.cmbEmployee.Visibility = Visibility.Collapsed;
                        inputBox.lblAsignee.Visibility = Visibility.Collapsed;

                        inputBox.cmbCategories.Visibility = Visibility.Collapsed;
                        inputBox.lblCategory.Visibility = Visibility.Collapsed;

                        inputBox.cmbCCRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblCCRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.cmbTagRecomendUsers.Visibility = Visibility.Collapsed;
                        inputBox.lblTagRecomendation.Visibility = Visibility.Collapsed;

                        inputBox.lblRecommendationHeader.Visibility = Visibility.Collapsed;
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (memo != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && memo.Id != 0)
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                var commentId = procurementRepo.AddCommentLinkNotification(memo.Id, TransactionItemType.Memo, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                if (commentId != null)
                                {
                                    foreach (var user in frmInputBox.Comment.TaggedList)
                                    {
                                        if (frmInputBox.FlagForTag == true)
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo with Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                    }

                                    foreach (var user in frmInputBox.Comment.CCUsersList)
                                    {
                                        if (frmInputBox.FlagForCC == true)
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Memo Subject:" + memo.Subject, memo.Id, TransactionItemType.Memo, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                    }
                                }
                            }

                            //procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (memo.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Memo first to add a comment!");
                        }

                    }

                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show("Select a Memo to Add new comment!");
                }
            }catch(Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
            
                
                
            
        }

        private void btnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            int memoId;
            if (isCommunicationTabSelected == true)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                memoId = notification.TransactionId;
                memo = memoRepo.GetMemo(memoId);
            }
            else if (isCommunicationTabSelected == false)
            {
                memo = grdMemo.SelectedItem as Memo;
            }

            if (memo != null)
            {
                if (grdAttachments.Visibility == Visibility.Visible)
                    grdAttachments.Visibility = Visibility.Collapsed;
                else
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(memo.Id, TransactionItemType.Memo);
                    grdAttachments.Visibility = Visibility.Visible;
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
                        if (str.Contains("Memo"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Memo);
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
                        if (str.Contains("Tasks"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Tasks);
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

        private void btnSetVoid_Click(object sender, RoutedEventArgs e)
        {

            int memoId;
            if (isCommunicationTabSelected == true)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                memoId = notification.TransactionId;
                memo = memoRepo.GetMemo(memoId);
            }
            else if (isCommunicationTabSelected == false)
            {
                memo = grdMemo.SelectedItem as Memo;
            }
            if (memo != null)
            {
                if (memo.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Memos") != null))
                {
                    if (DXMessageBox.Show("This Task is currently in the list of Void Memos s! Do you want to remove it from Void?", "Remove Void Memos", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        memo.isVoid = false;
                        memoRepo.UpdateMemo(memo);

                        // grdVoid.Visibility = Visibility.Collapsed;
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Memos") != null && memo.isVoid != true)
                {
                    if (DXMessageBox.Show("This Task is not currently in the list of Void Memos s! Do you want to move it to Void Memos?", "Add to Void Memos", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        memo.isVoid = true;
                        memoRepo.UpdateMemo(memo);

                        // grdVoid.Visibility = Visibility.Visible;

                    }
                }

                loadcomments();
                //var thisWindow = Window.GetWindow(this);
                //thisWindow.Close();
            }
            else
            {
                DXMessageBox.Show("Please select Memo!");
            }
        }

        private void btnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Attach file in Memos") == null)
            {
                DXMessageBox.Show("Permission required to Attach a file!");
                return;
            }
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveMemoAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            int memoId;
            if (isCommunicationTabSelected == true)
            {
                notification = grdNotifications.GetFocusedRow() as Notification;
                memoId = notification.TransactionId;
                memo = memoRepo.GetMemo(memoId);
            }
            else if (isCommunicationTabSelected == false)
            {
                memo = grdMemo.SelectedItem as Memo;
            }

            if (memo != null)
            {
                string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
                string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
                if (cmbCategory1.SelectedItem != null)
                {
                    if (memo.Id != 0)
                    {
                        try
                        {

                            int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";
                            fileDialog.Multiselect = false;
                            string sourceFile = @"";
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Attachments\\Memo\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment1.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += memo.Id + "_" + TransactionItemType.Memo.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.Memo);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), memo.Id, TransactionItemType.Memo, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, memo.Id, (int)TransactionItemType.Memo, "Added a new attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Tasks);
                                                imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                                btnAttachNew.ToolTip = "Attach";
                                                btnAttachNew.IsEnabled = true;
                                                btnAttachment1.Content = "Select";
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
                                                btnAttachment1.Content = "Select";
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
            else
            {
                DXMessageBox.Show("Please Select Memo!");
            }
        }

        private void btnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdMemo);
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Void Memos") != null))
            {
                lblMemoHeading.Content = "Void Memos";
                grdMemo.ItemsSource = memoRepo.GetAllVoidMemos(SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                DXMessageBox.Show("Permission Required!");
            }
        }

        private void DXTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {
            if (tabMemoCommunicator.IsSelected == true)
                isCommunicationTabSelected = true;
            else if (tabMemoOriginator.IsSelected == true)
                isCommunicationTabSelected = false;
            loadcomments();
        }

        private void mbtnMemoRegister_Click(object sender, RoutedEventArgs e)
        {
            lblMemoHeading.Content = "Memo Register";
            grdMemo.ItemsSource = memoRepo.GetAllMemos(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnPerformanceReview_Click(object sender, RoutedEventArgs e)
        {
            int memoId = 0;
            if (isCommunicationTabSelected == true)
            {
                if (grdNotifications.GetFocusedRow() == null)
                {
                    memoId = 0;
                    DXMessageBox.Show("Please select a notification");
                    return;
                }
                else
                    memoId = notification.TransactionId;

                memo = memoRepo.GetMemo(memoId);
            }
            else if (isCommunicationTabSelected == false)
            {
                memo = grdMemo.SelectedItem as Memo;
                memoId = memo.Id;
            }


            var review = memoRepo.GetPerformanceReviewByMemoId(memoId);

            if (review != null)
            {
                var editWindow = new PerformanceReviewWindow();
                editWindow.reviewIdToEdit = review.Id;
                editWindow.editFlag = true;
                editWindow.memoId = review.MemoId.Value;
                editWindow.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("This memo has no Performance review attached to it!");
            }



        }

        private void mbtnPerformanceSummary_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Performance Review Summary") != null)
            {
                var editWindow = new ReviewSummaryWindow();
                editWindow.WindowState = WindowState.Maximized;
                editWindow.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required!");
            }          
        }

    }
}
