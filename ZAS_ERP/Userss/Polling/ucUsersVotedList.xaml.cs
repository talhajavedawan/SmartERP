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

namespace ZAS_ERP.Userss.Polling
{
    /// <summary>
    /// Interaction logic for ucUsersVotedList.xaml
    /// </summary>
    public partial class ucUsersVotedList : UserControl
    {
        public ucUsersVotedList()
        {
            InitializeComponent();
        }

        private void GrdUsers_CustomRowFilter(object sender, DevExpress.Xpf.Grid.RowFilterEventArgs e)
        {

        }
    }
}
