using ERP_BL;
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

namespace ZAS_ERP.Chat.ChatUsercontrols
{
    /// <summary>
    /// Interaction logic for ucReceiverPicture.xaml
    /// </summary>

    public partial class ucReceiverPicture : UserControl
    {


        public ucReceiverPicture()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            brdAtt.Visibility = Visibility.Visible;
            brdClickME.Visibility = Visibility.Collapsed;
        }
    }
}
