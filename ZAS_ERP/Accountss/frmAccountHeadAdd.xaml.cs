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

namespace ZAS_ERP.Accountss
{
    /// <summary>
    /// Interaction logic for frmAccountHeadAdd.xaml
    /// </summary>
    public partial class frmAccountHeadAdd : Window
    {
        public frmAccountHeadAdd()
        {
            InitializeComponent();
        }

        private void Cmbtype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkissubsidroy_Checked(object sender, RoutedEventArgs e)
        {
            lookupParentAccount.Visibility = Visibility.Visible;
            lblParentAccount.Visibility = Visibility.Visible;
        }

        private void chkissubsidroy_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupParentAccount.Visibility = Visibility.Collapsed;
            lblParentAccount.Visibility = Visibility.Collapsed;
        }

        private void CmbCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LookupParentAccount_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
