using DevExpress.Xpf.Core;
using ERP_BL.Config;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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

namespace ZAS_ERP.start
{
    /// <summary>
    /// Interaction logic for pageDbConnect.xaml
    /// </summary>
    public partial class pageDbConnect : Page
    {
        public static string dbname;
       
        private bool isMousePress = false;
        Config config = new Config();
        public pageDbConnect()
        {
            InitializeComponent();

            SystemLog.LogInfo(this.GetType(), "Form Initialized ");
            
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isMousePress == false)
            {
                txtPassword.Text = txtUsername.Text;
                txtPassword.Visibility = Visibility.Visible;
                txtUsername.Visibility = Visibility.Collapsed;
                var bmImg = new BitmapImage(new Uri(@"/ZAS_ERP;component/images/EviewHide.png", UriKind.Relative));
                imgEye.Source = bmImg;
                isMousePress = true;
            }
            else
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtUsername.Visibility = Visibility.Visible;
                var image = new BitmapImage(new Uri("/ZAS_ERP;component/images/Eview.png", UriKind.Relative));
                imgEye.Source = image;
                isMousePress = false;
            }

        }

        private void cmbdbSelect_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                dbname = this.cmbdbSelect.Text.ToString();
                {
                    string conString = "server=" + SYSTEM_STATIC.server + ";uid=sa;pwd=" + ERP_BL.Config.Server.password + "; database=mk_" + dbname + "; MultipleActiveResultSets=true";
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    config.ConnectionStrings.ConnectionStrings["DBContextERP"].ConnectionString = conString;
                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("connectionStrings");
                    SystemLog.LogInfo(this.GetType(), "Connection string changed for configuration file");

                    lblVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[0] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[1] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[2];
                }
                
                    lblstatusBar.Content = "Please enter credentials";
                    SystemLog.LogInfo(this.GetType(), "Credentials Required to Login");
                    txtUsername.IsEnabled = true;
                    txtUsername.Focus();
            }

            catch (Exception ex)
            {
                DXMessageBox.Show(this, ex.Message, "Failed", MessageBoxButton.OK, MessageBoxImage.Stop);
                Application.Current.Shutdown();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (frmserverConnect.DBList.Count > 0)
                    foreach (string DB in frmserverConnect.DBList)
                    {
                        cmbdbSelect.Items.Add(DB);
                        lblstatusBar.Content = "Select from available Databases";
                    }
                else
                    lblstatusBar.Content = "No Database was found on Selected Server, Create New Company Group!";
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), "Error Occured while populating list of databses " + ex.ToString());

            }
            if (cmbdbSelect.Items.Count != 0)
            {
                    if (cmbdbSelect.Items.Contains((string)Properties.Settings.Default["dbname"]))
                        cmbdbSelect.SelectedItem = (string)Properties.Settings.Default["dbname"];
                    else
                    {
                        cmbdbSelect.SelectedIndex = 0;
                        cmbdbSelect.Focus();
                    }

            }
        }   
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            frmserverConnect frmserver = new frmserverConnect();
            frmserverConnect.serverConnected = true;
            frmserver.Show();
            var win = Window.GetWindow(this);
            win.Close();
        }
        private void TxtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            txtPassword.Text = txtUsername.Text;
        }
        private void TxtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            txtUsername.Text = txtPassword.Text;
        }
    }
}
