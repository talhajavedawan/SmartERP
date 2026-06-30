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

namespace ZAS_ERP.Chat
{
    /// <summary>
    /// Interaction logic for ucReplyMsg.xaml
    /// </summary>
    public partial class ucReplyMsg : UserControl
    {
        public ucReplyMsg()
        {
            InitializeComponent();
        }

        private void ReplyClose_Click(object sender, RoutedEventArgs e)
        {
            replyTxt.Text = "";

        }
        
    }
   
    //<UserControl.ContextMenu>
    //    <ContextMenu>
    //        <MenuItem Header = "Reply" Click="Reply_Click"/>
    //    </ContextMenu>
    //</UserControl.ContextMenu>
}
