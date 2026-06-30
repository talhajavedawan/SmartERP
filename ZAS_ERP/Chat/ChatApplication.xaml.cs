using ERP_BL;
using ERP_BL.ChatManager;
using ERP_BL.Databases;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using ZAS_ERP.Chat.ChatUsercontrols;
using ZAS_ERP.Chat.Usercontrols;
using ZAS_ERP.Chat.Usercontrols.ChatUsercontrols;
using Brushes = System.Drawing.Brushes;

namespace ZAS_ERP.Chat
{
    /// <summary>
    /// Interaction logic for newChat1.xaml
    /// </summary>
    public partial class ChatApplication : Window
    {

        private MediaPlayer mediaPlayer = new MediaPlayer();
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();

        //--------------------------------------------------------------List to store user Object-------------------------------------------------------------------
        public List<User> users;
        List<ucContacts> listCollectionContact = new List<ucContacts>();
        List<ucRecentChats> listCollectionRecent = new List<ucRecentChats>();

        public MainWindow myParent = null;

     //------------------------------------------------------------- Dictionary to store All previous Chat of login User--------------------------------------

        public IDictionary<int, long> PrevGroupChats = new Dictionary<int, long>();
        public IDictionary<int, One2OneMessage> PrevChats = new Dictionary<int, One2OneMessage>();

        IDictionary<int, int> prevProps = new Dictionary<int, int>();

      //------------------------------------------------------------timer for group Checker------------------------------------------------------------------
        private Timer aTimer;

     //---------------------------------------------------------- Vaiables for using login id and Receiver Id----------------------------------------------

        public int loggedinId = 0;
        public int openChatId = 0;
        public long lastmsgId = 0;
        public int startChecker = 1;
        public long msgCounter = 0;
       
        string selectedFileName = null;
        float total = 0;
        public int msgIdentifier = 0;
        public int groupMsgId;
        public int ch = 1;
        public long groupMsgCounter = 0;
        public long oneToOnemsgCounter = 0;
        public long groupMainMsgCounter = 2;
        
       

//-----------------------------------------------------------------------Objects from other Windows and Classes------------------------------------------

        UsersRepo userRepo = new UsersRepo();

        ChatManager cm = new ChatManager();

        Attach att = new Attach();
       
        //--------------------------------------------------------------Variables for attachements-----------------------------------------------------
        string path = null;
        int senderId = 0;
        int receiverId = 0;
        ERP_BL.Enums.ChatFileType type;
        public string fileAttachPath;
        public string fileName;
        public string fileSize;
        private Icon iconForFile;
        public string fileIcon;
       

        private System.Windows.Forms.NotifyIcon m_notifyIcon;

        public ChatApplication()
        {
            InitializeComponent();
            //loggedinId = MainWindow.currentUserid;
            DataContext = HeaderBinding.GetDetails();
        }

        //-----------------------------------------------------------Window Loaded----------------------------------------------------------- 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //AllbuttonAnimations();
            //RecentChatAnimation();        
            GetLoginName(SYSTEM_STATIC.currentUser.userName);

            LoadAllUsers();

            getContacts();

            LoadAllRecentChats();

            loadAllGroups();

            aTimer = new System.Timers.Timer(20000);
           
            aTimer.Elapsed += OnTimer;
            aTimer.Enabled = true;
            OnetoOneMessageCounter();

            GroupMessageCounter();


        }




        //--------------------------------------------------------- Animations--------------------------------------------------------- 


        public void TickerAnimation()
        {
           
        }
        public void AllbuttonAnimations()
        {
            // buttons Animations
            //DoubleAnimation dbl = new DoubleAnimation();
            //dbl.From = 0;
            //dbl.To = btnGroups.ActualHeight;
            //dbl.Duration = TimeSpan.FromSeconds(5);
            ////dbl.RepeatBehavior = RepeatBehavior.Forever;
            //btnRecentChat.BeginAnimation(HeightProperty, dbl);
            //btnActive.BeginAnimation(HeightProperty, dbl);
            //btnContacts.BeginAnimation(HeightProperty, dbl);
            //btnGroups.BeginAnimation(HeightProperty, dbl);
            //btnMakeNewGroup.BeginAnimation(HeightProperty, dbl);


            //dbl.EasingFunction = new QuarticEase();
            
        }
        public void RecentChatAnimation()
        {
            DoubleAnimation ListsAnimation = new DoubleAnimation();

            ListsAnimation.From = 0;
            ListsAnimation.To = lstChat.ActualHeight;
            ListsAnimation.Duration = TimeSpan.FromSeconds(5);
            lstChat.BeginAnimation(HeightProperty, ListsAnimation);
            lstRecentChat.BeginAnimation(HeightProperty, ListsAnimation);
            ListsAnimation.EasingFunction = new QuarticEase();
        }


