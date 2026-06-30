using System;
using System.Collections.Generic;
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

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for ucSelectOption.xaml
    /// </summary>
    public partial class ucSelectOption : UserControl
    {
        public ucSelectOption()
        {
            InitializeComponent();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if(btnShareAll.IsChecked == true)
            {
                MainWindow.isSpecific = 1;
                Window myWindow = Window.GetWindow(this);
                myWindow.Close();

            }
           if(btnSetSpecific.IsChecked == true)
            {
                MainWindow.isSpecific = 2;
                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            if (btnSetGroups.IsChecked == true)
            {
                MainWindow.isSpecific = 3;
                Window myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        //private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MainWindow.isSpecific = 0;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
