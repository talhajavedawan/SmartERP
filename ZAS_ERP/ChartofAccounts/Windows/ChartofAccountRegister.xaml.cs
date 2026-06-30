using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
using ZAS_ERP.ChartofAccounts.UserControls;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for ChartofAccountRegister.xaml
    /// </summary>
    public partial class ChartofAccountRegister : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        List<cmbitem> TreeItems = new List<cmbitem>();
        public ChartofAccountRegister()
        {
            InitializeComponent();
        }
        public ChartofAccountRegister(ucChartofAccountList coaListRegister)
        {
            InitializeComponent();
            faRightGrid.Children.Add(coaListRegister);

        }
        private void TreeViewCOAStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
     
        private void AddCOABarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            formSelectAccountType addAccountType = new formSelectAccountType();
            addAccountType.Show();
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