        //--------------------------------------------------------- Messsage Counter Work and group Checker Timer--------------------------------------------------------- 

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            GroupChecker();
        }

        public void OnetoOneMessageCounter()
        {  
            Dispatcher.Invoke(() => 
            {
                oneToOnemsgCounter = 0;
                foreach (ucRecentChats items in lstRecentChat.Items)
                {
                    if(string.IsNullOrWhiteSpace(items.msgCounter.Text))
                    {
                        oneToOnemsgCounter += 0;
                    }
                    else
                        oneToOnemsgCounter += Convert.ToInt32(items.msgCounter.Text);
                }               
                if(oneToOnemsgCounter == 0)
                    brdrOneToOneMsgCounter.Visibility = Visibility.Collapsed;
                else
                    brdrOneToOneMsgCounter.Visibility = Visibility.Visible;
                mainRecentCounter.Text = oneToOnemsgCounter.ToString();
            }, DispatcherPriority.ContextIdle);
            ErpIconChanger();
        }
        public void GroupMessageCounter()
        {
            Dispatcher.Invoke(() =>
            {
                groupMainMsgCounter = 0;
                foreach (ucRecentChats items in lstGroups.Items)
                {
                    if (string.IsNullOrWhiteSpace(items.msgCounter.Text))
                    {
                        groupMainMsgCounter += 0;
                    }
                    else
                        groupMainMsgCounter += Convert.ToInt32(items.msgCounter.Text);
                }

                if (groupMainMsgCounter == 0)
                    bdrGroupMsgCounter.Visibility = Visibility.Collapsed;
                else
                bdrGroupMsgCounter.Visibility = Visibility.Visible;
                groupMainCounter.Text = groupMainMsgCounter.ToString();
            }, DispatcherPriority.ContextIdle);
            ErpIconChanger ();
        }
        //--------------------------------------------------------- Messsage Counter Work and group Checker Timer--------------------------------------------------------- 


        public void GetLoginName(string userName)
        {
            ucLoginBinding._userName = userName;
            ucLoginBinding._initials = ExtractInitialsFromName(userName);
            ucLogin obj = new ucLogin();
            loginGrid.Children.Add(obj);
        }
        //----------------------------------------------------------Interface related Code On Mouse Enter mouse leaves buttons opacities changing------------------------------------ 


        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            btnVideoCall.Opacity = 10;
        }
        private void VideoButton_MouseLeave(object sender, MouseEventArgs e)
        {
            btnVideoCall.Opacity = 0.7;
        }
        private void BtnAudioCall_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAudioCall.Opacity = 10;
        }
        private void BtnAudioCall_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAudioCall.Opacity = 0.7;
        }
        private void BtnCreatGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            btnCreatGroup.Opacity = 10;

        }
        private void BtnCreatGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            btnCreatGroup.Opacity = 0.7;
        }
        private void BtnAddFile_MouseEnter(object sender, MouseEventArgs e)
        {
            btnAddFile.Opacity = 10;
        }
        private void BtnAddFile_MouseLeave(object sender, MouseEventArgs e)
        {
            btnAddFile.Opacity = 0.7;
        }
        private void BtnSendContact_MouseEnter(object sender, MouseEventArgs e)
        {
            btnSendContact.Opacity = 10;
        }
        private void BtnSendContact_MouseLeave(object sender, MouseEventArgs e)
        {
            btnSendContact.Opacity = 0.7;

        }
        private void BtnVoiceRecord_MouseEnter(object sender, MouseEventArgs e)
        {
            btnVoiceRecord.Opacity = 10;

        }
        private void BtnVoiceRecord_MouseLeave(object sender, MouseEventArgs e)
        {
            btnVoiceRecord.Opacity = 0.7;
        }


        //--------------------------------------------------------------------Navigations Buttons--------------------------------------------------------------


        private void BtnRecentChat_Click(object sender, RoutedEventArgs e)
        {
            //DoubleAnimation dbl = new DoubleAnimation();
            //dbl.From = 0;
            //dbl.To = 1550;
            //dbl.Duration = TimeSpan.FromSeconds(5);
            //lstRecentChat.BeginAnimation(HeightProperty, dbl);
            //dbl.EasingFunction = new QuarticEase();

            //player.SoundLocation = @"E:\Projects\ZAS-ERP-Repo\MK-ERP\ZAS_ERP\Chat\Sounds\msgBeep.wav";
            //player.Play();
            lstGroups.Visibility = Visibility.Collapsed;
            chatHeaderName.Text = "";
            
            lblChats.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Blue);
            lblContacts.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblGroups.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblActive.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);

            contactsSearchGrid.Visibility = Visibility.Collapsed;
            recentSearchGrid.Visibility = Visibility.Visible;
            lstActive.Visibility = Visibility.Collapsed;
            lstRecentChat.Visibility = Visibility.Visible;
            lstContacts.Visibility = Visibility.Collapsed;
            btnActive.IsChecked = false;
            btnGroups.IsChecked = false;
            btnContacts.IsChecked = false;


        }

        private void BtnActive_Click(object sender, RoutedEventArgs e)
        {
           
            lstGroups.Visibility = Visibility.Collapsed;
            lstChat.Visibility = Visibility.Collapsed;
            chatHeaderName.Text = "";
            lstActive.Visibility = Visibility.Visible;
            lstRecentChat.Visibility = Visibility.Collapsed;
            lstContacts.Visibility = Visibility.Collapsed;
            btnRecentChat.IsChecked = false;
            btnContacts.IsChecked = false;
            btnGroups.IsChecked = false;

            lblChats.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblContacts.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblGroups.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblActive.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Blue);


        }

        private void BtnContacts_Click(object sender, RoutedEventArgs e)
        {
            //DoubleAnimation dbl = new DoubleAnimation();
            //dbl.From = 0;
            //dbl.To = 1550;
            //dbl.Duration = TimeSpan.FromSeconds(2);
            //lstContacts.BeginAnimation(HeightProperty, dbl);
            //dbl.EasingFunction = new QuarticEase();

            msgIdentifier = 0;
            lstGroups.Visibility = Visibility.Collapsed;

            chatHeaderName.Text = "";
            contactsSearchGrid.Visibility = Visibility.Visible;
            recentSearchGrid.Visibility = Visibility.Collapsed;
            lstActive.Visibility = Visibility.Collapsed;
            lstRecentChat.Visibility = Visibility.Collapsed;
            lstContacts.Visibility = Visibility.Visible;
            btnGroups.IsChecked = false;
            btnRecentChat.IsChecked = false;
            btnActive.IsChecked = false;

            lblChats.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblContacts.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Blue);
            lblGroups.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblActive.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            
        }

        private void BtnGroups_Click(object sender, RoutedEventArgs e)
        {

            //DoubleAnimation dbl = new DoubleAnimation();


            //dbl.From = 0;
            //dbl.To = 1550;
            //dbl.Duration = TimeSpan.FromSeconds(5);
            //lstGroups.BeginAnimation(HeightProperty, dbl);
            //dbl.EasingFunction = new QuarticEase();

            msgIdentifier = 1;
            chatHeaderName.Text = "";
            lstActive.Visibility = Visibility.Collapsed;
            lstRecentChat.Visibility = Visibility.Collapsed;
            lstContacts.Visibility = Visibility.Collapsed;
            lstGroups.Visibility = Visibility.Visible;
            btnActive.IsChecked = false;
            btnContacts.IsChecked = false;
            btnRecentChat.IsChecked = false;
            lblChats.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblContacts.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
            lblGroups.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Blue);
            lblActive.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Black);
        }
       
        
