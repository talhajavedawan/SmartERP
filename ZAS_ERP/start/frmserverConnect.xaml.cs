using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ERP_BL.Config;
using ERP_BL.Databases;
using RestSharp;

namespace ZAS_ERP.start

{
    
    /// <summary>
    /// Interaction logic for frmserverConnect.xaml
    /// </summary>
    public partial class frmserverConnect : Window
    {
        public static List<string> DBList;
        Config config = new Config();
        public static bool serverConnected= false;
        public frmserverConnect()
        {
            SystemLog.LogInfo(this.GetType(), "Erp Application Started");
            InitializeComponent();         
            SystemLog.LogInfo(this.GetType(), "Form Intialized");
            try
            {
                this.KeyUp += new KeyEventHandler(OnButtonKeyDown);
                cmbserverAdress.Visibility = Visibility.Collapsed;
                lblstatusBar.Content = "Enter Server Ip Adress and Port number to get started!";
                SYSTEM_STATIC.server = (string)Properties.Settings.Default.serverip.ToString();
                if (SYSTEM_STATIC.server != "" && serverConnected == false)
                {
                    SystemLog.LogInfo(this.GetType(), "Trying To connect to server name= " + SYSTEM_STATIC.server + " Stored in Settings. ");
                    connecttoserver();

                }
                else
                {
                      lblstatusBar.Content = "Select from available servers or Enter Server Address";
                      lblstatusBar.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                }
            }
            catch(Exception ex)
            {
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error " +ex.ToString());
            }
        }
        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void connecttoserver()
        {
            try
            {
                DBList = config.getDBListAssociated(SYSTEM_STATIC.server, false);
                if (DBList?.Count > 0)
                {
                    lblstatusBar.Content = SYSTEM_STATIC.server + "Server Connected!";
                    serverConnected = true;
                    this.Hide();

                    frmDbConnectt dpfrom = new frmDbConnectt();
                    dpfrom.Closed += (s, args) => this.Close();
                    dpfrom.Show();

                    this.Hide();
                    if (!System.Diagnostics.Debugger.IsAttached)
                        checkVersionCrash();
                }
                else
                {
                    lblstatusBar.Content = "Unable to connect to mentioned server. ";
                    ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Database not found on the current Server");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("New version is"))
                {
                    MessageBox.Show(ex.Message, "Version Update", MessageBoxButton.OK, MessageBoxImage.Stop);
                    System.Windows.Application.Current.Shutdown();
                }
                MessageBox.Show(ex.ToString());
                ERP_BL.Databases.SystemLog.LogError(this.GetType(), "Error Connecting SQL Servers "+ ex.ToString());

            }
        }

        private void checkVersionCrash()
        {
            var thisApp = Assembly.GetExecutingAssembly();
            AssemblyName name = new AssemblyName(thisApp.FullName);

            string softwareVersion =  name.Version.ToString();
            if (!ERP_BL.BaseClasses.UpdateChecker.isSystemUpToDate(softwareVersion))
                throw new Exception("New version is released, please update");
        }

        private void btnconnectServer_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.server = cmbserverAdress.Text;
            Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
            Properties.Settings.Default.Save();
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Connecting to  SQL Servers Adress selected value =" + cmbserverAdress.Text);

            connecttoserver();
        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                if(btnWan.Visibility == Visibility.Collapsed && txtWan.Visibility == Visibility.Collapsed)  
                {
                    btnWan.Visibility = Visibility.Visible;
                    txtWan.Visibility = Visibility.Visible;
                }
                else
                {
                    btnWan.Visibility = Visibility.Collapsed;
                    txtWan.Visibility = Visibility.Collapsed;
                }
            }
           
            if (e.Key == Key.Return)
            {
                SYSTEM_STATIC.server = cmbserverAdress.Text;
                Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
                Properties.Settings.Default.Save();
                ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Connecting to  SQL Servers Adress selected value =" + cmbserverAdress.Text);

                connecttoserver();
            }   
        }

      

        private void BtnIp_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnIp.Visibility = Visibility.Collapsed;
        }

        private void BtnIp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnIp.Visibility = Visibility.Collapsed;
            txtIp.Visibility = Visibility.Collapsed;
            cmbserverAdress.Visibility = Visibility.Visible;
            btnconnectServer.IsEnabled = true;
        }

        private void BtnLan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SYSTEM_STATIC.server = "192.168.10.91,1411";
            Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
            Properties.Settings.Default.Save();
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Connecting to  SQL Servers Adress selected value =" + "192.168.10.91,1411");
            connecttoserver();
        }
        private void BtnWan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SYSTEM_STATIC.server = "119.156.232.242,1411";
            Properties.Settings.Default["serverip"] = SYSTEM_STATIC.server.ToString();
            Properties.Settings.Default.Save();
            ERP_BL.Databases.SystemLog.LogInfo(this.GetType(), "Connecting to  SQL Servers Adress selected value =" + "119.156.232.242,1411");
            connecttoserver();
        }
    }
}
