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
using System.Windows.Shapes;

namespace ZAS_ERP.PopupNotificatios
{
    /// <summary>
    /// Interaction logic for frmShowPopup.xaml
    /// </summary>
    public partial class frmShowPopup : Window
    {
        public frmShowPopup()
        {
            InitializeComponent();
        }

        private void btncloseclick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
