using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ERP_BL.Databases;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.XtraReports.UI;
using DevExpress.Xpf.Core;
using ERP_BL.Enums;

using ZAS_ERP.SaleOrderFolder.UserControls;
using ERP_BL.Reports;
using ZAS_ERP.Procurementss.Inquiriess.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;
using ZAS_ERP.Procurementss.Offerss.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss.UserControls;
using ZAS_ERP.Procurementss.Billss.UserControls;
using ZAS_ERP.Procurementss.PurchaseOrderss.UserControls;
using ZAS_ERP.Procurementss.SaleInvoicess.UserControls;
using ZAS_ERP.Userss;
using System.Threading;
using Microsoft.Win32;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.Windows;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.CreditCards.UserControls;
using ZAS_ERP.Employee;
using ZAS_ERP.Employeess;
using ZAS_ERP.HR.Leaves;
using ZAS_ERP.HR;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.ChartofAccounts.UserControls;
using ZAS_ERP.Tax;
using ZAS_ERP.TemplateFields;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.Windows;
using ZAS_ERP.Bankings.InterCompanyWindows;
using System.Deployment.Application;
using System.Reflection;
using ZAS_ERP.Calender;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using ZAS_ERP.UserTasks;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Payments.Windows;
using System.IO;
using ERP_BL.BackgroundImages;
using System.ComponentModel;
using ZAS_ERP.BackgroundUpload;
using ZAS_ERP.Procurementss.PurchaseInvoice.UserControls;
using ZAS_ERP.PopupNotificatios;
using TableDependency.SqlClient.Base.EventArgs;
using Notifications.Wpf;
using ZAS_ERP.Commentss;
using ZAS_ERP.Procurementss;
using System.Windows.Threading;

using ZAS_ERP.Procurementss.CostSheet;
using ZAS_ERP.Bankings.Loans;
using ZAS_ERP.Procurementss.Billss;
using ZAS_ERP.Procurementss.Inventory;
using ZAS_ERP.Bankings.Loans.UserControls;
using ZAS_ERP.Bankings.Loans.Windows;
using ZAS_ERP.Customerss;
using ZAS_ERP.ToDoTasks.Windows;
using System.Windows.Documents;
using ZAS_ERP.ToDoTasks.UserControls;
using ZAS_ERP.ExchangeRates.UserControls;
using ZAS_ERP.Procurementss.SharedReports;
using System.Diagnostics;

using ZAS_ERP.Bankings.InterBankTransfer.Windows;
using ZAS_ERP.Country;
using ZAS_ERP.CashBook;
using Microsoft.AspNetCore.SignalR.Client;

