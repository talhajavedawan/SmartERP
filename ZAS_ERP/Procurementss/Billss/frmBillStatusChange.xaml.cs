using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for frmInqyuiryStatusChange.xaml
    /// </summary>
    public partial class frmBillStatusChange : Window
    {
        public frmBillStatusChange()
        {
            InitializeComponent();
            ucStatuschange status = new ucStatuschange();
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
        public frmBillStatusChange(BillRepo repo)
        {
            InitializeComponent();
            ucStatuschange status = new ucStatuschange(repo);
            this.statusGrid.Children.Clear();
            this.statusGrid.Children.Add(status);
        }
    }
}
