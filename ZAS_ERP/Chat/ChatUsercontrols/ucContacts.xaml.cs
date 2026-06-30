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

namespace ZAS_ERP.Chat.Usercontrols.ChatUsercontrols
{
    /// <summary>
    /// Interaction logic for ucContacts.xaml
    /// </summary>
    public partial class ucContacts : UserControl
    {
        public ucContacts()
        {
            InitializeComponent();
            
            DataContext = ucContactsBinding.GetChatDetails();
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            brdContact.BorderBrush = Brushes.LightGreen;
            Background = Brushes.LightBlue;


        }
        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            brdContact.BorderBrush = Brushes.Gray;
            Background = Brushes.Transparent;
        }
    }
}
