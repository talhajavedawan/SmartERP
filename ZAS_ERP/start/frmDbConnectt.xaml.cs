using ERP_BL.Config;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using ERP_BL.ExchangeRates;

namespace ZAS_ERP.start
{
    /// <summary>
    /// Interaction logic for frmDbConnectTest.xaml
    /// </summary>
    public partial class frmDbConnectt : Window

    {

        BackgroundWorker bgWorker = new BackgroundWorker();
        public MachineAPI.MachineAPI machineAPI = new MachineAPI.MachineAPI();
        bool check = false;
        public static string dbname;
        UsersRepo rolesRepo = new UsersRepo();
        User user;
        pageDbConnect pagedbconn = new pageDbConnect();
        pageDbConnectPassword pagedbpassword = new pageDbConnectPassword();
        Config config = new Config();
        bool validate = false;
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        public frmDbConnectt()
        {
            SystemLog.LogInfo(this.GetType(), "Erp Application Started");
            InitializeComponent();

            //**********************************************************************Background Doworker Event ***************************************************************
            this.KeyUp += new KeyEventHandler(OnButtonKeyDown);
            //this.pageFrmServer = frmserver;
            SystemLog.LogInfo(this.GetType(), "Form Intialized");

        }
        public void CheckPowerUser()
        {
            pUser puser = rolesRepo.getPowerUserByUsername(pagedbconn.txtUsername.Text);
            if (puser != null)
            {
                pagedbpassword = new pageDbConnectPassword();
                btnlogindb.Content = "Login";
                SYSTEM_STATIC.LoggedInByPowerUser = true;
                MainWindow.currentUserName = puser.userName;
                SystemLog.CurrentUseruserName = puser.userName;
                Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
                Properties.Settings.Default.Save();
                Properties.Settings.Default["dbname"] = frmDbConnectt.dbname.ToString();
                Properties.Settings.Default.Save();
                SYSTEM_STATIC.AllowedPermissions = new List<Permission>();
                SYSTEM_STATIC.AllowedPermissions = rolesRepo.getAllPermissions();
                SYSTEM_STATIC.LoggedInByPowerUser = true;

                main.Content = pagedbpassword;

                return;
            }
        }
        public void ValidateUserMachine()
        {

        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ValidateLoginUser();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
            }
        }
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (user != null)
            {
                user.Roles = new List<Role>();
                user.Roles = rolesRepo.getUserRoles(user.id).Distinct().ToList();
                SYSTEM_STATIC.AllowedPermissions = new List<Permission>();
                SYSTEM_STATIC.currentUserRoles = new List<Role>();
                SYSTEM_STATIC.exchangeRateGroups = exchangeRateGroupRepo.GetAll();
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
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            pagedbconn = new pageDbConnect();
            main.Content = pagedbconn;
            btnlogindb.Visibility = Visibility.Visible;
        }
        private void btnlogindb_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ValidateLoginUser();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = null;
            });

        }
        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void ValidateLoginUser()
        {

            try
            {
                if (check == false)
                {
                    if (pagedbconn.cmbdbSelect.SelectedIndex != -1)
                    {
                        if (pagedbconn.lblstatusBar.Content.ToString() == "This is Your First Run, Please Add New Company.")
                        {
                            dbname = this.pagedbconn.cmbdbSelect.SelectedText.ToString();
                            frmstartWizard wiz = new frmstartWizard();
                            wiz.Show();
                            SystemLog.LogInfo(this.GetType(), "New Group Company Wizard Started!,This is the first run for the System!");
                            this.Close();
                        }
                        else if (pagedbconn.txtUsername.Text == "")
                        {
                            MessageBox.Show("Please Enter Username");
                            pagedbconn.txtUsername.Focus();
                            return;
                        }
                        else if (pagedbconn.txtUsername.Text != "")
                        {
                            //System.Threading.Thread th = new System.Threading.Thread(() =>
                            //{
                            //    var pusers = new List<pUser>();
                            //    if (System.Diagnostics.Debugger.IsAttached)
                            //    {
                            //        pusers = config.connectDB(pagedbconn.cmbdbSelect.Text);
                            //    }
                            //    else
                            //    {
                            //        pusers = config.connectDB(pagedbconn.cmbdbSelect.Text, Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[0] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[1]);
                            //    }

                            //    CheckPowerUser();
                            //});
                            //th.Start();
                            //th.Join();
                            if (user == null)
                            {
                                user = rolesRepo.getuserbyUsername(pagedbconn.txtUsername.Text);
                                if (user == null)
                                    MessageBox.Show("Invalid UserName, Please Enter a Valid UserName", "Invalid credentials", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            if (user != null)
                            {
                                bgWorker.DoWork += BgWorker_DoWork;
                                if (!bgWorker.IsBusy)
                                    bgWorker.RunWorkerAsync();

                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    LoadPasswordPage();
                                });
                            }
                        }
                    }
                }
                else
                {
                    if (SYSTEM_STATIC.GenerateSHA512String(pagedbpassword.txtpasswordBox.Text) == user.password)
                    {
                        user.userSettings = new List<UserSettings>();
                        user.userSettings = rolesRepo.getUserSettings(user.id).Distinct().ToList();
                    }
                    else
                    {
                        if (user.password != pagedbpassword.txtpasswordBox.Text)
                        {
                            pagedbpassword.txtPasswordMatch.Foreground = new SolidColorBrush(Colors.Red);
                            pagedbpassword.txtPasswordMatch.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                    //************************************************** MachineAPIStart*******************************************************************
                    if (user != null)
                    {
                        if (!System.Diagnostics.Debugger.IsAttached)
                        {
                            string productName = "ZASERP";
                            //if (user.machineKey == null)
                            //{
                            //    var userKey = machineAPI.GenKey(productName);

                            //    if (userKey != null)
                            //    {
                            //        user.machineKey = userKey;
                            //        try
                            //        {
                            //            rolesRepo.updateUserKey(user, userKey);
                            //        }
                            //         catch (Exception ex)
                            //        {
                            //            MessageBox.Show("Error in getting machine key " + ex.Message);
                            //        }

                            //        MessageBox.Show("Please contact admin for system approval.", "Unapproved system", MessageBoxButton.OK, MessageBoxImage.Hand);
                            //    }

                            //}
                            //validate = machineAPI.validatekey(user.machineKey, "ZASERP");
                            //if (user.rdcMachineKey != null)
                            //{

                            //    var validateRDC = machineAPI.validatekey(user.rdcMachineKey, "ZASERP");
                            //    if (validateRDC == true && validate == true)
                            //    {
                            //        if (user.isKeyApproved == false && user.isRDCKeyApproved == false)
                            //        {
                            //            MessageBox.Show("Your access is not approved yet. \nContact admin", "Un-approved Access", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //            return;
                            //        }
                            //    }
                            //}




                            if (validate == false)
                            {
                                MessageBox.Show("You are not allowed to use this system. \nAdmin is being informed for un-authorized access", "Un-Authorized Access", MessageBoxButton.OK, MessageBoxImage.Stop);

                                return;
                            }
                            if(user.isKeyApproved==true)
                            {

                            }
                            else
                            if(user.isRDCKeyApproved==true)
                            {

                            }
                            else if (user.isKeyApproved == false)
                            {
                                MessageBox.Show("Your access is not approved yet. \nContact admin", "Un-approved Access", MessageBoxButton.OK, MessageBoxImage.Stop);
                                return;
                            }
                            if (user.isActive != true)
                            {
                                MessageBox.Show("This user is currently inActive! Please contact your Administrator");
                                return;
                            }
                        }
                        //user.isLoggedIn = true;
                        //this.Dispatcher.Invoke((Action)(() =>
                        //{
                        //    rolesRepo.updateLoginUser(user.id);
                        //}));


                    }
                    else
                    {
                        MessageBox.Show("User name and password doesn't match", "Invalid credentials", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //**************************************************MachineApiEnd*******************************************************************


                    MainWindow mainWindow = new MainWindow();
                    //main.Content = pagedbpassword;
                    Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default["dbname"] = pageDbConnect.dbname.ToString();
                    Properties.Settings.Default.Save();

                    int recheckCount = 1;
                    checkFinishWorker:

                    if (bgWorker.IsBusy && recheckCount <= 3)
                    {
                        recheckCount++;
                        System.Threading.Thread.Sleep(2000);
                        goto checkFinishWorker;
                    }
                    mainWindow.Show();
                    this.Close();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in updating user " + ex.Message);
            }


        }
        public void LoadPasswordPage()
        {
            pagedbpassword = new pageDbConnectPassword();
            pagedbpassword.txtpasswordBox.Focus();
            grdMenu.Visibility = Visibility.Visible;
            pagedbpassword.txtpasswordBox.IsEnabled = false;
            pagedbpassword.lblVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[0] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[1] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[2]; // 3 fractions version
            SYSTEM_STATIC.currentUser = user;
            SystemLog.CurrentUserId = user.id;
            SystemLog.CurrentUseruserName = user.userName;
            SYSTEM_STATIC.LoggedInByPowerUser = false;
            MainWindow.currentUserid = user.id;
            MainWindow.currentUserName = user.userName;
            pagedbpassword.txtUserName.Text = user.employee.person.FName + " " + user.employee.person.LName;
            if (user.employee.person.Photo != null)
                pagedbpassword.ConvertByteToBmp(user.employee.person.Photo);
            main.Content = pagedbpassword;
            btnlogindb.Content = "Login";
            check = true;

        }

        private void barLanAuthentication_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //if (user != null)
            //{

            //    if (string.IsNullOrEmpty(user.machineKey))
            //    {
            //        string productName = "SmartERP";
            //        var userKey = machineAPI.GenKey(productName);

            //        if (userKey != null)
            //        {
            //            user.machineKey = userKey;
            //            try
            //            {
            //                rolesRepo.updateUserKey(user, userKey);
            //            }
            //            catch (Exception ex)
            //            {
            //                MessageBox.Show("Error in getting machine key " + ex.Message);
            //            }

            //            MessageBox.Show("Please contact admin for system approval.", "Unapproved system", MessageBoxButton.OK, MessageBoxImage.Hand);
            //            this.Close();
            //        }

            //    }
            //    else if (user.isKeyApproved != true)
            //    {
            //        MessageBox.Show("Please contact admin for system approval.", "Unapproved system", MessageBoxButton.OK, MessageBoxImage.Hand);
            //        this.Close();
            //    }
            //    else
            //    {
            //        validate = machineAPI.validatekey(user.machineKey, "ZASERP");
            //        grdMenu.Visibility = Visibility.Collapsed;
            //        pagedbpassword.txtpasswordBox.IsEnabled = true;
            //    }

            //}
          pagedbpassword.txtpasswordBox.IsEnabled = true;

        }
        private void barRDCAuthentication_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.rdcMachineKey))
                {

                    string productName = "ZASERP";
                    var userKey = machineAPI.GenKey(productName);

                    if (userKey != null)
                    {
                        user.rdcMachineKey = userKey;
                        try
                        {
                            rolesRepo.updateRDCUserKey(user, userKey);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error in getting machine key " + ex.Message);
                        }

                        MessageBox.Show("Please contact admin for system approval.", "Unapproved system", MessageBoxButton.OK, MessageBoxImage.Hand);
                        this.Close();
                    }

                }
                else if (user.isRDCKeyApproved != true)
                {
                    MessageBox.Show("Please contact admin for system approval.", "Unapproved system", MessageBoxButton.OK, MessageBoxImage.Hand);
                    this.Close();
                }
                else
                {
                    validate = machineAPI.validatekey(user.rdcMachineKey, "ZASERP");
                    grdMenu.Visibility = Visibility.Collapsed;
                    pagedbpassword.txtpasswordBox.IsEnabled = true;
                }
            }
        }
    }

}