using System.Threading.Tasks;
using ERP_BL.ChartofAccounts;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Vendorss;
using ZAS_ERP.Procurementss.LoanAdvance.Windows;
using System.Globalization;
using ZAS_ERP.HR.Salary.EmploymentSalary.UserControls;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.Procurementss.Offerss;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Accordion;
using ZAS_ERP.Procurementss.Budget;
using System.Windows.Shell;
using ZAS_ERP.Principalss;
using ZAS_ERP.Reportss;
using ZAS_ERP.Procurementss.SaleOrderss;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings.STL.Windows;
using ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls;
using ZAS_ERP.Interest.UserControls;
using ZAS_ERP.MarginPerc.UserControls;
using ERP_BL.User;
using ZAS_ERP.Userss.Polling;
using ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows;
using ZAS_ERP.Reportss.ReportTitle;
using ZAS_ERP.Procurementss.StatusClasses.Windows;
using System.Collections.ObjectModel;
using ZAS_ERP.Procurementss.SaleInvoicess;
using ZAS_ERP.Procurementss.LoanAdvance.CompanyLoans.UserControls;
using ZAS_ERP.AssetRentalss.UserControls;
using ZAS_ERP.AssetRentalss.RentalOrderss.UserControls;
using ZAS_ERP.AssetRentalss.TenantRentals.UserControls;
using ZAS_ERP.Companiess.DepartmentLevel.Windows;
using ZAS_ERP.Memos;
using ZAS_ERP.VATBook.UserControls;
using ERP_BL.Procurements.Memos;
using ZAS_ERP.CashFlow.Windows;
using ZAS_ERP.FilesAndDocss.Documentss;
using ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls;
using ZAS_ERP.BussinessLogicss.IndustryType;
using ZAS_ERP.Memos.PerformanceReview;

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Item
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
    }

    public partial class MainWindow : DXRibbonWindow
    {

        int PopUpId = 0;
        int popUpimageId = 0;


        //ERP_BL.PopupNotificatios.popupNotificatinRepo PopupRepo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();
        //ERP_BL.PopupNotificatios.popupNotifications notifications = new ERP_BL.PopupNotificatios.popupNotifications();
        int IdChange = 0;
        public bool isInstant = true;
        public bool isShareAll = true;
        public bool isWindowLoad = true;
        //public static bool isSpecific = false;
        public static int isSpecific;
        bool bankListWindowFlag = false;
        bool deductionListWindowFlag = false;
        bool accntListWindowFlag = false;
        bool clctionMthdListWindowFlag = false;
        BackgroundWorker bgWorker = new BackgroundWorker();
        BitmapImage bmImg = new BitmapImage();
        OpenFileDialog fileDialog = new OpenFileDialog();
        public static int currentUserid;
        public static int groupCompanyid;
        public static string currentUserName;
        public bool? loadSentNotification = false;
        public int unReadCount { get; set; }
        public DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException { get; private set; }


        BackgroundImagesRepo imagesRepo = new BackgroundImagesRepo();
        GridReportRepo reportsRepo = new GridReportRepo();
        List<int> deptIds = new List<int>();
        List<int> compIds = new List<int>();
        List<int> empIds = new List<int>();
        List<SharedGridGroup> groupsWithCompany = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithDepartment = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithEmployee = new List<SharedGridGroup>();
        Process process = new Process();
        public HubConnection connection;



        string urlLocal = "http://localhost:53353/chathub";
        string urlLAN = "http://192.168.10.19:6001/chathub";
        string urlWAN = "http://203.135.42.34:6001/chathub";
        String unicodeConnected = "\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044";
        // string currentOnlineUser = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
        int currentOnlieUserId = SYSTEM_STATIC.currentUser.id;
        List<User> onlineUsers = new List<User>();
        User user = new User();
        UsersRepo rolesRepo = new UsersRepo();
        ucCommentsGrid grdNotifications = new ucCommentsGrid();
        string Error = "";

        InactivityTracker inactivityTracker;
        private winGridReportCenter standardReportCenter;
        private winGridReportCenter memorizedReportCenter;
        private winGridReportCenter reportCenter;

        public MainWindow()
        {
            try
            {
              

                InitializeComponent();

                InitializeInactivityTracker();


                mainWindow.Title = "ZAS-ERP " + "(" + getRunningVersion() + ")";
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void InitializeInactivityTracker()
        {
            TimeSpan inactivityThreshold = TimeSpan.FromMinutes(60); // Adjust the inactive time threshold as needed
            inactivityTracker = new InactivityTracker(inactivityThreshold);
            inactivityTracker.myParent = this;
            //inactivityTracker.myParent = this;
        }




        private void SetNotificationButtonVisibility()
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            var not = notificationsRepo.getNotification(SYSTEM_STATIC.currentUser.id);
       

            if (not > 0)
            {
                btnGlow.Visibility = Visibility.Visible;
                // draw an image to overlay
                var dg = new DrawingGroup();
                var dc = dg.Open();

                if (not >= 100)
                {
                    dc.DrawEllipse(Brushes.OrangeRed, new Pen(Brushes.OrangeRed, 1), new Point(15, 15), 15, 15);

                    dc.DrawText(new FormattedText(not.ToString(), System.Threading.Thread.CurrentThread.CurrentUICulture, System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial"), 15, Brushes.White), new Point(0, 7));
                }
                else if (not >= 10)
                {
                    dc.DrawEllipse(Brushes.OrangeRed, new Pen(Brushes.OrangeRed, 1), new Point(11, 11), 11, 11);

                    dc.DrawText(new FormattedText(not.ToString(), System.Threading.Thread.CurrentThread.CurrentUICulture, System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial"), 16, Brushes.White), new Point(1, 2));
                }
                else

                {
                    dc.DrawEllipse(Brushes.OrangeRed, new Pen(Brushes.OrangeRed, 1), new Point(8, 8), 8, 8);
                    dc.DrawText(new FormattedText(not.ToString(), System.Threading.Thread.CurrentThread.CurrentUICulture, System.Windows.FlowDirection.LeftToRight,
                    new Typeface("Arial"), 14, Brushes.White), new Point(4, 0));
                }
                   


                dc.Close();
                var geometryImage = new DrawingImage(dg);
                geometryImage.Freeze();

                // set on this window
                var tbi = new TaskbarItemInfo();
                tbi.Overlay = geometryImage;
                //tbi.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Paused;
                //tbi.ProgressValue = 1;
                ExtensionMethods.FlashWindow(this, 4);              
     
              



                this.TaskbarItemInfo = tbi;
            }
            else
            {
                btnGlow.Visibility = Visibility.Collapsed;
                //// draw an image to overlay
                //var dg = new DrawingGroup();
                //var dc = dg.Open();
                //dc.DrawEllipse(Brushes.Blue, new Pen(Brushes.LightBlue, 1), new Point(8, 8), 8, 8);
                //dc.DrawText(new FormattedText("3", System.Threading.Thread.CurrentThread.CurrentUICulture, System.Windows.FlowDirection.LeftToRight,
                //    new Typeface("Arial"), 16, Brushes.White), new Point(4, 0));
                //dc.Close();
                //var geometryImage = new DrawingImage(dg);
                //geometryImage.Freeze();

                //// set on this window
                //var tbi = new TaskbarItemInfo();
                //tbi.Overlay = geometryImage;

                this.TaskbarItemInfo = null;
            }
        }

        public void SetPollingButtonVisibility()
        {
            PollRepo pollRepo = new PollRepo();
            var count = pollRepo.GetInstantPollForUser(SYSTEM_STATIC.currentUser.id);
            if (count > 0)
            {
                mbtnPollingsGlow.Visibility = Visibility.Visible;
                mbtnPollings.Visibility = Visibility.Collapsed;
            }
            else
            {
                mbtnPollings.Visibility = Visibility.Visible;
                mbtnPollingsGlow.Visibility = Visibility.Collapsed;
            }
        }

        private async void connectToUrl()
        {
            try


            {
                if (!Debugger.IsAttached)
                {


                    if (SYSTEM_STATIC.server == "Fsociety96")
                    {
                        connection = new HubConnectionBuilder()
                                      .WithUrl(urlLAN)
                                      .Build();



                        connection.Closed += async (error) =>
                        {
                            await Task.Delay(new Random().Next(0, 5) * 1000);
                            await connection.StartAsync();
                        };


                        connection.On<string, int>("ReceiveMessage", (unicodeConnected, userId) =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {

                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f") //Polling work
                                {
                                    SetPollingButtonVisibility();
                                }
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e") //Urgent Notification work
                                {
                                    SetNotificationButtonVisibility();
                                }
                                if (unicodeConnected == "\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044") //CONNECTED
                                {
                                    rolesRepo = new UsersRepo();
                                    user = rolesRepo.getOnlineUser(userId);
                                    bool containsItem = onlineUsers.Any(x => x.id == userId);
                                    if (containsItem == true)
                                    {
                                        onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));

                                    }
                                    onlineUsers.Add(user);
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }



                                if (unicodeConnected == "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044")
                                {
                                    onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }

                                if (unicodeConnected == "\u0053\u0045\u004e\u0044\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e") //sendnotification
                                {
                                    sendInstantNotification();
                                }
                                if (unicodeConnected == "\u0042\u004c\u0049\u004e\u004b\u004f\u004e") //BLINKON
                                {
                                    BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                    var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                    if (groupImage.Count > 0)
                                    {
                                        mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                        blinkButton(mbtnBackgroundImage, 300, 20.0);
                                    }

                                }
                            }));
                        });
               

                        connection.On("ShowNotification", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {


                                sendInstantNotification();
                            }));

                        });

                        connection.On("ShowDesktopBackground", () =>
                        {

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                getBackground();


                            }));


                        });
                        connection.On("ShowUpdateNotification", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {


                                getUpdateNotification();
                            }));


                        });
                        connection.On("ShowBlinkOn", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {




                            }));

                        });
                        await connectAsync();
                    }
                    if (SYSTEM_STATIC.server == "119.156.232.242,1411")
                    {



                        connection = new HubConnectionBuilder()
                  .WithUrl(urlWAN)
                  .Build();


                        connection.Closed += async (error) =>
                        {

                            await Task.Delay(new Random().Next(0, 5) * 1000);
                            await connection.StartAsync();
                        };


                        connection.On<string, int>("ReceiveMessage", (unicodeConnected, userId) =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f") //Polling work
                                {
                                    SetPollingButtonVisibility();
                                }
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e")
                                {
                                    SetNotificationButtonVisibility();
                                }
                                if (unicodeConnected == "\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044") //CONNECTED
                                {
                                    rolesRepo = new UsersRepo();
                                    user = rolesRepo.getOnlineUser(userId);
                                    bool containsItem = onlineUsers.Any(x => x.id == userId);
                                    if (containsItem == true)
                                    {
                                        onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));

                                    }
                                    onlineUsers.Add(user);
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }



                                if (unicodeConnected == "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044")
                                {
                                    onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }
                                if (unicodeConnected == "\u0053\u0045\u004e\u0044\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e") //sendnotification
                                {
                                    sendInstantNotification();
                                }
                                if (unicodeConnected == "\u0042\u004c\u0049\u004e\u004b\u004f\u004e") //BLINKON
                                {
                                    BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                    var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                    if (groupImage.Count > 0)
                                    {
                                        mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                        blinkButton(mbtnBackgroundImage, 300, 20.0);
                                    }

                                }
                            }));
                        });

                        connection.On("ShowNotification", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {

                                sendInstantNotification();
                            }));

                        });

                        connection.On("ShowDesktopBackground", () =>

                        {

                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                getBackground();


                            }));


                        });

                        connection.On("ShowUpdateNotification", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                getUpdateNotification();
                            }));

                        });


                        connection.On("ShowBlinkOn", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                if (groupImage.Count > 0)
                                {
                                    mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                    blinkButton(mbtnBackgroundImage, 300, 20.0);
                                }


                            }));

                        });
                        await connectAsync();
                    }
                    if (SYSTEM_STATIC.server == @"(localdb)\ERPSOFTWARE3")
                    {
                        connection = new HubConnectionBuilder()


                                      .WithUrl(urlLocal)
                                      .Build();


                        connection.Closed += async (error) =>
                        {

                            await Task.Delay(new Random().Next(0, 5) * 1000);
                            await connection.StartAsync();
                        };


                        connection.On<string, int>("ReceiveMessage", (unicodeConnected, userId) =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f") //Polling work
                                {
                                    SetPollingButtonVisibility();
                                }
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e")
                                {
                                    SetNotificationButtonVisibility();
                                }
                                if (unicodeConnected == "\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044") //CONNECTED
                                {
                                    rolesRepo = new UsersRepo();
                                    user = rolesRepo.getOnlineUser(userId);
                                    bool containsItem = onlineUsers.Any(x => x.id == userId);
                                    if (containsItem == true)
                                    {

                                        onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));
                                    }
                                    onlineUsers.Add(user);
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }

                                if (unicodeConnected == "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044")
                                {
                                    //rolesRepo = new _usersRepo();
                                    // User LogoutUser;
                                    onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }
                                if (unicodeConnected == "\u0053\u0045\u004e\u0044\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e") //sendnotification
                                {
                                    sendInstantNotification();
                                }
                                if (unicodeConnected == "\u0042\u004c\u0049\u004e\u004b\u004f\u004e") //BLINKON
                                {
                                    BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                    var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                    if (groupImage.Count > 0)
                                    {
                                        mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                        blinkButton(mbtnBackgroundImage, 300, 20.0);
                                    }

                                }

                            }));
                        });
                        connection.On("ShowNotification", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                sendInstantNotification();
                            }));

                        });
                        connection.On("ShowBlinkOn", () =>
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                if (groupImage.Count > 0)
                                {
                                    mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                    blinkButton(mbtnBackgroundImage, 300, 20.0);
                                }


                            }));

                        });

                        await connectAsync();
                    }
                }

                else
                {



                    connection = new HubConnectionBuilder()
                                 .WithUrl(urlLocal)
                                 .Build();




                    connection.Closed += async (error) =>
                    {
                        await Task.Delay(new Random().Next(0, 5) * 1000);
                        await connection.StartAsync();

                    };

                    connection.On<string, int>("ReceiveMessage", (unicodeConnected, userId) =>
                    {
                        try
                        {
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f") //Polling work
                                {
                                    SetPollingButtonVisibility();
                                }
                                if (unicodeConnected == "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e")
                                {
                                    SetNotificationButtonVisibility();
                                }
                                if (unicodeConnected == "\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044") //CONNECTED
                                {
                                    rolesRepo = new UsersRepo();
                                    user = rolesRepo.getOnlineUser(userId);
                                    bool containsItem = onlineUsers.Any(x => x.id == userId);
                                    if (containsItem == true)
                                    {
                                        onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));

                                    }
                                    onlineUsers.Add(user);
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct().ToList();
                                }


                                if (unicodeConnected == "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044")
                                {
                                    //rolesRepo = new _usersRepo();
                                    //onlineUsers = new List<User>();
                                    //User LogoutUser;
                                    // user = rolesRepo.getOnlineUser(userId); 
                                    onlineUsers.Remove(onlineUsers.FirstOrDefault(x => x.id == userId));
                                    grdOnlineUsers.ItemsSource = null;
                                    grdOnlineUsers.ItemsSource = onlineUsers.Distinct();
                                }
                                if (unicodeConnected == "\u0053\u0045\u004e\u0044\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e") //sendnotification
                                {
                                    sendInstantNotification();
                                }
                                if (unicodeConnected == "\u0042\u004c\u0049\u004e\u004b\u004f\u004e") //BLINKON
                                {
                                    BackgroundImagesRepo repo = new BackgroundImagesRepo();
                                    var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);
                                    if (groupImage.Count > 0)
                                    {
                                        mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                        blinkButton(mbtnBackgroundImage, 300, 20.0);
                                    }

                                }
                            }));

                        }
                        catch (Exception ex)
                        {
                            DXMessageBox.Show(ex.Message);
                        }
                    });



                    connection.On("ShowNotification", () =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {


                            sendInstantNotification();
                        }));

                    });

                    connection.On("ShowDesktopBackground", () =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            getBackground();
                        }));

                    });


                    connection.On("ShowUpdateNotification", () =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            getUpdateNotification();
                        }));

                    });
                    connection.On("ShowBlinkOn", () =>
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            BackgroundImagesRepo repo = new BackgroundImagesRepo();
                            var groupImage = repo.GetUserTaskGroup(SYSTEM_STATIC.currentUser.id);

                            if (groupImage.Count > 0)
                            {
                                mbtnBackgroundImage.Foreground = Brushes.Yellow;
                                blinkButton(mbtnBackgroundImage, 300, 20.0);
                            }


                        }));

                    });

                    await connectAsync();
             

                }

            }
            catch (Exception ex)
            {
 
                DXMessageBox.Show(ex.Message, " Web server not found!");

            }

        }

        public async Task connectAsync()
        {
            try
            {
                await connection.StartAsync();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {

                getBackground();



                // getUpdateNotification();
            }));
        }
        private Version getRunningVersion()
        {
            try
            {
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch (Exception)
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }
        private void btnNewCompany_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            frmcompanyadd.Owner = this;
            frmcompanyadd.Show();
        }

        private void mbtnuserscenter_Click(object sender, RoutedEventArgs e)
        {
            Userss.frmUserssCenter frmUserss = new Userss.frmUserssCenter();
            frmUserss.Owner = this;
            frmUserss.Show();
        }
        private void mbtninquiryadd_Click(object sender, RoutedEventArgs e)
        {
            Inquiriess.frmInquiryadd inquiryadd = new Inquiriess.frmInquiryadd();
            inquiryadd.Owner = this;
            inquiryadd.Show();
        }

        private void mbtnCustomercenter_Click(object sender, RoutedEventArgs e)
        {
            Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter();
            customerCenter.Owner = this;
            customerCenter.Show();
        }
        public void LoadThemes()
        {

            List<cmbitem> list = new List<cmbitem>();
            cmbitem lightBlue = new cmbitem()
            {
                name = Themes.LightBlue.ToString()

            };
            cmbitem deepBlue = new cmbitem()
            {
                name = Themes.DeepBlue.ToString()

            };
            cmbitem silver = new cmbitem()
            {
                name = Themes.Silver.ToString()
            };

            list.Add(lightBlue);
            list.Add(deepBlue);
            list.Add(silver);


            cbTheme.ItemsSource = list;
        }

        private async void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                LoadThemes();
                compIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
                deptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
                btnUsermenu.Content = "Welcome " + currentUserName + "!";
                //btnUsermenu.Content = "Welcome " + currentUserName + "!";


                // load statuses in the background
                //System.Threading.Thread th = new Thread(() =>
                //{
                //    SYSTEM_STATIC.PopulateTransactionPanel();
                //});
                //th.Start(); 
                // StartTimer();
                Dispatcher.Invoke(() =>
                {
                    ZAS_ERP.Reportss.FavouriteReports.Windows.winFavouriteReports reports = new ZAS_ERP.Reportss.FavouriteReports.Windows.winFavouriteReports();
                    reports.Show();
                }, DispatcherPriority.ContextIdle);
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += OnDoWork;
                worker.RunWorkerAsync();
                bgWorker.DoWork += BgWorker_DoWork; //Create event
                if (!bgWorker.IsBusy) //Check busy or not
                    bgWorker.RunWorkerAsync();


                enableMenu();
                getCountAllUnreadNotifications();
                getCountAllUnreadMemoNotifications();


                Thread t = new Thread(new ThreadStart(TaskTimer));
                t.IsBackground = true;
                t.Start();
                var loginUser = SYSTEM_STATIC.currentUser;


                onlineUsers = rolesRepo.getOnlineUserList();
                LoginUserDetails userDetails = new LoginUserDetails();
                userDetails.LoginTime = DateTime.Now;
                userDetails.userId = loginUser.id;
                rolesRepo.updateLoginUser(loginUser.id, userDetails);

                connectToUrl();
                if (!Debugger.IsAttached)
                    await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);
                else
                    await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);


                //UI thread exception handling event

                this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);


                //Exception  in task thread
                TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
                //Exception caught by non UI thread
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                //BackgroundImagesRepo repo = new BackgroundImagesRepo();
                //var user = SYSTEM_STATIC.currentUser.id;
                //var groupImage = repo.GetUserTaskGroup(user);
                //if (groupImage != null)
                //{
                //    mbtnBackgroundImage.Foreground = Brushes.Yellow;
                //    blinkButton(mbtnBackgroundImage, 300, 20.0);
                //}
                //BackgroundImagesRepo repo = new BackgroundImagesRepo(); //Added
                //var user = SYSTEM_STATIC.currentUser.id;
                //var groupImage = repo.GetUserTaskGroup(user);
                //if (groupImage != null)
                //{

                if (SYSTEM_STATIC.currentUser.isBlink == true)
                {
                    mbtnBackgroundImage.Foreground = Brushes.Yellow;
                    blinkButton(mbtnBackgroundImage, 300, 20.0);
                }

                //}



                //blinkButton(btnGlow, 500, 60000.0);
                UrgentNotificationGlow();
                PollingsGlow();

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }


        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {

                Exception ex = (Exception)e.ExceptionObject;
                string stack = ex.StackTrace;
                string source = ex.Source;
                string message = ex.Message;
                string LF = Environment.NewLine;

                if (stack.IndexOf(Environment.NewLine) > -1)
                {
                    var subtext = stack.Substring(0, stack.IndexOf(Environment.NewLine));
                    Error = $"A crash happen at {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ff", CultureInfo.InvariantCulture)}{LF}" +
                                   $"This is what crashed: {source}{LF}" +
                                   $"This is the error message : {LF}{message}{LF + LF}" +
                                   $"These are the last instructions executed before the crash : {LF}{subtext}{LF + LF}";
                }



                logoutCrashingUser();
                currentUserid = 0;
                currentUserName = "";

                SYSTEM_STATIC.currentUser = null;
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception)
            {

            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.Exception;
                string stack = ex.StackTrace;
                string source = ex.Source;
                string message = ex.Message;
                string LF = Environment.NewLine;

                var subtext = stack.Substring(0, stack.IndexOf(Environment.NewLine));
                Error = $"A crash happen at {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ff", CultureInfo.InvariantCulture)}{LF}" +
                               $"This is what crashed: {source}{LF}" +
                               $"This is the error message : {LF}{message}{LF + LF}" +
                               $"These are the last instructions executed before the crash : {LF}{subtext}{LF + LF}";
                logoutCrashingUser();

                currentUserid = 0;
                currentUserName = "";

                SYSTEM_STATIC.currentUser = null;
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception)
            {

            }
        }


        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.Exception;
                string stack = ex.StackTrace;
                string source = ex.Source;
                string message = ex.Message;
                string LF = Environment.NewLine;

                var subtext = stack.Substring(0, stack.IndexOf(Environment.NewLine));
                Error = $"A crash happen at {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ff", CultureInfo.InvariantCulture)}{LF}" +
                               $"This is what crashed: {source}{LF}" +
                               $"This is the error message : {LF}{message}{LF + LF}" +
                               $"These are the last instructions executed before the crash : {LF}{subtext}{LF + LF}";
                logoutCrashingUser();
   

                currentUserid = 0;
                currentUserName = "";

                SYSTEM_STATIC.currentUser = null;
                System.Windows.Application.Current.Shutdown();
            }
            catch (Exception)
            {


            }
        }

        private void NotificatoinTable_Changed(object sender, RecordChangedEventArgs<Notification> e)
        {


            try
            {

                var change = e.ChangeType;
                if (change == TableDependency.SqlClient.Base.Enums.ChangeType.Insert || change == TableDependency.SqlClient.Base.Enums.ChangeType.Update) //Changetype is an Enum 
                {


                    if (e.Entity.UserId == SYSTEM_STATIC.currentUser.id)
                    {
                        UsersRepo repo = new UsersRepo();
                        var sendingUser = repo.getSender((int)e.Entity.SendingUserId);

                        var notificationManager = new NotificationManager();
                        string messageDescription = null;
                        if (e.Entity.Description.Length > 30)
                            messageDescription = e.Entity.Description.Substring(0, Math.Min(e.Entity.Description.Length, 20));
                        messageDescription = e.Entity.Description;


                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Information",
                            Message = sendingUser.userName + " Tagged you" + "\n" + messageDescription + "...",
                            Type = Notifications.Wpf.NotificationType.Information
                        });
                    }
                    else

                        if (e.Entity.SendingUserId == SYSTEM_STATIC.currentUser.id)
                    {
                        UsersRepo repo = new UsersRepo();
                        var taggedUser = repo.getSender((int)e.Entity.UserId);
                        //var sendingUser = e.Entity.SendingUser.userName;


                        var notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Information",
                            Message = "You have Tagged " + taggedUser.userName + "\nPlease Refresh your notifications",
                            Type = Notifications.Wpf.NotificationType.Success
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public void TaskTimer()
        {
            Timer timer;
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds((5 * 60));

            timer = new System.Threading.Timer((e) =>
            {

                WriteInfoToRegistry();
            }, null, startTimeSpan, periodTimeSpan);

        }

        public void WriteInfoToRegistry()
        {

            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {

                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long ms = (long)(DateTime.UtcNow - epoch).TotalMilliseconds;

                string userInfo = user.id.ToString("00000000") + ms.ToString();

                string[] info = new string[] { userInfo };

                try
                {
                    using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    using (var key = hklm.CreateSubKey(@"SOFTWARE\MicroKosm\ZAS ERP", writable: true))
                    {
                        if (key != null)
                        {
                            key.SetValue("type", userInfo);
                            key.Close();
                        }
                        else
                            key.Close();
                    }
                }
                catch (Exception ex) { }
            }


        }
        //-----------------------Grid Reports Works-------------------------------------Grid Reports Works-------------------------Grid Reports Works
        public void LoadReports()
        {
            GridReportRepo repo = new GridReportRepo();


            //All Reports icons loading

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Procurment Panel") != null)
            {
                //Standard Icons loading

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") == null)
                {
                    barStandardInquiry.IsVisible = false;
                    barMemorizedInquiry.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") == null)
                {
                    barStandardOffer.IsVisible = false;
                    barMemorizedOffer.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") == null)
                {
                    barStandardSaleOrder.IsVisible = false;
                    barMemorizedSaleOrder.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") == null)
                {
                    barStandardPurchaseOrder.IsVisible = false;
                    barMemorizedPurchaseOrder.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") == null)
                {
                    barStandardBill.IsVisible = false;
                    barMemorizedBill.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") == null)
                {
                    barStandardSaleInvoice.IsVisible = false;
                    barMemorizedSaleInvoice.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") == null)
                {
                    barStandardSaleReceipts.IsVisible = false;
                    barMemorizedSaleReceipts.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") == null)
                {
                    barStandardInterBankTransfers.IsVisible = false;
                    barMemorizedInterBankTransfers.IsVisible = false;
                }

                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") == null)
                {
                    barStandardAdminBills.IsVisible = false;
                    barMemorizedAdminBills.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") == null)
                {
                    barStandardPurchaseInvoices.IsVisible = false;
                    barMemorizedPurchaseInvoices.IsVisible = false;
                }
                else

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") == null)
                {
                    barStandardPayments.IsVisible = false;
                    barMemorizedPayments.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") == null)
                {
                    barChartofAccount.IsVisible = false;
                    barMemorizedChartofAccount.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Module") == null)
                {
                    barToDoTask.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Reports") == null)
                {
                    barMemorizedTodoTasks.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") == null)
                {
                    barStandardTargetRewards.IsVisible = false;
                    barMemorizedTargetRewards.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") == null)
                {
                    barStandardTrialBalance.IsVisible = false;
                    barMemorizedTrialBalance.IsVisible = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Loans") == null)
                {
                    barStandardTrialBalance.IsVisible = false;
                    barMemorizedTrialBalance.IsVisible = false;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") == null)
                    {
                        barStandardSTL.IsVisible = false;
                        barMemorizedSTL.IsVisible = false;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Loans and Advances") == null)
                    {
                        barStandardLoanAdvances.IsVisible = false;
                        barMemorizedLoanAdvances.IsVisible = false;
                    }
                }

                try
                {
                    mbtnInquiryReport.Items.Clear();
                    mbtnOfferReport.Items.Clear();
                    mbtnSaleOrderReport.Items.Clear();
                    mbtnMemorizedInquiryReport.Items.Clear();
                    mbtnMemorizedOfferReport.Items.Clear();
                    mbtnMemorizedSaleOrderReport.Items.Clear();
                    mbtnPurchaseOrderReport.Items.Clear();
                    mbtnMemorizedPurchaseOrderReport.Items.Clear();
                    mbtnBillReport.Items.Clear();
                    mbtnMemorizedBillReport.Items.Clear();
                    mbtnMemorizedSaleInvoicesReport.Items.Clear();
                    mbtnSaleInvoicesReport.Items.Clear();
                    mbtnMemorizedSaleReceiptsReport.Items.Clear();
                    mbtnSaleReceiptsReport.Items.Clear();
                    mbtnInterBankTransferReport.Items.Clear();
                    mbtnMemorizedInterBankTransferReport.Items.Clear();
                    mbtnAdminBillReport.Items.Clear();
                    mbtnMemorizedAdminBillsReport.Items.Clear();
                    mbtnMemorizedPurchaseInvoicesReport.Items.Clear();
                    mbtnMemorizedPaymentsReport.Items.Clear();
                    mbtnPurchaseInvoicesReport.Items.Clear();
                    mbtnPaymentsReport.Items.Clear();
                    mbtnChartofAccountReport.Items.Clear();
                    mbtnMemorizedChartofAccountReport.Items.Clear();
                    mbtnTodoReport.Items.Clear();
                    mbtnMemorizedTodoTasksReport.Items.Clear();
                    mbtnMemorizedTargetRewardsReport.Items.Clear();
                    mbtnStandardTargetRewardsReport.Items.Clear();
                    mbtnStandardTargetRewardsReport.Items.Clear();
                    mbtnMemorizedTrialBalanceReport.Items.Clear();
                    mbtnStandardTrialBalanceReport.Items.Clear();
                    mbtnTasksReport.Items.Clear();
                    mbtnMemorizedSTLReport.Items.Clear();
                    mbtnMemorizedTasksReport.Items.Clear();
                    mbtnStandardSTLReport.Items.Clear();
                    mbtnMemorizedLoanAdvancesReport.Items.Clear();
                    mbtnStandardLoanAdvancesReport.Items.Clear();
                    var groups = repo.GetALLReportGroups();


                    foreach (var group in groups)
                    {
                        switch (group.gridReportType)
                        {
                            case GridReportType.StandardReport:
                                {

                                    if (group.parentId == null)
                                        continue;

                                    if (group.parent.groupName == "Inquiries" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();

                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;


                                        barSplitButton.Content = group.groupName;

                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnInquiryReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            barbuttonItem.ItemClick += new ItemClickEventHandler(InquiryReportsClick);
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;




                                            popupMenu.Items.Add(barbuttonItem);





                                        }
                                        barSplitButton.PopupControl = popupMenu;

                                    }
                                    else

                                        if (group.parent.groupName == "Offers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnOfferReport.Items.Add(barSplitButton);

                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        barStandardReports.ItemClickBehaviour = PopupItemClickBehaviour.None;




                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            barbuttonItem.ItemClick += new ItemClickEventHandler(OffersReportsClick);
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            popupMenu.Items.Add(barbuttonItem);


                                            //MenuItem mbtnReport = new MenuItem();

                                            //mbtnReport.Header = report.reportName;
                                            //Image reportImage = new Image();
                                            //reportImage.Source = new BitmapImage(new Uri("/ZAS_ERP;component/images/Report_16x16.png", UriKind.RelativeOrAbsolute));
                                            //mbtnReport.Icon = reportImage;
                                            //barbuttonItem.ItemClick += new RoutedEventHandler(OffersReportsClick);

                                            //mbtnfferGroup.Items.Add(mbtnReport);
                                        }
                                        barSplitButton.PopupControl = popupMenu;

                                    }
                                    else

                                        if (group.parent.groupName == "Sale Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnSaleOrderReport.Items.Add(barSplitButton);
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();


                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;




                                            if (report.settingkey == "Sale Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderReportsClick); }

                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnBillReport.Items.Add(barSplitButton);
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;

                                            if (report.settingkey == "Bill Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(BillRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(BillsClick); }

                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Purchase Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnPurchaseOrderReport.Items.Add(barSplitButton);
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();


                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;

                                            if (report.settingkey == "Purchase Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(PORegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(POClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Sale Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnSaleInvoicesReport.Items.Add(barSplitButton);
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            barbuttonItem.ItemClick += new ItemClickEventHandler(SaleInvoicesReportsClick);
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                    if (group.parent.groupName == "Sale Receipts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnSaleReceiptsReport.Items.Add(barSplitButton);
                                        barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;

                                            if (report.settingkey == "Sale Receipt Register")
                                            {
                                                barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptRegisterReportsClick);
                                            }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Inter-Bank Transfers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnInterBankTransferReport.Items.Add(barSplitButton);

                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {

                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Inter-Bank Transfer Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(InterBankTransferRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(InterBankTransferClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Admin Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnAdminBillReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Admin Bill Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(AdminBillRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(AdminBillClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Purchase Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnPurchaseInvoicesReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Purchase Invoice Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(PurchaseInvoiceRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(PurchaseInvoiceClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Payments" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnPaymentsReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Payment Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else

                                        if (group.parent.groupName == "Chart of Accounts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnChartofAccountReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Chart of Accounts Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(ChartofAccountRegisterReportsClick); }
                                            else
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(ChartofAccountClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else
                                    if (group.parent.groupName == "Targets" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Reports") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnTodoReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            if (report.settingkey == "Step Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(TodoTasksRegisterReportsClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else
                                    if (group.parent.groupName == "Target Rewards" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnStandardTargetRewardsReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            //if (report.settingkey == "Step Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(TargetRwewardRegisterReportsClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else
                                    if (group.parent.groupName == "Tasks" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnTasksReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            //if (report.settingkey == "Step Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(TasksRegisterReportsClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    else
                                    if (group.parent.groupName == "Trial Balance" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";

                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnStandardTrialBalanceReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            //if (report.settingkey == "Step Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(TrialBalanceReportsClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }

                                    else
                                    if (group.parent.groupName == "STL" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
                                    {

                                        var reports = repo.GetALLReportsbyGroupId(group.Id);

                                        DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                        barSplitButton.Content = group.groupName;
                                        var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";

                                        BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                        barSplitButton.Glyph = bitmap;
                                        mbtnStandardSTLReport.Items.Add(barSplitButton);
                                        DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                        foreach (var report in reports)
                                        {
                                            DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                            barbuttonItem.Content = report.reportName;
                                            var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                            BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                            barbuttonItem.Glyph = bitmapReport;
                                            //if (report.settingkey == "Step Register")
                                            { barbuttonItem.ItemClick += new ItemClickEventHandler(STLReportsClick); }
                                            popupMenu.Items.Add(barbuttonItem);
                                        }
                                        barSplitButton.PopupControl = popupMenu;
                                    }
                                    break;
                                }


                            case GridReportType.MemorizedReport:
                                {
                                    if (group.parentId == null)
                                        continue;
                                    if (group.userId == SystemLog.CurrentUserId)
                                    {

                                        if (group.parent.groupName == "Inquiries" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;


                                            mbtnMemorizedInquiryReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();


                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    barbuttonItem.ItemClick += new ItemClickEventHandler(InquiryReportsClick);
                                                    popupMenu.Items.Add(barbuttonItem);

                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Offers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedOfferReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    barbuttonItem.ItemClick += new ItemClickEventHandler(OffersReportsClick);
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Sale Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedSaleOrderReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Sale Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderReportsClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedBillReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Bill Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(BillRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(BillsClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Purchase Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedPurchaseOrderReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Purchase Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(PORegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(POClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Sale Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedSaleInvoicesReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {
                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    barbuttonItem.ItemClick += new ItemClickEventHandler(SaleInvoicesReportsClick);
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Sale Receipts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedSaleReceiptsReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {
                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Sale Receipt Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptClick); }
                                                    popupMenu.Items.Add(barbuttonItem);


                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }
                                        }
                                        else
                                            if (group.parent.groupName == "Inter-Bank Transfers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedInterBankTransferReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Inter-Bank Transfer Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(InterBankTransferRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(InterBankTransferClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;
                                            }

                                        }

                                        else
                                            if (group.parent.groupName == "Admin Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedAdminBillsReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Admin Bill Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(AdminBillRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(AdminBillClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }

                                        }
                                        else
                                            if (group.parent.groupName == "Purchase Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedPurchaseInvoicesReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Purchase Invoice Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(PurchaseInvoiceRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(PurchaseInvoiceClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }

                                        }
                                        else
                                            if (group.parent.groupName == "Payments" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedPaymentsReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Payment Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentRegisterReportsClick); }
                                                    else
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }

                                        }
                                        else
                                    if (group.parent.groupName == "Targets" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Reports") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedTodoTasksReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    if (report.settingkey == "Step Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(TodoTasksRegisterReportsClick); }

                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }

                                        }
                                        else
                                    if (group.parent.groupName == "Target Rewards" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedTargetRewardsReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    //if (report.settingkey == "Step Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(TargetRwewardRegisterReportsClick); }

                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }
                                        }
                                        else
                                    if (group.parent.groupName == "Tasks" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedTasksReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    //if (report.settingkey == "Step Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(TasksRegisterReportsClick); }

                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }
                                        }
                                        else

                                    if (group.parent.groupName == "Trial Balance" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedTrialBalanceReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    //if (report.settingkey == "Step Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(TrialBalanceReportsClick); }

                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }
                                        }
                                        else
                                        if (group.parent.groupName == "Chart of Accounts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedChartofAccountReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    //if (report.settingkey == "Payment Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(ChartofAccountClick); }
                                                    //else
                                                    //{ barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }
                                        }
                                        else
                                        if (group.parent.groupName == "STL" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);

                                            DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                                            barSplitButton.Content = group.groupName;
                                            var imgUri = "/ZAS_ERP;component/images/UserGroup_32x32.png";
                                            BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                                            barSplitButton.Glyph = bitmap;
                                            mbtnMemorizedSTLReport.Items.Add(barSplitButton);
                                            DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                                            foreach (var report in reports)
                                            {
                                                if (report.userId == SystemLog.CurrentUserId)
                                                {

                                                    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                                                    barbuttonItem.Content = report.reportName;
                                                    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                                                    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                                                    barbuttonItem.Glyph = bitmapReport;
                                                    //if (report.settingkey == "Payment Register")
                                                    { barbuttonItem.ItemClick += new ItemClickEventHandler(STLReportsClick); }
                                                    //else
                                                    //{ barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentClick); }
                                                    popupMenu.Items.Add(barbuttonItem);
                                                }
                                                barSplitButton.PopupControl = popupMenu;


                                            }
                                        }
                                        break;

                                    }
                                    break;
                                }
                        }
                    }
                }

                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            else
            {
                mbtnInquiryReport.Visibility = Visibility.Collapsed;
                mbtnOfferReport.Visibility = Visibility.Collapsed;
                mbtnSaleOrderReport.Visibility = Visibility.Collapsed;
                mbtnPurchaseOrderReport.Visibility = Visibility.Collapsed;
                mbtnBillReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedInquiryReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedOfferReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedSaleOrderReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedPurchaseOrderReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedBillReport.Visibility = Visibility.Collapsed;
                mbtnSaleInvoicesReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedSaleInvoicesReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedPurchaseInvoicesReport.Visibility = Visibility.Collapsed;
                mbtnPurchaseInvoicesReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedPaymentsReport.Visibility = Visibility.Collapsed;
                mbtnPaymentsReport.Visibility = Visibility.Collapsed;
                mbtnStandardTargetRewardsReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedTargetRewardsReport.Visibility = Visibility.Collapsed;
                mbtnStandardTrialBalanceReport.Visibility = Visibility.Collapsed;
                mbtnMemorizedTrialBalanceReport.Visibility = Visibility.Collapsed;

            }
        }
        private void LoadSharedReports()
        {
            GridReportRepo repo = new GridReportRepo();
            mbtnSharedInquiryReport.Items.Clear();
            mbtnSharedOfferReport.Items.Clear();
            mbtnSharedSaleOrderReport.Items.Clear();
            mbtnSharedBillReport.Items.Clear();
            mbtnSharedPurchaseOrderReport.Items.Clear();
            mbtnSharedSaleInvoicesReport.Items.Clear();
            mbtnSharedSaleReceiptsReport.Items.Clear();
            mbtnSharedInterBankTransferReport.Items.Clear();
            mbtnSharedAdminBillReport.Items.Clear();
            mbtnSharedPurchaseInvoicesReport.Items.Clear();
            mbtnSharedPaymentsReport.Items.Clear();
            mbtnSharedChartofAccountReport.Items.Clear();
            var allSharedGroups = repo.GetAllSharedGroups();
            allSharedGroups = allSharedGroups.Where(x => x.isVoid != true && x.parentId != null).ToList();
            groupsWithCompany.Clear();
            groupsWithDepartment.Clear();
            groupsWithEmployee.Clear();
            foreach (var group in allSharedGroups)
            {
                if (group.Companies.Count != 0)
                {
                    foreach (var company in group.Companies)
                    {
                        if (compIds.Contains(company.Id))
                        {
                            groupsWithCompany.Add(group);
                            break;
                        }
                    }
                }
                else
                {
                    groupsWithCompany.Add(group);
                    groupsWithCompany = groupsWithCompany.Distinct().ToList();
                }
            }
            foreach (var group in groupsWithCompany)
            {
                if (group.Departments.Count != 0)
                {
                    foreach (var department in group.Departments)
                    {
                        if (deptIds.Contains(department.Id))
                        {
                            groupsWithDepartment.Add(group);
                            break;
                        }
                    }
                }
                else
                {
                    groupsWithDepartment.Add(group);
                    groupsWithDepartment = groupsWithDepartment.Distinct().ToList();

                }
            }
            foreach (var group in groupsWithDepartment)
            {
                if (group.Employees.Count != 0)
                {
                    empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                    foreach (var department in group.Departments)
                    {
                        if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                        {
                            groupsWithEmployee.Add(group);
                            break;
                        }
                    }
                }
                else
                {
                    groupsWithEmployee.Add(group);
                    groupsWithEmployee.Distinct().ToList();

                }
            }
            foreach (var group in groupsWithEmployee)
            {
                if (group.parentId == null)
                    continue;
                if (group.Parent.groupName == "Inquiries" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                {
                    //var reports = repo.GetALLSharedReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedInquiryReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;

                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(InquiryReportsClick);
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Offers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                {
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedOfferReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    foreach (var report in group.SharedReports)
                    {

                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(OffersSharedReportsClick);
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Sale Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                {
                    //var reports = repo.GetALLReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedSaleOrderReport.Items.Add(barSplitButton);
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {

                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;

                        if (report.settingkey == "Sale Register")
                        { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderSharedRegisterReportsClick); }
                        else
                        { barbuttonItem.ItemClick += new ItemClickEventHandler(SaleOrderSharedRegisterReportsClick); }

                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
                {

                    //var reports = repo.GetALLReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedBillReport.Items.Add(barSplitButton);
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {

                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(BillSharedReportClick);
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Purchase Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                {
                    //var reports = repo.GetALLReportsbyGroupId(group.Id);

                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedPurchaseOrderReport.Items.Add(barSplitButton);
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(POSharedReportsClick);
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Sale Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                {
                    //var reports = repo.GetALLReportsbyGroupId(group.Id);

                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedSaleInvoicesReport.Items.Add(barSplitButton);
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {

                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(SaleInvoicesSharedReportClick);
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Sale Receipts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
                {

                    var reports = repo.GetALLReportsbyGroupId(group.Id);

                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedSaleReceiptsReport.Items.Add(barSplitButton);
                    barSplitButton.ItemClickBehaviour = PopupItemClickBehaviour.None;
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    //foreach (var report in reports)
                    //{

                    //    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                    //    barbuttonItem.Content = report.reportName;
                    //    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                    //    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                    //    barbuttonItem.Glyph = bitmapReport;

                    //    if (report.settingkey == "Sale Receipt Register")
                    //    {
                    //        barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptRegisterReportsClick);
                    //    }
                    //    else
                    //    { barbuttonItem.ItemClick += new ItemClickEventHandler(ReceiptClick); }
                    //    popupMenu.Items.Add(barbuttonItem);
                    //}
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Inter-Bank Transfers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
                {

                    //var reports = repo.GetALLReportsbyGroupId(group.Id);

                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedInterBankTransferReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                    foreach (var report in group.SharedReports)
                    {

                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;

                        barbuttonItem.ItemClick += new ItemClickEventHandler(InterBankTransferSharedReportClick);

                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Admin Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
                {
                    //var reports = repo.GetALLReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedAdminBillReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(AdminBillSharedReportsClick);
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Purchase Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                {
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedPurchaseInvoicesReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();
                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        barbuttonItem.ItemClick += new ItemClickEventHandler(PurchaseInvoiceSharedReportClick);
                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Payments" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
                {

                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedPaymentsReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();


                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        if (report.settingkey == "Payment Register")
                        { barbuttonItem.ItemClick += new ItemClickEventHandler(PaymentRegisterSharedReportsClick); }

                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Chart of Accounts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
                {

                    var reports = repo.GetALLReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedChartofAccountReport.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();

                    //foreach (var report in reports)
                    //{
                    //    DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                    //    barbuttonItem.Content = report.reportName;
                    //    var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                    //    BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                    //    barbuttonItem.Glyph = bitmapReport;
                    //    if (report.settingkey == "Chart of Accounts Register")
                    //    { barbuttonItem.ItemClick += new ItemClickEventHandler(ChartofAccountRegisterReportsClick); }
                    //    else
                    //    { barbuttonItem.ItemClick += new ItemClickEventHandler(ChartofAccountClick); }
                    //    popupMenu.Items.Add(barbuttonItem);
                    //}
                    barSplitButton.PopupControl = popupMenu;
                }
                else
            if (group.Parent.groupName == "Targets" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Reports") != null)
                {

                    var reports = repo.GetALLReportsbyGroupId(group.Id);
                    DevExpress.Xpf.Bars.BarSplitButtonItem barSplitButton = new DevExpress.Xpf.Bars.BarSplitButtonItem();
                    barSplitButton.Content = group.groupName;
                    var imgUri = "/ZAS_ERP;component/images/Team_32x32.png";
                    BitmapImage bitmap = new BitmapImage(new Uri(imgUri, UriKind.Relative));
                    barSplitButton.Glyph = bitmap;
                    mbtnSharedTodoTasksReports.Items.Add(barSplitButton);
                    DevExpress.Xpf.Bars.PopupMenu popupMenu = new DevExpress.Xpf.Bars.PopupMenu();


                    foreach (var report in group.SharedReports)
                    {
                        DevExpress.Xpf.Bars.BarButtonItem barbuttonItem = new DevExpress.Xpf.Bars.BarButtonItem();
                        barbuttonItem.Content = report.reportName;
                        var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                        BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));
                        barbuttonItem.Glyph = bitmapReport;
                        if (report.settingkey == "Step Register")
                        { barbuttonItem.ItemClick += new ItemClickEventHandler(TodoTasksRegisterSharedReportsClick); }

                        popupMenu.Items.Add(barbuttonItem);
                    }
                    barSplitButton.PopupControl = popupMenu;
                }
            }
        }

        //admin bill click start
        private void AdminBillRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void AdminBillSharedReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucAdminBIllSharedReport reportView = new ucAdminBIllSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PurchaseInvoiceRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucPIView reportView = new ucPIView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPIView reportView = new ucPIView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void PurchaseInvoiceSharedReportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPISharedReport reportView = new ucPISharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void PaymentRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void PaymentRegisterSharedReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPaymentSharedReport reportView = new ucPaymentSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);     
            }

        }
        private void TodoTasksRegisterSharedReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucTodoTaskSharedReport reportView = new ucTodoTaskSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void ChartofAccountRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void TodoTasksRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucTodoTaskReport reportView = new ucTodoTaskReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucTodoTaskReport reportView = new ucTodoTaskReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
        }
        private void TargetRwewardRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                winTargetRegisterReport reportView = new winTargetRegisterReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                winTargetRegisterReport reportView = new winTargetRegisterReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
        }
        private void TasksRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucTaskGridView reportView = new ucTaskGridView(report, report.reportName);
                reportView.Show();
            }
            else
            {
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucTaskGridView reportView = new ucTaskGridView(report, report.reportName);
                reportView.Show();
            }
        }
        private void TrialBalanceReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                winTrialBalanceReport reportView = new winTrialBalanceReport(report, report.reportName);
                reportView.Show();
            }
            else
            {
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                winTrialBalanceReport reportView = new winTrialBalanceReport(report, report.reportName);
                reportView.Show();
            }
        }
        private void STLReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                winSTLGridView reportView = new winSTLGridView(report, report.reportName);
                reportView.Show();
            }
            else
            {
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                winSTLGridView reportView = new winSTLGridView(report, report.reportName);
                reportView.Show();
            }
        }


        private void AdminBillClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        //admin bill click end

        private void PurchaseInvoiceClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucPIView reportView = new ucPIView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPIView reportView = new ucPIView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void PaymentClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        //Report Clicks
        private void ChartofAccountClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void SaleInvoicesReportsClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucSaleInvoiceView reportView = new ucSaleInvoiceView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleInvoiceView reportView = new ucSaleInvoiceView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
        }
        private void SaleInvoicesSharedReportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleInvoiceSharedReport reportView = new ucSaleInvoiceSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void OffersReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            string title = (sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucOffersReportView reportView = new ucOffersReportView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucOffersReportView reportView = new ucOffersReportView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void OffersSharedReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            SharedReport report = new SharedReport();
            string title = (sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString();
            report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
            if (report != null)
            {
                ucOffersSharedReport reportView = new ucOffersSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
        }
        private void BillRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            string title = (sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString();
            //report = repo.GetReportByName((sender as MenuItem).Header.ToString());
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucBillsView reportView = new ucBillsView(report, title);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucBillsView reportView = new ucBillsView(report, title);
                reportView.myParent = this;
                reportView.Show();
            }


        }
        private void BillSharedReportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucBillSharedReport reportView = new ucBillSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void PORegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucPurchaseOrderView reportView = new ucPurchaseOrderView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPurchaseOrderView reportView = new ucPurchaseOrderView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void POSharedReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPurchaseOrderSharedReport reportView = new ucPurchaseOrderSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
        private void ReceiptRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void InterBankTransferRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            GridReport report = new GridReport();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void InterBankTransferClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void InterBankTransferSharedReportClick(object sender, RoutedEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            SharedReport report = new SharedReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
            if (report != null)
            {
                ucIBTSharedReport reportView = new ucIBTSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }

        private void SaleOrderRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GridReport report = new GridReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                if (report != null)
                {

                    ucSaleOrderReportView reportView = new ucSaleOrderReportView(report, report.reportName);
                    reportView.myParent = this;
                    reportView.Show();
                }
                else
                {
                    report = null;
                    report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                    ucSaleOrderReportView reportView = new ucSaleOrderReportView(report, report.reportName);
                    reportView.myParent = this;
                    reportView.Show();
                }
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void SaleOrderReportsClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            SaleOrderRepo soRepo = new SaleOrderRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null && report.gridReportType == GridReportType.MemorizedReport)
            {
                ucSaleOrderReportView reportView = new ucSaleOrderReportView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleOrderReportView reportView = new ucSaleOrderReportView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }


        }
        private void SaleOrderSharedRegisterReportsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                SharedReport report = new SharedReport();
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleOrderSharedReport reportView = new ucSaleOrderSharedReport(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        private void InquiryReportsClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            GridReport report = new GridReport();
            SharedReport sharedReport = new SharedReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            sharedReport = repo.GetSharedReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
            if (report != null)
            {
                ucInquiryView reportView = new ucInquiryView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            if (sharedReport != null)
            {

                ucInquirySharedReport reportView = new ucInquirySharedReport(sharedReport, sharedReport.reportName);
                reportView.myParent = this;
                reportView.Show();
                //report = null;
                //report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                //ucInquiryView reportView = new ucInquiryView(report, report.reportName);
                //reportView.myParent = this;
                //reportView.Show();
            }

            //TextBlock text = new TextBlock();
            //// Create a printing link. 
            //ReportHeader = (sender as MenuItem).Header.ToString();
            //DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //PrintableControlLink link = new PrintableControlLink((TableView)gridControl.View);
            //link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            //link.PageHeaderData = (sender as MenuItem).Header.ToString();
            //link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            //link.DocumentName = (sender as MenuItem).Header.ToString();
            //link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            //link.ReportHeaderTemplate = dataTemplate;
            //link.Landscape = true;
            //// Show a preview. 
            //PrintHelper.ShowRibbonPrintPreview(this, link);
        }


        private void BillsClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            BillRepo billRepo = new BillRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                ucBillsView reportView = new ucBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucBillsView reportView = new ucBillsView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }

        private void POClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucPurchaseOrderView reportView = new ucPurchaseOrderView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucPurchaseOrderView reportView = new ucPurchaseOrderView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }
        private void ReceiptClick(object sender, RoutedEventArgs e)
        {
            GridReport report = new GridReport();
            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
            if (report != null)
            {
                report = repo.GetReportByNameAndUserId((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString(), currentUserid);
                ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }
            else
            {
                report = null;
                report = repo.GetReportByName((sender as DevExpress.Xpf.Bars.BarButtonItem).Content.ToString());
                ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                reportView.myParent = this;
                reportView.Show();
            }

        }

        public void enableMenu()
        {

            int tryCount = 0;

            if (!SYSTEM_STATIC.LoggedInByPowerUser)
            {


                recheck:
                try
                {

                    if (SYSTEM_STATIC.isLoadingPermissions)
                    {
                        System.Threading.Thread.Sleep(2000);
                        goto recheck;
                    }
                    //tryCount++;
                    //if (tryCount > 10)
                    //{
                    //    MessageBox.Show("Unable to load permissions");
                    //    System.Windows.Forms.Application.Exit();
                    //}
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Upload Background Image") == null)
                    //{ desktopBackground.IsEnabled = false; }
                    //MessageBox.Show("Is SystemLogic Exist ? " + (SystemLogic == null ? true : false));
                    //System.Threading.Thread.Sleep(500);

                    rpageInquiry.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Inquiries") != null) ? true : false;
                    rpageOffer.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Offers") != null) ? true : false;



                    btnRentals.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Rentals") != null) ? true : false;
                    ribbonHome.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Upload Background Image") != null) ? true : false;
                    desktopBackground.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Upload Background Image") != null) ? true : false;
                    //Checking for permission and enabling respective Menu Items
                    //System.Threading.Thread.Sleep(500);

                    mheadCompany.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Center") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadEmployee.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Employee Center") != null) ? true : false;
                    //mheadCustomer.IsVisible = (SystemLogic.AllowedPermissions.Find(x => x.Name == "Customer Center") != null) ? true : false;



                    // mheadList.IsEnabled = (SystemLogic.AllowedPermissions.Find(x => x.Name == "Lists") != null) ? true : false;
                    // mheadVendor.IsEnabled = (SystemLogic.AllowedPermissions.Find(x => x.Name == "Vendor Center") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadPrincipal.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Principal Center") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadAccountant.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null) ? true : false;
                    // mheadReports.IsEnabled = (SystemLogic.AllowedPermissions.Find(x => x.Name == "Report Center") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mbtnCoshSheetFields.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Cost Sheet Settings") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    barFixedAssets.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadList.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Lists") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadVendor.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Vendor") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    btnBanking.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Banking") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mheadCustomer.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Procurment Panel") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    //btnTargets.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);


                    mheadReports.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reports") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    barStandardReports.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Standard Reports") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mbtnReportGroupList.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Report Group") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    mbtnMemorizedReport.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memorized Reports") != null) ? true : false;
                    //System.Threading.Thread.Sleep(500);

                    //mheadLoans.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Loans") != null) ? true : false;

                    mheadToDoTask.IsVisible = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Module") != null) ? true : false;
                    barSharedReports.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Shared Reports") != null) ? true : false;

                    mbtnPollings.Visibility = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Polling") != null) ? Visibility.Visible : Visibility.Collapsed;

                    barPettyCash.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Petty Cash") != null) ? true : false;
                    mHeadCashBook.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Petty Cash") != null) ? true : false;
                    barVATBook.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "VAT Book") != null) ? true : false;
                    mHeadVATBook.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "VAT Book") != null) ? true : false;

                    mbtnEmploymeesDataRetrievalRegister.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Access Data Retrieval Dates Structure") != null) ? true : false;

                    if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Advances") != null)
                    {
                        barLoansAdvances.IsEnabled = true;
                        mbtnEnterLoansAdvance.IsEnabled = true;
                        mbtnLoansAdvanceRegister.IsEnabled = true;
                    }
                    else
                    {
                        barLoansAdvances.IsEnabled = false;
                        mbtnEnterLoansAdvance.IsEnabled = false;
                        mbtnLoansAdvanceRegister.IsEnabled = false;
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Company Loans") != null)
                    {
                        barLoans.IsEnabled = true;
                        mbtnCompanyEnterLoan.IsEnabled = true;
                        mbtnCompanyLoansRegister.IsEnabled = true;
                    }
                    else
                    {
                        barLoans.IsEnabled = false;
                        mbtnCompanyEnterLoan.IsEnabled = false;
                        mbtnCompanyLoansRegister.IsEnabled = false;
                    }

                    //mHeadPayroll.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Payroll") != null) ? true : false;
                    mHeadEmploymentSalary.IsVisible = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Employment Salary") != null) ? true : false;
                    barTaskss.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Tasks") != null) ? true : false;
                    mHeadTraveling.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Traveling Record") != null) ? true : false;

                    btnPRITregister.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Can View PRIT Register") != null) ? true : false;

                    btnRentals.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Rentals") != null) ? true : false;


                    btnRentalContract.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Rental Contracts") != null) ? true : false;
                    btnRentalOrder.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Rental Orders") != null) ? true : false;
                    btnRentalInvoice.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Rental Invoices") != null) ? true : false;
                    btnAssets.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Assets") != null) ? true : false;
                    btnTenants.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Tenants") != null) ? true : false;

                    mbtnEnterRentalContract.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Add Rental Contracts") != null) ? true : false;
                    mbtnEnterRentalOrder.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Add Rental Orders") != null) ? true : false;
                    mbtnEnterRentalInvoice.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Add Rental Invoices") != null) ? true : false;
                    mbtnAsset.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Add New Assets") != null) ? true : false;
                    mbtnTenants.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Add New Tenants") != null) ? true : false;

                    barMemos.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Memos") != null) ? true : false;

                    btnFile.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "File") != null) ? true : false;

                    mHeadDocuments.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Documents") != null) ? true : false;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Excaption in permission \n" + ex.Message);
                    System.Threading.Thread.Sleep(10000);
                    //MessageBox.Show("Goto after sleep of 10 sec");
                    goto recheck;
                }
            }
            else
            {
                //btnProfile.Visibility = Visibility.Collapsed;


            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Marketing Fields Settings") != null)
            {
                mbtnSummarySheetFields.IsEnabled = true;
            }
            else
            {
                mbtnSummarySheetFields.IsEnabled = false;


            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Cost Sheet Settings") != null)
            {
                mbtnCoshSheetFields.IsEnabled = true;

            }
            else
            {
                mbtnCoshSheetFields.IsEnabled = false;

            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            //{
            //    mbtnfixedassets.IsEnabled = true;

            //}
            //else
            //{
            //    mbtnfixedassets.IsEnabled = false;

            //}


            //Permission to access Bank Lists
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bank Lists") != null)
            {
                mheadBankAccountList.IsEnabled = true;
            }
            else
            {
                mheadBankAccountList.IsEnabled = false;
            }

            //Permission to Add Sale Receipt
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt") != null)
            {
                mbtnEnterReceipt.IsEnabled = true;
            }
            else
            {
                mbtnEnterReceipt.IsEnabled = false;
            }

            //Permission to access List of Sale Receipts
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
            {
                mbtnReceiptRegister.IsEnabled = true;
            }
            else
            {
                mbtnReceiptRegister.IsEnabled = false;
            }


            //Permission to add new Bank Transfer
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                mbtnInterBankTransfer.IsEnabled = true;
            }
            else
            {
                mbtnInterBankTransfer.IsEnabled = false;
            }

            //Permission to access List of Bank Tranfers
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                mbtnBankTransferRegister.IsEnabled = true;
                mbtnBankTransferRegister.IsEnabled = true;
            }
            else
            {
                mbtnBankTransferRegister.IsEnabled = false;
            }


            //Permission to open Fixed Asset Adjustment
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Fixed Assets Adjustment") != null)
            //{
            //    mbtnFaAdjustment.IsEnabled = true;

            //}
            //else
            //{
            //    mbtnFaAdjustment.IsEnabled = false;

            //}
            //Permissions To View Online USers
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Online Utility") != null)
            {
                btnOnline.IsEnabled = true;

            }
            else
            {
                btnOnline.IsEnabled = false;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "HRM") != null)
            {
                mheadEmployee.IsVisible = true;
            }
            else
            {
                mheadEmployee.IsVisible = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access Employment Centre") != null)
            {
                mnuemployeeadd.IsEnabled = true;
            }
            else
            {
                mnuemployeeadd.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Leave Register") != null)
            {
                mnuemployeeReg.IsEnabled = true;
            }
            else
            {
                mnuemployeeReg.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access Employee Leaves") != null)
            {
                mbtnLeavesRegister.IsEnabled = true;
            }
            else
            {
                mbtnLeavesRegister.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Profile") != null)
            {
                btnProfile.IsEnabled = true;
            }
            else
            {

            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") == null)
            {

                barAdminBill.IsVisible = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") == null)
            {
                barVendorBill.IsVisible = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") == null)
            {
                barPI.IsVisible = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") == null)
            {
                barSI.IsVisible = false;
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") == null)
            {
                barPO.IsVisible = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") == null)
            {
                barSO.IsVisible = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") == null)
            {
                barOffer.IsVisible = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") == null)
            {
                barInquiries.IsVisible = false;
            }




        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {


            if (DXMessageBox.Show("Do You wish To Logout. Click Yes to Continue, or No to Cancel", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes, MessageBoxOptions.DefaultDesktopOnly) == MessageBoxResult.Yes)
            {
                //logoutUser();
                //foreach (Window window in mainWindow.OwnedWindows)
                //    window.Close();

                start.frmDbConnectt frmdb = new start.frmDbConnectt(/*null*/); //FIXIT
                frmdb.Show();
                foreach (Window window in Application.Current.Windows)
                    if (window.Name != "mainWindow" && window.Name != "winfrmDbConnect")
                    {
                        window.Close();
                    }
                mainWindow.Close();
            }
        }

        public async void logoutUser()
        {
            UsersRepo userRepo = new UsersRepo();

            var loginUser = SYSTEM_STATIC.currentUser;
            if (loginUser != null)
            {
                //loginUser.isLoggedIn = false;
                try
                {
                    // userRepo.updateuser(loginUser);
                    //LoginUserDetails userDetails = new LoginUserDetails();
                    //userDetails.LogoutTime = DateTime.Now;
                    //userDetails.userId = loginUser.id;
                    userRepo.updateLogoutUser(loginUser.id);
                    unicodeConnected = "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044";
                    currentOnlieUserId = SYSTEM_STATIC.currentUser.id;
                    await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in LoggingOut user " + ex.Message);
                }

            }
        }

        public async void logoutCrashingUser()
        {
            UsersRepo userRepo = new UsersRepo();

            var loginUser = SYSTEM_STATIC.currentUser;
            if (loginUser != null)
            {
                //loginUser.isLoggedIn = false;
                try
                {
                    // userRepo.updateuser(loginUser);
                    //LoginUserDetails userDetails = new LoginUserDetails();
                    //userDetails.LogoutTime = DateTime.Now;
                    //userDetails.userId = loginUser.id;

                    userRepo.updateCrashingLogoutUser(loginUser.id, Error);
                    unicodeConnected = "\u0044\u0049\u0053\u0043\u004f\u004e\u004e\u0045\u0043\u0054\u0045\u0044";
                    currentOnlieUserId = SYSTEM_STATIC.currentUser.id;
                    await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in LoggingOut user " + ex.Message);
                }

            }
        }

        public async void UrgentNotificationGlow()
        {
            //loginUser.isLoggedIn = false;
            try
            {
                unicodeConnected = "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e";
                await connection.InvokeAsync("SendMessage", unicodeConnected, SYSTEM_STATIC.currentUser.id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Urgent Notification Pending " + ex.Message);
            }
        }

        public async void PollingsGlow()
        {
            //loginUser.isLoggedIn = false;
            try
            {
                unicodeConnected = "\u0055\u0052\u0047\u0045\u004e\u0054\u004e\u004f";
                await connection.InvokeAsync("SendMessage", unicodeConnected, SYSTEM_STATIC.currentUser.id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Urgent Notification Pending " + ex.Message);
            }
        }

        private void mbtnInquiryCenter_Click(object sender, RoutedEventArgs e)
        {
            //Inquiriess.frmInquiryCenter inquirycenter = new Inquiriess.frmInquiryCenter();
            //inquirycenter.Show();
        }






        private void mbtnStatusPanel_Click(object sender, RoutedEventArgs e)
        {
            //DocumentPanel panel = new DocumentPanel();
            //panel.Content = new Uri(@"BussinessLogicss/frmStatusPanel.xaml", UriKind.Relative);
            //panel.Caption = "Procurment Statuses List";

            //panel.AllowDock = true;
            //mdimaincontainer.Add(panel);
            //BussinessLogicss.frmStatusPanel statusPanel = new BussinessLogicss.frmStatusPanel();
            //statusPanel.Owner = this;
            //statusPanel.Show();

            //documentLayoutManager1.DockController.Activate(panel);
        }





        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if(inactivityTracker.logoutFromInactivity==true && inactivityTracker.winLoaded == false)
            {
                e.Cancel = true;
                ZAS_ERP.start.winInactivityAuthentication winInactivityAuthentication = new ZAS_ERP.start.winInactivityAuthentication(inactivityTracker);
                winInactivityAuthentication.ShowDialog();
            }
            else
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close ZAS-ERP ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == MessageBoxResult.Yes)
                {

                    logoutUser();
                    currentUserid = 0;
                    currentUserName = "";


                    SYSTEM_STATIC.currentUser = null;
                    System.Windows.Application.Current.Shutdown();

                    e.Cancel = false;
                }
                else if (inputfromUser == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }













        private void MbtnInquiryReport_Click(object sender, RoutedEventArgs e)
        {
            InquiryRepo Repo = new InquiryRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = Repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnSaleOrderReport_Click(object sender, RoutedEventArgs e)
        {
            SaleOrderRepo Repo = new SaleOrderRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = Repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnOfferReport_Click(object sender, RoutedEventArgs e)
        {
            OfferRepo Repo = new OfferRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = Repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }
        private void MbtnInquiryRepor_Click(object sender, RoutedEventArgs e)
        {
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportInquirySingle(new Inquiry()));
            reportPanel.Show();
        }

        private void MbtnSaleOrderRepor_Click(object sender, RoutedEventArgs e)
        {
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportOfferSingle(new Offer()));
            reportPanel.Show();
        }

        private void MbtnOfferRepor_Click(object sender, RoutedEventArgs e)
        {
            //Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportSaleOrderSingle(new SaleOrder()));
            //reportPanel.Show();
        }
        private void MbtnReportCenter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnReportDepartment_Click(object sender, RoutedEventArgs e)
        {
            DepartmentRepo Repo = new DepartmentRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = Repo.GetDepartments();

            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnReportPrincipal_Click(object sender, RoutedEventArgs e)
        {
            PrincipalRepo repo = new PrincipalRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnReportVendor_Click(object sender, RoutedEventArgs e)
        {
            VendorRepo repo = new VendorRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnReportEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeRepo repo = new EmployeeRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = repo.GetActiveEmployees();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }

        private void MbtnReportCompany_Click(object sender, RoutedEventArgs e)
        {
            CompanyRepo repo = new CompanyRepo();
            XtraReport xtrareport = new XtraReport();
            xtrareport.DataSource = repo.getAll();
            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(xtrareport);
            reportPanel.Show();
        }





        private void MbtnDesignationadd_Click(object sender, RoutedEventArgs e)
        {
            Employeess.frmEmployeesList designationsList = new Employeess.frmEmployeesList();
            designationsList.ShowDialog();
        }
        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            Userss.frmUserProfile userProfile = new Userss.frmUserProfile();
            userProfile.ShowDialog();
        }
        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            Userss.frmChangePassword frmChangePassword = new Userss.frmChangePassword();
            frmChangePassword.ShowDialog();
        }

        private void LoadTransactionGrid(string TransactionName)
        {






            TransactionItemType transactionItemType = TransactionItemType.UnDefined;
            DataType dataType = DataType.All;
            switch (TransactionName)
            {
                case "Inquiries":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.All;

                        break;
                    }
                case "Inquiries(Open)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.Open;
                        break;
                    }




                case "Inquiries(Close)":
                    {
                        transactionItemType = TransactionItemType.Inquiry;
                        dataType = DataType.Closed;
                        break;
                    }
                case "Offers":
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.All;
                    break;
                case "Offers(Open)":
                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.Open;

                    break;





                case "Offers(Close)":

                    transactionItemType = TransactionItemType.Offer;
                    dataType = DataType.Closed;

                    break; 
                case "Module Contract":
                    transactionItemType = TransactionItemType.ModuleContract;
                    dataType = DataType.All;
                    break;
                case "Module Contract(Open)":
                    transactionItemType = TransactionItemType.ModuleContract;
                    dataType = DataType.Open;

                    break;





                case "Module Contract(Close)":

                    transactionItemType = TransactionItemType.ModuleContract;
                    dataType = DataType.Closed;

                    break;
                case "Sale Orders":

                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.All;

                    break;
                case "Sale Orders(Open)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.Open;

                    break;
                case "Sale Orders(Close)":
                    transactionItemType = TransactionItemType.Sale_Order;
                    dataType = DataType.Closed;

                    break;
                case "Sale Invoices":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.All;
                    break;
                case "Sale Invoices(Open)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.Open; break;





                case "Sale Invoices(Close)":
                    transactionItemType = TransactionItemType.Sale_Invoice;
                    dataType = DataType.Closed;

                    break;
                case "Memorandum Sales":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.All;

                    break;
                case "Memorandum Sales(Open)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.Open;

                    break;




                case "Memorandum Sales(Close)":
                    transactionItemType = TransactionItemType.Memorandum_Sale;
                    dataType = DataType.Closed;
                    break;

                case "Purchase Orders":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.All;
                    break;
                case "Purchase Orders(Open)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.Open;
                    break;





                case "Purchase Orders(Close)":
                    transactionItemType = TransactionItemType.Purchase_Order;
                    dataType = DataType.Closed;
                    break;
                case "Vendor Bills":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.All;
                    break;
                case "Vendor Bills(Open)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.Open;
                    break;





                case "Vendor Bills(Close)":
                    transactionItemType = TransactionItemType.Bill;
                    dataType = DataType.Closed;
                    break;
                case "Purchase Invoices":
                    transactionItemType = TransactionItemType.Purchase_Invoice;
                    dataType = DataType.All;
                    break;
                case "Purchase Invoices(Open)":
                    transactionItemType = TransactionItemType.Purchase_Invoice;
                    dataType = DataType.Open; break;
                case "Purchase Invoices(Close)":
                    transactionItemType = TransactionItemType.Purchase_Invoice;
                    dataType = DataType.Closed;

                    break;
                case "STL":
                    transactionItemType = TransactionItemType.STL;
                    dataType = DataType.All;
                    break;
                case "STL (Open)":
                    transactionItemType = TransactionItemType.STL;
                    dataType = DataType.Open; break;
                case "STL (Close)":
                    transactionItemType = TransactionItemType.STL;
                    dataType = DataType.Closed;

                    break;

                case "Inventory Adjustments":
                    transactionItemType = TransactionItemType.InventoryAdjustment;
                    dataType = DataType.All;
                    break;
                case "Inventory Adjustments(Open)":
                    transactionItemType = TransactionItemType.InventoryAdjustment;
                    dataType = DataType.Open;
                    break;
                case "Inventory Adjustments(Close)":
                    transactionItemType = TransactionItemType.InventoryAdjustment;
                    dataType = DataType.Closed;
                    break;

            }

            if (transactionItemType == TransactionItemType.STL)
            {
                winSTLRegister stlRegister = new winSTLRegister(1, transactionItemType, dataType);
                stlRegister.Show();
            }
            else
            {
                Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(1, transactionItemType, dataType);
                customerCenter.Show();
            }
        }
        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void BankList_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            bankListWindowFlag = false;
            e.Cancel = false;
        }
        private void Account_List_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            accntListWindowFlag = false;
            e.Cancel = false;
        }
        private void Collection_Method_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            clctionMthdListWindowFlag = false;
            e.Cancel = false;
        }
        private void MheadReports_MouseEnter(object sender, MouseEventArgs e)
        {
            LoadReports();
        }
        private void AddContactPerson_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            ucFrmAddContactPerson obj = new ucFrmAddContactPerson();
            win.Height = 300;
            win.Width = 800;
            win.Content = obj;
            win.Title = "Add Contact Person";
            //win.Show();

            win.ShowDialog();
        }
        private void MbtnCardHolderList_Click(object sender, ItemClickEventArgs e)
        {
            ucCardHolderList cardHolderList = new ucCardHolderList();
            Window cardHolderWin = new Window();
            cardHolderWin.Content = cardHolderList;
            cardHolderWin.Height = 450;
            cardHolderWin.Width = 400;
            cardHolderWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cardHolderWin.Show();
        }
        private void MbtnPayBills_Click(object sender, RoutedEventArgs e)
        {
            ucFrmPayments ucFrmPayment = new ucFrmPayments();
            DXWindow win = new DXWindow();
            //Theme theme = new Theme("NewTheme", "DevExpress.Xpf.Themes.NewTheme.v18.1");
            //theme.AssemblyName = "DevExpress.Xpf.Themes.NewTheme.v18.1";
            //Theme.RegisterTheme(theme);
            ThemeManager.SetThemeName(win, "DeepBlue");

            win.Title = "Enter Bill Payment";
            win.Content = ucFrmPayment;
            //ucFrmBankTransfer.frmBankTranfer.Height = 650;
            win.WindowState = WindowState.Maximized;
            //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }
        private void MbtnPayeeCategoryList_Click(object sender, RoutedEventArgs e)
        {
            //ucPayeeCategoryList categoryList = new ucPayeeCategoryList();
            //Window window = new Window();
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.Content = categoryList;
            //window.Show();
        }









        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCalendar_Click(object sender, RoutedEventArgs e)
        {
            ucCalender uccalendar = new ucCalender();
            grdRight.Children.Add(uccalendar);

            //Window win = new Window();
            //win.Content = uccalendar;
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Show();




        }

        private void BtnCalculator_Click(object sender, RoutedEventArgs e)
        {
            ucCalculator uccalculator = new ucCalculator();
            grdRight.Children.Add(uccalculator);

            // win.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        private void BtnHome_Click_1(object sender, RoutedEventArgs e)
        {
            // ucHome uchome = new ucHome();
            //  grdRight.Children.Add(uchome);

        }

        private void Btnnewcompany_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            frmcompanyadd.Owner = this;
            frmcompanyadd.Show();
        }


        private void docControlvisibility()
        {
            //if(layoutGrpDocManager.Items.Count == 0)
            //{
            //    dockLayoutManager.;
            //}
        }



        private void bAbout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            //MdiChild mdiChild = new MdiChild();
            //mdiChild.Height = 500;
            //mdiChild.Width = 900;


            //ucAbout about = new ucAbout();

            //mdiChild.Content = about;
            //mdiMainContainer.Children.Add(mdiChild);

        }

        private void BtnBarHome_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            // ucHome uchome = new ucHome();
            //  grdRight.Children.Add(uchome);
        }

        private void mbtnCurrencies_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            BussinessLogicss.frmCurrencyList frmCurrency = new BussinessLogicss.frmCurrencyList();
            frmCurrency.Owner = this;
            frmCurrency.Show();
        }

        private void MbtnSalesexchangerate_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            BussinessLogicss.frmSalesExchangeRateList exchangeRateList = new BussinessLogicss.frmSalesExchangeRateList();
            exchangeRateList.ShowDialog();
        }



        private void mbtnProducts_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Items") != null)
            {
                Productss.frmItemList frmItem = new Productss.frmItemList();
                frmItem.Owner = this;
                frmItem.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to view List of Items!");
            }
        }

        private void mbtnCategories_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Productss.frmCategoriesList frmItem = new Productss.frmCategoriesList();
            frmItem.Owner = this;
            frmItem.Show();
        }

        private void mbtnCategoryItems_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Productss.frmCategoryItemList frmItem = new Productss.frmCategoryItemList();
            frmItem.Owner = this;
            frmItem.Show();
        }

        private void mbtnUnitOfMeasure_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Productss.frmUnitOfMeasureList frmItem = new Productss.frmUnitOfMeasureList();
            frmItem.Owner = this;
            frmItem.Show();
        }

        private void mbtnItemNature_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Productss.frmProductNatureList frmItem = new Productss.frmProductNatureList();
            frmItem.Owner = this;
            frmItem.Show();
        }

        private void MbtnInquiryStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.Inquiriess.frmInquiryStatusList statusList = new Procurementss.Inquiriess.frmInquiryStatusList();
            statusList.ShowDialog();
        }

        private void MbtnofferStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.Offerss.frmOfferStatusList statusList = new Procurementss.Offerss.frmOfferStatusList();
            statusList.ShowDialog();
        }

        private void MbtnSaleOrderStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.SaleOrderss.frmSaleOrderStatusList statusList = new Procurementss.SaleOrderss.frmSaleOrderStatusList();
            statusList.ShowDialog();
        }

        private void MbtnSaleInvoiceStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.SaleInvoicess.frmSaleInvoiceStatusList statusList = new Procurementss.SaleInvoicess.frmSaleInvoiceStatusList();
            statusList.ShowDialog();
        }

        private void MbtnPurchaseOrderStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.PurchaseOrderss.frmPurchaseOrderStatusList statusList = new Procurementss.PurchaseOrderss.frmPurchaseOrderStatusList();
            statusList.ShowDialog();
        }

        private void MbtnMemorandumSaleStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.MemorandumSaless.frmMemorandumSaleStatusList statusList = new Procurementss.MemorandumSaless.frmMemorandumSaleStatusList();
            statusList.ShowDialog();

        }

        private void mbtnBillStatusList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.Billss.frmBillStatusList statusList = new Procurementss.Billss.frmBillStatusList();
            statusList.ShowDialog();
        }

        private void MbtnVendorPaymentStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Vendorss.frmVendorPaymentStatusList frmVendorPaymentStatusList = new Vendorss.frmVendorPaymentStatusList();
            frmVendorPaymentStatusList.ShowDialog();
        }

        private void MbtnAssetStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnReceiptStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                Window win = new Window();
                ucSaleReceiptsStatusList obj = new ucSaleReceiptsStatusList();
                win.Height = 450;
                win.Width = 500;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Content = obj;
                win.Title = "Sale Receipt Status List";
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            catch
            {

            }
        }

        private void MbtnInterBankTransStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucInterBankTransStatusList ucInterBankTransStatusList = new ucInterBankTransStatusList();

            ucInterBankTransStatusList.interBankTransStatusWin.Content = ucInterBankTransStatusList;
            ucInterBankTransStatusList.interBankTransStatusWin.Show();
        }

        private void MbtnNotificationsFlag_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucNotificationFlagList ucInterBankTransStatusList = new ucNotificationFlagList();
            Window flagWindow = new Window();
            flagWindow.Content = ucInterBankTransStatusList;
            flagWindow.Show();
        }

        private void MbtnEmployeeStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winEmployeeStatusList win = new winEmployeeStatusList();
            win.ShowDialog();
        }

        private void MbtnLeavesStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winLeaveStatusList win = new winLeaveStatusList();
            win.ShowDialog();
        }

        private void MbtnJVStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucJournalVoucherStatusList statusList = new ucJournalVoucherStatusList();
            Window win = new Window();

            win.Content = statusList;
            win.Show();
            //statusList.ShowDialog();
        }

        private void MbtnNewBillsStatus_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucBillStatusList statusList = new ucBillStatusList();
            DXWindow win = new DXWindow();

            win.Title = "Bills Register";

            win.Content = statusList;
            //ThemeManager.SetThemeName(win, "DeepBlue");

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnBillTypeList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.frmBillTypeList billtypeList = new Procurementss.frmBillTypeList();
            billtypeList.ShowDialog();
        }

        private void MbtnWarrantiesies_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmWarrantyList list = new frmWarrantyList();
            list.ShowDialog();
        }

        private void mbtnindustries_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmIndustryTypeList industryType = new frmIndustryTypeList();
            industryType.Owner = this;
            industryType.Show();
        }


        private void mbtnpaymentTerm_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Termss.frmPaymentTermList paymentTerms = new Termss.frmPaymentTermList();
            paymentTerms.Owner = this;
            paymentTerms.Show();
        }

        private void mbtnIncoterm_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Termss.frmIncoTermList incoTerms = new Termss.frmIncoTermList();
            incoTerms.Owner = this;
            incoTerms.Show();
        }

        private void MbtnAttachmentCategory_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Attachmentss.frmAttachmentCategoriesList categoriesList = new Attachmentss.frmAttachmentCategoriesList();
            categoriesList.ShowDialog();
        }

        private void MbtnCommentCategory_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Commentss.frmCategoriesList frmCategoriesList = new Commentss.frmCategoriesList();
            frmCategoriesList.Show();
        }

        private void MbtnTargerList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Targets") != null)
            {
                Targetss.frmTargetList targetList = new Targetss.frmTargetList();
                targetList.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to View List of Targets", "Permission Denied!");

            }

        }

        private void MbtnTargetTypeList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Target Types") != null)
            {
                Targetss.frmTargetTypeList targetTypeList = new Targetss.frmTargetTypeList();
                targetTypeList.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view List of Target Types", "Permission Denied!");

            }
        }

        private void MbtnDesignationList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmDesignationsList winDesigLst = new frmDesignationsList();
            winDesigLst.Show();
        }

        private void MbtnFunctionList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winAddFunctionList winAssetStatusList = new winAddFunctionList();
            winAssetStatusList.ShowDialog();
        }

        private void MbtnDepartmentReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Target Achivement") != null)
            {
                Targetss.frmTargetRegisterByDepartment achivedByDepartment = new Targetss.frmTargetRegisterByDepartment();
                achivedByDepartment.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view Achivement Targets", "Permission Denied!");

            }
        }

        private void MbtnTaxTypeList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window win = new Window();
            ucTaxTypeList taxTypeList = new ucTaxTypeList();
            win.Content = taxTypeList;
            win.Title = "Tax Type List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Height = 600;
            win.Width = 450;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnTaxList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window win = new Window();
            ucTaxList taxList = new ucTaxList();
            win.Content = taxList;
            win.Title = "Tax List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Height = 600;
            //win.Width = 450;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnTemplateList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucTemplateList templateList = new ucTemplateList();
            Window window = new Window();

            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = templateList;
            window.Show();
        }

        private void MbtnTemplateFields_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            FieldSelector win12 = new FieldSelector();
            win12.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win12.Show();
        }

        private void MbtnCOA_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            try
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Chart of Accounts") != null)
                {

                    ucChartofAccountList accountslistRegister = new ucChartofAccountList();
                    var register = new ChartofAccountRegister(accountslistRegister);
                    register.Show();
                }
                else
                    DXMessageBox.Show("Permission required to view list of Accounts", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void BtnGeneralJournalEntries_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {


            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.JV, 0);
            procurmentPanel.Show();
        }

        private void MbtnJournalTransactionRegister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winTransactionRegister register = new winTransactionRegister();
            ucTransactions transactionList = new ucTransactions();
            register.faRightGrid.Children.Add(transactionList);
            register.Show();
        }

        private void btnCompanyCenter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Companiess.frmcompanyCenter companycenter = new Companiess.frmcompanyCenter();
            companycenter.Owner = this;
            companycenter.Show();

        }

        //private void Mbtnexchangerate_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //  //  BussinessLogicss.frmExchangeRateList exchangeRateList = new BussinessLogicss.frmExchangeRateList();
        //   // exchangeRateList.ShowDialog();
        //}

        private void Mbtnexchangerate_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List market Exchange Rates") != null)
            {
                BussinessLogicss.frmMarketExchangeRateList exchangeRateList = new BussinessLogicss.frmMarketExchangeRateList();
                exchangeRateList.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view List of Market Exchange Rate", "Permission Denied!");

            }

        }

        private void mbtnmycompany_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Companiess.frmcompanyCenter companycenter = new Companiess.frmcompanyCenter();
            companycenter.Owner = this;
            companycenter.Show();
        }

        private void MbtnUsernRoles_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Userss.frmUsersRoles usersRoles = new Userss.frmUsersRoles();
            usersRoles.Show();
        }

        private void mbtnCustomers_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Customer Center") != null)
            //{
            //    Customerss.frmCustomerCenter customerCenter = new Customerss.frmCustomerCenter(0, TransactionItemType.UnDefined, DataType.All);

            //    customerCenter.Show();
            //}
            //else
            //{
            //    DXMessageBox.Show("Permission Denied!");
            //}

        }

        private void BtnOpenTransaction_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var button = (DevExpress.Xpf.Bars.BarButtonItem)sender;
            LoadTransactionGrid(button.Content.ToString());
            e.Handled = true;
        }


        private void MbtnCoshSheetFields_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.frmCostSheetFieldList frmCostSheetFieldList = new Procurementss.frmCostSheetFieldList();
            frmCostSheetFieldList.ShowDialog();
        }

        private void MbtnSummarySheetFields_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Procurementss.frmSummarySheetFieldList frmSheetFieldList = new Procurementss.frmSummarySheetFieldList();
            frmSheetFieldList.ShowDialog();
        }

        private void MbtnTargetAchived_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of All Achived Targets") != null)
            {
                Targetss.frmAchivedTarget achivedTarget = new Targetss.frmAchivedTarget();
                achivedTarget.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view Achivement Targets", "Permission Denied!");

            }
        }

        private void MbtnTargetregister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Target Achivement") != null)
            {
                Targetss.frmTargetRegister achivedByDepartment = new Targetss.frmTargetRegister();
                achivedByDepartment.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view Achivement Targets", "Permission Denied!");

            }
        }

        private void MbtnEnterBills_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill") != null)
            {
                ucFrmBillAdd ucFrmBill = new ucFrmBillAdd();
                Window win = new Window();

                win.Title = "Enter Bills";

                win.Content = ucFrmBill;

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Bill!");
            }
        }

        private void MbtnBillsList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name != "List of Admin Bills") != null)
            {
                AdminBillsRegister win = new AdminBillsRegister();
                win.Show();
                //ucBillList billList = new ucBillList();
                //DXWindow win = new DXWindow();

                //win.Title = "Bills Register";

                //win.Content = billList;
                ////ThemeManager.SetThemeName(win, "DeepBlue");

                //win.WindowState = WindowState.Maximized;
                //win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View Bills Register!");
            }
        }

        private void MbtnPayeeList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucPayeeList payeeList = new ucPayeeList();
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = payeeList;
            window.Show();
        }

        private void MbtnBillReferenceList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucBillReferenceList billReferenceList = new ucBillReferenceList();
            Window win = new Window();
            win.Content = billReferenceList;
            win.Show();
        }

        private void MbtnAdminBillLinkAdd_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bill Link") != null)
            {
                ucAdminBillTypeList adminBillTypeAdd = new ucAdminBillTypeList();
                Window win = new Window();
                win.Height = 450;
                win.Width = 550;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;


                win.Content = adminBillTypeAdd;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Admin Bill Link!");
            }
        }

        private void mbtnvendorcenter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Vendor Center") != null)
            {
                Vendorss.frmVendorCenter frmVendor = new Vendorss.frmVendorCenter();
                frmVendor.Owner = this;
                frmVendor.Show();
            }
            else
            {
                MessageBox.Show("Permission Denied"); 
            }
        }

        private void Mbtnfixedassets_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            bool isEnable = false;
            foreach (Window w in Application.Current.Windows)
            {
                if (w.Name == "faWindow")
                {
                    isEnable = true;
                    w.Activate();
                }
            }


            if (isEnable == false)
            {

            }
        }

        private void MbtnfaAdjustment_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //ucGridAssetValueAdjustment uc = new ucGridAssetValueAdjustment();
            //Window win = new Window();
            //win.Title = "Asset value adjustment";
            //win.Content = uc;
            //win.Width = 815;
            //win.Height = 585;
 

        }

        private void MbtnFuelLog_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void mbtnprincipalcenter_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Principalss.frmPrincipalCenter principalCenter = new Principalss.frmPrincipalCenter();
            principalCenter.Owner = this;
            principalCenter.Show();
        }

        private void mnuemployeeadd_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Employeess.frmEmployeeCenter employee = new Employeess.frmEmployeeCenter();
            employee.Owner = this;
            employee.Show();
        }

        private void MnuemployeeReg_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Employeess.frmEmployeeCenter employee = new Employeess.frmEmployeeCenter();
            employee.Owner = this;
            //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;
            //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;
            //employee.empTabControl.SelectedTabItem = SelectedTabItem.registersTab;

            employee.registersTab.IsSelected = true;
            employee.registersGrid.Visibility = Visibility.Visible;

            employee.Show();
        }

        private void MbtnLeavesRegister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winLeavesRegister win = new winLeavesRegister();
            win.Show();
        }

        private void MbtnLeavesSummary_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            winLeaveSummary win = new winLeaveSummary();
            win.Show();
        }

        private void Enter_Sale_Receipt_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                Window moduleWin = new Window();

                ucSaleReceiptSelectModule frmModuleSelect = new ucSaleReceiptSelectModule();
                //ucFrmPayments frmBillPayments = new ucFrmPayments();


                moduleWin.Content = frmModuleSelect;


                //enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //enterPaymentWin.WindowState = WindowState.Maximized;


                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.Width = 400;
                moduleWin.Height = 250;
                moduleWin.ResizeMode = ResizeMode.CanMinimize;
                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Sale Receipt!");
            }

        }

        private void SaleReceiptRegister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
                {
                    SalesReceiptRegister salesReceiptRegister = new SalesReceiptRegister();

                    salesReceiptRegister.Show();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Sale Receipt!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnInterBankTransfer_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                //ucFrmBankTransfer.frmBankTranfer.Height = 650;
                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                ucFrmBankTransfer.frmBankTranfer.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;

            }
        }

        private void MbtnBankTransferRegister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    //Window win = new Window();
                    InterBankTransferRegister interBankTransferWin = new InterBankTransferRegister();
                    interBankTransferWin.Show();
                    //ucBankTransferRegister bankTransferRegister = new ucBankTransferRegister();
                    //win.Content = bankTransferRegister;
                    //win.WindowState = WindowState.Maximized;
                    //win.Show();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view list of Inter-Bank Transfer!");
                    return;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnBankTransferInterCompany_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucBankTransferInterCompany uc = new ucBankTransferInterCompany();
            Window win = new Window();
            win.Content = uc;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnInterCompanyBankTransRegister_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            interCompanyBanktransfer win = new interCompanyBanktransfer();
            //Window win = new Window();
            //win.Content = obj;
            win.Show();
        }


        private void BranchList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window bank_win = new Window();

            try
            {
                if (bankListWindowFlag == false)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Banks") != null)
                    {
                        bank_win = new Window();

                        ucBranchList obj = new ucBranchList();

                        bank_win.Closing += BankList_Window_Closing;
                        bank_win.Content = obj;
                        bank_win.Title = "Bank List";
                        bank_win.Show();
                        bankListWindowFlag = true;
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Banks!");
                        return;
                    }

                }
                else
                {
                    if (bank_win.WindowState == WindowState.Minimized)
                        bank_win.WindowState = WindowState.Normal;
                    else
                        bank_win.Activate();
                }
            }
            catch
            {

            }
        }

        private void AccountList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window accnt_win = new Window();
            try
            {
                if (accntListWindowFlag == false)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Accounts") != null)
                    {
                        accnt_win = new Window();
                        ucAccountList obj = new ucAccountList();

                        accnt_win.Closing += Account_List_Window_Closing;
                        accnt_win.Content = obj;
                        accnt_win.WindowState = WindowState.Maximized;
                        accnt_win.Title = "Account List";
                        accnt_win.Show();

                        accntListWindowFlag = true;
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Accounts!");
                        return;
                    }

                }
                else
                {
                    if (accnt_win.WindowState == WindowState.Minimized)
                        accnt_win.WindowState = WindowState.Normal;
                    else
                        accnt_win.Activate();
                }
            }

            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void AddCollectionMethod_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            UcListWindow clction_win = new UcListWindow();

            try
            {
                if (clctionMthdListWindowFlag == false)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Collection Methods") != null)
                    {
                        clction_win = new UcListWindow();
                        ucCollectionMethodList obj = new ucCollectionMethodList();

                        clction_win.Closing += Collection_Method_Window_Closing;
                        clction_win.Height = 450;
                        clction_win.Width = 500;
                        clction_win.ResizeMode = ResizeMode.CanMinimize;
                        clction_win.Content = obj;
                        clction_win.Title = "Collection Method List";
                        clction_win.Show();
                        clctionMthdListWindowFlag = true;
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Collection methods!");
                        return;
                    }
                }
                else
                {
                    if (clction_win.WindowState == WindowState.Minimized)
                        clction_win.WindowState = WindowState.Normal;
                    else
                        clction_win.Activate();
                }
            }
            catch
            {

            }
        }

        private void MbtnTransferMethod_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ucTransferMethodList ucTransferMethod = new ucTransferMethodList();


            ucTransferMethod.tranferMethodListWin.Content = ucTransferMethod;
            ucTransferMethod.tranferMethodListWin.Show();
        }

        private void MbtnDeductionList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            Window deduction_win = new Window();

            try
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Deductions") != null)
                {
                    deduction_win = new Window();
                    ucDeductionList obj = new ucDeductionList();

                    deduction_win.Height = 450;
                    deduction_win.Width = 500;
                    deduction_win.ResizeMode = ResizeMode.CanMinimize;
                    deduction_win.Content = obj;
                    deduction_win.Title = "Deduction List";

                    deduction_win.Show();
                    deductionListWindowFlag = true;

                }
                else
                {

                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view List of Deductions!");
                    return;
                }
            }
            catch
            {

            }




        }
        private void RibbonControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RibbonControl ribbonControl = sender as RibbonControl;

            if (ribbonControl.SelectedPage.Name == "mheadReports")
            {
                LoadReports();
                LoadSharedReports();
            }

        }

        private void mbtnCustomerReports_Click(object sender, ItemClickEventArgs e)
        {
            myReport report = new myReport();

            var inquiryRepo = new InquiryRepo();
            report.DataSource = inquiryRepo.getAllTemp();

            Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(report);
            reportPanel.Show();
        }

        private void MbtnReportGroupList_Click(object sender, ItemClickEventArgs e)
        {
            Reportss.frmReportGroupList reportGroupList = new Reportss.frmReportGroupList();
            reportGroupList.Show();
        }
        private void MbtnTrialBalance_Click(object sender, ItemClickEventArgs e)
        {
            try
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
                {
                    winTrialBalance trialBalance = new winTrialBalance();
                    trialBalance.Show();
                }
                else
                    DXMessageBox.Show("Permission required to view Trial Balance", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void MbtnProfitandLoss_Click(object sender, ItemClickEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Profit & Loss") != null)
            {
                winProfitandLoss profitandLoss = new winProfitandLoss();
                profitandLoss.Show();
            }
            else
                DXMessageBox.Show("Permission required to view Profit & Loss", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnChat_Click(object sender, ItemClickEventArgs e)

        {

        }
        private void RibbonControl_RibbonPopupMenuShowing(object sender, RibbonPopupMenuShowingEventArgs e)
        {
            e.Cancel = true;
        }
        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
        private void BarButtonItem_ItemClick_1(object sender, ItemClickEventArgs e)
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reconcile Chart of Account") != null)
            {
                winSelectReconcileAccount reconcileAccount = new winSelectReconcileAccount();
                reconcileAccount.Show();
            }
            else

                DXMessageBox.Show("Permission required to Reconcile Account!");

        }

        private void MbtnCreditCardList_Click(object sender, ItemClickEventArgs e)
        {
            ucCreditCardsList creditCardsList = new ucCreditCardsList();
            Window creditCardWin = new Window();
            creditCardWin.Content = creditCardsList;
            creditCardWin.Show();
        }

        private void MbtnCreditCardTypeList_Click(object sender, ItemClickEventArgs e)
        {
            ucCreditCardTypeList creditCardsList = new ucCreditCardTypeList();
            Window creditCardWin = new Window();
            creditCardWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            creditCardWin.Content = creditCardsList;
            creditCardWin.Show();
        }

        private void BtnAccordionCalculator_Click(object sender, RoutedEventArgs e)
        {
            FavouriteItems.Children.Clear();
            ucCalculator uccalculator = new ucCalculator();
            FavouriteItems.Children.Add(uccalculator);
        }
        private void BtnAccordionCalendar_Click(object sender, RoutedEventArgs e)
        {
            FavouriteItems.Children.Clear();
            ucCalender uccalculator = new ucCalender();
            FavouriteItems.Children.Add(uccalculator);
        }
        private void BtnAccordionTask_Click(object sender, RoutedEventArgs e)
        {
            FavouriteItems.Children.Clear();
            ucUserTasks uccalculator = new ucUserTasks();
            FavouriteItems.Children.Add(uccalculator);
        }
        private void BtnCloseTabs_Click(object sender, RoutedEventArgs e)
        {
            getBackground();
        }
        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void MbtnBalanceSheet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Balance Sheet Detail") != null)
                {
                    winBalanceSheet balanceSheet = new winBalanceSheet();
                    balanceSheet.Show();
                }
                else
                    DXMessageBox.Show("Permission required to View Balance Sheet Detail", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void MbtnPreviousReconcilation_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Reconcilation History") != null)
            {
                winReconcilationHistory winReconcilationHistory = new winReconcilationHistory();
                winReconcilationHistory.Show();
            }
            else
                DXMessageBox.Show("Permission required to View Reconcilation History", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void MbtnEnterBillPayment_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                Window moduleWin = new Window();

                ucFrmModuleSelect frmModuleSelect = new ucFrmModuleSelect();
                //ucFrmPayments frmBillPayments = new ucFrmPayments();

                moduleWin.Content = frmModuleSelect;

                //enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //enterPaymentWin.WindowState = WindowState.Maximized;

                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.Width = 400;
                moduleWin.Height = 250;
                moduleWin.ResizeMode = ResizeMode.CanMinimize;
                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Payment!");
            }




        }

        private void MbtnBillPaymentList_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                PaymentRegister moduleWin = new PaymentRegister();
                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;


                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Payments!");
            }



        }
        private void MbtnPaymentStatus_Click(object sender, ItemClickEventArgs e)
        {
            ucPaymentStatusList statusList = new ucPaymentStatusList();
            DXWindow win = new DXWindow();

            win.Title = "Payment Status Register";

            win.Content = statusList;
            //ThemeManager.SetThemeName(win, "DeepBlue");

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnPaymentMethodList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucPaymentMethodList methodList = new ucPaymentMethodList();
            DXWindow win = new DXWindow();

            win.Title = "Payment Method Register";

            win.Content = methodList;
            //ThemeManager.SetThemeName(win, "DeepBlue");

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnPurchaseInvoiceStatusList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusList statusList = new Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusList();
            statusList.ShowDialog();
        }
        //private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        //{
        //    ucTask task = new ucTask();
        //    DocumentPanel documentPanel = new DocumentPanel();
        //    documentPanel.Content = task;
        //    documentPanel.Caption = "Task";
        //    documentPanel.Name = "Task";


        private void btnAddNewImage_Click(object sender, ItemClickEventArgs e)
        {
            //frmBackground frmbg = new frmBackground();
            //frmbg.Show();
        }

        private void BtnSetBackground_ItemClick(object sender, ItemClickEventArgs e)
        {

            //frmUploadBackground frmUpload = new frmUploadBackground();
            //frmUpload.Show();

        }
        private void BtnApplyImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadBackground();

        }



        public void getBackground()
        {
            try
            {
                string path = @"";
                BackgroundImagesRepo imagesRepo = new BackgroundImagesRepo();
                List<ERP_BL.BackgroundImages.BackgroundImages> store = imagesRepo.getSpecificImage();
                if (store == null || store.Count == 0)
                {
                    ERP_BL.BackgroundImages.BackgroundImages storeShareAll = imagesRepo.getShareAllImage();
                    if (storeShareAll != null)
                    {
                        if (!string.IsNullOrEmpty(storeShareAll.Path))
                        {
                            path = storeShareAll.Path;
                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                            var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);

                        }

                        else
                            return;

                        var _str = path;

                        int pos = path.LastIndexOf("/") + 1;
                        int posId = path.IndexOf(",");
                        _str = path.Substring(pos, path.Length - pos);

                        string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                        string[] fileEntries = Directory.GetFiles(root);
                        path = path.Substring(pos, path.Length - pos);
                        foreach (string fileName in fileEntries)
                        {
                            int position = fileName.LastIndexOf("\\") + 1;
                            var _filename = fileName.Substring(position, fileName.Length - position);
                            if (_filename == path)
                            {


                                BitmapImage bitmap = new BitmapImage();
                                using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                {
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = fs;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();

                                    FavouriteItems.Children.Clear();
                                    Image img = new Image();
                                    img.Stretch = Stretch.Fill;
                                    img.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                    FavouriteItems.Children.Add(img);

                                    Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                                    var image = bitmap;
                                    var elements = grid.Children;

                                    foreach (UIElement _element in elements)
                                    {
                                        if (_element is Image)
                                        {
                                            var imge = (Image)_element;
                                            imge.Source = image;
                                            break;
                                        }

                                    }
                                }
                                break;






                            }
                        }
                    }
                    return;
                }

                //if(isRenew == false)
                //{
                //isRenew = true;
                if (isShareAll == true)
                {



                    List<string> listOfEmp = new List<string>();
                    int sizeOfList = store.Count - 1;
                    for (int i = sizeOfList; i >= 0; i--)
                    {

                        var _store = store[i];
                        listOfEmp = new List<string>(_store.EmployeeIds.Split(new string[] { "," }, StringSplitOptions.None));



                        if (listOfEmp.Contains(SYSTEM_STATIC.currentUser.employeeId.ToString()))
                        {


                            if (!string.IsNullOrEmpty(_store.Path))
                            {
                                path = _store.Path;
                                BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                                var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);

                            }

                            else
                                return;

                            var _str = path;

                            int pos = path.LastIndexOf("/") + 1;
                            int posId = path.IndexOf(",");
                            _str = path.Substring(pos, path.Length - pos);

                            string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                            string[] fileEntries = Directory.GetFiles(root);
                            path = path.Substring(pos, path.Length - pos);
                            foreach (string fileName in fileEntries)
                            {
                                int position = fileName.LastIndexOf("\\") + 1;
                                var _filename = fileName.Substring(position, fileName.Length - position);
                                if (_filename == path)
                                {

                                    //List<string> listOfEmp = new List<string>(store.EmployeeIds.Split(new string[] { "," }, StringSplitOptions.None));
                                    //ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();

                                    BitmapImage bitmap = new BitmapImage();
                                    using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                    {
                                        bitmap.BeginInit();
                                        bitmap.StreamSource = fs;
                                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmap.EndInit();

                                        FavouriteItems.Children.Clear();
                                        Image img = new Image();
                                        img.Stretch = Stretch.Fill;
                                        img.Name = "imgDynamic1";
                                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                        FavouriteItems.Children.Add(img);

                                        Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                                        var image = bitmap;
                                        var elements = grid.Children;

                                        foreach (UIElement _element in elements)
                                        {
                                            if (_element is Image)
                                            {
                                                var imge = (Image)_element;
                                                imge.Source = image;
                                                break;
                                            }

                                        }
                                    }
                                    break;

                                }

                            }
                            isShareAll = false;
                            break;
                        }


                    }
                }

                //    List<string> listOfEmpNew = new List<string>();
                //int sizeOfListNew = store.Count - 1;
                //for (int i = sizeOfListNew; i >= 0; i--) 
                //{
                //    var _store = store[i];
                //    listOfEmpNew = new List<string>(_store.EmployeeIds.Split(new string[] { "," }, StringSplitOptions.None));


                //    if (!listOfEmp.Contains(SystemLogic.currentUser.employeeId.ToString()))
                //    {

                if (isShareAll == true)
                {


                    {
                        ERP_BL.BackgroundImages.BackgroundImages storeShareAll = imagesRepo.getShareAllImage();
                        if (storeShareAll != null && storeShareAll.EmployeeIds == null)
                        {
                            if (!string.IsNullOrEmpty(storeShareAll.Path))
                            {
                                path = storeShareAll.Path;
                                BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                                var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);

                            }

                            else
                                return;

                            var _str = path;

                            int pos = path.LastIndexOf("/") + 1;
                            int posId = path.IndexOf(",");
                            _str = path.Substring(pos, path.Length - pos);

                            string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                            string[] fileEntries = Directory.GetFiles(root);
                            path = path.Substring(pos, path.Length - pos);
                            foreach (string fileName in fileEntries)
                            {
                                int position = fileName.LastIndexOf("\\") + 1;
                                var _filename = fileName.Substring(position, fileName.Length - position);
                                if (_filename == path)
                                {


                                    BitmapImage bitmap = new BitmapImage();
                                    using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                    {
                                        bitmap.BeginInit();
                                        bitmap.StreamSource = fs;
                                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                        bitmap.EndInit();

                                        FavouriteItems.Children.Clear();
                                        Image img = new Image();
                                        img.Stretch = Stretch.Fill;
                                        img.Name = "imgDynamic1";
                                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                        FavouriteItems.Children.Add(img);

                                        Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                                        var image = bitmap;
                                        var elements = grid.Children;

                                        foreach (UIElement _element in elements)
                                        {
                                            if (_element is Image)
                                            {
                                                var imge = (Image)_element;
                                                imge.Source = image;
                                                break;
                                            }

                                        }
                                    }
                                    break;






                                }
                            }
                        }


                    }
                }
            }
            catch
            {

            }
        }


        public void loadBackground()
        {
            selectEmployeeWindow selectEmployee = new selectEmployeeWindow();

            try
            {
                Window win = new Window();
                ucSelectOption selectoption = new ucSelectOption();
                win.Content = selectoption;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Width = 400;
                win.Height = 250;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
                if (isSpecific == 1)
                {

                    string sourceFile = @"";
                    string exePath = System.Environment.GetCommandLineArgs()[0];
                    string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                    destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                    System.IO.Directory.CreateDirectory(destination);
                    sourceFile = fileDialog.FileName;
                    if (!string.IsNullOrEmpty(sourceFile))
                    {
                        destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                        if (File.Exists(destination))
                        {
                            File.Delete(destination);
                        }
                        System.IO.File.Copy(sourceFile, destination);
                        BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                        BackgroundImages shareAllBgImages = new BackgroundImages();
                        var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                        if (result.Item1)
                        {
                            shareAllBgImages.Path = result.Item2;
                            shareAllBgImages.userId = SYSTEM_STATIC.currentUser.id;
                            shareAllBgImages.UploadedTime = DateTime.Now;
                            bgImages.DeleteBackground(shareAllBgImages);
                            //bgImages.AddEmployee(result.Item2, selectEmployee.empId);
                        }

                    }
                }
                if (isSpecific == 2)
                {
                    selectEmployee = new selectEmployeeWindow();
                    selectEmployee.ShowDialog();
                    if (selectEmployee.isCancle == true)
                    {
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        sourceFile = fileDialog.FileName;
                        if (!string.IsNullOrEmpty(sourceFile))
                        {
                            destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }
                            System.IO.File.Copy(sourceFile, destination);

                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                            var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                            if (result.Item1)
                            {
                                bgImages.AddEmployee(result.Item2, selectEmployee.empId, SYSTEM_STATIC.currentUser.id);
                            }

                        }
                    }

                    // MessageBox.Show("Under Constrution please wait for some days");
                }
                if (isSpecific == 3)
                {
                    frmSelectGroups selectGroups = new frmSelectGroups();
                    selectGroups.ShowDialog();
                    if (selectGroups.isCancle == true)
                    {
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "ErpBackground\\BackgroundImages\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        sourceFile = fileDialog.FileName;
                        if (!string.IsNullOrEmpty(sourceFile))
                        {
                            destination += MainWindow.currentUserid + "_" + TransactionItemType.BackgroundImages.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (File.Exists(destination))
                            {
                                File.Delete(destination);
                            }
                            System.IO.File.Copy(sourceFile, destination);

                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(destination);
                            var result = bgImages.startUploadingBackgroundImage(TransactionItemType.BackgroundImages);
                            if (result.Item1)
                            {
                                selectGroups.taskRepoGroup.AddGroup(result.Item2, selectGroups.groupId, SYSTEM_STATIC.currentUser.id, selectGroups.groupUser);
                                //bgImages.AddGroup(result.Item2, selectGroups.groupId, SYSTEM_STATIC.currentUser.id, selectGroups.groupUser);

                            }

                        }
                    }


                }



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }


        }


        private void BtnpreviewNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (bmImg.StreamSource != null)
                {
                    if (bmImg.PixelWidth > 700 && bmImg.PixelHeight > 500)

                    {

                        FavouriteItems.Children.Clear(); //Clear previous children
                        Image img = new Image();
                        img.Stretch = Stretch.Fill;
                        RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                        FavouriteItems.Children.Add(img); //add new properties 
                        Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                        var image = bmImg;
                        var elements = grid.Children;
                        foreach (UIElement _element in elements)
                        {
                            if (_element is Image) //check element in an image
                            {
                                var imge = (Image)_element;
                                imge.Source = image;
                                break;
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Upload image with higher dimension");
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload First to Preview the images");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        private void BtnUpload_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {
                try
                {

                    if (MainWindow.currentUserid != 0)
                    {
                        var abc = isSpecific;
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; .png;";
                        fileDialog.Multiselect = false;

                        if (fileDialog.ShowDialog() == true)
                        {
                            var fileName = System.IO.Path.GetFileName(fileDialog.FileName);
                            var filePath = System.IO.Path.GetFullPath(fileDialog.FileName);
                            bmImg = new BitmapImage();
                            using (FileStream fs = new FileStream(filePath, FileMode.Open))
                            {
                                bmImg.BeginInit();
                                bmImg.StreamSource = fs;
                                bmImg.CacheOption = BitmapCacheOption.OnLoad;
                                bmImg.EndInit();
                                MessageBox.Show("Background Image is successfully Uploaded Press ok!");
                            }
                        }
                    }

                }
                catch
                {

                }
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Upload Image!");
            }
        }



        private void BtnPopupCreate_ItemClick(object sender, ItemClickEventArgs e)          

        {
            frmCreatePopup createPopup = new frmCreatePopup();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {


                //createPopup = new frmCreatePopup();
                createPopup.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Create Notifications!");
            }
        }


        private async void BtnShowNotification_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                currentOnlieUserId = SYSTEM_STATIC.currentUser.id;
                unicodeConnected = "\u0053\u0045\u004e\u0044\u004e\u004f\u0054\u0049\u0046\u0049\u0043\u0041\u0054\u0049\u004f\u004e";
                await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);
                //await  connection.InvokeAsync("SendNotification");


            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }



            // getInstantNotification();previouslyUsed
        }

        //private void BtnShowNotification_ItemClick(object sender, ItemClickEventArgs e)
        //{

        //    getInstantNotification();
        //}






        //static Timer x;
        //void InstantNotification()
        //{



        //    x = new Timer(timer_Elapsed1, null, 10000, 60000);
        //}


        //public void timer_Elapsed1(object state)
        //{


        //    //Thread.Sleep(100);
        //    var popup = PopupRepo.getShareAllPopup();


        //    if (popup != null)
        //    {
        //        if (popup.Id != PopUpId)
        //        {



        //            getInstantNotification();
        //        }


        //        //x.Change(10000, Timeout.Infinite);


        //    }


        //}


        //DispatcherTimer time = null;
        //void StartTimer()
        //{
        //    time = new DispatcherTimer();
        //    time.Interval = TimeSpan.FromSeconds(10);
        //    time.Tick += new EventHandler(timer_Elapsed);
        //    time.Start();
        //}
        //private void timer_Elapsed(object sender, EventArgs e)
        //{
        //    time.Stop();
        //    getPopupNotifications();
        //    isWindowLoad = false;
        //}

        public void getInstantNotification()
        {


            Application.Current.Dispatcher.Invoke((Action)delegate {
                try
                {
                    ERP_BL.PopupNotificatios.popupNotificatinRepo PopupRepo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();



                    var popup = PopupRepo.getShareAllPopup();
                    if (popup != null)
                        PopUpId = popup.Id;


                    if (isWindowLoad)
                    {
                        if (popup != null)
                        {
                            isWindowLoad = false;
                            frmShowPopup showPopup = new frmShowPopup();
                            showPopup.notificationLabel.Text = popup.popupText;
                            showPopup.notificationLabel.FontSize = popup.FontSize;
                            showPopup.notificationTitle.FontSize = popup.FontSizeHeading;


                            if (popup.fontWeightHeading == "ExtraBold")
                                showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(800);
                            else

                                showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(100);

                            if (popup.fontWeight == "ExtraBold")
                                showPopup.notificationLabel.FontWeight = FontWeight.FromOpenTypeWeight(800);



                            if (popup.Italic == "Italic")
                                showPopup.notificationLabel.FontStyle = FontStyles.Italic;
                            else
                                showPopup.notificationLabel.FontStyle = FontStyles.Normal;



                            if (popup.ItalicHeading == "Italic")
                                showPopup.notificationTitle.FontStyle = FontStyles.Italic;
                            else
                                showPopup.notificationTitle.FontStyle = FontStyles.Normal;

                            showPopup.notificationTitle.Text = popup.Title;


                            //showPopup.notificationLabel.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));
                            //showPopup.notificationTitle.Text = popup.Title;


                            if (popup.TitleColorCode != null && popup.TextColorCode != null)
                            {
                                Color color = (Color)ColorConverter.ConvertFromString(popup.TitleColorCode);
                                showPopup.notificationTitle.Foreground = new System.Windows.Media.SolidColorBrush(color);
                                Color color1 = (Color)ColorConverter.ConvertFromString(popup.TextColorCode);
                                showPopup.notificationLabel.Foreground = new System.Windows.Media.SolidColorBrush(color1);
                            }

                            showPopup.ShowDialog();
                        }

                    }

                    else
                    {
                        if (popup != null)
                        {
                            frmShowPopup showPopup = new frmShowPopup();

                            showPopup.notificationLabel.Text = popup.popupText;
                            showPopup.notificationLabel.FontSize = popup.FontSize;
                            showPopup.notificationTitle.FontSize = popup.FontSizeHeading;

                            if (popup.fontWeightHeading == "ExtraBold")
                                showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(800);
                            else
                                showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(100);

                            if (popup.fontWeight == "ExtraBold")
                                showPopup.notificationLabel.FontWeight = FontWeight.FromOpenTypeWeight(800);


                            if (popup.Italic == "Italic")
                                showPopup.notificationLabel.FontStyle = FontStyles.Italic;
                            else
                                showPopup.notificationLabel.FontStyle = FontStyles.Normal;


                            if (popup.ItalicHeading == "Italic")
                                showPopup.notificationTitle.FontStyle = FontStyles.Italic;
                            else
                                showPopup.notificationTitle.FontStyle = FontStyles.Normal;

                            showPopup.notificationTitle.Text = popup.Title;

                            if (popup.TitleColorCode != null && popup.TextColorCode != null)
                            {
                                Color color = (Color)ColorConverter.ConvertFromString(popup.TitleColorCode);
                                showPopup.notificationTitle.Foreground = new System.Windows.Media.SolidColorBrush(color);
                                Color color1 = (Color)ColorConverter.ConvertFromString(popup.TextColorCode);
                                showPopup.notificationLabel.Foreground = new System.Windows.Media.SolidColorBrush(color1);
                            }

                            showPopup.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Please add First to show a notification");
                        }
                    }


                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            });


        }
        public void getPopupNotifications()
        {
            try
            {
                ERP_BL.PopupNotificatios.popupNotificatinRepo PopupRepo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();



                var popup = PopupRepo.getShareAllPopup();



                if (isWindowLoad)
                {

                    if (popup != null)
                    {
                        frmShowPopup showPopup = new frmShowPopup();
                        showPopup.notificationLabel.Text = popup.popupText;
                        showPopup.notificationTitle.Text = popup.Title;


                        if (popup.TitleColorCode != null && popup.TextColorCode != null)
                        {

                            Color color = (Color)ColorConverter.ConvertFromString(popup.TitleColorCode);
                            showPopup.notificationTitle.Foreground = new System.Windows.Media.SolidColorBrush(color);
                            Color color1 = (Color)ColorConverter.ConvertFromString(popup.TextColorCode);
                            showPopup.notificationLabel.Foreground = new System.Windows.Media.SolidColorBrush(color1);
                        }



                        showPopup.ShowDialog();
                    }



                }


                else if (isInstant == false)
                {

                    if (popup != null)
                    {
                        frmShowPopup showPopup = new frmShowPopup();
                        showPopup.notificationLabel.Text = popup.popupText;
                        showPopup.notificationTitle.Text = popup.Title;

                        if (popup.TitleColorCode != null && popup.TextColorCode != null)
                        {
                            Color color = (Color)ColorConverter.ConvertFromString(popup.TitleColorCode);
                            showPopup.notificationTitle.Foreground = new System.Windows.Media.SolidColorBrush(color);
                            Color color1 = (Color)ColorConverter.ConvertFromString(popup.TextColorCode);
                            showPopup.notificationLabel.Foreground = new System.Windows.Media.SolidColorBrush(color1);
                        }

                        showPopup.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Please add First to show a notification");
                    }
                }




            }

            catch (Exception ex)
            {

                DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }

        private void BtnDeletePopupNotification_ItemClick(object sender, ItemClickEventArgs e)
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {



                var response = MessageBox.Show("Do you really want to delete Notifications?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (response == MessageBoxResult.No)
                {


                }
                else
                {

                    ERP_BL.PopupNotificatios.popupNotificatinRepo Repo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();
                    ERP_BL.PopupNotificatios.popupNotifications popupNotifications = new ERP_BL.PopupNotificatios.popupNotifications();
                    var store = Repo.getShareAllPopup();
                    if (store != null)
                    {
                        Repo.DeletePopupNotifications(popupNotifications);
                        MessageBox.Show("Official notification has be deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Their is No Official notification Detected!");
                    }





                }
            }


            else
            {
                DXMessageBox.Show("You are not allowed to Delete Notifications!");
            }

        }
        private void MbtnNotifications_ItemClick(object sender, ItemClickEventArgs e)
        {
            FavouriteItems.Children.Clear();
            grdNotifications = new ucCommentsGrid();
            grdNotifications.urgentNotification = false;
            grdNotifications.myParent = this;
            FavouriteItems.Children.Add(grdNotifications);
        }

        public void getCountAllUnreadNotifications()
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            var unReadCount = notificationsRepo.CountAllUnreadNotifications(currentUserid);
            //            txtUnReadcount.Text = unReadCount.ToString();







            mbtnNotifications.Content = "(" + unReadCount.ToString() + ")";
        }

        public void getCountAllUnreadMemoNotifications()
        {
            MemoNotificationsRepo notificationsRepo = new MemoNotificationsRepo();
            var unReadCount = notificationsRepo.CountAllUnreadNotifications(currentUserid);
            //            txtUnReadcount.Text = unReadCount.ToString();







            barMemos.Content = "(" + unReadCount.ToString() + ")";
        }






        private void MbtnPQList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucPQList pqList = new ucPQList();
            Window win = new Window();
            win.Content = pqList;
            win.Show();

        }

        private void MbtnShppingTermsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucShipmentTermsList stList = new ucShipmentTermsList();
            Window win = new Window();
            win.Content = stList;
            win.Show();


        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            SYSTEM_STATIC.PopulateTransactionPanel();
            this.Dispatcher.Invoke(new Action(() =>
            {

                mheadVendor.IsEnabled = true;

                mheadCustomer.IsEnabled = true;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Banking") != null)
                {
                    btnBanking.IsEnabled = true;
                }
                else
                {
                    btnBanking.IsEnabled = false;
                }
                mbtnNotifications.IsEnabled = true;
                try
                {


                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(ex.Message);
                }
            }));
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }
        private void BarRentalAssetsList_ItemClick(object sender, ItemClickEventArgs e)
        {


        }
        private void BarTenantList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void BarRentalItem_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void BarButtonItem_ItemClick_2(object sender, ItemClickEventArgs e)
        {
        }
        private void BarTenantContractList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void MbtnLoanList_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoansRegister loansListWin = new LoansRegister();
            loansListWin.Title = "Loan Window";
            //win.Content = loansList;
            loansListWin.WindowState = WindowState.Maximized;
            loansListWin.Show();
        }
        private void BarOwnerList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void MbtnFacitiyNatureList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucFacilityNatureList facilityNatureList = new ucFacilityNatureList();
            Window win = new Window();
            win.Content = facilityNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnEnterLoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucFrmLoans frmLoans = new ucFrmLoans();
            DXWindow win = new DXWindow();
            win.Title = "Loan Window";
            win.Content = frmLoans;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }
        private void MbtnBillCategoryList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucBillCategoryList billCategoryList = new ucBillCategoryList();
            Window win = new Window();
            win.Content = billCategoryList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void BarNotOwnedList_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void MbtnManagementSummaryList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucManagementSummaryList frmSummary = new ucManagementSummaryList();
            Window win = new Window();
            win.Content = frmSummary;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnAdminBillNatureList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAdminBillNatureList frmBillNature = new ucAdminBillNatureList();
            Window win = new Window();
            win.Content = frmBillNature;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnRentalAssetStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
        }
        private void BarInventoryReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inventory") != null)
            {

                MainView view = new MainView();
                //ucInventory inventory = new ucInventory();
                view.Show();
            }
            else
                DXMessageBox.Show("Permission Required to Inventory", "Permission Denied!");
        }
        private void MbtnLoansStatus_Click(object sender, ItemClickEventArgs e)
        {
            ucLoansStatusList statusList = new ucLoansStatusList();
            DXWindow win = new DXWindow();
            win.Title = "Loans Status Register";
            win.Content = statusList;

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void BarInventoryDetailReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inventory") != null)
            {
            }
            else
                DXMessageBox.Show("Permission Required to Inventory", "Permission Denied!");
        }
        private void MbtnReligionTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucReligionList religionList = new ucReligionList();
            Window win = new Window();

            win.Content = religionList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnToDoList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ToDoTasksPanel toDoTasks = new ToDoTasksPanel();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Show();
        }


        private void BtnAddInquiry_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, 0);
            procurmentPanel.Show();
        }
        private void BtnAddOffer_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Offer, 0);
            procurmentPanel.Show();
        }
        private void BtnAddSO_ItemClick(object sender, ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to create link SO? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Create Sale order from reference key") != null)
                {
                    ucSelectParentSOKey ucSelectParentSOKey = new ucSelectParentSOKey();
                    ucSelectParentSOKey.ShowDialog();
                    if (ucSelectParentSOKey.soReftId != 0)
                    {


                        Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, 0, ucSelectParentSOKey.soReftId, true, true, true);
                        procurmentPanel.Show();
                    }
                }
                else
                {
                    DXMessageBox.Show("You do not have permission Create Sale order from reference ke", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, 0);
                procurmentPanel.Show();
            }

        }
        private void BtnAddSI_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, 0);
            procurmentPanel.isCommission = true;
            procurmentPanel.Show();
        }
        private void BtnAddPO_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, 0);
            procurmentPanel.Show();
        }
        private void BtnAddPI_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, 0);
            procurmentPanel.isCommission = true;
            procurmentPanel.Show();
        }



        private void BtnAddVBill_ItemClick(object sender, ItemClickEventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, 0);
            procurmentPanel.Show();
        }

        private void MbtnTaskTargetTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTargetTypeList targetTypeList = new ucTargetTypeList();
            Window win = new Window();
            win.Content = targetTypeList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnVendorBillReferenceList_Click(object sender, ItemClickEventArgs e)
        {
            ucVendorBillRefList billReferenceList = new ucVendorBillRefList();
            Window win = new Window();
            win.Content = billReferenceList;
            win.Show();
        }
        private void BtnGeneralJournalEntries_Click(object sender, EventArgs e)
        {
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.JV, 0);
            procurmentPanel.Show();
        }
        private void MbtnToDoTaskStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucToDoTaskStatusList statusList = new ucToDoTaskStatusList();
            DXWindow win = new DXWindow();
            win.Title = "Tasks Status Register";
            win.Content = statusList;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnTaskStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTaskStatusList statusList = new ucTaskStatusList();
            DXWindow win = new DXWindow();
            win.Title = "Tasks Status Register";
            win.Content = statusList;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void BarToDoTask_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTasksList tasksList = new ucTasksList();
            ToDoTasksPanel toDoTasks = new ToDoTasksPanel();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.gridTaskPanel.Children.Add(tasksList);
            toDoTasks.Show();
        }
        private void MbtnVendorBillNatureList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucVendorBillNatureList frmBillNature = new ucVendorBillNatureList();
            Window win = new Window();
            win.Content = frmBillNature;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void mbtnSalesExchangerates_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Exchange Rates") != null)
            {


                ucSalesER ucExchangeRegister = new ucSalesER();
                ucExchangeRegister.Show();
            }

            else
            {


                DXMessageBox.Show("Permission Required to View Exchange Rates", "Permission Denied!");
            }
        }



        private void mbtnMarketExchangerates_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Exchange Rates") != null)
            {

                ucMarketER ucExchangeRegister = new ucMarketER();
                ucExchangeRegister.Show();
            }

            else
            {

                DXMessageBox.Show("Permission Required to View Exchange Rates", "Permission Denied!");
            }
        }
        private void MbtnSharedGroupList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Shared Report Groups Register") != null)
            {
                SharedGroupList sharedGroupList = new SharedGroupList();
                sharedGroupList.Show();
            }
            else
            {
                DXMessageBox.Show("View Shared Report Groups Register", "Permission Denied!");


            }

        }
        private void BtnUploadImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmCreatePopupImage createPopup = new frmCreatePopupImage();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {

                createPopup = new frmCreatePopupImage();
                createPopup.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Create Notifications!");
            }
        }


        private void BtnShowNotiImage_ItemClick(object sender, ItemClickEventArgs e)
        {

            getUpdateNotification();


        }

        private void getUpdateNotification()
        {
            try
            {

                string path = @"";

                BackgroundImages backgroundImages = imagesRepo.getPopupImage();
                if (backgroundImages != null)
                {


                    popUpimageId = backgroundImages.Id;
                    if (!string.IsNullOrEmpty(backgroundImages.Path))
                    {



                        path = backgroundImages.Path;
                        BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                        bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);
                    }

                    else
                        return;
                    var _str = path;
                    int pos = path.LastIndexOf("/") + 1;
                    int posId = path.IndexOf(",");
                    _str = path.Substring(pos, path.Length - pos);
                    string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                    string[] fileEntries = Directory.GetFiles(root);
                    path = path.Substring(pos, path.Length - pos);
                    foreach (string fileName in fileEntries)
                    {
                        int position = fileName.LastIndexOf("\\") + 1;
                        var _filename = fileName.Substring(position, fileName.Length - position);
                        if (_filename == path)
                        {


                            BitmapImage bitmap = new BitmapImage();
                            using (FileStream fs = new FileStream(root + path, FileMode.Open))
                            {


                                bitmap.BeginInit();
                                bitmap.StreamSource = fs;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                frmShowPopupImage popup = new frmShowPopupImage();


                                if (backgroundImages.isUpdate == true)
                                {
                                    popup.btncloseImageNoti.Visibility = Visibility.Visible;

                                    popup.grdMain.Children.Clear();
                                    Image img = new Image();
                                    img.Stretch = Stretch.Fill;
                                    img.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

                                    popup.grdMain.Children.Add(img);
                                    Grid grid = (Grid)popup.grdMain;
                                    var image = bitmap;
                                    var elements = grid.Children;
                                    foreach (UIElement _element in elements)
                                    {
                                        if (_element is Image)
                                        {
                                            var imge = (Image)_element;
                                            imge.Source = image;

                                            popup.ShowDialog();
                                            break;
                                        }
                                    }
                                }
                                else
                                {



                                    var currentVersion = popup._str;
                                    var ReleasedVersion = popup.sub;
                                    if (!string.IsNullOrEmpty(currentVersion) && !string.IsNullOrEmpty(ReleasedVersion))
                                    {
                                        if (currentVersion == ReleasedVersion)
                                        {
                                            //popup.btnUpdate.IsEnabled = false;
                                            return;
                                        }
                                        else
                                        {
                                            popup.txtUpdateAvailable.Visibility = Visibility.Visible;
                                            //popup.txtCurrentVersion.Visibility = Visibility.Visible;
                                            //popup.runningVersion.Visibility = Visibility.Visible;

                                            popup.txtInformation.Visibility = Visibility.Visible;
                                            popup.txtReleasedVersion.Visibility = Visibility.Visible;
                                            popup.releaseVersion.Visibility = Visibility.Visible;

                                            popup.btnUpdate.Visibility = Visibility.Visible;

                                            popup.btncloseVersion.Visibility = Visibility.Visible;
                                            //popup.btnUpdate.IsEnabled = true;
                                            popup.grdMain.Children.Clear();
                                            Image img = new Image();
                                            img.Stretch = Stretch.Fill;
                                            img.Name = "imgDynamic1";
                                            RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);

                                            popup.grdMain.Children.Add(img);
                                            Grid grid = (Grid)popup.grdMain;
                                            var image = bitmap;
                                            var elements = grid.Children;
                                            foreach (UIElement _element in elements)
                                            {
                                                if (_element is Image)
                                                {
                                                    var imge = (Image)_element;
                                                    imge.Source = image;

                                                    popup.ShowDialog();
                                                    break;
                                                }
                                            }
                                        }
                                    }





                                }




                            }
                            break;
                        }
                    }

                }


                return;
            }
            catch (Exception ex)
            {



                MessageBox.Show(ex.Message);
            }
        }



        private void BtnDeletePopupImage_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Upload Background Image") != null)
            {




                var response = MessageBox.Show("Do you really want to delete notifications?", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (response == MessageBoxResult.No)
                {



                }
                else
                {
                    BackgroundImagesRepo repo = new BackgroundImagesRepo();
                    BackgroundImages images = new BackgroundImages();
                    var store = repo.getPopupImage();
                    if (store != null)
                    {
                        repo.DeleteImage(images);
                        MessageBox.Show("Official notification has be deleted successfully!");
                    }
                    else
                    {


                        MessageBox.Show("Their is no official notification detected!");
                    }




                }

            }



            else
            {



                DXMessageBox.Show("You are not allowed to delete notifications!");
            }

        }
        private void SubItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var linkControl = (BarSubItemLinkControl)sender;
            if (linkControl.IsLinkInRadialMenu)
                linkControl.ShowPopup();
        }

        private void BtnConvert_ItemClick(object sender, ItemClickEventArgs e)
        {



        }

        private void MbtnCountryList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();



            ucCountryList ucCountry = new ucCountryList();
            win.Content = ucCountry;
            win.Title = "Country List";
            win.Show();
        }

        private void BarPettyCash_ItemClick(object sender, ItemClickEventArgs e)
        {
            //ucPettyCashList pettyCashList = new ucPettyCashList();

            //Window toDoTasks = new Window();
            //toDoTasks.WindowState = WindowState.Maximized;
            //toDoTasks.Content = pettyCashList;
            //toDoTasks.Title = "Cash Book";
            //toDoTasks.Show();
        }

        private void MbtnActivePettyCash_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = sender as BarButtonItem;

            ucPettyCashList pettyCashList = new ucPettyCashList((string)button.Content);

            Window toDoTasks = new Window();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = pettyCashList;
            toDoTasks.Title = "Cash Book";
            toDoTasks.Show();
        }
       

        private void MbtnInActivePettyCash_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = sender as BarButtonItem;

            ucPettyCashList pettyCashList = new ucPettyCashList((string)button.Content);

            Window toDoTasks = new Window();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = pettyCashList;
            toDoTasks.Title = "Cash Book";
            toDoTasks.Show();
        }

        private void BankList_Click(object sender, ItemClickEventArgs e)
        {
            ucBankList bankList = new ucBankList();
            Window win = new Window();
            win.Content = bankList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnCustomReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZAS_ERP.CustomReportsGroup.ucAddCustomGroup ucAddCustomGroup = new ZAS_ERP.CustomReportsGroup.ucAddCustomGroup();
            Window toDoTasks = new Window();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = ucAddCustomGroup;
            toDoTasks.Title = "Custom Group";
            toDoTasks.Show();

        }

        private void MbtnApplicantType_Click(object sender, ItemClickEventArgs e)
        {
            ucApplicantTypeList applicantList = new ucApplicantTypeList();
            Window win = new Window();
            win.Content = applicantList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }






        private void BgWorker_SystemStatic(object sender, DoWorkEventArgs e)
        {


            if (user != null)
            {
                user.Roles = new List<Role>();
                user.Roles = rolesRepo.getUserRoles(user.id).Distinct().ToList();
                SYSTEM_STATIC.AllowedPermissions = new List<Permission>();
                SYSTEM_STATIC.currentUserRoles = new List<Role>();

                //var depts = SYSTEM_STATIC.currentUser.employee.Companies[1].departments.Where(x=>x.DeptName.StartsWith("PRI")).ToList();
                if (user.Roles != null)
                    foreach (Role role in user.Roles)
                    {
                        SYSTEM_STATIC.currentUserRoles.Add(role);
                        if (role.Permissions != null)
                            SYSTEM_STATIC.AllowedPermissions.AddRange(role.Permissions.Distinct().ToList());
                        else
                            SystemLog.LogInfo(this.GetType(), "Role Name = " + role.Name + " had Permissions = null");
                    }
            }
            SYSTEM_STATIC.isLoadingPermissions = false;
            SYSTEM_STATIC.LoadOutlookEmails();

        }

        private void MbtnRefreshERP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            user = null;
            rolesRepo = new UsersRepo();
            ChartofAccountsRepo chartOfAccountRepo = new ChartofAccountsRepo();
            chartOfAccountRepo.FixNullTransactions();
            grdProgressBar.Visibility = Visibility.Visible;
            if (user == null)
            {
                user = rolesRepo.getuserbyUsername(SYSTEM_STATIC.currentUser.userName);
                if (user == null)
                    MessageBox.Show("Invalid UserName, Please Enter a Valid UserName", "Invalid credentials", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (user != null)
            {
                SYSTEM_STATIC.PopulateTransactionPanel();
                SYSTEM_STATIC.currentUser = user;
                worker.DoWork += BgWorker_SystemStatic;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                if (!worker.IsBusy)
                    worker.RunWorkerAsync();

                //Application.Current.Dispatcher.Invoke((Action)delegate {
                //    LoadPasswordPage();
                //});

            }
            enableMenu();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            grdProgressBar.Visibility = Visibility.Collapsed;
        }
        private void MbtnApplicant_Click(object sender, ItemClickEventArgs e)
        {
            ucApplicantList applicantList = new ucApplicantList();
            Window win = new Window();
            win.Content = applicantList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void MbtnvendorcenterRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Register") != null)
            {
                ucVendorCenterGrid ucVendorCenter = new ucVendorCenterGrid();
                Window win = new Window();
                win.Content = ucVendorCenter;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {

                DXMessageBox.Show("Permission denied!");
            }

        }

        private void barLoansAdvances_ItemClick(object sender, ItemClickEventArgs e)
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
            {
                ucSelectLoansAdvanceType frmLoansAdvances = new ucSelectLoansAdvanceType();
                frmLoansAdvances.advanceTemplate = LoansAdvanceTemplate.Advance;
                Window win = new Window();
                win.Height = 250;
                win.Width = 400;
                win.Content = frmLoansAdvances;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }


        }

        private void MbtnLoansAdvanceStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucLoansAdvanceStatusList statusList = new ucLoansAdvanceStatusList();
            DXWindow win = new DXWindow();

            win.Title = "Loans Advance Status Register";

            win.Content = statusList;
            //ThemeManager.SetThemeName(win, "DeepBlue");

            win.WindowState = WindowState.Maximized;
            win.Show();
        }
        private void mbtnLoansAdvanceRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
            {


                LoansAdvanceRegister loansAdvancesRegister = new LoansAdvanceRegister();
                //Window win = new Window();
                //win.Content = loansAdvancesList;
                loansAdvancesRegister.WindowState = WindowState.Maximized;
                loansAdvancesRegister.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                loansAdvancesRegister.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private async void btnInstantImageChange_ItemClick(object sender, ItemClickEventArgs e)
        {

            try
            {
                await connection.InvokeAsync("SendDesktopBackground");


            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }


        }

        private async void btnInstantUpdateNoti_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("SendUpdateNotification");
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }




        //online click start
        private void btnOnline_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Online Users") != null)
                {
                    if (grdOnlineUserGrid.Visibility == Visibility.Collapsed)
                    {
                        grdOnlineUserGrid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grdOnlineUserGrid.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }


        }
        private void grdOnlineUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                var employee = grdOnlineUsers.GetRowByListIndex(e.ListSourceRowIndex) as User;
                if (e.Column.FieldName == "online" && e.IsGetData)
                {
                    e.Value = "online";
                }
                if (e.Column.FieldName == "EmployeeName" && e.IsGetData)
                {
                    string employeeName = "";

                    if (employee == null)
                    {

                    }
                    if (employee != null)
                    {
                        if (!String.IsNullOrEmpty(employee.employee.person.FName))
                            employeeName = employeeName + employee.employee.person.FName;
                        if (!String.IsNullOrEmpty(employee.employee.person.LName))
                            employeeName = employeeName + " " + employee.employee.person.LName;
                        if (employee.loginUserDetails.Count > 0)
                        {
                            var count = employee.loginUserDetails.Count;
                            var lastItem = employee.loginUserDetails.LastOrDefault();
                            employeeName = employeeName + System.Environment.NewLine + lastItem.LoginTime.Value.ToString("h:mm tt");
                        }
                        e.Value = employeeName;
                    }
                }
                if (e.Column.FieldName == "LastLoginTime" && e.IsGetData)
                {
                    if (employee != null)
                    {
                        if (employee.loginUserDetails.Count > 2)
                        {
                            var item = employee.loginUserDetails[employee.loginUserDetails.Count - 2];
                            e.Value = item.LoginTime.Value.ToString("h:mm tt");
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void onlineUserTableView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View User History") != null)
            {
                rolesRepo = new UsersRepo();
                var selectedRow = (grdOnlineUsers.SelectedItem as User);
                if (selectedRow != null)
                {
                    ucLoginUserDetail ucLoginUser = new ucLoginUserDetail();
                    ucLoginUser.login = rolesRepo.getlogUserDetails(selectedRow.id);
                    ucLoginUser.addCategoryWindow.WindowState = WindowState.Maximized;
                    ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucLoginUser.addCategoryWindow.Content = ucLoginUser;
                    ucLoginUser.addCategoryWindow.ShowDialog();
                }
            }
        }
        private void btnViewHistory_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View User History") != null)
            {
                rolesRepo = new UsersRepo();
                var selectedRow = (grdOnlineUsers.SelectedItem as User);
                if (selectedRow != null)
                {
                    ucLoginUserDetail ucLoginUser = new ucLoginUserDetail();
                    ucLoginUser.login = rolesRepo.getlogUserDetails(selectedRow.id);
                    ucLoginUser.addCategoryWindow.WindowState = WindowState.Maximized;
                    ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucLoginUser.addCategoryWindow.Content = ucLoginUser;
                    ucLoginUser.addCategoryWindow.ShowDialog();
                }
            }
        }
        private void btnUploadedBy_ItemClick(object sender, ItemClickEventArgs e)
        {

            BackgroundImagesRepo repo = new BackgroundImagesRepo();
            ucBackgroundImageUploadedDetails ucLoginUser = new ucBackgroundImageUploadedDetails();
            ucLoginUser.images = repo.getAllUploadedDetails();

            if (ucLoginUser.images.Count != 0)
            {
                ucLoginUser.addCategoryWindow.Width = 1000;
                ucLoginUser.addCategoryWindow.Height = 500;
                ucLoginUser.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucLoginUser.addCategoryWindow.Content = ucLoginUser;
                ucLoginUser.addCategoryWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please upload First");
            }
        }

        private void barEmploymentSalary_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employment Salary") != null)
            {
                ucEmploymentSalaryAdd employmentSalaryAdd = new ucEmploymentSalaryAdd();
                employmentSalaryAdd.employmentSalaryId = 0;
                employmentSalaryAdd.editFlag = false;
                Window win = new Window();
                win.Title = "Employment Salary";
                win.Height = 500;
                win.Width = 900;
                win.Content = employmentSalaryAdd;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Employment Salary Contract!");
            }
        }
        private void mbtnEmploymentSalaryRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Employment Salary") != null)
            {
                ucEmploymentSalaryRegister employmentSalaryRegister = new ucEmploymentSalaryRegister();

                Window win = new Window();
                win.Title = "Employment Salary Register";
                win.Content = employmentSalaryRegister;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Employment Salary!");
            }
        }
        private void btnGroups_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucFrmBackgroundGroupList ucVendorCenter = new ucFrmBackgroundGroupList();
            Window win = new Window();
            win.Content = ucVendorCenter;
            win.WindowState = WindowState.Maximized;
            win.Title = "Background Images Groups";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void BarTasks_ItemClick(object sender, ItemClickEventArgs e)
        {


        }
        private void BarTaskss_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of User Tasks") != null)
            {
                FavouriteItems.Children.Clear();
                ucTaskGrid grdTasks = new ucTaskGrid();
                grdTasks.myParent = this;
                FavouriteItems.Children.Add(grdTasks);
            }
            else
            {
                DXMessageBox.Show("Permission Required to View List of User Tasks!");
            }
        }
        public void getSpecificBackground()
        {
            try
            {
                string path = @"";
                BackgroundImagesRepo imagesRepo = new BackgroundImagesRepo();
                List<ERP_BL.BackgroundImages.BackgroundImages> store = imagesRepo.getSpecificImage();
                if (store == null || store.Count == 0)
                {
                    DXMessageBox.Show("No Specific Image is Uploaded For You");
                    return;
                }
                List<string> listOfEmp = new List<string>();
                int sizeOfList = store.Count - 1;
                for (int i = sizeOfList; i >= 0; i--)
                {
                    var _store = store[i];
                    listOfEmp = new List<string>(_store.EmployeeIds.Split(new string[] { "," }, StringSplitOptions.None));
                    if (listOfEmp.Contains(SYSTEM_STATIC.currentUser.employeeId.ToString()))
                    {


                        if (!string.IsNullOrEmpty(_store.Path))
                        {


                            path = _store.Path;
                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                            var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);
                        }
                        else
                            return;
                        var _str = path;
                        int pos = path.LastIndexOf("/") + 1;
                        int posId = path.IndexOf(",");
                        _str = path.Substring(pos, path.Length - pos);
                        string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                        string[] fileEntries = Directory.GetFiles(root);
                        path = path.Substring(pos, path.Length - pos);
                        foreach (string fileName in fileEntries)
                        {
                            int position = fileName.LastIndexOf("\\") + 1;
                            var _filename = fileName.Substring(position, fileName.Length - position);
                            if (_filename == path)
                            {


                                BitmapImage bitmap = new BitmapImage();
                                using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                {


                                    bitmap.BeginInit();
                                    bitmap.StreamSource = fs;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();
                                    FavouriteItems.Children.Clear();
                                    Image img = new Image();
                                    img.Stretch = Stretch.Fill;
                                    img.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                    FavouriteItems.Children.Add(img);

                                    Grid grid = (Grid)FavouriteItems;
                                    var image = bitmap;
                                    var elements = grid.Children;
                                    foreach (UIElement _element in elements)
                                    {


                                        if (_element is Image)
                                        {


                                            var imge = (Image)_element;
                                            imge.Source = image;
                                            break;
                                        }
                                    }
                                }
                                break;
                            }


                        }
                        break;
                    }
                }
            }
            catch
            {

            }
        }
        public void getShareAllBackground()
        {
            try
            {
                string path = @"";
                BackgroundImagesRepo imagesRepo = new BackgroundImagesRepo();

                ERP_BL.BackgroundImages.BackgroundImages storeShareAll = imagesRepo.getShareAllImage();
                if (storeShareAll != null && storeShareAll.EmployeeIds == null)
                {
                    if (!string.IsNullOrEmpty(storeShareAll.Path))
                    {
                        path = storeShareAll.Path;
                        BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                        var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);
                    }
                    else
                        return;

                    var _str = path;

                    int pos = path.LastIndexOf("/") + 1;
                    int posId = path.IndexOf(",");
                    _str = path.Substring(pos, path.Length - pos);

                    string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                    string[] fileEntries = Directory.GetFiles(root);
                    path = path.Substring(pos, path.Length - pos);
                    foreach (string fileName in fileEntries)
                    {
                        int position = fileName.LastIndexOf("\\") + 1;
                        var _filename = fileName.Substring(position, fileName.Length - position);
                        if (_filename == path)
                        {
                            BitmapImage bitmap = new BitmapImage();
                            using (FileStream fs = new FileStream(root + path, FileMode.Open))
                            {
                                bitmap.BeginInit();
                                bitmap.StreamSource = fs;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();

                                FavouriteItems.Children.Clear();
                                Image img = new Image();
                                img.Stretch = Stretch.Fill;
                                img.Name = "imgDynamic1";
                                RenderOptions.SetBitmapScalingMode(img, BitmapScalingMode.HighQuality);
                                FavouriteItems.Children.Add(img);

                                Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                                var image = bitmap;
                                var elements = grid.Children;

                                foreach (UIElement _element in elements)
                                {
                                    if (_element is Image)
                                    {
                                        var imge = (Image)_element;
                                        imge.Source = image;
                                        break;
                                    }

                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void btnClaimDiscount_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Claim/Discount Lists") != null)
            {
                winClaimList winClaimList = new winClaimList();
                winClaimList.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View View Claim/Discount Lists", "Information");
            }
        }
        private void btnFOCSamlping_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View FOC/Sampling Lists") != null)
            {
                winFOCSamplingList winFOCSamplingList = new winFOCSamplingList();
                winFOCSamplingList.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View FOC/Sampling Lists", "Information");
            }
        }
        private void btnPassOn_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View PassOn Lists") != null)
            {
                winPassOnList winPassOnList = new winPassOnList();
                winPassOnList.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View PassOn Lists", "Information");
            }
        }

        private void blinkButton(Button menu, int duration, double repetition)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,


                To = 0.2,
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                AutoReverse = true,


                RepeatBehavior = new RepeatBehavior(repetition)
            };
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(opacityAnimation);
            Storyboard.SetTarget(opacityAnimation, menu);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));


            storyboard.Begin(menu);


        }


        private void mbtnGroupWallpaper_Click(object sender, RoutedEventArgs e)
        {
            mbtnBackgroundImage.Foreground = Brushes.Black;
            // mbtnBackgroundImage.Background = Brushes.Transparent;
            // mbtnBackgroundImage.BorderThickness = new Thickness(0, 0, 0, 0);

            if (SYSTEM_STATIC.currentUser.isBlink == true)
            {
                UsersRepo rolesRepo = new UsersRepo();
                rolesRepo.updateTaskGroupUser(SYSTEM_STATIC.currentUser);
                //rolesRepo.updateTaskGroupUserId(SYSTEM_STATIC.currentUser.id);
            }


            try
            {
                string path = @"";
                BackgroundImagesRepo repo = new BackgroundImagesRepo();
                var user = SYSTEM_STATIC.currentUser.id;
                var groupImage = repo.GetUserTaskGroup(user);
                if (groupImage != null)
                {
                    if (groupImage.Count != 0)
                    {
                        var lastImage = groupImage.Last();
                        if (!string.IsNullOrEmpty(lastImage.Path))
                        {
                            path = lastImage.Path;
                            BackgroundImagesRepo bgImages = new BackgroundImagesRepo(path);
                            var result = bgImages.startDownloadBackgroundImage(path, TransactionItemType.BackgroundImages);
                        }

                        else
                            return;

                        var _str = path;

                        int pos = path.LastIndexOf("/") + 1;
                        int posId = path.IndexOf(",");
                        _str = path.Substring(pos, path.Length - pos);

                        string root = @"ErpBackground\\BackgroundImages\\Downloaded\\";
                        string[] fileEntries = Directory.GetFiles(root);
                        path = path.Substring(pos, path.Length - pos);
                        foreach (string fileName in fileEntries)
                        {
                            int position = fileName.LastIndexOf("\\") + 1;
                            var _filename = fileName.Substring(position, fileName.Length - position);
                            if (_filename == path)
                            {


                                BitmapImage bitmap = new BitmapImage();
                                using (FileStream fs = new FileStream(root + path, FileMode.Open))
                                {
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = fs;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();

                                    FavouriteItems.Children.Clear();
                                    Image img2 = new Image();
                                    img2.Stretch = Stretch.Fill;
                                    img2.Name = "imgDynamic1";
                                    RenderOptions.SetBitmapScalingMode(img2, BitmapScalingMode.HighQuality);
                                    FavouriteItems.Children.Add(img2);

                                    Grid grid = (Grid)FavouriteItems; //get grid and grid name is favouriteitem
                                    var image = bitmap;
                                    var elements = grid.Children;

                                    foreach (UIElement _element in elements)
                                    {
                                        if (_element is Image)
                                        {
                                            var imge = (Image)_element;
                                            imge.Source = image;
                                            break;
                                        }

                                    }
                                }
                                break;
                            }
                        }
                    }
                    else DXMessageBox.Show("No Background wallpaper is uploaded for you!");
                }
                else DXMessageBox.Show("You are not part of any group!");



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void mbtnSpecificWallpaper(object sender, RoutedEventArgs e)
        {
            getSpecificBackground();
        }

        private void mbtnAllWallpaper(object sender, RoutedEventArgs e)
        {
            getShareAllBackground(); //comment added 
        }

        private void btnLoadAdvance_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
            {
                ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                Window win = new Window();
                win.Content = frmLoansAdvances;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }

        }
        private void btnUniqueNumber_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Unique Number Lists") != null)
            {
                winUniqueNumberList winUniqueNumberList = new winUniqueNumberList();
                winUniqueNumberList.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View Unique Number Lists", "Information");
            }

        }
        private async void btnInstantGroupWallpaper_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                currentOnlieUserId = SYSTEM_STATIC.currentUser.id;
                unicodeConnected = "\u0042\u004c\u0049\u004e\u004b\u004f\u004e";
                await connection.InvokeAsync("SendMessage", unicodeConnected, currentOnlieUserId);
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        public void sendInstantNotification()
        {


            Application.Current.Dispatcher.Invoke((Action)delegate {
                try
                {
                    ERP_BL.PopupNotificatios.popupNotificatinRepo PopupRepo = new ERP_BL.PopupNotificatios.popupNotificatinRepo();
                    var popup = PopupRepo.getShareAllPopup();
                    //if (popup != null)
                    //    PopUpId = popup.Id;

                    if (popup != null)
                    {

                        frmShowPopup showPopup = new frmShowPopup();
                        showPopup.notificationLabel.Text = popup.popupText;
                        showPopup.notificationLabel.FontSize = popup.FontSize;
                        showPopup.notificationTitle.FontSize = popup.FontSizeHeading;




                        if (popup.fontWeightHeading == "ExtraBold")
                            showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(800);
                        else


                            showPopup.notificationTitle.FontWeight = FontWeight.FromOpenTypeWeight(100);

                        if (popup.fontWeight == "ExtraBold")
                            showPopup.notificationLabel.FontWeight = FontWeight.FromOpenTypeWeight(800);




                        if (popup.Italic == "Italic")
                            showPopup.notificationLabel.FontStyle = FontStyles.Italic;
                        else
                            showPopup.notificationLabel.FontStyle = FontStyles.Normal;





                        if (popup.ItalicHeading == "Italic")
                            showPopup.notificationTitle.FontStyle = FontStyles.Italic;
                        else
                            showPopup.notificationTitle.FontStyle = FontStyles.Normal;

                        showPopup.notificationTitle.Text = popup.Title;



                        //showPopup.notificationLabel.FontWeight = System.Windows.FontWeight.FromOpenTypeWeight(int.Parse(popup.fontWeight));
                        //showPopup.notificationTitle.Text = popup.Title;




                        if (popup.TitleColorCode != null && popup.TextColorCode != null)
                        {
                            Color color = (Color)ColorConverter.ConvertFromString(popup.TitleColorCode);
                            showPopup.notificationTitle.Foreground = new System.Windows.Media.SolidColorBrush(color);
                            Color color1 = (Color)ColorConverter.ConvertFromString(popup.TextColorCode);
                            showPopup.notificationLabel.Foreground = new System.Windows.Media.SolidColorBrush(color1);
                        }

                        showPopup.ShowDialog();
                    }

                }
                catch (Exception ex)
                {
                    DXMessageBox.Show(this, ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            });


        }

        private void mbtnBudgetStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget Status") != null)
            {
                Procurementss.Budget.frmBudgetStatusList statusList = new Procurementss.Budget.frmBudgetStatusList();
                statusList.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Edit Budget Status", "Information");

            }
        }

        private void btnBudget_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget") != null)
            {
                frmBudgetAdd budget = new frmBudgetAdd();
                budget.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Add Budget", "Information");
            }
        }
        private void mbtnBudgetCoshSheetFields_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBudgetCostSheetFieldList frmCostSheetFieldList = new frmBudgetCostSheetFieldList();
            frmCostSheetFieldList.ShowDialog();
        }
        private void MbtnEfficiencyPointsList_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Window deduction_win = new Window();
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Deductions") != null)
                {
                    deduction_win = new Window();
                    ucEfficiencyPointsList obj = new ucEfficiencyPointsList();
                    deduction_win.Height = 450;
                    deduction_win.Width = 500;
                    deduction_win.ResizeMode = ResizeMode.CanMinimize;
                    deduction_win.Content = obj;
                    deduction_win.Title = "Efficiency Points List";
                    deduction_win.Show();
                    deductionListWindowFlag = true;
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required List of Budget!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void mbtnPrincipal_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Principal Center") != null)
            {
                ucPrincipalRegister CustomerCenterGrid = new ucPrincipalRegister();
                Window win = new Window();
                win.Content = CustomerCenterGrid;
                win.Title = "Principal Center";
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.WindowState = WindowState.Maximized;
                win.Show();
            }

            else
            {

                DXMessageBox.Show("Permission Denied!");
            }
        }

        private void BarButtonItem_ItemClick_3(object sender, ItemClickEventArgs e)
        {

        }

        private void btnOpernBudgets_ItemClick(object sender, ItemClickEventArgs e)
        {





            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Budget") != null)
            {
                winBudgetRegister register = new winBudgetRegister();
                register.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required List of Budget!");
            }
        }

        private void btnCloseBudgets_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void MbtnTaskType_Click(object sender, ItemClickEventArgs e)
        {
            ucTaskTypeList taskTypeList = new ucTaskTypeList();
            Window win = new Window();
            win.Content = taskTypeList;
            win.Show();
        }
        private void mbtnReportCustomization_ItemClick(object sender, ItemClickEventArgs e)
        {
            winCustomiseReport win = new winCustomiseReport();
            win.ShowDialog();
        }

        private void MbtnTargetGroup_Click(object sender, ItemClickEventArgs e)
        {
            ucTargetGroupList taskGroupList = new ucTargetGroupList();
            Window win = new Window();
            win.Title = "Group Level List";
            win.Content = taskGroupList;
            win.Show();
        }
        private void mbtnPerformancesheetFields_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of PerformanceSheet heads") != null)
            {
                winPerformanceSheetFieldList list = new winPerformanceSheetFieldList();
                list.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("You are not allowed to see list of Performance sheet heads.");
            }
        }

        private void BtnGlow_Click(object sender, RoutedEventArgs e)
        {
            FavouriteItems.Children.Clear();



            grdNotifications = new ucCommentsGrid();
            grdNotifications.urgentNotification = true;
            grdNotifications.myParent = this;
            FavouriteItems.Children.Add(grdNotifications);
        }

        private void MbtnCalculationType_Click(object sender, ItemClickEventArgs e)
        {
            ucStatusCalculationTypeList taskTypeList = new ucStatusCalculationTypeList();
            Window win = new Window();
            win.Content = taskTypeList;
            win.Show();
        }

        private void BarRewards_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Target Rewards") != null)
            {
                ucTargetRewardsRegister ucTargetRewards = new ucTargetRewardsRegister();
                DXWindow win = new DXWindow();
                win.WindowState = WindowState.Maximized;
                win.Title = "Target Rewards";
                win.Content = ucTargetRewards;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of Target Rewards!");
            }
        }

        private void MbtnChangeLogs_Click(object sender, RoutedEventArgs e)
        {
            if (changeLogsPanel.Visibility == Visibility.Visible)
                changeLogsPanel.Visibility = Visibility.Collapsed;
            else
            {
                changeLogsPanel.Visibility = Visibility.Visible;
                ERP_BL.Attach attachment = new ERP_BL.Attach();
                var str = attachment.GetChangeLogs();
                //txtChangeLogs.Text = str;
                grdChangeLogs.ItemsSource = str;
            }


            //DXMessageBox.Show(str);
        }

        private void mbtnRewardStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Target Reward Statuses") != null)
            {
                Window win = new Window();

                ucRewardStatusList statusList = new ucRewardStatusList();
                win.Content = statusList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View List of Target Reward Statuses", "Information");

            }
        }

        private void mbtnSTLStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucSTLSTatusList statusList = new ucSTLSTatusList();
            DXWindow win = new DXWindow();
            win.Title = "STL Status Register";
            win.Content = statusList;

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnSTLRegister_Click(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of STL") != null)
            {
                winSTLRegister moduleWin = new winSTLRegister();
                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;


                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View List of STL!");
            }
        }

        private void BarButtonItem_ItemClick_4(object sender, ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var button = (DevExpress.Xpf.Bars.BarButtonItem)sender;
            LoadTransactionGrid(button.Content.ToString());
            e.Handled = true;
        }

        private void btnCloseSTL_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var button = (DevExpress.Xpf.Bars.BarButtonItem)sender;
            LoadTransactionGrid(button.Content.ToString());
            //e.Handled = true;
        }

        private void btnOpenSTL_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var button = (DevExpress.Xpf.Bars.BarButtonItem)sender;
            LoadTransactionGrid(button.Content.ToString());
            e.Handled = true;
        }

        private void btnSTL_ItemClick(object sender, ItemClickEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            var button = (DevExpress.Xpf.Bars.BarButtonItem)sender;
            LoadTransactionGrid(button.Content.ToString());
            e.Handled = true;
        }


        private void MbtnCountryList_ItemClick_1(object sender, ItemClickEventArgs e)
        {

        }

        private void BarTravelRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTravelingRecordList frmTravelingRecord = new ucTravelingRecordList();

            DXWindow win = new DXWindow();
            win.Content = frmTravelingRecord;
            win.WindowState = WindowState.Maximized;
            win.Title = "Travelling Record";
            win.Show();
        }

        private void mbtnTravelingStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Traveling Statuses") != null)
            {
                Window win = new Window();
                ucTravelingStatusList statusList = new ucTravelingStatusList();
                win.Content = statusList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("You don't have permission to View List of Traveling Statuses", "Information");

            }
        }

        private void MbtnTravelerList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();


            ucTravelerList ucTraveler = new ucTravelerList();
            win.Content = ucTraveler;
            win.Title = "Traveler List";
            win.Show();
        }

        private void MbtnTraveling_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record") != null)
            {
                ucFrmTravelingRecord frmTraveler = new ucFrmTravelingRecord();
                Window win = new Window();
                win.Content = frmTraveler;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void MbtnAirlineList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();


            ucAirlineList ucAirline = new ucAirlineList();
            win.Content = ucAirline;
            win.Title = "Airline List";
            win.Show();
        }

        private void mbtnGridReportCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (reportCenter == null || !reportCenter.IsVisible)
            {
                reportCenter = new winGridReportCenter();
                reportCenter.Show();
            }
            else
            {
                if (reportCenter.WindowState == WindowState.Minimized)
                {
                    reportCenter.WindowState = WindowState.Normal;
                }
                reportCenter.Activate();
            }
        }

        private void mbtnGridMemorizedReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (memorizedReportCenter == null || !memorizedReportCenter.IsVisible)
            {
                memorizedReportCenter = new winGridReportCenter();
                memorizedReportCenter.Title = "Memorized Reports";
                memorizedReportCenter.Show();
            }
            else
            {
                if (memorizedReportCenter.WindowState == WindowState.Minimized)
                {
                    memorizedReportCenter.WindowState = WindowState.Normal;
                }
                memorizedReportCenter.Activate();
            }
            memorizedReportCenter.tabStandard.Visibility = Visibility.Collapsed;
        }
        private void mbtnGridStandardReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (standardReportCenter == null || !standardReportCenter.IsVisible)
            {
                standardReportCenter = new winGridReportCenter();
                standardReportCenter.Title = "Standard Reports";
                standardReportCenter.Show();
            }
            else
            {
                if (standardReportCenter.WindowState == WindowState.Minimized)
                {
                    standardReportCenter.WindowState = WindowState.Normal;
                }
                standardReportCenter.Activate();
            }
            standardReportCenter.tabMemorized.Visibility = Visibility.Collapsed;
        }
        private void mbtnInterestList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();
            ucInterestList interestList = new ucInterestList();
            win.Content = interestList;
            win.Title = "Interest List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void mbtnInterestTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();
            ucInterestTypeList interestTypeList = new ucInterestTypeList();
            win.Content = interestTypeList;
            win.Title = "Interest Type List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Height = 600;
            win.Width = 450;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }



        private void mbtnMarginPercTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();
            ucCashMargintypeList interestTypeList = new ucCashMargintypeList();
            win.Content = interestTypeList;
            win.Title = "Cash Margin Perc. Type List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Height = 600;
            win.Width = 450;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void mbtnMarginPercList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();
            ucCashMarginList interestList = new ucCashMarginList();
            win.Content = interestList;
            win.Title = "Cash Margin Perc. List";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnPollings_Click(object sender, RoutedEventArgs e)
        {
            if (grdPollings.Visibility == Visibility.Visible)
                grdPollings.Visibility = Visibility.Collapsed;
            else
            {
                if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "View List of Pollings") != null)
                {
                    grdPollings.Visibility = Visibility.Visible;
                    PollRepo pollRepo = new PollRepo();
                    grdCntrlPollings.ItemsSource = pollRepo.GetAllPolls(SYSTEM_STATIC.currentUser.id).Where(x => x.ValidUntil >= DateTime.Now).ToList();
                }
                else
                {
                    DXMessageBox.Show("Permission required to View List of Pollings!");
                }


            }
        }

        private void GrdCntrlPollings_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlPollings.GetRowByListIndex(e.ListSourceRowIndex) as Poll;

            switch (e.Column.FieldName)
            {
                case "UsersInFavor":
                    e.Value = row.usersInFavor?.Count();
                    break;

                case "UsersNotInFavor":
                    e.Value = row.usersNotInFavor?.Count();
                    break;
            }
        }

        private void MbtnViewPoll_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdCntrlPollings.SelectedItem as Poll;

            if (selectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Edit Poll") != null)
                {
                    ucFrmCreatePoll frmCreatePoll = new ucFrmCreatePoll();
                    frmCreatePoll.pollId = selectedItem.Id;
                    frmCreatePoll.EditFlag = true;
                    Window window = new Window();
                    window.Width = 400;
                    window.Height = 480;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ResizeMode = ResizeMode.CanMinimize;
                    window.Content = frmCreatePoll;
                    window.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Create New Poll!");
                }
            }
        }

        private void MbtnCreateNewPoll_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Create New Poll") != null)
            {
                ucFrmCreatePoll frmCreatePoll = new ucFrmCreatePoll();
                frmCreatePoll.pollId = 0;
                frmCreatePoll.EditFlag = false;
                Window window = new Window();
                window.Width = 400;
                window.Height = 480;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ResizeMode = ResizeMode.CanMinimize;
                window.Content = frmCreatePoll;
                window.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Permission required to Create New Poll!");
            }
        }

        Poll poll = new Poll();
        private void GrdCntrlPollings_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var selectedItem = grdCntrlPollings.SelectedItem;


            if (selectedItem != null)
            {
                poll = selectedItem as Poll;
                var date = DateTime.Now;


                if (date.Date > poll.ValidUntil.Value.Date)
                    grdVoteButtons.IsEnabled = false;
                else
                    grdVoteButtons.IsEnabled = true;

                poll = selectedItem as Poll;

                if (poll.initiatedBy != null)
                {
                    if (poll.initiatedBy.employee != null)
                    {
                        if (poll.initiatedBy.employee.person != null)
                        {
                            var byteImg = poll.initiatedBy.employee.person.Photo;
                            if (byteImg != null)
                            {
                                var image = GetBitmapImageFromByteArray(byteImg);
                                imgInitiatedBy.ImageSource = image;
                            }

                            txtInitiatedBy.Text = poll.initiatedBy.employee.person.FName + " " + poll.initiatedBy.employee.person.LName;
                        }
                    }
                    if (poll.initiatedBy.employee != null)
                    {
                        txtDesignation.Text = poll.initiatedBy.employee.DesignationTitle;
                    }

                }

                datValidUntil.EditValue = poll.ValidUntil.Value;
                txtPollTitle.Text = poll.Title;

                if (poll.usersInFavor != null)
                    txtAgreedUsersCount.Text = poll.usersInFavor.Count().ToString();

                if (poll.usersNotInFavor != null)
                    txtDisagreedUsersCount.Text = poll.usersNotInFavor.Count().ToString();


                if (poll.usersInFavor != null && poll.usersNotInFavor != null)
                {
                    if (poll.taskGroup != null && poll.taskGroup.usersBulk != null && poll.taskGroup.usersBulk.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.usersBulk.Count() - (poll.usersInFavor.Count() + poll.usersNotInFavor.Count())).ToString();
                    else if (poll.taskGroup != null && poll.taskGroup.users != null && poll.taskGroup.users.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.users.Count() - (poll.usersInFavor.Count() + poll.usersNotInFavor.Count())).ToString();
                }
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

        private void BtnAgree_Click(object sender, RoutedEventArgs e)
        {
            if (poll != null && poll.Id > 0)
            {
                if (DXMessageBox.Show("Do you really want to Poll in Favor?", "Polling", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    PollRepo pollRepo = new PollRepo();
                    poll = pollRepo.GetPoll(poll.Id);

                    if (poll.usersInFavor == null)
                        poll.usersInFavor = new List<User>();


                    if (poll.usersInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) != null)
                    {
                        DXMessageBox.Show("You already Voted in Favor!");
                        return;
                    }
                    if (poll.usersNotInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) != null)
                    {
                        if (DXMessageBox.Show("You voted against! Do you want to change your opinion?", "Polling", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            poll.usersNotInFavor.Remove(poll.usersNotInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id));
                        else
                            return;
                    }

                    poll.usersInFavor.Add(pollRepo.GetUser(SYSTEM_STATIC.currentUser.id));
                    pollRepo.UpdatePoll(poll);
                }
                if (poll.usersInFavor != null)
                    txtAgreedUsersCount.Text = poll.usersInFavor.Count().ToString();
                if (poll.usersNotInFavor != null)
                    txtDisagreedUsersCount.Text = poll.usersNotInFavor.Count().ToString();

                if (poll.usersInFavor != null && poll.usersNotInFavor != null)
                {
                    if (poll.taskGroup != null && poll.taskGroup.usersBulk != null && poll.taskGroup.usersBulk.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.usersBulk.Count() - poll.usersInFavor.Count() + poll.usersNotInFavor.Count()).ToString();
                    else if (poll.taskGroup != null && poll.taskGroup.users != null && poll.taskGroup.users.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.users.Count() - poll.usersInFavor.Count() + poll.usersNotInFavor.Count()).ToString();
                }
            }
        }

        private void BtnDisagree_Click(object sender, RoutedEventArgs e)
        {
            if (poll != null && poll.Id > 0)
            {
                if (DXMessageBox.Show("Do you really want to Poll against?", "Polling", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    PollRepo pollRepo = new PollRepo();
                    poll = pollRepo.GetPoll(poll.Id);

                    if (poll.usersNotInFavor == null)
                        poll.usersNotInFavor = new List<User>();

                    if (poll.usersNotInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) != null)
                    {
                        DXMessageBox.Show("You already Voted against!");
                        return;
                    }
                    if (poll.usersInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) != null)
                    {
                        if (DXMessageBox.Show("You voted in Favor! Do you want to change your opinion?", "Polling", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            poll.usersInFavor.Remove(poll.usersInFavor.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id));
                        else
                            return;
                    }

                    poll.usersNotInFavor.Add(pollRepo.GetUser(SYSTEM_STATIC.currentUser.id));
                    pollRepo.UpdatePoll(poll);
                }


                if (poll.usersInFavor != null)
                    txtAgreedUsersCount.Text = poll.usersInFavor.Count().ToString();
                if (poll.usersNotInFavor != null)
                    txtDisagreedUsersCount.Text = poll.usersNotInFavor.Count().ToString();

                if (poll.usersInFavor != null && poll.usersNotInFavor != null)
                {
                    if (poll.taskGroup != null && poll.taskGroup.usersBulk != null && poll.taskGroup.usersBulk.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.usersBulk.Count() - poll.usersInFavor.Count() + poll.usersNotInFavor.Count()).ToString();
                    else if (poll.taskGroup != null && poll.taskGroup.users != null && poll.taskGroup.users.Count() > 0)
                        txtNeutralUsersCount.Text = (poll.taskGroup.users.Count() - poll.usersInFavor.Count() + poll.usersNotInFavor.Count()).ToString();
                }
            }
        }

        private void BtnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            PollRepo pollRepo = new PollRepo();
            grdCntrlPollings.ItemsSource = pollRepo.GetAllPolls(SYSTEM_STATIC.currentUser.id).Where(x => x.ValidUntil >= DateTime.Now).ToList(); ;
        }

        private void BtnAgreedCount_Click(object sender, RoutedEventArgs e)
        {

            if (poll != null && poll.Id > 0)
            {
                if (poll.pollingType == PollingType.Open_Polling)
                {
                    ucUsersVotedList usersVotedList = new ucUsersVotedList();
                    usersVotedList.grdUsers.ItemsSource = poll.usersInFavor;

                    DXWindow window = new DXWindow();
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Height = 600;
                    window.Width = 400;
                    window.Title = "Agreed Users List";
                    window.Content = usersVotedList;
                    window.ShowDialog();
                }

                else if (poll.pollingType == PollingType.Hidden_Polling && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "By Pass Hidden Polling Permissions") != null)
                {
                    ucUsersVotedList usersVotedList = new ucUsersVotedList();
                    usersVotedList.grdUsers.ItemsSource = poll.usersInFavor;

                    DXWindow window = new DXWindow();
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Height = 600;
                    window.Width = 400;
                    window.Title = "Agreed Users List";
                    window.Content = usersVotedList;
                    window.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("It's not an Open Polling or either you don't have permission to View hidden polling details!");
                }


            }
            else
            {
                DXMessageBox.Show("Please select a Poll first!");
            }
        }

        private void BtnNeutralCount_Click(object sender, RoutedEventArgs e)
        {
            if (poll != null && poll.Id > 0)
            {
                if (poll.pollingType == PollingType.Open_Polling)
                {
                    ucUsersVotedList usersVotedList = new ucUsersVotedList();
                    List<User> userss = new List<User>();

                    if (poll.taskGroup != null && poll.taskGroup.usersBulk != null && poll.taskGroup.usersBulk.Count() > 0)
                    {
                        userss = poll.taskGroup.usersBulk.Where(p => !poll.usersInFavor.All(p2 => p2.id != p.id) && !poll.usersNotInFavor.All(p2 => p2.id != p.id)).ToList();
                    }
                    else if (poll.taskGroup != null && poll.taskGroup.users != null && poll.taskGroup.users.Count() > 0)
                    {
                        userss = poll.taskGroup.users.Where(p => poll.usersInFavor.All(p2 => p2.id != p.id) && poll.usersNotInFavor.All(p2 => p2.id != p.id)).ToList();
                    }

                    usersVotedList.grdUsers.ItemsSource = userss;

                    DXWindow window = new DXWindow();
                    window.WindowState = WindowState.Maximized;
                    window.Title = "Agreed Users List";
                    window.Content = usersVotedList;
                    window.ShowDialog();
                }
                else if (poll.pollingType == PollingType.Hidden_Polling && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "By Pass Hidden Polling Permissions") != null)
                {
                    ucUsersVotedList usersVotedList = new ucUsersVotedList();
                    List<User> userss = new List<User>();

                    if (poll.taskGroup != null && poll.taskGroup.usersBulk != null && poll.taskGroup.usersBulk.Count() > 0)
                    {
                        userss = poll.taskGroup.usersBulk.Where(p => !poll.usersInFavor.All(p2 => p2.id != p.id) && !poll.usersNotInFavor.All(p2 => p2.id != p.id)).ToList();
                    }
                    else if (poll.taskGroup != null && poll.taskGroup.users != null && poll.taskGroup.users.Count() > 0)
                    {
                        userss = poll.taskGroup.users.Where(p => poll.usersInFavor.All(p2 => p2.id != p.id) && poll.usersNotInFavor.All(p2 => p2.id != p.id)).ToList();
                    }

                    usersVotedList.grdUsers.ItemsSource = userss;

                    DXWindow window = new DXWindow();
                    window.WindowState = WindowState.Maximized;
                    window.Title = "Agreed Users List";
                    window.Content = usersVotedList;
                    window.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("It's not an Open Polling or either you don't have permission to View hidden polling details!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select a Poll first!");
            }
        }

        private void BtnDisagreedCount_Click(object sender, RoutedEventArgs e)
        {
            if (poll.pollingType == PollingType.Open_Polling)
            {
                ucUsersVotedList usersVotedList = new ucUsersVotedList();
                usersVotedList.grdUsers.ItemsSource = poll.usersNotInFavor;

                DXWindow window = new DXWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Height = 600;
                window.Width = 400;
                window.Title = "Disagreed Users List";
                window.Content = usersVotedList;
                window.ShowDialog();
            }
            else if (poll.pollingType == PollingType.Hidden_Polling && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "By Pass Hidden Polling Permissions") != null)
            {
                ucUsersVotedList usersVotedList = new ucUsersVotedList();
                usersVotedList.grdUsers.ItemsSource = poll.usersNotInFavor;

                DXWindow window = new DXWindow();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Height = 600;
                window.Width = 400;
                window.Title = "Disagreed Users List";
                window.Content = usersVotedList;
                window.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("It's not an Open Polling or either you don't have permission to View hidden polling details!");
            }
        }

        private void BtnActivePollings_Click(object sender, RoutedEventArgs e)
        {

            PollRepo pollRepo = new PollRepo();
            grdCntrlPollings.ItemsSource = pollRepo.GetAllPolls(SYSTEM_STATIC.currentUser.id).Where(x => x.ValidUntil >= DateTime.Now).ToList();

        }

        private void BtnInActivePollings_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Pollings List") != null)
            {
                PollRepo pollRepo = new PollRepo();
                grdCntrlPollings.ItemsSource = pollRepo.GetAllPolls(SYSTEM_STATIC.currentUser.id).Where(x => x.ValidUntil < DateTime.Now).ToList();
            }
            else
            {
                DXMessageBox.Show("Permission required to view list of InActive Pollings!");
            }

        }

        private void BtnAllPollings_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of All Pollings") != null)
            {
                PollRepo pollRepo = new PollRepo();
                grdCntrlPollings.ItemsSource = pollRepo.GetAllPolls(SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                DXMessageBox.Show("Permission required to view list of All Pollings!");
            }
        }

        private void MbtnDeletePoll_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Delete Polling") != null)
            {
                if (DXMessageBox.Show("Do you want to Delete it?", "Delete Poll", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var selectedItem = grdCntrlPollings.SelectedItem as Poll;
                    if (poll != null)
                    {
                        PollRepo pollRepo = new PollRepo();
                        pollRepo.DeletePoll(poll.Id);
                        DXMessageBox.Show("Deleted Successfully!");
                    }
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Delete Poll!");
            }
        }

        private void mbtnInventoryAdjustmentStatusList_ItemClick(object sender, ItemClickEventArgs e)
        {
            winInventoryAdjustmentStatusList statusList = new winInventoryAdjustmentStatusList();
            statusList.ShowDialog();
        }

        private void btnDefaultTheme_Click(object sender, RoutedEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = Theme.DeepBlue.Name;
        }



        private void cbTheme_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });

            switch ((cbTheme.SelectedItem as cmbitem).name)
            {

                case "LightBlue":
                    ApplicationThemeHelper.ApplicationThemeName = Theme.VS2010.Name;
                    break;
                case "Silver":
                    ApplicationThemeHelper.ApplicationThemeName = Theme.Office2010Silver.Name;
                    break;
                case "DeepBlue":
                    ApplicationThemeHelper.ApplicationThemeName = Theme.DeepBlue.Name;
                    break;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnRentedVehicleOwnerList_Click(object sender, ItemClickEventArgs e)
        {
            ucRentedVehicleOwerList vehicleOwerList = new ucRentedVehicleOwerList();
            Window win = new Window();
            win.Content = vehicleOwerList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnMaintenanceHeadList_Click(object sender, ItemClickEventArgs e)
        {
            ucMaintenanceHeadsList ucMaintenanceList = new ucMaintenanceHeadsList();
            Window win = new Window();
            win.Content = ucMaintenanceList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnWarehouseList_Click(object sender, ItemClickEventArgs e)
        {
            ucWarehouseList ucWarehouse = new ucWarehouseList();
            Window win = new Window();
            win.Content = ucWarehouse;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnLotNumberList_Click(object sender, ItemClickEventArgs e)
        {
            ucLotNumberList ucLotNumber = new ucLotNumberList();
            Window win = new Window();
            win.Content = ucLotNumber;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnReportTitleList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Grid Report Title") != null)
            {
                winReportTitleList list = new winReportTitleList();
                list.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to View Grid Report titles!");
            }
        }

        private void MbtnPackingStyleList_Click(object sender, ItemClickEventArgs e)
        {
            ucPackingStyleList ucPackingStyle = new ucPackingStyleList();
            Window win = new Window();
            win.Content = ucPackingStyle;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnStatusClassesStatusList_ItemClick(object sender, ItemClickEventArgs e)
        {
            winStatusClassList statusClass = new winStatusClassList();
            statusClass.Show();
        }

        private void MbtnReceiveNoteList_Click(object sender, ItemClickEventArgs e)
        {
            ucGoodReceiveNoteList ucGoodReceiveNote = new ucGoodReceiveNoteList();
            Window win = new Window();
            win.Content = ucGoodReceiveNote;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
        private void mbtnFavouriteReports_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZAS_ERP.Reportss.FavouriteReports.Windows.winFavouriteReports reports = new ZAS_ERP.Reportss.FavouriteReports.Windows.winFavouriteReports();
            reports.Show();
        }

        private void mbtnCustomerListing_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Customer Center") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Register") != null)
            {
                ucCustomerCenterGrid CustomerCenterGrid = new ucCustomerCenterGrid();
                Window win = new Window();
                win.Content = CustomerCenterGrid;
                win.Title = "Customer Center";
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied!");
            }
        }

        private void mbtnCustomerSaleOrders_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Sale Orders") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Customer Center") != null)
            {
                winCustomerSaleOrders win = new winCustomerSaleOrders();
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied!");
            }
        }

        private void BtnPRITregister_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucPRITregister pRITregister = new ucPRITregister();
            Window win = new Window();
            win.Content = pRITregister;
            win.Title = "PRIT Register";
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnCustomerCreditsReports_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credits Report") != null)
            {
                winSICustomerCredits credit = new winSICustomerCredits();
                credit.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied!");

            }
        }

        private void btnAuditYearAdjustment_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Audit Year Register") != null)
            {
                winAuditAdjustments winAuditAdjustments = new winAuditAdjustments();
                winAuditAdjustments.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied!");
            }
        }

        private void mbtnVendorPurchaseOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Purchase Orders") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Vendor Center") != null)
            {
                winVendorPurchaseOrders win = new winVendorPurchaseOrders();
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied!");
            }
        }

        private void barCompanyLoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
            {
                ucSelectLoansAdvanceType frmLoansAdvances = new ucSelectLoansAdvanceType();
                frmLoansAdvances.advanceTemplate = LoansAdvanceTemplate.Loan;
                Window win = new Window();
                win.Height = 250;
                win.Width = 400;
                win.Content = frmLoansAdvances;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnCompanyLoanRegister_ItemClick(object sender, ItemClickEventArgs e)
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Company Loans") != null)
            {
                ucCompanyLoanList companyLoanList = new ucCompanyLoanList();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = companyLoanList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void MbtnLenderType_Click(object sender, ItemClickEventArgs e)
        {
            ucLenderTypeList applicantList = new ucLenderTypeList();
            Window win = new Window();
            win.Content = applicantList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnLender_Click(object sender, ItemClickEventArgs e)
        {
            ucLenderList applicantList = new ucLenderList();
            Window win = new Window();
            win.Content = applicantList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnEnterAsset_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
            Window win = new Window();
            win.Content = frmAddAssetRental;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnAssetTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetRentalTypeList assetRentalTypeList = new ucAssetRentalTypeList();
            Window win = new Window();
            win.Content = assetRentalTypeList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnTenantList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTenantRentalList tenantRentalList = new ucTenantRentalList();
            Window win = new Window();
            win.Content = tenantRentalList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnRentalContractRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Contracts") != null)
            {
                ucRentalContractList rentalContractList = new ucRentalContractList();
                Window win = new Window();
                win.Content = rentalContractList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnEnterRentalContract_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts") != null)
            {
                ucFrmRentalContractAdd frmRentalContractAdd = new ucFrmRentalContractAdd();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = frmRentalContractAdd;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            inactivityTracker.ResetInactivityTimer();
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            inactivityTracker.ResetInactivityTimer();
        }

        private void mbtnCOAGroups_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Chart of Account Groups") != null)
            {
                winChartofAccountGroupList winChartofAccountGroupList = new winChartofAccountGroupList();
                winChartofAccountGroupList.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void sendWhatsApp(string number, string message)
        {

        }

        private void MbtnRentalContractStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucRentalContractStatusList statusList = new ucRentalContractStatusList();
            DXWindow win = new DXWindow();

            win.Title = "Rental Contract Status Register";

            win.Content = statusList;
            //ThemeManager.SetThemeName(win, "DeepBlue");

            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnAssetRentalStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Asset Status") != null)
            {
                ucAssetRentalStatusList statusList = new ucAssetRentalStatusList();
                DXWindow win = new DXWindow();

                win.Title = "Asset Status Register";

                win.Content = statusList;
                //ThemeManager.SetThemeName(win, "DeepBlue");

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnRentalOrderRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Orders") != null)
            {
                ucRentalOrderList rentalOrderList = new ucRentalOrderList();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = rentalOrderList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnEnterRentalOrder_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void MbtnRentalOrderStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Rental Order Status") != null)
            {
                ucRentalOrderStatusList statusList = new ucRentalOrderStatusList();
                DXWindow win = new DXWindow();

                win.Title = "Rental Order Statuses";

                win.Content = statusList;
                //ThemeManager.SetThemeName(win, "DeepBlue");

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnAsset_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Assets") != null)
            {
                ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = frmAddAssetRental;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnAssetRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Assets") != null)
            {
                ucAssetRentalsList assetRentalsList = new ucAssetRentalsList();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = assetRentalsList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void MbtnTenantRentalStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Tenant Status") != null)
            {
                ucTenantRentalStatusList statusList = new ucTenantRentalStatusList();
                DXWindow win = new DXWindow();

                win.Title = "Tenant Statuses";

                win.Content = statusList;
                //ThemeManager.SetThemeName(win, "DeepBlue");

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnTenantsRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Tenants") != null)
            {
                ucTenantRentalList tenantRentalList = new ucTenantRentalList();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = tenantRentalList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnTenant_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Tenants") != null)
            {
                ucFrmTenantRentalAdd tenantRentalAdd = new ucFrmTenantRentalAdd();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = tenantRentalAdd;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnTravelSummary_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucTravelingSummary frmTravelingRecord = new ucTravelingSummary();

            DXWindow win = new DXWindow();
            win.Content = frmTravelingRecord;
            win.WindowState = WindowState.Maximized;
            win.Title = "Travelling Summary";
            win.Show();
        }

        private void mbtnmoduleContractStatusList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusList statusList = new ZAS_ERP.Procurementss.ModuleContract.frmModuleContractStatusList();
            statusList.ShowDialog();
        }

        private void mbtnDepLevelList_ItemClick(object sender, ItemClickEventArgs e)
        { 
            winDepartmentLevelList list = new winDepartmentLevelList();
            list.Show();
        }

        private void barMemos_ItemClick(object sender, ItemClickEventArgs e)
        {
            
                FavouriteItems.Children.Clear();
                ucMemoGrid memoGrid = new ucMemoGrid();
                memoGrid.myParent = this;
                FavouriteItems.Children.Add(memoGrid);
            
        }

        private void mbtnMemoGroupList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //ucMemoGroupList memoGroupList = new ucMemoGroupList();

            //DXWindow win = new DXWindow();
            //win.Content = memoGroupList;
            //win.WindowState = WindowState.Maximized;
            //win.Title = "Travelling Summary";
            //win.Show();
        }

        private void MbtnActiveVATBook_ItemClick(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = sender as BarButtonItem;

            ZAS_ERP.VATBook.UserControls.ucVatBookList vatBookList = new ZAS_ERP.VATBook.UserControls.ucVatBookList((string)button.Content);

            DXWindow toDoTasks = new DXWindow();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = vatBookList;
            toDoTasks.Title = "VAT Book";
            toDoTasks.Show();
        }
        private void mbtnInActiveVATBook_ItemClick(object sender, ItemClickEventArgs e)
        {

            BarButtonItem button = sender as BarButtonItem;
            ZAS_ERP.VATBook.UserControls.ucVatBookList vatBookList = new ZAS_ERP.VATBook.UserControls.ucVatBookList((string)button.Content);
            DXWindow toDoTasks = new DXWindow();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = vatBookList;
            toDoTasks.Title = "VAT Book";
            toDoTasks.Show();
        }

        private void barVATBook_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void mbtnVATBookReferenceList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucVATBookReferenceList vatBookReferenceList = new ucVATBookReferenceList();
            DXWindow win = new DXWindow();
            win.Content = vatBookReferenceList;
            win.Show();
        }

        private void mbtnActiveVATBook_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            BarButtonItem button = sender as BarButtonItem;

            ZAS_ERP.VATBook.UserControls.ucVatBookList vatBookList = new ZAS_ERP.VATBook.UserControls.ucVatBookList((string)button.Content);

            DXWindow toDoTasks = new DXWindow();
            toDoTasks.WindowState = WindowState.Maximized;
            toDoTasks.Content = vatBookList;
            toDoTasks.Title = "VAT Book";
            toDoTasks.Show();
        }

        private void mbtnAssetNatureList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetNatureList assetNatureList = new ucAssetNatureList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnAssetLocationList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetRentalLocationList assetNatureList = new ucAssetRentalLocationList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void MbtnCityList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();
            ucCityList ucCountry = new ucCityList();
            win.Content = ucCountry;
            win.Title = "Country List";
            win.Show();
        }

        private void barCashFlow_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Cashflow statement") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                winCashFlowCenter cashFlowCenter = new winCashFlowCenter();
                cashFlowCenter.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }
private void mbtnAssetBook_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();



            ucAssetBookList ucAssetBook = new ucAssetBookList();
            win.Content = ucAssetBook;
            win.Title = "Asset Book";
            win.Show();
        }

        private void mbtnAssetUnitList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();



            ucAssetRentalUnitList ucAssetBook = new ucAssetRentalUnitList();
            win.Content = ucAssetBook;
            win.Title = "Asset Unit List";
            win.Show();
        }

        private void mbtnAssetBrandList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetBrandList assetNatureList = new ucAssetBrandList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnAssetModelList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetModelList assetNatureList = new ucAssetModelList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnAssetNumberList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetNumberList assetNatureList = new ucAssetNumberList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnDocumentTypeList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucDocumentTypeList assetRentalTypeList = new ucDocumentTypeList();
            Window win = new Window();
            win.Content = assetRentalTypeList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnDocumentAuthorityList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucDocumentAuthorityList ucDocumentAuthority = new ucDocumentAuthorityList();
            Window win = new Window();
            win.Content = ucDocumentAuthority;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void BarDocumentRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucDocumentList documentList = new ucDocumentList();

            DXWindow win = new DXWindow();
            win.Content = documentList;
            win.WindowState = WindowState.Maximized;
            win.Title = "Document Register";
            win.Show();
        }

        private void mbtnDocument_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document") != null)
            {
                ucFrmDocumentAdd frmDocumentAdd = new ucFrmDocumentAdd();

                DXWindow win = new DXWindow();
                win.Content = frmDocumentAdd;
                win.WindowState = WindowState.Maximized;
                win.Title = "Document";
                win.Show();
            }
        }

        private void MbtnDocumentStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Document Statuses") != null)
            {
                ucDocumentStatusList statusList = new ucDocumentStatusList();
                DXWindow win = new DXWindow();

                win.Title = "Document Statuses";

                win.Content = statusList;
                //ThemeManager.SetThemeName(win, "DeepBlue");

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnDocumentTemplateList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucDocumentTemplateList ucDocumentTemplate = new ucDocumentTemplateList();
            Window win = new Window();
            win.Content = ucDocumentTemplate;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void mbtnAssetSubNatureList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucAssetSubNatureList assetNatureList = new ucAssetSubNatureList();
            Window win = new Window();
            win.Content = assetNatureList;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }

        private void btnRefKeys_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List SO Ref Keys") != null)
            {
                ucSORefKeys keys = new ucSORefKeys();
                DXWindow win = new DXWindow();
                win.Content = keys;
                win.Show();
            }
        }

        private void mbtnEnterRentalInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void mbtnRentalInvoiceRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Invoices") != null)
            {
                ucRentalInvoiceList rentalOrderList = new ucRentalInvoiceList();
                Window win = new Window();
                win.WindowState = WindowState.Maximized;
                win.Content = rentalOrderList;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnRentalInvoiceStatus_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Rental Invoice Status") != null)
            {
                ucRentalInvoiceStatusList statusList = new ucRentalInvoiceStatusList();
                DXWindow win = new DXWindow();

                win.Title = "Rental Invoice Statuses";

                win.Content = statusList;
                //ThemeManager.SetThemeName(win, "DeepBlue");

                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnVendorNature_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmIndustryTypeListManual industryType = new frmIndustryTypeListManual();
            industryType.Owner = this;
            industryType.Show();
        }

        private void mbtnVendorNatureManual_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmVendorNatureManualList industryType = new frmVendorNatureManualList();
            industryType.Owner = this;
            industryType.Show();
        }

        private void mbtnVendorFinder_ItemClick(object sender, ItemClickEventArgs e)
        {
            

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor finder") != null)
            {
                winVendorFinder finder = new winVendorFinder();
                finder.Show();
            }
            else
            {
                DXMessageBox.Show("Permission Denied", "Information");
            }
        }

        private void mbtnEmploymeesDataRetrievalRegister_ItemClick(object sender, ItemClickEventArgs e)
        {
            Window win = new Window();

            ucFrmEmployeeDataRetrievalDateAdd ucFrmEmployeeData = new ucFrmEmployeeDataRetrievalDateAdd();

            win.Content = ucFrmEmployeeData;
            win.Show();
        }

        private void mbtnCOAGroupslist_ItemClick(object sender, ItemClickEventArgs e)
        {
            winChartofAccounts winChartofAccounts = new winChartofAccounts();
            winChartofAccounts.Show();
        }

        private void mbtnPerformanceIndicators_ItemClick(object sender, ItemClickEventArgs e)
        {
            PerformanceIndicatorWindow performanceIndicator = new PerformanceIndicatorWindow();
            performanceIndicator.Show();
         }
    }
}

