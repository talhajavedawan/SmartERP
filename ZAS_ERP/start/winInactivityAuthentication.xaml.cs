using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ZAS_ERP.start
{
    /// <summary>
    /// Interaction logic for winInactivityAuthentication.xaml
    /// </summary>
    public partial class winInactivityAuthentication : ThemedWindow
    {
        pageInactivityAuthentication pagePasswordAuthentication = new pageInactivityAuthentication();
        bool closingFromAuthBtn = false;
        InactivityTracker inactivityTracker;
        public winInactivityAuthentication()
        {
            InitializeComponent();
        } 
        public winInactivityAuthentication(InactivityTracker _inactivityTracker)
        {
            InitializeComponent();
            inactivityTracker = _inactivityTracker;
        }
        private void btncloseclick(object sender, RoutedEventArgs e)
        {

        }

        private void btnlogindb_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.currentUser.password != SYSTEM_STATIC.GenerateSHA512String(pagePasswordAuthentication.txtpasswordBox.Text))
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Wrong password, Please try again.", "Wrong password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                closingFromAuthBtn = true;
                inactivityTracker.winLoaded = false;
                inactivityTracker.logoutFromInactivity = false;
                this.Close();
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inactivityTracker.winLoaded = true;
            pagePasswordAuthentication = new pageInactivityAuthentication();
            pagePasswordAuthentication.txtUserName.Text = SYSTEM_STATIC.currentUser.userName;
            pagePasswordAuthentication.lblVersion.Content = Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[0] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[1] + "." + Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.')[2]; // 3 fractions version

            if (SYSTEM_STATIC.currentUser.employee.person.Photo != null)
                pagePasswordAuthentication.ConvertByteToBmp(SYSTEM_STATIC.currentUser.employee.person.Photo);
            main.Content = pagePasswordAuthentication;
            btnlogindb.Visibility = Visibility.Visible;
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closingFromAuthBtn != true)
            {
                e.Cancel = true;
                inactivityTracker.logoutFromInactivity = false;
            }
        }
    }
}
