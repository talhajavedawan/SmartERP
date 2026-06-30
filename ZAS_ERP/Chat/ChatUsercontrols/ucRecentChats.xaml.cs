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

namespace ZAS_ERP.Chat.Usercontrols
{
    /// <summary>
    /// Interaction logic for ucRecentChats.xaml
    /// </summary>
    public partial class ucRecentChats : UserControl
    {
        
        public ucRecentChats()
        {
            InitializeComponent(); 
            DataContext=ucRecentBinding.GetDetails();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            brdRecents.BorderBrush = Brushes.GreenYellow;
            Background = Brushes.LightBlue;
           
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            brdRecents.BorderBrush = Brushes.Gray;
            Background = Brushes.Transparent;
          
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            msgCounter.Text = "";  
            
            brdrMsgCounter.Visibility = Visibility.Collapsed;
        }
    }
}
