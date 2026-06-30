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

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for frmJournalVoucherStatusChange.xaml
    /// </summary>
    public partial class frmJournalVoucherStatusChange : Window
    {
        public frmJournalVoucherStatusChange()
        {
            InitializeComponent();
            ucJournalVoucherStatusChange status = new ucJournalVoucherStatusChange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
        public frmJournalVoucherStatusChange(JournalVoucherRepo repo)
        {
            InitializeComponent();
            ucJournalVoucherStatusChange status = new ucJournalVoucherStatusChange(repo);
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
    }
}