//-----------------------------------------------------------------------------Groups Work----------------------------------------------------------------------------------------------------
      
        public void loadAllGroups()
        {
            try { 
            var getGroups = cm.GetAllGroups(false);
            List<ucRecentChats> groups = new List<ucRecentChats>();
            var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(loggedinId));
            ChatUser loggedInUser = new ChatUser()
            {
                employeeId = SYSTEM_STATIC.currentUser.employeeId,
                userName = userDetail.userName
            };
            foreach (var group in getGroups)
            {
                foreach (var user in group.users)
                {
                    if (user.employeeId == loggedInUser.employeeId)
                    {
                        try
                        {
                            var msg = cm.getGroupMsg(group.id, false);
                            if (msg == null||msg.Count==0)
                            {
                                ucRecentBinding._userName = group.groupName;
                                ucRecentBinding._msgBody = "";
                                ucRecentBinding._initials = ExtractInitialsFromName(group.groupName);
                                ucRecentBinding._contactId = group.id;
                                ucRecentChats obj = new ucRecentChats();
                                groups.Add(obj);
                                GroupMessage grpMessage = new GroupMessage()
                                {
                                    MessageBody = "",
                                    msgId = 0,
                                    sender = loggedinId,
                                };
                                PrevGroupChats.Add(group.id, 0);
                            }
                            else
                            {
                                var lastmsg = msg.Last();
                                ucRecentBinding._userName = group.groupName;
                                ucRecentBinding._msgBody = lastmsg.MessageBody;
                                ucRecentBinding._initials = ExtractInitialsFromName(group.groupName);
                                ucRecentBinding._contactId = group.id;
                                ucRecentChats obj = new ucRecentChats();
                                groups.Add(obj);

                                GroupMessage grpMessage = new GroupMessage()
                                {
                                    MessageBody = lastmsg.MessageBody,
                                    msgId = lastmsg.msgId,
                                    sender = lastmsg.sender,
                                    readers = lastmsg.readers,
                                    attachPath = lastmsg.attachPath
                                };
                                PrevGroupChats.Add(group.id, lastmsg.msgId);
                            }
                        }
                        catch { }
                    }
                }
            }
            lstGroups.ItemsSource = groups;
                //LoadMsgsForRecentGroups();
                //GroupChecker();
            }
            catch { }
        }

        public async Task GroupChecker()
        {
            await Task.Run(() =>
            {
                //while (ch == 1)
                //{
                    try
                    {
                        int l = 0;
                        var groups = cm.GetAllGroups(false);

                        Dispatcher.Invoke(() => {

                            foreach (var m in PrevGroupChats)
                            {
                                //--------Need to change this code-------
                                var msg = cm.getGroupMsg(m.Key, false);
                                //var lastMsg = cm.getGroupMsg(m.Key, lstChat.Items.Count , false);

                                var lastMsg = msg.Last();
                                //--------Need to change this code-------
                                var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(lastMsg.sender));

                                foreach (var group in groups)
                                {
                                    if (group.id == m.Key && m.Value != lastMsg.msgId && lastMsg.sender == loggedinId)
                                    {
                                        //Dispatcher.Invoke(() =>
                                        //{
                                        var selectedGroup = (ucRecentChats)lstGroups.SelectedItem;
                                        selectedGroup.msgBody.Text = lastMsg.MessageBody;
                                        ucGroupSenderBinding._msgBody = lastMsg.MessageBody;
                                        ucGroupSenderBinding._senderName = userDetail.userName;
                                        ucGroupSenderBinding._msgId = lastMsg.msgId;
                                        ucGroupSenderBinding._seenStamp = DateTime.Now.ToString();
                                        ucGroupSender sent = new ucGroupSender();
                                        lstChat.Items.Add(sent);
                                        lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                                        //}, DispatcherPriority.ContextIdle);
                                        PrevGroupChats[group.id] = lastMsg.msgId;
                                        l = 1;
                                        break;
                                    }
                                    else
                                        if (group.id == m.Key && m.Value != lastMsg.msgId && lastMsg.sender != loggedinId)
                                    {
                                        //Dispatcher.Invoke(() =>
                                        //{
                                        var selectedGroup = (ucRecentChats)lstGroups.SelectedItem;
                                        if (selectedGroup != null && Convert.ToInt32(selectedGroup.contactId.Text) == group.id) {
                                            
                                                selectedGroup.msgBody.Text = lastMsg.MessageBody;
                                                ucGroupReceiverBinding._msgBody = lastMsg.MessageBody;
                                                ucGroupReceiverBinding._recName = userDetail.userName;
                                                ucGroupReceiverBinding._msgId = lastMsg.msgId.ToString();
                                                ucGroupReceiverBinding._seenStamp = DateTime.Now.ToString();
                                                ucGroupReceiver recver = new ucGroupReceiver();
                                                lstChat.Items.Add(recver);
                                                lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                                                PrevGroupChats[group.id] = lastMsg.msgId;
                                                l = 1;
                                                break;  
                                        }
                                        
                                        else
                                        {
                                            groupMsgCounter=groupMsgCounter+1;
                                            notifier.ShowInformation(userDetail.userName + " " + "to" + " " + group.groupName + "" + Environment.NewLine + "" + lastMsg.MessageBody);
                                           
                                            var grp = (ucRecentChats)lstGroups.Items[group.id];
                                            grp.msgCounter.Text = groupMsgCounter.ToString();
                                            grp.msgBody.Text = lastMsg.MessageBody;
                                            grp.brdrMsgCounter.Visibility = Visibility.Visible;
                                            PrevGroupChats[group.id] = lastMsg.msgId;
                                            GroupMessageCounter();
                                            
                                            l = 1;
                                            break;
                                        }
                                        //}, DispatcherPriority.ContextIdle);
                                    }
                                }
                                if (l == 1)
                                {
                                    break;
                                }
                            }

                        }, DispatcherPriority.ContextIdle);

                        GC.Collect();
                    }
                    catch { }
                //}
            });

        }

        public void  UpdateMessage( int groupId, string message , long msgId)
        {
            Dispatcher.Invoke(() =>
            {
                
                var updateGroup = (ucRecentChats)lstGroups.Items[groupId];
                updateGroup.msgBody.Text = message;
                updateGroup.lastMsgId.Text = msgId.ToString();
            });
        }
       
        private void LstGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                GroupMessageCounter();
                lstRecentChat.UnselectAll();
                lstContacts.UnselectAll();
                lstChat.Items.Clear();
                groupMsgCounter = 0;
                openChatId = 0;
                var selectedGroup = (ucRecentChats)lstGroups.SelectedItem;
                chatHeaderName.Text = selectedGroup.userName.Text;
                loadGroupUsers(Convert.ToInt32(selectedGroup.contactId.Text));
                groupUsers.Visibility = Visibility.Visible;
                LoadAllGroupChat(Convert.ToInt32(selectedGroup.contactId.Text));
                lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                
            } catch { }
            
        }
 
        public void LoadAllGroupChat(int id)
        {
            try
            {
                var msgs = cm.getGroupMsg(id, false);

                foreach (var msg in msgs)
                {
                    var message = msg.ToString();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(msg.sender));
                    if (msg.sender == loggedinId)
                    {
                        ucGroupSenderBinding._senderName = userDetail.userName;
                        ucGroupSenderBinding._msgBody = msg.MessageBody;
                        ucGroupSenderBinding._msgId = msg.msgId;
                        ucGroupSender ucSend = new ucGroupSender();
                        lstChat.Items.Add(ucSend);
                       
                    }
                    else
                    {
                        ucGroupReceiverBinding._recName = userDetail.userName;
                        ucGroupReceiverBinding._msgBody = msg.MessageBody;        
                        ucGroupReceiver obj = new ucGroupReceiver();
                        lstChat.Items.Add(obj);
                        
                    }

                }
                
            }
            catch { }
            lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
        }

        public void LoadMsgsForRecentGroups()
        {
            var groups = cm.GetAllGroups(false);

            foreach (var items in lstGroups.Items)
            {
                var group = (ucRecentChats)items;
                var msg = cm.getGroupMsg(Convert.ToInt32(group.contactId.Text), false);
                var lastMsg = msg.Last();
                group.msgBody.Text = lastMsg.MessageBody;
            }

        }
  
        public void loadGroupUsers(int id)
        {
            var group = cm.GetGroup(id, false);

            groupUsers.ItemsSource = group.users;
        }
      
        public void SendGroupMsg()
        {

            TextRange textRange = new TextRange(chatBox.Document.ContentStart, chatBox.Document.ContentEnd);

            if (string.IsNullOrEmpty(textRange.Text) || string.IsNullOrWhiteSpace(textRange.Text))
            {
                return;
            }
            else
            {
                PushGroupMsgToDB(textRange.Text);
                //ucGroupSenderBinding._senderName = SystemLogic.currentUser.userName;
                //ucGroupSenderBinding._msgBody = textRange.Text;
                //ucGroupSenderBinding._seenStamp = DateTime.Now.ToString();
                //ucGroupSenderBinding._readReceipt = readReceipt.Sent.ToString();
                //ucGroupSender send = new ucGroupSender();

                //var selectedGroup = (ucRecentChats)lstGroups.SelectedItem;

                //selectedGroup.msgBody.Text = textRange.Text;
                //lstChat.Items.Add(send);

                lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);

                chatBox.Document.Blocks.Clear();
            }
        }

        public void PushGroupMsgToDB(string message)
        {
            List<reader> readers = new List<reader>();

            var selectedGroup = (ucRecentChats)lstGroups.SelectedItem;

            var sGroup = cm.GetGroup(Convert.ToInt32(selectedGroup.contactId.Text), false);

            foreach (var items in sGroup.users)
            {
                readers.Add(items.toReader());
            }
            groupMsgId = lstChat.Items.Count;
            GroupMessage msg = new GroupMessage()
            {
                MessageBody = message,
                msgId = groupMsgId,
                sender = loggedinId,
                readers = readers
            };
            cm.pushGroupMsg(msg, sGroup, false);
        }

        private void BtnMakeNewGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //exampleMainWindow obj = new exampleMainWindow();
                MakeGroup obj = new MakeGroup();
                obj.ShowDialog();
            }
            catch { }
        }

