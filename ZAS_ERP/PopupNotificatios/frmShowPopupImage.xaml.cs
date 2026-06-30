using ERP_BL.BackgroundImages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace ZAS_ERP.PopupNotificatios
{
    /// <summary>
    /// Interaction logic for frmShowPopupImage.xaml
    /// </summary>
    public partial class frmShowPopupImage : Window
    {
        public string version = "";
        public string input;
        public string _str;
        public string sub;
        public frmShowPopupImage()
        {
            InitializeComponent();            
            try
            {
                version = getRunningVersion().ToString();
                _str = version;
                int pos = version.LastIndexOf(".");
                _str = version.Substring(0, pos);
              

                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead("http://203.135.42.34:8090/updates.txt"))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            var releasedVersion = sr.ReadToEnd();
                            input = releasedVersion;
                            int endIndex = input.IndexOf(Environment.NewLine, 52);
                            sub = input.Substring(52, endIndex-52);
                            releaseVersion.Text = sub;
                            runningVersion.Text = sub;
                        }
                    }
                }

            }
            catch (Exception)
            {
                //MessageBox.Show("New update is released! Press Ok to continue.");
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            this.Close();
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
       
        //private string GetPathForExe(string fileName)
        //{
        //    string a = Environment.MachineName;
        //    string b = Environment.UserDomainName;
        //    string c = Environment.UserName;
        //    string p = @"C:\Users\" + c;
        //    string d = p;
        //    string path1 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);




        //    String path = Environment.GetEnvironmentVariable("path");
        //    String[] folders = path.Split(';');
        //    var s = folders;
           
        //    sub = input.Substring(52, 6);


        //    foreach (String folder in folders)
        //    {
        //        if (File.Exists(folder + fileName))
        //        {
        //            return folder + fileName;
        //        }
        //        else if (File.Exists(folder + "\\" + fileName))
        //        {
        //            return folder + "\\" + fileName;
        //        }
        //    }

        //    return String.Empty;
        //}

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = MessageBox.Show("Do you really want to update ERP? Please save your all data before updating the system", "Notifications...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (response == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string updaterPath = path + "\\" + "Check for updates.lnk";
                    if (updaterPath.Contains("Check for updates.lnk"))
                    {
                        Process.Start(updaterPath);
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