//------------------------------------------------------------------OneToOneChat Work -----------------------------------------------------------------------------

        public void LoadAllUsers()
        {
            
            try
            { users = userRepo.getAllActiveUsersForChat(); }
            catch
            { }

        }
       
        public void getContacts() 
        {
            lstContacts.Items.Clear();
            foreach (var user in users)
            {
                ucContactsBinding._userName = user.userName;
                ucContactsBinding._initials = ExtractInitialsFromName(user.userName);
                ucContactsBinding._contactId = user.id;
                ucContacts contact = new ucContacts();
                lstContacts.Items.Add(contact);
                listCollectionContact.Add(contact);
            }
        }
       
        private void SearchContact_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchContact.Text) == false)
            {
                lstContacts.Items.Clear();
                foreach (ucContacts uc in listCollectionContact)
                {
                    if (uc.userName.Text.ToUpperInvariant().StartsWith(searchContact.Text))
                    {
                        //lstBox_recentChat.Items.Clear();
                        lstContacts.Items.Add(uc);
                    }
                }

                foreach (ucContacts uc in listCollectionContact)
                {


                    if (uc.userName.Text.ToLowerInvariant().StartsWith(searchContact.Text))
                    {
                        //lstBox_recentChat.Items.Clear();
                        lstContacts.Items.Add(uc);
                    }
                }
            }
            else
             if (searchContact.Text == "")
            {
                lstContacts.Items.Clear();
                foreach (var uc in listCollectionContact)
                {
                    lstContacts.Items.Add(uc);
                }
            }

        }
        
        private void SearchRecent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchRecent.Text) == false)
            {
                searchRecent.Text.ToLower();
                lstRecentChat.Items.Clear();
                foreach (ucRecentChats uc in listCollectionRecent)
                {

                    if (uc.userName.Text.ToUpperInvariant().StartsWith(searchRecent.Text))
                    {
                        //lstBox_recentChat.Items.Clear();
                        lstRecentChat.Items.Add(uc);
                    }
                }

                foreach (ucRecentChats uc in listCollectionRecent)
                {


                    if (uc.userName.Text.ToLowerInvariant().StartsWith(searchRecent.Text))
                    {
                        //lstBox_recentChat.Items.Clear();
                        lstRecentChat.Items.Add(uc);
                    }
                }
            }
            else
            if (searchRecent.Text == "")
            {
                lstRecentChat.Items.Clear();
                foreach (var uc in listCollectionRecent)
                {
                    lstRecentChat.Items.Add(uc);
                }
            }
        }
        
        private void LstContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            msgIdentifier = 0;
            lstChat.Items.Clear();
            
            try
            {
                lstRecentChat.UnselectAll();
                lstGroups.UnselectAll();
                lstChat.Visibility = Visibility.Visible;

                var selectedContact = (ucContacts)lstContacts.Items[lstContacts.SelectedIndex];

                chatHeaderName.Text = selectedContact.userName.Text;

                openChatId = Convert.ToInt32(selectedContact.contactId.Text);

               

                loadAllOneToOnemsgs();

                chatGrid.Visibility = Visibility.Visible;

                chatBox.Focus();

                openChatId = Convert.ToInt32(selectedContact.contactId.Text);
            }
            catch { }
        }


        public string ExtractInitialsFromName(string name)
        {
            string initials = Regex.Replace(name, @"[\p{P}\p{S}\p{C}\p{N}]+", "");
            initials = Regex.Replace(initials, @"\p{Z}+", " ");
            initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);
            initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();
            if (initials.Length > 2)
            {
                initials = initials.Substring(0, 2);
            }
            return initials.ToUpperInvariant();
        }
        public async Task loadAllOneToOnemsgs() 
        {
            try
            {
                var msgs = cm.getOne2OneMsg(loggedinId, openChatId);
                if (msgs != null)
                {
                    foreach (var msg in msgs)
                    {
                        if (msg.receiver == loggedinId)
                        {
                            if (!string.IsNullOrEmpty(msg.attachPath) && msg.attachPath != "NA")
                            {
                                var fName = System.IO.Path.GetFileName(msg.attachPath);
                                ucReceiverPicture pic = new ucReceiverPicture();
                                pic.fileName.Content = fName;
                                pic.localPath.Text = msg.attachPath;
                                pic.receiver.Text = msg.receiver.ToString();
                                pic.sender.Text = msg.sender.ToString();
                                
                                //total += new FileInfo(fName).Length;
                                //float size = total / 1024;
                                //if (size < 1)
                                //{
                                //    pic.fileSize.Content = Convert.ToByte(size) + "Kb";
                                //}
                                //else
                                //    pic.fileSize.Content = size + "Mb";

                                pic.btnDownload.Click += new RoutedEventHandler(Downloading);
                                lstChat.Items.Add(pic);
                            }
                            else
                            {
                                ucReceiverBinding._msgBody = msg.messageBody;
                                ucReceiverBinding._seenStamp = Convert.ToString(msg.seenStamp);
                                ucReceiver reciever = new ucReceiver();
                                lstChat.Items.Add(reciever);
                            }
                        }
                        else
                        if (msg.sender == loggedinId)
                        {
                            if (!string.IsNullOrEmpty(msg.attachPath) && msg.attachPath != "NA")
                            {
                                var fName = System.IO.Path.GetFileName(msg.attachPath);

                                ucSenderPicture pic = new ucSenderPicture();

                                pic.fileName.Content = fName;

                                //total += new FileInfo(fName).Length;

                                //float size = total / 1024;

                                //if (size < 1)
                                //{
                                //    pic.fileSize.Content = Convert.ToByte(size) + "Kb";

                                //}
                                //else
                                //    pic.fileSize.Content = size + "Mb";

                                //var dFilePath=att.downloadFile(ERP_BL.Enums.ChatFileType.One2One, fName, msg.sender, msg.receiver);

                                pic.btnFile.Click += new RoutedEventHandler(OpenAttachement);

                                selectedFileName = msg.attachPath;

                                lstChat.Items.Add(pic);
                            }
                            else
                            {
                                ucSenderBinding._msgBody = msg.messageBody;
                                ucSenderBinding._seenStamp = msg.seenStamp.ToString();
                                ucSenderBinding._readReceipt = msg.readReceipt.ToString();
                                ucSender item = new ucSender();
                                lstChat.Items.Add(item);
                            }

                        }
                    }
                }

                lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
            }
            catch
            {
            }
            lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);

        }
       
        private void ChatGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (msgIdentifier == 0)
                {
                    SendOnetoOneMsg();
                }
                else
                    if (msgIdentifier == 1)
                {
                    SendGroupMsg();
                }
            }
        }
             
        public void SendOnetoOneMsg() 
        {

            try
            {
                TextRange textRange = new TextRange(chatBox.Document.ContentStart, chatBox.Document.ContentEnd);

                ucSenderPicture obj = new ucSenderPicture();
                if (!string.IsNullOrWhiteSpace(fileAttachPath) && openChatId != 0)
                {
                    //ucSenderPictureBinding._fileName = fileName;
                    //ucSenderPictureBinding._fileSize = fileSize;
                    //ucSenderPicture senderFile = new ucSenderPicture();
                    //senderFile.Margin = new Thickness(600, 0, 0, 0);
                    //senderFile.brdrClose.Visibility = Visibility.Collapsed;
                    //senderFile.btnFile.Click += new RoutedEventHandler(OpenAttachement);
                    //ShowThumbnail(fileIcon);

                    //PushMsgtoDb(textRange.Text, DateTime.Now, openChatId, readReceipt.Sent, loggedinId, lstChat.Items.Count, fileAttachPath);

                    //lstChat.Items.Add(senderFile);

                }
                else
                if (string.IsNullOrEmpty(textRange.Text) || string.IsNullOrWhiteSpace(textRange.Text))
                {
                    return;
                }
                else
                {
                    ucSenderBinding._msgBody = textRange.Text;
                    ucSenderBinding._readReceipt = readReceipt.Sent.ToString();
                    ucSenderBinding._seenStamp = DateTime.Now.ToString();
                    ucSenderBinding._recId = openChatId.ToString();
                    ucSender send = new ucSender();

                    lstChat.Items.Add(send);

                    lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);

                    lastmsgId = Convert.ToInt64(lstChat.Items.Count);
                    PushMsgtoDb(textRange.Text, DateTime.Now, openChatId, readReceipt.Sent, loggedinId, lstChat.Items.Count, "");
                    chatBox.Document.Blocks.Clear();
                }
            }
            catch { }
           
        }
        
        public void PushMsgtoDb(string msgBody, DateTime time, int receiverId, readReceipt readRcpt, int senderId, long msgId, string attachPath1)
        {
            if(!String.IsNullOrEmpty(attachPath1))
            {
                att.uploadFile(ERP_BL.Enums.ChatFileType.One2One, fileAttachPath, loggedinId, openChatId);
                fileAttachPath = null;
                fileSize = null;
                fileIcon = null;
                fileName = null;
                grpboxChat.Children.RemoveAt(1);   
            }    
            One2OneMessage msg = new One2OneMessage
            {
                messageBody = msgBody,
                seenStamp = time,
                receiver = receiverId,
                readReceipt = readRcpt,
                sender = senderId,
                msgId = msgId,
                attachPath=attachPath1
            };
            cm.pushOne2OneMsg(msg);
        }
        
        private Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
               parentWindow: Application.Current.MainWindow,
               corner: Corner.BottomRight,
               offsetX: 10,
               offsetY: 10);
            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(30),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));
            cfg.Dispatcher = Application.Current.Dispatcher;
        });
        
        private void LstRecentChat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {  
            groupUsers.Visibility = Visibility.Collapsed;
            msgIdentifier = 0;
            try
            {
                lstGroups.UnselectAll();
                lstContacts.UnselectAll();
                lstChat.Visibility = Visibility.Visible;
                lstChat.Items.Clear();

                var selectedContact = (ucRecentChats)lstRecentChat.Items[lstRecentChat.SelectedIndex];


                selectedContact.msgCounter.Visibility = Visibility.Collapsed;

                selectedContact.msgCounter.Text = null;

                msgCounter = 0;

                chatHeaderName.Text = selectedContact.userName.Text;

                openChatId = Convert.ToInt32(selectedContact.contactId.Text);

                loadAllOneToOnemsgs();

                chatGrid.Visibility = Visibility.Visible;

                chatBox.Focus();

                openChatId = Convert.ToInt32(selectedContact.contactId.Text);
                OnetoOneMessageCounter();
            }
            catch { }
        }
    
        public async Task Checker()
        {
            await Task.Run(() =>
            {
                while (startChecker == 1)
                {
                    int i = 0;
                    int l = 0;
                    var chat = cm.getOne2OneMsg(loggedinId);
                    foreach (var _msg in chat.Children<JObject>())
                    {
                        foreach (var prop in _msg.Properties())
                        {
                            var msgs = _msg.SelectToken(prop.Name).Children();

                            var arr = prop.ToArray().Last();
                            var msg6 = arr.Children();
                            if (string.IsNullOrEmpty(arr.ToString()))
                                return;
                            else
                            {
                                One2OneMessage msgObj = null;
                                try
                                {
                                    var temp = msg6.Last();

                                    msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());
                                }
                                catch
                                {
                                    var temp = msg6.Last().Children().First();
                                    msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());
                                }
                                Dispatcher.Invoke(() =>
                                {
                                    foreach (var obj in PrevChats)
                                    {
                                        var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(prop.Name));
                                        if (userDetail != null)
                                            //          Code for new Chat User    //
                                            if (!PrevChats.Keys.Contains(Convert.ToInt32(prop.Name)))
                                            {
                                                ucRecentBinding._userName = userDetail.userName;
                                                ucRecentBinding._initials = ExtractInitialsFromName(userDetail.userName);
                                                ucRecentBinding._msgBody = msgObj.messageBody;
                                                ucRecentBinding._seenStamp = msgObj.seenStamp.ToString();
                                                ucRecentBinding._contactId = Convert.ToInt32(prop.Name);
                                                ucRecentBinding._receiver = msgObj.receiver;
                                                ucRecentBinding._sender = msgObj.sender;
                                                if (msgObj.sender != loggedinId)
                                                {
                                                    if (obj.Key != openChatId)
                                                    {
                                                        Dispatcher.Invoke(() => {
                                                            notifier.ShowInformation(userDetail.userName + Environment.NewLine + msgObj.messageBody.ToString());
                                                            OnetoOneMessageCounter();
                                                        }, DispatcherPriority.ContextIdle);
                                                        //notifier.ShowInformation(userDetail.userName + Environment.NewLine + msgObj.messageBody.ToString());

                                                        ucRecentChats item = new ucRecentChats();
                                                        lstRecentChat.Items.Add(item);
                                                        One2OneMessage newMsg = new One2OneMessage()
                                                        {
                                                            msgId = msgObj.msgId,
                                                            messageBody = msgObj.messageBody,
                                                            sender = msgObj.sender,
                                                            receiver = msgObj.receiver,
                                                            seenStamp = msgObj.seenStamp,
                                                            readReceipt = msgObj.readReceipt
                                                        };
                                                        PrevChats.Add(Convert.ToInt32(prop.Name), newMsg);
                                                        var list = PrevChats.Keys.ToList();
                                                        list.Sort();
                                                    }
                                                    else
                                                    {

                                                        ucReceiverBinding._msgBody = msgObj.messageBody;
                                                        ucReceiverBinding._seenStamp = msgObj.seenStamp.ToString();
                                                        ucReceiver receiverMsg = new ucReceiver();
                                                        lstChat.Items.Add(receiverMsg);
                                                    }
                                                }
                                                else
                                                {
                                                    //flag2 = 0;
                                                    ucRecentChats item = new ucRecentChats();
                                                    lstRecentChat.Items.Add(item);
                                                    One2OneMessage newMsg = new One2OneMessage()
                                                    {
                                                        msgId = msgObj.msgId,
                                                        messageBody = msgObj.messageBody,
                                                        sender = msgObj.sender,
                                                        receiver = msgObj.receiver,
                                                        seenStamp = msgObj.seenStamp,
                                                        readReceipt = msgObj.readReceipt
                                                    };
                                                    PrevChats.Add(Convert.ToInt32(prop.Name), newMsg);
                                                    var list = PrevChats.Keys.ToList();
                                                    list.Sort();
                                                }
                                                if (i < lstRecentChat.Items.Count)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                                continue;
                                    }
                                }, DispatcherPriority.ContextIdle);
                            }
                        }
                        try
                        {
                            foreach (var obj1 in PrevChats)
                            {
                                var msgs1 = _msg.Properties();
                                var ob = _msg.SelectToken(obj1.Key.ToString()).Children();
                                var arr1 = msgs1.ToArray().Last();
                                var msg8 = arr1.Children();
                                if (string.IsNullOrEmpty(arr1.ToString()))
                                    return;
                                else
                                {
                                    One2OneMessage msgObj1 = null;
                                    try
                                    {
                                        var temp1 = ob.Last();
                                        msgObj1 = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp1.ToString());
                                    }
                                    catch
                                    {
                                        var temp1 = ob.Last().Children().First();
                                        msgObj1 = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp1.ToString());
                                    }
                                    //                                        Code for Delivered                          //

                                    if (obj1.Value.readReceipt != msgObj1.readReceipt && msgObj1.sender == loggedinId && msgObj1.readReceipt != readReceipt.Seen)
                                    {
                                        Dispatcher.Invoke(() =>
                                        {
                                            var value = lstChat.Items.Count - 1;
                                            var select = (ucSender)lstChat.Items[value];
                                            select.readReceipt.Text = readReceipt.Delivered.ToString();
                                            obj1.Value.readReceipt = msgObj1.readReceipt;

                                        }, DispatcherPriority.ContextIdle);
                                        var list = PrevChats.ToList();
                                        list.Sort();
                                        break;
                                    }
                                    else
                                    {
                                        if (obj1.Value.readReceipt != msgObj1.readReceipt && msgObj1.receiver == openChatId && msgObj1.sender == loggedinId)
                                        {
                                            Dispatcher.Invoke(() =>
                                            {
                                                var value = lstChat.Items.Count - 1;
                                                var select = (ucSender)lstChat.Items[value];
                                                select.readReceipt.Text = readReceipt.Seen.ToString();
                                                obj1.Value.readReceipt = msgObj1.readReceipt;
                                            }, DispatcherPriority.ContextIdle);
                                            var list = PrevChats.ToList();
                                            list.Sort();
                                            break;
                                        }
                                    }
                                    var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(obj1.Key));
                                    if (userDetail != null)

                                        if (obj1.Value.msgId != msgObj1.msgId)
                                        {

                                            Dispatcher.Invoke(() =>
                                            {
                                                var slctdItem = (ucRecentChats)lstRecentChat.Items[l];
                                                slctdItem.msgBody.Text = msgObj1.messageBody;
                                                slctdItem.seenStamp.Text = msgObj1.seenStamp.ToString();
                                                ucReceiverBinding._msgBody = msgObj1.messageBody;
                                                ucReceiverBinding._seenStamp = msgObj1.seenStamp.ToString();

                                                if (msgObj1.sender != loggedinId)
                                                {
                                                    if (obj1.Key != openChatId)
                                                    {
                                                        msgCounter++;
                                                        slctdItem.msgCounter.Text = msgCounter.ToString();
                                                        slctdItem.brdrMsgCounter.Visibility = Visibility.Visible;

                                                        
                                                        Dispatcher.Invoke(() => {
                                                            notifier.ShowInformation(userDetail.userName + Environment.NewLine + msgObj1.messageBody.ToString());
                                                            OnetoOneMessageCounter();
                                                        }, DispatcherPriority.ContextIdle);
                                                        //notifier.ShowInformation(userDetail.userName + Environment.NewLine + msgObj1.messageBody.ToString());

                                                    }
                                                    else
                                                    {
                                                        if (!String.IsNullOrEmpty(msgObj1.attachPath))
                                                        {
                                                            DownloadFile(ERP_BL.Enums.ChatFileType.One2One, msgObj1.attachPath, msgObj1.sender, msgObj1.receiver);
                                                        }
                                                        else
                                                        {
                                                            ucReceiver rec = new ucReceiver();
                                                            ucReceiverBinding._msgBody = msgObj1.messageBody;
                                                            lstChat.Items.Add(rec);
                                                            lstChat.ScrollIntoView(lstChat.Items[lstChat.Items.Count - 1]);
                                                        }

                                                    }
                                                    obj1.Value.msgId = msgObj1.msgId;
                                                }
                                                else

                                                    obj1.Value.msgId = msgObj1.msgId;
                                            }, DispatcherPriority.ContextIdle);
                                        }
                                }
                                if (l < prevProps.Keys.Count)
                                {
                                    l++;
                                }
                            }
                        }
                        catch { }
                    }
                }
            });
            GC.Collect();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var selectedMsg = (ucReceiverPicture)lstChat.Items[lstChat.SelectedIndex];
            Process.Start(selectedMsg.downloadPath.Text);
        }

        private void Downloading(object sender, RoutedEventArgs e)
        {

            var selectedMsg = lstChat.Items[lstChat.SelectedIndex] as ucReceiverPicture;

            var downloadPath = att.downloadFile(ERP_BL.Enums.ChatFileType.One2One, selectedMsg.localPath.Text, Convert.ToInt32(selectedMsg.sender.Text), Convert.ToInt32(selectedMsg.receiver.Text));

            selectedMsg.downloadPath.Text = downloadPath;

            selectedMsg.btnOpen.Click += new RoutedEventHandler(OpenFile);

            selectedMsg.btnDownload.Visibility = Visibility.Collapsed;

            selectedMsg.btnOpen.Visibility = Visibility.Visible;
        }

       
        public void DownloadFile(ERP_BL.Enums.ChatFileType fType, string lpath, int senderId, int receiverId)
        {
            ucReceiverPicture attachFile = new ucReceiverPicture();

            attachFile.fileType.Text = type.ToString();

            attachFile.localPath.Text = lpath;

            attachFile.sender.Text = senderId.ToString();

            attachFile.btnDownload.Click += new RoutedEventHandler(Downloading);

            lstChat.Items.Add(attachFile);

        }

        public void LoadAllRecentChats()
        {
            reChecker:
            JArray chat = null;
            try
            {
                chat = cm.getOne2OneMsg(loggedinId);
                //chatBox.Text = chat.ToString();

                var item = chat.Children<JObject>();

                var sring = chat.ToString();

                if (sring == "[\r\n  null\r\n]")
                {
                    cm.pushOne2OneMsg(new One2OneMessage() { sender = 0, receiver = loggedinId, messageBody = "Welcome to Connect!", msgId = 0, readReceipt = 0 });
                    goto reChecker;
                }
                //int chatterId = -1;
                //foreach (var child in chat.Children())
                //{
                //    chatterId++;
                //    if (child.Children().Count() <= 0)
                //        continue;
                //    // 8 -> 0 chat
                //    Console.Write(child);
                //    foreach (var chatHead in child.Children())
                //    {
                //        // each message details. 
                //        MessageBox.Show("ChatterId : " +chatterId + chatHead.SelectToken("messageBody").ToString());
                //    }

                //}
                foreach (var _msg in chat.Children<JObject>())
                {
                    foreach (var prop in _msg.Properties())
                    {
                        if (prop.Name == "0")
                        {
                            continue;
                        }
                        var userDetail = users.FirstOrDefault(x => x.id == Convert.ToInt16(prop.Name));
                        if (userDetail != null)
                            ucRecentBinding._userName = userDetail.userName.ToString();
                        ucRecentBinding._initials = ExtractInitialsFromName(userDetail.userName.ToString());
                        var msgs = _msg.SelectToken(prop.Name).Children();
                        var arr = prop.ToArray().Last();
                        var msg6 = arr.Children();

                        if (string.IsNullOrEmpty(arr.ToString()))
                            return;
                        else
                        {
                            One2OneMessage msgObj = null;
                            try
                            {
                                var temp = msg6.Last();
                                msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());
                            }
                            catch
                            {
                                var temp = msg6.Last().Children().First();
                                msgObj = Newtonsoft.Json.JsonConvert.DeserializeObject<One2OneMessage>(temp.ToString());
                            }
                            if (!String.IsNullOrEmpty(msgObj.attachPath) && msgObj.attachPath != "NA")
                            {
                                ucRecentBinding._msgBody = msgObj.attachPath;
                            }
                            else
                            ucRecentBinding._msgBody = msgObj.messageBody;
                            ucRecentBinding._seenStamp = msgObj.seenStamp.ToString();
                            ucRecentBinding._readReceipt = msgObj.readReceipt.ToString();
                            ucRecentBinding._receiver = msgObj.receiver;
                            ucRecentBinding._sender = msgObj.sender;
                            ucRecentBinding._contactId = Convert.ToInt32(prop.Name);
                            ucRecentChats recent = new ucRecentChats();
                            lstRecentChat.Items.Add(recent);
                            listCollectionRecent.Add(recent);
                            prevProps.Add(Convert.ToInt32(prop.Name), Convert.ToInt32(msgObj.msgId));
                            One2OneMessage msg = new One2OneMessage()
                            {
                                msgId = msgObj.msgId,
                                messageBody = msgObj.messageBody,
                                sender = msgObj.sender,
                                receiver = msgObj.receiver,
                                seenStamp = msgObj.seenStamp,
                                readReceipt = msgObj.readReceipt
                            };
                            PrevChats.Add(Convert.ToInt32(prop.Name), msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            GC.Collect();
            Checker();
        }

        private void BtnMakeGroup_Click(object sender, RoutedEventArgs e)
        {
            lstChat.Visibility = Visibility.Collapsed;

            MakeGroup obj = new MakeGroup();

            obj.Show();
        }

        private void Reply_Click(object sender, RoutedEventArgs e)
        {
            chatBox.Focus();
            var senderItem = lstChat.Items[lstChat.SelectedIndex] as ucSender;
            try
            {
                if (lstChat.Items[lstChat.SelectedIndex] == (ucReceiver)lstChat.Items[lstChat.SelectedIndex])
                {
                    replyGrid.Visibility = Visibility.Visible;

                    var selectedChat = (ucReceiver)lstChat.Items[lstChat.SelectedIndex];

                    var reply = selectedChat.Receiver_chatbox.Text;

                    replyMsg.Text = "Reply To: \"" + reply + "\"";

                    // win.replyMsg.Text = sender_chatbox.Text;
                    //  Console.WriteLine("");
                }
            }
            catch { }

        }

        private void ReplyCLose_Click(object sender, RoutedEventArgs e)
        {
            replyMsg.Text = "";
            replyGrid.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    this.DragMove();
                }
        }

        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //MaximizeButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                //MaximizeButton.Content = "2";
            }

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            //this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        private void BtnAttach_Click(object sender, RoutedEventArgs e)
        {

            if (openChatId != 0)
            {
                TextRange textRange = new TextRange(chatBox.Document.ContentStart, chatBox.Document.ContentEnd);

                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
                {
                    Multiselect = false,
                    ValidateNames = true,
                    Filter = "All files|*.*",
                };
               
                Nullable<bool> result = dlg.ShowDialog();
               
                if (result == true)
                {
                    // Open document   
                    //fileAttachPath = dlg.FileName;
                    
                    //ucSenderPictureBinding._fileName = dlg.SafeFileName;
                    //fileName = dlg.SafeFileName;
                    //total += new FileInfo(fileAttachPath).Length;
                    //float size = total / 1024;
                    //if (size < 1)
                    //{
                    //    ucSenderPictureBinding._fileSize = Convert.ToByte(size) + " " + "Kb";
                    //    fileSize = Convert.ToByte(size) + " " + "Kb";
                       
                    //}
                    //else
                    //    ucSenderPictureBinding._fileSize = size + " " + "Mb";
                    //fileSize = size + " " + "Mb";

                    //fileIcon = System.IO.Path.GetExtension(dlg.FileName);

                    //ShowThumbnail(fileIcon);

                    //ucSenderPicture pic = new ucSenderPicture();
                    //pic.btnFile.Click += new RoutedEventHandler(OpenAttachement);

                    //selectedFileName = dlg.FileName;

                    //fileAttachPath = dlg.FileName;


                    //pic.SetValue(Grid.RowProperty, 0);

                    //pic.close.Click += new RoutedEventHandler(CloseFile);

                    //grpboxChat.Children.Add(pic);

                    //chatBox.Focus();   
                }
            }
            else
                MessageBox.Show("Please select Contact or group to send Attachements");    
        }

        public void ShowThumbnail(string value)
        {
            switch (value)
            {
                //case ".png":
                //    ucSenderPictureBinding._fileIcon= "/ZAS_ERP;component/Chat/Images/iconPng.png";
                //    break;
                //case ".docx":
                //    ucSenderPictureBinding._fileIcon ="/ZAS_ERP;component/Chat/Images/iconWord.png";
                //    break;
                //case ".xlsx":
                //    ucSenderPictureBinding._fileIcon = "/ZAS_ERP;component/Chat/Images/iconExcel.png";
                //    break;
                //case ".pdf":
                //    ucSenderPictureBinding._fileIcon = "/ZAS_ERP;component/Chat/Images/iconWord.png";
                //    break;
                //case ".jpg":
                //    ucSenderPictureBinding._fileIcon = "/ZAS_ERP;component/Chat/Images/iconJpg.png";
                //    break;
            }
        }

        public void CloseFile(object sender, RoutedEventArgs e)
        {
            fileAttachPath = null;
            fileSize = null;
            fileIcon = null;
            fileName = null;
            grpboxChat.Children.RemoveAt(1);
        }

        private void OpenAttachement(object sender, RoutedEventArgs e)
        {
            Process.Start(selectedFileName);
        }

        private void ChatBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void ChatBox_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                tb.Text = string.Format("{0}", ((string[])text)[0]);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void ChatBox_Loaded(object sender, RoutedEventArgs e)
        {
            chatBox.AddHandler(RichTextBox.DragOverEvent, new DragEventHandler(RichTextBox_DragOver), true);
            chatBox.AddHandler(RichTextBox.DropEvent, new DragEventHandler(RichTextBox_Drop), true);
        }

        private void RichTextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] docPath = (string[])e.Data.GetData(DataFormats.FileDrop);

                // By default, open as Rich Text (RTF).
                var dataFormat = DataFormats.Bitmap;

                // If the Shift key is pressed, open as plain text.
                if (e.KeyStates == DragDropKeyStates.ShiftKey)
                {
                    dataFormat = DataFormats.Bitmap;
                }

                System.Windows.Documents.TextRange range;
                System.IO.FileStream fStream;
                if (System.IO.File.Exists(docPath[0]))
                {
                    try
                    {
                        // Open the document in the RichTextBox.
                        range = new System.Windows.Documents.TextRange(chatBox.Document.ContentStart, chatBox.Document.ContentEnd);
                        fStream = new System.IO.FileStream(docPath[0], System.IO.FileMode.OpenOrCreate);
                        range.Load(fStream, dataFormat);
                        fStream.Close();
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("File could not be opened. Make sure the file is a text file.");
                    }
                }
            }

        }
        private void RichTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        public void ErpIconChanger()
        {
            //if (groupMainMsgCounter != 0 || oneToOnemsgCounter!=0)
            //{
            //    myParent.btnChat.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/Chat/Images/iconUnreadChat.png", UriKind.RelativeOrAbsolute));
            //}
            //else
            //    myParent.btnChat.Glyph = new BitmapImage(new Uri("/ZAS_ERP;component/Chat/Images/iconReadChat.png", UriKind.RelativeOrAbsolute));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TickerAdmin obj = new TickerAdmin();
            //obj.Show();
        }

        
    }

    //---------------------------------------------------Ticker Work----------------------------------------------------------------------



}


